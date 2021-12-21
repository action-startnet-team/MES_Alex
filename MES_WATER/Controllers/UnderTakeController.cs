//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using MES_WATER.Models;
//using MES_WATER.Repository;
//using System.Data;
//using System.Linq.Dynamic;
//using System.Web.Security;
//using System.Reflection;
//using Newtonsoft.Json;

//namespace MES_WATER.Controllers
//{
//    public class UnderTakeController : Controller
//    {
//        UnderTake UT = new UnderTake();
//        //共用函式庫
//        Comm comm = new Comm();
//        //取得單號
//        WebReference.WmsApi ws = new WebReference.WmsApi();
//        GetModelValidation gmv = new GetModelValidation();

//        // GET: Undertake


//        //請購單------------------------------------------------------------------------------------------------------------------------------
//        public ActionResult STT01(string pLink, string pTkCode)
//        {
//            ViewBag.SorCode = "";
//            ViewBag.QuerySDate = "2004/01/01";
//            ViewBag.QueryEDate = DateTime.Now.ToString("yyyy/MM/dd");
//            ViewBag.TkCode = pTkCode;
//            ViewBag.Link = pLink;
//            return View();
//        }

//        [HttpPost]
//        public ActionResult STT01(FormCollection form)
//        {
//            switch (form["submit"])
//            {
//                case "search":
//                    //查詢
//                    ViewBag.SorCode = Get_STT01_SorCode(form["sor_code"], form["per_code"], form["s_date"], form["e_date"]);
//                    ViewBag.QuerySorCode = form["sor_code"];
//                    ViewBag.QueryPerCode = form["per_code"];
//                    //ViewBag.QuerySupCode = form["sup_code"];
//                    //ViewBag.QueryKInvNo = form["k_inv_no"];
//                    ViewBag.QuerySDate = form["s_date"];
//                    ViewBag.QueryEDate = form["e_date"];
//                    ViewBag.TkCode = form["ut_code"];
//                    ViewBag.Link = form["link"];
//                    break;
//                case "Retrieve":
//                    //取回      
//                    string sUTTable = UT.Chk_LinkTable(form["link"]); //被承接資料表
//                    string sUTKey = UT.Chk_LinkKey(form["link"]); //被承接鍵值欄位

//                    string sSorTable = "STT01_0100";
//                    string sSorKey = "stt01_0100"; //承接鍵值欄位
//                    string sSorField = "por_code"; //單號欄位

//                    if (form["checkbox"] != "" && form["checkbox"] != null)
//                    {
//                        for (int i = 0; i < form["checkbox"].Split(',').Length; i++)
//                        {
//                            string sSorCode = form["checkbox"].Split(',')[i]; //承接單號
//                            string sUTCode = comm.Chg_HtmlToDB(form["ut_code"], "textbox"); //被承接單號

//                            if (comm.sGetDecimal(form["sq2_" + sSorCode + ""]) > 0)
//                            {
//                                switch (form["link"])
//                                {                                    
//                                    case "STT02_A":
//                                        STT02_0100 newData = new STT02_0100();

//                                        newData.stt02_0100 = comm.sGetInt32(ws.AutoInt2(sUTTable).ToString());
//                                        newData.pur_code = sUTCode;
//                                        newData.scr_no = UT.Get_NextScrNo(sUTTable, "pur_code", sUTCode);
//                                        newData.pro_code = UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, "pro_code");
//                                        newData.pro_qty = UT.Chk_SorQty(comm.sGetDecimal(UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, "sor_qty2")), comm.sGetDecimal(form["sq2_" + sSorCode + ""]));
//                                        newData.res_qty = 0;
//                                        newData.pro_price = comm.sGetDecimal(UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, "sal_price"));
//                                        newData.rea_price = comm.sGetDecimal(UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, "sal_price"));
//                                        newData.hop_date = "";
//                                        newData.res_date = "";
//                                        //------
//                                        newData.sor_code = UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, sSorField);
//                                        newData.sor_serial = comm.sGetInt32(sSorCode);
//                                        newData.des_memo = "";
//                                        newData.sta_code = "";
//                                        newData.is_end = "N";
//                                        newData.sto_code = "";

//                                        STT02_0100Repository repoSTT02_0100 = new STT02_0100Repository();
//                                        repoSTT02_0100.InsertData(newData);

