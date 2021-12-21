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
    public class WMB180AController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();

        public ActionResult Index()
        {
            return View();
        }

        public class WMB180A
        {
            [DisplayName("料號")]
            public string pro_code { get; set; }

            [DisplayName("料號")]
            public string pro_name { get; set; }

            [DisplayName("期初量")]
            public double qty_s { get; set; }

            [DisplayName("入庫量")]
            public double qty_in { get; set; }

            [DisplayName("出庫量")]
            public double qty_out { get; set; }

            [DisplayName("期末量")]
            public double qty_e { get; set; }
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
            List<WMB180A> result = new List<WMB180A>();

            // 沒有下查詢條件時就沒有資料
            if (string.IsNullOrEmpty(pWhere))
            {
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }

            // 將前面的查詢欄位放到query_data
            JqGridQueryData query_data = comm.parseQuery(pWhere);

            // 取得報表
            result.AddRange(Get_RptData(query_data));

            // 回傳給view
            var returnObj = new
            {
                data = result
            };

            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }

        private List<WMB180A> Get_RptData(JqGridQueryData query_data)
        {
            List<WMB180A> result = new List<WMB180A>();

            string sStoCode = query_data.find("sto_code"); //儲位
            string sProCodeS = query_data.find("pro_code", "S"); //產品編號
            string sProCodeE = query_data.find("pro_code", "E"); //產品編號
            string sStoDate = query_data.find("sto_date"); //產品編號

            Dictionary<string, object> sSqlParams = new Dictionary<string, object>();

            //抓取入出庫記錄內容
            string sSubWhere = "";
            if (!string.IsNullOrEmpty(sStoCode))
            {
                //sSubWhere = " AND loc_code IN (SELECT loc_code FROM WMB02_0000 WHERE sto_code= '" + sStoCode + "' )";
                sSubWhere = " AND loc_code IN (SELECT loc_code FROM WMB02_0000 WHERE sto_code= @sto_code )";
                sSqlParams.Add("@sto_code", sStoCode);
            }

            string sSubWhere3 = "";
            if (!string.IsNullOrEmpty(sProCodeS))
            {
                //sSubWhere3 = " AND a.pro_code BETWEEN '" + sProCodeS + "' AND '" + sProCodeE  + "' ";
                sSubWhere3 = " AND a.pro_code BETWEEN @pro_code_s AND @pro_code_e ";
                sSqlParams.Add("@pro_code_s", sProCodeS);
                sSqlParams.Add("@pro_code_e", sProCodeE);
            }

            string sQtySDateWhere = " and ins_date < @sto_date_QtyS ";
            string sQtyInDateWhere = " and ins_date like @sto_date_QtyIn ";
            string sQtyOutDateWhere = " and ins_date like @sto_date_QtyOut ";
            sSqlParams.Add("@sto_date_QtyS", sStoDate + "/01");
            sSqlParams.Add("@sto_date_QtyIn", sStoDate + "/%");
            sSqlParams.Add("@sto_date_QtyOut", sStoDate + "/%");

            string sSql = @"select s.*, qty_s + qty_in - qty_out as qty_e , b.pro_name as pro_name2
                            from (
	                            SELECT a.pro_code      
	                            ,( SELECT ISNULL(SUM(CASE WHEN ins_type = 'I' THEN pro_qty ELSE pro_qty * -1 END), 0) FROM WMT0200 WHERE pro_code = a.pro_code " + sQtySDateWhere + sSubWhere + @" ) AS qty_s   
	                            ,( SELECT ISNULL(pro_qty, 0) FROM WMT0200 WHERE pro_code = a.pro_code and ins_type = 'I' " + sQtyInDateWhere + sSubWhere + @" ) AS qty_in   
	                            ,( SELECT ISNULL(pro_qty*-1, 0) FROM WMT0200 WHERE pro_code = a.pro_code and ins_type = 'O' " + sQtyOutDateWhere + sSubWhere + @" ) AS qty_out    
	                            FROM WMT0200 a 
	                            WHERE 1=1 "
                                 + sSubWhere3
                                 + sSubWhere
                                 + @"group by a.pro_code
                            ) s
                            LEFT JOIN MEB20_0000 b ON s.pro_code=b.pro_code
                            where qty_s != 0 or qty_in != 0 or qty_out != 0 or qty_s != 0
                            order by s.pro_code
                            ";

            DataTable dtTmp = comm.Get_DataTable(sSql, sSqlParams, true);

            foreach (DataRow dr in dtTmp.Rows)
            {
                // 
                WMB180A data = new WMB180A();
                data.pro_code = dr["pro_code"].ToString();
                data.pro_name = dr["pro_name2"].ToString();  // 新增顯示品名
                data.qty_s = comm.sGetDouble(dr["qty_s"].ToString());
                data.qty_in = comm.sGetDouble(dr["qty_in"].ToString());
                data.qty_out = comm.sGetDouble(dr["qty_out"].ToString());
                data.qty_e = comm.sGetDouble(dr["qty_e"].ToString());

                result.Add(data);
            }


            return result;
        }



        // 取得物料列表
        //string sSql = "SELECT DISTINCT a.pro_code"
        //            + "      ,0.1 AS qty_s"
        //            + "      ,0.1 AS qty_in"
        //            + "      ,0.1 AS qty_out"
        //            + "      ,0.1 AS qty_e"
        //            + ",b.pro_name as pro_name2"
        //            + " FROM WMT0200 a"
        //            + "  LEFT JOIN MEB20_0000 b ON a.pro_code=b.pro_code"
        //            + " WHERE 1=1"
        //            + sSubWhere3 + sSubWhere  
        //            + " ORDER BY a.pro_code ";


        //            string sSql = @"select s.*, qty_s + qty_in - qty_out as qty_e , b.pro_name as pro_name2
        //from (
        //	SELECT a.pro_code      
        //	,( SELECT ISNULL(SUM(CASE WHEN ins_type = 'I' THEN pro_qty ELSE pro_qty * -1 END), 0) FROM WMT0200 WHERE pro_code = a.pro_code and ins_date < '" + sStoDate + "/01' " + sSubWhere + @" ) AS qty_s   
        //	,( SELECT ISNULL(pro_qty, 0) FROM WMT0200 WHERE pro_code = a.pro_code and ins_type = 'I' and ins_date like '" + sStoDate + "/%' " + sSubWhere + @" )AS qty_in   
        //	,( SELECT ISNULL(pro_qty*-1, 0) FROM WMT0200 WHERE pro_code = a.pro_code and ins_type = 'O' and ins_date like '" + sStoDate + "/%' " + sSubWhere + @" )AS qty_out    
        //	FROM WMT0200 a 
        //	WHERE 1=1 "
        //     + sSubWhere3
        //     + sSubWhere
        //     + @"group by a.pro_code
        //) s
        //LEFT JOIN MEB20_0000 b ON s.pro_code=b.pro_code
        //where qty_s != 0 or qty_in != 0 or qty_out != 0 or qty_s != 0
        //order by s.pro_code
        //";

        //DataTable dtTmp = comm.Get_DataTable(sSql, sSqlParams);
        //foreach (DataRow dr in dtTmp.Rows)
        //{
        //    //data.qty_s = Get_qty_s1(data.pro_code, sStoDate, sSubWhere) - Get_qty_s2(data.pro_code, sStoDate, sSubWhere);
        //    //data.qty_in = Get_qty_in(data.pro_code, sStoDate, sSubWhere);
        //    //data.qty_out = Get_qty_out(data.pro_code, sStoDate, sSubWhere);
        //    //data.qty_e = data.qty_s + data.qty_in - data.qty_out;

        //    // double型態怎麼判斷等於0， 下面這個是錯的
        //    //if ((int)data.qty_e != 0 || (int)data.qty_s != 0 || (int)data.qty_in != 0 || (int)data.qty_out != 0)
        //    //{
        //    //    WMB180A data2 = new WMB180A();
        //    //    //data2.pro_code = dr["pro_code"].ToString();
        //    //    data2.pro_name = dr["pro_name2"].ToString();  // 新增顯示品名
        //    //    data2.qty_s = comm.sGetDouble(dr["qty_s"].ToString());
        //    //    data2.qty_in = comm.sGetDouble(dr["qty_in"].ToString());
        //    //    data2.qty_out = comm.sGetDouble(dr["qty_out"].ToString());
        //    //    data2.qty_e = comm.sGetDouble(dr["qty_e"].ToString());
        //    //    result.Add(data);
        //    //}
        //}


        //private double Get_qty_s1(string sProCode, string pDate, string pSubWhere)
        //{
        //    string sSql = "SELECT ISNULL(SUM(pro_qty),0) AS qty FROM WMT0200 WHERE pro_code='" + sProCode + "' AND ins_type='I' AND ins_date<'" + pDate + "/01'" + pSubWhere;
        //    DataTable dtTmp = comm.Get_DataTable(sSql);
        //    var ret = comm.sGetDouble(dtTmp.Rows[0][0].ToString());
        //    return ret;
        //}

        //private double Get_qty_s2(string sProCode, string pDate, string pSubWhere)
        //{

        //    string sSql = "SELECT ISNULL(SUM(pro_qty),0) AS qty FROM WMT0200 WHERE pro_code='" + sProCode + "' AND ins_type='O' AND ins_date<'" + pDate + "/01'" + pSubWhere;
        //    DataTable dtTmp = comm.Get_DataTable(sSql);
        //    var ret = comm.sGetDouble(dtTmp.Rows[0][0].ToString());
        //    return ret;
        //}

        //// 取得入庫量
        //private double Get_qty_in(string sProCode, string pDate, string pSubWhere)
        //{

        //    string sSql = "SELECT ISNULL(SUM(pro_qty),0) AS qty FROM WMT0200 WHERE pro_code='" + sProCode + "' AND ins_type='I' AND ins_date like '" + pDate + "/%'" + pSubWhere;
        //    DataTable dtTmp = comm.Get_DataTable(sSql);
        //    var ret = comm.sGetDouble(dtTmp.Rows[0][0].ToString());
        //    return ret;
        //}

        //// 取得出庫量
        //private double Get_qty_out(string sProCode, string pDate, string pSubWhere)
        //{

        //    string sSql = "SELECT ISNULL(SUM(pro_qty),0) AS qty FROM WMT0200 WHERE pro_code='" + sProCode + "' AND ins_type='O' AND ins_date like '" + pDate + "/%'" + pSubWhere;
        //    DataTable dtTmp = comm.Get_DataTable(sSql);
        //    var ret = comm.sGetDouble(dtTmp.Rows[0][0].ToString());
        //    return ret;
        //}



    }
}