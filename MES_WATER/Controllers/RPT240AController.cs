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
    public class RPT240AController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();
        CheckData CD = new CheckData();
        //需要用到的Repo
        RPT24_0000Repository repoRPT24_0000 = new RPT24_0000Repository();
        string sPrgCode = "RPT240A";
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Upload()
        {
            return View();
        }
        public class RPT240A
        {
            [DisplayName("品號")]
            public string ITEM_CODE { get; set; }

            [DisplayName("品名")]
            public string ITEM_NAME { get; set; }

            [DisplayName("規格")]
            public string ITEM_SPECIFICATION { get; set; }

            [DisplayName("最後入庫日")]
            public string LAST_RECEIPT_DATE { get; set; }

            [DisplayName("最後出庫日")]
            public string LAST_ISSUE_DATE { get; set; }

            [DisplayName("庫存量")]
            public decimal INVENTORY_QTY { get; set; }


            [DisplayName("呆料天數")]
            public string DAYDIFF { get; set; }
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
            List<RPT240A> result = new List<RPT240A>();
            
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

            ////
            //result = Get_RptData(query_data);

            // 回傳給view
            var returnObj = new
            {
                data = result
            };

            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }


        public List<RPT240A> Get_StatData(JqGridQueryData query_data)
        {
            List<RPT240A> result = new List<RPT240A>();
            string sPLANT_CODE = query_data.find("PLANT_CODE");//工廠
            string sWAREHOUSE_CODE = query_data.find("WAREHOUSE_CODE");//倉庫
            string sFEATURE_GROUP_CODE = query_data.find("FEATURE_GROUP_CODE");//群組
            string sDAYDIFF = query_data.find("DAYDIFF");//天數
            string sLAST_RECEIPT_DATE_S = query_data.find("LAST_RECEIPT_DATE", "S");//預計交貨日_起
            string sLAST_RECEIPT_DATE_E = query_data.find("LAST_RECEIPT_DATE", "E");//預計交貨日_止


            string sSql = "";
            int i;
            DataTable dtTmp = comm.Get_AlexDataTable(sSql);

            //抓取資料
            sSql = @" SELECT  top 1000  ITEM.ITEM_CODE, ITEM.ITEM_NAME, ITEM.ITEM_SPECIFICATION, ITEM_WAREHOUSE.LAST_RECEIPT_DATE, 
            ITEM_WAREHOUSE.LAST_ISSUE_DATE, ITEM_WAREHOUSE.INVENTORY_QTY,
		    IIF(ITEM_WAREHOUSE.LAST_RECEIPT_DATE>ITEM_WAREHOUSE.LAST_ISSUE_DATE, 
            DATEDIFF(day,ITEM_WAREHOUSE.LAST_RECEIPT_DATE,SYSDATETIME()),DATEDIFF(day,ITEM_WAREHOUSE.LAST_ISSUE_DATE,SYSDATETIME())) AS DAYDIFF
            FROM  ITEM 
            INNER JOIN ITEM_WAREHOUSE ON ITEM.ITEM_BUSINESS_ID = ITEM_WAREHOUSE.ITEM_ID 
            INNER JOIN WAREHOUSE ON ITEM_WAREHOUSE.WAREHOUSE_ID = WAREHOUSE.WAREHOUSE_ID 
			LEFT JOIN PLANT on	left(WAREHOUSE.WAREHOUSE_CODE, 3) = PLANT.PLANT_CODE
            INNER JOIN FEATURE_GROUP ON FEATURE_GROUP.FEATURE_GROUP_ID=ITEM.FEATURE_GROUP_ID
            WHERE ITEM_WAREHOUSE.INVENTORY_QTY<>0";

                    
            if (!string.IsNullOrEmpty(sPLANT_CODE)) { sSql += " AND PLANT.PLANT_CODE ='" + sPLANT_CODE + "'"; }
            if (!string.IsNullOrEmpty(sWAREHOUSE_CODE)) { sSql += " AND WAREHOUSE.WAREHOUSE_CODE ='" + sWAREHOUSE_CODE + "'"; }
            if (!string.IsNullOrEmpty(sFEATURE_GROUP_CODE)) { sSql += " AND FEATURE_GROUP_CODE ='" + sFEATURE_GROUP_CODE + "'"; }
            if (!string.IsNullOrEmpty(sDAYDIFF)) { sSql += @" AND IIF(ITEM_WAREHOUSE.LAST_RECEIPT_DATE>ITEM_WAREHOUSE.LAST_ISSUE_DATE, 
            DATEDIFF(day, ITEM_WAREHOUSE.LAST_RECEIPT_DATE, SYSDATETIME()),DATEDIFF(day, ITEM_WAREHOUSE.LAST_ISSUE_DATE, SYSDATETIME()))> '" + sDAYDIFF + "'"; }



            dtTmp = comm.Get_AlexDataTable(sSql);
            comm.Ins_BDP20_0000("admin", "RPT240A", "RPT", sSql);


            for (i = 0; i < dtTmp.Rows.Count; i++)
            {
                RPT240A data = new RPT240A();
                data.ITEM_CODE = dtTmp.Rows[i]["ITEM_CODE"].ToString();
                data.ITEM_NAME = dtTmp.Rows[i]["ITEM_NAME"].ToString();
                data.ITEM_SPECIFICATION = dtTmp.Rows[i]["ITEM_SPECIFICATION"].ToString();
                data.LAST_RECEIPT_DATE = dtTmp.Rows[i]["LAST_RECEIPT_DATE"].ToString();
                data.LAST_ISSUE_DATE = dtTmp.Rows[i]["LAST_ISSUE_DATE"].ToString();
                data.INVENTORY_QTY = comm.sGetDecimal(dtTmp.Rows[i]["INVENTORY_QTY"].ToString());
                data.DAYDIFF = dtTmp.Rows[i]["DAYDIFF"].ToString();

                result.Add(data);
            }

            return result;
        }
        public string Get_Data(string pDOC_NO, string pSequenceNumber, string sFieldName)
        {
            string sReturn = "";
            string sSql = "";
            sSql = "select distinct buy_reply,store_reply from MBA_E30 where DOC_NO='" + pDOC_NO + "' and SequenceNumber='" + pSequenceNumber + "'";
            DataTable dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                sReturn = comm.sGetString(dtTmp.Rows[0][sFieldName].ToString());
            }
            return sReturn;
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase upload, FormCollection form)
        {
            bool isUpdate = form.AllKeys.Contains("isUpdate") ? true : false;

            DataTable dt = comm.CsvToDataTable(upload);

            var dtCols = dt.Columns;
            int save_count = 0;
            List<RPT24_0000> notSaveList = new List<RPT24_0000>();

            foreach (DataRow dr in dt.Rows)
            {
                RPT24_0000 data = new RPT24_0000();
                data.ITEM_CODE = dr["品號"].ToString();
                data.ITEM_NAME = dr["品名"].ToString();
                data.ITEM_SPECIFICATION = dr["規格"].ToString();
                data.LAST_RECEIPT_DATE = dr["最後入庫日"].ToString();
                data.update_at = DateTime.Now.ToString("yyyy/MM/dd");
                data.usr_code = User.Identity.Name;

                //repoRPT23_0000.UpdateData(data);
                //save_count += 1;
                if (comm.Chk_RelData("MBA_E30", "DOC_NO", data.ITEM_CODE))
                {

                    repoRPT24_0000.InsertData(data);
                    save_count += 1;
                }
                else
                {
                    if (isUpdate)
                    {
                        repoRPT24_0000.UpdateData(data);
                        save_count += 1;
                    }
                    else
                    {
                        notSaveList.Add(data);
                    }
                }
            }

            ViewBag.count = save_count;
            ViewBag.notSaveList = notSaveList;
            return View();
        }
    }
}