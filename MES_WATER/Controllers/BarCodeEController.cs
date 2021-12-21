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

    public class BarCodeEController : Controller
    {
        //程式代號
        string sPrgCode = "BarCodeE";
        //共用函式庫
        Comm comm = new Comm();

        public ActionResult Index()
        {
            return View();
        }

        public class BarCodeE
        {
            [DisplayName("料號")]
            public string field00 { get; set; }

            [DisplayName("ERP 料號")]
            [Required(ErrorMessage = "請輸入{0}")]
            public string field01 { get; set; }

            [DisplayName("製造批號")]
            public string field02 { get; set; }

            [DisplayName("包裝數量")]
            [Required(ErrorMessage = "請輸入{0}")]
            public string field03 { get; set; }

            [DisplayName("製造日期")]
            [Required(ErrorMessage = "請輸入{0}")]
            public string field04 { get; set; }

            [Display(Name = "印表機名稱")]
            [Required(ErrorMessage = "請輸入{0}")]
            public string print_name { get; set; }
            [DisplayName("標籤種類")]
            [Required(ErrorMessage = "請輸入{0}")]
            //[StringLength(10, ErrorMessage = "長度最多{1}個字!")]
            public string label_code { get; set; }

            [DisplayName("json格式")]
            public string json { get; set; }

            [DisplayName("輸入日期")]
            public string ins_date { get; set; }

            [DisplayName("輸入時間")]
            public string ins_time { get; set; }

            [DisplayName("列印資料")]
            public string print_data { get; set; }


            [DisplayName("標籤名稱")]
            public string label_name { get; set; }


            public string usr_code { get; set; }
        }

        public class QRCodeE
        {
            public string pro_code { get; set; }
            public string lot_no { get; set; }
            public string pro_qty { get; set; }
            public string mfc_date { get; set; }
            public string label_code { get; set; }
            public string erp_pro_code { get; set; }
            public string erp_pro_name { get; set; }
            public string cus_pro_name { get; set; }
            public string sup_code { get; set; }
            public string qr_code { get; set; }
            public string prt_cnt { get; set; }

            public string usr_code { get; set; }
        }

        [HttpPost]
        public ActionResult Index(FormCollection form, BarCodeE BarCodeE)
        {




            // MVC model驗證
            if (ModelState.IsValid)
            {
                // 自訂義 資料驗證
                //bool bIsOK = Chk_Ins_Main(form);

                // 資料驗證失敗
                //if (!bIsOK)
                //{
                //    ViewBag.showErrMsg = true;
                //    ViewBag.prg_code = sPrgCode;
                //    return View(model);
                //}

                //執行存檔
                BarCodeE data = new BarCodeE();

                comm.Set_ModelValue(data, form);
                data.ins_date = comm.Get_Date();
                data.ins_time = comm.Get_Time();
                string label_code = data.label_code.ToString();
                string qr_code = data.field01.ToString() + "%" + data.field02.ToString() +
                                 "%%%B%" + Guid.NewGuid().ToString("N") + "%" + data.field03.ToString();
                //string print_data = @"[{""pro_code"":""" + data.field01.ToString() +
                //                       @""",""lot_no"":""" + data.field02.ToString() +
                //                       @""",""pro_qty"":""" + data.field03.ToString() +
                //                       @""",""mfc_date"":""" + data.field04.ToString() +
                //                       @""",""label_code"":""" + label_code +
                //                       @""",""erp_pro_name"":""" +
                //                       @""",""cus_pro_name"":""" + 
                //                       @""",""sup_code"":"""",""qr_code"":""" + qr_code +
                //                       @""",""prt_cnt"":""1""}]";


                QRCodeE iQrCode = new QRCodeE();
                iQrCode.pro_code = data.field00.ToString();
                iQrCode.erp_pro_code = data.field01.ToString();
                iQrCode.lot_no = data.field02.ToString();
                iQrCode.pro_qty = data.field03.ToString();
                iQrCode.mfc_date = data.field04.ToString();
                iQrCode.label_code = label_code;

                iQrCode.erp_pro_name = comm.Get_QueryData("MEB20_0000", iQrCode.erp_pro_code, "pro_code", "pro_name");
                iQrCode.cus_pro_name = "";
                iQrCode.sup_code = "";
                iQrCode.qr_code = qr_code;
                iQrCode.prt_cnt = "1";
                string print_data = JsonConvert.SerializeObject(new List<QRCodeE>() { iQrCode }); 
                //ApiResult<string> result = new ApiResult<string>(JsonConvert.SerializeObject(list));
                data.print_data = print_data;

                InsertData(data);
                // 新增紀錄資料
                // comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);
                //存完檔回到主頁，如果不跳回主頁要在這裡做修改
                return RedirectToAction("Index");
            }
            ViewBag.showErrMsg = true;
            ViewBag.prg_code = sPrgCode;
            return View(BarCodeE);
        }

        public void InsertData(BarCodeE data)
        {
            string sSql = "INSERT INTO " +
                          "   PRT01_0000 (  prt_type,  prt_kind,  print_name,   print_data," +
                          "                 ins_date,  ins_time,  usr_code  )" +
                          "     VALUES( 'A', 'A', @print_name," +
                          "                 @print_data, @ins_date, @ins_time, @usr_code)";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, data);
            }
        }
    }
}