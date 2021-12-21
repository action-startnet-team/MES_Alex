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
using iTextSharp.text;
using System.Threading;

namespace MES_WATER.Controllers
{
    [Authorize] //登入驗證
    [HandleError(View = "Error")]  //錯誤導向

    public class MET010AController : JsonNetController
    {
        //程式代號
        public static string sPrgCode = "MET010A";

        //需要用到的Repo
        MET01_0000Repository repoMET01_0000 = new MET01_0000Repository();
        MET01_0100Repository repoMET01_0100 = new MET01_0100Repository();
        DTS01_0000Repository repoDTS01_0000 = new DTS01_0000Repository();

        //共用函式庫
        Comm comm = new Comm();
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
            //comm.ConvertNull("MET01_0000");

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

            //設定轉檔區間預設值
            ViewBag.date_s = DateTime.Now.ToString("yyyyMMdd");
            ViewBag.date_e = DateTime.Now.ToString("yyyyMMdd");
            ViewBag.code_s = "0000000000";
            ViewBag.code_e = "9999999999";

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

            List<MET01_0000> list = new List<MET01_0000>();
            list = repoMET01_0000.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // 明細檔 jqgrid資料來源
        [HttpPost]
        public ActionResult Get_GridData_D1(string pTkCode)
        {
            //string sPrgCode = pubPrgCode;
            string sUsrCode = User.Identity.Name;
            List<MET01_0100> list = new List<MET01_0100>();
            list = repoMET01_0100.Get_DataList(sUsrCode, sPrgCode, pTkCode);

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
            MET01_0000 data = new MET01_0000();
            //data.pur_date = DateTime.Now.ToString("yyyy/MM/dd");
            //data.exg_rate = 1;
            //data.stv_code = "NT";
            data.plan_start_date = DateTime.Now.ToString("yyyy/MM/dd");
            data.plan_end_date = DateTime.Now.ToString("yyyy/MM/dd");

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
            MET01_0000 newData = repoMET01_0000.GetDTO(pTkCode);
            return View(newData);
        }

        /// <summary>
        /// 拆解母製令及子製令
        /// </summary>
        /// <returns></returns>
        public ActionResult TranMoData(string pMoCode)
        {
            string sSql = "";

            //依工單號碼取得用料明細
            sSql = "select * from MET01_0100 where mo_code = '" + pMoCode + "' order by ";
            //半成品類別需要再拆出子工單

            ViewBag.MoCode = pMoCode;


            ////新增模式的預設值
            //MET01_0000 data = new MET01_0000();
            ////data.pur_date = DateTime.Now.ToString("yyyy/MM/dd");
            ////data.exg_rate = 1;
            ////data.stv_code = "NT";

            return View();
        }

        /// <summary>
        /// (修改區) 主檔的新增頁面將值存進DB
        /// </summary>
        /// <param name="form">畫面上輸入的form元件中的控制項集合</param>
        /// <param name="model">要存檔的model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Insert(FormCollection form, MET01_0000 model)
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
                MET01_0000 data = new MET01_0000();

                // 預設賦值
                comm.Set_ModelValue(data, form);

                // 特別取值
                data.mo_status = "10";
                data.mo_start_date = "";
                data.mo_end_date = "";
                data.mo_out_date = "";
                data.mo_qty = 0;
                data.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
                data.ins_time = DateTime.Now.ToString("HH:mm:ss");
                data.usr_code = User.Identity.Name;
                data.last_date = DateTime.Now.ToString("yyyy/MM/dd");
                data.last_time = DateTime.Now.ToString("HH:mm:ss");
                
                repoMET01_0000.InsertData(data);
                // 新增紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);

                //拆解BOM資料到MET01_0100明細
                repoMET01_0100.InsertByMEB23_0100(data.bom_code, data.mo_code, data.plan_qty);

                //拆解子母製令
                Ins_DetailMoData(data.mo_code, "0");

