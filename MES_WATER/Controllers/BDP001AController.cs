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


namespace MES_WATER.Controllers
{
    public class BDP001AController : Controller
    {
        viewBDP001ARepository repoviewBDP001A = new viewBDP001ARepository();
        BDP09_0000Repository repoBDP09_0000 = new BDP09_0000Repository();
        BDP09_0100Repository repoBDP09_0100 = new BDP09_0100Repository();
        Comm comm = new Comm();

        // GET: BDP001A
        public ActionResult Index()
        {
            List<viewBDP001A> bdp001a_list = new List<viewBDP001A>();
            bdp001a_list = repoviewBDP001A.Get_DataList("", "");
            ViewBag.bdp001a_list = bdp001a_list;
            ViewBag.prg_code = "BDP001A";
            ViewBag.LimitType = "";
            ViewBag.GrpCode = "";
            ViewBag.UsrCode = "";
            ViewBag.Searched = "false";

            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            ViewBag.prg_code = "BDP001A";
            //畫面的查詢條件
            string sLimitType = form["limit_type"];

            //取資料回傳
            List<viewBDP001A> bdp001a_list = new List<viewBDP001A>();

            //有寫入動作
            if (form["submit"] == "save")
            {
                if (form["limit_type"] != "" & form["limit_type"] != null)
                    if (form["usr_code"] != "" & form["usr_code"] != null || form["grp_code"] != "" & form["grp_code"] != null)
                    {
                        string sSql = "";

                        if (form["limit_type"] == "B")
                        { sSql = "delete BDP09_0000 where usr_code = '" + form["usr_code"] + "'"; }
                        else
                        { sSql = "delete BDP09_0100 where grp_code = '" + form["grp_code"] + "'"; }
                        comm.Connect_DB(sSql);

                        //所有作業
                        sSql = "select * from BDP04_0000 where is_use = 'Y' order by prg_code ";
                        var dtTmp = comm.Get_DataTable(sSql);

                        //權限設定
                        sSql = "select * from BDP21_0100 where code_code = 'limit_str' and is_use = 'Y'";
                        var dtTmp2 = comm.Get_DataTable(sSql);

                        for (int i = 0; i < dtTmp.Rows.Count; i++)
                        {
                            //權限字串
                            string sLimitStr = "";
                            for (int u = 0; u < dtTmp2.Rows.Count; u++)
                            {
                                if (form["checkbox-" + dtTmp.Rows[i]["prg_code"] + "-" + dtTmp2.Rows[u]["field_code"]] != null)
                                {
                                    sLimitStr += dtTmp2.Rows[u]["field_code"];
                                }
                            }

                            //是否使用
                            string sIsUse = "Y";
                            if (form["checkbox-" + dtTmp.Rows[i]["prg_code"] + "-is_use"] != null)
                            { sIsUse = "Y"; }
                            else
                            { sIsUse = "N"; }

                            if (form["limit_type"] == "B")
                            {
                                BDP09_0000 newData = new BDP09_0000();

                                newData.usr_code = form["usr_code"];
                                newData.prg_code = dtTmp.Rows[i]["prg_code"].ToString();
                                newData.limit_str = sLimitStr;
                                newData.is_use = sIsUse;

                                repoBDP09_0000.InsertData(newData);
                            }
                            else
                            {
                                BDP09_0100 newData = new BDP09_0100();

                                newData.grp_code = form["grp_code"];
                                newData.prg_code = dtTmp.Rows[i]["prg_code"].ToString();
                                newData.limit_str = sLimitStr;
                                newData.is_use = sIsUse;

                                repoBDP09_0100.InsertData(newData);
                            }
                        }
                    }
            }

            //查詢動作 A 角色別 B.使用者別
            switch (form["limit_type"])
            {
                case "A":
                    bdp001a_list = repoviewBDP001A.Get_DataList(sLimitType, form["grp_code"]);
                    break;
                case "B":
                    bdp001a_list = repoviewBDP001A.Get_DataList(sLimitType, form["usr_code"]);
                    break;
                default:
                    break;
            }

            ViewBag.bdp001a_list = bdp001a_list;
            ViewBag.LimitType = form["limit_type"];
            ViewBag.GrpCode = form["grp_code"];
            ViewBag.UsrCode = form["usr_code"];
            ViewBag.Searched = "true";
            return View();
        }


    }
}