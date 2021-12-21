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
    public class RPT100AController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();

        public ActionResult Index()
        {
            return View();
        }

        public class RPT100A
        {
            [DisplayName("線別")]
            public string line_code { get; set; }

            [DisplayName("物料代碼")]
            public string pro_code { get; set; }

            [DisplayName("物料名稱")]
            public string pro_name { get; set; }

            [DisplayName("單位")]
            public string unit { get; set; }

            [DisplayName("生產日期")]
            public string mo_start_date { get; set; }

            [DisplayName("工單號")]
            public string mo_code { get; set; }

            [DisplayName("計畫使用量")]
            public decimal plan_qty { get; set; }

            [DisplayName("實際使用量")]
            public decimal rel_qty { get; set; }

            [DisplayName("生產數量")]
            public decimal mo_qty { get; set; }

            [DisplayName("損耗量")]
            public decimal dis_qty { get; set; }

            [DisplayName("損耗率(%)")]
            public string dis_rate { get; set; }

            [DisplayName("標準損耗量")]
            public decimal std_dis_qty { get; set; }

            [DisplayName("標準損耗率(%)")]
            public string std_dis_rate { get; set; }
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
            List<RPT100A> result = new List<RPT100A>();

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


        public List<RPT100A> Get_StatData(JqGridQueryData query_data)
        {
            List<RPT100A> result = new List<RPT100A>();

            string sStartDate = query_data.find("mo_start_date", "S");
            string sEndDate = query_data.find("mo_start_date", "E");
            string sLineCode = query_data.find("line_code");
            string sProCode_S = query_data.find("pro_code", "S");
            string sProCode_E = query_data.find("pro_code", "E");
            string sMoCode = query_data.find("mo_code");
            string sSql = "";
            DataTable dtTmp = comm.Get_DataTable(sSql);

            //抓取資料
            sSql = "select plan_line_code,MET01_0000.pro_code,pro_name,MET01_0000.pro_unit,mo_start_date,MET01_0000.mo_code,MET01_0000.plan_qty,MED06_0000.pro_qty " +
                   "       ,(select isnull(sum(ok_qty + ng_qty),0) from MEM01_0000 where mo_code = MET01_0000.mo_code) as rel_qty"+
                   "       ,MET01_0100.dis_qty" +
                   "  from MET01_0000 " +
                   "  left join MED06_0000 on MET01_0000.mo_code = MED06_0000.mo_code " +
                   "  left join MEB20_0000 on MEB20_0000.pro_code = MET01_0000.pro_code " +
                   "  left join MET01_0100 on MET01_0000.pro_code = MET01_0100.pro_code " +
                   " where 1=1 ";
            if (!string.IsNullOrEmpty(sStartDate)) { sSql += " AND MET01_0000.mo_start_date >='" + sStartDate + "'"; }
            if (!string.IsNullOrEmpty(sEndDate)) { sSql += " AND MET01_0000.mo_start_date <='" + sEndDate + "'"; }
            if (!string.IsNullOrEmpty(sLineCode)) { sSql += " AND MET01_0000.plan_line_code='" + sLineCode + "'"; }
            if (!string.IsNullOrEmpty(sProCode_S)) { sSql += " AND MET01_0000.pro_code >='" + sProCode_S + "'"; }
            if (!string.IsNullOrEmpty(sProCode_E)) { sSql += " AND MET01_0000.pro_code <='" + sProCode_E + "'"; }
            if (!string.IsNullOrEmpty(sMoCode)) { sSql += " AND MET01_0000.mo_code='" + sMoCode + "'"; }

            dtTmp = comm.Get_DataTable(sSql);
            comm.Ins_BDP20_0000("admin", "RPT100A", "RPT", sSql);
            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                DataRow r = dtTmp.Rows[i];
                RPT100A data = new RPT100A();
                data.line_code = r["plan_line_code"].ToString();
                data.pro_code = r["pro_code"].ToString();
                data.pro_name = r["pro_name"].ToString();
                data.unit = r["pro_unit"].ToString();
                data.mo_start_date = dtTmp.Rows[i]["mo_start_date"].ToString();
                data.mo_code = r["mo_code"].ToString();
                data.plan_qty = comm.sGetDecimal(r["plan_qty"].ToString());
                data.rel_qty = comm.sGetDecimal(r["rel_qty"].ToString());
                data.mo_qty = comm.sGetDecimal(r["pro_qty"].ToString());
                data.std_dis_qty = comm.sGetDecimal(r["dis_qty"].ToString());

                data.dis_qty = data.plan_qty - data.rel_qty;
                if (data.dis_qty < 0) { data.dis_qty = 0; }
               
                data.dis_rate = "0%";
                if (data.rel_qty != 0) 
                    if (data.rel_qty - data.plan_qty > 0) {
                        data.dis_rate = (Math.Round((data.rel_qty - data.plan_qty) / data.rel_qty, 4) * 100).ToString() + "%";
                    }
                                                   
                data.std_dis_rate = "0%";
                if (data.plan_qty != 0) { data.std_dis_rate = (Math.Round(data.std_dis_qty / data.plan_qty, 4) * 100).ToString() + "%"; }
                    
                result.Add(data);
            }

            return result;
        }

    }
}