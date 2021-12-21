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
    public class RPT160AController : Controller
    {
        string sPrgCode = "RPT160A";
        // 共用函數庫
        Comm comm = new Comm();
        RPT16_0000Repository repoRPT160 = new RPT16_0000Repository();

        public ActionResult Index()
        {
            return View();
        }

        public class RPT160A
        {
            //[DisplayName("線別")]
            //public string line_code { get; set; }

            [DisplayName("工單代號")]
            public string wrk_code { get; set; }

            [DisplayName("產品編號")]
            public string pro_code { get; set; }

            [DisplayName("產品名稱")]
            public string pro_name { get; set; }

            [DisplayName("製程代號")]
            public string work_code { get; set; }

            [DisplayName("製程名稱")]
            public string work_name { get; set; }

            [DisplayName("計畫數量")]
            public string plan_qty { get; set; }

            [DisplayName("良品量")]
            public string ok_qty { get; set; }

            [DisplayName("不良品量")]
            public string ng_qty { get; set; }

            [DisplayName("排程生產日期")]
            public string sch_date_s { get; set; }

            [DisplayName("排程生產時間")]
            public string sch_time_s { get; set; }
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
            List<RPT160A> result = new List<RPT160A>();

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

        /// <summary>
        /// (固定區)主檔 首頁 按下查詢按鈕 JqGrid資料來源
        /// </summary>
        /// <param name="pWhere">使用者下的查詢條件 Json</param>
        /// <returns></returns>
        public ActionResult Get_GridDataByQuery(string pWhere)
        {
            string sUsrCode = User.Identity.Name;
            //string sPrgCode = sPrgCode;
            List<RPT16_0000> list = new List<RPT16_0000>();
            list = repoRPT160.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public List<RPT160A> Get_RptData(JqGridQueryData query_data)
        {
            List<RPT160A> result = new List<RPT160A>();
            
            string sDate = query_data.find("mo_start_date");
            string sLineCode = query_data.find("line_code");
            string sSql = "";
            int i;
            DataTable dtTmp = comm.Get_DataTable(sSql);

            //抓取已安排生產的所有工單
            sSql = "select MET01_0000.*,MEB20_0000.pro_name,MEM01_0000.ok_qty,MEM01_0000.ng_qty,MEM01_0000.work_code " +
                   "  from MET01_0000 " +
                   "  left join MEB20_0000 on MET01_0000.pro_code=MEB20_0000.pro_code " +
                   "  left join MEM01_0000 on MEM01_0000.mo_code=MET01_0000.mo_code " +
                   " where sch_date_s ='" + sDate + "'";
            if (!string.IsNullOrEmpty(sLineCode)) { sSql += " AND plan_line_code='" + sLineCode + "'"; }
            dtTmp = comm.Get_DataTable(sSql);
            //comm.Ins_BDP20_0000("admin", "RPT160A", "RPT", sSql);

            for (i = 0; i < dtTmp.Rows.Count; i++)
            {
                RPT160A data = new RPT160A();
                //data.line_code = dtTmp.Rows[i]["plan_line_code"].ToString();
                data.wrk_code = dtTmp.Rows[i]["mo_code"].ToString();
                data.pro_code = dtTmp.Rows[i]["pro_code"].ToString();
                data.pro_name = dtTmp.Rows[i]["pro_name"].ToString();
                data.work_code = dtTmp.Rows[i]["work_code"].ToString();
                data.work_name = comm.Get_QueryData("MEB30_0000", data.work_code, "work_code", "work_name");
                data.plan_qty = dtTmp.Rows[i]["plan_qty"].ToString();
                data.ok_qty = dtTmp.Rows[i]["ok_qty"].ToString();
                data.ng_qty = dtTmp.Rows[i]["ng_qty"].ToString();

                //data.ok_qty = Get_OKQty(dtTmp.Rows[i]["mo_code"].ToString());
                //data.ng_qty = Get_NGQty(dtTmp.Rows[i]["mo_code"].ToString());
                data.sch_date_s = dtTmp.Rows[i]["sch_date_s"].ToString();
                data.sch_time_s = dtTmp.Rows[i]["sch_time_s"].ToString();
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