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
    public class RPT090AController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();

        public ActionResult Index()
        {
            return View();
        }

        public class RPT090A
        {
            [DisplayName("生產線別")]
            public string line_code { get; set; }

            [DisplayName("設備名稱")]
            public string mac_code { get; set; }

            [DisplayName("設備運轉分")]
            public string work_time { get; set; }

            [DisplayName("故障分")]
            public string fault_time { get; set; }

            [DisplayName("待機分")]
            public string stanby_time { get; set; }

            [DisplayName("故障率(%)")]
            public string fault_rate { get; set; }

            [DisplayName("稼動率(%)")]
            public string utilization { get; set; }
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
            List<RPT090A> result = new List<RPT090A>();

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


        public List<RPT090A> Get_StatData(JqGridQueryData query_data)
        {
            List<RPT090A> result = new List<RPT090A>();

            string sStartDate = query_data.find("mo_start_date", "S");
            string sEndDate = query_data.find("mo_start_date", "E");
            string sLineCode = query_data.find("line_code");
            string sMacCode_S = query_data.find("mac_code", "S");
            string sMacCode_E = query_data.find("mac_code", "E");
            string sSql = "";
            int i;
            DataTable dtTmp = comm.Get_DataTable(sSql);
            sSql = "  select DATEDIFF(MINUTE,min(MEM01_0000.work_time_s),Max(MEM01_0000.work_time_e))as work_time,mac_code,mo_start_date ,MET01_0000.plan_line_code" +
                   "  from MEM01_0000 left join MET01_0000  on MET01_0000.mo_code = MEM01_0000.mo_code" +
                   "  LEFT JOIN MEB20_0000 on MEB20_0000.pro_code = MET01_0000.pro_code " +
                   "  where mac_code <> '' " +
                   "  and mo_start_date<> '' ";

    
            //抓取資料
            //sSql = " SELECT MET01_0000.*, MEB23_0000.version, MEB20_0000.pro_name, MET04_0300.pro_code as pro_code2, a.pro_name as pro_name2, MET04_0300.pro_qty as atl_consm_qty " +
            //       " FROM MET01_0000 " +
            //       " LEFT JOIN MEB23_0000 on MEB23_0000.bom_code = MET01_0000.bom_code " +
            //       " LEFT JOIN MEB20_0000 on MEB20_0000.pro_code = MET01_0000.pro_code " +
            //       " LEFT JOIN MET04_0300 on MET04_0300.mo_code = MET01_0000.mo_code " +
            //       " LEFT JOIN MEB20_0000 as a on a.pro_code = MET04_0300.pro_code " +
            //       " WHERE 1=1 ";
            if (!string.IsNullOrEmpty(sStartDate)) { sSql += " AND MET01_0000.mo_start_date >='" + sStartDate + "'"; }
            if (!string.IsNullOrEmpty(sEndDate)) { sSql += " AND MET01_0000.mo_start_date <='" + sEndDate + "'"; }
            if (!string.IsNullOrEmpty(sLineCode)) { sSql += " AND MEB20_0000.line_code='" + sLineCode + "'"; }
            if (!string.IsNullOrEmpty(sMacCode_S)) { sSql += " AND MEM01_0000.mac_code >='" + sMacCode_S + "'"; }
            if (!string.IsNullOrEmpty(sMacCode_E)) { sSql += " AND MEM01_0000.mac_code <='" + sMacCode_E + "'"; }
            sSql += " group by mac_code, MET01_0000.mo_start_date,MET01_0000.plan_line_code ";
            dtTmp = comm.Get_DataTable(sSql);
            comm.Ins_BDP20_0000("admin", "RPT090A", "RPT", sSql);


            for (i = 0; i < dtTmp.Rows.Count; i++)
            {
                RPT090A data = new RPT090A();
                data.line_code = dtTmp.Rows[i]["plan_line_code"].ToString();
                data.mac_code = dtTmp.Rows[i]["mac_code"].ToString();
                data.work_time = dtTmp.Rows[i]["work_time"].ToString();
                string sMoStartDate = dtTmp.Rows[i]["mo_start_date"].ToString();
                //故障時間(無法抓)
                data.fault_time = "0";
                if ( !string.IsNullOrEmpty(data.mac_code) && !string.IsNullOrEmpty(sMoStartDate))
                {
                    sSql = "";
                    sSql = " select  sum(DATEDIFF (MINUTE,time_s,time_e)) as sum_min " +
                           " from MED04_0000 "+
                           " where  mac_code = '" + data.mac_code + "'"+
                           "   and  date_s='"+ sMoStartDate + "'";
                    DataTable dt_MED04 = comm.Get_DataTable(sSql);
                    if (dt_MED04.Rows.Count > 0)
                    {
                        data.fault_time = dt_MED04.Rows[0]["sum_min"].ToString();
                    }
                    else
                    {
                        data.fault_time = "0";
                    }
                }


                //待機分
                data.stanby_time = "0";
                //故障率
                data.fault_rate = "0%";
                if (!string.IsNullOrEmpty(data.fault_time) && !string.IsNullOrEmpty(data.work_time))
                {
                    decimal fault_time = decimal.Parse(data.fault_time);
                    decimal work_time = decimal.Parse(data.work_time);
                    if (work_time != 0)
                        data.fault_rate = decimal.Round((100 * (fault_time / work_time)), 2, MidpointRounding.ToEven).ToString() + "%";
                }


                //稼動率
                data.utilization ="0%";

                if (!string.IsNullOrEmpty(data.fault_time) && !string.IsNullOrEmpty(data.work_time))
                {
                    decimal fault_time = decimal.Parse(data.fault_time);
                    decimal work_time = decimal.Parse(data.work_time);
                    if (work_time != 0)
                        data.fault_rate = decimal.Round(100-(100 * (fault_time / work_time)), 2, MidpointRounding.ToEven).ToString() + "%";
                }



                result.Add(data);
            }

            return result;
        }

    }
}