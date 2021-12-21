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
    public class QMB17_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得QMB17_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO QMB17_0000</returns>
        public QMB17_0000 GetDTO(string pTkCode)
        {
            QMB17_0000 datas = new QMB17_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM QMB17_0000";
            }
            else
            {
                sSql = "SELECT * FROM QMB17_0000 where unit_code=@unit_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@unit_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new QMB17_0000
                        {
                            unit_code = comm.sGetString(reader["unit_code"].ToString()),
                            unit_name = comm.sGetString(reader["unit_name"].ToString()),
                            cmemo = comm.sGetString(reader["cmemo"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得QMB17_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List QMB17_0000</returns>
        //public List<QMB17_0000> Get_DataList(string pTkCode)
        //{
        //    List<QMB17_0000> list = new List<QMB17_0000>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM QMB17_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM QMB17_0000 where unit_code=@unit_code";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@unit_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            QMB17_0000 data = new QMB17_0000();

        //            data.unit_code = comm.sGetString(reader["unit_code"].ToString());
        //            data.unit_name = comm.sGetString(reader["unit_name"].ToString());
        //            data.cmemo = comm.sGetString(reader["cmemo"].ToString());
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
        //public List<QMB17_0000> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_unit_code", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<QMB17_0000> list = new List<QMB17_0000>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    //sSql = "SELECT * FROM QMB17_0000";
        //    sSql = "SELECT * FROM QMB17_0000";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        //sqlCommand.Parameters.Add(new SqlParameter("@unit_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            QMB17_0000 data = new QMB17_0000();

        //            data.unit_code = comm.sGetString(reader["unit_code"].ToString());
        //            data.unit_name = comm.sGetString(reader["unit_name"].ToString());
        //            data.cmemo = comm.sGetString(reader["cmemo"].ToString());
        //            data.qtest_item_type = comm.sGetString(reader["qtest_item_type"].ToString());

        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            //資料邏輯刪除、修改
        //            //if (arr_LockGrpCode.Contains(data.unit_code)) {
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
        public List<QMB17_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<QMB17_0000> list = new List<QMB17_0000>();

            string sSql = " SELECT * FROM QMB17_0000 ";

            // 取得資料
            list = comm.Get_ListByQuery<QMB17_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個QMB17_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="QMB17_0000">DTO</param>
        public void InsertData(QMB17_0000 QMB17_0000)
        {
            string sSql = " INSERT INTO " +
                          " QMB17_0000 (  unit_code,  unit_name,  cmemo ) " +

                          "     VALUES ( @unit_code, @unit_name, @cmemo ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, QMB17_0000);
            }
        }

        /// <summary>
        /// 傳入一個QMB17_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="QMB17_0000">DTO</param>
        public void UpdateData(QMB17_0000 QMB17_0000)
        {
            string sSql = " UPDATE QMB17_0000              " +
                          "    SET unit_name = @unit_name, " +
                          "        cmemo     = @cmemo      " +
                          "  WHERE unit_code = @unit_code  ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, QMB17_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM QMB17_0000 WHERE unit_code = @unit_code ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { unit_code = pTkCode });
            }
        }

    }
}