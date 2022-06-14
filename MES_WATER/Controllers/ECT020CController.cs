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
using NPOI.SS.Formula.Functions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MES_WATER.Controllers
{
    [Authorize] //登入驗證
    [HandleError(View = "Error")]  //錯誤導向

    public class ECT020CController : JsonNetController
    {
        //程式代號
        string sPrgCode = "ECT020C";
        public string sJson { get; set; }
        //需要用到的Repo
        ECT02_0200Repository repoECT02_0200 = new ECT02_0200Repository();
        ECT02_0000Repository repoECT02_0000 = new ECT02_0000Repository();
        ECT02_0100Repository repoECT02_0100 = new ECT02_0100Repository();
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

            ViewBag.isDisplay = "N";
            return View();
        }

        /// <summary>
        /// (固定區)主檔 首頁 按下查詢按鈕 JqGrid資料來源
        /// </summary>
        /// <param name="pWhere">使用者下的查詢條件 Json</param>
        /// <returns></returns>
        //public ActionResult Get_GridDataByQuery(string pWhere)
        //{
        //    DataTable dt = (DataTable)Session["TempTable"];
        //    List<T> list = DataTableToList<T>(dt);
        //    Session.Remove("TempTable");
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult Get_Edition(string cus_code)    //大衛版---下拉選單1()
        {
            DataTable dt = comm.Get_DataTable("select * from ECB04_0000 where CUSTOMER_CODE='" + cus_code + "' order by EDITION");
            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase upload, FormCollection form)
        {
            //刪除暫存匯入資料
            //repoECT02_0200.DeleteTempData();
            try
            {
                bool isUpdate = form.AllKeys.Contains("isUpdate") ? true : false;
                string customer_code = comm.sGetString(form["cus_code"]);
                string edition = comm.sGetString(form["edition"]);

                //Excel Table
                DataTable dt = comm.XlsToDataTable(upload);

                //取得欄位設定
                DataTable dt2 = comm.Get_DataTable("select * from ECB04_0000 " +
                    "left join ECB04_0100 on ECB04_0000.SALES_CUSTOMER_CODE_EDITION = ECB04_0100.SALES_CUSTOMER_CODE_EDITION " +
                    "left join ECB05_0000 on ECB04_0100.ERP_FIELD_CODE = ECB05_0000.ERP_FIELD_CODE " +
                    "where CUSTOMER_CODE = '" + customer_code + "' and EDITION = '" + edition + "' order by SERIAL_NUM");

                //製作標頭為英文字母的表格
                DataTable dt3 = new DataTable();

                string columnNames = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z," +
                                      "AA,AB,AC,AD,AE,AF,AG,AH,AI,AJ,AK,AL,AM,AN,AO,AP,AQ,AR,AS,AT,AU,AV,AW,AX,AY,AZ," +
                                      "BA,BB,BC,BD,BE,BF,BG,BH,BI,BJ,BK,BL,BM,BN,BO,BP,BQ,BR,BS,BT,BU,BV,BW,BX,BY,BZ," +
                                      "CA,CB,CC,CD,CE,CF,CG,CH,CI,CJ,CK,CL,CM,CN,CO,CP,CW,CR,CS,CT,CU,CV,CW,CX,CY,CZ";
                string[] columns = columnNames.Split(',');
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    DataColumn column = new DataColumn(columns[i].Trim());
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

                //製作預覽表格
                DataTable TempTable = new DataTable();
                //TempTable.Columns.Add("客戶編號");

                foreach (DataRow dr2 in dt2.Rows)
                {
                    var n = dr2["ERP_FIELD_NAME"] == null ? "" : dr2["ERP_FIELD_NAME"];


                    TempTable.Columns.Add(n.ToString(), System.Type.GetType("System.String"));

                }
               
                int save_count = 0;
                List<ECT02_0000> notSaveList = new List<ECT02_0000>();
               
                foreach (DataRow dr in dt3.Rows)
                {
                    int order_num = 1;
                    ECT02_0000 data = new ECT02_0000();
                    data.SALES_ORDER_NO_ID = Guid.NewGuid().ToString();
                    data.SALES_CUSTOMER_CODE_EDITION = dt2.Rows[0]["SALES_CUSTOMER_CODE_EDITION"].ToString();
                    data.SALES_ORDER_NO = "";

                    //找出出貨單號
                    int n = 0;
                    foreach (DataRow dr2 in dt2.Rows)
                    {
                        if (dr2["ERP_FIELD_CODE"].ToString() == "DELEVERY_ORDER")
                        {
                            data.SALES_ORDER_NO = dr[dr2["EXCEL_CODE"].ToString()].ToString();
                        }

                        if (dr[dr2["EXCEL_CODE"].ToString()].ToString() == "")
                        {
                            n++;
                        }

                        //if (dr2["ERP_FIELD_VALUE"].ToString() != "")
                        //{
                        //    dr2["EXCEL_CODE"].ToString() = dr2["ERP_FIELD_VALUE"].ToString();
                        //}
                    }

                    //跳過EXCEL中的空行
                    if (n == dt2.Rows.Count)
                    {
                        continue;
                    }

                    //必須有出貨單號，如果沒有則alert訊息
                    if (data.SALES_ORDER_NO == "")
                    {
                        ViewBag.respectkeycode = 1;
                        return View();
                    }

                    //建立TempTable新資料行
                    DataRow drow = TempTable.NewRow();
                    //drow["客戶編號"] = customer_code;

                    foreach (DataRow dr2 in dt2.Rows)
                    {

                        if (dr2["ERP_FIELD_VALUE"].ToString() != "")
                        {
                            drow[dr2["ERP_FIELD_NAME"].ToString()] = dr2["ERP_FIELD_VALUE"].ToString();
                        }
                        else if(dr2["is_null"].ToString() == "Y") { drow[dr2["ERP_FIELD_NAME"].ToString()] = ""; }
                        else if(dr2["BACK_VALUE"].ToString() != "") { drow[dr2["ERP_FIELD_NAME"].ToString()] = dr2["ERP_FIELD_NAME"].ToString() + dr2["BACK_VALUE"].ToString(); }
                        else { drow[dr2["ERP_FIELD_NAME"].ToString()] = dr[dr2["EXCEL_CODE"].ToString()].ToString(); }



                    }
                    TempTable.Rows.Add(drow);

                    //檢查是否重複
                    if (comm.Chk_RelDataBySql("select * from ECT02_0000 where SALES_ORDER_NO ='" + data.SALES_ORDER_NO + "'"))
                    {
                        ECT02_0100 data2 = new ECT02_0100();
                        foreach (DataRow dr2 in dt2.Rows)
                        {
                            data2.ect02_0100 = Guid.NewGuid().ToString();
                            data2.SALES_ORDER_NO_ID = data.SALES_ORDER_NO_ID;
                            data2.SERIAL_NUM = order_num.ToString();
                            data2.ERP_FIELD_CODE = dr2["ERP_FIELD_CODE"].ToString();
                            data2.VALUE = dr[dr2["EXCEL_CODE"].ToString()].ToString();
                            order_num++;

                            repoECT02_0100.InsertData(data2);
                        }
                        repoECT02_0000.InsertData(data);
                        save_count += 1;
                    }
                    else
                    {
                        // 是否覆蓋
                        if (isUpdate)
                        {
                            ECT02_0100 data2 = new ECT02_0100();
                            foreach (DataRow dr2 in dt2.Rows)
                            {
                                data2.ect02_0100 = Guid.NewGuid().ToString();
                                data2.SALES_ORDER_NO_ID = data.SALES_ORDER_NO_ID;
                                data2.SERIAL_NUM = order_num.ToString();
                                data2.ERP_FIELD_CODE = dr2["ERP_FIELD_CODE"].ToString();
                                data2.VALUE = dr[dr2["EXCEL_CODE"].ToString()].ToString();
                                order_num++;

                                //repoECT02_0100.UpdateData(data2);
                                string delete_key = comm.Get_QueryData("ECT02_0000", data.SALES_ORDER_NO, "SALES_ORDER_NO", "SALES_ORDER_NO_ID");
                                repoECT02_0100.DeleteData(delete_key);
                                repoECT02_0100.InsertData(data2);
                            }
                            //repoECT02_0000.UpdateData(data);
                            repoECT02_0000.DeleteData2(data.SALES_ORDER_NO);
                            repoECT02_0000.InsertData(data);
                            save_count += 1;
                        }
                        else
                        {
                            notSaveList.Add(data);
                        }
                    }
                }
                //Session["TempTable"] = TempTable;
                ViewBag.isDisplay = "Y";
                ViewBag.count = save_count;
                ViewBag.notSaveList = notSaveList;
                ViewBag.TempTable = TempTable;
                ViewBag.TempTable2 = JsonConvert.SerializeObject(TempTable).ToString();
                ViewBag.cus_name = comm.Get_QueryData("ECB01_0000", customer_code, "CUSTOMER_CODE", "CUSTOMER_NAME");
                ViewBag.cus_fullname = comm.Get_QueryData("ECB01_0000", customer_code, "CUSTOMER_CODE", "CUSTOMER_FULL_NAME");
                ViewBag.fileName = ViewBag.cus_name + "_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xls";
                Save_FilesExcel(ViewBag.TempTable2, ViewBag.cus_name);
            }
            catch (Exception e)
            {
                ViewBag.respectcode = 1;
                Console.WriteLine("error: " + e.Message);
                ViewBag.Message = e.Message;
            }
            return View();
        }

        //public ActionResult Download_Data(string tmp, string cusname)
        //{
        //    //string tmp2 = tmp.Replace("&quot;", @"""");
        //    //DataTable tmp3 = (DataTable)JsonConvert.DeserializeObject(tmp2, (typeof(DataTable)));

        //    //DataTable exdt = new DataTable();
        //    //foreach (DataColumn column in tmp3.Columns)
        //    //{
        //    //    exdt.Columns.Add(column.ColumnName);
        //    //}
        //    //for (int i = 0; i < tmp3.Rows.Count; i++)
        //    //{
        //    //    DataRow drow = exdt.NewRow();
        //    //    for (int j = 0; j < exdt.Columns.Count; j++)
        //    //    {
        //    //        drow[j] = tmp3.Rows[i][j].ToString();
        //    //    }
        //    //    exdt.Rows.Add(drow);
        //    //}


        //    string odate = DateTime.Now.ToString("yyyyMMdd");
        //    string filename = cusname + "_" + odate + ".xls";

        //    var originalDirectory = new DirectoryInfo(string.Format("{0}Upload\\EC_EXCEL\\", Server.MapPath(@"\")));
        //    string pathString = System.IO.Path.Combine(originalDirectory.ToString());
        //    //System.IO.Directory.CreateDirectory(pathString);

        //    var path = pathString + filename;
        //    Download(path, filename);
        //    //Download(TableToExcel(exdt, path), filename);

        //    return Json("", JsonRequestBehavior.AllowGet);
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
        //public void Download(IWorkbook excel, string pRptName)
        //{
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        excel.Write(ms);
        //        byte[] byt = ms.ToArray();
        //        ms.Flush();
        //        ms.Close();
        //        Response.Clear();
        //        Response.AddHeader("Content-Disposition", "attachment;filename=" + pRptName);
        //        Response.AddHeader("Content-Length", byt.Length.ToString());
        //        Response.ContentType = "application/octet-stream";
        //        Response.BinaryWrite(byt);
        //        byt = null;
        //    }
        //}
        //public void Download(string path, string pRptName)
        //{
        //    FileStream stream = new FileStream(path,FileMode.Open);
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        stream.Write(ms.ToArray(), 0, ms.ToArray().Length);
        //        byte[] byt = ms.ToArray();
        //        stream.Flush();
        //        stream.Close();
        //        Response.Clear();
        //        Response.AddHeader("Content-Disposition", "attachment;filename=" + pRptName);
        //        Response.AddHeader("Content-Length", byt.Length.ToString());
        //        Response.ContentType = "application/octet-stream";
        //        Response.BinaryWrite(byt);
        //        byt = null;
        //    }
        //}

        //public List<T> DataTableToList<T>(DataTable dt) where T : class, new()
        //{
        //    List<T> list = new List<T>();
        //    T t = new T();
        //    PropertyInfo[] prop = t.GetType().GetProperties();
        //    //遍歷所有DataTable的行
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        t = new T();
        //        //通過反射獲取T類型的所有成員
        //        foreach (PropertyInfo pi in prop)
        //        {
        //            //DataTable列名=屬性名
        //            if (dt.Columns.Contains(pi.Name))
        //            {
        //                //屬性值不為空
        //                if (dr[pi.Name] != DBNull.Value)
        //                {
        //                    object value = Convert.ChangeType(dr[pi.Name], pi.PropertyType);
        //                    //給T類型字段賦值
        //                    pi.SetValue(t, value, null);
        //                }
        //            }
        //        }
        //        //將T類型添加到集合list
        //        list.Add(t);
        //    }
        //    return list;

        //}


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
        //    ECT02_0200 newData = new ECT02_0200();

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

        //    ECT02_0200 newData = repoECT02_0200.GetDTO(pTkCode);

        //    return View(newData);
        //}


        ///// <summary>
        ///// (修改區) 主檔的新增頁面將值存進DB
        ///// </summary>
        ///// <param name="form">畫面上輸入的form元件中的控制項集合</param>
        ///// <param name="model">要存檔的model</param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult Insert(FormCollection form, ECT02_0200 model)
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
        //        ECT02_0200 data = new ECT02_0200();
        //        comm.Set_ModelValue(data, form);

        //        //在取完畫面上的值後，如果有一些別名欄位要變更值，可以在這邊2次加工

        //        repoECT02_0200.InsertData(data);
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
        //public ActionResult Update(FormCollection form, ECT02_0200 model)
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
        //        ECT02_0200 data = new ECT02_0200();
        //        comm.Set_ModelValue(data, form);
        //        ECT02_0200 sBefore = comm.GetData<ECT02_0200>(data);
        //        repoECT02_0200.UpdateData(data);
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
        //    ECT02_0200 sBefore = comm.GetData<ECT02_0200>(pTkCode);
        //    repoECT02_0200.DeleteData(pTkCode);
        //    //刪除紀錄資料
        //    comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "delete", sBefore, "");
        //    return RedirectToAction("Index");
        //}
        /* 資料處理 向上 */

        //資料檢查 向下//
        //主檔的檢查
        [HttpPost]
        public ActionResult Check_Data(FormCollection form, ECT02_0200 model)
        {
            bool isSuccess = false;
            //檢查資料代碼是否重覆
            string key = gmv.GetKey<ECT02_0200>(new ECT02_0200());
            string sWhere = "where " + key + "='" + form[key].ToString() + "'";
            bool hasRow = !comm.Chk_RelData("ECT02_0200", sWhere);
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

        public void Save_FilesExcel(string sTemp, string sCusName)
        {
            string tmp2 = sTemp.Replace("&quot;", @"""");
            DataTable tmp3 = (DataTable)JsonConvert.DeserializeObject(tmp2, (typeof(DataTable)));

            DataTable exdt = new DataTable();
            foreach (DataColumn column in tmp3.Columns)
            {
                exdt.Columns.Add(column.ColumnName);
            }
            for (int i = 0; i < tmp3.Rows.Count; i++)
            {
                DataRow drow = exdt.NewRow();
                for (int j = 0; j < exdt.Columns.Count; j++)
                {
                    drow[j] = tmp3.Rows[i][j].ToString();
                }
                exdt.Rows.Add(drow);
            }


            string odate = DateTime.Now.ToString("yyyyMMdd_HHmm");
            string filename = sCusName + "_" + odate + ".xls";

            //創建資料夾
            var originalDirectory = new DirectoryInfo(string.Format("{0}Upload\\EC_EXCEL\\", Server.MapPath(@"\")));
            string pathString = System.IO.Path.Combine(originalDirectory.ToString());
            System.IO.Directory.CreateDirectory(pathString);

            var path = pathString + filename;
            TableToExcel(exdt, path);

        }
    }
}