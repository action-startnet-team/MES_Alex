using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MES_WATER.Models;
using MES_WATER.Repository;
using System.Data;
using System.Linq.Dynamic;
using System.Web.Security;
using System.Reflection;
using Newtonsoft.Json;

namespace MES_WATER.Controllers
{
    [Authorize] //登入驗證
    [HandleError(View = "Error")]  //錯誤導向

    public class DSB090A_v1Controller : JsonNetController
    {
        //程式代號
        public static string pubPrgCode = "DSB090A";
        Comm comm = new Comm();

        public ActionResult Index()
        {
            ViewBag.limit_str = comm.Get_LimitByUsrCode(User.Identity.Name, pubPrgCode);
            ViewBag.prg_code = pubPrgCode;
            return View();
        }

        //public JsonResult Get_DataTable_Data(string pMacCode)
        //{
        //    // sql data
        //    Dictionary<string, object> sqlParams = new Dictionary<string, object>();
        //    sqlParams.Add("@mac_code", pMacCode);

        //    string sql = "select * "
        //               + " from DSB09_0000 "
        //               + " where mac_code = @mac_code"
        //               + "   order by dsb09_0000";
        //    DataTable dt = comm.Get_DataTable(sql, sqlParams);


        //    return Json(dt, JsonRequestBehavior.AllowGet);
        //}

        public class DSB09_0000
        {
            public string mac_code { get; set; }
            public string station_code { get; set; }
            public string work_code { set; get; }
            public int start { get; set; }
            public int stop { get; set; }
            public int opt { get; set; }
        }

        public JsonResult test()
        {
            string sMacCode = "mac001";
            string sDate = "2020/04/24";

            // sql data
            Dictionary<string, object> sqlParams = new Dictionary<string, object>();
            sqlParams.Add("@mac_code", sMacCode);
            sqlParams.Add("@ins_date", sDate);

            string sql = "select * "
                       + " from MED15_0000 "
                       + " where mac_code = @mac_code"
                       + "   and ins_date = @ins_date"
                       + "   and addr_code = 0"
                       + " order by update_at";

            DataTable dt = comm.Get_DataTable(sql, sqlParams);

            // DataTable filter example
            //DataTable entries = dt.Select("addr_code = 1 and value1 = 1 and ins_time > #12:15:21#").CopyToDataTable();

            List<DSB09_0000> data = new List<DSB09_0000>();

            DataRow[] entries = dt.Select("value1 = 1");

            // job 1
            // s1
            DataRow s1_start = dt.Select("value1 = 1 and station_code = 1")[0];

            string s1_start_time = s1_start["ins_time"].ToString();

            DataRow s1_stop = dt.Select("value1 = 0 and station_code = 1 and ins_time >= #" + s1_start_time + "#")[0];

            string s1_stop_time = s1_stop["ins_time"].ToString();

            DSB09_0000 WS1 = new DSB09_0000();
            WS1.mac_code = sMacCode;
            WS1.station_code = "1";
            WS1.work_code = "Job1";
            WS1.start = 1;
            WS1.stop = Cal_Interval(s1_start_time, s1_stop_time);
            WS1.opt = Cal_Interval(s1_start_time, s1_stop_time);

            data.Add(WS1);

            // s2
            DataRow s2_start = dt.Select("value1 = 1 and station_code = 2 and ins_time >= #" + s1_stop_time + "#")[0];

            string s2_start_time = s2_start["ins_time"].ToString();

            DataRow s2_stop = dt.Select("value1 = 0 and station_code = 2 and ins_time >= #" + s2_start_time + "#")[0];

            string s2_stop_time = s2_stop["ins_time"].ToString();

            DSB09_0000 WS2 = new DSB09_0000();
            WS2.mac_code = sMacCode;
            WS2.station_code = "2";
            WS2.work_code = "Job1";
            WS2.start = Cal_Interval(s2_start_time, s1_start_time);
            WS2.stop = Cal_Interval(s2_stop_time, s1_start_time);
            WS2.opt = Cal_Interval(s2_stop_time, s2_start_time);

            data.Add(WS2);

            // s3
            DataRow s3_start = dt.Select("value1 = 1 and station_code = 3 and ins_time >= #" + s2_stop_time + "#")[0];

            string s3_start_time = s3_start["ins_time"].ToString();

            DataRow s3_stop = dt.Select("value1 = 0 and station_code = 3 and ins_time >= #" + s3_start_time + "#")[0];

            string s3_stop_time = s3_stop["ins_time"].ToString();

            DSB09_0000 WS3 = new DSB09_0000();
            WS3.mac_code = sMacCode;
            WS3.station_code = "3";
            WS3.work_code = "Job1";
            WS3.start = Cal_Interval(s3_start_time, s1_start_time);
            WS3.stop = Cal_Interval(s3_stop_time, s1_start_time);
            WS3.opt = Cal_Interval(s3_start_time, s3_stop_time);

            data.Add(WS3);

            // s4
            DataRow s4_start = dt.Select("value1 = 1 and station_code = 4 and ins_time >= #" + s3_stop_time + "#")[0];

            string s4_start_time = s4_start["ins_time"].ToString();

            DataRow s4_stop = dt.Select("value1 = 0 and station_code = 4 and ins_time >= #" + s4_start_time + "#")[0];

            string s4_stop_time = s4_stop["ins_time"].ToString();

            DSB09_0000 WS4 = new DSB09_0000();
            WS4.mac_code = sMacCode;
            WS4.station_code = "4";
            WS4.work_code = "Job1";
            WS4.start = Cal_Interval(s4_start_time, s1_start_time);
            WS4.stop = Cal_Interval(s4_stop_time, s1_start_time);
            WS4.opt = Cal_Interval(s4_start_time, s4_stop_time);

            data.Add(WS4);


            List<string> time_line = new List<string>();

            time_line.Add(s1_start_time);
            time_line.Add(s1_stop_time);

            time_line.Add(s2_start_time);
            time_line.Add(s2_stop_time);

            time_line.Add(s3_start_time);
            time_line.Add(s3_stop_time);

            time_line.Add(s4_start_time);
            time_line.Add(s4_stop_time);

            List<int> work_time = new List<int>();

            int work_time1 = Cal_Interval(s1_start_time, s1_stop_time);
            int work_time2 = Cal_Interval(s2_start_time, s2_stop_time);
            int work_time3 = Cal_Interval(s3_start_time, s3_stop_time);
            int work_time4 = Cal_Interval(s4_start_time, s4_stop_time);
            work_time.Add(work_time1);
            work_time.Add(work_time2);
            work_time.Add(work_time3);
            work_time.Add(work_time4);

            List<int> break_time = new List<int>();

            int break_time1 = Cal_Interval(s2_start_time, s1_stop_time);
            int break_time2 = Cal_Interval(s3_start_time, s2_stop_time);
            int break_time3 = Cal_Interval(s4_start_time, s3_stop_time);

            break_time.Add(break_time1);
            break_time.Add(break_time2);
            break_time.Add(break_time3);

            var returnObj = new
            {
                time_line = time_line,
                work_time = work_time,
                break_time = break_time,
                data = data
            };

            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }

