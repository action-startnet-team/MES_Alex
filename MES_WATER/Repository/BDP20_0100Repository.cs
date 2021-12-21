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
    public class BDP20_0100Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得BDP20_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO BDP20_0100</returns>
        public BDP20_0100 GetDTO(string pTkCode)
        {
            BDP20_0100 datas = new BDP20_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP20_0100";
            }
            else
            {
                sSql = "SELECT * FROM BDP20_0100 WHERE bdp20_0100 = @bdp20_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@bdp20_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new BDP20_0100
                        {
                            bdp20_0100 = reader.GetInt32(reader.GetOrdinal("bdp20_0100")),
                            usr_code = reader.GetString(reader.GetOrdinal("usr_code")),
                            prg_code = reader.GetString(reader.GetOrdinal("prg_code")),
                            usr_date = reader.GetString(reader.GetOrdinal("usr_date")),
                            usr_time = reader.GetString(reader.GetOrdinal("usr_time")),
                            cmemo = reader.GetString(reader.GetOrdinal("cmemo")),
                            exception = reader.GetString(reader.GetOrdinal("exception")),

                        };
                    }
                }
            }
            return datas;
        }

        /// <summary>
        /// 取得BDP20_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List BDP20_0100</returns>
        public List<BDP20_0100> Get_DataList(string pTkCode)
        {
            List<BDP20_0100> list = new List<BDP20_0100>();
            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP20_0100";
            }
            else
            {
                sSql = "SELECT * FROM BDP20_0100 WHERE bdp20_0100 = @bdp20_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@bdp20_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP20_0100 data = new BDP20_0100();
                    data.bdp20_0100 = reader.GetInt32(reader.GetOrdinal("bdp20_0100"));
                    data.usr_code = reader["usr_code"].ToString();
                    data.prg_code = reader["prg_code"].ToString();
                    data.usr_date = reader["usr_date"].ToString();
                    data.usr_time = reader["usr_time"].ToString();
                    data.cmemo = reader["cmemo"].ToString();
                    data.cmemo = reader["exception"].ToString();

                    list.Add(data);
                }

            }
            return list;
        }

        /// <summary>
        /// 傳入一個BDP20_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="BDP20_0100">DTO</param>
        public void InsertData(BDP20_0100 BDP20_0100)
        {
            string sSql = "INSERT INTO " +
                          "BDP20_0100 ( usr_code,  prg_code, " +
                          "             usr_date,  usr_time,  cmemo, exception) " +
                          "    VALUES (@usr_code, @prg_code, " +
                          "            @usr_date, @usr_time, @cmemo, @exception)";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, BDP20_0100);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@bdp20_0100", BDP20_0100.bdp20_0100));
                //sqlCommand.Parameters.Add(new SqlParameter("@usr_code", BDP20_0100.usr_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@prg_code", BDP20_0100.prg_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@usr_date", BDP20_0100.usr_date));
                //sqlCommand.Parameters.Add(new SqlParameter("@usr_time", BDP20_0100.usr_time));
                //sqlCommand.Parameters.Add(new SqlParameter("@cmemo", BDP20_0100.cmemo));
                //sqlCommand.Parameters.Add(new SqlParameter("@exception", BDP20_0100.exception));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個BDP20_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="BDP20_0100">DTO</param>
        public void UpdateData(BDP20_0100 BDP20_0100)
        {
            string sSql = " UPDATE BDP20_0100 " +
                          "    SET usr_code = @usr_code, " +
                          "        prg_code = @prg_code, " +
                          "        usr_date = @usr_date, " +
                          "        usr_time = @usr_time, " +
                          "        exception = @exception " +
                          "  WHERE bdp20_0100 = @bdp20_0100";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, BDP20_0100);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@bdp20_0100", BDP20_0100.bdp20_0100));
                //sqlCommand.Parameters.Add(new SqlParameter("@usr_code", BDP20_0100.usr_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@prg_code", BDP20_0100.prg_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@usr_date", BDP20_0100.usr_date));
                //sqlCommand.Parameters.Add(new SqlParameter("@usr_time", BDP20_0100.usr_time));
                //sqlCommand.Parameters.Add(new SqlParameter("@cmemo", BDP20_0100.cmemo));
                //sqlCommand.Parameters.Add(new SqlParameter("@exception", BDP20_0100.exception));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM BDP20_0100 WHERE bdp20_0100 = @bdp20_0100";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { bdp20_0100 = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@bdp20_0100", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得BDP20_0100角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetBDP20_0100_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("bdp20_0100", System.Type.GetType("System.String"));
            dtDat.Columns.Add("usr_code", System.Type.GetType("System.String"));
            dtDat.Columns.Add("prg_code", System.Type.GetType("System.String"));

            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP20_0100";
            }
            else
            {
                sSql = "SELECT * FROM BDP20_0100 WHERE bdp20_0100='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["bdp20_0100"] = dtTmp.Rows[i]["bdp20_0100"];
                drow["usr_code"] = dtTmp.Rows[i]["usr_code"];
                drow["prg_code"] = dtTmp.Rows[i]["prg_code"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}