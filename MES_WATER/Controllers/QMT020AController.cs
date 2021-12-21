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

namespace MES_WATER.Controllers
{
    [Authorize] //登入驗證
    [HandleError(View = "Error")]  //錯誤導向

    public class QMT020AController : JsonNetController
    {
        //程式代號
        public static string sPrgCode = "QMT020A";

        //需要用到的Repo
        QMT02_0000Repository repoQMT02_0000 = new QMT02_0000Repository();

        //共用函式庫
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();
        GetData GD = new GetData();
        DynamicTable DT = new DynamicTable();
        CheckData CD = new CheckData();
        
        //表單欄位table
        public string pubFieldTable = "EPB02_0100";

        //索引鍵
        public string pubPKCode()
        {
            return DT.Get_Table_PKField(pubFieldTable);
        }


        /* 資料處理 向下 */
        /// <summary>
        /// (固定區) 主檔 首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //要結合權限控制
            ViewBag.limit_str = comm.Get_LimitByUsrCode(User.Identity.Name, sPrgCode);
            //comm.ConvertNull("QMT02_0000");

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

            List<QMT02_0000> list = new List<QMT02_0000>();
            list = repoQMT02_0000.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// (修改區) 主檔 新增
        /// 1.新增模式下控項的預設值在這邊設定
        /// </summary>
        /// <returns></returns>
        public ActionResult Insert()
        {
            //新增模式的預設值
            QMT02_0000 data = new QMT02_0000();
            //data.pur_date = DateTime.Now.ToString("yyyy/MM/dd");
            //data.exg_rate = 1;
            //data.stv_code = "NT";

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
            string sDataCode = comm.Get_Data("QMT02_0000", pTkCode, "qmt02_0000", "datacode");
            string sQRCode = comm.Get_Data("QMT02_0000", pTkCode, "qmt02_0000", "qr_code");
            string sQmtCode = comm.Get_Data("QMT02_0000", pTkCode, "qmt02_0000", "qmt_code");
            string sQsheetCode = comm.Get_Data("QMB03_0100", sDataCode, "datacode", "qsheet_code");

            object route = new object();
            route = new {
                K = sQsheetCode,
                QRcode = sQRCode,
                V = sQmtCode,
            };            
            return RedirectToAction("Report", route);
        }

        public ActionResult ISOK_P(string qmt02_0000)
        {
            comm.Upd_Data("QMT02_0000", "qmt02_0000", qmt02_0000, "is_ok", "P");
            return View("Index");
        }

        public ActionResult Report(string K, string QRcode, string V = "")
        {
            //20004143%20200408%202006160001%0000%B%E739DA748DC0434D8B9CB7B6325DD5A5%15000
            //20004143%20200408%202006160001%      0000%       B%E739DA748DC0434D8B9CB7B6325DD5A5%15000
            //產品編號%    批號%    來源單號%供應商代碼%條碼類別%                      TrackingNo% 數量

            if (QRcode.Split('%').Length < 6) { return RedirectToAction("Insert"); }
            ViewBag.Key = K;
            ViewBag.Value = V;
            ViewBag.QRcode = QRcode;
            return View();
        }

