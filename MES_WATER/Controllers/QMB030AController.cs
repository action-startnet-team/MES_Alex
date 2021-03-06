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
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Dapper;

namespace MES_WATER.Controllers
{
    [Authorize] //登入驗證
    [HandleError(View = "Error")]  //錯誤導向

    public class QMB030AController : JsonNetController
    {
        //程式代號
        public static string sPrgCode = "QMB030A";

        //需要用到的Repo
        QMB03_0000Repository repoQMB03_0000 = new QMB03_0000Repository();
        QMB03_0100Repository repoQMB03_0100 = new QMB03_0100Repository();
        QMB02_0000Repository repoQMB02_0000 = new QMB02_0000Repository();

        //共用函式庫
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();
        DynamicTable DT = new DynamicTable();

        //取得單號
        WebReference.WmsApi ws = new WebReference.WmsApi();


        /* 資料處理 向下 */
        /// <summary>
        /// (固定區) 主檔 首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //要結合權限控制
            ViewBag.limit_str = comm.Get_LimitByUsrCode(User.Identity.Name, sPrgCode);
            //comm.ConvertNull("QMB03_0000");

            // 使用者, controllerName, actionName
            string usr_code = User.Identity.Name;
            string prg_code = ControllerContext.RouteData.Values["controller"].ToString();
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
            ViewBag.prg_code = sPrgCode;
            return View();
        }


