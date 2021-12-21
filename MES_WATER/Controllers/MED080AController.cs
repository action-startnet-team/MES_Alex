using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MES_WATER.Models;
using System.Data;

namespace MES_WATER.Controllers
{
    public class MED080AController : Controller
    {
        Comm comm = new Comm();
        CheckData CD = new CheckData();
        CheckMed CM = new CheckMed();

        // GET: MED080A
        public ActionResult Index()
        {
            CM.Chk_Med01(DateTime.Now.ToString("yyyy/MM/dd"));
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            

            ViewBag.Date = form["ins_date"];
            return View();
        }


        /// <summary>
        /// 計算Table內的資料筆數
        /// </summary>
        /// <param name="pTable">資料表</param>
        /// <param name="pDate">日期</param>
        /// <param name="pIsNg">是否異常</param>
        /// <returns></returns>
        public int Get_DataCount(string pTable, string pDate,string pIsNg = "N") {

            string sSubWhere = " and is_ng <> 'Y'";
            if (pIsNg == "Y") { sSubWhere = " and is_ng = 'Y'"; }

            string sSql = "select * from " + pTable +
                          " where ins_date = @ins_date" +
                          sSubWhere;
            DataTable dtTmp = comm.Get_DataTable(sSql, "ins_date", pDate);            
            return dtTmp.Rows.Count;
        }

        /// <summary>
        /// 檢查日期
        /// </summary>
        /// <param name="pDate"></param>
        /// <returns></returns>
        public bool Chk_IsDate(string pDate) {
            return CD.IsDate(pDate);
        }

    }
}