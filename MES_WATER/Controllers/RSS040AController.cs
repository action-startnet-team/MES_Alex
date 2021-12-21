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
    public class RSS040AController : Controller
    {
        Comm comm = new Comm();
        GetData GD = new GetData();
        DynamicTable DT = new DynamicTable();
        CheckData CD = new CheckData();
        ExportController Exp = new ExportController();
        Review RV = new Review();
        ReportReview RpRv = new ReportReview();

        // GET: RSS040A
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection form, HttpPostedFileBase file)
        {
            object data = new object();
            
            string sReportCode = comm.sGetString(form["report_code"]);
            string sTkCode = RpRv.Get_ReportGroupCode(sReportCode);

            //將上傳的範本放在/Upload/ReportTmp，命名為選擇的報表代號
            UploadFile(file, sTkCode, Server.MapPath("~/Upload/ReportTmp/"));

            //新增出一組報表群組碼
            data = new
            {
                report_group_code = sTkCode,
                report_code = sReportCode,
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










        private void UploadFile(HttpPostedFileBase file, string pReportCode,string pFileLocation)
        {
            if (Request.Files["file"].ContentLength > 0)
            {
                string extension =
                    System.IO.Path.GetExtension(file.FileName);

                if (extension == ".xls" || extension == ".xlsx")
                {
                    string fileLocation = pFileLocation + pReportCode + extension;
                    if (System.IO.File.Exists(fileLocation)) // 驗證檔案是否存在
                    {
                        System.IO.File.Delete(pFileLocation + pReportCode + ".xls");
                        System.IO.File.Delete(pFileLocation + pReportCode + ".xlsx");
                    }
                    Request.Files["file"].SaveAs(fileLocation); // 存放檔案到伺服器上
                }
            }
        }

        //public bool Chk_UsrIsReviewerOfReport(string pReportCode)
        //{
        //    return RpRv.Chk_UsrIsReviewerOfReport(pReportCode, User.Identity.Name);
        //}

        public string Get_ReviewUser(string pReviewCode)
        {
            return RpRv.Get_ReviewUser(pReviewCode);
        }



    }
}