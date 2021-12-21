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
    public class BDP30_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得BDP30_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO BDP30_0000</returns>
        public BDP30_0000 GetDTO(string pTkCode)
        {
            BDP30_0000 datas = new BDP30_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP30_0000";
            }
            else
            {
                sSql = "SELECT * FROM BDP30_0000 where bdp30_0000=@bdp30_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@bdp30_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new BDP30_0000
                        {
                            bdp30_0000 = reader.GetInt32(reader.GetOrdinal("bdp30_0000")),
                            prg_code = reader.GetString(reader.GetOrdinal("prg_code")),
                            usr_code = reader.GetString(reader.GetOrdinal("usr_code")),
                            view_code = reader.GetString(reader.GetOrdinal("view_code")),
                            col_index = reader.GetInt32(reader.GetOrdinal("col_index")),
                            col_width = reader.GetInt32(reader.GetOrdinal("col_index")),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得BDP30_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List BDP30_0000</returns>
        public List<BDP30_0000> Get_DataList(string pTkCode)
        {
            List<BDP30_0000> list = new List<BDP30_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP30_0000";
            }
            else
            {
                sSql = "SELECT * FROM BDP30_0000 where bdp30_0000=@bdp30_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@bdp30_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP30_0000 data = new BDP30_0000();
                    data.bdp30_0000 = reader.GetInt32(reader.GetOrdinal("bdp30_0000"));
                    data.prg_code = reader.GetString(reader.GetOrdinal("prg_code"));
                    data.usr_code = reader.GetString(reader.GetOrdinal("usr_code"));
                    data.view_code = reader.GetString(reader.GetOrdinal("view_code"));
                    data.col_index = reader.GetInt32(reader.GetOrdinal("col_index"));
                    data.col_width = reader.GetInt32(reader.GetOrdinal("col_index"));

                    data.can_delete = "Y";
                    data.can_update = "Y";
                    list.Add(data);
                }

            }
            return list;
        }

        /// <summary>
        /// 取得使用者可以編輯的資料，結合商務邏輯權限
        /// </summary>
        /// <param name="pUsrCode"></param>
        /// <param name="pPrgCode"></param>
        /// <returns></returns>
        public List<BDP30_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_bdp30_0000", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<BDP30_0000> list = new List<BDP30_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = "SELECT * FROM BDP30_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@bdp30_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP30_0000 data = new BDP30_0000();

                    data.bdp30_0000 = reader.GetInt32(reader.GetOrdinal("bdp30_0000"));
                    data.prg_code = reader.GetString(reader.GetOrdinal("prg_code"));
                    data.usr_code = reader.GetString(reader.GetOrdinal("usr_code"));
                    data.view_code = reader.GetString(reader.GetOrdinal("view_code"));
                    data.col_index = reader.GetInt32(reader.GetOrdinal("col_index"));
                    data.col_width = reader.GetInt32(reader.GetOrdinal("col_index"));

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.bdp30_0000))
                    //{
                    //    data.can_delete = "N";
                    //    data.can_update = "N";
                    //}

                    list.Add(data);
                }
            }
            return list;
        }



        #endregion

        /// <summary>
        /// 傳入一個BDP30_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="bdp30_0000">DTO</param>
        public void InsertData(BDP30_0000 bdp30_0000)
        {
            string sSql = "INSERT INTO " +
                          " BDP30_0000 (prg_code, usr_code, view_code, col_index, col_width) " +
                          "     VALUES (@prg_code, @usr_code, @view_code, @col_index, @col_width) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, bdp30_0000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@bdp30_0000", bdp30_0000.bdp30_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@prg_code", bdp30_0000.prg_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@usr_code", bdp30_0000.usr_code));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個BDP30_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="bdp30_0000">DTO</param>
        public void UpdateData(BDP30_0000 bdp30_0000)
        {
            string sSql = " UPDATE BDP30_0000 " +
                          "     SET col_width = @col_width" +
                          "     WHERE prg_code= @prg_code " +
                          "         and usr_code = @usr_code " +
                          "         and view_code = @view_code " +
                          "         and col_index = @col_index ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, bdp30_0000);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@bdp30_0000", bdp30_0000.bdp30_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@prg_code", bdp30_0000.prg_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@usr_code", bdp30_0000.usr_code));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM BDP30_0000 WHERE bdp30_0000 = @bdp30_0000;";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { bdp30_0000 = pTkCode });

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@bdp30_0000", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }





        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得BDP30_0000角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetBDP30_0000_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("bdp30_0000", System.Type.GetType("System.String"));
            dtDat.Columns.Add("prg_code", System.Type.GetType("System.String"));
            dtDat.Columns.Add("usr_code", System.Type.GetType("System.String"));

            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP30_0000";
            }
            else
            {
                sSql = "SELECT * FROM BDP30_0000 where bdp30_0000='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["bdp30_0000"] = dtTmp.Rows[i]["bdp30_0000"];
                drow["prg_code"] = dtTmp.Rows[i]["prg_code"];
                drow["usr_code"] = dtTmp.Rows[i]["usr_code"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}