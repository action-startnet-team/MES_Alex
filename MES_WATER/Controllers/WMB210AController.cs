using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MES_WATER.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace MES_WATER.Controllers
{
    public class WMB210AController : JsonNetController
    {
        Comm comm = new Comm();

        // GET: DSB100a
        public ActionResult Index()
        {
            return View();
        }

    }
}