        [HttpPost]
        public ActionResult Report(FormCollection form)
        {
            object data = new object();
            string sQRCode = Request.QueryString["QRcode"];
            string sQsheetCode = comm.sGetString(form["qsheet_code"]);

            //如果沒有帶檢驗紀錄表編號進來的話，才新建一個編號
            string sTkCode = comm.Get_TkCode("QMT020A");
            string sValue = Request.QueryString["V"];            
            if (!string.IsNullOrEmpty(sValue))
            {
                //先把原本的紀錄刪除
                sTkCode = sValue;
                comm.Del_QueryData("QMT02_0000", "qmt_code", sTkCode);
            }

            //檢驗表代號回找電子表單欄位
            string sEpbFieldArray = comm.Get_Data("EPB02_0100", sQsheetCode, "epb_code", "field_code");
            string sDataCodeArray = comm.Get_Data("EPB02_0100", sQsheetCode, "epb_code", "sor_field");
            for (int i = 0;i < sEpbFieldArray.Split(',').Length;i++) {
                
                string sEpbField = sEpbFieldArray.Split(',')[i];
                string sDataCode = sDataCodeArray.Split(',')[i];
                //欄位的值
                string sQmtValue = comm.sGetString(form[sEpbField]);

                data = new {
                    qmt_code = sTkCode,
                    datacode = sDataCode,
                    pro_code = comm.sGetString(form["pro_code"]),
                    lot_no = comm.sGetString(form["lot_no"]),
                    qmt_value = sQmtValue,
                    is_ok = Chk_QmtValueIsOk(sDataCode, sQmtValue),
                    ins_date = DateTime.Now.ToString("yyyy/MM/dd"),
                    ins_time = DateTime.Now.ToString("HH:mm:ss"),
                    usr_code = User.Identity.Name,
                    qr_code = sQRCode,
                };
                DT.InsertData("QMT02_0000", data);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 檢查判定結果是否通過
        /// </summary>
        /// <param name="pDataCode"></param>
        /// <param name="pQmtValue"></param>
        /// <returns></returns>
        public string Chk_QmtValueIsOk(string pDataCode,string pQmtValue)
        {
            string val = "N";
            string sQtestItemType = comm.Get_Data("QMB03_0100", pDataCode,"datacode","qtest_item_type");
            decimal sQtestUp = comm.sGetDecimal(comm.Get_Data("QMB03_0100", pDataCode, "datacode", "qtest_up"));
            decimal sQtestDown = comm.sGetDecimal(comm.Get_Data("QMB03_0100", pDataCode, "datacode", "qtest_down"));

            switch (sQtestItemType) {
                case "B":
                    decimal dQmtValue = comm.sGetDecimal(pQmtValue);
                    if (sQtestUp >= dQmtValue && dQmtValue >= sQtestDown)
                    {
                        val = "Y";
                    }
                    else {
                        val = "N";
                    }
                    break;
                case "C":
                    switch (pQmtValue) {
                        case "on":
                            val = "Y";
                            break;
                        case "":
                            val = "N";
                            break;
                    }
                    break;
                case "D":
                    val = "Y";
                    break;
            }
            return val;
        }

        /// <summary>
        /// 利用檢驗紀錄表編號與欄位回找資料
        /// </summary>
        /// <param name="pValue">檢驗紀錄表編號</param>
        /// <param name="pFieldCode">品檢紀錄的欄位鍵值(datacode)</param>
        /// <returns></returns>
        public string Get_FieldValue(string pValue,string pFieldCode) {
            string val = "";
            string sSql = "select * " +
                          "  from QMT02_0000" +
                          " where qmt_code = @qmt_code" +
                          "   and datacode = @datacode";
            var dtTmp = comm.Get_DataTable(sSql, "qmt_code,datacode", pValue + "," + pFieldCode);
            if (dtTmp.Rows.Count > 0) {
                val = GD.DataFieldToStr(dtTmp, "qmt_value");
            }            
            return val;
        }






        public ActionResult ConvertQsheet() {

            //要結合權限控制
            ViewBag.prg_code = sPrgCode;

            // 使用者, controllerName, actionName
            string usr_code = User.Identity.Name;
            //string prg_code = sPrgCode;
            string view_code = ControllerContext.RouteData.Values["action"].ToString();

            //取得欄位寬度
            List<BDP30_0000> colWidth_list = comm.Get_BDP30_0000(usr_code, sPrgCode, "Index");
            ViewBag.colWidth_list = colWidth_list;

            //取得欄位顯示
            List<BDP30_0100> is_show_list = comm.Get_BDP30_0100(usr_code, sPrgCode, "Index");
            ViewBag.is_show_list = is_show_list;

            return View();
        }

        [HttpPost]
        public ActionResult ConvertQsheet(FormCollection form) {
            object data = new object();
            string qmt02_0000Array = comm.sGetString(form["qmt02_0000"]);
            string sTkCode = comm.Get_TkCode("QMT030A");
            string sProCode = comm.Get_Data("QMT02_0000", qmt02_0000Array.Split(',')[0], "qmt02_0000", "pro_code");
            string sLotNo = comm.Get_Data("QMT02_0000", qmt02_0000Array.Split(',')[0], "qmt02_0000", "lot_no");
            string sScrNo = comm.Get_Data("QMT02_0000", qmt02_0000Array.Split(',')[0], "qmt02_0000", "scr_no");

            //單頭
            data = new {
                qmt_code = sTkCode,
                rel_type = comm.Get_QueryData("WMT0200", "where pro_code = '" + sProCode + "' and lot_no = '" + sLotNo + "' and ins_type = 'I' and is_check = 'N '", "rel_type"),
                rel_code = comm.Get_QueryData("WMT0200", "where pro_code = '" + sProCode + "' and lot_no = '" + sLotNo + "' and ins_type = 'I' and is_check = 'N '", "rel_code"),
                pro_code = sProCode,
                lot_no = sLotNo,
                scr_no = comm.Get_QueryData("WMT0200", "where pro_code = '" + sProCode + "' and lot_no = '" + sLotNo + "' and ins_type = 'I' and is_check = 'N '", "scr_no"),
            };
            DT.InsertData("QMT03_0000", data);

            //單身
            string sIsRec = "Y";
            for (int i = 0; i < qmt02_0000Array.Split(',').Length; i++) {
                string qmt02_0000 = qmt02_0000Array.Split(',')[i];
                string sDataCode = comm.Get_Data("QMT02_0000", qmt02_0000, "qmt02_0000", "datacode");
                string sQmtCode = comm.Get_Data("QMT02_0000", qmt02_0000, "qmt02_0000", "qmt_value");
                string sIsOk = comm.Get_Data("QMT02_0000", qmt02_0000, "qmt02_0000", "is_ok");
                if (sIsOk == "N") {
                    sIsRec = "N";
                }
                data = new {
                    qmt02_0000 = qmt02_0000,
                    qmt_code = sTkCode,
                    datacode = sDataCode,
                    qmt_value = sQmtCode,
                    is_ok = sIsOk,
                    ins_date = DateTime.Now.ToString("yyyy/MM/dd"),
                    ins_time = DateTime.Now.ToString("HH:mm:ss"),
                    usr_code = User.Identity.Name, 
                };
                DT.InsertData("QMT03_0100", data);

                //更新使用標記
                data = new {
                    qmt02_0000 = qmt02_0000,
                    is_end = "Y",
                };
                DT.UpdateData("QMT02_0000", "qmt02_0000", data);

                //更新允收標記
                data = new {
                    qmt_code = sTkCode,
                    is_rec = sIsRec,
                };
                DT.UpdateData("QMT03_0000", "qmt_code", data);
            }
            return RedirectToAction("Index","QMT030A");
        }



        /// <summary>
        /// (修改區) 主檔的新增頁面將值存進DB
        /// </summary>
        /// <param name="form">畫面上輸入的form元件中的控制項集合</param>
        /// <param name="model">要存檔的model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Insert(FormCollection form, QMT02_0000 model)
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
                QMT02_0000 data = new QMT02_0000();

                // 預設賦值
                comm.Set_ModelValue(data, form);

                // 特別取值
                //data.cus_name = comm.Get_QueryData("STB08_0000", data.cus_code, "cus_code", "cus_name");
                //data.is_dps = "N";
                //data.usr_code = User.Identity.Name;
                //data.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
                //data.ins_time = DateTime.Now.ToString("HH:mm:ss");

                repoQMT02_0000.InsertData(data);
                // 新增紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);
                return RedirectToAction("Update", sPrgCode, new { pTkCode = data.qmt_code });
            }
            ViewBag.showErrMsg = true;
            return View(model);
        }

