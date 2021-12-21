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
    public class RSS02_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得RSS02_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO RSS02_0000</returns>
        public RSS02_0000 GetDTO(string pTkCode)
        {
            RSS02_0000 datas = new RSS02_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM RSS02_0000";
            }
            else
            {
                sSql = "SELECT * FROM RSS02_0000 where report_code=@report_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@report_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new RSS02_0000
                        {
                            report_code = comm.sGetString(reader["report_code"].ToString()),
                            report_name = comm.sGetString(reader["report_name"].ToString()),
                            report_type = comm.sGetString(reader["report_type"].ToString()),
                            cmemo = comm.sGetString(reader["cmemo"].ToString()),
                            data_source_type = comm.sGetString(reader["data_source_type"].ToString()),
                            etl_code = comm.sGetString(reader["etl_code"].ToString()),
                            epb_code = comm.sGetString(reader["epb_code"].ToString()),
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
        /// 取得RSS02_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List RSS02_0000</returns>
        public List<RSS02_0000> Get_DataList(string pTkCode)
        {
            List<RSS02_0000> list = new List<RSS02_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM RSS02_0000";
            }
            else
            {
                sSql = "SELECT * FROM RSS02_0000 where report_code=@report_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@report_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    RSS02_0000 data = new RSS02_0000();

                    data.report_code = comm.sGetString(reader["report_code"].ToString());
                    data.report_name = comm.sGetString(reader["report_name"].ToString());
                    data.report_type = comm.sGetString(reader["report_type"].ToString());
                    data.cmemo = comm.sGetString(reader["cmemo"].ToString());
                    data.data_source_type = comm.sGetString(reader["data_source_type"].ToString());
                    data.etl_code = comm.sGetString(reader["etl_code"].ToString());
                    data.epb_code = comm.sGetString(reader["epb_code"].ToString());
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
        public List<RSS02_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_report_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<RSS02_0000> list = new List<RSS02_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = " SELECT * FROM RSS02_0000 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@report_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    RSS02_0000 data = new RSS02_0000();

                    data.report_code = comm.sGetString(reader["report_code"].ToString());
                    data.report_name = comm.sGetString(reader["report_name"].ToString());
                    data.report_type = comm.sGetString(reader["report_type"].ToString());
                    data.cmemo = comm.sGetString(reader["cmemo"].ToString());
                    data.data_source_type = comm.sGetString(reader["data_source_type"].ToString());
                    data.etl_code = comm.sGetString(reader["etl_code"].ToString());
                    data.epb_code = comm.sGetString(reader["epb_code"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.report_code)) {
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
        public List<RSS02_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<RSS02_0000> list = new List<RSS02_0000>();

            string sSql = " SELECT DISTINCT RSS02_0000.report_code, RSS02_0000.*, RSS01_0000.etl_name, EPB02_0000.epb_name, BDP21_0100.field_name as report_type_name, A.field_name as data_source_type_name " +
                          " FROM RSS02_0000 " +
                          " LEFT JOIN RSS02_0100 on RSS02_0100.report_code = RSS02_0000.report_code " +
                          " LEFT JOIN RSS01_0000 on RSS01_0000.etl_code = RSS02_0000.etl_code " +
                          " LEFT JOIN EPB02_0000 on EPB02_0000.epb_code = RSS02_0000.epb_code " +
                          " LEFT JOIN BDP21_0100 on BDP21_0100.field_code = RSS02_0000.report_type and BDP21_0100.code_code = 'report_type' " +
                          " LEFT JOIN BDP21_0100 as A on A.field_code = RSS02_0000.data_source_type and A.code_code = 'data_source_type' ";

            // 取得資料
            list = comm.Get_ListByQuery<RSS02_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
                //        data.report_name = comm.sGetString(reader["report_code"].ToString()) + " - " + comm.sGetString(reader["report_name"].ToString());

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
        /// 傳入一個RSS02_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="RSS02_0000">DTO</param>
        public void InsertData(RSS02_0000 RSS02_0000)
        {
            string sSql = " INSERT INTO " +
                          " RSS02_0000 (  report_code,  report_name,  report_type,  cmemo,  data_source_type,  etl_code,  epb_code,  ins_date,  ins_time,  usr_code ) " +
                          "     VALUES ( @report_code, @report_name, @report_type, @cmemo, @data_source_type, @etl_code, @epb_code, @ins_date, @ins_time, @usr_code ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, RSS02_0000);
            }
        }

        /// <summary>
        /// 傳入一個RSS02_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="RSS02_0000">DTO</param>
        public void UpdateData(RSS02_0000 RSS02_0000)
        {
            string sSql = " UPDATE RSS02_0000                               " +
                          "    SET report_name       =  @report_name,       " +
                          "        report_type       =  @report_type,       " +
                          "        cmemo             =  @cmemo,             " +
                          "        data_source_type  =  @data_source_type,  " +
                          "        etl_code          =  @etl_code,          " +
                          "        epb_code          =  @epb_code,          " +
                          "        ins_date          =  @ins_date,          " +
                          "        ins_time          =  @ins_time,          " +
                          "        usr_code          =  @usr_code           " +
                          "  WHERE report_code       =  @report_code        " ;

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, RSS02_0000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@report_code", RSS02_0000.report_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@report_code", RSS02_0000.report_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@report_name", RSS02_0000.report_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM RSS02_0000 WHERE report_code = @report_code; " +
                          "DELETE FROM RSS02_0000 WHERE report_code = @report_code; " ;
            //sSql += " Delete from BDP09_0100 where report_code = @report_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { report_code = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@report_code", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }

     
        ////暫存DataTable參考
        //// <summary>
        //// 取得RSS02_0000角色的DataTable
        //// </summary>
        //// <param name = "pTkCode" > 有傳鍵值取一筆，鍵值空白取全部</param>
        //// <returns></returns>
        //public DataTable GetRSS02_0000_dt(string pTkCode)
        //{
        //    DataTable dtTmp = new DataTable();
        //    DataTable dtDat = new DataTable();
        //    dtDat.Columns.Add("report_code", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("report_code", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("report_name", System.Type.GetType("System.String"].ToString());

        //    string sSql = "";
        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM RSS02_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM RSS02_0000 where report_code='" + pTkCode + "'";
        //    }
        //    dtTmp = comm.Get_DataTable(sSql);

        //    int i;
        //    for (i = 1; i < dtTmp.Rows.Count - 1; i++)
        //    {
        //        DataRow drow = dtDat.NewRow();
        //        drow["report_code"] = dtTmp.Rows[i]["report_code"];
        //        drow["report_code"] = dtTmp.Rows[i]["report_code"];
        //        drow["report_name"] = dtTmp.Rows[i]["report_name"];
        //        dtDat.Rows.Add(drow);
        //    } 
        //    return dtDat;
        //}
    }
}