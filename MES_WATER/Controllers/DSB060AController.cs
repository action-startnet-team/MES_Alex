using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MES_WATER.Models;

namespace MES_WATER.Controllers
{
    public class DSB060AController : Controller
    {
        Comm comm = new Comm();
        // GET: DSB060A
        public ActionResult Index_old()
        {
            return View();
        }
        public ActionResult Index()
        {
            //設定signalR Hub來源
            ViewBag.signalr_url = comm.Get_QueryData("BDP00_0000", "signalr_url", "par_name", "par_value");

            return View();
        }
    }
}