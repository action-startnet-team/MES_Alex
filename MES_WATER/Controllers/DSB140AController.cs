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
    public class DSB140AController : JsonNetController
    {
        Comm comm = new Comm();
        MET01_0000Repository repoMET01_0000 = new MET01_0000Repository();
        WMB140AController WMB140AController = new WMB140AController();

        // GET: WMB140B
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Get_MoData(string mo_code)
        {
            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            sqlparams.Add("@mo_code", mo_code);
            string sql = " Select top 1 MET01_0000.*, MEB20_0000.pro_name " +
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
            DataTable returnData = new DataTable();
            returnData.Columns.Add("datetime", typeof(string));
            returnData.Columns.Add("usr_code", typeof(string));
            returnData.Columns.Add("usr_name", typeof(string));
            returnData.Columns.Add("work_code", typeof(string));
            returnData.Columns.Add("work_name", typeof(string));
            returnData.Columns.Add("station_code", typeof(string));
            returnData.Columns.Add("station_name", typeof(string));
            returnData.Columns.Add("mo_code", typeof(string));
            returnData.Columns.Add("pro_code", typeof(string));
            returnData.Columns.Add("pro_name", typeof(string));
            returnData.Columns.Add("remark", typeof(string));
            returnData.Columns.Add("mac_code", typeof(string));
            returnData.Columns.Add("mac_name", typeof(string));
            
            // 上下工
            DataTable MED01_0000 = Get_MED_Table("MED01_0000", form);
            if (MED01_0000.Rows.Count > 0)
            {
                foreach (DataRow dr in MED01_0000.Rows)
                {
                    DataRow rowData = returnData.NewRow();
                    rowData["datetime"] = dr["ins_date"] + " " + dr["ins_time"];
                    rowData["usr_code"] = dr["usr_code"];
                    rowData["usr_name"] = dr["usr_name"];
                    rowData["work_code"] = dr["work_code"];
                    rowData["work_name"] = dr["work_name"];
                    rowData["station_code"] = dr["station_code"];
                    rowData["station_name"] = dr["station_name"];
                    rowData["mo_code"] = dr["mo_code"];
                    rowData["pro_code"] = dr["pro_code"];
                    rowData["pro_name"] = dr["pro_name"];
                    rowData["remark"] = "上下工";
                    rowData["mac_code"] = dr["mac_code"];
                    rowData["mac_name"] = dr["mac_name"];
                    returnData.Rows.Add(rowData);
                }
            }

            // 工單狀態
            DataTable MED02_0000 = Get_MED_Table("MED02_0000", form);
            if (MED02_0000.Rows.Count > 0)
            {
                foreach (DataRow dr in MED02_0000.Rows)
                {
                    DataRow rowData = returnData.NewRow();
                    rowData["datetime"] = dr["ins_date"] + " " + dr["ins_time"];
                    rowData["usr_code"] = dr["usr_code"];
                    rowData["usr_name"] = dr["usr_name"];
                    rowData["work_code"] = dr["work_code"];
                    rowData["work_name"] = dr["work_name"];
                    rowData["station_code"] = dr["station_code"];
                    rowData["station_name"] = dr["station_name"];
                    rowData["mo_code"] = dr["mo_code"];
                    rowData["pro_code"] = dr["pro_code"];
                    rowData["pro_name"] = dr["pro_name"];
                    rowData["remark"] = "工單狀態";
                    rowData["mac_code"] = dr["mac_code"];
                    rowData["mac_name"] = dr["mac_name"];
                    returnData.Rows.Add(rowData);
                }
            }

            // 不良報工
            DataTable MED03_0000 = Get_MED_Table("MED03_0000", form);
            if (MED03_0000.Rows.Count > 0)
            {
                foreach (DataRow dr in MED03_0000.Rows)
                {
                    DataRow rowData = returnData.NewRow();
                    rowData["datetime"] = dr["ins_date"] + " " + dr["ins_time"];
                    rowData["usr_code"] = dr["usr_code"];
                    rowData["usr_name"] = dr["usr_name"];
                    rowData["work_code"] = dr["work_code"];
                    rowData["work_name"] = dr["work_name"];
                    rowData["station_code"] = dr["station_code"];
                    rowData["station_name"] = dr["station_name"];
                    rowData["mo_code"] = dr["mo_code"];
                    rowData["pro_code"] = dr["pro_code"];
                    rowData["pro_name"] = dr["pro_name"];
                    rowData["remark"] = "不良報工";
                    rowData["mac_code"] = dr["mac_code"];
                    rowData["mac_name"] = dr["mac_name"];
                    returnData.Rows.Add(rowData);
                }
            }

            DataTable MED04_0000 = Get_MED_Table("MED04_0000", form);
            if (MED04_0000.Rows.Count > 0)
            {
                foreach (DataRow dr in MED04_0000.Rows)
                {
                    DataRow rowData = returnData.NewRow();
                    rowData["datetime"] = dr["ins_date"] + " " + dr["ins_time"];
                    rowData["usr_code"] = dr["usr_code"];
                    rowData["usr_name"] = dr["usr_name"];
                    rowData["work_code"] = dr["work_code"];
                    rowData["work_name"] = dr["work_name"];
                    rowData["station_code"] = dr["station_code"];
                    rowData["station_name"] = dr["station_name"];
                    rowData["mo_code"] = dr["mo_code"];
                    rowData["pro_code"] = dr["pro_code"];
                    rowData["pro_name"] = dr["pro_name"];
                    rowData["remark"] = "停機原因";
                    rowData["mac_code"] = dr["mac_code"];
                    rowData["mac_name"] = dr["mac_name"];
                    returnData.Rows.Add(rowData);
                }
            }

            // 上料
            DataTable MED06_0000 = Get_MED_Table("MED06_0000", form);
            if (MED06_0000.Rows.Count > 0)
            {
                foreach (DataRow dr in MED06_0000.Rows)
                {
                    DataRow rowData = returnData.NewRow();
                    rowData["datetime"] = dr["ins_date"] + " " + dr["ins_time"];
                    rowData["usr_code"] = dr["usr_code"];
                    rowData["usr_name"] = dr["usr_name"];
                    rowData["work_code"] = dr["work_code"];
                    rowData["work_name"] = dr["work_name"];
                    rowData["station_code"] = dr["station_code"];
                    rowData["station_name"] = dr["station_name"];
                    rowData["mo_code"] = dr["mo_code"];
                    rowData["pro_code"] = dr["pro_code"];
                    rowData["pro_name"] = dr["pro_name"];
                    rowData["remark"] = "上料";
                    rowData["mac_code"] = dr["mac_code"];
                    rowData["mac_name"] = dr["mac_name"];
                    returnData.Rows.Add(rowData);
                }
            }

            // 退料
            DataTable MED07_0000 = Get_MED_Table("MED07_0000", form);
            if (MED07_0000.Rows.Count > 0)
            {
                foreach (DataRow dr in MED07_0000.Rows)
                {
                    DataRow rowData = returnData.NewRow();
                    rowData["datetime"] = dr["ins_date"] + " " + dr["ins_time"];
                    rowData["usr_code"] = dr["usr_code"];
                    rowData["usr_name"] = dr["usr_name"];
                    rowData["work_code"] = dr["work_code"];
                    rowData["work_name"] = dr["work_name"];
                    rowData["station_code"] = dr["station_code"];
                    rowData["station_name"] = dr["station_name"];
                    rowData["mo_code"] = dr["mo_code"];
                    rowData["pro_code"] = dr["pro_code"];
                    rowData["pro_name"] = dr["pro_name"];
                    rowData["remark"] = "退料";
                    rowData["mac_code"] = dr["mac_code"];
                    rowData["mac_name"] = dr["mac_name"];
                    returnData.Rows.Add(rowData);
                }
            }
            
            DataTable MED09_0000 = Get_MED_Table("MED09_0000", form);
            if (MED09_0000.Rows.Count > 0)
            {
                foreach (DataRow dr in MED09_0000.Rows)
                {
                    DataRow rowData = returnData.NewRow();
                    rowData["datetime"] = dr["ins_date"] + " " + dr["ins_time"];
                    rowData["usr_code"] = dr["usr_code"];
                    rowData["usr_name"] = dr["usr_name"];
                    rowData["work_code"] = dr["work_code"];
                    rowData["work_name"] = dr["work_name"];
                    rowData["station_code"] = dr["station_code"];
                    rowData["station_name"] = dr["station_name"];
                    rowData["mo_code"] = dr["mo_code"];
                    rowData["pro_code"] = dr["pro_code"];
                    rowData["pro_name"] = dr["pro_name"];
                    rowData["remark"] = "良品報工";
                    rowData["mac_code"] = dr["mac_code"];
                    rowData["mac_name"] = dr["mac_name"];
                    returnData.Rows.Add(rowData);
                }
            }

            var orderedRows = from row in returnData.AsEnumerable()
                              orderby row.Field<string>("datetime") //descending
                              select row;

            // 不加這個判斷CopyToDataTable會出錯
            if (orderedRows.Count() > 0)
            {
                returnData = orderedRows.CopyToDataTable();
            }

            return Json(returnData, JsonRequestBehavior.AllowGet);

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
        
        private DataTable Get_MED_Table(string pTable, FormCollection form)
        {
            string usr_code_multi = form["usr_code"];
            string cal_date = form["cal_date"];
            string station_code = form["station_code"];
            string mo_code = form["mo_code"];
            string process_code = form["process_code"];
            
            // 取得報工資料

            Dictionary<string, object> sqlParmas = new Dictionary<string, object>();

            string sSubWhere = " Where 1=1 ";

            if (!string.IsNullOrEmpty(usr_code_multi))
            {
                string[] usr_code_array = usr_code_multi.Split(',');

                string inUsrCode = "(";
                string param_name = "";
                for (int i = 0; i < usr_code_array.Length; i++)
                {
                    param_name = "@usr_code_" + i.ToString();
                    sqlParmas.Add(param_name, usr_code_array[i]);
                    inUsrCode += param_name + ",";
                }

                inUsrCode = inUsrCode.Substring(0, inUsrCode.Length - 1); // 去除最後的逗點
                inUsrCode += ")";

                sSubWhere += " and " + pTable + ".usr_code in " + inUsrCode;
            }

            if (!string.IsNullOrEmpty(cal_date))
            {
                sqlParmas.Add("@cal_date", cal_date);
                sSubWhere += " and " + pTable + ".ins_date = @cal_date ";
            }

            if (!string.IsNullOrEmpty(station_code))
            {
                sqlParmas.Add("@station_code", station_code + "%");

                if (pTable == "MED01_0000" || pTable == "MED02_0000")
                    sSubWhere += " and MEB29_0200.station_code like @station_code ";
                else
                    sSubWhere += " and " + pTable + ".station_code like @station_code ";
            }

            if (!string.IsNullOrEmpty(mo_code))
            {
                sqlParmas.Add("@mo_code", mo_code + "%");
                sSubWhere += " and " + pTable + ".mo_code like @mo_code ";
            }

            if (!string.IsNullOrEmpty(process_code))
            {
                sqlParmas.Add("@process_code", process_code + "%");
                if(pTable == "MED01_0000" || pTable == "MED02_0000")
                    sSubWhere += " and MEB30_0100.work_code like @process_code ";
                else
                    sSubWhere += " and " + pTable + ".work_code like @process_code ";
            }

            //string tmpsSql = "";
            //if (pTable == "MED06_0000")
            //{
            //    tmpsSql = "left join MEB20_0000 on MEB20_0000.pro_code =" + pTable + @".pro_code";
            //}
            //else
            //{
            //    tmpsSql = "left join MEB20_0000 on MEB20_0000.pro_code = MET01_0000.pro_code ";
            //}

            //string sPro = "";
            //string jPro = "";
            //if (pTable == "MED01_0000" || pTable == "MED02_0000")
            //{
            //    sPro = " ,ISNULL(MET01_0000.pro_code, '') as pro_code " +
            //           " ,ISNULL(MEB20_0000.pro_name, '') as pro_name ";
            //    jPro = " left join MET01_0000 on MET01_0000.mo_code = " + pTable + ".mo_code " +
            //           " left join MEB20_0000 on MEB20_0000.pro_code = MET01_0000.pro_code ";
            //}
            //else
            //{
            //    sPro = " ,ISNULL(MEB20_0000.pro_name, '') as pro_name ";
            //    jPro = " left join MEB20_0000 on MEB20_0000.pro_code = " + pTable + ".pro_code ";
            //}

            //string sStationCode = "";
            //string jStationCode = "";
            //if (pTable == "MED01_0000" || pTable == "MED02_0000")
            //{
            //    sStationCode = ",ISNULL(MEB29_0200.station_code, '') as station_code";
            //    jStationCode = "left join MEB30_0100 on MEB30_0100.mac_code = MEB29_0200.mac_code";
            //}

            //string sWorkCode = "";
            //string jWorkCode = "";
            //if (pTable == "MED01_0000" || pTable == "MED02_0000")
            //{
            //    sWorkCode = ",ISNULL(MEB30_0100.work_code, '') as work_code";
            //    jWorkCode = "left join MEB30_0100 on MEB30_0100.station_code = MEB29_0200.station_code";
            //}

            //string sSql = " select " + pTable + ".* , " +
            //              " ISNULL(BDP08_0000.usr_name, '') as usr_name, " +
            //              " ISNULL(MEB15_0000.mac_name, '') as mac_name, " +
            //              " ISNULL(MEB29_0200.station_code, '') as station_code, " +
            //              " ISNULL(MEB29_0000.station_name, '') as station_name  " +
            //              sPro +
            //              sStationCode +
            //              sWorkCode +
            //              " from " + pTable +
            //              jPro +
            //              jStationCode +
            //              jWorkCode + 
            //              " left join BDP08_0000 on BDP08_0000.usr_code = " + pTable + ".usr_code " + 
            //              " left join MEB15_0000 on MEB15_0000.mac_code = " + pTable + ".mac_code " +
            //              " left join MEB29_0200 on MEB29_0200.mac_code = " + pTable + ".mac_code " +
            //              " left join MEB29_0000 on MEB29_0000.station_code = MEB29_0200.station_code " +
            //              sSubWhere + 
            //              " order by ins_date, ins_time ";

            string sSql = "";
            if (pTable == "MED01_0000" || pTable == "MED02_0000")
            {
                sSql = " select " + pTable + ".*, " +
                       " ISNULL(BDP08_0000.usr_name, '') as usr_name, " +
                       " ISNULL(MEB15_0000.mac_name, '') as mac_name, " +
                       " ISNULL(MET01_0000.pro_code, '') as pro_code, " +
                       " ISNULL(MEB20_0000.pro_name, '') as pro_name, " +
                       " ISNULL(MEB29_0200.station_code, '') as station_code, " +
                       " ISNULL(MEB29_0000.station_name, '') as station_name, " +
                       " ISNULL(MEB30_0100.work_code, '') as work_code, " +
                       " ISNULL(MEB30_0000.work_name, '') as work_name  " +
                       " from " + pTable +
                       " left join BDP08_0000 on BDP08_0000.usr_code = " + pTable + ".usr_code " +
                       " left join MEB15_0000 on MEB15_0000.mac_code = " + pTable + ".mac_code " +
                       " left join MET01_0000 on MET01_0000.mo_code = " + pTable + ".mo_code " +
                       " left join MEB20_0000 on MEB20_0000.pro_code = MET01_0000.pro_code " +
                       " left join MEB29_0200 on MEB29_0200.mac_code = " + pTable + ".mac_code " +
                       " left join MEB29_0000 on MEB29_0000.station_code = MEB29_0200.station_code " +
                       " left join MEB30_0100 on MEB30_0100.station_code = MEB29_0200.station_code " +
                       " left join MEB30_0000 on MEB30_0000.work_code = MEB30_0100.work_code " +
                       sSubWhere +
                       " order by ins_date, ins_time ";
            }
            else
            {
                string sProCode = "";
                string jProCode = "";
                string jProName = "";
                if (pTable == "MED04_0000")
                {
                    sProCode = " ,ISNULL(MET01_0000.pro_code, '') as pro_code ";
                    jProCode = " left join MET01_0000 on MET01_0000.mo_code = " + pTable + ".mo_code ";
                    jProName = " left join MEB20_0000 on MEB20_0000.pro_code = MET01_0000.pro_code ";
                }
                else
                {
                    jProName = " left join MEB20_0000 on MEB20_0000.pro_code = " + pTable + ".pro_code ";
                }
                sSql = " select " + pTable + ".* , " +
                       " ISNULL(BDP08_0000.usr_name, '') as usr_name, " +
                       " ISNULL(MEB15_0000.mac_name, '') as mac_name, " +
                       " ISNULL(MEB20_0000.pro_name, '') as pro_name, " +
                       " ISNULL(MEB29_0000.station_name, '') as station_name, " +
                       " ISNULL(MEB30_0000.work_name, '') as work_name " +
                       sProCode +
                       " from " + pTable +
                       " left join BDP08_0000 on BDP08_0000.usr_code = " + pTable + ".usr_code " +
                       " left join MEB15_0000 on MEB15_0000.mac_code = " + pTable + ".mac_code " +
                       jProCode +
                       jProName +
                       " left join MEB29_0000 on MEB29_0000.station_code = " + pTable + ".station_code " +
                       " left join MEB30_0000 on MEB30_0000.work_code = " + pTable + ".work_code " +
                       sSubWhere +
                       " order by ins_date, ins_time ";
            }

            DataTable dtTmp = comm.Get_DataTable(sSql, sqlParmas, true);


            return dtTmp;
        }

    }
}