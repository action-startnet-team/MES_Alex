using System;
using Dapper;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using MES_WATER.Models;
using MES_WATER.Repository;
using Newtonsoft.Json;
using System.Linq.Dynamic;
using System.Web.Security;
using System.Reflection;
using System.Data.SqlClient;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MES_WATER.Controllers
{
    [Authorize] //登入驗證
    [HandleError(View = "Error")]  //錯誤導向

    public class WMR020AController : JsonNetController
    {
        //程式代號
        string sPrgCode = "WMR020A";
        //需要用到的Repo
        WMB02_0000Repository repoWMB02_0000 = new WMB02_0000Repository();
        //共用函式庫
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();
        public class WMR020A
        {
            [DisplayName("json")]
            public string json { get; set; }

            [Display(Name = "配對碼")]
            public string usr_code { get; set; }

            [Display(Name = "標籤類別", Prompt = "請輸入條碼")]
            public string label_code { get; set; }

            [Display(Name = "印表機", Prompt = "請輸入印表機")]
            public string print_name { get; set; }
            public string ins_date { get; set; }
            public string ins_time { get; set; }
            [Display(Name = "列印張數", Prompt = "請輸入印表機")]
            public string prt_cnt { get; set; }
            public string qr_code { get; set; }
            
        }
        public ActionResult Index()
        {
            return View();
        }
        public void InsertData(WMR020A data)
        {
            string sSql = "INSERT INTO " +
                          "   PRT01_0000 (  prt_type,  prt_kind,  print_name,   print_data," +
                          "                 ins_date,  ins_time,  usr_code , label_code  )" +
                          "     VALUES( 'A', 'B', @print_name," +
                          "                 @json, @ins_date, @ins_time, @usr_code, @label_code)";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, data);
            }
        }
        /* 資料處理 向下 */
        /// <summary>
        /// (固定區) 主檔 首頁
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public ActionResult Index(FormCollection form, WMR020A WMR020A)
        {

            // MVC model驗證
            if (ModelState.IsValid)
            {
                // 自訂義 資料驗證
                bool bIsOK = Chk_Ins_Main(form);

                //執行存檔
                WMR020A data = new WMR020A();
                comm.Set_ModelValue(data, form);
                data.ins_date = comm.Get_Date();
                data.ins_time = comm.Get_Time();
                //data.json = JsonConvert.SerializeObject(new List<WMR020A>() { data });
                //data.json = data.json.Replace(",\"json\":\"\"", "");
                for (int i = 0; i < int.Parse(data.prt_cnt); i++)
                {
                    InsertData(data);
                    // 新增紀錄資料
                    comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);
                }

                //存完檔回到主頁，如果不跳回主頁要在這裡做修改
                return RedirectToAction("Index");
            }
            ViewBag.showErrMsg = true;
            ViewBag.prg_code = sPrgCode;
            return View(WMR020A);
        }
        private bool Chk_Ins_Main(FormCollection form)
        {
            bool bIsOK = true;
            return bIsOK;
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

            List<WMB02_0000> list = new List<WMB02_0000>();
            list = repoWMB02_0000.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}