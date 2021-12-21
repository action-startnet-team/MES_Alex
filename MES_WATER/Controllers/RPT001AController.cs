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
    public class RPT001AController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();

        public ActionResult Index()
        {
            return View();
        }

        public class RPT001A
        {
            [DisplayName("工單號")]
            public string mo_code { get; set; }

            [DisplayName("產品號")]
            public string pro_code { get; set; }

            [DisplayName("生產品項")]
            public string pro_name { get; set; }

            [DisplayName("生產日期")]
            public string mo_start_date { get; set; }

            [DisplayName("上機位置")]
            public string mac_code { get; set; }

            [DisplayName("上機時間")]
            public string in_time { get; set; }

            [DisplayName("下機時間")]
            public string out_time { get; set; }

            [DisplayName("機時(分)")]
            public string duration_mac { get; set; }

            [DisplayName("上機人員")]
            public string usr_code { get; set; }

            [DisplayName("上工時間")]
            public string time_s { get; set; }

            [DisplayName("下工時間")]
            public string time_e { get; set; }

            [DisplayName("人時(分)")]
            public string duration_usr { get; set; }

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
            List<RPT001A> result = new List<RPT001A>();

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


        public List<RPT001A> Get_StatData(JqGridQueryData query_data)
        {
            List<RPT001A> result = new List<RPT001A>();

            string sCaldateS = query_data.find("mo_start_date", "S"); //生產日期
            string sCaldateE = query_data.find("mo_start_date", "E"); //生產日期
            string sProCode = query_data.find("pro_code"); //產品編號
            string sLineCode = query_data.find("mo_code"); //公單
            
            string sSql = "";
            sSql = "select * from MET01_0000 a " +
                   "left join MEB20_0000 b on a.pro_code = b.pro_code where 1=1 ";

            if (!string.IsNullOrEmpty(sCaldateS) || !string.IsNullOrEmpty(sCaldateE))
                sSql += " and a.mo_end_date between '" + sCaldateS + "' and '" + sCaldateE + "'";
            if (!string.IsNullOrEmpty(sProCode))
                sSql += " and a.pro_code = '" + sProCode + "'";
            if (!string.IsNullOrEmpty(sLineCode))
                sSql += " and a.mo_code = '" + sLineCode + "'";

            DataTable dtTmp = comm.Get_DataTable(sSql);
            
            int i;
            for (i = 0; i < dtTmp.Rows.Count; i++)
            {
                RPT001A data = new RPT001A();
                string mo_code = dtTmp.Rows[i]["mo_code"].ToString();
                
                data.mo_code = mo_code;
                data.pro_code = dtTmp.Rows[i]["pro_code"].ToString();
                data.pro_name = dtTmp.Rows[i]["pro_name"].ToString();
                data.mo_start_date = dtTmp.Rows[i]["mo_start_date"].ToString();
                data.mac_code = "";
                data.in_time = "";
                data.out_time = "";
                data.duration_mac = "";
                data.usr_code = "";
                data.time_s = "";
                data.time_e = "";
                data.duration_usr = "";

                TimeSpan ts = new TimeSpan();

                sSql = "select * from MED02_0000 where mo_status_wrk = 'IN' and mo_code = '" + mo_code + "'";
                DataTable dtTmp_1 = comm.Get_DataTable(sSql);
                if (dtTmp_1.Rows.Count != 0)
                {
                    data.mac_code = dtTmp_1.Rows[0]["mac_code"].ToString();

                    string in_time = "";
                    if (dtTmp_1.Rows.Count != 0)
                        in_time = dtTmp_1.Rows[0]["ins_date"].ToString() + " " + dtTmp_1.Rows[0]["ins_time"].ToString();

                    data.in_time = in_time;
                    DateTime inTime;
                    DateTime.TryParse(in_time, out inTime);

                    sSql = "select ins_date, ins_time from MED02_0000 where mo_status_wrk = 'OUT' and mo_code = '" + mo_code + "'";
                    DataTable dtTmp_2 = comm.Get_DataTable(sSql);

                    string out_time = "";
                    if (dtTmp_2.Rows.Count != 0)
                        out_time = dtTmp_2.Rows[0]["ins_date"].ToString() + " " + dtTmp_2.Rows[0]["ins_time"].ToString();

                    data.out_time = out_time;
                    DateTime outTime;
                    DateTime.TryParse(out_time, out outTime);
                    
                    if (!string.IsNullOrEmpty(in_time) && !string.IsNullOrEmpty(out_time))
                        ts = new TimeSpan(inTime.Ticks - outTime.Ticks);

                    data.duration_mac = (!string.IsNullOrEmpty(in_time) && !string.IsNullOrEmpty(out_time)) ? ts.Minutes.ToString() : "";
                }

                sSql = "select * from MED01_0000 where mo_code = '" + mo_code + "'";
                DataTable dtTmp_3 = comm.Get_DataTable(sSql);

                if(dtTmp_3.Rows.Count != 0)
                {
                    data.usr_code = dtTmp_3.Rows[i]["usr_code"].ToString();
                    string s_time = dtTmp_3.Rows[i]["date_s"].ToString() + " " + dtTmp_3.Rows[i]["time_s"].ToString();
                    string e_time = dtTmp_3.Rows[i]["date_e"].ToString() + " " + dtTmp_3.Rows[i]["time_e"].ToString();
                    data.time_s = s_time;
                    data.time_e = e_time;

                    DateTime sTime;
                    DateTime.TryParse(s_time, out sTime);
                    DateTime eTime;
                    DateTime.TryParse(e_time, out eTime);

                    if (s_time != " " && e_time != " ")
                        ts = new TimeSpan(sTime.Ticks - eTime.Ticks);
                    data.duration_usr = (s_time != " " && e_time != " ") ? ts.Minutes.ToString() : "";
                }               
                result.Add(data);
            }            
            return result;
        }
    }
}
