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
    public class MEB37_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MEB37_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEB37_0000</returns>
        public MEB37_0000 GetDTO(string pTkCode)
        {
            MEB37_0000 datas = new MEB37_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB37_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEB37_0000 where ng_code=@ng_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@ng_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEB37_0000
                        {

                            ng_code = comm.sGetString(reader["ng_code"].ToString()),
                            ng_name = comm.sGetString(reader["ng_name"].ToString()),
                            ng_type_code = comm.sGetString(reader["ng_type_code"].ToString()),
                            ng_kind_code = comm.sGetString(reader["ng_kind_code"].ToString()),
                            cmemo = comm.sGetString(reader["cmemo"].ToString()),

                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MEB37_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MEB37_0000</returns>
        public List<MEB37_0000> Get_DataList(string pTkCode)
        {
            List<MEB37_0000> list = new List<MEB37_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB37_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEB37_0000 where ng_code=@ng_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@ng_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB37_0000 data = new MEB37_0000();

                    data.ng_code = comm.sGetString(reader["ng_code"].ToString());
                    data.ng_name = comm.sGetString(reader["ng_name"].ToString());
                    data.ng_type_code = comm.sGetString(reader["ng_type_code"].ToString());
                    data.ng_kind_code = comm.sGetString(reader["ng_kind_code"].ToString());
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
        public List<MEB37_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_ng_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MEB37_0000> list = new List<MEB37_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = "SELECT * FROM MEB37_0000";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@ng_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB37_0000 data = new MEB37_0000();

                    data.ng_code = comm.sGetString(reader["ng_code"].ToString());
                    data.ng_name = comm.sGetString(reader["ng_name"].ToString());
                    data.ng_type_code = comm.sGetString(reader["ng_type_code"].ToString());
                    data.ng_kind_code = comm.sGetString(reader["ng_kind_code"].ToString());
                    data.cmemo = comm.sGetString(reader["cmemo"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.ng_code)) {
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
        public List<MEB37_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MEB37_0000> list = new List<MEB37_0000>();

            string sSql = "    SELECT MEB37_0000.*, MEB36_0000.ng_type_name as ng_type_name, MEB38_0000.ng_kind_name as ng_kind_name " +
                          "      FROM MEB37_0000 " +
                          " left join MEB36_0000 on MEB36_0000.ng_type_code = MEB37_0000.ng_type_code " +
                          " left join MEB38_0000 on MEB38_0000.ng_kind_code = MEB37_0000.ng_kind_code " ;


            // 取得資料
            list = comm.Get_ListByQuery<MEB37_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個MEB37_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEB37_0000">DTO</param>
        public void InsertData(MEB37_0000 MEB37_0000)
        {
            string sSql = "INSERT INTO " +
                          " MEB37_0000 (  ng_code,  ng_name,  ng_type_code,  ng_kind_code,  cmemo ) " +
                          "     VALUES ( @ng_code, @ng_name, @ng_type_code, @ng_kind_code, @cmemo ) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB37_0000);
            }
        }

        /// <summary>
        /// 傳入一個MEB37_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEB37_0000">DTO</param>
        public void UpdateData(MEB37_0000 MEB37_0000)
        {
            string sSql = " UPDATE MEB37_0000                      " +
                          "    SET ng_name      =  @ng_name,       " +
                          "        ng_type_code =  @ng_type_code,  " +
                          "        ng_kind_code =  @ng_kind_code,  " +
                          "        cmemo        =  @cmemo          " +
                          "  WHERE ng_code      =  @ng_code        " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB37_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MEB37_0000 WHERE ng_code = @ng_code;";
            //sSql += " Delete from BDP09_0100 where ng_code = @ng_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { ng_code = pTkCode });
            }
        }
    }
}