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

namespace MES_WATER.Controllers
{
    public class RPT011AController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();

        public ActionResult Index()
        {
            return View();
        }

        public class RPT011A
        {
            [DisplayName("日期區間")]
            public string date { get; set; }

            [DisplayName("員工編號")]
            public string usr_code { get; set; }

            [DisplayName("人員")]
            public string usr_name { get; set; }

            [DisplayName("標準時數")]
            public string std_time { get; set; }

            [DisplayName("實際上班時數")]
            public string total_time { get; set; }

            [DisplayName("實際上班時數(正常)")]
            public string std_total_time { get; set; }

            [DisplayName("實際加班時數")]
            public string overtime { get; set; }

            [DisplayName("除外時數")]
            public string except_time { get; set; }

            [DisplayName("作業效率")]
            public string rate { get; set; }
        }

        /// <summary>
        /// View的Table資料來源
        /// </summary>
        /// <param name="pWhere">View傳來的查詢資料，JSON字串</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Get_DataTableData(string pWhere)
        {
            // 報表欄位結構
            List<RPT011A> result = new List<RPT011A>();

            // 沒有下查詢條件時就沒有資料
            if (string.IsNullOrEmpty(pWhere))
            {
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }

            // 將前面的查詢欄位放到query_data
            JqGridQueryData query_data = new JqGridQueryData();
            if (!string.IsNullOrEmpty(pWhere))
            {
                List<JqGridQueryData> query_datas = JsonConvert.DeserializeObject<List<JqGridQueryData>>(pWhere);
                if (query_datas.Count > 0)
                {
                    query_data = query_datas[0];
                }
            }
            result.AddRange(Get_StatData(query_data));

            // 回傳給view
            var returnObj = new
            {
                data = result
            };

            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }


        public List<RPT011A> Get_StatData(JqGridQueryData query_data)
        {
            List<RPT011A> result = new List<RPT011A>();
            string sDateS = query_data.find("date", "S");
            string sDateE = query_data.find("date", "E");

            // 標準工時
            string sSql = @"select sum(temp.std_time) as std_time
                            from
                            (
                            select 
                            case when CONVERT(DATETIME, time_s) < CONVERT(DATETIME, time_e) 
                            then DATEDIFF(hour, CONVERT(DATETIME, time_s), CONVERT(DATETIME, time_e))
                            when CONVERT(DATETIME, time_s) > CONVERT(DATETIME, time_e) 
                            then DATEDIFF(hour, CONVERT(DATETIME, time_s), CONVERT(DATETIME, time_e)) + 24 end as std_time
                            from MEB13_0000 ) temp";            

            DataTable dtTemp = comm.Get_DataTable(sSql);
            string std_time = dtTemp.Rows[0]["std_time"].ToString();
            sSql = @"select time_s from MEB13_0000 where class_name = '白班'";
            dtTemp = comm.Get_DataTable(sSql);
            DateTime s = DateTime.Parse(dtTemp.Rows[0]["time_s"].ToString());
            sSql = @"select time_e from MEB13_0000 where class_name = '夜班'";
            dtTemp = comm.Get_DataTable(sSql);
            DateTime e = DateTime.Parse(dtTemp.Rows[0]["time_e"].ToString());
            string time_s = (s.Hour < 10 ? "0" + s.Hour.ToString() : s.Hour.ToString()) + ":00:00";
            string time_e = (e.Hour < 10 ? "0" + e.Hour.ToString() : e.Hour.ToString()) + ":00:00";
            bool isOverDay = s.Hour > e.Hour;// 跨天(夜班跨過一天)
            // usr_list人員清單
            sSql = @"select * from BDP08_0000";
            dtTemp = comm.Get_DataTable(sSql);

            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                string usr_code = dtTemp.Rows[i]["usr_code"].ToString();

                sSql = @"select a.*, ROUND(DATEDIFF(minute, CONVERT(DATETIME, b.date_s + ' ' + b.time_s), CONVERT(DATETIME, b.date_e + ' ' + b.time_e)) / 60.0, 2) as except_time
                        from MED01_0100 a 
                        left join MED05_0000 b on a.mo_code = b.mo_code and a.usr_code = b.usr_code
                        where a.date_e <> '' and a.usr_code = '" + usr_code + "'";
                if (sDateS != "")
                    sSql += " and CONVERT(DATE, '" + sDateS + "') <= CONVERT(DATE, a.date_s)";
                if (sDateE != "")
                    sSql += " and CONVERT(DATE, a.date_e) <= CONVERT(DATE, '" + sDateE + "')";
                DataTable dtTemp2 = comm.Get_DataTable(sSql);

                string except_time = "";
                TimeSpan work_time = new TimeSpan();
                 TimeSpan work_time_normal = new TimeSpan();
                for (int j = 0; j < dtTemp2.Rows.Count; j++)
                {
                    except_time = dtTemp2.Rows[0]["except_time"].ToString();
                    DateTime start_time = DateTime.Parse(dtTemp2.Rows[0]["date_s"].ToString() + ' ' + dtTemp2.Rows[0]["time_s"].ToString());
                    DateTime end_time = DateTime.Parse(dtTemp2.Rows[0]["date_e"].ToString() + ' ' + dtTemp2.Rows[0]["time_e"].ToString());

                    TimeSpan temp = end_time - start_time;
                    work_time += temp;

                    DateTime start_time_normal;
                    DateTime end_time_normal;                    
                    if (!isOverDay)
                    {
                        start_time_normal = start_time.TimeOfDay > s.TimeOfDay ? start_time : new DateTime(start_time.Year, start_time.Month, start_time.Day, s.Hour, s.Minute, s.Second);
                        end_time_normal = e.TimeOfDay > end_time.TimeOfDay ? end_time : new DateTime(end_time.Year, end_time.Month, end_time.Day, e.Hour, e.Minute, e.Second);                        
                    }
                    else
                    {                      
                        if (start_time.TimeOfDay < e.TimeOfDay)
                            start_time_normal = start_time;
                        else
                            start_time_normal = start_time.TimeOfDay > s.TimeOfDay ? start_time : new DateTime(start_time.Year, start_time.Month, start_time.Day, s.Hour, s.Minute, s.Second);

                        if (end_time.TimeOfDay > s.TimeOfDay)
                            end_time_normal = end_time;
                        else
                            end_time_normal = e.TimeOfDay > end_time.TimeOfDay ? end_time : new DateTime(end_time.Year, end_time.Month, end_time.Day, e.Hour, e.Minute, e.Second);
                    }
                    TimeSpan temp2 = end_time_normal - start_time_normal;
                    work_time_normal += temp2;
                }

                double total_hour = work_time.TotalHours;// 實際工時
                double total_hour_normal = work_time_normal.TotalHours;// 實際工時(正常)

                RPT011A data = new RPT011A();

                data.date = sDateS + "~" + sDateE;
                data.usr_code = usr_code;
                data.usr_name = dtTemp.Rows[i]["usr_name"].ToString();
                data.std_time = std_time;
                //string A = dtTemp.Rows[i]["A"].ToString();// 實際上班時數
                //string B = dtTemp.Rows[i]["B"].ToString();// 實際上班時數(正常)
                data.total_time = total_hour.ToString("#0.00");
                data.std_total_time = total_hour_normal.ToString("#0.00");
                data.overtime = (total_hour - total_hour_normal).ToString("#0.00");
                data.except_time = except_time != "" ? except_time : "0";
                data.rate = total_hour != 0 ? ((total_hour_normal / total_hour) * 100 ).ToString("#0.00") + "%": "0%";

                result.Add(data);

            }

