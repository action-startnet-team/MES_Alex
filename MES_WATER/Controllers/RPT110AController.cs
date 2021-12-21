using MES_WATER.Models;
using MES_WATER.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MES_WATER.Controllers
{
    public class RPT110AController : Controller
    {

        Comm comm = new Comm();
        public ActionResult Index()
        {
            return View();
        }

        public class RPT110A_0000
        {
            [DisplayName("站別名稱")]
            public string work_name { get; set; }
            [DisplayName("品號")]
            public string pro_code { get; set; }
            [DisplayName("類別")]
            public string type { get; set; }
            [DisplayName("1")]
            public decimal Total_1 { get; set; }
            [DisplayName("2")]
            public decimal Total_2 { get; set; }
            [DisplayName("3")]
            public decimal Total_3 { get; set; }
            [DisplayName("4")]
            public decimal Total_4 { get; set; }
            [DisplayName("5")]
            public decimal Total_5 { get; set; }
            [DisplayName("6")]
            public decimal Total_6 { get; set; }
            [DisplayName("7")]
            public decimal Total_7 { get; set; }
            [DisplayName("8")]
            public decimal Total_8 { get; set; }
            [DisplayName("9")]
            public decimal Total_9 { get; set; }
            [DisplayName("10")]
            public decimal Total_10 { get; set; }
            [DisplayName("11")]
            public decimal Total_11 { get; set; }
            [DisplayName("12")]
            public decimal Total_12 { get; set; }
            [DisplayName("13")]
            public decimal Total_13 { get; set; }
            [DisplayName("14")]
            public decimal Total_14 { get; set; }
            [DisplayName("15")]
            public decimal Total_15 { get; set; }
            [DisplayName("16")]
            public decimal Total_16 { get; set; }
            [DisplayName("17")]
            public decimal Total_17 { get; set; }
            [DisplayName("18")]
            public decimal Total_18 { get; set; }
            [DisplayName("19")]
            public decimal Total_19 { get; set; }
            [DisplayName("20")]
            public decimal Total_20 { get; set; }
            [DisplayName("21")]
            public decimal Total_21 { get; set; }
            [DisplayName("22")]
            public decimal Total_22 { get; set; }
            [DisplayName("23")]
            public decimal Total_23 { get; set; }
            [DisplayName("24")]
            public decimal Total_24 { get; set; }
            [DisplayName("25")]
            public decimal Total_25 { get; set; }
            [DisplayName("26")]
            public decimal Total_26 { get; set; }
            [DisplayName("27")]
            public decimal Total_27 { get; set; }
            [DisplayName("28")]
            public decimal Total_28 { get; set; }
            [DisplayName("29")]
            public decimal Total_29 { get; set; }
            [DisplayName("30")]
            public decimal Total_30 { get; set; }
            [DisplayName("31")]
            public decimal Total_31 { get; set; }
            [DisplayName("合計")]
            public decimal Total { get; set; }
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
            List<RPT110A_0000> result = new List<RPT110A_0000>();

            // 沒有下查詢條件時就沒有資料
            if (string.IsNullOrWhiteSpace(pWhere))
            {
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }

            // 將前面的查詢欄位放到query_data
            JqGridQueryData query_data = comm.parseQuery(pWhere);

            string work_time_s = query_data.find("work_time", "S");  //生產日期開始
            string work_time_e = "";//生產日期結束
            if (string.IsNullOrWhiteSpace(work_time_s))
            {
            }
            else
            {
                work_time_e = work_time_s;
                int year = int.Parse(work_time_s.Substring(0, 4));
                int day = int.Parse(work_time_s.Substring(5, 2));
                int thismonthofDays = DateTime.DaysInMonth(year, day);
                work_time_s = work_time_s + "/" + "01";
                work_time_e = work_time_e + "/" + thismonthofDays;//生產日期結束
            }
            string line_code = query_data.find("line_code");  //線別
            string pro_code = query_data.find("pro_code");  //品號

            //開始抓取資料
            string sSql = "";

            Dictionary<string, object> sSqlParams = new Dictionary<string, object>();

            sSql = @"select work_name,pro_code,type, [1],[2],[3],[4],[5],[6],[7],[8],[9],[10],
                    [11],[12],[13],[14],[15],[16],[17],[18],[19],[20],[21],[22],[23],[24],
                    [25],[26],[27],[28],[29],[30],[31]
                    into  #TotalTable
                    from (SELECT work_name,pro_code,
                    'OK' as type,
                    ISNULL([1],0) AS [1],ISNULL([2],0) AS [2],ISNULL([3],0) AS [3],
                    ISNULL([4],0) AS [4],ISNULL([5],0) AS [5],ISNULL([6],0) AS [6],
                    ISNULL([7],0) AS [7],ISNULL([8],0) AS [8],ISNULL([9],0) AS [9],
                    ISNULL([10],0) AS [10],ISNULL([11],0) AS [11],ISNULL([12],0) AS [12],
                    ISNULL([13],0) AS [13],ISNULL([14],0) AS [14],ISNULL([15],0) AS [15],
                    ISNULL([16],0) AS [16],ISNULL([17],0) AS [17],ISNULL([18],0) AS [18],
                    ISNULL([19],0) AS [19],ISNULL([20],0) AS [20],ISNULL([21],0) AS [21],
                    ISNULL([22],0) AS [22],ISNULL([23],0) AS [23],ISNULL([24],0) AS [24],
                    ISNULL([25],0) AS [25],ISNULL([26],0) AS [26],ISNULL([27],0) AS [27],
                    ISNULL([28],0) AS [28],ISNULL([29],0) AS [29],ISNULL([30],0) AS [30],
                    ISNULL([31],0) AS [31]
                    from
                    (select dbo.MEB30_0000.work_name ,dbo.MEB20_0000.pro_code,
                    DAY(dbo.MEM01_0000.work_time_s) as everyday,SUM(dbo.MEM01_0000.ok_qty) as ok_total
                    from dbo.MEM01_0000
                    join dbo.MEB30_0000
                    on dbo.MEM01_0000.work_code= dbo.MEB30_0000.work_code
                    join dbo.MET01_0000
                    on dbo.MEM01_0000.mo_code=dbo.MET01_0000.mo_code
                    join dbo.MEB20_0000 
                    on dbo.MET01_0000.pro_code=dbo.MEB20_0000.pro_code
                    group by dbo.MEB30_0000.work_name,dbo.MEB20_0000.pro_code,dbo.MEM01_0000.work_time_s,dbo.MEB20_0000.line_code,MEB30_0000.work_code";
            sSql += GetWhereCondition(1, work_time_s, work_time_e, line_code, pro_code);
            if (!string.IsNullOrWhiteSpace(work_time_s))
            {
                sSqlParams.Add("@work_time_s1", work_time_s);
            }
            if (!string.IsNullOrWhiteSpace(work_time_e))
            {
                sSqlParams.Add("@work_time_e1", work_time_e);
            }
            if (!string.IsNullOrWhiteSpace(line_code))
            {
                sSqlParams.Add("@line_code1", line_code);
            }
            if (!string.IsNullOrWhiteSpace(pro_code))
            {
                sSqlParams.Add("@pro_code1", pro_code);
            }
            sSql += @" ) Mytable
            PIVOT
            (
	            SUM(Mytable.ok_total)
	            FOR MyTable.everyday
	            IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],
	            [13],[14],[15],[16],[17],[18],[19],[20],[21],[22],[23],
	            [24],[25],[26],[27],[28],[29],[30],[31])
            ) AS piv
            union
            SELECT work_name,pro_code,
            'NG' as type,
            ISNULL([1],0) AS 一號,ISNULL([2],0) AS 二號,ISNULL([3],0) AS 三號,
            ISNULL([4],0) AS 四號,ISNULL([5],0) AS 五號,ISNULL([6],0) AS 六號,
            ISNULL([7],0) AS 七號,ISNULL([8],0) AS 八號,ISNULL([9],0) AS 九號,
            ISNULL([10],0) AS 十號,ISNULL([11],0) AS 十一號,ISNULL([12],0) AS 十二號,
            ISNULL([13],0) AS 十三號,ISNULL([14],0) AS 十四號,ISNULL([15],0) AS 十五號,
            ISNULL([16],0) AS 十六號,ISNULL([17],0) AS 十七號,ISNULL([18],0) AS 十八號,
            ISNULL([19],0) AS 十九號,ISNULL([20],0) AS 二十號,ISNULL([21],0) AS 二十一號,
            ISNULL([22],0) AS 二十二號,ISNULL([23],0) AS 二十三號,ISNULL([24],0) AS 二十四號,
            ISNULL([25],0) AS 二十五號,ISNULL([26],0) AS 二十六號,ISNULL([27],0) AS 二十七號,
            ISNULL([28],0) AS 二十八號,ISNULL([29],0) AS 二十九號,ISNULL([30],0) AS 三十號,
            ISNULL([31],0) AS 三十一號
            from
            (select dbo.MEB30_0000.work_name ,dbo.MEB20_0000.pro_code,
            DAY(dbo.MEM01_0000.work_time_s) as everyday,SUM(dbo.MEM01_0000.ng_qty) as ng_total
            from dbo.MEM01_0000
            join dbo.MEB30_0000
            on dbo.MEM01_0000.work_code= dbo.MEB30_0000.work_code
            join dbo.MET01_0000
            on dbo.MEM01_0000.mo_code=dbo.MET01_0000.mo_code
            join dbo.MEB20_0000 
            on dbo.MET01_0000.pro_code=dbo.MEB20_0000.pro_code
            group by dbo.MEB30_0000.work_name,dbo.MEB20_0000.pro_code,dbo.MEM01_0000.work_time_s,dbo.MEB20_0000.line_code,MEB30_0000.work_code";
            sSql += GetWhereCondition(2, work_time_s, work_time_e, line_code, pro_code);
            if (!string.IsNullOrWhiteSpace(work_time_s))
            {
                sSqlParams.Add("@work_time_s2", work_time_s);
            }
            if (!string.IsNullOrWhiteSpace(work_time_e))
            {
                sSqlParams.Add("@work_time_e2", work_time_e);
            }
            if (!string.IsNullOrWhiteSpace(line_code))
            {
                sSqlParams.Add("@line_code2", line_code);
            }
            if (!string.IsNullOrWhiteSpace(pro_code))
            {
                sSqlParams.Add("@pro_code2", pro_code);
            }
            sSql += @" ) Mytable
            PIVOT
            (
	            SUM(Mytable.ng_total)
	            FOR MyTable.everyday
	            IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],
	            [13],[14],[15],[16],[17],[18],[19],[20],[21],[22],[23],
	            [24],[25],[26],[27],[28],[29],[30],[31])
            ) AS piv
            ) productReport
            order by work_name,pro_code,type";
            sSql += @" 
            select work_name,pro_code,type,[1],[2],[3],[4],[5],[6],[7],[8],[9],[10],
            [11],[12],[13],[14],[15],[16],[17],[18],[19],[20],[21],[22],[23],[24],
            [25],[26],[27],[28],[29],[30],[31]
            from #TotalTable            

            drop table #TotalTable";

            DataTable dtTmp = null;
            if (sSqlParams.Count > 0)
            {
                dtTmp = comm.Get_DataTable(sSql, sSqlParams);
            }
            else
            {
                dtTmp = comm.Get_DataTable(sSql);
            }
            int i;
            for (i = 0; i < dtTmp.Rows.Count; i++)
            {
                RPT110A_0000 row = new RPT110A_0000();
                row.work_name = dtTmp.Rows[i]["work_name"].ToString();
                row.pro_code = dtTmp.Rows[i]["pro_code"].ToString();
                row.type = dtTmp.Rows[i]["type"].ToString();
                row.Total_1 = dtTmp.Rows[i]["1"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["1"].ToString());
                row.Total_2 = dtTmp.Rows[i]["2"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["2"].ToString());
                row.Total_3 = dtTmp.Rows[i]["3"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["3"].ToString());
                row.Total_4 = dtTmp.Rows[i]["4"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["4"].ToString());
                row.Total_5 = dtTmp.Rows[i]["5"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["5"].ToString());
                row.Total_6 = dtTmp.Rows[i]["6"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["6"].ToString());
                row.Total_7 = dtTmp.Rows[i]["7"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["7"].ToString());
                row.Total_8 = dtTmp.Rows[i]["8"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["8"].ToString());
                row.Total_9 = dtTmp.Rows[i]["9"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["9"].ToString());
                row.Total_10 = dtTmp.Rows[i]["10"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["10"].ToString());

                row.Total_11 = dtTmp.Rows[i]["11"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["11"].ToString());
                row.Total_12 = dtTmp.Rows[i]["12"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["12"].ToString());
                row.Total_13 = dtTmp.Rows[i]["13"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["13"].ToString());
                row.Total_14 = dtTmp.Rows[i]["14"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["14"].ToString());
                row.Total_15 = dtTmp.Rows[i]["15"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["15"].ToString());
                row.Total_16 = dtTmp.Rows[i]["16"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["16"].ToString());
                row.Total_17 = dtTmp.Rows[i]["17"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["17"].ToString());
                row.Total_18 = dtTmp.Rows[i]["18"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["18"].ToString());
                row.Total_19 = dtTmp.Rows[i]["19"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["19"].ToString());
                row.Total_20 = dtTmp.Rows[i]["20"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["20"].ToString());

                row.Total_21 = dtTmp.Rows[i]["21"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["21"].ToString());
                row.Total_22 = dtTmp.Rows[i]["22"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["22"].ToString());
                row.Total_23 = dtTmp.Rows[i]["23"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["23"].ToString());
                row.Total_24 = dtTmp.Rows[i]["24"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["24"].ToString());
                row.Total_25 = dtTmp.Rows[i]["25"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["25"].ToString());
                row.Total_26 = dtTmp.Rows[i]["26"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["26"].ToString());
                row.Total_27 = dtTmp.Rows[i]["27"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["27"].ToString());
                row.Total_28 = dtTmp.Rows[i]["28"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["28"].ToString());
                row.Total_29 = dtTmp.Rows[i]["29"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["29"].ToString());
                row.Total_30 = dtTmp.Rows[i]["30"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["30"].ToString());

                row.Total_31 = dtTmp.Rows[i]["31"].ToString() == "" ? 0 : decimal.Parse(dtTmp.Rows[i]["31"].ToString());

                Decimal total = 0;
                Decimal temp = 0;
                for (int j = 1; j < 32; j++)
                {
                    decimal.TryParse(dtTmp.Rows[i][j.ToString()].ToString(), out temp);
                    total = total + temp;
                }
                row.Total = total;

                result.Add(row);
            }

            //result = Get_RptData(query_data);

            // 回傳給view
            var returnObj = new
            {
                data = result
            };

            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 加上SQL where 條件上的參數
        /// </summary>
        /// <param name="number">同一個參數可以重複出現</param>
        /// <param name="work_time_s">生產起始日期</param>
        /// <param name="work_time_e">生產結束日期</param>
        /// <param name="line_code">線別</param>
        /// <param name="pro_code">品號</param>
        /// <returns></returns>
        public string GetWhereCondition(int number, string work_time_s, string work_time_e, string line_code, string pro_code)
        {
            string strwhere = "";
            if (string.IsNullOrWhiteSpace(work_time_s) && string.IsNullOrWhiteSpace(work_time_e))
            {
                if (string.IsNullOrWhiteSpace(line_code))
                {
                    if (string.IsNullOrWhiteSpace(pro_code))
                    {
                        return strwhere;
                    }
                    else
                    {
                        strwhere = @" 
having dbo.MEB20_0000.pro_code=@pro_code" + number;
                    }
                }
                else
                {
                    strwhere = @" 
having dbo.MEB30_0000.work_code=@line_code" + number;
                    if (string.IsNullOrWhiteSpace(pro_code))
                    { }
                    else
                    {
                        strwhere += " and dbo.MEB20_0000.pro_code=@pro_code" + number;
                    }
                }
            }
            else
            {
                strwhere = @" 
having dbo.MEM01_0000.work_time_s between @work_time_s" + number + " and @work_time_e" + number;
                if (string.IsNullOrWhiteSpace(line_code))
                {
                    if (string.IsNullOrWhiteSpace(pro_code))
                    { }
                    else
                    {
                        strwhere += " and dbo.MEB20_0000.pro_code=@pro_code" + number;
                    }
                }
                else
                {
                    strwhere += " and dbo.MEB30_0000.work_code = @line_code" + number;
                    if (string.IsNullOrWhiteSpace(pro_code))
                    { }
                    else
                    {
                        strwhere += " and dbo.MEB20_0000.pro_code=@pro_code" + number;
                    }
                }
            }
            return strwhere;
        }
    }
}
