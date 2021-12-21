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
    public class EMT02_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得EMT02_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO EMT02_0000</returns>
        public EMT02_0000 GetDTO(string pTkCode)
        {
            EMT02_0000 datas = new EMT02_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMT02_0000";
            }
            else
            {
                sSql = "SELECT * FROM EMT02_0000 where dev_check_code=@dev_check_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@dev_check_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new EMT02_0000
                        {
                            dev_check_code = comm.sGetString(reader["dev_check_code"].ToString()),
                            dev_check_date = comm.sGetString(reader["dev_check_date"].ToString()),
                            dev_code = comm.sGetString(reader["dev_code"].ToString()),
                            cmemo = comm.sGetString(reader["cmemo"].ToString()),
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
        /// 取得EMT02_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List EMT02_0000</returns>
        //public List<EMT02_0000> Get_DataList(string pTkCode)
        //{
        //    List<EMT02_0000> list = new List<EMT02_0000>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM EMT02_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM EMT02_0000 where dev_check_code=@dev_check_code";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@dev_check_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            EMT02_0000 data = new EMT02_0000();

        //            data.dev_check_code = comm.sGetString(reader["dev_check_code"].ToString());
        //            data.dev_check_date = comm.sGetString(reader["dev_check_date"].ToString());
        //            data.cmemo = comm.sGetString(reader["cmemo"].ToString());
        //            data.dev_code = comm.sGetString(reader["dev_code"].ToString());
        //            data.ins_date = comm.sGetString(reader["ins_date"].ToString());
        //            data.ins_time = comm.sGetString(reader["ins_time"].ToString());
        //            data.usr_code = comm.sGetString(reader["usr_code"].ToString());
        //            data.cmemo = comm.sGetString(reader["cmemo"].ToString());


        //            data.can_delete = "Y";
        //            data.can_update = "Y";
        //            list.Add(data);
        //        }

        //    }
        //    return list;
        //}

        /// <summary>
        /// 取得使用者可以編輯的資料，結合商務邏輯權限
        /// </summary>
        /// <param name="pUsrCode"></param>
        /// <param name="pPrgCode"></param>
        /// <returns></returns>
        //public List<EMT02_0000> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_dev_check_code", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<EMT02_0000> list = new List<EMT02_0000>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    sSql = "SELECT * FROM EMT02_0000";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            EMT02_0000 data = new EMT02_0000();

        //            data.dev_check_code = comm.sGetString(reader["dev_check_code"].ToString());
        //            data.dev_check_date = comm.sGetString(reader["dev_check_date"].ToString());
        //            data.cmemo = comm.sGetString(reader["cmemo"].ToString());
        //            data.dev_code = comm.sGetString(reader["dev_code"].ToString());
        //            data.ins_date = comm.sGetString(reader["ins_date"].ToString());
        //            data.ins_time = comm.sGetString(reader["ins_time"].ToString());
        //            data.usr_code = comm.sGetString(reader["usr_code"].ToString());
        //            data.cmemo = comm.sGetString(reader["cmemo"].ToString());

        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            list.Add(data);
        //        }
        //    }
        //    return list;
        //}
        #endregion

        /// <summary>
        /// 取得查詢資料，結合使用者權限
        /// </summary>
        /// <param name="pUsrCode">使用者代碼</param>
        /// <param name="pPrgCode">功能代碼</param>
        /// <param name="pWhere">JSON查詢字串</param>
        /// <returns></returns>
        public List<EMT02_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<EMT02_0000> list = new List<EMT02_0000>();

            string sSql = " SELECT distinct EMT02_0000.dev_check_code, EMT02_0000.*, EMB07_0000.dev_name " +
                          " FROM EMT02_0000 " +
                          " left join EMT02_0100 on EMT02_0100.dev_check_code = EMT02_0100.dev_check_code " +
                          " left join EMB07_0000 on EMB07_0000.dev_code = EMT02_0000.dev_code " +
                          " left join EMB21_0000 on EMB21_0000.chk_item_code = EMT02_0100.chk_item_code " +
                          " left join BDP21_0100 on BDP21_0100.field_code = EMT02_0100.is_ok and BDP21_0100.code_code = 'is_ok_EMT02_0100' ";
            // 取得資料
            list = comm.Get_ListByQuery<EMT02_0000>(sSql, pWhere, pUsrCode, pPrgCode);

            // 權限設定
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);

            bool chk;
            DataTable dt;
            for (int i = 0; i < list.Count; i++)
            {
                chk = false;
                dt = comm.Get_DataTable("select * from EMT02_0100 where dev_check_code = '" + list[i].dev_check_code + "'");
                if (dt.Rows.Count >= 1)
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[j]["is_ok"].ToString()) || !string.IsNullOrEmpty(dt.Rows[j]["sor_code"].ToString()))
                        {
                            chk = true;
                        }
                    }
                }

                //檢查授權刪除、修改
                if(chk)
                    list[i].can_delete = "N";
                else
                    list[i].can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                list[i].can_update = sLimitStr.Contains("M") ? "Y" : "N";
            }

            return list;

        }

        /// <summary>
        /// 傳入一個EMT02_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="EMT02_0000">DTO</param>
        public void InsertData(EMT02_0000 EMT02_0000)
        {
            string sSql = "INSERT INTO " +
                          " EMT02_0000 (  dev_check_code,  dev_check_date,  dev_code,  cmemo,  ins_date,  ins_time,  usr_code ) " +
                          "     VALUES ( @dev_check_code, @dev_check_date, @dev_code, @cmemo, @ins_date, @ins_time, @usr_code ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMT02_0000);
            }
        }

        /// <summary>
        /// 傳入一個EMT02_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="EMT02_0000">DTO</param>
        public void UpdateData(EMT02_0000 EMT02_0000)
        {
            string sSql = " UPDATE EMT02_0000                        " +
                          "    SET dev_check_date = @dev_check_date, " +
                          "        cmemo          = @cmemo,          " +
                          "        dev_code       = @dev_code,       " +
                          "        ins_date       = @ins_date,       " +
                          "        ins_time       = @ins_time,       " +
                          "        usr_code       = @usr_code        " +
                          "  WHERE dev_check_code = @dev_check_code  ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMT02_0000);

            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM EMT02_0000 WHERE dev_check_code = @dev_check_code ";
            sSql += " DELETE FROM EMT02_0100 WHERE dev_check_code = @dev_check_code ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { dev_check_code = pTkCode });

            }
        }

    }
}