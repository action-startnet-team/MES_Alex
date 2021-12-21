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
    public class QMB14_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得QMB14_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO QMB14_0000</returns>
        public QMB14_0000 GetDTO(string pTkCode)
        {
            QMB14_0000 datas = new QMB14_0000();

            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM QMB14_0000";
            }
            else
            {
                sSql = "SELECT * FROM QMB14_0000 where spc_code=@spc_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@spc_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new QMB14_0000
                        {
                            spc_code = comm.sGetString(reader["spc_code"].ToString()),
                            spc_name = comm.sGetString(reader["spc_name"].ToString()),
                            up_limit = comm.sGetDecimal(reader["up_limit"].ToString()),
                            down_limit = comm.sGetDecimal(reader["down_limit"].ToString()),
                            epb_code = comm.sGetString(reader["epb_code"].ToString()),
                            epb_field_code = comm.sGetString(reader["epb_field_code"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得QMB14_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List QMB14_0000</returns>
        //  public List<QMB14_0000> Get_DataList(string pTkCode)
        //  {
        //      List<QMB14_0000> list = new List<QMB14_0000>();
        //      string sSql = "";
        //
        //      if (string.IsNullOrEmpty(pTkCode))
        //      {
        //          sSql = "SELECT * FROM QMB14_0000";
        //      }
        //      else
        //      {
        //          sSql = "SELECT * FROM QMB14_0000 where spc_code=@spc_code";
        //      }
        //
        //
        //      using (SqlConnection con_db = comm.Set_DBConnection())
        //      {
        //          SqlCommand sqlCommand = new SqlCommand(sSql);
        //          sqlCommand.Connection = con_db;
        //          sqlCommand.Parameters.Add(new SqlParameter("@spc_code", pTkCode));
        //          SqlDataReader reader = sqlCommand.ExecuteReader();
        //
        //          while (reader.Read())
        //          {
        //              QMB14_0000 data = new QMB14_0000();
        //
        //              data.spc_code = comm.sGetInt32(reader["spc_code"].ToString());
        //              data.spc_name = comm.sGetString(reader["spc_name"].ToString());
        //              data.up_limit = comm.sGetString(reader["up_limit"].ToString());
        //              data.down_limit = comm.sGetString(reader["down_limit"].ToString());
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
        //public List<QMB14_0000> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_spc_code", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<QMB14_0000> list = new List<QMB14_0000>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    //sSql = "SELECT * FROM QMB14_0000";
        //    sSql = "SELECT * FROM QMB14_0000";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        //sqlCommand.Parameters.Add(new SqlParameter("@spc_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            QMB14_0000 data = new QMB14_0000();

        //            data.spc_code = comm.sGetInt32(reader["spc_code"].ToString());
        //            data.spc_name = comm.sGetString(reader["spc_name"].ToString());
        //            data.up_limit = comm.sGetString(reader["up_limit"].ToString());
        //            data.down_limit = comm.sGetString(reader["down_limit"].ToString());
        //            data.alm_table = comm.sGetString(reader["alm_table"].ToString());


        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            //資料邏輯刪除、修改
        //            //if (arr_LockGrpCode.Contains(data.spc_code)) {
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
        public List<QMB14_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<QMB14_0000> list = new List<QMB14_0000>();

            string sSql = "select QMB14_0000.*,epb_name,EPB02_0100.field_name as epb_field_name " +
                          "  from QMB14_0000  " +
                          "  left join EPB02_0000 on QMB14_0000.epb_code = EPB02_0000.epb_code" +
                          "  left join EPB02_0100 on QMB14_0000.epb_code = EPB02_0100.epb_code and QMB14_0000.epb_field_code = EPB02_0100.field_code";

            // 取得資料
            list = comm.Get_ListByQuery<QMB14_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個QMB14_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="QMB14_0000">DTO</param>
        public void InsertData(QMB14_0000 QMB14_0000)
        {
            string sSql = "INSERT INTO " +
                          " QMB14_0000 (   spc_code,  spc_name,  up_limit,  down_limit )  " +
                          "     VALUES (  @spc_code, @spc_name, @up_limit, @down_limit )  " ;

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, QMB14_0000);
            }
        }

        /// <summary>
        /// 傳入一個QMB14_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="QMB14_0000">DTO</param>
        public void UpdateData(QMB14_0000 QMB14_0000)
        {
            string sSql = " UPDATE QMB14_0000               " +
                          "    SET spc_name   = @spc_name,  " +
                          "        up_limit   = @up_limit,  " +
                          "        down_limit = @down_limit " +
                          "  WHERE spc_code   = @spc_code   " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, QMB14_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM QMB14_0000 WHERE spc_code = @spc_code;";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { spc_code = pTkCode });
            }
        }


    }
}