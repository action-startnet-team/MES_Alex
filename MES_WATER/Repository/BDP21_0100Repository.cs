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
    public class BDP21_0100Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();
        /// <summary>
        /// 取得BDP21_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO BDP21_0100</returns>
        public BDP21_0100 GetDTO(string pTkCode)
        {
            BDP21_0100 datas = new BDP21_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP21_0100";
            }
            else
            {
                sSql = "SELECT * FROM BDP21_0100 WHERE bdp21_0100 = @bdp21_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@bdp21_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new BDP21_0100
                        {
                            bdp21_0100 = reader.GetInt32(reader.GetOrdinal("bdp21_0100")),
                            code_code = reader.GetString(reader.GetOrdinal("code_code")),
                            field_code = reader.GetString(reader.GetOrdinal("field_code")),
                            field_name = reader.GetString(reader.GetOrdinal("field_name")),
                            scr_no = reader.GetInt32(reader.GetOrdinal("scr_no")),
                            is_use = reader.GetString(reader.GetOrdinal("is_use")),
                        };
                    }
                }
            }
            return datas;
        }

        /// <summary>
        /// 根據code_code取得BDP21_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List BDP21_0100</returns>
        public List<BDP21_0100> Get_DataList(string pTkCode)
        {
            List<BDP21_0100> list = new List<BDP21_0100>();
            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP21_0100 order by code_code, scr_no ";
            }
            else
            {
                sSql = "SELECT * FROM BDP21_0100 WHERE code_code=@code_code order by scr_no";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@code_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP21_0100 data = new BDP21_0100();
                    data.bdp21_0100 = reader.GetInt32(reader.GetOrdinal("bdp21_0100"));
                    data.code_code = reader.GetString(reader.GetOrdinal("code_code"));
                    data.field_code = reader.GetString(reader.GetOrdinal("field_code"));
                    data.field_name = reader.GetString(reader.GetOrdinal("field_name"));
                    data.scr_no = reader.GetInt32(reader.GetOrdinal("scr_no"));
                    data.is_use = reader.GetString(reader.GetOrdinal("is_use"));
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
        public List<BDP21_0100> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);

            List<BDP21_0100> list = new List<BDP21_0100>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = "SELECT * FROM BDP21_0100 order by code_code, scr_no";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@grp_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP21_0100 data = new BDP21_0100();

                    data.bdp21_0100 = reader.GetInt32(reader.GetOrdinal("bdp21_0100"));
                    data.code_code = reader.GetString(reader.GetOrdinal("code_code"));
                    data.field_code = reader.GetString(reader.GetOrdinal("field_code"));
                    data.field_name = reader.GetString(reader.GetOrdinal("field_name"));
                    data.scr_no = reader.GetInt32(reader.GetOrdinal("scr_no"));
                    data.is_use = reader.GetString(reader.GetOrdinal("is_use"));

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
        /// 根據pTkCode取得BDP21_0100資料表內容，並結合使用者權限
        /// </summary>
        /// <param name="pUsrCode">使用者代號</param>
        /// <param name="pPrgCode">功能代號</param>
        /// <param name="pTkCode">要抓取的條件，field value</param>
        /// <returns></returns>
        public List<BDP21_0100> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode="")
        {
            List<BDP21_0100> list = new List<BDP21_0100>();
            string foreignKey = gmv.GetKey<BDP21_0000>(new BDP21_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP21_0100 where " + foreignKey + "=@" + foreignKey + " order by scr_no";
            }
            else
            {
                sSql = "SELECT * FROM BDP21_0100";
            }
            //取得該使用者可以看的資料
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter(foreignKey, pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP21_0100 data = new BDP21_0100();

                    data.bdp21_0100 = reader.GetInt32(reader.GetOrdinal("bdp21_0100"));
                    data.code_code = reader.GetString(reader.GetOrdinal("code_code"));
                    data.field_code = reader.GetString(reader.GetOrdinal("field_code"));
                    data.field_name = reader.GetString(reader.GetOrdinal("field_name"));
                    data.scr_no = reader.GetInt32(reader.GetOrdinal("scr_no"));
                    data.is_use = reader.GetString(reader.GetOrdinal("is_use"));

                    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
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
        /// 傳入一個BDP21_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="BDP21_0100">DTO</param>
        public void InsertData(BDP21_0100 BDP21_0100)
        {
            string sSql = "INSERT INTO " +
                          " BDP21_0100 ( code_code,  field_code,  field_name,  scr_no,  is_use )" +
                          "     VALUES (@code_code, @field_code, @field_name, @scr_no, @is_use)" ;

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                 con_db.Execute(sSql, BDP21_0100);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@code_code", BDP21_0100.code_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@field_code", BDP21_0100.field_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@field_name", BDP21_0100.field_name));
                //sqlCommand.Parameters.Add(new SqlParameter("@scr_no", BDP21_0100.scr_no.ToString()));
                //sqlCommand.Parameters.Add(new SqlParameter("@is_use", BDP21_0100.is_use));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個BDP21_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="BDP21_0100">DTO</param>
        public void UpdateData(BDP21_0100 BDP21_0100)
        {
            string sSql = " UPDATE BDP21_0100  " +
                          "    SET code_code = @code_code, " +
                          "        field_code = @field_code, " +
                          "        field_name = @field_name," +
                          "        scr_no = @scr_no," + 
                          "        is_use = @is_use " +
                          "  WHERE bdp21_0100 = @bdp21_0100";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, BDP21_0100);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@code_code", BDP21_0100.code_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@field_code", BDP21_0100.field_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@field_name", BDP21_0100.field_name));
                //sqlCommand.Parameters.Add(new SqlParameter("@scr_no", BDP21_0100.scr_no.ToString()));
                //sqlCommand.Parameters.Add(new SqlParameter("@is_use", BDP21_0100.is_use));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM BDP21_0100 WHERE bdp21_0100 = @bdp21_0100";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { bdp21_0100 = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@bdp21_0100", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得BDP21_0100角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetBDP21_0100_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("bdp21_0100", System.Type.GetType("System.String"));
            dtDat.Columns.Add("code_code", System.Type.GetType("System.String"));
            dtDat.Columns.Add("field_code", System.Type.GetType("System.String"));

            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP21_0100";
            }
            else
            {
                sSql = "SELECT * FROM BDP21_0100 WHERE bdp21_0100='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["bdp21_0100"] = dtTmp.Rows[i]["bdp21_0100"];
                drow["code_code"] = dtTmp.Rows[i]["code_code"];
                drow["field_code"] = dtTmp.Rows[i]["field_code"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}