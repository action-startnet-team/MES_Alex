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

    public class BDP210AController : Controller
    {
        //程式代號
        string pubPrgCode = "BDP210A";

        //需要用到的Repo
        BDP21_0000Repository repoBDP21_0000 = new BDP21_0000Repository();
        BDP21_0100Repository repoBDP21_0100 = new BDP21_0100Repository();

        //共用函式庫
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        // 首頁 
        public ActionResult Index()
        {
            ViewBag.sPrgCode = pubPrgCode;
            return View();
        }

        // 主檔 資料處理 向下  //
        //修改頁面
        public ActionResult Update(string pTkCode)
        {
            BDP21_0000 data = repoBDP21_0000.GetDTO(pTkCode);
            ViewBag.limit_str = comm.Get_LimitByUsrCode(User.Identity.Name, pubPrgCode);
            ViewBag.prg_code = pubPrgCode;
            return View(data);
        }

        //修改頁面回傳值後的處理
        [HttpPost]
        public ActionResult Update(FormCollection form, BDP21_0000 model)
        {
            if (ModelState.IsValid)
            {
                //取值並且做html值與DB所需值的轉換
                BDP21_0000 newData = new BDP21_0000();
                newData.code_code = comm.Chg_HtmlToDB(form["code_code"], "textbox");
                newData.code_name = comm.Chg_HtmlToDB(form["code_name"], "textbox");
                newData.cmemo = comm.Chg_HtmlToDB(form["cmemo"], "textbox");
                newData.show_type = comm.Chg_HtmlToDB(form["show_type"], "textbox");


                // 修改前資料
                //string sBefore = comm.Get_DataForBDP20_0000(newData);
                BDP21_0000 sBefore = comm.GetData<BDP21_0000>(newData);

                // sql執行
                repoBDP21_0000.UpdateData(newData);

                // 紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, pubPrgCode, "update", sBefore, newData);

                return RedirectToAction("Index");
            }
            ViewBag.prg_code = pubPrgCode;
            return View(model);
        }

        //新增頁面
        public ActionResult Insert()
        {
            //新增模式傳DTO是為了呈現欄位名稱
            BDP21_0000 data = new BDP21_0000();
            ViewBag.prg_code = pubPrgCode;
            return View(data);
        }

        //新增頁面回傳值後的處理
        [HttpPost]
        public ActionResult Insert(FormCollection form, BDP21_0000 model)
        {
            if (ModelState.IsValid)
            {
                BDP21_0000 newData = new BDP21_0000();
                newData.code_code = comm.Chg_HtmlToDB(form["code_code"], "textbox");
                newData.code_name = comm.Chg_HtmlToDB(form["code_name"], "textbox");
                newData.cmemo = comm.Chg_HtmlToDB(form["cmemo"], "textbox");
                newData.show_type = comm.Chg_HtmlToDB(form["show_type"], "textbox");

                //comm.InsertData("BDP21_0000", newData);
                repoBDP21_0000.InsertData(newData);

                // 紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, pubPrgCode, "insert", "", newData);

                return RedirectToAction("Update", pubPrgCode, new { pTkCode = newData.code_code });
            }
            ViewBag.prg_code = pubPrgCode;
            return View(model);
        }

        //主檔按下刪除後的處理
        [HttpPost]
        public ActionResult Delete(string pTkCode)
        {
            // 刪除前資料
            //var deletedData = repoBDP08_0000.GetDTO(pTkCode);
            BDP21_0000 sBefore = comm.GetData<BDP21_0000>(pTkCode);

            // sql執行
            repoBDP21_0000.DeleteData(pTkCode);

            // 紀錄資料
            comm.Ins_BDP20_0000ForMdy(User.Identity.Name, pubPrgCode, "delete", sBefore, "");

            return RedirectToAction("Index");
        }

        //資料檢查 向下//
        //主檔的檢查
        [HttpPost]
        public ActionResult Check_Data(FormCollection form, BDP21_0000 model)
        {
            bool isSuccess = false;
            //檢查資料代碼是否重覆
            string key = gmv.GetKey<BDP21_0000>(new BDP21_0000());
            string sWhere = "where " + key + "='" + form[key].ToString() + "'";
            bool hasRow = !comm.Chk_RelData("BDP21_0000", sWhere);
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

        //首頁主檔JqGird的資料來源
        public ActionResult Get_GridDataByQuery(string pWhere)
        {
            string sUsrCode = User.Identity.Name;
            string sPrgCode = pubPrgCode;

            List<BDP21_0000> list = new List<BDP21_0000>();
            list = repoBDP21_0000.Get_DataListByQuery(sUsrCode, pubPrgCode, pWhere);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Get_GridData()
        {
            string sUsrCode = User.Identity.Name;
            string sPrgCode = pubPrgCode;

            List<BDP21_0000> list = new List<BDP21_0000>();
            list = repoBDP21_0000.Get_DataList(sUsrCode, sPrgCode);

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        // 主檔 資料處理 向上//

        //  D1 資料處理 向下  //
        //首頁明細檔JqGrid的資料來源
        [HttpPost]
        public ActionResult Get_GridData_D1(string pTkCode)
        {
            string sPrgCode = pubPrgCode;
            string sUsrCode = User.Identity.Name;
            List<BDP21_0100> list = new List<BDP21_0100>();
            list = repoBDP21_0100.Get_DataList(sUsrCode, sPrgCode, pTkCode);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //明細檔 JqGrid的新增回傳
        [HttpPost]
        public ActionResult Insert_D1(FormCollection form)
        {
            BDP21_0100 newData = new BDP21_0100();

            newData.code_code = comm.Chg_HtmlToDB(form["code_code"], "textbox");
            newData.field_code = comm.Chg_HtmlToDB(form["field_code"], "textbox");
            newData.field_name = comm.Chg_HtmlToDB(form["field_name"], "textbox");
            newData.scr_no = Int32.Parse(form["scr_no"]);
            newData.is_use = comm.Chg_HtmlToDB(form["is_use"], "textbox");

            repoBDP21_0100.InsertData(newData);

            // 紀錄資料
            comm.Ins_BDP20_0000ForMdy(User.Identity.Name, pubPrgCode, "insert", "", newData);

            return RedirectToAction("Index");
        }

        //明細檔 JqGrid的更新回傳
        [HttpPost]
        public void Update_D1(FormCollection form)
        {
            //取值並且做html值與DB所需值的轉換
            BDP21_0100 newData = new BDP21_0100();
            newData.bdp21_0100 = Int32.Parse(form["bdp21_0100"]);
            newData.code_code = comm.Chg_HtmlToDB(form["code_code"], "textbox");
            newData.field_code = comm.Chg_HtmlToDB(form["field_code"], "textbox");
            newData.field_name = comm.Chg_HtmlToDB(form["field_name"], "textbox");
            newData.scr_no = Int32.Parse(form["scr_no"]);
            newData.is_use = comm.Chg_HtmlToDB(form["is_use"], "textbox");

            // 修改前資料
            BDP21_0100 sBefore = comm.GetData<BDP21_0100>(newData);

            repoBDP21_0100.UpdateData(newData);

            // 紀錄資料
            comm.Ins_BDP20_0000ForMdy(User.Identity.Name, pubPrgCode, "update", sBefore, newData);

        }

        //明細檔 JqGrid的刪除回傳
        [HttpPost]
        public void Delete_D1(String pTkCode)
        {
            // 
            BDP21_0100 sBefore = comm.GetData<BDP21_0100>(pTkCode);

            repoBDP21_0100.DeleteData(pTkCode);

            // 紀錄資料
            comm.Ins_BDP20_0000ForMdy(User.Identity.Name, pubPrgCode, "delete", sBefore, "");
        }

        //明細檔存檔時候的檢查
        [HttpPost]
        public ActionResult Check_Data_D1(FormCollection form)
        {
            //檢查資料代碼是否重覆
            string code_code = form["code_code"].ToString();
            string field_code = form["field_code"].ToString();
            string sWhere = " where code_code='" + code_code + "' and field_code='" + field_code + "'";
            if (comm.Chk_RelData("BDP21_0100", sWhere))
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
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
        // D1 資料處理 向上//
    }
}