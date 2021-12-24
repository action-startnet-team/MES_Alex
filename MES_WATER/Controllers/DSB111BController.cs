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
    public class DSB111BController : JsonNetController
    {
        Comm comm = new Comm();

        // GET: DSB100a
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index_test()
        {
            return View();
        }

        public ActionResult Board()
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
        /// 建立 DSB10_0000 的期初資料
        /// </summary>
        /// <param name="mac_code_list"></param>
        public void Init_DataInDB(List<string> mac_code_list)
        {
            if (mac_code_list.Count <= 0)
            {
                return;
            }

            // 
            List<DSB10_0000> list = new List<DSB10_0000>();
            string sSql = " Select * "
                        + " from MEB15_0000 "
                        + " left join DSB10_0000 on MEB15_0000.mac_code = DSB10_0000.mac_code  "
                        + " where MEB15_0000.mac_code in @mac_code_list";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                list = con_db.Query<DSB10_0000>(sSql, new { mac_code_list = mac_code_list }).ToList();
            }

            List<string> existed_list = list.Select(x => x.mac_code).ToList();

            List<string> filter_list = mac_code_list.Where(x => !existed_list.Contains(x)).ToList();
            if (filter_list.Count > 0)
            {
                // 新增mac_code的預設資料
                sSql = "Insert into DSB10_0000 (mac_code, status, pro_qty, stop_time, work_time, pro_rate) values (@mac_code, @status, @pro_qty, @stop_time, @work_time, @pro_rate) ";
                using (SqlConnection con_db = comm.Set_DBConnection())
                {
                    DSB10_0000 data = new DSB10_0000();
                    data.status = "D";
                    data.pro_qty = 0;
                    data.pro_rate = 0;
                    data.stop_time = 0;
                    data.work_time = 0;
                    data.mo_code = "";
                    foreach (string mac_code in filter_list)
                    {
                        data.mac_code = mac_code;
                        con_db.Execute(sSql, data);
                    }
                }
            }

            // end of Init_DataInDB
        }


        /// <summary>
        /// 取得機台清單
        /// </summary>
        /// <returns></returns>
        public JsonResult Init_Get_MacCodeList()
        {
            // 機台基本檔
            string sql = "Select * from MEB15_0000 left join DSB10_0000 on MEB15_0000.mac_code = DSB10_0000.mac_code ";
            DataTable dt = comm.Get_DataTable(sql);

            List<string> mac_code_list = new List<string>();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    mac_code_list.Add(dr["mac_code"].ToString());
                }
            }

            // 設置 DSB10_0000 期初
            Init_DataInDB(mac_code_list);

            return Json(dt, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// ajax定時抓資料
        /// </summary>
        /// <param name="pJson"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Get_OeeData(string pJson)
        {
            List<string> mac_code_list = JsonConvert.DeserializeObject<List<string>>(pJson);

            // 計算oee資料，寫入DSB10_0000
            foreach (string mac_code in mac_code_list)
            {
                Update_DSB10_0000_ByMacCode(mac_code);
            }

            // 抓取DSB10_0000的資料，回傳給前端
            Dictionary<string, Oee> result = new Dictionary<string, Oee>();
            foreach (string mac_code in mac_code_list)
            {
                result.Add(mac_code, Get_OEE_ByMacCode(mac_code));
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public void Update_DSB10_0000_ByMacCode(string mac_code)
        {
            return;
            // 取得最新 MEM02_00000
            DataTable MEM02_0000 = Get_MEM02_0000_Current(mac_code);

            // 如果有工單資料
            if (MEM02_0000.Rows.Count > 0)
            {
                // 目前 工單資料
                string mo_code = MEM02_0000.Rows[0]["mo_code"].ToString();
                int mem02_0000 = comm.sGetInt32(MEM02_0000.Rows[0]["mem02_0000"].ToString());

                // 產量
                double pro_qty = Get_ProQty(mem02_0000);

                // 停機
                int stop_time = Get_StopTime(mo_code, mac_code);

                // 工時
                int work_time = Get_WorkTime(mo_code, mac_code);

                // 稼動
                double pro_rate = 0;

                // 更新 DSB10_0000
                DSB10_0000 data = new DSB10_0000();
                data.mac_code = mac_code;
                data.mo_code = mo_code;
                data.pro_qty = pro_qty;
                data.pro_rate = pro_rate;
                data.stop_time = stop_time;
                data.work_time = work_time;
                Update_DSB10_0000(data);
            }
            else
            {
                // 清空數據 
                DSB10_0000 data = new DSB10_0000();
                data.mac_code = mac_code;
                data.mo_code = "";
                data.pro_qty = 0;
                data.pro_rate = 0;
                data.stop_time = 0;
                data.work_time = 0;
                Update_DSB10_0000(data);
            }

        }


        public void Update_DSB10_0000(DSB10_0000 data)
        {

            string sql = " Update DSB10_0000 "
                       + " Set  "
                       + "  pro_qty = @pro_qty"
                       + "  ,stop_time = @stop_time"
                       + "  ,work_time = @work_time"
                       + "  ,pro_rate = @pro_rate"
                       + "  ,mo_code = @mo_code"
                       + "  where mac_code = @mac_code";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sql, data);
            }

        }


        public DataTable Get_MEM02_0000_Current(string mac_code)
        {
            Dictionary<string, object> sqlParams = new Dictionary<string, object>();
            sqlParams.Add("@mac_code", mac_code);
            string sql = " Select top 1 * "
                       + " from MEM02_0000 "
                       + " where mac_code = @mac_code "
                       + "   and status = 'E' "
                       + " order by time_s desc";

            DataTable dt = comm.Get_DataTable2("action_mvc", sql, sqlParams);

            return dt;
        }

        public DataTable Get_MEM03_0000_Current(string mac_code)
        {
            Dictionary<string, object> sqlParams = new Dictionary<string, object>();
            sqlParams.Add("@mac_code", mac_code);
            string sql = " Select top 1 * "
                       + " from MEM03_0000 "
                       + " where mac_code = @mac_code "
                       + " order by time_s desc";

            DataTable dt = comm.Get_DataTable2("action_mvc", sql, sqlParams);

            return dt;
        }

        /// <summary>
        /// 取得工時
        /// </summary>
        /// <param name="mo_code"></param>
        /// <param name="mac_code"></param>
        /// <returns></returns>
        public int Get_WorkTime(string mo_code, string mac_code)
        {
            int result = 0;

            Dictionary<string, object> sqlparams = new Dictionary<string, object>();

            sqlparams.Add("@mo_code", mo_code);
            sqlparams.Add("@mac_code", mac_code);

            string sql = @"select mo_code, mac_code, sum(DATEDIFF(MINUTE, CONVERT(datetime, time_s, 121), CONVERT(datetime, ISNULL(time_e, getdate()), 121)) ) as work_time
                            from MEM02_0000
                            where mo_code = @mo_code
                              and mac_code = @mac_code
                            group by mo_code, mac_code";
            DataTable dt = comm.Get_DataTable2("action_mvc", sql, sqlparams);

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                result += comm.sGetInt32(dr["work_time"].ToString());
            }

            return result;
        }

        /// <summary>
        /// 取得產量
        /// </summary>
        /// <param name="mem02_0000"></param>
        /// <returns></returns>
        public double Get_ProQty(int mem02_0000)
        {
            double result = 0;

            Dictionary<string, object> sqlparams = new Dictionary<string, object>();
            sqlparams.Add("@mem02_0000", mem02_0000);
            string sql = @"select mem02_0000, sum(ok_qty) as sum_ok_qty, sum(ng_qty) as sum_ng_qty
                            from MEM02_0100
                            where mem02_0000 = @mem02_0000
                            group by mem02_0000";
            DataTable dt = comm.Get_DataTable2("action_mvc", sql, sqlparams);

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                double sum_ok_qty = comm.sGetDouble(dr["sum_ok_qty"].ToString());
                double sum_ng_qty = comm.sGetDouble(dr["sum_ng_qty"].ToString());

                result += sum_ok_qty - sum_ng_qty;
            }

            return result;
        }

        /// <summary>
        /// 取得停機時間
        /// </summary>
        /// <param name="mo_code"></param>
        /// <param name="mac_code"></param>
        /// <returns></returns>
        public int Get_StopTime(string mo_code, string mac_code)
        {
            int result = 0;

            Dictionary<string, object> sqlparams = new Dictionary<string, object>();

            sqlparams.Add("@mo_code", mo_code);
            sqlparams.Add("@mac_code", mac_code);

            string sql = @"select mo_code, mac_code, sum(DATEDIFF(MINUTE, CONVERT(datetime, time_s, 121), CONVERT(datetime, ISNULL(time_e, getdate()), 121)) ) as stop_time
                            from MEM03_0000
                            where mo_code = @mo_code
                              and mac_code = @mac_code
                            group by mo_code, mac_code";
            DataTable dt = comm.Get_DataTable2("action_mvc", sql, sqlparams);

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                result += comm.sGetInt32(dr["stop_time"].ToString());
            }

            return result;
        }

        public class Oee
        {
            public string mac_code { get; set; }
            public string mo_code { get; set; }
            public string status { get; set; }
            public double yield { get; set; }
            public double utilization { get; set; }
            public double stop_time { get; set; }
            public double work_time { get; set; }
        }

        /// <summary>
        /// 計算單一機台的資訊
        /// </summary>
        /// <param name="pMacCode"></param>
        /// <returns></returns>
        public Oee Get_OEE_ByMacCode(string pMacCode)
        {
            Oee result = new Oee();

            result.mac_code = pMacCode;


            //DSB10_0000 data = Get_One_DSB10_0000_ByMacCode(pMacCode);

            DynamicParameters sqlparams = new DynamicParameters();
            sqlparams.Add("@mac_code", pMacCode);
            string sql = "select * from DSB10_0000 where mac_code = @mac_code ";

            DSB10_0000 data = new DSB10_0000();
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                data = con_db.QueryFirstOrDefault<DSB10_0000>(sql, sqlparams);
            }

            if (data != null)
            {
                result.mo_code = data.mo_code;
                result.status = data.status;
                result.yield = data.pro_qty;
                result.utilization = data.pro_rate;
                result.stop_time = data.stop_time;
                result.work_time = data.work_time;
            }

            return result;
        }

        public class DSB10_0000
        {
            public string mac_code { get; set; }
            public string mo_code { get; set; }
            public string status { get; set; }
            public double pro_qty { get; set; }
            public double stop_time { get; set; }
            public double work_time { get; set; }
            public double pro_rate { get; set; }
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

            return Json(line_code_list, JsonRequestBehavior.AllowGet);
        }


        public List<DSB10_0000> Get_All_DSB10_0000()
        {
            List<DSB10_0000> result = new List<DSB10_0000>();

            string sSql = "Select * from DSB10_0000";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                result = con_db.Query<DSB10_0000>(sSql).ToList();
            }

            if (result == null)
            {
                result = new List<DSB10_0000>();
            }

            return result;
        }

        /// <summary>
        /// 沒有符合條件的對象則回傳null
        /// </summary>
        /// <param name="pMacCode"></param>
        /// <returns></returns>
        public DSB10_0000 Get_One_DSB10_0000_ByMacCode(string pMacCode)
        {

            List<DSB10_0000> list = Get_All_DSB10_0000();

            var find = list.Where(x => x.mac_code == pMacCode);

            if (find.Count() > 0)
            {
                DSB10_0000 result = find.ToList()[0];

                return result;
            }

            return null;
        }

    }
}