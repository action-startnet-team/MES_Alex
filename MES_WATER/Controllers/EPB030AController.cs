using Dapper;
using MES_WATER.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using NPOI;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Data;
using Newtonsoft.Json;

namespace MES_WATER.Controllers
{

    public class EPB030AController : Controller
    {
        Comm comm = new Comm();
        GetData GD = new GetData();
        DynamicTable DT = new DynamicTable();
        CheckData CD = new CheckData();
        WebReference.WmsApi WA = new WebReference.WmsApi();

        //表單欄位table
        public string pubFieldTable = "EPB02_0100";

        //索引鍵
        public string pubPKCode() {
            return DT.Get_Table_PKField(pubFieldTable);
        }

        // GET: EPB030A
        public ActionResult Index()
        {
            Set_Cookie();

            return View();
        }


        public ActionResult Report(string K,string V = "")
        {
            //K=表單代號，V=電子表單資料鍵值
            ViewBag.Key = K;
            ViewBag.Value = V;
            return View();
        }

        [HttpPost]
        public ActionResult Report(FormCollection form)
        {
            object data = new object();
            //先檢查表單的存檔類別
            string sEpbCode = comm.sGetString(form["epb_code"]);//表單代號
            string sSaveType = comm.Get_QueryData("EPB02_0000", form["epb_code"], "epb_code", "save_type");
            string sSaveMethod = comm.Get_QueryData("EPB02_0000", form["epb_code"], "epb_code", "save_method");
            string sKeyValue = comm.sGetString(form[Get_KeyField(form["epb_code"])]);//表單資料鍵值

            switch (form["submit"]) {
                case "save":
                    //儲存
                    switch (sSaveType)
                    {
                        case "A":
                            //寫入電子資料
                            Ins_EPBData(form);
                            //寫入審核資料
                            //Ins_Review(sKeyValue);
                            break;
                        case "B":
                            //指定table
                            DT.Dynamic_Insert(form, sSaveMethod);
                            break;
                        case "C":
                            //web api
                            string sApiFieldArray = GD.Get_Data("EPB02_0100", sEpbCode, "epb_code", "field_code");
                            string sToken = GD.Get_Data("BDP08_0000", User.Identity.Name, "usr_code", "token");

                            if (!string.IsNullOrEmpty(sApiFieldArray))
                            {
                                string JsonApi = GD.DataToJson(sApiFieldArray, form);

                                switch (sSaveMethod)
                                {
                                    case "Ins_Login":
                                        WA.Ins_Login(sToken, JsonApi);
                                        break;
                                    case "Ins_Logout":
                                        WA.Ins_Logout(sToken, JsonApi);
                                        break;
                                    case "Get_PreparList":
                                        WA.Get_PreparList(sToken, DateTime.Now.ToString("yyyy/MM/dd"), "");
                                        break;
                                    case "Ins_StopData":
                                        WA.Ins_StopData(sToken, JsonApi);
                                        break;
                                }
                            }
                            break;
                        case "D":
                            //檢驗紀錄
                            //string sNewScrNo = (comm.Get_AutoIntMax("QMT01_0000", "scr_no", "") + 1).ToString();
                            //string sProCodeArray = comm.Get_Data("QMB03_0200", sEpbCode, "qsheet_code", "pro_code"); //檢驗表關聯的料號
                            //for (int i = 0;i < sProCodeArray.Split(',').Length;i++) {
                            //    string sProCode = sProCodeArray.Split(',')[i];
                            //    data = new
                            //    {
                            //        qmt_code = sEpbCode + sNewScrNo.PadLeft(5, '0'),
                            //        pur_code = sEpbCode,
                            //        scr_no = sNewScrNo,
                            //        pro_code = sProCode,
                            //        ins_date = DateTime.Now.ToString("yyyy/MM/dd"),
                            //        ins_time = DateTime.Now.ToString("HH:mm:ss"),
                            //        usr_code = User.Identity.Name,
                            //    };
                            //}
                            
                            break;
                    }
                    break;
                case "modify":
                    //刪除電子資料
                    comm.Del_QueryData("EPB03_0000", "key_value", sKeyValue);
                    //寫入電子資料
                    Ins_EPBData(form);                    
                    break;
            }            
            return RedirectToAction("DataView","EPB030B",new { K = form["epb_code"] });
        }


