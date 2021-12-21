using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

using MES_WATER.Models;
using Dapper;

namespace MES_WATER.Repository
{
    public class viewBDP001ARepository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得登入使用者的權限設定
        /// </summary>
        /// <param name="pLimitType">A.使用者別/B.角色別</param>
        /// <param name="pTkCode">如果是A則傳使用者帳號，如果是B則傳角色代號</param>
        /// <returns></returns>
        public List<viewBDP001A> Get_DataList(string pLimitType, string pTkCode)
        {
            List<viewBDP001A> list = new List<viewBDP001A>();
            string sSql = "";

            //使用者別
            sSql = "SELECT * FROM BDP04_0000 where is_use = 'Y' order by prg_code";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    viewBDP001A data = new viewBDP001A();
                    data.prg_code = reader["prg_code"].ToString();
                    data.prg_name = comm.Get_QueryData("BDP04_0000", reader["prg_code"].ToString(), "prg_code", "prg_name");
                    data.is_use = CheckIsUse(pLimitType, pTkCode, reader["prg_code"].ToString());

                    if (reader["limit_str"].ToString().Contains("A"))
                    { data.a = CheckLimit(pLimitType, pTkCode, reader["prg_code"].ToString(), "A"); }
                    else
                    { data.a = "L"; }

                    if (reader["limit_str"].ToString().Contains("M"))
                    { data.m = CheckLimit(pLimitType, pTkCode, reader["prg_code"].ToString(), "M"); }
                    else
                    { data.m = "L"; }

                    if (reader["limit_str"].ToString().Contains("D"))
                    { data.d = CheckLimit(pLimitType, pTkCode, reader["prg_code"].ToString(), "D"); }
                    else
                    { data.d = "L"; }

                    if (reader["limit_str"].ToString().Contains("E"))
                    { data.e = CheckLimit(pLimitType, pTkCode, reader["prg_code"].ToString(), "E"); }
                    else
                    { data.e = "L"; }

                    if (reader["limit_str"].ToString().Contains("P"))
                    { data.p = CheckLimit(pLimitType, pTkCode, reader["prg_code"].ToString(), "P"); }
                    else
                    { data.p = "L"; }

                    list.Add(data);
                }
            }
            return list;
        }

        /// <summary>
        /// 檢查程式代號的權限
        /// </summary>
        /// <param name="pLimitType">權限類別</param>
        /// <param name="pTkCode">帳號代號</param>
        /// <param name="pPrgCode">程式代號</param>
        /// <param name="pLimitStr">權限代號(A,M,D,E,P)</param>
        /// <returns></returns>
        public string CheckLimit(string pLimitType, string pTkCode, string pPrgCode, string pLimitStr)
        {
            string sSql = "";
            if (pLimitType == "B")
            {
                sSql = "select * from BDP09_0000 " +
                       " where usr_code = '" + pTkCode + "' " +
                       "  and  prg_code = '" + pPrgCode + "' ";
            }
            else
            {
                sSql = "select * from BDP09_0100 " +
                       " where grp_code = '" + pTkCode + "' " +
                       "  and  prg_code = '" + pPrgCode + "' ";
            }
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                string sLimitState = "";
                while (reader.Read())
                {
                    if (reader["limit_str"].ToString().Contains(pLimitStr))
                    { sLimitState = "Y"; }
                    else
                    { sLimitState = "N"; }
                }
                return sLimitState;
            }
        }

        /// <summary>
        /// 檢查是否使用
        /// </summary>
        /// <param name="pLimitType">權限類別</param>
        /// <param name="pTkCode">帳號代號</param>
        /// <param name="pPrgCode">程式代號</param>
        /// <returns></returns>
        public string CheckIsUse(string pLimitType, string pTkCode, string pPrgCode)
        {
            string sSql = "";
            if (pLimitType == "B")
            {
                sSql = "select * from BDP09_0000 " +
                       " where usr_code = '" + pTkCode + "' " +
                       "  and  prg_code = '" + pPrgCode + "' ";
            }
            else
            {
                sSql = "select * from BDP09_0100 " +
                       " where grp_code = '" + pTkCode + "' " +
                       "  and  prg_code = '" + pPrgCode + "' ";
            }
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                string sIsUse = "";
                while (reader.Read())
                {
                    sIsUse = reader["is_use"].ToString();                    
                }
                return sIsUse;
            }
        }
           
    }
}