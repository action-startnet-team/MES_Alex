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
    public class RPT040AController : JsonNetController
    {
        Comm comm = new Comm();
        MET01_0000Repository repoMET01_0000 = new MET01_0000Repository();
        WMB140AController WMB140AController = new WMB140AController();
        GetData GD = new GetData();
        CheckData CD = new CheckData();


        // GET: WMB140B
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Get_MoData(string mo_code)
        {
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            sqlparams.Add("@mo_code", mo_code);
            string sql = "Select top 1 MET01_0000.*, MEB20_0000.pro_name " +
                         " from MET01_0000  " +
                         " left join MEB20_0000 on MEB20_0000.pro_code = MET01_0000.pro_code" +
                         " where mo_code = @mo_code"; 
            DataTable dt = comm.Get_DataTable(sql, sqlparams);

            if (dt.Rows.Count < 0)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

            return Json(dt, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Get_TimeLineData(FormCollection form)
        {

            string mo_code = form["mo_code"];
            string pro_lot_no = form["pro_lot_no"];

            DataTable dt = Get_MEM01_0000(mo_code);

            List<object> returnList = new List<object>();
            foreach(DataRow dr in dt.Rows)
            {
                DateTime date_s = DateTime.Parse(dr["date_s"].ToString());
                DateTime date_e = DateTime.Parse(dr["date_e"].ToString());

                //
                //int cal_work_min = 0;
                //// 結束日期大於開始日期 但是work_min是0的狀況
                //if (DateTime.Compare(date_s, date_e) < 0 && comm.sGetInt32(dr["work_sec"].ToString()) == 0)
                //{
                //    cal_work_min = (int)date_e.Subtract(date_s).TotalMinutes;
                //} else
                //{
                //    cal_work_min = comm.sGetInt32(dr["work_sec"].ToString());
                //}

                var data_start = new
                {
                    type = "start",
                    work_code = dr["work_code"],
                    datetime = Chk_MagicDateTime(date_s) ? "" : dr["date_s"],
                    ok_qty = dr["ok_qty"],
                    ng_qty = dr["ng_qty"],
                    //work_min = cal_work_min,
                    //station_code = dr["station_code"],
                    station_name = dr["station_name"],
                    //mac_code = dr["mac_code"],
                    mac_name = dr["mac_name"],
                    //usr_code = dr["usr_code"],
                    usr_name = dr["usr_name"]

                };

                // 結束時間小於開始時間
                if (DateTime.Compare(date_s, date_e) > 0 )
                {

                }
                

                var data_end = new
                {
                    type = "end",
                    work_code = dr["work_code"],
                    datetime = Chk_MagicDateTime(date_e) ? "" : dr["date_e"],
                };

                returnList.Add(data_start);

                // 如果未開始就 不顯示結束
                if (!string.IsNullOrEmpty(data_start.datetime.ToString()))
                {
                    returnList.Add(data_end);
                }
            }


            return Json(returnList, JsonRequestBehavior.AllowGet);

        }

        public string Get_LastQty(string mo_code)
        {
            DataTable dt;
            if (!string.IsNullOrEmpty(mo_code))
            {
                string sSql = " select MET03_0000.*, MEM01_0000.ok_qty " +
                                        " from MET03_0000 " +
                                        " left join MEM01_0000 on MEM01_0000.mo_code = MET03_0000.mo_code and MEM01_0000.work_code = MET03_0000.work_code " +
                                        " where MET03_0000.mo_code = '" + mo_code + "'" +
                                        " order by wrk_code DESC ";
                dt = comm.Get_DataTable(sSql);
                string ok_qty = "0.00";
                if (dt.Rows.Count >=1)
                {
                    string tmp = dt.Rows[0]["ok_qty"].ToString();
                    ok_qty =  string.IsNullOrEmpty(dt.Rows[0]["ok_qty"].ToString()) ? "0.00" : dt.Rows[0]["ok_qty"].ToString();
                }
                return ok_qty;
            }
            return "0.00";
        }

        // magic datetime: 1900-01-01 00:00:00.000
        public bool Chk_MagicDateTime(DateTime pDateTime)
        {
            DateTime magic_datetime = DateTime.Parse("1900-01-01 00:00:00.000");
            if (DateTime.Compare(magic_datetime, pDateTime) == 0)
            {
                return true;
            }
            return false;
        }

        public DataTable Get_MEM01_0000(string mo_code)
        {
            string sSql = "";

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("usr_name", System.Type.GetType("System.String"));
            dtDat.Columns.Add("work_code", System.Type.GetType("System.String"));
            dtDat.Columns.Add("station_name", System.Type.GetType("System.String"));
            dtDat.Columns.Add("mac_name", System.Type.GetType("System.String"));
            dtDat.Columns.Add("ok_qty", System.Type.GetType("System.String"));
            dtDat.Columns.Add("ng_qty", System.Type.GetType("System.String"));
            dtDat.Columns.Add("date_s", System.Type.GetType("System.String"));
            dtDat.Columns.Add("date_e", System.Type.GetType("System.String"));

            //沒有條件則回傳空的DataTable
            if (string.IsNullOrEmpty(mo_code))
            {
                return dtDat;
            }

            sSql = "select * from MEM01_0000 where mo_code='" + mo_code + "' order by work_time_s" ;
            comm.Ins_BDP20_0000("DTS", "DTS", "ADD", sSql);
            DataTable dtTmp = comm.Get_DataTable(sSql);
            comm.Ins_BDP20_0000("DTS", "DTS", "ADD2", sSql);

            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["usr_name"] = comm.Get_QueryData("BDP08_0000",comm.sGetString(dtTmp.Rows[i]["usr_code"].ToString()), "usr_code", "usr_name");
                drow["work_code"] = comm.Get_QueryData("MEB30_0000", comm.sGetString(dtTmp.Rows[i]["work_code"].ToString()),  "work_code", "work_name");
                drow["station_name"] = comm.Get_QueryData("MEB29_0000", comm.sGetString(dtTmp.Rows[i]["station_code"].ToString()),  "station_code", "station_name");
                drow["mac_name"] = comm.Get_QueryData("MEB15_0000", comm.sGetString( dtTmp.Rows[i]["mac_code"].ToString()),  "mac_code", "mac_name");
                drow["ok_qty"] = comm.sGetString(dtTmp.Rows[i]["ok_qty"].ToString());
                drow["ng_qty"] = comm.sGetString(dtTmp.Rows[i]["ng_qty"].ToString());
                drow["date_s"] = comm.sGetDateTime(dtTmp.Rows[i]["work_time_s"].ToString());
                drow["date_s"] = string.Format("{0:yyyy-mm-dd HH:mm:ss}", drow["date_s"]);
                drow["date_e"] = comm.sGetDateTime(dtTmp.Rows[i]["work_time_e"].ToString());                
                drow["date_e"] = string.Format("{0:yyyy-mm-dd HH:mm:ss}", drow["date_e"]);

                dtDat.Rows.Add(drow);
            }
            return dtDat;            
        }


        public string Get_QmtValueByMoCode(string pMoCode) {
            string sSql = "";
            DataTable dtTmp = new DataTable();
            DataTable dtTmp2 = new DataTable();
            DateTime dtStartTime = new DateTime();           
            DateTime dtEndTime = new DateTime();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("spc_code", System.Type.GetType("System.String"));
            dtDat.Columns.Add("field_value", System.Type.GetType("System.String"));
            dtDat.Columns.Add("usr_code", System.Type.GetType("System.String"));
            dtDat.Columns.Add("ins_date", System.Type.GetType("System.String"));
            dtDat.Columns.Add("is_excep", System.Type.GetType("System.String"));

            //沒有條件則回傳空的DataTable
            if (string.IsNullOrEmpty(pMoCode))
            {
                return JsonConvert.SerializeObject(dtDat);
            }

            //先找出有設定檢驗設定的項目QMB14_0000
            sSql = "select * "+
                   "  from QMB14_0000";
            dtTmp = comm.Get_DataTable(sSql);
            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                DataRow r = dtTmp.Rows[i];
                string sSpcCode = r["spc_code"].ToString();
                string sSpcName = r["spc_name"].ToString();
                decimal dUpLimit = comm.sGetDecimal(r["up_limit"].ToString());
                decimal dDownLimit = comm.sGetDecimal(r["down_limit"].ToString());

                //找出管制項目對應的電子表單欄位
                sSql = "select * " +
                       "  from EPB02_0100" +
                       " where sor_table = 'QMB14_0000' " +
                       "   and sor_field = 'spc_code'" +
                       "   and sor_key = '" + sSpcCode + "'";
                dtTmp2 = comm.Get_DataTable(sSql);
                if (dtTmp2.Rows.Count > 0) {
                    DataRow r2 = dtTmp2.Rows[0];
                    string sEpbCode = r2["epb_code"].ToString();
                    string sFieldCode = r2["field_code"].ToString();

                    //利用製令找出開始與結束時間
                    sSql = "select * " +
                           "  from MEM01_0000" +
                           " where mo_code = @mo_code " +
                           " order by work_time_s,work_time_e";                        
                    dtTmp2 = comm.Get_DataTable(sSql,"mo_code", pMoCode);
                    if (dtTmp2.Rows.Count > 0) {
                        string sStartTime = dtTmp2.Rows[0]["work_time_s"].ToString();
                        string sEndTime = dtTmp2.Rows[dtTmp2.Rows.Count - 1]["work_time_e"].ToString();

                        if (CD.IsDate(sStartTime)) {
                            dtStartTime = DateTime.Parse(sStartTime);
                        }

                        if (CD.IsDate(sEndTime))
                        {
                            dtEndTime = DateTime.Parse(sEndTime);
                        }
                        else {
                            //如果結束時間沒有日期，則代表為未結束，則可以接收最新的檢驗紀錄
                            dtEndTime = DateTime.Now;
                        }

                        //由於MEM01_0000是還未開始就會先把歷程造出來
                        //所以只能判斷有開始時間才繼續
                        //if (DateTime.Compare(dtEndTime, dtStartTime) > 0)
                        if (dtStartTime != new DateTime())
                        {
                            //查詢檢驗結果，條件加在這
                            sSql = "select * " +
                                   "  from EPB03_0000" +
                                   " where epb_code = '" + sEpbCode + "'" +
                                   "   and field_code = '" + sFieldCode + "'" +
                                   "   and ins_date + ' ' + ins_time between '" + dtStartTime.ToString("yyyy/MM/dd HH:mm:ss") + "' and '" + dtEndTime.ToString("yyyy/MM/dd HH:mm:ss") + "'";
                            dtTmp2 = comm.Get_DataTable(sSql);
                            for (int u = 0; u < dtTmp2.Rows.Count; u++)
                            {
                                DataRow ur = dtTmp2.Rows[u];
                                string sFieldValue = ur["field_value"].ToString();
                                string sInsDate = ur["ins_date"].ToString();
                                string sInsTime = ur["ins_time"].ToString();
                                string sUsrCode = ur["usr_code"].ToString();

                                //管制項目的概念為數值且需判斷上下限
                                if (CD.IsNumeric(sFieldValue))
                                {
                                    DataRow drow = dtDat.NewRow();
                                    drow["spc_code"] = sSpcName;
                                    drow["field_value"] = sFieldValue;
                                    drow["usr_code"] = sUsrCode;
                                    drow["ins_date"] = sInsDate + " " + sInsTime;
                                    drow["is_excep"] = Chk_QmtValue_IsException(comm.sGetDecimal(sFieldValue), dUpLimit, dDownLimit);

                                    dtDat.Rows.Add(drow);
                                }
                            }
                        }
                    }                   
                }
            }
            if (dtDat.Rows.Count > 0) {
                DataView dv = dtDat.DefaultView;
                dv.Sort = "spc_code,ins_date";
                dtDat = dv.ToTable();
            }           
            return JsonConvert.SerializeObject(dtDat);
        }

        public string  Chk_QmtValue_IsException(decimal pQmtValue, decimal pUpLimit, decimal pDownLimit) {
            string val = "N";
            if (pQmtValue > pUpLimit || pQmtValue < pDownLimit) { val = "Y"; }
            return val;
        }



    }
}