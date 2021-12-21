using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MES_WATER.Models;
using MES_WATER.Repository;
using Newtonsoft.Json;
using System.Collections;
using System.Data.SqlClient;
using Dapper;
using System.ComponentModel;

namespace MES_WATER.Controllers
{
    public class RPT009AController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();

        public ActionResult Index()
        {
            return View();
        }

        public class RPT009A
        {
            [DisplayName("線別")]
            public string line_name { get; set; }

            [DisplayName("機台")]
            public string mac_name { get; set; }

            [DisplayName("派工單")]
            public string wrk_code { get; set; }

            [DisplayName("派工數量")]
            public string pro_qty { get; set; }

            [DisplayName("總數量")]
            public string total_qty { get; set; }

            [DisplayName("開工日時間")]
            public string start_time { get; set; }

            [DisplayName("完工日時間")]
            public string end_time { get; set; }

            [DisplayName("開工時數")]
            public string total_time { get; set; }

            [DisplayName("停機時數")]
            public string stop_time { get; set; }

            [DisplayName("生產時數")]
            public string produce_time { get; set; }

            [DisplayName("POH")]
            public string poh { get; set; }

            [DisplayName("PH")]
            public string ph { get; set; }
        }

        /// <summary>
        /// View的Table資料來源
        /// </summary>
        /// <param name="pWhere">View傳來的查詢資料，JSON字串</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Get_DataTableData(string pWhere)
        {
            // 報表欄位結構
            List<RPT009A> result = new List<RPT009A>();

            // 沒有下查詢條件時就沒有資料
            if (string.IsNullOrEmpty(pWhere))
            {
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }

            // 將前面的查詢欄位放到query_data
            JqGridQueryData query_data = new JqGridQueryData();
            if (!string.IsNullOrEmpty(pWhere))
            {
                List<JqGridQueryData> query_datas = JsonConvert.DeserializeObject<List<JqGridQueryData>>(pWhere);
                if (query_datas.Count > 0)
                {
                    query_data = query_datas[0];
                }
            }
            result.AddRange(Get_StatData(query_data));

            // 回傳給view
            var returnObj = new
            {
                data = result
            };

            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }


        public List<RPT009A> Get_StatData(JqGridQueryData query_data)
        {
            List<RPT009A> result = new List<RPT009A>();            
            string sDateS = query_data.find("date", "S"); 
            string sDateE = query_data.find("date", "E");
            string sMoCode = query_data.find("mo_code");
            string sMacName = query_data.find("mac_name");
            string sLineName = query_data.find("line_name");

            // mo_code清單
            string sSql = @"select mo_code
                            from MET01_0000
                            where 1=1";

            if (sDateS != "")
                sSql += " and CONVERT(DATE, '" + sDateS + "') <= CONVERT(DATE, mo_start_date)";
            if (sDateE != "")
                sSql += " and CONVERT(DATE, mo_end_date) <= CONVERT(DATE, '" + sDateE + "')";
            if (sMoCode != "")
                sSql += " and mo_code = '" + sMoCode + "'";

            DataTable dtTemp = comm.Get_DataTable(sSql);

            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                string mo_code = dtTemp.Rows[i]["mo_code"].ToString();

                sSql = @"select g.line_name, f.mac_name, b.wrk_code, b.pro_qty, sum(d.ok_qty) as ok_qty, sum(d.ng_qty) as ng_qty,
                        CONVERT(DATETIME,  c.ins_date + ' ' + c.ins_time) as start_time,
                        CONVERT(DATETIME,  c.end_date + ' ' + c.end_time) as end_time,
                        DATEDIFF(second, CONVERT(DATETIME,  c.ins_date + ' ' + c.ins_time), CONVERT(DATETIME,  c.end_date + ' ' + c.end_time))/ 3600.0 as total_time
                        from MET01_0000 a
                        inner join MET03_0000 b on a.mo_code = b.mo_code
                        left join MED02_0000 c on a.mo_code = c.mo_code and c.wrk_code = b.wrk_code                        
                        left join MEM01_0000 d on d.mo_code = a.mo_code and d.work_code = b.work_code
                        left join MEB15_0000 f on f.mac_code = c.mac_code
                        left join MEB12_0000 g on g.line_code = a.plan_line_code
                        where c.end_date <> '' and a.mo_code = '" + mo_code + "'";
                if (sMacName != "")
                    sSql += " and f.mac_name like '%" + sMacName + "%'";
                if (sLineName != "")
                    sSql += " and g.line_name like '%" + sLineName + "%'";
                sSql += " GROUP BY g.line_name, f.mac_name, b.wrk_code, b.pro_qty, c.ins_date, c.ins_time, c.end_date, c.end_time";

                //sSql = @"select g.line_name, f.mac_name, a.wrk_code, a.pro_qty, sum(b.pro_qty) as ok_qty, sum(c.ng_qty) as ng_qty,
                //        CONVERT(DATETIME,  d.date_s + ' ' + d.time_s) as start_time,
                //        CONVERT(DATETIME,  d.date_e + ' ' + d.time_e) as end_time,
                //        DATEDIFF(second, CONVERT(DATETIME,  d.date_s + ' ' + d.time_s), CONVERT(DATETIME,  d.date_e + ' ' + d.time_e))/ 3600.0 as total_time
                //        from MET03_0000 a
                //        left join MED09_0000 b on a.wrk_code = b.wrk_code
                //        left join MED03_0000 c on a.wrk_code = c.wrk_code
                //        left join MED08_0000 d on a.wrk_code = d.wrk_code
                //        left join MEB29_0000 e on a.station_code = e.station_code
                //        left join MEB15_0000 f on e.mac_code = f.mac_code
                //        left join MEB12_0000 g on f.line_code = g.line_code
                //        where a.mo_code = '" + mo_code + "'";

                //sSql += " GROUP BY g.line_name, f.mac_name, a.wrk_code, a.pro_qty, d.date_s, d.time_s,d.date_e, d.time_e";

                DataTable dtTmp = comm.Get_DataTable(sSql);

                for (int j = 0; j < dtTmp.Rows.Count; j++)
                {                    
                    string ok_qty = dtTmp.Rows[j]["ok_qty"].ToString();
                    string ng_qty = dtTmp.Rows[j]["ng_qty"].ToString();
                    string total_qty = (comm.sGetDecimal(ok_qty) + comm.sGetDecimal(ng_qty)).ToString("#0");
                    string pro_qty = dtTmp.Rows[j]["pro_qty"].ToString();
                    string total_time = dtTmp.Rows[j]["total_time"].ToString();
                    string wrk_code = dtTmp.Rows[j]["wrk_code"].ToString();

                    RPT009A data = new RPT009A();

                    data.line_name = dtTmp.Rows[j]["line_name"].ToString();
                    data.mac_name = dtTmp.Rows[j]["mac_name"].ToString();
                    data.wrk_code = wrk_code;
                    data.pro_qty = pro_qty;
                    data.total_qty = total_qty;
                    data.start_time = dtTmp.Rows[j]["start_time"].ToString();
                    data.end_time = dtTmp.Rows[j]["end_time"].ToString();

                    data.total_time = comm.sGetDecimal(total_time) > 0 ? comm.sGetDecimal(total_time).ToString("#0.00") : "0";// 預防異常資料

                    // 派工單範圍內的停機時間
                    sSql = @"select 
                            case when 
                            (
                            CONVERT(DATETIME,  e.date_s + ' ' + e.time_s) BETWEEN CONVERT(DATETIME,  d.ins_date + ' ' + d.ins_time) and CONVERT(DATETIME,  d.end_date + ' ' + d.end_time)
                            and
                            CONVERT(DATETIME,  e.date_e + ' ' + e.time_e) BETWEEN CONVERT(DATETIME,  d.ins_date + ' ' + d.ins_time) and CONVERT(DATETIME,  d.end_date + ' ' + d.end_time)
                            )
                            then DATEDIFF(second, CONVERT(DATETIME,  e.date_s + ' ' + e.time_s), CONVERT(DATETIME,  e.date_e + ' ' + e.time_e))/ 3600.0 
                            when CONVERT(DATETIME,  e.date_s + ' ' + e.time_s) BETWEEN CONVERT(DATETIME,  d.ins_date + ' ' + d.ins_time) and CONVERT(DATETIME,  d.end_date + ' ' + d.end_time)
                            then DATEDIFF(second, CONVERT(DATETIME,  e.date_s + ' ' + e.time_s), CONVERT(DATETIME,  d.end_date + ' ' + d.end_time))/ 3600.0 
                            when CONVERT(DATETIME,  e.date_e + ' ' + e.time_e) BETWEEN CONVERT(DATETIME,  d.ins_date + ' ' + d.ins_time) and CONVERT(DATETIME,  d.end_date + ' ' + d.end_time)
                            then DATEDIFF(second, CONVERT(DATETIME,  d.ins_date + ' ' + d.ins_time), CONVERT(DATETIME,  e.date_e + ' ' + e.time_e))/ 3600.0 
                            when CONVERT(DATETIME,  e.date_s + ' ' + e.time_s) < CONVERT(DATETIME,  d.ins_date + ' ' + d.ins_time) and CONVERT(DATETIME,  e.date_e + ' ' + e.time_e) > CONVERT(DATETIME,  d.end_date + ' ' + d.end_time)
                            then DATEDIFF(second, CONVERT(DATETIME,  d.ins_date + ' ' + d.ins_time), CONVERT(DATETIME,  d.end_date + ' ' + d.end_time))/ 3600.0 
                            else 0 end as stop_time 
                            from MED02_0000 d 
                            left join MED04_0000 e on d.wrk_code = e.wrk_code 
                            and 
                            (
                            (
                            CONVERT(DATETIME,  e.date_s + ' ' + e.time_s) BETWEEN CONVERT(DATETIME,  d.ins_date + ' ' + d.ins_time) and CONVERT(DATETIME,  d.end_date + ' ' + d.end_time)
                            and
                            CONVERT(DATETIME,  e.date_e + ' ' + e.time_e) BETWEEN CONVERT(DATETIME,  d.ins_date + ' ' + d.ins_time) and CONVERT(DATETIME,  d.end_date + ' ' + d.end_time)
                            )
                            or
                            CONVERT(DATETIME,  e.date_s + ' ' + e.time_s) BETWEEN CONVERT(DATETIME,  d.ins_date + ' ' + d.ins_time) and CONVERT(DATETIME,  d.end_date + ' ' + d.end_time)
                            or
                            CONVERT(DATETIME,  e.date_e + ' ' + e.time_e) BETWEEN CONVERT(DATETIME,  d.ins_date + ' ' + d.ins_time) and CONVERT(DATETIME,  d.end_date + ' ' + d.end_time)
                            or
                            CONVERT(DATETIME,  e.date_s + ' ' + e.time_s) < CONVERT(DATETIME,  d.ins_date + ' ' + d.ins_time) and CONVERT(DATETIME,  e.date_e + ' ' + e.time_e) > CONVERT(DATETIME,  d.end_date + ' ' + d.end_time)
                            )
                            where d.end_date <> '' and d.wrk_code = '" + wrk_code + "'";

                    DataTable dtTmp2 = comm.Get_DataTable(sSql);
                    string stop_time = dtTmp2.Rows.Count > 0 ? dtTmp2.Rows[0]["stop_time"].ToString() : "0";
                    data.stop_time = comm.sGetDecimal(stop_time).ToString("#0.00");
                    string produce_time = (comm.sGetDecimal(total_time) - comm.sGetDecimal(stop_time)).ToString("#0.00");// 輸出到小數點後2位
                    data.produce_time = produce_time;
                    data.poh = comm.sGetDecimal(produce_time) != 0 ? (comm.sGetDecimal(total_qty) / comm.sGetDecimal(produce_time)).ToString("#0.0000") : "0";
                    data.ph = comm.sGetDecimal(total_qty) != 0 ? (comm.sGetDecimal(produce_time) / comm.sGetDecimal(total_qty)).ToString("#0.0000") : "0";

                    result.Add(data);
                }
            }
            
            return result;
        }

    }
}
