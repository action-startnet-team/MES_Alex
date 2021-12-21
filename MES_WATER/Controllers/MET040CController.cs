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

    public class MET040CController : JsonNetController
    {
        //程式代號
        string sPrgCode = "MET040C";
        //需要用到的Repo
        MET04_0200Repository repoMET04_0200 = new MET04_0200Repository();
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
            //string sPrgCode = sPrgCode;

            List<MET04_0200> list = new List<MET04_0200>();
            list = repoMET04_0200.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public DataTable Get_RecData(string pMoCode)
        {
            string sSql = "SELECT a.* FROM MET04_0200 a" +
                " WHERE a.mo_code = @mo_code";
            return comm.Get_DataTable(sSql, "mo_code", pMoCode);
        }


        /// <summary>
        /// (修改區) 主檔 新增
        /// 1.新增模式下控項的預設值在這邊設定
        /// </summary>
        /// <returns></returns>
        public ActionResult Insert(string pTkCode)
        {
            //要結合權限控制
            ViewBag.prg_code = sPrgCode;
            ViewBag.mo_code = pTkCode;
            //取得歷程
            ViewBag.rec_data = Get_RecData(pTkCode);


            //新增模式的預設值
            MET04_0200 newData = new MET04_0200();

            newData.mo_code = pTkCode;
            //newData.pro_date_s = DateTime.Now.ToString("yyyy/MM/dd");
            //newData.pro_time_s = "08:00";
            //newData.pro_date_e = DateTime.Now.ToString("yyyy/MM/dd");
            //newData.pro_time_e = "17:00";
            newData.pro_date_s = Get_WorkTimeSByMoCode(pTkCode).ToString("yyyy/MM/dd");
            newData.pro_time_s = Get_WorkTimeSByMoCode(pTkCode).ToString("HH:mm");
            newData.pro_date_e = Get_WorkTimeEByMoCode(pTkCode).ToString("yyyy/MM/dd");
            newData.pro_time_e = Get_WorkTimeEByMoCode(pTkCode).ToString("HH:mm");
            newData.pro_qty = Get_MEM01_ProQty(pTkCode);
            newData.ISM03 = newData.pro_qty;
            newData.ISM04 = newData.pro_qty;
            newData.ISM0301 = Get_SAP0100_Data(pTkCode, "VGW03");
            newData.ISM0401 = Get_SAP0100_Data(pTkCode, "VGW04");
            newData.ISM02 = Get_MacTimeSpan(Get_WorkTimeSByMoCode(pTkCode), Get_WorkTimeEByMoCode(pTkCode));
            newData.pro_unit = comm.Get_QueryData("MET01_0000", pTkCode, "mo_code", "pro_unit");
            newData.ISM0101 = newData.ISM02;
            newData.ISM01 = Get_Person(newData.pro_time_s, newData.pro_time_e, newData.pro_date_s);


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

            MET04_0200 newData = repoMET04_0200.GetDTO(pTkCode);

            return View(newData);
        }


        /// <summary>
        /// (修改區) 主檔的新增頁面將值存進DB
        /// </summary>
        /// <param name="form">畫面上輸入的form元件中的控制項集合</param>
        /// <param name="model">要存檔的model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Insert(FormCollection form, MET04_0200 model)
        {
            // MVC model驗證
            if (ModelState.IsValid)
            {
                // 自訂義 資料驗證
                bool bIsOK = Chk_Ins_Main(form);
                //取得歷程
                MET04_0200 data = new MET04_0200();
                comm.Set_ModelValue(data, form);
                ViewBag.mo_code = data.mo_code;

                // 資料驗證失敗
                if (!bIsOK)
                {
                    ViewBag.rec_data = Get_RecData(data.mo_code);
                    ViewBag.showErrMsg = true;
                    ViewBag.prg_code = sPrgCode;
                    return View(data);
                }

                //執行存檔

                //在取完畫面上的值後，如果有一些別名欄位要變更值，可以在這邊2次加工
                string mo_code = data.mo_code;
                data.ISM03 = data.ISM03 * data.ISM0301;
                data.ISM04 = data.ISM04 * data.ISM0401;
                data.ureport_code = comm.Get_TkCode(sPrgCode);
                data.pro_unit = comm.Get_QueryData("MET01_0000", mo_code, "mo_code", "pro_unit");
                data.usr_code = User.Identity.Name;
                data.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
                data.ins_time = DateTime.Now.ToString("HH:mm:ss");
                data.sap_scr_no = comm.Get_SapScrNo(mo_code);

                repoMET04_0200.InsertData(data);
                // 新增DTS01
                comm.Ins_DTS01_0000("ZMES9999", User.Identity.Name, data);
                // 新增紀錄資料
                comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);
                ViewBag.rec_data = Get_RecData(data.mo_code);

                //存完檔回到主頁，如果不跳回主頁要在這裡做修改
                //return RedirectToAction("Index");

            }
            ViewBag.showErrMsg = false;
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
        public ActionResult Update(FormCollection form, MET04_0200 model)
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
                MET04_0200 data = new MET04_0200();
                comm.Set_ModelValue(data, form);

                MET04_0200 sBefore = comm.GetData<MET04_0200>(data);
                repoMET04_0200.UpdateData(data);
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
        //[HttpPost]
        //public ActionResult Delete(string pTkCode)
        //{
        //    //刪除前的檢查要在JqGrid送出前檢查，所以對應Chk_Del_Main這個函數

        //    MET04_0200 sBefore = comm.GetData<MET04_0200>(pTkCode);
        //    // 新增DTS01
        //    comm.Ins_DTS01_0000("ZMES9994", User.Identity.Name, sBefore);

        //    repoMET04_0200.DeleteData(pTkCode);
        //    //刪除紀錄資料
        //    comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "delete", sBefore, "");
        //    return RedirectToAction("Index");
        //}

        public ActionResult Delete(string pTkCode)
        {
            //刪除前的檢查要在JqGrid送出前檢查，所以對應Chk_Del_Main這個函數
            MET04_0200 sBefore = comm.GetData<MET04_0200>(pTkCode);
            // 新增DTS01
            comm.Ins_DTS01_0000("ZMES9994", User.Identity.Name, sBefore);

            sBefore.is_del = "P";
            repoMET04_0200.UpdateData(sBefore);
            return RedirectToAction("Insert", sPrgCode, new { pTkCode = sBefore.mo_code });
        }
        /* 資料處理 向上 */

        //資料檢查 向下//
        //主檔的檢查
        [HttpPost]
        public ActionResult Check_Data(FormCollection form, MET04_0200 model)
        {
            bool isSuccess = false;
            //檢查資料代碼是否重覆
            string key = gmv.GetKey<MET04_0200>(new MET04_0200());
            string sWhere = "where " + key + "='" + form[key].ToString() + "'";
            bool hasRow = !comm.Chk_RelData("MET04_0200", sWhere);
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
            //檢查工單是否已確認
            if (comm.Chk_Mo_IsOk("MET04_0200", form["mo_code"]))
            {
                bIsOK = false;
                ModelState.AddModelError("mo_code", "工單已確認");
            }

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
        public string Get_MoData(string pMoCode, string pType)
        {
            string sReturn = "";

            //請購單與客戶欄位的欄位名稱不同，所以要另外對應
            sReturn = comm.Get_QueryData("MET01_0000", pMoCode, "mo_code", pType);
            return sReturn;
        }
        /// <summary>
        /// 抓取電費、燃費標準數量
        /// </summary>
        /// <param name="pMoCode"></param>
        /// <param name="pField"></param>
        /// <returns></returns>
        public decimal Get_SAP0100_Data(string pMoCode, string pField)
        {
            decimal sReturn = 0;
            string tmpStdQty = GD.Get_Data("SAP0100", pMoCode, "mo_code", "BMSCH").ToString();
            //標準數量
            decimal dStdQty = comm.sGetDecimal(GD.Get_Data("SAP0100", pMoCode, "mo_code", "BMSCH").ToString());
            //欄位標準值
            
            decimal dFieldQty = comm.sGetDecimal(GD.Get_Data("SAP0100", pMoCode, "mo_code", pField).ToString());
            if (dStdQty != 0)
            {
                sReturn = (dFieldQty / dStdQty);
            }

            return sReturn;
        }
        ///// <summary>
        ///// 抓取機時
        ///// </summary>
        ///// <param name="pMoCode"></param>
        ///// <returns></returns>
        //public decimal Get_MacTime (string pMoCode)
        //{
        //    decimal dMacTime = 0;
        //    string sSql = "";
        //    sSql = "SELECT TOP 1 DATEDIFF(HOUR, work_time_e , (SELECT TOP 1 work_time_s "+
        //           "                            FROM mem01_0000                         "+
        //           "                            WHERE mo_code = '"+ pMoCode + "'        "+
        //           "                            ORDER BY work_code)  ) as TimeDiff      "+
        //           "FROM mem01_0000                     "+
        //           "WHERE mo_code = '"+ pMoCode + "'    "+
        //           "ORDER BY work_code DESC             ";
        //    DataTable dtTmp = comm.Get_DataTable(sSql);
        //    if (dtTmp.Rows.Count > 0) {
        //        dMacTime  = comm.sGetDecimal(dtTmp.Rows[0]["TimeDiff"].ToString());

        //    }
        //    return dMacTime;

        //}
        public DateTime Get_WorkTimeSByMoCode(string pMoCode)
        {
            DateTime sWorkTimeS = DateTime.Now;
            string sSql = "";
            sSql = "Select Top 1 work_time_s " +
                   " from MEM01_0000 " +
                   " where mo_code ='" + pMoCode + "'" +
                   " order by work_code ";
            DataTable dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                if (DateTime.TryParse(dtTmp.Rows[0]["work_time_s"].ToString(), out sWorkTimeS))
                {
                    return sWorkTimeS;
                }
            }
            return sWorkTimeS;
        }
        public DateTime Get_WorkTimeEByMoCode(string pMoCode)
        {
            DateTime sWorkTimeE = DateTime.Now;
            string sWorkCode = Get_LastWorkCode(pMoCode);
            string sSql = "";
            sSql = "Select Top 1 work_time_e " +
                   " from MEM01_0000 " +
                   " where mo_code ='" + pMoCode + "'" +
                   "  and work_code='" + sWorkCode + "'" +
                   "  and work_time_e<>''";
            DataTable dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                if (DateTime.TryParse(dtTmp.Rows[0]["work_time_e"].ToString(), out sWorkTimeE))
                {
                    return sWorkTimeE;
                }
            }
            return sWorkTimeE;
        }
        public decimal Get_MacTimeSpan(DateTime pWorkTimeS, DateTime pWorkTimeE)
        {
            decimal dMacTime = 0;
            TimeSpan macTime = pWorkTimeE - pWorkTimeS;
            dMacTime = comm.sGetDecimal(macTime.TotalHours.ToString("0.00#"));

            return dMacTime;
        }
        public decimal Get_MEM01_ProQty(string pMoCode)
        {
            decimal dProQty = 0;
            string sWorkCode = Get_LastWorkCode(pMoCode);
            string sSql = "select ok_qty from MEM01_0000 where mo_code='" + pMoCode + "' and work_code='" + sWorkCode + "'";
            DataTable dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                dProQty = comm.sGetDecimal(dtTmp.Rows[0]["ok_qty"].ToString());
            }

            return dProQty;
        }
        /// <summary>
        /// 抓取工單最後站別
        /// </summary>
        /// <param name="pMoCode"></param>
        /// <returns></returns>
        public string Get_LastWorkCode(string pMoCode)
        {
            string sWorkCode = "";
            string sSql = "";
            sSql = "select * from MET01_0100 where mo_code ='" + pMoCode + "'";
            DataTable dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                foreach (DataRow row in dtTmp.Rows)
                {
                    switch (row["work_code"].ToString())
                    {
                        case "L1SMB05":
                            return "L1SMB05";
                        case "L2SMB05":
                            return "L2SMB05";
                    }
                }
            }
            return sWorkCode;
        }
        public decimal Get_Person(string pWorkTimeS,string pWorkTimeE,string pWorkDate)
        {
            decimal sCountPerson = 0;
            string sSql = "";
            sSql= "select count(*) as PersonCount from MED01_0100"+
                 " where date_s ='"+ pWorkDate + "' "+
                 " and time_s between '" + pWorkTimeS + "' and '" + pWorkTimeS + "'";
            DataTable dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                sCountPerson = comm.sGetDecimal(dtTmp.Rows[0]["PersonCount"].ToString());
            }
            return sCountPerson;
        }
        
    }
}