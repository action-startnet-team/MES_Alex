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
    public class WMB130AController : Controller
    {
        string sPrgCode = "WMB130A";
        // 共用函數庫
        Comm comm = new Comm();
        WMB13_0000Repository repoWMB130 = new WMB13_0000Repository();


        public ActionResult Index()
        {
            return View();
        }

        public class WMB130A
        {
            [DisplayName("倉庫")]
            public string sto_name { get; set; }

            [DisplayName("儲位編號")]
            public string loc_code { get; set; }

            [DisplayName("儲位名稱")]
            public string loc_name { get; set; }

            [DisplayName("品號")]
            public string pro_code { get; set; }

            [DisplayName("品名")]
            public string pro_name { get; set; }

            [DisplayName("批號")]
            public string lot_no { get; set; }

            [DisplayName("數量")]
            public double sto_qty { get; set; }
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
            List<WMB130A> result = new List<WMB130A>();

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

            result.AddRange(Get_RptData(query_data));

            ////
            //result = Get_RptData(query_data);

            // 回傳給view
            var returnObj = new
            {
                data = result
            };

            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// (固定區)主檔 首頁 按下查詢按鈕 JqGrid資料來源
        /// </summary>
        /// <param name="pWhere">使用者下的查詢條件 Json</param>
        /// <returns></returns>
        public ActionResult Get_GridDataByQuery(string pWhere)
        {
            string sUsrCode = User.Identity.Name;
            //string sPrgCode = sPrgCode;

            List<WMB13_0000> list = new List<WMB13_0000>();
            list = repoWMB130.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        private List<WMB130A> Get_RptData(JqGridQueryData pQueryData)
        {
            List<WMB130A> result = new List<WMB130A>();

            // 查詢欄位值
            string sStaType = pQueryData.find("sta_type");
            string sStoType = pQueryData.find("sto_type");
            string sDate = pQueryData.find("cal_date");
            string sProCodeS = pQueryData.find("pro_code", "S");
            string sProCodeE = pQueryData.find("pro_code", "E");
            string sStoCode = pQueryData.find("sto_code");

            // 準備傳給dataTable的sql參數
            Dictionary<string, object> sSqlParams = new Dictionary<string, object>();

            // 設置SQL條件子句和參數
            string sDateWhere = "";
            if (!string.IsNullOrEmpty(sDate))
            {
                sDateWhere = " And ins_date <= @ins_date ";
                sSqlParams.Add("@ins_date", sDate + " 23:59:59");
            }
            string sProWhere = "";
            if (!string.IsNullOrEmpty(sProCodeS))
            {
                sProWhere = " AND s.pro_code BETWEEN @pro_code_s AND @pro_code_e ";
                sSqlParams.Add("@pro_code_s", sProCodeS);
                sSqlParams.Add("@pro_code_e", sProCodeE);
            }

            string sStoWhere = "";
            if (!string.IsNullOrEmpty(sStoCode))
            {
                sStoWhere = " AND s.sto_code = @sto_code ";
                sSqlParams.Add("@sto_code", sStoCode);
            }

            // sql語法，後續根據類別不同加入欄位和條件
            string sDefaultSql = @"Select s.* ,b.pro_name ,c.sto_name
                            from (
	                            Select pro_code, sto_code {0}
	                            ,( 
		                            SELECT ISNULL(SUM(CASE WHEN ins_type = 'I' THEN pro_qty ELSE pro_qty * -1 END), 0) 
		                            FROM WMT0200
		                            WHERE pro_code = a.pro_code
		                            and sto_code = a.sto_code"
                                    + sDateWhere
                                    + @"{1}
	                            ) as sto_qty
	                            from WMT0200 as a
	                            group by pro_code, sto_code {2}
                            ) as s
                            left join MEB20_0000 as b on b.pro_code = s.pro_code
                            left join WMB01_0000 as c on c.sto_code = s.sto_code
                            where sto_qty != 0"
                            + sProWhere + sStoWhere
                            + @"order by sto_code, pro_code {3}";

            string sSql = "";
            string sSelectCols_InSumSql = "";
            string sWhere_InSumSql = "";
            string sGroup_InSumSql = "";
            string sOrderCols = "";

            // 目前類別選項都有"請選擇"選項 (空值)
            // 預設若選到"請選擇"時
            sStoType = string.IsNullOrEmpty(sStoType) ? "sto_code" : sStoType;
            sStaType = string.IsNullOrEmpty(sStaType) ? "pro_code" : sStaType;

            // 倉庫xSKU
            if (sStoType == "sto_code" && sStaType == "pro_code")
            {
                sSelectCols_InSumSql = "";
                sWhere_InSumSql = "";
                sGroup_InSumSql = "";
                sOrderCols = "";

                sSql = string.Format(sDefaultSql, sSelectCols_InSumSql, sWhere_InSumSql, sGroup_InSumSql, sOrderCols);
            }

            // 儲位xSKU
            if (sStoType == "loc_code" && sStaType == "pro_code")
            {
                sSelectCols_InSumSql = ",loc_code";
                sWhere_InSumSql = " and loc_code = a.loc_code ";
                sGroup_InSumSql = " ,loc_code";
                sOrderCols = ",loc_code";

                sSql = string.Format(sDefaultSql, sSelectCols_InSumSql, sWhere_InSumSql, sGroup_InSumSql, sOrderCols);
            }

            // 倉庫x批號
            if (sStoType == "sto_code" && sStaType == "lot_no")
            {
                sSelectCols_InSumSql = ",lot_no";
                sWhere_InSumSql = " and lot_no = a.lot_no ";
                sGroup_InSumSql = " ,lot_no";
                sOrderCols = ",lot_no";

                sSql = string.Format(sDefaultSql, sSelectCols_InSumSql, sWhere_InSumSql, sGroup_InSumSql, sOrderCols);
            }

            // 儲位x批號
            if (sStoType == "loc_code" && sStaType == "lot_no")
            {
                sSelectCols_InSumSql = ",loc_code,lot_no";
                sWhere_InSumSql = " and loc_code = a.loc_code and lot_no = a.lot_no ";
                sGroup_InSumSql = " ,loc_code,lot_no";
                sOrderCols = ",loc_code,lot_no";

                sSql = string.Format(sDefaultSql, sSelectCols_InSumSql, sWhere_InSumSql, sGroup_InSumSql, sOrderCols);
            }

            // 
            DataTable dtTmp = comm.Get_DataTable(sSql, sSqlParams, true, new Dictionary<string, string>() { { "usr_type", "RPT" } });

            foreach (DataRow dr in dtTmp.Rows)
            {
                WMB130A data = new WMB130A();
                data.pro_code = dr["pro_code"].ToString();
                data.pro_name = dr["pro_name"].ToString();
                data.lot_no = dtTmp.Columns.Contains("lot_no") ? dr["lot_no"].ToString() : "";
                data.sto_name = dr["sto_name"].ToString();
                data.loc_code = dtTmp.Columns.Contains("loc_code") ? dr["loc_code"].ToString() : "";
                data.sto_qty = comm.sGetDouble(dr["sto_qty"].ToString());

                result.Add(data);
            }

            return result;
        }


    }
}