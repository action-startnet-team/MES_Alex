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
    public class RPT030BController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();
        CheckData CD = new CheckData();

        public ActionResult Index()
        {
            return View();
        }

        public class RPT030B
        {
            [DisplayName("物料代碼")]
            public string pro_code { get; set; }

            [DisplayName("物料名稱")]
            public string pro_name { get; set; }

            [DisplayName("異常項目代碼")]
            public string qtest_item_code { get; set; }

            [DisplayName("異常項目名稱")]
            public string qtest_item_name { get; set; }

            [DisplayName("異常次數")]
            public int ng_times { get; set; }

            [DisplayName("供應商")]
            public string tra_code { get; set; }

            [DisplayName("來料日期")]
            public string sto_date { get; set; }
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
            List<RPT030B> result = new List<RPT030B>();

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


        public List<RPT030B> Get_StatData(JqGridQueryData query_data)
        {
            List<RPT030B> result = new List<RPT030B>();

            string sStartDate = query_data.find("ins_date", "S");
            string sEndDate = query_data.find("ins_date", "E");
            string sSql = "";
            int i;
            DataTable dtTmp = comm.Get_DataTable(sSql);

            //抓取資料
            sSql = " SELECT QMT04_0100.qmt04_0100, QMT04_0100.qtest_item_code, QMT04_0000.pro_code, QMT04_0000.rel_code, WMT0200.sto_date, WMT0200.tra_code, MEB20_0000.pro_name, QMB02_0000.qtest_item_name " +
                   " FROM QMT04_0100 " +
                   " LEFT JOIN QMT04_0000 on QMT04_0000.qmt_code = QMT04_0100.qmt_code " +
                   " LEFT JOIN WMT0200 on WMT0200.rel_code = QMT04_0000.rel_code " +
                   " LEFT JOIN MEB20_0000 on MEB20_0000.pro_code = QMT04_0000.pro_code " +
                   " LEFT JOIN QMB02_0000 on QMB02_0000.qtest_item_code = QMT04_0100.qtest_item_code " +
                   " WHERE 1=1 ";
            if (!string.IsNullOrEmpty(sStartDate)) { sSql += " AND WMT0200.sto_date >='" + sStartDate + "'"; }
            if (!string.IsNullOrEmpty(sEndDate)) { sSql += " AND WMT0200.sto_date <='" + sEndDate + "'"; }

            dtTmp = comm.Get_DataTable(sSql);
            comm.Ins_BDP20_0000("admin", "RPT030B", "RPT", sSql);


            for (i = 0; i < dtTmp.Rows.Count; i++)
            {
                RPT030B data = new RPT030B();
                data.pro_code = dtTmp.Rows[i]["pro_code"].ToString();
                data.pro_name = dtTmp.Rows[i]["pro_name"].ToString();
                data.qtest_item_code = dtTmp.Rows[i]["qtest_item_code"].ToString();
                data.qtest_item_name = dtTmp.Rows[i]["qtest_item_name"].ToString();
                data.ng_times = 0;
                DataTable dt = new DataTable();
                if (!string.IsNullOrEmpty(data.qtest_item_code) && !string.IsNullOrEmpty(dtTmp.Rows[i]["qmt04_0100"].ToString()))
                {
                    dt = comm.Get_DataTable(" select * " +
                                            " from QMT04_0110 " +
                                            " where qmt04_0100 = '" + dtTmp.Rows[i]["qmt04_0100"].ToString() + "'" +
                                            " and qtest_item_code = '" + data.qtest_item_code + "'");
                    data.ng_times = dt.Rows.Count;
                }
                else
                    data.ng_times = 0;
                data.tra_code = dtTmp.Rows[i]["tra_code"].ToString();
                data.sto_date = dtTmp.Rows[i]["sto_date"].ToString();
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