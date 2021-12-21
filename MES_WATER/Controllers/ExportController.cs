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
using ZXing;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using Font = System.Drawing.Font;

namespace MES_WATER.Controllers
{
    public class ExportController : Controller
    {
        Comm comm = new Comm();
        GetData GD = new GetData();
        DynamicTable DT = new DynamicTable();
        CheckData CD = new CheckData();


        public ActionResult Export_Routing_Multi(string pRptCode, string pMoCodeList)
        {
            string sSql = @"
                    select mo_code,wrk_code,wrk_date,c.work_code,c.work_name,a.pro_code,b.pro_name,pro_qty ,a.pro_unit from MET03_0000 a 
                    left join MEB20_0000 b on a.pro_code =b.pro_code 
                    left join MEB30_0000 c on a.work_code = c.work_code
                    where mo_code ='" + pMoCodeList + "' ";
            DataTable dtTmp = comm.Get_DataTable(sSql); 
            //document model
            MemoryStream workStream = new MemoryStream();
            Document document = new Document();
            PdfWriter.GetInstance(document, workStream).CloseStream = false; //writer pdf
            //PDF  SET-----------------------------
            PdfPTable table = new PdfPTable(4); //set pdftable
            table.TotalWidth = 400;
            BaseFont chBaseFont = BaseFont.CreateFont("c:\\windows\\fonts\\KAIU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED); //font set
            iTextSharp.text.Font helloFont = new iTextSharp.text.Font(chBaseFont, 14);//font set size
            Paragraph pa = new Paragraph();
            QRCodeEncoder chtEncoder = new QRCodeEncoder();
            
            //-------------------------------//
            if (dtTmp.Rows.Count > 0)
            {
                document.Open();
                for (int i = 0; i < dtTmp.Rows.Count; i++)
                {
                    string wrk_date = dtTmp.Rows[i]["wrk_date"].ToString();
                    string pro_code = dtTmp.Rows[i]["pro_code"].ToString();
                    string pro_name = dtTmp.Rows[i]["pro_name"].ToString();
                    string wrk_code = dtTmp.Rows[i]["wrk_code"].ToString();
                    string pro_unit = dtTmp.Rows[i]["pro_unit"].ToString();
                    string work_code = dtTmp.Rows[i]["work_code"].ToString();
                    string work_name =  dtTmp.Rows[i]["work_name"].ToString();
                    string pro_qty = dtTmp.Rows[i]["pro_qty"].ToString();
                    //----第三方先產生bitmap----
                    System.Drawing.Bitmap qrBitmap1 = chtEncoder.Encode(wrk_code); //將內容轉碼成 QR code            
                    //再將bitmap轉換成iTextSharp.text.Image
                    iTextSharp.text.Image qrImage1 = iTextSharp.text.Image.GetInstance(qrBitmap1, BaseColor.BLACK);
                    qrImage1.ScalePercent(50);
                    qrImage1.Alignment = Element.ALIGN_CENTER;
                                                              
                    CreateTable(pMoCodeList, table, helloFont,"製令號碼");
                    PdfPCell cell_2 = new PdfPCell(new Paragraph("派工號碼　"+ wrk_code, helloFont));
                    cell_2.Colspan = 1;
                    table.AddCell(cell_2);
                    PdfPCell qr_code = new PdfPCell(qrImage1);
                    
                    qr_code.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(qr_code );

                    CreateTable(wrk_date, table, helloFont, "工單日期");
                    CreateTable(work_code+" "+work_name, table, helloFont, "製程");
                    CreateTable(pro_code, table, helloFont, "產品編號");
                    CreateTable(pro_name, table, helloFont, "產品名稱");
                    CreateTable(pro_qty, table, helloFont, "生產數量");
                    CreateTable(pro_unit, table, helloFont, "單位");

                    //document.Add(new Paragraph("製令號碼　:" + pMoCodeList, helloFont));//加入內容
                    //document.Add(new Paragraph("派工號碼  :" + wrk_code, helloFont));//加入內容
                    //document.Add(new Paragraph("工單日期　:" + wrk_date, helloFont));//加入內容
                    //document.Add(new Paragraph("產品編號　:" + pro_code, helloFont));//加入內容
                    //document.Add(new Paragraph("生產數量　:" + pro_qty, helloFont));//加入內容
                    //document.Add(new Paragraph("單位　    :" + pro_unit, helloFont));//加入內容
                    //document.Add(new Paragraph("製程代碼　:" + work_code, helloFont));//加入內容
                    //document.Add(new Paragraph("工單條碼　:", helloFont));//加入內容
                    document.Add(table);
                    //document.Add(pa);
                    document.Add(new Paragraph("\n\n"));
                    //pa.Clear();
                    table.DeleteBodyRows();
                    // document.Add(new Paragraph(DateTime.Now.ToString()));//加入內容
                }
                //加入內容
                document.Close();

                byte[] byteInfo = workStream.ToArray();
                workStream.Write(byteInfo, 0, byteInfo.Length);
                workStream.Position = 0;
                Response.Buffer = true;
                Response.AddHeader("Content-Disposition", "attachment; filename= " + Server.HtmlEncode(pMoCodeList + ".pdf")); //檔案名稱
                Response.ContentType = "APPLICATION/pdf";
                Response.BinaryWrite(byteInfo);
                //return new FileStreamResult(workStream, "application/pdf");

            }
            return new EmptyResult();
        }

        private static void CreateTable(string pMoCodeList, PdfPTable table, iTextSharp.text.Font helloFont , string title_name)
        {
            PdfPCell cell_1 = new PdfPCell(new Paragraph(title_name, helloFont));
            cell_1.Colspan = 1;
            table.AddCell(cell_1);
            PdfPCell pMoCode = new PdfPCell(new Paragraph(pMoCodeList, helloFont));
            table.AddCell(pMoCode);
        }

        public ActionResult ExportByDataTable_QMT(string pTkCode)
        {
            string sSql = "";
            DataTable dtTmp = new DataTable();

            XSSFWorkbook workbook;
            using (FileStream fs = new FileStream(Server.MapPath("~/Upload/Report/") + "B014-1.xlsx", FileMode.Open, FileAccess.Read))
            {
                workbook = new XSSFWorkbook(fs);
            }
            ISheet Sheet_Table = workbook.GetSheetAt(1);
            IRow Row_Table = Sheet_Table.GetRow(0);
            ICell[] Cell = new ICell[Row_Table.LastCellNum];

            //表頭
            sSql = "select QMT04_0000.ins_date,FORMAT(WMT0200.sto_date,'yyyy/MM/dd') as sto_date,QMT04_0000.rel_code,WMT0200.tra_code,QMT04_0000.pro_code,pro_name,WMT0200.sor_no,WMT0200.pro_qty,QMT04_0000.is_rec " +
                   "  from QMT04_0000 " +
                   "  left join WMT0200 on QMT04_0000.wmt0200 = WMT0200.wmt0200 " +
                   "  left join MEB20_0000 on QMT04_0000.pro_code = MEB20_0000.pro_code" +
                   " where qmt_code = @qmt_code";
            dtTmp = comm.Get_DataTable(sSql, "qmt_code", pTkCode);
            if (dtTmp.Rows.Count > 0)
            {
                DataRow r = dtTmp.Rows[0];
                IRow Row = Sheet_Table.CreateRow(1);

                //照順序直接灌進表頭
                for (int i = 0; i < dtTmp.Columns.Count; i++)
                {
                    if (Row.GetCell(i) == null)
                    {
                        Cell[i] = Row.CreateCell(i);
                    }
                    else
                    {
                        Cell[i] = Row.GetCell(i);
                    }
                    string sField = dtTmp.Columns[i].ToString();
                    switch (sField)
                    {
                        case "is_rec":
                            dtTmp.Columns[sField].MaxLength = 10;
                            switch (r[sField].ToString())
                            {
                                case "P":
                                    r[sField] = "尚未判定";
                                    break;
                                case "Y":
                                    r[sField] = "允收";
                                    break;
                                case "N":
                                    r[sField] = "拒收";
                                    break;
                            }
                            break;
                    }
                    Cell[i].SetCellValue(r[sField].ToString());
                }
            }

            ISheet Sheet2 = workbook.GetSheetAt(2);
            IRow Row_title2 = Sheet2.GetRow(0);
            ICell[] Cell2 = new ICell[Row_title2.LastCellNum];

            //檢驗項目-尺寸
            sSql = "select QMT04_0100.*,qtest_item_name " +
                   "  from QMT04_0100" +
                   "  left join QMB02_0000 on QMT04_0100.qtest_item_code = QMB02_0000.qtest_item_code" +
                   " where qmt_code = @qmt_code" +
                   "   and QMT04_0100.qtest_item_type = 'B'";
            dtTmp = comm.Get_DataTable(sSql, "qmt_code", pTkCode);
            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                IRow Row;
                if (Sheet2.GetRow(i + 1) == null)
                {
                    Row = Sheet2.CreateRow(i + 1);
                }
                else
                {
                    Row = Sheet2.GetRow(i + 1);
                }

                DataRow r = dtTmp.Rows[i];
                string qmt04_0100 = r["qmt04_0100"].ToString();
                string sQtestItemCode = r["qtest_item_code"].ToString();
                string sQtestItemName = r["qtest_item_name"].ToString();
                string sQtestItemType = r["qtest_item_type"].ToString();
                string sIsOk = "";

                //dtTmp.Columns["is_ok"].MaxLength = 10;
                //switch (sIsOk)
                //{                   
                //    case "P":
                //        sIsOk = "尚未判定";
                //        break;
                //    case "Y":
                //        sIsOk = "允收";
                //        break;
                //    case "N":
                //        sIsOk = "拒收";
                //        break;
                //}

                //Row.CreateCell(0).SetCellValue(sQtestItemCode);
                Row.CreateCell(0).SetCellValue(sQtestItemName);
                Row.CreateCell(6).SetCellValue(sIsOk);

                //檢驗紀錄
                sSql = "select top 5 * " +
                       "  from QMT04_0110 " +
                       " where qmt04_0100 = @qmt04_0100";
                var dtTmp2 = comm.Get_DataTable(sSql, "qmt04_0100", qmt04_0100);
                for (int u = 0; u < dtTmp2.Rows.Count; u++)
                {
                    DataRow ur = dtTmp2.Rows[u];
                    int UCnt = u + 1;
                    if (Row.GetCell(UCnt) == null)
                    {
                        Cell2[UCnt] = Row.CreateCell(UCnt);
                    }
                    else
                    {
                        Cell2[UCnt] = Row.GetCell(UCnt);
                    }
                    Cell2[UCnt].SetCellValue(ur["qmt_value"].ToString());

                }
            }

            ISheet Sheet3 = workbook.GetSheetAt(3);
            IRow Row_title3 = Sheet3.GetRow(0);
            ICell[] Cell3 = new ICell[Row_title3.LastCellNum];

            //檢驗項目-外觀
            sSql = "select QMT04_0100.*,qtest_item_name " +
                   "  from QMT04_0100" +
                   "  left join QMB02_0000 on QMT04_0100.qtest_item_code = QMB02_0000.qtest_item_code" +
                   " where qmt_code = @qmt_code" +
                   "   and QMT04_0100.qtest_item_type = 'C'";
            dtTmp = comm.Get_DataTable(sSql, "qmt_code", pTkCode);
            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                IRow Row;
                if (Sheet3.GetRow(i + 1) == null)
                {
                    Row = Sheet3.CreateRow(i + 1);
                }
                else
                {
                    Row = Sheet3.GetRow(i + 1);
                }

                DataRow r = dtTmp.Rows[i];
                string qmt04_0100 = r["qmt04_0100"].ToString();
                string sQtestItemCode = r["qtest_item_code"].ToString();
                string sQtestItemName = r["qtest_item_name"].ToString();
                string sQtestItemType = r["qtest_item_type"].ToString();
                string sIsOk = "";

                //dtTmp.Columns["is_ok"].MaxLength = 10;
                //switch (sIsOk)
                //{
                //    case "P":
                //        sIsOk = "尚未判定";
                //        break;
                //    case "Y":
                //        sIsOk = "允收";
                //        break;
                //    case "N":
                //        sIsOk = "拒收";
                //        break;
                //}

                //Row.CreateCell(0).SetCellValue(sQtestItemCode);
                Row.CreateCell(0).SetCellValue(sQtestItemName);
                Row.CreateCell(6).SetCellValue(sIsOk);

                //檢驗紀錄
                sSql = "select top 5 * " +
                       "  from QMT04_0110 " +
                       " where qmt04_0100 = @qmt04_0100";
                var dtTmp2 = comm.Get_DataTable(sSql, "qmt04_0100", qmt04_0100);
                for (int u = 0; u < dtTmp2.Rows.Count; u++)
                {
                    DataRow ur = dtTmp2.Rows[u];
                    int UCnt = u + 1;
                    if (Row.GetCell(UCnt) == null)
                    {
                        Cell2[UCnt] = Row.CreateCell(UCnt);
                    }
                    else
                    {
                        Cell2[UCnt] = Row.GetCell(UCnt);
                    }

                    Cell2[UCnt].SetCellValue(ur["qmt_value"].ToString());

                }
            }
            for (int i = 1; i <= 3; i++)
            {
                workbook.SetSheetHidden(i, 1);
            }

