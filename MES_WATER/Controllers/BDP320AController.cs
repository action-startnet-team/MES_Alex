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

    public class BDP320AController : JsonNetController
    {
        //程式代號
        string sPrgCode = "BDP320A";
        //需要用到的Repo
        BDP32_0000Repository repoBDP32_0000 = new BDP32_0000Repository();
        //共用函式庫
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();


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

            List<BDP32_0000> list = new List<BDP32_0000>();
            list = repoBDP32_0000.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

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
            //ViewBag.limit_str = comm.Get_LimitByUsrCode(User.Identity.Name, sPrgCode);
            ViewBag.prg_code = sPrgCode;

            //新增模式的預設值
            BDP32_0000 newData = new BDP32_0000();
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
            //ViewBag.limit_str = comm.Get_LimitByUsrCode(User.Identity.Name, sPrgCode);
            ViewBag.prg_code = sPrgCode;

            BDP32_0000 newData = repoBDP32_0000.GetDTO(pTkCode);

            return View(newData);
        }


        /// <summary>
        /// (修改區) 主檔的新增頁面將值存進DB
        /// </summary>
        /// <param name="form">畫面上輸入的form元件中的控制項集合</param>
        /// <param name="model">要存檔的model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Insert(FormCollection form, BDP32_0000 model)
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
                BDP32_0000 data = new BDP32_0000();
                comm.Set_ModelValue(data, form);

                //在取完畫面上的值後，如果有一些別名欄位要變更值，可以在這邊2次加工
                //data.usr_code = User.Identity.Name;
                //data.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
                //data.ins_time = DateTime.Now.ToString("HH:mm:ss");

                repoBDP32_0000.InsertData(data);
                // 紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);
                //存完檔回到主頁，如果不跳回主頁要在這裡做修改
                return RedirectToAction("Index");

                //BDP32_0000 newData = new BDP32_0000();
                //newData.inq_code = comm.Chg_HtmlToDB(form["inq_code"], "textbox");
                //newData.inq_date = comm.Chg_HtmlToDB(form["inq_date"], "textbox");
                //newData.pro_code = comm.Chg_HtmlToDB(form["pro_code"], "textbox");
                //newData.pro_name = comm.Chg_HtmlToDB(form["pro_name"], "textbox");
                //newData.pro_spc = comm.Chg_HtmlToDB(form["pro_spc"], "textbox");
                //newData.sup_code = comm.Chg_HtmlToDB(form["sup_code"], "textbox");
                //newData.can_date = comm.Chg_HtmlToDB(form["can_date"], "textbox");
                //newData.stv_code = comm.Chg_HtmlToDB(form["stv_code"], "textbox");
                //newData.exg_rate = comm.sGetDecimal(form["exg_rate"]);
                //newData.pro_qty = comm.sGetDecimal(form["pro_qty"]);
                ////------
                //newData.pro_price = comm.sGetDecimal(form["pro_price"]);
                //newData.cmemo = comm.Chg_HtmlToDB(form["cmemo"], "textbox");
                //newData.inq_per = comm.Chg_HtmlToDB(form["inq_per"], "textbox");
                //newData.sto_state = comm.Chg_HtmlToDB(form["sto_state"], "textbox");
                //newData.is_onsale = comm.Chg_HtmlToDB(form["is_onsale"], "textbox");
                //newData.usr_code = comm.Chg_HtmlToDB(form["usr_code"], "textbox");
                //newData.ins_date = comm.Chg_HtmlToDB(form["ins_date"], "textbox");
                //newData.ins_time = comm.Chg_HtmlToDB(form["ins_time"], "textbox");
                //newData.sor_type = comm.Chg_HtmlToDB(form["sor_type"], "textbox");
                //newData.per_code = comm.Chg_HtmlToDB(form["per_code"], "textbox");
                ////------
                //newData.sup_name = comm.Chg_HtmlToDB(form["sup_name"], "textbox");
                //newData.typ1_code = comm.Chg_HtmlToDB(form["typ1_code"], "textbox");

                //newData.usr_code = User.Identity.Name;
                //newData.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
                //newData.ins_time = DateTime.Now.ToString("HH:mm:ss");
                //repoBDP32_0000.InsertData(newData);
                //ViewBag.prg_code = sPrgCode;

                //return RedirectToAction("Index");
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
        public ActionResult Update(FormCollection form, BDP32_0000 model)
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
                BDP32_0000 data = new BDP32_0000();
                comm.Set_ModelValue(data, form);
                BDP32_0000 sBefore = comm.GetData<BDP32_0000>(data);
                repoBDP32_0000.UpdateData(data);
                // 紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "update", sBefore, data);

                return RedirectToAction("Index");


                ////取值並且做html值與DB所需值的轉換
                //BDP32_0000 newData = new BDP32_0000();

                //newData.inq_code = comm.Chg_HtmlToDB(form["inq_code"], "textbox");
                //newData.inq_date = comm.Chg_HtmlToDB(form["inq_date"], "textbox");

                //newData.pro_code = comm.Chg_HtmlToDB(form["pro_code"], "textbox");
                ////newData.pro_name = comm.Chg_HtmlToDB(form["pro_name"], "textbox");
                //newData.pro_name = comm.Get_QueryData("STB01_0000", newData.pro_code, "pro_code", "pro_name");

                //newData.pro_spc = comm.Chg_HtmlToDB(form["pro_spc"], "textbox");
                //newData.sup_code = comm.Chg_HtmlToDB(form["sup_code"], "textbox");
                ////newData.sup_name = comm.Chg_HtmlToDB(form["sup_name"], "textbox");
                //newData.sup_name = comm.Get_QueryData("STB10_0000", newData.sup_code, "sup_code", "sup_name");

                //newData.can_date = comm.Chg_HtmlToDB(form["can_date"], "textbox");
                //newData.stv_code = comm.Chg_HtmlToDB(form["stv_code"], "textbox");
                //newData.exg_rate = comm.sGetDecimal(form["exg_rate"]);
                //newData.pro_qty = comm.sGetDecimal(form["pro_qty"]);
                ////------
                //newData.pro_price = comm.sGetDecimal(form["pro_price"]);
                //newData.cmemo = comm.Chg_HtmlToDB(form["cmemo"], "textbox");
                //newData.inq_per = comm.Chg_HtmlToDB(form["inq_per"], "textbox");

                //newData.sto_state = comm.Chg_HtmlToDB(form["sto_state"], "textbox");
                //newData.is_onsale = comm.Chg_HtmlToDB(form["is_onsale"], "textbox");

                //newData.usr_code = comm.Chg_HtmlToDB(form["usr_code"], "textbox");
                //newData.ins_date = comm.Chg_HtmlToDB(form["ins_date"], "textbox");
                //newData.ins_time = comm.Chg_HtmlToDB(form["ins_time"], "textbox");
                //newData.sor_type = comm.Chg_HtmlToDB(form["sor_type"], "textbox");

                //newData.per_code = comm.Chg_HtmlToDB(form["per_code"], "textbox");
                ////------
                //newData.typ1_code = comm.Chg_HtmlToDB(form["typ1_code"], "textbox");
                ////newData.pro_unit = comm.Chg_HtmlToDB(form["pro_unit"], "textbox");
                ////newData.spro_code = comm.Chg_HtmlToDB(form["spro_code"], "textbox");
                //////newData.package_qty = comm.sGetDecimal(form["package_qty"]);
                ////newData.package_type = comm.Chg_HtmlToDB(form["package_type"], "textbox");
                //////newData.min_pur_qty = comm.sGetDecimal(form["min_pur_qty"]);
                ////newData.sup_tel1 = comm.Chg_HtmlToDB(form["sup_tel1"], "textbox");
                ////newData.pre_date = comm.Chg_HtmlToDB(form["pre_date"], "textbox");
                ////newData.trans_mode = comm.Chg_HtmlToDB(form["trans_mode"], "textbox");
                ////newData.trans_term = comm.Chg_HtmlToDB(form["trans_term"], "textbox");
                ////newData.make_place = comm.Chg_HtmlToDB(form["make_place"], "textbox");
                //repoBDP32_0000.UpdateData(newData);
                //return RedirectToAction("Index");
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
            BDP32_0000 sBefore = comm.GetData<BDP32_0000>(pTkCode);
            repoBDP32_0000.DeleteData(pTkCode);
            comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "delete", sBefore, "");
            return RedirectToAction("Index");
        }
        /* 資料處理 向上 */

        //資料檢查 向下//
        //主檔的檢查
        [HttpPost]
        public ActionResult Check_Data(FormCollection form, BDP32_0000 model)
        {
            bool isSuccess = false;
            //檢查資料代碼是否重覆
            string key = gmv.GetKey<BDP32_0000>(new BDP32_0000());
            string sWhere = "where " + key + "='" + form[key].ToString() + "'";
            bool hasRow = !comm.Chk_RelData("BDP32_0000", sWhere);
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
        //public string Get_ProData(string pProCode, string pType)
        //{
        //    string sReturn = "";
        //    sReturn = comm.Get_QueryData("STB01_0000", pProCode, "pro_code", pType);
        //    return sReturn;
        //}

        /// <summary>
        /// 取得廠商的聯絡人
        /// </summary>
        /// <param name="sup_code"></param>
        /// <returns></returns>
        //public JsonResult Get_SupAtn(string sup_code)
        //{
        //    string sSql = "select STB10_0100.* from STB10_0100 where sup_code = @sup_code ";
        //    DataTable dtTmp = comm.Get_DataTable(sSql, "sup_code", sup_code);

        //    return Json(dtTmp, JsonRequestBehavior.AllowGet);
        //}
    }
}