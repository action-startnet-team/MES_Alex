using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MES_WATER.Models;

namespace MES_WATER.Controllers
{
    public class DSB040AController : Controller
    {
        Comm comm = new Comm();
        // GET: DSB040A

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

        public ActionResult Index_bak(bool IsCarousel = false, int interval = 0)
        {

            //bool IsCarousel = (bool)Session["IsCarousel"];
            //int interval = (int)Session["Carousel_interval"];

            string controller = ControllerContext.RouteData.Values["controller"].ToString();


            if ((TempData["activeCarousel"] != null && (bool)TempData["activeCarousel"]))
            {

            }
            if (IsCarousel && interval != 0)
            {
                //Session["Carousel_group"] = true;

                //var cookie = Request.Cookies["Carousel"];

                //bool IsCarousel = cookie["active"] == "Y" ? true : false;
                //int interval = int.Parse(cookie["interval"]);

                //if (IsCarousel)
                //{
                //    ViewBag.IsCarousel = IsCarousel;
                //    ViewBag.interval = interval;
                //}

                ViewBag.IsCarousel = IsCarousel;
                ViewBag.interval = interval;
            }


            //ViewBag.test = Request.Cookies["Carousel"]["active"];

            //string IsCarousel = Response.Cookies["IsCarousel"].Value;
            //string interval = Response.Cookies["Carousel_interval"].Value;

            //if (IsCarousel == "Y")
            //{
            //    ViewBag.IsCarousel = IsCarousel;
            //    ViewBag.interval = interval;
            //}

            return View();
        }

    }
}