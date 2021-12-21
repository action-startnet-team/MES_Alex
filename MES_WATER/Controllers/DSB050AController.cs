using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MES_WATER.Models;

namespace MES_WATER.Controllers
{
    public class DSB050AController : Controller
    {
        Comm comm = new Comm();
        // GET: DSB050A
        public ActionResult Index(bool IsCarousel = false, int interval = 0)
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