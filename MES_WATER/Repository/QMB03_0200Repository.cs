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
    public class QMB03_0200Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得QMB03_0200資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO QMB03_0200</returns>
        public QMB03_0200 GetDTO(string pTkCode)
        {
            QMB03_0200 datas = new QMB03_0200();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM QMB03_0200";
            }
            else
            {
                sSql = "SELECT * FROM QMB03_0200 where qmb03_0200=@qmb03_0200";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@qmb03_0200", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new QMB03_0200
                        {

                            qmb03_0200 = comm.sGetInt32(reader["qmb03_0200"].ToString()),
                            qsheet_code = comm.sGetString(reader["qsheet_code"].ToString()),
                            pro_code = comm.sGetString(reader["pro_code"].ToString()),

                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得QMB03_0200資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List QMB03_0200</returns>
        public List<QMB03_0200> Get_DataList(string pTkCode)
        {
            List<QMB03_0200> list = new List<QMB03_0200>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM QMB03_0200";
            }
            else
            {
                sSql = "SELECT * FROM QMB03_0200 where qmb03_0200=@qmb03_0200";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@qmb03_0200", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    QMB03_0200 data = new QMB03_0200();

                    data.qmb03_0200 = comm.sGetInt32(reader["qmb03_0200"].ToString());
                    data.qsheet_code = comm.sGetString(reader["qsheet_code"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());


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
        public List<QMB03_0200> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_qmb03_0200", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<QMB03_0200> list = new List<QMB03_0200>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = "SELECT * FROM QMB03_0200";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@qmb03_0200", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    QMB03_0200 data = new QMB03_0200();

                    data.qmb03_0200 = comm.sGetInt32(reader["qmb03_0200"].ToString());
                    data.qsheet_code = comm.sGetString(reader["qsheet_code"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.qmb03_0200)) {
                    //    data.can_delete = "N";
                    //    data.can_update = "N";
                    //}

                    list.Add(data);
                }
            }
            return list;
        }
        #endregion

        /// <summary>
        /// 取得查詢資料，結合使用者權限
        /// </summary>
        /// <param name="pUsrCode">使用者代碼</param>
        /// <param name="pPrgCode">功能代碼</param>
        /// <param name="pWhere">JSON查詢字串</param>
        /// <returns></returns>
        public List<QMB03_0200> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<QMB03_0200> list = new List<QMB03_0200>();

            string sSql = " SELECT QMB03_0200.*, MEB20_0000.pro_name, QMB03_0000.qsheet_name " +
                          " FROM QMB03_0200 " +
                          " left join MEB20_0000 on MEB20_0000.pro_code = QMB03_0200.pro_code " +
                          " left join QMB03_0000 on QMB03_0000.qsheet_code = QMB03_0200.qsheet_code ";

            // 取得資料
            list = comm.Get_ListByQuery<QMB03_0200>(sSql, pWhere, pUsrCode, pPrgCode);

            // 權限設定
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            //string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_mtp_code", "par_name", "par_value");
            //var arr_LockGrpCode = sLockGrpCode.Split(',');

            for (int i = 0; i < list.Count; i++)
            {
                //檢查授權刪除、修改
                list[i].can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                list[i].can_update = sLimitStr.Contains("M") ? "Y" : "N";

                //        // 特例 轉換
                //        data.sup_name = data.sup_code + " - " + comm.sGetString(reader["sup_name"].ToString());
                //        data.sto_name = comm.sGetString(reader["sto_code"].ToString()) + " - " + comm.sGetString(reader["sto_name"].ToString());

                //        data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                //        data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                //        //資料邏輯刪除、修改
                //        //if (arr_LockGrpCode.Contains(data.mtp_code)) {
                //        //    data.can_delete = "N";
                //        //    data.can_update = "N";
                //        //}
            }

            return list;

        }

        /// <summary>
        /// 傳入一個QMB03_0200的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="QMB03_0200">DTO</param>
        public void InsertData(QMB03_0200 QMB03_0200)
        {
            string sSql = "INSERT INTO " +
                          " QMB03_0200 (  qsheet_code,  pro_code ) " +
                          "     VALUES ( @qsheet_code, @pro_code ) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, QMB03_0200);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@qmb03_0200", QMB03_0200.qmb03_0200));
                //sqlCommand.Parameters.Add(new SqlParameter("@qmb03_0200", QMB03_0200.qmb03_0200));
                //sqlCommand.Parameters.Add(new SqlParameter("@area_name", QMB03_0200.area_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個QMB03_0200的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="QMB03_0200">DTO</param>
        public void UpdateData(QMB03_0200 QMB03_0200)
        {
            string sSql = " UPDATE QMB03_0200                    " +
                          "    SET qsheet_code  =  @qsheet_code, " +
                          "        pro_code     =  @pro_code     " +
                          "  WHERE qmb03_0200   =  @qmb03_0200   " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, QMB03_0200);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@qmb03_0200", QMB03_0200.qmb03_0200));
                //sqlCommand.Parameters.Add(new SqlParameter("@qmb03_0200", QMB03_0200.qmb03_0200));
                //sqlCommand.Parameters.Add(new SqlParameter("@area_name", QMB03_0200.area_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM QMB03_0200 WHERE qmb03_0200 = @qmb03_0200; " ;
            //sSql += " Delete from BDP09_0100 where qmb03_0200 = @qmb03_0200; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { qmb03_0200 = pTkCode });

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@qmb03_0200", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }



        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得QMB03_0200角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetQMB03_0200_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("qmb03_0200", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("qmb03_0200", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("area_name", System.Type.GetType("System.String"].ToString());

            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM QMB03_0200";
            }
            else
            {
                sSql = "SELECT * FROM QMB03_0200 where qmb03_0200='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["qmb03_0200"] = dtTmp.Rows[i]["qmb03_0200"];
                drow["qmb03_0200"] = dtTmp.Rows[i]["qmb03_0200"];
                drow["area_name"] = dtTmp.Rows[i]["area_name"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}