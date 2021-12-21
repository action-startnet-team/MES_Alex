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
using System.Data.SqlClient;
using Dapper;
using NPOI;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;

namespace MES_WATER.Controllers
{
    public class EPB050AController : Controller
    {
        Comm comm = new Comm();
        EPB05_0000Repository repoEPB05_0000 = new EPB05_0000Repository();
        DynamicTable DT = new DynamicTable();
        GetData GD = new GetData();
        Review RV = new Review();
        ReportReview RpRv = new ReportReview();
        ExportController Exp = new ExportController();

        public string PrgCode() {
            return ControllerContext.RouteData.Values["controller"].ToString();
        }

        // GET: EPB050A
        public ActionResult Index()
        {
            Set_Cookie();

            ViewBag.prg_code = PrgCode();

            // 使用者, controllerName, actionName
            string usr_code = User.Identity.Name;
            string prg_code = PrgCode();
            string view_code = ControllerContext.RouteData.Values["action"].ToString();

            //取得欄位寬度
            List<BDP30_0000> colWidth_list = comm.Get_BDP30_0000(usr_code, prg_code, view_code);
            List<BDP30_0000> colWidth_list_D1 = comm.Get_BDP30_0000(usr_code, prg_code, view_code + "_D1");
            ViewBag.colWidth_list = colWidth_list;
            ViewBag.colWidth_list_D1 = colWidth_list_D1;

            //取得欄位顯示
            List<BDP30_0100> is_show_list = comm.Get_BDP30_0100(usr_code, prg_code, view_code);
            List<BDP30_0100> is_show_D1_list = comm.Get_BDP30_0100(usr_code, prg_code, view_code + "_D1");
            ViewBag.is_show_list = is_show_list;
            ViewBag.is_show_D1_list = is_show_D1_list;
            return View();
        }

