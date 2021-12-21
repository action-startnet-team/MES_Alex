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
    public class EMB21_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得EMB21_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO EMB21_0000</returns>
        public EMB21_0000 GetDTO(string pTkCode)
        {
            EMB21_0000 datas = new EMB21_0000();

            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMB21_0000";
            }
            else
            {
                sSql = "SELECT * FROM EMB21_0000 where chk_item_code=@chk_item_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@chk_item_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new EMB21_0000
                        {
                            chk_item_code = comm.sGetString(reader["chk_item_code"].ToString()),
                            chk_item_name = comm.sGetString(reader["chk_item_name"].ToString()),
                            chk_item_memo = comm.sGetString(reader["chk_item_memo"].ToString()),
                            chk_item_type = comm.sGetString(reader["chk_item_type"].ToString()),
                            dev_code = comm.sGetString(reader["dev_code"].ToString()),
                            scr_no = comm.sGetString(reader["scr_no"].ToString()),
                            is_use = comm.sGetString(reader["is_use"].ToString()),
                            ins_date = comm.sGetString(reader["ins_date"].ToString()),
                            ins_time = comm.sGetString(reader["ins_time"].ToString()),
                            usr_code = comm.sGetString(reader["usr_code"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得EMB21_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List EMB21_0000</returns>
        //  public List<EMB21_0000> Get_DataList(string pTkCode)
        //  {
        //      List<EMB21_0000> list = new List<EMB21_0000>();
        //      string sSql = "";
        //
        //      if (string.IsNullOrEmpty(pTkCode))
        //      {
        //          sSql = "SELECT * FROM EMB21_0000";
        //      }
        //      else
        //      {
        //          sSql = "SELECT * FROM EMB21_0000 where chk_item_code=@chk_item_code";
        //      }
        //
        //
        //      using (SqlConnection con_db = comm.Set_DBConnection())
        //      {
        //          SqlCommand sqlCommand = new SqlCommand(sSql);
        //          sqlCommand.Connection = con_db;
        //          sqlCommand.Parameters.Add(new SqlParameter("@chk_item_code", pTkCode));
        //          SqlDataReader reader = sqlCommand.ExecuteReader();
        //
        //          while (reader.Read())
        //          {
        //              EMB21_0000 data = new EMB21_0000();
        //
        //              data.chk_item_code = comm.sGetInt32(reader["chk_item_code"].ToString());
        //              data.chk_item_name = comm.sGetString(reader["chk_item_name"].ToString());
        //              data.chk_item_memo = comm.sGetString(reader["chk_item_memo"].ToString());
        //              data.chk_item_type = comm.sGetString(reader["chk_item_type"].ToString());
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
        //public List<EMB21_0000> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_chk_item_code", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<EMB21_0000> list = new List<EMB21_0000>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    //sSql = "SELECT * FROM EMB21_0000";
        //    sSql = "SELECT * FROM EMB21_0000";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        //sqlCommand.Parameters.Add(new SqlParameter("@chk_item_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            EMB21_0000 data = new EMB21_0000();

        //            data.chk_item_code = comm.sGetInt32(reader["chk_item_code"].ToString());
        //            data.chk_item_name = comm.sGetString(reader["chk_item_name"].ToString());
        //            data.chk_item_memo = comm.sGetString(reader["chk_item_memo"].ToString());
        //            data.chk_item_type = comm.sGetString(reader["chk_item_type"].ToString());
        //            data.alm_table = comm.sGetString(reader["alm_table"].ToString());


        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            //資料邏輯刪除、修改
        //            //if (arr_LockGrpCode.Contains(data.chk_item_code)) {
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
        public List<EMB21_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<EMB21_0000> list = new List<EMB21_0000>();

            string sSql = " SELECT EMB21_0000.*, BDP21_0100.field_name as chk_item_type_name, EMB07_0000.dev_name " +
                          " FROM EMB21_0000 " +
                          " left join BDP21_0100 on BDP21_0100.field_code = EMB21_0000.chk_item_type and BDP21_0100.code_code = 'qtest_item_type' " +
                          " left join EMB07_0000 on EMB07_0000.dev_code = EMB21_0000.dev_code ";

            // 取得資料
            list = comm.Get_ListByQuery<EMB21_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個EMB21_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="EMB21_0000">DTO</param>
        public void InsertData(EMB21_0000 EMB21_0000)
        {
            string sSql = "INSERT INTO " +
                          " EMB21_0000 (  chk_item_code,  chk_item_name,  chk_item_memo,  chk_item_type,  dev_code, " +
                          "               scr_no,  is_use,  ins_date,  ins_time,  usr_code ) " +
                          "     VALUES ( @chk_item_code, @chk_item_name, @chk_item_memo, @chk_item_type, @dev_code, " +
                          "              @scr_no, @is_use, @ins_date, @ins_time, @usr_code ) ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMB21_0000);
            }
        }

        /// <summary>
        /// 傳入一個EMB21_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="EMB21_0000">DTO</param>
        public void UpdateData(EMB21_0000 EMB21_0000)
        {
            string sSql = " UPDATE EMB21_0000                      " +
                          "    SET chk_item_name = @chk_item_name, " +
                          "        chk_item_memo = @chk_item_memo, " +
                          "        chk_item_type = @chk_item_type, " +
                          "        dev_code      = @dev_code,      " +
                          "        scr_no        = @scr_no,        " +
                          "        is_use        = @is_use,        " +
                          "        ins_date      = @ins_date,      " +
                          "        ins_time      = @ins_time,      " +
                          "        usr_code      = @usr_code       " +
                          "  WHERE chk_item_code = @chk_item_code  ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMB21_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM EMB21_0000 WHERE chk_item_code = @chk_item_code ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { chk_item_code = pTkCode });
            }
        }


    }
}