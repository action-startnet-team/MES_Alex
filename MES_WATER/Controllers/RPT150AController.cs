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
    public class RPT150AController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();

        public ActionResult Index()
        {
            return View();
        }

        public class RPT150A
        {
            [DisplayName("品名")]
            public string pro_code { get; set; }

            [DisplayName("通路")]
            public string distributor { get; set; }

            [DisplayName("期初量")]
            public string begin_qty { get; set; }

            [DisplayName("入庫量")]
            public string in_qty { get; set; }

            [DisplayName("出庫量")]
            public string out_qty { get; set; }

            [DisplayName("庫存量")]
            public string pro_qty { get; set; }

            [DisplayName("工廠庫存")]
            public string fac_qty { get; set; }
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
            List<RPT150A> result = new List<RPT150A>();

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


        public List<RPT150A> Get_StatData(JqGridQueryData query_data)
        {
            List<RPT150A> result = new List<RPT150A>();

            string sDate_S = query_data.find("date", "S");
            string sDate_E = query_data.find("date", "E");
            string sProCode_S = query_data.find("pro_code", "S");
            string sProCode_E = query_data.find("pro_code", "E");
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
            if (!string.IsNullOrEmpty(sDate_S)) { sSql += " AND XXXXXX.mo_start_date >='" + sDate_S + "'"; }
            if (!string.IsNullOrEmpty(sDate_E)) { sSql += " AND XXXXXX.mo_start_date <='" + sDate_E + "'"; }
            if (!string.IsNullOrEmpty(sProCode_S)) { sSql += " AND XXXXXX.pro_code >='" + sProCode_S + "'"; }
            if (!string.IsNullOrEmpty(sProCode_E)) { sSql += " AND XXXXXX.pro_code <='" + sProCode_E + "'"; }

            dtTmp = comm.Get_DataTable(sSql);
            comm.Ins_BDP20_0000("admin", "RPT150A", "RPT", sSql);


            for (i = 0; i < dtTmp.Rows.Count; i++)
            {
                RPT150A data = new RPT150A();
                data.pro_code = dtTmp.Rows[i]["pro_code"].ToString();
                data.distributor = dtTmp.Rows[i]["distributor"].ToString();
                data.begin_qty = dtTmp.Rows[i]["begin_qty"].ToString();
                data.in_qty = dtTmp.Rows[i]["in_qty"].ToString();
                data.out_qty = dtTmp.Rows[i]["out_qty"].ToString();
                data.pro_qty = dtTmp.Rows[i]["pro_qty"].ToString();
                data.fac_qty = dtTmp.Rows[i]["fac_qty"].ToString();
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