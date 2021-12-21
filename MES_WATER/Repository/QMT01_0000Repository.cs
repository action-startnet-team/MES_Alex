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
    public class QMT01_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得QMT01_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO QMT01_0000</returns>
        public QMT01_0000 GetDTO(string pTkCode)
        {
            QMT01_0000 datas = new QMT01_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT  * FROM QMT01_0000";
            }
            else
            {
                sSql = "SELECT * FROM QMT01_0000 where qmt_code=@qmt_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@qmt_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new QMT01_0000
                        {

                            qmt_code = comm.sGetString(reader["qmt_code"].ToString()),
                            pur_code = comm.sGetString(reader["pur_code"].ToString()),
                            scr_no = comm.sGetInt32(reader["scr_no"].ToString()),
                            pro_code = comm.sGetString(reader["pro_code"].ToString()),
                            PLNTY = comm.sGetString(reader["PLNTY"].ToString()),
                            PLNNR = comm.sGetString(reader["PLNNR"].ToString()),
                            PLNAL = comm.sGetString(reader["PLNAL"].ToString()),
                            PLNKN = comm.sGetString(reader["PLNKN"].ToString()),
                            ins_date = comm.sGetString(reader["ins_date"].ToString()),
                            ins_time = comm.sGetString(reader["ins_time"].ToString()),
                            usr_code = comm.sGetString(reader["usr_code"].ToString()),

                        };
                    }
                }
            }
            return datas;
        }

        /// <summary>
        /// 取得QMT01_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List QMT01_0000</returns>
        public List<QMT01_0000> Get_DataList(string pTkCode)
        {
            List<QMT01_0000> list = new List<QMT01_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM QMT01_0000";
            }
            else
            {
                sSql = "SELECT * FROM QMT01_0000 where qmt_code=@qmt_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@qmt_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    QMT01_0000 data = new QMT01_0000();

                    data.qmt_code = comm.sGetString(reader["qmt_code"].ToString());
                    data.pur_code = comm.sGetString(reader["pur_code"].ToString());
                    data.scr_no = comm.sGetInt32(reader["scr_no"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.PLNTY = comm.sGetString(reader["PLNTY"].ToString());
                    data.PLNNR = comm.sGetString(reader["PLNNR"].ToString());
                    data.PLNAL = comm.sGetString(reader["PLNAL"].ToString());
                    data.PLNKN = comm.sGetString(reader["PLNKN"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());


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
        public List<QMT01_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_qmt_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<QMT01_0000> list = new List<QMT01_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = " SELECT * FROM QMT01_0000 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@qmt_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    QMT01_0000 data = new QMT01_0000();

                    data.qmt_code = comm.sGetString(reader["qmt_code"].ToString());
                    data.pur_code = comm.sGetString(reader["pur_code"].ToString());
                    data.scr_no = comm.sGetInt32(reader["scr_no"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.PLNTY = comm.sGetString(reader["PLNTY"].ToString());
                    data.PLNNR = comm.sGetString(reader["PLNNR"].ToString());
                    data.PLNAL = comm.sGetString(reader["PLNAL"].ToString());
                    data.PLNKN = comm.sGetString(reader["PLNKN"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.qmt_code)) {
                    //    data.can_delete = "N";
                    //    data.can_update = "N";
                    //}

                    list.Add(data);
                }
            }
            return list;
        }

        /// <summary>
        /// 取得查詢資料，結合使用者權限
        /// </summary>
        /// <param name="pUsrCode">使用者代碼</param>
        /// <param name="pPrgCode">功能代碼</param>
        /// <param name="pWhere">JSON查詢字串</param>
        /// <returns></returns>
        public List<QMT01_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<QMT01_0000> list = new List<QMT01_0000>();/*, B.field_name as loc_type_name*/

            string sSql = " SELECT distinct QMT01_0000.qmt_code, QMT01_0000.* ,  MEB20_0000.pro_name as pro_name  ,BDP08_0000.usr_name as usr_name                 " +
                          " FROM QMT01_0000                                                                                                " +
                          " left join QMT01_0100 on QMT01_0100.qmt_code = QMT01_0000.qmt_code                            " +
                          " left join BDP08_0000 on BDP08_0000.usr_code = QMT01_0000.usr_code                                   " +
                          " left join MEB20_0000 on MEB20_0000.pro_code = QMT01_0000.pro_code                                   ";
            // 取得資料
            list = comm.Get_ListByQuery<QMT01_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
                //        data.sup_name = data.pro_code + " - " + comm.sGetString(reader["sup_name"].ToString());
                //        data.pur_code = comm.sGetString(reader["qmt_code"].ToString()) + " - " + comm.sGetString(reader["pur_code"].ToString());

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
        /// 傳入一個QMT01_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="QMT01_0000">DTO</param>
        public void InsertData(QMT01_0000 QMT01_0000)
        {
            string sSql = " INSERT INTO " +
                          " QMT01_0000 (  qmt_code,  pur_code,  scr_no,  pro_code ) " +
                          "     VALUES ( @qmt_code, @pur_code, @scr_no, @pro_code ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, QMT01_0000);
            }
        }

        /// <summary>
        /// 傳入一個QMT01_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="QMT01_0000">DTO</param>
        public void UpdateData(QMT01_0000 QMT01_0000)
        {
            string sSql = " UPDATE QMT01_0000                 " +
                          "    SET pur_code =  @pur_code,     " +
                          "        scr_no  =  @scr_no,      " +
                          "        pro_code =  @pro_code      " +
                          "  WHERE qmt_code =  @qmt_code      ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, QMT01_0000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@qmt_code", QMT01_0000.qmt_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@qmt_code", QMT01_0000.qmt_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@pur_code", QMT01_0000.pur_code));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM QMT01_0000 WHERE qmt_code = @qmt_code; " +
                          " DELETE FROM QMT01_0100 WHERE qmt_code = @qmt_code; " ;
            //sSql += " Delete from BDP09_0100 where qmt_code = @qmt_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { qmt_code = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@qmt_code", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        ////暫存DataTable參考
        //// <summary>
        //// 取得QMT01_0000角色的DataTable
        //// </summary>
        //// <param name = "pTkCode" > 有傳鍵值取一筆，鍵值空白取全部</param>
        //// <returns></returns>
        //public DataTable GetQMT01_0000_dt(string pTkCode)
        //{
        //    DataTable dtTmp = new DataTable();
        //    DataTable dtDat = new DataTable();
        //    dtDat.Columns.Add("qmt_code", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("qmt_code", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("pur_code", System.Type.GetType("System.String"].ToString());

        //    string sSql = "";
        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM QMT01_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM QMT01_0000 where qmt_code='" + pTkCode + "'";
        //    }
        //    dtTmp = comm.Get_DataTable(sSql);

        //    int i;
        //    for (i = 1; i < dtTmp.Rows.Count - 1; i++)
        //    {
        //        DataRow drow = dtDat.NewRow();
        //        drow["qmt_code"] = dtTmp.Rows[i]["qmt_code"];
        //        drow["qmt_code"] = dtTmp.Rows[i]["qmt_code"];
        //        drow["pur_code"] = dtTmp.Rows[i]["pur_code"];
        //        dtDat.Rows.Add(drow);
        //    }
        //    return dtDat;
        //}
    }
}