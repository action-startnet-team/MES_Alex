using MES_WATER.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MES_WATER.Controllers
{
    public class BDP090AController : Controller
    {

        Comm comm = new Comm();
        DynamicTable DT = new DynamicTable();
        GetData GD = new GetData();
        Review RV = new Review();

        // GET: BDP090A
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            object data = new object();
            if (!string.IsNullOrEmpty(form["usr_code"])) {
                string sUsrCode = form["usr_code"];

                string sSql = "delete BDP09_0200 where usr_code = '" + sUsrCode + "'";
                comm.Connect_DB(sSql);

                string sEpbCodeArray = Get_EPBCode();
                for (int i = 0; i < sEpbCodeArray.Split(',').Length; i++) {
                    string sEpbCode = sEpbCodeArray.Split(',')[i];
                    string sIsUse = "N";
                    string sLimitStr = "";
                    if (!string.IsNullOrEmpty(form["is_use_" + sEpbCode])) {
                        sIsUse = "Y";
                    }
                    if (!string.IsNullOrEmpty(form["limit_str_" + sEpbCode])) {
                        sLimitStr = form["limit_str_" + sEpbCode];
                    }

                    data = new
                    {
                        usr_code = sUsrCode,
                        epb_code = sEpbCode,
                        limit_str = sLimitStr,
                        is_use = sIsUse,
                    };
                    DT.InsertData("BDP09_0200", data);
                }
                ViewBag.UsrCode = sUsrCode;
            }
            return View();
        }



        public string Get_User() {
            string sSql = "select * from BDP08_0000";
            return GD.DataFieldToSTA(sSql, "usr_code,usr_name");
        }

        public string Get_EPBCode()
        {
            string sSql = "select * from EPB02_0000";
            return GD.DataFieldToStr(sSql, "epb_code");
        }

        public string Get_UsrLimit(string pUsrCode) {
            string sSql = "select * from BDP09_0200" +
                          " where usr_code = '" + pUsrCode + "'";
            return GD.DataFieldToSTA(sSql, "epb_code,limit_str","/");
        }

        public string Get_UsrIsOk(string pUsrCode)
        {
            string sSql = "select * from BDP09_0200" +
                          " where usr_code = '" + pUsrCode + "'";
            return GD.DataFieldToSTA(sSql, "epb_code,is_use");
        }



    }
}