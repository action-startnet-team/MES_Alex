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
using MES_WATER.Controllers;

namespace MES_WATER.Controllers
{
    [Authorize] //登入驗證
    [HandleError(View = "Error")]  //錯誤導向

    public class ECB040AController : JsonNetController
    {
        //程式代號
        public static string sPrgCode = "ECB040A";

        //需要用到的Repo
        ECB04_0000Repository repoECB04_0000 = new ECB04_0000Repository();
        ECB04_0100Repository repoECB04_0100 = new ECB04_0100Repository();
        GooFlowRepository repoFlow = new GooFlowRepository();

        //共用函式庫
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();
        DynamicTable DT = new DynamicTable();
        CheckData CD = new CheckData();


        /* 資料處理 向下 */
        /// <summary>
        /// (固定區) 主檔 首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //要結合權限控制
            ViewBag.limit_str = comm.Get_LimitByUsrCode(User.Identity.Name, sPrgCode);
            //comm.ConvertNull("ECB04_0000");

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

            List<ECB04_0000> list = new List<ECB04_0000>();
            list = repoECB04_0000.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // 明細檔 jqgrid資料來源
        [HttpPost]
        public ActionResult Get_GridData_D1(string pTkCode)
        {
            //string sPrgCode = pubPrgCode;
            string sUsrCode = User.Identity.Name;
            List<ECB04_0100> list = new List<ECB04_0100>();
            list = repoECB04_0100.Get_DataList(sUsrCode, sPrgCode, pTkCode);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// (修改區) 主檔 新增
        /// 1.新增模式下控項的預設值在這邊設定
        /// </summary>
        /// <returns></returns>
        public ActionResult Insert()
        {
            //新增模式的預設值
            ECB04_0000 data = new ECB04_0000();

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
            ECB04_0000 newData = repoECB04_0000.GetDTO(pTkCode);
            ViewBag.pTkCode = pTkCode;
            return View(newData);
        }

        /// <summary>
        /// (修改區) 主檔的新增頁面將值存進DB
        /// </summary>
        /// <param name="form">畫面上輸入的form元件中的控制項集合</param>
        /// <param name="model">要存檔的model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Insert(FormCollection form, ECB04_0000 model)
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
                ECB04_0000 data = new ECB04_0000();

                // 預設賦值
                comm.Set_ModelValue(data, form);

                data.SALES_CUSTOMER_CODE_EDITION = Guid.NewGuid().ToString();
                //data.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
                //data.ins_time = DateTime.Now.ToString("HH:mm:ss");
                //data.usr_code = User.Identity.Name;

                repoECB04_0000.InsertData(data);

                // 新增紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);

                return RedirectToAction("Update", sPrgCode, new { pTkCode = data.SALES_CUSTOMER_CODE_EDITION });
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
            ECB04_0100 data = new ECB04_0100();

            comm.Set_ModelValue(data, form);
            data.ecb04_0100 = Guid.NewGuid().ToString();
            repoECB04_0100.InsertData(data);

            // 新增紀錄資料
            comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);

        }
        /// <summary>
        /// (修改區) 主檔 修改
        /// 1.依資料鍵值到DB取回資料呈現在修改模式頁面
        /// </summary>
        /// <param name = "pTkCode" > 資料鍵值 </ param >
        /// < returns ></ returns >
        [HttpPost]
        public ActionResult Update(FormCollection form, ECB04_0000 model)
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

                ECB04_0000 data = new ECB04_0000();

                comm.Set_ModelValue(data, form);

                ECB04_0000 sBefore = comm.GetData<ECB04_0000>(data);
                repoECB04_0000.UpdateData(data);
                //更新紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "update", sBefore, data);

