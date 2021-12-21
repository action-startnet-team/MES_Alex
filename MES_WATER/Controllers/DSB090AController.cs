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

    public class DSB090AController : JsonNetController
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

        public ActionResult Index2()
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

            public string addr_code { get; set; }
            public string work_code { set; get; }
            public int start { get; set; }
            public int stop { get; set; }
            public int opt { get; set; }

            public string start_time { get; set; }
            public string stop_time { get; set; }
        }

        public class MED15_0000
        {
            public int med15_0000 { get; set; }
            public string mac_code { get; set; }
            public string value_code { set; get; }
            public int value1 { get; set; }
            public string ins_date { get; set; }
            public string ins_time { get; set; }
            public string station_code { get; set; }
            public string addr_code { get; set; }
            public string update_at { get; set; }
        }

        private DSB09_0000 Cal_StationInterval(DataTable pData, string pStationCode, string pAddrCode, string pStartTime)
        {
            DSB09_0000 result = new DSB09_0000();



            return result;
        }

        public JsonResult Get_Station(/*DataTable pData, string pStationCode, string pAddrCode, string pPrevStopTime, string pInitTime)*/)
        {
            // params
            string sMacCode = "mac001";
            string sDate = "2020/05/04";

            DataTable data = Get_MED15_0000(sMacCode, sDate);

            DataRow[] station1_arr = data.Select("value1 = 1 And station_code = 1 And addr_code = 100");

            DataRow s1 = station1_arr[0];

            string start_time = s1["ins_time"].ToString();

            DataRow[] stopTmpDt = null;
            DataRow stopRow = null;
            if (!string.IsNullOrEmpty(start_time))
            {
                stopTmpDt = data.Select("value1 <> 1 And station_code = 1 And addr_code = 100 And ins_time >= #" + start_time + "#");
                if (stopTmpDt.Length > 0)
                {
                    stopRow = stopTmpDt[0];
                }
            }

            string stop_time = stopRow == null ? "" : stop_time = stopRow["ins_time"].ToString();


            DSB09_0000 result = new DSB09_0000();
            result.start_time = start_time;
            result.stop_time = stop_time;
            result.start = Cal_Interval(start_time, start_time);
            result.stop = Cal_Interval(stop_time, start_time);
            result.opt = Cal_Interval(stop_time, start_time);

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        private DSB09_0000 Find_NextStation(DataTable pData, string pStationCode, string pAddrCode, string pPrevStopTime, string pInitTime)
        {
            
            DataRow[] startTmpDt = null;
            DataRow startRow = null;
            if (!string.IsNullOrEmpty(pPrevStopTime))
            {
                startTmpDt = pData.Select("value1 = 1 And station_code = " + pStationCode + " And addr_code = " + pAddrCode + " And ins_time >= #" + pPrevStopTime + "#");
                if (startTmpDt.Length > 0)
                {
                    startRow = startTmpDt[0];
                }
            }

            string start_time = startRow == null ? "" : startRow["ins_time"].ToString();

            DataRow[] stopTmpDt = null;
            DataRow stopRow = null;
            if (!string.IsNullOrEmpty(start_time))
            {
                stopTmpDt = pData.Select("value1 <> 1 And station_code = " + pStationCode + " And addr_code = " + pAddrCode + " And ins_time >= #" + start_time + "#");
                if (stopTmpDt.Length > 0)
                {
                    stopRow = stopTmpDt[0];
                }
            }

            string stop_time = stopRow == null ? "" : stop_time = stopRow["ins_time"].ToString();
         

            DSB09_0000 data = new DSB09_0000();
            data.mac_code = "";
            data.station_code = pStationCode;
            data.addr_code = pAddrCode;
            data.work_code = "";
            data.start_time = start_time;
            data.stop_time = stop_time;
            data.start = Cal_Interval(start_time, pInitTime);
            data.stop = Cal_Interval(stop_time, pInitTime);
            data.opt = Cal_Interval(stop_time, start_time);

           

            // 清除計算過的資料
            if (startRow != null)
            {
                pData.Rows.Remove(startRow);
            }
            if (stopRow != null)
            {
                pData.Rows.Remove(stopRow);
            }

            return data;
        }

        private DataTable Get_MED15_0000(string pMacCode, string pDate) {
            Dictionary<string, object> sqlParams = new Dictionary<string, object>();
            sqlParams.Add("@mac_code", pMacCode);
            sqlParams.Add("@ins_date", pDate);

            string sql = "select * "
                       + " from MED15_0000 "
                       + " where mac_code = @mac_code"
                       + "   and ins_date = @ins_date"
                       + " order by update_at";

            DataTable dt = comm.Get_DataTable(sql, sqlParams);

            return dt;
        }


        public JsonResult test2(string pMacCode, string pDate)
        {
            // params
            //string sMacCode = "mac001";
            //string sDate = "2020/05/04";

            DataTable dt = Get_MED15_0000(pMacCode, pDate);

            //return Json(dt, JsonRequestBehavior.AllowGet);

            List<List<DSB09_0000>> returnList = new List<List<DSB09_0000>>();

            //DataRow[] entries = dt.Select("value1 = 1 and addr_code = 100");

            DataRow[] entries = dt.Select("value1 = 1 And station_code = 1 And addr_code = 100");

            // while(entries.Length > 0)  //這樣寫不行
            // 不能用for，因為Get_Station會變動原本資料
            while (dt.Select("value1 = 1 And station_code = 1 And addr_code = 100").Length > 0)
            {
                List<DSB09_0000> job1 = new List<DSB09_0000>();

                DataRow job1_row = dt.Select("value1 = 1 And station_code = 1 And addr_code = 100")[0];
                string job1_initTime = job1_row["ins_time"].ToString();

                DSB09_0000 WS1 = Find_NextStation(dt, "1", "100", job1_initTime, job1_initTime);

                job1.Add(WS1);

                DSB09_0000 WS2 = Find_NextStation(dt, "2", "200", WS1.stop_time, job1_initTime);

                job1.Add(WS2);

                DSB09_0000 WS3 = Find_NextStation(dt, "3", "300", WS2.stop_time, job1_initTime);

                job1.Add(WS3);

                DSB09_0000 WS4 = Find_NextStation(dt, "4", "400", WS3.stop_time, job1_initTime);

                job1.Add(WS4);

                DSB09_0000 WS5 = Find_NextStation(dt, "5", "500", WS4.stop_time, job1_initTime);

                job1.Add(WS5);

                DSB09_0000 WS6 = Find_NextStation(dt, "6", "600", WS5.stop_time, job1_initTime);

                job1.Add(WS6);

                DSB09_0000 WS7 = Find_NextStation(dt, "7", "700", WS6.stop_time, job1_initTime);

                job1.Add(WS7);

                DSB09_0000 WS8 = Find_NextStation(dt, "8", "800", WS7.stop_time, job1_initTime);

                job1.Add(WS8);


                returnList.Add(job1);
            }


            //List<DSB09_0000> job2 = new List<DSB09_0000>();

            //DataRow job2_row = dt.Select("value1 = 1 And station_code = 1 And addr_code = 100")[0];
            //string job2_initTime = job2_row["ins_time"].ToString();

            //DSB09_0000 WS1_2 = Get_Station(dt, "1", "100", job2_initTime, job2_initTime);

            //job2.Add(WS1_2);

            //DSB09_0000 WS2_2 = Get_Station(dt, "2", "200", WS1_2.stop_time, job2_initTime);

            //job2.Add(WS2_2);

            //DSB09_0000 WS3_2 = Get_Station(dt, "3", "300", WS2_2.stop_time, job2_initTime);

            //job2.Add(WS3_2);

            //DSB09_0000 WS4_2 = Get_Station(dt, "4", "400", WS3_2.stop_time, job2_initTime);

            //job2.Add(WS4_2);

            //returnList.Add(job2);


            return Json(returnList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult test()
        {
            // params
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

            List<List<DSB09_0000>> list = new List<List<DSB09_0000>>();

            DataRow[] entries = dt.Select("value1 = 1");

            //list.Add(Cal_VSMData(dt, entries[0]));
            //list.Add(Cal_VSMData(dt, entries[1]));

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        private List<DSB09_0000> Cal_VSMData(DataTable pDataTable, DataRow pStart)
        {

            DataTable dt = pDataTable;
            string sMacCode = "mac001";

            List<DSB09_0000> data = new List<DSB09_0000>();

            List<string> station_list = new List<string>() { "1", "2", "3", "4" };

            // s1
            //DataRow s1_start = pStart;
            ////DataRow s1_start = dt.Select("value1 = 1 and station_code = 1")[0];

            //string s1_start_time = s1_start["ins_time"].ToString();

            //DataRow s1_stop = dt.Select("value1 = 0 and station_code = 1 and ins_time >= #" + s1_start_time + "#")[0];

            //string s1_stop_time = s1_stop["ins_time"].ToString();

            List<DSB09_0000> returnData = new List<DSB09_0000>();

            string current = pStart["ins_time"].ToString();

            MED15_0000 MED15_0000 = new MED15_0000();

            MED15_0000 = FindCurrentStop(dt, current, "1");
            DSB09_0000 station1 = new DSB09_0000();
            station1.station_code = "1";
            station1.work_code = "Job1";
            station1.start = 1;
            station1.stop = Cal_Interval(current, MED15_0000.ins_time);
            station1.opt = Cal_Interval(current, MED15_0000.ins_time);

            returnData.Add(station1);

            for (int i = 1; i < station_list.Count; i++)
            {
                MED15_0000 = FindNextStart(dt, MED15_0000.ins_time, station_list[i]);
                FindCurrentStop(dt, MED15_0000.ins_time, station_list[i]);
                DSB09_0000 DSB09_0000 = new DSB09_0000();

            }


            string s1_start_time = pStart["ins_time"].ToString();

            string s1_stop_time = FindCurrentStop(dt, s1_start_time, "1").ins_time;


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



            return data;
        }

        private MED15_0000 FindNextStart(DataTable pDt, string pPrevStopTime, string pStationCode)
        {
            MED15_0000 returnData = new MED15_0000();

            DataRow[] tmp = pDt.Select("value1 = 1 and station_code = " + pStationCode + " and ins_time >= #" + pPrevStopTime + "#");

            if (tmp.Length < 0)
            {
                return returnData;
            }

            DataRow start = tmp[0];
            returnData.med15_0000 = comm.sGetInt32(start["med15_0000"].ToString());
            returnData.ins_time = start["ins_time"].ToString();

            return returnData;
        }

        private MED15_0000 FindCurrentStop(DataTable pDt, string pCurrentStartTime, string pStationCode)
        {
            MED15_0000 returnData = new MED15_0000();

            DataRow[] tmp = pDt.Select("value1 = 0 and station_code = " + pStationCode + " and ins_time >= #" + pCurrentStartTime + "#");

            if (tmp.Length < 0)
            {
                return returnData;
            }

            DataRow stop = tmp[0];
            returnData.med15_0000 = comm.sGetInt32(stop["med15_0000"].ToString());
            returnData.ins_time = stop["ins_time"].ToString();

            return returnData;

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
            if (string.IsNullOrEmpty(pDate1) || string.IsNullOrEmpty(pDate2))
            {
                return 0;
            }
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

        [HttpPost]
        public JsonResult Delete_MED15_0000() {
            string sql = "delete med15_0000";
            comm.Connect_DB(sql);

            var returnObj = new {
                success = "true" 
            };
            return Json(returnObj);
        }


    }
}