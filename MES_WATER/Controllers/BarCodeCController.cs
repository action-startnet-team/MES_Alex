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

    public class BarCodeCController : Controller
    {
        //程式代號
        string sPrgCode = "BarCodeC";
        //共用函式庫
        Comm comm = new Comm();

        public ActionResult Index()
        {
            return View();
        }

        public class BarCodeC
        {
            [DisplayName("產品編號")]
            [Required(ErrorMessage = "請輸入{0}")]
            public string pro_code { get; set; }

            [DisplayName("製造批號")]
            public string field01 { get; set; }

            [DisplayName("包裝數量")]
            [Required(ErrorMessage = "請輸入{0}")]
            public string field02 { get; set; }

            [DisplayName("材料批號")]
            public string field03 { get; set; }

            [DisplayName("製造日期")]
            [Required(ErrorMessage = "請輸入{0}")]
            public string field04 { get; set; }

            [DisplayName("json格式")]
            public string json { get; set; }

            [DisplayName("標籤種類")]
            [Required(ErrorMessage = "請輸入{0}")]
            public string label_code { get; set; }

            [DisplayName("標籤名稱")]
            public string label_name { get; set; }

            [DisplayName("列印資料")]
            public string print_data { get; set; }

            [DisplayName("輸入日期")]
            public string ins_date { get; set; }

            [DisplayName("輸入時間")]
            public string ins_time { get; set; }

            [Display(Name = "印表機名稱")]
            [Required(ErrorMessage = "請輸入{0}")]
            public string print_name { get; set; }
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
        public ActionResult Index(FormCollection form, BarCodeC BarCodeC)
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
                BarCodeC data = new BarCodeC();
                comm.Set_ModelValue(data, form);
                string label_code = data.label_code.ToString();
                string qr_code = data.pro_code.ToString()+"%" + data.field01.ToString() +
                                 "%%%B%" + Guid.NewGuid().ToString("N") + "%" + data.field02.ToString() ;   

                data.ins_date = comm.Get_Date();
                data.ins_time = comm.Get_Time();
                QRCodeE iQrCode = new QRCodeE();
                
                iQrCode.pro_code = data.pro_code;
                iQrCode.lot_no = data.field01.ToString();
                iQrCode.pro_qty = data.field02.ToString();
                iQrCode.mfc_date = data.field04.ToString();
                iQrCode.label_code = label_code;
                iQrCode.erp_pro_code = data.pro_code;
                iQrCode.erp_pro_name = Get_ProName(iQrCode.erp_pro_code);
                iQrCode.cus_pro_name = "";
                iQrCode.pro_code = "";
                iQrCode.sup_code = "";
                iQrCode.qr_code = qr_code;
                iQrCode.prt_cnt = "1";
                string print_data = JsonConvert.SerializeObject(new List<QRCodeE>() { iQrCode });
                //ApiResult<string> result = new ApiResult<string>(JsonConvert.SerializeObject(list));
                data.print_data = print_data;


                InsertData(data);
                // 新增紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);
                //存完檔回到主頁，如果不跳回主頁要在這裡做修改
                return RedirectToAction("Index");
            }
            ViewBag.showErrMsg = true;
            ViewBag.prg_code = sPrgCode;
            return View(BarCodeC);
        }

        public void InsertData(BarCodeC data)
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
        public string Get_ProName(string pProCode)
        {
            string sReturn = "";
            string sSql = "select * from MEB20_0000 where pro_code='"+ pProCode + "'";
            DataTable dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                sReturn = comm.sGetString(dtTmp.Rows[0]["pro_name"].ToString());
            }
            return sReturn;
        }
    }
}