        /// <summary>
        /// (固定區)主檔 首頁 按下查詢按鈕 JqGrid資料來源
        /// </summary>
        /// <param name="pWhere">使用者下的查詢條件 Json</param>
        /// <returns></returns>
        public ActionResult Get_GridDataByQuery(string pWhere)
        {
            string sUsrCode = User.Identity.Name;
            //string sPrgCode = pubPrgCode;

            List<QMB03_0000> list = new List<QMB03_0000>();
            list = repoQMB03_0000.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // 明細檔 jqgrid資料來源
        [HttpPost]
        public ActionResult Get_GridData_D1(string pTkCode)
        {
            //string sPrgCode = pubPrgCode;
            string sUsrCode = User.Identity.Name;
            List<QMB03_0100> list = new List<QMB03_0100>();
            list = repoQMB03_0100.Get_DataList(sUsrCode, sPrgCode, pTkCode);

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// (修改區) 主檔 上傳
        /// 1.上傳模式下控項的預設值在這邊設定
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload(string QsheetCode= "")
        {
            //新增模式的預設值
            ViewData["qsheet_code"] = QsheetCode;
            return View();
        }
        /// <summary>
        /// (修改區) 主檔 上傳
        /// 1.上傳模式下控項的預設值在這邊設定
        /// </summary>
        /// <returns></returns>
        public ActionResult DeteleFile(string QsheetCode="")
        {
            string nextAspx = "Upload?QsheetCode=" + QsheetCode;
            //新增模式的預設值
            TempData["message"] = "刪除成功";
            if (!comm.FileByDelData(QsheetCode))
            {
                TempData["message"] = "無資料";
            }
            return Redirect(nextAspx);
        }


        /// <summary>
        /// (修改區) 主檔 新增
        /// 1.新增模式下控項的預設值在這邊設定
        /// </summary>
        /// <returns></returns>
        public ActionResult Insert()
        {
            //新增模式的預設值
            QMB03_0000 data = new QMB03_0000();

            return View(data);
        }


        /// <summary>
        /// (修改區) 主檔 修改
        /// 1.依資料鍵值到DB取回資料呈現在修改模式頁面
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        /// <returns></returns>
        public ActionResult Update(string pTkCode)
        {
            QMB03_0000 newData = repoQMB03_0000.GetDTO(pTkCode);
            return View(newData);
        }

        public ActionResult Get_InsertData_D1(string pWhere)
        {
            string sUsrCode = User.Identity.Name;

            List<QMB02_0000> list = new List<QMB02_0000>();
            list = repoQMB02_0000.Get_DataListByQuery(sUsrCode, "QMB020A", pWhere);

            return Json(list, JsonRequestBehavior.AllowGet);
        }



        public ActionResult ConvertToEPB(string pTkCode) {
            string sSql = "";
            object data = new object();
            //先判斷有沒有建立過該檢驗紀錄表
            if (string.IsNullOrEmpty(comm.Get_Data("EPB02_0000", pTkCode,"epb_code", "epb_code")) ) {                
                //建立電子表單主檔
                data = new {
                    epb_code = pTkCode,
                    epb_name = comm.Get_Data("QMB03_0000", pTkCode,"qsheet_code","qsheet_name"),
                    epb_type_code = "QSEET",
                    is_use = "Y",
                    save_type = "D",
                    save_method = pTkCode,
                };
                DT.InsertData("EPB02_0000", data);                              
            }
            //建立電子表單明細檔
            sSql = "select * from QMB03_0100" +
                   " where qsheet_code = @qsheet_code";
            var dtTmp = comm.Get_DataTable(sSql, "qsheet_code", pTkCode);
            for (int i = 0; i < dtTmp.Rows.Count; i++) {
                DataRow r = dtTmp.Rows[i];
                
                string sQtestItemType = comm.Get_Data("QMB02_0000", r["qtest_item_code"].ToString(), "qtest_item_code", "qtest_item_type"); //檢驗資料類型                
                string sDataCode = r["datacode"].ToString();
                string sQtestUp = comm.sGetDecimal(comm.Get_Data("QMB03_0100", sDataCode, "datacode", "qtest_up")).ToString("G29");
                string sQtestDown = comm.sGetDecimal(comm.Get_Data("QMB03_0100", sDataCode, "datacode", "qtest_down")).ToString("G29");
                string sFieldMemo = "";

                string sProCode = comm.Get_Data("QMB02_0000", r["qtest_item_code"].ToString(), "qtest_item_code", "pro_code");
                string sUnitCode = comm.Get_Data("WMB06_0000", sProCode, "pro_code", "unit_code");
                string sUnitName = comm.Get_Data("WMB07_0000", sUnitCode, "unit_code", "unit_name");

                if (sQtestItemType == "B") { sFieldMemo = sQtestDown + " ~ " + sQtestUp + " " + sUnitName; }

                data = new
                {
                    epb_code = pTkCode,
                    field_code = r["qtest_item_code"].ToString(),
                    field_name = comm.Get_Data("QMB02_0000", r["qtest_item_code"].ToString(), "qtest_item_code", "qtest_item_name"),
                    field_memo = sFieldMemo,
                    scr_no = (i + 1).ToString(),
                    ctr_type = Set_CtrTypeByQtest(sQtestItemType),
                    ctr_default_value = Set_CtrDefaultValueByQtest(sQtestItemType),
                    data_type = Set_DataTypeByQtest(sQtestItemType),
                    field_length = "0",
                    is_key = "N",
                    need_value = "N",
                    is_multi = "Y",
                    sor_table = "QMB03_0100",
                    sor_key = pTkCode,
                    sor_field = r["datacode"].ToString(),
                };
                //先刪除再新增
                comm.Del_QueryData("EPB02_0100", "sor_field", r["datacode"].ToString());
                DT.InsertData("EPB02_0100", data);
            }
            return RedirectToAction("Update", "EPB020A",new { pTkCode = pTkCode });
        }



        public bool Chk_FieldExist(string pTkCode, string pField) {
            bool val = false;
            string sSql = "select * from EPB02_0100" +
                          " where epb_code = @epb_code" +
                          "   and field_code = @field_code";
            var dtTmp = comm.Get_DataTable(sSql, "epb_code,field_code", pTkCode + "," + pField);
            if (dtTmp.Rows.Count > 0) {
                val = true;
            }
            return val;
        }

        /// <summary>
        /// 根據檢驗資料類型改變電子表單項目的類別
        /// </summary>
        /// <param name="pQtestItemType"></param>
        /// <param name="pField"></param>
        public string Set_DataTypeByQtest(string pQtestItemType)
        {
            string val = "S"; //預設S
            switch (pQtestItemType) {
                case "B":
                    //起止值
                    val = "F";
                    break;
                case "C":
                    //判定值
                    break;
                case "D":
                    //文字記錄
                    break;
            }
            return val;
        }

        public string Set_CtrTypeByQtest(string pQtestItemType)
        {
            string val = "T"; 
            switch (pQtestItemType)
            {
                case "B":
                    //起止值
                    break;
                case "C":
                    //判定值
                    val = "C";
                    break;
                case "D":
                    //文字記錄
                    break;
            }
            return val;
        }

        public string Set_CtrDefaultValueByQtest(string pQtestItemType)
        {
            string val = ""; 
            switch (pQtestItemType)
            {
                case "B":
                    //起止值
                    val = "0";
                    break;
                case "C":
                    //判定值
                    break;
                case "D":
                    //文字記錄
                    break;
            }
            return val;
        }



        /// <summary>
        /// (修改區) 主檔的新增頁面將值存進DB
        /// </summary>
        /// <param name="form">畫面上輸入的form元件中的控制項集合</param>
        /// <param name="model">要存檔的model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Insert(FormCollection form, QMB03_0000 model)
        {
            // MVC model驗證
            if (ModelState.IsValid)
            {
                // 自訂義 資料驗證
                bool bIsOK = Chk_Ins_Main(form);

                // 資料驗證失敗
                if (!bIsOK)
                {
                    ViewBag.showErrMsg = true;
                    return View(model);
                }
                // 初始
                QMB03_0000 data = new QMB03_0000();

                // 預設賦值
                comm.Set_ModelValue(data, form);

                // 特別取值
                if (data.qsheet_type != "IPQC" && data.qsheet_type != "FQC")
                {
                    data.work_code = "";
                }

                //data.cus_name = comm.Get_QueryData("STB08_0000", data.cus_code, "cus_code", "cus_name");
                //data.is_dps = "N";
                data.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
                data.ins_time = DateTime.Now.ToString("HH:mm:ss");
                data.usr_code = User.Identity.Name;

                repoQMB03_0000.InsertData(data);
                // 新增紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);
                return RedirectToAction("Update", sPrgCode, new { pTkCode = data.qsheet_code });
            }
            ViewBag.showErrMsg = true;
            return View(model);
        }

        /// <summary>
        /// (修改區) 明細jqGrid的新增處理，將值存進DB
        /// </summary>
        /// <param name="form">畫面上輸入的form元件中的控制項集合</param>
        /// <returns></returns>
        [HttpPost]
        public void Insert_D1(FormCollection form)
        {
            QMB03_0100 data = new QMB03_0100();

            comm.Set_ModelValue(data, form);

            data.datacode = data.qsheet_code + "-" + data.scr_no;

            repoQMB03_0100.InsertData(data);
            // 新增紀錄資料
            comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);
        }
        /// <summary>
        /// (修改區) 主檔 修改
        /// 1.依資料鍵值到DB取回資料呈現在修改模式頁面
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(FormCollection form, QMB03_0000 model)
        {
            // MVC model驗證
            if (ModelState.IsValid)
            {
                // 自訂義 資料驗證
                bool bIsOK = Chk_Upd_Main(form);

                // 資料驗證失敗
                if (!bIsOK)
                {
                    ViewBag.showErrMsg = true;
                    return View(model);
                }

                QMB03_0000 data = new QMB03_0000();

                comm.Set_ModelValue(data, form);
                if (data.qsheet_type != "IPQC" && data.qsheet_type != "FQC")
                {
                    data.work_code = "";
                }

                QMB03_0000 sBefore = comm.GetData<QMB03_0000>(data);
                repoQMB03_0000.UpdateData(data);
                //更新紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "update", sBefore, data);

                return RedirectToAction("Index");
            }
            ViewBag.showErrMsg = true;
            return View(model);
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase upload, FormCollection form)
        {
            string nextAspx = "Update?pTkCode="+ form["qsheet_code"].ToString();
            if (!comm.FileByUpdateData(upload,form["qsheet_code"].ToString()))
            {
                return View();
            }

            return Redirect(nextAspx);
        }

        /// <summary>
        /// (修改區) 明細jqGrid的修改處理，將值存進DB
        /// </summary>
        /// <param name="form">畫面上輸入的form元件中的控制項集合</param>
        /// <param name="model">要存檔的model</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Update_D1(FormCollection form)
        {
            CheckDataResult result = Chk_Upd_D1(form);
            if (result.bIsOK)
            {
                QMB03_0100 data = new QMB03_0100();

                comm.Set_ModelValue(data, form);

                data.datacode = data.qsheet_code + "-" + data.scr_no;

                QMB03_0100 sBefore = comm.GetData<QMB03_0100>(data);
                repoQMB03_0100.UpdateData(data);
                //更新紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "update", sBefore, data);

            }
            return Json(result);
        }

        /// <summary>
        /// (修改區) 按下刪除後刪除DB動作
        /// </summary>
        /// <param name="pTkCode">要刪除的鍵值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(string pTkCode)
        {
            //刪主檔
            QMB03_0000 sBefore = comm.GetData<QMB03_0000>(pTkCode);
            repoQMB03_0000.DeleteData(pTkCode);
            //刪除紀錄資料
            comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "delete", sBefore, "");
            return RedirectToAction("Index");
        }

