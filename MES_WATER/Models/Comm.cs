using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;
using System.Linq.Dynamic;
using MES_WATER.Repository;
using System.Reflection;
using System.ComponentModel;
using System.Web.Mvc;

using System.Web.Helpers;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Dapper;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using LumenWorks.Framework.IO.Csv;
using System.Text;
using System.Threading;
using MES_WATER.Helpers;

namespace MES_WATER.Models
{
    
    public class Comm
    {
        /// <summary>
        /// 取得資料庫連線字串
        /// </summary>
        /// <returns>資料庫連線字串</returns>
        public string Get_ConnStr()
        {
            string defaultDB = "con_db";
            string chosenDB = HttpContext.Current.Session["ChosenDB"] != null ? HttpContext.Current.Session["ChosenDB"].ToString() : "";
            string dbName = chosenDB != "" ? chosenDB : defaultDB;

            string connStr = WebConfigurationManager.ConnectionStrings[dbName].ConnectionString;
            
            return connStr;
        }

        public string Get_ConnStr2(string db)
        {
            string defaultDB = db;
            string chosenDB = HttpContext.Current.Session["ChosenDB"] != null ? HttpContext.Current.Session["ChosenDB"].ToString() : "";
            string connStr = WebConfigurationManager.ConnectionStrings[defaultDB].ConnectionString;

            return connStr;
        }

        /// <summary>
        /// 與資料庫做連線
        /// </summary>
        /// <returns>回傳一個SqlConnection的物件</returns>
        public SqlConnection Set_DBConnection()
        {
            SqlConnection Connection_Db;
            Connection_Db = new SqlConnection(Get_ConnStr());
            Connection_Db.Open();
            return Connection_Db;
        }

        public SqlConnection Set_DBConnection2(string db)
        {
            SqlConnection Connection_Db;
            Connection_Db = new SqlConnection(Get_ConnStr2(db));
            Connection_Db.Open();
            return Connection_Db;
        }

        public SqlConnection Get_SqlConn(string conn_name)
        {
            string connStr = WebConfigurationManager.ConnectionStrings[conn_name].ConnectionString;
            SqlConnection sqlConn = new SqlConnection(connStr);
            sqlConn.Open();
            return sqlConn;
        }

        /// <summary>
        /// 傳入一個SQL的資料表，自定義傳入參數，回傳一個DataTable
        /// </summary>
        /// <param name="sTable">SQL 資料表</param>
        ///  <param name="pOption">選擇方式</param>
        /// <param name="pSearchname">搜尋哪個資料表</param>
        ///  <param name="pSearchValue">搜尋哪個值</param>
        /// <returns></returns>
        public string Get_strSQL(string sTable, string pOption = "", string pSearchname = "", string pSearchValue = "")
        {
            string strSql;
            switch (pOption)
            {
                case "s":
                    strSql = "select * from " + sTable + " where " + pSearchname + " = '" + pSearchValue + "'";
                    break;

                default:
                    strSql = "select * from " + sTable + " where " + pSearchname + " = @" + pSearchValue;
                    break;
            }

            return strSql;
        }



        /// <summary>
        /// 傳入一個SQL語法，回傳一個DataTable
        /// </summary>
        /// <param name="pSql">Select語法</param>
        /// <returns></returns>
        public DataTable Get_DataTable(string pSql)
        {
            DataTable datatable = new DataTable();
            try
            {
                if (pSql.Length > 0)
                {
                    using (SqlConnection con_db = Set_DBConnection())
                    {
                        SqlDataAdapter Adapter = new SqlDataAdapter(pSql, con_db);
                        Adapter.Fill(datatable);
                    }
                }
                return datatable;
            }
            catch (Exception)
            {
                //錯誤處理
                throw;
            }
        }

        public DataTable Get_DataTable(string pSql, object pSqlParams)
        {
            DataTable datatable = new DataTable();
            try
            {
                using (SqlConnection con_db = Set_DBConnection())
                {
                    SqlCommand sqlCommand = new SqlCommand();

                    sqlCommand.CommandText = pSql;

                    foreach (var prop in pSqlParams.GetType().GetProperties())
                    {
                        sqlCommand.Parameters.Add(new SqlParameter(prop.Name, prop.GetValue(pSqlParams)));
                    }

                    sqlCommand.Connection = con_db;

                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    datatable.Load(reader);
                }
                return datatable;
            }
            catch (Exception)
            {
                //錯誤處理
                throw;
            }
        }

        public DataTable Get_AlexDataTable(string pSql)
        {
            DataTable datatable = new DataTable();
            try
            {
                if (pSql.Length > 0)
                {
                    using (SqlConnection con_db = Set_DBConnection2("alex_ori"))
                    {
                        SqlDataAdapter Adapter = new SqlDataAdapter(pSql, con_db);
                        Adapter.Fill(datatable);
                    }
                }
                return datatable;
            }
            catch (Exception)
            {
                //錯誤處理
                throw;
            }
        }

        /// <summary>
        /// 傳入一個SQL語法和自訂義的參數值，回傳一個DataTable
        /// </summary>
        /// <param name="pSql">SQL語法</param>
        /// <param name="pParams">SQL參數值</param>
        /// <param name="bSaveSql">是否儲存SQL語法和參數值到BPD20_0000</param>
        /// <param name="pUsrData">自訂義usr_code, prg_code, usr_type，預設儲存的是空字串</param>
        /// <returns></returns>
        public DataTable Get_DataTable(string pSql, Dictionary<string, object> pParams, bool bSaveSql = false, Dictionary<string, string> pUsrData = null)
        {
            DataTable datatable = new DataTable();
            try
            {
                if (pSql.Length > 0)
                {
                    if (bSaveSql)
                    {
                        if (pUsrData == null)
                        {
                            Ins_BDP20_0000("", "", "", pSql, pParams);
                        }
                        else
                        {
                            string sUsrCode = pUsrData.Keys.Contains("usr_code") ? pUsrData["usr_code"] : "";
                            string sPrgCode = pUsrData.Keys.Contains("prg_code") ? pUsrData["prg_code"] : "";
                            string sUsrType = pUsrData.Keys.Contains("usr_type") ? pUsrData["usr_type"] : "";
                            Ins_BDP20_0000(sUsrCode, sPrgCode, "Select", pSql, pParams);
                        }
                    }
                    using (SqlConnection con_db = Set_DBConnection())
                    {
                        SqlCommand sqlCommand = new SqlCommand();
                        sqlCommand.Connection = con_db;
                        // 設置sql參數值
                        List<SqlParameter> list = new List<SqlParameter>();
                        foreach (KeyValuePair<string, object> item in pParams)
                        {
                            SqlParameter sp = new SqlParameter(item.Key, item.Value);
                            sqlCommand.Parameters.Add(sp);
                            //list.Add(sp);
                        }
                        //sqlCommand.Parameters.AddRange(list.ToArray());
                        sqlCommand.CommandText = pSql;
                        SqlDataReader reader = sqlCommand.ExecuteReader();
                        datatable.Load(reader);
                    }
                }
                return datatable;
            }
            catch (Exception)
            {
                //錯誤處理
                throw;
            }
        }

        /// <summary>
        // 取得自訂義的Datatable，要設定connectionStrings (Web.config)
        /// </summary>
        /// <param name="pConnName"></param>
        /// <param name="pSql"></param>
        /// <param name="pSqlParams"></param>
        /// <returns></returns>
        public DataTable Get_DataTable2(string pConnName, string pSql, Dictionary<string, object> pSqlParams = null )
        {
            DataTable datatable = new DataTable();
            try
            {
                if (pSql.Length > 0)
                {
                    using (SqlConnection con_db = Get_SqlConn(pConnName))
                    {
                        SqlCommand sqlCommand = new SqlCommand();
                        sqlCommand.Connection = con_db;
                        // 設置sql的查詢參數
                        if (pSqlParams != null)
                        {
                            foreach (KeyValuePair<string, object> item in pSqlParams)
                            {
                                SqlParameter sqlParam = new SqlParameter(item.Key, item.Value);
                                sqlCommand.Parameters.Add(sqlParam);
                            }
                        }
                   
                        sqlCommand.CommandText = pSql;
                        SqlDataReader reader = sqlCommand.ExecuteReader();
                        datatable.Load(reader);
                    }
                }
                return datatable;
            }
            catch (Exception)
            {
                //錯誤處理
                throw;
            }
        }


        /// <summary>
        /// 傳入一個SQL語法，自定義傳入參數，回傳一個DataTable
        /// </summary>
        /// <param name="pSql">Select語法</param>
        /// <returns></returns>
        public DataTable Get_DataTable(string pSql, string pPara = "", string pParaValue = "")
        {
            DataTable datatable = new DataTable();
            try
            {
                if (pSql.Length > 0)
                {
                    using (SqlConnection con_db = Set_DBConnection())
                    {
                        SqlCommand sqlCommand = new SqlCommand(pSql);
                        sqlCommand.Connection = con_db;
                        for (int i = 0; i < pPara.Split(',').Length; i++)
                        {
                            sqlCommand.Parameters.Add(new SqlParameter("@" + pPara.Split(',')[i], pParaValue.Split(',')[i]));
                        }
                        SqlDataReader reader = sqlCommand.ExecuteReader();
                        datatable.Load(reader);
                    }
                }
                return datatable;
            }
            catch (Exception)
            {
                //錯誤處理
                throw;
            }
        }


        public string RepSql(string pStr)
        {
            if (string.IsNullOrEmpty(pStr)) return "";

            //過濾攻擊字元
            string sStr = Regex.Replace(pStr, @"\b(exec(ute)?|select|update|insert|delete|drop|create)\b|[;]|(-{2})|(/\*.*\*/)", string.Empty, RegexOptions.IgnoreCase);
            return sStr.Trim().Replace("'", "''");
        }

