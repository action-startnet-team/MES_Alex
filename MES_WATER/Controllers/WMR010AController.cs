using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MES_WATER.Models;
using MES_WATER.Repository;
using Newtonsoft.Json;
using System.Collections;
using System.Data.SqlClient;
using Dapper;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MES_WATER.Controllers
{
    [Authorize] //登入驗證
    [HandleError(View = "Error")]  //錯誤導向

    public class WMR010AController : Controller
    {
        //程式代號
        string sPrgCode = "WMR010A";

        
        //共用函式庫
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        public class WMR010A
        {
            [Display(Name = "料號")]
            public string  pro_code { get; set; }

            [Display(Name = "品名")]
            public string pro_name { get; set; }

            [Display(Name = "批號")]
            public string lot_no { get; set; }
            [Display(Name = "規格")]
            public string pro_sec { get; set; }

            [Display(Name = "數量")]
            public string pro_qty { get; set; }

            [Display(Name = "數量單位")]
            public string pro_unin_qty { get; set; }

            [DisplayName("json")]
            public string json { get; set; }

            [Display(Name = "MAC配對位置")]
            public string usr_code { get; set; }

            [Display(Name = "標籤類別", Prompt = "請輸入條碼")]
            public string label_code { get; set; }

            [Display(Name = "輸入條碼", Prompt = "請輸入條碼")]
            public string label_type { get; set; }
            [Display(Name = "印表機", Prompt = "請輸入印表機")]
            public string print_name { get; set; }
            [Display(Name = "製造日期")]
            public string ins_date { get; set; }
            public string ins_time { get; set; }
            public string prt_cnt { get; set; }
            public string qr_code { get; set; }

        }


        public ActionResult Index()
        {
            return View();
        }

        private bool Chk_Ins_Main(FormCollection form)
        {
            bool bIsOK = true;
            return bIsOK;
        }

        [HttpPost]
        public ActionResult Index(FormCollection form, WMR010A WMR010A)
        {
            // MVC model驗證
            if (ModelState.IsValid)
            {
                // 自訂義 資料驗證
                bool bIsOK = Chk_Ins_Main(form);

                //執行存檔
                WMR010A data = new WMR010A();
                comm.Set_ModelValue(data, form);
                //data.ins_date = comm.Get_Date();
                data.ins_time = comm.Get_Time();
                data.prt_cnt = "1";
                data.pro_code = data.pro_code.ToString();
                data.qr_code = data.pro_code.ToString() + "%" + data.pro_name.ToString() + 
                               "%%%B%" + Guid.NewGuid().ToString("N") + "%" + data.pro_qty.ToString();
                data.json = JsonConvert.SerializeObject(new List<WMR010A>() { data });
                data.json = data.json.Replace(",\"json\":\"\"", "");
                InsertData(data);
                // 新增紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);
                //存完檔回到主頁，如果不跳回主頁要在這裡做修改
                return RedirectToAction("Index");
            }
            ViewBag.showErrMsg = true;
            ViewBag.prg_code = sPrgCode;
            return View(WMR010A);
        }

        [HttpPost]
        public ActionResult Check_Data(FormCollection form, WMR010A model)
        {
            bool isSuccess = false;
            //檢查資料代碼是否重覆
            string key = gmv.GetKey<WMR010A>(new WMR010A());
            string sWhere = "where " + key + "='" + form[key].ToString() + "'";
            bool hasRow = !comm.Chk_RelData("WMR010A", sWhere);
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

        public void InsertData(WMR010A data)
        {
            string sSql = "INSERT INTO " +
                          "   PRT01_0000 (  prt_type,  prt_kind,  print_name,   print_data," +
                          "                 ins_date,  ins_time,  usr_code  )" +
                          "     VALUES( 'A', 'A', @print_name," +
                          "                 @json, @ins_date, @ins_time, @usr_code)";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, data);
            }
        }
    }
}