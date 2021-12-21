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
    public class RSS03_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得RSS03_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO RSS03_0000</returns>
        public RSS03_0000 GetDTO(string pTkCode)
        {
            RSS03_0000 datas = new RSS03_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM RSS03_0000";
            }
            else
            {
                sSql = "SELECT * FROM RSS03_0000 where report_group_code=@report_group_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@report_group_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new RSS03_0000
                        {
                            report_group_code = comm.sGetString(reader["report_group_code"].ToString()),
                            report_code = comm.sGetString(reader["report_code"].ToString()),
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
        /// 取得RSS03_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List RSS03_0000</returns>
        //public List<RSS03_0000> Get_DataList(string pTkCode)
        //{
        //    List<RSS03_0000> list = new List<RSS03_0000>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM RSS03_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM RSS03_0000 where report_group_code=@report_group_code";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@report_group_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            RSS03_0000 data = new RSS03_0000();

        //            data.report_group_code = comm.sGetString(reader["report_group_code"].ToString());
        //            data.report_code = comm.sGetString(reader["report_code"].ToString());
        //            data.cmemo = comm.sGetString(reader["cmemo"].ToString());
        //            data.ins_date = comm.sGetString(reader["ins_date"].ToString());
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
        //public List<RSS03_0000> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_report_group_code", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<RSS03_0000> list = new List<RSS03_0000>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    sSql = "SELECT * FROM RSS03_0000";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            RSS03_0000 data = new RSS03_0000();

        //            data.report_group_code = comm.sGetString(reader["report_group_code"].ToString());
        //            data.report_code = comm.sGetString(reader["report_code"].ToString());
        //            data.cmemo = comm.sGetString(reader["cmemo"].ToString());
        //            data.ins_date = comm.sGetString(reader["ins_date"].ToString());
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
        public List<RSS03_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<RSS03_0000> list = new List<RSS03_0000>();

            string sSql = " SELECT distinct RSS03_0000.report_group_code, RSS03_0000.*,report_name " +
                          "                 ,(select out_date from EPB05_0000 where report_group_code = RSS03_0000.report_group_code and result_code in('99','98')) as end_date " + 
                          " FROM RSS03_0000 " +
                          " left join RSS03_0100 on RSS03_0100.report_group_code = RSS03_0100.report_group_code " +
                          " left join RSS02_0000 on RSS03_0000.report_code = RSS02_0000.report_code " +
                          " left join EPB03_0000 on EPB03_0000.key_value = RSS03_0100.epb_key_value ";
                          //" where RSS03_0000.report_group_code in(select report_group_code from EPB05_0000 where result_code = '99')";
            // 取得資料
            list = comm.Get_ListByQuery<RSS03_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個RSS03_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="RSS03_0000">DTO</param>
        public void InsertData(RSS03_0000 RSS03_0000)
        {
            string sSql = "INSERT INTO " +
                          " RSS03_0000 (  report_group_code,  report_code,  ins_date,  ins_time,  usr_code ) " +
                          "     VALUES ( @report_group_code, @report_code, @ins_date, @ins_time, @usr_code ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, RSS03_0000);
            }
        }

        /// <summary>
        /// 傳入一個RSS03_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="RSS03_0000">DTO</param>
        public void UpdateData(RSS03_0000 RSS03_0000)
        {
            string sSql = " UPDATE RSS03_0000                             " +
                          "    SET report_code       = @report_code,      " +
                          "        ins_date          = @ins_date,         " +
                          "        ins_time          = @ins_time,         " +
                          "        usr_code          = @usr_code          " +
                          "  WHERE report_group_code = @report_group_code ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, RSS03_0000);

            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM RSS03_0000 WHERE report_group_code = @report_group_code " +
                          " DELETE FROM RSS03_0100 WHERE report_group_code = @report_group_code " +
                          " DELETE FROM EPB05_0000 WHERE report_group_code = @report_group_code ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { report_group_code = pTkCode });

            }
        }

    }
}