using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace MES_WATER.Controllers
{
    public class TablesController : JsonNetController
    {
        // GET: Tables
        public ActionResult Index()
        {
            return View();
        }
    }
}