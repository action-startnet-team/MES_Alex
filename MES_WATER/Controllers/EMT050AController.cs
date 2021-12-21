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

namespace MES_WATER.Controllers
{
    [Authorize] //登入驗證
    [HandleError(View = "Error")]  //錯誤導向

    public class EMT050AController : JsonNetController
    {
        //程式代號
        string sPrgCode = "EMT050A";
        //需要用到的Repo
        EMT05_0000Repository repoEMT05_0000 = new EMT05_0000Repository();
        //共用函式庫
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();
        DynamicTable DT = new DynamicTable();


        /* 資料處理 向下 */
        /// <summary>
        /// (固定區) 主檔 首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //要結合權限控制
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
            //string sPrgCode = sPrgCode;

            List<EMT05_0000> list = new List<EMT05_0000>();
            list = repoEMT05_0000.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// (修改區) 主檔 新增
        /// 1.新增模式下控項的預設值在這邊設定
        /// </summary>
        /// <returns></returns>
        public ActionResult Insert()
        {
            //要結合權限控制
            ViewBag.prg_code = sPrgCode;

            //新增模式的預設值
            EMT05_0000 newData = new EMT05_0000();
            newData.call_date = DateTime.Now.ToString("yyyy/MM/dd");


            return View(newData);
        }

        /// <summary>
        /// (修改區) 主檔 修改
        /// 1.依資料鍵值到DB取回資料呈現在修改模式頁面
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        /// <returns></returns>
        public ActionResult Update(string pTkCode)
        {
            ViewBag.prg_code = sPrgCode;

            EMT05_0000 newData = repoEMT05_0000.GetDTO(pTkCode);

            return View(newData);
        }


        /// <summary>
        /// (修改區) 主檔的新增頁面將值存進DB
        /// </summary>
        /// <param name="form">畫面上輸入的form元件中的控制項集合</param>
        /// <param name="model">要存檔的model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Insert(FormCollection form, EMT05_0000 model)
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
                    ViewBag.prg_code = sPrgCode;
                    return View(model);
                }

                //執行存檔

                EMT05_0000 data = new EMT05_0000();
                comm.Set_ModelValue(data, form);

                //在取完畫面上的值後，如果有一些別名欄位要變更值，可以在這邊2次加工
                data.call_code = comm.Get_TkCode(sPrgCode);
                data.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
                data.ins_time = DateTime.Now.ToString("HH:mm:ss");
                data.usr_code = User.Identity.Name;

                data.emt01_0100 = "-1";

                DT.InsertData("EMT05_0000", data);
                //repoEMT05_0000.InsertData(data);

