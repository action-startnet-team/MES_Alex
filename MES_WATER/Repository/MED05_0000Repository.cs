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
    public class MED05_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MED05_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MED05_0000</returns>
        public MED05_0000 GetDTO(string pTkCode)
        {
            MED05_0000 datas = new MED05_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MED05_0000";
            }
            else
            {
                sSql = "SELECT * FROM MED05_0000 where med05_0000=@med05_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@med05_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MED05_0000
                        {

                            med05_0000 = comm.sGetInt32(reader["med05_0000"].ToString()),
                            mo_code = comm.sGetString(reader["mo_code"].ToString()),
                            wrk_code = comm.sGetString(reader["wrk_code"].ToString()),
                            mac_code = comm.sGetString(reader["mac_code"].ToString()),
                            except_code = comm.sGetString(reader["except_code"].ToString()),
                            date_s = comm.sGetString(reader["date_s"].ToString()),
                            time_s = comm.sGetString(reader["time_s"].ToString()),
                            date_e = comm.sGetString(reader["date_e"].ToString()),
                            time_e = comm.sGetString(reader["time_e"].ToString()),
                            ins_date = comm.sGetString(reader["ins_date"].ToString()),
                            ins_time = comm.sGetString(reader["ins_time"].ToString()),
                            usr_code = comm.sGetString(reader["usr_code"].ToString()),
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
        /// 取得MED05_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MED05_0000</returns>
        public List<MED05_0000> Get_DataList(string pTkCode)
        {
            List<MED05_0000> list = new List<MED05_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MED05_0000";
            }
            else
            {
                sSql = "SELECT * FROM MED05_0000 where med05_0000=@med05_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@med05_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MED05_0000 data = new MED05_0000();

                    data.med05_0000 = comm.sGetInt32(reader["med05_0000"].ToString());
                    data.mo_code = comm.sGetString(reader["mo_code"].ToString());
                    data.wrk_code = comm.sGetString(reader["wrk_code"].ToString());
                    data.mac_code = comm.sGetString(reader["mac_code"].ToString());
                    data.except_code = comm.sGetString(reader["except_code"].ToString());
                    data.date_s = comm.sGetString(reader["date_s"].ToString());
                    data.time_s = comm.sGetString(reader["time_s"].ToString());
                    data.date_e = comm.sGetString(reader["date_e"].ToString());
                    data.time_e = comm.sGetString(reader["time_e"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
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
        public List<MED05_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_med05_0000", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MED05_0000> list = new List<MED05_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM MED05_0000";
            sSql = "SELECT * FROM MED05_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@med05_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MED05_0000 data = new MED05_0000();


                    data.med05_0000 = comm.sGetInt32(reader["med05_0000"].ToString());
                    data.mo_code = comm.sGetString(reader["mo_code"].ToString());
                    data.wrk_code = comm.sGetString(reader["wrk_code"].ToString());
                    data.mac_code = comm.sGetString(reader["mac_code"].ToString());
                    data.except_code = comm.sGetString(reader["except_code"].ToString());
                    data.date_s = comm.sGetString(reader["date_s"].ToString());
                    data.time_s = comm.sGetString(reader["time_s"].ToString());
                    data.date_e = comm.sGetString(reader["date_e"].ToString());
                    data.time_e = comm.sGetString(reader["time_e"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
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
                    //if (arr_LockGrpCode.Contains(data.med05_0000)) {
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
        public List<MED05_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MED05_0000> list = new List<MED05_0000>();

            string sSql = " SELECT MED05_0000.*, MEB15_0000.mac_name, MEB46_0000.except_name " +
                          " FROM MED05_0000 " +
                          " left join MEB15_0000 on MEB15_0000.mac_code = MED05_0000.mac_code " +
                          " left join MEB46_0000 on MEB46_0000.except_code = MED05_0000.except_code ";

            // 取得資料
            list = comm.Get_ListByQuery<MED05_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個MED05_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MED05_0000">DTO</param>
        public void InsertData(MED05_0000 MED05_0000)
        {
            string sSql = "INSERT INTO " +
                          " MED05_0000 (  mo_code,  wrk_code,  mac_code,  except_code,  date_s,  time_s,     " +
                          "               date_e,  time_e,  ins_date,  ins_time,  usr_code,  des_memo,       " +
                          "               is_ng,  is_end,  end_memo,  end_date,  end_time,  end_usr_code )   " +

                          "     VALUES ( @mo_code, @wrk_code, @mac_code, @except_code, @date_s,  @time_s,    " +
                          "              @date_e, @time_e, @ins_date, @ins_time, @usr_code, @des_memo,       " +
                          "              @is_ng, @is_end, @end_memo, @end_date, @end_time, @end_usr_code )   " ;

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MED05_0000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@med05_0000", MED05_0000.med05_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@med05_0000", MED05_0000.med05_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@sup_type_name", MED05_0000.sup_type_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個MED05_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MED05_0000">DTO</param>
        public void UpdateData(MED05_0000 MED05_0000)
        {
            string sSql = " UPDATE MED05_0000                     " +
                          "    SET mo_code       =  @mo_code,     " +
                          "        wrk_code      =  @wrk_code,    " +
                          "        mac_code      =  @mac_code ,   " +
                          "        except_code   =  @except_code, " +
                          "        date_s        =  @date_s ,     " +
                          "        time_s        =  @time_s,      " +
                          "        date_e        =  @date_e ,     " +
                          "        time_e        =  @time_e  ,    " +
                          "        ins_date      =  @ins_date,    " +
                          "        ins_time      =  @ins_time,    " +
                          "        usr_code      =  @usr_code,    " +
                          "        des_memo      =  @des_memo,    " +
                          "        is_ng         =  @is_ng,       " +
                          "        is_end        =  @is_end,      " +
                          "        end_memo      =  @end_memo,    " +
                          "        end_date      =  @end_date,    " +
                          "        end_time      =  @end_time,    " +
                          "        end_usr_code  =  @end_usr_code, " +
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
                          "  WHERE med05_0000    =  @med05_0000   " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MED05_0000);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@med05_0000", MED05_0000.med05_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@med05_0000", MED05_0000.med05_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@sup_type_name", MED05_0000.sup_type_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MED05_0000 WHERE med05_0000 = @med05_0000;";
            //sSql += " Delete from BDP09_0100 where med05_0000 = @med05_0000; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { med05_0000 = pTkCode });

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@med05_0000", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }






        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得MED05_0000角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetMED05_0000_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("med05_0000", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("med05_0000", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("sup_type_name", System.Type.GetType("System.String"].ToString());

            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MED05_0000";
            }
            else
            {
                sSql = "SELECT * FROM MED05_0000 where med05_0000='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["med05_0000"] = dtTmp.Rows[i]["med05_0000"];
                drow["med05_0000"] = dtTmp.Rows[i]["med05_0000"];
                drow["sup_type_name"] = dtTmp.Rows[i]["sup_type_name"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}