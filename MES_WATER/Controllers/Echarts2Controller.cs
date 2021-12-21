using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MES_WATER.Controllers
{
    public class Echarts2Controller : Controller
    {
        // GET: Echarts
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 由連字元分隔的32位數字
        /// </summary>
        /// <returns></returns>
        private static string GetGuid()
        {
            System.Guid guid = new Guid();
            guid = Guid.NewGuid();
            return guid.ToString();
        }

        public ActionResult BarGraph(string pTkCode )
        {
            ViewBag.date = pTkCode;

            ViewBag.id = "bar_" + GetGuid();
            return PartialView();
        }

        public ActionResult BarGraph_Process(string pTkCode)
        {
            ViewBag.date = pTkCode;

            ViewBag.id = "bar_" + GetGuid();
            return PartialView();
        }

        public ActionResult BarGraph_CNC(string pTkCode)
        {
            ViewBag.date = pTkCode;

            ViewBag.id = "bar_" + GetGuid();
            return PartialView();
        }
        public ActionResult Gauge(string pTkCode)
        {
            ViewBag.mac_code = pTkCode;
            //ViewBag.id = "gauge_" + GetGuid();
            return PartialView();
        }

        //public ActionResult LineGraph(List<string> data_utilization_rate, List<string> data_capacity_efficiency, List<string> data_yield, string u_limit)
        //{
        //    ViewBag.data_utilization_rate = data_utilization_rate;
        //    ViewBag.data_capacity_efficiency = data_capacity_efficiency;
        //    ViewBag.data_yield = data_yield;
        //    ViewBag.u_limit = decimal.ToInt32(decimal.Parse(u_limit));

        //    ViewBag.id = "line_" + GetGuid();
        //    return PartialView();
        //}
        public ActionResult LineGraph(string pTkCode)
        {
            ViewBag.mac_code = pTkCode;
            //ViewBag.id = "line_" + GetGuid();
            return PartialView();
        }


        public ActionResult Block(string pTkCode, int header_fontSize, int section_fontSize )
        {

            ViewBag.mac_code = pTkCode;
            ViewBag.header_fontSize = header_fontSize;
            ViewBag.section_fontSize = section_fontSize;


            return PartialView();
        }
        // Bug: 如果頁面同時沒有PieGraph, MiniPie圖怪怪的
        public ActionResult MiniPie()
        {
            return PartialView();
        }

        public ActionResult PieGraph_MO(string pTkCode)
        {

            ViewData["pTkCode"] = pTkCode;
            return PartialView();
        }

        public ActionResult Pie_test(Dictionary<string, string> pParams)
        {
            foreach(var key in pParams.Keys)
            {
                ViewData[key] = pParams[key];
            }

            return PartialView();
        }

        public ActionResult PieGraph_Yield(string pTkCode)
        {
            ViewBag.pTkCode = pTkCode;
            return PartialView();
        }


        public ActionResult PieGraph_CNC(string pTkCode)
        {
            ViewBag.date = pTkCode;
            return PartialView();
        }

        public ActionResult PieArea()
        {
            return PartialView();
        }

        public ActionResult DonutGraph()
        {
            return PartialView();
        }

        public ActionResult ScatterGraph()
        {
            return PartialView();
        }


        public ActionResult HorizontalBar()
        {
            return PartialView();
        }

        public ActionResult WorldMap()
        {
            return PartialView();
        }

        public ActionResult Pyramid()
        {
            return PartialView();
        }
        
        public ActionResult Sonar()
        {
            return PartialView();
        }


    }
}