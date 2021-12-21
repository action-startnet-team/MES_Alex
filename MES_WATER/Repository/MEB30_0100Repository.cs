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
    public class MEB30_0100Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得MEB30_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEB30_0100</returns>
        public MEB30_0100 GetDTO(string pTkCode)
        {
            MEB30_0100 datas = new MEB30_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB30_0100";
            }
            else
            {
                sSql = "SELECT * FROM MEB30_0100 where work_code=@work_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@work_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEB30_0100
                        {
                            meb30_0100 = comm.sGetInt32(reader["meb30_0100"].ToString()),                
                            work_code = comm.sGetString(reader["work_code"].ToString()),
                            station_code = comm.sGetString(reader["station_code"].ToString()),

                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MEB30_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MEB30_0100</returns>
        public List<MEB30_0100> Get_DataList(string pTkCode)
        {
            List<MEB30_0100> list = new List<MEB30_0100>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB30_0100";
            }
            else
            {
                sSql = "SELECT * FROM MEB30_0100 where work_code=@work_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@work_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB30_0100 data = new MEB30_0100();

                    data.meb30_0100 = comm.sGetInt32(reader["meb30_0100"].ToString());
                    data.work_code = comm.sGetString(reader["work_code"].ToString());
                    data.station_code = comm.sGetString(reader["station_code"].ToString());

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
        public List<MEB30_0100> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_work_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MEB30_0100> list = new List<MEB30_0100>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = " SELECT * FROM MEB30_0100 " ;

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@work_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB30_0100 data = new MEB30_0100();

                    data.meb30_0100 = comm.sGetInt32(reader["meb30_0100"].ToString());
                    data.work_code = comm.sGetString(reader["work_code"].ToString());     
                    data.station_code = comm.sGetString(reader["station_code"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.work_code)) {
                    //    data.can_delete = "N";
                    //    data.can_update = "N";
                    //}

                    list.Add(data);
                }
            }
            return list;
        }
        #endregion
        public List<MEB30_0100> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<MEB30_0100> list = new List<MEB30_0100>();
            string foreignKey = gmv.GetKey<MEB30_0000>(new MEB30_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT MEB30_0100.* , MEB29_0000.station_name as station_name " +
                       " FROM MEB30_0100                                      " +
                       " left join MEB29_0000 on MEB29_0000.station_code = MEB30_0100.station_code " +
                       " where MEB30_0100. " + foreignKey + "=@" + foreignKey ;
            }
            else
            { 
                sSql = "SELECT * FROM MEB30_0100";
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
                    
                    MEB30_0100 data = new MEB30_0100();

                    data.meb30_0100 = comm.sGetInt32(reader["meb30_0100"].ToString());
                    data.work_code = comm.sGetString(reader["work_code"].ToString());
                    data.station_code = comm.sGetString(reader["station_code"].ToString());
                    data.station_name = comm.sGetString(reader["station_name"].ToString());

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
        /// 傳入一個MEB30_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEB30_0100">DTO</param>
        /// 

        //取得識別碼

        public void InsertData(MEB30_0100 MEB30_0100)
        {
            //MEB30_0100.meb30_0100 = comm.sGetInt32(ws.AutoInt2("MEB30_0100").ToString());

            string sSql = "INSERT INTO " +
                          " MEB30_0100 (  work_code,   station_code   ) " +
                          "     VALUES (@work_code,  @station_code   ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB30_0100);
            }
        }

        /// <summary>
        /// 傳入一個MEB30_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEB30_0100">DTO</param>
        public void UpdateData(MEB30_0100 MEB30_0100)
        {
            //string pTkCode = MEB30_0100.work_code.ToString();
            //Int32 iProQty = comm.sGetInt32(comm.Get_Data("MEB30_0100", pTkCode, "work_code", "pro_qty"));
            //Int32 iSorSerial = comm.sGetInt32(comm.Get_Data("MEB30_0100", pTkCode, "work_code", "sor_serial"));

            //ws.Cal_TraQty("DEL", "STT01_0100", "res_qty", iProQty, "where stt01_0100=" + iSorSerial);
            //ws.Cal_TraQty("ADD", "STT01_0100", "res_qty", comm.sGetInt32(MEB30_0100.pro_qty.ToString()), "where stt01_0100=" + comm.sGetString(MEB30_0100.sor_serial.ToString()));


            string sSql = " UPDATE MEB30_0100            " +
                          "    SET work_code = @work_code, " +
                          "        station_code = @station_code " +
                          "  WHERE meb30_0100 = @meb30_0100  ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB30_0100);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@work_code", MEB30_0100.work_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@work_code", MEB30_0100.work_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@meb30_0100", MEB30_0100.meb30_0100));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MEB30_0100 WHERE meb30_0100 = @meb30_0100;";
            //sSql += " Delete from BDP09_0100 where work_code = @work_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { meb30_0100 = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@work_code", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }
        
        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得MEB30_0100角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetMEB30_0100_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();
            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("work_code", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("work_code", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("meb30_0100", System.Type.GetType("System.String"].ToString());
            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB30_0100";
            }
            else
            {
                sSql = "SELECT * FROM MEB30_0100 where work_code='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["work_code"] = dtTmp.Rows[i]["work_code"];
                drow["work_code"] = dtTmp.Rows[i]["work_code"];
                drow["meb30_0100"] = dtTmp.Rows[i]["meb30_0100"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}