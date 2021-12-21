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
using System.Web.Security;

namespace MES_WATER.Controllers
{
    public class EPBLoginController : Controller
    {
        EPB030AController CT_EPB030A = new EPB030AController();

        Comm comm = new Comm();
        GetData GD = new GetData();
        DynamicTable DT = new DynamicTable();
        CheckData CD = new CheckData();



        public string PrgCode() {
            return ControllerContext.RouteData.Values["controller"].ToString();
        }


        public ActionResult Login(string usr_code, string usr_pass)
        {
            ViewBag.prj_name = comm.Get_QueryData("BDP00_0000", "prj_name", "par_name", "par_value");
            ViewBag.com_name = comm.Get_QueryData("BDP00_0000", "com_name", "par_name", "par_value");
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection post)
        {
            //var obj = Server.CreateObject("Snfun1.dll");

            Comm comm = new Comm();
            ViewBag.prj_name = comm.Get_QueryData("BDP00_0000", "prj_name", "par_name", "par_value");
            ViewBag.com_name = comm.Get_QueryData("BDP00_0000", "com_name", "par_name", "par_value");

            string usr_code = post["usr_code"];
            string usr_pass = post["usr_pass"];
            string usr_name = comm.Get_QueryData("BDP08_0000", usr_code, "usr_code", "usr_name");
            string grp_code = comm.Get_QueryData("BDP08_0000", usr_code, "usr_code", "grp_code");

            if (comm.Chk_Login(usr_code, usr_pass))
            {
                //Session["usr_name"] = usr_name;
                //Session["usr_code"] = usr_code;
                //登入成功 轉頁
                var ticket = new FormsAuthenticationTicket(
                version: 1,
                name: usr_code.ToString(), //可以放使用者Id
                issueDate: DateTime.UtcNow,//現在UTC時間
                expiration: DateTime.UtcNow.AddMinutes(30),//Cookie有效時間=現在時間往後+30分鐘
                isPersistent: true,// 是否要記住我 true or false
                userData: grp_code, //可以放使用者角色名稱
                cookiePath: FormsAuthentication.FormsCookiePath);

                var encryptedTicket = FormsAuthentication.Encrypt(ticket); //把驗證的表單加密
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                Response.Cookies.Add(cookie);


                // 儲存資訊到BDP20_0000
                comm.Ins_BDP20_0000(usr_code, "Login", "Login", "登入時間: " + comm.Get_Time());

                FormsAuthentication.RedirectFromLoginPage(usr_code, false);

                return RedirectToAction("SelectProject", PrgCode(), null);
                //return RedirectToAction("Index", "Blank", null);
            }
            ViewBag.Message = "登入失敗，請檢查帳號密碼";
            return View();
        }


        public ActionResult LogOut()
        {
            // 儲存資訊到BDP20_0000
            comm.Ins_BDP20_0000(User.Identity.Name, "Logout", "Login", "登出時間: " + comm.Get_Time());
            //comm.Ins_BDP20_0000(User.Identity.Name, "Logout", "Logout", "登出時間: " + comm.Get_Time());

            FormsAuthentication.SignOut();

            return RedirectToAction("Login", PrgCode(), null);
        }

        // GET: EPBLogin
        public ActionResult Index()
        {
            Set_Cookie();
            return View();
        }


        public ActionResult Report(string K)
        {
            ViewBag.Key = K;

            return View();
        }

        [HttpPost]
        public ActionResult Report(FormCollection form)
        {
            Ins_EPBData(form);

            return RedirectToAction("Index");
        }


        public ActionResult SelectProject() {

            return View();
        }

        public ActionResult ReadQRCode()
        {
            return View();
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
            string sQmtCode = comm.sGetString(form["qmt_code"]);
            string qmt04_0100_Array = Get_QmtItem(sQmtCode, "qmt04_0100");

            if (!string.IsNullOrEmpty(qmt04_0100_Array)) {
                for (int i = 0; i < qmt04_0100_Array.Split(',').Length; i++){
                    string qmt04_0100 = qmt04_0100_Array.Split(',')[i];
                    string sQtestItemCode = Get_QmtItem(sQmtCode, "qtest_item_code").Split(',')[i];
                    string sValue = "N";
                    if (comm.sGetString(form[qmt04_0100]) == "on") {
                        sValue = "Y";
                    }

                    data = new
                    {
                        qmt04_0100 = qmt04_0100,
                        qtest_item_code = sQtestItemCode,
                        qmt_value = sValue,
                        ins_date = DateTime.Now.ToString("yyyy/MM/dd"),
                        ins_time = DateTime.Now.ToString("HH:mm:ss"),
                        usr_code = User.Identity.Name,
                    };
                    DT.InsertData("QMT04_0110", data);
                }
            }
            return RedirectToAction("QMTReport_Mobile", new { K= sQmtCode });
        }


        public string Get_QmtItem(string pQmtCode,string pField) {
            string sSql = "";
            sSql = "select QMT04_0100.*,QMB02_0000.qtest_item_name " +
                   "  from QMT04_0100" +
                   "  left join QMB02_0000 on QMT04_0100.qtest_item_code = QMB02_0000.qtest_item_code" +
                   " where QMT04_0100.qmt_code = @qmt_code" +
                   "   and QMT04_0100.qtest_item_type = 'C'"; //手機板只做外觀檢
            var dtTmp = comm.Get_DataTable(sSql, "qmt_code", pQmtCode);
            return GD.DataFieldToStr(dtTmp, pField);
        }


