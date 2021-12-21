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
    public class QMB03_0100Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得QMB03_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO QMB03_0100</returns>
        public QMB03_0100 GetDTO(string pTkCode)
        {
            QMB03_0100 datas = new QMB03_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM QMB03_0100";
            }
            else
            {
                sSql = "SELECT * FROM QMB03_0100 where qmb03_0100=@qmb03_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@qmb03_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new QMB03_0100
                        {
                            qmb03_0100 = comm.sGetInt32(reader["qmb03_0100"].ToString()),
                            qsheet_code = comm.sGetString(reader["qsheet_code"].ToString()),
                            qtest_item_code = comm.sGetString(reader["qtest_item_code"].ToString()),
                            qtest_item_type = comm.sGetString(reader["qtest_item_type"].ToString()),
                            scr_no = comm.sGetString(reader["scr_no"].ToString()),
                            datacode = comm.sGetString(reader["datacode"].ToString()),
                            qtest_up = comm.sGetDecimal(reader["qtest_up"].ToString()),
                            qtest_down = comm.sGetDecimal(reader["qtest_down"].ToString()),
                            work_code = comm.sGetString(reader["work_code"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得QMB03_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List QMB03_0100</returns>
        public List<QMB03_0100> Get_DataList(string pTkCode)
        {
            List<QMB03_0100> list = new List<QMB03_0100>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM QMB03_0100";
            }
            else
            {
                sSql = "SELECT * FROM QMB03_0100 where qmb03_0100=@qmb03_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@qmb03_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    QMB03_0100 data = new QMB03_0100();

                    data.qmb03_0100 = comm.sGetInt32(reader["qmb03_0100"].ToString());
                    data.qsheet_code = comm.sGetString(reader["qsheet_code"].ToString());
                    data.qtest_item_code = comm.sGetString(reader["qtest_item_code"].ToString());
                    data.qtest_item_type = comm.sGetString(reader["qtest_item_type"].ToString());
                    data.scr_no = comm.sGetString(reader["scr_no"].ToString());
                    data.datacode = comm.sGetString(reader["datacode"].ToString());
                    data.qtest_up = comm.sGetDecimal(reader["qtest_up"].ToString());
                    data.qtest_down = comm.sGetDecimal(reader["qtest_down"].ToString());
                    data.tool_code = comm.sGetString(reader["tool_code"].ToString());
                    data.unit_code = comm.sGetString(reader["unit_code"].ToString());
                    data.qtest_rate = comm.sGetString(reader["qtest_rate"].ToString());
                    data.qtest_memo = comm.sGetString(reader["qtest_memo"].ToString());
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
        public List<QMB03_0100> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_qmb03_0100", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<QMB03_0100> list = new List<QMB03_0100>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = " SELECT * FROM QMB03_0100 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    QMB03_0100 data = new QMB03_0100();

                    data.qmb03_0100 = comm.sGetInt32(reader["qmb03_0100"].ToString());
                    data.qsheet_code = comm.sGetString(reader["qsheet_code"].ToString());
                    data.qtest_item_code = comm.sGetString(reader["qtest_item_code"].ToString());
                    data.scr_no = comm.sGetString(reader["scr_no"].ToString());
                    data.datacode = comm.sGetString(reader["datacode"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";
                    
                    list.Add(data);
                }
            }
            return list;
        }
        #endregion

        public List<QMB03_0100> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<QMB03_0100> list = new List<QMB03_0100>();
            string foreignKey = gmv.GetKey<QMB03_0000>(new QMB03_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT QMB03_0100.*, QMB02_0000.qtest_item_name, BDP21_0100.field_name as qtest_item_type_name, QMB16_0000.tool_name, QMB17_0000.unit_name, MEB30_0000.work_name" +
                       " FROM QMB03_0100 " +
                       " left join QMB02_0000 on QMB02_0000.qtest_item_code = QMB03_0100.qtest_item_code " +
                       " left join BDP21_0100 on BDP21_0100.field_code = QMB03_0100.qtest_item_type and BDP21_0100.code_code = 'qtest_item_type' " +
                       " left join QMB16_0000 on QMB16_0000.tool_code = QMB03_0100.tool_code " +
                       " left join QMB17_0000 on QMB17_0000.unit_code = QMB03_0100.unit_code " +
                       " left join MEB30_0000 on MEB30_0000.work_code = QMB03_0100.work_code " +
                       " where QMB03_0100. " + foreignKey + "=@" + foreignKey +
                       " order by scr_no " ;
            }
            else
            {
                sSql = "SELECT * FROM QMB03_0100";
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

                    QMB03_0100 data = new QMB03_0100();

                    data.qmb03_0100 = comm.sGetInt32(reader["qmb03_0100"].ToString());
                    data.qsheet_code = comm.sGetString(reader["qsheet_code"].ToString());
                    data.qtest_item_code = comm.sGetString(reader["qtest_item_code"].ToString());
                    data.qtest_item_name = comm.sGetString(reader["qtest_item_name"].ToString());
                    data.qtest_item_type = comm.sGetString(reader["qtest_item_type"].ToString());
                    data.qtest_item_type_name = comm.sGetString(reader["qtest_item_type_name"].ToString());
                    data.scr_no = comm.sGetString(reader["scr_no"].ToString());
                    data.datacode = comm.sGetString(reader["datacode"].ToString());
                    data.qtest_up = comm.sGetDecimal(reader["qtest_up"].ToString());
                    data.qtest_down = comm.sGetDecimal(reader["qtest_down"].ToString());
                    data.tool_code = comm.sGetString(reader["tool_code"].ToString());
                    data.tool_name = comm.sGetString(reader["tool_name"].ToString());
                    data.unit_code = comm.sGetString(reader["unit_code"].ToString());
                    data.unit_name = comm.sGetString(reader["unit_name"].ToString());
                    data.qtest_rate = comm.sGetString(reader["qtest_rate"].ToString());
                    data.qtest_memo = comm.sGetString(reader["qtest_memo"].ToString());
                    data.work_code = comm.sGetString(reader["work_code"].ToString());
                    data.work_name = comm.sGetString(reader["work_name"].ToString());
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
        /// 傳入一個QMB03_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="QMB03_0100">DTO</param>
        public void InsertData(QMB03_0100 QMB03_0100)
        {
            string sSql = "INSERT INTO " +
                          " QMB03_0100 (  qsheet_code,  qtest_item_code,  qtest_item_type,  scr_no,  datacode,  qtest_up,  qtest_down , "+
                          " tool_code, unit_code, qtest_rate, qtest_memo , work_code ) " +
                          "     VALUES ( @qsheet_code, @qtest_item_code, @qtest_item_type, @scr_no, @datacode, @qtest_up, @qtest_down, "+
                          " @tool_code, @unit_code, @qtest_rate, @qtest_memo , @work_code  ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, QMB03_0100);
            }
        }

        /// <summary>
        /// 傳入一個QMB03_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="QMB03_0100">DTO</param>
        public void UpdateData(QMB03_0100 QMB03_0100)
        {
            string sSql = " UPDATE QMB03_0100                           " +
                          "    SET qtest_item_code =  @qtest_item_code, " +
                          "        qtest_item_type =  @qtest_item_type, " +
                          "        scr_no          =  @scr_no,          " +
                          "        datacode        =  @datacode,        " +
                          "        qtest_up        =  @qtest_up,        " +
                          "        qtest_down      =  @qtest_down,      " +
                          "        tool_code       =  @tool_code,       " +
                          "        unit_code       =  @unit_code,       " +
                          "        qtest_rate      =  @qtest_rate,      " +
                          "        qtest_memo      =  @qtest_memo,      " +
                          "        work_code       =  @work_code        " +
                          "  WHERE qmb03_0100      =  @qmb03_0100       " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, QMB03_0100);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM QMB03_0100 WHERE qmb03_0100 = @qmb03_0100 ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { qmb03_0100 = pTkCode });
            }
        }

    }
}