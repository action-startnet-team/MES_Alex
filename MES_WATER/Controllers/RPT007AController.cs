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
    public class RPT007AController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();

        public ActionResult Index()
        {
            return View();
        }

        public class RPT007A
        {
            [DisplayName("機台代碼")]
            public string mac_code { get; set; }

            [DisplayName("機台名稱")]
            public string mac_name { get; set; }

            [DisplayName("派工單")]
            public string wrk_code { get; set; }

            [DisplayName("製程")]
            public string work_code { get; set; }

            [DisplayName("產品代碼")]
            public string pro_code { get; set; }

            [DisplayName("機台代碼")]
            public string pro_name { get; set; }

            [DisplayName("計畫產量")]
            public string plan_qty { get; set; }

            [DisplayName("良品")]
            public string pro_qty { get; set; }

            [DisplayName("不良品")]
            public string ng_qty { get; set; }

            [DisplayName("達成率")]
            public string complete_rate { get; set; }

            [DisplayName("良率")]
            public string pro_rate { get; set; }

            [DisplayName("開工日期")]
            public string ins_date { get; set; }

            [DisplayName("開工時間")]
            public string ins_time { get; set; }

            [DisplayName("人員")]
            public string usr_code { get; set; }

            [DisplayName("工單狀態")]
            public string mo_status { get; set; }
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
            List<RPT007A> result = new List<RPT007A>();

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


        public List<RPT007A> Get_StatData(JqGridQueryData query_data)
        {
            List<RPT007A> result = new List<RPT007A>();

            string sMo_status = query_data.find("mo_status"); //工單狀態
                       
            string sIns_date = query_data.find("ins_date"); //建立日期

            string sSql = @"select a.mac_code, a.mac_name, c.wrk_code, c.work_code, c.pro_code, d.pro_name, e.plan_qty, sum(f.pro_qty) as pro_qty, sum(g.ng_qty) as ng_qty, c.ins_date, c.ins_time, c.usr_code, c.mo_status
                            from MEB15_0000 a
                            left join MEB29_0200 b on a.mac_code = b.mac_code
                            left join MET03_0000 c on c.station_code = b.station_code
                            left join MEB20_0000 d on c.pro_code = d.pro_code
                            left join MET01_0000 e on e.mo_code = c.mo_code
                            left join MED09_0000 f on f.wrk_code = c.wrk_code
                            left join MED03_0000 g on g.wrk_code = c.wrk_code
                            where 1=1";
            if (sMo_status != "")
                sSql += " and c.mo_status = '" + sMo_status + "'";
            if (sIns_date != "")
                sSql += " and c.ins_date = '" + sIns_date + "'";
            sSql += " GROUP BY a.mac_code, a.mac_name, c.wrk_code, c.work_code, c.pro_code, d.pro_name, e.plan_qty, f.pro_qty, g.ng_qty, c.ins_date, c.ins_time, c.usr_code, c.mo_status";

            DataTable dtTmp = comm.Get_DataTable(sSql);

            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                string pro_qty = dtTmp.Rows[i]["pro_qty"].ToString();
                string ng_qty = dtTmp.Rows[i]["ng_qty"].ToString();
                string pro_rate = "0";
                if ((comm.sGetInt32(pro_qty) + comm.sGetInt32(ng_qty)) != 0)
                {
                    pro_rate = (comm.sGetInt32(pro_qty) / (comm.sGetInt32(pro_qty) + comm.sGetInt32(ng_qty))).ToString();
                }

                string complete_rate = "0";
                //

                RPT007A data = new RPT007A();

                data.mac_code = dtTmp.Rows[i]["mac_code"].ToString();
                data.mac_name = dtTmp.Rows[i]["mac_name"].ToString();
                data.wrk_code = dtTmp.Rows[i]["wrk_code"].ToString();
                data.work_code = dtTmp.Rows[i]["work_code"].ToString();
                data.pro_code = dtTmp.Rows[i]["pro_code"].ToString();
                data.pro_name = dtTmp.Rows[i]["pro_name"].ToString();
                data.plan_qty = dtTmp.Rows[i]["plan_qty"].ToString();
                data.pro_qty = pro_qty;
                data.ng_qty = ng_qty;
                data.complete_rate = complete_rate + "%";
                data.pro_rate = pro_rate + "%";
                data.ins_date = dtTmp.Rows[i]["ins_date"].ToString();
                data.ins_time = dtTmp.Rows[i]["ins_time"].ToString();
                data.usr_code = dtTmp.Rows[i]["usr_code"].ToString();
                data.mo_status = dtTmp.Rows[i]["mo_status"].ToString();

                result.Add(data);
            }
            return result;
        }        

    }
}
