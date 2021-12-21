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
    public class RPT010AController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();

        public ActionResult Index()
        {
            return View();
        }

        public class RPT010A
        {
            [DisplayName("製令號")]
            public string mo_code { get; set; }
            [DisplayName("品號")]
            public string pro_code { get; set; }

            [DisplayName("品名")]
            public string pro_name { get; set; }

            [DisplayName("製程")]
            public string work_code { get; set; }

            [DisplayName("日期")]
            public string mft_date { get; set; }

            [DisplayName("生產數")]
            public double pro_qty { get; set; }

            [DisplayName("良品數")]
            public double ok_qty { get; set; }

            [DisplayName("不良數")]
            public double ng_qty { get; set; }

            [DisplayName("良率")]
            public string ok_rate { get; set; }

            [DisplayName("實際工時")]
            public string act_min { get; set; }

            [DisplayName("計劃停線")]
            public string plan_stop { get; set; }

            [DisplayName("非計劃停線")]
            public string non_plan_stop{ get; set; }

            [DisplayName("稼動率")]
            public string utilization_rate { get; set; }

            [DisplayName("產能利用率")]
            public string act_rate { get; set; }

            [DisplayName("標準產能")]
            public string std_rate { get; set; }
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
            List<RPT010A> result = new List<RPT010A>();

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


        public List<RPT010A> Get_StatData(JqGridQueryData query_data)
        {
            List<RPT010A> result = new List<RPT010A>();

            string sCaldateS = query_data.find("mo_start_date", "S"); //統計開始日期
            string sCaldateE = query_data.find("mo_start_date", "E"); //統計開始日期
            string sProCode = query_data.find("pro_code"); //產品編號
            string sLineCode = query_data.find("line_code"); //線別

            //從製令去抓迴圈
            string sSql = "";

            sSql = "select * from MET01_0000 " +
                   " where mo_end_date between '" + sCaldateS + "' and '" + sCaldateE + "'";
            DataTable dtTmp = comm.Get_DataTable(sSql);
            int i;
            for (i = 0; i < dtTmp.Rows.Count ; i++)
            {
            
                
                //從報工歷程檔抓資料 
                sSql = "select * from MEM01_0000 where mo_code ='" + dtTmp.Rows[i]["mo_code"].ToString() + "' "+
                    " and work_time_e !=''"+
                    " order by work_time_s";
                DataTable dtFun = comm.Get_DataTable(sSql);

                if (dtFun.Rows.Count > 0)
                {
                    string ok_rate = "";
                    string std_rate = "";
                    int j;
                    for (j = 0; j < dtFun.Rows.Count ; j++)
                    {
                            RPT010A data = new RPT010A();
                        data.pro_code = dtTmp.Rows[i]["pro_code"].ToString();
                        data.pro_name = comm.Get_QueryData("MEB20_0000", data.pro_code, "pro_code", "pro_name");
                        data.mo_code = dtTmp.Rows[i]["mo_code"].ToString();
                       
                        string sMacCode = dtFun.Rows[j]["mac_code"].ToString();
                        string sStationCode = comm.Get_QueryData("MEB29_0000", sMacCode, "mac_code", "station_code");
                       
                        string sWorkStartTime  = !string.IsNullOrEmpty(dtFun.Rows[j]["work_time_s"].ToString()) ? DateTime.Parse(dtFun.Rows[j]["work_time_s"].ToString()).ToString() : "";
                        string sWorkEndTime = !string.IsNullOrEmpty(dtFun.Rows[j]["work_time_e"].ToString()) ? DateTime.Parse(dtFun.Rows[j]["work_time_e"].ToString()).ToString() : "";

                        data.mft_date = !string.IsNullOrEmpty(dtFun.Rows[j]["work_time_s"].ToString()) ? DateTime.Parse(dtFun.Rows[j]["work_time_s"].ToString()).ToString("yyyy/MM/dd") : "";
                        data.work_code = dtFun.Rows[j]["work_code"].ToString();
                        data.ok_qty=comm.sGetDouble( dtFun.Rows[j]["ok_qty"].ToString());
                        data.ng_qty= comm.sGetDouble(dtFun.Rows[j]["ng_qty"].ToString());
                        ok_rate = Get_ProYield(data.ok_qty.ToString(), data.ng_qty.ToString());
                        data.ok_rate = ok_rate;//良品率
                        data.act_min = Get_ActMin(sWorkStartTime,sWorkEndTime);
                        data.pro_qty = data.ok_qty + data.ng_qty;
                        std_rate = Get_StrQtyMEB2001(sStationCode, data.pro_code);//標準產能

                        data.std_rate = std_rate;
                        data.utilization_rate = Get_Utilization(sWorkStartTime,sWorkEndTime,sMacCode);

                        data.act_rate = Get_CURate(data.ok_qty.ToString(), data.ng_qty.ToString(), sMacCode, sWorkStartTime,sWorkEndTime,std_rate);

                        result.Add(data);

                    }
                    //data.mft_date = DateTime.Parse dtFun.Rows[0]["work_time_s"].ToString();
                  
                }



            }

            ////開始抓取資料
            //double dFirstQty = 0;
            //double iQty = 0;
            //string sSql = "";

            //sSql = "SELECT SUM(CASE WHEN ins_type='I' THEN pro_qty ELSE pro_qty*-1 END) AS first_qty" +
            //       "  FROM WMT0200 " +
            //       " WHERE convert(nvarchar,ins_date,111)<='" + sCaldateS + "'" +
            //       "   AND pro_code='" + sProCode + "'";
            //if (!string.IsNullOrEmpty(sLotno)) { sSql += " AND lot_no='" + sLotno + "'"; }
            //if (!string.IsNullOrEmpty(sLocCode)) { sSql += " AND loc_code='" + sLocCode + "'"; }
            //DataTable dtTmp = comm.Get_DataTable(sSql);

            //int i;
            //for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            //{
            //    dFirstQty = comm.sGetfloat(dtTmp.Rows[i]["first_qty"].ToString());
            //}

            ////抓取入出庫記錄內容
            //sSql = "SELECT a.*,b.pro_name FROM WMT0200 a" +
            //       "  LEFT JOIN MEB20_0000 b ON a.pro_code=b.pro_code" +
            //       " WHERE convert(nvarchar,a.ins_date,111) <='" + sCaldateS + "'" +
            //       "   AND a.pro_code='" + sProCode + "' AND a.pro_qty<>0";
            //if (!string.IsNullOrEmpty(sLotno)) { sSql += " AND lot_no='" + sLotno + "'"; }
            //if (!string.IsNullOrEmpty(sLocCode)) { sSql += " AND loc_code='" + sLocCode + "'"; }
            //sSql += " ORDER BY a.ins_date";

            //comm.Ins_BDP20_0000("admin", "RPT010A", "RPT", sSql);

            //dtTmp = comm.Get_DataTable(sSql);
            //for (i = 0; i < dtTmp.Rows.Count; i++)
            //{
            //    RPT010A data = new RPT010A();
            //    data.pro_code = dtTmp.Rows[i]["pro_code"].ToString();
            //    data.pro_name = dtTmp.Rows[i]["pro_name"].ToString();
            //    data.lot_no = dtTmp.Rows[i]["lot_no"].ToString();
            //    data.loc_code = dtTmp.Rows[i]["loc_code"].ToString();
            //    data.loc_name = comm.Get_QueryData("WMB02_0000", data.loc_code, "loc_code", "loc_name");
            //    data.ins_type = dtTmp.Rows[i]["ins_type"].ToString();
            //    data.ins_date = dtTmp.Rows[i]["ins_date"].ToString();
            //    data.pro_qty = comm.sGetfloat(dtTmp.Rows[i]["pro_qty"].ToString());

            //    switch (data.ins_type)
            //    {
            //        case "I":
            //            data.ins_type = "入庫";
            //            iQty += data.pro_qty;
            //            break;
            //        case "O":
            //            data.ins_type = "出庫";
            //            data.pro_qty = data.pro_qty * -1;
            //            iQty += data.pro_qty;
            //            break;
            //        default:
            //            data.ins_type = "未知";
            //            break;
            //    }

            //    data.sto_qty = iQty + dFirstQty;
            //    result.Add(data);
            //}

            return result;
        }

        /// <summary>
        /// 良品率
        /// </summary>
        /// <param name="pOkQty"></param>
        /// <param name="pNgQty"></param>
        /// <returns></returns>
        private string Get_ProYield(string pOkQty, string pNgQty)
        {
            string sProYield = "";
            try
            {
                double dOkQty = double.Parse(pOkQty);
                double dNgQty = double.Parse(pNgQty);
                if (dOkQty == 0)
                {
                    return 0.ToString("0.%");
                }
                sProYield = (dOkQty / (dOkQty + dNgQty)).ToString("0.##%");

                return sProYield;
            }
            catch (Exception ex)
            {
                return 0.ToString("0.%");
            }

        }
        private string Get_Utilization(string pWorkStartTime,string pWorkEndTime, string pMacCode)
        {
            //機器稼動率公式=實際工時/計畫工時

            try
            {
                //每日計畫工時
                string sSql = "";
                string sStopSql = "";
                string sMacCode = pMacCode;
               
                sSql = "select std_time From MEB15_0000" +
                       " where mac_code ='" + pMacCode + "'";
                DataTable dtTmp = comm.Get_DataTable(sSql);
                if (dtTmp.Rows.Count > 0)
                {
                    DateTime dWorkEndTime = DateTime.Parse(pWorkEndTime);
                    DateTime dWorkStartTime = DateTime.Parse(pWorkStartTime);
                    string sWorkEndTime = pWorkEndTime;

                    double StopMinCount = 0;//總共停機時間
                    //除外時間查詢，工單開始時間到現在的停機時間
                    sStopSql = " Select * from MED04_0000" +
                               " where  mac_code='" + sMacCode + "'" +
                                " and (date_s+' '+time_s)>'" + dWorkStartTime.ToString("yyyy/MM/dd HH:mm:ss") + "'";
                    DataTable dtStopTmp = comm.Get_DataTable(sStopSql);
                    if (dtStopTmp.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtStopTmp.Rows)
                        {
                            DateTime dStartTime = DateTime.Parse(row["time_s"].ToString());
                            DateTime dEndTime = DateTime.Now;
                            if (row["time_e"].ToString() != "")
                            {
                                dEndTime = DateTime.Parse(row["time_e"].ToString());
                            }
                            TimeSpan dDiffTime = dEndTime - dStartTime;
                            StopMinCount += dDiffTime.TotalMinutes;
                        }
                    }

                    double dPlanTime = double.Parse(dtTmp.Rows[0]["std_time"].ToString());
                    if (dPlanTime == 0)
                    {
                        return 0.ToString("0.%");
                    }
                    //實際工時
                    if (dWorkEndTime < dWorkStartTime)
                    {

                        //實際工時
                      
                        DateTime dNowTime = DateTime.Now;
                        TimeSpan diffTime = dNowTime - dWorkStartTime;
                        double dUtil1 = diffTime.TotalMinutes - StopMinCount;
                        double dUtil2 = diffTime.TotalMinutes;
                        //稼動率
                        double tUtilization = dUtil1 / dUtil2;
                        if (tUtilization > 1)
                        {
                            tUtilization = 1;
                        }
                        return tUtilization.ToString("0.##%");
                    }
                    else
                    {

                        TimeSpan diffTime = dWorkEndTime - dWorkStartTime;
                        double dUtil1 = diffTime.TotalMinutes - StopMinCount;
                        double dUtil2 = diffTime.TotalMinutes;
                        //稼動率
                        double tUtilization = dUtil1 / dUtil2;
                        if (tUtilization > 1)
                        {
                            tUtilization = 1;
                        }
                        return tUtilization.ToString("0.##%");
                    }
                }
                else
                {
                    return 0.ToString("0.%");
                }

                return 0.ToString("0.%");
            }
            catch (Exception ex)
            {
                return 0.ToString("0.%");
            }
        }
        private string Get_CURate(string pOkQty, string pNgQty, string pMacCode, string pWorkStartTime,string pWorkEndTime,string pStoQty)
        {
            //產能利用率=((良品數+不良品數)/實際工作時間)/(計畫工作產能/計畫工作時間)
            try
            {
                //每日計畫工時
                string sSql = "";
                sSql = "select std_time,std_qty From MEB15_0000" +
                       " where mac_code ='" + pMacCode + "'";
                DataTable dtTmp = comm.Get_DataTable(sSql);
                if (dtTmp.Rows.Count > 0)
                {
                    double dPlanTime = double.Parse(dtTmp.Rows[0]["std_time"].ToString());
                    double dPlanQty = double.Parse(dtTmp.Rows[0]["std_qty"].ToString());
                    double dOkQty = double.Parse(pOkQty);
                    double dNgQty = double.Parse(pNgQty);
                    if ((dOkQty + dNgQty) == 0 || dPlanQty == 0 || dPlanTime == 0)
                    {
                        return 0.ToString("0.%");
                    }
                    //實際工時
                    DateTime dWorkStartTime = DateTime.Parse(pWorkStartTime);
                    DateTime dWorkEndTime = DateTime.Parse(pWorkEndTime);
                    DateTime dEndTime = DateTime.Now;
                    if (dWorkStartTime > dWorkEndTime)
                    {
                        dEndTime = DateTime.Now;
                    }
                    else
                    {
                         dEndTime = DateTime.Parse(pWorkEndTime);
                    }
                   
                    TimeSpan diffTime = dEndTime - dWorkStartTime;


                    double tmpCU1 = ((dOkQty + dNgQty) / diffTime.TotalMinutes);
                    double tmpCU2 = double.Parse(pStoQty);
                    ////產能利用率
                    double CURate = tmpCU1 / tmpCU2;
                    ////假設產能利用率>1，則回傳100%
                    if (CURate > 1)
                    {
                        CURate = 1;
                    }
                    //產能利用率
                    //double CURate = ((dOkQty + dNgQty) / diffTime.TotalHours) / (dPlanQty / dPlanTime);

                    return CURate.ToString("0.##%");
                }
            }
            catch (Exception ex)
            {
                return 0.ToString("0.%");
            }
            return 0.ToString("0.%");
        }
        /// <summary>
        /// 標準工時
        /// </summary>
        /// <param name="pStationCode"></param>
        /// <param name="pProCode"></param>
        /// <returns></returns>
        public string Get_StrQtyMEB2001(string pStationCode, string pProCode)
        {
            double sStdQty = 0;
            double sStdTime = 0;
            string sFinalStd = "";
            string sSql = "";
            sSql = "select std_qty,std_time From MEB20_0100" +
                   " where station_code ='" + pStationCode + "'" +
                   "   and pro_code='" + pProCode + "'";
            DataTable dtTmp = comm.Get_DataTable(sSql);

            if (dtTmp.Rows.Count > 0)
            {
                //sReturn = dtTmp.Rows[0]["pFieldCode"].ToString().Trim();
                try
                {
                    sStdQty = double.Parse(dtTmp.Rows[0]["std_qty"].ToString().Trim());
                    sStdTime = double.Parse(dtTmp.Rows[0]["std_time"].ToString().Trim());
                    if (sStdQty != 0 && sStdTime != 0)
                    {
                        sFinalStd = (sStdQty / 60).ToString("0.##");
                    }
                    return sFinalStd;
                }
                catch (Exception ex)
                {
                    return sFinalStd;
                }
            }

            return sFinalStd;

        }
        /// <summary>
        /// 實際工時計算
        /// </summary>
        /// <param name="pWorkStartTime"></param>
        /// <param name="pWorkEndTime"></param>
        /// <returns></returns>
        private string Get_ActMin(string pWorkStartTime ,string pWorkEndTime)
        {

            try
            {
                DateTime dWorkStartTime = DateTime.Parse(pWorkStartTime);
                DateTime dWorkEndTime = DateTime.Parse(pWorkEndTime);
                TimeSpan diffTime = dWorkEndTime - dWorkStartTime;
                return diffTime.TotalMinutes.ToString("0.##");
            }
            catch (Exception ex)
            {
                return "";
            }
           
        }

    }
}