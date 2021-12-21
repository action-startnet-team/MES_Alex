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
    public class MAC01_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MAC01_0000 DTO
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MAC01_0000</returns>
        public MAC01_0000 GetDTO(string pTkCode)
        {
            MAC01_0000 datas = new MAC01_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MAC01_0000";
            }
            else
            {
                sSql = "SELECT * FROM MAC01_0000 where mac_code = @mac_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@mac_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MAC01_0000
                        {
                            mac_code = reader.GetString(reader.GetOrdinal("mac_code")),
                            mac_name = reader.GetString(reader.GetOrdinal("mac_name")),
                            wrk_time = reader.GetDecimal(reader.GetOrdinal("wrk_time")),
                            wrk_qty = reader.GetDecimal(reader.GetOrdinal("wrk_qty")),
                            u_limit = reader.GetDecimal(reader.GetOrdinal("u_limit")),

                            img_url = reader.IsDBNull(reader.GetOrdinal("img_url")) ? "" : reader.GetString(reader.GetOrdinal("img_url")),
                        };
                    }
                }
            }
            return datas;
        }

        /// <summary>
        /// 取得MAC01_0000所有資料表內容
        /// </summary>
        /// <returns></returns>
        public List<MAC01_0000> Get_DataList()
        {
            List<MAC01_0000> list = new List<MAC01_0000>();
            string sSql = "";

            sSql = "Select * from MAC01_0000 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    //reader.GetString(reader.GetOrdinal("dsb01_0000"));
                    MAC01_0000 data = new MAC01_0000();
                    data.mac_code = reader.GetString(reader.GetOrdinal("mac_code"));
                    data.mac_name = reader.GetString(reader.GetOrdinal("mac_name"));
                    data.wrk_time = reader.GetDecimal(reader.GetOrdinal("wrk_time"));
                    data.wrk_qty = reader.GetDecimal(reader.GetOrdinal("wrk_qty"));
                    data.u_limit = reader.GetDecimal(reader.GetOrdinal("u_limit"));

                    list.Add(data);
                }
            }
            return list;
        }

        public void InsertData(MAC01_0000 meb01_0000)
        {
            string sSql = "INSERT INTO " +
                          " MAC01_0000 (mac_code, mac_name, wrk_time, wrk_qty, u_limit) " +
                          "     VALUES (@mac_code, @mac_name, @wrk_time, @wrk_qty, @u_limit)";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, meb01_0000);
            }
        }

        /// <summary>
        /// 傳入一個MAC01_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="meb01_0000">DTO</param>
        public void UpdateData(MAC01_0000 meb01_0000)
        {
            string sSql = " UPDATE MAC01_0000 " +
                          "    SET mac_name= @mac_name" +
                          "        ,wrk_time  = @wrk_time " +
                          "        ,wrk_qty  = @wrk_qty " +
                          "        ,u_limit  = @u_limit " +
                          "  WHERE mac_code= @mac_code ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, meb01_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MAC01_0000 WHERE mac_code = @mac_code;";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { mac_code = pTkCode });
            }
        }


    }
}