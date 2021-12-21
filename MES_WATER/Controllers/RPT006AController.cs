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
    public class RPT006AController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();

        public ActionResult Index()
        {
            return View();
        }

        public class RPT006A
        {
            [DisplayName("項次")]
            public string item { get; set; }

            [DisplayName("代碼")]
            public string ng_code { get; set; }

            [DisplayName("不良現象")]
            public string ng_name { get; set; }

            [DisplayName("不良數目")]
            public string ng_qty { get; set; }

            [DisplayName("不良總數%")]
            public string ng_rate { get; set; }
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
            List<RPT006A> result = new List<RPT006A>();

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

            // 回傳給view
            var returnObj = new
            {
                data = result
            };

            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }


        public List<RPT006A> Get_StatData(JqGridQueryData query_data)
        {
            List<RPT006A> result = new List<RPT006A>();

            string sNg_name = query_data.find("ng_name"); //工作站
            string sMo_code = query_data.find("mo_code"); //製令號碼 
            string sPro_code = query_data.find("pro_code"); //產品編號             
            string sIns_date = query_data.find("ins_date"); //建立日期

            string sSql = @"select a.mo_code, a.ng_code, b.ng_name, a.ng_qty, c.mo_qty
                            from MED03_0000 a
                            left join MEB37_0000 b on a.ng_code = b.ng_code
                            left join MET01_0000 c on a.mo_code = c.mo_code
                            where 1=1";
            if (sNg_name != "")
                sSql += " and b.ng_name = '" + sNg_name + "'";
            if (sIns_date != "")
                sSql += " and a.ins_date = '" + sIns_date + "'";
            if (sPro_code != "")
                sSql += " and a.pro_code = '" + sPro_code + "'";
            if (sMo_code != "")
                sSql += " and a.mo_code = '" + sMo_code + "'";

            DataTable dtTmp = comm.Get_DataTable(sSql);

            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                string mo_code = dtTmp.Rows[i]["mo_code"].ToString();
                string ng_rate = comm.sGetInt32(dtTmp.Rows[i]["mo_qty"].ToString()) != 0 ? (comm.sGetInt32(dtTmp.Rows[i]["ng_qty"].ToString()) / comm.sGetInt32(dtTmp.Rows[i]["mo_qty"].ToString())).ToString() : "0";
                RPT006A data = new RPT006A();

                data.item = (i + 1).ToString();
                data.ng_code = dtTmp.Rows[i]["ng_code"].ToString();
                data.ng_name = dtTmp.Rows[i]["ng_name"].ToString();
                data.ng_qty = dtTmp.Rows[i]["ng_qty"].ToString();
                data.ng_rate = ng_rate + "%";

                result.Add(data);
            }
            return result;
        }

        [HttpPost]
        public JsonResult Get_LotNoList(string pro_code)
        {
            string sSql = "select distinct(lot_no)" +
                         "from MED06_0000 where pro_code = '" + pro_code + "'";
            DataTable dtTmp = comm.Get_DataTable(sSql);

            List<string> result = new List<string>();
            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                result.Add(dtTmp.Rows[i]["lot_no"].ToString());
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
