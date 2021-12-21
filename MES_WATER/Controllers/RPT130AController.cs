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
    public class RPT130AController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();
        CheckData CD = new CheckData();

        public ActionResult Index()
        {
            return View();
        }

        public class RPT130A
        {
            [DisplayName("日期")]
            public string date { get; set; }

            [DisplayName("線別")]
            public string line_name { get; set; }

            [DisplayName("規格")]
            public string pro_spc { get; set; }

            [DisplayName("績效標準")]
            public decimal std_qty { get; set; }

            [DisplayName("實際產量")]
            public decimal rel_qty { get; set; }

            [DisplayName("效率")]
            public string produce_eff { get; set; }

            [DisplayName("開始時間")]
            public string time_s { get; set; }

            [DisplayName("結束時間")]
            public string time_e { get; set; }

            [DisplayName("休息時間(分)")]
            public decimal rest_min { get; set; }

            [DisplayName("扣除工時(分)")]
            public decimal dis_min { get; set; }

            [DisplayName("績效工時")]
            public decimal pfm_time { get; set; }

            [DisplayName("備註")]
            public string memo { get; set; }
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
            List<RPT130A> result = new List<RPT130A>();

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


        public List<RPT130A> Get_StatData(JqGridQueryData query_data)
        {
            List<RPT130A> result = new List<RPT130A>();
            
            string sMoStartDate_S = query_data.find("mo_start_date", "S");
            string sMoStartDate_E = query_data.find("mo_start_date", "E");
            string sLineCode = query_data.find("line_code");
            //string sProCode_S = query_data.find("pro_code", "S");
            //string sProCode_E = query_data.find("pro_code", "E");
            string sSql = "";
            string sSubWhere = "";
            DataTable dtTmp = comm.Get_DataTable(sSql);

            if (!string.IsNullOrEmpty(sMoStartDate_S)) { sSubWhere += " AND MET01_0000.mo_start_date >='" + sMoStartDate_S + "'"; }
            if (!string.IsNullOrEmpty(sMoStartDate_E)) { sSubWhere += " AND MET01_0000.mo_start_date <='" + sMoStartDate_E + "'"; }
            if (!string.IsNullOrEmpty(sLineCode)) { sSubWhere += " AND MET01_0000.plan_line_code='" + sLineCode + "'"; }
            //if (!string.IsNullOrEmpty(sProCode_S)) { sSubWhere += " AND MET01_0000.pro_code >='" + sProCode_S + "'"; }
            //if (!string.IsNullOrEmpty(sProCode_E)) { sSubWhere += " AND MET01_0000.pro_code <='" + sProCode_E + "'"; }

            sSubWhere += " and pro_spc <> ''";

            sSql = "select MET01_0000.mo_start_date,line_name,pro_spc,isnull(sum(MED09_0000.pro_qty),0) as pro_qty" +
                   "  from MET01_0000" +
                   "  left join SAPCOST_0100 on MET01_0000.pro_code = SAPCOST_0100.pro_code " +
                   "  left join MED09_0000 on MET01_0000.mo_code = MED09_0000.mo_code " +
                   "  left join MEB12_0000 on MEB12_0000.line_code = MET01_0000.plan_line_code " +
                   " where 1=1 " + sSubWhere +
                   " group by MET01_0000.mo_start_date,line_name,pro_spc";            

            dtTmp = comm.Get_DataTable(sSql);
            comm.Ins_BDP20_0000("admin", "RPT130A", "RPT", sSql);           
            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                DataRow r = dtTmp.Rows[i];
                RPT130A data = new RPT130A();
                string sProSpc = r["pro_spc"].ToString();

                data.date = "";
                if (CD.IsDate(r["mo_start_date"].ToString())) {
                    DateTime InsDate = DateTime.Parse(r["mo_start_date"].ToString());
                    data.date = InsDate.ToString("yyyy年MM月dd日");

                    data.line_name = r["line_name"].ToString();
                    data.pro_spc = sProSpc;                   
                    data.rel_qty = comm.sGetDecimal(r["pro_qty"].ToString());
                    data.rest_min = 0;
                  
                    string sInsDate = InsDate.ToString("yyyy/MM/dd");
                    DateTime TimeS = Get_WorkDateTimeByDate(sInsDate, sProSpc, "S");
                    DateTime TimeE = Get_WorkDateTimeByDate(sInsDate, sProSpc, "E");

                    data.time_s = TimeS.ToString("HH:mm");
                    data.time_e = TimeE.ToString("HH:mm");

                    DateTime Noon = DateTime.Parse(TimeS.ToString("yyyy/MM/dd") + " 12:00:00");
                    if (DateTime.Compare(TimeS, Noon) < 0 && DateTime.Compare(Noon, TimeE) < 0)
                    {
                        data.rest_min = 15;
                    }

                    data.dis_min = Get_ExceptCost(sInsDate, sProSpc);
                    
                    data.pfm_time = 0;
                    //如果結束時間比開始時間晚才計算
                    if (DateTime.Compare(TimeE, TimeS) > 0) {
                        TimeSpan ts = new TimeSpan(TimeE.Ticks - TimeS.Ticks);

                        data.pfm_time = Math.Round(((decimal)ts.TotalMinutes - data.dis_min - data.rest_min) / 60, 2);
                    }

                    decimal dPerfStdCost = comm.sGetDecimal(comm.Get_Data("SAPCOST_0000", sProSpc, "pro_spc", "perf_stdcost"));
                    data.std_qty = data.pfm_time * dPerfStdCost;

                    data.produce_eff = "0%";
                    if (data.std_qty != 0)
                    {
                        data.produce_eff = (Math.Round(data.rel_qty / data.std_qty, 4) * 100).ToString("G29") + "%";
                    }

                    data.memo = Get_ExceptMemo(sInsDate, sProSpc);

                    result.Add(data);
                }               
            }
            return result;
        }


        public DateTime  Get_WorkDateTimeByDate(string pDate,string pProSpc,string pDateTimeType) {
            DateTime val = new DateTime();

            string sTimeCode = "";
            string sAsc = "";

            switch (pDateTimeType) {
                case "S":
                    sTimeCode = "work_time_s";
                    break;
                case "E":
                    sTimeCode = "work_time_e";
                    sAsc = " desc ";
                    break;
            }

            string sSql = "select top 1 " + sTimeCode +
                          "  from MET01_0000" +
                          "  left join SAPCOST_0100 on MET01_0000.pro_code = SAPCOST_0100.pro_code " +
                          "  left join MEM01_0000 on MET01_0000.mo_code = MEM01_0000.mo_code " +
                          " where mo_start_date = '"+ pDate + "'" +
                          "   and SAPCOST_0100.pro_spc = '"+ pProSpc + "'" +
                          "   and "+ sTimeCode + " is not null " +
                          "   and " + sTimeCode + " <> '1900-01-01 00:00:00.000'" +
                          " order by " + sTimeCode + sAsc;

            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0) {
                DataRow r = dtTmp.Rows[0];
                if (CD.IsDate(r[sTimeCode].ToString())) {
                    val = DateTime.Parse(r[sTimeCode].ToString());
                }
            }
            return val;
        }


        public decimal Get_ExceptCost(string pDate, string pProSpc) {
            decimal val = 0;
            string sSql = "select isnull(sum(except_cost),0) as except_cost" +
                          "  from MED05_0000" +
                          "  left join MET01_0000 on MET01_0000.mo_code = MED05_0000.mo_code" +
                          "  left join MEB46_0000 on MED05_0000.except_code = MEB46_0000.except_code" +
                          "  left join SAPCOST_0100 on MET01_0000.pro_code = SAPCOST_0100.pro_code" +
                          " where mo_start_date = '"+ pDate + "'" +
                          "   and SAPCOST_0100.pro_spc = '"+ pProSpc + "'";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                val = comm.sGetDecimal(dtTmp.Rows[0][0].ToString());
            }
            return val;
        }

        public string Get_ExceptMemo(string pDate, string pProSpc)
        {
            string val = "";
            string sSql = "select MED05_0000.except_code, except_name,count(*) as cnt" +
                          "  from MED05_0000" +
                          "  left join MET01_0000 on MET01_0000.mo_code = MED05_0000.mo_code" +
                          "  left join MEB46_0000 on MED05_0000.except_code = MEB46_0000.except_code" +
                          "  left join SAPCOST_0100 on MET01_0000.pro_code = SAPCOST_0100.pro_code" +
                          " where mo_start_date = '" + pDate + "'" +
                          "   and SAPCOST_0100.pro_spc = '" + pProSpc + "'" +
                          " group by MED05_0000.except_code,except_name " +
                          " order by MED05_0000.except_code";
            var dtTmp = comm.Get_DataTable(sSql);
            for (int i = 0; i < dtTmp.Rows.Count; i++) {
                DataRow r = dtTmp.Rows[i];
                if (val != "") { val += "<br/>"; }

                val += r["except_name"].ToString() + " * " + r["cnt"].ToString(); ;
            }            
            return val;
        }


    }
}