using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MES_WATER.Models;
using MES_WATER.Repository;

namespace MES_WATER.Controllers
{

    public class APKController : Controller
    {


        // GET: Announcement
        //public ActionResult board(string id)
        //{
        //    ViewBag.bdp23_0000 = id;
             
        //    return View();
        //}
        public ActionResult Index()
        {
            ViewBag.APK = "";
            return View();
        }



    }
}