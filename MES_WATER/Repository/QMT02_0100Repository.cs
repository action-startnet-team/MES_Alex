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
    public class QMT02_0100Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得QMT02_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO QMT02_0100</returns>
        public QMT02_0100 GetDTO(string pTkCode)
        {
            QMT02_0100 datas = new QMT02_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM QMT02_0100";
            }
            else
            {
                sSql = "SELECT * FROM QMT02_0100 where qmt02_0100=@qmt02_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@qmt02_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new QMT02_0100
                        {
                            qmt02_0100 = comm.sGetInt32(reader["qmt02_0100"].ToString()),
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
        ///// 取得QMT02_0100資料表內容
        ///// </summary>
        ///// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        ///// < returns > List QMT02_0100</returns>
        //public List<QMT02_0100> Get_DataList(string pTkCode)
        //{
        //    List<QMT02_0100> list = new List<QMT02_0100>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM QMT02_0100";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM QMT02_0100 where qmt02_0100=@qmt02_0100";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@qmt02_0100", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            QMT02_0100 data = new QMT02_0100();

        //            data.qmt02_0100 = comm.sGetInt32(reader["qmt02_0100"].ToString());
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
        //public List<QMT02_0100> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_qmt02_0100", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<QMT02_0100> list = new List<QMT02_0100>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    sSql = " SELECT * FROM QMT02_0100 ";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        //sqlCommand.Parameters.Add(new SqlParameter("@qmt02_0100", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            QMT02_0100 data = new QMT02_0100();

        //            data.qmt02_0100 = comm.sGetInt32(reader["qmt02_0100"].ToString());
        //            data.qmt_code = comm.sGetString(reader["qmt_code"].ToString());
        //            data.datacode = comm.sGetString(reader["datacode"].ToString());
        //            data.qmt_value = comm.sGetString(reader["qmt_value"].ToString());

        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            //資料邏輯刪除、修改
        //            //if (arr_LockGrpCode.Contains(data.qmt02_0100)) {
        //            //    data.can_delete = "N";
        //            //    data.can_update = "N";
        //            //}

        //            list.Add(data);
        //        }
        //    }
        //    return list;
        //}
        #endregion

        public List<QMT02_0100> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<QMT02_0100> list = new List<QMT02_0100>();
            string foreignKey = gmv.GetKey<QMT02_0000>(new QMT02_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT QMT02_0100.* " +
                       " FROM QMT02_0100 " +
                       " where QMT02_0100. " + foreignKey + "=@" + foreignKey;
            }
            else
            {
                sSql = "SELECT * FROM QMT02_0100";
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

                    QMT02_0100 data = new QMT02_0100();
                    
                    data.qmt02_0100 = comm.sGetInt32(reader["qmt02_0100"].ToString());
                    data.qmt_code = comm.sGetString(reader["qmt_code"].ToString());
                    data.datacode = comm.sGetString(reader["datacode"].ToString());
                    data.qmt_value = comm.sGetString(reader["qmt_value"].ToString());
                    data.is_ok = comm.sGetString(reader["is_ok"].ToString());
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
        /// 傳入一個QMT02_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="QMT02_0100">DTO</param>
        public void InsertData(QMT02_0100 QMT02_0100)
        {
            string sSql = "INSERT INTO " +
                          " QMT02_0100 (  qmt_code,  datacode,  qmt_value,  is_ok,  ins_date,  ins_time,  usr_code ) " +
                          "     VALUES ( @qmt_code, @datacode, @qmt_value, @is_ok, @ins_date, @ins_time, @usr_code ) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, QMT02_0100);
            }
        }

        /// <summary>
        /// 傳入一個QMT02_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="QMT02_0100">DTO</param>
        public void UpdateData(QMT02_0100 QMT02_0100)
        {
            string sSql = " UPDATE QMT02_0100                " +
                          "    SET qmt_code   =  @qmt_code,  " +
                          "        datacode   =  @datacode,  " +
                          "        qmt_value  =  @qmt_value, " +
                          "        is_ok      =  @is_ok,     " +
                          "        ins_date   =  @ins_date,  " +
                          "        ins_time   =  @ins_time,  " +
                          "        usr_code   =  @usr_code   " +
                          "  WHERE qmt02_0100 =  @qmt02_0100 " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, QMT02_0100);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM QMT02_0100 WHERE qmt02_0100 = @qmt02_0100 " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { qmt02_0100 = pTkCode });
            }
        }

    }
}