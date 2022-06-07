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
    public class ECB05_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得ECB05_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO ECB05_0000</returns>
        public ECB05_0000 GetDTO(string pTkCode)
        {
            ECB05_0000 datas = new ECB05_0000();

            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM ECB05_0000";
            }
            else
            {
                sSql = "SELECT * FROM ECB05_0000 where ERP_FIELD_CODE=@ERP_FIELD_CODE";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@ERP_FIELD_CODE", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new ECB05_0000
                        {
                            ERP_FIELD_CODE = comm.sGetString(reader["ERP_FIELD_CODE"].ToString()),
                            ERP_FIELD_NAME = comm.sGetString(reader["ERP_FIELD_NAME"].ToString()),
                            ERP_FORM_NAME = comm.sGetString(reader["ERP_FORM_NAME"].ToString()),
                            ERP_FIELD_VALUE = comm.sGetString(reader["ERP_FIELD_VALUE"].ToString()),
                            is_edit = comm.sGetString(reader["is_edit"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

       #region //
        /// <summary>
        /// 取得ECB05_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List ECB05_0000</returns>
      //  public List<ECB05_0000> Get_DataList(string pTkCode)
      //  {
      //      List<ECB05_0000> list = new List<ECB05_0000>();
      //      string sSql = "";
      //
      //      if (string.IsNullOrEmpty(pTkCode))
      //      {
      //          sSql = "SELECT * FROM ECB05_0000";
      //      }
      //      else
      //      {
      //          sSql = "SELECT * FROM ECB05_0000 where ECB05_0000=@ECB05_0000";
      //      }
      //
      //
      //      using (SqlConnection con_db = comm.Set_DBConnection())
      //      {
      //          SqlCommand sqlCommand = new SqlCommand(sSql);
      //          sqlCommand.Connection = con_db;
      //          sqlCommand.Parameters.Add(new SqlParameter("@ECB05_0000", pTkCode));
      //          SqlDataReader reader = sqlCommand.ExecuteReader();
      //
      //          while (reader.Read())
      //          {
      //              ECB05_0000 data = new ECB05_0000();
      //
      //              data.ECB05_0000 = comm.sGetInt32(reader["ECB05_0000"].ToString());
      //              data.alm_code = comm.sGetString(reader["alm_code"].ToString());
      //              data.alm_name = comm.sGetString(reader["alm_name"].ToString());
      //              data.is_use = comm.sGetString(reader["is_use"].ToString());
      //              data.alm_table = comm.sGetString(reader["alm_table"].ToString());
      //              data.alm_field = comm.sGetString(reader["alm_field"].ToString());
      //              data.alm_type = comm.sGetString(reader["alm_type"].ToString());
      //              data.upper_limit = comm.sGetfloat(reader["upper_limit"].ToString());
      //              data.lower_limit = comm.sGetfloat(reader["lower_limit"].ToString());
      //              data.alm_formula = comm.sGetString(reader["alm_formula"].ToString());
      //
      //              data.can_delete = "Y";
      //              data.can_update = "Y";
      //              list.Add(data);
      //          }
      //
      //      }
      //      return list;
      //  }

        ///// <summary>
        ///// 取得使用者可以編輯的資料，結合商務邏輯權限
        ///// </summary>
        ///// <param name="pUsrCode"></param>
        ///// <param name="pPrgCode"></param>
        ///// <returns></returns>
        //public List<ECB05_0000> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_ECB05_0000", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<ECB05_0000> list = new List<ECB05_0000>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    //sSql = "SELECT * FROM ECB05_0000";
        //    sSql = "SELECT * FROM ECB05_0000";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        //sqlCommand.Parameters.Add(new SqlParameter("@ECB05_0000", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            ECB05_0000 data = new ECB05_0000();

        //            data.ECB05_0000 = comm.sGetInt32(reader["ECB05_0000"].ToString());
        //            data.alm_code = comm.sGetString(reader["alm_code"].ToString());
        //            data.alm_name = comm.sGetString(reader["alm_name"].ToString());
        //            data.is_use = comm.sGetString(reader["is_use"].ToString());
        //            data.alm_table = comm.sGetString(reader["alm_table"].ToString());


        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            //資料邏輯刪除、修改
        //            //if (arr_LockGrpCode.Contains(data.ECB05_0000)) {
        //            //    data.can_delete = "N";
        //            //    data.can_update = "N";
        //            //}

        //            list.Add(data);
        //        }
        //    }
        //    return list;
        //}
        #endregion

        /// <summary>
        /// 取得查詢資料，結合使用者權限
        /// </summary>
        /// <param name="pUsrCode">使用者代碼</param>
        /// <param name="pPrgCode">功能代碼</param>
        /// <param name="pWhere">JSON查詢字串</param>
        /// <returns></returns>
        public List<ECB05_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<ECB05_0000> list = new List<ECB05_0000>();

            string sSql = @" SELECT case ECB05_0000.ERP_FIELD_CODE
                            when 'DELEVERY_ORDER'   then ERP_FIELD_CODE+'  此欄位必須存在' ELSE ERP_FIELD_CODE END as ERP_FIELD_CODE
                            ,ERP_FIELD_NAME,ERP_FORM_NAME,ERP_FIELD_VALUE,is_edit 
                            from ECB05_0000";

            // 取得資料
            list = comm.Get_ListByQuery<ECB05_0000>(sSql, pWhere, pUsrCode, pPrgCode);

            // 權限設定
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);

            for (int i = 0; i < list.Count; i++)
            {
                //檢查授權刪除、修改
                if(list[i].ERP_FIELD_CODE == "DELEVERY_ORDER  此欄位必須存在") { list[i].can_delete = sLimitStr.Contains("D") ? "N" : "N"; }
                else 
                {
                    list[i].can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                }
                list[i].can_update = sLimitStr.Contains("M") ? "Y" : "N";

            }

            return list;

        }

        /// <summary>
        /// 傳入一個ECB05_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="ECB05_0000">DTO</param>
        public void InsertData(ECB05_0000 ECB05_0000)
        {
            string sSql = "INSERT INTO " +
                          " ECB05_0000 (  ERP_FIELD_CODE,   ERP_FIELD_NAME, ERP_FORM_NAME, ERP_FIELD_VALUE, is_edit )  " +
                          "     VALUES ( @ERP_FIELD_CODE,  @ERP_FIELD_NAME, @ERP_FORM_NAME, @ERP_FIELD_VALUE, @is_edit )  ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, ECB05_0000);
            }
        }

        /// <summary>
        /// 傳入一個ECB05_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="ECB05_0000">DTO</param>
        public void UpdateData(ECB05_0000 ECB05_0000)
        {
            string sSql = " UPDATE ECB05_0000 " +
                          "    SET ERP_FIELD_NAME  = @ERP_FIELD_NAME,    " +
                          "        ERP_FORM_NAME  = @ERP_FORM_NAME,    " +
                        "        ERP_FIELD_VALUE  = @ERP_FIELD_VALUE,    " +
                          "        is_edit  = @is_edit    " +
                          "  WHERE ERP_FIELD_CODE= @ERP_FIELD_CODE ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, ECB05_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM ECB05_0000 WHERE ERP_FIELD_CODE = @ERP_FIELD_CODE;";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { ERP_FIELD_CODE = pTkCode });
            }
        }
    }
}