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
    public class ECB01_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得ECB01_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO ECB01_0000</returns>
        public ECB01_0000 GetDTO(string pTkCode)
        {
            ECB01_0000 datas = new ECB01_0000();

            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM ECB01_0000";
            }
            else
            {
                sSql = "SELECT * FROM ECB01_0000 where CUSTOMER_CODE=@ECB01_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@ECB01_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        DateTime date = comm.sGetDateTime(comm.sGetString(reader["SETUP_DATE"].ToString()));
                        datas = new ECB01_0000
                        {
                            CUSTOMER_CODE = comm.sGetString(reader["CUSTOMER_CODE"].ToString()),
                            CUSTOMER_NAME = comm.sGetString(reader["CUSTOMER_NAME"].ToString()),
                            CUSTOMER_FULL_NAME = comm.sGetString(reader["CUSTOMER_FULL_NAME"].ToString()),
                            CUSTOMER_PROPERTY = comm.sGetString(reader["CUSTOMER_PROPERTY"].ToString()),
                            SETUP_DATE = comm.sGetDateTime(reader["SETUP_DATE"].ToString()),
                            SETUP_DATE2 = date.ToString("yyyy/MM/dd"),
                            REMARK = comm.sGetString(reader["REMARK"].ToString())
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
        public List<ECB01_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<ECB01_0000> list = new List<ECB01_0000>();

            //string sSql = " SELECT ECB01_0000.*" +
            //            " FROM ECB01_0000 " ;
            string sSql =
              " SELECT  ECB01_0000.* from ECB01_0000";

            // 取得資料
            list = comm.Get_ListByQuery<ECB01_0000>(sSql, pWhere, pUsrCode, pPrgCode);

            // 權限設定
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            //string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_mtp_code", "par_name", "par_value");
            //var arr_LockGrpCode = sLockGrpCode.Split(',');

            for (int i = 0; i < list.Count; i++)
            {
                //檢查授權刪除、修改
                list[i].can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                list[i].can_update = sLimitStr.Contains("M") ? "Y" : "N";
            }

            return list;

        }

        /// <summary>
        /// 傳入一個ECB01_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="ECB01_0000">DTO</param>
        public void InsertData(ECB01_0000 ECB01_0000)
        {
            string sSql = "INSERT INTO " +
                          " ECB01_0000 (  CUSTOMER_CODE,   CUSTOMER_NAME,  CUSTOMER_FULL_NAME,   CUSTOMER_PROPERTY, SETUP_DATE ,REMARK )  " +
                          "     VALUES ( @CUSTOMER_CODE,  @CUSTOMER_NAME, @CUSTOMER_FULL_NAME,  @CUSTOMER_PROPERTY,@SETUP_DATE2 ,@REMARK )  ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, ECB01_0000);
            }
        }

        /// <summary>
        /// 傳入一個ECB01_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="ECB01_0000">DTO</param>
        public void UpdateData(ECB01_0000 ECB01_0000)
        {
            string sSql = " UPDATE ECB01_0000 " +
                          "    SET CUSTOMER_CODE  = @CUSTOMER_CODE,    " +
                          "        CUSTOMER_NAME  = @CUSTOMER_NAME,    " +
                          "        CUSTOMER_FULL_NAME  = @CUSTOMER_FULL_NAME,      " +
                          "        CUSTOMER_PROPERTY  = @CUSTOMER_PROPERTY,      " +
                          "        SETUP_DATE  = @SETUP_DATE2,     " +
                          "        REMARK  = @REMARK      " +
                          "  WHERE CUSTOMER_CODE= @CUSTOMER_CODE ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, ECB01_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM ECB01_0000 WHERE CUSTOMER_CODE = @CUSTOMER_CODE;";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { CUSTOMER_CODE = pTkCode });
            }
        }

        public void DeleteData2()
        {
            string sSql = "DELETE FROM ECB01_0000";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql);
            }
        }

        public void InsertOriData()
        {
            string sql = "select * from CUSTOMER";

            DataTable dt2 = comm.Get_AlexDataTable(sql);

            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                string CUSTOMER_CODE = dt2.Rows[i]["CUSTOMER_CODE"].ToString();
                string CUSTOMER_NAME = dt2.Rows[i]["CUSTOMER_NAME"].ToString();
                string CUSTOMER_FULL_NAME = dt2.Rows[i]["CUSTOMER_FULL_NAME"].ToString();
                string CUSTOMER_PROPERTY = "";
                DateTime SETUP_DATE = Convert.ToDateTime(dt2.Rows[i]["SETUP_DATE"].ToString());
                string REMARK = dt2.Rows[i]["REMARK"].ToString();

                string sSql = " INSERT INTO " +
                              " ECB01_0000 (  CUSTOMER_CODE,  CUSTOMER_NAME,  CUSTOMER_FULL_NAME ,   SETUP_DATE,   REMARK ) " +
                              "     VALUES ( @CUSTOMER_CODE, @CUSTOMER_NAME, @CUSTOMER_FULL_NAME ,  @SETUP_DATE,  @REMARK ) ";

                using (SqlConnection con_db = comm.Set_DBConnection())
                {
                    SqlCommand sqlCommand = new SqlCommand(sSql);
                    sqlCommand.Connection = con_db;
                    sqlCommand.Parameters.Add(new SqlParameter("@CUSTOMER_CODE", CUSTOMER_CODE));
                    sqlCommand.Parameters.Add(new SqlParameter("@CUSTOMER_NAME", CUSTOMER_NAME));
                    sqlCommand.Parameters.Add(new SqlParameter("@CUSTOMER_FULL_NAME", CUSTOMER_FULL_NAME));
                    //sqlCommand.Parameters.Add(new SqlParameter("@CUSTOMER_PROPERTY", CUSTOMER_PROPERTY));
                    sqlCommand.Parameters.Add(new SqlParameter("@SETUP_DATE", SETUP_DATE));
                    sqlCommand.Parameters.Add(new SqlParameter("@REMARK", REMARK));
                    sqlCommand.ExecuteNonQuery();
                }
            }

        }
    }
}