//                                        break;
//                                }
//                            }
//                        }
//                    }
//                    return RedirectToAction("Update", form["link"], new { pTkCode = form["ut_code"] });
//            }
//            return View();
//        }

//        public string Get_STT01_SorCode(string pSorCode, string pPerCode = "", string pSDate = "", string pEDate = "")
//        {
//            string sWhereStr = "";

//            sWhereStr = " where STT01_0000.por_date between '" + pSDate + "' and '" + pEDate + "'" +
//                        "   and (pro_qty-res_qty>0) " +
//                        "   and is_end<>'Y' " +
//                        "   and (end_date = '' or end_date is null) ";
//            //"   and BDM06_0000.chk_usr1<>'' ";

//            if (!string.IsNullOrEmpty(pPerCode))
//            {
//                sWhereStr += " and STT01_0000.per_code = '" + pPerCode + "'";
//            }

//            if (!string.IsNullOrEmpty(pSorCode))
//            {
//                sWhereStr += " and STT01_0000.por_code like '%" + pSorCode + "%'";
//            }

//            string sSql = UT.Get_TableDataStr("STT01_0100") +
//                          sWhereStr +
//                          " order by stt01_0100";
//            return comm.DataFieldToStr(sSql, "stt01_0100");
//        }

//        //採購單------------------------------------------------------------------------------------------------------------------------------
//        public ActionResult STT02(string pLink, string pTkCode)
//        {
//            ViewBag.SorCode = "";
//            ViewBag.QuerySDate = "2004/01/01";
//            ViewBag.QueryEDate = DateTime.Now.ToString("yyyy/MM/dd");
//            ViewBag.TkCode = pTkCode;
//            ViewBag.Link = pLink;
//            return View();
//        }

//        [HttpPost]
//        public ActionResult STT02(FormCollection form)
//        {
//            switch (form["submit"])
//            {
//                case "search":
//                    //查詢
//                    ViewBag.SorCode = Get_STT02_SorCode(form["sor_code"], form["sup_code"], form["s_date"], form["e_date"]);
//                    ViewBag.QuerySorCode = form["sor_code"];
//                    ViewBag.QueryPerCode = form["per_code"];
//                    ViewBag.QuerySupCode = form["sup_code"];
//                    ViewBag.QuerySDate = form["s_date"];
//                    ViewBag.QueryEDate = form["e_date"];
//                    ViewBag.TkCode = form["ut_code"];
//                    ViewBag.Link = form["link"];
//                    break;
//                case "Retrieve":
//                    //取回      
//                    string sUTTable = UT.Chk_LinkTable(form["link"]); //被承接資料表
//                    string sUTKey = UT.Chk_LinkKey(form["link"]); //被承接鍵值欄位

//                    string sSorTable = "STT02_0100";
//                    string sSorKey = "stt02_0100"; //承接鍵值欄位                    
//                    string sSorField = "pur_code"; //單號欄位

//                    if (form["checkbox"] != "" && form["checkbox"] != null)
//                    {
//                        for (int i = 0; i < form["checkbox"].Split(',').Length; i++)
//                        {
//                            string sSorCode = form["checkbox"].Split(',')[i]; //承接單號
//                            string sUTCode = comm.Chg_HtmlToDB(form["ut_code"], "textbox"); //被承接單號

//                            if (comm.sGetDecimal(form["sq2_" + sSorCode + ""]) > 0)
//                            {
//                                switch (form["link"]) {
//                                    case "STT03_A":
//                                        STT03_0100 newData = new STT03_0100();
                               
//                                        newData.stt03_0100 = comm.sGetInt32(ws.AutoInt2(sUTTable).ToString());
//                                        newData.mtp_code = sUTCode;
//                                        newData.scr_no = UT.Get_NextScrNo(sUTTable, "mtp_code", sUTCode);
//                                        newData.pro_code = UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, "pro_code");
//                                        newData.pro_qty = UT.Chk_SorQty(comm.sGetDecimal(UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, "sor_qty2")), comm.sGetDecimal(form["sq2_" + sSorCode + ""]));
//                                        newData.res_qty = 0;
//                                        newData.pro_price = comm.sGetDecimal(UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, "pro_price"));
//                                        newData.rea_price = comm.sGetDecimal(UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, "rea_price"));                                
//                                        //------
//                                        newData.sor_code = UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, sSorField);
//                                        newData.sor_serial = comm.sGetInt32(sSorCode);
//                                        newData.des_memo = "";
//                                        newData.sta_code = "";
//                                        newData.is_end = "N";
//                                        newData.sto_code = UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, "sto_code");

