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
    public class MED04_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MED04_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MED04_0000</returns>
        public MED04_0000 GetDTO(string pTkCode)
        {
            MED04_0000 datas = new MED04_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = @"SELECT a.COLUMN_NAME as 'key',M.* FROM MEB15_0000 m 
                    LEFT JOIN INFORMATION_SCHEMA.COLUMNS a  on a.TABLE_NAME = 'MEA_E02' and a.COLUMN_NAME = m.address_code";
            }
            else
            {
                sSql = "SELECT * FROM MED04_0000 where med04_0000=@med04_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@med04_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MED04_0000
                        {

                            //med04_0000 = comm.sGetInt32(reader["med04_0000"].ToString()),
                            mac_name = comm.sGetString(reader["mac_name"].ToString()),
                            time_s = comm.sGetString(reader["update_at"].ToString()),
                            time_e = comm.sGetString(reader["update_at"].ToString()),
                            ins_date = ""

                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MED04_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MED04_0000</returns>
        public List<MED04_0000> Get_DataList(string pTkCode)
        {
            List<MED04_0000> list = new List<MED04_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = @"SELECT a.COLUMN_NAME as 'key',M.* FROM MEB15_0000 m 
                    LEFT JOIN INFORMATION_SCHEMA.COLUMNS a  on a.TABLE_NAME = 'MEA_E02' and a.COLUMN_NAME = m.address_code";
            }
            else
            {
                sSql = "SELECT * FROM MED04_0000 where med04_0000=@med04_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@med04_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MED04_0000 data = new MED04_0000();

                    //data.med04_0000 = comm.sGetInt32(reader["med04_0000"].ToString());
                    data.mac_name = comm.sGetString(reader["mac_name"].ToString());
                    data.time_s = comm.sGetString(reader["update_at"].ToString());
                    data.time_e = comm.sGetString(reader["update_at"].ToString());
                    data.ins_date = "";
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
        public List<MED04_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_med04_0000", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MED04_0000> list = new List<MED04_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM MED04_0000";
            sSql = @"SELECT a.COLUMN_NAME as 'key',M.* FROM MEB15_0000 m 
                    LEFT JOIN INFORMATION_SCHEMA.COLUMNS a  on a.TABLE_NAME = 'MEA_E02' and a.COLUMN_NAME = m.address_code";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@med04_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MED04_0000 data = new MED04_0000();
                    //data.med04_0000 = comm.sGetInt32(reader["med04_0000"].ToString());
                    data.mac_name = comm.sGetString(reader["mac_name"].ToString());
                    data.time_s = comm.sGetString(reader["update_at"].ToString());
                    data.time_e = comm.sGetString(reader["update_at"].ToString());
                    data.ins_date = "";

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.med04_0000)) {
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
        public List<MED04_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MED04_0000> list = new List<MED04_0000>();

            string sSql = @"SELECT a.COLUMN_NAME as 'key',M.* FROM MEB15_0000 m 
                    LEFT JOIN INFORMATION_SCHEMA.COLUMNS a  on a.TABLE_NAME = 'MEA_E02' and a.COLUMN_NAME = m.address_code";

            // 取得資料
            list = comm.Get_ListByQuery<MED04_0000>(sSql, pWhere, pUsrCode, pPrgCode);

            // 權限設定
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            //string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_mtp_code", "par_name", "par_value");
            //var arr_LockGrpCode = sLockGrpCode.Split(',');

            for (int i = 0; i < list.Count; i++)
            {
                //檢查授權刪除、修改


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
        /// 傳入一個MED04_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MED04_0000">DTO</param>
        public void InsertData(MED04_0000 MED04_0000)
        {
            string sSql = "INSERT INTO " +
                          " MED04_0000 (  mo_code,  wrk_code, work_code, station_code,  mac_code,  stop_code,  date_s,  time_s,     " +
                          "               date_e,  time_e,  ins_date,  ins_time,  usr_code  )   " +
                          "     VALUES ( @mo_code, @wrk_code, @work_code, @station_code, @mac_code, @stop_code, @date_s,  @time_s,    " +
                          "              @date_e,  @time_e,   @ins_date , @ins_time, @usr_code )   ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MED04_0000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@med04_0000", MED04_0000.med04_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@med04_0000", MED04_0000.med04_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@sup_type_name", MED04_0000.sup_type_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個MED04_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MED04_0000">DTO</param>
        public void UpdateData(MED04_0000 MED04_0000)
        {
            string sSql = " UPDATE MED04_0000                  " +
                          "    SET mo_code    =  @mo_code,     " +
                          "        wrk_code   =  @wrk_code ,   " +
                          "        work_code   =  @work_code , " +
                          "        station_code   =  @station_code ,   " +
                          "        mac_code   =  @mac_code ,   " +
                          "        stop_code  =  @stop_code ,  " +
                          "        date_s     =  @date_s ,     " +
                          "        time_s     =  @time_s,      " +
                          "        date_e     =  @date_e ,     " +
                          "        time_e     =  @time_e  ,    " +
                          "        ins_date   =  @ins_date,    " +
                          "        ins_time   =  @ins_time,    " +
                          "        usr_code   =  @usr_code,     " +
                          "        des_memo     =  @des_memo,     " +
                          "        is_ng        =  @is_ng,        " +
                          "        is_end       =  @is_end,       " +
                          "        end_memo     =  @end_memo,     " +
                          "        end_date     =  @end_date,     " +
                          "        end_time     =  @end_time,     " +
                          "        end_usr_code     =  @end_usr_code, " +
                          "        user_field_01     =  @user_field_01,      " +
                          "        user_field_02     =  @user_field_02,      " +
                          "        user_field_03     =  @user_field_03,      " +
                          "        user_field_04     =  @user_field_04,      " +
                          "        user_field_05     =  @user_field_05,      " +
                          "        user_field_06     =  @user_field_06,      " +
                          "        user_field_07     =  @user_field_07,      " +
                          "        user_field_08     =  @user_field_08,      " +
                          "        user_field_09     =  @user_field_09,      " +
                          "        user_field_10     =  @user_field_10       " +
                          "  WHERE med04_0000 =  @med04_0000   ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MED04_0000);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@med04_0000", MED04_0000.med04_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@med04_0000", MED04_0000.med04_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@sup_type_name", MED04_0000.sup_type_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MED04_0000 WHERE med04_0000 = @med04_0000;";
            //sSql += " Delete from BDP09_0100 where med04_0000 = @med04_0000; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { med04_0000 = pTkCode });

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@med04_0000", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }






        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得MED04_0000角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetMED04_0000_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("med04_0000", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("med04_0000", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("sup_type_name", System.Type.GetType("System.String"].ToString());

            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MED04_0000";
            }
            else
            {
                sSql = "SELECT * FROM MED04_0000 where med04_0000='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["med04_0000"] = dtTmp.Rows[i]["med04_0000"];
                drow["med04_0000"] = dtTmp.Rows[i]["med04_0000"];
                drow["sup_type_name"] = dtTmp.Rows[i]["sup_type_name"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}