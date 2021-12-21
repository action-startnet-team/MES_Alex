using Dapper;
using MES_WATER.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using NPOI;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Data;
using System.Web.Routing;
using Newtonsoft.Json;
using System.Text;

namespace MES_WATER.Controllers
{
    public class RSS030BController : Controller
    {
        Comm comm = new Comm();
        GetData GD = new GetData();
        DynamicTable DT = new DynamicTable();
        CheckData CD = new CheckData();
        ExportController Exp = new ExportController();
        Review RV = new Review();
        ReportReview RpRv = new ReportReview();

        //表單欄位table
        public string pubFieldTable = "RSS02_0100";

        //索引鍵
        public string pubPKCode()
        {
            return DT.Get_Table_PKField(pubFieldTable);
        }

        public string epbTable = "EPB02_0100";

        //索引鍵
        public string epbPKCode()
        {
            return DT.Get_Table_PKField(epbTable);
        }

        public string Get_EtlCode(string sReportCode)
        {
            string sEtlCode = GD.Get_Data("RSS02_0000", sReportCode, "report_code", "etl_code");
            return sEtlCode;
        }


        // GET: RSS030B
        public ActionResult Index()
        {
            Set_Cookie();
            return View();
        }

        public ActionResult Report(string K)
        {
            //K = 報表代號
            ViewBag.Key = K;
            ViewBag.SqlStr = Sql_SelectStr(Get_EtlCode(K)) + " where 1=0";

            return View();
        }



        [HttpPost]
        public ActionResult Report(FormCollection form)
        {
            string sRptCode = comm.sGetString(form["report_code"]);
            DataTable dtTmp = comm.Get_DataTable(Get_SqlStr(form));
            string sFileLocaltion = Server.MapPath("~/Upload/Report/");

            switch (form["submit"])
            {
                case "query":                    
                    break;
                case "download":                   
                    switch (sRptCode) {
                        //case "C020":                                                        
                        //    //特殊報表則另外寫
                        //    //dtTmp = Get_CustomDataTable(sRptCode, dtTmp);
                        //    IWorkbook Excel = Exp.Export_ExcelByDataTable(sRptCode, dtTmp, sFileLocaltion);
                        //    Excel = Exp.CodeReplace(Excel);
                        //    Download_Excel(Excel, sRptCode + ".xls");
                        //    break;
                        default:
                            Export_Excel(sRptCode, Get_SqlStr(form));
                            break;
                    }
                    break;
                case "review":
                    IWorkbook Excel = Exp.Export_ExcelByDataTable(sRptCode, dtTmp, sFileLocaltion);

                    object data = new object();
                    string sTkCode = RpRv.Get_ReportGroupCode(sRptCode);

                    //將上傳的範本放在/Upload/ReportTmp，命名為選擇的報表代號
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Excel.Write(ms);
                        byte[] byt = ms.ToArray();
                        ms.Flush();
                        ms.Close();
                        System.IO.File.WriteAllBytes(Server.MapPath("~/Upload/ReportTmp/" + sTkCode + ".xls"), byt);
                        byt = null;
                    }

                    //新增出一組報表群組碼
                    data = new
                    {
                        report_group_code = sTkCode,
                        report_code = sRptCode,
                        report_type = "B",
                        ins_date = DateTime.Now.ToString("yyyy/MM/dd"),
                        ins_time = DateTime.Now.ToString("HH:mm:ss"),
                        usr_code = User.Identity.Name,
                    };
                    DT.InsertData("RSS03_0000", data);

                    //再把報表集成群組碼放到審核作業
                    RpRv.Ins_ReviewByReportGroup(sTkCode, User.Identity.Name);
                    return RedirectToAction("Index", "EPB050A");
            }
            ViewBag.Key = sRptCode;
            ViewBag.SqlStr = Get_SqlStr(form);
            return View();
        }


        



