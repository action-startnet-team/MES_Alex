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
    public class WMB01_0100Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得WMB02_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO WMB01_0100</returns>
        public WMB02_0000 GetDTO(string pTkCode)
        {
            WMB02_0000 datas = new WMB02_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM WMB02_0000";
            }
            else
            {
                sSql = "SELECT * FROM WMB02_0000 where loc_code=@loc_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@loc_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new WMB02_0000
                        {                   
                            loc_code = comm.sGetString(reader["loc_code"].ToString()),
                            loc_name = comm.sGetString(reader["loc_name"].ToString()),
                            sto_code = comm.sGetString(reader["sto_code"].ToString()),
                            loc_type = comm.sGetString(reader["loc_type"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得WMB02_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List WMB01_0100</returns>
        public List<WMB02_0000> Get_DataList(string pTkCode)
        {
            List<WMB02_0000> list = new List<WMB02_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM WMB02_0000";
            }
            else
            {
                sSql = "SELECT * FROM WMB02_0000 where loc_code=@loc_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@loc_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    WMB02_0000 data = new WMB02_0000();

                    data.loc_code = comm.sGetString(reader["loc_code"].ToString());
                    data.loc_name = comm.sGetString(reader["loc_name"].ToString());
                    data.sto_code = comm.sGetString(reader["sto_code"].ToString());
                    data.loc_type = comm.sGetString(reader["loc_type"].ToString());

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
        public List<WMB02_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_loc_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<WMB02_0000> list = new List<WMB02_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = " SELECT * FROM WMB02_0000 " ;

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@loc_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    WMB02_0000 data = new WMB02_0000();

                    data.loc_code = comm.sGetString(reader["loc_code"].ToString());
                    data.loc_name = comm.sGetString(reader["loc_name"].ToString());
                    data.sto_code = comm.sGetString(reader["sto_code"].ToString());
                    data.loc_type = comm.sGetString(reader["loc_type"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.loc_code)) {
                    //    data.can_delete = "N";
                    //    data.can_update = "N";
                    //}

                    list.Add(data);
                }
            }
            return list;
        }
        #endregion
        public List<WMB02_0000> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<WMB02_0000> list = new List<WMB02_0000>();
            string foreignKey = gmv.GetKey<WMB01_0000>(new WMB01_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT WMB02_0000.*, WMB01_0000.sto_name as sto_name, BDP21_0100.field_name as loc_type_name              " +
                       " FROM WMB02_0000                                                                                           " +
                       " left join WMB01_0000 on WMB01_0000.sto_code = WMB02_0000.sto_code                                         " +
                       " left join BDP21_0100 on BDP21_0100.field_code = WMB02_0000.loc_type and BDP21_0100.code_code = 'loc_type' " +
                       " where WMB02_0000. " + foreignKey + "=@" + foreignKey ;
            }
            else
            { 
                sSql = "SELECT * FROM WMB02_0000";
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
                    
                    WMB02_0000 data = new WMB02_0000();

                    data.loc_code = comm.sGetString(reader["loc_code"].ToString());
                    data.loc_name = comm.sGetString(reader["loc_name"].ToString());
                    data.sto_code = comm.sGetString(reader["sto_code"].ToString());
                    data.loc_type = comm.sGetString(reader["loc_type"].ToString());
                    data.loc_type_name = comm.sGetString(reader["loc_type_name"].ToString());

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
        /// 傳入一個WMB01_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="WMB01_0100">DTO</param>
        public void InsertData(WMB02_0000 WMB01_0100)
        {
            string sSql = "INSERT INTO " +
                          " WMB02_0000 (  loc_code,      loc_name,   sto_code,     loc_type ) " +
                          "     VALUES ( @loc_code,     @loc_name,   @sto_code,   @loc_type ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMB01_0100);
            }
        }

        /// <summary>
        /// 傳入一個WMB01_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="WMB01_0100">DTO</param>
        public void UpdateData(WMB02_0000 WMB01_0100)
        {
            //string pTkCode = WMB01_0100.loc_code.ToString();
            //Int32 iProQty = comm.sGetInt32(comm.Get_Data("WMB01_0100", pTkCode, "loc_code", "pro_qty"));
            //Int32 iSorSerial = comm.sGetInt32(comm.Get_Data("WMB01_0100", pTkCode, "loc_code", "sor_serial"));

            //ws.Cal_TraQty("DEL", "STT01_0100", "res_qty", iProQty, "where stt01_0100=" + iSorSerial);
            //ws.Cal_TraQty("ADD", "STT01_0100", "res_qty", comm.sGetInt32(WMB01_0100.pro_qty.ToString()), "where stt01_0100=" + comm.sGetString(WMB01_0100.sor_serial.ToString()));


            string sSql = " UPDATE WMB02_0000            " +
                          "    SET loc_name = @loc_name, " +
                          "        sto_code = @sto_code, " +
                          "        loc_type = @loc_type  " +
                          "  WHERE loc_code = @loc_code  " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMB01_0100);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@loc_code", WMB01_0100.loc_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@loc_code", WMB01_0100.loc_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@loc_name", WMB01_0100.loc_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM WMB02_0000 WHERE loc_code = @loc_code;";
            //sSql += " Delete from BDP09_0100 where loc_code = @loc_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { loc_code = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@loc_code", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }
        
        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得WMB01_0100角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetWMB01_0100_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();
            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("loc_code", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("loc_code", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("loc_name", System.Type.GetType("System.String"].ToString());
            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM WMB01_0100";
            }
            else
            {
                sSql = "SELECT * FROM WMB01_0100 where loc_code='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["loc_code"] = dtTmp.Rows[i]["loc_code"];
                drow["loc_code"] = dtTmp.Rows[i]["loc_code"];
                drow["loc_name"] = dtTmp.Rows[i]["loc_name"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}