            workbook.GetSheetAt(0).ForceFormulaRecalculation = true;

            Download(workbook, "B014-1.xlsx");
            return new EmptyResult();
        }




        /// <summary>
        /// 匯入範本，依照表單代號產出Excel
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

                excel = CodeReplace(excel);

                Download(excel, sRptCode + Path.GetExtension(fileLocation));
            }
            return new EmptyResult();
        }







        /// <summary>
        /// 匯入樞紐分析表，依照表單代號產出Excel
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public IWorkbook Export_ExcelByDataTable(string pRptCode, DataTable pDataTble, string pFileLocation)
        {
            DataTable dtTmp = null;
            string sRptCode = pRptCode;
            string sEtlCode = GD.Get_Data("RSS02_0000", sRptCode, "report_code", "etl_code");

            string fileLocation = pFileLocation + Get_SampleName(sRptCode, pFileLocation);
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

                //在資料表裡面 取用指定欄位的資料
                dtTmp = pDataTble;
                for (int i = 0; i < dtTmp.Rows.Count; i++)
                {
                    DataRow r = dtTmp.Rows[i];
                    if (sheet.GetRow(i + 1) == null)
                    {
                        Row_tmp = sheet.CreateRow(i + 1);
                    }
                    else
                    {
                        Row_tmp = sheet.GetRow(i + 1);
                    }

                    for (int u = 0; u < dtTmp.Columns.Count; u++)
                    {
                        DataColumn Col = dtTmp.Columns[u];
                        string sField = Col.ToString();
                        string sValue = r[sField].ToString();

                        if (Row_tmp.GetCell(u) == null)
                        {
                            Cell[u] = Row_tmp.CreateCell(u);  //建立單元格
                        }
                        else
                        {
                            Cell[u] = Row_tmp.GetCell(u);
                        }
                        Cell[u].SetCellValue(sValue);
                    }
                }
                //重新讀取公式
                ISheet sheet2 = excel.GetSheetAt(0);
                sheet2.ForceFormulaRecalculation = true;

                return excel;
            }
            return null;
        }



        /// <summary>
        /// 取得指定名稱與路徑的EXCEL，若沒取到則回傳NULL
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public IWorkbook Get_Excel(string pExcelName, string pServerPath)
        {
            string sFileLocation = pServerPath + Get_SampleName(pExcelName, pServerPath);
            if (System.IO.File.Exists(sFileLocation)) // 驗證檔案是否存在
            {
                IWorkbook excel;
                // 檔案讀取
                using (FileStream files = new FileStream(sFileLocation, FileMode.Open, FileAccess.Read))
                {
                    if (Path.GetExtension(sFileLocation).ToLower() == ".xls")
                    {
                        excel = new HSSFWorkbook(files);
                    }
                    else
                    {
                        excel = new XSSFWorkbook(files);
                    }
                }
                              
                //重新讀取公式
                ISheet sheet = excel.GetSheetAt(0);
                sheet.ForceFormulaRecalculation = true;

                return excel;
            }
            return null;
        }








        /// <summary>
        /// 選擇資料匯出
        /// </summary>
        /// <param name="pRptCode">報表代號</param>
        /// <param name="pValueArray">識別碼陣列</param>
        /// <returns></returns>
        public ActionResult Export_EPB(string pRptCode, string pValueArray,bool pIsReview = true)
        {
            string sSql = "";

            string fileLocation = Server.MapPath("~/Upload/Report/") + Get_SampleName(pRptCode);
            if (System.IO.File.Exists(fileLocation)) // 驗證檔案是否存在
            {
                // 建立一個工作簿
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
                IRow Row = (IRow)sheet.CreateRow(0);  //獲取Sheet1工作表的首行
                IRow Row_tmp;

                //先清除RowData
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    sheet.RemoveRow(sheet.GetRow(i));
                }

                sSql = "select * " +
                   "  from RSS02_0000 " +
                   " where report_code = '" + pRptCode + "'";
                string sEpbCode = GD.DataFieldToStr(sSql, "epb_code");


                sSql = "select * " +
                       "  from EPB02_0100 " +
                       " where epb_code = '" + sEpbCode + "' " +
                       "   and is_key <> 'Y' " +
                       "  order by scr_no ";
                var dtTmp = comm.Get_DataTable(sSql);
                if (dtTmp.Rows.Count > 0)
                {
                    ICell[] Cell = new ICell[dtTmp.Rows.Count];
                    for (int i = 0; i < Cell.Count(); i++)
                    {
                        string sFieldName = dtTmp.Rows[i]["field_name"].ToString();
                        Cell[i] = (ICell)Row.CreateCell(i);  //建立單元格
                        Cell[i].SetCellValue(sFieldName); //賦值為字串

                        string sValueArray = pValueArray;
                        if (string.IsNullOrEmpty(sValueArray)) { sValueArray = ""; }
                        sSql = "select * " +
                                "  from EPB03_0000 " +
                                " where epb_code = '" + dtTmp.Rows[i]["epb_code"].ToString() + "' " +
                                "   and field_code = '" + dtTmp.Rows[i]["field_code"].ToString() + "' " +
                                "   and key_value in (" + GD.StrArrayToSql(sValueArray) + ")" +
                                "  order by charindex(key_value,'" + sValueArray + "')";
                        var dtTmp2 = comm.Get_DataTable(sSql);
                        for (int u = 0; u < dtTmp2.Rows.Count; u++)
                        {
                            string sValue = dtTmp2.Rows[u]["field_value"].ToString();

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
                //重新讀取公式
                sheet.ForceFormulaRecalculation = true;
                ISheet sheet2 = excel.GetSheetAt(0);
                sheet2.ForceFormulaRecalculation = true;

                string sReviewCode = comm.Get_QueryData("EPB04_0000", sEpbCode, "epb_code", "review_code");

                //審核
                if (pIsReview)
                {
                    excel = CodeReplace(excel, sReviewCode);
                }
                else {
                    excel = CodeReplace(excel,"");
                }
                
                Download(excel, pRptCode + Path.GetExtension(fileLocation));

            }
            return new EmptyResult();
        }




        /// <summary>
        /// 選擇資料匯出
        /// </summary>
        /// <param name="pRptCode">報表代號</param>
        /// <param name="pValueArray">識別碼陣列</param>
        /// <returns></returns>
        public ActionResult Export_ReportGroup(string pReportGroupCode,bool pIsReview = true)
        {
            string sSql = "";
            string sRptCode = Get_ReportGroupData(pReportGroupCode, "report_code");
            string fileLocation = Server.MapPath("~/Upload/Report/") + Get_SampleName(sRptCode);
            if (System.IO.File.Exists(fileLocation)) // 驗證檔案是否存在
            {
                // 建立一個工作簿
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
                IRow Row = (IRow)sheet.CreateRow(0);  //獲取Sheet1工作表的首行
                IRow Row_tmp;

                //先清除RowData
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    sheet.RemoveRow(sheet.GetRow(i));
                }

                sSql = "select * " +
                   "  from RSS02_0000 " +
                   " where report_code = '" + sRptCode + "'";
                string sEpbCode = GD.DataFieldToStr(sSql, "epb_code");


                sSql = "select * " +
                       "  from EPB02_0100 " +
                       " where epb_code = '" + sEpbCode + "' " +
                       "   and is_key <> 'Y' " +
                       "  order by scr_no ";
                var dtTmp = comm.Get_DataTable(sSql);
                if (dtTmp.Rows.Count > 0)
                {
                    ICell[] Cell = new ICell[dtTmp.Rows.Count];
                    for (int i = 0; i < Cell.Count(); i++)
                    {
                        string sFieldName = dtTmp.Rows[i]["field_name"].ToString();
                        Cell[i] = (ICell)Row.CreateCell(i);  //建立單元格
                        Cell[i].SetCellValue(sFieldName); //賦值為字串

                        string sValueArray = GD.Get_Data("RSS03_0100", pReportGroupCode,"report_group_code","epb_key_value");
                        if (string.IsNullOrEmpty(sValueArray)) { sValueArray = ""; }
                        sSql = "select * " +
                                "  from EPB03_0000 " +
                                " where epb_code = '" + dtTmp.Rows[i]["epb_code"].ToString() + "' " +
                                "   and field_code = '" + dtTmp.Rows[i]["field_code"].ToString() + "' " +
                                "   and key_value in (" + GD.StrArrayToSql(sValueArray) + ")" +
                                "  order by charindex(key_value,'" + sValueArray + "')";
                        var dtTmp2 = comm.Get_DataTable(sSql);
                        for (int u = 0; u < dtTmp2.Rows.Count; u++)
                        {
                            string sValue = dtTmp2.Rows[u]["field_value"].ToString();

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
                //重新讀取公式
                sheet.ForceFormulaRecalculation = true;
                ISheet sheet2 = excel.GetSheetAt(0);
                sheet2.ForceFormulaRecalculation = true;

                string sReviewCode = comm.Get_QueryData("EPB04_0000", sEpbCode, "epb_code", "review_code");
                string sReportGroupUsr = Get_ReportGroupData(pReportGroupCode, "usr_code");

                object data = new object();
                data = new {
                    review_level = sReviewCode,
                    usr_code = sReportGroupUsr,
                };

                //審核
                if (pIsReview)
                {
                    excel = CodeReplace(excel, data);
                }
                else
                {
                    excel = CodeReplace(excel,new object());
                }

                Download(excel, sRptCode + Path.GetExtension(fileLocation));

            }
            return new EmptyResult();
        }









        /// <summary>
        /// 套表取代代號
        /// </summary>
        /// <param name="excel"></param>
        /// <param name="pCode">代號的函式鍵值</param>
        /// <returns></returns>
        public IWorkbook CodeReplace(IWorkbook excel, string pCode = "")
        {
            //自定義取代符號
            string SM = "[";
            string EM = "]";
            char SPM = '[';

            string sValue = "";
            DataTable dtTmp = new DataTable();
            ISheet sheet = excel.GetSheetAt(0);
            IRow Rtmp;
            ICell[] Ctmp;
            //先檢查是否有替換符號 [ ] 中間的值為欄位名稱
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                if ((IRow)sheet.GetRow(i) == null)
                {
                    Rtmp = (IRow)sheet.CreateRow(i);
                    Ctmp = new ICell[0];
                }
                else
                {
                    Rtmp = (IRow)sheet.GetRow(i);

                    //會出現Rtmp.LastCellNum不等於null 但是是-1的狀況
                    if (Rtmp.LastCellNum >= 0)
                    {
                        Ctmp = new ICell[Rtmp.LastCellNum];
                    }
                    else
                    {
                        Ctmp = new ICell[0];
                    }
                }

                for (int u = 0; u < Ctmp.Count(); u++)
                {
                    if ((ICell)Rtmp.GetCell(u) == null)
                    {
                        Ctmp[u] = (ICell)Rtmp.CreateCell(u);
                    }
                    else
                    {
                        Ctmp[u] = (ICell)Rtmp.GetCell(u);
                    }

                    string sCstr = Ctmp[u].ToString();

                    if (sCstr.Contains(SM) && sCstr.Contains(EM))
                    {
                        //檢查一個cell裡面出現幾個[ ]
                        int MarkCnt = sCstr.Split(SPM).Length - 1;

                        //第2次之後從上次結束標記開始找
                        int endtmp = 0; //上次結束位置
                        string NewStr = sCstr; //要取代的字串
                        for (int n = 0; n < MarkCnt; n++)
                        {
                            string CtrTmp = sCstr;
                            if (n > 0) { CtrTmp = sCstr.Substring(endtmp + 1); }

                            int s_mark = CtrTmp.IndexOf(SM);//開始標記
                            int e_mark = CtrTmp.IndexOf(EM);//結束標記

                            //找到的字串
                            string sField = CtrTmp.Substring(s_mark + 1, e_mark - s_mark - 1);

                            //根據處理不同代號用不同的函式
                            if (sField.Contains("review_level"))
                            {
                                sValue = ReplaceStr_Review(pCode, sField);
                            }
                            else if (sField.Contains("usr_code"))
                            {
                                sValue = User.Identity.Name + " - " + GD.Get_Data("BDP08_0000", User.Identity.Name, "usr_code", "usr_name");
                            }
                            else
                            {
                                sValue = pCode;
                            }

                            if (pCode == "") { sValue = ""; }

                            //如果沒有取到值，則給空值
                            NewStr = NewStr.Replace(SM + sField + EM, sValue);
                            Ctmp[u].SetCellValue(NewStr);
                            endtmp = e_mark;
                        }
                    }
                }
            }
            return excel;
        }



        /// <summary>
        /// 套表取代代號
        /// </summary>
        /// <param name="excel"></param>
        /// <param name="pCode">代號的函式鍵值</param>
        /// <returns></returns>
        public IWorkbook CodeReplace(IWorkbook excel, object pObject)
        {
            //自定義取代符號
            string SM = "[";
            string EM = "]";
            char SPM = '[';

            string sValue = "";
            DataTable dtTmp = new DataTable();
            ISheet sheet = excel.GetSheetAt(0);
            IRow Rtmp;
            ICell[] Ctmp;
            //先檢查是否有替換符號 [ ] 中間的值為欄位名稱
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                if ((IRow)sheet.GetRow(i) == null)
                {
                    Rtmp = (IRow)sheet.CreateRow(i);
                    Ctmp = new ICell[0];
                }
                else
                {
                    Rtmp = (IRow)sheet.GetRow(i);

                    //會出現Rtmp.LastCellNum不等於null 但是是-1的狀況
                    if (Rtmp.LastCellNum >= 0)
                    {
                        Ctmp = new ICell[Rtmp.LastCellNum];
                    }
                    else
                    {
                        Ctmp = new ICell[0];
                    }
                }

                for (int u = 0; u < Ctmp.Count(); u++)
                {
                    if ((ICell)Rtmp.GetCell(u) == null)
                    {
                        Ctmp[u] = (ICell)Rtmp.CreateCell(u);
                    }
                    else
                    {
                        Ctmp[u] = (ICell)Rtmp.GetCell(u);
                    }

                    string sCstr = Ctmp[u].ToString();

                    if (sCstr.Contains(SM) && sCstr.Contains(EM))
                    {
                        //檢查一個cell裡面出現幾個[ ]
                        int MarkCnt = sCstr.Split(SPM).Length - 1;

                        //第2次之後從上次結束標記開始找
                        int endtmp = 0; //上次結束位置
                        string NewStr = sCstr; //要取代的字串
                        for (int n = 0; n < MarkCnt; n++)
                        {
                            string CtrTmp = sCstr;
                            if (n > 0) { CtrTmp = sCstr.Substring(endtmp + 1); }

                            int s_mark = CtrTmp.IndexOf(SM);//開始標記
                            int e_mark = CtrTmp.IndexOf(EM);//結束標記

                            //找到的字串
                            string sField = CtrTmp.Substring(s_mark + 1, e_mark - s_mark - 1);

                            //根據處理不同代號用不同的函式-------------------------------------
                            if (sField.Contains("review_level"))
                            {
                                sValue = ReplaceStr_Review(GD.Get_ObjectValue(pObject, "review_level"), sField);
                            }
                            else if (sField.Contains("usr_code"))
                            {                                
                                sValue = GD.Get_ObjectValue(pObject, "usr_code") + " - " + GD.Get_Data("BDP08_0000", GD.Get_ObjectValue(pObject, "usr_code"), "usr_code", "usr_name");
                            }
                            else
                            {
                                sValue = "";
                            }
                            //----------------------------------------------------------------

                            //如果沒有取到值，則給空值
                            NewStr = NewStr.Replace(SM + sField + EM, sValue);
                            Ctmp[u].SetCellValue(NewStr);
                            endtmp = e_mark;
                        }
                    }
                }
            }
            return excel;
        }




        /// <summary>
        /// 審核設定取代
        /// </summary>
        /// <param name="pReviewCode"></param>
        /// <param name="String"></param>
        /// <returns></returns>
        public string ReplaceStr_Review(string pReviewCode, string String)
        {
            string sValue = "";
            string sReviedwLevel = comm.StrRigth(String, 1);
            string sSql = "select * from EPB04_0100" +
                           " where review_code = '" + pReviewCode + "' " +
                           "   and review_level = '" + sReviedwLevel + "'";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                string sUsrCode = dtTmp.Rows[0]["usr_code"].ToString();
                string sUsrName = comm.Get_QueryData("BDP08_0000", sUsrCode, "usr_code", "usr_name");
                sValue = sUsrCode + " - " + sUsrName;
            }
            return sValue;
        }


        public string Get_ReportGroupData(string pReportGroupCode,string pFieldCode) {
            return comm.Get_Data("RSS03_0000", pReportGroupCode, "report_group_code", pFieldCode);
        }




        ///// <summary>
        ///// 匯出常用字串:QRCode
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult Export_CommonStr(string Key)
        //{
        //    string sSql = "";

        //    // 建立一個工作簿
        //    HSSFWorkbook excel = new HSSFWorkbook();
        //    ISheet sheet = excel.CreateSheet("常用字串");
        //    sheet.DefaultColumnWidth = 25;

        //    sSql = "select * from EPB02_0200 " +
        //           " where epb02_0100 = '" + Key + "'";
        //    var dtTmp = comm.Get_DataTable(sSql);
        //    for (int i = 0; i < dtTmp.Rows.Count; i++)
        //    {                
        //        string sName = dtTmp.Rows[i]["option_name"].ToString();

        //        IRow Row = (IRow)sheet.CreateRow(i);
        //        Row.HeightInPoints = 100;
        //        ICell[] Cell = new ICell[3];
        //        Cell[0] = (ICell)Row.CreateCell(0);  
        //        Cell[0].SetCellValue(sName);
        //        Cell[0].CellStyle = Expot_Cs(excel);

        //        BarcodeWriter bw = new BarcodeWriter();
        //        bw.Format = BarcodeFormat.QR_CODE;
        //        // QR_CODE 的長寬 (px)
        //        bw.Options.Width = 120;
        //        bw.Options.Height = 120;
        //        bw.Options.Hints.Add(EncodeHintType.CHARACTER_SET, "utf-8");
        //        Bitmap bmp = bw.Write(sName);

        //        // 圖片
        //        int pictureIdx = excel.AddPicture(ImageToByte(bmp), PictureType.JPEG);
        //        IDrawing _IDrawing = sheet.CreateDrawingPatriarch();
        //        // 1.指定放在哪個儲存格
        //        IClientAnchor _IClientAnchor = _IDrawing.CreateAnchor(0, 0, 0, 0, 1, i, 1, i);
        //        // 2.把圖片放進去
        //        IPicture _IPicture = _IDrawing.CreatePicture(_IClientAnchor, pictureIdx);
        //        _IPicture.Resize();
        //        // 3.設定圖片在儲存格X.Y軸偏移的距離(這步驟一定要最後做喔)
        //        _IClientAnchor.Dx1 = 70;
        //        _IClientAnchor.Dy1 = 30;
        //    }       
        //    Download(excel, "常用字串列印.xls");

        //    return new EmptyResult();
        //}


        /// <summary>
        /// 匯出常用字串條碼
        /// </summary>
        /// <returns></returns>
        public ActionResult Export_CommonStr(string Key)
        {
            string sSql = "";
            string fileLocation = Server.MapPath("~/Upload/Sample/") + "Common_Str.xls";
            if (System.IO.File.Exists(fileLocation)) // 驗證檔案是否存在
            {
                // 建立一個工作簿
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

                ISheet sheet = excel.GetSheetAt(1);


                sSql = "select * from EPB02_0200 " +
                       " where epb02_0100 = '" + Key + "'";
                var dtTmp = comm.Get_DataTable(sSql);
                for (int i = 0; i < dtTmp.Rows.Count; i++)
                {
                    string sCode = dtTmp.Rows[i]["option_code"].ToString();
                    string sName = dtTmp.Rows[i]["option_name"].ToString();

                    IRow Row;
                    ICell[] Cell = new ICell[3];

                    if ((IRow)sheet.GetRow(i + 1) == null)
                    {
                        Row = (IRow)sheet.CreateRow(i + 1);
                    }
                    else
                    {
                        Row = (IRow)sheet.GetRow(i + 1);
                    }

                    if ((ICell)Row.GetCell(0) == null)
                    {
                        Cell[0] = (ICell)Row.CreateCell(0);  //建立單元格
                    }
                    else
                    {
                        Cell[0] = (ICell)Row.GetCell(0);
                    }
                    Cell[0].SetCellValue(sCode);

                    if ((ICell)Row.GetCell(1) == null)
                    {
                        Cell[1] = (ICell)Row.CreateCell(1);  //建立單元格
                    }
                    else
                    {
                        Cell[1] = (ICell)Row.GetCell(1);
                    }
                    Cell[1].SetCellValue(sName); //賦值為字串

                    if ((ICell)Row.GetCell(2) == null)
                    {
                        Cell[2] = (ICell)Row.CreateCell(2);  //建立單元格
                    }
                    else
                    {
                        Cell[2] = (ICell)Row.GetCell(2);
                    }
                    Cell[2].SetCellValue("*" + sCode + "*"); //賦值為字串
                }

                sheet.ForceFormulaRecalculation = true;
                ISheet sheet2 = excel.GetSheetAt(0);
                sheet2.ForceFormulaRecalculation = true;

                Download(excel, "常用字串列印.xls");
            }
            return new EmptyResult();
        }


        /// <summary>
        /// 取代套表型
        /// </summary>
        /// <param name="Key">識別碼</param>
        /// <returns></returns>
        public ActionResult Export_Sample(string Key)
        {
            string sSql = "";
            string SM = "[";
            string EM = "]";
            char SPM = '[';
            DataTable dtTmp = null;
            string sRptCode = GD.Get_Data("EPB03_0000", Key, "field_value", "epb_code");

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
                ISheet sheet = excel.GetSheetAt(0);

                //先檢查是否有替換符號 [ ] 中間的值為欄位名稱
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    IRow Rtmp = (IRow)sheet.GetRow(i);
                    ICell[] Ctmp = new ICell[Rtmp.LastCellNum];

                    for (int u = 0; u < Ctmp.Count(); u++)
                    {
                        Ctmp[u] = (ICell)Rtmp.GetCell(u);
                        string sCstr = Ctmp[u].ToString();

                        if (sCstr.Contains(SM) && sCstr.Contains(EM))
                        {
                            //檢查一個cell裡面出現幾個[ ]
                            int MarkCnt = sCstr.Split(SPM).Length - 1;

                            //第2次之後從上次結束標記開始找
                            int endtmp = 0;
                            string NewStr = sCstr;
                            for (int n = 0; n < MarkCnt; n++)
                            {
                                string CtrTmp = sCstr;
                                if (n > 0) { CtrTmp = sCstr.Substring(endtmp + 1); }

                                int s_mark = CtrTmp.IndexOf(SM);
                                int e_mark = CtrTmp.IndexOf(EM);
                                string sField = CtrTmp.Substring(s_mark + 1, e_mark - s_mark - 1);

                                sSql = "select * from EPB03_0000" +
                                       " where key_value = '" + Key + "' " +
                                       "   and field_code = '" + sField + "'";
                                dtTmp = comm.Get_DataTable(sSql);
                                if (dtTmp.Rows.Count > 0)
                                {
                                    string sValue = dtTmp.Rows[0]["field_value"].ToString();
                                    NewStr = NewStr.Replace(SM + sField + EM, sValue);
                                    Ctmp[u].SetCellValue(NewStr);

                                    endtmp = e_mark;
                                }
                            }
                        }
                    }
                }
                Download(excel, sRptCode + Path.GetExtension(fileLocation));
            }
            return new EmptyResult();
        }




        public IRow Get_Row(ISheet pSheet, int Index)
        {
            IRow irReturn;
            if (pSheet.GetRow(Index) == null)
            {
                irReturn = (IRow)pSheet.CreateRow(Index);
            }
            else
            {
                irReturn = (IRow)pSheet.GetRow(Index);
            }
            return irReturn;
        }

        public ICell Get_Cell(IRow pRow, int Index)
        {
            ICell[] icReturn = new ICell[pRow.LastCellNum];
            if ((ICell)pRow.GetCell(Index) == null)
            {
                icReturn[Index] = (ICell)pRow.CreateCell(Index);  //建立單元格
            }
            else
            {
                icReturn[Index] = (ICell)pRow.GetCell(Index);
            }
            return icReturn[Index];
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


        public ICellStyle Expot_Cs(IWorkbook excel)
        {
            IFont _IFont = (IFont)excel.CreateFont();
            _IFont.FontHeightInPoints = 20;

            ICellStyle cs = (ICellStyle)excel.CreateCellStyle();
            cs.VerticalAlignment = VerticalAlignment.Center;
            cs.Alignment = HorizontalAlignment.Center;
            cs.SetFont(_IFont);

            return cs;
        }

        public static byte[] ImageToByte(System.Drawing.Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
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

        public string Get_SampleName(string pCode, string pFileLocation)
        {
            string sValue = "";
            string fileLocation = pFileLocation +  pCode;
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



        /// <summary>
        /// 匯入樞紐分析表，依照表單代號產出Excel
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public IWorkbook Export_WMT0200ExcelByDataTable( DataTable pDataTble, string pFileLocation)
        {
            DataTable dtTmp = null;
            //string sRptCode = pRptCode;
            //string sEtlCode = GD.Get_Data("RSS02_0000", sRptCode, "report_code", "etl_code");

            //檔案路徑
            string fileLocation = pFileLocation + Get_SampleName("WMT0200", pFileLocation);
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

                //在資料表裡面 取用指定欄位的資料
                dtTmp = pDataTble;
                for (int i = 0; i < dtTmp.Rows.Count; i++)
                {
                    DataRow r = dtTmp.Rows[i];
                    if (sheet.GetRow(i + 1) == null)
                    {
                        Row_tmp = sheet.CreateRow(i + 1);
                    }
                    else
                    {
                        Row_tmp = sheet.GetRow(i + 1);
                    }

                    for (int u = 0; u < dtTmp.Columns.Count; u++)
                    {
                        DataColumn Col = dtTmp.Columns[u];
                        string sField = Col.ToString();
                        string sValue = r[sField].ToString();

                        if (Row_tmp.GetCell(u) == null)
                        {
                            Cell[u] = Row_tmp.CreateCell(u);  //建立單元格
                        }
                        else
                        {
                            Cell[u] = Row_tmp.GetCell(u);
                        }
                        Cell[u].SetCellValue(sValue);
                    }
                }
                //重新讀取公式
                ISheet sheet2 = excel.GetSheetAt(0);
                sheet2.ForceFormulaRecalculation = true;

                return excel;
            }
            return null;
        }





    }
}