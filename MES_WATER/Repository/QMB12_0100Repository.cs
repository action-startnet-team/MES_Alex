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
    public class QMB12_0100Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得QMB12_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO QMB12_0100</returns>
        public QMB12_0100 GetDTO(string pTkCode)
        {
            QMB12_0100 datas = new QMB12_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM QMB12_0100";
            }
            else
            {
                sSql = "SELECT * FROM QMB12_0100 where qmb12_0100=@qmb12_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@qmb12_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new QMB12_0100
                        {
                            qmb12_0100 = comm.sGetString(reader["qmb12_0100"].ToString()),
                            aql_code = comm.sGetString(reader["aql_code"].ToString()),
                            aql_down = comm.sGetDecimal(reader["aql_down"].ToString()),
                            aql_up = comm.sGetDecimal(reader["aql_up"].ToString()),
                            sample_qty = comm.sGetDecimal(reader["sample_qty"].ToString()),
                            ng_a = comm.sGetDecimal(reader["ng_a"].ToString()),
                            ng_b = comm.sGetDecimal(reader["ng_b"].ToString()),
                            ng_c = comm.sGetDecimal(reader["ng_c"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得QMB12_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List QMB12_0100</returns>
        //public List<QMB12_0100> Get_DataList(string pTkCode)
        //{
        //    List<QMB12_0100> list = new List<QMB12_0100>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM QMB12_0100";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM QMB12_0100 where qmb12_0100=@qmb12_0100";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@qmb12_0100", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            QMB12_0100 data = new QMB12_0100();

        //            data.qmb12_0100 = comm.sGetString(reader["qmb12_0100"].ToString());
        //            data.aql_code = comm.sGetString(reader["aql_code"].ToString());
        //            data.aql_down = comm.sGetString(reader["aql_down"].ToString());
        //            data.aql_up = comm.sGetString(reader["aql_up"].ToString());
        //            data.sample_qty = comm.sGetString(reader["sample_qty"].ToString());

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
        //public List<QMB12_0100> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_qmb12_0100", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<QMB12_0100> list = new List<QMB12_0100>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料

        //    sSql = "SELECT * FROM QMB12_0100";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            QMB12_0100 data = new QMB12_0100();

        //            data.qmb12_0100 = comm.sGetString(reader["qmb12_0100"].ToString());
        //            data.aql_code = comm.sGetString(reader["aql_code"].ToString());
        //            data.aql_down = comm.sGetString(reader["aql_down"].ToString());
        //            data.aql_up = comm.sGetString(reader["aql_up"].ToString());
        //            data.sample_qty = comm.sGetString(reader["sample_qty"].ToString());

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
        public List<QMB12_0100> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<QMB12_0100> list = new List<QMB12_0100>();
            string foreignKey = gmv.GetKey<QMB12_0000>(new QMB12_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT QMB12_0100.* " +
                       " FROM QMB12_0100 " +
                       " where QMB12_0100. " + foreignKey + "=@" + foreignKey +
                       " order by qmb12_0100 " ;
            }
            else
            {
                sSql = "SELECT * FROM QMB12_0100";
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

                    QMB12_0100 data = new QMB12_0100();

                    data.qmb12_0100 = comm.sGetString(reader["qmb12_0100"].ToString());
                    data.aql_code = comm.sGetString(reader["aql_code"].ToString());
                    data.aql_down = comm.sGetDecimal(reader["aql_down"].ToString());
                    data.aql_up = comm.sGetDecimal(reader["aql_up"].ToString());
                    data.sample_qty = comm.sGetDecimal(reader["sample_qty"].ToString());
                    data.ng_a = comm.sGetDecimal(reader["ng_a"].ToString());
                    data.ng_b = comm.sGetDecimal(reader["ng_b"].ToString());
                    data.ng_c = comm.sGetDecimal(reader["ng_c"].ToString());


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
        /// 傳入一個QMB12_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="QMB12_0100">DTO</param>
        public void InsertData(QMB12_0100 QMB12_0100)
        {
            string sSql = "INSERT INTO " +
                          " QMB12_0100 (  aql_code,  aql_down,  aql_up,  sample_qty,  ng_a,  ng_b,  ng_c ) " +
                          "     VALUES ( @aql_code, @aql_down, @aql_up, @sample_qty, @ng_a, @ng_b, @ng_c ) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, QMB12_0100);
            }
        }

        /// <summary>
        /// 傳入一個QMB12_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="QMB12_0100">DTO</param>
        public void UpdateData(QMB12_0100 QMB12_0100)
        {
            string sSql = " UPDATE QMB12_0100                 " +
                          "    SET aql_down   =  @aql_down,   " +
                          "        aql_up     =  @aql_up,     " +
                          "        sample_qty =  @sample_qty, " +
                          "        ng_a       =  @ng_a,       " +
                          "        ng_b       =  @ng_b,       " +
                          "        ng_c       =  @ng_c        " +
                          "  WHERE qmb12_0100 =  @qmb12_0100  " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, QMB12_0100);

            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM QMB12_0100 WHERE qmb12_0100 = @qmb12_0100; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { qmb12_0100 = pTkCode });

            }
        }

    }
}