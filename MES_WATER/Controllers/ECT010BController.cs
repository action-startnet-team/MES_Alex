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
using System.Collections;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace MES_WATER.Controllers
{
    [Authorize] //登入驗證
    [HandleError(View = "Error")]  //錯誤導向

    public class ECT010BController : JsonNetController
    {
        //程式代號
        string sPrgCode = "ECT010B";
        //需要用到的Repo
        ECT01_0000Repository repoECT01_0100 = new ECT01_0000Repository();
        //共用函式庫
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();
        ExportController exrepo = new ExportController();

        /* 資料處理 向下 */
        /// <summary>
        /// (固定區) 主檔 首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //要結合權限控制
            //ViewBag.limit_str = comm.Get_LimitByUsrCode(User.Identity.Name, sPrgCode);
            ViewBag.prg_code = sPrgCode;

            // 使用者, controllerName, actionName
            string usr_code = User.Identity.Name;
            string prg_code = sPrgCode;
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

            //刪除暫存匯入資料
            repoECT01_0100.DeleteTempData();

            return View();
        }

        /// <summary>
        /// (固定區)主檔 首頁 按下查詢按鈕 JqGrid資料來源
        /// </summary>
        /// <param name="pWhere">使用者下的查詢條件 Json</param>
        /// <returns></returns>
        public ActionResult Get_GridDataByQuery(string pWhere)
        {
            string sUsrCode = User.Identity.Name;
            //string sPrgCode = sPrgCode;

            List<ECT01_0100> list = new List<ECT01_0100>();
            list = repoECT01_0100.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);


            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Get_Edition(string cus_code)
        {
            DataTable dt = comm.Get_DataTable("select * from ECB02_0000 where CUSTOMER_CODE='" + cus_code + "' order by EDITION");
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase upload, FormCollection form)
        {
            try
            {
                //刪除暫存匯入資料
                repoECT01_0100.DeleteTempData();

                bool isUpdate = form.AllKeys.Contains("isUpdate") ? true : false;
                string customer_code = comm.sGetString(form["cus_code"]);
                string edition = comm.sGetString(form["edition"]);

                DataTable dt = comm.XlsToDataTable(upload);
                DataTable dt2 = comm.Get_DataTable("select * from ECB02_0000 where CUSTOMER_CODE = '" + customer_code + "' and EDITION = '" + edition + "'");
                DataTable dt3 = new DataTable();

                string columnNames = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";
                string[] columns = columnNames.Split(',');
                for (int i = 0; i < 27; i++)
                {
                    DataColumn column = new DataColumn(columns[i]);
                    dt3.Columns.Add(column);
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow drow = dt3.NewRow();
                    for (int j = 0; j < dt3.Columns.Count; j++)
                    {
                        drow[j] = dt.Rows[i][j].ToString();
                    }
                    dt3.Rows.Add(drow);
                }

                int save_count = 0;
                int order_num = 1;
                int cnt = 1;    //判斷訂單欄位是否有同樣的延續的標準紀錄順序
                string att = "";    //判斷訂單欄位比對判斷
                int chkcnt = 0; //  要放迴圈外面，不然每次都被重置為0，就無法跳到 chkcnt>0 的 else if
                List<ECT01_0000> notSaveList = new List<ECT01_0000>();
                foreach (DataRow dr in dt3.Rows)
                {

                    //跳過EXCEL中的空行
                    int n = 0;
                    for (int i = 0; i < dt3.Columns.Count; i++)
                    {
                        if (dr[dt3.Columns[i].ColumnName].ToString() == "")
                        {
                            n++;
                        }
                    }
                    if (n == dt3.Columns.Count)
                    {
                        continue;
                    }

                    string ORDER_NO_MAPPING = dt2.Rows[0]["ORDER_NO_MAPPING"].ToString() != "" ? dr[dt2.Rows[0]["ORDER_NO_MAPPING"].ToString()].ToString() : "";
                    string ITEM_ID_MAPPING = dt2.Rows[0]["ITEM_ID_MAPPING"].ToString() != "" ? dr[dt2.Rows[0]["ITEM_ID_MAPPING"].ToString()].ToString() : "";
                    int PACKING_QTY_MAPPING = dt2.Rows[0]["PACKING_QTY_MAPPING"].ToString() != "" ? comm.sGetInt32(dr[dt2.Rows[0]["PACKING_QTY_MAPPING"].ToString()].ToString()) : 0;
                    DateTime NEED_DELIVERY_DATE_MAPPING;
                    string NEED_DELIVERY_DATE_MAPPING2;
                    try
                    {
                        NEED_DELIVERY_DATE_MAPPING = dt2.Rows[0]["NEED_DELIVERY_DATE_MAPPING"].ToString() != "" ? Convert.ToDateTime(dr[dt2.Rows[0]["NEED_DELIVERY_DATE_MAPPING"].ToString()]) : System.DateTime.Now;
                        NEED_DELIVERY_DATE_MAPPING2 = dt2.Rows[0]["NEED_DELIVERY_DATE_MAPPING"].ToString() != "" ? Convert.ToDateTime(dr[dt2.Rows[0]["NEED_DELIVERY_DATE_MAPPING"].ToString()]).ToString("yyyy/MM/dd") : "";
                    }
                    catch (Exception e)
                    {
                        ViewBag.timecode = 1;
                        return View();
                    }
                    string CUSTOMER_REMARK = dt2.Rows[0]["CUSTOMER_REMARK"].ToString() != "" ? dr[dt2.Rows[0]["CUSTOMER_REMARK"].ToString()].ToString() : "";
                    string PRODUCTION_REMARK1 = dt2.Rows[0]["PRODUCTION_REMARK1"].ToString() != "" ? dr[dt2.Rows[0]["PRODUCTION_REMARK1"].ToString()].ToString() : "";
                    string PRODUCTION_REMARK2 = dt2.Rows[0]["PRODUCTION_REMARK2"].ToString() != "" ? dr[dt2.Rows[0]["PRODUCTION_REMARK2"].ToString()].ToString() : "";


                    //必須有訂單單號，如果沒有則alert訊息
                    if (ORDER_NO_MAPPING == "")
                    {
                        ViewBag.respectkeycode = 1;
                        return View();
                    }

                    ECT01_0000 data = new ECT01_0000();
                    data.ORDER_NO_ID = Guid.NewGuid().ToString("N");
                    data.ORDER_NO = ORDER_NO_MAPPING;
                    data.CUSTOMER_CODE = customer_code;
                    data.CUSTOMER_NAME = comm.Get_QueryData("ECB01_0000", customer_code, "CUSTOMER_CODE", "CUSTOMER_NAME");
                    data.EDITION = edition;
                    data.ITEM_ID = ITEM_ID_MAPPING;
                    data.PACKING_QTY = PACKING_QTY_MAPPING;
                    //string delivery_date = Convert.ToDateTime(dr[dt2.Rows[0]["NEED_DELIVERY_DATE_MAPPING"].ToString()]).ToString("yyyy/MM/dd");
                    //data.NEED_DELIVERY_DATE = Convert.ToDateTime(delivery_date);
                    data.NEED_DELIVERY_DATE = NEED_DELIVERY_DATE_MAPPING;
                    data.CUSTOMER_REMARK = CUSTOMER_REMARK;
                    data.PRODUCTION_REMARK1 = PRODUCTION_REMARK1;
                    data.PRODUCTION_REMARK2 = PRODUCTION_REMARK2;
                    data.CHANGE_ROLE = User.Identity.Name;
                    data.CHANGE_DATE = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                    //int cnt = 1;    //判斷訂單欄位是否有同樣的延續的標準紀錄順序
                    ECT01_0100 data2 = new ECT01_0100();    
                    data2.CUSTOMER_CODE = customer_code;
                    data2.ORDER_NO = ORDER_NO_MAPPING;
                    if(cnt == 1)    //暫存訂單欄位的第一筆資料
                    {
                        att = data2.ORDER_NO;
                    }
                    data2.ITEM_ID = ITEM_ID_MAPPING;
                    
                    if(cnt > 1 && data2.ORDER_NO == att)    //同樣++
                    {
                        order_num++;
                    }
                    else  //不同歸1，且重新定義新的訂單內容
                    {
                        order_num = 1;
                        att = data2.ORDER_NO;
                    }
                    data2.ORDER_NUM = order_num;
                    data2.PACKING_QTY = PACKING_QTY_MAPPING;
                    data2.NEED_DELIVERY_DATE = NEED_DELIVERY_DATE_MAPPING2;
                    data2.CUSTOMER_REMARK = CUSTOMER_REMARK;
                    data2.PRODUCTION_REMARK1 = PRODUCTION_REMARK1;
                    data2.PRODUCTION_REMARK2 = PRODUCTION_REMARK2;

                    cnt++;
                    //order_num++;
                    //轉檔資料暫存
                    repoECT01_0100.InsertTempData(data2);

                    //  string str = data.ORDER_NO + data.ITEM_ID;

                    //  if (comm.Chk_RelData("ECT01_0000", "ORDER_NO", data.ORDER_NO))
                    //  if(comm.Chk_RelData("ECT01_0000", "ORDER_NO", data.ORDER_NO) || (comm.Chk_RelData("ECT01_0000", "ITEM_ID", data.ITEM_ID)))
                    //  if (comm.Chk_RelData("ECT01_0000", "ORDER_NO + ITEM_ID", data.ORDER_NO + data.ITEM_ID))
                    //if (comm.Chk_RelData("ECT01_0000", "ORDER_NO + ITEM_ID", data.ORDER_NO + data.ITEM_ID))
                    //{
                    //    repoECT01_0100.InsertData(data);
                    //    save_count += 1;
                    //}
                    //int chkcnt = 0; //  要放迴圈外面，不然每次都被重置為0，就無法跳到 chkcnt>0 的 else if
                    //如果資料庫沒有重複，就True；有就False
                    //if (comm.Chk_RelData("ECT01_0000", "ORDER_NO", data.ORDER_NO) || !(comm.Chk_RelData("ECT01_0000", "ORDER_NO", data.ORDER_NO)))

                    //連此 客戶單號 都沒有 ， 直接輸入
                    if (comm.Chk_RelData1("ECT01_0000", "ORDER_NO", "ITEM_ID", data.ORDER_NO, data.ITEM_ID))   //從原本 沒有這個，進入-->變成 有這個，進入
                    {
                        //if (comm.Chk_RelData("ECT01_0000", "ITEM_ID", data.ITEM_ID))
                        //{
                        //repoECT01_0100.InsertData(data);
                        //save_count += 1;
                        //}
                        repoECT01_0100.InsertData(data);
                        save_count += 1;

                        //chkcnt++;
                    }
                    //  因為都不會跳到else if，不管chkcnt有無大於0，所以要寫在else裡，只是條件要寫好，像是save_count的部分
                    //else if(comm.Chk_RelData("ECT01_0000", "ITEM_ID", data.ITEM_ID))    //  不能寫在else內，因為如果在else有通過，又覆蓋，save_count會多加一次
                    //{
                    //    repoECT01_0100.InsertData(data);
                    //    save_count += 1;
                    //}
                    else
                    {
                        //repoECT01_0100.InsertData(data);
                        //save_count += 1;

                        // 有此 客戶單號 ，但沒有此產品編號 ， 直接輸入
                        //if (comm.Chk_RelData("ECT01_0000", "ITEM_ID", data.ITEM_ID))    //因為前面有重複的 data.ITEM_ID，才會傳不上去
                        //{
                        //    repoECT01_0100.InsertData(data);
                        //    save_count += 1;
                        //}
                        //else
                        //{
                        // 是否覆蓋
                        //if (isUpdate)
                        //{
                        //    repoECT01_0100.UpdateData(data);
                        //    save_count += 1;
                        //}
                        //else
                        //{
                        //    notSaveList.Add(data);
                        //}
                        //}
                        if (isUpdate)
                        {
                            repoECT01_0100.UpdateData(data);
                            save_count += 1;
                        }
                        else
                        {
                            notSaveList.Add(data);
                        }
                    }
                }
                ViewBag.isDisplay = "Y";
                ViewBag.count = save_count;
                ViewBag.notSaveList = notSaveList;
                ViewBag.cus_name = comm.Get_QueryData("ECB01_0000", customer_code, "CUSTOMER_CODE", "CUSTOMER_NAME");
                ViewBag.cus_fullname = comm.Get_QueryData("ECB01_0000", customer_code, "CUSTOMER_CODE", "CUSTOMER_FULL_NAME");
            }
            catch (Exception e)
            {
                ViewBag.respectcode = 1;
                Console.WriteLine("error: " + e.Message);
            }
            return View();
        }

        public ActionResult Download_Data(string cusname)
        {
            DataTable tmp = comm.Get_DataTable("select * from ECT01_0100");
            DataTable exdt = new DataTable();

            string columnNames = " 客戶編號,客戶單號,產品編號,序號,訂購數量,欲交日,客戶備註,生產備註(一),生產備註(二) ";
            string[] columns = columnNames.Split(',');
            for (int i = 0; i < columns.Length; i++)
            {
                DataColumn column = new DataColumn(columns[i]);
                exdt.Columns.Add(column);
            }
            for (int i = 0; i < tmp.Rows.Count; i++)
            {
                DataRow drow = exdt.NewRow();
                for (int j = 0; j < exdt.Columns.Count; j++)
                {
                    drow[j] = tmp.Rows[i][j].ToString();
                }
                exdt.Rows.Add(drow);
            }

            string odate = DateTime.Now.ToString("yyyyMMdd");
            string filename = cusname + "_" + odate + ".xls";

            var originalDirectory = new DirectoryInfo(string.Format("{0}Upload\\EC_EXCEL\\", Server.MapPath(@"\")));
            string pathString = System.IO.Path.Combine(originalDirectory.ToString());
            System.IO.Directory.CreateDirectory(pathString);

            var path = pathString + filename;

            Download(TableToExcel(exdt, path), filename);

            return Json("", JsonRequestBehavior.AllowGet);
        }
        //public static void DataTableToExcel2(DataTable dt, string strFileName)
        //{
        //    HSSFWorkbook workbook = new HSSFWorkbook();
        //    HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet("sheet0");
        //    //顯示 Table 0 的所有欄位名稱
        //    HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);
        //    foreach (DataColumn column in dt.Columns)
        //    {
        //        headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
        //    }
        //    //顯示 所有資料列
        //    int rowIndex = 1;
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
        //        foreach (DataColumn column in dt.Columns)
        //        {
        //            dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
        //        }
        //        dataRow = null;
        //        rowIndex++;
        //    }


        //    // 產生 Excel 資料流
        //    MemoryStream ms = new MemoryStream();
        //    workbook.Write(ms);
        //    headerRow = null;
        //    sheet = null;
        //    workbook = null;

        //    // 設定強制下載標頭
        //    // 輸出檔案
        //    ms.Close();
        //    ms.Dispose();
        //}
        //public static HSSFWorkbook DataTableToExcel(DataTable dt, string strFileName)
        //{

        //    HSSFWorkbook workbook = new HSSFWorkbook();

        //    ISheet sheet = workbook.CreateSheet("sheet0");
        //    IRow dataRow = sheet.CreateRow(0);
        //    foreach (DataColumn column in dt.Columns)
        //    {
        //        dataRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
        //    }


        //    //填充內容  
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        dataRow = sheet.CreateRow(i + 1);
        //        for (int j = 0; j < dt.Columns.Count; j++)
        //        {
        //            dataRow.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
        //        }
        //    }
        //    workbook.Write(new FileStream(strFileName, FileMode.Create, FileAccess.Write));
        //    return workbook;
        //}

        public static IWorkbook TableToExcel(DataTable dt, string file)
        {
            IWorkbook workbook;
            string fileExt = Path.GetExtension(file).ToLower();
            if (fileExt == ".xlsx") { workbook = new XSSFWorkbook(); } else if (fileExt == ".xls") { workbook = new HSSFWorkbook(); } else { workbook = null; }
            ISheet sheet = string.IsNullOrEmpty(dt.TableName) ? workbook.CreateSheet("Sheet1") : workbook.CreateSheet(dt.TableName);

            //表头  
            IRow row = sheet.CreateRow(0);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ICell cell = row.CreateCell(i);
                cell.SetCellValue(dt.Columns[i].ColumnName);
            }

            //数据  
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row1 = sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ICell cell = row1.CreateCell(j);
                    cell.SetCellValue(dt.Rows[i][j].ToString());
                }
            }

            //转为字节数组  
            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            var buf = stream.ToArray();

            //保存为Excel文件  
            using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
            }
            return workbook;
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

        /// <summary>
        /// (修改區) 主檔 新增
        /// 1.新增模式下控項的預設值在這邊設定
        /// </summary>
        /// <returns></returns>
        //public ActionResult Insert()
        //{
        //    //要結合權限控制
        //    //ViewBag.limit_str = comm.Get_LimitByUsrCode(User.Identity.Name, sPrgCode);
        //    ViewBag.prg_code = sPrgCode;

        //    //新增模式的預設值
        //    ECT01_0000 newData = new ECT01_0000();

        //    //由於datetime格式無法儲存 用SETUP_DATE2 取代 SETUP_DATE

        //    return View(newData);
        //}

        ///// <summary>
        ///// (修改區) 主檔 修改
        ///// 1.依資料鍵值到DB取回資料呈現在修改模式頁面
        ///// </summary>
        ///// <param name="pTkCode">資料鍵值</param>
        ///// <returns></returns>
        //public ActionResult Update(string pTkCode)
        //{
        //    //ViewBag.limit_str = comm.Get_LimitByUsrCode(User.Identity.Name, sPrgCode);
        //    ViewBag.prg_code = sPrgCode;

        //    ECT01_0000 newData = repoECT01_0100.GetDTO(pTkCode);

        //    return View(newData);
        //}


        ///// <summary>
        ///// (修改區) 主檔的新增頁面將值存進DB
        ///// </summary>
        ///// <param name="form">畫面上輸入的form元件中的控制項集合</param>
        ///// <param name="model">要存檔的model</param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult Insert(FormCollection form, ECT01_0000 model)
        //{

        //    // MVC model驗證
        //    if (ModelState.IsValid)
        //    {
        //        // 自訂義 資料驗證
        //        bool bIsOK = Chk_Ins_Main(form);

        //        // 資料驗證失敗
        //        if (!bIsOK)
        //        {
        //            ViewBag.showErrMsg = true;
        //            ViewBag.prg_code = sPrgCode;
        //            return View(model);
        //        }

        //        //執行存檔
        //        ECT01_0000 data = new ECT01_0000();
        //        comm.Set_ModelValue(data, form);

        //        //在取完畫面上的值後，如果有一些別名欄位要變更值，可以在這邊2次加工

        //        repoECT01_0100.InsertData(data);
        //        // 新增紀錄資料
        //        comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);
        //        //存完檔回到主頁，如果不跳回主頁要在這裡做修改
        //        return RedirectToAction("Index");


        //    }
        //    ViewBag.showErrMsg = true;
        //    ViewBag.prg_code = sPrgCode;
        //    return View(model);
        //}

        ///// <summary>
        ///// (修改區) 主檔的修改頁面將值存進DB
        ///// </summary>
        ///// <param name="form">畫面上輸入的form元件中的控制項集合</param>
        ///// <param name="model">要存檔的model</param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult Update(FormCollection form, ECT01_0000 model)
        //{
        //    // MVC model驗證 資料格式檢查
        //    if (ModelState.IsValid)
        //    {
        //        // 自定義 資料邏輯檢查
        //        bool bIsOK = Chk_Upd_Main(form);

        //        // 資料驗證失敗
        //        if (!bIsOK)
        //        {
        //            ViewBag.showErrMsg = true;
        //            ViewBag.prg_code = sPrgCode;
        //            return View(model);
        //        }

        //        //執行存檔
        //        ECT01_0000 data = new ECT01_0000();
        //        comm.Set_ModelValue(data, form);
        //        ECT01_0000 sBefore = comm.GetData<ECT01_0000>(data);
        //        repoECT01_0100.UpdateData(data);
        //        //更新紀錄資料
        //        comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "update", sBefore, data);
        //        return RedirectToAction("Index");



        //    }
        //    ViewBag.showErrMsg = true;
        //    ViewBag.prg_code = sPrgCode;
        //    return View(model);
        //}

        /// <summary>
        /// (修改區) 按下刪除後刪除DB動作
        /// </summary>
        /// <param name="pTkCode">要刪除的鍵值</param>
        /// <returns></returns>
        //[HttpPost]
        //public ActionResult Delete(string pTkCode)
        //{
        //    //刪除前的檢查要在JqGrid送出前檢查，所以對應Chk_Del_Main這個函數
        //    ECT01_0000 sBefore = comm.GetData<ECT01_0000>(pTkCode);
        //    repoECT01_0100.DeleteData(pTkCode);
        //    //刪除紀錄資料
        //    comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "delete", sBefore, "");
        //    return RedirectToAction("Index");
        //}
        /* 資料處理 向上 */

        //資料檢查 向下//
        //主檔的檢查
        [HttpPost]
        public ActionResult Check_Data(FormCollection form, ECT01_0100 model)
        {
            bool isSuccess = false;
            //檢查資料代碼是否重覆
            string key = gmv.GetKey<ECT01_0100>(new ECT01_0100());
            string sWhere = "where " + key + "='" + form[key].ToString() + "'";
            bool hasRow = !comm.Chk_RelData("ECT01_0100", sWhere);
            if (hasRow)
            {
                ModelState.AddModelError(key, "代碼已存在!");
                isSuccess = true;
            }
            var returnData = new
            {
                // 成功與否
                IsSuccess = isSuccess,
                // ModelState錯誤訊息 
                ModelStateErrors = ModelState.Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(k => k.Key, k => k.Value.Errors.Select(e => e.ErrorMessage).ToArray())
            };
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(returnData), "application/json");
        }

        /* 資料檢查 向下 */
        /// <summary>
        /// (修改處) 新增資料前的資料檢查點
        /// </summary>
        /// <param name="form">畫面上輸入的值集合</param>
        /// <returns></returns>
        private bool Chk_Ins_Main(FormCollection form)
        {
            bool bIsOK = true;

            //** 依作業不同有不同的檢查點 向下
            //檢查有錯時用以下程式碼顯示錯誤訊息
            //程式參考
            //if (!comm.Chk_IdNo(form["pro_code"]).isValid)
            //{
            //    bDataIsValid = false;
            //    ModelState.AddModelError("pro_code", comm.Chk_IdNo(form["pro_code"]).message);
            //}

            //** 依作業不同有不同的檢查點 向上

            //檢查結果回傳
            return bIsOK;
        }

        /// <summary>
        /// (修改處) 修改資料前的資料檢查點
        /// </summary>
        /// <param name="form">畫面上輸入的值集合</param>
        /// <returns></returns>
        private bool Chk_Upd_Main(FormCollection form)
        {
            // 自訂義資料檢查開始
            bool bIsOK = true;

            //** 依作業不同有不同的檢查點 向下

            //檢查有錯時用以下程式碼顯示錯誤訊息
            //程式參考
            //if (!comm.Chk_IdNo(form["pro_code"]).isValid)
            //{
            //    bDataIsValid = false;
            //    ModelState.AddModelError("pro_code", comm.Chk_IdNo(form["pro_code"]).message);
            //}

            //** 依作業不同有不同的檢查點 向上
            return bIsOK;
        }

        /// <summary>
        /// (修改處) 刪除資料前的資料檢查點
        /// </summary>
        /// <param name="form">畫面上輸入的值集合</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Chk_Del_Main(FormCollection form)
        {
            bool bIsOK = true;
            string message = "";
            //檢查資料代碼是否重覆

            //** 依作業不同有不同的檢查點 向下

            //檢查有錯時用以下程式碼顯示錯誤訊息
            //程式參考
            //if (true)
            //{
            //    bIsOK = false;
            //    message += "<div class='text-danger'>";
            //    message += "<li> 測試1 </li>";
            //    message += "</div>";
            //}

            //** 依作業不同有不同的檢查點 向上

            var result = new
            {
                isValid = bIsOK,
                message = message
            };

            return Json(result);
        }

        /* 資料檢查 向上 */

        /// <summary>
        /// 前端Ajax控項資料代名稱
        /// </summary>
        /// <param name="pCusCode">客戶編號</param>
        /// <param name="pType">要取回的欄位</param>
        /// <returns></returns>
        public string Get_ProData(string pProCode, string pType)
        {
            string sReturn = "";
            sReturn = comm.Get_QueryData("STB01_0000", pProCode, "pro_code", pType);
            return sReturn;
        }

        /// <summary>
        /// 取得廠商的聯絡人
        /// </summary>
        /// <param name="sup_code"></param>
        /// <returns></returns>
        public JsonResult Get_SupAtn(string sup_code)
        {
            string sSql = "select STB10_0100.* from STB10_0100 where sup_code = @sup_code ";
            DataTable dtTmp = comm.Get_DataTable(sSql, "sup_code", sup_code);

            return Json(dtTmp, JsonRequestBehavior.AllowGet);
        }
    }
}