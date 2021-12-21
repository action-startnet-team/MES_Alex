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

    public class RSS020AController : JsonNetController
    {
        //程式代號
        public static string sPrgCode = "RSS020A";

        //需要用到的Repo
        RSS02_0000Repository repoRSS02_0000 = new RSS02_0000Repository();
        RSS02_0100Repository repoRSS02_0100 = new RSS02_0100Repository();

        //共用函式庫
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();
        DynamicTable DT = new DynamicTable();
        GetData GD = new GetData();
        

        /* 資料處理 向下 */
        /// <summary>
        /// (固定區) 主檔 首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //要結合權限控制
            ViewBag.limit_str = comm.Get_LimitByUsrCode(User.Identity.Name, sPrgCode);
            //comm.ConvertNull("RSS02_0000");

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
        public string Get_GridDataByQuery(string pWhere)
        {
            string sUsrCode = User.Identity.Name;
            //string sPrgCode = pubPrgCode;

            List<RSS02_0000> list = new List<RSS02_0000>();
            list = repoRSS02_0000.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

            for (int i = 0;i < list.Count;i++) {
                string File = list[i].report_code;
                if (!string.IsNullOrEmpty(Get_SampleName(File))) {
                    list[i].upload = "已上傳";
                }
            }
            return JsonConvert.SerializeObject(list);
        }

        // 明細檔 jqgrid資料來源
        [HttpPost]
        public ActionResult Get_GridData_D1(string pTkCode)
        {
            //string sPrgCode = pubPrgCode;
            string sUsrCode = User.Identity.Name;
            List<RSS02_0100> list = new List<RSS02_0100>();
            list = repoRSS02_0100.Get_DataList(sUsrCode, sPrgCode, pTkCode);

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
            RSS02_0000 data = new RSS02_0000();
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
            RSS02_0000 newData = repoRSS02_0000.GetDTO(pTkCode);
            return View(newData);
        }

        /// <summary>
        /// (修改區) 主檔的新增頁面將值存進DB
        /// </summary>
        /// <param name="form">畫面上輸入的form元件中的控制項集合</param>
        /// <param name="model">要存檔的model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Insert(FormCollection form, RSS02_0000 model)
        {
            var i = form["data_source_type"];
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
                RSS02_0000 data = new RSS02_0000();
                // 預設賦值
                comm.Set_ModelValue(data, form);
                // 特別取值
                if (i == "A")
                {
                    data.epb_code = "";
                }
                else if (i == "B")
                {
                    data.etl_code = "";
                }
                else
                {
                    data.epb_code = "";
                    data.etl_code = "";
                }
                data.usr_code = User.Identity.Name;
                data.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
                data.ins_time = DateTime.Now.ToString("HH:mm:ss");

                repoRSS02_0000.InsertData(data);
                // 新增紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);
                return RedirectToAction("Update", sPrgCode, new { pTkCode = data.report_code });
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
            RSS02_0100 data = new RSS02_0100();

            comm.Set_ModelValue(data, form);

            repoRSS02_0100.InsertData(data);
        }

        //[HttpPost]
        //public void Insert_N(string SelectString, string EtlCode)
        //{
        //    RSS02_0100 data = new RSS02_0100();

        //    string sSql = "delete  RSS02_0100 " +
        //                  " where report_code = @report_code";
        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        con_db.Execute(sSql, new { report_code = EtlCode });
        //    }

        //    for (int i = 0; i < DT.Get_SqlField(SelectString).Split(',').Length; i++)
        //    {
        //        string str = DT.Get_SqlField(SelectString).Split(',')[i];

        //        data.report_code = EtlCode;
        //        data.field_code = str;
        //        data.field_name = "";
        //        data.data_type = "";

        //        repoRSS02_0100.InsertData(data);
        //    }



        //}


        /// <summary>
        /// (修改區) 主檔 修改
        /// 1.依資料鍵值到DB取回資料呈現在修改模式頁面
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(FormCollection form, RSS02_0000 model, HttpPostedFileBase file)
        {
            var i = form["data_source_type"];
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

                //如果有上傳範本 則存放至伺服器
                UploadFile(file, model.report_code);

                RSS02_0000 data = new RSS02_0000();

                comm.Set_ModelValue(data, form);
                if (i == "A")
                {
                    data.epb_code = "";
                }
                else if (i == "B")
                {
                    data.etl_code = "";
                }
                else
                {
                    data.epb_code = "";
                    data.etl_code = "";
                }
                //RSS02_0000 sBefore = comm.GetData<RSS02_0000>(data);
                repoRSS02_0000.UpdateData(data);
                //更新紀錄資料
                //comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "update", sBefore, data);
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
                RSS02_0100 data = new RSS02_0100();

                comm.Set_ModelValue(data, form);

                RSS02_0100 sBefore = comm.GetData<RSS02_0100>(data);
                repoRSS02_0100.UpdateData(data);
                //更新紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "update", sBefore, data);
            }
            return Json(result);
        }


        /// <summary>
        /// (修改區) 明細jqGrid的修改與新增chosen-select所使用的SQL方法
        /// </summary>
        /// <param name="pTkCode">要搜尋的鍵值</param>
        /// <returns></returns>
        public JsonResult Get_SelectCode(string pTkCode)
        {
            List<DDLList> list = new List<DDLList>();

            if (string.IsNullOrEmpty(pTkCode))
            {
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            string sSql = "select top 10000 select_code as field_code, select_name as field_name " +
                          " from BDP31_0000 " +
                          " where select_code like @select_code ";
            using (SqlConnection conn = comm.Set_DBConnection())
            {
                list = conn.Query<DDLList>(sSql, new { select_code = pTkCode + "%" }).ToList();
            }

            return Json(list, JsonRequestBehavior.AllowGet);
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
            RSS02_0000 sBefore = comm.GetData<RSS02_0000>(pTkCode);
            repoRSS02_0000.DeleteData(pTkCode);
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
            RSS02_0100 sBefore = comm.GetData<RSS02_0100>(pTkCode);
            repoRSS02_0100.DeleteData(pTkCode);
            //刪除紀錄資料
            comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "delete", sBefore, "");
        }

        /* 資料處理 向上 */

        //資料檢查 向下//
        //主檔的檢查
        [HttpPost]
        public ActionResult Check_Data(FormCollection form, RSS02_0000 model)
        {
            bool isSuccess = false;
            //檢查資料代碼是否重覆
            string key = gmv.GetKey<RSS02_0000>(new RSS02_0000());
            string sWhere = "where " + key + "='" + form[key].ToString() + "'";
            bool hasRow = !comm.Chk_RelData("RSS02_0000", sWhere);
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

            //重複值檢查
            //if (!comm.Chk_RelData("RSS02_0100", "usr_code", form["usr_code"]))
            //{
            //    bIsOK = false;
            //    message += "<p> 人員代號重複! </p> ";
            //}
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
            //** 依作業不同有不同的檢查點 向下
            //if (comm.sGetInt32(form["pro_price"]) == 0)
            //{
            //    result.bIsOK = false;
            //    result.message += "<li> 單價等於零，不可修改 </li>";
            //}

            //** 依作業不同有不同的檢查點 向上

            return result;
        }


        public string pubFieldTable = "EPB02_0100";

        //索引鍵
        public string pubPKCode()
        {
            return DT.Get_Table_PKField(pubFieldTable);
        }


        public ActionResult DataView(string K)
        {
            ViewBag.Key = K;

            return View();
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
                          "   and is_key <> 'Y'" + 
                          "  order by scr_no ";
            return comm.DataFieldToStr(sSql, "epb02_0100");
        }


        /// <summary>
        /// 取得該表單 資料筆數
        /// </summary>
        /// <param name="pEpbCode"></param>
        /// <returns></returns>
        public string Get_DataValue(string pEpbCode)
        {
            string sValue = "";
            string sKeyField = "";

            string sSql = "select * from EPB02_0100 " +
                          " where epb_code = '" + pEpbCode + "' " +
                          "   and is_key = 'Y'";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                sKeyField = dtTmp.Rows[0]["field_code"].ToString();

                sSql = "select * from EPB03_0000 " +
                       " where epb_code = '" + pEpbCode + "' " +
                       "   and field_code = '" + sKeyField + "'";
                sValue = GD.DataFieldToStr(sSql, "key_value");
            }
            return sValue;
        }


        /// <summary>
        /// 取得該欄位的值
        /// </summary>
        /// <param name="pEpbCode">表單代號</param>
        /// <param name="pField">欄位</param>
        /// <returns></returns>
        public string Get_FieldValue(string pEpbCode, string pField, string pKeyValue)
        {
            string sValue = "";
            string sSql = "select * from EPB03_0000 " +
                           " where epb_code = '" + pEpbCode + "' " +
                           "   and field_code = '" + pField + "'" +
                           "   and key_value = '" + pKeyValue + "'";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                sValue = dtTmp.Rows[0]["field_value"].ToString();
            }
            return sValue;
        }


        public string Chk_Source(string R)
        {
            string sValue = "";
            string fileLocation = Server.MapPath("~/Upload/Report/") + R + ".xls";
            if (!System.IO.File.Exists(fileLocation)) // 驗證檔案是否存在
            {
                sValue = "尚未上傳範本";
            }
            return sValue;
        }


        private void UploadFile(HttpPostedFileBase file,string pReportCode) {
            if (Request.Files["file"].ContentLength > 0)
            {
                string extension =
                    System.IO.Path.GetExtension(file.FileName);

                if (extension == ".xls" || extension == ".xlsx")
                {
                    string fileLocation = Server.MapPath("~/Upload/Report/") + pReportCode + extension;
                    if (System.IO.File.Exists(fileLocation)) // 驗證檔案是否存在
                    {
                        System.IO.File.Delete(Server.MapPath("~/Upload/Report/") + pReportCode + ".xls");
                        System.IO.File.Delete(Server.MapPath("~/Upload/Report/") + pReportCode + ".xlsx");
                    }
                    Request.Files["file"].SaveAs(fileLocation); // 存放檔案到伺服器上
                }
            }
        }

        public string Get_SampleName(string pCode) {
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


        /* 資料檢查 向上 */
    }
}