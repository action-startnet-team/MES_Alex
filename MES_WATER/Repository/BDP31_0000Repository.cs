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
    public class BDP31_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得BDP31_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO BDP31_0000</returns>
        public BDP31_0000 GetDTO(string pTkCode)
        {
            BDP31_0000 datas = new BDP31_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP31_0000";
            }
            else
            {
                sSql = "SELECT * FROM BDP31_0000 where select_code=@select_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@select_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new BDP31_0000
                        {
                            select_code = comm.sGetString(reader["select_code"].ToString()),
                            select_name = comm.sGetString(reader["select_name"].ToString()),
                            select_type = comm.sGetString(reader["select_type"].ToString()),
                            tsql_select = comm.sGetString(reader["tsql_select"].ToString()),
                            tsql_where = comm.sGetString(reader["tsql_where"].ToString()),
                            tsql_order = comm.sGetString(reader["tsql_order"].ToString()),

                        };
                    }
                }
            }
            return datas;
        }

        /// <summary>
        /// 取得查詢資料，結合使用者權限
        /// </summary>
        /// <param name="pUsrCode">使用者代碼</param>
        /// <param name="pPrgCode">功能代碼</param>
        /// <param name="pWhere">JSON查詢字串</param>
        /// <returns></returns>
        public List<BDP31_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<BDP31_0000> list = new List<BDP31_0000>();

            string sSql = "SELECT * FROM BDP31_0000";
            
            // 取得資料
            list = comm.Get_ListByQuery<BDP31_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個BDP31_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="BDP31_0000">DTO</param>
        public void InsertData(BDP31_0000 BDP31_0000)
        {

            string sSql = "INSERT INTO " +
                          " BDP31_0000 (  select_code,  select_name,  select_type,  tsql_select,  tsql_where,  tsql_order ) " +
                          "     VALUES ( @select_code, @select_name, @select_type, @tsql_select, @tsql_where, @tsql_order ) ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, BDP31_0000);
            }
        }

        /// <summary>
        /// 傳入一個BDP31_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="BDP31_0000">DTO</param>
        public void UpdateData(BDP31_0000 BDP31_0000)
        {
            string sSql = " UPDATE BDP31_0000 " +
                          "    SET select_name = @select_name,    " +
                          "        select_type = @select_type,    " +
                          "        tsql_select = @tsql_select,    " +
                          "        tsql_where  = @tsql_where,     " +
                          "        tsql_order  = @tsql_order      " +
                          "  WHERE select_code = @select_code     " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, BDP31_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM BDP31_0000 WHERE select_code = @select_code;";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { select_code = pTkCode });
            }
        }
        
    }
}