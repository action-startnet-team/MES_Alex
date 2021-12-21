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
    public class MEB43_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MEB43_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEB43_0000</returns>
        public MEB43_0000 GetDTO(string pTkCode)
        {
            MEB43_0000 datas = new MEB43_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB43_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEB43_0000 where ng_memo_code=@ng_memo_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@ng_memo_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEB43_0000
                        {

                            ng_memo_code = comm.sGetString(reader["ng_memo_code"].ToString()),
                            ng_memo_name = comm.sGetString(reader["ng_memo_name"].ToString()),
                            cmemo = comm.sGetString(reader["cmemo"].ToString()),
                        


                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MEB43_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MEB43_0000</returns>
        public List<MEB43_0000> Get_DataList(string pTkCode)
        {
            List<MEB43_0000> list = new List<MEB43_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB43_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEB43_0000 where ng_memo_code=@ng_memo_code";
            }
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@ng_memo_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB43_0000 data = new MEB43_0000();

                    data.ng_memo_code = comm.sGetString(reader["ng_memo_code"].ToString());
                    data.ng_memo_name = comm.sGetString(reader["ng_memo_name"].ToString());
                    data.cmemo = comm.sGetString(reader["cmemo"].ToString());
                 


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
        public List<MEB43_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_ng_memo_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MEB43_0000> list = new List<MEB43_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM MEB43_0000";
            sSql = "SELECT * FROM MEB43_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@ng_memo_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB43_0000 data = new MEB43_0000();

                    data.ng_memo_code = comm.sGetString(reader["ng_memo_code"].ToString());
                    data.ng_memo_name = comm.sGetString(reader["ng_memo_name"].ToString());
                    data.cmemo = comm.sGetString(reader["cmemo"].ToString());


                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.ng_memo_code)) {
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
        public List<MEB43_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MEB43_0000> list = new List<MEB43_0000>();

            string sSql = " SELECT distinct MEB43_0000.ng_memo_code, MEB43_0000.* " +
                          " FROM MEB43_0000 " +
                          @"left join (
                                select MEB43_0100.*, MEB37_0000.ng_name
                                from MEB43_0100
                                left join MEB37_0000 on MEB37_0000.ng_code = MEB43_0100.ng_code
                            ) as s on s.ng_memo_code = MEB43_0000.ng_memo_code ";

            // 取得資料
            list = comm.Get_ListByQuery<MEB43_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個MEB43_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEB43_0000">DTO</param>
        public void InsertData(MEB43_0000 MEB43_0000)
        {
            string sSql = "INSERT INTO " +
                          " MEB43_0000 (   ng_memo_code,  ng_memo_name,  cmemo )" +
                          "     VALUES (  @ng_memo_code, @ng_memo_name, @cmemo )" ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB43_0000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@ng_memo_code", MEB43_0000.ng_memo_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@ng_memo_code", MEB43_0000.ng_memo_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@ng_memo_name", MEB43_0000.ng_memo_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個MEB43_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEB43_0000">DTO</param>
        public void UpdateData(MEB43_0000 MEB43_0000)
        {
            string sSql = " UPDATE MEB43_0000                     " +
                          "    SET ng_memo_name     =  @ng_memo_name,     " +
                          "        cmemo      =  @cmemo      " +
                          "  WHERE ng_memo_code     =  @ng_memo_code      " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB43_0000);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@ng_memo_code", MEB43_0000.ng_memo_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@ng_memo_code", MEB43_0000.ng_memo_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@ng_memo_name", MEB43_0000.ng_memo_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MEB43_0000 WHERE ng_memo_code = @ng_memo_code;";
            //sSql += " Delete from BDP09_0100 where ng_memo_code = @ng_memo_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { ng_memo_code = pTkCode });

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@ng_memo_code", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }






        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得MEB43_0000角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetMEB43_0000_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("ng_memo_code", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("ng_memo_code", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("ng_memo_name", System.Type.GetType("System.String"].ToString());

            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB43_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEB43_0000 where ng_memo_code='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["ng_memo_code"] = dtTmp.Rows[i]["ng_memo_code"];
                drow["ng_memo_code"] = dtTmp.Rows[i]["ng_memo_code"];
                drow["ng_memo_name"] = dtTmp.Rows[i]["ng_memo_name"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}