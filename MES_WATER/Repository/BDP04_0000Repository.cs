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
    public class BDP04_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得BDP04_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO BDP04_0000</returns>
        public BDP04_0000 GetDTO(string pTkCode)
        {
            BDP04_0000 datas = new BDP04_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP04_0000";
            }
            else
            {
                sSql = "SELECT * FROM BDP04_0000 WHERE prg_code = @prg_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@prg_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new BDP04_0000
                        {
                            prg_code = reader.GetString(reader.GetOrdinal("prg_code")),
                            prg_name = reader.GetString(reader.GetOrdinal("prg_name")),
                            sys_code = reader.GetString(reader.GetOrdinal("sys_code")),
                            is_use = reader.GetString(reader.GetOrdinal("is_use")),
                            limit_str = reader.GetString(reader.GetOrdinal("limit_str")),
                        };
                    }
                }
            }
            return datas;
        }

        /// <summary>
        /// 取得BDP04_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List BDP04_0000</returns>
        public List<BDP04_0000> Get_DataList(string pTkCode)
        {
            List<BDP04_0000> list = new List<BDP04_0000>();
            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP04_0000";
            }
            else
            {
                sSql = "SELECT * FROM BDP04_0000 WHERE prg_code = @prg_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@prg_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP04_0000 data = new BDP04_0000();
                    data.prg_code = reader["prg_code"].ToString();
                    data.prg_name = reader["prg_name"].ToString();
                    data.sys_code = reader["sys_code"].ToString();
                    data.is_use = reader["is_use"].ToString();
                    data.limit_str = reader["limit_str"].ToString();

                    list.Add(data);
                }

            }
            return list;
        }

        public List<BDP04_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {  
            //取得使用者程式授權
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);

            List<BDP04_0000> list = new List<BDP04_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = "SELECT * FROM BDP04_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP04_0000 data = new BDP04_0000();
                    data.prg_code = reader["prg_code"].ToString();
                    data.prg_name = reader["prg_name"].ToString();
                    data.sys_code = reader["sys_code"].ToString();
                    data.is_use = reader["is_use"].ToString();
                    data.limit_str = reader["limit_str"].ToString();


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
        /// 取得查詢資料，結合使用者權限
        /// </summary>
        /// <param name="pUsrCode">使用者代碼</param>
        /// <param name="pPrgCode">功能代碼</param>
        /// <param name="pWhere">JSON查詢字串</param>
        /// <returns></returns>
        public List<BDP04_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<BDP04_0000> list = new List<BDP04_0000>();

            string sSql = " SELECT BDP04_0000.* " +
                          " FROM BDP04_0000 ";
            //string sSql = " SELECT O.prg_code,A.menu_name AS prg_name, O.sys_code,O.is_use,O.limit_str FROM BDP04_0000 O " +
            //  "  LEFT JOIN BDP03_0000 A  ON A.prg_code = O.prg_code ";
            // 取得資料
            list = comm.Get_ListByQuery<BDP04_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個BDP04_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="BDP04_0000">DTO</param>
        public void InsertData(BDP04_0000 BDP04_0000)
        {
            string sSql = "INSERT INTO " +
                          " BDP04_0000  ( prg_code,  prg_name,  sys_code,  is_use,  limit_str )" +
                          "     VALUES  (@prg_code, @prg_name, @sys_code, @is_use, @limit_str) ";
            sSql += "; Update BDP03_0000 Set is_use=@is_use Where prg_code=@prg_code";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, BDP04_0000);
            }
        }

        /// <summary>
        /// 傳入一個BDP04_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="BDP04_0000">DTO</param>
        public void UpdateData(BDP04_0000 BDP04_0000)
        {
            string sSql = " UPDATE BDP04_0000 " +
                          "    SET prg_name = @prg_name , "+
                          "        sys_code = @sys_code , "+
                          "        is_use   = @is_use   , "+
                          "        limit_str= @limit_str  "+
                          "  WHERE prg_code = @prg_code";
            sSql += "; Update BDP03_0000 Set is_use=@is_use , menu_name=@prg_name Where prg_code=@prg_code";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {

                con_db.Execute(sSql, BDP04_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM BDP04_0000 WHERE prg_code = @prg_code";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { prg_code = pTkCode });
            }
        }

    }
}