using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MES_WATER.Models;
using MES_WATER.Repository;
using System.Data;
using System.Linq.Dynamic;
using System.Web.Security;
using System.Reflection;
using Newtonsoft.Json;
using System.Collections;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Formula.Functions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MES_WATER.Controllers
{
    [Authorize] //登入驗證
    [HandleError(View = "Error")]  //錯誤導向

    public class DSB110CController : JsonNetController
    {
        //程式代號
        string sPrgCode = "DSB110C";
        public string sJson { get; set; }
        //需要用到的Repo
        ECT02_0200Repository repoECT02_0200 = new ECT02_0200Repository();
        //共用函式庫
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();
        ExportController exrepo = new ExportController();

        /* 資料處理 向下 */
        /// <summary>
        /// (固定區) 主檔 首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //要結合權限控制
            //ViewBag.limit_str = comm.Get_LimitByUsrCode(User.Identity.Name, sPrgCode);
            ViewBag.prg_code = sPrgCode;

            // 使用者, controllerName, actionName
            string usr_code = User.Identity.Name;
            string prg_code = sPrgCode;
            string view_code = ControllerContext.RouteData.Values["action"].ToString();

            //取得欄位寬度
            List<BDP30_0000> colWidth_list = comm.Get_BDP30_0000(usr_code, prg_code, view_code);
            List<BDP30_0000> colWidth_list_D1 = comm.Get_BDP30_0000(usr_code, prg_code, view_code + "_D1");
            ViewBag.colWidth_list = colWidth_list;
            ViewBag.colWidth_list_D1 = colWidth_list_D1;

            //取得欄位顯示
            List<BDP30_0100> is_show_list = comm.Get_BDP30_0100(usr_code, prg_code, view_code);
            List<BDP30_0100> is_show_D1_list = comm.Get_BDP30_0100(usr_code, prg_code, view_code + "_D1");
            ViewBag.is_show_list = is_show_list;
            ViewBag.is_show_D1_list = is_show_D1_list;

            ViewBag.isDisplay = "N";
            return View();
        }

        /// <summary>
        /// (固定區)主檔 首頁 按下查詢按鈕 JqGrid資料來源
        /// </summary>
        /// <param name="pWhere">使用者下的查詢條件 Json</param>
        /// <returns></returns>
        public ActionResult Get_GridDataByQuery(string pWhere,string pMacCode,string pStartDate,string pEndDate)
        {
            string sUsrCode = User.Identity.Name;
            List<DSB11_0200> list = new List<DSB11_0200>();
            //string sPrgCode = sPrgCode;
            string sql = " select MO_DOC_NO,pro_code,min(a.TRANSACTION_DATE) as date from MBA_E10 a " +
                         " left join MET02_0000 b on a.MO_DOC_NO = b.mo_code " +
                         " where a.TRANSACTION_DATE between '" + pStartDate + "' and '" + pEndDate + "' and a.QTY<>0 ";
            if (!string.IsNullOrEmpty(pMacCode)) { sql += " and XMACHINE_CODE='" + pMacCode + "' "; }
            sql+= " group by MO_DOC_NO,pro_code ";
            DataTable dt = comm.Get_DataTable(sql);
            if (dt.Rows.Count>0)
            {
                foreach (DataRow dr in dt.Rows) {
                    DSB11_0200 data = new DSB11_0200();
                    data.mo_code = dr["MO_DOC_NO"].ToString();
                    data.pro_code = dr["pro_code"].ToString();
                    data.pro_time = dr["date"].ToString();
                    data.ok_qty = comm.sGetDouble(SumTable(data.mo_code, pStartDate, pEndDate).Rows[0]["qty"].ToString()).ToString("0");
                    data.ng_qty = comm.sGetDouble(SumTable(data.mo_code, pStartDate, pEndDate).Rows[0]["ng_qty"].ToString()).ToString("0");
                    list.Add(data);
                }
            }
            //list = repoDSB11_0200.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public DataTable SumTable(string pMoCode, string pStartDate, string pEndDate)
        {
            string sql = " select sum(QTY) as qty,sum(SCRAP_QTY) as ng_qty from MBA_E10 " +
                         " where MO_DOC_NO = '"+ pMoCode + "' and TRANSACTION_DATE between '"+ pStartDate + "' and '"+ pEndDate + "' ";
            DataTable dt = comm.Get_DataTable(sql);
            return dt;
        }

        public ActionResult Get_Info(string query_date_s, string query_date_e, string mac_code)
        {
            List<object> list = new List<object>();
            double work_time = 0;
            string oee = "";
            double qty = 0;
            DateTime dt = Convert.ToDateTime(query_date_s);
            DateTime dt2 = Convert.ToDateTime(query_date_e);
            TimeSpan ts = dt2.Subtract(dt);
            for (int i = 0; i <=ts.TotalDays;i++)
            {
                string start_date = Convert.ToDateTime(query_date_s).AddDays(i).ToString("yyyy-MM-dd");
                string end_date = Convert.ToDateTime(start_date).AddDays(1).ToString("yyyy-MM-dd");
                string sql = " select max(TRANSACTION_DATE) as max_time,min(TRANSACTION_DATE) as min_time,sum(QTY) as qty from MBA_E10 " +
                             " where TRANSACTION_DATE between '"+ start_date + "' and '"+ end_date + "' ";
                if (!string.IsNullOrEmpty(mac_code)) { sql += " and XMACHINE_CODE='" + mac_code + "' "; }
                DataTable dtime = comm.Get_DataTable(sql);
                TimeSpan ts2 = new TimeSpan();
                string maxtime = "2021/01/01";
                string min_time = "2021/01/01";
                double qty2 = 0;
                if (!Convert.IsDBNull(dtime.Rows[0]["max_time"]))
                {
                    maxtime = dtime.Rows[0]["max_time"].ToString();
                    min_time = dtime.Rows[0]["min_time"].ToString();
                    qty2 = Convert.ToDouble(dtime.Rows[0]["qty"].ToString());
                }
                if (dtime.Rows.Count>0)
                {
                   ts2 = Convert.ToDateTime(maxtime).Subtract(Convert.ToDateTime(min_time));
                }
                work_time += ts2.TotalHours;
                qty += qty2; 
            }
            //oee = (work_time / (8 * (ts.TotalDays+1)) * 100).ToString("0") + "%";
            oee = (qty / (13*(ts.TotalDays + 1)*270) * 100).ToString("0") + "%";
            var data = new
            {
                oee_data = oee,
                qty_data = qty,
            };
            list.Add(data);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetData(string query_date_s, string query_date_e, string mac_code)
        {
            string fquery_date_s = Convert.ToDateTime(query_date_s).ToString("yyyy-MM-dd");
            string fquery_date_e = Convert.ToDateTime(query_date_e).AddDays(1).ToString("yyyy-MM-dd");
            string sql = " select sum(Convert(int,QTY)) as qty,CONVERT(varchar, TRANSACTION_DATE, 23) as TDate from MBA_E10 " +
                         " where TRANSACTION_DATE between '" + fquery_date_s + "' and '" + fquery_date_e + "' ";
            if (!string.IsNullOrEmpty(mac_code)) { sql += " and XMACHINE_CODE='" + mac_code + "' "; }
            sql += " group by CONVERT(varchar, TRANSACTION_DATE, 23) order by CONVERT(varchar, TRANSACTION_DATE, 23)";
            DataTable dt = comm.Get_DataTable(sql);

            //日期區間
            DateTime dt1 = DateTime.Parse(query_date_s);
            DateTime dt2 = DateTime.Parse(query_date_e);
            int days = (dt2 - dt1).Days;

            int j = 0;
            List<Object> list = new List<Object>();
            for (int i = 0; i <= days; i++)
            {
                string date = dt1.AddDays(i).ToString("MM月dd日");
                string date2 = dt1.AddDays(i).ToString("yyyy-MM-dd");
                string qty = "0";
                if (dt.Rows.Count>0) {
                    if (j < dt.Rows.Count)
                    {
                        if (date2 == dt.Rows[j]["TDate"].ToString())
                        {
                            date = Convert.ToDateTime(dt.Rows[j]["TDate"]).ToString("MM月dd日");
                            qty = dt.Rows[j]["qty"].ToString();
                            j++;
                        }
                    }
                }

                var obj = new
                {
                    ins_date = date,
                    totalqty = qty
                };
                list.Add(obj);
            }
            comm.Ins_BDP20_0000(User.Identity.Name, sPrgCode, "select", "", "");
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    
    //資料檢查 向下//
    //主檔的檢查
    [HttpPost]
        public ActionResult Check_Data(FormCollection form, ECT02_0200 model)
        {
            bool isSuccess = false;
            //檢查資料代碼是否重覆
            string key = gmv.GetKey<ECT02_0200>(new ECT02_0200());
            string sWhere = "where " + key + "='" + form[key].ToString() + "'";
            bool hasRow = !comm.Chk_RelData("ECT02_0200", sWhere);
            if (hasRow)
            {
                ModelState.AddModelError(key, "代碼已存在!");
                isSuccess = true;
            }
            var returnData = new
            {
                // 成功與否
                IsSuccess = isSuccess,
                // ModelState錯誤訊息 
                ModelStateErrors = ModelState.Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(k => k.Key, k => k.Value.Errors.Select(e => e.ErrorMessage).ToArray())
            };
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(returnData), "application/json");
        }

        /* 資料檢查 向下 */
        /// <summary>
        /// (修改處) 新增資料前的資料檢查點
        /// </summary>
        /// <param name="form">畫面上輸入的值集合</param>
        /// <returns></returns>
        private bool Chk_Ins_Main(FormCollection form)
        {
            bool bIsOK = true;

            //** 依作業不同有不同的檢查點 向下
            //檢查有錯時用以下程式碼顯示錯誤訊息
            //程式參考
            //if (!comm.Chk_IdNo(form["pro_code"]).isValid)
            //{
            //    bDataIsValid = false;
            //    ModelState.AddModelError("pro_code", comm.Chk_IdNo(form["pro_code"]).message);
            //}

            //** 依作業不同有不同的檢查點 向上

            //檢查結果回傳
            return bIsOK;
        }

        /// <summary>
        /// (修改處) 修改資料前的資料檢查點
        /// </summary>
        /// <param name="form">畫面上輸入的值集合</param>
        /// <returns></returns>
        private bool Chk_Upd_Main(FormCollection form)
        {
            // 自訂義資料檢查開始
            bool bIsOK = true;

            //** 依作業不同有不同的檢查點 向下

            //檢查有錯時用以下程式碼顯示錯誤訊息
            //程式參考
            //if (!comm.Chk_IdNo(form["pro_code"]).isValid)
            //{
            //    bDataIsValid = false;
            //    ModelState.AddModelError("pro_code", comm.Chk_IdNo(form["pro_code"]).message);
            //}

            //** 依作業不同有不同的檢查點 向上
            return bIsOK;
        }

        /// <summary>
        /// (修改處) 刪除資料前的資料檢查點
        /// </summary>
        /// <param name="form">畫面上輸入的值集合</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Chk_Del_Main(FormCollection form)
        {
            bool bIsOK = true;
            string message = "";
            //檢查資料代碼是否重覆

            //** 依作業不同有不同的檢查點 向下

            //檢查有錯時用以下程式碼顯示錯誤訊息
            //程式參考
            //if (true)
            //{
            //    bIsOK = false;
            //    message += "<div class='text-danger'>";
            //    message += "<li> 測試1 </li>";
            //    message += "</div>";
            //}

            //** 依作業不同有不同的檢查點 向上

            var result = new
            {
                isValid = bIsOK,
                message = message
            };

            return Json(result);
        }

        /* 資料檢查 向上 */

        /// <summary>
        /// 前端Ajax控項資料代名稱
        /// </summary>
        /// <param name="pCusCode">客戶編號</param>
        /// <param name="pType">要取回的欄位</param>
        /// <returns></returns>
        public string Get_ProData(string pProCode, string pType)
        {
            string sReturn = "";
            sReturn = comm.Get_QueryData("STB01_0000", pProCode, "pro_code", pType);
            return sReturn;
        }

        /// <summary>
        /// 取得廠商的聯絡人
        /// </summary>
        /// <param name="sup_code"></param>
        /// <returns></returns>
        public JsonResult Get_SupAtn(string sup_code)
        {
            string sSql = "select STB10_0100.* from STB10_0100 where sup_code = @sup_code ";
            DataTable dtTmp = comm.Get_DataTable(sSql, "sup_code", sup_code);

            return Json(dtTmp, JsonRequestBehavior.AllowGet);
        }
    }
}