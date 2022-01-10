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
    public class RPT230AController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();
        CheckData CD = new CheckData();

        public ActionResult Index()
        {
            return View();
        }

        public class RPT230A
        {
            [DisplayName("訂單單號")]
            public string DOC_NO { get; set; }

            [DisplayName("序號")]
            public string SequenceNumber { get; set; }

            [DisplayName("客戶")]
            public string CUSTOMER_NAME { get; set; }

            [DisplayName("品名")]
            public string ITEM_DESCRIPTION { get; set; }

            [DisplayName("規格")]
            public string ITEM_SPECIFICATION { get; set; }

            [DisplayName("訂單量")]
            public string BUSINESS_QTY { get; set; }

            [DisplayName("已交量")]
            public string DELIVERED_BUSINESS_QTY { get; set; }

            [DisplayName("未交量")]
            public string NON_QTY { get; set; }

            [DisplayName("已包量")]
            public string X_PACK_BUSINESS_QTY { get; set; }

            [DisplayName("預計交貨日")]
            public string PLAN_DELIVERY_DATE { get; set; }

            [DisplayName("交期確認")]
            public string X_CHECK_SHIP_DATE { get; set; }

            [DisplayName("客戶單號")]
            public string CUSTOMER_ORDER_NO { get; set; }

            [DisplayName("客戶品號")]
            public string CUSTOMER_ITEM_CODE { get; set; }

            [DisplayName("業務回覆")]
            public string TEST { get; set; }

            [DisplayName("生管回覆")]
            public string TEST01 { get; set; }

            [DisplayName("出貨回覆")]
            public string TEST02 { get; set; }
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
            List<RPT230A> result = new List<RPT230A>();

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


        public List<RPT230A> Get_StatData(JqGridQueryData query_data)
        {
            List<RPT230A> result = new List<RPT230A>();
            string sSALES_CENTER_CODE = query_data.find("SALES_CENTER_CODE");//銷售區域
            string sPLANT_CODE_S = query_data.find("PLANT_CODE", "S");//工廠_起
            string sPLANT_CODE_E = query_data.find("PLANT_CODE", "E");//工廠_止
            string sDOC_NO_S = query_data.find("DOC_NO", "S");//訂單單號_起
            string sDOC_NO_E = query_data.find("DOC_NO", "E");//訂單單號_止
            string sPLAN_DELIVERY_DATE_S = query_data.find("PLAN_DELIVERY_DATE", "S");//預計交貨日_起
            string sPLAN_DELIVERY_DATE_E = query_data.find("PLAN_DELIVERY_DATE", "E");//預計交貨日_止
            string sCUSTOMER_ORDER_NO_S = query_data.find("CUSTOMER_ORDER_NO", "S");//客戶單號_起
            string sCUSTOMER_ORDER_NO_E = query_data.find("CUSTOMER_ORDER_NO", "E");//客戶單號_止
            string sCUSTOMER_CODE = query_data.find("CUSTOMER_CODE");//客戶
            string sCUSTOMER_ITEM_CODE_S = query_data.find("CUSTOMER_ITEM_CODE", "S");//客戶單號_起
            string sCUSTOMER_ITEM_CODE_E = query_data.find("CUSTOMER_ITEM_CODE", "E");//客戶單號_止

            string sSql = "";
            int i;
            DataTable dtTmp = comm.Get_DataTable(sSql);

            //抓取資料
            sSql = @" SELECT  SALES_ORDER_DOC.DOC_NO, SALES_ORDER_DOC_D.SequenceNumber, CUSTOMER.CUSTOMER_NAME, --訂單單號、序號、客戶
                    SALES_ORDER_DOC_D.ITEM_DESCRIPTION, SALES_ORDER_DOC_D.ITEM_SPECIFICATION, --品名、規格
                    SALES_ORDER_DOC_D.BUSINESS_QTY, SALES_ORDER_DOC_SD.DELIVERED_BUSINESS_QTY, --訂單量、已交量
                    (SALES_ORDER_DOC_D.BUSINESS_QTY - SALES_ORDER_DOC_SD.DELIVERED_BUSINESS_QTY) AS NON_QTY, --未交量(訂單 - 已交)
                    SALES_ORDER_DOC_SD.X_PACK_BUSINESS_QTY, SALES_ORDER_DOC_SD.PLAN_DELIVERY_DATE, --已包量、預交期
                    SALES_ORDER_DOC_SD.X_CHECK_SHIP_DATE, SALES_ORDER_DOC.CUSTOMER_ORDER_NO, --交期確認、客戶單號
                    CUSTOMER_ITEM.CUSTOMER_ITEM_CODE--客戶品號
                    FROM    SALES_ORDER_DOC 
                    INNER JOIN SALES_ORDER_DOC_D ON SALES_ORDER_DOC.SALES_ORDER_DOC_ID = SALES_ORDER_DOC_D.SALES_ORDER_DOC_ID 
                    INNER JOIN SALES_ORDER_DOC_SD ON SALES_ORDER_DOC_D.SALES_ORDER_DOC_D_ID = SALES_ORDER_DOC_SD.SALES_ORDER_DOC_D_ID
                    INNER JOIN CUSTOMER ON SALES_ORDER_DOC.CUSTOMER_ID = CUSTOMER.CUSTOMER_BUSINESS_ID 
                    LEFT OUTER JOIN CUSTOMER_ITEM ON SALES_ORDER_DOC_D.CUSTOMER_ITEM_ID = CUSTOMER_ITEM.CUSTOMER_ITEM_ID 
                    INNER JOIN PLANT ON SALES_ORDER_DOC_SD.DELIVERY_PLANT_ID = PLANT.PLANT_ID 
                    INNER JOIN SALES_CENTER ON SALES_ORDER_DOC.Owner_Org_ROid = SALES_CENTER.SALES_CENTER_ID
                    WHERE SALES_ORDER_DOC_SD.[CLOSE]= 0";

            if (!string.IsNullOrEmpty(sSALES_CENTER_CODE)) { sSql += " AND SALES_CENTER.SALES_CENTER_CODE ='" + sSALES_CENTER_CODE + "'"; }
            if (!string.IsNullOrEmpty(sPLANT_CODE_S)) { sSql += " AND PLANT.PLANT_CODE >='" + sPLANT_CODE_S + "'"; }
            if (!string.IsNullOrEmpty(sPLANT_CODE_E)) { sSql += " AND PLANT.PLANT_CODE <='" + sPLANT_CODE_E + "'"; }
            if (!string.IsNullOrEmpty(sDOC_NO_S)) { sSql += " AND SALES_ORDER_DOC.DOC_NO >='" + sDOC_NO_S + "'"; }
            if (!string.IsNullOrEmpty(sDOC_NO_E)) { sSql += " AND SALES_ORDER_DOC.DOC_NO <='" + sDOC_NO_E + "'"; }
            if (!string.IsNullOrEmpty(sPLAN_DELIVERY_DATE_S)) { sSql += " AND PLANT.PLAN_DELIVERY_DATE >='" + sPLAN_DELIVERY_DATE_S + "'"; }
            if (!string.IsNullOrEmpty(sPLAN_DELIVERY_DATE_E)) { sSql += " AND PLANT.PLAN_DELIVERY_DATE <='" + sPLAN_DELIVERY_DATE_E + "'"; }
            if (!string.IsNullOrEmpty(sCUSTOMER_ORDER_NO_S)) { sSql += " AND CUSTOMER.CUSTOMER_ORDER_NO >='" + sCUSTOMER_ORDER_NO_S + "'"; }
            if (!string.IsNullOrEmpty(sCUSTOMER_ORDER_NO_E)) { sSql += " AND CUSTOMER.CUSTOMER_ORDER_NO <='" + sCUSTOMER_ORDER_NO_E + "'"; }
            if (!string.IsNullOrEmpty(sCUSTOMER_CODE)) { sSql += " AND CUSTOMER.CUSTOMER_CODE <='" + sCUSTOMER_CODE + "'"; }
            if (!string.IsNullOrEmpty(sCUSTOMER_ITEM_CODE_S)) { sSql += " AND CUSTOMER_ITEM.CUSTOMER_ITEM_CODE >='" + sCUSTOMER_ITEM_CODE_S + "'"; }
            if (!string.IsNullOrEmpty(sCUSTOMER_ITEM_CODE_E)) { sSql += " AND CUSTOMER_ITEM.CUSTOMER_ITEM_CODE <='" + sCUSTOMER_ITEM_CODE_E + "'"; }

            dtTmp = comm.Get_DataTable(sSql);
            comm.Ins_BDP20_0000("admin", "RPT230A", "RPT", sSql);


            for (i = 0; i < dtTmp.Rows.Count; i++)
            {
                RPT230A data = new RPT230A();
                data.DOC_NO = dtTmp.Rows[i]["DOC_NO"].ToString();
                data.SequenceNumber = dtTmp.Rows[i]["SequenceNumber"].ToString();
                data.CUSTOMER_NAME = dtTmp.Rows[i]["CUSTOMER_NAME"].ToString();
                data.ITEM_DESCRIPTION = dtTmp.Rows[i]["ITEM_DESCRIPTION"].ToString();
                data.ITEM_SPECIFICATION = dtTmp.Rows[i]["ITEM_SPECIFICATION"].ToString();
                data.BUSINESS_QTY = dtTmp.Rows[i]["BUSINESS_QTY"].ToString();
                data.DELIVERED_BUSINESS_QTY = dtTmp.Rows[i]["DELIVERED_BUSINESS_QTY"].ToString();
                data.NON_QTY = dtTmp.Rows[i]["NON_QTY"].ToString();
                data.X_PACK_BUSINESS_QTY = dtTmp.Rows[i]["X_PACK_BUSINESS_QTY"].ToString();
                data.PLAN_DELIVERY_DATE = dtTmp.Rows[i]["PLAN_DELIVERY_DATE"].ToString();
                data.X_CHECK_SHIP_DATE = dtTmp.Rows[i]["X_CHECK_SHIP_DATE"].ToString();
                data.CUSTOMER_ORDER_NO = dtTmp.Rows[i]["CUSTOMER_ORDER_NO"].ToString();
                data.CUSTOMER_ITEM_CODE = dtTmp.Rows[i]["CUSTOMER_ITEM_CODE"].ToString();

                result.Add(data);
            }

            return result;
        }

    }
}