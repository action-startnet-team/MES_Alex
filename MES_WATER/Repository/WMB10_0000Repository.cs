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
    public class WMB10_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得WMB10_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO WMB10_0000</returns>
        public WMB10_0000 GetDTO(string pTkCode)
        {
            WMB10_0000 datas = new WMB10_0000();

            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM WMB10_0000";
            }
            else
            {
                sSql = "SELECT * FROM WMB10_0000 where sup_code=@sup_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@sup_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new WMB10_0000
                        {
                            sup_code = comm.sGetString(reader["sup_code"].ToString()),
                            sup_name = comm.sGetString(reader["sup_name"].ToString()),
                            sup_scode = comm.sGetString(reader["sup_scode"].ToString()),
                            is_use = comm.sGetString(reader["is_use"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得WMB10_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List WMB10_0000</returns>
        //  public List<WMB10_0000> Get_DataList(string pTkCode)
        //  {
        //      List<WMB10_0000> list = new List<WMB10_0000>();
        //      string sSql = "";
        //
        //      if (string.IsNullOrEmpty(pTkCode))
        //      {
        //          sSql = "SELECT * FROM WMB10_0000";
        //      }
        //      else
        //      {
        //          sSql = "SELECT * FROM WMB10_0000 where sup_code=@sup_code";
        //      }
        //
        //
        //      using (SqlConnection con_db = comm.Set_DBConnection())
        //      {
        //          SqlCommand sqlCommand = new SqlCommand(sSql);
        //          sqlCommand.Connection = con_db;
        //          sqlCommand.Parameters.Add(new SqlParameter("@sup_code", pTkCode));
        //          SqlDataReader reader = sqlCommand.ExecuteReader();
        //
        //          while (reader.Read())
        //          {
        //              WMB10_0000 data = new WMB10_0000();
        //
        //              data.sup_code = comm.sGetInt32(reader["sup_code"].ToString());
        //              data.sup_name = comm.sGetString(reader["sup_name"].ToString());
        //              data.sup_scode = comm.sGetString(reader["sup_scode"].ToString());
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
        //public List<WMB10_0000> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_sup_code", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<WMB10_0000> list = new List<WMB10_0000>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    //sSql = "SELECT * FROM WMB10_0000";
        //    sSql = "SELECT * FROM WMB10_0000";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        //sqlCommand.Parameters.Add(new SqlParameter("@sup_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            WMB10_0000 data = new WMB10_0000();

        //            data.sup_code = comm.sGetInt32(reader["sup_code"].ToString());
        //            data.sup_name = comm.sGetString(reader["sup_name"].ToString());
        //            data.sup_scode = comm.sGetString(reader["sup_scode"].ToString());
        //            data.is_use = comm.sGetString(reader["is_use"].ToString());
        //            data.alm_table = comm.sGetString(reader["alm_table"].ToString());


        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            //資料邏輯刪除、修改
        //            //if (arr_LockGrpCode.Contains(data.sup_code)) {
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
        public List<WMB10_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<WMB10_0000> list = new List<WMB10_0000>();

            string sSql = " SELECT * FROM WMB10_0000  ";

            // 取得資料
            list = comm.Get_ListByQuery<WMB10_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個WMB10_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="WMB10_0000">DTO</param>
        public void InsertData(WMB10_0000 WMB10_0000)
        {
            string sSql = "INSERT INTO " +
                          " WMB10_0000 (   sup_code,  sup_name,  sup_scode,  is_use )  " +
                          "     VALUES (  @sup_code, @sup_name, @sup_scode, @is_use )  ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMB10_0000);
            }
        }

        /// <summary>
        /// 傳入一個WMB10_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="WMB10_0000">DTO</param>
        public void UpdateData(WMB10_0000 WMB10_0000)
        {
            string sSql = " UPDATE WMB10_0000               " +
                          "    SET sup_name   = @sup_name,  " +
                          "        sup_scode   = @sup_scode,  " +
                          "        is_use = @is_use " +
                          "  WHERE sup_code   = @sup_code   ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMB10_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM WMB10_0000 WHERE sup_code = @sup_code;";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { sup_code = pTkCode });
            }
        }


    }
}