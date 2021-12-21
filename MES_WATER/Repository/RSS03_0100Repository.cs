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
    public class RSS03_0100Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得RSS03_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO RSS03_0100</returns>
        public RSS03_0100 GetDTO(string pTkCode)
        {
            RSS03_0100 datas = new RSS03_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM RSS03_0100";
            }
            else
            {
                sSql = "SELECT * FROM RSS03_0100 where rss03_0100=@rss03_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@rss03_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new RSS03_0100
                        {
                            rss03_0100 = comm.sGetInt32(reader["rss03_0100"].ToString()),
                            report_group_code = comm.sGetString(reader["report_group_code"].ToString()),
                            epb_key_value = comm.sGetString(reader["epb_key_value"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得RSS03_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List RSS03_0100</returns>
        //public List<RSS03_0100> Get_DataList(string pTkCode)
        //{
        //    List<RSS03_0100> list = new List<RSS03_0100>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM RSS03_0100";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM RSS03_0100 where rss03_0100=@rss03_0100";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@rss03_0100", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            RSS03_0100 data = new RSS03_0100();

        //            data.rss03_0100 = comm.sGetString(reader["rss03_0100"].ToString());
        //            data.report_group_code = comm.sGetString(reader["report_group_code"].ToString());
        //            data.epb_key_value = comm.sGetString(reader["epb_key_value"].ToString());
        //            data.aql_up = comm.sGetString(reader["aql_up"].ToString());
        //            data.sample_qty = comm.sGetString(reader["sample_qty"].ToString());

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
        //public List<RSS03_0100> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_rss03_0100", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<RSS03_0100> list = new List<RSS03_0100>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料

        //    sSql = "SELECT * FROM RSS03_0100";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            RSS03_0100 data = new RSS03_0100();

        //            data.rss03_0100 = comm.sGetString(reader["rss03_0100"].ToString());
        //            data.report_group_code = comm.sGetString(reader["report_group_code"].ToString());
        //            data.epb_key_value = comm.sGetString(reader["epb_key_value"].ToString());
        //            data.aql_up = comm.sGetString(reader["aql_up"].ToString());
        //            data.sample_qty = comm.sGetString(reader["sample_qty"].ToString());

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
        public List<RSS03_0100> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<RSS03_0100> list = new List<RSS03_0100>();
            string foreignKey = gmv.GetKey<RSS03_0000>(new RSS03_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT distinct EPB03_0000.key_value, RSS03_0100.*, EPB03_0000.epb_code, EPB03_0000.ins_date, EPB03_0000.ins_time, EPB03_0000.usr_code " +
                       " FROM RSS03_0100 " +
                       " left join EPB03_0000 on EPB03_0000.key_value = RSS03_0100.epb_key_value " +
                       " where EPB03_0000.epb03_0000 in (select min(epb03_0000) as epb03_0000 from EPB03_0000 group by key_value) " +
                       " and RSS03_0100. " + foreignKey + "=@" + foreignKey +
                       " order by rss03_0100 ";
            }
            else
            {
                sSql = "SELECT * FROM RSS03_0100";
            }
            //取得該使用者可以看的資料
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter(foreignKey, pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    RSS03_0100 data = new RSS03_0100();

                    data.rss03_0100 = comm.sGetInt32(reader["rss03_0100"].ToString());
                    data.report_group_code = comm.sGetString(reader["report_group_code"].ToString());
                    data.epb_key_value = comm.sGetString(reader["epb_key_value"].ToString());
                    data.epb_code = comm.sGetString(reader["epb_code"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());

                    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改

                    list.Add(data);
                }

            }
            return list;
        }

        /// <summary>
        /// 傳入一個RSS03_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="RSS03_0100">DTO</param>
        public void InsertData(RSS03_0100 RSS03_0100)
        {
            string sSql = "INSERT INTO " +
                          " RSS03_0100 (  report_group_code,  epb_key_value ) " +
                          "     VALUES ( @report_group_code, @epb_key_value ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, RSS03_0100);
            }
        }

        /// <summary>
        /// 傳入一個RSS03_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="RSS03_0100">DTO</param>
        public void UpdateData(RSS03_0100 RSS03_0100)
        {
            string sSql = " UPDATE RSS03_0100                     " +
                          "    SET epb_key_value = @epb_key_value " +
                          "  WHERE rss03_0100    = @rss03_0100    ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, RSS03_0100);

            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM RSS03_0100 WHERE rss03_0100 = @rss03_0100 ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { rss03_0100 = pTkCode });

            }
        }

    }
}