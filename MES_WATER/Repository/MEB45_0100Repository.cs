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
    public class MEB45_0100Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得MEB45_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEB45_0100</returns>
        public MEB45_0100 GetDTO(string pTkCode)
        {
            MEB45_0100 datas = new MEB45_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB45_0100";
            }
            else
            {
                sSql = "SELECT * FROM MEB45_0100 where mac_code=@mac_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@mac_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEB45_0100
                        {
                            meb45_0100 = comm.sGetInt32(reader["meb45_0100"].ToString()),
                            mac_code = comm.sGetString(reader["mac_code"].ToString()),
                            stop_code = comm.sGetString(reader["stop_code"].ToString()),

                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MEB45_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MEB45_0100</returns>
        public List<MEB45_0100> Get_DataList(string pTkCode)
        {
            List<MEB45_0100> list = new List<MEB45_0100>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB45_0100";
            }
            else
            {
                sSql = "SELECT * FROM MEB45_0100 where mac_code=@mac_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@mac_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB45_0100 data = new MEB45_0100();

                    data.meb45_0100 = comm.sGetInt32(reader["meb45_0100"].ToString());
                    data.mac_code = comm.sGetString(reader["mac_code"].ToString());
                    data.stop_code = comm.sGetString(reader["stop_code"].ToString());

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
        public List<MEB45_0100> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_mac_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MEB45_0100> list = new List<MEB45_0100>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = " SELECT * FROM MEB45_0100 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB45_0100 data = new MEB45_0100();

                    data.meb45_0100 = comm.sGetInt32(reader["meb45_0100"].ToString());
                    data.mac_code = comm.sGetString(reader["mac_code"].ToString());
                    data.stop_code = comm.sGetString(reader["stop_code"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";
                    
                    list.Add(data);
                }
            }
            return list;
        }
        #endregion
        public List<MEB45_0100> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<MEB45_0100> list = new List<MEB45_0100>();
            //string foreignKey = gmv.GetKey<MEB15_0000>();
            string foreignKey = gmv.GetKey<MEB45_0000>(new MEB45_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT MEB45_0100.*, MEB45_0000.stop_name , MEB15_0000.mac_name " +
                       " FROM MEB45_0100 " +
                       " left join MEB45_0000 on MEB45_0000.stop_code = MEB45_0100.stop_code " +
                       " left join MEB15_0000 on MEB15_0000.mac_code = MEB45_0100.mac_code " +
                       " where MEB45_0100. " + foreignKey + "=@" + foreignKey;
            }
            else
            {
                sSql = "SELECT * FROM MEB45_0100";
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

                    MEB45_0100 data = new MEB45_0100();

                    data.meb45_0100 = comm.sGetInt32(reader["meb45_0100"].ToString());
                    data.mac_code = comm.sGetString(reader["mac_code"].ToString());
                    data.mac_name = comm.sGetString(reader["mac_name"].ToString());
                    data.stop_code = comm.sGetString(reader["stop_code"].ToString());
                    data.stop_name = comm.sGetString(reader["stop_name"].ToString());

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
        /// 取得MEB45_0100的資料
        /// </summary>
        /// <param name="pUsrCode"></param>
        /// <param name="pPrgCode"></param>
        /// <param name="pTkCode"></param>
        /// <param name="pTkValue"></param>
        /// <returns></returns>
        public List<MEB45_0100> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode, string pTkValue)
        {
            List<MEB45_0100> list = new List<MEB45_0100>();
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT MEB45_0100.*, MEB45_0000.stop_name " +
                       " FROM MEB45_0100 " +
                       " left join MEB45_0000 on MEB45_0000.stop_code = MEB45_0100.stop_code " +
                       " where MEB45_0100. " + pTkCode + "=@" + pTkCode;
            }
            else
            {
                sSql = "SELECT * FROM MEB45_0100";
            }

            //取得該使用者可以看的資料
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter(pTkCode, pTkValue));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {

                    MEB45_0100 data = new MEB45_0100();

                    data.meb45_0100 = comm.sGetInt32(reader["meb45_0100"].ToString());
                    data.mac_code = comm.sGetString(reader["mac_code"].ToString());
                    data.stop_code = comm.sGetString(reader["stop_code"].ToString());
                    data.stop_name = comm.sGetString(reader["stop_name"].ToString());

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
        /// 傳入一個MEB45_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEB45_0100">DTO</param>
        /// 

        //取得識別碼

        public void InsertData(MEB45_0100 MEB45_0100)
        {
            string sSql = " INSERT INTO " +
                          " MEB45_0100 (  mac_code,  stop_code   ) " +
                          "     VALUES ( @mac_code, @stop_code   ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB45_0100);
            }
        }


        /// <summary>
        /// 傳入一個MEB45_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEB45_0100">DTO</param>
        public void UpdateData(MEB45_0100 MEB45_0100)
        {
            string sSql = " UPDATE MEB45_0100                   " +
                          "    SET mac_code   = @mac_code,    " +
                          "        stop_code     = @stop_code       " +
                          "  WHERE meb30_0200  = @meb30_0200    ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB45_0100);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MEB45_0100 WHERE meb30_0200 = @meb30_0200;";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { meb30_0200 = pTkCode });
            }
        }
        

    }
}