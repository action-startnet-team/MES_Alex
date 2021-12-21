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
    public class BDP00_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得BDP00_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO BDP00_0000</returns>
        public BDP00_0000 GetDTO(string pTkCode)
        {
            BDP00_0000 datas = new BDP00_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP00_0000";
            }
            else
            {
                sSql = "SELECT * FROM BDP00_0000 where par_name = @par_name";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@par_name", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new BDP00_0000
                        {
                            par_name = reader.GetString(reader.GetOrdinal("par_name")),
                            par_value = reader.GetString(reader.GetOrdinal("par_value")),
                            par_memo = reader.GetString(reader.GetOrdinal("par_memo")),
                        };
                    }
                }
            }
            return datas;
        }

        /// <summary>
        /// 取得BDP00_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List BDP00_0000</returns>
        public List<BDP00_0000> Get_DataList(string pTkCode)
        {
            List<BDP00_0000> list = new List<BDP00_0000>();
            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP00_0000";
            }
            else
            {
                sSql = "SELECT * FROM BDP00_0000 where par_name=@par_name";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@par_name", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP00_0000 data = new BDP00_0000();
                    data.par_name = reader["par_name"].ToString();
                    data.par_value = reader["par_value"].ToString();
                    data.par_memo = reader["par_memo"].ToString();
                    list.Add(data);
                }

            }
            return list;
        }

        public List<BDP00_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            //取得使用者程式授權
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);

            List<BDP00_0000> list = new List<BDP00_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = "SELECT * FROM BDP00_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@grp_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP00_0000 data = new BDP00_0000();
                    data.par_name = reader["par_name"].ToString();
                    data.par_value = reader["par_value"].ToString();
                    data.par_memo = reader["par_memo"].ToString();

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改

                    list.Add(data);
                }

            }
            return list;
        }

        /// <summary>
        /// 傳入一個BDP00_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="BDP00_0000">DTO</param>
        public void InsertData(BDP00_0000 BDP00_0000)
        {
            string sSql = "INSERT INTO " +
                          " BDP00_0000 ( par_name,  par_value,  par_memo) " +
                          "     VALUES (@par_name, @par_value, @par_memo)";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, BDP00_0000);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@par_name", BDP00_0000.par_name));
                //sqlCommand.Parameters.Add(new SqlParameter("@par_value", BDP00_0000.par_value));
                //sqlCommand.Parameters.Add(new SqlParameter("@par_memo", BDP00_0000.par_memo));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個BDP00_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="BDP00_0000">DTO</param>
        public void UpdateData(BDP00_0000 BDP00_0000)
        {
            string sSql = " UPDATE BDP00_0000 " +
                          "    SET   par_value= @par_value, " +
                          "          par_memo = @par_memo " +
                          "  WHERE   par_name = @par_name ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, BDP00_0000);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@par_name", BDP00_0000.par_name));
                //sqlCommand.Parameters.Add(new SqlParameter("@par_value", BDP00_0000.par_value));
                //sqlCommand.Parameters.Add(new SqlParameter("@par_memo", BDP00_0000.par_memo));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM BDP00_0000 WHERE par_name = @par_name";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { par_name = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@par_name", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得BDP00_0000角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetBDP00_0000_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("par_name", System.Type.GetType("System.String"));
            dtDat.Columns.Add("par_value", System.Type.GetType("System.String"));
            dtDat.Columns.Add("par_memo", System.Type.GetType("System.String"));

            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP00_0000";
            }
            else
            {
                sSql = "SELECT * FROM BDP00_0000 where par_name='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["par_name"] = dtTmp.Rows[i]["par_name"];
                drow["par_value"] = dtTmp.Rows[i]["par_value"];
                drow["par_memo"] = dtTmp.Rows[i]["par_memo"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}