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
    public class MET10_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MBA_E10資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MBA_E10</returns>
        //public MBA_E10 GetDTO(string pTkCode)
        //{
        //    MBA_E10 datas = new MBA_E10();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM MBA_E10";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM MBA_E10 where MBA_E10_ID=@MBA_E10_ID";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@MBA_E10_ID", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                datas = new MBA_E10
        //                {
        //                    //MBA_E10_ID = comm.sGetString(reader["MBA_E10_ID"].ToString()),
        //                    //PLANT_ID = comm.sGetString(reader["PLANT_ID"].ToString()),
        //                    PLANT_CODE = comm.sGetString(reader["PLANT_CODE"].ToString()),
        //                    PLANT_NAME = comm.sGetString(reader["PLANT_NAME"].ToString()),
        //                    //WORK_CENTER_ID = comm.sGetString(reader["WORK_CENTER_ID"].ToString()),
        //                    WORK_CENTER_CODE = comm.sGetString(reader["WORK_CENTER_CODE"].ToString()),
        //                    WORK_CENTER_NAME = comm.sGetString(reader["WORK_CENTER_NAME"].ToString()),
        //                    //OPERATION_ID = comm.sGetString(reader["OPERATION_ID"].ToString()),
        //                    OPERATION_CODE = comm.sGetString(reader["OPERATION_CODE"].ToString()),
        //                    OPERATION_NAME = comm.sGetString(reader["OPERATION_NAME"].ToString()),
        //                    //MO_ID = comm.sGetString(reader["MO_ID"].ToString()),
        //                    MO_DOC_NO = comm.sGetString(reader["MO_DOC_NO"].ToString()),
        //                    TRANSACTION_DATE = comm.sGetString(reader["TRANSACTION_DATE"].ToString()),
        //                    QTY = comm.sGetDecimal(reader["QTY"].ToString()),
        //                    SCRAP_QTY = comm.sGetDecimal(reader["SCRAP_QTY"].ToString()),
        //                    //XOPERATOR_ID = comm.sGetString(reader["XOPERATOR_ID"].ToString()),
        //                    XOPERATOR_CODE = comm.sGetString(reader["XOPERATOR_CODE"].ToString()),
        //                    XOPERATOR_NAME = comm.sGetString(reader["XOPERATOR_NAME"].ToString()),
        //                    //XMACHINE_ID = comm.sGetString(reader["XMACHINE_ID"].ToString()),
        //                    XMACHINE_CODE = comm.sGetString(reader["XMACHINE_CODE"].ToString()),
        //                    XMACHINE_NAME = comm.sGetString(reader["XMACHINE_NAME"].ToString()),
        //                    STATUS = comm.sGetString(reader["STATUS"].ToString()),
        //                    ERRMSG = comm.sGetString(reader["ERRMSG"].ToString()),
        //                };
        //            }
        //        }
        //    }
        //    return datas;
        //}

        //#region
        ///// <summary>
        ///// 取得MBA_E10資料表內容
        ///// </summary>
        ///// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        ///// < returns > List MBA_E10</returns>
        //public List<MBA_E10> Get_DataList(string pTkCode)
        //{
        //    List<MBA_E10> list = new List<MBA_E10>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM MBA_E10";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM MBA_E10 where MBA_E10_ID=@MBA_E10_ID";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@MBA_E10_ID", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            MBA_E10 data = new MBA_E10();

        //                    //data.MBA_E10_ID = comm.sGetString(reader["MBA_E10_ID"].ToString());
        //                    //data.PLANT_ID = comm.sGetString(reader["PLANT_ID"].ToString());
        //                    data.PLANT_CODE = comm.sGetString(reader["PLANT_CODE"].ToString());
        //                    data.PLANT_NAME = comm.sGetString(reader["PLANT_NAME"].ToString());
        //                    //data.WORK_CENTER_ID = comm.sGetString(reader["WORK_CENTER_ID"].ToString());
        //                    data.WORK_CENTER_CODE = comm.sGetString(reader["WORK_CENTER_CODE"].ToString());
        //                    data.WORK_CENTER_NAME = comm.sGetString(reader["WORK_CENTER_NAME"].ToString());
        //                    //data.OPERATION_ID = comm.sGetString(reader["OPERATION_ID"].ToString());
        //                    data.OPERATION_CODE = comm.sGetString(reader["OPERATION_CODE"].ToString());
        //                    data.OPERATION_NAME = comm.sGetString(reader["OPERATION_NAME"].ToString());
        //                    //data.MO_ID = comm.sGetString(reader["MO_ID"].ToString());
        //                    data.MO_DOC_NO = comm.sGetString(reader["MO_DOC_NO"].ToString());
        //                    data.TRANSACTION_DATE = comm.sGetString(reader["TRANSACTION_DATE"].ToString());
        //                    data.QTY = comm.sGetDecimal(reader["QTY"].ToString());
        //                    data.SCRAP_QTY = comm.sGetDecimal(reader["SCRAP_QTY"].ToString());
        //                    //data.XOPERATOR_ID = comm.sGetString(reader["XOPERATOR_ID"].ToString());
        //                    data.XOPERATOR_CODE = comm.sGetString(reader["XOPERATOR_CODE"].ToString());
        //                    data.XOPERATOR_NAME = comm.sGetString(reader["XOPERATOR_NAME"].ToString());
        //                    //data.XMACHINE_ID = comm.sGetString(reader["XMACHINE_ID"].ToString());
        //                    data.XMACHINE_CODE = comm.sGetString(reader["XMACHINE_CODE"].ToString());
        //                    data.XMACHINE_NAME = comm.sGetString(reader["XMACHINE_NAME"].ToString());
        //                    data.STATUS = comm.sGetString(reader["STATUS"].ToString());
        //                    data.ERRMSG = comm.sGetString(reader["ERRMSG"].ToString());


        //            data.can_delete = "Y";
        //            data.can_update = "Y";
        //            list.Add(data);
        //        }

        //    }
        //    return list;
        //}

        /// <summary>
        /// 取得使用者可以編輯的資料，結合商務邏輯權限
        /// </summary>
        /// <param name="pUsrCode"></param>
        /// <param name="pPrgCode"></param>
        /// <returns></returns>
        //public List<MBA_E10> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_MBA_E10_ID", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<MBA_E10> list = new List<MBA_E10>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料

        //    sSql = "SELECT * FROM MBA_E10";


        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        //sqlCommand.Parameters.Add(new SqlParameter("@MBA_E10_ID", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            MBA_E10 data = new MBA_E10();

        //            //data.MBA_E10_ID = comm.sGetString(reader["MBA_E10_ID"].ToString());
        //            //data.PLANT_ID = comm.sGetString(reader["PLANT_ID"].ToString());
        //            data.PLANT_CODE = comm.sGetString(reader["PLANT_CODE"].ToString());
        //            data.PLANT_NAME = comm.sGetString(reader["PLANT_NAME"].ToString());
        //            //data.WORK_CENTER_ID = comm.sGetString(reader["WORK_CENTER_ID"].ToString());
        //            data.WORK_CENTER_CODE = comm.sGetString(reader["WORK_CENTER_CODE"].ToString());
        //            data.WORK_CENTER_NAME = comm.sGetString(reader["WORK_CENTER_NAME"].ToString());
        //            //data.OPERATION_ID = comm.sGetString(reader["OPERATION_ID"].ToString());
        //            data.OPERATION_CODE = comm.sGetString(reader["OPERATION_CODE"].ToString());
        //            data.OPERATION_NAME = comm.sGetString(reader["OPERATION_NAME"].ToString());
        //            //data.MO_ID = comm.sGetString(reader["MO_ID"].ToString());
        //            data.MO_DOC_NO = comm.sGetString(reader["MO_DOC_NO"].ToString());
        //            data.TRANSACTION_DATE = comm.sGetString(reader["TRANSACTION_DATE"].ToString());
        //            data.QTY = comm.sGetDecimal(reader["QTY"].ToString());
        //            data.SCRAP_QTY = comm.sGetDecimal(reader["SCRAP_QTY"].ToString());
        //            //data.XOPERATOR_ID = comm.sGetString(reader["XOPERATOR_ID"].ToString());
        //            data.XOPERATOR_CODE = comm.sGetString(reader["XOPERATOR_CODE"].ToString());
        //            data.XOPERATOR_NAME = comm.sGetString(reader["XOPERATOR_NAME"].ToString());
        //            //data.XMACHINE_ID = comm.sGetString(reader["XMACHINE_ID"].ToString());
        //            data.XMACHINE_CODE = comm.sGetString(reader["XMACHINE_CODE"].ToString());
        //            data.XMACHINE_NAME = comm.sGetString(reader["XMACHINE_NAME"].ToString());
        //            data.STATUS = comm.sGetString(reader["STATUS"].ToString());
        //            data.ERRMSG = comm.sGetString(reader["ERRMSG"].ToString());

        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            //資料邏輯刪除、修改
        //            //if (arr_LockGrpCode.Contains(data.MBA_E10_ID)) {
        //            //    data.can_delete = "N";
        //            //    data.can_update = "N";
        //            //}

        //            list.Add(data);
        //        }
        //    }
        //    return list;
        //}
        //#endregion

        /// <summary>
        /// 取得查詢資料，結合使用者權限
        /// </summary>
        /// <param name="pUsrCode">使用者代碼</param>
        /// <param name="pPrgCode">功能代碼</param>
        /// <param name="pWhere">JSON查詢字串</param>
        /// <returns></returns>
        public List<MBA_E10> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MBA_E10> list = new List<MBA_E10>();

            //string sSql = "SELECT PLANT_CODE,  PLANT_NAME,   WORK_CENTER_CODE," +
            //              "WORK_CENTER_NAME,  OPERATION_CODE ,  OPERATION_NAME,   MO_DOC_NO,  TRANSACTION_DATE, " +
            //              "QTY,  SCRAP_QTY,   XOPERATOR_CODE,  XOPERATOR_NAME,   XMACHINE_CODE,  XMACHINE_NAME, STATUS, ERRMSG FROM MBA_E10 ";

            string sSql = "SELECT top 2000 * FROM MBA_E10  WITH ( NOLOCK )";
            // 取得資料
            list = comm.Get_ListByQuery<MBA_E10>(sSql, pWhere, pUsrCode, pPrgCode, "TRANSACTION_DATE desc");

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
        /// 傳入一個MBA_E10的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MBA_E10">DTO</param>
        public void InsertData(MBA_E10 MBA_E10)
        {
            string sSql = "INSERT INTO " +
                          " MBA_E10 (  PLANT_CODE,  PLANT_NAME,   WORK_CENTER_CODE," +
                          "WORK_CENTER_NAME,  OPERATION_CODE ,  OPERATION_NAME,   MO_DOC_NO,  TRANSACTION_DATE, " +
                          "QTY,  SCRAP_QTY,   XOPERATOR_CODE,  XOPERATOR_NAME,   XMACHINE_CODE,  XMACHINE_NAME, STATUS, ERRMSG)" +
                          "     VALUES (   @PLANT_CODE,  @PLANT_NAME,  @WORK_CENTER_CODE," +
                          "@WORK_CENTER_NAME,  @OPERATION_CODE ,  @OPERATION_NAME,  @MO_DOC_NO,  @TRANSACTION_DATE ," +
                          "@QTY,  @SCRAP_QTY,  @XOPERATOR_CODE,  @XOPERATOR_NAME,   @XMACHINE_CODE,  @XMACHINE_NAME, @STATUS, @ERRMSG)";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MBA_E10);
            }
        }

        /// <summary>
        /// 傳入一個MBA_E10的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MBA_E10">DTO</param>
        //public void UpdateData(MBA_E10 MBA_E10)
        //{
        //    string sSql = " UPDATE MBA_E10 " +
        //                  "    SET plan_out_date  =  @plan_out_date, " +
        //                  "        sor_code   =  @sor_code,  " +
        //                  "        cus_name  =  @cus_name, " +
        //                  "        pro_code  =  @pro_code, " +
        //                  "        pro_name   =  @pro_name,  " +
        //                  "        pro_spec  =  @pro_spec, " +
        //                  "        spec_a  =  @spec_a, " +
        //                  "        spec_b   =  @spec_b,  " +
        //                  "        spec_c  =  @spec_c, " +
        //                  "        plan_qty  =  @plan_qty, " +
        //                  "        mo_qty   =  @mo_qty,  " +
        //                  "        mo_status =  @mo_status         " +
        //                  "  WHERE MBA_E10_ID  =  @MBA_E10_ID  ";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        con_db.Execute(sSql, MBA_E10);
        //    }
        //}

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MBA_E10 WHERE MBA_E10_ID = @MBA_E10_ID ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { MBA_E10_ID = pTkCode });
            }
        }


    }
}