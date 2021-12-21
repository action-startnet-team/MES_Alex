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
    public class RPT140AController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();

        public ActionResult Index()
        {
            return View();
        }

        public class RPT140A
        {
            [DisplayName("工單號碼")]
            public string mo_code { get; set; }

            [DisplayName("生產日期")]
            public string mo_start_date { get; set; }

            [DisplayName("實際完成日")]
            public string mo_end_date { get; set; }

            [DisplayName("生產版本")]
            public string version { get; set; }

            [DisplayName("成品料號")]
            public string pro_code { get; set; }

            [DisplayName("產品說明")]
            public string pro_name { get; set; }

            [DisplayName("排程數量")]
            public decimal plan_qty { get; set; }

            [DisplayName("實際產量")]
            public decimal mo_qty { get; set; }

            [DisplayName("單位")]
            public string pro_unit { get; set; }

            [DisplayName("用料")]
            public string pro_code2 { get; set; }

            [DisplayName("用料說明")]
            public string pro_name2 { get; set; }

            [DisplayName("需求數量")]
            public decimal pro_qty { get; set; }

            [DisplayName("標準耗用量")]
            public decimal dis_qty { get; set; }

            [DisplayName("實際耗用量")]
            public decimal atl_consm_qty { get; set; }

            [DisplayName("差異數量")]
            public decimal dif_qty { get; set; }

            //[DisplayName("單位")]
            //public string pro_unit2 { get; set; }

            [DisplayName("差異比率")]
            public decimal dif_ratio { get; set; }

            [DisplayName("標準生產")]
            public long std_qty { get; set; }

            [DisplayName("實際生產")]
            public decimal atl_qty { get; set; }

            [DisplayName("製令效率")]
            public decimal mo_eff { get; set; }

            [DisplayName("人時")]
            public int per_time { get; set; }

            //[DisplayName("機時")]
            //public int mac_time { get; set; }


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
            List<RPT140A> result = new List<RPT140A>();

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


        public List<RPT140A> Get_StatData(JqGridQueryData query_data)
        {
            List<RPT140A> result = new List<RPT140A>();

            string sStartDate = query_data.find("start_date", "S");  //生產日期
            string sEndDate = query_data.find("start_date", "E");  //生產日期
            string sMoCode = query_data.find("mo_code");  //工單號碼
            string sProCode = query_data.find("pro_code");  //工單號碼
            string sProCode2 = query_data.find("pro_code2");  //工單號碼
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
                   " WHERE 1=1 " ;
            if (!string.IsNullOrEmpty(sStartDate)) { sSql += " AND MET01_0000.mo_start_date >='" + sStartDate + "'"; }
            if (!string.IsNullOrEmpty(sEndDate)) { sSql += " AND MET01_0000.mo_start_date <='" + sEndDate + "'"; }
            if (!string.IsNullOrEmpty(sMoCode)) { sSql += " AND MET01_0000.mo_code='" + sMoCode + "'"; }
            if (!string.IsNullOrEmpty(sProCode)) { sSql += " AND MET01_0000.pro_code like'%" + sProCode + "%'"; }
            if (!string.IsNullOrEmpty(sProCode2)) { sSql += " AND MET04_0300.pro_code like'%" + sProCode2 + "%'"; }

            dtTmp = comm.Get_DataTable(sSql);
            comm.Ins_BDP20_0000("admin", "RPT140A", "RPT", sSql);
            

            for (i = 0; i < dtTmp.Rows.Count; i++)
            {
                RPT140A data = new RPT140A();
                data.mo_code = dtTmp.Rows[i]["mo_code"].ToString();
                data.mo_start_date = dtTmp.Rows[i]["mo_start_date"].ToString();
                data.mo_end_date = dtTmp.Rows[i]["mo_end_date"].ToString();
                data.version = dtTmp.Rows[i]["version"].ToString();
                data.pro_code = dtTmp.Rows[i]["pro_code"].ToString();
                data.pro_name = dtTmp.Rows[i]["pro_name"].ToString();
                data.plan_qty = comm.sGetDecimal(dtTmp.Rows[i]["plan_qty"].ToString());
                data.mo_qty = comm.sGetDecimal(dtTmp.Rows[i]["mo_qty"].ToString());
                data.pro_unit = dtTmp.Rows[i]["pro_unit"].ToString();
                data.pro_code2 = dtTmp.Rows[i]["pro_code2"].ToString();
                data.pro_name2 = dtTmp.Rows[i]["pro_name2"].ToString();
                data.pro_qty = comm.Get_QueryData<decimal>("MET01_0100", "where mo_code = '"+ data.mo_code + "' AND pro_code = '"+data.pro_code2+"'", "pro_qty");
                data.dis_qty = comm.Get_QueryData<decimal>("MET01_0100", "where mo_code = '" + data.mo_code + "' AND pro_code = '" + data.pro_code2 + "'", "dis_qty");
                data.atl_consm_qty = comm.sGetDecimal(dtTmp.Rows[i]["atl_consm_qty"].ToString());
                data.dif_qty = data.dis_qty - data.atl_consm_qty;

                if (data.dis_qty == 0)
                {
                    data.dif_ratio = 0;
                }
                else
                {
                    data.dif_ratio = (data.dif_qty / data.dis_qty) * 100;
                }

                data.std_qty = comm.Get_QueryData<long>("MEB12_0100", "where line_code = '" + dtTmp.Rows[i]["plan_line_code"].ToString() + "' AND pro_code = '" + dtTmp.Rows[i]["pro_code"].ToString() + "'", "std_qty");
                
                //至MET04_0100取pro_qty總合-起
                decimal tt_qty = 0;
                sSql = " select * from MET04_0100 where mo_code = @mo_code AND pro_code = @pro_code ";
                Dictionary<string, object> sSqlParams = new Dictionary<string, object>();
                sSqlParams.Add("@mo_code", data.mo_code);
                sSqlParams.Add("@pro_code", dtTmp.Rows[i]["pro_code"].ToString());
                DataTable dt = comm.Get_DataTable(sSql, sSqlParams);
                for (int j = 0; j < dt.Rows.Count;j++)
                {
                    tt_qty += Convert.ToDecimal(dt.Rows[j]["pro_qty"]);
                }
                data.atl_qty = tt_qty;
                //至MET04_0100取pro_qty總合-止

                if (data.atl_qty == 0) data.mo_eff = 0; else data.mo_eff = (data.std_qty / data.atl_qty)*100;
                data.per_time = 0;

                //至MET04_0400取sub_minute總合-起
                int tt_time = 0;
                sSql = " select * from MET04_0400 where mo_code = @mo_code ";
                Dictionary<string, object> sSqlParams2 = new Dictionary<string, object>();
                sSqlParams2.Add("@mo_code", data.mo_code);
                //sSqlParams2.Add("@pro_code", dtTmp.Rows[i]["pro_code"].ToString());
                DataTable dt2 = comm.Get_DataTable(sSql, sSqlParams2);
                for (int j = 0; j < dt2.Rows.Count; j++)
                {
                    tt_time += Convert.ToInt32(dt2.Rows[j]["sub_minute"]);
                }
                data.per_time = tt_time;
                //至MET04_0400取sub_minute總合-止
                //data.per_time = 0;

                result.Add(data);
            }

            return result;
        }

    }
}