        /// <summary>
        /// (修改區) 主檔 修改
        /// 1.依資料鍵值到DB取回資料呈現在修改模式頁面
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(FormCollection form, QMT02_0000 model)
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

                QMT02_0000 data = new QMT02_0000();

                comm.Set_ModelValue(data, form);

                QMT02_0000 sBefore = comm.GetData<QMT02_0000>(data);
                repoQMT02_0000.UpdateData(data);
                //更新紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "update", sBefore, data);

                return RedirectToAction("Index");
            }
            ViewBag.showErrMsg = true;
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
            //刪主檔
            QMT02_0000 sBefore = comm.GetData<QMT02_0000>(pTkCode);
            repoQMT02_0000.DeleteData(pTkCode);
            //刪除紀錄資料
            comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "delete", sBefore, "");
            return RedirectToAction("Index");
        }

        /* 資料處理 向上 */

        //資料檢查 向下//
        //主檔的檢查
        [HttpPost]
        public ActionResult Check_Data(FormCollection form, QMT02_0000 model)
        {
            bool isSuccess = false;
            //檢查資料代碼是否重覆
            string key = gmv.GetKey<QMT02_0000>(new QMT02_0000());
            string sWhere = "where " + key + "='" + form[key].ToString() + "'";
            bool hasRow = !comm.Chk_RelData("QMT02_0000", sWhere);
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

        public bool CheckProCode(string pro_code)
        {
            bool chk = true;
            string qsheet_code = comm.Get_QueryData("QMB03_0200", pro_code, "pro_code", "qsheet_code");
            if (qsheet_code == "") {
                chk = false;
            }
            return chk;
        }
        /* 資料檢查 向上 */
        
        //刷讀條碼取得資料



        public string Get_QmtCode(string Key)
        {
            string sSql = " select *, QMB03_0000.qsheet_name " +
                          " from QMB03_0200 " +
                          " left join QMB03_0000 on QMB03_0000.qsheet_code = QMB03_0200.qsheet_code" +
                          " where pro_code = '" + Key + "'";

            return GD.DataFieldToSTA(sSql, "qsheet_code,qsheet_name");
        }

        public string Get_Data(string T, string K, string KF, string F)
        {
            return GD.Get_Data(T, K, KF, F);
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


    }
}