//                                        STT03_0100Repository repoSTT03_0100 = new STT03_0100Repository();
//                                        repoSTT03_0100.InsertData(newData);
//                                        break;
//                                }                                
//                            }
//                        }
//                    }
//                    return RedirectToAction("Update", form["link"], new { pTkCode = form["ut_code"] });
//            }
//            return View();
//        }
       
//        public string Get_STT02_SorCode(string pSorCode, string pSupCode = "", string pSDate = "", string pEDate = "")
//        {
//            string sWhereStr = "";

//            sWhereStr = " where STT02_0000.pur_date between '" + pSDate + "' and '" + pEDate + "'" +
//                        "   and (pro_qty-res_qty>0) " +
//                        "   and is_end<>'Y' " +
//                        "   and (end_date = '' or end_date is null) ";
//            //"   and BDM06_0000.chk_usr1<>'' ";

//            //if (!string.IsNullOrEmpty(pPerCode))
//            //{
//            //    sWhereStr += " and STT02_0000.per_code = '" + pPerCode + "'";
//            //}

//            if (!string.IsNullOrEmpty(pSupCode))
//            {
//                sWhereStr += " and STT02_0000.sup_code = '" + pSupCode + "'";
//            }

//            if (!string.IsNullOrEmpty(pSorCode))
//            {
//                sWhereStr += " and STT02_0000.pur_code like '%" + pSorCode + "%'";
//            }

//            string sSql = UT.Get_TableDataStr("STT02_0100") +
//                          sWhereStr +
//                          " order by stt02_0100";
//            return comm.DataFieldToStr(sSql, "stt02_0100");
//        }


//        //進貨單------------------------------------------------------------------------------------------------------------------------------
//        public ActionResult STT03(string pLink, string pTkCode)
//        {
//            ViewBag.SorCode = "";
//            ViewBag.QuerySDate = "2004/01/01";
//            ViewBag.QueryEDate = DateTime.Now.ToString("yyyy/MM/dd");
//            ViewBag.TkCode = pTkCode;
//            ViewBag.Link = pLink;
//            return View();
//        }

//        [HttpPost]
//        public ActionResult STT03(FormCollection form)
//        {
//            switch (form["submit"])
//            {
//                case "search":
//                    //查詢
//                    ViewBag.SorCode = Get_STT03_SorCode(form["sor_code"], form["sup_code"], form["k_inv_no"], form["s_date"], form["e_date"]);
//                    ViewBag.QuerySorCode = form["sor_code"];
//                    ViewBag.QueryPerCode = form["per_code"];
//                    ViewBag.QuerySupCode = form["sup_code"];
//                    ViewBag.QueryKInvNo = form["k_inv_no"];
//                    ViewBag.QuerySDate = form["s_date"];
//                    ViewBag.QueryEDate = form["e_date"];
//                    ViewBag.TkCode = form["ut_code"];
//                    ViewBag.Link = form["link"];
//                    break;
//                case "Retrieve":
//                    //取回      
//                    string sUTTable = UT.Chk_LinkTable(form["link"]); //被承接資料表
//                    string sUTKey = UT.Chk_LinkKey(form["link"]); //被承接鍵值欄位

//                    string sSorTable = "STT03_0100";
//                    string sSorKey = "stt03_0100"; //承接鍵值欄位
//                    string sSorField = "mtp_code"; //單號欄位

//                    if (form["checkbox"] != "" && form["checkbox"] != null)
//                    {
//                        for (int i = 0; i < form["checkbox"].Split(',').Length; i++)
//                        {
//                            string sSorCode = form["checkbox"].Split(',')[i]; //承接單號
//                            string sUTCode = comm.Chg_HtmlToDB(form["ut_code"], "textbox"); //被承接單號

//                            if (comm.sGetDecimal(form["sq2_" + sSorCode + ""]) > 0)
//                            {
//                                switch (form["link"]) {
//                                    case "STT04_A":
//                                        STT04_0100 newData = new STT04_0100();

