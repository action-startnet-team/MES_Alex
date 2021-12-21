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

    public class view_MED090
    {
        [Key]
        [DisplayName("報工入庫識別碼")]
        public string med09_0000 { get; set; }
        [DisplayName("製令號碼")]
        public string mo_code { get; set; }

        [DisplayName("物料號碼")]
        public string pro_code { get; set; }

        [DisplayName("物料名稱")]
        public string pro_name { get; set; }
        [DisplayName("批號")]
        public string lot_no { get; set; }

        [DisplayName("數量")]
        public decimal pro_qty { get; set; }

        //[DisplayName("異動類別")]
        //public string ins_type { get; set; }

        //[DisplayName("異動類別名稱")]
        //public string ins_type_name { get; set; }


        [DisplayName("異動日期")]
        public string ins_date { get; set; }
    }
    public class view_MED090AController : JsonNetController
    {
        //程式代號
        string sPrgCode = "view_MED090A";

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
            ViewBag.PrgCode = pPrgCode;
            ViewBag.oModelType = new view_MED090().GetType();
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
            string sSql;
            List<view_MED090> list = new List<view_MED090>();
            sSql = @" 
                    SELECT MED09_0000.med09_0000,MED09_0000.mo_code,MEB15_0000.mac_code,MEB20_0000.pro_code, MED09_0000.pro_qty as pro_qty,
                    MEB20_0000.pro_name as pro_name, BDP08_0000.usr_name as usr_name,MED09_0000.ins_date,MED09_0000.pro_lot_no as lot_no
                    FROM MED09_0000 
                    left join MEB15_0000 on MEB15_0000.mac_code = MED09_0000.mac_code 
                    left join MEB20_0000 on MEB20_0000.pro_code = MED09_0000.pro_code 
                    left join BDP08_0000 on BDP08_0000.usr_code = MED09_0000.usr_code ";
            list = comm.Get_ListByQuery<view_MED090>(sSql, pWhere, sUsrCode, sPrgCode);
            return Json(list, JsonRequestBehavior.AllowGet);


        }


    }
}