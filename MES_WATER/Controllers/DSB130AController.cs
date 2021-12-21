using MES_WATER.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;


namespace MES_WATER.Controllers
{
    public class DSB130AController : JsonNetController
    {
        Comm comm = new Comm();

        // GET: DSB130A
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult Get_All_LineData(List<string> pTkCode_List)
        {
            Dictionary<string, object> returnObj = new Dictionary<string, object>();

            // 若在極短的時間內頻繁叫用，會使得Random物件的亂數種子皆相同，因此得到完全相同的亂數組。
            //Random rand = new Random();
            try
            {
                foreach (string pTkCode in pTkCode_List)
                {
                    object data = Get_LineData(pTkCode);


                    returnObj.Add(pTkCode, data);
                }

                return Json(returnObj);


            }
            catch (Exception e)
            {
                return Json("");
            }

        }

        public DataTable Get_Data()
        {
            DataTable dtDat = new DataTable();
            string sSql = "";

            dtDat.Columns.Add("mac_code", System.Type.GetType("System.String"));
            //取得當前已投產的機台
            sSql = "select mac_code from MEM01_0000 " +
                    "where work_time_s<GETDATE() and mac_code!=''" +
                    " and (work_time_e=''  or DateDiff(dd,work_time_e,getdate())=0)" +
                    " group by MEM01_0000.mac_code";
            ////取得所有機台
            //sSql = "select mac_code from MEB29_0000";
            var dtTmp = comm.Get_DataTable(sSql);
            int i;
            for (i = 0; i < dtTmp.Rows.Count; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["mac_code"] = dtTmp.Rows[i]["mac_code"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        private object Get_LineData(string pTkCode)
        {

            //Random rand = pRand;
            List<object> heading1_items = new List<object>();
            List<object> heading2_items = new List<object>();
            List<object> cell_items = new List<object>();
            List<object> oee_items = new List<object>();
            List<object> alarmMsg = new List<object>();
            string gauge_value = "";


            string sMacCode = pTkCode.ToUpper();
            string sMoCode = Get_DataByMacCode(sMacCode, "mo_code");
            string sMacName = Get_MacNameByMacCode(sMacCode, "mac_name");

            string sWorkTimeS = Get_DataByMacCode(sMacCode, "work_time_s");

            string sProCode = Get_ProCodeByMoCode(sMoCode, "pro_code");
            string sProName = comm.Get_QueryData("MEB20_0000", sProCode, "pro_code", "pro_name");

            string sStationCode = comm.Get_QueryData("MEB29_0000", sMacCode, "mac_code", "station_code");
            string sWorkCode = comm.Get_QueryData("MEB30_0100", sStationCode, "station_code", "work_code");
            string sWorkName = comm.Get_QueryData("MEB30_0000", sWorkCode, "work_code", "work_name");

            //string sPlanQty = comm.sGetDecimal(Get_ProCodeByMoCode(sMoCode, "plan_qty")).ToString("0.");//計畫產量
            string sPlanQty = Get_WMT07ProQty(sMoCode, sWorkCode);//計畫產量




            string sOkQty = comm.sGetDecimal(Get_DataByMacCode(sMacCode, "ok_qty")).ToString("0.");//良品量
            string sNgQty = comm.sGetDecimal(Get_DataByMacCode(sMacCode, "ng_qty")).ToString("0.");//不良品量

            string sStdQTy = Get_StrQtyMEB2001(sStationCode, sProCode);//標準產量
            string sStdUnit = Get_StrUnitMEB2001(sStationCode, sProCode);

            string sEstimate = Get_Estimate(sPlanQty, sOkQty, sStdQTy);//預估完工時間
            string sAvgQty = Get_AvgQty(sWorkTimeS, sOkQty);//平均工時

            string sProYield = Get_ProYield(sOkQty, sNgQty);//生產良品率
            string sUtilization = Get_Utilization(sWorkTimeS, sMacCode, sWorkCode, sMoCode);//機器稼動率
            string sCURate = Get_CURate(sOkQty, sNgQty, sMacCode, sWorkTimeS, sStdQTy);//產能利用率

            string sAlarmMsg = Get_AlmMsg(sMacCode);


            sMacName = Chk_String(sMacName);
            sMoCode = Chk_String(sMoCode);
            sStdQTy = Chk_Digital(sStdQTy);
            if (sAlarmMsg == "Y")
            {
                sAlarmMsg = "警告";
            }
            gauge_value = Get_Gauge(sUtilization, sCURate, sProYield);


            heading1_items.Add(new { name = "", value = sMoCode, label = sMacName, });

            heading2_items.Add(new { name = "", value = sProName, label = "產品名稱", });
            heading2_items.Add(new { name = "", value = sWorkName, label = "工序名稱" });

            cell_items.Add(new { name = "", value = sPlanQty, label = "計劃產量 (個)" });
            cell_items.Add(new { name = "", value = sOkQty, label = "良品 (個)" });
            cell_items.Add(new { name = "", value = sNgQty, label = "不良品 (個)" });

            cell_items.Add(new { name = "", value = sStdQTy, label = "標準產量(" + sStdUnit + "/分)" });
            cell_items.Add(new { name = "", value = sAvgQty, label = "實際產量 (" + sStdUnit + "/分)" });
            cell_items.Add(new { name = "", value = sEstimate, label = "預估完工 (時)" });

            // 用ToString("P0")多兩個0
            oee_items.Add(new { name = "", value = sUtilization, label = "設備稼動率" });
            oee_items.Add(new { name = "", value = sCURate, label = "產能利用率" });
            oee_items.Add(new { name = "", value = sProYield, label = "生產良品率" });

            alarmMsg.Add(new { value = sAlarmMsg });
            //switch (pTkCode.ToUpper())
            //{
            //    case "A1L01001": //第一台機器
            //        gauge_value = 35;

            //        break;
            //    case "A02": //第二台機器
            //        gauge_value = 60;
            //        break;
            //    case "A03": //第三台機器

            //        gauge_value = 70;
            //        break;
            //    case "A04": //第四台機器

            //        gauge_value = 95;
            //        break;
            //    default:
            //        break;

            //}

            object gauge_params = new
            {
                value = gauge_value,    // rand.Next(20, 100)
                options = new List<object>() {
                        new {
                            baseLine = 40,
                            name = "不良",
                            color = "#ff624b",
                            title_color = "#ff624b",
                            title_backgroundColor = "rgba(255, 98, 75, 0.4)",
                        },
                        new {
                            baseLine = 70,
                            name = "普通",
                            color = "#ffca36",
                            title_color = "#ffca36",
                            title_backgroundColor = "rgba(255, 202, 54, 0.4)",
                        },
                        new {
                            baseLine = 110,
                            name = "良好",
                            color = "#63F371",
                            title_color = "#63F371",
                            title_backgroundColor = "rgba(99, 243, 113, 0.4)",
                        },

                    }
            };





            object data = new
            {
                heading1_items = heading1_items,
                heading2_items = heading2_items,
                cell_items = cell_items,
                gauge_params = gauge_params,
                oee_items = oee_items,
                alarmMsg = alarmMsg
            };

            return data;
        }

        public PartialViewResult LineItem(string id, string imgSrc)
        {
            ViewBag.imgSrc = imgSrc;
            ViewBag.id = id;
            return PartialView();
        }

        public PartialViewResult LineItem_Usr(string id, string imgSrc, string usr_name, string usr_code)
        {
            string alarm = "";
            string alarmMsg = "";
            alarm = Get_AlmMsg(id);
            if (alarm == "Y")
            {
                if (alarmMsg == "")
                {
                    usr_name = "警告";
                }
                else
                {
                    usr_name = alarmMsg;
                }
            }


            ViewBag.id = id;
            usr_name = "123";
            ViewBag.usr_name = usr_name;

            ViewBag.usr_code = usr_code;

            ViewBag.imgSrc = imgSrc;

            return PartialView();
        }

        public string Get_DataByMacCode(string pMacCode, string pFieldName)
        {
            string sReturn = "";
            string sSql = "";

            sSql = "Select top 1 * from MEM01_0000 " +
                   " where MEM01_0000.mac_code ='" + pMacCode + "'" +
                    "    and MEM01_0000.mac_code !=''" +
                    "    and MEM01_0000.work_time_s <> ''" +
                    "    and MEM01_0000.work_time_e = ''" +
                    " order by MEM01_0000.work_time_s desc";
            DataTable dtTmp = comm.Get_DataTable(sSql);

            if (dtTmp.Rows.Count > 0)
            {
                //sReturn = dtTmp.Rows[0]["pFieldCode"].ToString().Trim();
                try
                {
                    sReturn = dtTmp.Rows[0][pFieldName].ToString().Trim();
                }
                catch (Exception ex)
                {
                    sReturn = "";
                }
            }
            else
            {
                sReturn = "";
            }
            return sReturn;

        }

        public string Get_MacNameByMacCode(string pMacCode, string pFieldName)
        {
            string sReturn = "";
            string sSql = "";
            sSql = "select * From MEB15_0000" +
                   " where mac_code ='" + pMacCode + "'";
            DataTable dtTmp = comm.Get_DataTable(sSql);

            if (dtTmp.Rows.Count > 0)
            {
                //sReturn = dtTmp.Rows[0]["pFieldCode"].ToString().Trim();
                try
                {
                    sReturn = dtTmp.Rows[0]["mac_name"].ToString().Trim();
                }
                catch (Exception ex)
                {
                    sReturn = "";
                }
            }
            else
            {
                sReturn = "";
            }
            return sReturn;

        }
        public string Get_ProCodeByMoCode(string pMoCode, string pFieldName)
        {
            string sReturn = "";
            string sSql = "";
            sSql = "select * From MET01_0000" +
                   " where mo_code ='" + pMoCode + "'";
            DataTable dtTmp = comm.Get_DataTable(sSql);

            if (dtTmp.Rows.Count > 0)
            {
                //sReturn = dtTmp.Rows[0]["pFieldCode"].ToString().Trim();
                try
                {
                    sReturn = dtTmp.Rows[0][pFieldName].ToString().Trim();
                }
                catch (Exception ex)
                {
                    sReturn = "";
                }
            }
            else
            {
                sReturn = "";
            }
            return sReturn;

        }
        public string Get_StationCodeByMacCode(string pMacCode, string pFieldName)
        {
            string sReturn = "";
            string sSql = "";
            sSql = "select * From MEB29_0000" +
                   " where mac_code ='" + pMacCode + "'";
            DataTable dtTmp = comm.Get_DataTable(sSql);

            if (dtTmp.Rows.Count > 0)
            {
                //sReturn = dtTmp.Rows[0]["pFieldCode"].ToString().Trim();
                try
                {
                    if (pFieldName == "work_time_s")
                    {
                        sReturn = DateTime.Parse(dtTmp.Rows[0][pFieldName].ToString()).ToString("yyyy/MM/dd HH:mm:ss");
                    }
                    else
                    {
                        sReturn = dtTmp.Rows[0][pFieldName].ToString().Trim();
                    }

                }
                catch (Exception ex)
                {
                    sReturn = "";
                }
            }
            else
            {
                sReturn = "";
            }
            return sReturn;

        }
        /// <summary>
        /// 標準工時
        /// </summary>
        /// <param name="pStationCode"></param>
        /// <param name="pProCode"></param>
        /// <returns></returns>
        public string Get_StrQtyMEB2001(string pStationCode, string pProCode)
        {
            int sStdQty = 0;
            int sStdTime = 0;
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
                    sStdQty = int.Parse(dtTmp.Rows[0]["std_qty"].ToString().Trim());
                    sStdTime = int.Parse(dtTmp.Rows[0]["std_time"].ToString().Trim());
                    if (sStdQty != 0 && sStdTime != 0)
                    {
                        sFinalStd = (sStdQty / 60).ToString();
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
        /// 取得標準工時單位
        /// </summary>
        /// <param name="pStationCode"></param>
        /// <param name="pProCode"></param>
        /// <returns></returns>
        public string Get_StrUnitMEB2001(string pStationCode, string pProCode)
        {

            string sFinalStd = "";
            string sSql = "";
            sSql = "select pro_unit From MEB20_0100" +
                   " where station_code ='" + pStationCode + "'" +
                   "   and pro_code='" + pProCode + "'";
            DataTable dtTmp = comm.Get_DataTable(sSql);

            if (dtTmp.Rows.Count > 0)
            {
                //sReturn = dtTmp.Rows[0]["pFieldCode"].ToString().Trim();
                try
                {


                    sFinalStd = dtTmp.Rows[0]["pro_unit"].ToString();

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
        /// 平均工時
        /// </summary>
        /// <param name="pWorkStatTime"></param>
        /// <param name="pProQty"></param>
        /// <returns></returns>
        public string Get_AvgQty(string pWorkStatTime, string pProQty)
        {
            string sFinalStd = "";
            try
            {
                DateTime dWorkStartTime = DateTime.Parse(pWorkStatTime);
                DateTime dNowTime = DateTime.Now;
                TimeSpan diffTime = dNowTime - dWorkStartTime;
                double iProQty = double.Parse(pProQty);
                double diffT = diffTime.TotalMinutes;
                if (iProQty != 0 && diffT != 0)
                {
                    sFinalStd = (iProQty / diffT).ToString("0.");
                }

                return sFinalStd;
            }
            catch (Exception ex)
            {
                return 0.ToString();
            }

        }
        /// <summary>
        /// 預估完成時間
        /// </summary>
        /// <param name="sPlanQty"></param>
        /// <param name="sOkQty"></param>
        /// <param name="sStdQTy"></param>
        /// <returns></returns>
        public string Get_Estimate(string sPlanQty, string sOkQty, string sStdQTy)
        {
            string sFinalStd = "";
            try
            {
                double diffQty = (double.Parse(sPlanQty) - double.Parse(sOkQty)) / double.Parse(sStdQTy);

                return DateTime.Now.AddSeconds(diffQty).ToString("HH:mm"); ;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// 良品率
        /// </summary>
        /// <param name="pOkQty"></param>
        /// <param name="pNgQty"></param>
        /// <returns></returns>
        public string Get_ProYield(string pOkQty, string pNgQty)
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
        public string Get_Utilization(string pWorkStartTime, string pMacCode, string pWorkCode, string pMoCode)
        {
            //機器稼動率公式=實際工時/計畫工時

            try
            {
                //每日計畫工時
                string sSql = "";
                string sStopSql = "";//除外工時SQL
                string sMoCode = pMoCode;
                string sMacCode = pMacCode;

                DateTime WorkStartTime = DateTime.Parse(pWorkStartTime);

                sSql = "select std_time From MEB15_0000" +
                       " where mac_code ='" + pMacCode + "'";
                DataTable dtTmp = comm.Get_DataTable(sSql);
                if (dtTmp.Rows.Count > 0)
                {
                    double StopMinCount = 0;//總共停機時間
                    //除外時間查詢，工單開始時間到現在的停機時間
                    sStopSql = " Select * from MED04_0000" +
                               " where  mac_code='" + sMacCode + "'" +
                                " and (date_s+' '+time_s)>'" + WorkStartTime.ToString("yyyy/MM/dd HH:mm:ss") + "'";
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
                    DateTime dWorkStartTime = DateTime.Parse(pWorkStartTime);
                    DateTime dNowTime = DateTime.Now;
                    TimeSpan diffTime = dNowTime - dWorkStartTime;
                    double dUtil1 = diffTime.TotalMinutes - StopMinCount;
                    double dUtil2 = diffTime.TotalMinutes;
                    //稼動率
                    double tUtilization = dUtil1 / dUtil2;

                    //return tUtilization.ToString("0.##%");
                    return tUtilization.ToString("0.##%");
                }
                else
                {
                    return 0.ToString("0.%");
                }

                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        /// <summary>
        /// 傳入良品、不良品、機器編號、開工時間、標準產量，回傳產能利用率
        /// </summary>
        /// <param name="pOkQty"></param>
        /// <param name="pNgQty"></param>
        /// <param name="pMacCode"></param>
        /// <param name="pWorkStartTime"></param>
        /// <param name="pStoQty"></param>
        /// <returns></returns>
        public string Get_CURate(string pOkQty, string pNgQty, string pMacCode, string pWorkStartTime, string pStoQty)
        {
            //產能利用率=((良品數+不良品數)/實際工作時間)/標準產量
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
                    //實際產能
                    DateTime dWorkStartTime = DateTime.Parse(pWorkStartTime);
                    DateTime dNowTime = DateTime.Now;
                    TimeSpan diffTime = dNowTime - dWorkStartTime;
                    double tmpCU1 = ((dOkQty + dNgQty) / diffTime.TotalMinutes);
                    //double tmpCU2 = (dPlanQty /60) ;
                    double tmpCU2 = double.Parse(pStoQty);
                    //產能利用率
                    double CURate = tmpCU1 / tmpCU2;
                    //假設產能利用率>1，則回傳100%
                    if (CURate > 1)
                    {
                        CURate = 1;
                    }
                    return CURate.ToString("0.##%");
                }
            }
            catch (Exception ex)
            {
                return 0.ToString("0.%");
            }
            return 0.ToString("0.%");
        }
        public string Get_Gauge(string pUtilization, string pProYield, string pCURate)
        {
            try
            {
                double dUti = 0;
                double dProYield = 0;
                double dCURate = 0;

                if (pUtilization.EndsWith("%")) dUti = double.Parse(pUtilization.TrimEnd('%')) / 100;
                if (pProYield.EndsWith("%")) dProYield = double.Parse(pProYield.TrimEnd('%')) / 100;
                if (pCURate.EndsWith("%")) dCURate = double.Parse(pCURate.TrimEnd('%')) / 100;

                double dGauge = dUti * dProYield * dCURate * 100;

                return dGauge.ToString("0.##");
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        /// <summary>
        /// 輸入機器號，確認是否發警報
        /// </summary>
        /// <param name="pMacCode"></param>
        /// <returns></returns>
        public string Get_AlmMsg(string pMacCode)
        {
            string alarm = "";
            string alarmMsg = "";
            try
            {
                string sSql = "";
                sSql = "select top 1 * from AMB02_0000 " +
                        "where mac_code='" + pMacCode + "'" +
                        " order by alm_date desc,time_start desc";
                DataTable dtTmp = comm.Get_DataTable(sSql);
                if (dtTmp.Rows.Count > 0)
                {
                    string sdate = dtTmp.Rows[0]["alm_date"].ToString();
                    if (sdate == DateTime.Now.ToString("yyyy/MM/dd"))
                    {
                        DateTime sAlmTimeS = DateTime.Parse(dtTmp.Rows[0]["time_start"].ToString());
                        if (dtTmp.Rows[0]["time_end"].ToString() != "")
                        {
                            DateTime sAlmTimeE = DateTime.Parse(dtTmp.Rows[0]["time_end"].ToString());
                            if (DateTime.Now < sAlmTimeE && sAlmTimeS < DateTime.Now)
                            {
                                alarm = "Y";
                                alarmMsg = dtTmp.Rows[0]["alm_message"].ToString();
                            }
                        }
                        else
                        {
                            if (sAlmTimeS < DateTime.Now)
                            {
                                alarm = "Y";
                                alarmMsg = dtTmp.Rows[0]["alm_message"].ToString();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                alarm = "";
            }
            if (alarmMsg != "")
            {
                alarm = alarmMsg;
            }
            return alarm;
        }

        private string Chk_String(string pString)
        {
            string sString = pString;
            if (sString == "")
            {
                sString = "　";
            }
            return sString;
        }
        private string Chk_Digital(string pDigital)
        {
            string sDigital = pDigital;
            if (sDigital == "")
            {
                sDigital = "0";
            }
            return sDigital;
        }
        private string Get_WMT07ProQty(string pMoCode, string pWorkCode)
        {
            string sSql = "";
            sSql = "select pro_qty From WMT07_0000" +
                   " where mo_code ='" + pMoCode + "'" +
                   "   and work_code='" + pWorkCode + "'";
            DataTable dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                string sReturnData = double.Parse(dtTmp.Rows[0]["pro_qty"].ToString()).ToString("0.");
                return sReturnData;
            }
            return "";
        }

    }


}