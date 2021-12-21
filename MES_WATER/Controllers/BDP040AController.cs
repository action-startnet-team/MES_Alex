//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using MES_WATER.Models;
//using MES_WATER.Repository;
//using System.Data;
//using System.Linq.Dynamic;
//using System.Web.Security;
//using System.Reflection;

//namespace MES_WATER.Controllers
//{
//    [Authorize] //登入驗證
//    [HandleError(View = "Error")]  //錯誤導向

//    public class BDP040AController : Controller
//    {
//        //程式代號
//        public static string pubPrgCode = "BDP040A";
//        BDP04_0000Repository repoBDP04_0000 = new BDP04_0000Repository();
//        Comm comm = new Comm();
//        GetModelValidation GMV = new GetModelValidation();

//        public ActionResult Index()
//        {
//            ViewBag.limit_str = comm.Get_LimitByUsrCode(User.Identity.Name, pubPrgCode);
//            ViewBag.prg_code = pubPrgCode;
//            return View();
//        }

//        public JsonResult GetData_DataTable()
//        {
//            string sPrgCode = pubPrgCode;
//            string sUsrCode = User.Identity.Name;
//            List<BDP04_0000> list = new List<BDP04_0000>();
//            list = repoBDP04_0000.Get_DataList(sUsrCode, sPrgCode);

//            var returnObj = new
//            {
//                data = list
//            };
//            return Json(returnObj, JsonRequestBehavior.AllowGet);
//        }

//        [HttpPost]
//        public ActionResult GetData_Full(int draw, int start, int length, string searchText)
//        {
//            string sPrgCode = pubPrgCode;
//            string sUsrCode = User.Identity.Name;
//            //程式修改處 向下//
//            //要回傳的分頁資料
//            List<BDP04_0000> _myRecords = new List<BDP04_0000>();
//            _myRecords = repoBDP04_0000.Get_DataList(sUsrCode, sPrgCode);

//            //總資料
//            var query = _myRecords.AsEnumerable();

//            //查詢   
//            if (!string.IsNullOrEmpty(searchText))
//            {
//                // 對 class 中的 member 動態產生 query 清單
//                BDP04_0000 obj = new BDP04_0000();
//                List<IEnumerable<BDP04_0000>> arrQuery = new List<IEnumerable<BDP04_0000>>();
//                foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
//                {
//                    arrQuery.Add(_myRecords.Where(m => propertyInfo.GetValue(m).ToString().Contains(searchText)));
//                }
//                for (int i = 0; i < arrQuery.Count(); i++)
//                {
//                    if (Utils.IsAny(arrQuery[i]))
//                    {
//                        query = arrQuery[i];
//                        break;
//                    }
//                }
//            }
//            //程式修改處 向上//


//            int skip = start;//起始資料列索引值(略過幾筆)

//            #region jQuery DataTables的排序資料行
//            //jQuery DataTable的Column index
//            string col_index = Request.Form["order[0][column]"];
//            //col_index 換算成 資料行名稱
//            //排序資料行名稱
//            string sortColName = string.IsNullOrEmpty(col_index) ? "sysid" : Request.Form[$@"columns[{col_index}][data]"];
//            //升冪或降冪
//            string asc_desc = string.IsNullOrEmpty(Request.Form["order[0][dir]"]) ? "desc" : Request.Form["order[0][dir]"];//防呆
//            #endregion

//            //查詢&排序後的總筆數
//            int recordsTotal = 0;

//            //排序
//            query = query.OrderBy($@"{sortColName} {asc_desc}"); //排序使用到System.Linq.Dynamic

//            recordsTotal = query.Count();//查詢後的總筆數

//            if (length == -1)
//            {//抓全部資料
//                _myRecords = query.ToList();
//            }
//            else
//            {//分頁 
//                _myRecords = query.Skip(skip).Take(length)
//                            .ToList();
//            }

//            //回傳Json資料
//            var returnObj =
//              new
//              {
//                  draw = draw,
//                  recordsTotal = recordsTotal,
//                  recordsFiltered = recordsTotal,
//                  data = _myRecords//分頁後的資料 
//              };
//            return Json(returnObj, JsonRequestBehavior.AllowGet);
//        }

//        //  資料處理 向下  //
//        public ActionResult Update(string pTkCode)
//        {
//            BDP04_0000 data = repoBDP04_0000.GetDTO(pTkCode);
//            ViewBag.prg_code = pubPrgCode;
//            return View(data);
//        }

//        [HttpPost]
//        public ActionResult Update(FormCollection form, BDP04_0000 model)
//        {
//            if (ModelState.IsValid)
//            {
//                BDP04_0000 newData = new BDP04_0000();
//                newData.prg_code = comm.Chg_HtmlToDB(form["prg_code"], "textbox");
//                newData.prg_name = comm.Chg_HtmlToDB(form["prg_name"], "textbox");
//                newData.sys_code = comm.Chg_HtmlToDB(form["sys_code"], "textbox");
//                newData.is_use = comm.Chg_HtmlToDB(form["is_use"], "checkbox");
//                newData.limit_str = comm.Chg_HtmlToDB(form["limit_str"], "multiselect");
//                BDP04_0000 sBefore = comm.GetData<BDP04_0000>(newData);
//                repoBDP04_0000.UpdateData(newData);
//                // 紀錄資料
//                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, pubPrgCode, "update", sBefore, newData);
//                return RedirectToAction("Index");
//            }
//            ViewBag.prg_code = pubPrgCode;
//            return View(model);

