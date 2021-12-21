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
    public class EMB03_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得EMB03_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO EMB03_0000</returns>
        public EMB03_0000 GetDTO(string pTkCode)
        {
            EMB03_0000 datas = new EMB03_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMB03_0000";
            }
            else
            {
                sSql = "SELECT * FROM EMB03_0000 where sup_code=@sup_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@sup_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new EMB03_0000
                        {

                            sup_code = comm.sGetString(reader["sup_code"].ToString()),
                            sup_name = comm.sGetString(reader["sup_name"].ToString()),
                            is_use = comm.sGetString(reader["is_use"].ToString()),
                            cmemo = comm.sGetString(reader["cmemo"].ToString()),
                            main_per_name = comm.sGetString(reader["main_per_name"].ToString()),
                            main_tel = comm.sGetString(reader["main_tel"].ToString()),
                            sup_add = comm.sGetString(reader["sup_add"].ToString()),

                    };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得EMB03_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List EMB03_0000</returns>
        public List<EMB03_0000> Get_DataList(string pTkCode)
        {
            List<EMB03_0000> list = new List<EMB03_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMB03_0000";
            }
            else
            {
                sSql = "SELECT * FROM EMB03_0000 where sup_code=@sup_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@sup_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EMB03_0000 data = new EMB03_0000();

                    data.sup_code = comm.sGetString(reader["sup_code"].ToString());
                    data.sup_name = comm.sGetString(reader["sup_name"].ToString());
                    data.is_use = comm.sGetString(reader["is_use"].ToString());
                    data.cmemo = comm.sGetString(reader["cmemo"].ToString());
                    data.main_per_name = comm.sGetString(reader["main_per_name"].ToString());
                    data.main_tel = comm.sGetString(reader["main_tel"].ToString());
                    data.sup_add = comm.sGetString(reader["sup_add"].ToString());

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
        public List<EMB03_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_sup_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<EMB03_0000> list = new List<EMB03_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM EMB03_0000";
            sSql = "SELECT * FROM EMB03_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EMB03_0000 data = new EMB03_0000();

                    data.sup_code = comm.sGetString(reader["sup_code"].ToString());
                    data.sup_name = comm.sGetString(reader["sup_name"].ToString());
                    data.is_use = comm.sGetString(reader["is_use"].ToString());
                    data.cmemo = comm.sGetString(reader["cmemo"].ToString());
                    data.main_per_name = comm.sGetString(reader["main_per_name"].ToString());
                    data.main_tel = comm.sGetString(reader["main_tel"].ToString());
                    data.sup_add = comm.sGetString(reader["sup_add"].ToString());

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
        public List<EMB03_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<EMB03_0000> list = new List<EMB03_0000>();

            string sSql = " SELECT * FROM EMB03_0000 ";

            // 取得資料
            list = comm.Get_ListByQuery<EMB03_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個EMB03_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="EMB03_0000">DTO</param>
        public void InsertData(EMB03_0000 EMB03_0000)
        {
            string sSql = "INSERT INTO " +
                          " EMB03_0000 (  sup_code,  sup_name,  is_use,  cmemo,  main_per_name,  main_tel,  sup_add )   " +
                          "     VALUES ( @sup_code, @sup_name, @is_use, @cmemo, @main_per_name, @main_tel, @sup_add )   ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMB03_0000);
            }
        }

        /// <summary>
        /// 傳入一個EMB03_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="EMB03_0000">DTO</param>
        public void UpdateData(EMB03_0000 EMB03_0000)
        {
            string sSql = " UPDATE EMB03_0000                        " +
                          "    SET sup_name       =  @sup_name,      " +
                          "        is_use         =  @is_use,        " +
                          "        cmemo          =  @cmemo,         " +
                          "        main_per_name  =  @main_per_name, " +
                          "        main_tel       =  @main_tel,      " +
                          "        sup_add        =  @sup_add        " +
                          "  WHERE sup_code       =  @sup_code       ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMB03_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM EMB03_0000 WHERE sup_code = @sup_code;";
            //sSql += " Delete from BDP09_0100 where sup_code = @sup_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { sup_code = pTkCode });
            }
        }

    }
}