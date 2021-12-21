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

namespace MES_WATER.Controllers
{
    public class ExcelTestController : Controller
    {
        Comm comm = new Comm();
        GetData GD = new GetData();
        DynamicTable DT = new DynamicTable();
        CheckData CD = new CheckData();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Export_Test()
        {
            int num = 0;
            int num2 = 0;
            int sr_size = 7;
            int sr_exterior = 8;
            XSSFWorkbook workbook;
            using (FileStream fs = new FileStream(Server.MapPath("~/Upload/Report/") + "B014-1.xlsx", FileMode.Open, FileAccess.Read))
            {
                workbook = new XSSFWorkbook(fs);
            }

            //插入資料程式碼參考 向下//
            //XSSFSheet sheet = (XSSFSheet)workbook.CreateSheet("sheet2"); //新建資料表同時插入
            //sheet.CreateRow(0).CreateCell(0).SetCellValue(0);
            //XSSFSheet at_sheet = (XSSFSheet)workbook.GetSheetAt(0);//用索引取得資料表並插入 
            //at_sheet.CreateRow(0).CreateCell(0).SetCellValue("at sheet");
            //XSSFSheet sheet = (XSSFSheet)workbook.GetSheet("工作表1");   //用資料表名取得資料表並插入
            //sheet.CreateRow(1).CreateCell(0).SetCellValue(1);

            //插入資料程式碼參考 向上//

            DataTable dt = comm.Get_DataTable("select TOP(34) * from MEB20_0000");
            DataTable dt2 = comm.Get_DataTable("select TOP(12) * from MEB23_0000");
            num = (dt.Rows.Count > 1) ? dt.Rows.Count - 1 : 0;
            num2 = (dt2.Rows.Count > 1) ? dt2.Rows.Count - 1 : 0;
            XSSFSheet sheet = (XSSFSheet)workbook.GetSheet("工作表1");
            if (num >= 1)
            {
                sheet.ShiftRows(8, sheet.LastRowNum, num);
                for (int i = 1; i <= num; i++)
                {
                    sheet.CopyRow(sr_size, sr_size + i);
                    sheet.GetRow(sr_size + i).GetCell(0).SetCellValue("");
                }
            }
            if (num2 >= 1)
            {
                sheet.ShiftRows(9 + num, sheet.LastRowNum, num2);
                for (int i = 1; i <= num2; i++)
                {
                    sheet.CopyRow(sr_exterior + num, sr_exterior + num + i);
                    sheet.GetRow(sr_exterior + num + i).GetCell(0).SetCellValue("");
                }
            }
            
            string f1="", f2 = "", f3 = "", f4 = "", f5 = "", f6 = "", f7 = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                f1 = dt.Rows[i]["pro_name"].ToString();
                f2 = dt.Rows[i]["pro_code"].ToString();
                f3 = dt.Rows[i]["pro_spc"].ToString();
                f4 = dt.Rows[i]["pro_unit"].ToString();
                f5 = dt.Rows[i]["line_code"].ToString();
                f6 = dt.Rows[i]["pro_type_code"].ToString();
                f7 = dt.Rows[i]["is_use"].ToString();
                sheet.GetRow(sr_size + i).GetCell(1).SetCellValue(f1);
                sheet.GetRow(sr_size + i).GetCell(5).SetCellValue(f2);
                sheet.GetRow(sr_size + i).GetCell(7).SetCellValue(f3);
                sheet.GetRow(sr_size + i).GetCell(9).SetCellValue(f4);
                sheet.GetRow(sr_size + i).GetCell(10).SetCellValue(f5);
                sheet.GetRow(sr_size + i).GetCell(14).SetCellValue(f6);
                sheet.GetRow(sr_size + i).GetCell(15).SetCellValue(f7);
            }
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                f1 = dt2.Rows[i]["bom_name"].ToString();
                f2 = dt2.Rows[i]["bom_code"].ToString();
                f3 = dt2.Rows[i]["pro_code"].ToString();
                f4 = dt2.Rows[i]["pro_qty"].ToString();
                f5 = dt2.Rows[i]["pro_unit"].ToString();
                f6 = dt2.Rows[i]["version"].ToString();
                f7 = dt2.Rows[i]["ins_date"].ToString();
                sheet.GetRow(sr_exterior + num + i).GetCell(1).SetCellValue(f1);
                sheet.GetRow(sr_exterior + num + i).GetCell(5).SetCellValue(f2);
                sheet.GetRow(sr_exterior + num + i).GetCell(7).SetCellValue(f3);
                sheet.GetRow(sr_exterior + num + i).GetCell(9).SetCellValue(f4);
                sheet.GetRow(sr_exterior + num + i).GetCell(10).SetCellValue(f5);
                sheet.GetRow(sr_exterior + num + i).GetCell(14).SetCellValue(f6);
                sheet.GetRow(sr_exterior + num + i).GetCell(15).SetCellValue(f7);
            }

