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
    public class WMB170AController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();

        // 首頁
        public ActionResult Index()
        {
            return View();
        }

        public class WMB170A
        {
            [DisplayName("料號")]
            public string pro_code { get; set; }

            [DisplayName("品名")]
            public string pro_name { get; set; }

            [DisplayName("批號")]
            public string lot_no { get; set; }

            [DisplayName("倉庫")]
            public string sto_code { get; set; }

            [DisplayName("儲位")]
            public string loc_code { get; set; }

            [DisplayName("庫存日期")]
            public string sto_date { get; set; }
            //public DateTime sto_date { get; set; }

            [DisplayName("數量")]
            public int qty { get; set; }

            [DisplayName("天數")]
            public int days { get; set; }
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
            List<WMB170A> result = new List<WMB170A>();

            // 沒有下查詢條件時就沒有資料
            if (string.IsNullOrEmpty(pWhere))
            {
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }

            // 將前面的查詢欄位放到query_data
            JqGridQueryData query_data = comm.parseQuery(pWhere);

            // 取得報表
            result.AddRange(Get_RptData(query_data));

            // 回傳給view
            var returnObj = new
            {
                data = result
            };

            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }

        private List<WMB170A> Get_RptData(JqGridQueryData query_data)
        {
            List<WMB170A> result = new List<WMB170A>();

            string sProCodeS = query_data.find("pro_code", "S"); // 產品編號
            string sProCodeE = query_data.find("pro_code", "E"); // 產品編號
            string sStoCodeS = query_data.find("sto_code", "S"); // 倉庫編號
            string sStoCodeE = query_data.find("sto_code", "E"); // 倉庫編號
            string sDays = query_data.find("days"); // 呆滯天數

            Dictionary<string, object> sSqlParams = new Dictionary<string, object>();

            string sWhere = "";
            if (!string.IsNullOrEmpty(sProCodeS)) {
                sWhere += " AND a.pro_code >= @pro_code_s ";
                sSqlParams.Add("@pro_code_s", sProCodeS);
            }
            if (!string.IsNullOrEmpty(sProCodeE))
            {
                sWhere += " AND a.pro_code <= @pro_code_e ";
                sSqlParams.Add("@pro_code_e", sProCodeE);
            }

            if (!string.IsNullOrEmpty(sStoCodeS))
            {
                sWhere += " AND a.sto_code >= @sto_code_s ";
                sSqlParams.Add("@sto_code_s", sStoCodeS);
            }
            if (!string.IsNullOrEmpty(sStoCodeE))
            {
                sWhere += " AND a.sto_code <= @sto_code_e ";
                sSqlParams.Add("@sto_code_e", sStoCodeE);
            }

            string sDate = DateTime.Now.AddDays(-1*comm.sGetInt32(sDays)).ToString("yyyy/MM/dd");
            sSqlParams.Add("@sto_date", sDate);

            string sSql = @"declare @today datetime
                            set @today = cast(getdate() as varchar(8))
                            SELECT DISTINCT a.pro_code,c.pro_name,lot_no,b.sto_code,a.loc_code,sto_date 
                            ,isnull((
	                            SELECT sum(qty)
	                            FROM V_STO_QTY 
	                            WHERE pro_code= a.pro_code
		                          and loc_code = a.loc_code
	                            GROUP BY pro_code
                            ), 0) AS qty
                            ,DATEDIFF(DAY, sto_date, @today) AS days
                             FROM WMT0200 a 
                             LEFT JOIN WMB02_0000 b ON a.loc_code=b.loc_code
                             LEFT JOIN MEB20_0000 c ON a.pro_code=c.pro_code 
                              WHERE sto_date <= @sto_date" 
                              + sWhere 
                              + @" ORDER BY pro_code
                            ";

            DataTable dtTmp = comm.Get_DataTable(sSql, sSqlParams, true);

            foreach (DataRow dr in dtTmp.Rows)
            {
                // 
                WMB170A data = new WMB170A();
                data.pro_code = dr["pro_code"].ToString();
                data.pro_name = dr["pro_name"].ToString();  
                data.lot_no = dr["lot_no"].ToString();
                data.sto_code = dr["sto_code"].ToString();
                data.loc_code = dr["loc_code"].ToString();
                data.sto_date = DateTime.Parse(dr["sto_date"].ToString()).ToString("yyyy/MM/dd");
                data.qty = comm.sGetInt32(dr["qty"].ToString());
                data.days = comm.sGetInt32(dr["days"].ToString());
                result.Add(data);
            }

            return result;
        }




    }
}