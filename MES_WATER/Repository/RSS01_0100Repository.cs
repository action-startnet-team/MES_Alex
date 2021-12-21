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
    public class RSS01_0100Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得RSS01_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO RSS01_0100</returns>
        public RSS01_0100 GetDTO(string pTkCode)
        {
            RSS01_0100 datas = new RSS01_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM RSS01_0100";
            }
            else
            {
                sSql = "SELECT * FROM RSS01_0100 where rss01_0100=@rss01_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@rss01_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new RSS01_0100
                        {
                            rss01_0100 = comm.sGetInt32(reader["rss01_0100"].ToString()),
                            etl_code = comm.sGetString(reader["etl_code"].ToString()),
                            field_code = comm.sGetString(reader["field_code"].ToString()),
                            field_name = comm.sGetString(reader["field_name"].ToString()),
                            data_type = comm.sGetString(reader["data_type"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得RSS01_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List RSS01_0100</returns>
        public List<RSS01_0100> Get_DataList(string pTkCode)
        {
            List<RSS01_0100> list = new List<RSS01_0100>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM RSS01_0100";
            }
            else
            {
                sSql = "SELECT * FROM RSS01_0100 where rss01_0100=@rss01_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@rss01_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    RSS01_0100 data = new RSS01_0100();

                    data.rss01_0100 = comm.sGetInt32(reader["rss01_0100"].ToString());
                    data.etl_code = comm.sGetString(reader["etl_code"].ToString());
                    data.field_code = comm.sGetString(reader["field_code"].ToString());
                    data.field_name = comm.sGetString(reader["field_name"].ToString());
                    data.data_type = comm.sGetString(reader["data_type"].ToString());

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
        public List<RSS01_0100> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_rss01_0100", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<RSS01_0100> list = new List<RSS01_0100>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = " SELECT * FROM RSS01_0100 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@rss01_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    RSS01_0100 data = new RSS01_0100();

                    data.rss01_0100 = comm.sGetInt32(reader["rss01_0100"].ToString());
                    data.etl_code = comm.sGetString(reader["etl_code"].ToString());
                    data.field_code = comm.sGetString(reader["field_code"].ToString());
                    data.field_name = comm.sGetString(reader["field_name"].ToString());
                    data.data_type = comm.sGetString(reader["data_type"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.rss01_0100)) {
                    //    data.can_delete = "N";
                    //    data.can_update = "N";
                    //}

                    list.Add(data);
                }
            }
            return list;
        }
        #endregion
        public List<RSS01_0100> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<RSS01_0100> list = new List<RSS01_0100>();
            string foreignKey = gmv.GetKey<RSS01_0000>(new RSS01_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT RSS01_0100.*, BDP21_0100.field_name as data_type_name " +
                       " FROM RSS01_0100 " +
                       " left join BDP21_0100 on BDP21_0100.field_code = RSS01_0100.data_type and BDP21_0100.code_code = 'data_type' " +
                       " where RSS01_0100. " + foreignKey + "=@" + foreignKey +
                       " order by scr_no";
            }
            else
            {
                sSql = "SELECT * FROM RSS01_0100";
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

                    RSS01_0100 data = new RSS01_0100();

                    data.rss01_0100 = comm.sGetInt32(reader["rss01_0100"].ToString());
                    data.etl_code = comm.sGetString(reader["etl_code"].ToString());
                    data.field_code = comm.sGetString(reader["field_code"].ToString());
                    data.field_name = comm.sGetString(reader["field_name"].ToString());
                    data.data_type = comm.sGetString(reader["data_type"].ToString());
                    data.data_type_name = comm.sGetString(reader["data_type_name"].ToString());

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
        /// 傳入一個RSS01_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="RSS01_0100">DTO</param>
        public void InsertData(RSS01_0100 RSS01_0100)
        {
            string sSql = "INSERT INTO " +
                          " RSS01_0100 (  etl_code,  field_code,  field_name,  data_type ) " +
                          "     VALUES ( @etl_code, @field_code, @field_name, @data_type ) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, RSS01_0100);
            }
        }

        /// <summary>
        /// 傳入一個RSS01_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="RSS01_0100">DTO</param>
        public void UpdateData(RSS01_0100 RSS01_0100)
        {
            //string pTkCode = RSS01_0100.rss01_0100.ToString();
            //Int32 iProQty = comm.sGetInt32(comm.Get_Data("RSS01_0100", pTkCode, "rss01_0100", "pro_qty"));
            //Int32 iSorSerial = comm.sGetInt32(comm.Get_Data("RSS01_0100", pTkCode, "rss01_0100", "sor_serial"));

            //ws.Cal_TraQty("DEL", "STT01_0100", "res_qty", iProQty, "where stt01_0100=" + iSorSerial);
            //ws.Cal_TraQty("ADD", "STT01_0100", "res_qty", comm.sGetInt32(RSS01_0100.pro_qty.ToString()), "where stt01_0100=" + comm.sGetString(RSS01_0100.sor_serial.ToString()));


            string sSql = " UPDATE RSS01_0100                 " +
                          "    SET etl_code   =  @etl_code,   " +
                          "        field_code =  @field_code, " +
                          "        field_name =  @field_name, " +
                          "        data_type  =  @data_type   " +
                          "  WHERE rss01_0100 =  @rss01_0100  " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, RSS01_0100);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@rss01_0100", RSS01_0100.rss01_0100));
                //sqlCommand.Parameters.Add(new SqlParameter("@rss01_0100", RSS01_0100.rss01_0100));
                //sqlCommand.Parameters.Add(new SqlParameter("@etl_code", RSS01_0100.etl_code));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM RSS01_0100 WHERE rss01_0100 = @rss01_0100;";
            //sSql += " Delete from BDP09_0100 where rss01_0100 = @rss01_0100; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { rss01_0100 = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@rss01_0100", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }
        ////暫存DataTable參考
        ////<summary>
        ////取得RSS01_0100角色的DataTable
        ////</summary>
        ////<param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        ////<returns></returns>
        //public DataTable GetRSS01_0100_dt(string pTkCode)
        //{
        //    DataTable dtTmp = new DataTable();
        //    DataTable dtDat = new DataTable();
        //    dtDat.Columns.Add("rss01_0100", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("rss01_0100", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("etl_code", System.Type.GetType("System.String"].ToString());
        //    string sSql = "";
        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM RSS01_0100";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM RSS01_0100 where rss01_0100='" + pTkCode + "'";
        //    }
        //    dtTmp = comm.Get_DataTable(sSql);

        //    int i;
        //    for (i = 1; i < dtTmp.Rows.Count - 1; i++)
        //    {
        //        DataRow drow = dtDat.NewRow();
        //        drow["rss01_0100"] = dtTmp.Rows[i]["rss01_0100"];
        //        drow["rss01_0100"] = dtTmp.Rows[i]["rss01_0100"];
        //        drow["etl_code"] = dtTmp.Rows[i]["etl_code"];
        //        dtDat.Rows.Add(drow);
        //    }
        //    return dtDat;
        //}
    }
}