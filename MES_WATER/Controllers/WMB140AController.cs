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
    public class WMB140AController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();

        public ActionResult Index()
        {
            return View();
        }

        public class WMB140A
        {
            [DisplayName("品號")]
            public string pro_code { get; set; }

            [DisplayName("品名")]
            public string pro_name { get; set; }

            [DisplayName("批號")]
            public string lot_no { get; set; }

            [DisplayName("儲位")]
            public string loc_code { get; set; }

            [DisplayName("儲位名")]
            public string loc_name { get; set; }

            [DisplayName("數量")]
            public double pro_qty { get; set; }

            [DisplayName("結存數量")]
            public double sto_qty { get; set; }

            [DisplayName("異動別")]
            public string ins_type { get; set; }

            [DisplayName("異動日期")]
            public string ins_date { get; set; }


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
            List<WMB140A> result = new List<WMB140A>();

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


        public List<WMB140A> Get_StatData(JqGridQueryData query_data)
        {
            List<WMB140A> result = new List<WMB140A>();

            //string sCaldateS = query_data.GetParaConds("cal_date")[0].field_value; //統計開始日期
            ////string sCaldateE = query_data.GetParaConds("cal_date")[1].field_value; //統計結束日期
            //string sProCode = query_data.GetParaConds("pro_code")[0].field_value; //產品編號
            //string sLotno = query_data.GetParaConds("lot_no")[0].field_value; //批號
            //string sLocCode = query_data.GetParaConds("loc_code")[0].field_value; //儲位

            string sCaldateS = query_data.find("cal_date"); //統計開始日期
            //string sCaldateE = query_data.GetParaConds("cal_date")[1].field_value; //統計結束日期
            string sProCode = query_data.find("pro_code"); //產品編號
            string sLotno = query_data.find("lot_no"); //批號
            string sLocCode = query_data.find("loc_code"); //儲位

            //開始抓取資料
            double dFirstQty = 0;
            double iQty = 0;
            string sSql = "";

            sSql = "SELECT SUM(CASE WHEN ins_type='I' THEN pro_qty ELSE pro_qty*-1 END) AS first_qty" +
                   "  FROM WMT0200 " +
                   " WHERE convert(nvarchar,ins_date,111)<='" + sCaldateS + "'" +
                   "   AND pro_code='" + sProCode + "'";
            if (!string.IsNullOrEmpty(sLotno)) { sSql += " AND lot_no='" + sLotno + "'"; }
            if (!string.IsNullOrEmpty(sLocCode)) { sSql += " AND loc_code='" + sLocCode + "'"; }
            DataTable dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                dFirstQty = comm.sGetfloat(dtTmp.Rows[i]["first_qty"].ToString());
            }

            //抓取入出庫記錄內容
            sSql = "SELECT a.*,b.pro_name FROM WMT0200 a" +
                   "  LEFT JOIN MEB20_0000 b ON a.pro_code=b.pro_code" +
                   " WHERE convert(nvarchar,a.ins_date,111) <='" + sCaldateS + "'" +
                   "   AND a.pro_code='" + sProCode + "' AND a.pro_qty<>0";
            if (!string.IsNullOrEmpty(sLotno)) { sSql += " AND lot_no='" + sLotno + "'"; }
            if (!string.IsNullOrEmpty(sLocCode)) { sSql += " AND loc_code='" + sLocCode + "'"; }
            sSql += " ORDER BY a.ins_date,a.rel_code ";

            comm.Ins_BDP20_0000("admin", "WMB140A", "RPT", sSql);

            dtTmp = comm.Get_DataTable(sSql);
            for (i = 0; i < dtTmp.Rows.Count; i++)
            {
                WMB140A data = new WMB140A();
                data.pro_code = dtTmp.Rows[i]["pro_code"].ToString();
                data.pro_name = dtTmp.Rows[i]["pro_name"].ToString();
                data.lot_no = dtTmp.Rows[i]["lot_no"].ToString();
                data.loc_code = dtTmp.Rows[i]["loc_code"].ToString();
                data.loc_name = comm.Get_QueryData("WMB02_0000", data.loc_code, "loc_code", "loc_name");
                data.ins_type = dtTmp.Rows[i]["ins_type"].ToString();
                data.ins_date = dtTmp.Rows[i]["ins_date"].ToString();
                data.pro_qty = comm.sGetfloat(dtTmp.Rows[i]["pro_qty"].ToString());

                switch (data.ins_type)
                {
                    case "I":
                        data.ins_type = "入庫";
                        iQty += data.pro_qty;
                        break;
                    case "O":
                        data.ins_type = "出庫";
                        data.pro_qty = data.pro_qty * -1;
                        iQty += data.pro_qty;
                        break;
                    default:
                        data.ins_type = "未知";
                        break;
                }

                data.sto_qty = iQty + dFirstQty;
                result.Add(data);
            }

            return result;
        }

    }
}