        private List<DSB09_0000> Cal_Data()
        {
            string sMacCode = "mac001";
            string sDate = "2020/04/24";

            // sql data
            Dictionary<string, object> sqlParams = new Dictionary<string, object>();
            sqlParams.Add("@mac_code", sMacCode);
            sqlParams.Add("@ins_date", sDate);

            string sql = "select * "
                       + " from MED15_0000 "
                       + " where mac_code = @mac_code"
                       + "   and ins_date = @ins_date"
                       + "   and addr_code = 0"
                       + " order by update_at";

            DataTable dt = comm.Get_DataTable(sql, sqlParams);

            // DataTable filter example
            //DataTable entries = dt.Select("addr_code = 1 and value1 = 1 and ins_time > #12:15:21#").CopyToDataTable();

            List<DSB09_0000> data = new List<DSB09_0000>();

            DataRow[] entries = dt.Select("value1 = 1");

            // job 1
            // s1
            DataRow s1_start = dt.Select("value1 = 1 and station_code = 1")[0];

            string s1_start_time = s1_start["ins_time"].ToString();

            DataRow s1_stop = dt.Select("value1 = 0 and station_code = 1 and ins_time >= #" + s1_start_time + "#")[0];

            string s1_stop_time = s1_stop["ins_time"].ToString();

            DSB09_0000 WS1 = new DSB09_0000();
            WS1.mac_code = sMacCode;
            WS1.station_code = "1";
            WS1.work_code = "Job1";
            WS1.start = 0;
            WS1.stop = Cal_Interval(s1_start_time, s1_stop_time);
            WS1.opt = Cal_Interval(s1_start_time, s1_stop_time);

            data.Add(WS1);

            // s2
            DataRow s2_start = dt.Select("value1 = 1 and station_code = 2 and ins_time >= #" + s1_stop_time + "#")[0];

            string s2_start_time = s2_start["ins_time"].ToString();

            DataRow s2_stop = dt.Select("value1 = 0 and station_code = 2 and ins_time >= #" + s2_start_time + "#")[0];

            string s2_stop_time = s2_stop["ins_time"].ToString();

            DSB09_0000 WS2 = new DSB09_0000();
            WS2.mac_code = sMacCode;
            WS2.station_code = "2";
            WS2.work_code = "Job1";
            WS2.start = Cal_Interval(s2_start_time, s1_start_time);
            WS2.stop = Cal_Interval(s2_stop_time, s1_start_time);
            WS2.opt = Cal_Interval(s2_stop_time, s2_start_time);

            data.Add(WS2);

            // s3
            DataRow s3_start = dt.Select("value1 = 1 and station_code = 3 and ins_time >= #" + s2_stop_time + "#")[0];

            string s3_start_time = s3_start["ins_time"].ToString();

            DataRow s3_stop = dt.Select("value1 = 0 and station_code = 3 and ins_time >= #" + s3_start_time + "#")[0];

            string s3_stop_time = s3_stop["ins_time"].ToString();

            DSB09_0000 WS3 = new DSB09_0000();
            WS3.mac_code = sMacCode;
            WS3.station_code = "3";
            WS3.work_code = "Job1";
            WS3.start = Cal_Interval(s3_start_time, s1_start_time);
            WS3.stop = Cal_Interval(s3_stop_time, s1_start_time);
            WS3.opt = Cal_Interval(s3_start_time, s3_stop_time);

            data.Add(WS3);

            // s4
            DataRow s4_start = dt.Select("value1 = 1 and station_code = 4 and ins_time >= #" + s3_stop_time + "#")[0];

            string s4_start_time = s4_start["ins_time"].ToString();

            DataRow s4_stop = dt.Select("value1 = 0 and station_code = 4 and ins_time >= #" + s4_start_time + "#")[0];

            string s4_stop_time = s4_stop["ins_time"].ToString();

            DSB09_0000 WS4 = new DSB09_0000();
            WS4.mac_code = sMacCode;
            WS4.station_code = "4";
            WS4.work_code = "Job1";
            WS4.start = Cal_Interval(s4_start_time, s1_start_time);
            WS4.stop = Cal_Interval(s4_stop_time, s1_start_time);
            WS4.opt = Cal_Interval(s4_start_time, s4_stop_time);

            data.Add(WS4);

            return data;

        }

