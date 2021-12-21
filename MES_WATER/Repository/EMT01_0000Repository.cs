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
    public class EMT01_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得EMT01_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO EMT01_0000</returns>
        public EMT01_0000 GetDTO(string pTkCode)
        {
            EMT01_0000 datas = new EMT01_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMT01_0000";
            }
            else
            {
                sSql = "SELECT * FROM EMT01_0000 where maintain_code=@maintain_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@maintain_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new EMT01_0000
                        {
                            maintain_code = comm.sGetString(reader["maintain_code"].ToString()),
                            maintain_name = comm.sGetString(reader["maintain_name"].ToString()),
                            dev_code = comm.sGetString(reader["dev_code"].ToString()),
                            cmemo = comm.sGetString(reader["cmemo"].ToString()),
                            ins_date = comm.sGetString(reader["ins_date"].ToString()),
                            ins_time = comm.sGetString(reader["ins_time"].ToString()),
                            usr_code = comm.sGetString(reader["usr_code"].ToString()),
                            is_use = comm.sGetString(reader["is_use"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得EMT01_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List EMT01_0000</returns>
        //public List<EMT01_0000> Get_DataList(string pTkCode)
        //{
        //    List<EMT01_0000> list = new List<EMT01_0000>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM EMT01_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM EMT01_0000 where maintain_code=@maintain_code";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@maintain_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            EMT01_0000 data = new EMT01_0000();

        //            data.maintain_code = comm.sGetString(reader["maintain_code"].ToString());
        //            data.maintain_name = comm.sGetString(reader["maintain_name"].ToString());
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
        //public List<EMT01_0000> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_maintain_code", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<EMT01_0000> list = new List<EMT01_0000>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    sSql = "SELECT * FROM EMT01_0000";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            EMT01_0000 data = new EMT01_0000();

        //            data.maintain_code = comm.sGetString(reader["maintain_code"].ToString());
        //            data.maintain_name = comm.sGetString(reader["maintain_name"].ToString());
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
        public List<EMT01_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<EMT01_0000> list = new List<EMT01_0000>();

            string sSql = " SELECT distinct EMT01_0000.maintain_code, EMT01_0000.*, EMB07_0000.dev_name, BDP08_0000.usr_name " +
                          " FROM EMT01_0000 " +
                          " left join EMT01_0100 on EMT01_0100.maintain_code = EMT01_0100.maintain_code " +
                          " left join EMB07_0000 on EMB07_0000.dev_code = EMT01_0000.dev_code " +
                          " left join BDP08_0000 on BDP08_0000.usr_code = EMT01_0000.usr_code " +
                          " left join EMB08_0000 on EMB08_0000.main_item_code = EMT01_0100.main_item_code " +
                          " left join BDP08_0000 as A on A.usr_code = EMT01_0100.usr_code " ;
            // 取得資料
            list = comm.Get_ListByQuery<EMT01_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個EMT01_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="EMT01_0000">DTO</param>
        public void InsertData(EMT01_0000 EMT01_0000)
        {
            string sSql = "INSERT INTO " +
                          " EMT01_0000 (  maintain_code,  maintain_name,  dev_code,  cmemo,  ins_date,  ins_time,  usr_code ) " +
                          "     VALUES ( @maintain_code, @maintain_name, @dev_code, @cmemo, @ins_date, @ins_time, @usr_code ) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMT01_0000);
            }
        }

        /// <summary>
        /// 傳入一個EMT01_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="EMT01_0000">DTO</param>
        public void UpdateData(EMT01_0000 EMT01_0000)
        {
            string sSql = " UPDATE EMT01_0000                       " +
                          "    SET maintain_name =  @maintain_name, " +
                          "        cmemo         =  @cmemo,         " +
                          "        dev_code      =  @dev_code,      " +
                          "        ins_date      =  @ins_date,      " +
                          "        ins_time      =  @ins_time,      " +
                          "        usr_code      =  @usr_code       " +
                          "  WHERE maintain_code =  @maintain_code  " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMT01_0000);

            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM EMT01_0000 WHERE maintain_code = @maintain_code ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { maintain_code = pTkCode });

            }
        }

    }
}