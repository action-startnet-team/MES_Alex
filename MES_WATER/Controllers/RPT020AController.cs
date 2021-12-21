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
    public class RPT020AController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();
        CheckData CD = new CheckData();

        public ActionResult Index()
        {
            return View();
        }

        public class RPT020A
        {
            [DisplayName("線別")]
            public string line_code { get; set; }

            [DisplayName("線別名稱")]
            public string line_name { get; set; }

            [DisplayName("設備")]
            public string mac_code { get; set; }

            [DisplayName("設備名稱")]
            public string mac_name { get; set; }

            [DisplayName("停機類型")]
            public string stop_type { get; set; }

            [DisplayName("停機類型名稱")]
            public string stop_type_name { get; set; }

            [DisplayName("停機原因")]
            public string stop_code { get; set; }

            [DisplayName("停機原因名稱")]
            public string stop_name { get; set; }

            [DisplayName("開始日期")]
            public string date_s { get; set; }

            [DisplayName("開始時間")]
            public string time_s { get; set; }

            [DisplayName("結束日期")]
            public string date_e { get; set; }

            [DisplayName("結束時間")]
            public string time_e { get; set; }

            [DisplayName("持續時間")]
            public string sustained_time { get; set; }
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
            List<RPT020A> result = new List<RPT020A>();

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

            ////
            //result = Get_RptData(query_data);

            // 回傳給view
            var returnObj = new
            {
                data = result
            };

            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }


        public List<RPT020A> Get_StatData(JqGridQueryData query_data)
        {
            List<RPT020A> result = new List<RPT020A>();

            string sStartDate = query_data.find("date_s", "S");//日期_起
            string sEndDate = query_data.find("date_s", "E");//日期_止
            string sLineCode = query_data.find("line_code");//線別代號
            string sMacCode_S = query_data.find("mac_code", "S");//設備編號_起
            string sMacCode_E = query_data.find("mac_code", "E");//設備編號_止
            string sStopCode_S = query_data.find("stop_code", "S");//停機原因代號_起
            string sStopCode_E = query_data.find("stop_code", "E");//停機原因代號_止
            string sSql = "";
            int i;
            DataTable dtTmp = comm.Get_DataTable(sSql);

            //抓取資料
            sSql = " SELECT MED04_0000.*, MET01_0000.plan_line_code, MEB12_0000.line_name, MEB15_0000.mac_name, MEB45_0000.stop_name, MEB45_0000.stop_type, BDP21_0100.field_name as stop_type_name " +
                   " FROM MED04_0000 " +
                   " LEFT JOIN MET01_0000 on MET01_0000.mo_code = MED04_0000.mo_code " +
                   " LEFT JOIN MEB12_0000 on MEB12_0000.line_code = MET01_0000.plan_line_code " +
                   " LEFT JOIN MEB15_0000 on MEB15_0000.mac_code = MED04_0000.mac_code " +
                   " LEFT JOIN MEB45_0000 on MEB45_0000.stop_code = MED04_0000.stop_code " +
                   " LEFT JOIN BDP21_0100 on BDP21_0100.field_code = MEB45_0000.stop_type and BDP21_0100.code_code = 'stop_type' " +
                   " WHERE 1=1 ";
            if (!string.IsNullOrEmpty(sStartDate)) { sSql += " AND MED04_0000.date_s >='" + sStartDate + "'"; }
            if (!string.IsNullOrEmpty(sEndDate)) { sSql += " AND MED04_0000.date_s <='" + sEndDate + "'"; }
            if (!string.IsNullOrEmpty(sLineCode)) { sSql += " AND MET01_0000.plan_line_code='" + sLineCode + "'"; }
            if (!string.IsNullOrEmpty(sMacCode_S)) { sSql += " AND MED04_0000.mac_code >='" + sMacCode_S + "'"; }
            if (!string.IsNullOrEmpty(sMacCode_E)) { sSql += " AND MED04_0000.mac_code <='" + sMacCode_E + "'"; }
            if (!string.IsNullOrEmpty(sStopCode_S)) { sSql += " AND MED04_0000.stop_code >='" + sStopCode_S + "'"; }
            if (!string.IsNullOrEmpty(sStopCode_E)) { sSql += " AND MED04_0000.stop_code <='" + sStopCode_E + "'"; }

            dtTmp = comm.Get_DataTable(sSql);
            comm.Ins_BDP20_0000("admin", "RPT020A", "RPT", sSql);


            for (i = 0; i < dtTmp.Rows.Count; i++)
            {
                RPT020A data = new RPT020A();
                data.line_code = dtTmp.Rows[i]["plan_line_code"].ToString();
                data.line_name = dtTmp.Rows[i]["line_name"].ToString();
                data.mac_code = dtTmp.Rows[i]["mac_code"].ToString();
                data.mac_name = dtTmp.Rows[i]["mac_name"].ToString();
                data.stop_code = dtTmp.Rows[i]["stop_code"].ToString();
                data.stop_name = dtTmp.Rows[i]["stop_name"].ToString();
                data.stop_type = dtTmp.Rows[i]["stop_type"].ToString();
                data.stop_type_name = dtTmp.Rows[i]["stop_type_name"].ToString();
                data.date_s = dtTmp.Rows[i]["date_s"].ToString();
                data.time_s = dtTmp.Rows[i]["time_s"].ToString();
                data.date_e = dtTmp.Rows[i]["date_e"].ToString();
                data.time_e = dtTmp.Rows[i]["time_e"].ToString();
                DateTime start = new DateTime();
                DateTime end = new DateTime();
                if (CD.IsDate(data.date_s + " " + data.time_s) && CD.IsDate(data.date_e + " " + data.time_e))
                {
                    start = DateTime.Parse(data.date_s + " " + data.time_s);
                    end = DateTime.Parse(data.date_e + " " + data.time_e);
                    data.sustained_time = new TimeSpan(end.Ticks - start.Ticks).TotalSeconds.ToString();
                }
                else
                    data.sustained_time = "";
                //data.plan_qty = comm.sGetDecimal(dtTmp.Rows[i]["plan_qty"].ToString());
                //data.mo_qty = comm.sGetDecimal(dtTmp.Rows[i]["mo_qty"].ToString());
                //data.pro_unit = dtTmp.Rows[i]["pro_unit"].ToString();
                //data.pro_code2 = dtTmp.Rows[i]["pro_code2"].ToString();
                //data.pro_name2 = dtTmp.Rows[i]["pro_name2"].ToString();
                //data.pro_qty = comm.Get_QueryData<decimal>("MET01_0100", "where mo_code = '" + data.mo_code + "' AND pro_code = '" + data.pro_code2 + "'", "pro_qty");
                //data.dis_qty = comm.Get_QueryData<decimal>("MET01_0100", "where mo_code = '" + data.mo_code + "' AND pro_code = '" + data.pro_code2 + "'", "dis_qty");
                //data.atl_consm_qty = comm.sGetDecimal(dtTmp.Rows[i]["atl_consm_qty"].ToString());
                //data.dif_qty = data.dis_qty - data.atl_consm_qty;

                    //if (data.dis_qty == 0)
                    //{
                    //    data.dif_ratio = 0;
                    //}
                    //else
                    //{
                    //    data.dif_ratio = (data.dif_qty / data.dis_qty) * 100;
                    //}

                    //data.std_qty = comm.Get_QueryData<long>("MEB12_0100", "where line_code = '" + dtTmp.Rows[i]["plan_line_code"].ToString() + "' AND pro_code = '" + dtTmp.Rows[i]["pro_code"].ToString() + "'", "std_qty");

                    //至MET04_0100取pro_qty總合-起
                    //decimal tt_qty = 0;
                    //sSql = " select * from MET04_0100 where mo_code = @mo_code AND pro_code = @pro_code ";
                    //Dictionary<string, object> sSqlParams = new Dictionary<string, object>();
                    //sSqlParams.Add("@mo_code", data.mo_code);
                    //sSqlParams.Add("@pro_code", dtTmp.Rows[i]["pro_code"].ToString());
                    //DataTable dt = comm.Get_DataTable(sSql, sSqlParams);
                    //for (int j = 0; j < dt.Rows.Count; j++)
                    //{
                    //    tt_qty += Convert.ToDecimal(dt.Rows[j]["pro_qty"]);
                    //}
                    //data.atl_qty = tt_qty;
                    ////至MET04_0100取pro_qty總合-止

                    //if (data.atl_qty == 0) data.mo_eff = 0; else data.mo_eff = (data.std_qty / data.atl_qty) * 100;
                    //data.per_time = 0;

                    ////至MET04_0400取sub_minute總合-起
                    //int tt_time = 0;
                    //sSql = " select * from MET04_0400 where mo_code = @mo_code ";
                    //Dictionary<string, object> sSqlParams2 = new Dictionary<string, object>();
                    //sSqlParams2.Add("@mo_code", data.mo_code);
                    ////sSqlParams2.Add("@pro_code", dtTmp.Rows[i]["pro_code"].ToString());
                    //DataTable dt2 = comm.Get_DataTable(sSql, sSqlParams2);
                    //for (int j = 0; j < dt2.Rows.Count; j++)
                    //{
                    //    tt_time += Convert.ToInt32(dt2.Rows[j]["sub_minute"]);
                    //}
                    //data.per_time = tt_time;
                    //至MET04_0400取sub_minute總合-止
                    //data.per_time = 0;

                result.Add(data);
            }

            return result;
        }

    }
}