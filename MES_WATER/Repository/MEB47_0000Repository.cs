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
    public class MEB47_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MEB47_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEB47_0000</returns>
        public MEB47_0000 GetDTO(string pTkCode)
        {
            MEB47_0000 datas = new MEB47_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB47_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEB47_0000 where dbull_code=@dbull_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@dbull_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEB47_0000
                        {

                            dbull_code = comm.sGetString(reader["dbull_code"].ToString()),
                            dbull_date_s = comm.sGetString(reader["dbull_date_s"].ToString()),
                            dbull_date_e = comm.sGetString(reader["dbull_date_e"].ToString()),
                            dbull_con = comm.sGetString(reader["dbull_con"].ToString()),
                            dashboard_code = comm.sGetString(reader["dashboard_code"].ToString()),
                            is_use = comm.sGetString(reader["is_use"].ToString()),
                            ins_date = comm.sGetString(reader["ins_date"].ToString()),
                            ins_time = comm.sGetString(reader["ins_time"].ToString()),
                            usr_code = comm.sGetString(reader["usr_code"].ToString()),

                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MEB47_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MEB47_0000</returns>
        public List<MEB47_0000> Get_DataList(string pTkCode)
        {
            List<MEB47_0000> list = new List<MEB47_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB47_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEB47_0000 where dbull_code=@dbull_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@dbull_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB47_0000 data = new MEB47_0000();

                    data.dbull_code = comm.sGetString(reader["dbull_code"].ToString());
                    data.dbull_date_s = comm.sGetString(reader["dbull_date_s"].ToString());
                    data.dbull_date_e = comm.sGetString(reader["dbull_date_e"].ToString());
                    data.dbull_con = comm.sGetString(reader["dbull_con"].ToString());
                    data.dashboard_code = comm.sGetString(reader["dashboard_code"].ToString());
                    data.is_use = comm.sGetString(reader["is_use"].ToString());
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
        public List<MEB47_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_dbull_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MEB47_0000> list = new List<MEB47_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM MEB47_0000";
            sSql = "SELECT * FROM MEB47_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB47_0000 data = new MEB47_0000();

                    data.dbull_code = comm.sGetString(reader["dbull_code"].ToString());
                    data.dbull_date_s = comm.sGetString(reader["dbull_date_s"].ToString());
                    data.dbull_date_e = comm.sGetString(reader["dbull_date_e"].ToString());
                    data.dbull_con = comm.sGetString(reader["dbull_con"].ToString());
                    data.dashboard_code = comm.sGetString(reader["dashboard_code"].ToString());
                    data.is_use = comm.sGetString(reader["is_use"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());

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
        public List<MEB47_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MEB47_0000> list = new List<MEB47_0000>();

            string sSql = " SELECT *, BDP21_0100.field_name as dashboard_name " +
                          " FROM MEB47_0000 " +
                          " left join BDP21_0100 on BDP21_0100.field_code = MEB47_0000.dashboard_code and BDP21_0100.code_code = 'dashboard_code' ";

            // 取得資料
            list = comm.Get_ListByQuery<MEB47_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個MEB47_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEB47_0000">DTO</param>
        public void InsertData(MEB47_0000 MEB47_0000)
        {

            string sSql = "INSERT INTO " +
                          " MEB47_0000 (  dbull_code,  dbull_date_s,  dbull_date_e,  dbull_con,  dashboard_code, " +
                          "               is_use,  ins_date,  ins_time,  usr_code                              ) " +

                          "     VALUES ( @dbull_code, @dbull_date_s, @dbull_date_e, @dbull_con, @dashboard_code, " +
                          "              @is_use, @ins_date, @ins_time, @usr_code                              ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB47_0000);
            }
        }

        /// <summary>
        /// 傳入一個MEB47_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEB47_0000">DTO</param>
        public void UpdateData(MEB47_0000 MEB47_0000)
        {
            string sSql = " UPDATE MEB47_0000                         " +
                          "    SET dbull_date_s   =  @dbull_date_s,   " +
                          "        dbull_date_e   =  @dbull_date_e,   " +
                          "        dbull_con      =  @dbull_con,      " +
                          "        dashboard_code =  @dashboard_code, " +
                          "        is_use         =  @is_use,         " +
                          "        ins_date       =  @ins_date,       " +
                          "        ins_time       =  @ins_time,       " +
                          "        usr_code       =  @usr_code        " +
                          "  WHERE dbull_code     =  @dbull_code      " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB47_0000);

            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MEB47_0000 WHERE dbull_code = @dbull_code;";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { dbull_code = pTkCode });

            }
        }


    }
}