using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MES_WATER.Models;
using Newtonsoft.Json;

namespace MES_WATER.Controllers
{
    public class MenuController : JsonNetController
    {
        // GET: Menu
        Comm comm = new Comm();
        public ActionResult pMenu()
        {
            string usr_code = HttpContext.User.Identity.Name;
            //usr_code = "";  //測試空值使用
            if (string.IsNullOrEmpty(usr_code))
            {
                string sUrl = Request.Url.AbsoluteUri;
                sUrl = Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port + "/LoginTimeOut";

                //強制結束
                string sScript = "";
                sScript = "<script language='javascript' type='text/javascript'>";
                sScript += "document.location.href='" + sUrl + "'";
                sScript += "</script>";

                return Content(sScript);
            }
            else
            {
                List<BDP03_0000> menu_list_1 = comm.Get_BDP03_0000(usr_code, "1");
                List<BDP03_0000> menu_list_2 = comm.Get_BDP03_0000(usr_code, "2");
                List<BDP03_0000> menu_list_3 = comm.Get_BDP03_0000(usr_code, "3");
                List<BDP03_0000> menu_list_4 = comm.Get_BDP03_0000(usr_code, "4");

                ViewBag.menu_list_1 = menu_list_1;
                ViewBag.menu_list_2 = menu_list_2;
                ViewBag.menu_list_3 = menu_list_3;
                ViewBag.menu_list_4 = menu_list_4;

                return PartialView();
            }
        }


    }
}