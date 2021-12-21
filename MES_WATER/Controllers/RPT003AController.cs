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
    public class RPT003AController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();

        public ActionResult Index()
        {
            return View();
        }

        public class RPT003A
        {
            [DisplayName("製令號")]
            public string mo_code { get; set; }

            [DisplayName("產品代號")]
            public string pro_code { get; set; }

            [DisplayName("產品名稱")]
            public string pro_name { get; set; }

            [DisplayName("生產批號")]
            public string pro_lot_no { get; set; }

            [DisplayName("生產數量")]
            public string pro_qty { get; set; }

            [DisplayName("生產數量單位")]
            public string ok_unit { get; set; }

            [DisplayName("製程")]
            public string work_code { get; set; }

            [DisplayName("投料時間")]
            public string insTime { get; set; }
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
            List<RPT003A> result = new List<RPT003A>();

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


        public List<RPT003A> Get_StatData(JqGridQueryData query_data)
        {
            List<RPT003A> result = new List<RPT003A>();

            string sProCode = query_data.find("pro_code"); //原料編號
            string sLotNo = query_data.find("lot_no"); //原料批號            

            string sSql = "select a.mo_code, a.pro_code, a.pro_lot_no, SUM(pro_qty) as pro_qty " +
                   "from MED09_0000 a " +
                   "left join MEB20_0000 b on a.pro_code = b.pro_code " +
                   "GROUP BY a.pro_code, a.mo_code, a.pro_lot_no";

            DataTable dtTmp = comm.Get_DataTable(sSql);

            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                string mo_code = dtTmp.Rows[i]["mo_code"].ToString();
                string pro_code = dtTmp.Rows[i]["pro_code"].ToString();

                sSql = "select a.ok_unit, b.pro_name " +
                       "from MED09_0000 a " +
                       "left join MEB20_0000 b on a.pro_code = b.pro_code " +
                       "where a.mo_code = '" + mo_code + "' and a.pro_code = '" + pro_code + "'";
                DataTable dtTmp2 = comm.Get_DataTable(sSql);
                string pro_name = dtTmp2.Rows[0]["pro_name"].ToString();
                string ok_unit = dtTmp2.Rows[0]["ok_unit"].ToString();


                sSql = "select b.work_code, a.ins_date + ' ' + a.ins_time as insTime " +
                       "from MED06_0000 a " +
                       "left join MEB30_0000 b on a.work_code = b.work_code " +                       
                       "where a.mo_code = '" + mo_code + "' and a.pro_code = '" + sProCode + "' ";

                if (!string.IsNullOrEmpty(sLotNo))
                    sSql += "and a.lot_no = '" + sLotNo + "'";

                DataTable dtSrc = comm.Get_DataTable(sSql);

                if (dtSrc.Rows.Count > 0)
                {
                    for (int j = 0; j < dtSrc.Rows.Count; j++)
                    {
                        RPT003A data = new RPT003A();

                        data.mo_code = mo_code;
                        data.pro_code = pro_code;
                        data.pro_name = pro_name;
                        data.pro_lot_no = dtTmp.Rows[i]["pro_lot_no"].ToString();
                        data.pro_qty = dtTmp.Rows[i]["pro_qty"].ToString();
                        data.ok_unit = ok_unit;

                        data.work_code = dtSrc.Rows[j]["work_code"].ToString();
                        data.insTime = dtSrc.Rows[j]["insTime"].ToString();
                        
                        result.Add(data);
                    }
                }
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