        /// <summary>
        /// 新增電子表單資料
        /// </summary>
        /// <param name="form"></param>
        public void Ins_EPBData(FormCollection form) {
            string sSql = "select * from EPB02_0100 " +
                          " where epb_code = '" + form["epb_code"] + "'" +
                          "  order by scr_no";
            var dtTmp = comm.Get_DataTable(sSql);
            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                DataRow r = dtTmp.Rows[i];
                string Key = r[pubPKCode()].ToString();
                string sFieldCode = r["field_code"].ToString();
                string sFieldName = r["field_name"].ToString();
                string sCtrType = r["ctr_type"].ToString();
                string sCtrDefaultValue = r["ctr_default_value"].ToString();
                string sFieldValue = comm.sGetString(form[sFieldCode]);

                //核取勾選原本是顯示"on" 輸入DB改成"V"
                if (sCtrType == "C" && !string.IsNullOrEmpty(sFieldValue)) { sFieldValue = "V"; }

                //預設值如果給系統參數
                //則直接在後端給值
                switch (sCtrDefaultValue) {
                    case "INSDATE":
                        sFieldValue = DateTime.Now.ToString("yyyy/MM/dd");
                        break;
                    case "INSTIME":
                        sFieldValue = DateTime.Now.ToString("HH:mm:ss");
                        break;
                    case "USER":
                        sFieldValue = User.Identity.Name;
                        break;
                }

                object data = new object();
                data = new
                {
                    epb_code = form["epb_code"],
                    key_code = Get_KeyField(form["epb_code"]),
                    key_value = form[Get_KeyField(form["epb_code"])],
                    field_code = sFieldCode,
                    field_value = sFieldValue,
                    ins_date = DateTime.Now.ToString("yyyy/MM/dd"),
                    ins_time = DateTime.Now.ToString("HH:mm:ss"),
                    usr_code = User.Identity.Name,
                };
                DT.InsertData("EPB03_0000", data);
            }
        }

        /// <summary>
        /// 新增一筆審核作業
        /// </summary>
        /// <param name="Key">表單資料鍵值</param>
        public void Ins_Review(string Key)
        {
            object data = new object();
            string sEpbCode = comm.Get_QueryData("EPB03_0000", Key, "field_value", "epb_code");
            string sReviewCode = comm.Get_QueryData("EPB04_0000", sEpbCode, "epb_code", "review_code");

            //檢查此表單是否有審核設定作業
            if (Chk_CanReview(sEpbCode)) {            
                data = new
                {
                    review_code = sReviewCode,                
                    epb_code = sEpbCode,
                    epb_key = Key,
                    result_code = "",
                    is_ok = "P",
                    scr_no = "1",
                    ins_date = DateTime.Now.ToString("yyyy/MM/dd"),
                    ins_time = DateTime.Now.ToString("HH:mm:ss"),
                    usr_code = User.Identity.Name,
                    out_date = "",
                    out_time = "",
                    next_usr_code = "",
                    review_memo = "",
                };
                DT.InsertData("EPB05_0000", data);

                data = new
                {
                    todo_code = comm.Get_TkCode("ToDoList"),
                    todo_name = "您有新的待審核電子資料，審核代號:" + sReviewCode,
                    todo_url = "/EPB050A/Report?K=",
                    todo_key = Key,
                    is_use = "Y",
                    is_ok = "N",
                    usr_code = User.Identity.Name,
                };
                DT.InsertData("BDP16_0000", data);
            }
        }


        /// <summary>
        /// 檢查電子表單資料鍵值是否已經出現過
        /// </summary>
        /// <param name="Key">電子表單資料鍵值</param>
        /// <returns></returns>
        public bool Chk_IsHaveKeyValue(string Key) {
            bool sValue = false;
            string sSql = "select * from EPB03_0000 " +
                          " where field_value = @field_value";
            var dtTmp = comm.Get_DataTable(sSql, "field_value", Key);
            if (dtTmp.Rows.Count > 0) {
                sValue = true;
            }
            return sValue;
        }

        /// <summary>
        /// 修改表單資料
        /// </summary>
        /// <param name="form"></param>
        /// <param name="Key">表單資料鍵值</param>
        public void Upd_EpbData(FormCollection form,string Key){
            string sSql = "select * from EPB03_0000 " +
                          " where key_value = @key_value ";
            var dtTmp = comm.Get_DataTable(sSql, "key_value", Key);
            for (int i = 0; i < dtTmp.Rows.Count; i++) {
                string sFieldCode = dtTmp.Rows[i]["field_code"].ToString();
                string sFieldValue = comm.sGetString(form[sFieldCode]);
                if (!string.IsNullOrEmpty(sFieldValue)) {
                    sSql = "update EPB03_0000 " +
                       "  set field_value = @field_value " +
                       " where key_value  = @key_value " +
                       "   and field_code = @field_code ";
                    comm.Connect_DB(sSql, "field_value,key_value,field_code", sFieldValue + "," + Key + "," + sFieldCode);
                }               
            }
        }



