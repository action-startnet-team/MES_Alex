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

    public class BarCodeBController : Controller
    {
        //程式代號
        string sPrgCode = "BarCodeB";

        //共用函式庫
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        public ActionResult Index()
        {
            return View();
        }

        public class BarCodeB
        {
            [Key]
            [DisplayName("客戶料號-版次")]
            [Required(ErrorMessage = "請輸入{0}")]
            public string fieldA { get; set; }

            [DisplayName("製造批號")]
            public string fieldB { get; set; }

            [DisplayName("包裝數量")]
            [Required(ErrorMessage = "請輸入{0}")]
            public string fieldC { get; set; }

            [DisplayName("材料批號")]
            public string fieldD { get; set; }

            [DisplayName("製造日期")]
            [Required(ErrorMessage = "請輸入{0}")]
            public string fieldE { get; set; }

            [DisplayName("標籤種類")]
            [Required(ErrorMessage = "請輸入{0}")]
            public string label_code { get; set; }

            [DisplayName("印表機名稱")]
            [Required(ErrorMessage = "請輸入{0}")]
            public string print_name { get; set; }

            [DisplayName("json")]
            public string json { get; set; }

            public string qr_code { get; set; }
            public string prt_cnt { get; set; }

            [DisplayName("輸入日期")]
            public string ins_date { get; set; }

            [DisplayName("輸入時間")]
            public string ins_time { get; set; }

            public string pro_code { get; set; }
            public string usr_code { get; set; }

        }

        [HttpPost]
        public ActionResult Index(FormCollection form, BarCodeB BarCodeB)
        {
            // MVC model驗證
            if (ModelState.IsValid)
            {
                // 自訂義 資料驗證
                bool bIsOK = Chk_Ins_Main(form);

                // 資料驗證失敗
                //if (!bIsOK)
                //{
                //    ViewBag.showErrMsg = true;
                //    ViewBag.prg_code = sPrgCode;
                //    return View(model);
                //}

                //執行存檔
                BarCodeB data = new BarCodeB();
                comm.Set_ModelValue(data, form);

                //在取完畫面上的值後，如果有一些別名欄位要變更值，可以在這邊2次加工
                //data.cus_name = comm.Get_QueryData("STB08_0000", data.cus_code, "cus_code", "cus_name");
                //data.is_dps = "N";
                //data.usr_code = User.Identity.Name;
                //data.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
                //data.ins_time = DateTime.Now.ToString("HH:mm:ss");

                data.ins_date = comm.Get_Date();
                data.ins_time = comm.Get_Time();
                string qr_code = data.fieldA.ToString() + "%" + data.fieldB.ToString() +
                       "%%%B%" + Guid.NewGuid().ToString("N") + "%" + data.fieldC.ToString() ;
                data.qr_code = qr_code;
                data.pro_code = data.fieldA.ToString();
                data.prt_cnt = "1";
                data.json = JsonConvert.SerializeObject(new List<BarCodeB>() { data });
                data.json = data.json.Replace(",\"json\":\"\"", "");
                InsertData(data);
                // 新增紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);
                //存完檔回到主頁，如果不跳回主頁要在這裡做修改
                return RedirectToAction("Index");
            }
            ViewBag.showErrMsg = true;
            ViewBag.prg_code = sPrgCode;
            return View(BarCodeB);
        }

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
            //if (comm.Chk_Mo_IsOk("MET04_0100", form["mo_code"]))
            //{
            //    bIsOK = false;
            //    ModelState.AddModelError("mo_code", "工單已確認");
            //}

            //double pro_qty = double.Parse(form["pro_qty"]);
            //if (pro_qty <= 0)
            //{
            //    bIsOK = false;
            //    ModelState.AddModelError("pro_qty", "數量需大於0");
            //}
            //** 依作業不同有不同的檢查點 向上

            //檢查結果回傳
            return bIsOK;
        }

        [HttpPost]
        public ActionResult Check_Data(FormCollection form, BarCodeB model)
        {
            bool isSuccess = false;
            //檢查資料代碼是否重覆
            string key = gmv.GetKey<BarCodeB>(new BarCodeB());
            string sWhere = "where " + key + "='" + form[key].ToString() + "'";
            bool hasRow = !comm.Chk_RelData("BarCodeB", sWhere);
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

        public void InsertData(BarCodeB data)
        {
            //string sSql = "INSERT INTO " +
            //              "   BarCodeB (  field01,  field02,  field03,  field04,  field05,json ) " +
            //              "     VALUES ( @fieldA, @fieldB, @fieldC, @fieldD, @fieldE,@json ) ";
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