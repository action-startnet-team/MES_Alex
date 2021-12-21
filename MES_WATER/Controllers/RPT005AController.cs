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
    public class RPT005AController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();

        public ActionResult Index()
        {
            return View();
        }

        public class RPT005A
        {
            [DisplayName("工作站")]
            public string work_name { get; set; }

            [DisplayName("投入數量")]
            public string mo_qty { get; set; }

            [DisplayName("產出數量")]
            public string ok_total { get; set; }

            [DisplayName("計畫數量")]
            public string plan_qty { get; set; }

            [DisplayName("投入產出比")]
            public string mo_ok_rate { get; set; }

            [DisplayName("投入計畫比")]
            public string mo_plan_rate { get; set; }

            [DisplayName("產出計畫比")]
            public string ok_plan_qty { get; set; }
            
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
            List<RPT005A> result = new List<RPT005A>();

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


        public List<RPT005A> Get_StatData(JqGridQueryData query_data)
        {
            List<RPT005A> result = new List<RPT005A>();

            string sWork_name = query_data.find("work_name"); //工作站
            string sPro_code = query_data.find("pro_code"); //產品編號 
            string sMo_code = query_data.find("mo_code"); //製令號碼 
            string sSch_date_s = query_data.find("sch_date_s"); //開始日期

            string sSql = @"select dbo.MEB30_0000.work_name,
                            SUM(dbo.MET01_0000.mo_qty) as mo_qty, 
                            SUM(dbo.MEM01_0000.ok_qty) as ok_total,
                            SUM(dbo.MET01_0000.plan_qty) as plan_qty
                            from dbo.MEM01_0000
                            join dbo.MEB30_0000
                            on dbo.MEM01_0000.work_code= dbo.MEB30_0000.work_code
                            join dbo.MET01_0000
                            on dbo.MEM01_0000.mo_code=dbo.MET01_0000.mo_code
                            join dbo.MEB20_0000 
                            on dbo.MET01_0000.pro_code=dbo.MEB20_0000.pro_code
                            where 1=1";
            if (sWork_name != "")
                sSql += " and MEB30_0000.work_name = '" + sWork_name + "'";
            if (sSch_date_s != "")
                sSql += " and MET01_0000.sch_date_s = '" + sSch_date_s + "'";
            if (sPro_code != "")
                sSql += " and MET01_0000.pro_code = '" + sPro_code + "'";
            if (sMo_code != "")
                sSql += " and MET01_0000.mo_code = '" + sMo_code + "'";

            sSql += " group by dbo.MEB30_0000.work_name";
            DataTable dtTmp = comm.Get_DataTable(sSql);

            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {                
                string mo_qty = dtTmp.Rows[i]["mo_qty"].ToString();
                string ok_total = dtTmp.Rows[i]["ok_total"].ToString();
                string plan_qty = dtTmp.Rows[i]["plan_qty"].ToString();

                RPT005A data = new RPT005A();

                data.work_name = dtTmp.Rows[i]["work_name"].ToString();
                data.mo_qty = mo_qty;
                data.ok_total = ok_total;
                data.plan_qty = plan_qty;
                data.mo_ok_rate = comm.sGetDecimal(ok_total) == 0 ? "0%" : Math.Round(((comm.sGetDecimal(mo_qty) / comm.sGetDecimal(ok_total))*100), 2).ToString() + "%";
                data.mo_plan_rate = comm.sGetDecimal(plan_qty) == 0 ? "0%" : Math.Round(((comm.sGetDecimal(mo_qty) / comm.sGetDecimal(plan_qty)) * 100), 2).ToString() + "%";
                data.ok_plan_qty = comm.sGetDecimal(plan_qty) == 0 ? "0%" : Math.Round(((comm.sGetDecimal(ok_total) / comm.sGetDecimal(plan_qty)) * 100), 2).ToString() + "%";
                
                result.Add(data);
            }
            return result;
        }

        [HttpPost]
        public JsonResult Get_LotNoList(string pro_code)
        {
            string sSql = "select distinct(lot_no)" +
                         "from MED06_0000 where pro_code = '" + pro_code + "'";
            DataTable dtTmp = comm.Get_DataTable(sSql);

            List<string> result = new List<string>();
            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                result.Add(dtTmp.Rows[i]["lot_no"].ToString());
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