                return RedirectToAction("Update", sPrgCode, new { pTkCode = data.mo_code });
            }
            ViewBag.showErrMsg = true;
            return View(model);
        }

        /// <summary>
        /// 傳入一製令號碼，向下產出半成品製令，向下自動巡覽展開
        /// </summary>
        /// <param name="pMoCode"></param>
        /// <param name="pLevel"></param>
        public void Ins_DetailMoData(string pMoCode,string pLevel)
        {
            string sDetailMoList = ""; //拆出來的半成品製令列表，要再遞迴產一次製令

            //層級的判斷
            string iNowLevel =(comm.sGetInt32(pLevel) + 1).ToString();
            if (iNowLevel == "5")
            {
                //避免無窮迴圈，先鎖定到第5層就不再產制
                return;
            }

            //先找出這個製令向下的半成品標記，做為要產子製令的來源
            string sSql = "";
            sSql = "select * from MET01_0100 where mo_code='" + pMoCode + "' and pro_kind='B'";

            DataTable dtTmp = comm.Get_DataTable(sSql);
            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                //寫入主檔
                MET01_0000 met01_0000 = new MET01_0000();
                met01_0000.mo_code = GetTkCode();
                met01_0000.pro_code = comm.sGetString(dtTmp.Rows[i]["pro_code"].ToString());
                met01_0000.bom_code = Get_BomCodeByProCode(met01_0000.pro_code);
                met01_0000.sor_code ="";
                met01_0000.ord_code = "";
                met01_0000.cus_code = "";
                met01_0000.plan_start_date = comm.Get_QueryData("MET01_0000",pMoCode,"mo_code", "plan_start_date");
                met01_0000.plan_end_date = comm.Get_QueryData("MET01_0000", pMoCode, "mo_code", "plan_end_date");
                met01_0000.plan_out_date = comm.Get_QueryData("MET01_0000", pMoCode, "mo_code", "plan_out_date");
                met01_0000.plan_line_code = Get_LineCodeByProCode(met01_0000.pro_code);
                met01_0000.plan_qty = comm.sGetDecimal(dtTmp.Rows[i]["pro_qty"].ToString()); //子製令要生產的數量來自於母製令的用料明細
                met01_0000.pro_unit = comm.sGetString(dtTmp.Rows[i]["unit_code"].ToString()); //子製令要生產的單位來自於母製令的用料明細
                met01_0000.mo_status = "10";
                met01_0000.mo_start_date = "";
                met01_0000.mo_end_date = "";
                met01_0000.mo_out_date = "";
                met01_0000.mo_qty =0;
                met01_0000.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
                met01_0000.ins_time = DateTime.Now.ToString("HH:mm:ss");
                met01_0000.usr_code = User.Identity.Name;
                met01_0000.last_date = "";
                met01_0000.last_time = "";
                met01_0000.sch_date_s = "";
                met01_0000.sch_date_e = "";
                met01_0000.sch_time_s = "";
                met01_0000.sch_time_e = "";
                met01_0000.err_memo = "";
                met01_0000.mo_memo = "";
                met01_0000.mo_type = Get_BomInTypeByProCode(met01_0000.pro_code);
                met01_0000.mo_level = comm.sGetString(pLevel);
                met01_0000.up_mo_code = pMoCode;

                //異常檢查
                if (met01_0000.bom_code == "")
                {
                    met01_0000.mo_status = "70";
                    met01_0000.err_memo = "系統找不到預設的BOM，無法產出製令資料";
                }
                if (met01_0000.plan_line_code == "")
                {
                    met01_0000.mo_status = "70";
                    met01_0000.err_memo = "系統找不到預設的線別，無法產出製令資料";
                }
                repoMET01_0000.InsertData(met01_0000);

                //將半成品工串成字串陣列再投入做檢查
                sDetailMoList = sDetailMoList + met01_0000.mo_code + ",";

                //寫入明細檔
                repoMET01_0100.InsertByMEB23_0100(met01_0000.bom_code, met01_0000.mo_code, met01_0000.plan_qty);
            }

            //拆解明細製令再往下產出
            string[] sMoLists = sDetailMoList.Split(',');
            foreach(var MoCode in sMoLists)
            {
                if (MoCode.Trim() != "")
                {
                    //不等於空白則寫入明細製令
                    Ins_DetailMoData(MoCode, iNowLevel);
                }
            }
        }

        public string Get_BomCodeByProCode(string pProCode)
        {
            string sReturn = "";
            string sSql = "";
            sSql = "select * from MEB23_0000 " +
                   " where pro_code='" + pProCode + "'" +
                   "   and now_version='Y'";
            DataTable dtFun = comm.Get_DataTable(sSql);

            if (dtFun.Rows.Count > 0)
            {
                sReturn = dtFun.Rows[0]["bom_code"].ToString();
            }

            return sReturn;
        }

        public string Get_BomInTypeByProCode(string pProCode)
        {
            string sReturn = "";
            string sSql = "";
            sSql = "select * from MEB23_0000 " +
                   " where pro_code='" + pProCode + "'" +
                   "   and now_version='Y'";
            DataTable dtFun = comm.Get_DataTable(sSql);

            if (dtFun.Rows.Count > 0)
            {
                //如果為B或C則是混料
                if (dtFun.Rows[0]["in_type"].ToString() == "A")
                {
                    sReturn = "A";
                }
                else
                {
                    sReturn = "B";
                }
            }
            return sReturn;
        }

        public string Get_LineCodeByProCode(string pProCode)
        {
            string sReturn = "";
            string sSql = "";
            sSql = "select * from MEB20_0000 " +
                   " where pro_code='" + pProCode + "'";
            DataTable dtFun = comm.Get_DataTable(sSql);

            if (dtFun.Rows.Count > 0)
            {
                sReturn = dtFun.Rows[0]["line_code"].ToString();
            }

            return sReturn;
        }

        /// <summary>
        /// (修改區) 明細jqGrid的新增處理，將值存進DB
        /// </summary>
        /// <param name="form">畫面上輸入的form元件中的控制項集合</param>
        /// <returns></returns>
        [HttpPost]
        public void Insert_D1(FormCollection form)
        {
            MET01_0100 data = new MET01_0100();

            comm.Set_ModelValue(data, form);
            
            repoMET01_0100.InsertData(data);
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
        public ActionResult Update(FormCollection form, MET01_0000 model)
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

                MET01_0000 data = new MET01_0000();
                comm.Set_ModelValue(data, form);
                data.last_date = DateTime.Now.ToString("yyyy/MM/dd");
                data.last_time = DateTime.Now.ToString("HH:mm:ss");

                MET01_0000 sBefore = comm.GetData<MET01_0000>(data);
                repoMET01_0000.UpdateData(data);

                //更新紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "update", sBefore, data);

                //如果數量有改變，再變更明細
                if (sBefore.plan_qty != data.plan_qty)
                {
                    repoMET01_0100.UpdateByMEB23_0100(data.bom_code, data.mo_code, data.plan_qty);
                }
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
                MET01_0100 data = new MET01_0100();

                comm.Set_ModelValue(data, form);

                MET01_0100 sBefore = comm.GetData<MET01_0100>(data);
                repoMET01_0100.UpdateData(data);
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
            MET01_0000 sBefore = comm.GetData<MET01_0000>(pTkCode);
            repoMET01_0000.DeleteData(pTkCode);
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
            MET01_0100 sBefore = comm.GetData<MET01_0100>(pTkCode);
            repoMET01_0100.DeleteData(pTkCode);
            //刪除紀錄資料
            comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "delete", sBefore, "");
        }

        /* 資料處理 向上 */

        //資料檢查 向下//
        //主檔的檢查
        [HttpPost]
        public ActionResult Check_Data(FormCollection form, MET01_0000 model)
        {
            bool isSuccess = false;
            //檢查資料代碼是否重覆
            string key = gmv.GetKey<MET01_0000>(new MET01_0000());
            string sWhere = "where " + key + "='" + form[key].ToString() + "'";
            bool hasRow = !comm.Chk_RelData("MET01_0000", sWhere);
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
            CheckDataResult result = new CheckDataResult();
            result.bIsOK = true;
            result.message = "";
            string work_code = form["work_code"];
            string loc_code = form["loc_code"];

            if (loc_code != "")
            {
                DataTable loc_table = comm.Get_DataTable(" select MEB30_0100.*, MEB29_0200.mac_code, MEB15_0100.loc_code  " +
                                                         " from MEB30_0100 " +
                                                         " left join MEB29_0200 on MEB29_0200.station_code = MEB30_0100.station_code " +
                                                         " left join MEB15_0100 on MEB15_0100.mac_code = MEB29_0200.mac_code " +
                                                         " where MEB30_0100.work_code = '" + work_code + "' ");
                result.bIsOK = false;
                for (int i = 0; i < loc_table.Rows.Count; i++)
                {
                    if (loc_code == loc_table.Rows[i]["loc_code"].ToString())
                    {
                        result.bIsOK = true;
                    }
                }
                if (!result.bIsOK)
                {
                    result.message += "<li>此投料口不符</li>";
                }
            }

            //** 依作業不同有不同的檢查點 向下
            //if (comm.sGetInt32(form["pro_price"]) == 0)
            //{
            //    result.bIsOK = false;
            //    result.message += "<li> 單價等於零，不可修改 </li>";
            //}

            //** 依作業不同有不同的檢查點 向上
            
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
            result.message = "";
            string work_code = form["work_code"];
            string loc_code = form["loc_code"];

            if (loc_code != "")
            {
                DataTable loc_table = comm.Get_DataTable(" select MEB30_0100.*, MEB29_0200.mac_code, MEB15_0100.loc_code  " +
                                                         " from MEB30_0100 " +
                                                         " left join MEB29_0200 on MEB29_0200.station_code = MEB30_0100.station_code " +
                                                         " left join MEB15_0100 on MEB15_0100.mac_code = MEB29_0200.mac_code " +
                                                         " where MEB30_0100.work_code = '" + work_code + "' ");
                result.bIsOK = false;
                for (int i = 0; i < loc_table.Rows.Count; i++)
                {
                    if (loc_code == loc_table.Rows[i]["loc_code"].ToString())
                    {
                        result.bIsOK = true;
                    }
                }
                if (!result.bIsOK)
                {
                    result.message += "<li>此投料口不符</li>";
                }
            }

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

        // 寫入加載工單
        [HttpPost]
        public ActionResult Exdata(string date_s, string date_e, string code_s, string code_e)
        {
            DTS01_0000 DTS01_0000 = repoDTS01_0000.newData("ZMES9996", User.Identity.Name);
            DTS01_0000.con_request = @"[{""WERKS"":""QY01"",""STRDT"":"""+ date_s + 
                                     @""",""ENDDT"":""" + date_e +
                                     @""",""AUFNR_S"":""" + code_s +
                                     @""",""AUFNR_E"":""" + code_e + @"""}]";
            try
            {
                repoDTS01_0000.InsertData(DTS01_0000);
            } catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

            return Json("寫入轉檔排程，等待執行", JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get_BomCode(string pro_code)
        {
            string sSql = "select * from MEB23_0000 where pro_code = @pro_code ";
            DataTable dtTmp = comm.Get_DataTable(sSql, "pro_code", pro_code);

            return Json(dtTmp, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 強制結案按鈕功能
        /// </summary>
        /// <param name="pMoCode"></param>
        public JsonResult Upd_StopMoCode(string pMoCode)
        {
            //檢查目前工單是否開工
            bool bIsOK = true;
            string message = "";
            string mo_status = comm.Get_QueryData("MET01_0000", pMoCode, "mo_code", "mo_status");
            //檢查目前工單的派工單是否有mo_status為IN的資料
            mo_status = Chk_MoStatus(pMoCode);
            //準備中的工單
            if (mo_status == "20")
            {
                //更新工單狀態為20已取消
                repoMET01_0000.Upd_MoStatus(pMoCode, "10");
                //更新排成日為空
                repoMET01_0000.Upd_MET01_SchData(pMoCode);
                repoMET01_0000.Del_WMT07_0000_Data(pMoCode);
                repoMET01_0000.Del_MEM01_0000_Data(pMoCode);

            }
            //生產中的工單
            if (mo_status == "30")
            {
                repoMET01_0000.Upd_MoStatus(pMoCode, "90");
                repoMET01_0000.Del_WMT07_0000_Data(pMoCode);
                repoMET01_0000.Upd_MEM01_0000_Data(pMoCode);
                repoMET01_0000.Ins_MED02_0000(pMoCode);
                repoMET01_0000.Upd_MET03_MoStatus(pMoCode, "END");
            }




            var result = new
            {
                message = "更新成功，請重新整理後，確認資料內容"
            };
            return Json(result);

        }
        /// <summary>
        /// 確認目前派工單是否有開工的，MET03_0000的mo_status='IN'的資料
        /// </summary>
        /// <param name="pMoCode"></param>
        /// <returns></returns>
        public string Chk_MoStatus(string pMoCode)
        {
            string sMoStatus = "20";
            string sSql = "";
            sSql = "select * from  MET03_0000 where mo_code ='" + pMoCode + "' and mo_status='IN' ";
            DataTable dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                sMoStatus = "30";
            }
            return sMoStatus;
        }

        public string GetTkCode()
        {
            string num = "";
            Thread.Sleep(500);
            num = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            Thread.Sleep(500);
            return num;
        }

        public string GetQty(string bom_code)
        {
            string qty = "";
            qty = comm.Get_QueryData<decimal>("MEB23_0000", bom_code, "bom_code", "pro_qty").ToString();
            return qty;
        }

        public string GetUnit(string bom_code)
        {
            string unit = "";
            unit = comm.Get_QueryData("MEB23_0000", bom_code, "bom_code", "unit_code").ToString();
            return unit;
        }

        public string GetType(string bom_code)
        {
            string in_type = "";
            in_type = comm.Get_QueryData("MEB23_0000", bom_code, "bom_code", "in_type").ToString();
            return in_type;
        }

        public string GetLineCode(string pro_code)
        {
            string line_code = "";
            line_code = comm.Get_QueryData("MEB20_0000", pro_code, "pro_code", "line_code").ToString();
            return line_code;
        }
    }
}