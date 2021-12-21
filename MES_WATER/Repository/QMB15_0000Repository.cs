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
    public class QMB15_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得QMB15_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO QMB15_0000</returns>
        public QMB15_0000 GetDTO(string pTkCode)
        {
            QMB15_0000 datas = new QMB15_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM QMB15_0000";
            }
            else
            {
                sSql = "SELECT * FROM QMB15_0000 where spur_code=@spur_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@spur_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new QMB15_0000
                        {
                            spur_code = comm.sGetString(reader["spur_code"].ToString()),
                            spur_name = comm.sGetString(reader["spur_name"].ToString()),
                            is_use = comm.sGetString(reader["is_use"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得QMB15_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List QMB15_0000</returns>
        //public List<QMB15_0000> Get_DataList(string pTkCode)
        //{
        //    List<QMB15_0000> list = new List<QMB15_0000>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM QMB15_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM QMB15_0000 where spur_code=@spur_code";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@spur_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            QMB15_0000 data = new QMB15_0000();

        //            data.spur_code = comm.sGetString(reader["spur_code"].ToString());
        //            data.spur_name = comm.sGetString(reader["spur_name"].ToString());
        //            data.is_use = comm.sGetString(reader["is_use"].ToString());
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
        //public List<QMB15_0000> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_spur_code", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<QMB15_0000> list = new List<QMB15_0000>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    //sSql = "SELECT * FROM QMB15_0000";
        //    sSql = "SELECT * FROM QMB15_0000";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        //sqlCommand.Parameters.Add(new SqlParameter("@spur_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            QMB15_0000 data = new QMB15_0000();

        //            data.spur_code = comm.sGetString(reader["spur_code"].ToString());
        //            data.spur_name = comm.sGetString(reader["spur_name"].ToString());
        //            data.is_use = comm.sGetString(reader["is_use"].ToString());
        //            data.qtest_item_type = comm.sGetString(reader["qtest_item_type"].ToString());

        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            //資料邏輯刪除、修改
        //            //if (arr_LockGrpCode.Contains(data.spur_code)) {
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
        public List<QMB15_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<QMB15_0000> list = new List<QMB15_0000>();

            string sSql = " SELECT * FROM QMB15_0000 ";

            // 取得資料
            list = comm.Get_ListByQuery<QMB15_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個QMB15_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="QMB15_0000">DTO</param>
        public void InsertData(QMB15_0000 QMB15_0000)
        {
            string sSql = " INSERT INTO " +
                          " QMB15_0000 (  spur_code,  spur_name,  is_use ) " +

                          "     VALUES ( @spur_code, @spur_name, @is_use ) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, QMB15_0000);
            }
        }

        /// <summary>
        /// 傳入一個QMB15_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="QMB15_0000">DTO</param>
        public void UpdateData(QMB15_0000 QMB15_0000)
        {
            string sSql = " UPDATE QMB15_0000                          " +
                          "    SET spur_name       = @spur_name,       " +
                          "        is_use          = @is_use,          " +
                          "        qtest_item_type = @qtest_item_type, " +
                          "        is_use          = @is_use           " +
                          "  WHERE spur_code       = @spur_code        " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, QMB15_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM QMB15_0000 WHERE spur_code = @spur_code " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { spur_code = pTkCode });
            }
        }

    }
}