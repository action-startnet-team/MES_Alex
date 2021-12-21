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
    public class MEB20_0100Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得MEB20_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEB20_0100</returns>
        public MEB20_0100 GetDTO(string pTkCode)
        {
            MEB20_0100 datas = new MEB20_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB20_0100";
            }
            else
            {
                sSql = "SELECT * FROM MEB20_0100 where meb20_0100=@meb20_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@meb20_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEB20_0100
                        {
                            
                            meb20_0100 = comm.sGetInt32(reader["meb20_0100"].ToString()),
                            pro_code = comm.sGetString(reader["pro_code"].ToString()),
                            std_qty = comm.sGetInt32(reader["std_qty"].ToString()),
                            std_time = comm.sGetString(reader["std_time"].ToString()),
                            pro_unit = comm.sGetString(reader["pro_unit"].ToString()),
                            station_code = comm.sGetString(reader["station_code"].ToString()),
                            work_code = comm.sGetString(reader["work_code"].ToString()),

                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MEB20_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MEB20_0100</returns>
        public List<MEB20_0100> Get_DataList(string pTkCode)
        {
            List<MEB20_0100> list = new List<MEB20_0100>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB20_0100";
            }
            else
            {
                sSql = "SELECT * FROM MEB20_0100 where meb20_0100=@meb20_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@meb20_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB20_0100 data = new MEB20_0100();
                   
                    data.meb20_0100 = comm.sGetInt32(reader["meb20_0100"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.std_qty = comm.sGetInt32(reader["std_qty"].ToString());
                    data.std_time = comm.sGetString(reader["std_time"].ToString());
                    data.pro_unit = comm.sGetString(reader["pro_unit"].ToString());
                    data.station_code = comm.sGetString(reader["station_code"].ToString());
                    data.work_code = comm.sGetString(reader["work_code"].ToString());
                    
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
        public List<MEB20_0100> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_meb20_0100", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MEB20_0100> list = new List<MEB20_0100>();
            string sSql = "";

            //取得該使用者可以看的資料

            sSql = "SELECT * FROM MEB20_0100";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB20_0100 data = new MEB20_0100();

                    data.meb20_0100 = comm.sGetInt32(reader["meb20_0100"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.std_qty = comm.sGetInt32(reader["std_qty"].ToString());
                    data.std_time = comm.sGetString(reader["std_time"].ToString());
                    data.pro_unit = comm.sGetString(reader["pro_unit"].ToString());
                    data.station_code = comm.sGetString(reader["station_code"].ToString());
                    data.work_code = comm.sGetString(reader["work_code"].ToString());
                    
                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";
                    
                    list.Add(data);
                }
            }
            return list;
        }
        #endregion

        /// <summary>
        /// 取得查詢資料，結合使用者權限
        /// </summary>
        /// <param name="pUsrCode">使用者代碼</param>
        /// <param name="pPrgCode">功能代碼</param>
        /// <param name="pWhere">JSON查詢字串</param>
        /// <returns></returns>
        public List<MEB20_0100> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<MEB20_0100> list = new List<MEB20_0100>();
            string foreignKey = gmv.GetKey<MEB20_0000>(new MEB20_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT MEB20_0100.*, MEB20_0000.pro_name , MEB29_0000.station_name, MEB30_0000.work_name " +
                       " FROM MEB20_0100 " +
                       " left join MEB20_0000 on MEB20_0000.pro_code = MEB20_0100.pro_code " +
                       " left join MEB29_0000 on MEB29_0000.station_code = MEB20_0100.station_code " +
                       " left join MEB30_0000 on MEB30_0000.work_code = MEB20_0100.work_code " +
                       " where MEB20_0100. " + foreignKey + "=@" + foreignKey +
                       " order by meb20_0100";
            }
            else
            {
                sSql = "SELECT * FROM MEB20_0100";
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

                    MEB20_0100 data = new MEB20_0100();

                    data.meb20_0100 = comm.sGetInt32(reader["meb20_0100"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.pro_name = comm.sGetString(reader["pro_name"].ToString());
                    data.std_qty = comm.sGetInt32(reader["std_qty"].ToString());
                    data.std_time = comm.sGetString(reader["std_time"].ToString());
                    data.pro_unit = comm.sGetString(reader["pro_unit"].ToString());
                    data.station_code = comm.sGetString(reader["station_code"].ToString());
                    data.station_name = comm.sGetString(reader["station_name"].ToString());
                    data.work_code = comm.sGetString(reader["work_code"].ToString());
                    data.work_name = comm.sGetString(reader["work_name"].ToString());


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
        /// 傳入一個MEB20_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEB20_0100">DTO</param>
        public void InsertData(MEB20_0100 MEB20_0100)
        {
            string sSql = "INSERT INTO " +
                          " MEB20_0100 (  pro_code,  std_qty,  std_time,  pro_unit,  station_code,  work_code  ) " +
                          "     VALUES ( @pro_code, @std_qty, @std_time, @pro_unit, @station_code, @work_code  ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB20_0100);
            }
        }

        /// <summary>
        /// 傳入一個MEB20_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEB20_0100">DTO</param>
        public void UpdateData(MEB20_0100 MEB20_0100)
        {
            string sSql = " UPDATE MEB20_0100                     " +
                          "    SET pro_code     =  @pro_code,     " +
                          "        std_qty      =  @std_qty,      " +
                          "        std_time     =  @std_time,     " +
                          "        pro_unit     =  @pro_unit,     " +
                          "        station_code =  @station_code, " +
                          "        work_code    =  @work_code     " +
                          "  WHERE meb20_0100   =  @meb20_0100    " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB20_0100);
                
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MEB20_0100 WHERE meb20_0100 = @meb20_0100; " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { meb20_0100 = pTkCode });
                
            }
        }

        public void DeleteByProCode(string pProCode)
        {
            string sSql = "DELETE FROM MEB20_0100 WHERE pro_code = @pro_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { pro_code = pProCode });

            }
        }

    }
}