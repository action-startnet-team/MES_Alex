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

    public class WMB110AController : JsonNetController
    {
        //程式代號
        string sPrgCode = "WMB110A";
        //需要用到的Repo
        WMB11_0000Repository repoWMB11_0000 = new WMB11_0000Repository();
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

            List<WMB11_0000> list = new List<WMB11_0000>();
            list = repoWMB11_0000.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

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
            WMB11_0000 newData = new WMB11_0000();

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

            WMB11_0000 newData = repoWMB11_0000.GetDTO(pTkCode);

            return View(newData);
        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase upload, FormCollection form)
        {
            bool isUpdate = form.AllKeys.Contains("isUpdate") ? true : false;

            DataTable dt = comm.CsvToDataTable(upload);

            int save_count = 0;
            List<WMB11_0000> notSaveList = new List<WMB11_0000>();
            foreach (DataRow dr in dt.Rows)
            {
                WMB11_0000 data = new WMB11_0000();
                data.carrier_code = dr["載具編號"].ToString();
                data.carrier_name = dr["載具名稱"].ToString();
                data.is_use = dr["是否使用"].ToString();
                //data.usr_pass = string.IsNullOrEmpty(dr["密碼"].ToString()) ? "0000" : dr["密碼"].ToString();
                //data.usr_mail = dr["連絡mail"].ToString();
                //data.usr_tel1 = dr["連絡電話一"].ToString();
                //data.usr_tel2 = dr["連絡電話二"].ToString();
                //data.limit_type = dr["權限類別"].ToString();
                //data.grp_code = dr["角色代號"].ToString();
                //data.is_use = dr["是否使用"].ToString();
                //data.dep_code = dr["部門代號"].ToString();
                //data.dut_code = dr["職稱代號"].ToString();


                if (comm.Chk_RelData("WMB11_0000", "carrier_code", data.carrier_code))
                {
                    repoWMB11_0000.InsertData(data);
                    save_count += 1;
                }
                else
                {
                    // 是否覆蓋
                    if (isUpdate)
                    {
                        repoWMB11_0000.UpdateData(data);
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


        /// <summary>
        /// (修改區) 主檔的新增頁面將值存進DB
        /// </summary>
        /// <param name="form">畫面上輸入的form元件中的控制項集合</param>
        /// <param name="model">要存檔的model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Insert(FormCollection form, WMB11_0000 model)
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
                WMB11_0000 data = new WMB11_0000();
                comm.Set_ModelValue(data, form);

                //在取完畫面上的值後，如果有一些別名欄位要變更值，可以在這邊2次加工


                repoWMB11_0000.InsertData(data);

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
        public ActionResult Update(FormCollection form, WMB11_0000 model)
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
                WMB11_0000 data = new WMB11_0000();
                comm.Set_ModelValue(data, form);
                WMB11_0000 sBefore = comm.GetData<WMB11_0000>(data);
                repoWMB11_0000.UpdateData(data);
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
            WMB11_0000 sBefore = comm.GetData<WMB11_0000>(pTkCode);
            repoWMB11_0000.DeleteData(pTkCode);
            //刪除紀錄資料
            comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "delete", sBefore, "");
            return RedirectToAction("Index");
        }
        /* 資料處理 向上 */

        //資料檢查 向下//
        //主檔的檢查
        [HttpPost]
        public ActionResult Check_Data(FormCollection form, WMB11_0000 model)
        {
            bool isSuccess = false;
            //檢查資料代碼是否重覆
            string key = gmv.GetKey<WMB11_0000>(new WMB11_0000());
            string sWhere = "where " + key + "='" + form[key].ToString() + "'";
            bool hasRow = !comm.Chk_RelData("WMB11_0000", sWhere);
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