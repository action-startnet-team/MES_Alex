using MES_WATER.Models;
using MES_WATER.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace MES_WATER.Controllers
{
    [Authorize] //登入驗證
    //[HandleError(View = "Error")]  //錯誤導向
    public class DSB010CController : JsonNetController
    {
        Comm comm = new Comm();


        public ActionResult Index()
        {

            return RedirectToAction("Line1", new { IsCarousel = true, interval = 10 });
            //return View();
        }

        public ActionResult Line1(bool IsCarousel = false, int interval = 0)
        {
            if (IsCarousel && interval != 0)
            {

                ViewBag.IsCarousel = IsCarousel;
                ViewBag.interval = interval;
            }

            //設定signalR Hub來源
            ViewBag.signalr_url = comm.Get_QueryData("BDP00_0000", "signalr_url", "par_name", "par_value");

            return View();
        }

        public ActionResult Line2(bool IsCarousel = false, int interval = 0)
        {
            if (IsCarousel && interval != 0)
            {

                ViewBag.IsCarousel = IsCarousel;
                ViewBag.interval = interval;
            }

            //設定signalR Hub來源
            ViewBag.signalr_url = comm.Get_QueryData("BDP00_0000", "signalr_url", "par_name", "par_value");

            return View();
        }

    }
}