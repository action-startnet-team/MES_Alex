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
    public class BDP16_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得BDP16_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO BDP16_0000</returns>
        public BDP16_0000 GetDTO(string pTkCode)
        {
            BDP16_0000 datas = new BDP16_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP16_0000";
            }
            else
            {
                sSql = "SELECT * FROM BDP16_0000 WHERE todo_code = @todo_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@todo_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new BDP16_0000
                        {
                            todo_code = reader.GetString(reader.GetOrdinal("todo_code")),
                            todo_name = reader.GetString(reader.GetOrdinal("todo_name")),
                            todo_url = reader.GetString(reader.GetOrdinal("todo_url")),
                            todo_key = reader.GetString(reader.GetOrdinal("todo_key")),
                            is_use = reader.GetString(reader.GetOrdinal("todo_url")),
                        };
                    }
                }
            }
            return datas;
        }

        /// <summary>
        /// 取得BDP16_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List BDP16_0000</returns>
        public List<BDP16_0000> Get_DataList(string pTkCode)
        {
            List<BDP16_0000> list = new List<BDP16_0000>();
            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP16_0000";
            }
            else
            {
                sSql = "SELECT * FROM BDP16_0000 WHERE todo_code = @todo_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@todo_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP16_0000 data = new BDP16_0000();
                    data.todo_code = reader["todo_code"].ToString();
                    data.todo_name = reader["todo_name"].ToString();
                    data.todo_url = reader["todo_url"].ToString();
                    data.todo_key = reader["todo_key"].ToString();
                    data.is_use = reader["is_use"].ToString();
                    list.Add(data);
                }

            }
            return list;
        }

        /// <summary>
        /// 傳入一個BDP16_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="BDP16_0000">DTO</param>
        public void InsertData(BDP16_0000 BDP16_0000)
        {
            string sSql_BDP16 = " where todo_key='" + BDP16_0000.todo_key + "' and usr_code='" + BDP16_0000.usr_code + "'";
            string sIsOk = comm.Get_QueryData("BDP16_0000", sSql_BDP16, "is_ok");
            if (sIsOk != "N")
            {
                string sSql = "INSERT INTO " +
                          " BDP16_0000 ( todo_code,  todo_name,  todo_url,  todo_key,  is_use, is_ok, usr_code) " +
                          "     VALUES (@todo_code, @todo_name, @todo_url, @todo_key, @is_use, @is_ok, @usr_code)";
                using (SqlConnection con_db = comm.Set_DBConnection())
                {
                    con_db.Execute(sSql, BDP16_0000);

                }
            }
            
        }

        /// <summary>
        /// 傳入一個BDP16_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="BDP16_0000">DTO</param>
        public void UpdateData(BDP16_0000 BDP16_0000)
        {
            string sSql = " UPDATE BDP16_0000 " +
                          "    SET todo_name = @todo_name, " +
                          "        todo_url = @todo_url," +
                          "        todo_key = @todo_key," +
                          "        is_use  = @is_use " +
                          "  WHERE todo_code = @todo_code";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, BDP16_0000);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@todo_code", BDP16_0000.todo_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@todo_name", BDP16_0000.todo_name));
                //sqlCommand.Parameters.Add(new SqlParameter("@todo_url", BDP16_0000.todo_url));
                //sqlCommand.Parameters.Add(new SqlParameter("@todo_key", BDP16_0000.todo_key));
                //sqlCommand.Parameters.Add(new SqlParameter("@is_use", BDP16_0000.is_use));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM BDP16_0000 WHERE todo_code = @todo_code ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { todo_code = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@todo_code", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得BDP16_0000角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetBDP16_0000_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("todo_code", System.Type.GetType("System.String"));
            dtDat.Columns.Add("todo_name", System.Type.GetType("System.String"));
            dtDat.Columns.Add("todo_url", System.Type.GetType("System.String"));

            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP16_0000";
            }
            else
            {
                sSql = "SELECT * FROM BDP16_0000 WHERE todo_code='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["todo_code"] = dtTmp.Rows[i]["todo_code"];
                drow["todo_name"] = dtTmp.Rows[i]["todo_name"];
                drow["todo_url"] = dtTmp.Rows[i]["todo_url"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}