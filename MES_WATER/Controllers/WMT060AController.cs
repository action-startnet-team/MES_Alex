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

    public class WMT060AController : JsonNetController
    {
        //程式代號
        public static string sPrgCode = "WMT060A";

        //需要用到的Repo
        WMT06_0000Repository repoWMT06_0000 = new WMT06_0000Repository();
        WMT06_0100Repository repoWMT06_0100 = new WMT06_0100Repository();
        WMT06_0200Repository repoWMT06_0200 = new WMT06_0200Repository();

        //共用函式庫
        Comm comm = new Comm();
        GetData GD = new GetData();
        GetModelValidation gmv = new GetModelValidation();
        

        /* 資料處理 向下 */
        /// <summary>
        /// (固定區) 主檔 首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //要結合權限控制
            ViewBag.limit_str = comm.Get_LimitByUsrCode(User.Identity.Name, sPrgCode);
            //comm.ConvertNull("WMT06_0000");

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



        public ActionResult DeletePrepare(string K) {
            if (Chk_CanDeletePrepare(K)) {
                string sSql = "";
                sSql = "select MET03_0000.mo_code " +
                       "  from WMT06_0200 " +
                       "   left join MET03_0000 on WMT06_0200.wrk_code = MET03_0000.wrk_code " +
                       " where prepare_code = '"+ K + "'"+
                       "  group by MET03_0000.mo_code";
                var dtTmp = comm.Get_DataTable(sSql);
                for (int i = 0; i < dtTmp.Rows.Count; i++) {
                    string sMoCode = dtTmp.Rows[i]["mo_code"].ToString();
                    comm.Upd_QueryData("MET01_0000", "mo_code", sMoCode, "mo_status", "10");

                    string sInqCodeArray = comm.Get_Data("WMT10_0000", sMoCode, "mo_code", "inq_code");
                    for (int u = 0; u < GD.Get_SplitLength(sInqCodeArray); u++)
                    {
                        string sInqCode = sInqCodeArray.Split(',')[u];
                        comm.Del_QueryData("WMT10_0100", "inq_code", sInqCode);
                        comm.Del_QueryData("WMT10_0000", "inq_code", sInqCode);
                    }                                      
                }

                sSql = "select * from WMT06_0200 " +
                       " where prepare_code = '"+ K + "'";
                dtTmp = comm.Get_DataTable(sSql);
                for (int i = 0; i < dtTmp.Rows.Count; i++)
                {
                    string sWrkCode = dtTmp.Rows[i]["wrk_code"].ToString();
                    sSql = " delete MET03_0000 where wrk_code = '" + sWrkCode + "'";          
                    comm.Connect_DB(sSql);      
                }
                
                //刪除
                sSql = " delete WMT07_0000 where prepare_code = '"+ K + "'";
                sSql += " delete WMT06_0200 where prepare_code = '" + K + "'";
                sSql += " delete WMT06_0100 where prepare_code = '" + K + "'";
                sSql += " delete WMT06_0000 where prepare_code = '" + K + "'";
                comm.Connect_DB(sSql);
            }            
            return RedirectToAction("Index");
        }


        /// <summary>
        /// 檢查是否可以取消備料單
        /// </summary>
        /// <param name="pPrepareCode"></param>
        /// <returns></returns>
        public bool Chk_CanDeletePrepare(string pPrepareCode)
        {
            bool sValue = false;
            string sSql = "select * from WMT06_0110" +
                          "  left join WMT06_0100 on WMT06_0110.wmt06_0100 = WMT06_0100.wmt06_0100 " +
                          " where prepare_code = '" + pPrepareCode + "'";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count <= 0)
            {
                sValue = true;
            }
            return sValue;
        }


        /// <summary>
        /// (修改區) 主檔 修改
        /// 1.依資料鍵值到DB取回資料呈現在修改模式頁面
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        /// <returns></returns>
        public ActionResult Update(string pTkCode)
        {
            WMT06_0000 newData = repoWMT06_0000.GetDTO(pTkCode);
            return View(newData);
        }

        public ActionResult Update2(string pTkCode)
        {
            WMT06_0000 newData = repoWMT06_0000.GetDTO(pTkCode);
            return View(newData);
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

            List<WMT06_0000> list = new List<WMT06_0000>();
            list = repoWMT06_0000.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // 明細檔 jqgrid資料來源
        [HttpPost]
        public ActionResult Get_GridData_D1(string pTkCode)
        {
            //string sPrgCode = pubPrgCode;
            string sUsrCode = User.Identity.Name;
            List<WMT06_0100> list = new List<WMT06_0100>();
            list = repoWMT06_0100.Get_DataList(sUsrCode, sPrgCode, pTkCode);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Get_GridData_D2(string pTkCode)
        {
            //string sPrgCode = pubPrgCode;
            string sUsrCode = User.Identity.Name;
            List<WMT06_0200> list = new List<WMT06_0200>();
            list = repoWMT06_0200.Get_DataList(sUsrCode, sPrgCode, pTkCode);

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
            WMT06_0000 data = new WMT06_0000();
            //data.pur_date = DateTime.Now.ToString("yyyy/MM/dd");
            //data.exg_rate = 1;
            //data.stv_code = "NT";

            return View(data);
        }

        

        /// <summary>
        /// (修改區) 主檔的新增頁面將值存進DB
        /// </summary>
        /// <param name="form">畫面上輸入的form元件中的控制項集合</param>
        /// <param name="model">要存檔的model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Insert(FormCollection form, WMT06_0000 model)
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
                WMT06_0000 data = new WMT06_0000();

                // 預設賦值
                comm.Set_ModelValue(data, form);

                // 特別取值
                data.usr_code = User.Identity.Name;
                data.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
                data.ins_time = DateTime.Now.ToString("HH:mm:ss");

                repoWMT06_0000.InsertData(data);
                // 新增紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);
                return RedirectToAction("Update", sPrgCode, new { pTkCode = data.prepare_code });
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
            WMT06_0100 data = new WMT06_0100();

            comm.Set_ModelValue(data, form);

            repoWMT06_0100.InsertData(data);
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
        public ActionResult Update(FormCollection form, WMT06_0000 model)
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

                WMT06_0000 data = new WMT06_0000();

                comm.Set_ModelValue(data, form);

                WMT06_0000 sBefore = comm.GetData<WMT06_0000>(data);
                repoWMT06_0000.UpdateData(data);
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
                WMT06_0100 data = new WMT06_0100();

                comm.Set_ModelValue(data, form);


                WMT06_0100 sBefore = comm.GetData<WMT06_0100>(data);
                repoWMT06_0100.UpdateData(data);
                //更新紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "update", sBefore, data);

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
            WMT06_0000 sBefore = comm.GetData<WMT06_0000>(pTkCode);
            repoWMT06_0000.DeleteData(pTkCode);
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
            WMT06_0100 sBefore = comm.GetData<WMT06_0100>(pTkCode);
            repoWMT06_0100.DeleteData(pTkCode);
            //刪除紀錄資料
            comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "delete", sBefore, "");
        }

        /* 資料處理 向上 */

        //資料檢查 向下//
        //主檔的檢查
        [HttpPost]
        public ActionResult Check_Data(FormCollection form, WMT06_0000 model)
        {
            bool isSuccess = false;
            //檢查資料代碼是否重覆
            string key = gmv.GetKey<WMT06_0000>(new WMT06_0000());
            string sWhere = "where " + key + "='" + form[key].ToString() + "'";
            bool hasRow = !comm.Chk_RelData("WMT06_0000", sWhere);
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
            //if (!comm.Chk_RelData("WMT06_0100", "usr_code", form["usr_code"]))
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

        /* 資料檢查 向上 */
    }
}