                return RedirectToAction("Index");
            }
            ViewBag.showErrMsg = true;
            return View(model);
        }

        ///// <summary>
        ///// (修改區) 明細jqGrid的修改處理，將值存進DB
        ///// </summary>
        ///// <param name="form">畫面上輸入的form元件中的控制項集合</param>
        ///// <param name="model">要存檔的model</param>
        ///// <returns></returns>
        [HttpPost]
        public JsonResult Update_D1(FormCollection form)
        {
            CheckDataResult result = Chk_Upd_D1(form);
            if (result.bIsOK)
            {
                ECB04_0100 data = new ECB04_0100();

                comm.Set_ModelValue(data, form);

                //Int32 iSorSerial = comm.sGetInt32(comm.Get_Data("ECB04_0100", comm.sGetString(form["stt02_0100"]), "stt02_0100", "sor_serial"));
                //data.sor_serial = iSorSerial;

                ECB04_0100 sBefore = comm.GetData<ECB04_0100>(data);
                repoECB04_0100.UpdateData(data);
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
            ECB04_0000 sBefore = comm.GetData<ECB04_0000>(pTkCode);
            repoECB04_0000.DeleteData(pTkCode);
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
            ECB04_0100 sBefore = comm.GetData<ECB04_0100>(pTkCode);
            repoECB04_0100.DeleteData(pTkCode);
            //刪除紀錄資料
            comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "delete", sBefore, "");
        }

        /* 資料處理 向上 */

        //資料檢查 向下//
        //主檔的檢查
        //[HttpPost]
        //public ActionResult Check_Data(FormCollection form, ECB04_0000 model)
        //{
        //    bool isSuccess = false;
        //    //檢查資料代碼是否重覆
        //    string key = gmv.GetKey<ECB04_0000>(new ECB04_0000());
        //    string sWhere = "where " + key + "='" + form[key].ToString() + "'";
        //    bool hasRow = !comm.Chk_RelData("ECB04_0000", sWhere);
        //    if (hasRow)
        //    {
        //        ModelState.AddModelError(key, "代碼已存在!");
        //        isSuccess = true;
        //    }
        //    var returnData = new
        //    {
        //        // 成功與否
        //        IsSuccess = isSuccess,
        //        // ModelState錯誤訊息 
        //        ModelStateErrors = ModelState.Where(x => x.Value.Errors.Count > 0)
        //            .ToDictionary(k => k.Key, k => k.Value.Errors.Select(e => e.ErrorMessage).ToArray())
        //    };
        //    return Content(Newtonsoft.Json.JsonConvert.SerializeObject(returnData), "application/json");
        //}

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

            //重複值檢查
            if (!comm.Chk_RelData("ECB04_0100", "where sales_customer_code_edition = @sales_customer_code_edition and serial_num = @serial_num", "sales_customer_code_edition,serial_num", form["sales_customer_code_edition"] + "," + form["serial_num"]))
            {
                bIsOK = false;
                message += "<p> 目標欄位順序重複! </p> ";
            }
            if (!comm.Chk_RelData("ECB04_0100", "where sales_customer_code_edition = @sales_customer_code_edition and erp_field_code = @erp_field_code", "sales_customer_code_edition,erp_field_code", form["sales_customer_code_edition"] + "," + form["erp_field_code"]))
            {
                bIsOK = false;
                message += "<p> ERP欄位代碼重複! </p> ";
            }
            if (!comm.Chk_RelData("ECB04_0100", "where sales_customer_code_edition = @sales_customer_code_edition and EXCEL_CODE = @EXCEL_CODE", "sales_customer_code_edition,EXCEL_CODE", form["sales_customer_code_edition"] + "," + form["EXCEL_CODE"]))
            {
                bIsOK = false;
                message += "<p> 來源欄位代碼重複! </p> ";
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

            if (!comm.Chk_RelData("ECB04_0100", "where sales_customer_code_edition = @sales_customer_code_edition and serial_num = @serial_num", "sales_customer_code_edition,serial_num", form["sales_customer_code_edition"] + "," + form["serial_num"]))
            {
                result.bIsOK = false;
                result.message += "<p> 目標欄位順序重複! </p> ";
            }

            //if (!comm.Chk_RelData("ECB04_0100", "where sales_customer_code_edition = @sales_customer_code_edition and serial_num = @serial_num and erp_field_code = @erp_field_code and EXCEL_CODE = @EXCEL_CODE", "sales_customer_code_edition,serial_num,erp_field_code,EXCEL_CODE", form["sales_customer_code_edition"] + "," + form["serial_num"] + "," + form["erp_field_code"] + "," + form["EXCEL_CODE"]))
            //{
            //    result.bIsOK = false;
            //    result.message += "<p> 目標欄位順序重複! </p> ";
            //}

            //if (!comm.Chk_RelData("ECB04_0100", "where ecb04_0100 = @ecb04_0100 and erp_field_code = @erp_field_code", "ecb04_0100,erp_field_code", form["ecb04_0100"] + "," + form["erp_field_code"]))
            //{
            //    result.bIsOK = false;
            //    result.message += "<p> ERP欄位代碼重複! </p> ";
            //}
            //if (!comm.Chk_RelData("ECB04_0100", "where ecb04_0100 = @ecb04_0100 and EXCEL_CODE = @EXCEL_CODE", "ecb04_0100,EXCEL_CODE", form["ecb04_0100"] + "," + form["EXCEL_CODE"]))
            //{
            //    result.bIsOK = false;
            //    result.message += "<p> 來源欄位代碼重複! </p> ";
            //}
            //** 依作業不同有不同的檢查點 向下
            //if (comm.sGetInt32(form["pro_price"]) == 0)
            //{
            //    result.bIsOK = false;
            //    result.message += "<li> 單價等於零，不可修改 </li>";
            //}

            //** 依作業不同有不同的檢查點 向上

            return result;
        }
    }
}