﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

using MES_WATER.Models;
using Dapper;


namespace MES_WATER.Repository
{
    public class MED01_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MED01_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MED01_0000</returns>
        public MED01_0000 GetDTO(string pTkCode)
        {
            MED01_0000 datas = new MED01_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MED01_0000";
            }
            else
            {
                sSql = "SELECT * FROM MED01_0000 where med01_0000=@med01_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@med01_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MED01_0000
                        {

                            med01_0000 = comm.sGetInt32(reader["med01_0000"].ToString()),
                            mo_code = comm.sGetString(reader["mo_code"].ToString()),
                            wrk_code = comm.sGetString(reader["wrk_code"].ToString()),
                            mac_code = comm.sGetString(reader["mac_code"].ToString()),
                            ins_date = comm.sGetString(reader["ins_date"].ToString()),
                            ins_time = comm.sGetString(reader["ins_time"].ToString()),
                            usr_code = comm.sGetString(reader["usr_code"].ToString()),
                            login_status = comm.sGetString(reader["login_status"].ToString()),
                            des_memo = comm.sGetString(reader["des_memo"].ToString()),
                            is_ng = comm.sGetString(reader["is_ng"].ToString()),
                            is_end = comm.sGetString(reader["is_end"].ToString()),
                            end_memo = comm.sGetString(reader["end_memo"].ToString()),
                            end_date = comm.sGetString(reader["end_date"].ToString()),
                            end_time = comm.sGetString(reader["end_time"].ToString()),
                            end_usr_code = comm.sGetString(reader["end_usr_code"].ToString()),
          


                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MED01_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MED01_0000</returns>
        public List<MED01_0000> Get_DataList(string pTkCode)
        {
            List<MED01_0000> list = new List<MED01_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MED01_0000";
            }
            else
            {
                sSql = "SELECT * FROM MED01_0000 where med01_0000=@med01_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@med01_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MED01_0000 data = new MED01_0000();

                    data.med01_0000 = comm.sGetInt32(reader["med01_0000"].ToString());
                    data.mo_code = comm.sGetString(reader["mo_code"].ToString());
                    data.wrk_code = comm.sGetString(reader["wrk_code"].ToString());
                    data.mac_code = comm.sGetString(reader["mac_code"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                    data.login_status = comm.sGetString(reader["login_status"].ToString());
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
        public List<MED01_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_med01_0000", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MED01_0000> list = new List<MED01_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM MED01_0000";
            sSql = "SELECT * FROM MED01_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@med01_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MED01_0000 data = new MED01_0000();

                    data.med01_0000 = comm.sGetInt32(reader["med01_0000"].ToString());
                    data.mo_code = comm.sGetString(reader["mo_code"].ToString());
                    data.wrk_code = comm.sGetString(reader["wrk_code"].ToString());
                    data.mac_code = comm.sGetString(reader["mac_code"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                    data.login_status = comm.sGetString(reader["login_status"].ToString());
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
                    //if (arr_LockGrpCode.Contains(data.med01_0000)) {
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
        public List<MED01_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MED01_0000> list = new List<MED01_0000>();

            string sSql = " SELECT MED01_0000.*, MEB15_0000.mac_name, BDP21_0100.field_name as login_status_name " +
                          " FROM MED01_0000 " +
                          " left join MEB15_0000 on MEB15_0000.mac_code = MED01_0000.mac_code " +
                          " left join BDP21_0100 on BDP21_0100.field_code = MED01_0000.login_status and BDP21_0100.code_code = 'login_status' ";


            // 取得資料
            list = comm.Get_ListByQuery<MED01_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個MED01_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MED01_0000">DTO</param>
        public void InsertData(MED01_0000 MED01_0000)
        {
            string sSql = "INSERT INTO " +
                          " MED01_0000 (  mo_code,  wrk_code,  mac_code,   time_e,  ins_date,  ins_time,  usr_code,  login_status )   " +
                          "     VALUES ( @mo_code, @wrk_code, @mac_code,  @ins_date, @ins_time, @usr_code, @login_status )   " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MED01_0000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@med01_0000", MED01_0000.med01_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@med01_0000", MED01_0000.med01_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@sup_type_name", MED01_0000.sup_type_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個MED01_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MED01_0000">DTO</param>
        public void UpdateData(MED01_0000 MED01_0000)
        {
            string sSql = " UPDATE MED01_0000                    " +
                          "    SET mo_code      =  @mo_code,     " +
                          "        wrk_code      =  @wrk_code,     " +
                          "        mac_code     =  @mac_code,    " +
                          "        ins_date     =  @ins_date,    " +
                          "        ins_time     =  @ins_time,    " +
                          "        usr_code     =  @usr_code,    " +
                          "        login_status =  @login_status, " +
                          "        des_memo     =  @des_memo,     " +
                          "        is_ng        =  @is_ng,        " +
                          "        is_end       =  @is_end,       " +
                          "        end_memo     =  @end_memo,     " +
                          "        end_date     =  @end_date,     " +
                          "        end_time     =  @end_time,     " +
                          "        end_usr_code     =  @end_usr_code " +
                          "  WHERE med01_0000   =  @med01_0000   " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MED01_0000);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@med01_0000", MED01_0000.med01_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@med01_0000", MED01_0000.med01_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@sup_type_name", MED01_0000.sup_type_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MED01_0000 WHERE med01_0000 = @med01_0000;";
            //sSql += " Delete from BDP09_0100 where med01_0000 = @med01_0000; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { med01_0000 = pTkCode });

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@med01_0000", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }






        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得MED01_0000角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetMED01_0000_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("med01_0000", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("med01_0000", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("sup_type_name", System.Type.GetType("System.String"].ToString());

            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MED01_0000";
            }
            else
            {
                sSql = "SELECT * FROM MED01_0000 where med01_0000='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["med01_0000"] = dtTmp.Rows[i]["med01_0000"];
                drow["med01_0000"] = dtTmp.Rows[i]["med01_0000"];
                drow["sup_type_name"] = dtTmp.Rows[i]["sup_type_name"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}