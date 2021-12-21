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
    public class RPT120AController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();

        public ActionResult Index()
        {
            return View();
        }

        public class RPT120A
        {
            [DisplayName("日期")]
            public string date { get; set; }

            [DisplayName("規格")]
            public string format { get; set; }

            [DisplayName("標準產量")]
            public string std_qty { get; set; }

            [DisplayName("實際產量")]
            public string rel_qty { get; set; }

            [DisplayName("效率")]
            public string produce_eff { get; set; }

            [DisplayName("起訖工時(時)-起")]
            public string hour_s { get; set; }

            [DisplayName("起訖工時(分)-起")]
            public string min_s { get; set; }

            [DisplayName("起訖工時(時)-迄")]
            public string hour_e { get; set; }

            [DisplayName("起訖工時(分)-迄")]
            public string min_e { get; set; }

            [DisplayName("休息時間(分)")]
            public string rest_min { get; set; }

            [DisplayName("扣除工時(分)")]
            public string dis_min { get; set; }

            [DisplayName("績效工時")]
            public string pfm_time { get; set; }

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
            List<RPT120A> result = new List<RPT120A>();

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


        public List<RPT120A> Get_StatData(JqGridQueryData query_data)
        {
            List<RPT120A> result = new List<RPT120A>();

            string sYear = query_data.find("year");
            string sMacCode_S = query_data.find("mac_code", "S");
            string sMacCode_E = query_data.find("mac_code", "E");
            string sDepCode_S = query_data.find("dep_code", "S");
            string sDepCode_E = query_data.find("dep_code", "E");
            string sSql = "";
            int i;
            DataTable dtTmp = comm.Get_DataTable(sSql);

            //抓取資料
            sSql = " SELECT MET01_0000.*, MEB23_0000.version, MEB20_0000.pro_name, MET04_0300.pro_code as pro_code2, a.pro_name as pro_name2, MET04_0300.pro_qty as atl_consm_qty " +
                   " FROM MET01_0000 " +
                   " LEFT JOIN MEB23_0000 on MEB23_0000.bom_code = MET01_0000.bom_code " +
                   " LEFT JOIN MEB20_0000 on MEB20_0000.pro_code = MET01_0000.pro_code " +
                   " LEFT JOIN MET04_0300 on MET04_0300.mo_code = MET01_0000.mo_code " +
                   " LEFT JOIN MEB20_0000 as a on a.pro_code = MET04_0300.pro_code " +
                   " WHERE 1=1 ";
            if (!string.IsNullOrEmpty(sYear)) { sSql += " AND XXXXXX.year='" + sYear + "'"; }
            if (!string.IsNullOrEmpty(sMacCode_S)) { sSql += " AND XXXXXX.mac_code >='" + sMacCode_S + "'"; }
            if (!string.IsNullOrEmpty(sMacCode_E)) { sSql += " AND XXXXXX.mac_code <='" + sMacCode_E + "'"; }
            if (!string.IsNullOrEmpty(sDepCode_S)) { sSql += " AND XXXXXX.dep_code >='" + sDepCode_S + "'"; }
            if (!string.IsNullOrEmpty(sDepCode_E)) { sSql += " AND XXXXXX.dep_code <='" + sDepCode_E + "'"; }

            dtTmp = comm.Get_DataTable(sSql);
            comm.Ins_BDP20_0000("admin", "RPT120A", "RPT", sSql);


            for (i = 0; i < dtTmp.Rows.Count; i++)
            {
                RPT120A data = new RPT120A();
                data.date = dtTmp.Rows[i]["line_code"].ToString();
                data.format = dtTmp.Rows[i]["pro_code"].ToString();
                data.std_qty = dtTmp.Rows[i]["pro_name"].ToString();
                data.rel_qty = dtTmp.Rows[i]["unit"].ToString();
                data.produce_eff = dtTmp.Rows[i]["mo_start_date"].ToString();
                data.hour_s = dtTmp.Rows[i]["mo_code"].ToString();
                data.min_s = dtTmp.Rows[i]["plan_qty"].ToString();
                data.rel_qty = dtTmp.Rows[i]["rel_qty"].ToString();
                data.hour_e = dtTmp.Rows[i]["mo_qty"].ToString();
                data.min_e = dtTmp.Rows[i]["dis_qty"].ToString();
                data.rest_min = dtTmp.Rows[i]["dis_rate"].ToString();
                data.dis_min = dtTmp.Rows[i]["std_dis_qty"].ToString();
                data.pfm_time = dtTmp.Rows[i]["std_dis_rate"].ToString();
                data.memo = dtTmp.Rows[i]["memo"].ToString();
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