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

namespace MES_WATER.Controllers
{
    public class MET020AController : Controller
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
        /// 取得可排程的製令
        /// </summary>
        /// <returns></returns>
        public string Get_MoCode()
        {
            string sSql = "select * from MET01_0000 " +
                          " where sch_date_s = ''";
            return GD.DataFieldToStr(sSql, "mo_code");
        }


        /// <summary>
        /// 取得已經排程的製令
        /// </summary>
        /// <returns></returns>
        public string Get_SchMoCode() {
            string sSql = "select * from MET01_0000 " +
                          " where sch_date_s <> ''";
            return GD.DataFieldToStr(sSql,"mo_code");
        }

        //public string ModalShowData()
        //{
        //    string sSql = "select MET01_0000.* " +
        //                  "  from MET01_0000 " +
        //                  "   left join MEB20_0000 on MET01_0000.pro_code = MEB20_0000.pro_code";

        //    return GD.DataFieldToStr(sSql, "mo_code");
        //}
        

        /// <summary>
        /// 取得已經排程的製令
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCalendarData()
        {
            List<object> list = new List<object>();
            object data = new object();

            for (int i = 0; i < Get_SchMoCode().Split(',').Length; i++)
            {
                string sMoCode = Get_SchMoCode().Split(',')[i];
                string sSchDate = GD.Get_Data("MET01_0000", sMoCode, "mo_code", "sch_date_s");
                string sSchTime = GD.Get_Data("MET01_0000", sMoCode, "mo_code", "sch_time_s");
                string eSchDate = GD.Get_Data("MET01_0000", sMoCode, "mo_code", "sch_date_e");
                string eSchTime = GD.Get_Data("MET01_0000", sMoCode, "mo_code", "sch_time_e");
                string sProCode = GD.Get_Data("MET01_0000", sMoCode, "mo_code", "pro_code");
                string sProName = GD.Get_Data("MEB20_0000", sProCode, "pro_code", "pro_name");

                data = new
                {
                    id = sMoCode,
                    title = sProName + " - " + sMoCode,
                    start = sSchDate + " " + sSchTime,
                    end = eSchDate + " " + eSchTime,
                    className = "label-" + Get_Class(sMoCode),
                    allDay = false,
                    editable = Chk_CanEdit(sMoCode),
                };
                list.Add(data);
            }            
            return Json(list);
        }




        /// <summary>
        /// 更新排程日期
        /// </summary>
        /// <param name="K"></param>
        /// <param name="D"></param>
        public void TableUpdate(string K, string D,string ED) {
            string s_date = "";
            string s_time = "";
            string e_date = "";
            string e_time = "";
            if (!string.IsNullOrEmpty(D)) {
                s_date = DateTime.Parse(D).ToString("yyyy/MM/dd");
                s_time = DateTime.Parse(D).ToString("HH:mm:ss");
                e_date = DateTime.Parse(D).ToString("yyyy/MM/dd");
                e_time = DateTime.Parse(D).AddHours(2).ToString("HH:mm:ss");
            }

            if (!string.IsNullOrEmpty(ED)) {
                e_date = DateTime.Parse(ED).ToString("yyyy/MM/dd");
                e_time = DateTime.Parse(ED).ToString("HH:mm:ss");
            }
            
            object data = new object();
            data = new
            {
                mo_code = K,
                sch_date_s = s_date,
                sch_time_s = s_time,
                sch_date_e = e_date,
                sch_time_e = e_time,
            };
            UpdateSql(data);
        }



        public void UpdateSql(object data)
        {
            string sSql = "update MET01_0000 " +
                          "   set sch_date_s = @sch_date_s" +
                          "      ,sch_time_s = @sch_time_s" +
                          "      ,sch_date_e = @sch_date_e" +
                          "      ,sch_time_e = @sch_time_e" +
                          " where mo_code = @mo_code";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, data);
            }
        }


        /// <summary>
        /// 取得製令資訊
        /// </summary>
        /// <param name="K">製令代號</param>
        /// <param name="F">欄位</param>
        /// <returns></returns>
        public string GetData(string K,string F)
        {            
            return GD.Get_Data("MET01_0000",K,"mo_code",F);
        }

        /// <summary>
        /// 取得製令資訊
        /// </summary>
        /// <param name="K">製令代號</param>
        /// <param name="F">欄位</param>
        /// <returns></returns>
        public string GetTableData(string T,string K,string KF, string F)
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
            switch (DateType) {
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
            return GD.DataFieldToStr(sSql,"mo_code");
        }


        public bool Chk_CanEdit(string pMoCode) {
            bool sValue = false;
            string sStatus = GD.Get_Data("MET01_0000", pMoCode, "mo_code", "mo_status");
            switch (sStatus) {
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

            switch (sStatus) {
                case "10":
                    switch (sLine) {
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