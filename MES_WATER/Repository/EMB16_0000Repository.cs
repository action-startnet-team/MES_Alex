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
    public class EMB16_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得EMB16_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO EMB16_0000</returns>
        public EMB16_0000 GetDTO(string pTkCode)
        {
            EMB16_0000 datas = new EMB16_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMB16_0000";
            }
            else
            {
                sSql = "SELECT * FROM EMB16_0000 where repair_type_code=@repair_type_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@repair_type_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new EMB16_0000
                        {

                            repair_type_code = comm.sGetString(reader["repair_type_code"].ToString()),
                            repair_type_name = comm.sGetString(reader["repair_type_name"].ToString()),
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
        /// 取得EMB16_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List EMB16_0000</returns>
        public List<EMB16_0000> Get_DataList(string pTkCode)
        {
            List<EMB16_0000> list = new List<EMB16_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMB16_0000";
            }
            else
            {
                sSql = "SELECT * FROM EMB16_0000 where repair_type_code=@repair_type_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@repair_type_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EMB16_0000 data = new EMB16_0000();

                    data.repair_type_code = comm.sGetString(reader["repair_type_code"].ToString());
                    data.repair_type_name = comm.sGetString(reader["repair_type_name"].ToString());
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
        public List<EMB16_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_repair_type_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<EMB16_0000> list = new List<EMB16_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM EMB16_0000";
            sSql = "SELECT * FROM EMB16_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EMB16_0000 data = new EMB16_0000();

                    data.repair_type_code = comm.sGetString(reader["repair_type_code"].ToString());
                    data.repair_type_name = comm.sGetString(reader["repair_type_name"].ToString());
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
        public List<EMB16_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<EMB16_0000> list = new List<EMB16_0000>();

            string sSql = " SELECT * FROM EMB16_0000 ";

            // 取得資料
            list = comm.Get_ListByQuery<EMB16_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個EMB16_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="EMB16_0000">DTO</param>
        public void InsertData(EMB16_0000 EMB16_0000)
        {

            string sSql = "INSERT INTO " +
                          " EMB16_0000 (  repair_type_code,  repair_type_name,  cmemo,  is_use ) " +
                          "     VALUES ( @repair_type_code, @repair_type_name, @cmemo, @is_use ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMB16_0000);
            }
        }

        /// <summary>
        /// 傳入一個EMB16_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="EMB16_0000">DTO</param>
        public void UpdateData(EMB16_0000 EMB16_0000)
        {
            string sSql = " UPDATE EMB16_0000                            " +
                          "    SET repair_type_name = @repair_type_name, " +
                          "        cmemo            = @cmemo,            " +
                          "        is_use           = @is_use            " +
                          "  WHERE repair_type_code =  @repair_type_code  ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMB16_0000);

            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM EMB16_0000 WHERE repair_type_code = @repair_type_code;";
            //sSql += " Delete from BDP09_0100 where repair_type_code = @repair_type_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { repair_type_code = pTkCode });

            }
        }


    }
}