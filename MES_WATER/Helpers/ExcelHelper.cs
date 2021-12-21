using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MES_WATER.Models;
using System.Linq.Expressions;
using System.Reflection;
using System.Data;
using System.IO;
using NPOI.SS.UserModel;

namespace MES_WATER.Helpers
{

    public static class ExcelHelper
    {
        public static DataTable GetExcelList(Stream stream)
        {
            DataTable table = new DataTable();
            //导入excel 自动区分 xls 和 xlsx
            IWorkbook workbook = WorkbookFactory.Create(stream);
            ISheet sheet = workbook.GetSheetAt(0);//得到里面第一个sheet
                                                  //获取Excel的最大行数
            int rowsCount = sheet.PhysicalNumberOfRows;
            //为保证Table布局与Excel一样，这里应该取所有行中的最大列数（需要遍历整个Sheet）。
            //为少一交全Excel遍历，提高性能，我们可以人为把第0行的列数调整至所有行中的最大列数。
            int colsCount = sheet.GetRow(0).PhysicalNumberOfCells;
            for (int i = 0; i < colsCount; i++)
            {
                //将第一列设置成表头
                table.Columns.Add(sheet.GetRow(0).GetCell(i).ToString());
            }
            for (int x = 0; x < rowsCount; x++)
            {
                if (x == 0) continue; //去掉第一列
                DataRow dr = table.NewRow();
                for (int y = 0; y < colsCount; y++)
                {
                    //dr[y] = sheet.GetRow(x).GetCell(y).ToString();
                    var n = sheet.GetRow(x) != null && sheet.GetRow(x).GetCell(y) != null ? sheet.GetRow(x).GetCell(y) : null;
                    dr[y] = (n != null) ? n.ToString() : "";
                }
                table.Rows.Add(dr);
            }
            return table;
        }
    }
}