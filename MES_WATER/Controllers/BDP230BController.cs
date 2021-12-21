using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MES_WATER.Models;
using MES_WATER.Repository;

namespace MES_WATER.Controllers
{

    public class BDP230BController : Controller
    {

        BDP230BRepository repo = new BDP230BRepository();

        // GET: Announcement
        //public ActionResult board(string id)
        //{
        //    ViewBag.bdp23_0000 = id;
             
        //    return View();
        //}
        public ActionResult Index()
        {
            ViewBag.bdp23_0000 = "";
            return View();
        }

        public JsonResult Get_BoardData(string pSorDate, string pShowOKData)
        { 

            List<BDP23_0000> list = repo.Get_BoardData(User.Identity.Name, pSorDate, pShowOKData);

            //list = list.Take(10).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void Save_BoardData(string pTkCode, string pIsOk, string pOkDate, string pUsrMemo)
        {

            repo.UpdateData(pTkCode, pIsOk, pOkDate, pUsrMemo);

        }


    }
}