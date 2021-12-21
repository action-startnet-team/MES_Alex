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
    public class DTS01_0100Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得DTS01_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO DTS01_0100</returns>
        public DTS01_0100 GetDTO(string pTkCode)
        {
            DTS01_0100 datas = new DTS01_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM DTS01_0100";
            }
            else
            {
                sSql = "SELECT * FROM DTS01_0100 where dts01_0100=@dts01_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@dts01_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new DTS01_0100
                        {                   
                            dts01_0100 = comm.sGetString(reader["dts01_0100"].ToString()),
                            dts01_0000 = comm.sGetString(reader["dts01_0000"].ToString()),
                            table_name = comm.sGetString(reader["table_name"].ToString()),
                            con_response = comm.sGetString(reader["con_response"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得DTS01_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List DTS01_0100</returns>
        public List<DTS01_0100> Get_DataList(string pTkCode)
        {
            List<DTS01_0100> list = new List<DTS01_0100>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM DTS01_0100";
            }
            else
            {
                sSql = "SELECT * FROM DTS01_0100 where dts01_0100=@dts01_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@dts01_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    DTS01_0100 data = new DTS01_0100();

                    data.dts01_0100 = comm.sGetString(reader["dts01_0100"].ToString());
                    data.dts01_0000 = comm.sGetString(reader["dts01_0000"].ToString());
                    data.table_name = comm.sGetString(reader["table_name"].ToString());
                    data.con_response = comm.sGetString(reader["con_response"].ToString());

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
        public List<DTS01_0100> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_dts01_0100", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<DTS01_0100> list = new List<DTS01_0100>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = " SELECT * FROM DTS01_0100 " ;

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@dts01_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    DTS01_0100 data = new DTS01_0100();

                    data.dts01_0100 = comm.sGetString(reader["dts01_0100"].ToString());
                    data.dts01_0000 = comm.sGetString(reader["dts01_0000"].ToString());
                    data.table_name = comm.sGetString(reader["table_name"].ToString());
                    data.con_response = comm.sGetString(reader["con_response"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.dts01_0100)) {
                    //    data.can_delete = "N";
                    //    data.can_update = "N";
                    //}

                    list.Add(data);
                }
            }
            return list;
        }
        #endregion
        public List<DTS01_0100> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<DTS01_0100> list = new List<DTS01_0100>();
            string foreignKey = gmv.GetKey<DTS01_0000>(new DTS01_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT DTS01_0100.*             " +
                       " FROM   DTS01_0100               " +
                       " where  DTS01_0100. " + foreignKey + "=@" + foreignKey ;
            }
            else
            { 
                sSql = "SELECT * FROM DTS01_0100";
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
                    
                    DTS01_0100 data = new DTS01_0100();

                    data.dts01_0100 = comm.sGetString(reader["dts01_0100"].ToString());
                    data.dts01_0000 = comm.sGetString(reader["dts01_0000"].ToString());
                    data.table_name = comm.sGetString(reader["table_name"].ToString());
                    data.con_response = comm.sGetString(reader["con_response"].ToString());
                 

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
        /// 傳入一個DTS01_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="DTS01_0100">DTO</param>
        public void InsertData(DTS01_0100 DTS01_0100)
        {
            string sSql = "INSERT INTO " +
                          " DTS01_0100 (  dts01_0100,      dts01_0000,   table_name,     con_response ) " +
                          "     VALUES ( @dts01_0100,     @dts01_0000,   @table_name,   @con_response ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, DTS01_0100);
            }
        }

        /// <summary>
        /// 傳入一個DTS01_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="DTS01_0100">DTO</param>
        public void UpdateData(DTS01_0100 DTS01_0100)
        {
            //string pTkCode = DTS01_0100.dts01_0100.ToString();
            //Int32 iProQty = comm.sGetInt32(comm.Get_Data("DTS01_0100", pTkCode, "dts01_0100", "pro_qty"));
            //Int32 iSorSerial = comm.sGetInt32(comm.Get_Data("DTS01_0100", pTkCode, "dts01_0100", "sor_serial"));

            //ws.Cal_TraQty("DEL", "STT01_0100", "res_qty", iProQty, "where stt01_0100=" + iSorSerial);
            //ws.Cal_TraQty("ADD", "STT01_0100", "res_qty", comm.sGetInt32(DTS01_0100.pro_qty.ToString()), "where stt01_0100=" + comm.sGetString(DTS01_0100.sor_serial.ToString()));


            string sSql = " UPDATE DTS01_0100            " +
                          "    SET dts01_0000 = @dts01_0000, " +
                          "        table_name = @table_name, " +
                          "        con_response = @con_response  " +
                          "  WHERE dts01_0100 = @dts01_0100  " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, DTS01_0100);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@dts01_0100", DTS01_0100.dts01_0100));
                //sqlCommand.Parameters.Add(new SqlParameter("@dts01_0100", DTS01_0100.dts01_0100));
                //sqlCommand.Parameters.Add(new SqlParameter("@dts01_0000", DTS01_0100.dts01_0000));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM DTS01_0100 WHERE dts01_0100 = @dts01_0100;";
            //sSql += " Delete from BDP09_0100 where dts01_0100 = @dts01_0100; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { dts01_0100 = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@dts01_0100", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }
        
        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得DTS01_0100角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetDTS01_0100_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();
            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("dts01_0100", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("dts01_0100", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("dts01_0000", System.Type.GetType("System.String"].ToString());
            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM DTS01_0100";
            }
            else
            {
                sSql = "SELECT * FROM DTS01_0100 where dts01_0100='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["dts01_0100"] = dtTmp.Rows[i]["dts01_0100"];
                drow["dts01_0100"] = dtTmp.Rows[i]["dts01_0100"];
                drow["dts01_0000"] = dtTmp.Rows[i]["dts01_0000"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}