//                                        newData.stt04_0100 = comm.sGetInt32(ws.AutoInt2(sUTTable).ToString());
//                                        newData.mup_code = sUTCode;
//                                        newData.scr_no = UT.Get_NextScrNo(sUTTable, "mup_code", sUTCode);
//                                        newData.pro_code = UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, "pro_code");
//                                        newData.pro_qty = UT.Chk_SorQty(comm.sGetDecimal(UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, "sor_qty2")), comm.sGetDecimal(form["sq2_" + sSorCode + ""]));
//                                        //newData.res_qty = 0;
//                                        newData.pro_price = comm.sGetDecimal(UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, "pro_price"));
//                                        newData.rea_price = comm.sGetDecimal(UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, "rea_price"));
//                                        //------
//                                        newData.sor_code = UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, sSorField);
//                                        newData.sor_serial = comm.sGetInt32(sSorCode);
//                                        newData.des_memo = "";
//                                        newData.sta_code = "";
//                                        newData.is_end = "N";
//                                        newData.sto_code = UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, "sto_code");

//                                        STT04_0100Repository repoSTT04_0100 = new STT04_0100Repository();
//                                        repoSTT04_0100.InsertData(newData);
//                                        break;
//                                }
//                            }
//                        }
//                    }
//                    return RedirectToAction("Update", form["link"], new { pTkCode = form["ut_code"] });
//            }
//            return View();
//        }
  
//        public string Get_STT03_SorCode(string pSorCode, string pSupCode = "", string pKInvNo = "", string pSDate = "", string pEDate = "")
//        {
//            string sWhereStr = "";

//            sWhereStr = " where STT03_0000.mtp_date between '" + pSDate + "' and '" + pEDate + "'";
//            //"   and BDM06_0000.chk_usr1<>'' ";

//            //if (!string.IsNullOrEmpty(pPerCode))
//            //{
//            //    sWhereStr += " and STT03_0000.per_code = '" + pPerCode + "'";
//            //}

//            if (!string.IsNullOrEmpty(pSupCode))
//            {
//                sWhereStr += " and STT03_0000.sup_code = '" + pSupCode + "'";
//            }

//            if (!string.IsNullOrEmpty(pSorCode))
//            {
//                sWhereStr += " and STT03_0000.mtp_code like '%" + pSorCode + "%'";
//            }

//            if (!string.IsNullOrEmpty(pKInvNo))
//            {
//                sWhereStr += " and STT03_0000.k_inv_no like '%" + pKInvNo + "%'";
//            }

//            string sSql = UT.Get_TableDataStr("STT03_0100") +
//                          sWhereStr +
//                          " order by stt03_0100";
//            return comm.DataFieldToStr(sSql, "stt03_0100");
//        }


//        //借出單------------------------------------------------------------------------------------------------------------------------------
//        public ActionResult STT18(string pLink, string pTkCode)
//        {
//            ViewBag.SorCode = "";
//            ViewBag.QuerySDate = "2004/01/01";
//            ViewBag.QueryEDate = DateTime.Now.ToString("yyyy/MM/dd");
//            ViewBag.TkCode = pTkCode;
//            ViewBag.Link = pLink;
//            return View();
//        }

//        [HttpPost]
//        public ActionResult STT18(FormCollection form)
//        {
//            switch (form["submit"])
//            {
//                case "search":
//                    //查詢
//                    ViewBag.SorCode = Get_STT18_SorCode(form["sor_code"], form["cus_code"], form["s_date"], form["e_date"]);
//                    ViewBag.QuerySorCode = form["sor_code"];
//                    ViewBag.QueryPerCode = form["per_code"];
//                    ViewBag.QueryCusCode = form["cus_code"];
//                    ViewBag.QuerySDate = form["s_date"];
//                    ViewBag.QueryEDate = form["e_date"];
//                    ViewBag.TkCode = form["ut_code"];
//                    ViewBag.Link = form["link"];
//                    break;
//                case "Retrieve":
//                    //取回      
//                    string sUTTable = UT.Chk_LinkTable(form["link"]); //被承接資料表
//                    string sUTKey = UT.Chk_LinkKey(form["link"]); //被承接鍵值欄位

//                    string sSorTable = "STT18_0100";
//                    string sSorKey = "stt18_0100"; //承接鍵值欄位
//                    string sSorField = "ioa_code"; //單號欄位

//                    if (form["checkbox"] != "" && form["checkbox"] != null)
//                    {
//                        for (int i = 0; i < form["checkbox"].Split(',').Length; i++)
//                        {
//                            string sSorCode = form["checkbox"].Split(',')[i]; //承接單號
//                            string sUTCode = comm.Chg_HtmlToDB(form["ut_code"], "textbox"); //被承接單號

