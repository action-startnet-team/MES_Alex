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
    public class ECT02_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得ECT02_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO ECT02_0000</returns>
        //public ECT02_0000 GetDTO(string pTkCode)
        //{
        //    ECT02_0000 datas = new ECT02_0000();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM ECT02_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM ECT02_0000 where SALES_CUSTOMER_CODE_EDITION=@SALES_CUSTOMER_CODE_EDITION";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@SALES_CUSTOMER_CODE_EDITION", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                datas = new ECT02_0000
        //                {
        //                    SALES_CUSTOMER_CODE_EDITION = comm.sGetString(reader["SALES_CUSTOMER_CODE_EDITION"].ToString()),
        //                    CUSTOMER_CODE = comm.sGetString(reader["CUSTOMER_CODE"].ToString()),
        //                    EDITION = comm.sGetString(reader["EDITION"].ToString()),
        //                };
        //            }
        //        }
        //    }
        //    return datas;
        //}

        /// <summary>
        /// 取得查詢資料，結合使用者權限
        /// </summary>
        /// <param name="pUsrCode">使用者代碼</param>
        /// <param name="pPrgCode">功能代碼</param>
        /// <param name="pWhere">JSON查詢字串</param>
        /// <returns></returns>
        public List<ECT02_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<ECT02_0000> list = new List<ECT02_0000>();

            string sSql = " SELECT * from ECT02_0000 ";
            //" left join ECT02_0100 on ECT02_0000.SALES_CUSTOMER_CODE_EDITION = ECT02_0100.SALES_CUSTOMER_CODE_EDITION ";
            // 取得資料
            list = comm.Get_ListByQuery<ECT02_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個ECT02_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="ECT02_0000">DTO</param>
        public void InsertData(ECT02_0000 ECT02_0000)
        {
            string sSql = "INSERT INTO " +
                          " ECT02_0000 (  SALES_ORDER_NO_ID,  SALES_CUSTOMER_CODE_EDITION,  SALES_ORDER_NO ) " +
                          "     VALUES ( @SALES_ORDER_NO_ID, @SALES_CUSTOMER_CODE_EDITION, @SALES_ORDER_NO ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, ECT02_0000);
            }
        }

        /// <summary>
        /// 傳入一個ECT02_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="ECT02_0000">DTO</param>
        public void UpdateData(ECT02_0000 ECT02_0000)
        {
            string sSql = " UPDATE ECT02_0000                    " +
                          "    SET CUSTOMER_CODE    = @CUSTOMER_CODE,    " +
                          "        EDITION = @EDITION " +
                          "  WHERE SALES_CUSTOMER_CODE_EDITION     = @SALES_CUSTOMER_CODE_EDITION      ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, ECT02_0000);

            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM ECT02_0000 WHERE SALES_CUSTOMER_CODE_EDITION = @SALES_CUSTOMER_CODE_EDITION ";
            sSql += " DELETE FROM ECT02_0100 WHERE SALES_CUSTOMER_CODE_EDITION = @SALES_CUSTOMER_CODE_EDITION ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { SALES_CUSTOMER_CODE_EDITION = pTkCode });

            }
        }
        public void DeleteData2(string pTkCode)
        {
            string sSql = " DELETE FROM ECT02_0000 WHERE SALES_ORDER_NO = @SALES_ORDER_NO ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { SALES_ORDER_NO = pTkCode });

            }
        }
    }
}