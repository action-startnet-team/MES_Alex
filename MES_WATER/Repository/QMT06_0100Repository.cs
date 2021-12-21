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
    public class QMT06_0100Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();
        /// <summary>
        /// 取得QMT06_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO QMT06_0100</returns>
        public QMT06_0100 GetDTO(string pTkCode)
        {
            QMT06_0100 datas = new QMT06_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM QMT06_0100";
            }
            else
            {
                sSql = "SELECT * FROM QMT06_0100 WHERE qmt04_0100 = @qmt04_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@qmt04_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new QMT06_0100
                        {
                            qmt06_0100 = reader.GetInt32(reader.GetOrdinal("qmt06_0100")),
                            oqm_code = reader.GetString(reader.GetOrdinal("oqm_code")),
                            qtest_item_code = reader.GetString(reader.GetOrdinal("qtest_item_code")),
                            scr_no = reader.GetString(reader.GetOrdinal("scr_no")),
                            qtest_up = reader.GetDecimal(reader.GetOrdinal("qtest_up")),
                            qtest_down = reader.GetDecimal(reader.GetOrdinal("qtest_down")),
                            qtest_item_type = reader.GetString(reader.GetOrdinal("qtest_item_type")),
                            is_ok = reader.GetString(reader.GetOrdinal("is_ok")),
                            ins_date = reader.GetString(reader.GetOrdinal("ins_date")),
                            ins_time = reader.GetString(reader.GetOrdinal("ins_time")),
                            usr_code = reader.GetString(reader.GetOrdinal("usr_code")),
                            sample_sum_qty = reader.GetDecimal(reader.GetOrdinal("sample_sum_qty")),
                        };
                    }
                }
            }
            return datas;
        }

        /// <summary>
        /// 根據oqm_code取得QMT06_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List QMT06_0100</returns>
        public List<QMT06_0100> Get_DataList(string pTkCode)
        {
            List<QMT06_0100> list = new List<QMT06_0100>();
            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM QMT06_0100 order by oqm_code ";
            }
            else
            {
                sSql = "SELECT * FROM QMT06_0100 WHERE oqm_code=@oqm_code order by scr_no";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@oqm_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    QMT06_0100 data = new QMT06_0100();

                    data.qmt06_0100 = reader.GetInt32(reader.GetOrdinal("qmt06_0100"));
                    data.oqm_code = reader.GetString(reader.GetOrdinal("oqm_code"));
                    data.qtest_item_code = reader.GetString(reader.GetOrdinal("qtest_item_code"));
                    data.scr_no = reader.GetString(reader.GetOrdinal("scr_no"));
                    data.qtest_up = reader.GetDecimal(reader.GetOrdinal("qtest_up"));
                    data.qtest_down = reader.GetDecimal(reader.GetOrdinal("qtest_down"));
                    data.qtest_item_type = reader.GetString(reader.GetOrdinal("qtest_item_type"));
                    data.is_ok = reader.GetString(reader.GetOrdinal("is_ok"));
                    data.ins_date = reader.GetString(reader.GetOrdinal("ins_date"));
                    data.ins_time = reader.GetString(reader.GetOrdinal("ins_time"));
                    data.usr_code = reader.GetString(reader.GetOrdinal("usr_code"));
                    data.sample_sum_qty = reader.GetDecimal(reader.GetOrdinal("sample_sum_qty"));
                    data.can_delete = "Y";
                    data.can_update = "Y";

                    list.Add(data);
                }

            }
            return list;
        }

        /// <summary>
        /// 取得使用者可以編輯的資料，結合商務邏輯權限
        /// </summary>
        /// <param name="pUsrCode"></param>
        /// <param name="pPrgCode"></param>
        /// <returns></returns>
        public List<QMT06_0100> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);

            List<QMT06_0100> list = new List<QMT06_0100>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = "SELECT * FROM QMT06_0100 order by ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    QMT06_0100 data = new QMT06_0100();

                    data.qmt06_0100 = reader.GetInt32(reader.GetOrdinal("qmt06_0100"));
                    data.oqm_code = reader.GetString(reader.GetOrdinal("oqm_code"));
                    data.qtest_item_code = reader.GetString(reader.GetOrdinal("qtest_item_code"));
                    data.scr_no = reader.GetString(reader.GetOrdinal("scr_no"));
                    data.qtest_up = reader.GetDecimal(reader.GetOrdinal("qtest_up"));
                    data.qtest_down = reader.GetDecimal(reader.GetOrdinal("qtest_down"));
                    data.qtest_item_type = reader.GetString(reader.GetOrdinal("qtest_item_type"));
                    data.is_ok = reader.GetString(reader.GetOrdinal("is_ok"));
                    data.ins_date = reader.GetString(reader.GetOrdinal("ins_date"));
                    data.ins_time = reader.GetString(reader.GetOrdinal("ins_time"));
                    data.usr_code = reader.GetString(reader.GetOrdinal("usr_code"));
                    data.sample_sum_qty = reader.GetDecimal(reader.GetOrdinal("sample_sum_qty"));

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
        /// 根據pTkCode取得QMT06_0100資料表內容，並結合使用者權限
        /// </summary>
        /// <param name="pUsrCode">使用者代號</param>
        /// <param name="pPrgCode">功能代號</param>
        /// <param name="pTkCode">要抓取的條件，field value</param>
        /// <returns></returns>
        public List<QMT06_0100> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<QMT06_0100> list = new List<QMT06_0100>();
            string foreignKey = gmv.GetKey<QMT06_0000>(new QMT06_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT *, QMB02_0000.qtest_item_name, QMB02_0000.qtest_type_code, QMB01_0000.qtest_type_name, A.field_name as qtest_item_type_name, B.usr_name " +
                       " FROM QMT06_0100 " +
                       " left join QMB02_0000 on QMB02_0000.qtest_item_code = QMT06_0100.qtest_item_code " +
                       " left join QMB01_0000 on QMB01_0000.qtest_type_code = QMB02_0000.qtest_type_code " +
                       " left join BDP21_0100 as A on A.field_code = QMT06_0100.qtest_item_type and A.code_code = 'qtest_item_type' " +
                       " left join BDP08_0000 as B on B.usr_code = QMT06_0100.usr_code " +
                       " where " + foreignKey + "=@" + foreignKey +
                       " order by QMT06_0100.scr_no";
            }
            else
            {
                sSql = " SELECT * FROM QMT06_0100 ";
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
                    QMT06_0100 data = new QMT06_0100();
                    data.qmt06_0100 = comm.sGetInt32(reader["qmt06_0100"].ToString());
                    //data.qmt_value = "<a href='#' id='" + data.qmt04_0100 + "' class='qmt_code'><i class='ace-icon fa fa-pencil-square-o bigger-150'></i></a>";
                    data.oqm_code = comm.sGetString(reader["oqm_code"].ToString());
                    data.qtest_item_code = comm.sGetString(reader["qtest_item_code"].ToString());
                    data.qtest_item_name = comm.sGetString(reader["qtest_item_name"].ToString());
                    data.scr_no = comm.sGetString(reader["scr_no"].ToString());
                    data.qtest_up = comm.sGetDecimal(reader["qtest_up"].ToString());
                    data.qtest_down = comm.sGetDecimal(reader["qtest_down"].ToString());
                    data.qtest_item_type = comm.sGetString(reader["qtest_item_type"].ToString());
                    data.qtest_item_type_name = comm.sGetString(reader["qtest_item_type_name"].ToString());
                    data.ins_date = comm.sGetString(reader["is_ok"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                    data.usr_name = comm.sGetString(reader["usr_name"].ToString());
                    data.sample_sum_qty = comm.sGetDecimal(reader["sample_sum_qty"].ToString());

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
        /// 傳入一個QMT06_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="QMT06_0100">DTO</param>
        public void InsertData(QMT06_0100 QMT06_0100)
        {
            string sSql = "INSERT INTO " +
                          " QMT06_0100 ( oqm_code,  qtest_item_code,  scr_no,  qtest_up,  qtest_down, " +
                          "              qtest_item_type, is_ok, ins_date,  ins_time,  usr_code ) " +

                          "     VALUES (@oqm_code, @qtest_item_code, @scr_no, @qtest_up, @qtest_down, " +
                          "             @qtest_item_type, @is_ok,@ins_date, @ins_time, @usr_code ) ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, QMT06_0100);
            }
        }

        /// <summary>
        /// 傳入一個QMT06_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="QMT06_0100">DTO</param>
        public void UpdateData(QMT06_0100 QMT06_0100)
        {
            string sSql = " UPDATE QMT06_0100                          " +
                          "    SET oqm_code        = @oqm_code,        " +
                          "        qtest_item_code = @qtest_item_code, " +
                          "        scr_no          = @scr_no,          " +
                          "        qtest_up        = @qtest_up,        " +
                          "        qtest_down      = @qtest_down,      " +
                          "        qtest_item_type = @qtest_item_type, " +
                          "        is_ok           = @is_ok,           " +
                          "        ins_date        = @ins_date,        " +
                          "        ins_time        = @ins_time,        " +
                          "        usr_code        = @usr_code         " +
                          "  WHERE qmt06_0100      = @qmt06_0100       " ;

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, QMT06_0100);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM QMT06_0100 WHERE qmt06_0100 = @qmt06_0100";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { qmt06_0100 = pTkCode });
            }
        }



        /// <summary>
        /// 加總樣品數與缺陷數
        /// </summary>
        /// <param name="qmt04_0100">識別碼</param>
        public decimal  Get_NgQty(int qmt04_0100)
        {
            string sSql = "";
            DataTable dtTmp = new DataTable();
            object data = new object();
            decimal val = 0;
            sSql = @"select isnull(sum(sample_qty),0) as qty 
                       from QMT06_0110
                      where qmt04_0100 = @qmt04_0100
                        and is_ok = 'N'";
            dtTmp = comm.Get_DataTable(sSql, "qmt04_0100", qmt04_0100.ToString());
            if (dtTmp.Rows.Count > 0)
            {
                val = comm.sGetDecimal(dtTmp.Rows[0]["qty"].ToString());
            }
            return val;
        }





    }
}