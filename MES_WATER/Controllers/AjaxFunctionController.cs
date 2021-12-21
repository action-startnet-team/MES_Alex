using MES_WATER.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MES_WATER.Controllers
{
    public class AjaxFunctionController : Controller
    {
        Comm comm = new Comm();
        GetData GD = new GetData();
        DynamicTable DT = new DynamicTable();
        CheckData CD = new CheckData();
        // GET: AjaxFunction
        public ActionResult Index()
        {
            return View();
        }


        //Ajax函示庫
        //需使用時在view裡面加上這行，即可使用裡面的function
        //Html.RenderAction("AjaxFunction", "AjaxFunction");
        public ActionResult AjaxFunction()
        {
            return PartialView();
        }

        //前端模組插件
        //需使用時在view裡面加上這行，即可使用裡面的function
        //Html.RenderAction("ViewPlugin", "AjaxFunction");
        public ActionResult ViewPlugin()
        {
            return PartialView();
        }



        /// <summary>
        /// 取得資訊
        /// </summary>
        /// <param name="T">資料庫</param>
        /// <param name="K">鍵值</param>
        /// <param name="KF">鍵值欄位</param>
        /// <param name="F">欄位</param>
        /// <returns></returns>
        public string Get_Data(string T, string K, string KF, string F)
        {
            return GD.Get_Data(T, K, KF, F);
        }


        public void Upd_Data(string pTableCode, string pKeyCode, string pKeyValue, string pFieldCode, string pFieldValue)
        {
            comm.Upd_QueryData(pTableCode, pKeyCode, pKeyValue, pFieldCode, pFieldValue);
        }


        public string ParseToDecimel_G29(string pString) {
            string val = "0";
            try {
                val = decimal.Parse(pString).ToString("G29");
            } catch(IOException e) {
                return val;
            }
            return val;
        }


        public bool IsDate(string strDate)
        {
            return CD.IsDate(strDate);
        }





    }
}