//        }
//        public ActionResult Insert()
//        {
//            //新增模式傳DTO是為了呈現欄位名稱
//            BDP04_0000 data = new BDP04_0000();
//            ViewBag.prg_code = pubPrgCode;
//            return View(data);
//        }

//        [HttpPost]
//        public ActionResult Insert(FormCollection form, BDP04_0000 model)
//        {
//            if (ModelState.IsValid)
//            {
//                BDP04_0000 newData = new BDP04_0000();
//                newData.prg_code = comm.Chg_HtmlToDB(form["prg_code"], "textbox");
//                newData.prg_name = comm.Chg_HtmlToDB(form["prg_name"], "textbox");
//                newData.sys_code = comm.Chg_HtmlToDB(form["sys_code"], "textbox");
//                newData.is_use = comm.Chg_HtmlToDB(form["is_use"], "checkbox");
//                newData.limit_str = comm.Chg_HtmlToDB(form["limit_str"], "multiselect");
//                repoBDP04_0000.InsertData(newData);
//                // 紀錄資料
//                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, pubPrgCode, "insert", "", newData);
//                return RedirectToAction("Index");
//            }
//            ViewBag.prg_code = pubPrgCode;
//            return View(model);
//        }

//        public ActionResult Delete(string pTkCode)
//        {
//            BDP04_0000 sBefore = comm.GetData<BDP04_0000>(pTkCode);
//            repoBDP04_0000.DeleteData(pTkCode);
//            comm.Ins_BDP20_0000ForMdy(User.Identity.Name, pTkCode, "delete", sBefore, "");
//            return RedirectToAction("Index");
//        }
//        //資料處理 向上//


//        //資料檢查 向下//
//        [HttpPost]
//        public ActionResult Check_Data(FormCollection form, BDP04_0000 model)
//        {
//            bool isSuccess = false;
//            //檢查資料代碼是否重覆
//            string key = GMV.GetKey<BDP04_0000>(new BDP04_0000());
//            string sWhere = "where " + key + "='" + form[key].ToString() + "'";  //keyValue要加引號
//            bool hasRow = !comm.Chk_RelData("BDP04_0000", sWhere);  
//            if (hasRow)
//            {
//                ModelState.AddModelError(key, "代碼已存在!");
//                isSuccess = true;
//            }
//            var returnData = new
//            {
//                // 成功與否
//                IsSuccess = isSuccess,
//                // ModelState錯誤訊息 
//                ModelStateErrors = ModelState.Where(x => x.Value.Errors.Count > 0)
//                    .ToDictionary(k => k.Key, k => k.Value.Errors.Select(e => e.ErrorMessage).ToArray())
//            };
//            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(returnData), "application/json");
//        }
//        //資料檢查 向上//

//    }


//}
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

    public class BDP040AController : JsonNetController
    {
        //程式代號
        string sPrgCode = "BDP040A";
        //需要用到的Repo
        BDP04_0000Repository repoBDP04_0000 = new BDP04_0000Repository();
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

            List<BDP04_0000> list = new List<BDP04_0000>();
            list = repoBDP04_0000.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

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
            BDP04_0000 newData = new BDP04_0000();

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

            BDP04_0000 newData = repoBDP04_0000.GetDTO(pTkCode);

            return View(newData);
        }


        /// <summary>
        /// (修改區) 主檔的新增頁面將值存進DB
        /// </summary>
        /// <param name="form">畫面上輸入的form元件中的控制項集合</param>
        /// <param name="model">要存檔的model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Insert(FormCollection form, BDP04_0000 model)
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
                BDP04_0000 newData = new BDP04_0000();
                newData.prg_code = comm.Chg_HtmlToDB(form["prg_code"], "textbox");
                newData.prg_name = comm.Chg_HtmlToDB(form["prg_name"], "textbox");
                newData.sys_code = comm.Chg_HtmlToDB(form["sys_code"], "textbox");
                newData.is_use = comm.Chg_HtmlToDB(form["is_use"], "checkbox");
                newData.limit_str = comm.Chg_HtmlToDB(form["limit_str"], "multiselect");
                repoBDP04_0000.InsertData(newData);

                // 新增紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", newData);
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
        public ActionResult Update(FormCollection form, BDP04_0000 model)
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
                BDP04_0000 newData = new BDP04_0000();
                newData.prg_code = comm.Chg_HtmlToDB(form["prg_code"], "textbox");
                newData.prg_name = comm.Chg_HtmlToDB(form["prg_name"], "textbox");
                newData.sys_code = comm.Chg_HtmlToDB(form["sys_code"], "textbox");
                newData.is_use = comm.Chg_HtmlToDB(form["is_use"], "checkbox");
                newData.limit_str = comm.Chg_HtmlToDB(form["limit_str"], "multiselect");
                BDP04_0000 sBefore = comm.GetData<BDP04_0000>(newData);
                repoBDP04_0000.UpdateData(newData);
                //更新紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "update", sBefore, newData);
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
            BDP04_0000 sBefore = comm.GetData<BDP04_0000>(pTkCode);
            repoBDP04_0000.DeleteData(pTkCode);
            //刪除紀錄資料
            comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "delete", sBefore, "");
            return RedirectToAction("Index");
        }
        /* 資料處理 向上 */

        //資料檢查 向下//
        //主檔的檢查
        [HttpPost]
        public ActionResult Check_Data(FormCollection form, BDP04_0000 model)
        {
            bool isSuccess = false;
            //檢查資料代碼是否重覆
            string key = gmv.GetKey<BDP04_0000>(new BDP04_0000());
            string sWhere = "where " + key + "='" + form[key].ToString() + "'";
            bool hasRow = !comm.Chk_RelData("BDP04_0000", sWhere);
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