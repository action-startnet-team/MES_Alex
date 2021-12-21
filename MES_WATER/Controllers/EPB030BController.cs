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
using NPOI;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Data;

namespace MES_WATER.Controllers
{
    public class EPB030BController : Controller
    {
        Comm comm = new Comm();
        GetData GD = new GetData();
        DynamicTable DT = new DynamicTable();
        Review RV = new Review();

        //表單欄位table
        public string pubFieldTable = "EPB02_0100";

        //索引鍵
        public string pubPKCode()
        {
            return DT.Get_Table_PKField(pubFieldTable);
        }


        // GET: EPB030B
        public ActionResult Index()
        {
            Set_Cookie();
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            return View();
        }


        public ActionResult DataView(string K)
        {
            ViewBag.Key = K;

            return View();
        }

        public ActionResult Delete(string Key) {
            string sEpbCode = comm.Get_QueryData("EPB03_0000", Key, "field_value", "epb_code");
            //刪除電子表單資料
            comm.Del_QueryData("EPB03_0000", "key_value", Key);
            //刪除電子審核作業 
            comm.Del_QueryData("EPB05_0000", "epb_key", Key);
            //代辦事項ok
            comm.Upd_QueryData("BDP16_0000", "todo_key", Key, "is_ok", "Y");
            return RedirectToAction("DataView", new { K = sEpbCode });
        }

        
        
        /// <summary>
        /// 取得表單欄位
        /// </summary>
        /// <param name="Key">表單代號</param>
        /// <returns></returns>
        public string Get_EpbField(string Key)
        {
            string sSql = "select * from EPB02_0100 " +
                          " where epb_code = '" + Key + "'" +
                          "  order by scr_no ";
            return comm.DataFieldToStr(sSql, "epb02_0100");
        }

        /// <summary>
        /// 取得該表單 資料筆數
        /// </summary>
        /// <param name="pEpbCode"></param>
        /// <returns></returns>
        public int Get_DataCount(string pEpbCode)
        {
            int Cnt = 0;
            string sKeyField = "";
            
            string sSql = "select * from EPB02_0100 " +
                          " where epb_code = '" + pEpbCode + "' " +
                          "   and is_key = 'Y'";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0) {
                sKeyField = dtTmp.Rows[0]["field_code"].ToString();

                sSql = "select count(*) as cnt from EPB03_0000 " +
                       " where epb_code = '" + pEpbCode + "' " +
                       "   and field_code = '" + sKeyField + "'";
                var dtTmp2 = comm.Get_DataTable(sSql);
                if (dtTmp2.Rows.Count > 0) {
                    Cnt = int.Parse(dtTmp2.Rows[0]["cnt"].ToString());
                }
            }
            return Cnt;
        }


        /// <summary>
        /// 取得該表單 資料筆數
        /// </summary>
        /// <param name="pEpbCode"></param>
        /// <returns></returns>
        public string Get_DataValue(string pEpbCode)
        {
            string sValue = "";
            string sKeyField = "";

            string sSql = "select * from EPB02_0100 " +
                          " where epb_code = '" + pEpbCode + "' " +
                          "   and is_key = 'Y'";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                sKeyField = dtTmp.Rows[0]["field_code"].ToString();

                sSql = "select * from EPB03_0000 " +
                       " where epb_code = '" + pEpbCode + "' " +
                       "   and field_code = '" + sKeyField + "'";
                sValue = GD.DataFieldToStr(sSql, "key_value");
            }
            return sValue;
        }


        /// <summary>
        /// 取得該欄位的值
        /// </summary>
        /// <param name="pEpbCode">表單代號</param>
        /// <param name="pField">欄位</param>
        /// <param name="Index">索引</param>
        /// <returns></returns>
        public string Get_FieldValue(string pEpbCode,string pField,int Index) {
            string sValue = "";
            string  sSql = "select * from EPB03_0000 " +
                           " where epb_code = '" + pEpbCode + "' " +
                           "   and field_code = '" + pField + "'";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > Index) {
                sValue = dtTmp.Rows[Index]["field_value"].ToString();
            }                            
            return sValue;
        }

        public string Get_FieldValue(string Key, string pFieldCode)
        {
            string sSql = "select * from EPB03_0000 " +
                          " where key_value = '" + Key + "'" +
                          "   and field_code = '" + pFieldCode + "'";
            return GD.DataFieldToStr(sSql, "field_value");
        }

        /// <summary>
        /// 取得表單類型
        /// </summary>
        /// <returns></returns>
        public string Get_EpbType(string pUsrCode)
        {
            //取得表單類型
            string sSql = "select * from EPB01_0000 " +
                          "   where epb_type_code in (" + GD.StrArrayToSql(GD.Get_EpbCanUseType(pUsrCode)) + ")";
            return comm.DataFieldToStr(sSql, "epb_type_code");
        }

        /// <summary>
        /// 取得表單代號
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public string Get_EpbCode(string Key)
        {
            //取得電子表單權限
            string sSql = "select * from BDP09_0200 " +
                          " where usr_code = '" + User.Identity.Name + "'" +
                          "   and is_use = 'Y'";
            string sEpbCodeArray = GD.DataFieldToStr(sSql, "epb_code");

            sSql = "select * from EPB02_0000 " +
                   " where epb_type_code = '" + Key + "'" +
                   "   and epb_code in (" + GD.StrArrayToSql(sEpbCodeArray) + ")";
            return GD.DataFieldToSTA(sSql, "epb_code,epb_name");
        }


        /// <summary>
        /// 檢查是否已經設定審核作業
        /// </summary>
        /// <param name="pEpbCode"></param>
        /// <returns></returns>
        public bool Chk_CanReview(string pEpbCode) {
            bool sValue = false;
            string sSql = "select * from EPB04_0000" +
                          " where epb_code = '" + pEpbCode + "'" +
                          "   and is_use = 'Y'" ;
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0) {
                sValue = true;
            }
            return sValue;
        }

        /// <summary>
        /// 檢查是否審核過此筆資料
        /// </summary>
        /// <param name="pEpbCode"></param>
        /// <returns></returns>
        public bool Chk_IsReview(string Key)
        {
            bool sValue = false;
            string sSql = "select * from EPB05_0000" +
                          " where epb_key = '" + Key + "'";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                sValue = true;
            }
            return sValue;
        }

        public string Get_Data(string T,string K,string KF,string F) {
            return GD.Get_Data(T, K, KF, F);
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
            if (Request.Cookies["EpbType"] == null)
            {
                HttpCookie EpbType = new HttpCookie("EpbType")
                {
                    Value = "",
                    Expires = DateTime.Now.AddDays(1d),
                };
                Response.Cookies.Add(EpbType);
            }
            if (Request.Cookies["EpdCode"] == null)
            {
                HttpCookie EpdCode = new HttpCookie("EpdCode")
                {
                    Value = "",
                    Expires = DateTime.Now.AddDays(1d),
                };
                Response.Cookies.Add(EpdCode);
            }
        }


        public bool Chk_IsReviewUsrOfEpb(string pEpbCode) {
            return RV.Chk_IsReviewUsrOfEpb(pEpbCode,User.Identity.Name);
        }





    }
}