using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MES_WATER.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Web.UI.WebControls;

namespace MES_WATER.Controllers
{
    public class DSB110BController : JsonNetController
    {
        Comm comm = new Comm();
        string sPrgCode = "DSB110B";
        // GET: DSB100a
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult MoreItem() => PartialView();
        /// <summary>
        /// 單一機台的html，在Index的js有用到
        /// </summary>
        /// <returns></returns>
        public PartialViewResult MacItem()
        {
            return PartialView();
        }


        /// <summary>
        /// 取得機台清單
        /// </summary>
        /// <returns></returns>
        public JsonResult Init_Get_MacCodeList(string order, string line_code , string Item)
        {
            //抓取機台
            List<string> mac_code_list = new List<string>() { };
            //設定資料
            List<MEB15_0000> list = new List<MEB15_0000>();
            //設定SQL
            string sSql = "Select mac_code from MEB15_0000 with (nolock)";
            //判斷是否有線別內容
            if (line_code != "")  sSql += " where line_code=@line_code";

            //存入 List
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                mac_code_list = con_db.Query<string>(sSql, new { line_code }).ToList();
                if (Item != "")
                {
                    foreach (string macCode in (from MacCode in mac_code_list select MacCode).ToList())
                    {
                        if (!Item.Contains(macCode)) { mac_code_list.Remove(macCode); }
                    }
                }
                if(Item == "") { mac_code_list.Clear(); }
                //將相關mac_code資料寫入
                sSql = "Select * from MEB15_0000 with (nolock) where mac_code in @mac_code_list";
                list = con_db.Query<MEB15_0000>(sSql, new { mac_code_list }).ToList();
            }
            //List 內容排序
            if (!string.IsNullOrEmpty(order))
            {
                switch (order)
                {
                    case "mac_code": list = list.OrderBy(l => l.mac_code).ToList(); break;
                    case "mac_name": list = list.OrderBy(l => l.mac_name).ToList(); break;
                }
            }
            //轉Json
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Init_Get_MoreItemList(string mac_code, string dates,string count="10")
        {
            string sDate = DateTime.Now.AddDays(~comm.sGetInt32(dates)).ToString("yyyy/MM/dd");
            //string sSql = @"
            //    SELECT  top "+ count  + @"
            //  ROW_NUMBER() OVER(ORDER BY a.sch_date_s ,a.sch_date_e  ,a.wrk_code  ) AS #,
            //  a.sch_date_s AS '開工時間',   
            //  a.wrk_code  as '派工單號',
            //  a.pro_code as '產品',a.pro_qty AS '計畫',		
            //        (case a.mo_status 
            //   when 'NONE' then '_'
            //   when 'END' then '完成'
            //   when 'IN' then '開工'
            //   when 'STOP' then '暫停'
            //   END)  as '狀態' 
            //        ,ISNULL((SELECT sum(pro_qty) FROM MED09_0000 WHERE  wrk_code=a.wrk_code),0) AS '數量' 
            //        /*,case f.mac_code when '' then e.mac_code else f.mac_code end   as '機台'*/   /*以 mem01 實際進站開工為優先, 若無才取met03 */
            //    FROM MET03_0000 a
            //        LEFT JOIN MEB20_0000 c ON a.pro_code = c.pro_code
            //        LEFT JOIN MEM01_0000 f on(a.work_code = f.work_code and a.mo_code = f.mo_code)
            //    WHERE  a.sch_date_s >= @sch_date_s   AND  case f.mac_code when '' then a.mac_code else f.mac_code end = @mac_code
            //    Group  by a.pro_code,a.mo_status,f.ok_qty,a.mo_code,a.wrk_code,a.pro_qty,a.mac_code, f.mac_code, a.sch_date_s,a.sch_date_e  ";

            string sSql = @"
                SELECT  top " + count + @"
		            ROW_NUMBER() OVER(ORDER BY a.seq_no  ) AS #,
		            a.mo_code as '工單單號',c.day_target_qty AS '計畫',		
                    (case a.MO_STATUS 
                        when 'W' then '待排程'
			            when '0' then '待生產'
			            when '1' then '生產中'
			            when '2' then '結案'
			            when '3' then '強制結案'
			            END)  as '狀態' ,
						(case 
						when a.MO_STATUS ='1' then (SELECT TOP (1) SUM(m.QTY) AS QTY  FROM mba_e10 m   WHERE Convert(varchar,m.TRANSACTION_DATE,23) = Convert(varchar,GETDATE(),23) and  XMACHINE_CODE = @MACHINE_CODE) 
						when a.MO_STATUS ='0' then '0'
						END) as '產量'
--Convert(varchar,m.TRANSACTION_DATE,23) = Convert(varchar,GETDATE(),23) and
                    /*,ISNULL(a.QTY,0) AS '產量' */
                    /*,case f.mac_code when '' then e.mac_code else f.mac_code end   as '機台'*/   /*以 mem01 實際進站開工為優先, 若無才取met03 */
                FROM MET02_0000 a
                left join MEB12_0000 c on c.line_code = a.MACHINE_CODE
                WHERE  a.MO_STATUS in ('0', '1') AND a.MACHINE_CODE = @MACHINE_CODE order by a.seq_no ";
            //a.ins_date = @sch_date_s AND a.MO_STATUS in ('0', '1') AND

            DataTable dtTmp = comm.Get_DataTable(sSql,new {sch_date_s = DateTime.Now.ToString("yyyy-MM-dd"), MACHINE_CODE = mac_code });
            return Json(dtTmp, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Get_LineCodeList()
        {
            //抓取線別
            List<string> line_code_list = new List<string>() { };

            string sSql = "Select distinct line_code from MEB12_0000";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                line_code_list = con_db.Query<string>(sSql).ToList();
            }
            comm.Ins_BDP20_0000(User.Identity.Name, sPrgCode, "select", "", "");
            return Json(line_code_list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ajax更新資料
        /// </summary>
        /// <param name="pJson"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Get_OeeData(string pJson)
        {
            //try
            //{
                List<string> mac_code_list = JsonConvert.DeserializeObject<List<string>>(pJson);

                Dictionary<string, Oee> returnObj = new Dictionary<string, Oee>();
                foreach (string mac_code in mac_code_list)
                {
                    string sMoCode = Get_DataByStationCode(mac_code, "mo_code");

                    string sWrkCode = Get_DataByStationCode(mac_code, "OPERATION_CODE");
                    string spec_c = Get_DataByStationCode(mac_code, "spec_c");
                    //string sor_code = Get_DataByStationCode(mac_code, "sor_code");
                    string spec_a = Get_DataByStationCode(mac_code, "spec_a");
                    //double ing_qty = comm.sGetDouble(Get_DataByStationCode(mac_code, "SCRAP_QTY"));
                    //double irate = comm.sGetDouble(Get_DataByStationCode(mac_code, "irate"));
                    //double MAX_QTY = comm.sGetDouble(Get_DataByStationCode(mac_code, "MAX_QTY"));
                    //double Min_QTY = comm.sGetDouble(Get_DataByStationCode(mac_code, "Min_QTY"));
                    double QTY = comm.sGetDouble(Get_DataByStationCode(mac_code, "QTY"));
                    double hour_count = comm.sGetDouble(Get_DataByStationCode(mac_code, "hour_count"));
                    double pro_uph = comm.sGetDouble(Get_DataByStationCode(mac_code, "pro_uph"));
                    double day_target_qty = comm.sGetDouble(Get_DataByStationCode(mac_code, "day_target_qty"));


                //string sUsrName = Get_DataByStationCode(seq_no, "usr_code");
                    string sUsrName = "";
                    string sWorkTime = Get_DataByStationCode(mac_code, "ins_date");

                DataTable dt = comm.Get_DataTable("select top 1 * from MBA_E00 where MO_DOC_NO='" + sMoCode + "' order by TRANSACTION_DATE");
                if (dt.Rows.Count > 0)
                {
                    sWorkTime = dt.Rows[0]["TRANSACTION_DATE"].ToString();
                }
                //sUsrName = comm.Get_QueryData("BDP08_0000", sUsrName, "usr_code", "usr_name") + "(" + Get_UserCodeByMacCodeCount(work_code) + ")";
                double stop_time = 0;
                string key = "";
                //DataTable dt2 = comm.Get_DataTable(@"SELECT a.COLUMN_NAME+3 as 'key' FROM MEB15_0000 m 
                //       LEFT JOIN INFORMATION_SCHEMA.COLUMNS a  on a.TABLE_NAME = 'MEA_E02' and a.COLUMN_NAME = m.address_code
                //       where m.mac_code ='" + mac_code + "'");
                //if (dt2.Rows.Count>0)
                //{
                //    key = dt2.Rows[0]["key"].ToString();
                //}
                if (mac_code== "1001-M1" || mac_code == "1001-M1") { 
                    DataTable dt3 = comm.Get_DataTable(@"select TOP 1 * from MEA_E02 Where MACHINE_CODE='"+ mac_code + "' order by update_at DESC");
                    if (dt3.Rows.Count > 0 )
                    {
                        stop_time += comm.sGetDouble(dt3.Rows[0]["300185"].ToString());
                        stop_time += comm.sGetDouble(dt3.Rows[0]["300195"].ToString());
                        stop_time += comm.sGetDouble(dt3.Rows[0]["300205"].ToString());
                        stop_time += comm.sGetDouble(dt3.Rows[0]["300215"].ToString());
                        stop_time += comm.sGetDouble(dt3.Rows[0]["300225"].ToString());
                        stop_time += comm.sGetDouble(dt3.Rows[0]["300235"].ToString());
                        stop_time += comm.sGetDouble(dt3.Rows[0]["300245"].ToString());
                        stop_time += comm.sGetDouble(dt3.Rows[0]["300255"].ToString());
                    }
                }

                double OEE = 0;
                DataTable dt4 = comm.Get_DataTable(@"select TOP(1) *, 
                            DATEDIFF(minute, (select MIN(TRANSACTION_DATE) from MBA_E10 where Convert(varchar, TRANSACTION_DATE, 23) like  Convert(varchar, GETDATE(), 23) + '%'),
                                    (select MAX(TRANSACTION_DATE) from MBA_E10 where Convert(varchar, TRANSACTION_DATE, 23) like  Convert(varchar, GETDATE(), 23) + '%')) AS OEE
                            from MBA_E10
                            where TRANSACTION_DATE between convert(varchar(10),GETDATE(),120)+' 00:00:01.000'
                            and convert(varchar(10),GETDATE(),120)+' 23:59:59.999'
                            and XMACHINE_CODE='"+ mac_code + "'order by TRANSACTION_DATE desc");
                if (dt4.Rows.Count > 0)
                {
                    OEE = comm.sGetDouble(dt4.Rows[0]["OEE"].ToString());
                }

                Double Oee_2 = ((OEE - stop_time) / OEE);

                double completeRate = 0;
                    double iplan_qty = 0;
                    double iok_qty = 0;
                    iok_qty = QTY;
                    iplan_qty = QTY / (hour_count*pro_uph);

                    //double iplan_qty = comm.sGetDouble(Get_DataByStationCode(mac_code, "plan_qty"));
                    //double imo_qty = comm.sGetDouble(Get_DataByStationCode(seq_no, "mo_qty"));



                //if (iok_qty + ing_qty != 0)
                //{
                //    irate = iok_qty / (iok_qty + ing_qty);
                //}

                if (day_target_qty != 0)
                    {
                        completeRate = QTY / day_target_qty;// 達成率=良品/計畫輛
                    }

                    //double dRelTime = Get_RelTime(sWrkCode);
                    //double dStdRate = Get_StdRate(sMoCode, work_code);
                    ////實際產能計算
                    //double sEfficiency = Get_Efficiency(iok_qty, dRelTime, dStdRate);
                    Oee data = new Oee();
                    data.TkCode = mac_code;

                    data.status = Check_Status(sMoCode, sWrkCode, mac_code, "status");
                    //string sStopCode = Check_Status(sMoCode, sWrkCode, work_code, "stop_code");
                    //data.message = comm.Get_QueryData("MEB45_0000", sStopCode, "stop_code", "stop_name");
                    data.mo_code = sMoCode;




                    //如果沒有正在進行的工單則不帶值
                    if (string.IsNullOrEmpty(sMoCode))
                    {
                        iok_qty = 0;
                        //iplan_qty = 0;
                        //ing_qty = 0;
                        //irate = 0;
                        sUsrName = "";
                        //sWorkTime = "";
                        //completeRate = 0;
                    }

                    List<object> items = new List<object>();
                    //items.Add(new { name = "iplan_qty", value = (iok_qty + ing_qty).ToString() + "/" + iplan_qty.ToString(), label = "產量" });
                    items.Add(new { name = "day_target_qty", value = day_target_qty.ToString(), label = "今日生產計畫" });
                    items.Add(new { name = "QTY", value = QTY.ToString(), label = "今日生產產出" });
                    items.Add(new { name = "completeRate", value = completeRate.ToString("0.##%"), label = "達成率" });
                    //items.Add(new { name = "iok_qty", value = iok_qty.ToString(), label = "良品" });
                    items.Add(new { name = "iplan_qty", value = iplan_qty.ToString("0.##%"), label = "OEE" });
                    //items.Add(new { name = "sor_code", value = sor_code.ToString(), label = "工單型號" });
                    //items.Add(new { name = "ing_qty", value = ing_qty.ToString(), label = "不良品" });
                    items.Add(new { name = "Oee_2", value = Oee_2.ToString("0.##%"), label = "稼動率" });
                    items.Add(new { name = "stop_time", value = stop_time.ToString(), label = "停機時間" });
                    items.Add(new { name = "work_time", value = sWorkTime, label = "工單開始時間" });
                    //items.Add(new { name = "sEfficiency", value = sEfficiency.ToString("0.##%"), label = "效率" });

                    data.items = items;
                    returnObj.Add(data.TkCode, data);
                }

                return Json(returnObj, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception ex)
            //{
            //    List<Oee> empty = new List<Oee>();
            //    return Json(empty, JsonRequestBehavior.AllowGet);
            //}
        }

        public string Check_Status(string mo_code, string wrk_code, string mac_code, string field_code)
        {
            string sSql = " select distinct mo_code,* " +
                          " from MET02_0000 with (nolock)" +
                          " where 1=1" +
                          //" and date_e = ''" +
                          //" AND date_s != ''  " +
                          " AND mo_code = '" + mo_code + "'";
                          //" AND wrk_code = '" + wrk_code + "'" +
                          //" and mac_code ='" + mac_code + "'";

            DataTable dTmp = comm.Get_DataTable(sSql);
            //string sStatusCode = mo_code != "" ? "R" : "I";
            string sStatusCode = "I";
            if (dTmp.Rows.Count > 0)
            {
                if (dTmp.Rows[0]["MO_STATUS"].ToString() == "1")
                {
                    sStatusCode = "R";
                }
                else
                {
                    sStatusCode = "I";
                }
            }

            return sStatusCode;
        }

        public class Oee
        {
            public string TkCode { get; set; }
            public string mo_code { get; set; }
            public string status { get; set; }
            public string message { get; set; }
            public List<object> items { get; set; }
        }

        //public string Get_MoCodeByStationCode(string pStationCode)
        //{
        //    string sReturn = "";
        //    string sSql = "";
        //    sSql = "select top 1 * from MEM04_0000 " +
        //           " where station_code='" + pStationCode + "'"+
        //           " AND mo_code !='' ";
        //    DataTable dtTmp = comm.Get_DataTable(sSql);

        //    if (dtTmp.Rows.Count > 0)
        //    {
        //        sReturn = dtTmp.Rows[0]["mo_code"].ToString().Trim();
        //    }
        //    else
        //    {
        //        sReturn = "";
        //    }
        //    return sReturn;
        //}

        public string Get_DataByStationCode(string mac_code, string pFieldCode)
        {
            string sReturn = "";
            string sSql = "";
            //sSql = "select top 1 MEM01_0000.*,MET03_0000.wrk_code as wrk_code from MEM01_0000 " +
            //       " left join MET03_0000 on (MET03_0000.work_code = MEM01_0000.work_code and MET03_0000.mo_code=MEM01_0000.mo_code)" +
            //       " where MEM01_0000.work_code='" + pStationCode + "'"+
            //       "   and mac_code !=''" +
            //       "   and work_time_s <> ''" +
            //       "   and work_time_e = ''" +
            //       " order by work_time_s desc";
            //sSql = "select top 1 MEM01_0000.*,MET03_0000.wrk_code as wrk_code, MET01_0000.plan_qty, MET01_0000.mo_qty, MEB20_0000.pro_name from MEM01_0000 with (nolock)" +
            //       " left join MET03_0000 on (MET03_0000.work_code = MEM01_0000.work_code and MET03_0000.mo_code=MEM01_0000.mo_code)" +
            //       " left join MET01_0000 on MEM01_0000.mo_code = MET01_0000.mo_code" +
            //       " left join MEB20_0000 on MEB20_0000.pro_code = MET03_0000.pro_code" +
            //       " where MEM01_0000.mac_code='" + pStationCode + "'" +
            //       "   and work_time_s <> ''" +
            //       "   and work_time_e = ''" +
            //       " order by work_time_s desc";
            sSql = @"   SELECT SUM(m.QTY) AS QTY,
                       DATEDIFF(HOUR
                        ,(select TOP(1) TRANSACTION_DATE from MBA_E10 where  Convert(varchar,TRANSACTION_DATE,23) = Convert(varchar,GETDATE(),23) and Convert(varchar,TRANSACTION_DATE,108) > '01:00:00' order by  TRANSACTION_DATE )
	                    ,(select TOP(1) TRANSACTION_DATE from MBA_E10 where  Convert(varchar, TRANSACTION_DATE,23) = Convert(varchar,GETDATE(),23) and Convert(varchar,TRANSACTION_DATE,108) > '01:00:00' order by  TRANSACTION_DATE DESC)) AS hour_count
                        ,(select TOP(1) mo_code from MET02_0000 where Convert(varchar, TRANSACTION_DATE,23) = Convert(varchar,GETDATE(),23) order by TRANSACTION_DATE) AS mo_code
	                    , a.day_target_qty AS day_target_qty, c.pro_uph AS pro_uph
                       FROM mba_e10 m
                       left join MEB12_0000 a on a.line_code = m.XMACHINE_CODE 
                       left join MET02_0000 b on b.mo_code = m.MO_DOC_NO
                       left join MEB50_0000 c on c.ITEM_SPECIFICATION = b.pro_spec
                       WHERE Convert(varchar,m.TRANSACTION_DATE,23) = Convert(varchar,GETDATE(),23) AND XMACHINE_CODE='" + mac_code+"' "+
                       " group by a.day_target_qty, c.pro_uph";
            DataTable dtTmp = comm.Get_DataTable(sSql);

            if (dtTmp.Rows.Count > 0)
            {
                try
                {
                    sReturn = dtTmp.Rows[0][pFieldCode].ToString().Trim();
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

        public JsonResult Get_ProName(string pJson)
        {

            List<string> mac_code_list = JsonConvert.DeserializeObject<List<string>>(pJson);
            List<Pro> list = new List<Pro>();
            foreach (string mac_code in mac_code_list)
            {
                string sWrkCode = Get_DataByStationCode(mac_code, "OPERATION_CODE");
                string sProName = Get_DataByStationCode(mac_code, "pro_name");

                Pro data = new Pro();
                data.WrkCode = sWrkCode;
                data.ProName = sProName;

                list.Add(data);
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public class Pro
        {
            public string WrkCode { get; set; }
            public string ProName { get; set; }
        }
        /// <summary>
        /// 取得製程標準產能
        /// </summary>
        /// <param name="pMoCode"></param>
        /// <param name="pWorkCode"></param>
        /// <returns></returns>
        public double Get_StdRate(string pMoCode, string pWorkCode)
        {
            double dReturn = 0;
            //取得BOM版本
            string sBomCode = comm.Get_QueryData("MET01_0000", pMoCode, "mo_code", "bom_code");
            double dStdQty = Get_MEB23_0200(sBomCode, pWorkCode, "std_qty");
            double dStdTime = Get_MEB23_0200(sBomCode, pWorkCode, "std_time");
            if (dStdQty != 0 && dStdTime != 0)
            {
                dReturn = dStdQty / dStdTime;
            }
            return dReturn;
        }
        public double Get_MEB23_0200(string pBomCode, string pWorkCode, string pFieldName)
        {
            double dReturn = 0;
            string sSql = "select * from MEB23_0200 with (nolock) " +
                          " where bom_code='" + pBomCode + "' " +
                         "    and work_code='" + pWorkCode + "'";
            DataTable dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                string sFieldValue = comm.sGetString(dtTmp.Rows[0][pFieldName].ToString());
                if (sFieldValue != "")
                {
                    dReturn = comm.sGetDouble(sFieldValue);
                }
            }
            return dReturn;
        }
        /// <summary>
        /// 取得製程實際開工時間
        /// </summary>
        /// <param name="pWrkCode"></param>
        /// <returns></returns>
        public double Get_RelTime(string pWrkCode)
        {
            double sReturn = 0;
            string sSql = "";
            //取得製程實際開工時間
            sSql = "select  sum(" +
                   "            DATEDIFF(SECOND, CONVERT(datetime, ins_date+' ' +ins_time, 121), " +
                  "                              CONVERT(datetime,case when ISNULL((end_date + ' ' + end_time), getdate()) = '' then GETDATE() else ISNULL((end_date + ' ' + end_time), getdate()) end, 121) " +
                  "                     ) " +
                  "             ) as work_time " +
                  " from MED02_0000 with (nolock)" +
                  " where wrk_code = '" + pWrkCode + "'";
            DataTable dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                string sWorkTime = comm.sGetString(dtTmp.Rows[0]["work_time"].ToString());
                if (!string.IsNullOrEmpty(sWorkTime))
                {
                    sReturn = comm.sGetDouble(sWorkTime);
                }
            }
            return sReturn;
        }

        public double Get_Efficiency(double pOkQty, double pRelTime, double pStdRate)
        {
            double dReturn = 0;
            //此處皆須以秒為單位
            //效率=實際產能/標準產能
            //實際產能=良品/實際時間
            //標準產能=標準產量/標準時間
            if (pOkQty != 0 && pRelTime != 0 && pStdRate != 0)
            {
                dReturn = (pOkQty / pRelTime) / pStdRate;
            }
            return dReturn;
        }
        /// <summary>
        /// 取得目前機台的人員數
        /// </summary>
        /// <param name="pMacCode"></param>
        /// <returns></returns>
        public string Get_UserCodeByMacCodeCount(string pMacCode)
        {
            //取得目前人員是否有在該機台的任何資料
            string sReturn = "";
            string sSql = "";
            sSql = " select  count(mac_code) as conut from MED01_0100  with (nolock) " +
                   " where mac_code='" + pMacCode + "'" +
                   //"   and date_s='" + comm.Get_Date() + "'" +
                   "   and status='I'";

            DataTable dtTmp = comm.Get_DataTable(sSql);

            if (dtTmp.Rows.Count > 0)
            {
                sReturn = dtTmp.Rows[0]["conut"].ToString().Trim();
            }
            else
            {
                sReturn = "";
            }
            return sReturn;
        }
    }
}