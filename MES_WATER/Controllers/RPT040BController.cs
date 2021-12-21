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
    public class RPT040BController : JsonNetController
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

            DataTable dt = Get_MED09_0000(mo_code, pro_lot_no);

            List<object> returnList = new List<object>();
            foreach (DataRow dr in dt.Rows)
            {
                string ins_date = dr["ins_date"].ToString();
                string ins_time = dr["ins_time"].ToString();
                string date_s = ins_date + " " + ins_time;
                if (string.IsNullOrWhiteSpace(date_s))
                {
                    date_s = "";
                }

                string end_date = dr["end_date"].ToString();
                string end_time = dr["end_time"].ToString();
                string date_e = end_date + " " + end_time;
                if (string.IsNullOrWhiteSpace(date_e))
                {
                    date_e = "";
                }

                //
                int cal_work_min = 0;

                try
                {
                    cal_work_min = (int)DateTime.Parse(date_e).Subtract(DateTime.Parse(date_s)).TotalMinutes;
                }
                catch (Exception ex)
                {

                }

                var data_start = new
                {
                    type = "start",
                    work_code = dr["work_code"],
                    work_name = dr["work_name"],
                    pro_lot_no = dr["pro_lot_no"],
                    //datetime = Chk_MagicDateTime(date_s) ? "" : dr["date_s"],
                    datetime = date_s,
                    pro_qty = dr["pro_qty"],
                    work_min = cal_work_min,
                    station_code = dr["station_code"],
                    station_name = dr["station_name"],
                    mac_code = dr["mac_code"],
                    mac_name = dr["mac_name"],
                    usr_code = dr["usr_code"],
                    usr_name = dr["usr_name"]
                };

                var data_end = new
                {
                    type = "end",
                    work_code = dr["work_code"],
                    //datetime = Chk_MagicDateTime(date_e) ? "" : dr["date_e"],
                    datetime = date_e
                };

                returnList.Add(data_start);

                // 如果未開始就 不顯示結束
                //if (!string.IsNullOrEmpty(data_start.datetime.ToString()))
                //{
                //    returnList.Add(data_end);
                //}
            }


            return Json(returnList, JsonRequestBehavior.AllowGet);

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

        public DataTable Get_MED09_0000(string mo_code, string pro_lot_no)
        {
            DataTable dt = new DataTable();

            if (string.IsNullOrEmpty(mo_code))
            {
                return dt;
            }

            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            sqlparams.Add("@mo_code", mo_code);

            string subWhere = "";
            if (!string.IsNullOrEmpty(pro_lot_no))
            {
                subWhere = " and MED09_0000.pro_lot_no = @pro_lot_no ";
                sqlparams.Add("@pro_lot_no", pro_lot_no);
            }


            string sql = @"select MED09_0000.*
, isNull(MEB29_0000.station_name, '') as station_name
, isNull(MEB15_0000.mac_name, '') as mac_name
, isNull(BDP08_0000.usr_name, '') as usr_name
,isNull(MEB30_0000.work_name,'')as work_name
from MED09_0000
left join MEB29_0000 on MEB29_0000.station_code = MED09_0000.station_code
left join MEB30_0000 on MEB30_0000.work_code=MED09_0000.work_code
left join MEB15_0000 on MEB15_0000.mac_code = MED09_0000.mac_code
left join BDP08_0000 on BDP08_0000.usr_code = MED09_0000.usr_code
where MED09_0000.mo_code = @mo_code
" + subWhere + @"
order by MED09_0000.ins_date,MED09_0000.ins_time";

            dt = comm.Get_DataTable(sql, sqlparams);


            return dt;

        }

    }
}