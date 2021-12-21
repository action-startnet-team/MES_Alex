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
    public class EMB19_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得EMB19_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO EMB19_0000</returns>
        public EMB19_0000 GetDTO(string pTkCode)
        {
            EMB19_0000 datas = new EMB19_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMB19_0000";
            }
            else
            {
                sSql = "SELECT * FROM EMB19_0000 where fault_reason_code=@fault_reason_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@fault_reason_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new EMB19_0000
                        {

                            fault_reason_code = comm.sGetString(reader["fault_reason_code"].ToString()),
                            fault_reason_name = comm.sGetString(reader["fault_reason_name"].ToString()),
                            cmemo = comm.sGetString(reader["cmemo"].ToString()),
                            is_use = comm.sGetString(reader["is_use"].ToString()),



                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得EMB19_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List EMB19_0000</returns>
        public List<EMB19_0000> Get_DataList(string pTkCode)
        {
            List<EMB19_0000> list = new List<EMB19_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMB19_0000";
            }
            else
            {
                sSql = "SELECT * FROM EMB19_0000 where fault_reason_code=@fault_reason_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@fault_reason_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EMB19_0000 data = new EMB19_0000();

                    data.fault_reason_code = comm.sGetString(reader["fault_reason_code"].ToString());
                    data.fault_reason_name = comm.sGetString(reader["fault_reason_name"].ToString());
                    data.cmemo = comm.sGetString(reader["cmemo"].ToString());
                    data.is_use = comm.sGetString(reader["is_use"].ToString());

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
        public List<EMB19_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_fault_reason_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<EMB19_0000> list = new List<EMB19_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM EMB19_0000";
            sSql = "SELECT * FROM EMB19_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EMB19_0000 data = new EMB19_0000();

                    data.fault_reason_code = comm.sGetString(reader["fault_reason_code"].ToString());
                    data.fault_reason_name = comm.sGetString(reader["fault_reason_name"].ToString());
                    data.cmemo = comm.sGetString(reader["cmemo"].ToString());
                    data.is_use = comm.sGetString(reader["is_use"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";


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
        public List<EMB19_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<EMB19_0000> list = new List<EMB19_0000>();

            string sSql = " SELECT * FROM EMB19_0000 ";

            // 取得資料
            list = comm.Get_ListByQuery<EMB19_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個EMB19_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="EMB19_0000">DTO</param>
        public void InsertData(EMB19_0000 EMB19_0000)
        {

            string sSql = "INSERT INTO " +
                          " EMB19_0000 (  fault_reason_code,  fault_reason_name,  cmemo,  is_use ) " +
                          "     VALUES ( @fault_reason_code, @fault_reason_name, @cmemo, @is_use ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMB19_0000);
            }
        }

        /// <summary>
        /// 傳入一個EMB19_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="EMB19_0000">DTO</param>
        public void UpdateData(EMB19_0000 EMB19_0000)
        {
            string sSql = " UPDATE EMB19_0000                              " +
                          "    SET fault_reason_name = @fault_reason_name, " +
                          "        cmemo             = @cmemo,             " +
                          "        is_use            = @is_use             " +
                          "  WHERE fault_reason_code = @fault_reason_code  ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMB19_0000);

            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM EMB19_0000 WHERE fault_reason_code = @fault_reason_code;";
            //sSql += " Delete from BDP09_0100 where fault_reason_code = @fault_reason_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { fault_reason_code = pTkCode });

            }
        }


    }
}