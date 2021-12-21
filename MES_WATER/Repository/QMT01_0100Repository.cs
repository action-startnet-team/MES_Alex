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
    public class QMT01_0100Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得QMT01_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO QMT01_0100</returns>
        public QMT01_0100 GetDTO(string pTkCode)
        {
            QMT01_0100 datas = new QMT01_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM QMT01_0100";
            }
            else
            {
                sSql = "SELECT * FROM QMT01_0100 where qmt01_0100=@qmt01_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@qmt01_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new QMT01_0100
                        {
                            qmt01_0100 = comm.sGetInt32(reader["qmt01_0100"].ToString()),
                            qmt_code = comm.sGetString(reader["qmt_code"].ToString()),
                            qtest_item_name = comm.sGetString(reader["qtest_item_name"].ToString()),
                            qtest_item_memo = comm.sGetString(reader["qtest_item_memo"].ToString()),
                            qtest_type_code = comm.sGetString(reader["qtest_type_code"].ToString()),
                            qtest_unit = comm.sGetString(reader["qtest_unit"].ToString()),
                            qtest_up = comm.sGetDecimal(reader["qtest_up"].ToString()),
                            qtest_down = comm.sGetDecimal(reader["qtest_down"].ToString()),
                            qtest_item_type = comm.sGetString(reader["qtest_item_type"].ToString()),
                            VORNR = comm.sGetString(reader["VORNR"].ToString()),
                            MERKNR = comm.sGetString(reader["MERKNR"].ToString()),
                            AUSWMENGE1 = comm.sGetString(reader["AUSWMENGE1"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得QMT01_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List QMT01_0100</returns>
        public List<QMT01_0100> Get_DataList(string pTkCode)
        {
            List<QMT01_0100> list = new List<QMT01_0100>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM QMT01_0100";
            }
            else
            {
                sSql = "SELECT * FROM QMT01_0100 where qmt01_0100=@qmt01_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@qmt01_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    QMT01_0100 data = new QMT01_0100();

                 
                    data.qmt01_0100 = comm.sGetInt32(reader["qmt01_0100"].ToString());
                    data.qmt_code = comm.sGetString(reader["qmt_code"].ToString());
                    data.qtest_item_name = comm.sGetString(reader["qtest_item_name"].ToString());
                    data.qtest_item_memo = comm.sGetString(reader["qtest_item_memo"].ToString());
                    data.qtest_type_code = comm.sGetString(reader["qtest_type_code"].ToString());
                    data.qtest_unit = comm.sGetString(reader["qtest_unit"].ToString());
                    data.qtest_up = comm.sGetDecimal(reader["qtest_up"].ToString());
                    data.qtest_down = comm.sGetDecimal(reader["qtest_down"].ToString());
                    data.qtest_item_type = comm.sGetString(reader["qtest_item_type"].ToString());
                    data.VORNR = comm.sGetString(reader["VORNR"].ToString());
                    data.MERKNR = comm.sGetString(reader["MERKNR"].ToString());
                    data.AUSWMENGE1 = comm.sGetString(reader["AUSWMENGE1"].ToString());

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
        public List<QMT01_0100> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_qmt01_0100", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<QMT01_0100> list = new List<QMT01_0100>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = " SELECT * FROM QMT01_0100 " ;

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@qmt01_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    QMT01_0100 data = new QMT01_0100();

                    data.qmt01_0100 = comm.sGetInt32(reader["qmt01_0100"].ToString());
                    data.qmt_code = comm.sGetString(reader["qmt_code"].ToString());
                    data.qtest_item_name = comm.sGetString(reader["qtest_item_name"].ToString());
                    data.qtest_item_memo = comm.sGetString(reader["qtest_item_memo"].ToString());
                    data.qtest_type_code = comm.sGetString(reader["qtest_type_code"].ToString());
                    data.qtest_unit = comm.sGetString(reader["qtest_unit"].ToString());
                    data.qtest_up = comm.sGetDecimal(reader["qtest_up"].ToString());
                    data.qtest_down = comm.sGetDecimal(reader["qtest_down"].ToString());
                    data.qtest_item_type = comm.sGetString(reader["qtest_item_type"].ToString());
                    data.VORNR = comm.sGetString(reader["VORNR"].ToString());
                    data.MERKNR = comm.sGetString(reader["MERKNR"].ToString());
                    data.AUSWMENGE1 = comm.sGetString(reader["AUSWMENGE1"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.qmt01_0100)) {
                    //    data.can_delete = "N";
                    //    data.can_update = "N";
                    //}

                    list.Add(data);
                }
            }
            return list;
        }
        #endregion
        public List<QMT01_0100> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<QMT01_0100> list = new List<QMT01_0100>();
            string foreignKey = gmv.GetKey<QMT01_0000>(new QMT01_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT QMT01_0100.*, BDP21_0100.field_name as qtest_item_type_name              " +
                       " FROM QMT01_0100                                                                                           " +
                       " left join BDP21_0100 on BDP21_0100.field_code = QMT01_0100.qtest_item_type and BDP21_0100.code_code = 'qtest_item_type' " +
                       " where QMT01_0100. " + foreignKey + "=@" + foreignKey ;
            }
            else
            { 
                sSql = "SELECT * FROM QMT01_0100";
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
                    
                    QMT01_0100 data = new QMT01_0100();

                    data.qmt01_0100 = comm.sGetInt32(reader["qmt01_0100"].ToString());
                    data.qmt_code = comm.sGetString(reader["qmt_code"].ToString());
                    data.qtest_item_name = comm.sGetString(reader["qtest_item_name"].ToString());
                    data.qtest_item_memo = comm.sGetString(reader["qtest_item_memo"].ToString());
                    data.qtest_type_code = comm.sGetString(reader["qtest_type_code"].ToString());
                    data.qtest_unit = comm.sGetString(reader["qtest_unit"].ToString());
                    data.qtest_up = comm.sGetDecimal(reader["qtest_up"].ToString());
                    data.qtest_down = comm.sGetDecimal(reader["qtest_down"].ToString());
                    data.qtest_item_type = comm.sGetString(reader["qtest_item_type"].ToString());
                    data.qtest_item_type_name = comm.sGetString(reader["qtest_item_type_name"].ToString());
                    data.VORNR = comm.sGetString(reader["VORNR"].ToString());
                    data.MERKNR = comm.sGetString(reader["MERKNR"].ToString());
                    data.AUSWMENGE1 = comm.sGetString(reader["AUSWMENGE1"].ToString());

                    

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
        /// 傳入一個QMT01_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="QMT01_0100">DTO</param>
        public void InsertData(QMT01_0100 QMT01_0100)
        {
            string sSql = "INSERT INTO " +
                          " QMT01_0100 (  qmt01_0100,      loc_name,   qmt_code,     qtest_item_type ) " +
                          "     VALUES ( @qmt01_0100,     @loc_name,   @qmt_code,   @qtest_item_type ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, QMT01_0100);
            }
        }

        /// <summary>
        /// 傳入一個QMT01_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="QMT01_0100">DTO</param>
        public void UpdateData(QMT01_0100 QMT01_0100)
        {
            //string pTkCode = QMT01_0100.qmt01_0100.ToString();
            //Int32 iProQty = comm.sGetInt32(comm.Get_Data("QMT01_0100", pTkCode, "qmt01_0100", "pro_qty"));
            //Int32 iSorSerial = comm.sGetInt32(comm.Get_Data("QMT01_0100", pTkCode, "qmt01_0100", "sor_serial"));

            //ws.Cal_TraQty("DEL", "STT01_0100", "res_qty", iProQty, "where stt01_0100=" + iSorSerial);
            //ws.Cal_TraQty("ADD", "STT01_0100", "res_qty", comm.sGetInt32(QMT01_0100.pro_qty.ToString()), "where stt01_0100=" + comm.sGetString(QMT01_0100.sor_serial.ToString()));


            string sSql = " UPDATE QMT01_0100            " +
                          "    SET loc_name = @loc_name, " +
                          "        qmt_code = @qmt_code, " +
                          "        qtest_item_type = @qtest_item_type  " +
                          "  WHERE qmt01_0100 = @qmt01_0100  " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, QMT01_0100);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@qmt01_0100", QMT01_0100.qmt01_0100));
                //sqlCommand.Parameters.Add(new SqlParameter("@qmt01_0100", QMT01_0100.qmt01_0100));
                //sqlCommand.Parameters.Add(new SqlParameter("@loc_name", QMT01_0100.loc_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM QMT01_0100 WHERE qmt01_0100 = @qmt01_0100;";
            //sSql += " Delete from BDP09_0100 where qmt01_0100 = @qmt01_0100; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { qmt01_0100 = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@qmt01_0100", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }
        
        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得QMT01_0100角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetQMT01_0100_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();
            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("qmt01_0100", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("qmt01_0100", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("loc_name", System.Type.GetType("System.String"].ToString());
            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM QMT01_0100";
            }
            else
            {
                sSql = "SELECT * FROM QMT01_0100 where qmt01_0100='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["qmt01_0100"] = dtTmp.Rows[i]["qmt01_0100"];
                drow["qmt01_0100"] = dtTmp.Rows[i]["qmt01_0100"];
                drow["loc_name"] = dtTmp.Rows[i]["loc_name"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}