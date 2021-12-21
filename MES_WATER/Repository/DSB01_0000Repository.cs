using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

using MES_WATER.Models;
using Dapper;
using System.Collections;

namespace MES_WATER.Repository
{
    public class DSB01_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得DSP00_0000中的單一資料
        /// </summary>
        /// <param name = "pTkCode" > 鍵值  </ param >
        /// < returns > DTO DSP00_0000</returns>
        public DSB01_0000 GetDTO(string pTkCode)
        {
            DSB01_0000 data = new DSB01_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM DSB01_0000";
            }
            else
            {
                sSql = "SELECT * FROM DSB01_0000 where dsb01_0000=@dsb01_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@dsb01_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        data.dsb01_0000 = reader.GetInt32(reader.GetOrdinal("dsb01_0000"));
                        // 機台設定
                        data.mac_code = reader.GetString(reader.GetOrdinal("mac_code"));
                        data.mac_name = comm.Get_Data("MEB01_0000",reader.GetString(reader.GetOrdinal("mac_code")),"mac_code","mac_name")  ;
                        data.wrk_time = reader.GetDecimal(reader.GetOrdinal("wrk_time"));
                        data.wrk_qty = reader.GetDecimal(reader.GetOrdinal("wrk_qty"));
                        // 稼動率/產能效率/良率
                        data.utilization_rate = reader.GetDecimal(reader.GetOrdinal("utilization_rate"));
                        data.capacity_efficiency = reader.GetDecimal(reader.GetOrdinal("capacity_efficiency"));
                        data.yield = reader.GetDecimal(reader.GetOrdinal("yield"));

                        data.pro_qty = reader.GetDecimal(reader.GetOrdinal("pro_qty"));
                        data.ng_qty = reader.GetDecimal(reader.GetOrdinal("ng_qty"));
                        data.act_time = reader.GetDecimal(reader.GetOrdinal("act_time"));

                        data.cal_date = reader.GetDateTime(reader.GetOrdinal("cal_date")).ToString("yyyy/MM/dd");
                    }
                }
            }
            return data;
        }



        /// <summary>
        /// 取得DSB01_0000所有資料表內容
        /// </summary>
        /// <returns></returns>
        public List<DSB01_0000> Get_DataList(string pTkCode="")
        {
            List<DSB01_0000> list = new List<DSB01_0000>();
            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "Select DSB01_0000.*, MEB01_0000.mac_name " +
                   "  from DSB01_0000 " +
                   "  join MEB01_0000 on MEB01_0000.mac_code = DSB01_0000.mac_code order by cal_date";
            }else
            {
                sSql = " Select DSB01_0000.*, MEB01_0000.mac_name " +
                       "   from DSB01_0000 " +
                       "   join MEB01_0000 on MEB01_0000.mac_code = DSB01_0000.mac_code " +
                       "   where DSB01_0000.mac_code = @mac_code order by cal_date";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@mac_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    DSB01_0000 data = new DSB01_0000();
                    data.dsb01_0000 = reader.GetInt32(reader.GetOrdinal("dsb01_0000"));
                    // 機台設定
                    data.mac_code = reader.GetString(reader.GetOrdinal("mac_code"));
                    data.mac_name = reader.GetString(reader.GetOrdinal("mac_name"));
                    data.wrk_time = reader.GetDecimal(reader.GetOrdinal("wrk_time"));
                    data.wrk_qty = reader.GetDecimal(reader.GetOrdinal("wrk_qty"));
                    // 稼動率/產能效率/良率
                    data.utilization_rate = reader.GetDecimal(reader.GetOrdinal("utilization_rate"));
                    data.capacity_efficiency = reader.GetDecimal(reader.GetOrdinal("capacity_efficiency"));
                    data.yield = reader.GetDecimal(reader.GetOrdinal("yield"));
                    // 實際 良品/不良品/工作時間
                    data.pro_qty = reader.GetDecimal(reader.GetOrdinal("pro_qty"));
                    data.ng_qty = reader.GetDecimal(reader.GetOrdinal("ng_qty"));
                    data.act_time = reader.GetDecimal(reader.GetOrdinal("act_time"));

                    data.cal_date = reader.GetDateTime(reader.GetOrdinal("cal_date")).ToString("yyyy/MM/dd");

                    list.Add(data);
                }
            }
            return list;
        }

        public List<DSB01_0000> Get_Data(string pDate, List<string> pMacCodeList)
        {
            List<DSB01_0000> list = new List<DSB01_0000>();
            string sSql = "";
            if (string.IsNullOrEmpty(pDate))
            {
                sSql = "Select DSB01_0000.*, MEB01_0000.mac_name " +
                   "  from DSB01_0000 " +
                   "  left join MEB01_0000 on MEB01_0000.mac_code = DSB01_0000.mac_code " +
                   "    Where DSB01_0000.mac_code in @mac_code_list" +
                   "    order by cal_date";
            }
            else
            {
                sSql = " Select DSB01_0000.*, MEB01_0000.mac_name " +
                       "   from DSB01_0000 " +
                       "   left join MEB01_0000 on MEB01_0000.mac_code = DSB01_0000.mac_code " +
                       "   where DSB01_0000.cal_date = @cal_date " +
                       "    and DSB01_0000.mac_code in @mac_code_list" +
                       "    order by cal_date";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                list = con_db.Query<DSB01_0000>(sSql, new { cal_date = pDate, mac_code_list = pMacCodeList }).ToList();
            }
            return list;
        }

        public void InsertData(DSB01_0000 dsb01_0000)
        {
            string sSql = "INSERT INTO " +
                          " DSB01_0000 ( mac_code, utilization_rate, capacity_efficiency, yield, pro_qty, ng_qty, cal_date, act_time, wrk_time, wrk_qty) " +
                          "     VALUES (@mac_code,@utilization_rate,@capacity_efficiency,@yield,@pro_qty,@ng_qty,@cal_date,@act_time,@wrk_time,@wrk_qty)";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, dsb01_0000);
            }
        }


        /// <summary>
        /// 傳入一個DSP00_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="dsp00_0000">DTO</param>
        public void UpdateData(DSB01_0000 dsb01_0000)
        {
            string sSql = " UPDATE DSB01_0000 " +
                          "    SET pro_qty= @pro_qty" +
                          "       ,ng_qty  = @ng_qty " +
                          "       ,act_time  = @act_time " +
                          "       ,wrk_time  = @wrk_time " +
                          "       ,wrk_qty  = @wrk_qty " +
                          "       ,capacity_efficiency  = @capacity_efficiency " +
                          "       ,utilization_rate  = @utilization_rate " +
                          "       ,yield  = @yield " +
                          "  WHERE dsb01_0000= @dsb01_0000 ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, dsb01_0000);
            }
        }
        /// <summary>
        /// 傳入一個DSP00_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="dsp00_0000">DTO</param>
        public void UpdateDataByApi(DSB01_0000 dsb01_0000)
        {
            string sSql = " UPDATE DSB01_0000 " +
                          "    SET pro_qty= @pro_qty" +
                          "       ,act_time  = @act_time " +
                          "       ,capacity_efficiency  = @capacity_efficiency " +
                          "       ,utilization_rate = @utilization_rate " +
                          "       ,yield  = @yield " +
                          "  WHERE mac_code = @mac_code " + 
                          "    and cal_date = @cal_date";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, dsb01_0000);
            }
        }

        public void DeleteData(string pTkCode)
        {
            string sSql = "Delete from DSB01_0000" +
                          " where dsb01_0000 = @dsb01_0000";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { dsb01_0000 = pTkCode });
            }
        }

        public void DeleteDataByDate(DSB01_0000 dsb01_0000)
        {
            string sSql = "Delete from DSB01_0000" +
                          " where mac_code = @mac_code" +
                          "   and cal_date = @cal_date" ;

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { mac_code = dsb01_0000.mac_code, cal_date = dsb01_0000.cal_date });
            }
        }

        /// <summary>
        /// 取得欄位的資料list
        /// </summary>
        /// <param name="mac_code"></param>
        /// <returns></returns>
        public List<string> Get_ColumnDataList(string pColumnName, string pMac_code)
        {
            List<string> list = new List<string>();

            string sSql = "Select * from DSB01_0000 where mac_code = @mac_code";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@mac_code", pMac_code));
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(reader[pColumnName].ToString());
                }
            }
            return list;
        }


        /// <summary>
        /// 輸入機台代碼，取得稼動率平均值
        /// </summary>
        /// <param name="pColumnName"></param>
        /// <param name="pMac_code"></param>
        /// <returns></returns>
        public string Get_utilization_rate(string pMac_code)
        {

            string sSql = "Select avg(utilization_rate) from DSB01_0000 where mac_code = @mac_code";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@mac_code", pMac_code));
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    if (string.IsNullOrEmpty(reader[0].ToString()))
                    {
                        return "0.00";
                    }
                    return reader[0].ToString();
                }
            }
            return "0.00";
        }

        /// <summary>
        /// 輸入機台代碼，取得產能效率平均值
        /// </summary>
        /// <param name="pColumnName"></param>
        /// <param name="pMac_code"></param>
        /// <returns></returns>
        public string Get_capacity_efficiency(string pMac_code)
        {

            string sSql = "Select avg(capacity_efficiency) from DSB01_0000 where mac_code = @mac_code";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@mac_code", pMac_code));
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    if (string.IsNullOrEmpty(reader[0].ToString()))
                    {
                        return "0";
                    }
                    return reader[0].ToString();
                }
            }
            return "0";
        }

        /// <summary>
        /// 輸入機台代碼，取得良率平均值
        /// </summary>
        /// <param name="pColumnName"></param>
        /// <param name="pMac_code"></param>
        /// <returns></returns>
        public string Get_yield(string pMac_code)
        {

            string sSql = "Select avg(yield) from DSB01_0000 where mac_code = @mac_code";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@mac_code", pMac_code));
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    if (string.IsNullOrEmpty(reader[0].ToString()))
                    {
                        return "0";
                    }
                    return reader[0].ToString();
                }
            }
            return "0";
        }

        


    }
}