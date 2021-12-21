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
    public class RPT180AController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();
        CheckData CD = new CheckData();

        public ActionResult Index()
        {
            return View();
        }

        public class RPT180A
        {
            [DisplayName("工站代碼")]
            public string station_code { get; set; }

            [DisplayName("工站名稱")]
            public string station_name { get; set; }

            [DisplayName("開始日期")]
            public string date_s { get; set; }

            [DisplayName("開始時間")]
            public string time_s { get; set; }

            [DisplayName("結束日期")]
            public string date_e { get; set; }

            [DisplayName("結束時間")]
            public string time_e { get; set; }

            [DisplayName("製程時間")]
            public string wrk_time { get; set; }
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
            List<RPT180A> result = new List<RPT180A>();

            // 沒有下查詢條件時就沒有資料
            //if (string.IsNullOrEmpty(pWhere))
            //{
            //    return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            //}

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


        public List<RPT180A> Get_StatData(JqGridQueryData query_data)
        {
            List<RPT180A> result = new List<RPT180A>();

            string sMoCode = query_data.find("mo_code");
            string sSql = "";
            int i;
            DataTable dtTmp = comm.Get_DataTable(sSql);

            //抓取資料
            sSql = " SELECT * " +
                   " FROM MED02_0000 " +
                   " WHERE mo_status_wrk = 'IN' ";
            if (!string.IsNullOrEmpty(sMoCode)) { sSql += " AND MED02_0000.mo_code ='" + sMoCode + "'"; }

            dtTmp = comm.Get_DataTable(sSql);
            comm.Ins_BDP20_0000("admin", "RPT180A", "RPT", sSql);


            for (i = 0; i < dtTmp.Rows.Count; i++)
            {
                RPT180A data = new RPT180A();
                data.station_code = comm.Get_QueryData("MEB29_0000", dtTmp.Rows[i]["mac_code"].ToString(), "mac_code", "station_code");
                data.station_name = comm.Get_QueryData("MEB29_0000", dtTmp.Rows[i]["mac_code"].ToString(), "mac_code", "station_name");
                data.date_s = dtTmp.Rows[i]["ins_date"].ToString();
                data.time_s = dtTmp.Rows[i]["ins_time"].ToString();
                data.date_e = "";
                data.time_e = "";
                data.wrk_time = "";
                DataTable dt = comm.Get_DataTable(" select * " +
                                                  " from MED02_0000 " +
                                                  " where mo_code = '" + dtTmp.Rows[i]["mo_code"].ToString() + "'" +
                                                  " and wrk_code = '" + dtTmp.Rows[i]["wrk_code"].ToString() + "'" +
                                                  " and mo_status_wrk = 'END' ");
                DateTime start = new DateTime();
                DateTime end = new DateTime();
                if (dt.Rows.Count >= 1)
                {
                    data.date_e = dt.Rows[0]["ins_date"].ToString();
                    data.time_e = dt.Rows[0]["ins_time"].ToString();
                    if (CD.IsDate(data.date_s + " " + data.time_s) && CD.IsDate(data.date_e + " " + data.time_e))
                    {
                        start = DateTime.Parse(data.date_s + " " + data.time_s);
                        end = DateTime.Parse(data.date_e + " " + data.time_e);
                        data.wrk_time = new TimeSpan(end.Ticks - start.Ticks).TotalSeconds.ToString();
                    }
                }

                //DataTable dt = new DataTable();
                //DateTime start = new DateTime();
                //DateTime end = new DateTime();
                //if (!string.IsNullOrEmpty(dtTmp.Rows[i]["mo_code"].ToString()) && !string.IsNullOrEmpty(data.ins_date) && !string.IsNullOrEmpty(data.ins_time)) 
                //{
                //    dt = comm.Get_DataTable(" select * " +
                //                            " from MED02_0000 " +
                //                            " where mo_code = '" + dtTmp.Rows[i]["mo_code"].ToString() + "'" +
                //                            " and wrk_code < '" + dtTmp.Rows[i]["wrk_code"].ToString() + "'" +
                //                            " order by wrk_code DESC ");
                //    if (dt.Rows.Count != 0)
                //    {
                //        if (CD.IsDate(dt.Rows[0]["end_date"].ToString() + " " + dt.Rows[0]["end_time"].ToString()) && CD.IsDate(data.ins_date + " " + data.ins_time))
                //        {
                //            start = DateTime.Parse(dt.Rows[0]["end_date"].ToString() + " " + dt.Rows[0]["end_time"].ToString());
                //            end = DateTime.Parse(data.ins_date + " " + data.ins_time);
                //            data.wrk_time = new TimeSpan(end.Ticks - start.Ticks).TotalSeconds.ToString();
                //        }
                //    }
                //}


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