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
    public class RPT231AController : JsonNetController
    {
        // 共用函數庫
        Comm comm = new Comm();
        CheckData CD = new CheckData();
        //需要用到的Repo
        RPT23_0100Repository repoRPT23_0100 = new RPT23_0100Repository();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Upload()
        {
            return View();
        }
        public class RPT231A
        {
            [DisplayName("品號")]
            public string ITEM_CODE { get; set; }

            [DisplayName("預計到貨日")]
            public string PLAN_ARRIVAL_DATE { get; set; }

            [DisplayName("單號")]
            public string DOC_NO { get; set; }

            [DisplayName("供應商")]
            public string SUPPLIER_CODE { get; set; }

            [DisplayName("供應商名稱")]
            public string SUPPLIER_NAME { get; set; }

            [DisplayName("序號")]
            public string SequenceNumber { get; set; }


            [DisplayName("採購備註")]
            public string X_PURCHASE_REMARK { get; set; }

            [DisplayName("品名")]
            public string ITEM_NAME { get; set; }

            [DisplayName("規格")]
            public string ITEM_SPECIFICATION { get; set; }

            [DisplayName("訂單數量")]
            public decimal BUSINESS_QTY { get; set; }

            [DisplayName("業務單位")]
            public string UNIT_NAME { get; set; }

            [DisplayName("已交數量")]
            public decimal ARRIVED_BUSINESS_QTY { get; set; }

            [DisplayName("未交數量")]
            public decimal TEST { get; set; }

            [DisplayName("入庫數量")]
            public decimal RECEIPTED_BUSINESS_QTY { get; set; }

            [DisplayName("採購回覆")]
            public string buy_reply { get; set; }

            [DisplayName("倉管回覆")]
            public string store_reply { get; set; }
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
            List<RPT231A> result = new List<RPT231A>();

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


        public List<RPT231A> Get_StatData(JqGridQueryData query_data)
        {
            List<RPT231A> result = new List<RPT231A>();
            string sPLANT_CODE = query_data.find("PLANT_CODE");//工廠
            string sPLAN_ARRIVAL_DATE_S = query_data.find("PLAN_ARRIVAL_DATE", "S");//預計到貨日_起
            string sPLAN_ARRIVAL_DATE_E = query_data.find("PLAN_ARRIVAL_DATE", "E");//預計到貨日_止
            string sFEATURE_GROUP_CODE_S = query_data.find("FEATURE_GROUP_CODE", "S");//品號群組_起
            string sFEATURE_GROUP_CODE_E = query_data.find("FEATURE_GROUP_CODE", "E");//品號群組_止

            string sSql = "";
            int i;
            DataTable dtTmp = comm.Get_AlexDataTable(sSql);

            //抓取資料
            sSql = @" SELECT distinct TOP(1000) ITEM.ITEM_CODE, Convert(varchar,PURCHASE_ORDER_SD.PLAN_ARRIVAL_DATE,111) AS PLAN_ARRIVAL_DATE, PURCHASE_ORDER.DOC_NO, --品號、預計到貨日、採購單號
                        SUPPLIER.SUPPLIER_CODE, SUPPLIER.SUPPLIER_NAME, PURCHASE_ORDER_D.SequenceNumber, --供應商代號、名稱、序號
                        PURCHASE_ORDER_D.X_PURCHASE_REMARK, ITEM.ITEM_NAME, ITEM.ITEM_SPECIFICATION, --採購備註、品名、規格
                        PURCHASE_ORDER_D.BUSINESS_QTY, UNIT.UNIT_NAME, PURCHASE_ORDER_SD.ARRIVED_BUSINESS_QTY, --數量、單位、到貨量
                        PURCHASE_ORDER_SD.RECEIPTED_BUSINESS_QTY --入庫量
                        FROM    FEATURE_GROUP 
                        INNER JOIN ITEM ON FEATURE_GROUP.FEATURE_GROUP_ID = ITEM.FEATURE_GROUP_ID 
                        INNER JOIN PURCHASE_ORDER 
                        INNER JOIN PURCHASE_ORDER_D ON PURCHASE_ORDER.PURCHASE_ORDER_ID = PURCHASE_ORDER_D.PURCHASE_ORDER_ID 
                        INNER JOIN PURCHASE_ORDER_SD ON PURCHASE_ORDER_D.PURCHASE_ORDER_D_ID = PURCHASE_ORDER_SD.PURCHASE_ORDER_D_ID ON 
                        ITEM.ITEM_BUSINESS_ID = PURCHASE_ORDER_D.ITEM_ID 
                        INNER JOIN PLANT ON PURCHASE_ORDER.PLANT_ID = PLANT.PLANT_ID 
                        INNER JOIN SUPPLIER ON PURCHASE_ORDER.SUPPLIER_ID = SUPPLIER.SUPPLIER_BUSINESS_ID 
                        INNER JOIN UNIT ON ITEM.STOCK_UNIT_ID=UNIT.UNIT_ID
                        WHERE PURCHASE_ORDER.ApproveStatus='Y' and Convert(varchar,PURCHASE_ORDER_SD.PLAN_ARRIVAL_DATE,111) >= '2022/01/01'";
                    
            if (!string.IsNullOrEmpty(sPLANT_CODE)) { sSql += " AND PLANT.PLANT_CODE ='" + sPLANT_CODE + "'"; }
            if (!string.IsNullOrEmpty(sPLAN_ARRIVAL_DATE_S)) { sSql += " AND Convert(varchar,PURCHASE_ORDER_SD.PLAN_ARRIVAL_DATE,111) >='" + sPLAN_ARRIVAL_DATE_S + "'"; }
            if (!string.IsNullOrEmpty(sPLAN_ARRIVAL_DATE_E)) { sSql += " AND Convert(varchar,PURCHASE_ORDER_SD.PLAN_ARRIVAL_DATE,111) <='" + sPLAN_ARRIVAL_DATE_E + "'"; }
            if (!string.IsNullOrEmpty(sFEATURE_GROUP_CODE_S)) { sSql += " AND FEATURE_GROUP.FEATURE_GROUP_CODE >='" + sFEATURE_GROUP_CODE_S + "'"; }
            if (!string.IsNullOrEmpty(sFEATURE_GROUP_CODE_E)) { sSql += " AND FEATURE_GROUP.FEATURE_GROUP_CODE <='" + sFEATURE_GROUP_CODE_E + "'"; }
            sSql += " order by PURCHASE_ORDER_SD.PLAN_ARRIVAL_DATE";

            dtTmp = comm.Get_AlexDataTable(sSql);
            comm.Ins_BDP20_0000("admin", "RPT231A", "RPT", sSql);


            for (i = 0; i < dtTmp.Rows.Count; i++)
            {
                RPT231A data = new RPT231A();
                data.ITEM_CODE = dtTmp.Rows[i]["ITEM_CODE"].ToString();
                data.PLAN_ARRIVAL_DATE = dtTmp.Rows[i]["PLAN_ARRIVAL_DATE"].ToString();
                data.DOC_NO = dtTmp.Rows[i]["DOC_NO"].ToString();
                data.SUPPLIER_CODE = dtTmp.Rows[i]["SUPPLIER_CODE"].ToString();
                data.SUPPLIER_NAME = dtTmp.Rows[i]["SUPPLIER_NAME"].ToString();
                data.SequenceNumber = dtTmp.Rows[i]["SequenceNumber"].ToString();
                data.BUSINESS_QTY = comm.sGetDecimal(dtTmp.Rows[i]["BUSINESS_QTY"].ToString());
                data.X_PURCHASE_REMARK = dtTmp.Rows[i]["X_PURCHASE_REMARK"].ToString();
                data.ITEM_NAME = dtTmp.Rows[i]["ITEM_NAME"].ToString();
                data.ITEM_SPECIFICATION = dtTmp.Rows[i]["ITEM_SPECIFICATION"].ToString();
                data.BUSINESS_QTY = comm.sGetDecimal(dtTmp.Rows[i]["BUSINESS_QTY"].ToString());
                data.UNIT_NAME = dtTmp.Rows[i]["UNIT_NAME"].ToString();
                data.ARRIVED_BUSINESS_QTY = comm.sGetDecimal(dtTmp.Rows[i]["ARRIVED_BUSINESS_QTY"].ToString());
                data.RECEIPTED_BUSINESS_QTY = comm.sGetDecimal(dtTmp.Rows[i]["RECEIPTED_BUSINESS_QTY"].ToString());
                data.buy_reply = Get_Data(data.DOC_NO, data.SequenceNumber, "buy_reply");
                data.store_reply = Get_Data(data.DOC_NO, data.SequenceNumber, "store_reply");

                result.Add(data);
            }

            return result;
        }
        public string Get_Data(string pDOC_NO, string pSequenceNumber, string sFieldName)
        {
            string sReturn = "";
            string sSql = "";
            sSql = "select buy_reply,store_reply from MBA_E30 where DOC_NO='" + pDOC_NO + "' and SequenceNumber='" + pSequenceNumber + "'";
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
            List<RPT23_0100> notSaveList = new List<RPT23_0100>();

            foreach (DataRow dr in dt.Rows)
            {
                RPT23_0100 data = new RPT23_0100();
                data.DOC_NO = dr["單號"].ToString();
                data.SequenceNumber = dr["序號"].ToString();
                data.buy_reply = dr["採購回覆"].ToString();
                data.store_reply = dr["倉管回覆"].ToString();
                data.update_at = DateTime.Now.ToString("yyyy/MM/dd");
                data.usr_code = User.Identity.Name;

                //repoRPT23_0000.UpdateData(data);
                //save_count += 1;
                if (comm.Chk_RelData("MBA_E30", "DOC_NO", data.DOC_NO))
                {

                    repoRPT23_0100.InsertData(data);
                    save_count += 1;
                }
                else
                {
                    if (comm.Chk_RelData("MBA_E30", "SequenceNumber", data.SequenceNumber))
                    {
                        repoRPT23_0100.InsertData(data);
                        save_count += 1;
                    }
                    else
                    {
                        if (isUpdate)
                        {
                            repoRPT23_0100.UpdateData(data);
                            save_count += 1;
                        }
                        else
                        {
                            notSaveList.Add(data);
                        }
                    }
                }
            }

            ViewBag.count = save_count;
            ViewBag.notSaveList = notSaveList;
            return View();
        }
    }
}