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
    public class RPT170AController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();

        public ActionResult Index()
        {
            return View();
        }

        public class RPT170A
        {
            [DisplayName("線別代號")]
            public string line_code { get; set; }

            [DisplayName("線別名稱")]
            public string line_name { get; set; }

            [DisplayName("工單代號")]
            public string mo_code { get; set; }

            [DisplayName("製程代號")]
            public string work_code { get; set; }

            [DisplayName("製程名稱")]
            public string work_name { get; set; }

            [DisplayName("上料物料")]
            public string pro_code { get; set; }

            [DisplayName("物料名稱")]
            public string pro_name { get; set; }

            [DisplayName("物料批號")]
            public string lot_no { get; set; }

            [DisplayName("上料數量")]
            public decimal in_qty { get; set; }

            [DisplayName("上料時間")]
            public string in_time { get; set; }

            [DisplayName("退料數量")]
            public decimal out_qty { get; set; }

            [DisplayName("退料時間")]
            public string out_time { get; set; }
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
            List<RPT170A> result = new List<RPT170A>();
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
            //result.AddRange(Get_StatData(query_data));
            ////
            result = Get_RptData(query_data);
            // 回傳給view
            var returnObj = new
            {
                data = result
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }

        public List<RPT170A> Get_RptData(JqGridQueryData query_data)
        {
            List<RPT170A> result = new List<RPT170A>();

            string sDate = query_data.find("in_time");
            string sLineCode = query_data.find("line_code");
            string sSql = "";
            int i;
            DataTable dtTmp = comm.Get_DataTable(sSql);

            //抓取已安排生產的所有工單
            sSql = " select MEM05_0000.*, MET01_0000.plan_line_code, MEB12_0000.line_name, MEB30_0000.work_name, MEB20_0000.pro_name " +
                   " from MEM05_0000 " +
                   " left join MET01_0000 on MET01_0000.mo_code = MEM05_0000.mo_code " +
                   " left join MEB12_0000 on MEB12_0000.line_code = MET01_0000.plan_line_code " +
                   " left join MEB30_0000 on MEB30_0000.work_code = MEM05_0000.work_code " +
                   " left join MEB20_0000 on MEB20_0000.pro_code = MEM05_0000.pro_code " +
                   " where in_time ='" + sDate + "'";
            if (!string.IsNullOrEmpty(sLineCode)) { sSql += " AND plan_line_code='" + sLineCode + "'"; }
            dtTmp = comm.Get_DataTable(sSql);
            comm.Ins_BDP20_0000("admin", "RPT170A", "RPT", sSql);

            for (i = 0; i < dtTmp.Rows.Count; i++)
            {
                RPT170A data = new RPT170A();
                data.line_code = dtTmp.Rows[i]["plan_line_code"].ToString();
                data.line_name = dtTmp.Rows[i]["line_name"].ToString();
                data.mo_code = dtTmp.Rows[i]["mo_code"].ToString();
                data.work_code = dtTmp.Rows[i]["work_code"].ToString();
                data.work_name = dtTmp.Rows[i]["work_name"].ToString();
                data.pro_code = dtTmp.Rows[i]["pro_code"].ToString();
                data.pro_name = dtTmp.Rows[i]["pro_name"].ToString();
                data.lot_no = dtTmp.Rows[i]["lot_no"].ToString();
                data.in_qty = comm.sGetDecimal(dtTmp.Rows[i]["in_qty"].ToString());
                data.in_time = dtTmp.Rows[i]["in_time"].ToString();
                data.out_qty = comm.sGetDecimal(dtTmp.Rows[i]["out_qty"].ToString());
                data.out_time = dtTmp.Rows[i]["out_time"].ToString();
                result.Add(data);
            }

            return result;
        }

        /// <summary>
        /// 取得最後一站裝箱站的數量當做回報數量
        /// </summary>
        /// <param name="pMoCode"></param>
        /// <returns></returns>
        private string Get_OKQty(string pMoCode)
        {
            string sSql = "";
            sSql = "select ok_qty from MEM01_0000 " +
                   " where mo_code='" + pMoCode + "'" +
                   "   and right(work_code,2)='05'";
            DataTable dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                return dtTmp.Rows[0]["ok_qty"].ToString();
            }
            else
            {
                return "0";
            }
        }

        private string Get_NGQty(string pMoCode)
        {
            string sSql = "";
            sSql = "select ng_qty from MEM01_0000 " +
                   " where mo_code='" + pMoCode + "'" +
                   "   and right(work_code,2)='05'";
            DataTable dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                return dtTmp.Rows[0]["ng_qty"].ToString();
            }
            else
            {
                return "0";
            }
        }
    }
}