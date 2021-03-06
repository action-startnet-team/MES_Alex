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
    public class RPT030AController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();

        public ActionResult Index()
        {
            return View();
        }

        public class RPT030A
        {
            [DisplayName("線別")]
            public string line_code { get; set; }

            [DisplayName("線別名稱")]
            public string line_name { get; set; }

            [DisplayName("設備")]
            public string mac_code { get; set; }

            [DisplayName("設備名稱")]
            public string mac_name { get; set; }

            [DisplayName("日期")]
            public string ins_date { get; set; }

            [DisplayName("不良現象")]
            public string ng_code { get; set; }

            [DisplayName("不良現象名稱")]
            public string ng_name { get; set; }

            [DisplayName("不良數")]
            public decimal ng_qty { get; set; }
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
            List<RPT030A> result = new List<RPT030A>();

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


        public List<RPT030A> Get_StatData(JqGridQueryData query_data)
        {
            List<RPT030A> result = new List<RPT030A>();

            string sStartDate = query_data.find("ins_date", "S");
            string sEndDate = query_data.find("ins_date", "E");
            string sLineCode = query_data.find("line_code");
            string sMacCode_S = query_data.find("mac_code", "S");
            string sMacCode_E = query_data.find("mac_code", "E");
            string sSql = "";
            int i;
            DataTable dtTmp = comm.Get_DataTable(sSql);

            //抓取資料
            sSql = " SELECT MED03_0000.*, MET01_0000.plan_line_code, MEB12_0000.line_name, MEB15_0000.mac_name, MEB37_0000.ng_name " +
                   " FROM MED03_0000 " +
                   " LEFT JOIN MET01_0000 on MET01_0000.mo_code = MED03_0000.mo_code " +
                   " LEFT JOIN MEB12_0000 on MEB12_0000.line_code = MET01_0000.plan_line_code " +
                   " LEFT JOIN MEB15_0000 on MEB15_0000.mac_code = MED03_0000.mac_code " +
                   " LEFT JOIN MEB37_0000 on MEB37_0000.ng_code = MED03_0000.ng_code " +
                   " WHERE 1=1 ";
            if (!string.IsNullOrEmpty(sStartDate)) { sSql += " AND MED03_0000.ins_date >='" + sStartDate + "'"; }
            if (!string.IsNullOrEmpty(sEndDate)) { sSql += " AND MED03_0000.ins_date <='" + sEndDate + "'"; }
            if (!string.IsNullOrEmpty(sLineCode)) { sSql += " AND MET01_0000.plan_line_code = '" + sLineCode + "'"; }
            if (!string.IsNullOrEmpty(sMacCode_S)) { sSql += " AND MED03_0000.mac_code >='" + sMacCode_S + "'"; }
            if (!string.IsNullOrEmpty(sMacCode_E)) { sSql += " AND MED03_0000.mac_code <='" + sMacCode_E + "'"; }

            dtTmp = comm.Get_DataTable(sSql);
            comm.Ins_BDP20_0000("admin", "RPT030A", "RPT", sSql);


            for (i = 0; i < dtTmp.Rows.Count; i++)
            {
                RPT030A data = new RPT030A();
                data.line_code = dtTmp.Rows[i]["plan_line_code"].ToString();
                data.line_name = dtTmp.Rows[i]["line_name"].ToString();
                data.mac_code = dtTmp.Rows[i]["mac_code"].ToString();
                data.mac_name = dtTmp.Rows[i]["mac_name"].ToString();
                data.ins_date = dtTmp.Rows[i]["ins_date"].ToString();
                data.ng_code = dtTmp.Rows[i]["ng_code"].ToString();
                data.ng_name = dtTmp.Rows[i]["ng_name"].ToString();
                data.ng_qty = comm.sGetDecimal(dtTmp.Rows[i]["ng_qty"].ToString());
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