            #region 合併儲存格
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(sr_size, sr_size + num, 0, 0));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(sr_exterior + num, sr_exterior + num + num2, 0, 0));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(9 + num + num2, 9 + num + num2, 0, 2));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(9 + num + num2, 9 + num + num2, 3, 16));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(10 + num + num2, 10 + num + num2, 0, 2));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(10 + num + num2, 10 + num + num2, 3, 16));
            
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(12 + num + num2, 12 + num + num2, 0, 1));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(12 + num + num2, 12 + num + num2, 2, 4));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(12 + num + num2, 12 + num + num2, 5, 7));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(12 + num + num2, 12 + num + num2, 8, 9));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(12 + num + num2, 12 + num + num2, 10, 13));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(12 + num + num2, 12 + num + num2, 14, 16));
            for (int i = 0; i < 2 + num + num2; i++)
            {
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(sr_size + i, sr_size + i, 1, 4));
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(sr_size + i, sr_size + i, 5, 6));
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(sr_size + i, sr_size + i, 7, 8));
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(sr_size + i, sr_size + i, 10, 13));
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(sr_size + i, sr_size + i, 15, 16));
            }
            #endregion

            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "B014-1.xlsx"));
            Response.BinaryWrite(ms.ToArray());
            Response.Flush();
            Response.End();
            
            return View("Index");
        }


        public ActionResult Export_Test2()
        {
            int num = 0;
            int num2 = 0;
            int sr_size = 7; //尺寸開始ROW
            int sr_exterior = 8;  //外觀開始ROW
            XSSFWorkbook workbook;
            using (FileStream fs = new FileStream(Server.MapPath("~/Upload/Report/") + "B014-1_4.xlsx", FileMode.Open, FileAccess.Read))
            {
                workbook = new XSSFWorkbook(fs);
            }

            //插入資料程式碼參考 向下//
            //XSSFSheet sheet = (XSSFSheet)workbook.CreateSheet("sheet2"); //新建資料表同時插入
            //sheet.CreateRow(0).CreateCell(0).SetCellValue(0);
            //XSSFSheet at_sheet = (XSSFSheet)workbook.GetSheetAt(0);//用索引取得資料表並插入 
            //at_sheet.CreateRow(0).CreateCell(0).SetCellValue("at sheet");
            //XSSFSheet sheet = (XSSFSheet)workbook.GetSheet("工作表1");   //用資料表名取得資料表並插入
            //sheet.CreateRow(1).CreateCell(0).SetCellValue(1);

            //插入資料程式碼參考 向上//

            DataTable dt = comm.Get_DataTable("select TOP(10) * from MEB20_0000");
            DataTable dt2 = comm.Get_DataTable("select TOP(10) * from MEB23_0000");
            num = (dt.Rows.Count > 1) ? dt.Rows.Count - 1 : 0;
            num2 = (dt2.Rows.Count > 1) ? dt2.Rows.Count - 1 : 0;
            XSSFSheet sheet = (XSSFSheet)workbook.GetSheet("工作表1");
            if (num >= 1)
            {
                sheet.ShiftRows(8, sheet.LastRowNum, num);
                for (int i = 1; i <= num; i++)
                {
                    //sheet.CreateRow(sr_size + i);
                    sheet.CopyRow(sr_size, sr_size + i);
                    sheet.GetRow(sr_size + i).GetCell(0).SetCellValue("");
                }
            }
            if (num2 >= 1)
            {
                sheet.ShiftRows(9 + num, sheet.LastRowNum, num2);
                for (int i = 1; i <= num2; i++)
                {
                    //sheet.CreateRow(sr_exterior + num + i);
                    sheet.CopyRow(sr_exterior + num, sr_exterior + num + i);
                    sheet.GetRow(sr_exterior + num + i).GetCell(0).SetCellValue("");
                }
            }

            string f1 = "", f2 = "", f3 = "", f4 = "", f5 = "", f6 = "", f7 = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                f1 = dt.Rows[i]["pro_name"].ToString();
                f2 = dt.Rows[i]["pro_code"].ToString();
                f3 = dt.Rows[i]["pro_spc"].ToString();
                f4 = dt.Rows[i]["pro_unit"].ToString();
                f5 = dt.Rows[i]["line_code"].ToString();
                f6 = dt.Rows[i]["pro_type_code"].ToString();
                f7 = dt.Rows[i]["is_use"].ToString();
                sheet.GetRow(sr_size + i).GetCell(1).SetCellValue(f1);
                sheet.GetRow(sr_size + i).GetCell(5).SetCellValue(f2);
                sheet.GetRow(sr_size + i).GetCell(7).SetCellValue(f3);
                sheet.GetRow(sr_size + i).GetCell(9).SetCellValue(f4);
                sheet.GetRow(sr_size + i).GetCell(10).SetCellValue(f5);
                sheet.GetRow(sr_size + i).GetCell(14).SetCellValue(f6);
                sheet.GetRow(sr_size + i).GetCell(15).SetCellValue(f7);
            }
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                f1 = dt2.Rows[i]["bom_name"].ToString();
                f2 = dt2.Rows[i]["bom_code"].ToString();
                f3 = dt2.Rows[i]["pro_code"].ToString();
                f4 = dt2.Rows[i]["pro_qty"].ToString();
                f5 = dt2.Rows[i]["pro_unit"].ToString();
                f6 = dt2.Rows[i]["version"].ToString();
                f7 = dt2.Rows[i]["ins_date"].ToString();
                sheet.GetRow(sr_exterior + num + i).GetCell(1).SetCellValue(f1);
                sheet.GetRow(sr_exterior + num + i).GetCell(5).SetCellValue(f2);
                sheet.GetRow(sr_exterior + num + i).GetCell(7).SetCellValue(f3);
                sheet.GetRow(sr_exterior + num + i).GetCell(9).SetCellValue(f4);
                sheet.GetRow(sr_exterior + num + i).GetCell(10).SetCellValue(f5);
                sheet.GetRow(sr_exterior + num + i).GetCell(14).SetCellValue(f6);
                sheet.GetRow(sr_exterior + num + i).GetCell(15).SetCellValue(f7);
            }


            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "test.xlsx"));
            Response.BinaryWrite(ms.ToArray());
            Response.Flush();
            Response.End();


            return View("Index");
        }

        public ActionResult Export_Test3()
        {
            XSSFWorkbook workbook;
            using (FileStream fs = new FileStream(Server.MapPath("~/Upload/Report/") + "B014-1_2.xlsx", FileMode.Open, FileAccess.Read))
            {
                workbook = new XSSFWorkbook(fs);
            }
            XSSFSheet sheet = (XSSFSheet)workbook.GetSheet("工作表1");

            sheet.ShiftRows(9, 10, 1);
            //sheet.CreateRow(9);
            sheet.CopyRow(8, 9);
            //sheet.CreateRow(16);
            //sheet.CopyRow(15, 16);
            //for (int i = 0; i <= 7; i++)
            //{
            //    //sheet.CreateRow(7 + i);
            //    sheet.CopyRow(7, 7 + i);
            //    sheet.GetRow(7 + i).GetCell(0).SetCellValue("");
            //}

            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "test.xlsx"));
            Response.BinaryWrite(ms.ToArray());
            Response.Flush();
            Response.End();
            return View("Index");
        }

        public ActionResult Export_Test4()
        {
            XSSFWorkbook workbook;
            using (FileStream fs = new FileStream(Server.MapPath("~/Upload/Report/") + "test.xlsx", FileMode.Open, FileAccess.Read))
            {
                workbook = new XSSFWorkbook(fs);
            }
            XSSFSheet sheet = (XSSFSheet)workbook.GetSheet("工作表1");
            //string qq = sheet.GetRow(0).GetCell(0).ToString();
            //string qq1 = sheet.GetRow(1).GetCell(0).ToString();
            //string qq2 = sheet.GetRow(2).GetCell(0).ToString();
            //sheet.ShiftRows(8, 10, 1);
            sheet.CopyRow(0, 1);

            IRow R0 = sheet.GetRow(0);
            IRow R1 = sheet.GetRow(1);
            

            //sheet.CreateRow(16);
            //sheet.CopyRow(15, 16);
            //for (int i = 1; i <= 1; i++)
            //{
            //    //sheet.CreateRow(7 + i);
            //    sheet.CopyRow(7, 7 + i);
            //    sheet.GetRow(7 + i).GetCell(0).SetCellValue("");
            //}

            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "test.xlsx"));
            Response.BinaryWrite(ms.ToArray());
            Response.Flush();
            Response.End();
            return View("Index");
        }

        public string Export_Test5()
        {
            int num = 0;
            int num2 = 0;
            int sr_size = 7;
            int sr_exterior = 8;
            string qmt_code = "";
            XSSFWorkbook workbook;
            using (FileStream fs = new FileStream(Server.MapPath("~/Upload/Report/") + "B014-1.xlsx", FileMode.Open, FileAccess.Read))
            {
                workbook = new XSSFWorkbook(fs);
            }

            //插入資料程式碼參考 向下//
            //XSSFSheet sheet = (XSSFSheet)workbook.CreateSheet("sheet2"); //新建資料表同時插入
            //sheet.CreateRow(0).CreateCell(0).SetCellValue(0);
            //XSSFSheet at_sheet = (XSSFSheet)workbook.GetSheetAt(0);//用索引取得資料表並插入 
            //at_sheet.CreateRow(0).CreateCell(0).SetCellValue("at sheet");
            //XSSFSheet sheet = (XSSFSheet)workbook.GetSheet("工作表1");   //用資料表名取得資料表並插入
            //sheet.CreateRow(1).CreateCell(0).SetCellValue(1);

            //插入資料程式碼參考 向上//

            DataTable dt = comm.Get_DataTable("select TOP(34) * from MEB20_0000");
            DataTable dt2 = comm.Get_DataTable("select TOP(12) * from MEB23_0000");
            DataTable dt3 = comm.Get_DataTable(" select QMT04_0100.qmt04_0100, QMT04_0110.* " +
                                               " from QMT04_0100 " +
                                               " left join QMT04_0110 on QMT04_0110.qmt04_0100 = QMT04_0100.qmt04_0100 " +
                                               " where qmt_code = '" + qmt_code + "'");
            num = (dt.Rows.Count > 1) ? dt.Rows.Count - 1 : 0;
            num2 = (dt2.Rows.Count > 1) ? dt2.Rows.Count - 1 : 0;
            XSSFSheet sheet = (XSSFSheet)workbook.GetSheet("工作表1");
            if (num >= 1)
            {
                sheet.ShiftRows(8, sheet.LastRowNum, num);
                for (int i = 1; i <= num; i++)
                {
                    sheet.CopyRow(sr_size, sr_size + i);
                    sheet.GetRow(sr_size + i).GetCell(0).SetCellValue("");
                }
            }
            if (num2 >= 1)
            {
                sheet.ShiftRows(9 + num, sheet.LastRowNum, num2);
                for (int i = 1; i <= num2; i++)
                {
                    sheet.CopyRow(sr_exterior + num, sr_exterior + num + i);
                    sheet.GetRow(sr_exterior + num + i).GetCell(0).SetCellValue("");
                }
            }

            string f1 = "", f2 = "", f3 = "", f4 = "", f5 = "", f6 = "", f7 = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                f1 = dt.Rows[i]["pro_name"].ToString();
                f2 = dt.Rows[i]["pro_code"].ToString();
                f3 = dt.Rows[i]["pro_spc"].ToString();
                f4 = dt.Rows[i]["pro_unit"].ToString();
                f5 = dt.Rows[i]["line_code"].ToString();
                f6 = dt.Rows[i]["pro_type_code"].ToString();
                f7 = dt.Rows[i]["is_use"].ToString();
                sheet.GetRow(sr_size + i).GetCell(1).SetCellValue(f1);
                sheet.GetRow(sr_size + i).GetCell(5).SetCellValue(f2);
                sheet.GetRow(sr_size + i).GetCell(7).SetCellValue(f3);
                sheet.GetRow(sr_size + i).GetCell(9).SetCellValue(f4);
                sheet.GetRow(sr_size + i).GetCell(10).SetCellValue(f5);
                sheet.GetRow(sr_size + i).GetCell(14).SetCellValue(f6);
                sheet.GetRow(sr_size + i).GetCell(15).SetCellValue(f7);
            }
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                f1 = dt2.Rows[i]["bom_name"].ToString();
                f2 = dt2.Rows[i]["bom_code"].ToString();
                f3 = dt2.Rows[i]["pro_code"].ToString();
                f4 = dt2.Rows[i]["pro_qty"].ToString();
                f5 = dt2.Rows[i]["pro_unit"].ToString();
                f6 = dt2.Rows[i]["version"].ToString();
                f7 = dt2.Rows[i]["ins_date"].ToString();
                sheet.GetRow(sr_exterior + num + i).GetCell(1).SetCellValue(f1);
                sheet.GetRow(sr_exterior + num + i).GetCell(5).SetCellValue(f2);
                sheet.GetRow(sr_exterior + num + i).GetCell(7).SetCellValue(f3);
                sheet.GetRow(sr_exterior + num + i).GetCell(9).SetCellValue(f4);
                sheet.GetRow(sr_exterior + num + i).GetCell(10).SetCellValue(f5);
                sheet.GetRow(sr_exterior + num + i).GetCell(14).SetCellValue(f6);
                sheet.GetRow(sr_exterior + num + i).GetCell(15).SetCellValue(f7);
            }

            #region 合併儲存格
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(sr_size, sr_size + num, 0, 0));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(sr_exterior + num, sr_exterior + num + num2, 0, 0));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(9 + num + num2, 9 + num + num2, 0, 2));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(9 + num + num2, 9 + num + num2, 3, 16));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(10 + num + num2, 10 + num + num2, 0, 2));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(10 + num + num2, 10 + num + num2, 3, 16));

            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(12 + num + num2, 12 + num + num2, 0, 1));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(12 + num + num2, 12 + num + num2, 2, 4));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(12 + num + num2, 12 + num + num2, 5, 7));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(12 + num + num2, 12 + num + num2, 8, 9));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(12 + num + num2, 12 + num + num2, 10, 13));
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(12 + num + num2, 12 + num + num2, 14, 16));
            for (int i = 0; i < 2 + num + num2; i++)
            {
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(sr_size + i, sr_size + i, 1, 4));
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(sr_size + i, sr_size + i, 5, 6));
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(sr_size + i, sr_size + i, 7, 8));
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(sr_size + i, sr_size + i, 10, 13));
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(sr_size + i, sr_size + i, 15, 16));
            }
            #endregion

            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "B014-1.xlsx"));
            Response.BinaryWrite(ms.ToArray());
            Response.Flush();
            Response.End();

            return "";
        }


    }
}