        public ActionResult ReportByDataTable(string K)
        {
            //K = 報表代號
            ViewBag.Key = K;
            ViewBag.SqlStr = Sql_SelectStr(Get_EtlCode(K)) + " where 1=0";
            ViewBag.DataTable = new DataTable();

            return View();
        }

        [HttpPost]
        public ActionResult ReportByDataTable(FormCollection form)
        {
            string sRptCode = comm.sGetString(form["report_code"]);
            string sSql_Str = Get_SqlStr(form);            
            IWorkbook Excel;
            string sFileLocaltion = Server.MapPath("~/Upload/Report/");

            DataTable dtTmp = comm.Get_DataTable(sSql_Str);
            dtTmp = Get_CustomDataTable(sRptCode, dtTmp, form);

            DataTable dtReport = comm.Get_DataTable(sSql_Str);
            dtReport = Get_CustomDataTable(sRptCode, dtReport, form);
            dtReport = Get_CustomReportDataTable(sRptCode, dtReport, form);
            //特殊報表則另外寫
            Excel = Exp.Export_ExcelByDataTable(sRptCode, dtReport, sFileLocaltion);
            Excel = Exp.CodeReplace(Excel);

            switch (form["submit"])
            {
                case "query":
                    break;
                case "download":
                    switch (sRptCode)
                    {
                        case "C020":                                                        
                            
                            Download_Excel(Excel, sRptCode + ".xls");
                            break;
                        default:
                            Export_Excel(sRptCode, Get_SqlStr(form));
                            break;
                        
                    }
                    break;
                case "review":                    
                    object data = new object();
                    string sTkCode = RpRv.Get_ReportGroupCode(sRptCode);

                    //將上傳的範本放在/Upload/ReportTmp，命名為選擇的報表代號
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Excel.Write(ms);
                        byte[] byt = ms.ToArray();
                        ms.Flush();
                        ms.Close();
                        System.IO.File.WriteAllBytes(Server.MapPath("~/Upload/ReportTmp/" + sTkCode + ".xls"), byt);
                        byt = null;
                    }

                    //新增出一組報表群組碼
                    data = new
                    {
                        report_group_code = sTkCode,
                        report_code = sRptCode,
                        report_type = "B",
                        ins_date = DateTime.Now.ToString("yyyy/MM/dd"),
                        ins_time = DateTime.Now.ToString("HH:mm:ss"),
                        usr_code = User.Identity.Name,
                    };
                    DT.InsertData("RSS03_0000", data);

                    //再把報表集成群組碼放到審核作業
                    RpRv.Ins_ReviewByReportGroup(sTkCode, User.Identity.Name);
                    return RedirectToAction("Index", "EPB050A");
            }
            

