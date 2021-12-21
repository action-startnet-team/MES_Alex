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
    public class MEB48_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MEB48_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEB48_0000</returns>
        public MEB48_0000 GetDTO(string pTkCode)
        {
            MEB48_0000 datas = new MEB48_0000();

            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB48_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEB48_0000 where meb48_0000=@meb48_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@meb48_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEB48_0000
                        {
                            meb48_0000 = comm.sGetInt32(reader["meb48_0000"].ToString()),
                            scr_no = comm.sGetString(reader["scr_no"].ToString()),
                            table_code = comm.sGetString(reader["table_code"].ToString()),
                            field_code = comm.sGetString(reader["field_code"].ToString()),
                            field_name = comm.sGetString(reader["field_name"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MEB48_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MEB48_0000</returns>
        //  public List<MEB48_0000> Get_DataList(string pTkCode)
        //  {
        //      List<MEB48_0000> list = new List<MEB48_0000>();
        //      string sSql = "";
        //
        //      if (string.IsNullOrEmpty(pTkCode))
        //      {
        //          sSql = "SELECT * FROM MEB48_0000";
        //      }
        //      else
        //      {
        //          sSql = "SELECT * FROM MEB48_0000 where meb48_0000=@meb48_0000";
        //      }
        //
        //
        //      using (SqlConnection con_db = comm.Set_DBConnection())
        //      {
        //          SqlCommand sqlCommand = new SqlCommand(sSql);
        //          sqlCommand.Connection = con_db;
        //          sqlCommand.Parameters.Add(new SqlParameter("@meb48_0000", pTkCode));
        //          SqlDataReader reader = sqlCommand.ExecuteReader();
        //
        //          while (reader.Read())
        //          {
        //              MEB48_0000 data = new MEB48_0000();
        //
        //              data.meb48_0000 = comm.sGetInt32(reader["meb48_0000"].ToString());
        //              data.table_code = comm.sGetString(reader["table_code"].ToString());
        //              data.field_code = comm.sGetString(reader["field_code"].ToString());
        //              data.field_name = comm.sGetString(reader["field_name"].ToString());
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
        //public List<MEB48_0000> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_meb48_0000", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<MEB48_0000> list = new List<MEB48_0000>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    //sSql = "SELECT * FROM MEB48_0000";
        //    sSql = "SELECT * FROM MEB48_0000";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        //sqlCommand.Parameters.Add(new SqlParameter("@meb48_0000", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            MEB48_0000 data = new MEB48_0000();

        //            data.meb48_0000 = comm.sGetInt32(reader["meb48_0000"].ToString());
        //            data.table_code = comm.sGetString(reader["table_code"].ToString());
        //            data.field_code = comm.sGetString(reader["field_code"].ToString());
        //            data.field_name = comm.sGetString(reader["field_name"].ToString());
        //            data.alm_table = comm.sGetString(reader["alm_table"].ToString());


        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            //資料邏輯刪除、修改
        //            //if (arr_LockGrpCode.Contains(data.meb48_0000)) {
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
        public List<MEB48_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MEB48_0000> list = new List<MEB48_0000>();

            string sSql = " SELECT MEB48_0000.* , BDP21_0100.field_name as table_name FROM MEB48_0000 " +
                           " left join BDP21_0100 on BDP21_0100.field_code = MEB48_0000.table_code ";

            // 取得資料
            list = comm.Get_ListByQuery<MEB48_0000>(sSql, pWhere, pUsrCode, pPrgCode);

            // 權限設定
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);

            for (int i = 0; i < list.Count; i++)
            {
                //list[i].table_name = comm.Get_QueryData("BDP21_0100",list[i].table_code,"field_code","field_name");
                //檢查授權刪除、修改
                list[i].can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                list[i].can_update = sLimitStr.Contains("M") ? "Y" : "N";
            }

            return list;

        }

        /// <summary>
        /// 傳入一個MEB48_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEB48_0000">DTO</param>
        public void InsertData(MEB48_0000 MEB48_0000)
        {
            string sSql = "INSERT INTO " +
                          " MEB48_0000 ( scr_no,  table_code,  field_code,  field_name ) " +
                          "     VALUES (@scr_no, @table_code, @field_code, @field_name ) ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB48_0000);
            }
        }

        /// <summary>
        /// 傳入一個MEB48_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEB48_0000">DTO</param>
        public void UpdateData(MEB48_0000 MEB48_0000)
        {
            string sSql = " UPDATE MEB48_0000                " +
                          "    SET scr_no     = @scr_no,     " +
                          "        table_code = @table_code, " +
                          "        field_code = @field_code, " +
                          "        field_name = @field_name  " +
                          "  WHERE meb48_0000 = @meb48_0000  ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB48_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MEB48_0000 WHERE meb48_0000 = @meb48_0000;";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { meb48_0000 = pTkCode });
            }
        }

    }
}