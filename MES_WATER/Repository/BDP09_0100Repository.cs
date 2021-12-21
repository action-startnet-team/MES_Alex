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
    public class BDP09_0100Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得BDP09_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO BDP09_0100</returns>
        public BDP09_0100 GetDTO(string pTkCode)
        {
            BDP09_0100 datas = new BDP09_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP09_0100";
            }
            else
            {
                sSql = "SELECT * FROM BDP09_0100 WHERE bdp09_0100 = @bdp09_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@bdp09_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new BDP09_0100
                        {
                            bdp09_0100 = reader.GetInt32(reader.GetOrdinal("bdp09_0100")),
                            grp_code = reader.GetString(reader.GetOrdinal("grp_code")),
                            prg_code = reader.GetString(reader.GetOrdinal("prg_code")),
                            limit_str = reader.GetString(reader.GetOrdinal("limit_str")),
                            is_use = reader.GetString(reader.GetOrdinal("is_use")),
                        };
                    }
                }
            }
            return datas;
        }

        /// <summary>
        /// 取得BDP09_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List BDP09_0100</returns>
        public List<BDP09_0100> Get_DataList(string pTkCode)
        {
            List<BDP09_0100> list = new List<BDP09_0100>();
            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP09_0100";
            }
            else
            {
                sSql = "SELECT * FROM BDP09_0100 WHERE bdp09_0100 = @bdp09_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@bdp09_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP09_0100 data = new BDP09_0100();
                    data.bdp09_0100 = reader.GetInt32(reader.GetOrdinal("bdp09_0100"));
                    data.grp_code = reader["grp_code"].ToString();
                    data.prg_code = reader["prg_code"].ToString();
                    data.limit_str = reader["limit_str"].ToString();
                    data.is_use = reader["is_use"].ToString();
                    list.Add(data);
                }

            }
            return list;
        }

        /// <summary>
        /// 傳入一個BDP09_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="BDP09_0100">DTO</param>
        public void InsertData(BDP09_0100 BDP09_0100)
        {
            string sSql = "INSERT INTO " +
                          " BDP09_0100 (grp_code, prg_code,   limit_str,  is_use) " +
                          "     VALUES (@grp_code, @prg_code, @limit_str, @is_use)";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, BDP09_0100);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@bdp09_0100", BDP09_0100.bdp09_0100));
                //sqlCommand.Parameters.Add(new SqlParameter("@grp_code", BDP09_0100.grp_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@prg_code", BDP09_0100.prg_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@limit_str", BDP09_0100.limit_str));
                //sqlCommand.Parameters.Add(new SqlParameter("@is_use", BDP09_0100.is_use));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個BDP09_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="BDP09_0100">DTO</param>
        public void UpdateData(BDP09_0100 BDP09_0100)
        {
            string sSql = " UPDATE BDP09_0100 " +
                          "    SET grp_code  = @grp_code, " +
                          "        prg_code  = @prg_code, " +
                          "        limit_str = @limit_str," + 
                          "        is_use = @is_use " +
                          "WHERE bdp09_0100 = @bdp09_0100";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, BDP09_0100);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@bdp09_0100", BDP09_0100.bdp09_0100));
                //sqlCommand.Parameters.Add(new SqlParameter("@grp_code", BDP09_0100.grp_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@prg_code", BDP09_0100.prg_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@limit_str", BDP09_0100.limit_str));
                //sqlCommand.Parameters.Add(new SqlParameter("@is_use", BDP09_0100.is_use));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM BDP09_0100 WHERE bdp09_0100 = @bdp09_0100";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { bdp09_0100 = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@bdp09_0100", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得BDP09_0100角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetBDP09_0100_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("BDP09_0100", System.Type.GetType("System.String"));
            dtDat.Columns.Add("grp_code", System.Type.GetType("System.String"));
            dtDat.Columns.Add("prg_code", System.Type.GetType("System.String"));

            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP09_0100";
            }
            else
            {
                sSql = "SELECT * FROM BDP09_0100 WHERE BDP09_0100='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["BDP09_0100"] = dtTmp.Rows[i]["BDP09_0100"];
                drow["grp_code"] = dtTmp.Rows[i]["grp_code"];
                drow["prg_code"] = dtTmp.Rows[i]["prg_code"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}