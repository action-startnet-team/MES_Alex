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
    public class EMB17_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得EMB17_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO EMB17_0000</returns>
        public EMB17_0000 GetDTO(string pTkCode)
        {
            EMB17_0000 datas = new EMB17_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMB17_0000";
            }
            else
            {
                sSql = "SELECT * FROM EMB17_0000 where malf_code=@malf_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@malf_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new EMB17_0000
                        {

                            malf_code = comm.sGetString(reader["malf_code"].ToString()),
                            malf_name = comm.sGetString(reader["malf_name"].ToString()),
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
        /// 取得EMB17_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List EMB17_0000</returns>
        public List<EMB17_0000> Get_DataList(string pTkCode)
        {
            List<EMB17_0000> list = new List<EMB17_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMB17_0000";
            }
            else
            {
                sSql = "SELECT * FROM EMB17_0000 where malf_code=@malf_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@malf_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EMB17_0000 data = new EMB17_0000();

                    data.malf_code = comm.sGetString(reader["malf_code"].ToString());
                    data.malf_name = comm.sGetString(reader["malf_name"].ToString());
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
        public List<EMB17_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_malf_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<EMB17_0000> list = new List<EMB17_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM EMB17_0000";
            sSql = "SELECT * FROM EMB17_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EMB17_0000 data = new EMB17_0000();

                    data.malf_code = comm.sGetString(reader["malf_code"].ToString());
                    data.malf_name = comm.sGetString(reader["malf_name"].ToString());
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
        public List<EMB17_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<EMB17_0000> list = new List<EMB17_0000>();

            string sSql = " SELECT * FROM EMB17_0000 ";

            // 取得資料
            list = comm.Get_ListByQuery<EMB17_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個EMB17_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="EMB17_0000">DTO</param>
        public void InsertData(EMB17_0000 EMB17_0000)
        {

            string sSql = "INSERT INTO " +
                          " EMB17_0000 (  malf_code,  malf_name,  cmemo,  is_use ) " +
                          "     VALUES ( @malf_code, @malf_name, @cmemo, @is_use ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMB17_0000);
            }
        }

        /// <summary>
        /// 傳入一個EMB17_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="EMB17_0000">DTO</param>
        public void UpdateData(EMB17_0000 EMB17_0000)
        {
            string sSql = " UPDATE EMB17_0000              " +
                          "    SET malf_name = @malf_name, " +
                          "        cmemo     = @cmemo,     " +
                          "        is_use    = @is_use     " +
                          "  WHERE malf_code = @malf_code  ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMB17_0000);

            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM EMB17_0000 WHERE malf_code = @malf_code;";
            //sSql += " Delete from BDP09_0100 where malf_code = @malf_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { malf_code = pTkCode });

            }
        }


    }
}