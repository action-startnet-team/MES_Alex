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
    public class QMT03_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得QMT03_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO QMT03_0000</returns>
        public QMT03_0000 GetDTO(string pTkCode)
        {
            QMT03_0000 datas = new QMT03_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM QMT03_0000 ";
            }
            else
            {
                sSql = "SELECT * FROM QMT03_0000 where qmt_code=@qmt_code ";
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
                        datas = new QMT03_0000
                        {
                            qmt_code = comm.sGetString(reader["qmt_code"].ToString()),
                            rel_type = comm.sGetString(reader["rel_type"].ToString()),
                            rel_code = comm.sGetString(reader["rel_code"].ToString()),
                            pro_code = comm.sGetString(reader["pro_code"].ToString()),
                            lot_no = comm.sGetString(reader["lot_no"].ToString()),
                            scr_no = comm.sGetInt32(reader["scr_no"].ToString()),
                            is_rec = comm.sGetString(reader["is_rec"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }
        #region
        ///// <summary>
        ///// 取得QMT03_0000資料表內容
        ///// </summary>
        ///// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        ///// < returns > List QMT03_0000</returns>
        //public List<QMT03_0000> Get_DataList(string pTkCode)
        //{
        //    List<QMT03_0000> list = new List<QMT03_0000>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM QMT03_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM QMT03_0000 where qmt_code=@qmt_code";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@qmt_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            QMT03_0000 data = new QMT03_0000();

        //            data.qmt_code = comm.sGetString(reader["qmt_code"].ToString());
        //            data.rel_type = comm.sGetString(reader["rel_type"].ToString());
        //            data.rel_code = comm.sGetString(reader["rel_code"].ToString());
        //            data.pro_code = comm.sGetString(reader["pro_code"].ToString());
        //            data.lot_no = comm.sGetString(reader["lot_no"].ToString());
        //            data.scr_no = comm.sGetInt32(reader["scr_no"].ToString());
        //            data.qsheet_code = comm.sGetString(reader["qsheet_code"].ToString());
        //            data.is_ok = comm.sGetString(reader["is_ok"].ToString());

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
        //public List<QMT03_0000> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_qmt_code", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<QMT03_0000> list = new List<QMT03_0000>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    sSql = " SELECT * FROM QMT03_0000 ";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            QMT03_0000 data = new QMT03_0000();

        //            data.qmt_code = comm.sGetString(reader["qmt_code"].ToString());
        //            data.rel_type = comm.sGetString(reader["rel_type"].ToString());
        //            data.rel_code = comm.sGetString(reader["rel_code"].ToString());
        //            data.pro_code = comm.sGetString(reader["pro_code"].ToString());
        //            data.lot_no = comm.sGetString(reader["lot_no"].ToString());
        //            data.scr_no = comm.sGetInt32(reader["scr_no"].ToString());
        //            data.qsheet_code = comm.sGetString(reader["qsheet_code"].ToString());
        //            data.is_ok = comm.sGetString(reader["is_ok"].ToString());

        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            //資料邏輯刪除、修改
        //            //if (arr_LockGrpCode.Contains(data.qmt_code)) {
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
        public List<QMT03_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<QMT03_0000> list = new List<QMT03_0000>();

            string sSql = " SELECT distinct QMT03_0100.qmt_code, QMT03_0000.*, MEB20_0000.pro_name " +
                          " FROM QMT03_0000 " +
                          " left join QMT03_0100 on QMT03_0100.qmt_code = QMT03_0000.qmt_code " +
                          " left join MEB20_0000 on MEB20_0000.pro_code = QMT03_0000.pro_code " +
                          " left join QMB03_0100 on QMB03_0100.datacode = QMT03_0100.datacode " +
                          " left join QMB02_0000 on QMB02_0000.qtest_item_code = QMB03_0100.qtest_item_code " +
                          " left join BDP21_0100 on BDP21_0100.field_code = QMT03_0100.is_ok and BDP21_0100.code_code = 'is_ok' " ;

            // 取得資料
            list = comm.Get_ListByQuery<QMT03_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個QMT03_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="QMT03_0000">DTO</param>
        public void InsertData(QMT03_0000 QMT03_0000)
        {
            string sSql = " INSERT INTO " +
                          " QMT03_0000 (  qmt_code,  rel_type,  rel_code,  pro_code,  lot_no,  scr_no,  is_rec ) " +

                          "     VALUES ( @qmt_code, @rel_type, @rel_code, @pro_code, @lot_no, @scr_no, @is_rec ) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, QMT03_0000);
            }
        }

        /// <summary>
        /// 傳入一個QMT03_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="QMT03_0000">DTO</param>
        public void UpdateData(QMT03_0000 QMT03_0000)
        {
            string sSql = " UPDATE QMT03_0000                    " +
                          "    SET rel_type     =  @rel_type,    " +
                          "        rel_code     =  @rel_code,    " +
                          "        pro_code     =  @pro_code,    " +
                          "        lot_no       =  @lot_no,      " +
                          "        scr_no       =  @scr_no,      " +
                          "        is_rec       =  @is_rec       " +
                          "  WHERE qmt_code     =  @qmt_code     ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, QMT03_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM QMT03_0000 WHERE qmt_code = @qmt_code ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { qmt_code = pTkCode });
            }
        }

    }
}