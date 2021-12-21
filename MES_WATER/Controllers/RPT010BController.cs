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
    public class RPT010BController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();

        public ActionResult Index()
        {
            return View();
        }

        public class RPT010B
        {
            [DisplayName("線別")]
            public string line_name { get; set; }

            [DisplayName("產品")]
            public string pro_name { get; set; }

            [DisplayName("途程")]
            public string work_name { get; set; }

            [DisplayName("機台")]
            public string mac_name { get; set; }

            [DisplayName("標準工時")]
            public string std_time { get; set; }

            [DisplayName("稼動時間")]
            public string work_time { get; set; }

            [DisplayName("非稼動時數")]
            public string non_work_time { get; set; }

            [DisplayName("停機時數")]
            public string stop_time { get; set; }

            [DisplayName("稼動率")]
            public string work_rate { get; set; }

            [DisplayName("總產量")]
            public string total_qty { get; set; }

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
            List<RPT010B> result = new List<RPT010B>();

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


        public List<RPT010B> Get_StatData(JqGridQueryData query_data)
        {
            List<RPT010B> result = new List<RPT010B>();
            string sDateS = query_data.find("date", "S");
            string sDateE = query_data.find("date", "E");

            // mac_code清單
            string sSql = @"select distinct(mac_code), mac_name, std_time / 3600.0 as std_time
                            from MEB15_0000";
            DataTable dtTemp = comm.Get_DataTable(sSql);

            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                string mac_code = dtTemp.Rows[i]["mac_code"].ToString();
                string mac_name = dtTemp.Rows[i]["mac_name"].ToString();

                // 停機時數
                sSql = @"select 
                        sum(
                        case when CONVERT(DATETIME, date_s+' '+time_s) < CONVERT(DATETIME, date_e+' '+time_e)
                        then DATEDIFF(minute, CONVERT(DATETIME, date_s+' '+time_s), CONVERT(DATETIME, date_e+' '+time_e)) / 60.0 else 0 end) as stop_time
                        from MED04_0000
                        where mac_code = '" + mac_code + "'";
                if (sDateS != "")
                    sSql += " and '" + sDateS + "' <= CONVERT(DATE, date_s)";
                if (sDateE != "")
                    sSql += " and '" + sDateE + "' >= CONVERT(DATE, date_e)";

                DataTable temp = comm.Get_DataTable(sSql);
                string stop_time = temp.Rows[0]["stop_time"].ToString();

                // DB:std_time單位=秒
                sSql = @"select temp.line_name, temp.work_name, temp.pro_name, temp.pro_code, temp.mac_code, sum(temp.std_time) as std_time, sum(temp.work_time) as work_time, sum(temp.ok_qty) as ok_qty, sum(temp.ng_qty) as ng_qty
                        from
                        (
                        SELECT c.line_name, d.work_name, e.pro_name, e.pro_code, b.mac_code, (b.std_time/3600) * CEILING(DATEDIFF(minute, a.work_time_s, a.work_time_e) / (60*24.0)) as std_time, 
                        a.mem01_0000,
                        case when a.work_time_s < a.work_time_e then 
                        DATEDIFF(minute, a.work_time_s, a.work_time_e) / 60.0 else '0' end as work_time, a.ok_qty, a.ng_qty
                        from MET01_0000 x
                        left join mem01_0000 a on a.mo_code = x.mo_code
                        left join MEB15_0000 b on a.mac_code = b.mac_code
                        left join MEB12_0000 c on b.line_code = c.line_code
                        left join MEB30_0000 d on a.work_code = d.work_code
                        left join MEB20_0000 e on x.pro_code = e.pro_code
                        where x.mo_end_date <> '' and a.mac_code = '" + mac_code + "'";
                if (sDateS != "")
                    sSql += " and '" + sDateS + "' <= CONVERT(DATE, x.mo_start_date)";
                if (sDateE != "")
                    sSql += " and '" + sDateE + "' >= CONVERT(DATE, x.mo_end_date)";

                sSql += @") temp
                        GROUP BY temp.line_name, temp.work_name, temp.pro_name, temp.pro_code, temp.mac_code";

                DataTable dtTemp2 = comm.Get_DataTable(sSql);

                RPT010B data = new RPT010B();

                data.mac_name = mac_name;
                data.std_time = "0";
                // Init
                data.work_time = "0";
                data.non_work_time = "0";
                data.stop_time = "0";
                data.work_rate = "0";               
                data.total_qty = "0";
                data.poh = "0";
                data.ph = "0";

                if (dtTemp2.Rows.Count > 0)
                {
                    data.line_name = dtTemp2.Rows[0]["line_name"].ToString();
                    data.pro_name = dtTemp2.Rows[0]["pro_name"].ToString();
                    data.work_name = dtTemp2.Rows[0]["work_name"].ToString();

                    // 標準工時(hr)
                    string std_time = dtTemp2.Rows[0]["std_time"].ToString();
                    data.std_time = std_time;
                    /// 稼動時間
                    string work_time = (comm.sGetDecimal(dtTemp2.Rows[0]["work_time"].ToString()) - comm.sGetDecimal(stop_time)).ToString("#0.00");
                    data.work_time = work_time;
                    data.non_work_time = (comm.sGetDecimal(std_time) - comm.sGetDecimal(work_time)).ToString("#0.00");
                    data.stop_time = stop_time != "" ? stop_time : "0";
                    data.work_rate = comm.sGetDecimal(std_time) != 0 ? (comm.sGetDecimal(work_time) / comm.sGetDecimal(std_time)).ToString("#0.00") : "0";
                    string total_qty = (comm.sGetDecimal(dtTemp2.Rows[0]["ok_qty"].ToString()) + comm.sGetDecimal(dtTemp2.Rows[0]["ng_qty"].ToString())).ToString("#0");
                    data.total_qty = total_qty;
                    data.poh = comm.sGetDecimal(work_time) != 0 ? (comm.sGetDecimal(total_qty) / comm.sGetDecimal(work_time)).ToString("#0.00") : "0";
                    data.ph = comm.sGetDecimal(total_qty) != 0 ? (comm.sGetDecimal(work_time) / comm.sGetDecimal(total_qty)).ToString("#0.00") : "0";
                }

                result.Add(data);

                //for (int j = 0; j < dtTemp2.Rows.Count; j++)
                //{
                    //string pro_code = dtTemp2.Rows[j]["pro_code"].ToString();
                    //sSql = @"select sum(ng_qty) as ng_qty
                    //        from MED03_0000
                    //        where mac_code = '" + mac_code + "' and pro_code = '" + pro_code + "'";
                    //if (sDateS != "")
                    //    sSql += " and '" + sDateS + "' < ins_date";
                    //if (sDateE != "")
                    //    sSql += " and '" + sDateE + "' > ins_date";
                    //temp = comm.Get_DataTable(sSql);
                    //string ng_qty = temp.Rows[0]["ng_qty"].ToString();

                    //sSql = @"select sum(pro_qty) as ok_qty
                    //        from MED09_0000
                    //        where mac_code = '" + mac_code + "' and pro_code = '" + pro_code + "'";
                    //if (sDateS != "")
                    //    sSql += " and '" + sDateS + "' < ins_date";
                    //if (sDateE != "")
                    //    sSql += " and '" + sDateE + "' > ins_date";
                    //temp = comm.Get_DataTable(sSql);
                    //string ok_qty = temp.Rows[0]["ok_qty"].ToString();

                    

                    

                    
                //}
            }                            

            return result;
        }

    }
}
