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
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Dapper;

namespace MES_WATER.Controllers
{
    [Authorize] //登入驗證
    [HandleError(View = "Error")]  //錯誤導向

    public class QMT050AController : JsonNetController
    {
        //程式代號
        public static string sPrgCode = "QMT050A";

        //需要用到的Repo
        QMT05_0000Repository repoQMT05_0000 = new QMT05_0000Repository();
        QMT05_0100Repository repoQMT05_0100 = new QMT05_0100Repository();

        //共用函式庫
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();
        DynamicTable DT = new DynamicTable();
        CheckData CD = new CheckData();
        GetData GD = new GetData();

        //取得單號
        WebReference.WmsApi ws = new WebReference.WmsApi();


        /* 資料處理 向下 */
        /// <summary>
        /// (固定區) 主檔 首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //要結合權限控制
            ViewBag.limit_str = comm.Get_LimitByUsrCode(User.Identity.Name, sPrgCode);
            //comm.ConvertNull("QMT05_0000");

            // 使用者, controllerName, actionName
            string usr_code = User.Identity.Name;
            string prg_code = ControllerContext.RouteData.Values["controller"].ToString();
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
            ViewBag.prg_code = sPrgCode;
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
            //string sPrgCode = pubPrgCode;

            List<QMT05_0000> list = new List<QMT05_0000>();
            list = repoQMT05_0000.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // 明細檔 jqgrid資料來源
        [HttpPost]
        public ActionResult Get_GridData_D1(string pTkCode)
        {
            //string sPrgCode = pubPrgCode;
            string sUsrCode = User.Identity.Name;
            List<QMT05_0100> list = new List<QMT05_0100>();
            list = repoQMT05_0100.Get_DataList(sUsrCode, sPrgCode, pTkCode);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 成立檢驗批，將資料寫入QMT05_0000
        /// </summary>
        /// <param name="wmt0200"></param>
        /// <returns></returns>
        public ActionResult SetUpQTestLot(int med09_0000)
        {
            QMT05_0000 data = new QMT05_0000();
            data.ipqm_code = comm.Get_TkCode(sPrgCode);
            data.med09_0000 = med09_0000;
            data.mo_code = comm.Get_QueryData("MED09_0000", med09_0000.ToString(), "med09_0000", "mo_code");
            data.pro_code = comm.Get_QueryData("MED09_0000", med09_0000.ToString(), "med09_0000", "pro_code");
            data.lot_no = comm.Get_QueryData("MED09_0000", med09_0000.ToString(), "med09_0000", "lot_no");
            data.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
            data.ins_time = DateTime.Now.ToString("HH:mm:ss");
            data.usr_code = User.Identity.Name;
            repoQMT05_0000.InsertData(data);
            string qsheet_code = comm.Get_QueryData("QMB03_0200", data.pro_code, "pro_code", "qsheet_code");
            if (String.IsNullOrEmpty(qsheet_code))
            {
                return RedirectToAction("Index", "QMB050A");
            }
            else
            {
                DataTable dt = comm.Get_DataTable("select * from QMB03_0100 where qsheet_code = '" + qsheet_code + "'");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    QMT05_0100 data2 = new QMT05_0100();
                    data2.ipqm_code = data.ipqm_code;
                    data2.qtest_item_code = dt.Rows[i]["qtest_item_code"].ToString();
                    data2.scr_no = dt.Rows[i]["scr_no"].ToString();
                    data2.qtest_up = comm.sGetDecimal(dt.Rows[i]["qtest_up"].ToString());
                    data2.qtest_down = comm.sGetDecimal(dt.Rows[i]["qtest_down"].ToString());
                    data2.qtest_item_type = dt.Rows[i]["qtest_item_type"].ToString();
                    data2.is_ok = "Y";
                    data2.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
                    data2.ins_time = DateTime.Now.ToString("HH:mm:ss");
                    data2.usr_code = User.Identity.Name;
                    data2.sample_sum_qty = 0;
                    repoQMT05_0100.InsertData(data2);
                }

            }

            return RedirectToAction("Index", "QMB050A");
        }

        [HttpPost]
        public string Chk_InsLevelCode(string ins_level_code)
        {
            string chk = "N";
            if (!string.IsNullOrEmpty(comm.Get_QueryData("QMB10_0000", ins_level_code, "ins_level_code", "ins_level_code")))
            {
                chk = "Y";
            }
            return chk;
        }

