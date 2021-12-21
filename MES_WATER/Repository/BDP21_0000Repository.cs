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
    public class BDP21_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得BDP21_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO BDP21_0000</returns>
        public BDP21_0000 GetDTO(string pTkCode)
        {
            BDP21_0000 datas = new BDP21_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP21_0000";
            }
            else
            {
                sSql = "SELECT * FROM BDP21_0000 where code_code=@code_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@code_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new BDP21_0000
                        {
                            code_code = reader.GetString(reader.GetOrdinal("code_code")),
                            code_name = reader.GetString(reader.GetOrdinal("code_name")),
                            cmemo = reader.GetString(reader.GetOrdinal("cmemo")),
                            show_type = reader.GetString(reader.GetOrdinal("show_type")),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得BDP21_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List BDP21_0000</returns>
        public List<BDP21_0000> Get_DataList(string pTkCode)
        {
            List<BDP21_0000> list = new List<BDP21_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP21_0000";
            }
            else
            {
                sSql = "SELECT * FROM BDP21_0000 where code_code=@code_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@code_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP21_0000 data = new BDP21_0000();
                    data.code_code = reader["code_code"].ToString();
                    data.code_name = reader["code_name"].ToString();
                    data.cmemo = reader["cmemo"].ToString();
                    data.show_type = reader["show_type"].ToString();
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
        public List<BDP21_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            List<BDP21_0000> list = new List<BDP21_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = "SELECT * FROM BDP21_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@code_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP21_0000 data = new BDP21_0000();
                    data.code_code = reader["code_code"].ToString();
                    data.code_name = reader["code_name"].ToString();
                    data.cmemo = reader["cmemo"].ToString();
                    data.show_type = reader["show_type"].ToString();
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
        #endregion

        /// <summary>
        /// 取得查詢資料，結合使用者權限
        /// </summary>
        /// <param name="pUsrCode">使用者代碼</param>
        /// <param name="pPrgCode">功能代碼</param>
        /// <param name="pWhere">JSON查詢字串</param>
        /// <returns></returns>
        public List<BDP21_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<BDP21_0000> list = new List<BDP21_0000>();

            string sSql = " SELECT BDP21_0000.* " +
                          " FROM BDP21_0000 ";

            // 取得資料
            list = comm.Get_ListByQuery<BDP21_0000>(sSql, pWhere, pUsrCode, pPrgCode);

            // 權限設定
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);

            for (int i = 0; i < list.Count; i++)
            {
                //檢查授權刪除、修改
                list[i].can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                list[i].can_update = sLimitStr.Contains("M") ? "Y" : "N";
            }

            return list;

        }

        /// <summary>
        /// 傳入一個BDP21_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="BDP21_0000">DTO</param>
        public void InsertData(BDP21_0000 BDP21_0000)
        {
            string sSql = "INSERT INTO " +
                          " BDP21_0000 ( code_code, code_name, cmemo, show_type) " +
                          "     VALUES (@code_code, @code_name, @cmemo, @show_type)";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {

                con_db.Execute(sSql, BDP21_0000);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@code_code", BDP21_0000.code_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@code_name", BDP21_0000.code_name));
                //sqlCommand.Parameters.Add(new SqlParameter("@cmemo", BDP21_0000.cmemo));
                //sqlCommand.Parameters.Add(new SqlParameter("@show_type", BDP21_0000.show_type));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個BDP21_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="BDP21_0000">DTO</param>
        public void UpdateData(BDP21_0000 BDP21_0000)
        {
            string sSql = " UPDATE BDP21_0000 " +
                          "    SET code_name = @code_name, " +
                          "        cmemo = @cmemo, " +
                          "        show_type = @show_type " +
                          "  WHERE code_code = @code_code ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, BDP21_0000);


                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@code_code", BDP21_0000.code_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@code_name", BDP21_0000.code_name));
                //sqlCommand.Parameters.Add(new SqlParameter("@cmemo", BDP21_0000.cmemo));
                //sqlCommand.Parameters.Add(new SqlParameter("@show_type", BDP21_0000.show_type));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，主檔/明細檔要一併刪除
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM BDP21_0000 WHERE code_code = @code_code;";
            sSql += "DELETE FROM BDP21_0100 WHERE code_code = @code_code;";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { code_code = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@code_code", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得BDP21_0000角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetBDP21_0000_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("code_code", System.Type.GetType("System.String"));
            dtDat.Columns.Add("code_name", System.Type.GetType("System.String"));
            dtDat.Columns.Add("show_type", System.Type.GetType("System.String"));

            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP21_0000";
            }
            else
            {
                sSql = "SELECT * FROM BDP21_0000 where code_code='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["code_code"] = dtTmp.Rows[i]["code_code"];
                drow["code_name"] = dtTmp.Rows[i]["code_name"];
                drow["show_type"] = dtTmp.Rows[i]["show_type"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}