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
    public class RSS01_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得RSS01_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO RSS01_0000</returns>
        public RSS01_0000 GetDTO(string pTkCode)
        {
            RSS01_0000 datas = new RSS01_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM RSS01_0000";
            }
            else
            {
                sSql = "SELECT * FROM RSS01_0000 where etl_code=@etl_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@etl_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new RSS01_0000
                        {
                            etl_code = comm.sGetString(reader["etl_code"].ToString()),
                            etl_name = comm.sGetString(reader["etl_name"].ToString()),
                            cmemo = comm.sGetString(reader["cmemo"].ToString()),
                            select_string = comm.sGetString(reader["select_string"].ToString()),
                            where_string = comm.sGetString(reader["where_string"].ToString()),
                            ins_date = comm.sGetString(reader["ins_date"].ToString()),
                            ins_time = comm.sGetString(reader["ins_time"].ToString()),
                            usr_code = comm.sGetString(reader["usr_code"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        /// <summary>
        /// 取得RSS01_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List RSS01_0000</returns>
        public List<RSS01_0000> Get_DataList(string pTkCode)
        {
            List<RSS01_0000> list = new List<RSS01_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM RSS01_0000";
            }
            else
            {
                sSql = "SELECT * FROM RSS01_0000 where etl_code=@etl_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@etl_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    RSS01_0000 data = new RSS01_0000();

                    data.etl_code = comm.sGetString(reader["etl_code"].ToString());
                    data.etl_name = comm.sGetString(reader["etl_name"].ToString());
                    data.cmemo = comm.sGetString(reader["cmemo"].ToString());
                    data.select_string = comm.sGetString(reader["select_string"].ToString());
                    data.where_string = comm.sGetString(reader["where_string"].ToString());
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
        public List<RSS01_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_etl_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<RSS01_0000> list = new List<RSS01_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = " SELECT * FROM RSS01_0000 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@etl_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    RSS01_0000 data = new RSS01_0000();

                    data.etl_code = comm.sGetString(reader["etl_code"].ToString());
                    data.etl_name = comm.sGetString(reader["etl_name"].ToString());
                    data.cmemo = comm.sGetString(reader["cmemo"].ToString());
                    data.select_string = comm.sGetString(reader["select_string"].ToString());
                    data.where_string = comm.sGetString(reader["where_string"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.etl_code)) {
                    //    data.can_delete = "N";
                    //    data.can_update = "N";
                    //}

                    list.Add(data);
                }
            }
            return list;
        }

        /// <summary>
        /// 取得查詢資料，結合使用者權限
        /// </summary>
        /// <param name="pUsrCode">使用者代碼</param>
        /// <param name="pPrgCode">功能代碼</param>
        /// <param name="pWhere">JSON查詢字串</param>
        /// <returns></returns>
        public List<RSS01_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<RSS01_0000> list = new List<RSS01_0000>();

            string sSql = " SELECT DISTINCT RSS01_0000.etl_code, RSS01_0000.* " +
                          " FROM RSS01_0000 " +
                          " LEFT JOIN RSS01_0100 on RSS01_0100.etl_code = RSS01_0000.etl_code " ;

            // 取得資料
            list = comm.Get_ListByQuery<RSS01_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
                //        data.sup_name = data.cmemo + " - " + comm.sGetString(reader["sup_name"].ToString());
                //        data.etl_name = comm.sGetString(reader["etl_code"].ToString()) + " - " + comm.sGetString(reader["etl_name"].ToString());

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
        /// 傳入一個RSS01_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="RSS01_0000">DTO</param>
        public void InsertData(RSS01_0000 RSS01_0000)
        {
            string sSql = " INSERT INTO " +
                          " RSS01_0000 (  etl_code,  etl_name,  cmemo,  select_string,  where_string,  ins_date,  ins_time,  usr_code ) " +
                          "     VALUES ( @etl_code, @etl_name, @cmemo, @select_string, @where_string, @ins_date, @ins_time, @usr_code ) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, RSS01_0000);
            }
        }

        /// <summary>
        /// 傳入一個RSS01_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="RSS01_0000">DTO</param>
        public void UpdateData(RSS01_0000 RSS01_0000)
        {
            string sSql = " UPDATE RSS01_0000                               " +
                          "    SET etl_name          =  @etl_name,          " +
                          "        cmemo             =  @cmemo,             " +
                          "        select_string     =  @select_string,     " +
                          "        where_string      =  @where_string,      " +
                          "        ins_date          =  @ins_date,          " +
                          "        ins_time          =  @ins_time,          " +
                          "        usr_code          =  @usr_code           " +
                          "  WHERE etl_code          =  @etl_code           " ;

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, RSS01_0000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@etl_code", RSS01_0000.etl_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@etl_code", RSS01_0000.etl_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@etl_name", RSS01_0000.etl_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM RSS01_0000 WHERE etl_code = @etl_code; " +
                          " DELETE FROM RSS01_0100 WHERE etl_code = @etl_code; " ;
            //sSql += " Delete from BDP09_0100 where etl_code = @etl_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { etl_code = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@etl_code", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        ////暫存DataTable參考
        //// <summary>
        //// 取得RSS01_0000角色的DataTable
        //// </summary>
        //// <param name = "pTkCode" > 有傳鍵值取一筆，鍵值空白取全部</param>
        //// <returns></returns>
        //public DataTable GetRSS01_0000_dt(string pTkCode)
        //{
        //    DataTable dtTmp = new DataTable();
        //    DataTable dtDat = new DataTable();
        //    dtDat.Columns.Add("etl_code", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("etl_code", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("etl_name", System.Type.GetType("System.String"].ToString());

        //    string sSql = "";
        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM RSS01_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM RSS01_0000 where etl_code='" + pTkCode + "'";
        //    }
        //    dtTmp = comm.Get_DataTable(sSql);

        //    int i;
        //    for (i = 1; i < dtTmp.Rows.Count - 1; i++)
        //    {
        //        DataRow drow = dtDat.NewRow();
        //        drow["etl_code"] = dtTmp.Rows[i]["etl_code"];
        //        drow["etl_code"] = dtTmp.Rows[i]["etl_code"];
        //        drow["etl_name"] = dtTmp.Rows[i]["etl_name"];
        //        dtDat.Rows.Add(drow);
        //    }
        //    return dtDat;
        //}
    }
}