        [HttpPost]
        public JsonResult SetUpQTestLot2(string pRowDatas, string ins_level_code)
        {
            // 自訂義資料檢查開始.
            bool bIsOK = true;
            DataTable dtTmp = JsonConvert.DeserializeObject<DataTable>(pRowDatas);

            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                QMT05_0000 data = new QMT05_0000();
                data.ipqm_code = comm.Get_TkCode(sPrgCode);
                data.med09_0000 = comm.sGetInt32(dtTmp.Rows[i]["med09_0000"].ToString());
                data.mo_code = dtTmp.Rows[i]["mo_code"].ToString();
                data.pro_code = dtTmp.Rows[i]["pro_code"].ToString();
                data.lot_no = dtTmp.Rows[i]["lot_no"].ToString();
                data.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
                data.ins_time = DateTime.Now.ToString("HH:mm:ss");
                data.usr_code = User.Identity.Name;
                repoQMT05_0000.InsertData(data);

                string qsheet_code = comm.Get_QueryData("QMB03_0200", data.pro_code, "pro_code", "qsheet_code");
                if (String.IsNullOrEmpty(qsheet_code))
                {
                    bIsOK = false;
                    return Json(bIsOK);
                }
                else
                {
                    DataTable dt = comm.Get_DataTable("select * from QMB03_0100 where qsheet_code = '" + qsheet_code + "'");
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        QMT05_0100 data2 = new QMT05_0100();
                        data2.ipqm_code = data.ipqm_code;
                        data2.qtest_item_code = dt.Rows[j]["qtest_item_code"].ToString();
                        data2.scr_no = dt.Rows[j]["scr_no"].ToString();
                        data2.qtest_up = comm.sGetDecimal(dt.Rows[j]["qtest_up"].ToString());
                        data2.qtest_down = comm.sGetDecimal(dt.Rows[j]["qtest_down"].ToString());
                        data2.qtest_item_type = dt.Rows[j]["qtest_item_type"].ToString();
                        data2.is_ok = dt.Rows[j]["is_ok"].ToString();
                        data2.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
                        data2.ins_time = DateTime.Now.ToString("HH:mm:ss");
                        data2.usr_code = User.Identity.Name;
                        data2.sample_sum_qty = 0;
                        repoQMT05_0100.InsertData(data2);
                    }
                }

            }
            return Json(bIsOK);
        }


        /// <summary>
        /// (修改區) 主檔 新增
        /// 1.新增模式下控項的預設值在這邊設定
        /// </summary>
        /// <returns></returns>
        public ActionResult Insert(string pRptCode = "")
        {
            //新增模式的預設值
            QMT05_0000 data = new QMT05_0000();

            if (pRptCode != "")
            {
                string sSql = " select top(1) med09_0000,mo_code,pro_code,pro_lot_no from MED09_0000  " +
              "  WHERE med09_0000='" + pRptCode + "' "+
                " order by med09_0000 desc ";
                DataTable dTmp = comm.Get_DataTable(sSql);
                data.lot_no = dTmp.Rows[0]["pro_lot_no"].ToString();
                data.pro_code = dTmp.Rows[0]["pro_code"].ToString();
                data.mo_code = dTmp.Rows[0]["mo_code"].ToString();
                data.med09_0000 = int.Parse(dTmp.Rows[0]["med09_0000"].ToString());


            }

            return View(data);
        }

        /// <summary>
        /// (修改區) 主檔 修改
        /// 1.依資料鍵值到DB取回資料呈現在修改模式頁面
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        /// <returns></returns>
        public ActionResult Update(string pTkCode)
        {
            QMT05_0000 newData = repoQMT05_0000.GetDTO(pTkCode);
            newData.pro_name = comm.Get_QueryData("MEB20_0000", newData.pro_code, "pro_code", "pro_name");
            return View(newData);
        }

        /// <summary>
        /// (修改區) 主檔的新增頁面將值存進DB
        /// </summary>
        /// <param name="form">畫面上輸入的form元件中的控制項集合</param>
        /// <param name="model">要存檔的model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Insert(FormCollection form, QMT05_0000 model)
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
                    return View(model);
                }
                // 初始
                QMT05_0000 data = new QMT05_0000();

                // 預設賦值
                comm.Set_ModelValue(data, form);

                // 特別取值
                DataTable dtemp = comm.Get_DataTable("select top(1) med09_0000 from MED09_0000 where mo_code ='"+data.mo_code+"' order by med09_0000 desc");
                if (dtemp.Rows.Count > 0 && data.med09_0000==0) data.med09_0000 = int.Parse(dtemp.Rows[0]["med09_0000"].ToString());
                data.ipqm_code = comm.Get_TkCode(sPrgCode);
                data.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
                data.ins_time = DateTime.Now.ToString("HH:mm:ss");
                data.usr_code = User.Identity.Name;

                repoQMT05_0000.InsertData(data);


                //DataTable dQsheet_code = comm.Get_DataTable(@"
                //    select qsheet_code from QMB03_0000 a
                //    where qsheet_type='IPQC'
                //");
                string qsheet_code = comm.Get_QueryData("QMB03_0200", data.pro_code, "pro_code", "qsheet_code");
                DataTable dt = comm.Get_DataTable("select  QMB03_0100.*  from QMB03_0000 left join  QMB03_0100 on QMB03_0000.qsheet_code = qmb03_0100.qsheet_code " +
                    " where qsheet_type = 'IPQC' and  pro_code='" + data.pro_code + "'");
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    QMT05_0100 data2 = new QMT05_0100();
                    data2.ipqm_code = data.ipqm_code;
                    data2.qtest_item_code = dt.Rows[j]["qtest_item_code"].ToString();
                    data2.scr_no = dt.Rows[j]["scr_no"].ToString();
                    data2.is_ok = "1";
                    data2.qtest_up = comm.sGetDecimal(dt.Rows[j]["qtest_up"].ToString());
                    data2.qtest_down = comm.sGetDecimal(dt.Rows[j]["qtest_down"].ToString());
                    data2.sample_sum_qty = 0;
                    data2.qtest_item_type = dt.Rows[j]["qtest_item_type"].ToString();
                    data2.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
                    data2.ins_time = DateTime.Now.ToString("HH:mm:ss");
                    data2.usr_code = User.Identity.Name;
                    repoQMT05_0100.InsertData(data2);
                }



                // 新增紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);
                return RedirectToAction("Update", sPrgCode, new { pTkCode = data.ipqm_code });
            }
            ViewBag.showErrMsg = true;
            return View(model);
        }

        /// <summary>
        /// (修改區) 明細jqGrid的新增處理，將值存進DB
        /// </summary>
        /// <param name="form">畫面上輸入的form元件中的控制項集合</param>
        /// <returns></returns>
        [HttpPost]
        public void Insert_D1(FormCollection form)
        {
            QMT05_0100 data = new QMT05_0100();

            comm.Set_ModelValue(data, form);
            
            data.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
            data.ins_time = DateTime.Now.ToString("HH:mm:ss");
            data.usr_code = User.Identity.Name;

            repoQMT05_0100.InsertData(data);
            // 新增紀錄資料
            comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);
        }
        /// <summary>
        /// (修改區) 主檔 修改
        /// 1.依資料鍵值到DB取回資料呈現在修改模式頁面
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(FormCollection form, QMT05_0000 model)
        {
            // MVC model驗證
            if (ModelState.IsValid)
            {
                // 自訂義 資料驗證
                bool bIsOK = Chk_Upd_Main(form);

                // 資料驗證失敗
                if (!bIsOK)
                {
                    ViewBag.showErrMsg = true;
                    return View(model);
                }

                QMT05_0000 data = new QMT05_0000();

                comm.Set_ModelValue(data, form);

                QMT05_0000 sBefore = comm.GetData<QMT05_0000>(data);
                repoQMT05_0000.UpdateData(data);
                //更新紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "update", sBefore, data);

                return RedirectToAction("Index");
            }
            ViewBag.showErrMsg = true;
            return View(model);
        }

        /// <summary>
        /// (修改區) 明細jqGrid的修改處理，將值存進DB
        /// </summary>
        /// <param name="form">畫面上輸入的form元件中的控制項集合</param>
        /// <param name="model">要存檔的model</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Update_D1(FormCollection form)
        {
            CheckDataResult result = Chk_Upd_D1(form);
            if (result.bIsOK)
            {
                QMT05_0100 data = new QMT05_0100();

                comm.Set_ModelValue(data, form);


                //QMT05_0100 sBefore = comm.GetData<QMT05_0100>(data);
                repoQMT05_0100.UpdateData(data);
                //更新紀錄資料
                //comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "update", sBefore, data);

            }
            return Json(result);
        }

        /// <summary>
        /// (修改區) 按下刪除後刪除DB動作
        /// </summary>
        /// <param name="pTkCode">要刪除的鍵值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(string pTkCode)
        {
            //刪主檔
            QMT05_0000 sBefore = comm.GetData<QMT05_0000>(pTkCode);
            repoQMT05_0000.DeleteData(pTkCode);
            //刪除紀錄資料
            comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "delete", sBefore, "");
            return RedirectToAction("Index");
        }

        /// <summary>
        /// (修改區) 明細jqGrid的刪除DB動作
        /// </summary>
        /// <param name="form">畫面上輸入的form元件中的控制項集合</param>
        /// <param name="model">要存檔的model</param>
        /// <returns></returns>
        [HttpPost]
        public void Delete_D1(String pTkCode)
        {
            QMT05_0100 sBefore = comm.GetData<QMT05_0100>(pTkCode);
            repoQMT05_0100.DeleteData(pTkCode);
            //刪除紀錄資料
            comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "delete", sBefore, "");
        }

        /* 資料處理 向上 */

        //資料檢查 向下//
        //主檔的檢查
        [HttpPost]
        public ActionResult Check_Data(FormCollection form, QMT05_0000 model)
        {
            bool isSuccess = false;
            //檢查資料代碼是否重覆
            string key = gmv.GetKey<QMT05_0000>(new QMT05_0000());
            string sWhere = "where " + key + "='" + form[key].ToString() + "'";
            bool hasRow = !comm.Chk_RelData("QMT05_0000", sWhere);
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

        //資料檢查 向上//

        /* 資料檢查 向下 */
        // 主檔 新增資料的檢查
        private bool Chk_Ins_Main(FormCollection form)
        {
            bool bIsOK = true;

            //** 依作業不同有不同的檢查點 向下

            //if (form["atn_name"].Contains("姓名2"))
            //{
            //    bIsOK = false;
            //    ModelState.AddModelError("atn_name", "聯絡人是姓名2，不可新增");
            //}

            //** 依作業不同有不同的檢查點 向上

            //檢查結果回傳
            return bIsOK;
        }

        // 主檔 修改資料的檢查
        private bool Chk_Upd_Main(FormCollection form)
        {
            // 自訂義資料檢查開始
            bool bIsOK = true;

            //** 依作業不同有不同的檢查點 向下
            //if (form["per_code"] == "A79")
            //{
            //    bIsOK = false;
            //    ModelState.AddModelError("per_code", "採購人員是A79 - " + comm.Get_QueryData("STB18_0000", form["per_code"], "per_code", "per_name") + "，不可修改");
            //}
            //** 依作業不同有不同的檢查點 向上
            return bIsOK;
        }

        // 主檔 刪除資料的檢查
        [HttpPost]
        public JsonResult Chk_Del_Main(FormCollection form)
        {
            bool bIsOK = true;
            string message = "";

            //** 依作業不同有不同的檢查點 向下
            //if (comm.sGetInt32(form["pur_amt"]) > 0)
            //{
            //    bIsOK = false;
            //    message += "<div class='text-danger'>";
            //    message += "<li> 金額大於零，不可刪除 </li>";
            //    message += "</div>";
            //}
            //** 依作業不同有不同的檢查點 向上

            var result = new
            {
                bIsOK = bIsOK,
                message = message
            };

            return Json(result);
        }

        // 明細檔 新增資料的檢查
        [HttpPost]
        public ActionResult Chk_Ins_D1(FormCollection form)
        {
            bool bIsOK = true;
            string message = "";


            //檢查結果回傳
            var result = new
            {
                bIsOK = bIsOK,
                message = message
            };
            return Json(result);
        }

        // 明細檔 刪除資料的檢查
        [HttpPost]
        public JsonResult Chk_Del_D1(FormCollection form)
        {
            bool bIsOK = true;
            string message = "";

            //** 依作業不同有不同的檢查點 向下
            //if (comm.sGetInt32(form["res_qty"]) > 0)
            //{
            //    bIsOK = false;
            //    message += "<div class='text-danger'>";
            //    message += "<li> 已進貨量大於零，不可刪除 </li>";
            //    message += "</div>";
            //}
            //** 依作業不同有不同的檢查點 向上

            var result = new
            {
                bIsOK = bIsOK,
                message = message
            };
            return Json(result);
        }

        // 明細檔 修改資料的檢查
        public CheckDataResult Chk_Upd_D1(FormCollection form)
        {
            CheckDataResult result = new CheckDataResult();
            result.bIsOK = true;

            return result;
        }

        /* 資料檢查 向上 */

        public string GetTypeName(string code)
        {
            string type_name = comm.Get_BDP21_0000("qtest_item_type", code);
            return type_name;
        }

        public ActionResult SetQMTValue()
        {
            return View();
        }

        public DataTable Get_QmtValue_DataTable(string qmt04_0100)
        {
            string sSql = "";
            sSql = "select QMT05_0110.*,qtest_item_name" +
                   "  from QMT05_0110" +
                   "  left join QMB02_0000 on QMT05_0110.qtest_item_code = QMB02_0000.qtest_item_code" +
                   " where qmt04_0100 = @qmt04_0100" +
                   "  order by QMT05_0110.scr_no desc";
            DataTable dtTmp = comm.Get_DataTable(sSql, "qmt04_0100", qmt04_0100);
            //改排序
            dtTmp.Columns["qtest_item_name"].SetOrdinal(3);
            return dtTmp;
        }

        public string Get_QmtValue(string qmt04_0100)
        {
            DataTable dtTmp = Get_QmtValue_DataTable(qmt04_0100);
            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                DataRow r = dtTmp.Rows[i];
                for (int u = 0; u < dtTmp.Columns.Count; u++)
                {
                    DataColumn Col = dtTmp.Columns[u];
                    string sField = Col.ToString();
                    string sValue = r[sField].ToString();

                    //switch (sField)
                    //{
                    //    case "qmt_value":                        
                    //        break;                       
                    //}
                }
            }
            if (dtTmp.Rows.Count > 0)
            {
                //刪除不要的欄位
                //dtTmp.Columns.Remove("qmt04_0110");
                //dtTmp.Columns.Remove("qmt04_0100");
            }
            return JsonConvert.SerializeObject(dtTmp);
        }


        public void Delete_QmtValue(string qmt04_0110)
        {
            string qmt04_0100 = comm.Get_Data("QMT05_0110", qmt04_0110, "qmt04_0110", "qmt04_0100");
            comm.Del_QueryData("QMT05_0110", "qmt04_0110", qmt04_0110);
            Set_SumSampleQty(qmt04_0100);
        }

        public void Update_QmtValue(FormCollection form)
        {
            object data = new object();
            string sSql = "";
            DataTable dtTmp = new DataTable();
            string qmt04_0110 = comm.sGetString(form["qmt04_0110"]);
            string sQmtValue = comm.sGetString(form["qmt_value"]);
            string qmt04_0100 = comm.Get_Data("QMT05_0110", qmt04_0110, "qmt04_0110", "qmt04_0100");

            sSql = @"select * 
                       from QMT05_0100
                      where qmt04_0100 = @qmt04_0100
                    ";
            dtTmp = comm.Get_DataTable(sSql, "qmt04_0100", qmt04_0100);
            if (dtTmp.Rows.Count > 0)
            {
                DataRow r = dtTmp.Rows[0];
                string sQtestItemType = r["qtest_item_type"].ToString();
                decimal dQtestUp = comm.sGetDecimal(r["qtest_up"].ToString());
                decimal dQtestDown = comm.sGetDecimal(r["qtest_down"].ToString());
                string sIsOk = "";

                //根據檢驗類型不同給予不同的判斷方式
                switch (sQtestItemType)
                {
                    case "B":
                        sIsOk = CD.Chk_QmtValue_IsOk(comm.sGetDecimal(sQmtValue), dQtestUp, dQtestDown);
                        break;
                    case "C":
                        switch (sQmtValue)
                        {
                            case "Y":
                                sIsOk = "Y";
                                break;
                            case "N":
                                sIsOk = "N";
                                break;
                        }
                        break;
                }
                data = new
                {
                    qmt04_0110 = qmt04_0110,
                    qmt_value = sQmtValue,
                    is_ok = sIsOk,
                };
                DT.UpdateData("QMT05_0110", "qmt04_0110", data);

                //加總樣品數
                Set_SumSampleQty(qmt04_0100);
            }
        }

        public void Insert_QmtValue(FormCollection form, string pTkCode)
        {
            object data = new object();
            string qmt04_0100 = pTkCode;
            string sQtestItemCode = comm.Get_Data("QMT05_0100", qmt04_0100, "qmt04_0100", "qtest_item_code");
            string sQtestItemType = comm.Get_Data("QMT05_0100", qmt04_0100, "qmt04_0100", "qtest_item_type");
            string sQmtValue = comm.sGetString(form["qmt_value"]);

            data = new
            {
                qmt04_0100 = qmt04_0100,
                qtest_item_code = sQtestItemCode,
                qmt_value = sQmtValue,
                ins_date = DateTime.Now.ToString("yyyy/MM/dd"),
                ins_time = DateTime.Now.ToString("HH:mm:ss"),
                usr_code = User.Identity.Name,
                sample_qty = 1,
                scr_no = GD.Get_MaxScrNo("QMT05_0110", "where qmt04_0100=" + qmt04_0100, "scr_no") + 1,
            };
            DT.InsertData("QMT05_0110", data);
            Set_SumSampleQty(qmt04_0100);
        }

        public void Insert_QmtValue_batch(string pTkCode, int pBatchQty)
        {
            object data = new object();
            string qmt04_0100 = pTkCode;
            string sQtestItemCode = comm.Get_Data("QMT05_0100", qmt04_0100, "qmt04_0100", "qtest_item_code");
            string sQtestItemType = comm.Get_Data("QMT05_0100", qmt04_0100, "qmt04_0100", "qtest_item_type");
            string sQmtValue = "";
            decimal dSampleQty = 0;

            switch (sQtestItemType)
            {
                case "B":
                    sQmtValue = "0";
                    dSampleQty = 1;
                    break;
                case "C":
                    sQmtValue = "N";
                    dSampleQty = pBatchQty;
                    break;
            }
            for (int i = 0; i < pBatchQty / dSampleQty; i++)
            {
                data = new
                {
                    qmt04_0100 = qmt04_0100,
                    qtest_item_code = sQtestItemCode,
                    qmt_value = sQmtValue,
                    ins_date = DateTime.Now.ToString("yyyy/MM/dd"),
                    ins_time = DateTime.Now.ToString("HH:mm:ss"),
                    usr_code = User.Identity.Name,
                    sample_qty = dSampleQty,
                    scr_no = GD.Get_MaxScrNo("QMT05_0110", "where qmt04_0100=" + qmt04_0100, "scr_no") + 1,
                };
                DT.InsertData("QMT05_0110", data);
            }
            Set_SumSampleQty(qmt04_0100);
        }


        /// <summary>
        /// 加總樣品數缺陷數
        /// </summary>
        /// <param name="qmt04_0100">識別碼</param>
        public void Set_SumSampleQty(string qmt04_0100)
        {
            string sSql = "";
            DataTable dtTmp = new DataTable();
            object data = new object();
            string sSampleSumQty = "0";
            string sNgSumQty = "0";

            sSql = @"select isnull(sum(sample_qty),0) as qty 
                       from QMT05_0110
                      where qmt04_0100 = @qmt04_0100";
            dtTmp = comm.Get_DataTable(sSql, "qmt04_0100", qmt04_0100);
            if (dtTmp.Rows.Count > 0)
            {
                sSampleSumQty = dtTmp.Rows[0]["qty"].ToString();
            }
            sSql = @"select isnull(sum(sample_qty),0) as qty 
                       from QMT05_0110
                      where qmt04_0100 = @qmt04_0100 
                        and is_ok = 'N'";
            dtTmp = comm.Get_DataTable(sSql, "qmt04_0100", qmt04_0100);
            if (dtTmp.Rows.Count > 0)
            {
                sNgSumQty = dtTmp.Rows[0]["qty"].ToString();
            }
            data = new
            {
                qmt04_0100 = qmt04_0100,
                sample_sum_qty = sSampleSumQty,
                ng_sum_qty = sNgSumQty,
            };
            DT.UpdateData("QMT05_0100", "qmt04_0100", data);
        }

        /// <summary>
        /// 利用AQL機制檢查物料是否允收
        /// </summary>
        /// <param name="pQmtCode"></param>
        /// <returns></returns>
        //public string Chk_QMTIsRec(string pQmtCode)
        //{
        //    string sSql = "";
        //    DataTable dtTmp = new DataTable();
        //    DataTable dtTmp2 = new DataTable();
        //    DataTable dtTmp3 = new DataTable();
        //    object data = new object();
        //    string sErrMsg_SampleQty = "";
        //    string sErrMsg_NgQty = "";
        //    bool bIsOk = true;

        //    //先取得檢驗水準及進貨數量
        //    sSql = "select * from QMT05_0000 where ipqm_code = @ipqm_code";
        //    dtTmp = comm.Get_DataTable(sSql, "ipqm_code", pQmtCode);
        //    if (dtTmp.Rows.Count > 0)
        //    {
        //        DataRow r = dtTmp.Rows[0];
        //        string wmt0200 = r["wmt0200"].ToString();
        //        string sProQty = comm.Get_Data("WMT0200", wmt0200, "wmt0200", "pro_qty");
        //        string sAQLCode = r["ins_level_code"].ToString();

        //        //再判斷建議檢驗數量及容許缺陷數
        //        sSql = "select * " +
        //               "  from QMB12_0100 " +
        //               " where aql_code = @aql_code" +
        //               "   and aql_down < " + sProQty +
        //               "   and " + sProQty + " < aql_up";
        //        dtTmp = comm.Get_DataTable(sSql, "aql_code", sAQLCode);
        //        if (dtTmp.Rows.Count > 0)
        //        {
        //            DataRow r2 = dtTmp.Rows[0];
        //            string sSampleQty = r2["sample_qty"].ToString();
        //            decimal dNg_A = comm.sGetDecimal(r2["ng_a"].ToString());
        //            decimal dNg_B = comm.sGetDecimal(r2["ng_b"].ToString());
        //            decimal dNg_C = comm.sGetDecimal(r2["ng_c"].ToString());

        //            //判斷點:
        //            //1.檢驗樣本是否足夠
        //            sSql = "select QMT05_0100.*,qtest_item_name " +
        //                   "  from QMT05_0100 " +
        //                   "  left join QMB02_0000 on QMT05_0100.qtest_item_code = QMB02_0000.qtest_item_code" +
        //                   " where qmt_code = @qmt_code" +
        //                   "   and sample_sum_qty < " + sSampleQty;
        //            dtTmp = comm.Get_DataTable(sSql, "qmt_code", pQmtCode);
        //            if (dtTmp.Rows.Count > 0)
        //            {
        //                sErrMsg_SampleQty += "\n樣本數不足 " + sSampleQty + "個 的項目如下:\n";
        //                for (int i = 0; i < dtTmp.Rows.Count; i++)
        //                {
        //                    DataRow r3 = dtTmp.Rows[i];
        //                    string sQtestItemName = r3["qtest_item_name"].ToString();
        //                    sErrMsg_SampleQty += sQtestItemName + "\n";
        //                    bIsOk = false;
        //                }
        //            }

        //            //2.缺陷數是否超量(A、B、C三種分開加總判斷)
        //            //找出檢驗類型
        //            sSql = "select * " +
        //                   "  from QMB01_0000 ";
        //            dtTmp = comm.Get_DataTable(sSql);
        //            for (int i = 0; i < dtTmp.Rows.Count; i++)
        //            {
        //                DataRow r3 = dtTmp.Rows[i];
        //                string sQtestTypeCode = r3["qtest_type_code"].ToString();
        //                string sQtestTypeName = r3["qtest_type_name"].ToString();

        //                //找出檢驗項目的結果，加總起來判斷是否超過規定缺陷數
        //                sSql = "select isnull(sum(sample_qty),0) as qty " +
        //                       "  from QMT05_0110 " +
        //                       "  left join QMT05_0100 on QMT05_0110.qmt04_0100 = QMT05_0100.qmt04_0100" +
        //                       "  left join QMB02_0000 on QMT05_0110.qtest_item_code = QMB02_0000.qtest_item_code" +
        //                       " where qtest_type_code = '" + sQtestTypeCode + "'" +
        //                       "   and qmt_code = @qmt_code" +
        //                       "   and is_ok='N'";
        //                dtTmp3 = comm.Get_DataTable(sSql, "qmt_code", pQmtCode);
        //                if (dtTmp3.Rows.Count > 0)
        //                {
        //                    decimal dNg_Qty = comm.sGetDecimal(dtTmp3.Rows[0]["qty"].ToString());
        //                    decimal dRule_Qty = 0;

        //                    switch (sQtestTypeCode)
        //                    {
        //                        case "A":
        //                            dRule_Qty = dNg_A;
        //                            break;
        //                        case "B":
        //                            dRule_Qty = dNg_B;
        //                            break;
        //                        case "C":
        //                            dRule_Qty = dNg_C;
        //                            break;
        //                    }

        //                    if (dNg_Qty > dRule_Qty)
        //                    {
        //                        sErrMsg_NgQty += sQtestTypeName + " 超量，限定 " + dRule_Qty + " 個\n";
        //                        bIsOk = false;
        //                    }
        //                }
        //                if (sErrMsg_NgQty != "") { sErrMsg_NgQty = "\n缺陷數超量如下:\n" + sErrMsg_NgQty; }
        //            }
        //        }
        //        else
        //        {
        //            sErrMsg_NgQty = "\n AQL等級:" + sAQLCode + " 未設定批量為 " + sProQty + " 的範圍";
        //            bIsOk = false;
        //        }
        //    }

        //    data = new
        //    {
        //        IsOk = bIsOk,
        //        SampleQty = sErrMsg_SampleQty,
        //        NgQty = sErrMsg_NgQty,
        //    };
        //    return JsonConvert.SerializeObject(data);
        //}


        public decimal Get_SuggestSampleQty(string pAql, decimal pProQty)
        {
            decimal val = 0;
            string sSql = "";
            DataTable dtTmp = new DataTable();
            sSql = "select * " +
                   "  from QMB12_0100 " +
                   " where aql_code = @aql_code" +
                   "   and aql_down < " + pProQty +
                   "   and " + pProQty + " < aql_up";
            dtTmp = comm.Get_DataTable(sSql, "aql_code", pAql);
            if (dtTmp.Rows.Count > 0)
            {
                DataRow r = dtTmp.Rows[0];
                val = comm.sGetDecimal(r["sample_qty"].ToString());
            }
            return val;
        }




        /// <summary>
        /// 取得手機版的檢驗項目
        /// </summary>
        /// <param name="pQmtCode"></param>
        /// <param name="pField"></param>
        /// <returns></returns>
        public string Get_QmtItem_Moblie(string pQmtCode, string pField)
        {
            string sSql = "";
            sSql = "select QMT05_0100.*,QMB02_0000.qtest_item_name " +
                   "  from QMT05_0100" +
                   "  left join QMB02_0000 on QMT05_0100.qtest_item_code = QMB02_0000.qtest_item_code" +
                   " where QMT05_0100.ipqm_code = @ipqm_code" +
                   "   and QMT05_0100.qtest_item_type = 'C'"; //手機板只做外觀檢
            var dtTmp = comm.Get_DataTable(sSql, "ipqm_code", pQmtCode);
            return GD.DataFieldToStr(dtTmp, pField);
        }

        public ActionResult QMTReport_Mobile(string K)
        {
            //傳檢驗紀錄表號就可以查到所有入庫單相關資訊
            ViewBag.QmtCode = K;
            return View();
        }

        [HttpPost]
        public ActionResult QMTReport_Mobile(FormCollection form)
        {
            object data = new object();
            string sQmtCode = comm.sGetString(form["ipqm_code"]);
            string sAqlCode = comm.Get_Data("QMT05_0000", sQmtCode, "ipqm_code", "ins_level_code");
            string qmt05_0100_Array = Get_QmtItem_Moblie(sQmtCode, "qmt05_0100");
            decimal sStockProQty = comm.sGetDecimal(Get_WMT0200Data(sQmtCode, "pro_qty"));

            if (!string.IsNullOrEmpty(qmt05_0100_Array))
            {
                for (int i = 0; i < qmt05_0100_Array.Split(',').Length; i++)
                {
                    string qmt05_0100 = qmt05_0100_Array.Split(',')[i];
                    string sQtestItemCode = Get_QmtItem_Moblie(sQmtCode, "qtest_item_code").Split(',')[i];
                    string sValue = "N";
                    if (comm.sGetString(form[qmt05_0100]) == "on")
                    {
                        sValue = "Y";
                    }

                    if (sValue == "Y")
                    {
                        data = new
                        {
                            qmt05_0100 = qmt05_0100,
                            qtest_item_code = sQtestItemCode,
                            is_ok = sValue,
                            sample_qty = Get_SuggestSampleQty(sAqlCode, sStockProQty),
                            ins_date = DateTime.Now.ToString("yyyy/MM/dd"),
                            ins_time = DateTime.Now.ToString("HH:mm:ss"),
                            usr_code = User.Identity.Name,
                        };
                        DT.InsertData("QMT05_0110", data);
                        Set_SumSampleQty(qmt05_0100);
                    }
                }
            }
            return RedirectToAction("QMTReport_Mobile", new { K = sQmtCode });
        }




        public string Get_WMT0200Data(string pQmtCode, string pFieldCode)
        {
            string val = "";
            string sSql = " select QMT05_0000.* " +
                          "  from QMT05_0000 " +
                          "  left join MED09_0000 on QMT05_0000.med09_0000 = MED09_0000.med09_0000 " +
                          " where QMT05_0000.ipqm_code = @ipqm_code";
            DataTable dtTmp = comm.Get_DataTable(sSql, "ipqm_code", pQmtCode);
            if (dtTmp.Rows.Count > 0)
            {
                val = dtTmp.Rows[0][pFieldCode].ToString();
            }
            return val;
        }









    }
}