        /// <summary>
        /// (修改區) 明細jqGrid的刪除DB動作
        /// </summary>
        /// <param name="form">畫面上輸入的form元件中的控制項集合</param>
        /// <param name="model">要存檔的model</param>
        /// <returns></returns>
        [HttpPost]
        public void Delete_D1(String pTkCode)
        {
            QMB03_0100 sBefore = comm.GetData<QMB03_0100>(pTkCode);
            repoQMB03_0100.DeleteData(pTkCode);
            //刪除紀錄資料
            comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "delete", sBefore, "");
        }

        /* 資料處理 向上 */

        //資料檢查 向下//
        //主檔的檢查
        [HttpPost]
        public ActionResult Check_Data(FormCollection form, QMB03_0000 model)
        {
            bool isSuccess = false;
            //檢查資料代碼是否重覆
            string key = gmv.GetKey<QMB03_0000>(new QMB03_0000());
            string sWhere = "where " + key + "='" + form[key].ToString() + "'";
            bool hasRow = !comm.Chk_RelData("QMB03_0000", sWhere);
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

        //資料檢查 向上//

        /* 資料檢查 向下 */
        // 主檔 新增資料的檢查
        private bool Chk_Ins_Main(FormCollection form)
        {
            bool bIsOK = true;

            //** 依作業不同有不同的檢查點 向下

            //if (form["atn_name"].Contains("姓名2"))
            //{
            //    bIsOK = false;
            //    ModelState.AddModelError("atn_name", "聯絡人是姓名2，不可新增");
            //}

            //** 依作業不同有不同的檢查點 向上

            //檢查結果回傳
            return bIsOK;
        }

        // 主檔 修改資料的檢查
        private bool Chk_Upd_Main(FormCollection form)
        {
            // 自訂義資料檢查開始
            bool bIsOK = true;

            //** 依作業不同有不同的檢查點 向下
            //if (form["per_code"] == "A79")
            //{
            //    bIsOK = false;
            //    ModelState.AddModelError("per_code", "採購人員是A79 - " + comm.Get_QueryData("STB18_0000", form["per_code"], "per_code", "per_name") + "，不可修改");
            //}
            //** 依作業不同有不同的檢查點 向上
            return bIsOK;
        }

        // 主檔 刪除資料的檢查
        [HttpPost]
        public JsonResult Chk_Del_Main(FormCollection form)
        {
            bool bIsOK = true;
            string message = "";

            //** 依作業不同有不同的檢查點 向下
            //if (comm.sGetInt32(form["pur_amt"]) > 0)
            //{
            //    bIsOK = false;
            //    message += "<div class='text-danger'>";
            //    message += "<li> 金額大於零，不可刪除 </li>";
            //    message += "</div>";
            //}
            //** 依作業不同有不同的檢查點 向上

            var result = new
            {
                bIsOK = bIsOK,
                message = message
            };

            return Json(result);
        }

        // 明細檔 新增資料的檢查
        [HttpPost]
        public ActionResult Chk_Ins_D1(FormCollection form)
        {
            bool bIsOK = true;
            string message = "";

            //檢查scr_no是否為四位且皆為數字
            if (!Regex.IsMatch(form["scr_no"], "^.{4}$") || !Regex.IsMatch(form["scr_no"], "^.[0-9]+$"))
            {
                bIsOK = false;
                message += "<p> 順序應為四位數字! </p> ";
            }
            //檢查scr_no有無重複
            if (!comm.Chk_RelData("QMB03_0100", "where qsheet_code = @qsheet_code and scr_no = @scr_no", "qsheet_code,scr_no", form["qsheet_code"] + "," + form["scr_no"]))
            {
                bIsOK = false;
                message += "<p> 順序重複! </p> ";
            }

            //檢查結果回傳
            var result = new
            {
                bIsOK = bIsOK,
                message = message
            };
            return Json(result);
        }

        // 明細檔 刪除資料的檢查
        [HttpPost]
        public JsonResult Chk_Del_D1(FormCollection form)
        {
            bool bIsOK = true;
            string message = "";

            //** 依作業不同有不同的檢查點 向下
            //if (comm.sGetInt32(form["res_qty"]) > 0)
            //{
            //    bIsOK = false;
            //    message += "<div class='text-danger'>";
            //    message += "<li> 已進貨量大於零，不可刪除 </li>";
            //    message += "</div>";
            //}
            //** 依作業不同有不同的檢查點 向上

            var result = new
            {
                bIsOK = bIsOK,
                message = message
            };
            return Json(result);
        }

        // 明細檔 修改資料的檢查
        public CheckDataResult Chk_Upd_D1(FormCollection form)
        {
            CheckDataResult result = new CheckDataResult();
            result.bIsOK = true;
            //** 依作業不同有不同的檢查點 向下

            //檢查scr_no是否有更動，若有做更動則進行格式檢查
            string scr_no_old = comm.Get_QueryData("QMB03_0100", form["qmb03_0100"], "qmb03_0100", "scr_no");
            if (form["scr_no"] != scr_no_old)
            {
                //檢查scr_no是否為四位且皆為數字
                if (!Regex.IsMatch(form["scr_no"], "^.{4}$") || !Regex.IsMatch(form["scr_no"], "^.[0-9]+$"))
                {
                    result.bIsOK = false;
                    result.message += "<p> 順序應為四位數字! </p> ";
                }
                //檢查scr_no有無重複
                if (!comm.Chk_RelData("QMB03_0100", "where qsheet_code = @qsheet_code and scr_no = @scr_no", "qsheet_code,scr_no", form["qsheet_code"] + "," + form["scr_no"]))
                {
                    result.bIsOK = false;
                    result.message += "<p> 順序重複! </p> ";
                }
            }

            //** 依作業不同有不同的檢查點 向上

            return result;
        }

        /* 資料檢查 向上 */


        public string GetQtestItemName(string qtest_item_code)
        {
            string qtest_item_name = comm.Get_QueryData("QMB02_0000", qtest_item_code, "qtest_item_code", "qtest_item_name");
            return (qtest_item_name);
        }

        public string GetQtestItemTypeName(string qtest_item_type)
        {
            string qtest_item_type_name = comm.Get_BDP21_0000("qtest_item_type", qtest_item_type);
            return (qtest_item_type_name);
        }

        public string GetToolName(string tool_code)
        {
            string tool_name = comm.Get_QueryData("QMB16_0000", tool_code, "tool_code", "tool_name");
            return (tool_name);
        }

        public string GetUnitName(string unit_code)
        {
            string unit_name = comm.Get_QueryData("QMB17_0000", unit_code, "unit_code", "unit_name");
            return (unit_name);
        }

        public string GetWorkName(string work_code)
        {
            string work_name = comm.Get_QueryData("MEB30_0000", work_code, "work_code", "work_name");
            return (work_name);
        }
    }
}