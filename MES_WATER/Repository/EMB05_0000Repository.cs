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
    public class EMB05_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得EMB05_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO EMB05_0000</returns>
        public EMB05_0000 GetDTO(string pTkCode)
        {
            EMB05_0000 datas = new EMB05_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMB05_0000";
            }
            else
            {
                sSql = "SELECT * FROM EMB05_0000 where mai_type_code=@mai_type_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@mai_type_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new EMB05_0000
                        {

                            mai_type_code = comm.sGetString(reader["mai_type_code"].ToString()),
                            mai_type_name = comm.sGetString(reader["mai_type_name"].ToString()),
                            mia_type_memo = comm.sGetString(reader["mia_type_memo"].ToString()),



                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得EMB05_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List EMB05_0000</returns>
        public List<EMB05_0000> Get_DataList(string pTkCode)
        {
            List<EMB05_0000> list = new List<EMB05_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMB05_0000";
            }
            else
            {
                sSql = "SELECT * FROM EMB05_0000 where mai_type_code=@mai_type_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@mai_type_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EMB05_0000 data = new EMB05_0000();

                    data.mai_type_code = comm.sGetString(reader["mai_type_code"].ToString());
                    data.mai_type_name = comm.sGetString(reader["mai_type_name"].ToString());
                    data.mia_type_memo = comm.sGetString(reader["mia_type_memo"].ToString());

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
        public List<EMB05_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_mai_type_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<EMB05_0000> list = new List<EMB05_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM EMB05_0000";
            sSql = "SELECT * FROM EMB05_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@mai_type_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EMB05_0000 data = new EMB05_0000();

                    data.mai_type_code = comm.sGetString(reader["mai_type_code"].ToString());
                    data.mai_type_name = comm.sGetString(reader["mai_type_name"].ToString());
                    data.mia_type_memo = comm.sGetString(reader["mia_type_memo"].ToString());


                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.mai_type_code)) {
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
        public List<EMB05_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<EMB05_0000> list = new List<EMB05_0000>();

            string sSql = "SELECT * FROM EMB05_0000";

            // 取得資料
            list = comm.Get_ListByQuery<EMB05_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個EMB05_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="EMB05_0000">DTO</param>
        public void InsertData(EMB05_0000 EMB05_0000)
        {
            string sSql = "INSERT INTO " +
                          " EMB05_0000 (  mai_type_code,  mai_type_name,  mia_type_memo )   " +
                          "     VALUES ( @mai_type_code, @mai_type_name, @mia_type_memo )    ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMB05_0000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@mai_type_code", EMB05_0000.mai_type_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@mai_type_code", EMB05_0000.mai_type_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@mai_type_name", EMB05_0000.mai_type_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個EMB05_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="EMB05_0000">DTO</param>
        public void UpdateData(EMB05_0000 EMB05_0000)
        {
            string sSql = " UPDATE EMB05_0000 " +
                          "    SET mai_type_name  = @mai_type_name, " +
                          "        mia_type_memo  = @mia_type_memo  " +
                          "  WHERE mai_type_code  = @mai_type_code  " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMB05_0000);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@mai_type_code", EMB05_0000.mai_type_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@mai_type_code", EMB05_0000.mai_type_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@mai_type_name", EMB05_0000.mai_type_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM EMB05_0000 WHERE mai_type_code = @mai_type_code;";
            //sSql += " Delete from BDP09_0100 where mai_type_code = @mai_type_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { mai_type_code = pTkCode });

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@mai_type_code", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }






        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得EMB05_0000角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetEMB05_0000_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("mai_type_code", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("mai_type_code", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("mai_type_name", System.Type.GetType("System.String"].ToString());

            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMB05_0000";
            }
            else
            {
                sSql = "SELECT * FROM EMB05_0000 where mai_type_code='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["mai_type_code"] = dtTmp.Rows[i]["mai_type_code"];
                drow["mai_type_code"] = dtTmp.Rows[i]["mai_type_code"];
                drow["mai_type_name"] = dtTmp.Rows[i]["mai_type_name"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}