                int emt02_0100 = comm.Get_QueryData<int>("EMT02_0100",
                                                        " where dev_check_code = '" + data.dev_check_code + "' and chk_item_code = '" + data.chk_item_code + "'",
                                                        "emt02_0100");
                comm.Upd_QueryData("EMT02_0100", "emt02_0100", emt02_0100.ToString(), "sor_code", data.call_code);
                // 新增紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);
                //存完檔回到主頁，如果不跳回主頁要在這裡做修改
                return RedirectToAction("Index");


            }
            ViewBag.showErrMsg = true;
            ViewBag.prg_code = sPrgCode;
            return View(model);
        }



        /// <summary>
        /// (修改區) 主檔 新增
        /// 1.新增模式下控項的預設值在這邊設定
        /// </summary>
        /// <returns></returns>
        public ActionResult Insert_Maintain()
        {
            //要結合權限控制
            ViewBag.prg_code = sPrgCode;

            //新增模式的預設值
            EMT05_0000 newData = new EMT05_0000();
            newData.call_date = DateTime.Now.ToString("yyyy/MM/dd");


            return View(newData);
        }



        /// <summary>
        /// (修改區) 主檔的新增頁面將值存進DB
        /// </summary>
        /// <param name="form">畫面上輸入的form元件中的控制項集合</param>
        /// <param name="model">要存檔的model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Insert_Maintain(FormCollection form, EMT05_0000 model)
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
                    ViewBag.prg_code = sPrgCode;
                    return View(model);
                }

                //執行存檔

                EMT05_0000 data = new EMT05_0000();
                comm.Set_ModelValue(data, form);

                //在取完畫面上的值後，如果有一些別名欄位要變更值，可以在這邊2次加工
                data.call_code = comm.Get_TkCode(sPrgCode);
                data.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
                data.ins_time = DateTime.Now.ToString("HH:mm:ss");
                data.usr_code = User.Identity.Name;

                //保養項目main_item_code的欄位是放識別碼emt01_0100，所以需要加工
                data.emt01_0100 = data.main_item_code;
                data.main_item_code = comm.Get_Data("EMT01_0100", data.emt01_0100, "emt01_0100", "main_item_code");

                DT.InsertData("EMT05_0000", data);

                comm.Upd_QueryData("EMT01_0100", "emt01_0100", data.emt01_0100, "sor_code", data.call_code);
                // 新增紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);
                //存完檔回到主頁，如果不跳回主頁要在這裡做修改
                return RedirectToAction("Index");


            }
            ViewBag.showErrMsg = true;
            ViewBag.prg_code = sPrgCode;
            return View(model);
        }


        /// <summary>
        /// (修改區) 主檔的修改頁面將值存進DB
        /// </summary>
        /// <param name="form">畫面上輸入的form元件中的控制項集合</param>
        /// <param name="model">要存檔的model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(FormCollection form, EMT05_0000 model)
        {
            // MVC model驗證 資料格式檢查
            if (ModelState.IsValid)
            {
                // 自定義 資料邏輯檢查
                bool bIsOK = Chk_Upd_Main(form);

                // 資料驗證失敗
                if (!bIsOK)
                {
                    ViewBag.showErrMsg = true;
                    ViewBag.prg_code = sPrgCode;
                    return View(model);
                }

                //執行存檔
                EMT05_0000 data = new EMT05_0000();
                //先取得預設值
                data = repoEMT05_0000.GetDTO(comm.sGetString(form["call_code"]));

                DT.Set_ModelValue(data, form, false);             
                data.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
                data.ins_time = DateTime.Now.ToString("HH:mm:ss");
                data.usr_code = User.Identity.Name;
                EMT05_0000 sBefore = comm.GetData<EMT05_0000>(data);

                DT.UpdateData("EMT05_0000", "call_code", data);
                //repoEMT05_0000.UpdateData(data);

                //更新紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "update", sBefore, data);

                return RedirectToAction("Index");

            }

            ViewBag.showErrMsg = true;
            ViewBag.prg_code = sPrgCode;
            return View(model);
        }

        /// <summary>
        /// (修改區) 按下刪除後刪除DB動作
        /// </summary>
        /// <param name="pTkCode">要刪除的鍵值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(string pTkCode)
        {
            //刪除前的檢查要在JqGrid送出前檢查，所以對應Chk_Del_Main這個函數
            EMT05_0000 sBefore = comm.GetData<EMT05_0000>(pTkCode);

            //保養與叫修table也要清除sor_code
            comm.Upd_Data("EMT01_0100", "sor_code", pTkCode, "sor_code", "");
            comm.Upd_Data("EMT02_0100", "sor_code", pTkCode, "sor_code", "");

            repoEMT05_0000.DeleteData(pTkCode);
            //刪除紀錄資料
            comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "delete", sBefore, "");
            return RedirectToAction("Index");
        }


        public JsonResult Get_DevChkCode(string dev_code)
        {
            string sSql = " select dev_check_code, dev_check_date " +
                          " from EMT02_0000 " +
                          " where dev_code = @dev_code " +
                          " and dev_check_code in " +
                          " (select dev_check_code from EMT02_0100 where is_ok = 'X' and sor_code = '') ";
            DataTable dtTmp = comm.Get_DataTable(sSql, "dev_code", dev_code);
            return Json(dtTmp, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get_ChkItemCode(string dev_check_code)
        {
            string sSql = " select EMT02_0100.chk_item_code, EMB21_0000.chk_item_name " +
                          " from EMT02_0100 " +
                          " left join EMB21_0000 on EMB21_0000.chk_item_code = EMT02_0100.chk_item_code " +
                          " where dev_check_code = @dev_check_code " +
                          " and is_ok = 'X' and sor_code = '' ";
            DataTable dtTmp = comm.Get_DataTable(sSql, "dev_check_code", dev_check_code);
            return Json(dtTmp, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get_MaintainCode(string pDevCode)
        {
            string sSql = "select maintain_code, maintain_name " +
                          "  from EMT01_0000 " +
                          " where dev_code = @dev_code " +
                          "   and maintain_code in (select maintain_code from EMT01_0100 where is_ok = 'X' and sor_code = '')";
            DataTable dtTmp = comm.Get_DataTable(sSql, "dev_code", pDevCode);
            return Json(dtTmp, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get_MainItemCode(string pMaintainCode)
        {
            string sSql = "select EMT01_0100.emt01_0100, EMT01_0100.main_item_code,main_item_name,EMT01_0100.ins_date  " +
                          "  from EMT01_0100 " +
                          "  left join EMB08_0000 on EMT01_0100.main_item_code = EMB08_0000.main_item_code " +
                          " where EMT01_0100.maintain_code = @maintain_code " +
                          "   and is_ok = 'X'  " +
                          "   and sor_code = ''";
            DataTable dtTmp = comm.Get_DataTable(sSql, "maintain_code", pMaintainCode);
            return Json(dtTmp, JsonRequestBehavior.AllowGet);
        }


        /* 資料處理 向上 */

        //資料檢查 向下//
        //主檔的檢查
        [HttpPost]
        public ActionResult Check_Data(FormCollection form, EMT05_0000 model)
        {
            bool isSuccess = false;
            //檢查資料代碼是否重覆
            string key = gmv.GetKey<EMT05_0000>(new EMT05_0000());
            string sWhere = "where " + key + "='" + form[key].ToString() + "'";
            bool hasRow = !comm.Chk_RelData("EMT05_0000", sWhere);
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

    }
}