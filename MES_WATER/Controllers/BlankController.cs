using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MES_WATER.Models;
using System.Data;
using MES_WATER.Repository;

namespace MES_WATER.Controllers
{
    [Authorize]
    [HandleError(View = "Error")]
    public class BlankController : Controller
    {        
        // GET: Blank
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            return View();
        }
    }

   

}