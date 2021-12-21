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
    public class AMB01_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得AMB01_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO AMB01_0000</returns>
        public AMB01_0000 GetDTO(string pTkCode)
        {
            AMB01_0000 datas = new AMB01_0000();

            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM AMB01_0000";
            }
            else
            {
                sSql = "SELECT * FROM AMB01_0000 where amb01_0000=@amb01_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@amb01_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new AMB01_0000
                        {
                            amb01_0000 = comm.sGetInt32(reader["amb01_0000"].ToString()),
                            alm_code = comm.sGetString(reader["alm_code"].ToString()),
                            alm_name = comm.sGetString(reader["alm_name"].ToString()),
                            is_use = comm.sGetString(reader["is_use"].ToString()),
                            alm_table = comm.sGetString(reader["alm_table"].ToString()),

                            alm_field = comm.sGetString(reader["alm_field"].ToString()),
                            alm_type = comm.sGetString(reader["alm_type"].ToString()),
                            upper_limit = comm.sGetDouble(reader["upper_limit"].ToString()),
                            lower_limit = comm.sGetDouble(reader["lower_limit"].ToString()),
                            alm_formula = comm.sGetString(reader["alm_formula"].ToString())
                        };
                    }
                }
            }
            return datas;
        }

       #region //
        /// <summary>
        /// 取得AMB01_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List AMB01_0000</returns>
      //  public List<AMB01_0000> Get_DataList(string pTkCode)
      //  {
      //      List<AMB01_0000> list = new List<AMB01_0000>();
      //      string sSql = "";
      //
      //      if (string.IsNullOrEmpty(pTkCode))
      //      {
      //          sSql = "SELECT * FROM AMB01_0000";
      //      }
      //      else
      //      {
      //          sSql = "SELECT * FROM AMB01_0000 where amb01_0000=@amb01_0000";
      //      }
      //
      //
      //      using (SqlConnection con_db = comm.Set_DBConnection())
      //      {
      //          SqlCommand sqlCommand = new SqlCommand(sSql);
      //          sqlCommand.Connection = con_db;
      //          sqlCommand.Parameters.Add(new SqlParameter("@amb01_0000", pTkCode));
      //          SqlDataReader reader = sqlCommand.ExecuteReader();
      //
      //          while (reader.Read())
      //          {
      //              AMB01_0000 data = new AMB01_0000();
      //
      //              data.amb01_0000 = comm.sGetInt32(reader["amb01_0000"].ToString());
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
        //public List<AMB01_0000> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_amb01_0000", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<AMB01_0000> list = new List<AMB01_0000>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    //sSql = "SELECT * FROM AMB01_0000";
        //    sSql = "SELECT * FROM AMB01_0000";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        //sqlCommand.Parameters.Add(new SqlParameter("@amb01_0000", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            AMB01_0000 data = new AMB01_0000();

        //            data.amb01_0000 = comm.sGetInt32(reader["amb01_0000"].ToString());
        //            data.alm_code = comm.sGetString(reader["alm_code"].ToString());
        //            data.alm_name = comm.sGetString(reader["alm_name"].ToString());
        //            data.is_use = comm.sGetString(reader["is_use"].ToString());
        //            data.alm_table = comm.sGetString(reader["alm_table"].ToString());


        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            //資料邏輯刪除、修改
        //            //if (arr_LockGrpCode.Contains(data.amb01_0000)) {
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
        public List<AMB01_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<AMB01_0000> list = new List<AMB01_0000>();

            //string sSql = " SELECT AMB01_0000.*" +
            //            " FROM AMB01_0000 " ;
            string sSql =
              " SELECT distinct AMB01_0000.*, BDP21_0100.field_name as alm_type_name" +
              " FROM AMB01_0000 left join BDP21_0100 on" +
              " BDP21_0100.field_code = AMB01_0000.alm_type" +
              " and BDP21_0100.code_code = 'alm_type'";

            // 取得資料
            list = comm.Get_ListByQuery<AMB01_0000>(sSql, pWhere, pUsrCode, pPrgCode);

            // 權限設定
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            //string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_mtp_code", "par_name", "par_value");
            //var arr_LockGrpCode = sLockGrpCode.Split(',');

            for (int i = 0; i < list.Count; i++)
            {
                //檢查授權刪除、修改
                list[i].can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                list[i].can_update = sLimitStr.Contains("M") ? "Y" : "N";

                //        // 特例 轉換
                //        data.alm_code = data.amb01_0000 + " - " + comm.sGetString(reader["alm_code"].ToString());
                //        data.sto_name = comm.sGetString(reader["sto_code"].ToString()) + " - " + comm.sGetString(reader["sto_name"].ToString());

                //        data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                //        data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                //        //資料邏輯刪除、修改
                //        //if (arr_LockGrpCode.Contains(data.mtp_code)) {
                //        //    data.can_delete = "N";
                //        //    data.can_update = "N";
                //        //}
            }

            return list;

        }

        /// <summary>
        /// 傳入一個AMB01_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="AMB01_0000">DTO</param>
        public void InsertData(AMB01_0000 AMB01_0000)
        {
            string sSql = "INSERT INTO " +
                          " AMB01_0000 (  alm_code,   alm_name,  is_use,   alm_table, alm_field ,alm_type ,upper_limit ,lower_limit ,alm_formula)  " +
                          "     VALUES (  @alm_code,  @alm_name, @is_use,  @alm_table,@alm_field ,@alm_type ,@upper_limit ,@lower_limit ,@alm_formula )  ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, AMB01_0000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@amb01_0000", AMB01_0000.amb01_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@amb01_0000", AMB01_0000.amb01_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@alm_code", AMB01_0000.alm_code));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個AMB01_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="AMB01_0000">DTO</param>
        public void UpdateData(AMB01_0000 AMB01_0000)
        {
            string sSql = " UPDATE AMB01_0000 " +
                          "    SET alm_code  = @alm_code,    " +
                          "        alm_name  = @alm_name,    " +
                          "        is_use  = @is_use,      " +
                          "        alm_table  = @alm_table,      " +
                          "        alm_field  = @alm_field,     " +
                          "        alm_type  = @alm_type,      " +
                          "        upper_limit  = @upper_limit,      " +
                          "        lower_limit  = @lower_limit,      " +
                          "        alm_formula  = @alm_formula      " +
                          "  WHERE amb01_0000= @amb01_0000 ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, AMB01_0000);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@amb01_0000", AMB01_0000.amb01_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@amb01_0000", AMB01_0000.amb01_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@alm_code", AMB01_0000.alm_code));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM AMB01_0000 WHERE amb01_0000 = @amb01_0000;";
            //sSql += " Delete from BDP09_0100 where amb01_0000 = @amb01_0000; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { amb01_0000 = pTkCode });

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@amb01_0000", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }






        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得AMB01_0000角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetAMB01_0000_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("amb01_0000", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("amb01_0000", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("alm_code", System.Type.GetType("System.String"].ToString());

            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM AMB01_0000";
            }
            else
            {
                sSql = "SELECT * FROM AMB01_0000 where amb01_0000='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["amb01_0000"] = dtTmp.Rows[i]["amb01_0000"];
                drow["amb01_0000"] = dtTmp.Rows[i]["amb01_0000"];
                drow["alm_code"] = dtTmp.Rows[i]["alm_code"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}