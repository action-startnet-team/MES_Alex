using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MES_WATER.Controllers
{
    public class DSB070AController : Controller
    {
        // GET: DSB070A
        public ActionResult Index()
        {
            //Session["IsCarousel"] = true;

            //bool IsCarousel = true;

            // 時間單位: ms
            //Session["Carousel_interval"] = 5000;

            //Response.Cookies.Add(new HttpCookie("test", "HelloCookie"));

            //HttpCookie cookie = new HttpCookie("Carousel");//初始化並設置Cookie的名稱
            //DateTime dt = DateTime.Now;
            ////TimeSpan ts = new TimeSpan(0, 0, 1, 0, 0);//過期時間為1分鐘
            //TimeSpan ts = new TimeSpan(7, 0, 0, 0, 0);//過期時間為7天
            //cookie.Expires = dt.Add(ts);//設置過期時間
            //cookie.Values.Add("active", "Y");
            //cookie.Values.Add("interval", "5000");
            //Response.AppendCookie(cookie);
            //TempData["activeCarousel"] = true;

            bool IsCarousel = true;

            // 時間單位: seconds
            int interval = 30;

            //return RedirectToAction("Index", "DSB040A");
            return RedirectToAction("Index", "DSB040A", new { IsCarousel = IsCarousel, interval = interval });
        }
    }
}