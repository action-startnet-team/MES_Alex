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
    public class RPT004AController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();

        public ActionResult Index()
        {
            return View();
        }

        public class RPT004A
        {
            [DisplayName("客戶代碼")]
            public string cus_code { get; set; }

            [DisplayName("客戶名稱")]
            public string cus_name { get; set; }

            [DisplayName("客戶訂單")]
            public string ord_code { get; set; }

            [DisplayName("產品代碼")]
            public string pro_code { get; set; }

            [DisplayName("產品名稱")]
            public string pro_name { get; set; }

            [DisplayName("製令號碼")]
            public string mo_code { get; set; }

            [DisplayName("計畫產量")]
            public string plan_qty { get; set; }

            [DisplayName("單位")]
            public string pro_unit { get; set; }

            [DisplayName("達成率")]
            public string  ng_rate { get; set; }

            [DisplayName("計畫出貨日")]
            public string plan_out_date { get; set; }


            [DisplayName("製令備註")]
            public string mo_memo { get; set; }


            [DisplayName("良品")]
            //090
            public string mo_qty { get; set; }
            [DisplayName("不良品")]
            //030
            public string ng_qty { get; set; }

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
            List<RPT004A> result = new List<RPT004A>();

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


        public List<RPT004A> Get_StatData(JqGridQueryData query_data)
        {
            List<RPT004A> result = new List<RPT004A>();

            string out_date_s = query_data.find("plan_out_date_s", "S"); //開始日期
            string out_date_e = query_data.find("plan_out_date_s", "E");
            string sPro_code = query_data.find("pro_code"); //產品編號 
            string sMo_code = query_data.find("mo_code"); //製令號碼 

            //string sSql = "select MET01_0000.*, MEB20_0000.pro_name, MEB25_0000.cus_name " +
            //              "from MET01_0000 " +
            //              "left join MEB20_0000 on MET01_0000.pro_code = MEB20_0000.pro_code " +
            //              "left join MEB25_0000 on MET01_0000.cus_code = MEB25_0000.cus_code " +
            //              "where 1=1 ";
            //if (sSch_date_s != "")
            //    sSql += "and MET01_0000.sch_date_s = '" + sSch_date_s + "'";
            string sSql = @"
                    select MEB25_0000.cus_code,MEB25_0000.cus_name,MEB20_0000.pro_name,MET01_0000.*
                    from MET01_0000 
                    left join MEB20_0000 on MET01_0000.pro_code = MEB20_0000.pro_code 
                    left join MEB25_0000 on MET01_0000.cus_code = MEB25_0000.cus_code 
                    where 1=1     ";

            if (out_date_s != "")
                sSql += "and plan_out_date between '" + out_date_s + "' and '" + out_date_e + "'";
            if (sPro_code != "")
                sSql += "and MET01_0000.pro_code like '%" + sPro_code + "%'";
            if (sMo_code != "")
                sSql += "and MET01_0000.mo_code like '%" + sMo_code + "%'";
            DataTable dtTmp = comm.Get_DataTable(sSql);
            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                string mo_code = dtTmp.Rows[i]["mo_code"].ToString();  
                string mo_qty = dtTmp.Rows[i]["mo_qty"].ToString();
                //string plan_qty = dtTmp.Rows[i]["plan_qty"].ToString();

                //最後一個製程已完工的才計不良量
                //sSql = "select TOP 1 (ng_qty) as ng_qty " +
                //       "from MED03_0000 " +
                //       "where MED03_0000.mo_code = '" + mo_code + "' and MED03_0000.mo_code not in(select mo_code from MET03_0000 where mo_code='" + mo_code + "' and mo_status<>'IN')" +
                //       " order by wrk_code DESC";              
                //sSql = "select TOP 1 (ng_qty) as ng_qty " +
                //       "from MED03_0000 " +
                //       "where MED03_0000.mo_code = '" + mo_code + "' ";
                sSql = @" select ISNULL(sum(ng_qty),0) as tng_qty from med03_0000 
                        where wrk_code in(select TOP 1 wrk_code from MET03_0000 where mo_code = '" + mo_code + "' order by wrk_code desc)";
                DataTable dtSrc = comm.Get_DataTable(sSql);
                string ng_qty = dtSrc != null ? dtSrc.Rows[0]["tng_qty"].ToString() : "0";

                sSql = @"select ISNULL(sum(pro_qty),0) as tmo_qty from med09_0000 
                        where wrk_code in(select TOP 1 wrk_code from MET03_0000 where mo_code='" + mo_code + "'  order by wrk_code desc)";
                DataTable dtSrc_qty = comm.Get_DataTable(sSql);
                string ok_qty = dtSrc_qty != null ? dtSrc_qty.Rows[0]["tmo_qty"].ToString() : "0";
                sSql = @"
                    SELECT CONVERT(int,MET01_0000.plan_qty) as tmo_qty FROM MET01_0000
                    WHERE mo_code='" + mo_code + "' ";
                dtSrc_qty = comm.Get_DataTable(sSql);
                string plan_qty = dtSrc_qty != null ? dtSrc_qty.Rows[0]["tmo_qty"].ToString() : "0";

                float iOk_qty = comm.sGetfloat(ok_qty);
                float iPlan_qty = comm.sGetfloat(plan_qty);
                float ing_rate=0;
                if (iPlan_qty !=0 && iOk_qty != 0)
                    ing_rate = (iOk_qty / iPlan_qty )*100;
                else ing_rate = 0;
                RPT004A data = new RPT004A();

                data.cus_code = dtTmp.Rows[i]["cus_code"].ToString();
                data.cus_name = dtTmp.Rows[i]["cus_name"].ToString();
                data.ord_code = dtTmp.Rows[i]["ord_code"].ToString();
                data.pro_code = dtTmp.Rows[i]["pro_code"].ToString();
                data.pro_name = dtTmp.Rows[i]["pro_name"].ToString();
                data.mo_code = mo_code;
                data.plan_qty = dtTmp.Rows[i]["plan_qty"].ToString();
                data.pro_unit = dtTmp.Rows[i]["pro_unit"].ToString();
                data.ng_rate = ing_rate.ToString() +"%";
                data.plan_out_date = dtTmp.Rows[i]["plan_out_date"].ToString();
                data.mo_memo = dtTmp.Rows[i]["mo_memo"].ToString();
                data.mo_qty = ok_qty;//良品
                data.ng_qty = ng_qty;//不良品
                
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
