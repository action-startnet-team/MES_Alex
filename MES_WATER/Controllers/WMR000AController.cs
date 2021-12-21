using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MES_WATER.Models;
using MES_WATER.Repository;
using Newtonsoft.Json;
using System.Collections;
using System.Data.SqlClient;
using Dapper;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace MES_WATER.Controllers
{
    [Authorize] //登入驗證
    [HandleError(View = "Error")]  //錯誤導向

    public class WMR000AController : Controller
    {
        //程式代號
        string sPrgCode = "WMR000A";

        //共用函式庫
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        public ActionResult Index()
        {
            return View();
        }

    }
}