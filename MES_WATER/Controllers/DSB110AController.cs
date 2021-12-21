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

namespace MES_WATER.Controllers
{
    public class DSB110AController : JsonNetController
    {
        Comm comm = new Comm();

        // GET: DSB100a
        public ActionResult Index()
        {
            return View();
        }

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
        public JsonResult Init_Get_MacCodeList(string order, string line_code)
        {
            string sSql = "";
            ////抓取製程
            //List<string> work_code_list = new List<string>() {};

            //sSql = "Select work_code from MEB30_0000";
            //using (SqlConnection con_db = comm.Set_DBConnection())
            //{
            //    work_code_list = con_db.Query<string>(sSql).ToList();
            //}

            //List<MEB30_0000> list = new List<MEB30_0000>();
            //sSql = "";
            //sSql= "Select * from MEB30_0000 where work_code in @work_code_list";
            //using(SqlConnection con_db = comm.Set_DBConnection())
            //{
            //    list = con_db.Query<MEB30_0000>(sSql, new { work_code_list  = work_code_list }).ToList();
            //}

            //抓取機台
            List<string> mac_code_list = new List<string>() { };

            sSql = "Select mac_code from MEB15_0000";
            if (line_code != "")
                sSql += " where line_code=@line_code";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                if (line_code != "")
                    mac_code_list = con_db.Query<string>(sSql, new { line_code = line_code }).ToList();
                else
                    mac_code_list = con_db.Query<string>(sSql).ToList();
            }

            List<MEB15_0000> list = new List<MEB15_0000>();
            sSql = "";

            if(string.IsNullOrEmpty(order))
            {
                sSql = "Select * from MEB15_0000 where mac_code in @mac_code_list";
                using (SqlConnection con_db = comm.Set_DBConnection())
                {
                    list = con_db.Query<MEB15_0000>(sSql, new { mac_code_list = mac_code_list }).ToList();
                }
            }
            else
            {
                sSql = "Select * from MEB15_0000 where mac_code in @mac_code_list";
                using (SqlConnection con_db = comm.Set_DBConnection())
                {
                    list = con_db.Query<MEB15_0000>(sSql, new { mac_code_list = mac_code_list }).ToList();
                }
                if (order == "mac_code")
                {
                    list = list.OrderBy(l => l.mac_code).ToList();
                }
                else if (order == "mac_name")
                {
                    list = list.OrderBy(l => l.mac_name).ToList();
                }
            }
            
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Get_LineCodeList()
        {                        
            //抓取線別
            List<string> line_code_list = new List<string>() { };

            string sSql = "Select line_code from MEB12_0000";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                line_code_list = con_db.Query<string>(sSql).ToList();
            }

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
            try
            {
                List<string> work_code_list = JsonConvert.DeserializeObject<List<string>>(pJson);

                Dictionary<string, Oee> returnObj = new Dictionary<string, Oee>();

                foreach(string work_code in work_code_list)
                {
                    string sMoCode = Get_DataByStationCode(work_code, "wrk_code");
                    
                    double iok_qty = comm.sGetDouble(Get_DataByStationCode(work_code, "ok_qty"));
                    double ing_qty = comm.sGetDouble(Get_DataByStationCode(work_code, "ng_qty"));
                    double irate = 0;
                    string sUsrName = Get_DataByStationCode(work_code, "usr_code");
                    string sWorkTime = Get_DataByStationCode(work_code, "work_time_s");
                    sUsrName = comm.Get_QueryData("BDP08_0000", sUsrName, "usr_code", "usr_name");

                    double completeRate = 0;
                    double iplan_qty = comm.sGetDouble(Get_DataByStationCode(work_code, "plan_qty"));
                    double imo_qty = comm.sGetDouble(Get_DataByStationCode(work_code, "mo_qty"));

                    if (iok_qty + ing_qty != 0)
                    {
                        irate = iok_qty / (iok_qty + ing_qty);
                    }

                    if (iplan_qty != 0)
                    {
                        completeRate = iok_qty / iplan_qty;// 達成率=良品/計畫輛
                    }

                    Oee data = new Oee();
                    data.TkCode = work_code;
                    if (sMoCode != "")
                    {
                        data.status = "R";
                    }
                    else
                    {
                        data.status = "I";
                    }

                    data.mo_code = sMoCode;

                    //如果沒有正在進行的工單則不帶值
                    if (string.IsNullOrEmpty(sMoCode)) {
                        iok_qty = 0;
                        ing_qty = 0;
                        irate = 0;
                        sUsrName = "";
                        sWorkTime = "";
                        completeRate = 0;
                    }

                    List<object> items = new List<object>();
                    items.Add(new { name = "iplan_qty", value = (iok_qty + ing_qty).ToString() + "/" + iplan_qty.ToString(), label = "產量" });
                    items.Add(new { name = "irate", value = irate.ToString("0.##%"), label = "良品率" });                    
                    items.Add(new { name = "iok_qty", value= iok_qty.ToString(), label= "良品" });
                    items.Add(new { name = "completeRate", value = completeRate.ToString("0.##%"), label = "達成率" });
                    items.Add(new { name = "ing_qty", value= ing_qty.ToString(), label= "不良品" });
                    items.Add(new { name = "sUsrName", value = sUsrName, label = "人員" });
                    items.Add(new { name = "work_time", value = sWorkTime, label = "開始時間" });


                    data.items = items;
                    returnObj.Add(data.TkCode, data);
                }

                return Json(returnObj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Oee> empty = new List<Oee>();
                return Json(empty, JsonRequestBehavior.AllowGet);
            }
        }

        public class Oee
        {
            public string TkCode { get; set; }
            public string mo_code { get; set; }
            public string status { get; set; }

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
        
        public string Get_DataByStationCode(string pStationCode, string pFieldCode)
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
            sSql = "select top 1 MEM01_0000.*,MET03_0000.wrk_code as wrk_code, MET01_0000.plan_qty, MET01_0000.mo_qty, MEB20_0000.pro_name from MEM01_0000 " +
                   " left join MET03_0000 on (MET03_0000.work_code = MEM01_0000.work_code and MET03_0000.mo_code=MEM01_0000.mo_code)" +
                   " left join MET01_0000 on MEM01_0000.mo_code = MET01_0000.mo_code" +
                   " left join MEB20_0000 on MEB20_0000.pro_code = MET03_0000.pro_code" +
                   " where MEM01_0000.mac_code='" + pStationCode + "'" +
                   "   and work_time_s <> ''" +
                   "   and work_time_e = ''" +
                   " order by work_time_s desc";
            DataTable dtTmp = comm.Get_DataTable(sSql);

            if (dtTmp.Rows.Count > 0)
            {
                //sReturn = dtTmp.Rows[0]["pFieldCode"].ToString().Trim();
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
                string sWrkCode = Get_DataByStationCode(mac_code, "wrk_code");
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
    }
}