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
    public class RPT070AController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();

        public ActionResult Index()
        {
            return View();
        }

        public class RPT070A
        {
            [DisplayName("客戶代碼")]
            public string cus_code { get; set; }

            [DisplayName("訂單編號")]
            public string ord_code { get; set; }

            [DisplayName("訂單數量")]
            public string plan_qty { get; set; }

            [DisplayName("工單編號")]
            public string mo_code { get; set; }

            [DisplayName("產品名稱")]
            public string pro_name { get; set; }

            [DisplayName("工單數量")]
            public string mo_qty { get; set; }

            [DisplayName("工單完成日期")]
            public string mo_start_date { get; set; }
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
            List<RPT070A> result = new List<RPT070A>();

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


        public List<RPT070A> Get_StatData(JqGridQueryData query_data)
        {
            List<RPT070A> result = new List<RPT070A>();

            string sStartDate = query_data.find("mo_start_date", "S");
            string sEndDate = query_data.find("mo_start_date", "E");
            string sLotCode = query_data.find("lot_no");
            string sProCode = query_data.find("pro_code");
            string sSql = "";
            int i;
            DataTable dtTmp = comm.Get_DataTable(sSql);

            //抓取資料
            sSql = " SELECT MET01_0000.*, MEB20_0000.pro_name, MET04_0100.lot_no " +
                   " FROM MET01_0000 " +
                   " LEFT JOIN MEB20_0000 on MEB20_0000.pro_code = MET01_0000.pro_code " +
                   " LEFT JOIN MET04_0100 on MET04_0100.mo_code = MET01_0000.mo_code " +
                   " WHERE 1=1 ";
            if (!string.IsNullOrEmpty(sStartDate)) { sSql += " AND MET01_0000.mo_start_date >='" + sStartDate + "'"; }
            if (!string.IsNullOrEmpty(sEndDate)) { sSql += " AND MET01_0000.mo_start_date <='" + sEndDate + "'"; }
            if (!string.IsNullOrEmpty(sLotCode)) { sSql += " AND MET04_0100.lot_no like '%" + sLotCode + "%'"; }
            if (!string.IsNullOrEmpty(sProCode)) { sSql += " AND MET01_0000.pro_code='" + sProCode + "'"; }

            dtTmp = comm.Get_DataTable(sSql);
            comm.Ins_BDP20_0000("admin", "RPT070A", "RPT", sSql);


            for (i = 0; i < dtTmp.Rows.Count; i++)
            {
                RPT070A data = new RPT070A();
                data.cus_code = dtTmp.Rows[i]["cus_code"].ToString();
                data.ord_code = dtTmp.Rows[i]["ord_code"].ToString();
                data.plan_qty = dtTmp.Rows[i]["plan_qty"].ToString();
                data.mo_code = dtTmp.Rows[i]["mo_code"].ToString();
                data.pro_name = dtTmp.Rows[i]["pro_name"].ToString();
                data.mo_qty = dtTmp.Rows[i]["mo_qty"].ToString();
                data.mo_start_date = dtTmp.Rows[i]["mo_start_date"].ToString();


                result.Add(data);
            }

            return result;
        }

    }
}