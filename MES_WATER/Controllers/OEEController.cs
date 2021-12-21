using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MES_WATER.Models;
using MES_WATER.Repository;

namespace MES_WATER.Controllers
{
    public class OEEController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();

        // 用到的repo
        MAC01_0000Repository repoMAC01_0000 = new MAC01_0000Repository();

        // GET: OEE
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult block_01(string pMacCode, string pImgUrl = "", string pMacName = "")
        {
            // 機台基本資料
            MAC01_0000 MAC01_0000data = repoMAC01_0000.GetDTO(pMacCode);
            
            string sMacName = string.IsNullOrEmpty(pMacName) ? MAC01_0000data.mac_name : pMacName;

            string sImgUrl = string.IsNullOrEmpty(pImgUrl) ? MAC01_0000data.img_url : pImgUrl;

            ViewBag.mac_code = pMacCode;
            ViewBag.mac_name = sMacName;
            ViewBag.img_url = sImgUrl;
            return PartialView();
        }

        public PartialViewResult block_02(string pMacCode, string pImgUrl = "", string pMacName = "")
        {
            // 機台基本資料
            MAC01_0000 MAC01_0000data = repoMAC01_0000.GetDTO(pMacCode);

            string sMacName = string.IsNullOrEmpty(pMacName) ? MAC01_0000data.mac_name : pMacName;

            string sImgUrl = string.IsNullOrEmpty(pImgUrl) ? MAC01_0000data.img_url : pImgUrl;

            ViewBag.mac_code = pMacCode;
            ViewBag.mac_name = sMacName;
            ViewBag.img_url = sImgUrl;
            return PartialView();
        }

        [HttpPost]
        public JsonResult Get_OEE_List(string pDate, List<string> pMacCodeList)
        {
            if (string.IsNullOrEmpty(pDate))
            {
                pDate = comm.Get_Date();
            }

            DSB01_0000Repository repo = new DSB01_0000Repository();

            List<DSB01_0000> list = repo.Get_Data(pDate, pMacCodeList);

            //list = list.Where(x => pMacCodeList.Contains(x.mac_code)).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }


    }
}