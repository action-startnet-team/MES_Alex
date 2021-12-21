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
    public class QMB12_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得QMB12_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO QMB12_0000</returns>
        public QMB12_0000 GetDTO(string pTkCode)
        {
            QMB12_0000 datas = new QMB12_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM QMB12_0000";
            }
            else
            {
                sSql = "SELECT * FROM QMB12_0000 where aql_code=@aql_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@aql_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new QMB12_0000
                        {
                            aql_code = comm.sGetString(reader["aql_code"].ToString()),
                            aql_name = comm.sGetString(reader["aql_name"].ToString()),
                            aql_memo = comm.sGetString(reader["aql_memo"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得QMB12_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List QMB12_0000</returns>
        //public List<QMB12_0000> Get_DataList(string pTkCode)
        //{
        //    List<QMB12_0000> list = new List<QMB12_0000>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM QMB12_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM QMB12_0000 where aql_code=@aql_code";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@aql_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            QMB12_0000 data = new QMB12_0000();

        //            data.aql_code = comm.sGetString(reader["aql_code"].ToString());
        //            data.aql_name = comm.sGetString(reader["aql_name"].ToString());
        //            data.cmemo = comm.sGetString(reader["cmemo"].ToString());
        //            data.aql_memo = comm.sGetString(reader["aql_memo"].ToString());
        //            data.ins_date = comm.sGetString(reader["ins_date"].ToString());
        //            data.ins_time = comm.sGetString(reader["ins_time"].ToString());
        //            data.usr_code = comm.sGetString(reader["usr_code"].ToString());
        //            data.cmemo = comm.sGetString(reader["cmemo"].ToString());


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
        //public List<QMB12_0000> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_aql_code", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<QMB12_0000> list = new List<QMB12_0000>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    sSql = "SELECT * FROM QMB12_0000";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            QMB12_0000 data = new QMB12_0000();

        //            data.aql_code = comm.sGetString(reader["aql_code"].ToString());
        //            data.aql_name = comm.sGetString(reader["aql_name"].ToString());
        //            data.cmemo = comm.sGetString(reader["cmemo"].ToString());
        //            data.aql_memo = comm.sGetString(reader["aql_memo"].ToString());
        //            data.ins_date = comm.sGetString(reader["ins_date"].ToString());
        //            data.ins_time = comm.sGetString(reader["ins_time"].ToString());
        //            data.usr_code = comm.sGetString(reader["usr_code"].ToString());
        //            data.cmemo = comm.sGetString(reader["cmemo"].ToString());

        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

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
        public List<QMB12_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<QMB12_0000> list = new List<QMB12_0000>();

            string sSql = " SELECT distinct QMB12_0000.aql_code, QMB12_0000.* " +
                          " FROM QMB12_0000 " +
                          " left join QMB12_0100 on QMB12_0100.aql_code = QMB12_0100.aql_code " ;
            // 取得資料
            list = comm.Get_ListByQuery<QMB12_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個QMB12_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="QMB12_0000">DTO</param>
        public void InsertData(QMB12_0000 QMB12_0000)
        {
            string sSql = "INSERT INTO " +
                          " QMB12_0000 (  aql_code,  aql_name,  aql_memo ) " +
                          "     VALUES ( @aql_code, @aql_name, @aql_memo ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, QMB12_0000);
            }
        }

        /// <summary>
        /// 傳入一個QMB12_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="QMB12_0000">DTO</param>
        public void UpdateData(QMB12_0000 QMB12_0000)
        {
            string sSql = " UPDATE QMB12_0000            " +
                          "    SET aql_name = @aql_name, " +
                          "        aql_memo = @aql_memo  " +
                          "  WHERE aql_code = @aql_code  " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, QMB12_0000);

            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM QMB12_0000 WHERE aql_code = @aql_code ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { aql_code = pTkCode });

            }
        }

    }
}