//                            if (comm.sGetDecimal(form["sq2_" + sSorCode + ""]) > 0)
//                            {
//                                switch (form["link"])
//                                {
//                                    case "STT09_A":
//                                        STT09_0100 newData = new STT09_0100();

//                                        newData.stt09_0100 = comm.sGetInt32(ws.AutoInt2(sUTTable).ToString());
//                                        newData.sal_code = sUTCode;
//                                        newData.scr_no = UT.Get_NextScrNo(sUTTable, "sal_code", sUTCode);
//                                        newData.pro_code = UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, "pro_code");
//                                        newData.pro_qty = UT.Chk_SorQty(comm.sGetDecimal(UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, "sor_qty2")), comm.sGetDecimal(form["sq2_" + sSorCode + ""]));
//                                        //newData.res_qty = 0;
//                                        newData.pro_price = comm.sGetDecimal(UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, "pro_price"));
//                                        newData.sor_price = comm.sGetDecimal(UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, "pro_price"));
//                                        //------
//                                        newData.sor_code = UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, sSorField);
//                                        newData.sor_serial = comm.sGetInt32(sSorCode);
//                                        newData.des_memo = "";
//                                        newData.sta_code = "";
//                                        newData.is_end = "N";
//                                        newData.sto_code = UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, "sto_cst");

//                                        STT09_0100Repository repoSTT09_0100 = new STT09_0100Repository();
//                                        repoSTT09_0100.InsertData(newData);
//                                        break;

//                                    case "STT20_A":
//                                        STT20_0100 newData2 = new STT20_0100();

//                                        newData2.stt20_0100 = comm.sGetInt32(ws.AutoInt2(sUTTable).ToString());
//                                        newData2.ioc_code = sUTCode;
//                                        newData2.scr_no = UT.Get_NextScrNo(sUTTable, "ioc_code", sUTCode);
//                                        newData2.pro_code = UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, "pro_code");
//                                        newData2.pro_qty = UT.Chk_SorQty(comm.sGetDecimal(UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, "sor_qty2")), comm.sGetDecimal(form["sq2_" + sSorCode + ""]));
//                                        //newData.res_qty = 0;
//                                        newData2.pro_price = comm.sGetDecimal(UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, "pro_price"));
//                                        //newData2.sor_price = comm.sGetDecimal(UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, "s_price"));
//                                        //------
//                                        newData2.sor_code = UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, sSorField);
//                                        newData2.sor_serial = comm.sGetInt32(sSorCode);
//                                        newData2.des_memo = "";
//                                        newData2.sta_code = "";
//                                        //newData2.is_end = "N";
//                                        newData2.sto_cst = UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, "sto_cst");
//                                        newData2.sto_in = "";
//                                        newData2.ioc_stat = "1";

//                                        STT20_0100Repository repoSTT20_0100 = new STT20_0100Repository();
//                                        repoSTT20_0100.InsertData(newData2);
//                                        break;
//                                }
//                            }
//                        }
//                    }
//                    return RedirectToAction("Update", form["link"], new { pTkCode = form["ut_code"] });
//            }
//            return View();
//        }

//        public string Get_STT18_SorCode(string pSorCode, string pCusCode = "", string pSDate = "", string pEDate = "")
//        {
//            string sWhereStr = "";

//            sWhereStr = " where STT18_0000.ioa_date between '" + pSDate + "' and '" + pEDate + "' " +
//                        "   and (pro_qty-res_qty)>0 ";
//                        //"   and BDM06_0000.chk_usr1<>'' ";

//            if (!string.IsNullOrEmpty(pCusCode))
//            {
//                sWhereStr += " and STT18_0000.cst_code = '" + pCusCode + "'";
//            }

//            if (!string.IsNullOrEmpty(pSorCode))
//            {
//                sWhereStr += " and STT18_0000.ioa_code like '%" + pSorCode + "%'";
//            }

//            string sSql = UT.Get_TableDataStr("STT18_0100") +
//                          sWhereStr +
//                          " order by stt18_0100";
//            return comm.DataFieldToStr(sSql, "stt18_0100");
//        }


//        //詢價單------------------------------------------------------------------------------------------------------------------------------
//        public ActionResult STT22(string pLink, string pTkCode)
//        {
//            ViewBag.SorCode = "";
//            ViewBag.QuerySDate = "2004/01/01";
//            ViewBag.QueryEDate = DateTime.Now.ToString("yyyy/MM/dd");
//            ViewBag.TkCode = pTkCode;
//            ViewBag.Link = pLink;
//            return View();
//        }

