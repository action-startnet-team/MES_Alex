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
using System.Globalization;

namespace MES_WATER.Controllers
{
    [Authorize] //登入驗證
    [HandleError(View = "Error")]  //錯誤導向
    
    public class WMT010AController : JsonNetController
    {
        //程式代號
        public static string sPrgCode = "WMT010A";

        //需要用到的Repo
        WMT0100Repository repoWMT0100 = new WMT0100Repository();
        WMT0200Repository repoWMT0200 = new WMT0200Repository();

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

            List<WMT0100> list = new List<WMT0100>();
            list = repoWMT0100.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // 明細檔 jqgrid資料來源
        [HttpPost]
        public ActionResult Get_GridData_D1(string pTkCode)
        {
            //string sPrgCode = pubPrgCode;
            string sUsrCode = User.Identity.Name;
            List<WMT0200> list = new List<WMT0200>();
            list = repoWMT0200.Get_DataList(sUsrCode, sPrgCode, pTkCode);
            //list = repoWMT0200.Get_DataListByQuery(sUsrCode, sPrgCode, pTkCode);

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
            WMT0100 data = new WMT0100();
            //Sam
            //data.sto_date = Convert.ToString(DateTime.Now);
            data.sto_date = DateTime.Now.ToString("yyyy/MM/dd");
            //data.sto_time = DateTime.Now.ToString("HH:mm:ss");
            data.pro_qty = 1;

            data.is_error = "Y";
            data.ins_user = User.Identity.Name;


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

            WMT0100 newData = repoWMT0100.GetDTO(pTkCode);

            return View(newData);
        }

        /// <summary>
        /// (修改區) 主檔的新增頁面將值存進DB
        /// </summary>
        /// <param name="form">畫面上輸入的form元件中的控制項集合</param>
        /// <param name="model">要存檔的model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Insert(FormCollection form, WMT0100 model)
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
                WMT0100 data = new WMT0100();

                // 預設賦值
                comm.Set_ModelValue(data, form);

                // 特別取值
                data.ins_user = User.Identity.Name;
                //Sam
                //data.ins_date = Convert.ToString(DateTime.Now);
                //data.upd_date = Convert.ToString(DateTime.Now);
                //data.sto_date += (" " + data.sto_time);
                data.ins_date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                data.upd_date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                int data_id = repoWMT0100.InsertData(data);
                // 新增紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);

                return RedirectToAction("Update", sPrgCode, new { pTkCode = data_id });
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
            WMT0200 data = new WMT0200();

            comm.Set_ModelValue(data, form);

            repoWMT0200.InsertData(data);
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
        public ActionResult Update(FormCollection form, WMT0100 model)
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

                WMT0100 data = new WMT0100();
                comm.Set_ModelValue(data, form);
                data.upd_date = DateTime.Now.ToString("yyyy/MM/dd");

                WMT0100 sBefore = comm.GetData<WMT0100>(data);
                repoWMT0100.UpdateData(data);
                //更新紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "update", sBefore, data);


                return RedirectToAction("Index");
            }
            ViewBag.showErrMsg = true;
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
            //刪主檔
            WMT0100 sBefore = comm.GetData<WMT0100>(pTkCode);
            repoWMT0100.DeleteData(pTkCode);
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
            WMT0200 sBefore = comm.GetData<WMT0200>(pTkCode);
            repoWMT0200.DeleteData(pTkCode);
            //刪除紀錄資料
            comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "delete", sBefore, "");

        }


        /* 資料處理 向上 */


        /* 資料檢查 向下 */
        // 主檔 新增資料的檢查
        private bool Chk_Ins_Main(FormCollection form)
        {
            bool bIsOK = true;

            //** 依作業不同有不同的檢查點 向下

            if (form["pro_qty"] == "0")
            {
                bIsOK = false;
                ModelState.AddModelError("pro_qty", "數量不可為0");
            }

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

            if (form["pro_qty"] == "0")
            {
                bIsOK = false;
                ModelState.AddModelError("pro_qty", "數量不可為0");
            }

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
            /*if (comm.sGetInt32(form["pur_amt"]) > 0)
            {
                bIsOK = false;
                message += "<div class='text-danger'>";
                message += "<li> 金額大於零，不可刪除 </li>";
                message += "</div>";
            }*/
            //** 依作業不同有不同的檢查點 向上

            var result = new
            {
                bIsOK = bIsOK,
                message = message
            };

            return Json(result);
        }


        /* 資料檢查 向上 */


        /// <summary>
        /// 取得倉庫的儲位
        /// </summary>
        /// <param name="sup_code"></param>
        /// <returns></returns>
        public JsonResult Get_LocName(string sto_code)
        {
            string sSql = " select WMB02_0000.* from WMB02_0000 where sto_code = @sto_code ";
            DataTable dtTmp = comm.Get_DataTable(sSql, "sto_code", sto_code);

            return Json(dtTmp, JsonRequestBehavior.AllowGet);
        }
    }
}