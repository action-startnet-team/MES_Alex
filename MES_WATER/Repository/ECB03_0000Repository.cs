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
    public class ECB03_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得ECB03_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO ECB03_0000</returns>
        public ECB03_0000 GetDTO(string pTkCode)
        {
            ECB03_0000 datas = new ECB03_0000();

            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM ECB03_0000";
            }
            else
            {
                sSql = "SELECT * FROM ECB03_0000 where CUSTOMER_CODE=@ECB03_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@ECB03_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new ECB03_0000
                        {
                            CUSTOMER_CODE = comm.sGetString(reader["CUSTOMER_CODE"].ToString()),
                            CUSTOMER_NAME = comm.sGetString(reader["CUSTOMER_NAME"].ToString()),
                            CUSTOMER_FULL_NAME = comm.sGetString(reader["CUSTOMER_FULL_NAME"].ToString()),
                            CUSTOMER_PROPERTY = comm.sGetString(reader["CUSTOMER_PROPERTY"].ToString()),
                            CURRENCY_ID = comm.sGetString(reader["CURRENCY_ID"].ToString()),
                            DELIVERY_TERM_ID = comm.sGetString(reader["DELIVERY_TERM_ID"].ToString()),
                            ADDRESS = comm.sGetString(reader["ADDRESS"].ToString()),
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
        public List<ECB03_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<ECB03_0000> list = new List<ECB03_0000>();

            //string sSql = " SELECT ECB03_0000.*" +
            //            " FROM ECB03_0000 " ;
            string sSql =
              " SELECT  ECB03_0000.* from ECB03_0000";

            // 取得資料
            list = comm.Get_ListByQuery<ECB03_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個ECB03_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="ECB03_0000">DTO</param>
        public void InsertData(ECB03_0000 ECB03_0000)
        {
            string sSql = "INSERT INTO " +
                          " ECB03_0000 (  CUSTOMER_CODE,   CUSTOMER_NAME,  CUSTOMER_FULL_NAME,   CUSTOMER_PROPERTY, CURRENCY_ID ,DELIVERY_TERM_ID, ADDRESS, REMARK )  " +
                          "     VALUES ( @CUSTOMER_CODE,  @CUSTOMER_NAME, @CUSTOMER_FULL_NAME,  @CUSTOMER_PROPERTY,@CURRENCY_ID ,@DELIVERY_TERM_ID, @ADDRESS, @REMARK )  ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, ECB03_0000);
            }
        }

        /// <summary>
        /// 傳入一個ECB03_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="ECB03_0000">DTO</param>
        public void UpdateData(ECB03_0000 ECB03_0000)
        {
            string sSql = " UPDATE ECB03_0000 " +
                          "    SET CUSTOMER_CODE  = @CUSTOMER_CODE,    " +
                          "        CUSTOMER_NAME  = @CUSTOMER_NAME,    " +
                          "        CUSTOMER_FULL_NAME  = @CUSTOMER_FULL_NAME,      " +
                          "        CUSTOMER_PROPERTY  = @CUSTOMER_PROPERTY,      " +
                          "        CURRENCY_ID  = @CURRENCY_ID,     " +
                          "        DELIVERY_TERM_ID  = @DELIVERY_TERM_ID,      " +
                          "        ADDRESS  = @ADDRESS,      " +
                          "        REMARK  = @REMARK      " +
                          "  WHERE CUSTOMER_CODE= @CUSTOMER_CODE ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, ECB03_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM ECB03_0000 WHERE CUSTOMER_CODE = @CUSTOMER_CODE;";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { CUSTOMER_CODE = pTkCode });
            }
        }
    }
}