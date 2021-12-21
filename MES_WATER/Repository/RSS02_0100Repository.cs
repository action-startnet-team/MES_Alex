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
    public class RSS02_0100Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得RSS02_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO RSS02_0100</returns>
        public RSS02_0100 GetDTO(string pTkCode)
        {
            RSS02_0100 datas = new RSS02_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM RSS02_0100";
            }
            else
            {
                sSql = "SELECT * FROM RSS02_0100 where rss02_0100=@rss02_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@rss02_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new RSS02_0100
                        {
                            rss02_0100 = comm.sGetInt32(reader["rss02_0100"].ToString()),
                            report_code = comm.sGetString(reader["report_code"].ToString()),
                            field_code = comm.sGetString(reader["field_code"].ToString()),
                            field_name = comm.sGetString(reader["field_name"].ToString()),
                            scr_no = comm.sGetInt32(reader["scr_no"].ToString()),
                            ctr_type = comm.sGetString(reader["ctr_type"].ToString()),
                            ctr_default_value = comm.sGetString(reader["ctr_default_value"].ToString()),
                            data_type = comm.sGetString(reader["data_type"].ToString()),
                            select_code = comm.sGetString(reader["select_code"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得RSS02_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List RSS02_0100</returns>
        public List<RSS02_0100> Get_DataList(string pTkCode)
        {
            List<RSS02_0100> list = new List<RSS02_0100>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM RSS02_0100";
            }
            else
            {
                sSql = "SELECT * FROM RSS02_0100 where rss02_0100=@rss02_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@rss02_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    RSS02_0100 data = new RSS02_0100();

                    data.rss02_0100 = comm.sGetInt32(reader["rss02_0100"].ToString());
                    data.report_code = comm.sGetString(reader["report_code"].ToString());
                    data.field_code = comm.sGetString(reader["field_code"].ToString());
                    data.field_name = comm.sGetString(reader["field_name"].ToString());
                    data.scr_no = comm.sGetInt32(reader["scr_no"].ToString());
                    data.ctr_type = comm.sGetString(reader["ctr_type"].ToString());
                    data.ctr_default_value = comm.sGetString(reader["ctr_default_value"].ToString());
                    data.data_type = comm.sGetString(reader["data_type"].ToString());
                    data.select_code = comm.sGetString(reader["select_code"].ToString());

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
        public List<RSS02_0100> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_rss02_0100", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<RSS02_0100> list = new List<RSS02_0100>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = " SELECT * FROM RSS02_0100 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@rss02_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    RSS02_0100 data = new RSS02_0100();

                    data.rss02_0100 = comm.sGetInt32(reader["rss02_0100"].ToString());
                    data.report_code = comm.sGetString(reader["report_code"].ToString());
                    data.field_code = comm.sGetString(reader["field_code"].ToString());
                    data.field_name = comm.sGetString(reader["field_name"].ToString());
                    data.scr_no = comm.sGetInt32(reader["scr_no"].ToString());
                    data.ctr_type = comm.sGetString(reader["ctr_type"].ToString());
                    data.ctr_default_value = comm.sGetString(reader["ctr_default_value"].ToString());
                    data.data_type = comm.sGetString(reader["data_type"].ToString());
                    data.select_code = comm.sGetString(reader["select_code"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.rss02_0100)) {
                    //    data.can_delete = "N";
                    //    data.can_update = "N";
                    //}

                    list.Add(data);
                }
            }
            return list;
        }
        #endregion
        public List<RSS02_0100> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<RSS02_0100> list = new List<RSS02_0100>();
            string foreignKey = gmv.GetKey<RSS02_0000>(new RSS02_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT RSS02_0100.*, BDP21_0100.field_name as ctr_type_name, A.field_name as data_type_name, BDP31_0000.select_name " +
                       " FROM RSS02_0100 " +
                       " left join BDP21_0100 on BDP21_0100.field_code = RSS02_0100.ctr_type and BDP21_0100.code_code = 'ctr_type' " +
                       " left join BDP21_0100 as A on A.field_code = RSS02_0100.data_type and A.code_code = 'data_type' " +
                       " left join BDP31_0000 on BDP31_0000.select_code = RSS02_0100.select_code " +
                       " where RSS02_0100. " + foreignKey + "=@" + foreignKey;
            }
            else
            {
                sSql = "SELECT * FROM RSS02_0100";
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

                    RSS02_0100 data = new RSS02_0100();

                    data.rss02_0100 = comm.sGetInt32(reader["rss02_0100"].ToString());
                    data.report_code = comm.sGetString(reader["report_code"].ToString());
                    data.field_code = comm.sGetString(reader["field_code"].ToString());
                    data.field_name = comm.sGetString(reader["field_name"].ToString());
                    data.scr_no = comm.sGetInt32(reader["scr_no"].ToString());
                    data.ctr_type = comm.sGetString(reader["ctr_type"].ToString());
                    data.ctr_type_name = comm.sGetString(reader["ctr_type_name"].ToString());
                    data.ctr_default_value = comm.sGetString(reader["ctr_default_value"].ToString());
                    data.data_type = comm.sGetString(reader["data_type"].ToString());
                    data.data_type_name = comm.sGetString(reader["data_type_name"].ToString());
                    data.select_code = comm.sGetString(reader["select_code"].ToString());
                    data.select_name = comm.sGetString(reader["select_name"].ToString());

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
        /// 傳入一個RSS02_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="RSS02_0100">DTO</param>
        public void InsertData(RSS02_0100 RSS02_0100)
        {
            string sSql = "INSERT INTO " +
                          " RSS02_0100 (  report_code,  field_code,  field_name,  scr_no,  ctr_type,  ctr_default_value,  data_type,  select_code ) " +
                          "     VALUES ( @report_code, @field_code, @field_name, @scr_no, @ctr_type, @ctr_default_value, @data_type, @select_code ) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, RSS02_0100);
            }
        }

        /// <summary>
        /// 傳入一個RSS02_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="RSS02_0100">DTO</param>
        public void UpdateData(RSS02_0100 RSS02_0100)
        {
            //string pTkCode = RSS02_0100.rss02_0100.ToString();
            //Int32 iProQty = comm.sGetInt32(comm.Get_Data("RSS02_0100", pTkCode, "rss02_0100", "pro_qty"));
            //Int32 iSorSerial = comm.sGetInt32(comm.Get_Data("RSS02_0100", pTkCode, "rss02_0100", "sor_serial"));

            //ws.Cal_TraQty("DEL", "STT01_0100", "res_qty", iProQty, "where stt01_0100=" + iSorSerial);
            //ws.Cal_TraQty("ADD", "STT01_0100", "res_qty", comm.sGetInt32(RSS02_0100.pro_qty.ToString()), "where stt01_0100=" + comm.sGetString(RSS02_0100.sor_serial.ToString()));


            string sSql = " UPDATE RSS02_0100                               " +
                          "    SET report_code       =  @report_code,       " +
                          "        field_code        =  @field_code,        " +
                          "        field_name        =  @field_name,        " +
                          "        scr_no            =  @scr_no,            " +
                          "        ctr_type          =  @ctr_type,          " +
                          "        ctr_default_value =  @ctr_default_value, " +
                          "        data_type         =  @data_type,         " +
                          "        select_code       =  @select_code        " +
                          "  WHERE rss02_0100        =  @rss02_0100         " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, RSS02_0100);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@rss02_0100", RSS02_0100.rss02_0100));
                //sqlCommand.Parameters.Add(new SqlParameter("@rss02_0100", RSS02_0100.rss02_0100));
                //sqlCommand.Parameters.Add(new SqlParameter("@report_code", RSS02_0100.report_code));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM RSS02_0100 WHERE rss02_0100 = @rss02_0100;";
            //sSql += " Delete from BDP09_0100 where rss02_0100 = @rss02_0100; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { rss02_0100 = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@rss02_0100", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }
        ////暫存DataTable參考
        ////<summary>
        ////取得RSS02_0100角色的DataTable
        ////</summary>
        ////<param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        ////<returns></returns>
        //public DataTable GetRSS02_0100_dt(string pTkCode)
        //{
        //    DataTable dtTmp = new DataTable();
        //    DataTable dtDat = new DataTable();
        //    dtDat.Columns.Add("rss02_0100", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("rss02_0100", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("report_code", System.Type.GetType("System.String"].ToString());
        //    string sSql = "";
        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM RSS02_0100";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM RSS02_0100 where rss02_0100='" + pTkCode + "'";
        //    }
        //    dtTmp = comm.Get_DataTable(sSql);

        //    int i;
        //    for (i = 1; i < dtTmp.Rows.Count - 1; i++)
        //    {
        //        DataRow drow = dtDat.NewRow();
        //        drow["rss02_0100"] = dtTmp.Rows[i]["rss02_0100"];
        //        drow["rss02_0100"] = dtTmp.Rows[i]["rss02_0100"];
        //        drow["report_code"] = dtTmp.Rows[i]["report_code"];
        //        dtDat.Rows.Add(drow);
        //    }
        //    return dtDat;
        //}
    }
}