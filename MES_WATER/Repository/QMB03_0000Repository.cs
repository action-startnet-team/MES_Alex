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
    public class QMB03_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得QMB03_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO QMB03_0000</returns>
        public QMB03_0000 GetDTO(string pTkCode)
        {
            QMB03_0000 datas = new QMB03_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM QMB03_0000";
            }
            else
            {
                sSql = "SELECT * FROM QMB03_0000 where qsheet_code=@qsheet_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@qsheet_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new QMB03_0000
                        {
                            qsheet_code = comm.sGetString(reader["qsheet_code"].ToString()),
                            qsheet_name = comm.sGetString(reader["qsheet_name"].ToString()),
                            pro_code = comm.sGetString(reader["pro_code"].ToString()),
                            qsheet_memo = comm.sGetString(reader["qsheet_memo"].ToString()),
                            qsheet_type = comm.sGetString(reader["qsheet_type"].ToString()),
                            qtest_level_code = comm.sGetString(reader["qtest_level_code"].ToString()),
                            ins_level_code = comm.sGetString(reader["ins_level_code"].ToString()),
                            work_code = comm.sGetString(reader["work_code"].ToString()),
                            epb_code = comm.sGetString(reader["epb_code"].ToString()),
                            ins_date = comm.sGetString(reader["ins_date"].ToString()),
                            ins_time = comm.sGetString(reader["ins_time"].ToString()),
                            usr_code = comm.sGetString(reader["usr_code"].ToString()),
                            version = comm.sGetString(reader["version"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得QMB03_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List QMB03_0000</returns>
        //public List<QMB03_0000> Get_DataList(string pTkCode)
        //{
        //    List<QMB03_0000> list = new List<QMB03_0000>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM QMB03_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM QMB03_0000 where qsheet_code=@qsheet_code";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@qsheet_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            QMB03_0000 data = new QMB03_0000();

        //            data.qsheet_code = comm.sGetString(reader["qsheet_code"].ToString());
        //            data.qsheet_name = comm.sGetString(reader["qsheet_name"].ToString());
        //            data.qsheet_memo = comm.sGetString(reader["qsheet_memo"].ToString());
        //            data.qsheet_type = comm.sGetString(reader["qsheet_type"].ToString());
        //            data.qtest_level_code = comm.sGetString(reader["qtest_level_code"].ToString());
        //            data.qtest_level_name = comm.sGetString(reader["qtest_level_name"].ToString());
        //            data.ins_level_code = comm.sGetString(reader["ins_level_code"].ToString());
        //            data.ins_level_name = comm.sGetString(reader["ins_level_name"].ToString());
        //            data.work_code = comm.sGetString(reader["work_code"].ToString());
        //            data.epb_code = comm.sGetString(reader["epb_code"].ToString());
        //            data.epb_name = comm.sGetString(reader["epb_name"].ToString());

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
        //public List<QMB03_0000> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_qsheet_code", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<QMB03_0000> list = new List<QMB03_0000>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    sSql = " SELECT * FROM QMB03_0000 ";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            QMB03_0000 data = new QMB03_0000();

        //            data.qsheet_code = comm.sGetString(reader["qsheet_code"].ToString());
        //            data.qsheet_name = comm.sGetString(reader["qsheet_name"].ToString());
        //            data.qsheet_memo = comm.sGetString(reader["qsheet_memo"].ToString());
        //            data.qsheet_type = comm.sGetString(reader["qsheet_type"].ToString());
        //            data.qtest_level_code = comm.sGetString(reader["qtest_level_code"].ToString());
        //            data.qtest_level_name = comm.sGetString(reader["qtest_level_name"].ToString());
        //            data.ins_level_code = comm.sGetString(reader["ins_level_code"].ToString());
        //            data.ins_level_name = comm.sGetString(reader["ins_level_name"].ToString());
        //            data.epb_code = comm.sGetString(reader["epb_code"].ToString());
        //            data.epb_name = comm.sGetString(reader["epb_name"].ToString());

        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            //資料邏輯刪除、修改
        //            //if (arr_LockGrpCode.Contains(data.qsheet_code)) {
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
        public List<QMB03_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<QMB03_0000> list = new List<QMB03_0000>();

            string sSql = " SELECT distinct QMB03_0000.qsheet_code, QMB03_0000.*,MEB20_0000.pro_name, QMB09_0000.qtest_level_name, QMB10_0000.ins_level_name, EPB02_0000.epb_name, MEB30_0000.work_name, BDP08_0000.usr_name " +
                          " FROM QMB03_0000 " +
                          " left join QMB03_0100 on QMB03_0100.qsheet_code = QMB03_0000.qsheet_code " +
                          " left join QMB09_0000 on QMB09_0000.qtest_level_code = QMB03_0000.qtest_level_code " +
                          " left join QMB10_0000 on QMB10_0000.ins_level_code = QMB03_0000.ins_level_code " +
                          " left join EPB02_0000 on EPB02_0000.epb_code = QMB03_0000.epb_code " +
                          " left join MEB30_0000 on MEB30_0000.work_code = QMB03_0000.work_code " +
                          " left join BDP08_0000 on BDP08_0000.usr_code = QMB03_0000.usr_code " +
                          " left join QMB02_0000 on QMB02_0000.qtest_item_code = QMB03_0100.qtest_item_code " +
                          " left join MEB20_0000 on MEB20_0000.pro_code = QMB03_0000.pro_code  " +
                          " left join BDP21_0100 on BDP21_0100.field_code = QMB03_0100.qtest_item_type and BDP21_0100.code_code = 'qtest_item_type' " ;

            // 取得資料
            list = comm.Get_ListByQuery<QMB03_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個QMB03_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="QMB03_0000">DTO</param>
        public void InsertData(QMB03_0000 QMB03_0000)
        {
            string sSql = " INSERT INTO " +
                          " QMB03_0000 (  qsheet_code,  qsheet_name, pro_code,  qsheet_memo,  qsheet_type,  qtest_level_code, " +
                          "               ins_level_code,  work_code,  epb_code,  ins_date,  ins_time,  usr_code , version) " +

                          "     VALUES ( @qsheet_code, @qsheet_name, @pro_code  , @qsheet_memo, @qsheet_type, @qtest_level_code, " +
                          "              @ins_level_code, @work_code, @epb_code, @ins_date, @ins_time, @usr_code , @version ) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, QMB03_0000);
            }
        }

        /// <summary>
        /// 傳入一個QMB03_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="QMB03_0000">DTO</param>
        public void UpdateData(QMB03_0000 QMB03_0000)
        {
            string sSql = " UPDATE QMB03_0000                            " +
                          "    SET qsheet_name      = @qsheet_name,      " +
                          "        pro_code         = @pro_code,         " +
                          "        qsheet_memo      = @qsheet_memo,      " +
                          "        qsheet_type      = @qsheet_type,      " +
                          "        qtest_level_code = @qtest_level_code, " +
                          "        ins_level_code   = @ins_level_code,   " +
                          "        work_code        = @work_code,        " +
                          "        epb_code         = @epb_code,         " +
                          "        ins_date         = @ins_date,         " +
                          "        ins_time         = @ins_time,         " +
                          "        usr_code         = @usr_code,          " +
                          "        version         = @version            " +
                          "  WHERE qsheet_code      = @qsheet_code       " ;

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, QMB03_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM QMB03_0000 WHERE qsheet_code = @qsheet_code " +
                          "DELETE FROM QMB03_0100 WHERE qsheet_code = @qsheet_code " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { qsheet_code = pTkCode });
            }
        }
    }
}