            ViewBag.Key = sRptCode;
            ViewBag.SqlStr = Get_SqlStr(form);
            ViewBag.DataTable = dtTmp;
            return View();
        }



        public ActionResult DataView(string K)
        {
            //轉運站(這邊這樣寫是因為，選單的架構無法明確指定某個路徑所致)
            switch (K) {
                case "C020":
                    return RedirectToAction("ReportByDataTable", new { K = K });
                case "C021-1":
                case "C021-2":
                case "C004":
                case "Qmt_Report":
                    return RedirectToAction("Report", new { K = K });
                default:
                    break;
            }
            ViewBag.Key = K;

            return View();
        }

        [HttpPost]
        public ActionResult DataView(FormCollection form)
        {
            string sValue = "";
            if (!string.IsNullOrEmpty(form["checkbox"]))
            {
                //sValue = GD.StrArrayToSql(form["checkbox"]);
                sValue = form["checkbox"];
            }
            return RedirectToAction("Export_EPB", "Export", new { pRptCode = form["report_code"], pValueArray = sValue, pIsReview = false });
        }

        public ActionResult Export(string R, string KV)
        {
            return new EmptyResult();
        }


        public string GetData_DataTable(string pSql)
        {
            string sSql = pSql.Replace("&quot;", "'");
            var dtTmp = comm.Get_DataTable(sSql);
            return JsonConvert.SerializeObject(dtTmp);
        }


        public string Sql_SelectStr(string pEtlCode)
        {
            return comm.Get_QueryData("RSS01_0000", pEtlCode, "etl_code", "select_string");
        }

        public string Sql_WhereStr(string pEtlCode)
        {
            return comm.Get_QueryData("RSS01_0000", pEtlCode, "etl_code", "where_string");
        }

        public string Get_EtlData(string pEtlCode, string pFieldCode)
        {
            string sSql = "select * from RSS01_0100" +
                          " where etl_code = @etl_code";
            var dtTmp = comm.Get_DataTable(sSql, "etl_code", pEtlCode);
            return GD.DataFieldToStr(dtTmp, pFieldCode);
        }


        /// <summary>
        /// 取得表單類型
        /// </summary>
        /// <returns></returns>
        public string Get_EpbType(string pUsrCode)
        {
            //取得表單類型
            string sSql = "select * from EPB01_0000 " +
                          "   where epb_type_code in (" + GD.StrArrayToSql(GD.Get_EpbCanUseType(pUsrCode)) + ")";
            return comm.DataFieldToStr(sSql, "epb_type_code");
        }

        /// <summary>
        /// 取得表單代號
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public string Get_EpbCode(string Key)
        {
            //取得電子表單權限
            string sSql = "select * from BDP09_0200 " +
                          " where usr_code = '" + User.Identity.Name + "'" +
                          "   and is_use = 'Y'";
            string sEpbCodeArray = GD.DataFieldToStr(sSql, "epb_code");

            sSql = "select * from EPB02_0000 " +
                   " where epb_type_code = '" + Key + "'" +
                   "   and epb_code in (" + GD.StrArrayToSql(sEpbCodeArray) + ")";
            return GD.DataFieldToSTA(sSql, "epb_code,epb_name");
        }

        public string Get_ReportByEpb(string Key) {
            string sSql = "select * from RSS02_0000" +
                          " where epb_code = '" + Key + "'";
            return GD.DataFieldToSTA(sSql, "report_code,report_name");
        }


        public string Get_SqlStr(FormCollection form)
        {
            string sEtlCode = GD.Get_Data("RSS02_0000", form["report_code"], "report_code", "etl_code");

            string sWhere = " where " + Sql_WhereStr(sEtlCode);
            if (Sql_WhereStr(sEtlCode).Trim() == "") { sWhere += " 1=1"; }

            sWhere += Get_WhereStr(form);
            string sSql = Sql_SelectStr(sEtlCode) + sWhere;
            return sSql;
        }


        private string Get_WhereStr(FormCollection form)
        {
            string sValue = "";

            string sSql = "select * from RSS02_0100" +
                          " where report_code = '" + form["report_code"] + "'";
            var dtTmp = comm.Get_DataTable(sSql);
            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                string sField = dtTmp.Rows[i]["field_code"].ToString();
                string CtrType = dtTmp.Rows[i]["ctr_type"].ToString();

                sValue += GD.Sort_WhereType(form, CtrType, sField);
            }
            return sValue;
        }



        public string Get_Report(string Key)
        {
            //取得電子表單權限
            string sSql = "select * from BDP09_0200 " +
                          " where usr_code = '" + User.Identity.Name + "'" +
                          "   and is_use = 'Y'";
            string sEpbCodeArray = GD.DataFieldToStr(sSql, "epb_code");

            sSql = "select * from RSS02_0000 " +
                   " where report_type = '" + Key + "'" +
                   "   and (epb_code = '' or epb_code in (" + GD.StrArrayToSql(sEpbCodeArray) + "))";
            return GD.DataFieldToSTA(sSql, "report_code,report_name");
        }


        /// <summary>
        /// 取得報表欄位
        /// </summary>
        /// <param name="Key">報表代號</param>
        /// <returns></returns>
        public string Get_RptField(string Key)
        {
            string sSql = "select * from " + pubFieldTable +
                          " where report_code = '" + Key + "'" +
                          "  order by scr_no ";
            return comm.DataFieldToStr(sSql, pubPKCode());
        }


        /// <summary>
        /// 取得表單欄位
        /// </summary>
        /// <param name="Key">表單代號</param>
        /// <returns></returns>
        public string Get_EpbField(string Key)
        {
            string sSql = "select * from EPB02_0100 " +
                          " where epb_code = '" + Key + "'" +
                          "  order by scr_no ";
            return comm.DataFieldToStr(sSql, "epb02_0100");
        }


        /// <summary>
        /// 取得該表單 資料鍵值
        /// </summary>
        /// <param name="pEpbCode"></param>
        /// <returns></returns>
        public string Get_DataValue(string pEpbCode)
        {
            string sValue = "";
            string sKeyField = "";

            string sSql = "select * from EPB02_0100 " +
                          " where epb_code = '" + pEpbCode + "' " +
                          "   and is_key = 'Y'";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                sKeyField = dtTmp.Rows[0]["field_code"].ToString();

                //判斷審核設定
                //若該表單有審核設定
                if (RV.Chk_CanReview(pEpbCode))
                {
                    //則表單資料需要審核決行通過才會進到統計報表
                    sSql = "select * from EPB03_0000 " +
                       " where epb_code = '" + pEpbCode + "' " +
                       "   and field_code = '" + sKeyField + "'" +
                       "   and key_value in(" + GD.StrArrayToSql(RV.Get_ReviewPassData(pEpbCode)) + ")";
                }
                else
                {
                    //取得該表單內所有的鍵值
                    sSql = "select * from EPB03_0000 " +
                           " where epb_code = '" + pEpbCode + "' " +
                           "   and field_code = '" + sKeyField + "'";
                }
                //取得該表單內所有的鍵值
                sSql = "select * from EPB03_0000 " +
                       " where epb_code = '" + pEpbCode + "' " +
                       "   and field_code = '" + sKeyField + "'";
                sValue = GD.DataFieldToStr(sSql, "key_value");
            }
            return sValue;
        }

        /// <summary>
        /// 取得該欄位的值
        /// </summary>
        /// <param name="pEpbCode">表單代號</param>
        /// <param name="pField">欄位</param>
        /// <returns></returns>
        public string Get_FieldValue(string pEpbCode, string pField, string pKeyValue)
        {
            string sValue = "";
            string sSql = "select * from EPB03_0000 " +
                           " where epb_code = '" + pEpbCode + "' " +
                           "   and field_code = '" + pField + "'" +
                           "   and key_value = '" + pKeyValue + "'";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                sValue = dtTmp.Rows[0]["field_value"].ToString();
            }
            return sValue;
        }



        /// <summary>
        /// 檢查input
        /// </summary>
        /// <param name="Key">索引鍵</param>
        /// <param name="pValue">索引值</param>
        /// <returns></returns>
        public string Chk_Input(string Key, string pValue)
        {
            //return CD.Chk_Input(pubFieldTable, pubPKCode(), Key, pValue);
            return "";
        }

        public string Chk_Source(string R)
        {
            string sValue = "";
            string fileLocation = Server.MapPath("~/Upload/Report/") + R;
            if (!System.IO.File.Exists(fileLocation + ".xls") && !System.IO.File.Exists(fileLocation + ".xlsx")) // 驗證檔案是否存在
            {
                sValue = "尚未上傳範本";
            }
            return sValue;
        }

        public bool Chk_CanReview(string pEpbCode)
        {            
            return RpRv.Chk_CanReview(pEpbCode);
        }

        public bool Chk_UsrIsReviewerOfEpb(string pEpbCode)
        {           
            return RpRv.Chk_UsrIsReviewerOfEpb(pEpbCode,User.Identity.Name);
        }



        public string GetData(string T, string K, string KF, string F)
        {
            return GD.Get_Data(T, K, KF, F);
        }


        /// <summary>
        /// 儲存cookie
        /// </summary>
        /// <param name="pCookieName"></param>
        /// <param name="pValue"></param>
        public void Save_Cookie(string pCookieName, string pValue)
        {
            Response.Cookies[pCookieName].Value = pValue;
        }


        /// <summary>
        /// 設定cookie
        /// </summary>
        public void Set_Cookie()
        {
            //紀錄cookie
            if (Request.Cookies["RptType"] == null)
            {
                HttpCookie Cookie = new HttpCookie("RptType")
                {
                    Value = "",
                    Expires = DateTime.Now.AddDays(1d),
                };
                Response.Cookies.Add(Cookie);
            }

            if (Request.Cookies["RptCode"] == null)
            {
                HttpCookie Cookie = new HttpCookie("RptCode")
                {
                    Value = "",
                    Expires = DateTime.Now.AddDays(1d),
                };
                Response.Cookies.Add(Cookie);
            }
        }




        /// <summary>
        /// 匯入樞紐分析表，依照表單代號產出Excel
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public ActionResult Export_Excel(string pRptCode, string pSql)
        {
            string sSql = "";
            DataTable dtTmp = null;
            string sRptCode = pRptCode;
            string sEtlCode = GD.Get_Data("RSS02_0000", sRptCode, "report_code", "etl_code");

            string fileLocation = Server.MapPath("~/Upload/Report/") + Get_SampleName(sRptCode);
            if (System.IO.File.Exists(fileLocation)) // 驗證檔案是否存在
            {
                IWorkbook excel;
                // 檔案讀取
                using (FileStream files = new FileStream(fileLocation, FileMode.Open, FileAccess.Read))
                {
                    if (Path.GetExtension(fileLocation).ToLower() == ".xls")
                    {
                        excel = new HSSFWorkbook(files);
                    }
                    else
                    {
                        excel = new XSSFWorkbook(files);
                    }
                }
                ISheet sheet = excel.GetSheetAt(1);  // RowData 放在第二頁
                IRow Row = (IRow)sheet.GetRow(0);  //獲取Sheet1工作表的首行
                IRow Row_tmp;
                ICell[] Cell = new ICell[Row.LastCellNum];

                //先清除RowData
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    sheet.RemoveRow(sheet.GetRow(i));
                }

                for (int i = 0; i < Cell.Count(); i++)
                {
                    Cell[i] = (ICell)Row.GetCell(i);  //取得單元格
                    string sFieldName = Cell[i].ToString();

                    //依照Excel欄位名稱查找系統欄位代號
                    sSql = "select * from RSS01_0100 " +
                           " where etl_code = '" + sEtlCode + "' " +
                           "   and field_name = '" + sFieldName + "'";
                    dtTmp = comm.Get_DataTable(sSql);
                    if (dtTmp.Rows.Count > 0)
                    {
                        //取得欄位代號後，執行Sql語法
                        string sField = dtTmp.Rows[0]["field_code"].ToString();

                        //在資料表裡面 取用指定欄位的資料
                        var dtTmp2 = comm.Get_DataTable(pSql);
                        for (int u = 0; u < dtTmp2.Rows.Count; u++)
                        {
                            if (dtTmp2.Rows[u][sField] != null)
                            {
                                //塞進原Excel裡面
                                string sValue = dtTmp2.Rows[u][sField].ToString();

                                if ((IRow)sheet.GetRow(u + 1) == null)
                                {
                                    Row_tmp = (IRow)sheet.CreateRow(u + 1);
                                }
                                else
                                {
                                    Row_tmp = (IRow)sheet.GetRow(u + 1);
                                }

                                if ((ICell)Row_tmp.GetCell(i) == null)
                                {
                                    Cell[i] = (ICell)Row_tmp.CreateCell(i);  //建立單元格
                                }
                                else
                                {
                                    Cell[i] = (ICell)Row_tmp.GetCell(i);
                                }

                                Cell[i].SetCellValue(sValue); //賦值為字串

                            }
                        }
                    }
                }
                //重新讀取公式
                ISheet sheet2 = excel.GetSheetAt(0);
                sheet2.ForceFormulaRecalculation = true;

                excel = Exp.CodeReplace(excel);

                Download(excel, sRptCode + Path.GetExtension(fileLocation));
            }
            return new EmptyResult();
        }



        public string Get_SampleName(string pCode)
        {
            string sValue = "";
            string fileLocation = Server.MapPath("~/Upload/Report/") + pCode;
            if (System.IO.File.Exists(fileLocation + ".xls")) // 驗證檔案是否存在
            {
                sValue = pCode + ".xls";
            }
            else if (System.IO.File.Exists(fileLocation + ".xlsx"))
            {
                sValue = pCode + ".xlsx";
            }
            else { }

            return sValue;
        }



        private void Download(IWorkbook excel, string pRptCode)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                excel.Write(ms);
                byte[] byt = ms.ToArray();
                ms.Flush();
                ms.Close();
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + pRptCode);
                Response.AddHeader("Content-Length", byt.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.BinaryWrite(byt);
                byt = null;
            }
        }

        /// <summary>
        /// 送審
        /// </summary>
        /// <param name="pKeyArray">電子表單鍵值字串陣列</param>
        public void SendReview(string pReportCode, string pKeyArray) {
            string sSql = "";
            DataTable dtTmp = new DataTable();
            object data = new object();

            string sTkCode = RpRv.Get_ReportGroupCode(pReportCode);

            //先建立報表集成
            //表頭
            data = new {
                report_group_code = sTkCode,
                report_code = pReportCode,
                report_type = "A",
                ins_date = DateTime.Now.ToString("yyyy/MM/dd"),
                ins_time = DateTime.Now.ToString("HH:mm:ss"),
                usr_code = User.Identity.Name,
            };
            DT.InsertData("RSS03_0000", data);

            //表身
            for (int i = 0; i < pKeyArray.Split(',').Length; i++) {
                string sEpbKey = pKeyArray.Split(',')[i];

                data = new {
                    report_group_code = sTkCode,
                    epb_key_value = sEpbKey,
                };
                DT.InsertData("RSS03_0100", data);
            }

            //再把報表集成群組碼放到審核作業
            RpRv.Ins_ReviewByReportGroup(sTkCode, User.Identity.Name);
        }


        public string Get_ReviewUser(string pReviewCode) {
            return RpRv.Get_ReviewUser(pReviewCode);
        }

        public bool Chk_UsrIsReviewerOfReport(string pReportCode)
        {
            return RpRv.Chk_UsrIsReviewerOfReport(pReportCode, User.Identity.Name);
        }






        public DataTable Get_CustomDataTable(string pRptCode, DataTable pDataTable, FormCollection form) {
            //技術難題，這裡目前無法動態，需先知道有哪些控項，然後再另外控制
            string sSql = "";
            string sWhereStr = " where 1=1 " + Get_WhereStr(form);
            DataTable dtTmp = new DataTable();
            DataTable dtRet = pDataTable;
            switch (pRptCode) {
                case "C020":
                    //先新增1號到31號
                    dtRet.Columns.Add("點檢年月");
                    for (int i = 1; i <= 31; i++)
                    {
                        dtRet.Columns.Add(i.ToString());
                    }
                    //查出點檢的紀錄
                    sSql = "select distinct chk_item_code,dev_check_date,is_ok,EMT02_0000.usr_code  " +
                           "  from EMT02_0000" +
                           "  left join EMT02_0100 on EMT02_0000.dev_check_code = EMT02_0100.dev_check_code " +
                           sWhereStr;
                    dtTmp = comm.Get_DataTable(sSql);

                    for (int i = 0; i < dtTmp.Rows.Count; i++) {
                        DataRow r = dtTmp.Rows[i];
                        string sChkItemCode = r["chk_item_code"].ToString();
                        string sDevCheckDate = r["dev_check_date"].ToString().Trim();
                        string sIsOk = r["is_ok"].ToString();
                        string sUsrCode = r["usr_code"].ToString();
                        
                        //將找出的紀錄，放到表的對應項目與對應日期裡
                        for (int u = 0; u < dtRet.Rows.Count; u++) {
                            DataRow ur = dtRet.Rows[u];
                            ur["點檢年月"] = comm.StrLeft(sDevCheckDate,7);
                            if (ur["chk_item_code"].ToString() == sChkItemCode) {
                                int Days = comm.sGetInt32(comm.StrRigth(sDevCheckDate, 2));
                                if (dtRet.Columns.Contains(Days.ToString())) { ur[Days.ToString()] = sIsOk; }
                            }
                        }
                    }
                    break;
            }
            return dtRet;
        }


        public DataTable Get_CustomReportDataTable(string pRptCode, DataTable pDataTable, FormCollection form)
        {
            string sSql = "";
            string sWhereStr = " where 1=1 " + Get_WhereStr(form);
            DataTable dtTmp = new DataTable();
            DataTable dtRet = pDataTable;
            switch (pRptCode)
            {
                case "C020":
                    int rCnt = dtRet.Rows.Count;
                    if (rCnt < 21) {
                        for (int i = 0; i < 21 - rCnt; i++) {
                            DataRow rtR = dtRet.NewRow();
                            dtRet.Rows.Add(rtR);
                        }
                    }
                    //查出點檢的紀錄
                    sSql = "select distinct chk_item_code,dev_check_date,is_ok,EMT02_0000.usr_code,usr_name  " +
                           "  from EMT02_0000" +
                           "  left join EMT02_0100 on EMT02_0000.dev_check_code = EMT02_0100.dev_check_code " +
                           "  left join BDP08_0000 on EMT02_0000.usr_code = BDP08_0000.usr_code" +
                           sWhereStr;
                    dtTmp = comm.Get_DataTable(sSql);
                    for (int i = 0; i < dtTmp.Rows.Count; i++)
                    {
                        DataRow r = dtTmp.Rows[i];
                        string sChkItemCode = r["chk_item_code"].ToString();
                        string sDevCheckDate = r["dev_check_date"].ToString().Trim();
                        string sIsOk = r["is_ok"].ToString();
                        string sUsrCode = r["usr_code"].ToString();
                        string sUsrName = r["usr_name"].ToString();

                        //將找出的紀錄，放到表的對應項目與對應日期裡
                        for (int u = 0; u < dtRet.Rows.Count; u++)
                        {
                            DataRow ur = dtRet.Rows[u];
                            int Days = comm.sGetInt32(comm.StrRigth(sDevCheckDate, 2));
                            if (ur["chk_item_code"].ToString() == sChkItemCode)
                            {                                
                                if (dtRet.Columns.Contains(Days.ToString())) {
                                    ur[Days.ToString()] = sIsOk;
                                    //最後一行放保養人名稱
                                    dtRet.Rows[dtRet.Rows.Count - 1][Days.ToString()] = sUsrName;
                                }
                            }                                                       
                        }
                    }
                    break;
            }
            return dtRet;
        }






        public void Download_Excel(IWorkbook excel, string pRptName)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                excel.Write(ms);
                byte[] byt = ms.ToArray();
                ms.Flush();
                ms.Close();
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + pRptName);
                Response.AddHeader("Content-Length", byt.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.BinaryWrite(byt);
                byt = null;
            }
        }




    }
}