        private int Cal_Interval(string pDate1, string pDate2)
        {
            DateTime data1 = DateTime.Parse(pDate1);
            DateTime data2 = DateTime.Parse(pDate2);
            int result = (int)(data1 - data2).TotalSeconds;

            result = result < 0 ? -result : result;

            return result;

        }

        public JsonResult Get_PlanVSM_Data(string pMacCode)
        {

            //DataTable dt = Get_VSMData(sTkCode);


            //Dictionary<string, object> sSqlParams = new Dictionary<string, object>();
            //sSqlParams.Add("@mac_code", pMacCode);

            //string sSql = "select * from DSB09_0000 where mac_code = @mac_code";

            //DataTable dt = comm.Get_DataTable(sSql, sSqlParams);

            //DataRow WS1 = dt.Rows[0];
            //DataRow WS2 = dt.Rows[1];
            //DataRow WS3 = dt.Rows[2];
            //DataRow WS4 = dt.Rows[3];

            //int work_time1 = comm.sGetInt32(WS1["opt"].ToString());
            //int work_time2 = comm.sGetInt32(WS2["opt"].ToString());
            //int work_time3 = comm.sGetInt32(WS3["opt"].ToString());
            //int work_time4 = comm.sGetInt32(WS4["opt"].ToString());

            //int break_time1 = comm.sGetInt32(WS2["start"].ToString()) - comm.sGetInt32(WS1["stop"].ToString());
            //int break_time2 = comm.sGetInt32(WS3["start"].ToString()) - comm.sGetInt32(WS2["stop"].ToString());
            //int break_time3 = comm.sGetInt32(WS4["start"].ToString()) - comm.sGetInt32(WS3["stop"].ToString());


            List<DSB09_0000> data = Cal_Data();

            List<int> returnList = new List<int>();

            int work_time = 0;
            int break_time = 0;

            // 第一站 工作時間
            for (int i = 0; i < data[0].opt; i++)
            {
                returnList.Add(1);
            }

            // 其他站 休息時間 + 工作時間
            for (int i = 1; i < data.Count; i++)
            {
                // 目前站開始 - 上一站結束
                break_time = data[i].start - data[i - 1].stop;
                for (int j = 0; j < break_time; j++)
                {
                    returnList.Add(0);
                }

                // 假設休息時間已經算好記錄在欄位，或許可以用算的
                work_time = data[i].opt;
                for (int j = 0; j < work_time; j++)
                {
                    returnList.Add(1);
                }
            }


            return Json(returnList, JsonRequestBehavior.AllowGet);
        }



    }
}