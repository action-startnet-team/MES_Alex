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

namespace MES_WATER.Controllers
{
    [Authorize] //登入驗證
    [HandleError(View = "Error")]  //錯誤導向

    public class QMB020AController : JsonNetController
    {
        //程式代號
        string sPrgCode = "QMB020A";
        //需要用到的Repo
        QMB02_0000Repository repoQMB02_0000 = new QMB02_0000Repository();
        QMB03_0100Repository repoQMB03_0100 = new QMB03_0100Repository();
        //共用函式庫
        Comm comm = new Comm();
        GetData GD = new GetData();
        GetModelValidation gmv = new GetModelValidation();
        DynamicTable DT = new DynamicTable();


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

            List<QMB02_0000> list = new List<QMB02_0000>();
            list = repoQMB02_0000.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// (修改區) 主檔 新增
        /// 1.新增模式下控項的預設值在這邊設定
        /// </summary>
        /// <returns></returns>
        public ActionResult Insert()
        {
            //要結合權限控制
            ViewBag.prg_code = sPrgCode;

            //新增模式的預設值
            QMB02_0000 newData = new QMB02_0000();

            return View(newData);
        }

        /// <summary>
        /// (修改區) 主檔 修改
        /// 1.依資料鍵值到DB取回資料呈現在修改模式頁面
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        /// <returns></returns>
        public ActionResult Update(string pTkCode)
        {
            ViewBag.prg_code = sPrgCode;

            QMB02_0000 newData = repoQMB02_0000.GetDTO(pTkCode);

            return View(newData);
        }


        /// <summary>
        /// (修改區) 主檔的新增頁面將值存進DB
        /// </summary>
        /// <param name="form">畫面上輸入的form元件中的控制項集合</param>
        /// <param name="model">要存檔的model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Insert(FormCollection form, QMB02_0000 model)
        {
            // MVC model驗證
            if (ModelState.IsValid)
            {
                // 自訂義 資料驗證
                bool bIsOK = Chk_Ins_Main(form);

                // 資料驗證失敗
                if (!bIsOK)
                {
                    ViewBag.showErrMsg = true;
                    ViewBag.prg_code = sPrgCode;
                    return View(model);
                }

                //執行存檔
                QMB02_0000 data = new QMB02_0000();
                comm.Set_ModelValue(data, form);

                //在取完畫面上的值後，如果有一些別名欄位要變更值，可以在這邊2次加工
                data.usr_code = User.Identity.Name;
                data.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
                data.ins_time = DateTime.Now.ToString("HH:mm:ss");

                repoQMB02_0000.InsertData(data);
                // 新增紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);
                //存完檔回到主頁，如果不跳回主頁要在這裡做修改
                return RedirectToAction("Index");


            }
            ViewBag.showErrMsg = true;
            ViewBag.prg_code = sPrgCode;
            return View(model);
        }

        /// <summary>
        /// (修改區) 主檔的修改頁面將值存進DB
        /// </summary>
        /// <param name="form">畫面上輸入的form元件中的控制項集合</param>
        /// <param name="model">要存檔的model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(FormCollection form, QMB02_0000 model)
        {
            // MVC model驗證 資料格式檢查
            if (ModelState.IsValid)
            {
                // 自定義 資料邏輯檢查
                bool bIsOK = Chk_Upd_Main(form);

                // 資料驗證失敗
                if (!bIsOK)
                {
                    ViewBag.showErrMsg = true;
                    ViewBag.prg_code = sPrgCode;
                    return View(model);
                }

                //執行存檔
                QMB02_0000 data = new QMB02_0000();
                comm.Set_ModelValue(data, form);
                QMB02_0000 sBefore = comm.GetData<QMB02_0000>(data);
                repoQMB02_0000.UpdateData(data);
                //更新紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "update", sBefore, data);

                return RedirectToAction("Index");



            }
            ViewBag.showErrMsg = true;
            ViewBag.prg_code = sPrgCode;
            return View(model);
        }

        /// <summary>
        /// (修改區) 按下刪除後刪除DB動作
        /// </summary>
        /// <param name="pTkCode">要刪除的鍵值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(string pTkCode)
        {
            //刪除前的檢查要在JqGrid送出前檢查，所以對應Chk_Del_Main這個函數
            QMB02_0000 sBefore = comm.GetData<QMB02_0000>(pTkCode);
            repoQMB02_0000.DeleteData(pTkCode);
            //刪除紀錄資料
            comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "delete", sBefore, "");

            return RedirectToAction("Index");
        }

