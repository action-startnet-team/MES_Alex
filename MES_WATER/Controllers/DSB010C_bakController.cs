using MES_WATER.Models;
using MES_WATER.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MES_WATER.Controllers
{
    [Authorize] //登入驗證
    //[HandleError(View = "Error")]  //錯誤導向
    public class DSB010C_bakController : Controller
    {
        Comm comm = new Comm();

        public ActionResult Index_old()
        {
            string mac_code = "MA001";

            ViewBag.mac_code = mac_code;
            ViewBag.mac_name = comm.Get_QueryData("MEB01_0000", mac_code, "mac_code", "mac_name");

            return View();
        }

        public ActionResult Index2()
        {

            return View();
        }

        public ActionResult Index3()
        {

            return View();
        }

        public ActionResult Index()
        {
            //設定signalR Hub來源
            ViewBag.signalr_url = comm.Get_QueryData("BDP00_0000", "signalr_url", "par_name", "par_value");
            return View();
        }


        public ActionResult Index_bak()
        {
            //設定signalR Hub來源
            ViewBag.signalr_url = comm.Get_QueryData("BDP00_0000", "signalr_url", "par_name", "par_value");
            return View();
        }

        public ActionResult Index_bak2()
        {
            //設定signalR Hub來源
            ViewBag.signalr_url = comm.Get_QueryData("BDP00_0000", "signalr_url", "par_name", "par_value");
            return View();
        }




        public ActionResult Get_DSB01_0000(string mac_code)
        {
            string today = DateTime.Now.ToString("yyyy/MM/dd");
            today = "2019/11/19";

            DSB01_0000 dsb01_0000 = new DSB01_0000();
            string sSql = "select * from DSB01_0000 where mac_code = @mac_code  " +
                                            "and format(cal_date, 'yyyy/MM/dd') = @cal_date";
            DataTable dtTmp = comm.Get_DataTable(sSql, "mac_code,cal_date", mac_code + "," + today);
            if (dtTmp.Rows.Count > 0)
            {
                dsb01_0000.utilization_rate = decimal.Parse(dtTmp.Rows[0]["utilization_rate"].ToString());
                dsb01_0000.capacity_efficiency = decimal.Parse(dtTmp.Rows[0]["capacity_efficiency"].ToString());
                dsb01_0000.yield = decimal.Parse(dtTmp.Rows[0]["yield"].ToString());
                dsb01_0000.ng_qty = decimal.Parse(dtTmp.Rows[0]["ng_qty"].ToString());
                dsb01_0000.pro_qty = decimal.Parse(dtTmp.Rows[0]["pro_qty"].ToString());

            }
            decimal OEE_tmp = decimal.Multiply(dsb01_0000.utilization_rate, dsb01_0000.capacity_efficiency);
            decimal OEE_tmp2 = decimal.Multiply(OEE_tmp, dsb01_0000.yield);
            decimal OEE = Math.Round(decimal.Multiply(OEE_tmp2, (decimal)0.0001), 2);

            var data = new
            {
                utilization_rate = dsb01_0000.utilization_rate,
                capacity_efficiency = dsb01_0000.capacity_efficiency,
                yield = dsb01_0000.yield,
                OEE = OEE,
                all_qty = dsb01_0000.pro_qty + dsb01_0000.ng_qty
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        // 1013
        public JsonResult Get_Chart3Data(string pTkCode)
        {
            string mac_code = pTkCode;
            string date = DateTime.Now.ToString("yyyy/MM/dd");
            date = "2019/11/19";

            DataTable D1013 = Get_DSP01_0000(mac_code, "D1013", date, 120);
            Dictionary<string, object> D1013_data = new Dictionary<string, object>();
            for (int i = 0; i < D1013.Rows.Count; i++)
            {
                D1013_data.Add(DateTime.Parse(D1013.Rows[i]["ins_time"].ToString()).ToString("HH:mm:ss"), D1013.Rows[i]["para_value"]);
            }

            List<Dictionary<string, object>> datas = new List<Dictionary<string, object>> { };
            datas.Add(D1013_data);
            //var json = new {
            //    datas = datas
            //};
            return Json(datas, JsonRequestBehavior.AllowGet);
        }

        // 1007 ~ 1009
        public JsonResult Get_Chart2Data(string pTkCode)
        {
            string mac_code = pTkCode;
            string date = DateTime.Now.ToString("yyyy/MM/dd");
            date = "2019/11/19";

            DataTable D1007 = Get_DSP01_0000(mac_code, "D1007", date, 120);

            Dictionary<string, object> D1007_data = new Dictionary<string, object>();
            for (int i = 0; i < D1007.Rows.Count; i++)
            {
                D1007_data.Add(DateTime.Parse(D1007.Rows[i]["ins_time"].ToString()).ToString("HH:mm:ss"), D1007.Rows[i]["para_value"]);
            }

            DataTable D1008 = Get_DSP01_0000(mac_code, "D1008", date, 120);
            Dictionary<string, object> D1008_data = new Dictionary<string, object>();
            for (int i = 0; i < D1007.Rows.Count; i++)
            {
                D1008_data.Add(DateTime.Parse(D1008.Rows[i]["ins_time"].ToString()).ToString("HH:mm:ss"), D1008.Rows[i]["para_value"]);
            }

            DataTable D1009 = Get_DSP01_0000(mac_code, "D1009", date, 120);
            Dictionary<string, object> D1009_data = new Dictionary<string, object>();
            for (int i = 0; i < D1009.Rows.Count; i++)
            {
                D1009_data.Add(DateTime.Parse(D1009.Rows[i]["ins_time"].ToString()).ToString("HH:mm:ss"), D1009.Rows[i]["para_value"]);
            }


            List<Dictionary<string, object>> datas = new List<Dictionary<string, object>> { };
            datas.Add(D1007_data);
            datas.Add(D1008_data);
            datas.Add(D1009_data);
            //var json = new {
            //    datas = datas
            //};
            return Json(datas, JsonRequestBehavior.AllowGet);
        }

        // 1010 ~ 1012
        public JsonResult Get_ChartData(string pTkCode)
        {
            string mac_code = pTkCode;
            string date = DateTime.Now.ToString("yyyy/MM/dd");
            date = "2019/11/19";

            DataTable D1010 = Get_DSP01_0000(mac_code, "D1010", date, 120);

            Dictionary<string, object> D1010_data = new Dictionary<string, object>();
            for (int i = 0; i < D1010.Rows.Count; i++)
            {
                D1010_data.Add(DateTime.Parse(D1010.Rows[i]["ins_time"].ToString()).ToString("HH:mm:ss"), D1010.Rows[i]["para_value"]);
            }

            DataTable D1011 = Get_DSP01_0000(mac_code, "D1011", date, 120);
            Dictionary<string, object> D1011_data = new Dictionary<string, object>();
            for (int i = 0; i < D1011.Rows.Count; i++)
            {
                D1011_data.Add(DateTime.Parse(D1011.Rows[i]["ins_time"].ToString()).ToString("HH:mm:ss"), D1011.Rows[i]["para_value"]);
            }

            DataTable D1012 = Get_DSP01_0000(mac_code, "D1012", date, 120);
            Dictionary<string, object> D1012_data = new Dictionary<string, object>();
            for (int i = 0; i < D1012.Rows.Count; i++)
            {
                D1012_data.Add(DateTime.Parse(D1012.Rows[i]["ins_time"].ToString()).ToString("HH:mm:ss"), D1012.Rows[i]["para_value"]);
            }

            List<Dictionary<string, object>> datas = new List<Dictionary<string, object>> { };
            datas.Add(D1010_data);
            datas.Add(D1011_data);
            datas.Add(D1012_data);
            //var json = new {
            //    datas = datas
            //};
            return Json(datas, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取得日期區間內的參數資料
        /// </summary>
        /// <param name="mac_code"></param>
        /// <param name="para_code"></param>
        /// <param name="pDate">格式: yyyy/MM/dd</param>
        /// <param name="pInterval"></param>
        /// <returns></returns>
        //private Dictionary<string, object> Get_ParaData(string mac_code, string para_code, string pDate, int pInterval)
        //{
        //    //string sDate_end = DateTime.Parse(pDate).AddDays(1).ToString("yyyy/MM/dd");

        //    //string sSql = " select * from (select distinct top " + pInterval + " ins_time, mac_code, para_code, para_value  from DSP01_0000 " +
        //    //              "   where mac_code = @mac_code " +
        //    //              "       and para_code = @para_code" +
        //    //              "       and ins_time < @ins_time" +
        //    //              "       order by ins_time desc ) sq " +
        //    //              "   order by ins_time asc ";
        //    string sSql = " select distinct top " + pInterval + " ins_time, mac_code, para_code, para_value  from DSP01_0000 " +
        //                  "   where mac_code = @mac_code " +
        //                  "       and para_code = @para_code" +
        //                  "       and format(ins_time, 'yyyy/MM/dd') = @ins_time" +
        //                  "       order by ins_time ";

        //    string ParaName = "mac_code,para_code,ins_time";
        //    string ParaValue = mac_code + "," + para_code + "," + pDate;

        //    DataTable dtTmp = comm.Get_DataTable(sSql, ParaName, ParaValue);

        //    Dictionary<string, object> data = new Dictionary<string, object>();
        //    for (int i = 0; i < dtTmp.Rows.Count; i++)
        //    {
        //        data.Add(DateTime.Parse(dtTmp.Rows[i]["ins_time"].ToString()).ToString("HH:mm:ss"), dtTmp.Rows[i]["para_value"]);
        //    }
        //    return data;
        //}

        /// <summary>
        /// 取得一天的N筆參數資料
        /// </summary>
        /// <param name="mac_code"></param>
        /// <param name="para_code"></param>
        /// <param name="pdate">日期別</param>
        /// <param name="pInterval">現在這個時間往前取幾筆資料</param>
        /// <returns></returns>
        private DataTable Get_DSP01_0000(string mac_code, string para_code, string pdate, int pInterval)
        {
            string sSql = "";
            DataTable dtTmp = new DataTable();
            //string sDate_end = DateTime.Parse(pdate).AddDays(1).ToString("yyyy/MM/dd");
            //需要判斷日期 
            sSql = " select * from (select distinct top " + pInterval + " ins_time, mac_code, para_code, para_value  from DSP01_0000 " +
                   "   where mac_code = @mac_code " +
                   "       and para_code = @para_code" +
                   "       and format(ins_time, 'yyyy/MM/dd') = @ins_time" +
                   "       order by ins_time desc ) sq " +
                   "   order by ins_time asc ";

            string ParaName = "mac_code,para_code,ins_time";
            string ParaValue = mac_code + "," + para_code + "," + pdate;

            dtTmp = comm.Get_DataTable(sSql, ParaName, ParaValue);

            return dtTmp;
        }


        /// <summary>
        /// 取得當天單一機器目前的異常狀態
        /// </summary>
        /// <param name="mac_code">機器代碼</param>
        /// <param name="para_code">記憶體位置</param>
        /// <returns>0.無異常/1.有異常</returns>
        public string Get_ErrorState(string mac_code, string para_code)
        {
            string sSql = "";
            DataTable dtTmp = new DataTable();
            string today_date = DateTime.Now.ToString("yyyy/MM/dd");
            today_date = "2019/11/19";

            sSql = " select top 1 *  from DSP01_0000 " +
                   "  where mac_code = @mac_code " +
                   "    and para_code = @para_code" +
                   "    and format(ins_time, 'yyyy/MM/dd') = @ins_time" +
                   "  order by ins_time desc ";
            string ParaName = "mac_code,para_code,ins_time";
            string ParaValue = mac_code + "," + para_code + "," + today_date;

            dtTmp = comm.Get_DataTable(sSql, ParaName, ParaValue);
            if (dtTmp.Rows.Count > 0)
            {
                return dtTmp.Rows[0]["para_value"].ToString();
            }
            else
            {
                return "0";
            }
        }

        //取得異常記憶體
        public string Get_ExtremeMemory()
        {
            string sSql = "select * from DSP00_0000" +
                          " where para_code in('M1000','M1001','M1002','M1003','M1004','M1007','M1008','M1009','M1010','M1011','M1012','M1017')";
            return comm.DataFieldToStr(sSql, "para_code");
        }

        /// <summary>
        /// 取得當天燈號狀態
        /// </summary>
        /// <param name="pTkCode"></param>
        /// <returns></returns>
        public string Get_LightState(string pTkCode)
        {
            string mac_code = pTkCode;
            string today_date = DateTime.Now.ToString("yyyy/MM/dd");
            today_date = "2019/11/19";

            string sSql = " select top 1 *  from DSP01_0000 " +
                   "  where mac_code = @mac_code " +
                   "    and para_code in ('M1013', 'M1014', 'M1015') " +
                   "    and para_value = '1' " +
                   "    and format(ins_time, 'yyyy/MM/dd') = @ins_time" +
                   "  order by ins_time desc ";
            DataTable dtTmp = comm.Get_DataTable(sSql, "mac_code,ins_time", mac_code + "," + today_date);

            if (dtTmp.Rows.Count > 0)
            {
                string para_code = dtTmp.Rows[0]["para_code"].ToString();
                switch (para_code)
                {
                    //燈號公式變更
                    case "M1013":
                        return "green";
                    case "M1014":
                        return "yellow";
                    case "M1015":
                        return "red";
                }
            }
            return "";
        }

        /// <summary>
        /// 取得當天數量
        /// </summary>
        /// <param name="pTkCode"></param>
        /// <returns></returns>
        //public int Get_TodayCount(string pTkCode)
        //{
        //    string mac_code = pTkCode;
        //    string date = DateTime.Now.ToString("yyyy/MM/dd");
        //    date = "2019/11/19";

        //    int count = comm.Get_ParaCount(mac_code, "M1016", "1", date);

        //    return count;
        //}

    }
}