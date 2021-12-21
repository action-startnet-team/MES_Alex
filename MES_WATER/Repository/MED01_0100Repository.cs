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
    public class MED01_0100Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MED01_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MED01_0100</returns>
        public MED01_0100 GetDTO(string pTkCode)
        {
            MED01_0100 datas = new MED01_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MED01_0100";
            }
            else
            {
                sSql = "SELECT * FROM MED01_0100 where med01_0100=@med01_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@med01_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MED01_0100
                        {

                            med01_0100 = comm.sGetInt32(reader["med01_0100"].ToString()),
                            mo_code = comm.sGetString(reader["mo_code"].ToString()),
                            wrk_code = comm.sGetString(reader["wrk_code"].ToString()),
                            work_code = comm.sGetString(reader["work_code"].ToString()),
                            station_code = comm.sGetString(reader["station_code"].ToString()),
                            mac_code = comm.sGetString(reader["mac_code"].ToString()),
                            usr_code = comm.sGetString(reader["usr_code"].ToString()),
                            date_s = comm.sGetString(reader["date_s"].ToString()),
                            time_s = comm.sGetString(reader["time_s"].ToString()),
                            date_e = comm.sGetString(reader["date_e"].ToString()),
                            time_e = comm.sGetString(reader["time_e"].ToString()),
                            status = comm.sGetString(reader["status"].ToString()),
                            des_memo = comm.sGetString(reader["des_memo"].ToString()),
                            is_ng = comm.sGetString(reader["is_ng"].ToString()),
                            is_end = comm.sGetString(reader["is_end"].ToString()),
                            end_memo = comm.sGetString(reader["end_memo"].ToString()),
                            end_date = comm.sGetString(reader["end_date"].ToString()),
                            end_time = comm.sGetString(reader["end_time"].ToString()),
                            end_usr_code = comm.sGetString(reader["end_usr_code"].ToString()),
                            user_field_01 = comm.sGetString(reader["user_field_01"].ToString()),
                            user_field_02 = comm.sGetString(reader["user_field_02"].ToString()),
                            user_field_03 = comm.sGetString(reader["user_field_03"].ToString()),
                            user_field_04 = comm.sGetString(reader["user_field_04"].ToString()),
                            user_field_05 = comm.sGetString(reader["user_field_05"].ToString()),
                            user_field_06 = comm.sGetString(reader["user_field_06"].ToString()),
                            user_field_07 = comm.sGetString(reader["user_field_07"].ToString()),
                            user_field_08 = comm.sGetString(reader["user_field_08"].ToString()),
                            user_field_09 = comm.sGetString(reader["user_field_09"].ToString()),
                            user_field_10 = comm.sGetString(reader["user_field_10"].ToString())


                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MED01_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MED01_0100</returns>
        public List<MED01_0100> Get_DataList(string pTkCode)
        {
            List<MED01_0100> list = new List<MED01_0100>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MED01_0100";
            }
            else
            {
                sSql = "SELECT * FROM MED01_0100 where med01_0100=@med01_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@med01_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MED01_0100 data = new MED01_0100();

                           data.med01_0100 = comm.sGetInt32(reader["med01_0100"].ToString());
                           data.mo_code = comm.sGetString(reader["mo_code"].ToString());
                           data.wrk_code = comm.sGetString(reader["wrk_code"].ToString());
                           data.work_code = comm.sGetString(reader["work_code"].ToString());
                           data.station_code = comm.sGetString(reader["station_code"].ToString());
                           data.mac_code = comm.sGetString(reader["mac_code"].ToString());
                           data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                           data.date_s = comm.sGetString(reader["date_s"].ToString());
                           data.time_s = comm.sGetString(reader["time_s"].ToString());
                           data.date_e = comm.sGetString(reader["date_e"].ToString());
                           data.time_e = comm.sGetString(reader["time_e"].ToString());
                           data.status = comm.sGetString(reader["status"].ToString());
                           data.des_memo = comm.sGetString(reader["des_memo"].ToString());
                           data.is_ng = comm.sGetString(reader["is_ng"].ToString());
                           data.is_end = comm.sGetString(reader["is_end"].ToString());
                           data.end_memo = comm.sGetString(reader["end_memo"].ToString());
                           data.end_date = comm.sGetString(reader["end_date"].ToString());
                           data.end_time = comm.sGetString(reader["end_time"].ToString());
                           data.end_usr_code = comm.sGetString(reader["end_usr_code"].ToString());



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
        public List<MED01_0100> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_med01_0100", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MED01_0100> list = new List<MED01_0100>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM MED01_0100";
            sSql = "SELECT * FROM MED01_0100";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@med01_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MED01_0100 data = new MED01_0100();

                    data.med01_0100 = comm.sGetInt32(reader["med01_0100"].ToString());
                    data.mo_code = comm.sGetString(reader["mo_code"].ToString());
                    data.wrk_code = comm.sGetString(reader["wrk_code"].ToString());
                    data.work_code = comm.sGetString(reader["work_code"].ToString());
                    data.station_code = comm.sGetString(reader["station_code"].ToString());
                    data.mac_code = comm.sGetString(reader["mac_code"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                    data.date_s = comm.sGetString(reader["date_s"].ToString());
                    data.time_s = comm.sGetString(reader["time_s"].ToString());
                    data.date_e = comm.sGetString(reader["date_e"].ToString());
                    data.time_e = comm.sGetString(reader["time_e"].ToString());
                    data.status = comm.sGetString(reader["status"].ToString());
                    data.des_memo = comm.sGetString(reader["des_memo"].ToString());
                    data.is_ng = comm.sGetString(reader["is_ng"].ToString());
                    data.is_end = comm.sGetString(reader["is_end"].ToString());
                    data.end_memo = comm.sGetString(reader["end_memo"].ToString());
                    data.end_date = comm.sGetString(reader["end_date"].ToString());
                    data.end_time = comm.sGetString(reader["end_time"].ToString());
                    data.end_usr_code = comm.sGetString(reader["end_usr_code"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.med01_0100)) {
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
        public List<MED01_0100> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MED01_0100> list = new List<MED01_0100>();

            string sSql = " SELECT MED01_0100.*," +
                          " MEB29_0000.station_name as station_name," +
                          " MEB30_0000.work_name as work_name," +
                          " MEB15_0000.mac_name, " +
                          " BDP08_0000.usr_name as usr_name ," +
                          " BDP21_0100.field_name as status_name" +
                          " FROM MED01_0100" +
                          " left join MEB29_0000 on MEB29_0000.station_code = MED01_0100.station_code" +
                          " left join MEB30_0000 on MEB30_0000.work_code = MED01_0100.work_code" +
                          " left join MEB15_0000 on MEB15_0000.mac_code = MED01_0100.mac_code" +
                          " left join BDP08_0000 on BDP08_0000.usr_code = MED01_0100.usr_code" +
                          " left join BDP21_0100 on BDP21_0100.field_code = MED01_0100.status and BDP21_0100.code_code = 'login_status'";


            // 取得資料
            list = comm.Get_ListByQuery<MED01_0100>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個MED01_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MED01_0100">DTO</param>
        public void InsertData(MED01_0100 MED01_0100)
        {
            string sSql = "INSERT INTO " +
                          " MED01_0100 (  mo_code,  wrk_code, work_code, station_code, mac_code, usr_code, date_s, time_s, data_e, time_e, status )   " +
                          "     VALUES (@mo_code, @wrk_code, @work_code, @station_code, @mac_code, @usr_code, @date_s, @time_s, @data_e, @time_e, @status)   ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MED01_0100);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@med01_0100", MED01_0100.med01_0100));
                //sqlCommand.Parameters.Add(new SqlParameter("@med01_0100", MED01_0100.med01_0100));
                //sqlCommand.Parameters.Add(new SqlParameter("@sup_type_name", MED01_0100.sup_type_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個MED01_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MED01_0100">DTO</param>
        public void UpdateData(MED01_0100 MED01_0100)
        {
            string sSql = " UPDATE MED01_0100                    " +
                          "    SET mo_code      =  @mo_code,     " +
                          "        wrk_code     =  @wrk_code,     " +
                          "        work_code    =  @work_code,     " +
                          "        station_code =  @station_code,     " +
                          "        mac_code     =  @mac_code,    " +
                          "        usr_code     =  @usr_code,    " +
                          "        date_s     =  @date_s,    " +
                          "        time_s     =  @time_s,    " +
                          "        date_e     =  @date_e,    " +
                          "        time_e     =  @time_e,    " +
                          "        status     =  @status, " +
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
                          "  WHERE med01_0100   =  @med01_0100   " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MED01_0100);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@med01_0100", MED01_0100.med01_0100));
                //sqlCommand.Parameters.Add(new SqlParameter("@med01_0100", MED01_0100.med01_0100));
                //sqlCommand.Parameters.Add(new SqlParameter("@sup_type_name", MED01_0100.sup_type_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MED01_0100 WHERE med01_0100 = @med01_0100;";
            //sSql += " Delete from BDP09_0100 where med01_0100 = @med01_0100; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { med01_0100 = pTkCode });

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@med01_0100", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }






        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得MED01_0100角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetMED01_0100_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("med01_0100", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("med01_0100", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("sup_type_name", System.Type.GetType("System.String"].ToString());

            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MED01_0100";
            }
            else
            {
                sSql = "SELECT * FROM MED01_0100 where med01_0100='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["med01_0100"] = dtTmp.Rows[i]["med01_0100"];
                drow["med01_0100"] = dtTmp.Rows[i]["med01_0100"];
                drow["sup_type_name"] = dtTmp.Rows[i]["sup_type_name"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}