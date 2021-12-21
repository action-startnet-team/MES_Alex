using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MES_WATER.Models;
using MES_WATER.Repository;
using System.Data;

namespace MES_WATER.Controllers
{
    public class DSB080AController : JsonNetController
    {
        Comm comm = new Comm();
        Random rand = new Random();

        // GET: DSB080A
        public ActionResult Index()
        {
            int std_time = comm.sGetInt32(comm.Get_QueryData("MEB20_0100", "test", "pro_code", "std_time"));

            ViewBag.std_time = std_time;
            return View();
        }

        public PartialViewResult VSM(Dictionary<string, object> pParams)
        {

            // TryGetValue有轉型問題
            //object id = comm.Get_Guid();
            //object sWidth = 300;
            //object sHeight = 65;

            //pParams.TryGetValue("id", out id);
            //pParams.TryGetValue("width", out sWidth);
            //pParams.TryGetValue("height", out sHeight);

            string id = pParams.Keys.Contains("id") ? pParams["id"].ToString() : comm.Get_Guid();
            string sWidth = pParams.Keys.Contains("width") ? pParams["width"].ToString() : "300";
            string sHeight = pParams.Keys.Contains("height") ? pParams["height"].ToString() : "65";

            //
            string sAjaxUrl = pParams.Keys.Contains("ajaxUrl") ? pParams["ajaxUrl"].ToString() : "/DSB080A/Get_VSMData";
            string sAjaxData = pParams.Keys.Contains("ajaxData") ? pParams["ajaxData"].ToString() : "";


            ViewBag.id = id;
            ViewBag.sWidth = sWidth;
            ViewBag.sHeight = sHeight;
            ViewBag.sAjaxUrl = sAjaxUrl;

            return PartialView();
        }

        public JsonResult Get_VSMData_test(string pProCode)
        {

            //DataTable dt = Get_VSMData(sTkCode);

            List<int> list = new List<int>();

            Dictionary<string, object> sSqlParams = new Dictionary<string, object>();
            sSqlParams.Add("@pro_code", pProCode);
            string sSql = "select * from MEB20_0100 where pro_code = @pro_code";
            DataTable dt = comm.Get_DataTable(sSql, sSqlParams);
            
            // 間隔1秒
            int j = 0;
            foreach (DataRow dr in dt.Rows)
            {
                int time = comm.sGetInt32(dr["std_time"].ToString());
                if (time > 0)
                {
                    for (int i = 1; i <= time; i++)
                    {
                        list.Add(1);
                    }
                    j++;
                    // 製程間隔
                    if (j != dt.Rows.Count)
                    {
                        for (int i = 1; i <= 1 * 60; i++)
                        {
                            list.Add(0);
                        }
                    }
                }
                
            }

            

            

            //// 15分鐘
            //for(int i = 1; i <= 15*60; i++)
            //{
            //    list.Add(1);
            //}

            //// 1分鐘
            //for (int i = 1; i <= 1*60; i++)
            //{
            //    list.Add(0);
            //}

            //// 4分鐘
            //for (int i = 1; i <= 4*60; i++)
            //{
            //    list.Add(1);
            //}


            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Get_VSMData(Dictionary<string, string> pParams)
        {
            string sTkCode = pParams["TkCode"];

            DataTable dt = Get_VSMData(sTkCode);


            return Json(dt, JsonRequestBehavior.AllowGet);
        }



        public JsonResult Get_Status(string pTkCode="", string pDate="")
        {
            int result = 0;

            Dictionary<string, object> sSqlParams = new Dictionary<string, object>();

            sSqlParams.Add("@mac_code", pTkCode);
            sSqlParams.Add("@ins_date", pDate);

            string sSql = "select * "
                        + " from MED15_0000 "
                        + " where mac_code = @mac_code "
                        + "   and ins_date = @ins_date "
                        + "   and value1 = 1 "
                        + " order by update_at desc";

            DataTable dt = comm.Get_DataTable(sSql, sSqlParams);

            if (dt.Rows.Count > 0)
            {
                // addr_code: 0: start / 1: stop / 2: eng
                string addr_code = dt.Rows[0]["addr_code"].ToString();
                if (addr_code == "0")
                {
                    result = 1;
                }

            }

            // 抓MED08_0000資料
            //string sSql = @"select top 1 * from MED08_0000 where mo_code = @mo_code and date_s = @date_s order by med08_0000 desc";


            //DataTable dt = comm.Get_DataTable(sSql, sSqlParams);
            //if (dt.Rows.Count > 0)
            //{
            //    string date_s = dt.Rows[0]["date_s"].ToString();
            //    string time_s = dt.Rows[0]["time_s"].ToString();
            //    string date_e = dt.Rows[0]["date_e"].ToString();
            //    string time_e = dt.Rows[0]["time_e"].ToString();

            //    // 有開始日期 但 沒有結束日期 
            //    if (!string.IsNullOrEmpty(date_s) && !string.IsNullOrEmpty(time_s) && string.IsNullOrEmpty(date_e) && string.IsNullOrEmpty(time_e))
            //    {
            //        result = 1;
            //    }

            //    // 有開始日期 和 結束日期 
            //    if (!string.IsNullOrEmpty(date_s) && !string.IsNullOrEmpty(time_s) && !string.IsNullOrEmpty(date_e) && !string.IsNullOrEmpty(time_e))
            //    {
            //        result = 0;
            //    }

            //    // 其他 (預設 0)

            //}

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private DataTable Get_VSMData(string pTkCode)
        {

            string sSql = @"select * , date_s as cal_date, time_s as cal_time, '1' as status
from MED08_0000
where date_s != '' and time_s != ''
  and date_e = '' and time_e = ''
union
select *, date_e as cal_date, time_e as cal_time, '0' as status
from MED08_0000
where (date_s = '' and time_s = ''
  and date_e = '' and time_e = '') or (date_s != '' and time_s != '' and date_e != '' and time_e != '')
order by med08_0000

";

            DataTable dt = comm.Get_DataTable(sSql);

            return dt;

        }


    }
}