            //    sSql = "select temp.usr_code, temp.usr_name, sum(temp.A) as A, sum(temp.B) as B, temp.except_time " +
            //       "from " +
            //       "( " +
            //       "select b.usr_code, b.usr_name, " +
            //       "ROUND(DATEDIFF(minute, CONVERT(DATETIME, a.date_s + ' ' + a.time_s), CONVERT(DATETIME, a.date_e + ' ' + a.time_e)) / 60.0, 2) as A, " +
            //       "case when a.time_s < '" + time_s + "' and a.time_e > '" + time_e + "' " +
            //       "then ROUND(DATEDIFF(minute, CONVERT(DATETIME, '" + time_s + "'), CONVERT(DATETIME, '" + time_e + "')) / 60.0, 2) " +
            //       "when a.time_s < '" + time_s + "' then ROUND(DATEDIFF(minute, CONVERT(DATETIME, a.date_s + ' ' + '" + time_s + "'), CONVERT(DATETIME, a.date_e + ' ' + a.time_e)) / 60.0, 2) " +
            //       "when a.time_e > '" + time_e + "' then ROUND(DATEDIFF(minute, CONVERT(DATETIME, a.date_s + ' ' + a.time_s), CONVERT(DATETIME, a.date_e + ' ' + '" + time_e + "')) / 60.0, 2) " +
            //       "else ROUND(DATEDIFF(minute, CONVERT(DATETIME, a.date_s + ' ' + a.time_s), CONVERT(DATETIME, a.date_e + ' ' + a.time_e)) / 60.0, 2) " +
            //       "end " +
            //       "as B " +
            //       ",ROUND(DATEDIFF(minute, CONVERT(DATETIME, c.date_s + ' ' + c.time_s), CONVERT(DATETIME, c.date_e + ' ' + c.time_e)) / 60.0, 2) as except_time " +
            //       "from MED01_0100 a  " +
            //       "left join BDP08_0000 b on a.usr_code = b.usr_code " +
            //       "left join MED05_0000 c on a.usr_code = c.usr_code and a.wrk_code = c.wrk_code and a.mo_code = c.mo_code " +
            //       "where a.date_e <> '' ";
            //if (sDateS != "")
            //    sSql += " and CONVERT(DATE, '" + sDateS + "') <= CONVERT(DATE, a.date_s)";
            //if (sDateE != "")
            //    sSql += " and CONVERT(DATE, a.date_e) <= CONVERT(DATE, '" + sDateE + "')";

            //sSql += ") temp ";
            //sSql += "GROUP BY temp.usr_code, temp.usr_name, temp.except_time";

            //dtTemp = comm.Get_DataTable(sSql);

            //for (int i = 0; i < dtTemp.Rows.Count; i++)
            //{                
            //    RPT011A data = new RPT011A();

            //    data.date = sDateS + "~" + sDateE;
            //    data.usr_code = dtTemp.Rows[i]["usr_code"].ToString();
            //    data.usr_name = dtTemp.Rows[i]["usr_name"].ToString();
            //    data.std_time = std_time;
            //    string A = dtTemp.Rows[i]["A"].ToString();// 實際上班時數
            //    string B = dtTemp.Rows[i]["B"].ToString();// 實際上班時數(正常)
            //    data.total_time = A;
            //    data.std_total_time = B;
            //    data.overtime = (comm.sGetDecimal(A) - comm.sGetDecimal(B)).ToString();
            //    data.except_time = dtTemp.Rows[i]["except_time"].ToString();
            //    data.rate = comm.sGetDecimal(A) != 0 ? (comm.sGetDecimal(B) / comm.sGetDecimal(A)).ToString() : "0";                

            //    result.Add(data);                
            //}

            return result;
        }

    }
}
