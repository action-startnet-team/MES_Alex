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

    public class WMR040AController : JsonNetController
    {
        //程式代號
        public static string sPrgCode = "WMR040A";

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

        public ActionResult test()
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


        /* 資料檢查 向上 */


        /// <summary>
        /// 取得倉庫的儲位
        /// </summary>
        /// <param name="sup_code"></param>
        /// <returns></returns>
        public JsonResult Get_LocName(string sto_code)
        {
            string sSql = "select WMB02_0000.* from WMB02_0000 where sto_code = @sto_code ";
            DataTable dtTmp = comm.Get_DataTable(sSql, "sto_code", sto_code);

            return Json(dtTmp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void Update(FormCollection form)
        {
            // 自訂義 資料驗證
            bool bIsOK = Chk_Upd_Main(form);

            // 資料驗證失敗
            if (!bIsOK)
            {
                return;
            }

            WMT0100 data = new WMT0100();
            comm.Set_ModelValue(data, form);
            data.upd_date = DateTime.Now.ToString("yyyy/MM/dd");

            WMT0100 sBefore = comm.GetData<WMT0100>(data);
            repoWMT0100.UpdateData(data);

            //更新紀錄資料
            comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "update", sBefore, data);
        }

        [HttpPost]
        public void UpdateAll(string pData)
        {
            // 自訂義 資料驗證
            //bool bIsOK = Chk_Upd_Main(form);

            //// 資料驗證失敗
            //if (!bIsOK)
            //{
            //    return;
            //}

            List<WMT0100> datas = JsonConvert.DeserializeObject<List<WMT0100>>(pData);

            foreach(WMT0100 data in datas)
            {
                data.upd_date = DateTime.Now.ToString("yyyy/MM/dd");

                WMT0100 sBefore = comm.GetData<WMT0100>(data);
                repoWMT0100.UpdateData(data);

                //更新紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "update", sBefore, data);
            }
            
        }


    }
}