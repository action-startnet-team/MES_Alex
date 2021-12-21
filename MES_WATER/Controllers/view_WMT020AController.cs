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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MES_WATER.Controllers
{
    [Authorize] //登入驗證
    [HandleError(View = "Error")]  //錯誤導向
    public class view_WMT0200 {
        [DisplayName("識別碼")]
        public string wmt0200 { get; set; }
        [DisplayName("單別")]
        [StringLength(4, ErrorMessage = "長度最多{1}個字!")]
        public string rel_type { get; set; }

        [DisplayName("單號")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string rel_code { get; set; }

        [DisplayName("序號")]
        public int scr_no { get; set; }

        [DisplayName("異動類別")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string ins_type { get; set; }

        [DisplayName("異動類別名稱")]
        public string ins_type_name { get; set; }

        [DisplayName("物料編號")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string pro_code { get; set; }

        [DisplayName("物料名稱")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string pro_name { get; set; }

        [DisplayName("批號")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string lot_no { get; set; }

        [DisplayName("數量")]
        public decimal pro_qty { get; set; }


        [DisplayName("異動日期")]
        public string ins_date { get; set; }
    }
    public class view_WMT020AController : JsonNetController
    {
        //程式代號
        string sPrgCode = "view_WMT020A";
            
        //共用函式庫
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /* 資料處理 向下 */
        /// <summary>
        /// (固定區) 主檔 首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string pPrgCode = "")
        {
            ViewBag.PrgCode = "QMT040A";
            ViewBag.oModelType = new view_WMT0200().GetType();
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
            string sSql;
            List<view_WMT0200> list = new List<view_WMT0200>();

            sSql = " SELECT WMT0200.*, BDP21_0100.field_name as ins_type_name, WMB01_0000.sto_name, WMB02_0000.loc_name, WMB03_0000.pallet_name as container_name, WMB10_0000.sup_name,WMT0200.scr_no  " +
                          " FROM WMT0200 " +
                          " left join BDP21_0100 on BDP21_0100.field_code = WMT0200.ins_type and BDP21_0100.code_code = 'ins_type' " +
                          " left join WMB01_0000 on WMB01_0000.sto_code = WMT0200.sto_code " +
                          " left join WMB02_0000 on WMB02_0000.loc_code = WMT0200.loc_code " +
                          " left join WMB03_0000 on WMB03_0000.pallet_code = WMT0200.container " +
                          " left join WMB10_0000 on WMB10_0000.sup_code = WMT0200.sup_code " +
                          "  WHERE ins_type='I' ";
            list = comm.Get_ListByQuery<view_WMT0200>(sSql, pWhere, sUsrCode, sPrgCode);
            return Json(list, JsonRequestBehavior.AllowGet);


        }


    }
}