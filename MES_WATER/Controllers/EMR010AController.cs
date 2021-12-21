using Dapper;
using MES_WATER.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using Newtonsoft.Json;

namespace MES_WATER.Controllers
{
    public class EMR010AController : Controller
    {
        Comm comm = new Comm();
        GetData GD = new GetData();
        DynamicTable DT = new DynamicTable();
        // GET: MET020A
        public ActionResult Index()
        {
            Set_Cookie();
            return View();
        }


        /// <summary>
        /// 取得日曆模組的資料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetCalendarData(FormCollection form)
        {
            string sSql = "";
            string sSubWhere = "";
            DataTable dtTmp = new DataTable();
            List<object> list = new List<object>();
            object data = new object();

            string sDevCode = comm.sGetString(form["dev_code"]);
            string sMaintainType = comm.sGetString(form["maintain_type"]);

            sSubWhere = " and EMT01_0000.dev_code = '" + sDevCode + "'";

            if (sDevCode == "ALL") { sSubWhere = ""; }

            if (sMaintainType.Contains("A")) {
                //保養項目
                sSql = "select EMT01_0100.*,main_item_name,dev_name " +
                       "  from EMT01_0100 " +
                       "  left join EMT01_0000 on EMT01_0000.maintain_code = EMT01_0100.maintain_code " +
                       "  left join EMB08_0000 on EMB08_0000.main_item_code = EMT01_0100.main_item_code " +
                       "  left join EMB07_0000 on EMB07_0000.dev_code = EMT01_0000.dev_code  " +
                       " where 1=1" +
                       sSubWhere;
                dtTmp = comm.Get_DataTable(sSql);
                for (int i = 0; i < dtTmp.Rows.Count; i++)
                {
                    DataRow r = dtTmp.Rows[i];
                    string ID = r["emt01_0100"].ToString();
                    string sTitle = GD.DateStrParse(r["maintain_cycle"].ToString()) + "_" + r["dev_name"].ToString() + "-" + r["main_item_name"].ToString();
                    string sStart = r["ins_date"].ToString() + " " + r["ins_time"].ToString();
                    string sEnd = "";
                    string sClassName = "label-info";
                    if (!string.IsNullOrEmpty(r["act_date"].ToString())) { sClassName = "label-grey"; }

                    bool bEditable = false;

                    data = new
                    {
                        id = ID,
                        title = sTitle,
                        start = sStart,
                        end = sEnd,
                        className = sClassName,
                        allDay = true,
                        editable = bEditable,
                    };
                    list.Add(data);
                }
            }
            


            if (sMaintainType.Contains("B"))
            {
                //點檢項目
                sSubWhere = " and EMT02_0000.dev_code = '" + sDevCode + "'";
                if (sDevCode == "ALL") { sSubWhere = ""; }

                sSql = "select EMT02_0100.*,dev_name,chk_item_name,EMT02_0000.dev_check_date " +
                       "  from EMT02_0100 " +
                       "  left join EMT02_0000 on EMT02_0100.dev_check_code = EMT02_0000.dev_check_code " +
                       "  left join EMB07_0000 on EMB07_0000.dev_code = EMT02_0000.dev_code " +
                       "  left join EMB21_0000 on EMT02_0100.chk_item_code = EMB21_0000.chk_item_code  " +
                       " where 1=1" +
                       sSubWhere;
                dtTmp = comm.Get_DataTable(sSql);
                for (int i = 0; i < dtTmp.Rows.Count; i++)
                {
                    DataRow r = dtTmp.Rows[i];
                    string ID = r["emt02_0100"].ToString();
                    string sTitle = r["dev_name"].ToString() + "-" + r["chk_item_name"].ToString();
                    string sStart = r["dev_check_date"].ToString() + " 00:00:00";
                    string sEnd = "";
                    string sClassName = "label-success";
                    //if (!string.IsNullOrEmpty(r["act_date"].ToString())) { sClassName = "label-grey"; }

                    bool bEditable = false;

                    data = new
                    {
                        id = ID,
                        title = sTitle,
                        start = sStart,
                        end = sEnd,
                        className = sClassName,
                        allDay = true,
                        editable = bEditable,
                    };
                    list.Add(data);
                }
            }
            

            if (sMaintainType.Contains("C"))
            {
                //叫修項目
                sSubWhere = " and EMT05_0000.dev_code = '" + sDevCode + "'";
                if (sDevCode == "ALL") { sSubWhere = ""; }

                sSql = "select EMT05_0000.*,dev_name,chk_item_name " +
                       "  from EMT05_0000 " +
                       "  left join EMB07_0000 on EMB07_0000.dev_code = EMT05_0000.dev_code " +
                       "  left join EMB21_0000 on EMT05_0000.chk_item_code = EMB21_0000.chk_item_code " +
                       " where 1=1" +
                       sSubWhere;
                dtTmp = comm.Get_DataTable(sSql);
                for (int i = 0; i < dtTmp.Rows.Count; i++)
                {
                    DataRow r = dtTmp.Rows[i];
                    string ID = r["call_code"].ToString();
                    string sTitle = r["dev_name"].ToString() + "-" + r["chk_item_name"].ToString();
                    string sStart = r["call_date"].ToString() + " 00:00:00";
                    string sEnd = "";
                    string sClassName = "label-pink";

                    bool bEditable = false;

                    data = new
                    {
                        id = ID,
                        title = sTitle,
                        start = sStart,
                        end = sEnd,
                        className = sClassName,
                        allDay = true,
                        editable = bEditable,
                    };
                    list.Add(data);
                }
            }
            

            if (sMaintainType.Contains("D"))
            {
                //維修項目
                sSubWhere = " and EMT06_0000.dev_code = '" + sDevCode + "'";
                if (sDevCode == "ALL") { sSubWhere = ""; }

                sSql = "select EMT06_0000.*,dev_name,fault_handle_name " +
                       "  from EMT06_0000 " +
                       "  left join EMB07_0000 on EMB07_0000.dev_code = EMT06_0000.dev_code " +
                       "  left join EMB20_0000 on EMB20_0000.fault_handle_code = EMT06_0000.fault_handle_code " +
                       "  left join EMB14_0000 on EMB14_0000.per_code = EMT06_0000.per_code" +
                       " where 1=1" +
                       sSubWhere;
                dtTmp = comm.Get_DataTable(sSql);
                for (int i = 0; i < dtTmp.Rows.Count; i++)
                {
                    DataRow r = dtTmp.Rows[i];
                    string ID = r["rep_code"].ToString();
                    string sTitle = r["dev_name"].ToString() + "-" + r["fault_handle_name"].ToString();
                    string sStart = r["rep_date"].ToString() + " 00:00:00";
                    string sEnd = "";
                    string sClassName = "label-purple";

                    bool bEditable = false;

                    data = new
                    {
                        id = ID,
                        title = sTitle,
                        start = sStart,
                        end = sEnd,
                        className = sClassName,
                        allDay = true,
                        editable = bEditable,
                    };
                    list.Add(data);
                }
            }          

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取得彈出視窗的欄位與內容
        /// </summary>
        /// <returns></returns>
        public string Get_ModalTable(string pModalId) {
            string sSql = "";
            DataTable dtTmp = new DataTable();
            sSql = "select * from EMT01_0100" +
                   " where emt01_0100='"+ pModalId + "'";
            dtTmp = comm.Get_DataTable(sSql);
            return JsonConvert.SerializeObject(dtTmp);
        }




        /// <summary>
        /// 更新排程日期
        /// </summary>
        /// <param name="K"></param>
        /// <param name="D"></param>
        public void TableUpdate(string ID, string pStartDate, string pEndDate)
        {
            //string 
        }



        


        /// <summary>
        /// 取得製令資訊
        /// </summary>
        /// <param name="K">製令代號</param>
        /// <param name="F">欄位</param>
        /// <returns></returns>
        public string GetData(string K, string F)
        {
            return GD.Get_Data("MET01_0000", K, "mo_code", F);
        }

        /// <summary>
        /// 取得製令資訊
        /// </summary>
        /// <param name="K">製令代號</param>
        /// <param name="F">欄位</param>
        /// <returns></returns>
        public string GetTableData(string T, string K, string KF, string F)
        {
            return GD.Get_Data(T, K, KF, F);
        }

        /// <summary>
        /// 更新該日期區間的排程
        /// </summary>
        /// <param name="D">日期</param>
        /// <returns></returns>
        public string Get_DateOfMod(string DateType, string D)
        {
            string s_date = "";
            string e_date = "";
            //利用日期及顯示型態算出區間
            switch (DateType)
            {
                case "month":
                    s_date = DateTime.Parse(D).ToString("yyyy/MM") + "/01";
                    e_date = DateTime.Parse(s_date).AddMonths(1).AddDays(-1).ToString("yyyy/MM/dd");
                    break;
                case "agendaWeek":
                    int week = int.Parse(DateTime.Parse(D).DayOfWeek.ToString("d"));
                    s_date = DateTime.Parse(D).AddDays(week * -1).ToString("yyyy/MM/dd");
                    e_date = DateTime.Parse(s_date).AddDays(6).ToString("yyyy/MM/dd");
                    break;
                case "agendaDay":
                    s_date = DateTime.Parse(D).ToString("yyyy/MM/dd");
                    e_date = DateTime.Parse(D).ToString("yyyy/MM/dd");
                    break;
            }
            //找出計畫開工日在此區間內的排程
            string sSql = "select * from MET01_0000 " +
                          " where plan_start_date between '" + s_date + "' and '" + e_date + "'" +
                          "   and mo_status in ('10','70')" +
                          "   and sch_date_s = ''";
            return GD.DataFieldToStr(sSql, "mo_code");
        }


        public bool Chk_CanEdit(string pMoCode)
        {
            bool sValue = false;
            string sStatus = GD.Get_Data("MET01_0000", pMoCode, "mo_code", "mo_status");
            switch (sStatus)
            {
                case "10":
                    sValue = true;
                    break;
            }
            return sValue;
        }


        public string Get_Class(string pMoCode)
        {
            //依照線別分顏色
            string sStatus = GD.Get_Data("MET01_0000", pMoCode, "mo_code", "mo_status");
            string sLine = GD.Get_Data("MET01_0000", pMoCode, "mo_code", "plan_line_code");
            string sValue = "";

            switch (sStatus)
            {
                case "10":
                    switch (sLine)
                    {
                        case "A1L00":
                            sValue = "yellow";
                            break;
                        case "A1L01":
                            sValue = "info";
                            break;
                        case "A1L02":
                            sValue = "success";
                            break;
                        case "A1L03":
                            sValue = "pink";
                            break;
                        case "1":  //以下備用
                            sValue = "grey";
                            break;
                        case "2":
                            sValue = "danger";
                            break;
                        case "3":
                            sValue = "purple";
                            break;
                    }
                    break;
                default:
                    sValue = "grey";
                    break;
            }
            return sValue;
        }

        /// <summary>
        /// 儲存cookie
        /// </summary>
        /// <param name="pCookieName"></param>
        /// <param name="pValue"></param>
        public void Save_Cookie(string pCookieName, string pValue)
        {
            Response.Cookies[pCookieName].Value = pValue;
        }


        /// <summary>
        /// 設定cookie
        /// </summary>
        public void Set_Cookie()
        {
            //紀錄cookie
            if (Request.Cookies["DateState"] == null)
            {
                HttpCookie EpbType = new HttpCookie("DateState")
                {
                    Value = "",
                    Expires = DateTime.Now.AddDays(1d),
                };
                Response.Cookies.Add(EpbType);
            }
        }


    }
}