        public string EPBFieldTable = "EPB02_0100";

        //索引鍵
        public string EPBPKCode()
        {
            return DT.Get_Table_PKField(EPBFieldTable);
        }


        /// <summary>
        /// 新增電子表單資料
        /// </summary>
        /// <param name="form"></param>
        public void Ins_EPBData(FormCollection form)
        {
            string sSql = "select * from EPB02_0100 " +
                          " where epb_code = '" + form["epb_code"] + "'" +
                          "  order by scr_no";
            var dtTmp = comm.Get_DataTable(sSql);
            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                DataRow r = dtTmp.Rows[i];
                string Key = r["epb02_0100"].ToString();
                string sFieldCode = r["field_code"].ToString();
                string sFieldName = r["field_name"].ToString();
                string sCtrType = r["ctr_type"].ToString();
                string sCtrDefaultValue = r["ctr_default_value"].ToString();
                string sFieldValue = comm.sGetString(form[sFieldCode]);

                //核取勾選原本是顯示"on" 輸入DB改成"V"
                if (sCtrType == "C" && !string.IsNullOrEmpty(sFieldValue)) { sFieldValue = "V"; }

                //預設值如果給系統參數
                //則直接在後端給值
                switch (sCtrDefaultValue)
                {
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
        /// 取得表單索引欄位
        /// </summary>
        /// <param name="Key">表單代號</param>
        /// <returns></returns>
        public string Get_KeyField(string epb_code)
        {
            string sValue = "";
            string sSql = "select * from EPB02_0100 " +
                          " where epb_code = '" + epb_code + "' " +
                          "   and is_key = 'Y'";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                sValue = dtTmp.Rows[0]["field_code"].ToString();
            }
            return sValue;
        }

        /// <summary>
        /// 取得表單類型
        /// </summary>
        /// <returns></returns>
        public string Get_EpbType()
        {
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
                          " where epb_type_code = '" + Key + "'";
            return GD.DataFieldToSTA(sSql, "epb_code,epb_name");
        }


        public string Get_CommonStr(string Key)
        {
            string sSql = "select * from EPB02_0200 " +
                          " where epb02_0100 = '" + Key + "'" +
                          "  order by scr_no";
            return GD.DataFieldToStr(sSql, "option_name");
        }



        public string Get_Common_OptionName(string Id,string Val) {
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
        public string Chk_Input(string Key, string pValue)
        {
            return CD.Chk_Input(EPBFieldTable, EPBPKCode(), Key, pValue);
        }

        /// <summary>
        /// 檢查該表單是否只有一個鍵值
        /// </summary>
        /// <param name="Key">索引鍵</param>
        /// <param name="pValue">索引值</param>
        /// <returns></returns>
        public bool Chk_OnlyKey(string pEpbCode)
        {
            return CD.Chk_OnlyKey(pEpbCode);
        }

        /// <summary>
        /// 取得未判定的計畫檢驗紀錄表
        /// </summary>
        /// <param name="Key">料號</param>
        /// <returns></returns>
        public string Get_QmtCode(string Key)
        {
            //在QMT04_0000裡面找計劃檢驗表，條件為未通過(is_rec <> 'Y')
            string sSql = "select QMT04_0000.qmt_code,qsheet_name " +
                          "  from QMT04_0000 " +
                          "  left join QMB03_0200 on QMT04_0000.pro_code = QMB03_0200.pro_code" +
                          "  left join QMB03_0000 on QMB03_0200.qsheet_code = QMB03_0000.qsheet_code" +
                          " where QMT04_0000.pro_code = '" + Key + "'" +
                          "   and is_rec = 'P'";
            return GD.DataFieldToSTA(sSql, "qmt_code,qsheet_name");
        }

        /// <summary>
        /// 取得未判定的計畫檢驗紀錄表
        /// </summary>
        /// <param name="Key">料號</param>
        /// <returns></returns>
        public string Get_QmtCodeByQRCode(string pQRCode)
        {
            //在QMT04_0000裡面找計劃檢驗表，條件為未通過(is_rec <> 'Y')
            string sSql = "select QMT04_0000.qmt_code,qsheet_name " +
                          "  from QMT04_0000 " +
                          "  left join QMB03_0200 on QMT04_0000.pro_code = QMB03_0200.pro_code" +
                          "  left join QMB03_0000 on QMB03_0200.qsheet_code = QMB03_0000.qsheet_code" +
                          "  left join WMT0200 on QMT04_0000.wmt0200 = WMT0200.wmt0200 " +
                          " where barcode = '" + pQRCode + "'" +
                          "   and is_rec <> 'Y'";
            return GD.DataFieldToSTA(sSql, "qmt_code,qsheet_name");
        }



        /// <summary>
        /// 儲存cookie
        /// </summary>
        /// <param name="pCookieName"></param>
        /// <param name="pValue"></param>
        public void Save_Cookie(string pCookieName, string pValue)
        {
            Response.Cookies[pCookieName].Value = pValue;
        }


        /// <summary>
        /// 設定cookie
        /// </summary>
        public void Set_Cookie()
        {
            //紀錄cookie
            if (Request.Cookies["EpbType"] == null)
            {
                HttpCookie EpbType = new HttpCookie("EpbType")
                {
                    Value = "",
                    Expires = DateTime.Now.AddDays(1d),
                };
                Response.Cookies.Add(EpbType);
            }
            if (Request.Cookies["EpdCode"] == null)
            {
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