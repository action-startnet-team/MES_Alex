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

    public class MET040BController : JsonNetController
    {
        //程式代號
         string sPrgCode = "MET040B";
        //需要用到的Repo
        MET04_0100Repository repoMET04_0100 = new MET04_0100Repository();
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

            List<MET04_0100> list = new List<MET04_0100>();
            list = repoMET04_0100.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public DataTable Get_RecData(string pMoCode)
        {
            string sSql = "SELECT a.*,b.pro_name FROM MET04_0100 a" +
                " LEFT JOIN MEB20_0000 b ON a.pro_code = b.pro_code" +
                " WHERE a.mo_code = @mo_code";
            return comm.Get_DataTable(sSql, "mo_code", pMoCode);
        }


        /// <summary>
        /// (修改區) 主檔 新增
        /// 1.新增模式下控項的預設值在這邊設定
        /// </summary>
        /// <returns></returns>
        public ActionResult Insert(string pTkCode)
        {
            //要結合權限控制
            //ViewBag.limit_str = comm.Get_LimitByUsrCode(User.Identity.Name, sPrgCode);
            ViewBag.prg_code = sPrgCode;
            ViewBag.mo_code = pTkCode;
            //取得歷程
            ViewBag.rec_data = Get_RecData(pTkCode);

            //新增模式的預設值
            MET04_0100 newData = new MET04_0100();
            newData.loc_code = "F000";
            newData.lot_no = DateTime.Now.ToString("yyyyMMdd");
            newData.pro_code = comm.Get_QueryData("MET01_0000", pTkCode, "mo_code", "pro_code");
            newData.pro_unit = comm.Get_QueryData("MET01_0000", pTkCode, "mo_code", "pro_unit");

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

            MET04_0100 newData = repoMET04_0100.GetDTO(pTkCode);

            return View(newData);
        }


        /// <summary>
        /// (修改區) 主檔的新增頁面將值存進DB
        /// </summary>
        /// <param name="form">畫面上輸入的form元件中的控制項集合</param>
        /// <param name="model">要存檔的model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Insert(FormCollection form, MET04_0100 model)
        {
            // MVC model驗證
            if (ModelState.IsValid)
            {
                // 自訂義 資料驗證
                bool bIsOK = Chk_Ins_Main(form);
                //取得歷程
                MET04_0100 data = new MET04_0100();
                comm.Set_ModelValue(data, form);
                ViewBag.mo_code = data.mo_code;

                // 資料驗證失敗
                if (!bIsOK)
                {
                    ViewBag.rec_data = Get_RecData(data.mo_code);
                    ViewBag.showErrMsg = true;
                    ViewBag.prg_code = sPrgCode;
                    return View(data);
                }

                //執行存檔
                //在取完畫面上的值後，如果有一些別名欄位要變更值，可以在這邊2次加工
                data.pro_code = comm.Get_QueryData("MET01_0000", data.mo_code, "mo_code", "pro_code");
                data.usr_code = User.Identity.Name;
                data.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
                data.ins_time = DateTime.Now.ToString("HH:mm:ss");
                data.ureport_code = comm.Get_TkCode("MET040B");
                //data.lot_no = comm.Get_LotNo(data.mo_code);

                repoMET04_0100.InsertData(data);
                // 新增DTS01
                comm.Ins_DTS01_0000("ZMES9998", User.Identity.Name, data);
                // 新增紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);
                ViewBag.rec_data = Get_RecData(data.mo_code);

                //存完檔回到主頁，如果不跳回主頁要在這裡做修改
                //return RedirectToAction("Index");
            }
            ViewBag.showErrMsg = false;
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
        public ActionResult Update(FormCollection form, MET04_0100 model)
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
                MET04_0100 data = new MET04_0100();
                comm.Set_ModelValue(data, form);
                MET04_0100 sBefore = comm.GetData<MET04_0100>(data);
                data.usr_code = User.Identity.Name;
                data.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
                data.ins_time = DateTime.Now.ToString("HH:mm:ss");
                repoMET04_0100.UpdateData(data);
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
        //[HttpPost]
        //public ActionResult Delete(string pTkCode)
        //{
        //    //刪除前的檢查要在JqGrid送出前檢查，所以對應Chk_Del_Main這個函數
        //    MET04_0100 sBefore = comm.GetData<MET04_0100>(pTkCode);
        //    // 新增DTS01
        //    comm.Ins_DTS01_0000("ZMES9993", User.Identity.Name, sBefore);

        //    repoMET04_0100.DeleteData(pTkCode);
        //    //刪除紀錄資料
        //    comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "delete", sBefore, "");
        //    return RedirectToAction("Index");
        //}
        
        public ActionResult Delete(string pTkCode)
        {
            //刪除前的檢查要在JqGrid送出前檢查，所以對應Chk_Del_Main這個函數
            MET04_0100 sBefore = comm.GetData<MET04_0100>(pTkCode);
            // 新增DTS01
            comm.Ins_DTS01_0000("ZMES9993", User.Identity.Name, sBefore);

            sBefore.is_del = "P";
            repoMET04_0100.UpdateData(sBefore);
            return RedirectToAction("Insert", sPrgCode, new { pTkCode = sBefore.mo_code });
        }
        /* 資料處理 向上 */

        //資料檢查 向下//
        //主檔的檢查
        [HttpPost]
        public ActionResult Check_Data(FormCollection form, MET04_0100 model)
        {
            bool isSuccess = false;
            //檢查資料代碼是否重覆
            string key = gmv.GetKey<MET04_0100>(new MET04_0100());
            string sWhere = "where " + key + "='" + form[key].ToString() + "'";
            bool hasRow = !comm.Chk_RelData("MET04_0100", sWhere);
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

            //檢查工單是否已確認
            if (comm.Chk_Mo_IsOk("MET04_0100", form["mo_code"]))
            {
                bIsOK = false;
                ModelState.AddModelError("mo_code", "工單已確認");
            }

            double pro_qty = double.Parse(form["pro_qty"]);
            if (pro_qty <= 0)
            {
                bIsOK = false;
                ModelState.AddModelError("pro_qty", "數量需大於0");
            }
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
        /// <param name="pMoCode">客戶編號</param>
        /// <param name="pType">要取回的欄位</param>
        /// <returns></returns>
        public string Get_MoData(string pMoCode, string pType)
        {
            string sReturn = "";

            //請購單與客戶欄位的欄位名稱不同，所以要另外對應
                sReturn = comm.Get_QueryData("MET01_0000", pMoCode, "mo_code", pType);
            return sReturn;
        }
    }
}