        public ActionResult Get_GridDataByQuery(string pWhere,string pReviewShowType)
        {
            string sUsrCode = User.Identity.Name;
            string sSql = "";
            DataTable dtTmp;

            List<EPB05_0000> list = new List<EPB05_0000>();
            list = repoEPB05_0000.Get_DataListByQuery(sUsrCode, PrgCode(), pWhere);

            //只顯示最新的審核紀錄
            List<string> NewestData = new List<string>();

            for (int i = 0; i < list.Count; i++) {
                string sEpbKey = list[i].report_group_code;

                sSql = "select top 1 * " +
                       "  from EPB05_0000 " +
                       " where report_group_code = '" + sEpbKey + "'" +
                       "  order by scr_no desc ";
                dtTmp = comm.Get_DataTable(sSql);
                if (dtTmp.Rows.Count > 0) {
                    NewestData.Add(dtTmp.Rows[0]["epb05_0000"].ToString());
                }
            }
            list = list.Where(x => NewestData.Contains(x.epb05_0000)).ToList();


            //資料顯示條件(複選)
            //SQL語法:跟使用者有關的所有審核資料
            //10.未審:順位還沒有到使用者
            //20.應審:順位已到使用者
            //30.已審:使用者已經審核過該資料，但該資料還沒有結案
            //40.已結案:資料已結案
            List<string> sql_in_list = new List<string>();

            if (!string.IsNullOrEmpty(pReviewShowType)) {                
                for (int i = 0;i < pReviewShowType.Split(',').Length;i++) {
                    string sShowType = pReviewShowType.Split(',')[i];
                    
                    DataTable dt = new DataTable();

                    //跟使用者有關的資料
                    sSql = "select report_group_code " +
                           "  from EPB05_0000 " +
                           "   left join EPB04_0100 on EPB05_0000.review_code = EPB04_0100.review_code " +
                           " where EPB04_0100.usr_code = '" + sUsrCode + "'" +
                           "   or (scr_no = '1' and EPB05_0000.usr_code = '" + sUsrCode + "')" +
                           "  group by report_group_code ";
                    dtTmp = comm.Get_DataTable(sSql);
                    for (int u = 0; u < dtTmp.Rows.Count; u++)
                    {
                        string sEpbKey = dtTmp.Rows[u]["report_group_code"].ToString();

                        switch (sShowType)
                        {
                            case "10":
                                //10.未審:使用者尚未審核通過此資料，但順位還沒有到使用者
                                if (!RpRv.Chk_IsReview(sEpbKey, sUsrCode) && RpRv.Get_UsrFinalReviewData(sEpbKey, sUsrCode, "is_ok") != "P")
                                {
                                    sql_in_list.Add(sEpbKey);
                                }
                                break;
                            case "20":
                                //20.應審:使用者尚未審核通過此資料，且順位已到使用者
                                if (!RpRv.Chk_IsReview(sEpbKey, sUsrCode) && RpRv.Get_UsrFinalReviewData(sEpbKey, sUsrCode, "is_ok") == "P")
                                {
                                    sql_in_list.Add(sEpbKey);
                                }
                                break;
                            case "30":
                                //30.已審:使用者已經審核過該資料，但該資料還沒有結案
                                if (RpRv.Chk_IsReview(sEpbKey, sUsrCode) && !RpRv.Chk_ReviewIsEnd(sEpbKey))
                                {
                                    sql_in_list.Add(sEpbKey);
                                }
                                break;
                            case "40":
                                if (RpRv.Chk_ReviewIsEnd(sEpbKey))
                                {
                                    sql_in_list.Add(sEpbKey);
                                }
                                break;
                        }
                    }                    
                }                
            }
            list = list.Where(x => sql_in_list.Contains(x.report_group_code)).ToList();

            list = list.OrderByDescending(x => x.ins_date).ThenByDescending(x => x.ins_time).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        


        public ActionResult Report(string K)
        {
            string sReportCode = comm.Get_Data("RSS03_0000", K, "report_group_code", "report_code");
            string sEpbCode = comm.Get_QueryData("RSS02_0000", sReportCode, "report_code", "epb_code");
            //string sEpbCode = comm.Get_QueryData("EPB03_0000", K, "field_value", "epb_code");
            ViewBag.ScrNo = K; //生成碼
            ViewBag.Key = sEpbCode; //表單代號

            //若使用者有相關審核資料，則點擊連結進入時，清除通知
            RpRv.TodoList_Ok(K, User.Identity.Name);

            return View();
        }

        [HttpPost]
        public ActionResult Report(FormCollection form)
        {
            string sReportGroupCode = comm.sGetString(form["ExportToExcel"]);            
            string sReportType = comm.Get_Data("RSS03_0000", sReportGroupCode, "report_group_code", "report_type");                        
            bool bIsReview = RpRv.Chk_ReportGroupIsPass(sReportGroupCode);

            switch (sReportType) {
                case "A":
                    return RedirectToAction("Export_ReportGroup", "Export", new { pReportGroupCode = sReportGroupCode, pIsReview = bIsReview });
                case "B":
                    //從/Upload/ReportTmp 取出報表 並且填上審核人員                  
                    IWorkbook excel;
                    excel = Exp.Get_Excel(sReportGroupCode, Server.MapPath("~/Upload/ReportTmp/"));

                    object ReplaceStr = new object();
                    string sReportCode = comm.Get_Data("RSS03_0000", sReportGroupCode, "report_group_code", "report_code");
                    string sReviewCode = comm.Get_Data("EPB04_0000", sReportCode, "report_code", "review_code");
                    string sUsrCode = comm.Get_Data("RSS03_0000", sReportGroupCode, "report_group_code", "usr_code");
                   
                    //如果審核通過才取代
                    if (bIsReview) {
                        ReplaceStr = new
                        {
                            review_level = sReviewCode,
                            usr_code = sUsrCode,
                        };
                    }
                    
                    excel = Exp.CodeReplace(excel, ReplaceStr);
                    Download(excel, sReportCode + ".xls");
                    return new EmptyResult();
            }
            ViewBag.ScrNo = sReportGroupCode; //生成碼
            return View();
        }


        public string Sql_EPB050A(string K) {
            string sSql = "select scr_no, usr_code as review_level, usr_code, usr_code as dut_code, out_date, out_time, result_code, review_memo " + 
                          "  from EPB05_0000" +
                          " where report_group_code = '" + K + "'" +
                          "  order by scr_no";
            return sSql;
        }

        /// <summary>
        /// 電子表單審核作業 審核紀錄
        /// </summary>
        /// <param name="K"></param>
        /// <returns></returns>
        public string Get_GridData(string K) {           
            var dtTmp = Get_DataTable_Value(K);            
            return JsonConvert.SerializeObject(dtTmp);
        }


        //審核當前資料，並且新增下一位待審人員及發待辦事項
        public void Review(string K,string Result, string Review_memo,string IsNext) {
            //K:報表群組碼

            string sIsNext = IsNext;
            string sResult = Result;

            //如果該人員為最後一個層級
            //則審核通過時為決行通過
            if (RpRv.Get_UsrIsFinalLevel(RpRv.Get_ReviewCode(K), User.Identity.Name))
                if (sResult == "01") {
                    sResult = "99";
                    sIsNext = "N";
                }

            //做出審核         
            RpRv.Get_Review(K, sResult, Review_memo, User.Identity.Name);

            if (sIsNext == "Y") {
                //新增下一位待審人員
                RpRv.Ins_ReviewByReportGroup(K, RpRv.Get_NextUser(K, User.Identity.Name, sResult));       
            }

            //如果審核結果為99或98，且還有更高審核層級的人員未審核
            //則通知更高審核層級的人員，該筆資料已決行
            if (sResult == "98" || sResult == "99") {
                string sHighUsrArray = RpRv.Get_HigherReviewUsr(K);
                if (!string.IsNullOrEmpty(sHighUsrArray)) {
                    object data = new object();
                    string sRptCode = comm.Get_Data("RSS03_0000", K, "report_group_code", "report_code");
                    string sRptName = comm.Get_Data("RSS02_0000", sRptCode, "report_code", "report_name");

                    for (int i = 0; i < sHighUsrArray.Split(',').Length; i++)
                    {
                        string sHighUsr = sHighUsrArray.Split(',')[i];

                        data = new
                        {
                            todo_code = comm.Get_TkCode("ToDoList"),
                            todo_name = "您有與您相關的審核資料已被決行結案，點擊了解詳情，報表名稱:" + sRptName,
                            todo_url = "/EPB050A/Report?K=",
                            todo_key = K,
                            is_use = "Y",
                            is_ok = "N",
                            usr_code = sHighUsr,
                        };
                        DT.InsertData("BDP16_0000", data);
                    }
                }                
            }


        }

        /// <summary>
        /// 資料表的值
        /// </summary>
        /// <param name="K">電子表單生成碼</param>
        /// <returns></returns>
        public DataTable Get_DataTable_Value(string K) {
            var dtTmp = comm.Get_DataTable(Sql_EPB050A(K));
            
            for (int r = 0; r < dtTmp.Rows.Count; r++) {
                DataRow Row = dtTmp.Rows[r];
                string TableField = DT.Get_SqlField(dtTmp);            
                for (int i = 0; i < TableField.Split(',').Length; i++) {
                    string sField = TableField.Split(',')[i]; //欄位
                    string sValue = Row[sField].ToString();  //欄位的值

                    switch (sField) {
                        case "review_level":
                            Row[sField] = Get_CodeName(sField, RpRv.Get_UsrReviewLevel(RpRv.Get_ReviewCode(K), sValue));
                            break;
                        case "dut_code":
                            Row[sField] = GD.Get_DutName(sValue);
                            break;
                        case "result_code":
                            Row[sField] = Get_CodeName(sField, sValue);
                            break;
                        case "is_ok":
                            Row[sField] = Get_CodeName(sField, sValue);
                            break;
                        case "epb_code":
                            Row[sField] = Row[sField] + " - " + comm.Get_QueryData("EPB02_0000", sValue, "epb_code","epb_name");
                            break;
                        case "usr_code":
                            Row[sField] = Row[sField] + " - " + comm.Get_QueryData("BDP08_0000", sValue, "usr_code", "usr_name");
                            break;
                        case "next_usr_code":
                            Row[sField] = Row[sField] + " - " + comm.Get_QueryData("BDP08_0000", sValue, "usr_code", "usr_name");
                            break;
                    }
                }
            }                      
            return dtTmp;
        }





        /// <summary>
        /// 取得代號的代號名稱
        /// </summary>
        /// <param name="pCode">欄位</param>
        /// <param name="pCodeValue">代號</param>
        /// <returns></returns>
        public string Get_CodeName(string pCode, string pCodeValue) {
            string sValue = "";
            switch (pCode) {
                case "review_level":
                    switch (pCodeValue)
                    {
                        case "0":
                            sValue = "打單人員";
                            break;
                        case "1":
                            sValue = "層級一";
                            break;
                        case "2":
                            sValue = "層級二";
                            break;
                        case "3":
                            sValue = "層級三";
                            break;
                        case "4":
                            sValue = "層級四";
                            break;
                        case "5":
                            sValue = "層級五";
                            break;
                        default:
                            break;
                    }
                    break;
                case "result_code":
                    switch (pCodeValue) {
                        case "01":
                            sValue = "審核通過";
                            break;
                        case "02":
                            sValue = "審核退回";
                            break;
                        case "99":
                            sValue = "決行通過";
                            break;
                        case "98":
                            sValue = "決行不通過";
                            break;
                        default:
                            break;
                    }
                    break;
                case "is_ok":
                    switch (pCodeValue)
                    {
                        case "P":
                            sValue = "尚未處理";
                            break;
                        case "Y":
                            sValue = "已處理";
                            break;
                        case "N":
                            sValue = "不用處理";
                            break;
                    }
                    break;
                default:
                    break;
            }
            return sValue;
        }



        public string Get_FieldName(string pFieldCode) {
            string sValue = "";
            switch (pFieldCode) {
                case "review_level":
                    sValue = "審核層級";
                    break;
                case "dut_code":
                    sValue = "職稱";
                    break;
                case "epb05_0000":
                    sValue = "識別碼";
                    break;
                case "review_code":
                    sValue = "審核設定代號";
                    break;
                case "epb_code":
                    sValue = "電子表單代號";
                    break;
                case "epb_key":
                    sValue = "電子表單資料鍵值";
                    break;
                case "result_code":
                    sValue = "審核結果";
                    break;
                case "is_ok":
                    sValue = "處理標記";
                    break;
                case "scr_no":
                    sValue = "序號";
                    break;
                case "ins_date":
                    sValue = "建立日期";
                    break;
                case "ins_time":
                    sValue = "建立時間";
                    break;
                case "usr_code":
                    sValue = "使用者編號";
                    break;
                case "out_date":
                    sValue = "審核日期";
                    break;
                case "out_time":
                    sValue = "審核時間";
                    break;
                case "next_usr_code":
                    sValue = "下一級應審人員";
                    break;
                case "review_memo":
                    sValue = "審核意見";
                    break;
                default:
                    break;
            }
            return sValue;
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

        public void Set_Cookie()
        {
            //紀錄cookie
            if (Request.Cookies["ReviewType"] == null)
            {
                HttpCookie Cookie = new HttpCookie("ReviewType")
                {
                    Value = "",
                    Expires = DateTime.Now.AddDays(1d),
                };
                Response.Cookies.Add(Cookie);
            }            
        }


        public void Download(IWorkbook excel, string pRptName)
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