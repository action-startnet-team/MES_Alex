using MES_WATER.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MES_WATER.Controllers
{
    public class MailLoginController : Controller
    {
        Mail mail = new Mail();
        Comm comm = new Comm();
        
        public ActionResult Index(string pToken)
        {            
            string sSql = "select * from mailLog " +
                         " where token = '" + pToken + "' ";
            var dtTmp = comm.Get_DataTable(sSql);

            if (dtTmp.Rows.Count > 0) { 
                string sAccount = mail.Get_UsrData(dtTmp.Rows[0]["usr_code"].ToString(), "usr_code");
                string sPassword = mail.Get_UsrData(dtTmp.Rows[0]["usr_code"].ToString(), "usr_pass");
                string sGrpCode = mail.Get_UsrData(dtTmp.Rows[0]["usr_code"].ToString(), "grp_code");
                
                //進行自動登入，存indenity.name
                if (comm.Chk_Login(sAccount, sPassword))
                {                    
                    //確認Token在MailLog Table是否存在，不存在拒絕登入，並轉回login頁面
                    var ticket = new FormsAuthenticationTicket(
                    version: 1,
                    name: sAccount.ToString(), //可以放使用者Id
                    issueDate: DateTime.UtcNow,//現在UTC時間
                    expiration: DateTime.UtcNow.AddMinutes(30),//Cookie有效時間=現在時間往後+30分鐘
                    isPersistent: true,// 是否要記住我 true or false
                    userData: sGrpCode, //可以放使用者角色名稱
                    cookiePath: FormsAuthentication.FormsCookiePath);

                    var encryptedTicket = FormsAuthentication.Encrypt(ticket); //把驗證的表單加密
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(cookie);

                    FormsAuthentication.RedirectFromLoginPage(sAccount, false);
                }
                

                sSql = "select * from MFT02_0100 " +
                       " where inq_code = '" + dtTmp.Rows[0]["tk_code"].ToString() + "' " +
                       "   and TA006 = '" + sAccount + "'";
                var dtTmp2 = comm.Get_DataTable(sSql);

                //依信件類型轉址， 詢價單通知Type='A01' http://test.startnet.com.tw:232/SUT010B/Update?pTkCode=15
                switch (dtTmp.Rows[0]["mail_type"].ToString()) {
                    case "A01":
                        return RedirectToAction("Update", "SUT010B", new { pTkCode = dtTmp2.Rows[0]["mft02_0100"].ToString() });
                    default:
                        return RedirectToAction("Index", "Login", null);
                }
            }


            return RedirectToAction("Index", "Login", null);
        }


        

    }
}