        public ActionResult Insert_D1(string QsheetCode)
        {
            //要結合權限控制
            ViewBag.prg_code = sPrgCode;

            // 使用者, controllerName, actionName
            string usr_code = User.Identity.Name;
            //string prg_code = sPrgCode;
            //string view_code = ControllerContext.RouteData.Values["action"].ToString();

            //取得欄位寬度
            List<BDP30_0000> colWidth_list = comm.Get_BDP30_0000(usr_code, "QMB020A", "Index");
            ViewBag.colWidth_list = colWidth_list;

            //取得欄位顯示
            List<BDP30_0100> is_show_list = comm.Get_BDP30_0100(usr_code, "QMB020A", "Index");
            ViewBag.is_show_list = is_show_list;

            ViewBag.QsheetCode = QsheetCode;

            return View();
        }

        public ActionResult Get_InsertData_D1(string pWhere)
        {
            string sUsrCode = User.Identity.Name;

            List<QMB02_0000> list = new List<QMB02_0000>();
            list = repoQMB02_0000.Get_DataListByQuery(sUsrCode, "QMB020A", pWhere);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Insert_D1(string QsheetCode, FormCollection form)
        {
            object data = new object();
            if (!string.IsNullOrEmpty(form["item_code"]))
            {
                var all_item_code = form["item_code"].Split(',');
                for (int i = 0; i < all_item_code.Length; i++)
                {
                    //QMB03_0100 data = new QMB03_0100();
                    //data.qsheet_code = QsheetCode;
                    //data.qtest_item_code = all_item_code[i];
                    //data.scr_no = Get_ScrNo(QsheetCode);
                    //data.datacode = QsheetCode + "-" + data.scr_no;
                    //repoQMB03_0100.InsertData(data);

                    string sQtestItemCode = all_item_code[i];
                    string sScrNo = Get_ScrNo(QsheetCode);
                    string sQtestUp = comm.Get_Data("QMB02_0000", sQtestItemCode, "qtest_item_code", "qtest_up"); //規格上限
                    string sQtestDown = comm.Get_Data("QMB02_0000", sQtestItemCode, "qtest_item_code", "qtest_down"); //允差下限
                    string sQtestItemType = comm.Get_Data("QMB02_0000", sQtestItemCode, "qtest_item_code", "qtest_item_type"); //檢驗項目類別

                    data = new {
                        qsheet_code = QsheetCode,
                        qtest_item_code = sQtestItemCode,
                        scr_no = sScrNo,
                        datacode = QsheetCode + "-" + sScrNo,
                        qtest_up = sQtestUp,
                        qtest_down = sQtestDown,
                        qtest_item_type = sQtestItemType,
                    };
                    DT.InsertData("QMB03_0100", data);
                }
            }

            return RedirectToAction("Update", "QMB030A", new {pTkCode = QsheetCode });
        }

        public string Get_ScrNo(string qsheet_code)
        {
            string scr_no = "0010";
            string sSql = " select MAX (scr_no) as scr_no from QMB03_0100 where qsheet_code = @qsheet_code ";
            var dtTmp = comm.Get_DataTable(sSql, "qsheet_code", qsheet_code);
            string max_scr_no = GD.DataFieldToStr(dtTmp, "scr_no");
            if (max_scr_no != "")
            {
                int i = comm.sGetInt32(max_scr_no.Substring(0, 3) + "0") + 10;
                //if (i<10)
                //{
                //    scr_no = "00" + Convert.ToString(i) + "0";
                //}
                //else
                //{
                //    scr_no = "0" + Convert.ToString(i) + "0";
                //}
                scr_no = i.ToString().PadLeft(4, '0');
            }

            return (scr_no);
        }

        /* 資料處理 向上 */

        //資料檢查 向下//
        //主檔的檢查
        [HttpPost]
        public ActionResult Check_Data(FormCollection form, QMB02_0000 model)
        {
            bool isSuccess = false;
            //檢查資料代碼是否重覆
            string key = gmv.GetKey<QMB02_0000>(new QMB02_0000());
            string sWhere = "where " + key + "='" + form[key].ToString() + "'";
            bool hasRow = !comm.Chk_RelData("QMB02_0000", sWhere);
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



        /// <summary>
        /// 1.將QMB02_0000整合成不包含pro_code的檢驗項目，放置於QMB02_0000_temp
        /// 2.根據QMB02_0000所包含的pro_code為其在QMB03_0000建立檢驗表(一物料一檢驗表)
        /// 3.將QMB02_0000檢驗項目附帶上下限寫進QMB03_0100
        /// 4.QMB02_0000_temp覆蓋掉QMB02_0000
        /// </summary>
        /// <returns></returns>
        public ActionResult AllReSet()
        {
            string sSql = "";
            string qtest_item_code = "";
            string pro_code = "";

            //1.先刪除QMB02_0000_temp資料，將QMB02_0000整合成不包含pro_code的檢驗項目，放置於QMB02_0000_temp
            //刪除原資料
            sSql = "DELETE FROM QMB02_0000_temp ";
            using (SqlConnection con_db = comm.Set_DBConnection()) { con_db.Execute(sSql); }
            //抓取要寫入的資料
            DataTable dtQMB02_0000 = comm.Get_DataTable(" select distinct qtest_item_name, qtest_item_type from QMB02_0000 order by qtest_item_name ");
            //寫入資料
            for (int k = 0; k < dtQMB02_0000.Rows.Count; k++)
            {
                int k2 = k + 1;
                if (k2 < 10)
                    qtest_item_code = "00000" + k2.ToString();
                else if (k2 < 100)
                    qtest_item_code = "0000" + k2.ToString();
                else if (k2 < 1000)
                    qtest_item_code = "000" + k2.ToString();
                else if (k2 < 10000)
                    qtest_item_code = "00" + k2.ToString();
                else if (k2 < 100000)
                    qtest_item_code = "0" + k2.ToString();
                else
                    qtest_item_code = k2.ToString();

                QMB02_0000 QMB02_0000 = new QMB02_0000();
                QMB02_0000.qtest_item_code = qtest_item_code;
                QMB02_0000.pro_code = "";
                QMB02_0000.qtest_item_name = dtQMB02_0000.Rows[k]["qtest_item_name"].ToString();
                QMB02_0000.qtest_type_code = "";
                QMB02_0000.qtest_item_memo = "";
                QMB02_0000.qtest_up = "0";
                QMB02_0000.qtest_down = "0";
                QMB02_0000.qtest_item_type = dtQMB02_0000.Rows[k]["qtest_item_type"].ToString();
                QMB02_0000.is_use = "Y";
                QMB02_0000.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
                QMB02_0000.ins_time = DateTime.Now.ToString("HH:mm:ss");
                QMB02_0000.usr_code = "";

                DT.InsertData("QMB02_0000_temp", QMB02_0000);
            }

            //2.清掉QMB03_0000，再根據原QMB02_0000所包含的pro_code為其在QMB03_0000建立檢驗表(一物料一檢驗表)
            //刪除原資料
            sSql = "DELETE FROM QMB03_0000 ";
            using (SqlConnection con_db = comm.Set_DBConnection()) { con_db.Execute(sSql); }
            sSql = "DELETE FROM QMB03_0200 ";
            using (SqlConnection con_db = comm.Set_DBConnection()) { con_db.Execute(sSql); }
            //抓取要寫入的資料
            DataTable QMB02_pro_code = comm.Get_DataTable("select distinct pro_code from QMB02_0000");
            
            //寫入資料
            for (int l = 0; l < QMB02_pro_code.Rows.Count; l++)
            {
                QMB03_0000 QMB03_0000 =  new QMB03_0000();
                QMB03_0000.qsheet_code = "QS"+ QMB02_pro_code.Rows[l]["pro_code"].ToString();
                QMB03_0000.qsheet_name = comm.Get_QueryData("MEB20_0000", QMB02_pro_code.Rows[l]["pro_code"].ToString(), "pro_code", "pro_name");
                QMB03_0000.qsheet_memo = "";
                QMB03_0000.qsheet_type = "IQC";
                QMB03_0000.qtest_level_code = "";
                QMB03_0000.ins_level_code = "";
                QMB03_0000.work_code = "";
                QMB03_0000.epb_code = "";
                QMB03_0000.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
                QMB03_0000.ins_time = DateTime.Now.ToString("HH:mm:ss");
                QMB03_0000.usr_code = "";

                QMB03_0200 QMB03_0200 = new QMB03_0200();
                QMB03_0200.qsheet_code = "QS" + QMB02_pro_code.Rows[l]["pro_code"].ToString();
                QMB03_0200.pro_code = QMB02_pro_code.Rows[l]["pro_code"].ToString();

                DT.InsertData("QMB03_0000", QMB03_0000);
                DT.InsertData("QMB03_0200", QMB03_0200);
                
            }

            //3.將QMB02_0000檢驗項目附帶上下限寫進QMB03_0100
            string scr_no = "";
            //刪除原資料
            sSql = "DELETE FROM QMB03_0100 ";
            using (SqlConnection con_db = comm.Set_DBConnection()){con_db.Execute(sSql);}
            //抓取要寫入的資料
            DataTable dt = comm.Get_DataTable(" select QMB03_0000.qsheet_code, QMB03_0200.pro_code " +
                                              " from QMB03_0000 " +
                                              " left join QMB03_0200 on QMB03_0200.qsheet_code = QMB03_0000.qsheet_code");
            //寫入資料
            DataTable dt2;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                pro_code = dt.Rows[i]["pro_code"].ToString();
                dt2 = comm.Get_DataTable(" select QMB02_0000.qtest_item_name, QMB02_0000.qtest_item_type, QMB02_0000.qtest_up, QMB02_0000.qtest_down, QMB02_0000_temp.qtest_item_code " +
                                         " from QMB02_0000 " +
                                         " left join QMB02_0000_temp on QMB02_0000_temp.qtest_item_name = QMB02_0000.qtest_item_name " +
                                         " where QMB02_0000.pro_code = '" + pro_code + "'");
                
                for (int j = 0; j < dt2.Rows.Count; j++)
                {
                    int j2 = j + 1;
                    if (j2 < 10)
                        scr_no = "00" + j2.ToString() + "0";
                    else if (j2 >= 10)
                        scr_no = "0" + j2.ToString() + "0";
                    else
                        scr_no = j2.ToString() + "0";

                    QMB03_0100 QMB03_0100 = new Models.QMB03_0100();
                    QMB03_0100.qsheet_code = dt.Rows[i]["qsheet_code"].ToString();
                    QMB03_0100.qtest_item_code = dt2.Rows[j]["qtest_item_code"].ToString();
                    QMB03_0100.qtest_item_type = dt2.Rows[j]["qtest_item_type"].ToString();
                    QMB03_0100.scr_no = scr_no;
                    QMB03_0100.datacode = QMB03_0100.qsheet_code + "-" + scr_no;
                    QMB03_0100.qtest_up = comm.sGetDecimal(dt2.Rows[j]["qtest_up"].ToString());
                    QMB03_0100.qtest_down = comm.sGetDecimal(dt2.Rows[j]["qtest_down"].ToString());

                    DT.InsertData("QMB03_0100", QMB03_0100);
                    
                }

            }

            //4..QMB02_0000_temp覆蓋掉QMB02_0000
            //刪除原資料
            sSql = "DELETE FROM QMB02_0000 ";
            using (SqlConnection con_db = comm.Set_DBConnection()) { con_db.Execute(sSql); }
            //抓取要寫入的資料
            DataTable dtQMB02_0000_temp = comm.Get_DataTable("select * from QMB02_0000_temp");
            //寫入資料
            for (int m = 0; m < dtQMB02_0000_temp.Rows.Count; m++)
            {
                QMB02_0000 QMB02_0000 = new QMB02_0000();
                QMB02_0000.qtest_item_code = dtQMB02_0000_temp.Rows[m]["qtest_item_code"].ToString();
                QMB02_0000.qtest_item_name = dtQMB02_0000_temp.Rows[m]["qtest_item_name"].ToString();
                QMB02_0000.pro_code = dtQMB02_0000_temp.Rows[m]["pro_code"].ToString();
                QMB02_0000.qtest_type_code = dtQMB02_0000_temp.Rows[m]["qtest_type_code"].ToString();
                QMB02_0000.qtest_item_memo = dtQMB02_0000_temp.Rows[m]["qtest_item_memo"].ToString();
                QMB02_0000.qtest_up = dtQMB02_0000_temp.Rows[m]["qtest_up"].ToString();
                QMB02_0000.qtest_down = dtQMB02_0000_temp.Rows[m]["qtest_down"].ToString();
                QMB02_0000.qtest_item_type = dtQMB02_0000_temp.Rows[m]["qtest_item_type"].ToString();
                QMB02_0000.is_use = dtQMB02_0000_temp.Rows[m]["is_use"].ToString();
                QMB02_0000.ins_date = dtQMB02_0000_temp.Rows[m]["ins_date"].ToString();
                QMB02_0000.ins_time = dtQMB02_0000_temp.Rows[m]["ins_time"].ToString();
                QMB02_0000.usr_code = dtQMB02_0000_temp.Rows[m]["usr_code"].ToString();

                DT.InsertData("QMB02_0000", QMB02_0000);
                
            }



            return RedirectToAction("Index");
        }




    }
}