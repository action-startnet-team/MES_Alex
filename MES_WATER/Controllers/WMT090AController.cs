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

    public class WMT090AController : JsonNetController
    {
        //程式代號
        string sPrgCode = "WMT090A";
        //需要用到的Repo
        //WMB03_0000Repository repoWMB03_0000 = new WMB03_0000Repository();
        //WMT08_0100Repository repoWMT08_0100 = new WMT08_0100Repository();
        WMT09_0100Repository repoWMT09_0100 = new WMT09_0100Repository();
        //共用函式庫
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();
        public class WMT090A
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
        public class WMT09_0100
        {
            [Key]
            [DisplayName("識別碼")]
            public int wmt08_0100 { get; set; }

            [DisplayName("盤點單號")]
            public string erp_inventory_code { get; set; }

            [DisplayName("順序")]
            public string scr_no { get; set; }

            [DisplayName("料號")]
            public string pro_code { get; set; }

            [DisplayName("料名")]
            public string pro_name { get; set; }

            [DisplayName("批號")]
            public string lot_no { get; set; }

            [DisplayName("儲位編號")]
            public string loc_code { get; set; }

            [DisplayName("儲位名稱")]
            public string loc_name { get; set; }

            [DisplayName("QR條碼")]
            public string barcode { get; set; }

            [DisplayName("實盤量")]
            public decimal pro_qty { get; set; }

            [DisplayName("erp帳冊量")]
            public decimal sto_qty { get; set; }

            [DisplayName("單位編號")]
            public string unit_code { get; set; }

            [DisplayName("單位名稱")]
            public string unit_name { get; set; }
        }

        public ActionResult Index()
        {
            return View();
        }

        public void InsertData(WMT090A data)
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
        public ActionResult Index(FormCollection form, WMT090A WMT090A)
        {
            // MVC model驗證
            if (ModelState.IsValid)
            {
                // 自訂義 資料驗證
                bool bIsOK = Chk_Ins_Main(form);

                //執行存檔
                WMT090A data = new WMT090A();
                comm.Set_ModelValue(data, form);
                data.ins_date = comm.Get_Date();
                data.ins_time = comm.Get_Time();
                // 新增紀錄資料
                for (int i = 0; i < int.Parse(data.prt_cnt); i++)
                {
                    InsertData(data);
                    comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);
                }
                //存完檔回到主頁，如果不跳回主頁要在這裡做修改
                return RedirectToAction("Index");
            }
            ViewBag.showErrMsg = true;
            ViewBag.prg_code = sPrgCode;
            return View(WMT090A);
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
        public ActionResult Get_GridDataByQuery(string pWhere, string pInventoryCode, string pWmsCode)
        {
            string sUsrCode = User.Identity.Name;
            //string sPrgCode = sPrgCode;
            WMT09_0100 WMT09_0100 = new WMT09_0100();
            List<WMT09_0100> list = new List<WMT09_0100>();
            list = repoWMT09_0100.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

            //foreach (WMT08_0100 wMT in list)
            //{
            //    wMT.print_qty = wMT.pro_qty;
            //}
            //list.Add(new WMT08_0100());
            string sSql = "";
            DataTable dtTmp = new DataTable();

            if (!string.IsNullOrEmpty(pInventoryCode))
            {
                list = list.Where(x => x.erp_inventory_code == pInventoryCode).ToList();
            }
            if (!string.IsNullOrEmpty(pWmsCode) && !string.IsNullOrEmpty(pInventoryCode))
            {
                string sWmsCode = pWmsCode;
                string sErpCode = pInventoryCode;
                //檢查ERP單號是否結案
                if (chk_ERPDataIsOk(pInventoryCode))
                {
                    //查詢ERP盤點單相關物料                                        
                    sSql = "select pro_code ,sum(pro_qty) as pro_qty,lot_no from WMT08_0100" +
                           " where inventory_code = '" + sWmsCode + "'" +
                           " group by pro_code,lot_no";
                    dtTmp = comm.Get_DataTable(sSql);

                    for (int u = 0; u < list.Count(); u++)
                    {
                        if (list[u].lot_no == "")
                        {
                            for (int i = 0; i < dtTmp.Rows.Count; i++)
                            {
                                DataRow r = dtTmp.Rows[i];
                                string sProCode = r["pro_code"].ToString();
                                string sLotNo = r["lot_no"].ToString();
                                decimal dProQty = comm.sGetDecimal(r["pro_qty"].ToString());

                                if (list[u].pro_code == sProCode)
                                {
                                    list[u].pro_qty += dProQty;
                                }

                            }
                        }
                        else
                        {
                            for (int i = 0; i < dtTmp.Rows.Count; i++)
                            {
                                DataRow r = dtTmp.Rows[i];
                                string sProCode = r["pro_code"].ToString();
                                string sLotNo = r["lot_no"].ToString();
                                decimal dProQty = comm.sGetDecimal(r["pro_qty"].ToString());

                                if (list[u].pro_code == sProCode && list[u].lot_no == sLotNo)
                                {
                                    list[u].pro_qty = dProQty;
                                }
                            }
                        }

                    }


                };
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 查詢ERP單號是否為Y，若是則回傳false,若否則回傳true
        /// </summary>
        /// <param name="pInventoryCode"></param>
        /// <returns></returns>
        public bool chk_ERPDataIsOk(string pInventoryCode)
        {
            bool IsOk = true;
            //ERP單號是否結案
            string sYorN = comm.Get_Data("WMT09_0000", pInventoryCode, "erp_inventory_code", "is_ok");
            //已結案
            if (sYorN == "Y")
            {
                IsOk = false;
            }
            return IsOk;
        }
        /// <summary>
        /// 新增DTS佇列至資料庫
        /// </summary>
        /// <param name="pInventoryCode"></param>
        /// <param name="pWmsCode"></param>
        /// <returns></returns>
        public JsonResult InsDts(string pInventoryCode, string pWmsCode)
        {
            string sWmsSql = "";
            string sInventorySql = "";
            if (pInventoryCode != "" && pWmsCode != "")
            {
                ///更新WMT09_0100實盤量，並回寫SAP
                sWmsSql = "select pro_code ,sum(pro_qty) as pro_qty,lot_no from WMT08_0100" +
                               " where inventory_code = '" + pWmsCode + "'" +
                               " group by pro_code,lot_no";
                DataTable dtWmsTmp = comm.Get_DataTable(sWmsSql);
                sInventorySql = "select * from WMT09_0100 where erp_inventory_code='" + pInventoryCode + "'";
                DataTable dtErpTmp = comm.Get_DataTable(sInventorySql);
                WMT09_0100Repository repoWMT0901 = new WMT09_0100Repository();
                for (int u = 0; u < dtErpTmp.Rows.Count; u++)
                {
                    decimal tmpProQty = 0;
                    DataRow drErp = dtErpTmp.Rows[u];
                    if (drErp["lot_no"].ToString() == "")
                    {
                        for (int i = 0; i < dtWmsTmp.Rows.Count; i++)
                        {
                            DataRow r = dtWmsTmp.Rows[i];
                            string sProCode = r["pro_code"].ToString();
                            string sLotNo = r["lot_no"].ToString();
                            decimal dProQty = comm.sGetDecimal(r["pro_qty"].ToString());

                            if (drErp["pro_code"].ToString() == sProCode)
                            {
                                tmpProQty += dProQty;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < dtWmsTmp.Rows.Count; i++)
                        {
                            DataRow r = dtWmsTmp.Rows[i];
                            string sProCode = r["pro_code"].ToString();
                            string sLotNo = r["lot_no"].ToString();
                            decimal dProQty = comm.sGetDecimal(r["pro_qty"].ToString());

                            if (drErp["pro_code"].ToString() == sProCode && drErp["lot_no"].ToString() == sLotNo)
                            {
                                tmpProQty = dProQty;
                            }
                        }
                    }
                    repoWMT0901.UpdProQty(pInventoryCode, drErp["scr_no"].ToString(), tmpProQty.ToString());
                }
                comm.Ins_DTS01_0000(pInventoryCode, pWmsCode);
            }
            else if (pInventoryCode == "")
            {
                return Json("請先選擇ERP單號");
            }
            else if (pWmsCode == "")
            {
                return Json("請先選擇WMS單號");
            }
            return Json("回傳成功，請重新整理畫面");
        }
    }
}