using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MES_WATER.Models
{
    public class AllowedIpOnlyAttribute: AuthorizeAttribute
    {
        private string[] ipList = new string[] { };

        //建構式接收以逗號或分號分隔的IP清單，限定存取來源
        //TODO: 如要方便事後修改，可擴充成由config讀取IP清單，但會增加被破解風險
        public AllowedIpOnlyAttribute(string allowedIps)
        {
            ipList = allowedIps.Split(',', ';');
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //實作OnAuthorization，當來源IP不在清單上，彈出錯誤
            string clientIp = filterContext.HttpContext.Request.UserHostAddress;
            if (!ipList.Contains(clientIp))
            {
                throw new Exception("Disallowed Client IP: " + clientIp + "! ");
            }
        }
    }

    //限定本機存取為AllowedIpOnlyAttribute的特殊情境，限定IP=::1或127.0.0.1
    public class LocalhostOnlyAttribute: AllowedIpOnlyAttribute
    {
        public LocalhostOnlyAttribute():base("::1;127.0.0.1")
        {

        }
    }

}