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
    public class RPT200AController : Controller
    {
        string sPrgCode = "RPT200A";
        // 共用函數庫
        Comm comm = new Comm();
        RPT20_0000Repository repoRPT200 = new RPT20_0000Repository();

        public ActionResult Index()
        {
            return View();
        }

        public class RPT200A
        {
            [DisplayName("製程代碼")]
            public string work_code { get; set; }

            [DisplayName("製程名稱")]
            public string work_name { get; set; }

            [DisplayName("站別代碼")]
            public string station_code { get; set; }

            [DisplayName("站別名稱")]
            public string station_name { get; set; }

            [DisplayName("機器代碼")]
            public string mac_code { get; set; }

            [DisplayName("機器名稱")]
            public string mac_name { get; set; }
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
            List<RPT200A> result = new List<RPT200A>();

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

            List<RPT20_0000> list = new List<RPT20_0000>();
            list = repoRPT200.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public List<RPT200A> Get_StatData(JqGridQueryData query_data)
        {
            List<RPT200A> result = new List<RPT200A>();

            string sWorkCode = query_data.find("work_code");
            string sSql = "";
            int i;
            DataTable dtTmp = comm.Get_DataTable(sSql);

            //抓取資料
            sSql = " SELECT MEB15_0000.*, MEB29_0200.station_code, MEB29_0000.station_name, MEB30_0100.work_code, MEB30_0000.work_name " +
                   " FROM MEB15_0000 " +
                   " LEFT JOIN MEB29_0200 on MEB29_0200.mac_code = MEB15_0000.mac_code " +
                   " LEFT JOIN MEB29_0000 on MEB29_0000.station_code = MEB29_0200.station_code " +
                   " LEFT JOIN MEB30_0100 on MEB30_0100.station_code = MEB29_0200.station_code " +
                   " LEFT JOIN MEB30_0000 on MEB30_0000.work_code = MEB30_0100.work_code " +
                   " WHERE 1=1 ";
            if (!string.IsNullOrEmpty(sWorkCode)) { sSql += " AND MEB30_0100.work_code ='" + sWorkCode + "'"; }

            dtTmp = comm.Get_DataTable(sSql);
            comm.Ins_BDP20_0000("admin", "RPT200A", "RPT", sSql);


            for (i = 0; i < dtTmp.Rows.Count; i++)
            {
                RPT200A data = new RPT200A();
                data.work_code = dtTmp.Rows[i]["work_code"].ToString();
                data.work_name = dtTmp.Rows[i]["work_name"].ToString();
                data.station_code = dtTmp.Rows[i]["station_code"].ToString();
                data.station_name = dtTmp.Rows[i]["station_name"].ToString();
                data.mac_code = dtTmp.Rows[i]["mac_code"].ToString();
                data.mac_name = dtTmp.Rows[i]["mac_name"].ToString();
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