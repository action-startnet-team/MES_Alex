using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using MES_WATER.Models;

namespace MES_WATER.Controllers
{
    public class MoDataController : Controller
    {
        // 共用函數庫
        Comm comm = new Comm();

        // GET: MoData
        public ActionResult MoRecord(string mo_code)
        {
            //取得MEM01_0000資料
            string sSql = "SELECT a.*,d.work_name,e.chg_rate FROM MEM01_0000 a" +
                " LEFT JOIN MET01_0000 b ON a.mo_code = b.mo_code" +
                " LEFT JOIN MEB20_0000 c ON b.pro_code = c.pro_code" +
                " LEFT JOIN MEB30_0000 d ON a.work_code = d.work_code" +
                " LEFT JOIN WMB04_0000 e ON b.pro_code = e.pro_code AND e.unit_code_chg IN ('CAN', 'BOT')" +
                " WHERE a.mo_code = @mo_code";
            DataTable dtTmp = comm.Get_DataTable(sSql, "mo_code", mo_code);
            ViewBag.data = dtTmp;
            
            int work_min = 0;
            DateTime time_s = DateTime.Now;
            DateTime time_e = DateTime.Now;
            //取得良品/不良品數/總機時
            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                if (i == 0)
                {
                    time_s = DateTime.Parse(dtTmp.Rows[i]["work_time_s"].ToString());
                    time_e = DateTime.Parse(dtTmp.Rows[i]["work_time_e"].ToString());
                } else
                {
                    DateTime time_s2 = DateTime.Parse(dtTmp.Rows[i]["work_time_s"].ToString());
                    DateTime time_e2 = DateTime.Parse(dtTmp.Rows[i]["work_time_e"].ToString());
                    if (time_s2 < time_s) { time_s = time_s2; }
                    if (time_e2 > time_e) { time_e = time_e2; }
                }

                work_min += int.Parse(dtTmp.Rows[i]["work_sec"].ToString());
                string work_code = dtTmp.Rows[i]["work_code"].ToString();
                switch (work_code)
                {
                    case "L1SMB05":
                    case "L2SMB05":
                        ViewBag.ok_qty = double.Parse(dtTmp.Rows[i]["ok_qty"].ToString()).ToString();
                        ViewBag.ng_qty = double.Parse(dtTmp.Rows[i]["ng_qty"].ToString()).ToString();
                        ViewBag.unit = dtTmp.Rows[i]["ok_unit"].ToString();
                        break;
                    default:
                        break;
                }
            }
            //取得上報數量
            ViewBag.up_qty = comm.Get_UpQty(mo_code);
            //取得上工人數
            ViewBag.usr_cnt = Get_UsrCnt(mo_code, time_s, time_e);
            //取得停機時數
            int stop_min = Get_StopMin(mo_code);
            ViewBag.stop_hour = (stop_min / 60).ToString("#0.##");
            //計算總機時
            int all_min = work_min - stop_min;
            if (all_min < 0) { all_min = 0; }
            ViewBag.work_hour = (all_min / 60).ToString("#0.##");

            return View();
        }

        public int Get_UsrCnt(string pMoCode,DateTime pTimeS, DateTime pTimeE)
        {
            string sSql = "SELECT DISTINCT usr_code FROM MED01_0000" +
                " WHERE ins_date = @ins_date" +
                " AND ins_time BETWEEN @time_s AND @time_e" +
                " AND is_end<>'Y'";
            string sValue = pTimeS.ToString("yyyy/MM/dd") + "," + pTimeS.ToString("HH:mm:ss") + "," + pTimeE.ToString("HH:mm:ss");
            DataTable dtTmp = comm.Get_DataTable(sSql, "ins_date,time_s,time_e", sValue);
            return dtTmp.Rows.Count;
        }

        public int Get_StopMin(string pMoCode)
        {
            int min = 0;
            string sSql = "SELECT * FROM MED04_0000" +
                " WHERE mo_code = @mo_code" +
                " AND is_end<> 'Y'" +
                " AND time_e<> ''";
            DataTable dtTmp = comm.Get_DataTable(sSql, "mo_code", pMoCode);
            foreach (DataRow row in dtTmp.Rows)
            {
                string date_s = row["date_s"].ToString();
                string time_s = row["time_s"].ToString();
                string date_e = row["date_e"].ToString();
                string time_e = row["time_e"].ToString();
                DateTime start = DateTime.Parse(date_s + " " + time_s);
                DateTime end = DateTime.Parse(date_e + " " + time_e);
                TimeSpan diff  = end.Subtract(start);
               if (diff.TotalMinutes > 0 )
                {
                    min += Convert.ToInt16(diff.TotalMinutes);
                }
            }

            return min;
        }
    }
}