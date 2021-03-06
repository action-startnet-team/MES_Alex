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
    public class EMB08_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得EMB08_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO EMB08_0000</returns>
        public EMB08_0000 GetDTO(string pTkCode)
        {
            EMB08_0000 datas = new EMB08_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMB08_0000";
            }
            else
            {
                sSql = "SELECT * FROM EMB08_0000 where main_item_code=@main_item_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@main_item_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new EMB08_0000
                        {
                            main_item_code = comm.sGetString(reader["main_item_code"].ToString()),
                            main_item_name = comm.sGetString(reader["main_item_name"].ToString()),
                            main_item_memo = comm.sGetString(reader["main_item_memo"].ToString()),
                            qtest_item_type = comm.sGetString(reader["qtest_item_type"].ToString()),
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
        /// 取得EMB08_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List EMB08_0000</returns>
        //public List<EMB08_0000> Get_DataList(string pTkCode)
        //{
        //    List<EMB08_0000> list = new List<EMB08_0000>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM EMB08_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM EMB08_0000 where main_item_code=@main_item_code";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@main_item_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            EMB08_0000 data = new EMB08_0000();

        //            data.main_item_code = comm.sGetString(reader["main_item_code"].ToString());
        //            data.main_item_name = comm.sGetString(reader["main_item_name"].ToString());
        //            data.main_item_memo = comm.sGetString(reader["main_item_memo"].ToString());
        //            data.qtest_item_type = comm.sGetString(reader["qtest_item_type"].ToString());

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
        //public List<EMB08_0000> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_main_item_code", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<EMB08_0000> list = new List<EMB08_0000>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    //sSql = "SELECT * FROM EMB08_0000";
        //    sSql = "SELECT * FROM EMB08_0000";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        //sqlCommand.Parameters.Add(new SqlParameter("@main_item_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            EMB08_0000 data = new EMB08_0000();

        //            data.main_item_code = comm.sGetString(reader["main_item_code"].ToString());
        //            data.main_item_name = comm.sGetString(reader["main_item_name"].ToString());
        //            data.main_item_memo = comm.sGetString(reader["main_item_memo"].ToString());
        //            data.qtest_item_type = comm.sGetString(reader["qtest_item_type"].ToString());

        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            //資料邏輯刪除、修改
        //            //if (arr_LockGrpCode.Contains(data.main_item_code)) {
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
        public List<EMB08_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<EMB08_0000> list = new List<EMB08_0000>();

            string sSql = " SELECT *, BDP21_0100.field_name as qtest_item_type_name, BDP08_0000.usr_name " +
                          " FROM EMB08_0000 " +
                          " left join BDP21_0100 on BDP21_0100.field_code = EMB08_0000.qtest_item_type and BDP21_0100.code_code = 'qtest_item_type' " +
                          " left join BDP08_0000 on BDP08_0000.usr_code = EMB08_0000.usr_code ";

            // 取得資料
            list = comm.Get_ListByQuery<EMB08_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個EMB08_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="EMB08_0000">DTO</param>
        public void InsertData(EMB08_0000 EMB08_0000)
        {
            string sSql = " INSERT INTO " +
                          " EMB08_0000 (  main_item_code,  main_item_name,  main_item_memo,  qtest_item_type,  is_use, " +
                          "               ins_date,  ins_time,  usr_code ) " +
                          "     VALUES ( @main_item_code, @main_item_name, @main_item_memo, @qtest_item_type, @is_use, " +
                          "              @ins_date, @ins_time, @usr_code ) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMB08_0000);
            }
        }

        /// <summary>
        /// 傳入一個EMB08_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="EMB08_0000">DTO</param>
        public void UpdateData(EMB08_0000 EMB08_0000)
        {
            string sSql = " UPDATE EMB08_0000                          " +
                          "    SET main_item_name  = @main_item_name,  " +
                          "        main_item_memo  = @main_item_memo,  " +
                          "        qtest_item_type = @qtest_item_type, " +
                          "        is_use          = @is_use,          " +
                          "        ins_date        = @ins_date,        " +
                          "        ins_time        = @ins_time,        " +
                          "        usr_code        = @usr_code         " +
                          "  WHERE main_item_code  = @main_item_code   " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMB08_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM EMB08_0000 WHERE main_item_code = @main_item_code;";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { main_item_code = pTkCode });
            }
        }

    }
}