//        [HttpPost]
//        public ActionResult STT22(FormCollection form)
//        {
//            switch (form["submit"])
//            {
//                case "search":
//                    //查詢
//                    ViewBag.SorCode = Get_STT22_SorCode(form["sor_code"], form["per_code"], form["s_date"], form["e_date"]);
//                    ViewBag.QuerySorCode = form["sor_code"];
//                    ViewBag.QueryPerCode = form["per_code"];
//                    //ViewBag.QuerySupCode = form["sup_code"];
//                    //ViewBag.QueryKInvNo = form["k_inv_no"];
//                    ViewBag.QuerySDate = form["s_date"];
//                    ViewBag.QueryEDate = form["e_date"];
//                    ViewBag.TkCode = form["ut_code"];
//                    ViewBag.Link = form["link"];
//                    break;
//                case "Retrieve":
//                    //取回      
//                    string sUTTable = UT.Chk_LinkTable(form["link"]); //被承接資料表
//                    string sUTKey = UT.Chk_LinkKey(form["link"]); //被承接鍵值欄位

//                    string sSorTable = "STT22_0000";
//                    string sSorKey = "inq_code"; //承接鍵值欄位
//                    string sSorField = "inq_code"; //單號欄位

//                    if (form["checkbox"] != "" && form["checkbox"] != null)
//                    {
//                        for (int i = 0; i < form["checkbox"].Split(',').Length; i++)
//                        {
//                            string sSorCode = form["checkbox"].Split(',')[i]; //承接單號
//                            string sUTCode = comm.Chg_HtmlToDB(form["ut_code"], "textbox"); //被承接單號

//                            if (comm.sGetDecimal(form["sq2_" + sSorCode + ""]) > 0)
//                            {
//                                switch (form["link"])
//                                {
//                                    case "STT02_A":
//                                        STT02_0100 newData = new STT02_0100();

//                                        newData.stt02_0100 = comm.sGetInt32(ws.AutoInt2(sUTTable).ToString());
//                                        newData.pur_code = sUTCode;
//                                        newData.scr_no = UT.Get_NextScrNo(sUTTable, "pur_code", sUTCode);
//                                        newData.pro_code = UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, "pro_code");
//                                        newData.pro_qty = UT.Chk_SorQty(comm.sGetDecimal(UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, "sor_qty2")), comm.sGetDecimal(form["sq2_" + sSorCode + ""]));
//                                        newData.res_qty = 0;
//                                        newData.pro_price = comm.sGetDecimal(UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, "pro_price"));
//                                        newData.rea_price = comm.sGetDecimal(UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, "pro_price"));
//                                        newData.hop_date = "";
//                                        newData.res_date = "";
//                                        //------
//                                        newData.sor_code = UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, sSorField);
//                                        newData.sor_serial = 0;
//                                        newData.des_memo = "";
//                                        newData.sta_code = "";
//                                        newData.is_end = "N";
//                                        newData.sto_code = UT.Get_SorData(UT.Get_TableDataStr(sSorTable), sSorKey, sSorCode, "sto_state");

//                                        STT02_0100Repository repoSTT02_0100 = new STT02_0100Repository();
//                                        repoSTT02_0100.InsertData(newData);
//                                        break;
//                                }
//                            }
//                        }
//                    }
//                    return RedirectToAction("Update", form["link"], new { pTkCode = form["ut_code"] });
//            }
//            return View();
//        }

//        public string Get_STT22_SorCode(string pSorCode, string pPerCode = "", string pSDate = "", string pEDate = "")
//        {
//            string sWhereStr = "";
//            //string sSelectInq = "";

//            sWhereStr = " where STT22_0000.inq_date between '" + pSDate + "' and '" + pEDate + "'";

//            if (!string.IsNullOrEmpty(pPerCode))
//            {
//                sWhereStr += " and STT22_0000.per_code = '" + pPerCode + "'";
//            }

//            if (!string.IsNullOrEmpty(pSorCode))
//            {
//                sWhereStr += " and STT22_0000.inq_code like '%" + pSorCode + "%'";
//            }

//            string sSql = UT.Get_TableDataStr("STT22_0000") +
//                          sWhereStr +
//                          " order by STT22_0000.inq_code";
//            return comm.DataFieldToStr(sSql, "inq_code");
//        }

//    }
//}