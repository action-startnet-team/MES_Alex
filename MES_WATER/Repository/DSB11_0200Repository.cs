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
    public class DSB11_0200Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得ECB02_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO ECB02_0000</returns>
        //public DSB11_0200 GetDTO(string pTkCode)
        //{
        //    DSB11_0200 datas = new DSB11_0200();

        //    string sSql = "";

        //    //if (string.IsNullOrEmpty(pTkCode))
        //    //{
        //    //    sSql = "SELECT * FROM ECB02_0000";
        //    //}
        //    //else
        //    //{
        //    //    sSql = "SELECT * FROM ECB02_0000 where CUSTOMER_CODE_EDITION=@ECB02_0000";
        //    //}

        //    //using (SqlConnection con_db = comm.Set_DBConnection())
        //    //{
        //    //    SqlCommand sqlCommand = new SqlCommand(sSql);
        //    //    sqlCommand.Connection = con_db;
        //    //    sqlCommand.Parameters.Add(new SqlParameter("@ECB02_0000", pTkCode));
        //    //    SqlDataReader reader = sqlCommand.ExecuteReader();
        //    //    if (reader.HasRows)
        //    //    {
        //    //        while (reader.Read())
        //    //        {
        //    //            DateTime date = comm.sGetDateTime(comm.sGetString(reader["SETUP_DATE"].ToString()));
        //    //            datas = new ECB02_0000
        //    //            {
        //    //                CUSTOMER_CODE_EDITION = comm.sGetString(reader["CUSTOMER_CODE_EDITION"].ToString()),
        //    //                CUSTOMER_CODE = comm.sGetString(reader["CUSTOMER_CODE"].ToString()),
        //    //                CUSTOMER_NAME = comm.sGetString(reader["CUSTOMER_NAME"].ToString()),
        //    //                EDITION = comm.sGetString(reader["EDITION"].ToString()),
        //    //                ORDER_NO_MAPPING = comm.sGetString(reader["ORDER_NO_MAPPING"].ToString()),
        //    //                ITEM_ID_MAPPING = comm.sGetString(reader["ITEM_ID_MAPPING"].ToString()),
        //    //                PACKING_QTY_MAPPING = comm.sGetString(reader["PACKING_QTY_MAPPING"].ToString()),
        //    //                NEED_DELIVERY_DATE_MAPPING = comm.sGetString(reader["NEED_DELIVERY_DATE_MAPPING"].ToString()),
        //    //                CUSTOMER_REMARK = comm.sGetString(reader["CUSTOMER_REMARK"].ToString()),
        //    //                PRODUCTION_REMARK1 = comm.sGetString(reader["PRODUCTION_REMARK1"].ToString()),
        //    //                PRODUCTION_REMARK2 = comm.sGetString(reader["PRODUCTION_REMARK2"].ToString()),
        //    //                SETUP_ROLE = comm.sGetString(reader["SETUP_ROLE"].ToString()),
        //    //                SETUP_DATE = comm.sGetDateTime(reader["SETUP_DATE"].ToString()),
        //    //                SETUP_DATE2 = date.ToString("yyyy/MM/dd"),
        //    //                ORDER_DETAIL_NO = comm.sGetString(reader["ORDER_DETAIL_NO"].ToString())
        //    //            };
        //    //        }
        //    //    }
        //    //}
        //    //return datas;
        //}

       

        /// <summary>
        /// 取得查詢資料，結合使用者權限
        /// </summary>
        /// <param name="pUsrCode">使用者代碼</param>
        /// <param name="pPrgCode">功能代碼</param>
        /// <param name="pWhere">JSON查詢字串</param>
        /// <returns></returns>
        public List<ECB02_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<ECB02_0000> list = new List<ECB02_0000>();

            //string sSql = " SELECT ECB02_0000.*" +
            //            " FROM ECB02_0000 " ;
            string sSql =
              "SELECT * From ECB02_0000";

            // 取得資料
            list = comm.Get_ListByQuery<ECB02_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個ECB02_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="DSB11_0200">DTO</param>
        public void InsertData(DSB11_0200 DSB11_0200)
        {
            string sSql = "INSERT INTO " +
                          " ECB02_0000 (CUSTOMER_CODE_EDITION,  CUSTOMER_CODE,   CUSTOMER_NAME,  EDITION, ORDER_NO_MAPPING, ITEM_ID_MAPPING, PACKING_QTY_MAPPING, NEED_DELIVERY_DATE_MAPPING, CUSTOMER_REMARK, PRODUCTION_REMARK1, PRODUCTION_REMARK2, SETUP_ROLE, SETUP_DATE, ORDER_DETAIL_NO)  " +
                          "     VALUES ( @CUSTOMER_CODE_EDITION,  @CUSTOMER_CODE,   @CUSTOMER_NAME,  @EDITION, @ORDER_NO_MAPPING, @ITEM_ID_MAPPING, @PACKING_QTY_MAPPING, @NEED_DELIVERY_DATE_MAPPING, @CUSTOMER_REMARK, @PRODUCTION_REMARK1, @PRODUCTION_REMARK2, @SETUP_ROLE, @SETUP_DATE2, @ORDER_DETAIL_NO)  ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, DSB11_0200);
            }
        }

        /// <summary>
        /// 傳入一個ECB02_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="DSB11_0200">DTO</param>
        public void UpdateData(DSB11_0200 DSB11_0200)
        {
            string sSql = " UPDATE ECB02_0000 " +
                          "    SET CUSTOMER_CODE  = @CUSTOMER_CODE,    " +
                          "        CUSTOMER_NAME  = @CUSTOMER_NAME,    " +
                          "        EDITION  = @EDITION,      " +
                          "        ORDER_NO_MAPPING  = @ORDER_NO_MAPPING,      " +
                          "        ITEM_ID_MAPPING  = @ITEM_ID_MAPPING,     " +
                          "        PACKING_QTY_MAPPING  = @PACKING_QTY_MAPPING,      " +
                          "        NEED_DELIVERY_DATE_MAPPING  = @NEED_DELIVERY_DATE_MAPPING,    " +
                          "        CUSTOMER_REMARK  = @CUSTOMER_REMARK,      " +
                          "        PRODUCTION_REMARK1  = @PRODUCTION_REMARK1,      " +
                          "        PRODUCTION_REMARK2  = @PRODUCTION_REMARK2,     " +
                          "        SETUP_ROLE  = @SETUP_ROLE ,     " +
                          "        SETUP_DATE  = @SETUP_DATE2,    " +
                          "        ORDER_DETAIL_NO  = @ORDER_DETAIL_NO    " +
                          "  WHERE CUSTOMER_CODE_EDITION= @CUSTOMER_CODE_EDITION ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, DSB11_0200);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM ECB02_0000 WHERE CUSTOMER_CODE_EDITION = @CUSTOMER_CODE_EDITION;";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { CUSTOMER_CODE_EDITION = pTkCode });
            }
        }
    }
}