        /// <summary>
        /// 取得單一Table明確1對1鍵值資料共用函數
        /// </summary>
        /// <param name="pTableCode">要查詣的TABLE CODE</param>
        /// <param name="pKeyValue">使用者輸入的鍵值</param>
        /// <param name="pKeyCode">Table中鍵值的欄位名稱</param>
        /// <param name="pFieldValue">要取回的欄位名稱</param>
        /// <returns></returns>
        public string Get_QueryData(string pTableCode, string pKeyValue, string pKeyCode, string pFieldCode)
        {
            //串SQL字串
            string sSql = "select " + pFieldCode + " from " + pTableCode + " where " + pKeyCode + " = @" + pKeyCode;
            using (SqlConnection con_db = Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);              
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter(pKeyCode, pKeyValue));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetString(reader.GetOrdinal(pFieldCode));
                }
            }
            return "";
        }

        /// <summary>
        /// 取得單一Table明確1對1鍵值資料共用函數
        /// </summary>
        /// <param name="pTableCode">要查詢的資料表</param>
        /// <param name="pWhere">自己串where條件子句</param>
        /// <param name="pFieldValue">要取值的欄位</param>
        /// <returns></returns>
        public string Get_QueryData(string pTableCode, string pWhere, string pFieldValue)
        {
            //串SQL字串
            string sSql = "select " + pFieldValue + " from " + pTableCode + "  " + pWhere;

            using (SqlConnection con_db = Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetString(reader.GetOrdinal(pFieldValue));
                }
                return "";
            }
        }

        public T Get_QueryData<T>(string pTableCode, string pWhere, string pFieldValue)
        {
            T t = Activator.CreateInstance<T>();
            //串SQL字串
            string sSql = "select " + pFieldValue + " from " + pTableCode + "  " + pWhere;

            using (SqlConnection con_db = Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    var value = reader.GetValue(reader.GetOrdinal(pFieldValue));
                    return (T)value;

                }
            }
            return t;
        }

        /// <summary>
        /// 取得單一Table明確1對1鍵值資料共用函數
        /// </summary>
        /// <param name="pTableCode">要查詣的TABLE CODE</param>
        /// <param name="pKeyValue">使用者輸入的鍵值</param>
        /// <param name="pKeyCode">Table中鍵值的欄位名稱</param>
        /// <param name="pFieldValue">要取回的欄位名稱</param>
        /// <returns></returns>
        public T Get_QueryData<T>(string pTableCode, string pKeyValue, string pKeyCode, string pFieldValue)
        {
            T t = Activator.CreateInstance<T>();
            string sSql = "select " + pFieldValue + " from " + pTableCode + " where " + pKeyCode + " = @" + pKeyCode;
            using (SqlConnection con_db = Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter(pKeyCode, pKeyValue));
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    var value = reader.GetValue(reader.GetOrdinal(pFieldValue));
                    return (T)value;

                }
            }
            return t;
        }

        /// <summary>
        /// 取得BDP21_0000的選項名稱內容
        /// </summary>
        /// <param name="pCodeCode">選項代碼</param>
        /// <param name="pFieldValue">欄位代碼</param>
        /// <returns></returns>
        public string Get_BDP21_0000(string pCodeCode, string pFieldValue)
        {
            string sSql = "Select field_name From BDP21_0100 Where code_code=@code_code and field_code = @field_code";

            using (SqlConnection con_db = Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("code_code", pCodeCode));
                sqlCommand.Parameters.Add(new SqlParameter("field_code", pFieldValue));
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetString(reader.GetOrdinal("field_name"));
                }
                return "";
            }
        }

        /// <summary>
        /// 有重覆資料則回傳false
        /// </summary>
        /// <param name="pTableCode"></param>
        /// <param name="pWhere"></param>
        /// <returns></returns>
        public Boolean Chk_RelData(string pTableCode, string pWhere)
        {
            //串SQL字串
            string sSql = "select top 1 *  from " + pTableCode + " " + pWhere;
            using (SqlConnection con_db = Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    //有重覆資料
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public Boolean Chk_RelDataBySql(string sSql)
        {
            //串SQL字串
            using (SqlConnection con_db = Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    //有重覆資料
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 有重覆資料則回傳false
        /// </summary>
        /// <param name="pTableCode">table code</param>
        /// <param name="pFieldCode">欄位代碼</param>
        /// <param name="pFieldValue">欄位值</param>
        /// <returns></returns>
        public Boolean Chk_RelData(string pTableCode, string pFieldCode, string pFieldValue)
        {
            //串SQL字串
            string sSql = "select top 1 *  from " + pTableCode + " where " + pFieldCode + "=@" + pFieldCode;
            using (SqlConnection con_db = Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Parameters.Add(new SqlParameter(pFieldCode, pFieldValue));
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    //有重覆資料
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public Boolean Chk_RelData1(string pTableCode, string pFieldCode1, string pFieldCode2, string pFieldValue1, string pFieldValue2)
        {
            //串SQL字串
            string sSql = "select top 1 *  from " + pTableCode + " where " + pFieldCode1 + "=@" + pFieldCode1 + " and " + pFieldCode2 + "=@" + pFieldCode2;
            using (SqlConnection con_db = Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Parameters.Add(new SqlParameter(pFieldCode1, pFieldValue1));
                sqlCommand.Parameters.Add(new SqlParameter(pFieldCode2, pFieldValue2));
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    //有重覆資料
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 檢查資料是否重複，是則回傳false，否則回傳true
        /// </summary>
        /// <param name="pTableCode">要查詢的資料表</param>
        /// <param name="pWhere">自訂義參數化查詢條件字串</param>
        /// <param name="pParaNames">參數名稱，以逗號分隔</param>
        /// <param name="pParaValues">參數值，以逗號分隔</param>
        /// <returns></returns>
        public Boolean Chk_RelData(string pTableCode, string pWhere, string pParaNames, string pParaValues)
        {
            try
            {
                //串SQL字串
                string sSql = "select top 1 *  from " + pTableCode + " " + pWhere;
                using (SqlConnection con_db = Set_DBConnection())
                {
                    SqlCommand sqlCommand = new SqlCommand(sSql);
                    for (int i = 0; i < pParaNames.Split(',').Length; i++)
                    {
                        sqlCommand.Parameters.Add(new SqlParameter("@" + pParaNames.Split(',')[i], pParaValues.Split(',')[i]));
                    }
                    sqlCommand.Connection = con_db;
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        //有重覆資料
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }

            }
            catch (Exception)
            {
                //錯誤處理
                throw;
            }
        }

        /// <summary>
        /// 檢查登入帳號密碼
        /// </summary>
        /// <param name="usr_code">使用者代號</param>
        /// <param name="usr_pass">使用者密碼(未加密)</param>
        /// <returns></returns>
        public Boolean Chk_Login(string usr_code, string usr_pass)
        {
            if (string.IsNullOrEmpty(usr_code))
            {
                return false;
            }
            string sSql = "select * from BDP08_0000 " +
                          " where usr_code=@usr_code " +
                          "   and usr_pass=@usr_pass ";

            using (SqlConnection con_db = Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@usr_code", usr_code));
                sqlCommand.Parameters.Add(new SqlParameter("@usr_pass", usr_pass));

                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 取得使用者權限字串
        /// </summary>
        /// <param name="pUsrCode"></param>
        /// <param name="pPrgCode"></param>
        /// <returns></returns>
        public string Get_LimitByUsrCode(string pUsrCode, string pPrgCode)
        {
            string sLimitStr_Usr = "";
            string sLimitType = Get_QueryData("BDP08_0000", pUsrCode, "usr_code", "limit_type");
            string sLimitStr_Prg = Get_QueryData("BDP04_0000", pPrgCode, "prg_code", "limit_str");
            switch (sLimitType)
            {
                case "B": // 使用者別 BDP09_0000
                    sLimitStr_Usr = Get_LimitStr("BDP09_0000", pUsrCode, "usr_code", pPrgCode, "prg_code");
                    break;
                case "A": // 角色別 BDP09_0100
                    string pGrpCode = Get_QueryData("BDP08_0000", pUsrCode, "usr_code", "grp_code");
                    sLimitStr_Usr = Get_LimitStr("BDP09_0100", pGrpCode, "grp_code", pPrgCode, "prg_code");
                    break;
                default:
                    break;
            }
            string sLimitStr = "";
            foreach (var item in sLimitStr_Usr)
            {
                if (sLimitStr_Prg.Contains(item))
                {
                    sLimitStr += item;
                }
            }
            return sLimitStr;
        }

        /// <summary>
        /// 取得權限字串，傳入Table、使用者和程式相關參數
        /// </summary>
        /// <param name="pTableCode">資料表代號BDP090/BDP091</param>
        /// <param name="pKeyValue">usr_code/grp_code</param>
        /// <param name="pKeyCode">值</param>
        /// <param name="pPrgValue">程式代碼</param>
        /// <returns></returns>
        private string Get_LimitStr(string pTableCode, string pKeyValue, string pKeyCode, string pPrgValue, string pPrgCode)
        {
            string sSql = "select limit_str from " + pTableCode +
                          " where " + pKeyCode + " = @" + pKeyCode + " and " + pPrgCode + " = @" + pPrgCode;
            using (SqlConnection con_db = Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter(pKeyCode, pKeyValue));
                sqlCommand.Parameters.Add(new SqlParameter(pPrgCode, pPrgValue));

                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetString(reader.GetOrdinal("limit_str"));
                }
            }
            return "";
        }

        /// <summary>
        /// 處理Html值到DB的值轉換的集中函數
        /// </summary>
        /// <param name="pHtmlValue">Html控制項取到的值</param>
        /// <param name="pHtmlType">控制項類型名稱</param>
        /// <returns></returns>
        public string Chg_HtmlToDB(object pHtmlValue, string pHtmlType)
        {

            string sReturn = "";

            switch (pHtmlType)
            {
                case "checkbox": //switch類型的checkbox
                    if (pHtmlValue == null)
                    {
                        sReturn = "N";
                    }
                    else
                    {
                        sReturn = "Y";
                    }
                    break;
                case "textbox": //一般輸入框
                    if (pHtmlValue == null)
                    {
                        sReturn = "";
                    }
                    else
                    {
                        sReturn = pHtmlValue.ToString();
                    }
                    break;
                case "multiselect":
                    sReturn = (pHtmlValue == null ? "" : pHtmlValue.ToString().Replace(",", ""));
                    break;
                default:
                    break;
            }
            return sReturn;
        }

        /// <summary>
        /// 將DB讀出來的值，依HTML控項所需要的值做變更
        /// </summary>
        /// <param name="pDBValue">DB讀出來的值</param>
        /// <param name="pHtmlType">控制項類別</param>
        /// <returns></returns>
        public string Chg_DBToHtml(string pDBValue, string pHtmlType)
        {

            string sReturn = "";

            switch (pHtmlType)
            {
                case "checkbox": //switch類型的checkbox
                    if (pDBValue == "Y")
                    {
                        sReturn = "checked= checked";
                    }
                    break;
                default:
                    break;
            }
            return sReturn;
        }

        /// <summary>
        /// 連結DB 使用SQL語法
        /// </summary>
        /// <param name="pSql">SQL敘述</param>
        /// <param name="pPara">用@的參數</param>
        /// <param name="pParaValue">替代參數的值</param>
        public void Connect_DB(string pSql, string pPara = "", string pParaValue = "")
        {
            using (SqlConnection con_db = Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(pSql);

                for (int i = 0; i < pPara.Split(',').Length; i++)
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@" + pPara.Split(',')[i], pParaValue.Split(',')[i]));
                }
                sqlCommand.Connection = con_db;
                sqlCommand.ExecuteNonQuery();
            }
        }

        #region

        /// <summary>
        /// 從資料庫BDP21_0100 取得下拉選項的資料來源
        /// </summary>
        /// <param name="pCode">選項代碼關聯BDP210</param>
        /// <returns></returns>
        public List<DDLList> Get_DDLOption(string pCode)
        {
            List<DDLList> list = new List<DDLList>();
            string sSql = "";
            sSql = "SELECT field_code, field_name, (SELECT show_type FROM BDP21_0000 WHERE code_code=@code_code ) as show_type FROM BDP21_0100 where code_code=@code_code and is_use='Y' order by scr_no";
            using (SqlConnection con_db = Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@code_code", pCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    DDLList data = new DDLList();
                    data.field_code = reader["field_code"].ToString();
                    data.field_name = reader["field_name"].ToString();
                    data.show_type = reader["show_type"].ToString();
                    list.Add(data);
                }

            }
            return list;
        }
        /// <summary>
        /// 下拉選項單一table單一鍵值使用
        /// </summary>
        /// <param name="pTableCode">資料表</param>
        /// <param name="pFieldCode">欄位鍵值</param>
        /// <param name="pFieldName">欄位名稱</param>
        /// <param name="pShowType">A:全秀(預設),B:秀值,C:秀名</param>
        /// <returns></returns>
        public List<DDLList> Get_LabelDDL(string pTableCode, string pFieldCode = "", string pFieldName = "", string pFieldType = "", string pShowType = "A")
        {
            List<DDLList> list = new List<DDLList>();
            string sSql = "";

            sSql = "SELECT " + pFieldCode + " as field_code, " + pFieldName + " as field_name, '" + pShowType + "' as show_type FROM " + pTableCode +
                " where label_type ='" + pFieldType + "' and is_use = 'Y'  order by " + pFieldCode + " ";

            using (SqlConnection con_db = Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();
                sqlCommand.Parameters.Add(new SqlParameter("@code_code", pFieldCode));

                while (reader.Read())
                {
                    DDLList data = new DDLList();
                    data.field_code = reader["field_code"].ToString();
                    data.field_name = reader["field_name"].ToString();
                    data.show_type = reader["show_type"].ToString();
                    list.Add(data);
                }
            }
            return list;
        }
        /// <summary>
        /// 下拉選項單一table單一鍵值使用
        /// </summary>
        /// <param name="pTableCode">資料表</param>
        /// <param name="pFieldCode">欄位鍵值</param>
        /// <param name="pFieldName">欄位名稱</param>
        /// <param name="pShowType">A:全秀(預設),B:秀值,C:秀名</param>
        /// <returns></returns>
        public List<DDLList> Get_DDLOption(string pTableCode, string pFieldCode = "", string pFieldName = "", string pShowType = "A")
        {
            List<DDLList> list = new List<DDLList>();
            string sSql = "";

            sSql = "SELECT " + pFieldCode + " as field_code, " + pFieldName + " as field_name, '" + pShowType + "' as show_type FROM " + pTableCode + " order by " + pFieldCode + " ";

            using (SqlConnection con_db = Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();
                sqlCommand.Parameters.Add(new SqlParameter("@code_code", pFieldCode));

                while (reader.Read())
                {
                    DDLList data = new DDLList();
                    data.field_code = reader["field_code"].ToString();
                    data.field_name = reader["field_name"].ToString();
                    data.show_type = reader["show_type"].ToString();
                    list.Add(data);
                }
            }
            return list;
        }

        /// <summary>
        /// 取得自訂SQL語法的下拉選項的資料來源 (BDP31_0000)
        /// </summary>
        /// <param name="pSql">自定義有field_code,field_name的sql語法</param>
        /// <param name="pShowType">A:全秀(預設),B:秀值,C:秀名</param>
        /// <returns></returns>
        public List<DDLList> Get_DDLOption(string pSelectCode, string pShowType = "A")
        {
            List<DDLList> list = new List<DDLList>();

            string sSql = Get_DDLSql(pSelectCode);

            // 若沒有在BDP31_0000設定Sql語法，則在BDP21_0100中找清單資料
            if (string.IsNullOrEmpty(sSql))
            {
                list = Get_DDLOption(pSelectCode);

                return list;
            }

            using (SqlConnection con_db = Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandText = sSql;
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    DDLList data = new DDLList();
                    data.field_code = reader["field_code"].ToString();
                    data.field_name = reader["field_name"].ToString();
                    data.show_type = pShowType;
                    list.Add(data);
                }
            }
            return list;
        }

        public List<DDLList> Get_DDLOptionBySql(string sSql, string pShowType = "A")
        {
            List<DDLList> list = new List<DDLList>();


            // 若沒有在BDP31_0000設定Sql語法，則在BDP21_0100中找清單資料
            if (string.IsNullOrEmpty(sSql))
            {
                return list;
            }

            try
            {
                using (SqlConnection con_db = Set_DBConnection())
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandText = sSql;
                    sqlCommand.Connection = con_db;
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        DDLList data = new DDLList();
                        data.field_code = reader["field_code"].ToString();
                        data.field_name = reader["field_name"].ToString();
                        data.show_type = pShowType;
                        list.Add(data);
                    }
                }
            } catch (Exception ex)
            {

            }
            
            return list;
        }


        #endregion

        /// <summary>
        /// 取得下拉的Sql語法，在BDP31_0000設定
        /// </summary>
        /// <param name="pSelectCode"></param>
        /// <returns></returns>
        public string Get_DDLSql(string pSelectCode)
        {
            List<BDP31_0000> list = Get_BDP31_0000(pSelectCode);
            if (list.Count <= 0)
            {
                return "";
            }

            BDP31_0000 data = list[0];
            if (data == null)
            {
                return "";
            }

            string sSql = data.tsql_select + " " + data.tsql_where + " " + data.tsql_order;

            return sSql;
        }

        

        /// <summary>
        /// 取得BDP31_0000資料
        /// </summary>
        /// <param name="pSelectCode">下拉選項的代碼</param>
        /// <returns></returns>
        public List<BDP31_0000> Get_BDP31_0000(string pSelectCode)
        {
            List<BDP31_0000> list = new List<BDP31_0000>();
            string sSql = "select * from BDP31_0000";
            if (string.IsNullOrEmpty(pSelectCode))
            {
                sSql = "select * from BDP31_0000";
            }
            else
            {
                sSql = "select * from BDP31_0000 where select_code = @select_code";
            }
            using (SqlConnection con_db = Set_DBConnection())
            {
                list = con_db.Query<BDP31_0000>(sSql, new { select_code = pSelectCode }).ToList();
            }
            return list;
        }


        /// <summary>
        /// 取得報表功能查詢欄位
        /// </summary>
        /// <param name="pPrgCode"></param>
        /// <returns></returns>
        public List<BDP32_0000> Get_BDP32_0000(string pPrgCode)
        {
            List<BDP32_0000> list = new List<BDP32_0000>();
            string sSql = "";
            if (!string.IsNullOrEmpty(pPrgCode))
            {
                sSql = " Select * from BDP32_0000 Where prg_code = @prg_code order by scr_no";
            }
            else
            {
                sSql = " Select * from BDP32_0000 order by scr_no";
            }
            using (SqlConnection con_db = Set_DBConnection())
            {
                list = con_db.Query<BDP32_0000>(sSql, new { prg_code = pPrgCode }).ToList();
            }

            return list;
        }

        /// <summary>
        /// 依使用者取得待辦事項的資料RowData
        /// </summary>
        /// <param name="pUsrCode">使用者編號</param>
        /// <returns></returns>
        public List<BDP16_0000> Get_BDP16_0000(string pUsrCode)
        {
            List<BDP16_0000> datas = new List<BDP16_0000>();
            //待辦標記使用且尚未完成的資料要呈現
            string sSql = "";
            sSql = "select * " +
                   "  from BDP16_0000 " +
                   " where usr_code=@usr_code " +
                   "   and is_use='Y' " +
                   "   and is_ok='N' " +
                   " order by todo_code desc";

            using (SqlConnection con_db = Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@usr_code", pUsrCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BDP16_0000 data = new BDP16_0000
                        {
                            todo_code = reader.GetString(reader.GetOrdinal("todo_code")),
                            todo_name = reader.GetString(reader.GetOrdinal("todo_name")),
                            //todo_type = reader.GetString(reader.GetOrdinal("todo_type")),
                            todo_url = reader.GetString(reader.GetOrdinal("todo_url")) + reader.GetString(reader.GetOrdinal("todo_key")),
                            todo_key = reader.GetString(reader.GetOrdinal("todo_key")),
                            is_use = reader.GetString(reader.GetOrdinal("is_use")),
                            is_ok = reader.GetString(reader.GetOrdinal("is_ok")),
                            usr_code = reader.GetString(reader.GetOrdinal("usr_code")),
                        };
                        datas.Add(data);
                    }
                }
            }
            return datas;
        }

        ///// <summary>
        ///// 建立待辦事項的共用函數
        ///// </summary>
        ///// <param name="pToDoName">待辦主旨</param>
        ///// <param name="pToDoUrl">連結的URL 無連結則空白</param>
        ///// <param name="pToDoKey">連結的Key值</param>
        ///// <param name="pUsrCode">哪個使用者的待辦</param>
        //public void Ins_TodoList(string pToDoName,string pToDoUrl,string pToDoKey,string pUsrCode)
        //{
        //    //儲存todo資料 BDP16_0000
        //    BDP16_0000Repository repoBDP16_0000 = new BDP16_0000Repository();
        //    BDP16_0000 todoData = new BDP16_0000();
        //    todoData.todo_code = Get_TkCode("ToDoList");
        //    todoData.todo_name = pToDoName;
        //    todoData.todo_url = pToDoUrl;
        //    todoData.todo_key = pToDoKey;
        //    todoData.is_use = "Y";
        //    todoData.is_ok = "N";
        //    todoData.usr_code = pUsrCode;
        //    repoBDP16_0000.InsertData(todoData);
        //}



        /// <summary>
        /// 取得單號
        /// </summary>
        /// <param name="pPrgCode">程式編號或自訂代號</param>
        /// <returns></returns>
        public string Get_TkCode(string pPrgCode)
        {
            string sKey = "";  //字軌
            string sCode = ""; //單號
            int sNo = 0;       //序號

            switch (pPrgCode)
            {
                case "ToDoList": //待辦事項
                    sKey = "TD" + DateTime.Now.ToString("yyyyMMdd");
                    sNo = Get_AutoIntMax("BDP16_0000", "RIGHT(todo_code, 4)", "And todo_code LIKE '" + sKey + "%'") + 1;
                    sCode = sKey + StrRigth("0000" + sNo.ToString(), 4);
                    break;
                case "SUB020A": //供應商評鑑
                    sKey = "EV" + DateTime.Now.ToString("yyyyMMdd");
                    sNo = Get_AutoIntMax("SUB02_0000", "RIGHT(eva_code, 4)", "And eva_code LIKE '" + sKey + "%'") + 1;
                    sCode = sKey + StrRigth("0000" + sNo.ToString(), 4);
                    break;
                case "SUT020A": //詢價單
                    sKey = "I" + DateTime.Now.ToString("yyyyMMdd");
                    sNo = Get_AutoIntMax("MFT02_0000", "RIGHT(inq_code, 3)", "And inq_code LIKE '" + sKey + "%'") + 1;
                    sCode = sKey + StrRigth("000" + sNo.ToString(), 3);
                    break;
                case "WMT060A": //備料單
                    sKey = "PR" + DateTime.Now.ToString("yyyyMMdd");
                    sNo = Get_AutoIntMax("WMT06_0000", "RIGHT(prepare_code, 2)", "And prepare_code LIKE '" + sKey + "%'") + 1;
                    sCode = sKey + StrRigth("00" + sNo.ToString(), 2);
                    break;
                case "WMT070A": //上料檢查
                    sKey = "UP" + DateTime.Now.ToString("yyyyMMdd");
                    sNo = Get_AutoIntMax("WMT07_0000", "RIGHT(chkup_code, 2)", "And chkup_code LIKE '" + sKey + "%'") + 1;
                    sCode = sKey + StrRigth("00" + sNo.ToString(), 2);
                    break;
                case "MET040A": 
                    sKey = "UP" + DateTime.Now.ToString("yyyyMMdd");
                    sNo = Get_AutoIntMax("MET04_0000", "RIGHT(ureport_code, 4)", "And ureport_code LIKE '" + sKey + "%'") + 1;
                    sCode = sKey + StrRigth("0000" + sNo.ToString(), 4);
                    break;
                case "MET040B":
                    sKey = "UP" + DateTime.Now.ToString("yyyyMMdd");
                    sNo = Get_AutoIntMax("MET04_0100", "RIGHT(ureport_code, 4)", "And ureport_code LIKE '" + sKey + "%'") + 1;
                    sCode = sKey + StrRigth("0000" + sNo.ToString(), 4);
                    break;
                case "MET040C":
                    sKey = "UP" + DateTime.Now.ToString("yyyyMMdd");
                    sNo = Get_AutoIntMax("MET04_0200", "RIGHT(ureport_code, 6)", "And ureport_code LIKE '" + sKey + "%'") + 1;
                    sCode = sKey + StrRigth("000000" + sNo.ToString(), 6);
                    break;
                case "MET040D":
                    sKey = "UP" + DateTime.Now.ToString("yyyyMMdd");
                    sNo = Get_AutoIntMax("MET04_0300", "RIGHT(ureport_code, 6)", "And ureport_code LIKE '" + sKey + "%'") + 1;
                    sCode = sKey + StrRigth("000000" + sNo.ToString(), 6);
                    break;
                case "MET040E":
                    sKey = "UP" + DateTime.Now.ToString("yyyyMMdd");
                    sNo = Get_AutoIntMax("MET04_0400", "RIGHT(ureport_code, 6)", "And ureport_code LIKE '" + sKey + "%'") + 1;
                    sCode = sKey + StrRigth("000000" + sNo.ToString(), 6);
                    break;
                case "MET040F":
                    sKey = "UP" + DateTime.Now.ToString("yyyyMMdd");
                    sNo = Get_AutoIntMax("MET04_0500", "RIGHT(ureport_code, 6)", "And ureport_code LIKE '" + sKey + "%'") + 1;
                    sCode = sKey + StrRigth("000000" + sNo.ToString(), 6);
                    break;
                case "MET050A":
                    sKey = "UP" + DateTime.Now.ToString("yyyyMMdd");
                    sNo = Get_AutoIntMax("MET04_0600", "RIGHT(ureport_code, 6)", "And ureport_code LIKE '" + sKey + "%'") + 1;
                    sCode = sKey + StrRigth("000000" + sNo.ToString(), 6);
                    break;
                case "QMT020A":
                    sKey = "QM" + DateTime.Now.ToString("yyyyMMdd");
                    sNo = Get_AutoIntMax("QMT02_0000", "RIGHT(qmt_code, 4)", "And qmt_code LIKE '" + sKey + "%'") + 1;
                    sCode = sKey + StrRigth("0000" + sNo.ToString(), 4);
                    break;
                case "QMT030A":
                    sKey = "QM" + DateTime.Now.ToString("yyyyMMdd");
                    sNo = Get_AutoIntMax("QMT03_0000", "RIGHT(qmt_code, 4)", "And qmt_code LIKE '" + sKey + "%'") + 1;
                    sCode = sKey + StrRigth("0000" + sNo.ToString(), 4);
                    break;
                //case "QMB03_0100.scr_no":
                //    //sKey = "QM" + DateTime.Now.ToString("yyyyMMdd");
                //    sNo = Get_AutoIntMax("QMT02_0000", "RIGHT(scr_no, 4)", "And scr_no LIKE '" + 0 + "%'") + 1;
                //    sCode = StrRigth("0000" + sNo.ToString() + "0", 4);
                //    break;
                default:
                    break;
                case "QMT050A":
                    sKey = "IPQ" + DateTime.Now.ToString("yyyyMMdd");
                    sNo = Get_AutoIntMax("QMT05_0000", "RIGHT(ipqm_code, 4)", "And ipqm_code LIKE '" + sKey + "%'") + 1;
                    sCode = sKey + StrRigth("0000" + sNo.ToString(), 4);
                    break;
                case "QMT060A":
                    sKey = "OQ" + DateTime.Now.ToString("yyyyMMdd");
                    sNo = Get_AutoIntMax("QMT06_0000", "RIGHT(oqm_code, 4)", "And oqm_code LIKE '" + sKey + "%'") + 1;
                    sCode = sKey + StrRigth("0000" + sNo.ToString(), 4);
                    break;
                case "QMT040A":
                    sKey = "QM" + DateTime.Now.ToString("yyyyMMdd");
                    sNo = Get_AutoIntMax("QMT04_0000", "RIGHT(qmt_code, 4)", "And qmt_code LIKE '" + sKey + "%'") + 1;
                    sCode = sKey + StrRigth("0000" + sNo.ToString(), 4);
                    break;
                case "EMT020A":
                    sKey = "DC" + DateTime.Now.ToString("yyyyMMdd");
                    sNo = Get_AutoIntMax("EMT02_0000", "RIGHT(dev_check_code, 3)", "And dev_check_code LIKE '" + sKey + "%'") + 1;
                    sCode = sKey + StrRigth("000" + sNo.ToString(), 3);
                    break;
                case "EMT050A":
                    sKey = "CA" + DateTime.Now.ToString("yyyyMMdd");
                    sNo = Get_AutoIntMax("EMT05_0000", "RIGHT(call_code, 4)", "And call_code LIKE '" + sKey + "%'") + 1;
                    sCode = sKey + StrRigth("0000" + sNo.ToString(), 4);
                    break;
                case "WMT080A":
                    sKey = "IN" + DateTime.Now.ToString("yyyyMMdd");
                    sNo = Get_AutoIntMax("WMT08_0000", "RIGHT(inventory_code, 2)", "And inventory_code LIKE '" + sKey + "%'") + 1;
                    sCode = sKey + StrRigth("00" + sNo.ToString(), 2);
                    break;
                case "WMT090B":
                    sKey = "IN" + DateTime.Now.ToString("yyyyMMdd");
                    sNo = Get_AutoIntMax("WMT09_0000", "RIGHT(erp_inventory_code, 2)", "And erp_inventory_code LIKE '" + sKey + "%'") + 1;
                    sCode = sKey + StrRigth("00" + sNo.ToString(), 2);
                    break;
            }

            return sCode;
        }



        /// <summary>
        /// 取得單號
        /// </summary>
        /// <param name="pKeyId">代號</param>
        /// <param name="pTable">資料表</param>
        /// <param name="pTableCode">資料表的鍵值</param>
        /// <param name="pCodeCnt">補0的位數</param>
        /// <returns></returns>
        public string Get_TkCode(string pKeyId,string pTable,string pTableCode,int pCodeCnt)
        {
            string sKey = "";  //字軌
            string sCode = ""; //單號
            int sNo = 0;       //序號
            string sDigit = string.Empty.PadRight(pCodeCnt,'0');

            sKey = pKeyId + DateTime.Now.ToString("yyyyMMdd");
            sNo = Get_AutoIntMax(pTable, "RIGHT(" + pTableCode + ", " + pCodeCnt.ToString() + ")", "And " + pTableCode + " LIKE '" + sKey + "%'") + 1;
            sCode = sKey + StrRigth(sDigit + sNo.ToString(), pCodeCnt);

            return sCode;
        }



        /// <summary>
        /// 依傳入的Table及鍵值取得單號最大值
        /// </summary>
        /// <param name="pTableName">要查詢的table</param>
        /// <param name="pFieldName">要查詢的欄位</param>
        /// <param name="pSubWhere">要查詢的條件</param>
        /// <returns></returns>
        public int Get_AutoIntMax(string pTableName, string pFieldName, string pSubWhere)
        {
            int sRetun = 0;
            string sSql = "";
            sSql = "SELECT MAX(" + pFieldName + ") AS Max_no FROM " + pTableName + " WHERE 1=1 " + pSubWhere + " GROUP BY " + pFieldName + " ORDER BY " + pFieldName + " DESC";
            using (SqlConnection con_db = Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.Read())
                {
                    //if (!string.IsNullOrEmpty(reader["Max_no"].ToString()))
                    //{
                    sRetun = Int32.Parse(reader["Max_no"].ToString());
                    //}
                }
            }
            return sRetun;
        }

        /// <summary>
        /// 由連字元分隔的32位數字
        /// </summary>
        /// <returns></returns>
        public string Get_Guid()
        {
            return Guid.NewGuid().ToString().Replace("-","");
        }

        /// <summary>
        /// 取得BDP03_0000資料(功能選單資料)
        /// </summary>
        /// <param name="pPKCode">使用者代號，取得該使用者可以使用的選單功能</param>
        /// <param name="pLevel">選單層級1-4</param>
        /// <returns></returns>
        public List<BDP03_0000> Get_BDP03_0000(string pPKCode, string pLevel)
        {

            List<BDP03_0000> datas = new List<BDP03_0000>();
            string sSql = "";
            sSql = "select * " +
                           "  from BDP03_0000 " +
                           " where menu_level=@menu_level " +
                           "   and is_use='Y' " +
                           " order by menu_code ";
            using (SqlConnection con_db = Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@menu_level", pLevel));
                sqlCommand.Parameters.Add(new SqlParameter("@usr_code", pPKCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BDP03_0000 data = new BDP03_0000
                        {
                            menu_code = sGetString(reader.GetString(reader.GetOrdinal("menu_code"))),
                            menu_name = sGetString(reader.GetString(reader.GetOrdinal("menu_name"))),
                            prg_code = sGetString(reader.GetString(reader.GetOrdinal("prg_code"))),
                            menu_type = sGetString(reader.GetString(reader.GetOrdinal("menu_type"))),
                            menu_level = sGetString(reader.GetString(reader.GetOrdinal("menu_level"))),
                            //menu_grp = reader.GetString(reader.GetOrdinal("menu_grp")),
                            //scr_no = reader.GetInt32(reader.GetOrdinal("scr_no")),
                            is_use = sGetString(reader.GetString(reader.GetOrdinal("is_use"))),
                            menu_src = sGetString(reader.GetString(reader.GetOrdinal("menu_src"))),
                            //menu_icon = reader.GetString(reader.GetOrdinal("menu_icon")),
                        };
                        string sIsUse = "";
                        string sLimitType = Get_QueryData("BDP08_0000", pPKCode, "usr_code", "limit_type");
                        switch (sLimitType)
                        {
                            case "B": // BDP09_0000 使用者別
                                if (string.IsNullOrEmpty(data.prg_code))
                                {
                                    sIsUse = "Y";
                                }
                                else
                                {
                                    string sSql_A = " Where usr_code='" + pPKCode + "' and prg_code='" + data.prg_code + "'";
                                    sIsUse = Get_QueryData("BDP09_0000", sSql_A, "is_use");
                                }
                                break;
                            case "A": // BDP09_0100 角色別
                                if (string.IsNullOrEmpty(data.prg_code))
                                {
                                    sIsUse = "Y";
                                }
                                else
                                {
                                    string sGrpCode = Get_QueryData("BDP08_0000", pPKCode, "usr_code", "grp_code");
                                    string sSql_B = " Where grp_code='" + sGrpCode + "' and prg_code='" + data.prg_code + "'";
                                    sIsUse = Get_QueryData("BDP09_0100", sSql_B, "is_use");
                                }
                                break;
                            default:
                                sIsUse = "N";
                                break;
                        }
                        if (sIsUse == "Y")
                        {
                            datas.Add(data);
                        }
                    } // end while
                } // end hasRow
            }
            return datas;
        }


        /*字串處理向下*/
        public string StrLeft(string s, int length)
        {
            return s.Substring(0, length);
        }
        public string StrRigth(string s, int length)
        {
            return s.Substring(s.Length - length);
        }
        public string StrMid(string s, int start, int length)
        {
            return s.Substring(start, length);
        }


        public void ConvertNull(string pTableName)
        {
            string sDefault = "";
            string sSql = "SELECT * FROM INFORMATION_SCHEMA.Columns Where Table_Name = '" + pTableName + "'";
            DataTable dtTmp = Get_DataTable(sSql);

            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                if (dtTmp.Rows[i]["IS_NULLABLE"].ToString() == "YES")
                {
                    switch (dtTmp.Rows[i]["DATA_TYPE"].ToString())
                    {
                        case "nvarchar":
                            sDefault = "";
                            break;
                        case "decimal":
                            sDefault = "0";
                            break;
                        case "int":
                            sDefault = "0";
                            break;
                        default:
                            sDefault = "";
                            break;
                    }

                    sSql = "update " + pTableName +
                           "  set " + dtTmp.Rows[i]["COLUMN_NAME"].ToString() + " = '" + sDefault + "' " +
                           " where isnull(" + dtTmp.Rows[i]["COLUMN_NAME"].ToString() + ",'" + sDefault + "') = '" + sDefault + "'";
                    Connect_DB(sSql);
                }
            }
        }

        //轉型使用函數
        public Decimal sGetDecimal(string pValue)
        {
            bool success = false;
            Decimal result = 0;
            success = Decimal.TryParse(pValue, out result);

            return result;
        }

        public Int32 sGetInt32(string pValue)
        {
            bool success = false;
            Int32 result = 0;
            success = Int32.TryParse(pValue, out result);

            return result;
        }

        public Int64 sGetInt64(string pValue)
        {
            bool success = false;
            Int64 result = 0;
            success = Int64.TryParse(pValue, out result);

            return result;
        }

        public String sGetString(string pValue)
        {
            string result = pValue ?? "";
            result = result.Trim();
            return result;
        }

        public Double sGetDouble(string pValue)
        {
            bool success = false;
            Double result = 0;
            success = Double.TryParse(pValue, out result);

            return result;
        }

        public float sGetfloat(string pValue)
        {
            bool success = false;
            float result = 0;
            success = float.TryParse(pValue, out result);

            return result;
        }



        public DateTime sGetDateTime(string pValue)
        {
            bool success = false;
            DateTime result = DateTime.Now;
            success = DateTime.TryParse(pValue, out result);

            return result;
        }


        public bool Chk_DateForm(string pStr)
        {
            if (string.IsNullOrEmpty(pStr)) return false;
            //正則表示檢查
            string pattern = @"^(19|20)\d\d\/(0[1-9]|1[012])\/(0[1-9]|[12][0-9]|3[01])$";
            Regex reg = new Regex(pattern);
            Match match = reg.Match(pStr);
            if (!match.Success)
            {
                return false;
            }

            //日期轉換檢查
            DateTime test;
            if (!DateTime.TryParse(pStr, out test))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 利用SQL語法取得指定欄位的字串形式
        /// </summary>
        /// <param name="pFieldCode">指定欄位</param>
        /// <returns></returns>
        public string DataFieldToStr(string pSqlStr, string pFieldCode)
        {
            var dtTmp = Get_DataTable(pSqlStr);
            string sValue = "";
            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                if (i > 0) { sValue += ","; };               
                sValue += dtTmp.Rows[i][pFieldCode].ToString();
            }
            return sValue;
        }

        public string DataFieldToStr(DataTable dtTmp, string pFieldCode)
        {
            string sValue = "";
            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                if (i > 0) { sValue += ","; };
                sValue += dtTmp.Rows[i][pFieldCode].ToString();
            }
            return sValue;
        }

        public string Get_Data(string pTableCode, string pKeyValue, string pKeyCode, string pFieldValue)
        {
            //串SQL字串
            string sSql = "select " + pFieldValue + " from " + pTableCode + " where " + pKeyCode + " = @" + pKeyCode + "";
            DataTable dtTmp = Get_DataTable(sSql, pKeyCode, pKeyValue);
            return DataFieldToStr(dtTmp, pFieldValue);
        }

        /// <summary>
        /// 更新單一欄位
        /// </summary>
        /// <param name="pTable">資料表</param>
        /// <param name="pKeyField">鍵值欄位</param>
        /// <param name="pKeyValue">鍵值</param>
        /// <param name="pUpdField">更新欄位</param>
        /// <param name="pUpdValue">更新值</param>
        public void Upd_Data(string pTable, string pKeyField, string pKeyValue, string pUpdField, string pUpdValue)
        {
            string sSql = "update " + pTable + "" +
                          "  set " + pUpdField + " = '" + pUpdValue + "'" +
                          " where " + pKeyField + " = '" + pKeyValue + "'";
            Connect_DB(sSql);
        }


        /// <summary>
        /// 更新單一欄位，1對1鍵值
        /// </summary>
        /// <param name="pTableCode">資料表</param>
        /// <param name="pKeyCode">鍵值欄位</param>
        /// <param name="pKeyValue">鍵值</param>
        /// <param name="pFieldCode">更新欄位</param>
        /// <param name="pFieldValue">更新值</param>
        public void Upd_QueryData(string pTableCode, string pKeyCode, string pKeyValue, string pFieldCode, string pFieldValue)
        {
            string sSql = " UPDATE " + pTableCode +
                          "    SET " + pFieldCode + " = @" + pFieldCode +
                          "  WHERE " + pKeyCode + "   = @" + pKeyCode; 
            using (SqlConnection con_db = Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter(pKeyCode, pKeyValue));
                sqlCommand.Parameters.Add(new SqlParameter(pFieldCode, pFieldValue));
                sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 執行單一刪除語法
        /// </summary>
        /// <param name="pTableCode">執行刪除的目標Table</param>
        /// <param name="pKeyCode">鍵值欄位</param>
        /// <param name="pKeyValue">鍵值</param>
        public void Del_QueryData(string pTableCode, string pKeyCode, string pKeyValue)
        {
            string sSql = " DELETE " + pTableCode + 
                          "  WHERE " + pKeyCode + "   = @" + pKeyCode;
            using (SqlConnection con_db = Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter(pKeyCode, pKeyValue));
                sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 給傳進來的model設值
        /// </summary>
        /// <typeparam name="T">model類別</typeparam>
        /// <param name="obj">要設值的model</param>
        /// <param name="form">從html傳來的form資料</param>
        public void Set_ModelValue<T>(T obj, FormCollection form)
        {
            foreach (var property in obj.GetType().GetProperties())
            {
                string sFormValue = form[property.Name];
                switch (property.PropertyType.Name.ToLower())
                {
                    case "string":
                        obj.GetType().GetProperty(property.Name).SetValue(obj, sGetString(sFormValue));
                        break;
                    case "int":
                        obj.GetType().GetProperty(property.Name).SetValue(obj, sGetInt32(sFormValue));
                        break;
                    case "int32":
                        obj.GetType().GetProperty(property.Name).SetValue(obj, sGetInt32(sFormValue));
                        break;
                    case "int64":
                        obj.GetType().GetProperty(property.Name).SetValue(obj, sGetInt64(sFormValue));
                        break;
                    case "double":
                        obj.GetType().GetProperty(property.Name).SetValue(obj, sGetDouble(sFormValue));
                        break;
                    case "decimal":
                        obj.GetType().GetProperty(property.Name).SetValue(obj, sGetDecimal(sFormValue));
                        break;
                    case "datetime":
                        obj.GetType().GetProperty(property.Name).SetValue(obj, sGetDateTime(sFormValue));
                        break;
                    case "float":
                        obj.GetType().GetProperty(property.Name).SetValue(obj, sGetfloat(sFormValue));
                        break;
                    default:
                        break;
                };
            }
        }

        /// <summary>
        /// 取得現在日期字串，格式: yyyy/MM/dd
        /// </summary>
        /// <returns></returns>
        public string Get_Date()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 取得現在時間字串，格式: HH:mm:ss
        /// </summary>
        /// <returns></returns>
        public string Get_Time()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }

        /// <summary>
        /// 取得 欄位寬度資料清單
        /// </summary>
        /// <returns></returns>
        public List<BDP30_0000> Get_BDP30_0000(string pUsrCode, string pPrgCode, string pViewCode)
        {
            List<BDP30_0000> list = new List<BDP30_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = "SELECT * FROM BDP30_0000" +
                   "    where usr_code = @usr_code" +
                   "        and prg_code = @prg_code" +
                   "        and view_code = @view_code";

            using (SqlConnection con_db = Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@usr_code", pUsrCode));
                sqlCommand.Parameters.Add(new SqlParameter("@prg_code", pPrgCode));
                sqlCommand.Parameters.Add(new SqlParameter("@view_code", pViewCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP30_0000 data = new BDP30_0000();

                    data.bdp30_0000 = reader.GetInt32(reader.GetOrdinal("bdp30_0000"));
                    data.prg_code = reader.GetString(reader.GetOrdinal("prg_code"));
                    data.usr_code = reader.GetString(reader.GetOrdinal("usr_code"));
                    data.view_code = reader.GetString(reader.GetOrdinal("view_code"));
                    data.col_index = reader.GetInt32(reader.GetOrdinal("col_index"));
                    data.col_width = reader.GetInt32(reader.GetOrdinal("col_width"));


                    list.Add(data);
                }
            }
            if (list.Count > 0)
            {
                BDP30_0000 tmp = new BDP30_0000();
                for (int i = 0; i < 10; i++)
                {
                    list.Add(tmp);
                }
            }
            return list;
        }

        /// <summary>
        /// 取得 欄位顯示資料清單
        /// </summary>
        /// <param name="pUsrCode"></param>
        /// <param name="pPrgCode"></param>
        /// <param name="pViewCode"></param>
        /// <returns></returns>
        public List<BDP30_0100> Get_BDP30_0100(string pUsrCode, string pPrgCode, string pViewCode)
        {
            List<BDP30_0100> list = new List<BDP30_0100>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = "SELECT * FROM BDP30_0100" +
                   "    where usr_code = @usr_code" +
                   "        and prg_code = @prg_code" +
                   "        and view_code = @view_code";

            using (SqlConnection con_db = Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@usr_code", pUsrCode));
                sqlCommand.Parameters.Add(new SqlParameter("@prg_code", pPrgCode));
                sqlCommand.Parameters.Add(new SqlParameter("@view_code", pViewCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP30_0100 data = new BDP30_0100();

                    data.bdp30_0100 = reader.GetInt32(reader.GetOrdinal("bdp30_0100"));
                    data.prg_code = reader.GetString(reader.GetOrdinal("prg_code"));
                    data.usr_code = reader.GetString(reader.GetOrdinal("usr_code"));
                    data.view_code = reader.GetString(reader.GetOrdinal("view_code"));
                    data.col_index = reader.GetInt32(reader.GetOrdinal("col_index"));
                    data.is_show = reader.GetString(reader.GetOrdinal("is_show"));


                    list.Add(data);
                }
                if (list.Count > 0)
                {
                    BDP30_0100 tmp = new BDP30_0100();
                    for (int i = 0; i < 10; i++)
                    {
                        list.Add(tmp);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 取得查詢的下拉選單
        /// </summary>
        /// <param name="pPrgCode">程式代號( controllerName )</param>
        /// <param name="pShowType">A:全秀(預設),B:秀值,C:秀名</param>
        /// <returns></returns>
        public List<DDLList> Get_BDP30_0200(string pPrgCode, string pShowType = "A")
        {
            List<DDLList> list = new List<DDLList>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = "Select * from BDP30_0200" +
                   "    where prg_code = @prg_code " +
                   "    order by scr_no";

            using (SqlConnection con_db = Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@prg_code", pPrgCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    DDLList data = new DDLList();

                    data.field_code = reader.GetString(reader.GetOrdinal("table_code")) + "." + reader.GetString(reader.GetOrdinal("field_code"));
                    data.field_name = reader.GetString(reader.GetOrdinal("field_name"));
                    data.show_type = pShowType;
                    list.Add(data);
                }
            }
            return list;
        }

        /// <summary>
        /// 根據model取得下拉選單
        /// </summary>
        /// <typeparam name="T">model類別，對應資料庫table</typeparam>
        /// <param name="pShowType">A.值+名 / B.值 / C.名稱</param>
        /// <returns></returns>
        public List<DDLList> Get_ModelDDL<T>(string pShowType)
        {
            List<DDLList> result = new List<DDLList>();
            string tableName = typeof(T).Name;
            var list = typeof(T).GetProperties().Where(x => !new string[] { "can_delete", "can_update", "sor_type" }.Contains(x.Name) && !x.IsDefined(typeof(NotMappedAttribute))).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                DDLList data = new DDLList();
                // field_code
                data.field_code = tableName + "." + list[i].Name;

                // field_name
                object[] attributes = list[i].GetCustomAttributes(typeof(DisplayNameAttribute), false);
                if (attributes != null && attributes.Length > 0)
                {
                    var displayName = (DisplayNameAttribute)attributes[0];
                    data.field_name = displayName.DisplayName;
                }
                else
                {
                    data.field_name = "";
                }

                // show_type
                data.show_type = pShowType;

                result.Add(data);
            }
            return result;
        }

        /// <summary>
        /// 儲存訊息到BDP20_0000資料表
        /// </summary>
        /// <param name="pUsrCode">使用者代號</param>
        /// <param name="pPrgCode">程式代號</param>
        /// <param name="pUsrType">使用類型</param>
        /// <param name="pCMEMO">使用備註</param>
        public void Ins_BDP20_0000(string pUsrCode, string pPrgCode, string pUsrType, string pCMEMO, object pSqlParams = null, bool bCheckIsSave = true)
        {
            if (bCheckIsSave)
            {
                string is_save = Get_QueryData("BDP00_0000", "save_data_log", "par_name", "par_value");
                if (is_save != "Y")
                {
                    return;
                }
            }
            
            BDP20_0000 data = new BDP20_0000()
            {
                usr_code = pUsrCode,
                prg_code = pPrgCode,
                usr_date = Get_Date(),
                usr_time = Get_Time(),
                usr_type = pUsrType,
                cmemo = pCMEMO,
                params_json = pSqlParams == null ? "" : JsonConvert.SerializeObject(pSqlParams)
            };

            string sSql = "INSERT INTO " +
                          " BDP20_0000 ( usr_code,  prg_code," +
                          "              usr_date,  usr_time,  cmemo, usr_type, params_json) " +
                          "     VALUES (@usr_code, @prg_code," +
                          "             @usr_date, @usr_time, @cmemo, @usr_type, @params_json)";

            using (SqlConnection con_db = Set_DBConnection())
            {
                con_db.Execute(sSql, data);
            }
        }

        public void Ins_BDP20_0000ForMdy(string pUsrCode, string pPrgCode, string pMdyType, object pBefore, object pAfter)
        {
            string is_save = Get_QueryData("BDP00_0000", "save_data_log", "par_name", "par_value");
            if (is_save != "Y")
            {
                return;
            }

            string sCMemo = Get_DataForBDP20_0000(pMdyType, pBefore, pAfter);

            Ins_BDP20_0000(pUsrCode, pPrgCode, "Mdy", sCMemo);
        }

        public void Ins_BDP20_0100(string pUsrCode, string pPrgCode, string pFunCode, string pCMEMO, string pException)
        {
            string is_save = Get_QueryData("BDP00_0000", "save_error_log", "par_name", "par_value");
            if (is_save != "Y")
            {
                return;
            }

            BDP20_0100 data = new BDP20_0100()
            {
                usr_code = pUsrCode,
                prg_code = pPrgCode,
                usr_date = Get_Date(),
                usr_time = Get_Time(),
                exception = pException,
                fun_code = pFunCode,
                cmemo = pCMEMO
            };

            string sSql = "INSERT INTO " +
                          " BDP20_0100 ( usr_code,  prg_code," +
                          "              usr_date,  usr_time,  cmemo, exception , fun_code) " +
                          "     VALUES (@usr_code, @prg_code," +
                          "             @usr_date, @usr_time, @cmemo ,@exception ,@fun_code)";

            using (SqlConnection con_db = Set_DBConnection())
            {
                con_db.Execute(sSql, data);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMdyType"></param>
        /// <param name="pBefore"></param>
        /// <param name="pAfter"></param>
        /// <returns></returns>
        public string Get_DataForBDP20_0000(string pMdyType, object pBefore, object pAfter)
        {
            if (string.IsNullOrEmpty(pMdyType))
            {
                return "";
            }

            GetModelValidation gmv = new GetModelValidation();

            string type = "";
            string label = "";
            switch (pMdyType.ToLower())
            {
                case "insert":
                    type = "insert";
                    label = "新增資料";
                    break;
                case "update":
                    type = "update";
                    label = "更改資料";
                    break;
                case "delete":
                    type = "delete";
                    label = "刪除資料";
                    break;
                default:
                    break;
            }

            var recordData = new
            {
                type = type,
                label = label,
                before = new
                {
                    label = "變更前",
                    data = pBefore
                },
                after = new
                {
                    label = "變更後",
                    data = pAfter
                },
            };

            return JsonConvert.SerializeObject(recordData);
        }


        //public void Ins_BDP20_0000ForMdy(string pUsrCode, string pPrgCode, string pUsrType, object pDTO, string pTableCode = "", string pKeyCode = "")
        //{
        //    GetModelValidation gmv = new GetModelValidation();

        //    Type type = pDTO.GetType();

        //    string key = "";
        //    string keyValue = "";

        //    if (!(pDTO is string && string.IsNullOrEmpty(pDTO.ToString())))
        //    {
        //        // 取得在model設定的key
        //        key = gmv.GetKey(type);

        //        // 取得key value
        //        keyValue = type.GetProperty(key).GetValue(pDTO).ToString();
        //    }

        //    // DTO對應的table code, 預設和model名稱一樣
        //    if (string.IsNullOrEmpty(pTableCode))
        //    {
        //        pTableCode = type.Name;
        //    }

        //    // 要寫在cmemo的資料
        //    var recordData = new object();

        //    switch (pUsrType.ToLower())
        //    {
        //        case "insert":
        //            recordData = new
        //            {
        //                type = "insert",
        //                label = "新增資料",
        //                data = pDTO
        //            };
        //            break;
        //        case "update":
        //            var origData = GetData(key, keyValue, pTableCode);
        //            recordData = new
        //            {
        //                type = "update",
        //                label = "修改資料",
        //                updateBefore = new
        //                {
        //                    label = "變更前",
        //                    data = origData
        //                },
        //                updateAfter = new
        //                {
        //                    label = "變更後",
        //                    data = pDTO
        //                },
        //            };
        //            break;
        //        case "delete":
        //            var deletedData = GetData(key, keyValue, pTableCode);
        //            recordData = new
        //            {
        //                type = "delete",
        //                label = "刪除資料",
        //                data = deletedData
        //            };
        //            break;
        //        default:
        //            break;
        //    }

        //    BDP20_0000 data = new BDP20_0000()
        //    {
        //        usr_code = pUsrCode,
        //        prg_code = pPrgCode,
        //        usr_date = Get_Date(),
        //        usr_time = Get_Time(),
        //        usr_type = "Mdy",
        //        cmemo = JsonConvert.SerializeObject(recordData)
        //    };
        //    string sSql = "INSERT INTO " +
        //                  " BDP20_0000 ( usr_code,  prg_code," +
        //                  "              usr_date,  usr_time,  cmemo, usr_type) " +
        //                  "     VALUES (@usr_code, @prg_code," +
        //                  "             @usr_date, @usr_time, @cmemo, @usr_type)";

        //    using (SqlConnection con_db = Set_DBConnection())
        //    {
        //        con_db.Execute(sSql, data);
        //    }
        //}

        public string Get_DataForBDP20_0000(object pDTO)
        {
            GetModelValidation gmv = new GetModelValidation();

            Type type = pDTO.GetType();

            string key = "";
            string keyValue = "";
            string tableCode = "";

            if (!(pDTO is string && string.IsNullOrEmpty(pDTO.ToString())))
            {
                // 取得在model設定的key
                key = gmv.GetKey(type);

                // 取得key value
                keyValue = type.GetProperty(key).GetValue(pDTO).ToString();
            }

            tableCode = type.Name;

            var o = GetData(key, keyValue, tableCode);

            string result = JsonConvert.SerializeObject(o);

            return result;
        }

        // jqGrid 查詢用的函數 向下

        /// <summary>
        /// 傳入預設sql字串，取得查詢的List，並儲存sql字串到BDP20_0000
        /// </summary>
        /// <param name="pSql">預設的Select Sql字串，不含where</param>
        /// <param name="pWhere">查詢資料字串，JSON格式</param>
        /// <param name="pUsrCode">使用者代碼</param>
        /// <param name="pPrgCode">功能代碼</param>
        /// <returns></returns>
        public List<T> Get_ListByQuery<T>(string pSql, string pWhere, string pUsrCode, string pPrgCode, string pOrder = "")
        {
            List<T> list = new List<T>();
            BDP20_0000Repository repoBDP20_0000 = new BDP20_0000Repository();
            string result_sql = "";

            // 預設，查詢字串為null或空時
            if (string.IsNullOrEmpty(pWhere))
            {
                // 判斷傳進來的sql有沒有包含where
                Regex rgx2 = new Regex(@"where", RegexOptions.IgnoreCase);
                if (rgx2.IsMatch(pSql))
                {
                    pSql += " and 1=0 ";
                }
                else
                {
                    pSql += " Where 1=0 ";

                }

                result_sql = pSql;

                // 儲存sql字串到資料表BDP20_0000
                Ins_BDP20_0000(pUsrCode, pPrgCode, "Select", result_sql);
                using (SqlConnection con_db = Set_DBConnection())
                {
                    list = con_db.Query<T>(result_sql).ToList();
                }
                return list;
            }
            else
            {
                // 判斷傳進來的sql有沒有包含where
                Regex rgx = new Regex(@"where", RegexOptions.IgnoreCase);
                if (!rgx.IsMatch(pSql))
                {
                    pSql += " Where 1=1 ";
                }

                List<JqGridQueryData> query_datas = JsonConvert.DeserializeObject<List<JqGridQueryData>>(pWhere);
                DynamicParameters sqlParams = new DynamicParameters();
                // 巡覽查詢資料
                for (int i = 0; i < query_datas.Count; i++)
                {
                    var field_datas = query_datas[i].query_conditions;
                    string paraName = "";
                    string defaultSql = pSql;
                    string col_op = "and";
                    // 設置查詢的sql語法和參數
                    for (int j = 0; j < field_datas.Count; j++)
                    {
                        paraName = "@" + field_datas[j].field_code.Replace(".", "") + "_" + i.ToString() + j.ToString();
                        switch (field_datas[j].field_operator)
                        {
                            case "cn":  // 包含
                                defaultSql += " " + col_op + " " + field_datas[j].field_code + " like " + paraName;
                                sqlParams.Add(paraName, "%" + field_datas[j].field_value + "%");
                                break;
                            case "eq":  // 等於
                                defaultSql += " " + col_op + " " + field_datas[j].field_code + " = " + paraName;
                                sqlParams.Add(paraName, field_datas[j].field_value);
                                break;
                            case "gt":  // 大於
                                defaultSql += " " + col_op + " " + field_datas[j].field_code + " > " + paraName;
                                sqlParams.Add(paraName, field_datas[j].field_value);
                                break;
                            case "lt":  // 小於
                                defaultSql += " " + col_op + " " + field_datas[j].field_code + " < " + paraName;
                                sqlParams.Add(paraName, field_datas[j].field_value);
                                break;
                            case "bw":  // 開頭是
                                defaultSql += " " + col_op + " " + field_datas[j].field_code + " like " + paraName;
                                sqlParams.Add(paraName, field_datas[j].field_value + "%");
                                break;
                            case "ew":  // 結尾是
                                defaultSql += " " + col_op + " " + field_datas[j].field_code + " like " + paraName;
                                sqlParams.Add(paraName, "%" + field_datas[j].field_value);
                                break;
                            case "n_cn":    // 不包含
                                defaultSql += " " + col_op + " " + field_datas[j].field_code + " not like " + paraName;
                                sqlParams.Add(paraName, "%" + field_datas[j].field_value + "%");
                                break;
                            case "n_eq":    // 不等於
                                defaultSql += " " + col_op + " " + field_datas[j].field_code + " != " + paraName;
                                sqlParams.Add(paraName, field_datas[j].field_value);
                                break;
                            case "n_bw":    // 開頭不是
                                defaultSql += " " + col_op + " " + field_datas[j].field_code + " not like " + paraName;
                                sqlParams.Add(paraName, field_datas[j].field_value + "%");
                                break;
                            case "n_ew":    // 結尾不是
                                defaultSql += " " + col_op + " " + field_datas[j].field_code + " not like " + paraName;
                                sqlParams.Add(paraName, "%" + field_datas[j].field_value);
                                break;
                            default:
                                break;
                        }
                    }
                    // 查詢類別
                    switch (query_datas[i].query_type.ToUpper())
                    {
                        // 新查詢
                        case "NEW":
                            result_sql = defaultSql;
                            break;
                        // 保留已查詢
                        case "KEEP":
                            result_sql = "( " + result_sql + " )";
                            result_sql += " UNION ";
                            result_sql += "(" + defaultSql + ")";
                            break;
                        // 在已查詢中尋找
                        case "IN":
                            result_sql = "( " + result_sql + " )";
                            result_sql += " Intersect ";
                            result_sql += "(" + defaultSql + ")";
                            break;
                        default:
                            result_sql = defaultSql;
                            break;
                    }
                }

                //在最後進行order by 的字串串接
                if (!string.IsNullOrEmpty(pOrder))
                {
                    result_sql += " order by " + pOrder;
                }

                // 巡覽查詢資料 結束

                // 儲存sql字串到資料表BDP20_0000
                // 處理要儲存的sql參數
                if (sqlParams.ParameterNames.Count() > 0)
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    foreach (string name in sqlParams.ParameterNames)
                    {
                        dic.Add(name, sqlParams.Get<dynamic>(name));
                    }

                    Ins_BDP20_0000(pUsrCode, pPrgCode, "Select", result_sql, dic);
                }
                else
                {
                    Ins_BDP20_0000(pUsrCode, pPrgCode, "Select", result_sql, "");
                }

                // 取得資料
                using (SqlConnection con_db = Set_DBConnection())
                {
                    list = con_db.Query<T>(result_sql, sqlParams).ToList();
                }

                if (list == null)
                {
                    list = new List<T>();
                }

                return list;

            }
        }

        /// <summary>
        /// 傳入預設sql和查詢字串，取得帶sql語法的SqlCommand，並儲存sql字串到BDP20_0000
        /// </summary>
        /// <param name="pSql">預設sql字串</param>
        /// <param name="pWhere">查詢字串</param>
        /// <param name="pUsrCode">使用者代碼</param>
        /// <param name="pPrgCode">功能代碼</param>
        /// <returns></returns>
        public SqlCommand Get_SqlCommandByQuery(string pSql, string pWhere, string pUsrCode, string pPrgCode)
        {
            SqlCommand sqlCommand = new SqlCommand();
            string result_sql = "";

            if (string.IsNullOrEmpty(pWhere))
            {
                pSql += "Where 1=0 ";
                result_sql = pSql;
            }
            else
            {
                pSql += " Where 1=1 ";

                List<JqGridQueryData> query_datas = JsonConvert.DeserializeObject<List<JqGridQueryData>>(pWhere);

                for (int i = 0; i < query_datas.Count; i++)
                {
                    var field_data_list = query_datas[i].query_conditions;
                    string paraName = "";
                    string querySql = "";
                    // 設置sql語法和sqlCommand參數
                    for (int j = 0; j < field_data_list.Count; j++)
                    {
                        paraName = "@" + field_data_list[j].field_code.Replace(".", "") + "_" + i.ToString() + j.ToString();
                        querySql = Set_SqlAndParam("and", sqlCommand, pSql, field_data_list[j].field_operator, field_data_list[j].field_code, paraName, field_data_list[j].field_value);
                    }
                    // 查詢類別
                    switch (query_datas[i].query_type.ToUpper())
                    {
                        // 新查詢
                        case "NEW":
                            result_sql = querySql;
                            break;
                        // 保留已查詢
                        case "KEEP":
                            result_sql = "( " + result_sql + " )";
                            result_sql += " UNION ";
                            result_sql += "(" + querySql + ")";
                            break;
                        // 在已查詢中尋找
                        case "IN":
                            result_sql = "( " + result_sql + " )";
                            result_sql += " Intersect ";
                            result_sql += "(" + querySql + ")";
                            break;
                        default:
                            break;
                    }
                }
            }

            // 儲存sql字串到BDP20_0000
            BDP20_0000Repository repoBDP20_0000 = new BDP20_0000Repository();
            BDP20_0000 BDP20_0000data = new BDP20_0000()
            {
                usr_code = pUsrCode,
                prg_code = pPrgCode,
                usr_date = Get_Date(),
                usr_time = Get_Time(),
                cmemo = result_sql
            };
            repoBDP20_0000.InsertData(BDP20_0000data);

            sqlCommand.CommandText = result_sql;

            return sqlCommand;
        }

        /// <summary>
        /// 串接Sql語法字串，給予SqlCommand參數值，回傳Sql語法字串
        /// </summary>
        /// <param name="col_op">欄位間的邏輯運算子: 輸入 "and" 或 "or"</param>
        /// <param name="sqlCommand">要傳入參數的SqlCommand個體</param>
        /// <param name="pSql">串接的sql語法</param>
        /// <param name="field_op">帶入的欄位中的邏輯運算子</param>
        /// <param name="field_code">帶入的欄位名稱</param>
        /// <param name="paraName">給sqlCommand的參數名稱，"@"開頭</param>
        /// <param name="field_value">帶入的欄位值</param>
        /// <returns></returns>
        private string Set_SqlAndParam(string col_op, SqlCommand sqlCommand, string pSql, string field_op, string field_code, string paraName, string field_value)
        {
            switch (field_op)
            {
                case "cn":  // 包含
                    pSql += " " + col_op + " " + field_code + " like " + paraName;
                    sqlCommand.Parameters.Add(new SqlParameter(paraName, "%" + field_value + "%"));
                    break;
                case "eq":  // 等於
                    pSql += " " + col_op + " " + field_code + " = " + paraName;
                    sqlCommand.Parameters.Add(new SqlParameter(paraName, field_value));
                    break;
                case "gt":  // 大於
                    pSql += " " + col_op + " " + field_code + " > " + paraName;
                    sqlCommand.Parameters.Add(new SqlParameter(paraName, field_value));
                    break;
                case "lt":  // 小於
                    pSql += " " + col_op + " " + field_code + " < " + paraName;
                    sqlCommand.Parameters.Add(new SqlParameter(paraName, field_value));
                    break;
                case "bw":  // 開頭是
                    pSql += " " + col_op + " " + field_code + " like " + paraName;
                    sqlCommand.Parameters.Add(new SqlParameter(paraName, field_value + "%"));
                    break;
                case "ew":  // 結尾是
                    pSql += " " + col_op + " " + field_code + " like " + paraName;
                    sqlCommand.Parameters.Add(new SqlParameter(paraName, "%" + field_value));
                    break;
                case "n_cn":    // 不包含
                    pSql += " " + col_op + " " + field_code + " not like " + paraName;
                    sqlCommand.Parameters.Add(new SqlParameter(paraName, "%" + field_value + "%"));
                    break;
                case "n_eq":    // 不等於
                    pSql += " " + col_op + " " + field_code + " != " + paraName;
                    sqlCommand.Parameters.Add(new SqlParameter(paraName, field_value));
                    break;
                case "n_bw":    // 開頭不是
                    pSql += " " + col_op + " " + field_code + " not like " + paraName;
                    sqlCommand.Parameters.Add(new SqlParameter(paraName, field_value + "%"));
                    break;
                case "n_ew":    // 結尾不是
                    pSql += " " + col_op + " " + field_code + " not like " + paraName;
                    sqlCommand.Parameters.Add(new SqlParameter(paraName, "%" + field_value));
                    break;
                default:
                    break;
            }
            return pSql;
        }
        // 查詢用的函數 向上  //


        /// <summary>
        /// 檢查身分證格式
        /// </summary>
        /// <param name="per_idno"></param>
        /// <returns></returns>
        public CheckDataResult Chk_IdNo(string id)
        {
            id = id ?? "";
            CheckDataResult data = new CheckDataResult();
            // 使用「正規表達式」檢驗格式 [A~Z] {1}個數字 [0~9] {9}個數字
            var regex = new Regex("^[A-Z]{1}[0-9]{9}$");
            if (!regex.IsMatch(id))
            {
                //Regular Expression 驗證失敗，回傳 ID 錯誤
                data.bIsOK = false;
                data.message = "身分證基本格式錯誤";
                return data;

            }

            //除了檢查碼外每個數字的存放空間 
            int[] seed = new int[10];

            //建立字母陣列(A~Z)
            //A=10 B=11 C=12 D=13 E=14 F=15 G=16 H=17 J=18 K=19 L=20 M=21 N=22
            //P=23 Q=24 R=25 S=26 T=27 U=28 V=29 X=30 Y=31 W=32  Z=33 I=34 O=35            
            string[] charMapping = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "X", "Y", "W", "Z", "I", "O" };
            string target = id.Substring(0, 1); //取第一個英文數字
            for (int index = 0; index < charMapping.Length; index++)
            {
                if (charMapping[index] == target)
                {
                    index += 10;
                    //10進制的高位元放入存放空間   (權重*1)
                    seed[0] = index / 10;

                    //10進制的低位元*9後放入存放空間 (權重*9)
                    seed[1] = (index % 10) * 9;

                    break;
                }
            }
            for (int index = 2; index < 10; index++) //(權重*8~1)
            {   //將剩餘數字乘上權數後放入存放空間                
                seed[index] = Convert.ToInt32(id.Substring(index - 1, 1)) * (10 - index);
            }
            //檢查是否符合檢查規則，10減存放空間所有數字和除以10的餘數的個位數字是否等於檢查碼            
            //(10 - ((seed[0] + .... + seed[9]) % 10)) % 10 == 身分證字號的最後一碼   
            if ((10 - (seed.Sum() % 10)) % 10 != Convert.ToInt32(id.Substring(9, 1)))
            {
                data.bIsOK = false;
                data.message = "請輸入正確身分證";
                return data;
            }

            data.bIsOK = true;
            data.message = "身分證號碼正確";
            return data;
        }


        public List<string> Get_DBColumnNames(string pTableCode)
        {
            List<string> list = new List<string>();

            string sSql = "SELECT * FROM INFORMATION_SCHEMA.Columns Where Table_Name = @Table_Name ";

            DataTable dtTmp = Get_DataTable(sSql, "Table_Name", pTableCode);
            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                list.Add(dtTmp.Rows[i]["COLUMN_NAME"].ToString());
            }

            return list;
        }


        /// <summary>
        /// 參數化查詢的insert into語法字串，沒有帶參數值
        /// </summary>
        /// <param name="pTableCode">要動作的table code</param>
        /// <returns></returns>
        public string Get_InsSql(string pTableCode)
        {
            List<string> ColumnNames = Get_DBColumnNames(pTableCode);
            string sField = "";
            string sFieldValue = "";
            for (int i = 0; i < ColumnNames.Count; i++)
            {
                if (i > 0)
                {
                    sField += ",";
                    sFieldValue += ",";
                }
                sField += ColumnNames[i];
                sFieldValue += "@" + ColumnNames[i];
            }
            string result = "insert into " + pTableCode + " (" + sField + ")" +
                               "  values (" + sFieldValue + ")";

            return result;
        }

        /// <summary>
        /// 新增資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pTableCode"></param>
        /// <param name="form"></param>
        public void InsertData(string pTableCode, object obj)
        {
            using (SqlConnection con_db = Set_DBConnection())
            {
                con_db.Execute(Get_InsSql(pTableCode), obj);
            }
        }

        /// <summary>
        /// 根據Model名稱和設定KeyAttribute，從DB取得單一資料
        /// </summary>
        /// <typeparam name="T">資料model</typeparam>
        /// <param name="pDTO">DTO資料，從中取得鍵值</param>
        /// <param name="pKeyCode">鑑值欄位，預設為model自訂義的key</param>
        /// <param name="pTableCode">資料表，預設為model的名稱</param>
        /// <returns></returns>
        public T GetData<T>(object pDTO, string pKeyCode = "", string pTableCode = "")
        {
            T t = Activator.CreateInstance<T>();
            try
            {
                GetModelValidation gmv = new GetModelValidation();
                // 預設table code是model T的名稱
                if (string.IsNullOrEmpty(pTableCode))
                {
                    pTableCode = typeof(T).Name;
                }

                // 預設 key code 在 model設定
                if (string.IsNullOrEmpty(pKeyCode))
                {
                    pKeyCode = gmv.GetKey<T>();
                }

                string pKeyValue = typeof(T).GetProperty(pKeyCode).GetValue(pDTO).ToString();

                string sSql = "select * from " + pTableCode + " where " + pKeyCode + " = @" + pKeyCode;

                DynamicParameters sqlParams = new DynamicParameters();

                sqlParams.Add("@" + pKeyCode, pKeyValue);

                using (SqlConnection con_db = Set_DBConnection())
                {
                    t = con_db.QueryFirstOrDefault<T>(sSql, sqlParams);
                }

                return t;
            }
            catch (Exception ex)
            {
                return t;
            }

        }

        /// <summary>
        /// 根據Model名稱和設定KeyAttribute，從DB取得單一資料
        /// </summary>
        /// <typeparam name="T">資料model</typeparam>
        /// <param name="pKeyValue">鍵值</param>
        /// <param name="pKeyCode">鍵值欄位，預設為model自訂義的key</param>
        /// <param name="pTableCode">資料表，預設為model的名稱</param>
        /// <returns></returns>
        public T GetData<T>(string pKeyValue, string pKeyCode = "", string pTableCode = "")
        {
            GetModelValidation gmv = new GetModelValidation();
            // 預設table code是model T的名稱
            if (string.IsNullOrEmpty(pTableCode))
            {
                pTableCode = typeof(T).Name;
            }

            // 預設 key code 在 model設定
            if (string.IsNullOrEmpty(pKeyCode))
            {
                pKeyCode = gmv.GetKey<T>();
            }

            T t = Activator.CreateInstance<T>();
            string sSql = "select * from " + pTableCode + " where " + pKeyCode + " = @" + pKeyCode;

            DynamicParameters sqlParams = new DynamicParameters();

            sqlParams.Add("@" + pKeyCode, pKeyValue);

            using (SqlConnection con_db = Set_DBConnection())
            {
                t = con_db.QueryFirstOrDefault<T>(sSql, sqlParams);
            }

            return t;
        }

        /// <summary>
        /// 從DB取得單一資料
        /// </summary>
        /// <param name="pKeyCode">鑑值</param>
        /// <param name="pKeyValue">鑑值欄位</param>
        /// <param name="pTableCode">資料表</param>
        /// <returns></returns>
        public object GetData(string pKeyCode, string pKeyValue, string pTableCode)
        {
            var o = new object();

            if (string.IsNullOrEmpty(pTableCode))
                return o;

            string sSql = "select * from " + pTableCode + " where " + pKeyCode + " = @" + pKeyCode;
            DynamicParameters sqlParams = new DynamicParameters();

            sqlParams.Add("@" + pKeyCode, pKeyValue);

            using (SqlConnection con_db = Set_DBConnection())
            {
                o = con_db.QueryFirstOrDefault(sSql, sqlParams);
            }

            return o;
        }

        /// <summary>
        /// 解析查詢資料，目前是給報表用，其他不知道能不能用
        /// </summary>
        /// <param name="pWhere">一般是從前端傳來的查詢資料，JSON字串</param>
        /// <returns></returns>
        public JqGridQueryData parseQuery(string pWhere)
        {
            JqGridQueryData query_data = new JqGridQueryData();
            if (!string.IsNullOrEmpty(pWhere))
            {
                // pWhere格式參考: [ { query_type: 'New', query_conditions: [ { field_code:'', field_operator: '', field_value: '' }  ] }  ]
                // 整個pWhere是一個array，每個成員都是一個object查詢資料，其中查調條件在query_conditions: array
                List<JqGridQueryData> query_datas = JsonConvert.DeserializeObject<List<JqGridQueryData>>(pWhere);
                if (query_datas.Count > 0)
                {
                    query_data = query_datas[0];
                }
            }
            return query_data;
        }


        //// 

        public void Ins_DTS01_0000(string pFunctionCode, string pUsrCode, object pData)
        {
            MET01_0000Repository repoMET01_0000 = new MET01_0000Repository();
            MET01_0100Repository repoMET01_0100 = new MET01_0100Repository();
            DTS01_0000Repository repoDTS01_0000 = new DTS01_0000Repository();
            DTS01_0000 DTS01_0000 = new DTS01_0000();
            DTS01_0000.dts01_0000 = Get_Guid();
            DTS01_0000.con_code = "RFC";
            DTS01_0000.con_type = "B";
            DTS01_0000.con_function = pFunctionCode;
            DTS01_0000.result = "";
            DTS01_0000.message = "";
            DTS01_0000.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
            DTS01_0000.ins_time = DateTime.Now.ToString("HH:mm:ss");
            DTS01_0000.usr_code = pUsrCode;
            DTS01_0000.sch_date = "";
            DTS01_0000.sch_time = "";
            DTS01_0000.sch_usr_code = "";
            DTS01_0000.data_flag = "N";

            MET04_0100 data;
            MET04_0200 data2;
            WMT0200 WMT0200data;
            List<MET04_0300> list;
            DateTime date;
            Comm comm = new Comm();
            List<MET01_0100> MET01_0100;
            string date1 = "";
            string date2 = "";
            string date3 = "";
            string con_request = "";
            string sJson = "";
            string ins_date = "";
            string Nowdate = "";
            string sto_date = "";
            switch (pFunctionCode)
            {
                case "ZMES9993":
                    data = (MET04_0100)Convert.ChangeType(pData, typeof(MET04_0100));
                    con_request = @"[{""AUFNR"":""" + data.mo_code +
                                  @""",""MBLNR"":""" + data.sap_code +
                                  @""",""MJAHR"":""" + data.sap_no +
                                  @""",""table"":""MET04_0100"",""ureport_code"":""" + data.ureport_code + @"""}]";
                    DTS01_0000.con_request = con_request;
                    break;
                case "ZMES9993-1":
                    DTS01_0000.con_function = "ZMES9993";
                    DataTable dtData = (DataTable)Convert.ChangeType(pData, typeof(DataTable));
                    foreach (DataRow row in dtData.Rows)
                    {
                        if (sJson != "") { sJson += ","; }
                        sJson += @"{""AUFNR"":""" + row["mo_code"].ToString() +
                                  @""",""MBLNR"":""" + row["sap_code"].ToString() +
                                  @""",""MJAHR"":""" + row["sap_no"].ToString() +
                                  @""",""table"":""MET04_0300"",""ureport_code"":""" + row["mo_code"].ToString() + @"""}";
                    }

                    con_request = @"[" + sJson + "]";
                    DTS01_0000.con_request = con_request;
                    //data3 = (MET04_0300)Convert.ChangeType(pData, typeof(MET04_0300));
                    //con_request = @"[{""AUFNR"":""" + data3.mo_code +
                    //              @""",""MBLNR"":""" + data3.sap_code +
                    //              @""",""MJAHR"":""" + data3.sap_no +
                    //              @""",""table"":""MET04_0300"",""ureport_code"":""" + data3.ureport_code + @"""}]";
                    //DTS01_0000.con_request = con_request;
                    break;

                case "ZMES9994"://工單報工修改同步ERP
                    data2 = (MET04_0200)Convert.ChangeType(pData, typeof(MET04_0200));
                    con_request = @"[{""AUFNR"":""" + data2.mo_code +
                                  @""",""RUECK"":""" + data2.sap_code +
                                  @""",""RMZHL"":""" + data2.sap_no +
                                  @""",""table"":""MET04_0200"",""ureport_code"":""" + data2.ureport_code + @"""}]";
                    DTS01_0000.con_request = con_request;
                    break;

                //case "ZMES9997"://生產工單領料同步ERP
                //    list = (List<MET04_0300>)Convert.ChangeType(pData, typeof(List<MET04_0300>));
                //    foreach (MET04_0300 data3 in list)
                //    {
                //        MET01_0100 = repoMET01_0100.Get_DataList(data3.mo_code);
                //        if (sJson != "") { sJson += ","; }
                //        sJson += @"{""BLDAT"":""" + data3.ureport_date +
                //                 @""",""BUDAT"":""" + data3.ureport_date +
                //                 @""",""BWART"":""261"",""WERKS"":""QY01"",""MATNR"":""" + data3.pro_code +
                //                 @""",""ERFMG"":""" + data3.pro_qty.ToString() +
                //                 @""",""ERFME"":""" + data3.pro_unit +
                //                 @""",""LGORT"":""" + MET01_0100[0].LGORT +
                //                 @""",""CHARG"":""" + data3.lot_no +
                //                 @""",""AUFNR"":""" + data3.mo_code +
                //                 @""",""table"":""MET04_0300"",""ureport_code"":""" + data3.ureport_code + @"""}";
                //    }

                //    con_request = @"[" + sJson + "]";
                //    DTS01_0000.con_request = con_request;

                //    //date = DateTime.Parse(list[0].ureport_date);
                //    //date1 = date.ToString("yyyyMMdd");
                //    //MET01_0100 = repoMET01_0100.Get_DataList(data3.mo_code);
                //    //con_request = @"[{""BLDAT"":""" + data3.ureport_date +
                //    //              @""",""BUDAT"":""" + data3.ureport_date +
                //    //              @""",""BWART"":""261"",""WERKS"":""QY01"",""MATNR"":""" + data3.pro_code +
                //    //              @""",""ERFMG"":""" + data3.pro_qty.ToString() +
                //    //              @""",""ERFME"":""" + data3.pro_unit +
                //    //              @""",""LGORT"":""" + MET01_0100[0].LGORT +
                //    //              @""",""CHARG"":""" + data3.lot_no +
                //    //              @""",""AUFNR"":""" + data3.mo_code +
                //    //              @""",""table"":""MET04_0300"",""ureport_code"":""" + data3.ureport_code + @"""}]";
                //    //DTS01_0000.con_request = con_request;
                //    break;

                case "ZMES9998"://工單入庫同步ERP
                    data = (MET04_0100)Convert.ChangeType(pData, typeof(MET04_0100));
                    date = DateTime.Parse(data.ureport_date);
                    date1 = date.ToString("yyyyMMdd");
                    //MET01_0100 = repoMET01_0100.Get_DataList(data.mo_code);
                    string ELIKZ = "";
                    if (data.is_ok == "Y") ELIKZ = "X";
                    con_request = @"[{""BLDAT"":""" + data.ureport_date +
                                  @""",""BUDAT"":""" + data.ureport_date +
                                  @""",""BWART"":""101"",""AUFNR"":""" + data.mo_code +
                                  @""",""WERKS"":""QY01"",""MATNR"":""" + data.pro_code +
                                  @""",""ERFMG"":""" + data.pro_qty +
                                  @""",""ERFME"":""" + data.pro_unit +
                                  @""",""LGORT"":""" + data.loc_code +
                                  @""",""CHARG"":""" + data.lot_no +
                                  @""",""HSDAT"":""" + date1 +
                                  @""",""ELIKZ"":""" + ELIKZ +
                                  @""",""table"":""MET04_0100"",""ureport_code"":""" + data.ureport_code + @"""}]";
                    DTS01_0000.con_request = con_request;
                    break;

                case "ZMES9999"://工單報工同步ERP
                    data2 = (MET04_0200)Convert.ChangeType(pData, typeof(MET04_0200));
                    date = DateTime.Parse(data2.ureport_date);
                    date1 = date.ToString("yyyyMMdd");
                    date = DateTime.Parse(data2.pro_date_s);
                    date2 = date.ToString("yyyyMMdd");
                    date = DateTime.Parse(data2.pro_date_e);
                    date3 = date.ToString("yyyyMMdd");
                    string AUERU = "";
                    if (data2.is_ok == "Y") AUERU = "X";
                    if (data2.is_ok == "1") AUERU = "1"; //新增1為自動確認
                    con_request = @"[{""AUFNR"":""00" + data2.mo_code +
                                  @""",""VORNR"":""" + data2.up_type +
                                  @""",""APLFL"":""0"",""AUERU"":""" + AUERU +
                                  @""",""NUM"":""" + data2.sap_scr_no.ToString() +
                                  @""",""LMNGA"":""" + data2.pro_qty.ToString() +
                                  @""",""ERFME"":""" + data2.pro_unit +
                                  @""",""GRUND"":""" + data2.stop_code +
                                  @""",""BUDAT"":""" + data2.ureport_date +
                                  @""",""ISDD"":""" + date2 +
                                  @""",""IEDD"":""" + date3 +
                                  @""",""ISM01"":""" + data2.ISM01.ToString() +
                                  @""",""ISM02"":""" + data2.ISM02.ToString() +
                                  @""",""ISM03"":""" + data2.ISM03.ToString() +
                                  @""",""ISM04"":""" + data2.ISM04.ToString() +
                                  @""",""ISM05"":""" + data2.ISM05.ToString() +
                                  @""",""ISM06"":""" + data2.ISM06.ToString() +
                                  @""",""table"":""MET04_0200"",""ureport_code"":""" + data2.ureport_code + @"""}]";
                    DTS01_0000.con_request = con_request;
                    break;
                case "ZMES9978"://盤點單更新
                    DTS01_0000.con_function = "ZMES9978";
                    sJson += @"{""I_WERKS"":""" + "QY01" + @"""}";

                    con_request = @"[" + sJson + "]";

                    DTS01_0000.con_request = con_request;
                    break;
                case "ZMES9980"://預留單
                    DTS01_0000.con_function = "ZMES9980";
                    WMT0200data = (WMT0200)Convert.ChangeType(pData, typeof(WMT0200));
                    date = DateTime.Parse(WMT0200data.ins_date);
                    ins_date = date.ToString("yyyy/MM/dd");
                    Nowdate = DateTime.Now.ToString("yyyy/MM/dd");
                    if (sJson != "") { sJson += ","; }
                    sJson += @"{""RSNUM"":""" + WMT0200data.rel_code +
                              @""",""RSPOS"":""" + WMT0200data.scr_no +
                              @""",""MATNR"":""" + WMT0200data.pro_code +
                              @""",""UMWRK"":""" + "" +
                              @""",""UMLGO"":""" + "" +
                              @""",""WERKS"":""" + "QY01" +
                              @""",""LGORT"":""" + "P000" +
                              @""",""CHARG"":""" + WMT0200data.lot_no +
                              @""",""BDMNG"":""" + WMT0200data.pro_qty +
                              @""",""MEINS"":""" + "" +//基礎單位
                              @""",""BDTER"":""" + ins_date +
                              @""",""BWART"":""" + WMT0200data.rel_type +//異動類別
                              @""",""SGTXT"":""" + "" +
                              @""",""KOSTL"":""" + "" +
                              @""",""BUDAT"":""" + Nowdate +
                              @""",""MBLNR"":""" + "" +
                              @""",""MJAHR"":""" + "" +
                              @""",""MESSAGE"":""" + "" +
                              @""",""TYPE"":""" + "" +
                               @""",""wmt0200"":""" + WMT0200data.wmt0200 +
                              @""",""table"":""WMT0200"",""ureport_code"":""" + WMT0200data.rel_code + @"""}";

                    con_request = @"[" + sJson + "]";
                    DTS01_0000.con_request = con_request;
                    Upd_WMT0200_SapCode("等待SAP回傳訊息", WMT0200data.wmt0200);
                    break;
                case "ZMES9977"://盤點單
                    DTS01_0000.con_function = "ZMES9977";
                    WMT0200data = (WMT0200)Convert.ChangeType(pData, typeof(WMT0200));
                    date = DateTime.Parse(WMT0200data.ins_date);
                    ins_date = date.ToString("yyyyMMdd");
                    string year = date.ToString("yyyy");
                    if (sJson != "") { sJson += ","; }
                    sJson += @"{""I_WERKS"":""" + WMT0200data.sto_code +
                              @""",""I_GJAHR"":""" + year +
                              @""",""I_IBLNR"":""" + WMT0200data.scr_no +
                              @""",""I_IBLNR"":""" + WMT0200data.rel_code +
                              @""",""I_ZLDAT"":""" + ins_date +
                              @""",""I_LGORT"":""" + WMT0200data.loc_code +
                               @""",""wmt0200"":""" + WMT0200data.wmt0200 +
                              @""",""ureport_code"":""" + WMT0200data.rel_code + @"""}";

                    con_request = @"[" + sJson + "]";

                    DTS01_0000.con_request = con_request;
                    break;
                case "ZMES9970"://採購單

                    DTS01_0000.con_function = "ZMES9970";
                    WMT0200data = (WMT0200)Convert.ChangeType(pData, typeof(WMT0200));
                    date = DateTime.Parse(WMT0200data.sto_date);
                    sto_date = date.ToString("yyyy/MM/dd");
                    date = DateTime.Parse(WMT0200data.ins_date);
                    ins_date = date.ToString("yyyy/MM/dd");
                    Nowdate = DateTime.Now.ToString("yyyy/MM/dd");
                    if (sJson != "") { sJson += ","; }


                    sJson += @"{""BLDAT"":""" + sto_date +
                              @""",""BUDAT"":""" + Nowdate +
                              @""",""EBELN"":""" + WMT0200data.rel_code +
                              @""",""EBELP"":""" + WMT0200data.scr_no + //WMT0200data.scr_no +//採購文件項目號碼
                              @""",""BWART"":""" + "101" +//異動類型
                              @""",""WERKS"":""" + "QY01" +
                              @""",""MATNR"":""" + WMT0200data.pro_code +
                              @""",""INSMK"":""" + "" +//庫存類型
                              @""",""LGORT"":""" + "P000" +
                              @""",""CHARG"":""" + WMT0200data.lot_no +
                              @""",""HSDAT"":""" + WMT0200data.mft_date +
                              @""",""ERFMG"":""" + WMT0200data.pro_qty +
                              @""",""ERFME"":""" + "" +
                              @""",""wmt0200"":""" + WMT0200data.wmt0200 +
                              @""",""table"":""WMT0200"",""ureport_code"":""" + WMT0200data.rel_code + @"""}";
                    con_request = @"[" + sJson + "]";
                    DTS01_0000.con_request = con_request;
                    Upd_WMT0200_SapCode("等待SAP回傳訊息", WMT0200data.wmt0200);
                    break;
                case "ZMES9969"://採購退貨
                    DTS01_0000.con_function = "ZMES9969";
                    WMT0200data = (WMT0200)Convert.ChangeType(pData, typeof(WMT0200));
                    date = DateTime.Parse(WMT0200data.sto_date);
                    sto_date = date.ToString("yyyy/MM/dd");
                    date = DateTime.Parse(WMT0200data.ins_date);
                    ins_date = date.ToString("yyyy/MM/dd");
                    Nowdate = DateTime.Now.ToString("yyyy/MM/dd");
                    if (sJson != "") { sJson += ","; }

                    sJson += @"{""BLDAT"":""" + sto_date +
                              @""",""BUDAT"":""" + Nowdate +
                              @""",""EBELN"":""" + WMT0200data.rel_code +
                              @""",""EBELP"":""" + WMT0200data.scr_no + //WMT0200data.scr_no +//採購文件項目號碼
                              @""",""BWART"":""" + "106" +//異動類型
                              @""",""WERKS"":""" + "QY01" +
                              @""",""MATNR"":""" + WMT0200data.pro_code +
                              @""",""INSMK"":""" + "" +//庫存類型
                              @""",""LGORT"":""" + "P000" +
                              @""",""CHARG"":""" + WMT0200data.lot_no +
                              @""",""HSDAT"":""" + WMT0200data.mft_date +
                              @""",""ERFMG"":""" + WMT0200data.pro_qty +
                              @""",""ERFME"":""" + "" +
                              @""",""wmt0200"":""" + WMT0200data.wmt0200 +
                              @""",""table"":""WMT0200"",""ureport_code"":""" + WMT0200data.rel_code + @"""}";
                    con_request = @"[" + sJson + "]";
                    DTS01_0000.con_request = con_request;
                    Upd_WMT0200_SapCode("等待SAP回傳訊息", WMT0200data.wmt0200);
                    break;
                case "ZMES9979"://交貨單
                    DTS01_0000.con_function = "ZMES9979";
                    WMT0200data = (WMT0200)Convert.ChangeType(pData, typeof(WMT0200));
                    date = DateTime.Parse(WMT0200data.ins_date);
                    date1 = date.ToString("yyyyMMdd");

                    DateTime sto_date1 = DateTime.Parse(WMT0200data.sto_date);
                    sto_date = sto_date1.ToString("yyyyMMdd");
                    if (sJson != "") { sJson += ","; }
                    sJson += @"{""VBELN"":""" + WMT0200data.rel_code +
                              @""",""WADAT"":""" + sto_date +
                              @""",""WADAT_IST"":""" + date1 +
                              @""",""KUNNR"":""" + "" +
                              @""",""KUNAG"":""" + "" +
                              @""",""BTGEW"":""" + "" +
                              @""",""NTGEW"":""" + "" +
                              @""",""GEWEI"":""" + "" +
                              @""",""VOLUM"":""" + "" +
                              @""",""VOLEH"":""" + "" +
                              @""",""LIFSK"":""" + "" +
                              @""",""LFART"":""" + "" +
                               @""",""wmt0200"":""" + WMT0200data.wmt0200 +
                              @""",""table"":""WMT0200"",""ureport_code"":""" + WMT0200data.rel_code + @"""}";

                    con_request = @"[" + sJson + "]";
                    DTS01_0000.con_request = con_request;
                    Upd_WMT0200_SapCode("等待SAP回傳訊息", WMT0200data.wmt0200);
                    break;
                default:
                    break;
            }
            repoDTS01_0000.InsertData(DTS01_0000);
        }
        public void Ins_DTS01_0000(string pInventoryCode, string pWmsCode)
        {
            DTS01_0000 DTS01_0000 = new DTS01_0000();
            DTS01_0000Repository repoDTS01_0000 = new DTS01_0000Repository();
            DTS01_0000.dts01_0000 = Get_Guid();
            DTS01_0000.con_code = "RFC";
            DTS01_0000.con_type = "B";
            DTS01_0000.result = "";
            DTS01_0000.message = "";
            DTS01_0000.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
            DTS01_0000.ins_time = DateTime.Now.ToString("HH:mm:ss");
            DTS01_0000.usr_code = "";
            DTS01_0000.sch_date = "";
            DTS01_0000.sch_time = "";
            DTS01_0000.sch_usr_code = "";
            DTS01_0000.data_flag = "N";
            DTS01_0000.con_function = "ZMES9977";
            string sInsDate = Get_QueryData("WMT09_0000", pInventoryCode, "erp_inventory_code", "ins_date");
            string con_request = "";
            DateTime date = DateTime.Parse(sInsDate);
            string ins_date = date.ToString("yyyyMMdd");
            string year = date.ToString("yyyy");
            string sJson = "";
            if (sJson != "") { sJson += ","; }
            sJson += @"{""I_WERKS"":""" + "QY01" +
                      @""",""I_GJAHR"":""" + year +
                      @""",""I_IBLNR"":""" + pInventoryCode +
                      @""",""I_ZLDAT"":""" + ins_date +
                      @""",""I_LGORT"":""" + "P000" +
                      @""",""table"":""" + "WMT09_0100" +
                      @""",""ureport_code"":""" + "pInventoryCode" +
                      @""",""wmscode"":""" + pWmsCode +
                      @""",""inventorycode"":""" + pInventoryCode + @"""}";

            con_request = @"[" + sJson + "]";

            DTS01_0000.con_request = con_request;
            repoDTS01_0000.InsertData(DTS01_0000);
        }

        public string Get_LotNo(string pMoCode)
        {
            string sNowDate = DateTime.Now.ToString("yyyy/MM/dd");
            string sLineCode = Get_QueryData("MET01_0000", pMoCode, "mo_code", "plan_line_code");
            string sSql = @"SELECT a.mo_code FROM MET01_0000 a" +
                " LEFT JOIN MEB20_0000 b On a.pro_code=b.pro_code" +
                " WHERE a.mo_status IN ('20','30','50')" +
                " AND sch_date_s=@sch_date_s" +
                " AND plan_line_code=@plan_line_code" +
                " ORDER BY sch_date_s,sch_time_s";
            DataTable dtTmp = Get_DataTable(sSql, "sch_date_s,plan_line_code", sNowDate +","+ sLineCode);
            int iNo = 1;
            for (int i = 0; i <= dtTmp.Rows.Count - 1; i++)
            {
                string mo_code = dtTmp.Rows[i]["mo_code"].ToString();
                if (pMoCode== mo_code)
                {
                    break;
                } else
                {
                    iNo += 1;
                }
            }
            return DateTime.Now.ToString("yyyyMMdd") + StrRigth(sLineCode, 1) + iNo.ToString("00");
        }

        public int Get_SapScrNo(string pMoCode)
        {
            int sRetun = 1;
            string sSql = "";
            sSql = "SELECT ISNULL(MAX(sap_scr_no),0) AS Max_no FROM MET04_0200 WHERE mo_code='"+ pMoCode + "'";
            using (SqlConnection con_db = Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.Read())
                {
                    sRetun = int.Parse(reader["Max_no"].ToString()) + 1;
                }
            }
            return sRetun;
        }

        public bool Chk_Mo_IsOk(string pTable, string pMoCode)
        {
            bool bRetun = false;
            string sSql = "";
            sSql = "SELECT * FROM " + pTable +
                    " WHERE mo_code = '" + pMoCode + "'" +
                    " AND is_ok IN ('Y','1')" +
                    " AND is_del<> 'Y'";
            using (SqlConnection con_db = Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.Read())
                {
                    bRetun = true;
                }
            }
            return bRetun;
        }

        public double Get_MoQty(string pMoCode)
        {
            string sSql = "SELECT ISNULL(SUM(pro_qty),0) FROM WMT0200 WHERE rel_code=@mo_code AND rel_type='I'";
            DataTable dtTmp = Get_DataTable(sSql, "mo_code", pMoCode);
            return double.Parse(dtTmp.Rows[0][0].ToString());
        }

        public double Get_UpQty(string pMoCode)
        {
            string sSql = "SELECT ISNULL(SUM(pro_qty),0) FROM MET04_0100 WHERE mo_code=@mo_code AND is_del<>'Y'";
            DataTable dtTmp = Get_DataTable(sSql, "mo_code", pMoCode);
            return double.Parse(dtTmp.Rows[0][0].ToString());
        }



        /// <summary>
        /// 用SqlBulkCopy新增大量資料
        /// </summary>
        /// <param name="pDesTable">目的地資料表</param>
        /// <param name="pSrcDt">資料來源</param>
        /// <param name="pColMapDic">欄位對應dic，左邊是src，右邊是des</param>
        public void BulkInsertTable(string pDesTable, DataTable pSrcDt, Dictionary<string, string> pColMap)
        {
            SqlConnection conn = new SqlConnection(Get_ConnStr());
            conn.Open();
            using (SqlBulkCopy sqlBC = new SqlBulkCopy(conn))
            {
                //設定一個批次量寫入多少筆資料
                sqlBC.BatchSize = 1000;  // BatchSize要設多少是個issue，有人建議1000

                //設定逾時的秒數
                sqlBC.BulkCopyTimeout = 300;

                //設定 NotifyAfter 屬性，以便在每複製 10000 個資料列至資料表後，呼叫事件處理常式。
                //sqlBC.NotifyAfter = 10000;
                //sqlBC.SqlRowsCopied += new SqlRowsCopiedEventHandler(OnSqlRowsCopied);

                //設定要寫入的資料庫
                sqlBC.DestinationTableName = pDesTable;

                //對應資料行
                foreach (KeyValuePair<string, string> item in pColMap)
                {
                    sqlBC.ColumnMappings.Add(item.Key, item.Value);
                }

                //開始寫入
                sqlBC.WriteToServer(pSrcDt);

            }
            conn.Dispose();
        }

        // 轉換上傳的檔案

        /// <summary>
        /// 把上傳的csv file轉換成datatable
        /// </summary>
        /// <param name="upload"></param>
        /// <returns></returns>
        public DataTable CsvToDataTable(HttpPostedFileBase upload)
        {
            DataTable dt = new DataTable();
            Stream stream = upload.InputStream;
            StreamReader steamReader = new StreamReader(stream, Encoding.GetEncoding("Big5"));
            using (CsvReader csvReader = new CsvReader(steamReader, true))
            {
                dt.Load(csvReader);
            }

            return dt;

        }

        public DataTable XlsToDataTable(HttpPostedFileBase upload)
        {
            string[] fileex = { ".xls", ".xlsx" };

            DataTable dt = ExcelHelper.GetExcelList(upload.InputStream);
            //int imcount;
            //int errorcount;
            //AgentCompanyBll.ImportExcel(dt, LoginInfo.Loginid.ToInt(), out imcount, out errorcount);

            return dt;
        }
        /// <summary>
        /// 把上傳的csv file轉換成datatable
        /// </summary>
        /// <param name="upload"></param>
        /// <param name="sQsheetcode"></param>
        /// <returns></returns>
        public bool FileByUpdateData(HttpPostedFileBase upload,string sQsheetcode="")
        {
            string typeName, pro_code; 
            DataTable dTmp = Get_DataTable(" SELECT * FROM QMB03_0000 WHERE qsheet_code='" + sQsheetcode +"' ");

            if (upload.ContentLength > 0　&& dTmp.Rows.Count > 0)
            {
                typeName = dTmp.Rows[0]["qsheet_type"].ToString();
                pro_code = dTmp.Rows[0]["pro_code"].ToString();
                var fileName = Path.GetFileName(upload.FileName);
                var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Upload/E-sip"), fileName);
                var changeName = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Upload/E-sip"), typeName + "-" + pro_code + ".pdf");
                try
                {
                    if (File.Exists(changeName))
                    {
                        File.Delete(changeName);
                    }
                }
                catch
                {
                    File.Delete(path);
                }
                upload.SaveAs(path);
                File.Move(path, changeName);
               
            }
            else { return false; }
            return true;
        }
        /// <summary>
        /// 把上傳的csv file轉換成datatable
        /// </summary>
        /// <param name="upload"></param>
        /// <param name="sQsheetcode"></param>
        /// <returns></returns>
        public bool FileByDelData(string sQsheetcode = "")
        {
            string typeName, pro_code;
            DataTable dTmp = Get_DataTable(" SELECT * FROM QMB03_0000 WHERE qsheet_code='" + sQsheetcode + "' ");

            if (dTmp.Rows.Count > 0)
            {
                typeName = dTmp.Rows[0]["qsheet_type"].ToString();
                pro_code = dTmp.Rows[0]["pro_code"].ToString();
                var changeName = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Upload/E-sip"), typeName + "-" + pro_code + ".pdf");
                try
                {
                    if (File.Exists(changeName))
                    {
                        File.Delete(changeName);
                    }
                }
                catch
                {
                }
            }
            else { return false; }
            return true;
        }
        /// <summary>
        /// 取得timesStamp (跟javascript一樣)，預設以現在時間計算
        /// </summary>
        /// <param name="pDateTime"></param>
        /// <returns></returns>
        public double GetTimeStamp(DateTime? pDateTime = null)
        {
            // ref: https://stackoverflow.com/questions/31333008/timestamp-calculated-in-javascript-and-c-sharp-are-different

            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            DateTime utcNow = pDateTime == null ? DateTime.UtcNow : pDateTime.Value;

            // 結果跟js相比會多八小時，扣回來
            utcNow = utcNow.AddHours(-8);

            TimeSpan elapsedTime = utcNow - unixEpoch;
            double millis = elapsedTime.TotalMilliseconds;

            int result = (int)millis;

            return millis;
        }
        /// <summary>
        /// 更新WMT0200
        /// </summary>
        /// <param name="pRelCode"></param>
        /// <param name="pwmt0200"></param>
        public void Upd_WMT0200_SapCode(string pRelCode,string pwmt0200)
        {
            string sWmt0200 = pwmt0200;
            string sSql="update WMT0200 set sap_code='"+ pRelCode + "'"+
                        " where wmt0200='"+ sWmt0200 + "'";
            using (SqlConnection con_db = Set_DBConnection())
            {
                con_db.Execute(sSql);
            }
        }

    }
}
