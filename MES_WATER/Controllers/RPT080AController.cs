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
    public class RPT080AController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();
        CheckData CD = new CheckData();
        public ActionResult Index()
        {
            return View();
        }

        public class RPT080A
        {
            [DisplayName("生產線別")]
            public string plan_line_code { get; set; }

            [DisplayName("設備編號")]
            public string mac_code { get; set; }

            [DisplayName("設備名稱")]
            public string mac_name { get; set; }

            [DisplayName("生產品項")]
            public string pro_code { get; set; }

            [DisplayName("品項名稱")]
            public string pro_name { get; set; }

            [DisplayName("生產日期")]
            public string mo_start_date { get; set; }

            [DisplayName("工單號碼")]
            public string mo_code { get; set; }

            [DisplayName("開機時間")]
            public string work_second { get; set; }

            [DisplayName("停機時間")]
            public string stop_time { get; set; }

            [DisplayName("故障時間(分)")]
            public string fault_min { get; set; }

            [DisplayName("進瓶數量")]
            public string all_bot_qty { get; set; }

            [DisplayName("瓶子損耗率")]
            public string bot_loss_rate { get; set; }

            [DisplayName("生產數量")]
            public string iot_ok_qty { get; set; }

            [DisplayName("故障率(%)")]
            public string fault_rate { get; set; }

            [DisplayName("生產效率")]
            public string pro_eff { get; set; }
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
            List<RPT080A> result = new List<RPT080A>();

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


        public List<RPT080A> Get_StatData(JqGridQueryData query_data)
        {
            List<RPT080A> result = new List<RPT080A>();

            string sStartDate = query_data.find("mo_start_date", "S");//日期_起
            string sEndDate = query_data.find("mo_start_date", "E");//日期_止
            string sLineCode = query_data.find("line_code");
            string sMacCode_S = query_data.find("mac_code", "S");
            string sMacCode_E = query_data.find("mac_code", "E");
            string sMoCode = query_data.find("mo_code");
            string sSql = "";
            int i;
            DataTable dtTmp = comm.Get_DataTable(sSql);

            //抓取資料
            //sSql = " SELECT MET01_0000.*, MEB15_0000.mac_code, MEB15_0000.mac_name, MEB20_0000.pro_name, MEP06_0000.work_second " +
            //       " FROM MET01_0000 " +
            //       " LEFT JOIN MEB15_0000 on MEB15_0000.line_code = MET01_0000.plan_line_code " +
            //       " LEFT JOIN MEB20_0000 on MEB20_0000.pro_code = MET01_0000.pro_code " +
            //       " LEFT JOIN MEP06_0000 on MEP06_0000.mac_code = MEB15_0000.mac_code " +
            //       " WHERE 1=1 ";
            sSql = "SELECT MET01_0000.*, MEB15_0000.mac_name ,MEM01_0000.work_time_s,MEM01_0000.work_time_e,MEM01_0000.mac_code, MEB20_0000.pro_name" +
                  "  FROM MET01_0000 " +
                  "  LEFT JOIN MEM01_0000 on MET01_0000.mo_code = MEM01_0000.mo_code " +
                  "  LEFT JOIN MEB20_0000 on MEB20_0000.pro_code = MET01_0000.pro_code " +
                  "  LEFT JOIN MEB15_0000 on MEM01_0000.mac_code = MEB15_0000.mac_code " +
                  "  where 1 = 1 "+
                  "  and MEM01_0000.mac_code <> '' "+
                  "  and MEM01_0000.work_time_s <>'' "+
                  "  and MEM01_0000.work_time_e <>'' ";

            if (!string.IsNullOrEmpty(sStartDate)) { sSql += " AND MET01_0000.mo_start_date >='" + sStartDate + "'"; }
            if (!string.IsNullOrEmpty(sEndDate)) { sSql += " AND MET01_0000.mo_start_date <='" + sEndDate + "'"; }
            if (!string.IsNullOrEmpty(sLineCode)) { sSql += " AND MET01_0000.plan_line_code='" + sLineCode + "'"; }
            if (!string.IsNullOrEmpty(sMacCode_S)) { sSql += " AND MEB15_0000.mac_code >='" + sMacCode_S + "'"; }
            if (!string.IsNullOrEmpty(sMacCode_E)) { sSql += " AND MEB15_0000.mac_code <='" + sMacCode_E + "'"; }
            if (!string.IsNullOrEmpty(sMoCode)) { sSql += " AND MET01_0000.mo_code='" + sMoCode + "'"; }

            dtTmp = comm.Get_DataTable(sSql);
            comm.Ins_BDP20_0000("admin", "RPT080A", "RPT", sSql);


            for (i = 0; i < dtTmp.Rows.Count; i++)
            {
                RPT080A data = new RPT080A();
                data.plan_line_code = dtTmp.Rows[i]["plan_line_code"].ToString();
                data.mac_code = dtTmp.Rows[i]["mac_code"].ToString();
                data.mac_name = dtTmp.Rows[i]["mac_name"].ToString();
                data.pro_code = dtTmp.Rows[i]["pro_code"].ToString();
                data.pro_name = dtTmp.Rows[i]["pro_name"].ToString();
                data.mo_start_date = dtTmp.Rows[i]["mo_start_date"].ToString();
                data.mo_code = dtTmp.Rows[i]["mo_code"].ToString();
                data.work_second = dtTmp.Rows[i]["work_time_s"].ToString();

                data.stop_time = dtTmp.Rows[i]["work_time_e"].ToString();
                //停機時間(取MED04_0000在MEP04_0000開始，結束時間內所使用的停機時間)，若MEP04_0000同時有兩筆相同工單、設備的資料，此欄位會無法計算(顯示0)。
                //data.stop_time = "0";
                //if (!string.IsNullOrEmpty(data.mo_code) && !string.IsNullOrEmpty(data.mac_code))
                //{
                //    DataTable dt_MEP04 = comm.Get_DataTable(" select * from MEP04_0000 where mo_code = '" + data.mo_code + "' and mac_code = '" + data.mac_code + "'");
                //    if (dt_MEP04.Rows.Count == 1)
                //    {
                //        string time_s = dt_MEP04.Rows[0]["date_s"].ToString() + " " + dt_MEP04.Rows[0]["time_s"].ToString() + ":00";
                //        string time_e = dt_MEP04.Rows[0]["date_e"].ToString() + " " + dt_MEP04.Rows[0]["time_e"].ToString() + ":00";
                //        if (CD.IsDate(time_s) && CD.IsDate(time_e))
                //        {
                //            DateTime MEP04_time_s = DateTime.Parse(time_s);
                //            DateTime MEP04_time_e = DateTime.Parse(time_e);

                //            DataTable dt_MED04 = comm.Get_DataTable(" select * from MED04_0000 where mac_code = '" + data.mac_code + "'");
                //            DateTime time_gap_s = new DateTime();
                //            DateTime time_gap_e = new DateTime();
                //            int gap = 0;
                //            TimeSpan ts;
                //            if (dt_MED04.Rows.Count >= 1)
                //            {
                //                for (int j = 0; j < dt_MED04.Rows.Count; j++)
                //                {
                //                    time_s = dt_MED04.Rows[j]["date_s"] + " " + dt_MED04.Rows[j]["time_s"];
                //                    time_e = dt_MED04.Rows[j]["date_e"] + " " + dt_MED04.Rows[j]["time_e"];
                //                    if (CD.IsDate(time_s) && CD.IsDate(time_e))
                //                    {
                //                        time_gap_s = DateTime.Parse(time_s);
                //                        time_gap_e = DateTime.Parse(time_e);
                //                        if (time_gap_e > time_gap_s && time_gap_s >= MEP04_time_s && time_gap_e <= MEP04_time_e)
                //                        {
                //                            ts = time_gap_e - time_gap_s;
                //                            gap += int.Parse(ts.TotalSeconds.ToString());
                //                        }
                //                    }
                //                }
                //                data.stop_time = gap.ToString();
                //            }
                //        }
                //    }
                //}

                //故障時間(無法抓)
                data.fault_min = "0";
                if (!string.IsNullOrEmpty(data.mo_code) && !string.IsNullOrEmpty(data.mac_code))
                {
                    sSql = "";
                    sSql = " select  sum(DATEDIFF (MINUTE,time_s,time_e)) as sum_min "+
                           " from MED04_0000 where mo_code = '" + data.mo_code + "' and mac_code = '" + data.mac_code + "'";
                    DataTable dt_MED04 = comm.Get_DataTable(sSql);
                    if (dt_MED04.Rows.Count > 0)
                    {
                        data.fault_min = dt_MED04.Rows[0]["sum_min"].ToString();
                    }
                    else
                    {
                        data.fault_min = "0";
                    }
                }


                    //進瓶數量(MEP01_0000該工單、該機台的iot_ok_qty + iot_ng_qty)
                    data.all_bot_qty = "0";
                if (!string.IsNullOrEmpty(data.mo_code) && !string.IsNullOrEmpty(data.mac_code))
                {
                    //decimal iot_ok_qty = comm.Get_QueryData<decimal>("MEP01_0000", " where mo_code = '" + data.mo_code + "' and mac_code = '" + data.mac_code + "'", "iot_ok_qty");
                    //decimal iot_ng_qty = comm.Get_QueryData<decimal>("MEP01_0000", " where mo_code = '" + data.mo_code + "' and mac_code = '" + data.mac_code + "'", "iot_ng_qty");
                    //data.all_bot_qty = (iot_ok_qty + iot_ng_qty).ToString();
                    decimal iot_ok_qty = comm.Get_QueryData<decimal>("MEM01_0000", " where mo_code = '" + data.mo_code + "' and mac_code = '" + data.mac_code + "'", "ok_qty");
                    decimal iot_ng_qty = comm.Get_QueryData<decimal>("MEM01_0000", " where mo_code = '" + data.mo_code + "' and mac_code = '" + data.mac_code + "'", "ng_qty");
                    data.all_bot_qty = (iot_ok_qty + iot_ng_qty).ToString();
                }

                //生產數量(MEP01_0000 該工單、該機台的iot_ok_qty)
                data.iot_ok_qty = "0";
                if (!string.IsNullOrEmpty(data.mo_code) && !string.IsNullOrEmpty(data.mac_code))
                {
                    //decimal iot_ok_qty = comm.Get_QueryData<decimal>("MEP01_0000", " where mo_code = '" + data.mo_code + "' and mac_code = '" + data.mac_code + "'", "iot_ok_qty");
                    //data.iot_ok_qty = iot_ok_qty.ToString();
                    decimal iot_ok_qty = comm.Get_QueryData<decimal>("MEM01_0000", " where mo_code = '" + data.mo_code + "' and mac_code = '" + data.mac_code + "'", "ok_qty");
                    data.iot_ok_qty = iot_ok_qty.ToString();
                }

                //瓶子損耗率(100% - 生產數量/進瓶數量)
                data.bot_loss_rate = "";
                if (!string.IsNullOrEmpty(data.all_bot_qty) && !string.IsNullOrEmpty(data.iot_ok_qty))
                {
                    decimal all_bot_qty = decimal.Parse(data.all_bot_qty);
                    decimal iot_ok_qty = decimal.Parse(data.iot_ok_qty);
                    if (all_bot_qty != 0)
                        data.bot_loss_rate = decimal.Round((100 - (100 * (iot_ok_qty / all_bot_qty))), 2, MidpointRounding.ToEven).ToString() + "%";
                }

                //故障率(100% - 生產數量/進瓶數量)
                data.fault_rate = "";
                if (!string.IsNullOrEmpty(data.all_bot_qty) && !string.IsNullOrEmpty(data.iot_ok_qty))
                {
                    decimal all_bot_qty = decimal.Parse(data.all_bot_qty);
                    decimal iot_ok_qty = decimal.Parse(data.iot_ok_qty);
                    if (all_bot_qty != 0)
                        data.fault_rate = decimal.Round((100 -( 100 * (iot_ok_qty / all_bot_qty))), 2, MidpointRounding.ToEven).ToString() + "%";
                }
                //生產效率(生產數量/進瓶數量%)
                data.pro_eff = "";
                if (!string.IsNullOrEmpty(data.all_bot_qty) && !string.IsNullOrEmpty(data.iot_ok_qty))
                {
                    decimal all_bot_qty = decimal.Parse(data.all_bot_qty);
                    decimal iot_ok_qty = decimal.Parse(data.iot_ok_qty);
                    if (all_bot_qty != 0)
                        data.pro_eff = decimal.Round((100 * (iot_ok_qty / all_bot_qty)), 2, MidpointRounding.ToEven).ToString() + "%";
                }


                result.Add(data);
            }

            return result;
        }

    }
}