        /// <summary>
        /// 檢查是否已經設定審核作業
        /// </summary>
        /// <param name="pEpbCode"></param>
        /// <returns></returns>
        public bool Chk_CanReview(string pEpbCode)
        {
            bool sValue = false;
            string sSql = "select * from EPB04_0000" +
                          " where epb_code = '" + pEpbCode + "'" +
                          "   and is_use = 'Y'";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                sValue = true;
            }
            return sValue;
        }


        /// <summary>
        /// 取得表單索引欄位
        /// </summary>
        /// <param name="Key">表單代號</param>
        /// <returns></returns>
        public string Get_KeyField(string epb_code) {
            string sValue = "";
            string sSql = "select * from EPB02_0100 " +
                          " where epb_code = '" + epb_code + "' " +
                          "   and is_key = 'Y'";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0) {
                sValue = dtTmp.Rows[0]["field_code"].ToString();
            }
            return sValue;
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

        /// <summary>
        /// 取得表單類型
        /// </summary>
        /// <returns></returns>
        public string Get_EpbType() {
            //取得表單類型
            string sSql = "select * from EPB01_0000 ";
            return comm.DataFieldToStr(sSql, "epb_type_code");
        }

        /// <summary>
        /// 取得表單代號
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public string Get_EpbCode(string Key)
        {
            string sSql = "select * from EPB02_0000 " + 
                          " where epb_type_code = '"+ Key + "'";
            return GD.DataFieldToSTA(sSql, "epb_code,epb_name");
        }



        public string Get_FieldValue(string Key, string pFieldCode)
        {
            string sSql = "select * from EPB03_0000 " +
                          " where key_value = '" + Key + "'" +
                          "   and field_code = '" + pFieldCode + "'";
            return GD.DataFieldToStr(sSql, "field_value");
        }

        public string Get_CommonStr(string Key) {
            string sSql = "select * from EPB02_0200 " +
                          " where epb02_0100 = '" + Key + "'" +
                          "  order by scr_no";
            return GD.DataFieldToStr(sSql, "option_name");
        }

        public string Get_Common_OptionName(string Id, string Val)
        {
            string sValue = "";
            string sSql = "select * from EPB02_0200 " +
                          " where epb02_0100 = '" + Id + "' " +
                          "   and option_code = '" + Val + "'";
            sValue = GD.DataFieldToStr(sSql, "option_name");
            return sValue;
        }

        /// <summary>
        /// 檢查input
        /// </summary>
        /// <param name="Key">索引鍵</param>
        /// <param name="pValue">索引值</param>
        /// <returns></returns>
        public string Chk_Input(string Key,string pValue,bool pIsChkMulti = true) {
            return CD.Chk_Input(pubFieldTable, pubPKCode(), Key, pValue, pIsChkMulti);
        }

        

        /// <summary>
        /// 檢查該表單是否只有一個鍵值
        /// </summary>
        /// <param name="pEpbCode">表單代號</param>
        /// <returns></returns>
        public bool Chk_OnlyKey(string pEpbCode)
        {
            return CD.Chk_OnlyKey(pEpbCode);
        }


        /// <summary>
        /// 儲存cookie
        /// </summary>
        /// <param name="pCookieName"></param>
        /// <param name="pValue"></param>
        public void Save_Cookie(string pCookieName,string pValue) {
            Response.Cookies[pCookieName].Value = pValue;
        }


        /// <summary>
        /// 設定cookie
        /// </summary>
        public void Set_Cookie()
        {
            //紀錄cookie
            if (Request.Cookies["EpbType"] == null) {
                HttpCookie EpbType = new HttpCookie("EpbType")
                {
                    Value = "",
                    Expires = DateTime.Now.AddDays(1d),
                };
                Response.Cookies.Add(EpbType);
            }
            if (Request.Cookies["EpdCode"] == null) {
                HttpCookie EpdCode = new HttpCookie("EpdCode")
                {
                    Value = "",
                    Expires = DateTime.Now.AddDays(1d),
                };
                Response.Cookies.Add(EpdCode);
            }
        }

       

    }
}