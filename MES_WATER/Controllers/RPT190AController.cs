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
    public class RPT190AController : Controller
    {
        string sPrgCode = "RPT200A";
        // 共用函數庫
        Comm comm = new Comm();
        RPT19_0000Repository repoRPT190 = new RPT19_0000Repository();


        public ActionResult Index()
        {
            return View();
        }

        public class RPT190A
        {
            [DisplayName("工廠代號")]
            public string factory_code { get; set; }

            [DisplayName("工廠名稱")]
            public string factory_name { get; set; }

            [DisplayName("區域代號")]
            public string area_code { get; set; }

            [DisplayName("區域名稱")]
            public string area_name { get; set; }

            [DisplayName("線別代號")]
            public string line_code { get; set; }

            [DisplayName("線別名稱")]
            public string line_name { get; set; }
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
            List<RPT190A> result = new List<RPT190A>();

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
        /// <summary>
        /// (固定區)主檔 首頁 按下查詢按鈕 JqGrid資料來源
        /// </summary>
        /// <param name="pWhere">使用者下的查詢條件 Json</param>
        /// <returns></returns>
        public ActionResult Get_GridDataByQuery(string pWhere)
        {
            string sUsrCode = User.Identity.Name;
            //string sPrgCode = sPrgCode;

            List<RPT19_0000> list = new List<RPT19_0000>();
            list = repoRPT190.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public List<RPT190A> Get_StatData(JqGridQueryData query_data)
        {
            List<RPT190A> result = new List<RPT190A>();

            string sFactoryCode = query_data.find("factory_code");
            string sSql = "";
            int i;
            DataTable dtTmp = comm.Get_DataTable(sSql);

            //抓取資料
            sSql = " SELECT MEB12_0000.*, MEB11_0000.area_name, MEB11_0000.factory_code, MEB10_0000.factory_name " +
                   " FROM MEB12_0000 " +
                   " LEFT JOIN MEB11_0000 on MEB11_0000.area_code = MEB12_0000.area_code " +
                   " LEFT JOIN MEB10_0000 on MEB10_0000.factory_code = MEB11_0000.factory_code " +
                   " WHERE 1=1 ";
            if (!string.IsNullOrEmpty(sFactoryCode)) { sSql += " AND MEB10_0000.factory_code ='" + sFactoryCode + "'"; }

            dtTmp = comm.Get_DataTable(sSql);
            comm.Ins_BDP20_0000("admin", "RPT190A", "RPT", sSql);


            for (i = 0; i < dtTmp.Rows.Count; i++)
            {
                RPT190A data = new RPT190A();
                data.factory_code = dtTmp.Rows[i]["factory_code"].ToString();
                data.factory_name = dtTmp.Rows[i]["factory_name"].ToString();
                data.area_code = dtTmp.Rows[i]["area_code"].ToString();
                data.area_name = dtTmp.Rows[i]["area_name"].ToString();
                data.line_code = dtTmp.Rows[i]["line_code"].ToString();
                data.line_name = dtTmp.Rows[i]["line_name"].ToString();
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