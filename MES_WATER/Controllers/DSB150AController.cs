using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MES_WATER.Models;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;



namespace MES_WATER.Controllers
{
    public class DSB150AController : JsonNetController
    {
        Comm comm = new Comm();
        CheckData CD = new CheckData();
        // GET: DSB150A
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Echart()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Get_LineChartData(string pTkCode,string pInsDate)
        {
            string sSql = "";
            DataTable dtTmp = new DataTable();
            decimal dUpLimit =  comm.sGetDecimal(comm.Get_Data("QMB14_0000", pTkCode, "spc_code", "up_limit"));
            decimal dDownLimit =  comm.sGetDecimal(comm.Get_Data("QMB14_0000", pTkCode, "spc_code", "down_limit"));

            string sEpbCode = "";
            string sFieldCode = "";
            List<decimal> list_QmtValue = new List<decimal>();
            List<DateTime> list_InsDateTime = new List<DateTime>();

            //取得電子表單欄位
            sSql = "select * " +
                   "  from QMB14_0000" +
                   " where spc_code = @spc_code";                  
            dtTmp = comm.Get_DataTable(sSql, "spc_code", pTkCode);
            if (dtTmp.Rows.Count > 0) {
                DataRow r = dtTmp.Rows[0];
                sEpbCode = r["epb_code"].ToString();
                sFieldCode = r["epb_field_code"].ToString();

                //取得檢驗結果
                sSql = "select * " +
                       "  from EPB03_0000" +
                       " where epb_code = '" + sEpbCode + "'" +
                       "   and field_code = '" + sFieldCode + "'" +
                       "   and ins_date = '" + pInsDate + "'" +
                       " order by ins_time";
                dtTmp = comm.Get_DataTable(sSql);
                for (int i = 0; i < dtTmp.Rows.Count; i++) {
                    DataRow ir = dtTmp.Rows[i];
                    string sFieldValue = ir["field_value"].ToString();
                    string sInsDateTime = ir["ins_date"].ToString() + " " + ir["ins_time"].ToString();

                    //管制圖的架構限制了內容必須是數值
                    if (CD.IsNumeric(sFieldValue)) 
                        if (CD.IsDate(sInsDateTime)) {
                            list_QmtValue.Add(comm.sGetDecimal(sFieldValue));
                            list_InsDateTime.Add(DateTime.Parse(sInsDateTime));
                        }                                                              
                }
            }

            //直接給予x軸群組
            string[] xAxis_data = new string[list_InsDateTime.Count];
            for (int i = 0; i < list_InsDateTime.Count; i++) {
                xAxis_data[i] = list_InsDateTime[i].ToString("HH:mm:ss");
            }

            // 定義x軸和y軸的最小和最大
            //以管制來說，X軸是時間
            //string xAxis_start = "0";
            //string xAxis_end = "10";
            //以管制來說，Y軸是數值上下限(這裡要放最大與最小的結果)
            decimal yAxis_max = dUpLimit;
            if (list_QmtValue.Count > 0) 
                if (list_QmtValue.Max() > dUpLimit) { yAxis_max = list_QmtValue.Max(); }                
                       
            decimal yAxis_min = dDownLimit;
            if (list_QmtValue.Count > 0) 
                if (list_QmtValue.Min() < dDownLimit) { yAxis_min = list_QmtValue.Min(); }
            
            // 定義y軸顯示的刻度
            decimal upperLmt = dUpLimit;
            decimal lowerLmt = dDownLimit;
            decimal average = (dUpLimit + dDownLimit) / 2;

            //  設置座標資料 (實際數值)
            List<string[]> coordinate_data = new List<string[]>();
            for (int i = 0; i < list_QmtValue.Count; i++)
            {
                string[] sQmt_Coordinate = { list_InsDateTime[i].ToString("HH:mm:ss"), list_QmtValue[i].ToString() };
                coordinate_data.Add(sQmt_Coordinate);
            }


            //設置顯示水平線的資料(起點和終點連線)
            List<string[]> upperLmt_data = new List<string[]>();
            List<string[]> lowerLmt_data = new List<string[]>();
            List<string[]> average_data = new List<string[]>();

            if (list_QmtValue.Count > 0) {
                string sFirstTime = list_InsDateTime[0].ToString("HH:mm:ss");
                string sLastTime = list_InsDateTime[list_InsDateTime.Count - 1].ToString("HH:mm:ss");
                upperLmt_data.Add(new string[2] { sFirstTime, upperLmt.ToString() });
                upperLmt_data.Add(new string[2] { sLastTime, upperLmt.ToString() });
                lowerLmt_data.Add(new string[2] { sFirstTime, lowerLmt.ToString() });
                lowerLmt_data.Add(new string[2] { sLastTime, lowerLmt.ToString() });
                average_data.Add(new string[2] { sFirstTime, average.ToString() });
                average_data.Add(new string[2] { sLastTime, average.ToString() });
            }       

            var returnObj = new {
                xAxis_data = xAxis_data,
                //xAxis_start = xAxis_start,
                //xAxis_end = xAxis_end,
                yAxis_max = yAxis_max,
                yAxis_min = yAxis_min,
                upperLmt = upperLmt,
                lowerLmt = lowerLmt,
                average = average,
                coordinate_data = coordinate_data,
                upperLmt_data = upperLmt_data,
                lowerLmt_data = lowerLmt_data,
                average_data = average_data
            };

            return Json(returnObj);
        }
    }
}