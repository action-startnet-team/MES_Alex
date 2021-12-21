using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MES_WATER.Models;
using MES_WATER.Repository;
using Newtonsoft.Json;
using System.Data;

namespace MES_WATER.Controllers
{
    public class MESController : JsonNetController
    {
        //
        Comm comm = new Comm();


        /// <summary>
        /// 取得生產看板的資料
        /// </summary>
        /// <param name="pDateString"></param>
        /// <param name="pLineCode"></param>
        /// <returns></returns>
        public JsonResult Get_MoDataByLine(string pDateString, string pLineCode)
        {
            string sSql = "select mo_code, MET01_0000.pro_code, plan_qty, mo_status, pro_name "
                        + ",ISNULL((SELECT TOP 1 ok_qty FROM MEM01_0000 WHERE mo_code=MET01_0000.mo_code AND work_code IN ('L1SMB01','L2SMB01')),0) AS ok_qty"
                        + ",ISNULL((SELECT TOP 1 ng_qty FROM MEM01_0000 WHERE mo_code = MET01_0000.mo_code AND work_code IN('L1SMB01', 'L2SMB01')),0) AS ng_qty"
                        + ",ISNULL((SELECT TOP 1 ok_qty FROM MEM01_0000 WHERE mo_code = MET01_0000.mo_code AND work_code IN('L1SMB03', 'L2SMB03')),0) AS ok_qty2"
                        + ",ISNULL((SELECT TOP 1 ng_qty FROM MEM01_0000 WHERE mo_code = MET01_0000.mo_code AND work_code IN('L1SMB03', 'L2SMB03')),0) AS ng_qty2"
                        + ",ISNULL((SELECT TOP 1 ok_qty FROM MEM01_0000 WHERE mo_code = MET01_0000.mo_code AND work_code IN('L1SMB04', 'L2SMB04')),0) AS ok_qty3"
                        + ",ISNULL((SELECT TOP 1 ng_qty FROM MEM01_0000 WHERE mo_code = MET01_0000.mo_code AND work_code IN('L1SMB04', 'L2SMB04')),0) AS ng_qty3"
                        + ",ISNULL((SELECT TOP 1 ok_qty FROM MEM01_0000 WHERE mo_code = MET01_0000.mo_code AND work_code IN('L1SMB05', 'L2SMB05')),0) AS ok_qty4"
                        + ",ISNULL((SELECT TOP 1 ng_qty FROM MEM01_0000 WHERE mo_code = MET01_0000.mo_code AND work_code IN('L1SMB05', 'L2SMB05')),0) AS ng_qty4"
                        + ",(SELECT TOP 1 chkup_status FROM WMT07_0000 WHERE mo_code = MET01_0000.mo_code AND work_code IN('L1SMB01', 'L2SMB01')) AS sto_status"
                        + " from MET01_0000 "
                        + " left join MEB20_0000 on MEB20_0000.pro_code = MET01_0000.pro_code "
                        + " where 1=1 "
                        + "   and sch_date_s = @sch_date_s "
                        + "   and plan_line_code like @plan_line_code "
                        + "   and mo_status in (20, 30) "
                        + " order by sch_time_s";

            DataTable dtTmp = comm.Get_DataTable(sSql, "sch_date_s,plan_line_code", pDateString + "," + pLineCode);

            return Json(dtTmp, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取得投料指示
        /// </summary>
        /// <param name="pLineCode"></param>
        /// <returns></returns>
        public JsonResult Get_FeedHint(string pLineCode)
        {
            DataTable dtData = new DataTable();
            dtData.Columns.Add("pro_code");
            dtData.Columns.Add("pro_name");
            dtData.Columns.Add("pro_qty_sum");
            dtData.Columns.Add("res_qty_sum");
            dtData.Columns.Add("use_qty_sum");
            dtData.Columns.Add("loc_name");

            //取得投料列表
            string sSql = "SELECT a.pro_code,b.pro_name,CEILING(a.pro_qty) AS pro_qty" +
                ",ISNULL((SELECT TOP 1 (ok_qty+ng_qty) FROM MEM01_0000 WHERE mo_code=a.mo_code AND work_code IN ('L1SMB01','L2SMB01') ORDER BY ins_date DESC,ins_time DESC),0) AS use_qty" +
                ",(SELECT TOP 1 y.loc_name FROM MET03_0100 x LEFT JOIN WMB02_0000 y ON x.loc_code = y.loc_code WHERE wrk_code = a.wrk_code AND pro_code = a.pro_code) AS loc_name" +
                ",CEILING(a.res_qty) AS res_qty FROM WMT07_0000 a" +
                " LEFT JOIN MEB20_0000 b ON a.pro_code = b.pro_code" +
                " WHERE work_code=@work_code" +
                " AND prepare_code IN (SELECT prepare_code FROM WMT06_0000 WHERE prepare_date=@prepare_date)" +
                " ORDER BY scr_no";
            DataTable dtTmp = comm.Get_DataTable(sSql, "work_code,prepare_date", pLineCode + "," + DateTime.Now.ToString("yyyy/MM/dd"));
            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                string pro_code = dtTmp.Rows[i]["pro_code"].ToString();
                string pro_name = dtTmp.Rows[i]["pro_name"].ToString();
                int pro_qty = int.Parse(dtTmp.Rows[i]["pro_qty"].ToString());
                int res_qty = int.Parse(dtTmp.Rows[i]["res_qty"].ToString());
                double use_qty = double.Parse(dtTmp.Rows[i]["use_qty"].ToString());
                string loc_name = dtTmp.Rows[i]["loc_name"].ToString();

                bool bNewData = true;
                for (int j = 0; j < dtData.Rows.Count; j++)
                {
                    if (dtData.Rows[j]["pro_code"].ToString() == pro_code)
                    {
                        bNewData = false;
                        dtData.Rows[j]["pro_qty_sum"] = int.Parse(dtData.Rows[j]["pro_qty_sum"].ToString()) + pro_qty;
                        dtData.Rows[j]["res_qty_sum"] = int.Parse(dtData.Rows[j]["res_qty_sum"].ToString()) + res_qty;
                        dtData.Rows[j]["use_qty_sum"] = double.Parse(dtData.Rows[j]["use_qty_sum"].ToString()) + use_qty;
                        break;
                    }
                }
                if (bNewData)
                {
                    dtData.Rows.Add(pro_code, pro_name, pro_qty, res_qty, use_qty, loc_name);
                }
            }

            return Json(dtData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取得工單資訊
        /// </summary>
        /// <param name="pLineCode"></param>
        /// <param name="pDateTime"></param>
        /// <returns></returns>
        public JsonResult Get_MoMsgByLine(string pLineCode, string pDateTime)
        {
            string returnMsg = "";

            // 第一線
            if (pLineCode == "A1L01")
            {
                returnMsg = "工單訊息-第一線";
            }

            // 第二線
            if (pLineCode == "A1L02")
            {
                returnMsg = "工單訊息-第二線";
            }

            //var returnObj = new {
            //    mo_code =  "",
            //    line_code = "",
            //    time = "",
            //    message = returnMsg,
            //};
            return Json(returnMsg, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ???
        /// </summary>
        /// <param name="pLineCode"></param>
        /// <param name="pDateTime"></param>
        /// <returns></returns>
        public JsonResult Get_1stFloorPackDataByLine(string pLineCode)
        {
            var current_data = new object();
            var next_data = new object();
            string sDate = DateTime.Now.ToString("yyyy/MM/dd");
            //取得工單列表
            string sSql = "SELECT a.mo_code,a.pro_code,b.pro_name,a.plan_qty,a.mo_status" +
                ",ISNULL((SELECT TOP 1(ok_qty) FROM MEM01_0000 WHERE mo_code = a.mo_code AND work_code IN('L1SMB05', 'L2SMB05') ORDER BY ins_date DESC, ins_time DESC),0) AS ok_qty" +
                ",ISNULL((SELECT TOP 1(ng_qty) FROM MEM01_0000 WHERE mo_code = a.mo_code AND work_code IN('L1SMB05', 'L2SMB05') ORDER BY ins_date DESC, ins_time DESC),0) AS ng_qty" +
                " FROM MET01_0000 a" +
                " LEFT JOIN MEB20_0000 b ON a.pro_code = b.pro_code" +
                " WHERE a.plan_line_code =@line" +
                " AND sch_date_s =@date" +
                " AND a.mo_status IN ('20', '30')" +
                " AND a.mo_code NOT IN (SELECT rel_code FROM WMT0200 WHERE ins_date=a.sch_date_s AND rel_type='INW')" +
                " ORDER BY a.sch_time_s";
            DataTable dtTmp = comm.Get_DataTable(sSql, "line,date", pLineCode + "," + sDate);


            // 第一線
            var mo_data = new object();

            // 目前製令
            // 目前製令-製令資訊
            var mo_data_current = new List<dynamic>();
            if (dtTmp.Rows.Count >= 1)
            {
                DataRow row = dtTmp.Rows[0];
                string mo_status = row["mo_status"].ToString();
                string mo_status_name = "";
                switch (mo_status)
                {
                    case "20":
                        mo_status_name = "未開始";
                        break;
                    case "30":
                        mo_status_name = "生產中";
                        break;
                }
                mo_data = new
                {
                    mo_code = row["mo_code"].ToString(),
                    pro_code = row["pro_code"].ToString(),
                    pro_name = row["pro_name"].ToString(),
                    plan_qty = double.Parse(row["plan_qty"].ToString()),
                    current_qty = double.Parse(row["ok_qty"].ToString()),
                    ng_qty = double.Parse(row["ng_qty"].ToString()),
                    mo_status = mo_status,
                    mo_status_name = mo_status_name
                };
            }
            else
            {

                mo_data = new
                {
                    mo_code = "",
                    pro_code = "",
                    pro_name = "",
                    plan_qty = 0,
                    current_qty = 0,
                    ng_qty = 0,
                    mo_status = "",
                    mo_status_name = "無工單"
                };
            }
            mo_data_current.Add(mo_data);


            current_data = new
            {
                mo_info = mo_data_current
            };

            // 下一張製令
            // 下一張製令-產品資訊
            var mo_data_next = new List<dynamic>();
            if (dtTmp.Rows.Count >= 2)
            {
                DataRow row = dtTmp.Rows[1];
                string mo_status = row["mo_status"].ToString();
                string mo_status_name = "";
                switch (mo_status)
                {
                    case "20":
                        mo_status_name = "未開始";
                        break;
                    case "30":
                        mo_status_name = "生產中";
                        break;
                }
                mo_data = new
                {
                    mo_code = row["mo_code"].ToString(),
                    pro_code = row["pro_code"].ToString(),
                    pro_name = row["pro_name"].ToString(),
                    plan_qty = double.Parse(row["plan_qty"].ToString()),
                    current_qty = double.Parse(row["ok_qty"].ToString()),
                    ng_qty = double.Parse(row["ng_qty"].ToString()),
                    mo_status = mo_status,
                    mo_status_name = mo_status_name
                };
            }
            else
            {

                mo_data = new
                {
                    mo_code = "",
                    pro_code = "",
                    pro_name = "",
                    plan_qty = 0,
                    current_qty = 0,
                    ng_qty = 0,
                    mo_status = "",
                    mo_status_name = "無工單"
                };
            }
            mo_data_next.Add(mo_data);

            next_data = new
            {
                mo_info = mo_data_next
            };

            var returnObj = new
            {
                current_order = current_data,
                next_order = next_data
            };

            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取得生產分布的圓餅圖資料
        /// </summary>
        /// <param name="pLineCode"></param>
        /// <param name="pDateTime"></param>
        /// <returns></returns>
        public JsonResult Get_PieData_MO(string pLineCode, string pDateTime)
        {
            // group by 取plan_qty 總和
            string sSql = " Select pro_code, sum(plan_qty) as plan_qty_sum "
                        + "     from MET01_0000 "
                        + "     where 1 = 1 "
                        + "         and plan_line_code like @plan_line_code "
                        + "         and sch_date_s = @sch_date_s "
                        + "         and mo_status in (20, 30) "
                        + "     group by pro_code";

            // group 後left join 產品名稱的table
            sSql = " Select s.*, pro_name "
                        + " from (" + sSql + ") as s"
                        + " left join MEB20_0000 on MEB20_0000.pro_code = s.pro_code ";
            DataTable dtTmp = comm.Get_DataTable(sSql, "plan_line_code,sch_date_s", pLineCode + "%" + "," + pDateTime);

            List<object> seriseItem_data = new List<object>();
            object seriseItem_dataItem = new object();
            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                seriseItem_dataItem = new
                {
                    key = dtTmp.Rows[i]["pro_code"].ToString(),
                    name = dtTmp.Rows[i]["pro_name"].ToString(),
                    value = dtTmp.Rows[i]["plan_qty_sum"].ToString(),
                };
                seriseItem_data.Add(seriseItem_dataItem);
            }

            var returnObj = new
            {
                title = new { text = "生產分佈 - 圓餅圖" },
                //legend = new { },
                seriesItem = new
                {
                    name = "生產分佈",
                    data = seriseItem_data
                }
            };


            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取得良率的圓餅圖資料
        /// </summary>
        /// <param name="pTkCode"></param>
        /// <param name="pDateTime"></param>
        /// <returns></returns>
        public JsonResult Get_PieData_Yield(string pTkCode, string pDateTime)
        {

            var returnObj = new
            {
                title = new { text = "良率分佈 - 圓餅圖" },
                //legend = new { },
                seriesItem = new
                {
                    name = "良率分佈",
                    data = new List<object>() {
                        new { key = "", name = "良品", value = "310" },
                        new { key = "", name = "不良品", value = "55" },
                    }
                }
            };


            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取得目前製令的製程進度
        /// </summary>
        /// <param name="pMoCode"></param>
        /// <returns></returns>
        public JsonResult Get_MoProgress(string pMoCode)
        {
            string result = "";

            string sSql = "select mo_code, MET01_0000.pro_code, plan_qty, mo_status, pro_name" +
                     ",ISNULL((SELECT TOP 1 ok_qty FROM MEM01_0000 WHERE mo_code = MET01_0000.mo_code AND work_code IN('L1SMB01', 'L2SMB01')),0) AS ok_qty" +
                     ",ISNULL((SELECT TOP 1 ng_qty FROM MEM01_0000 WHERE mo_code = MET01_0000.mo_code AND work_code IN('L1SMB01', 'L2SMB01')),0) AS ng_qty" +
                     ",ISNULL((SELECT TOP 1 ok_qty FROM MEM01_0000 WHERE mo_code = MET01_0000.mo_code AND work_code IN('L1SMB03', 'L2SMB03')),0) AS ok_qty2" +
                     ",ISNULL((SELECT TOP 1 ng_qty FROM MEM01_0000 WHERE mo_code = MET01_0000.mo_code AND work_code IN('L1SMB03', 'L2SMB03')),0) AS ng_qty2" +
                     ",ISNULL((SELECT TOP 1 ok_qty FROM MEM01_0000 WHERE mo_code = MET01_0000.mo_code AND work_code IN('L1SMB04', 'L2SMB04')),0) AS ok_qty3" +
                     ",ISNULL((SELECT TOP 1 ng_qty FROM MEM01_0000 WHERE mo_code = MET01_0000.mo_code AND work_code IN('L1SMB04', 'L2SMB04')),0) AS ng_qty3" +
                     ",ISNULL((SELECT TOP 1 ok_qty FROM MEM01_0000 WHERE mo_code = MET01_0000.mo_code AND work_code IN('L1SMB05', 'L2SMB05')),0) AS ok_qty4" +
                     ",ISNULL((SELECT TOP 1 ng_qty FROM MEM01_0000 WHERE mo_code = MET01_0000.mo_code AND work_code IN('L1SMB05', 'L2SMB05')),0) AS ng_qty4" +
                     " from MET01_0000" +
                     " left join MEB20_0000 on MEB20_0000.pro_code = MET01_0000.pro_code" +
                     " where mo_code = @mo_code";

            DataTable dtTmp = comm.Get_DataTable(sSql, "mo_code", pMoCode);
            if (dtTmp.Rows.Count > 0)
            {
                double ok_qty = double.Parse(dtTmp.Rows[0]["ok_qty"].ToString());
                double ok_qty2 = double.Parse(dtTmp.Rows[0]["ok_qty2"].ToString());
                double ok_qty3 = double.Parse(dtTmp.Rows[0]["ok_qty3"].ToString());
                double ok_qty4 = double.Parse(dtTmp.Rows[0]["ok_qty4"].ToString());
                if (ok_qty4 > 0) { result = "裝箱"; }
                else if (ok_qty3 > 0) { result = "套標"; }
                else if (ok_qty2 > 0) { result = "貼標"; }
                else if (ok_qty > 0) { result = "投料"; }
            }
            //switch (pMoCode)
            //{
            //    case "3101016615":
            //        result = "投料";
            //        break;
            //    case "3101016614":
            //        result = "貼標";
            //        break;
            //    case "3101016616":
            //        result = "裝箱";
            //        break;
            //    case "3101016688":
            //        result = "裝箱";
            //        break;
            //}

            return Json(result, JsonRequestBehavior.AllowGet);
        }




        /// <summary>
        /// 取得生產看板的長條圖資料
        /// </summary>
        /// <param name="pTkCode"></param>
        /// <param name="pDateTime"></param>
        /// <returns></returns>
        public JsonResult Get_BarData(string pTkCode, string pDateTime)
        {
            // x軸資料和y軸資料數量要一樣
            List<string> xAxis_List = new List<string>() { "1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月" };
            List<double> yAxix_list1 = new List<double>() { 2.0, 4.9, 7.0, 23.2, 25.6, 76.7, 135.6, 162.2, 32.6, 20.0, 6.4, 3.3 };
            List<double> yAxis_list2 = new List<double>() { 2.6, 5.9, 9.0, 26.4, 28.7, 70.7, 175.6, 182.2, 48.7, 18.8, 6.0, 2.3 };
            //var list1 = xAxis_List.Select(x => new { x = "", y = "" }).ToList();
            //var list2 = xAxis_List.Select(x => new { x = "", y = "" }).ToList();

            var yAxis_data1 = new
            {
                label = "已完成",
                data = yAxix_list1
            };
            var yAxis_data2 = new
            {
                label = "未完成",
                data = yAxis_list2
            };

            var returnObj = new
            {
                xAxis_data = xAxis_List,
                yAxis_data1 = yAxis_data1,
                yAxis_data2 = yAxis_data2
            };

            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult Get_DSB040A_Data(string pLineCode, string pDateTime)
        //{
        //    var returnList = new List<dynamic>();
        //    var data = new object();
        //    data = new
        //    {
        //        mo_code = "MO0001",
        //        pro_code = "",
        //        pro_name = "多喝水-450ML",
        //        plan_qty = 16000,
        //        current_qty = 4281,
        //        ng_qty = 20,
        //        work_code = "",
        //        work_code_name = "貼標"
        //    };
        //    returnList.Add(data);

        //    data = new
        //    {
        //        mo_code = "MO0002",
        //        pro_code = "",
        //        pro_name = "多喝水-600ML",
        //        plan_qty = 32000,
        //        current_qty = 0,
        //        ng_qty = 0,
        //        work_code = "",
        //        work_code_name = ""
        //    };
        //    returnList.Add(data);

        //    data = new
        //    {
        //        mo_code = "",
        //        pro_code = "",
        //        pro_name = "",
        //        plan_qty = 0,
        //        current_qty = 0,
        //        ng_qty = 0,
        //        work_code = "",
        //        work_code_name = ""
        //    };
        //    returnList.Add(data);

        //    data = new
        //    {
        //        mo_code = "",
        //        pro_code = "",
        //        pro_name = "",
        //        plan_qty = 0,
        //        current_qty = 0,
        //        ng_qty = 0,
        //        work_code = "",
        //        work_code_name = ""
        //    };
        //    returnList.Add(data);

        //    return Json(returnList, JsonRequestBehavior.AllowGet);
        //}

    }
}