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
    public class QMT03_0100Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得QMT03_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO QMT03_0100</returns>
        public QMT03_0100 GetDTO(string pTkCode)
        {
            QMT03_0100 datas = new QMT03_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM QMT03_0100";
            }
            else
            {
                sSql = "SELECT * FROM QMT03_0100 where qmt03_0100=@qmt03_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@qmt03_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new QMT03_0100
                        {
                            qmt03_0100 = comm.sGetInt32(reader["qmt03_0100"].ToString()),
                            qmt_code = comm.sGetString(reader["qmt_code"].ToString()),
                            datacode = comm.sGetString(reader["datacode"].ToString()),
                            qmt_value = comm.sGetString(reader["qmt_value"].ToString()),
                            is_ok = comm.sGetString(reader["is_ok"].ToString()),
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
        ///// <summary>
        ///// 取得QMT03_0100資料表內容
        ///// </summary>
        ///// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        ///// < returns > List QMT03_0100</returns>
        //public List<QMT03_0100> Get_DataList(string pTkCode)
        //{
        //    List<QMT03_0100> list = new List<QMT03_0100>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM QMT03_0100";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM QMT03_0100 where qmt03_0100=@qmt03_0100";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@qmt03_0100", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            QMT03_0100 data = new QMT03_0100();

        //            data.qmt03_0100 = comm.sGetInt32(reader["qmt03_0100"].ToString());
        //            data.qmt_code = comm.sGetString(reader["qmt_code"].ToString());
        //            data.datacode = comm.sGetString(reader["datacode"].ToString());
        //            data.qmt_value = comm.sGetString(reader["qmt_value"].ToString());

        //            data.can_delete = "Y";
        //            data.can_update = "Y";
        //            list.Add(data);
        //        }

        //    }
        //    return list;
        //}

        ///// <summary>
        ///// 取得使用者可以編輯的資料，結合商務邏輯權限
        ///// </summary>
        ///// <param name="pUsrCode"></param>
        ///// <param name="pPrgCode"></param>
        ///// <returns></returns>
        //public List<QMT03_0100> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_qmt03_0100", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<QMT03_0100> list = new List<QMT03_0100>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    sSql = " SELECT * FROM QMT03_0100 ";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        //sqlCommand.Parameters.Add(new SqlParameter("@qmt03_0100", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            QMT03_0100 data = new QMT03_0100();

        //            data.qmt03_0100 = comm.sGetInt32(reader["qmt03_0100"].ToString());
        //            data.qmt_code = comm.sGetString(reader["qmt_code"].ToString());
        //            data.datacode = comm.sGetString(reader["datacode"].ToString());
        //            data.qmt_value = comm.sGetString(reader["qmt_value"].ToString());

        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            //資料邏輯刪除、修改
        //            //if (arr_LockGrpCode.Contains(data.qmt03_0100)) {
        //            //    data.can_delete = "N";
        //            //    data.can_update = "N";
        //            //}

        //            list.Add(data);
        //        }
        //    }
        //    return list;
        //}
        #endregion

        public List<QMT03_0100> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<QMT03_0100> list = new List<QMT03_0100>();
            string foreignKey = gmv.GetKey<QMT03_0000>(new QMT03_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT QMT03_0100.*, QMB03_0100.qtest_item_code, QMB02_0000.qtest_item_name, BDP21_0100.field_name as is_ok_name " +
                       " FROM QMT03_0100 " +
                       " left join QMB03_0100 on QMB03_0100.datacode = QMT03_0100.datacode " +
                       " left join QMB02_0000 on QMB02_0000.qtest_item_code = QMB03_0100.qtest_item_code " +
                       " left join BDP21_0100 on BDP21_0100.field_code = QMT03_0100.is_ok and BDP21_0100.code_code = 'is_ok' " +
                       " where QMT03_0100. " + foreignKey + "=@" + foreignKey;
            }
            else
            {
                sSql = "SELECT * FROM QMT03_0100";
            }
            //取得該使用者可以看的資料
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter(foreignKey, pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {

                    QMT03_0100 data = new QMT03_0100();

                    data.qmt03_0100 = comm.sGetInt32(reader["qmt03_0100"].ToString());
                    data.qmt_code = comm.sGetString(reader["qmt_code"].ToString());
                    data.datacode = comm.sGetString(reader["datacode"].ToString());
                    data.qtest_item_code = comm.sGetString(reader["qtest_item_code"].ToString());
                    data.qtest_item_name = comm.sGetString(reader["qtest_item_name"].ToString());
                    data.qmt_value = comm.sGetString(reader["qmt_value"].ToString());
                    data.is_ok = comm.sGetString(reader["is_ok"].ToString());
                    data.is_ok_name = comm.sGetString(reader["is_ok_name"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());

                    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改

                    list.Add(data);
                }

            }
            return list;
        }

        /// <summary>
        /// 傳入一個QMT03_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="QMT03_0100">DTO</param>
        public void InsertData(QMT03_0100 QMT03_0100)
        {
            string sSql = "INSERT INTO " +
                          " QMT03_0100 (  qmt_code,  datacode,  qmt_value,  is_ok,  ins_date,  ins_time,  usr_code ) " +
                          "     VALUES ( @qmt_code, @datacode, @qmt_value, @is_ok, @ins_date, @ins_time, @usr_code ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, QMT03_0100);
            }
        }

        /// <summary>
        /// 傳入一個QMT03_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="QMT03_0100">DTO</param>
        public void UpdateData(QMT03_0100 QMT03_0100)
        {
            string sSql = " UPDATE QMT03_0100                " +
                          "    SET qmt_code   =  @qmt_code,  " +
                          "        datacode   =  @datacode,  " +
                          "        qmt_value  =  @qmt_value, " +
                          "        is_ok      =  @is_ok,     " +
                          "        ins_date   =  @ins_date,  " +
                          "        ins_time   =  @ins_time,  " +
                          "        usr_code   =  @usr_code   " +
                          "  WHERE qmt03_0100 =  @qmt03_0100 ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, QMT03_0100);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM QMT03_0100 WHERE qmt03_0100 = @qmt03_0100 ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { qmt03_0100 = pTkCode });
            }
        }

    }
}