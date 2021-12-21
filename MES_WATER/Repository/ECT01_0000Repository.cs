﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

using MES_WATER.Models;
using Dapper;


namespace MES_WATER.Repository
{
    public class ECT01_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得ECT01_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO ECT01_0000</returns>
        //public ECT01_0000 GetDTO(string pTkCode)
        //{
        //    ECT01_0000 datas = new ECT01_0000();

        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM ECT01_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM ECT01_0000 where CUSTOMER_CODE=@ECT01_0000";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@ECT01_0000", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                DateTime date = comm.sGetDateTime(comm.sGetString(reader["SETUP_DATE"].ToString()));
        //                datas = new ECT01_0000
        //                {
        //                    CUSTOMER_CODE = comm.sGetString(reader["CUSTOMER_CODE"].ToString()),
        //                    CUSTOMER_NAME = comm.sGetString(reader["CUSTOMER_NAME"].ToString()),
        //                    CUSTOMER_FULL_NAME = comm.sGetString(reader["CUSTOMER_FULL_NAME"].ToString()),
        //                    CUSTOMER_PROPERTY = comm.sGetString(reader["CUSTOMER_PROPERTY"].ToString()),
        //                    SETUP_DATE = comm.sGetDateTime(reader["SETUP_DATE"].ToString()),
        //                    SETUP_DATE2 = date.ToString("yyyy/MM/dd"),
        //                    REMARK = comm.sGetString(reader["REMARK"].ToString())
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
        public List<ECT01_0100> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<ECT01_0100> list = new List<ECT01_0100>();

            //string sSql = " SELECT ECT01_0000.*" +
            //            " FROM ECT01_0000 " ;
            string sSql =
              " SELECT  ECT01_0100.* from ECT01_0100 ";

            // 取得資料
            list = comm.Get_ListByQuery<ECT01_0100>(sSql, pWhere, pUsrCode, pPrgCode);

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

        public List<ECT01_0000> Get_DataListByQuery2(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<ECT01_0000> list = new List<ECT01_0000>();

            //string sSql = " SELECT ECT01_0000.*" +
            //            " FROM ECT01_0000 " ;
            string sSql =
              " SELECT  ECT01_0000.* from ECT01_0000";

            // 取得資料
            list = comm.Get_ListByQuery<ECT01_0000>(sSql, pWhere, pUsrCode, pPrgCode);


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
        /// 傳入一個ECT01_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="ECT01_0000">DTO</param>
        public void InsertData(ECT01_0000 ECT01_0000)
        {
            string sSql = "INSERT INTO " +
                          " ECT01_0000 (  ORDER_NO_ID,   ORDER_NO,  CUSTOMER_CODE,   CUSTOMER_NAME,  EDITION ,  ITEM_ID,  PACKING_QTY,  NEED_DELIVERY_DATE,  CUSTOMER_REMARK,  PRODUCTION_REMARK1,  PRODUCTION_REMARK2,  CHANGE_ROLE,  CHANGE_DATE )  " +
                          "     VALUES ( @ORDER_NO_ID,  @ORDER_NO, @CUSTOMER_CODE,  @CUSTOMER_NAME, @EDITION , @ITEM_ID, @PACKING_QTY, @NEED_DELIVERY_DATE, @CUSTOMER_REMARK, @PRODUCTION_REMARK1, @PRODUCTION_REMARK2, @CHANGE_ROLE, @CHANGE_DATE )  ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, ECT01_0000);
            }
        }

        public void InsertTempData(ECT01_0100 ECT01_0100)
        {
            string sSql = "INSERT INTO " +
                          " ECT01_0100 (  CUSTOMER_CODE,   ORDER_NO,  ITEM_ID,   ORDER_NUM,  PACKING_QTY ,  NEED_DELIVERY_DATE,  CUSTOMER_REMARK,  PRODUCTION_REMARK1,  PRODUCTION_REMARK2 )  " +
                          "     VALUES ( @CUSTOMER_CODE,  @ORDER_NO, @ITEM_ID,  @ORDER_NUM, @PACKING_QTY , @NEED_DELIVERY_DATE, @CUSTOMER_REMARK, @PRODUCTION_REMARK1, @PRODUCTION_REMARK2 )  ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, ECT01_0100);
            }
        }

        /// <summary>
        /// 傳入一個ECT01_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="ECT01_0000">DTO</param>
        public void UpdateData(ECT01_0000 ECT01_0000)
        {
            string sSql = " UPDATE ECT01_0000 " +
                          "    SET ORDER_NO  = @ORDER_NO,    " +
                          "        CUSTOMER_CODE  = @CUSTOMER_CODE,      " +
                          "        CUSTOMER_NAME  = @CUSTOMER_NAME,      " +
                          "        EDITION  = @EDITION,     " +
                          "        ITEM_ID  = @ITEM_ID,     " +
                          "        PACKING_QTY  = @PACKING_QTY,    " +
                          "        NEED_DELIVERY_DATE  = @NEED_DELIVERY_DATE,    " +
                          "        CUSTOMER_REMARK  = @CUSTOMER_REMARK,      " +
                          "        PRODUCTION_REMARK1  = @PRODUCTION_REMARK1,      " +
                          "        PRODUCTION_REMARK2  = @PRODUCTION_REMARK2,     " +
                          "        CHANGE_ROLE  = @CHANGE_ROLE,     " +
                          "        CHANGE_DATE  = @CHANGE_DATE      " +
                          "  WHERE ORDER_NO= @ORDER_NO ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, ECT01_0000);
            }
        }

        ///// <summary>
        ///// 傳入一個鍵值，刪除、一次刪除一筆
        ///// </summary>
        ///// <param name="pTkCode">資料鍵值</param>
        //public void DeleteData(string pTkCode)
        //{
        //    string sSql = "DELETE FROM ECT01_0000 WHERE CUSTOMER_CODE = @CUSTOMER_CODE;";
        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        con_db.Execute(sSql, new { CUSTOMER_CODE = pTkCode });
        //    }
        //}

        public void DeleteTempData()
        {
            string sSql = "DELETE FROM ECT01_0100;";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql);
            }
        }
    }
}