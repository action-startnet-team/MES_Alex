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
    public class PRPT020AController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();

        public ActionResult Index()
        {
            return View();
        }

        public class PRPT020A
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

            [DisplayName("原料代號")]
            public string pro_code_source { get; set; }

            [DisplayName("原料名稱")]
            public string pro_name_source { get; set; }

            [DisplayName("原料批號")]
            public string lot_no { get; set; }

            [DisplayName("用料數量")]
            public string pro_qty_source { get; set; }

            [DisplayName("用料單位")]
            public string pro_unit { get; set; }

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
            List<PRPT020A> result = new List<PRPT020A>();

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


        public List<PRPT020A> Get_StatData(JqGridQueryData query_data)
        {
            List<PRPT020A> result = new List<PRPT020A>();

            string sProCode = query_data.find("pro_code"); //產品編號
            string sProLotNo = query_data.find("pro_lot_no"); //生產批號
            string sInsDate = query_data.find("ins_date"); //生產日期

            string sSql = "select a.mo_code, a.pro_code, a.pro_lot_no, SUM(pro_qty) as pro_qty " +
                   "from MED09_0000 a " +
                   "left join MEB20_0000 b on a.pro_code = b.pro_code " +
                   "where 1=1";

            if (!string.IsNullOrEmpty(sProCode))
                sSql += " and a.pro_code = '" + sProCode + "'";
            if (!string.IsNullOrEmpty(sProLotNo))
                sSql += " and a.pro_lot_no = '" + sProLotNo + "'";
            if (!string.IsNullOrEmpty(sInsDate))
                sSql += " and a.ins_date = '" + sInsDate + "'";

            sSql += " GROUP BY a.pro_code, a.mo_code, a.pro_lot_no";

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


                sSql = "select b.work_code, a.pro_code, c.pro_name, a.lot_no, a.pro_qty, a.pro_unit " +
                       "from MED06_0000 a " +
                       "left join MEB30_0000 b on a.work_code = b.work_code " +
                       "left join MEB20_0000 c on a.pro_code = c.pro_code " +
                       "where a.mo_code = '" + mo_code + "' and a.pro_code = '" + pro_code + "'";
                DataTable dtSrc = comm.Get_DataTable(sSql);

                if (dtSrc.Rows.Count > 0)
                {
                    for (int j = 0; j < dtSrc.Rows.Count; j++)
                    {
                        PRPT020A data = new PRPT020A();

                        data.mo_code = mo_code;
                        data.pro_code = pro_code;
                        data.pro_name = pro_name;
                        data.pro_lot_no = dtTmp.Rows[i]["pro_lot_no"].ToString();
                        data.pro_qty = dtTmp.Rows[i]["pro_qty"].ToString();
                        data.ok_unit = ok_unit;

                        data.work_code = dtSrc.Rows[j]["work_code"].ToString();
                        data.pro_code_source = dtSrc.Rows[j]["pro_code"].ToString();
                        data.pro_name_source = dtSrc.Rows[j]["pro_name"].ToString();
                        data.lot_no = dtSrc.Rows[j]["lot_no"].ToString();
                        data.pro_qty_source = dtSrc.Rows[j]["pro_qty"].ToString();
                        data.pro_unit = dtSrc.Rows[j]["pro_unit"].ToString();

                        result.Add(data);
                    }
                }
            }
            return result;
        }

        [HttpPost]
        public JsonResult Get_ProLotNoList(string pro_code)
        {
            string sSql = "select distinct(pro_lot_no)" +
                         "from MED09_0000 where pro_code = '" + pro_code + "'";
            DataTable dtTmp = comm.Get_DataTable(sSql);

            List<string> result = new List<string>();
            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                result.Add(dtTmp.Rows[i]["pro_lot_no"].ToString());
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Get_InsDate(string pro_code, string pro_lot_no)
        {
            string sSql = "select distinct(ins_date)" +
                         "from MED09_0000 where pro_code = '" + pro_code + "' and pro_lot_no = '" + pro_lot_no + "'";
            DataTable dtTmp = comm.Get_DataTable(sSql);

            List<string> result = new List<string>();
            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                result.Add(dtTmp.Rows[i]["ins_date"].ToString());
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}