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
    public class MEB29_0200Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得MEB29_0200資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEB29_0200</returns>
        public MEB29_0200 GetDTO(string pTkCode)
        {
            MEB29_0200 datas = new MEB29_0200();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB29_0200";
            }
            else
            {
                sSql = "SELECT * FROM MEB29_0200 where meb29_0200=@meb29_0200";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@meb29_0200", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEB29_0200
                        {
                            meb29_0200 = comm.sGetInt32(reader["meb29_0200"].ToString()),
                            station_code = comm.sGetString(reader["station_code"].ToString()),
                            mac_code = comm.sGetString(reader["mac_code"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MEB29_0200資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MEB29_0200</returns>
        //public List<MEB29_0200> Get_DataList(string pTkCode)
        //{
        //    List<MEB29_0200> list = new List<MEB29_0200>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM MEB29_0200";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM MEB29_0200 where meb29_0200=@meb29_0200";
        //    }


        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@meb29_0200", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            MEB29_0200 data = new MEB29_0200();


        //            data.meb29_0200 = comm.sGetInt32(reader["meb29_0200"].ToString());
        //            data.station_code = comm.sGetString(reader["station_code"].ToString());
        //            data.mac_code = comm.sGetString(reader["mac_code"].ToString());


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
        //public List<MEB29_0200> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_meb29_0200", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<MEB29_0200> list = new List<MEB29_0200>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    sSql = "SELECT * FROM MEB29_0200";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        //sqlCommand.Parameters.Add(new SqlParameter("@meb29_0200", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            MEB29_0200 data = new MEB29_0200();

        //            data.meb29_0200 = comm.sGetInt32(reader["meb29_0200"].ToString());
        //            data.station_code = comm.sGetString(reader["station_code"].ToString());
        //            data.mac_code = comm.sGetString(reader["mac_code"].ToString());


        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            //資料邏輯刪除、修改
        //            //if (arr_LockGrpCode.Contains(data.meb29_0200)) {
        //            //    data.can_delete = "N";
        //            //    data.can_update = "N";
        //            //}

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
        public List<MEB29_0200> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MEB29_0200> list = new List<MEB29_0200>();


            string sSql = " SELECT MEB29_0200.*, MEB15_0000.mac_name " +
                          " FROM MEB29_0200 " +
                          " left join MEB15_0000 on MEB15_0000.mac_code = MEB29_0200.mac_code ";
            // 取得資料
            list = comm.Get_ListByQuery<MEB29_0200>(sSql, pWhere, pUsrCode, pPrgCode);

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
        public List<MEB29_0200> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<MEB29_0200> list = new List<MEB29_0200>();
            string foreignKey = gmv.GetKey<MEB29_0000>(new MEB29_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT *, MEB15_0000.mac_name " +
                       " FROM MEB29_0200 " +
                       " left join MEB15_0000 on MEB15_0000.mac_code = MEB29_0200.mac_code" +
                       " where MEB29_0200. " + foreignKey + "=@" + foreignKey;
            }
            else
            {
                sSql = "SELECT * FROM MEB29_0200";
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
                    MEB29_0200 data = new MEB29_0200();

                    data.meb29_0200 = comm.sGetInt32(reader["meb29_0200"].ToString());
                    data.station_code = comm.sGetString(reader["station_code"].ToString());
                    data.mac_code = comm.sGetString(reader["mac_code"].ToString());
                    data.mac_name = comm.sGetString(reader["mac_name"].ToString());

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
        /// 傳入一個MEB29_0200的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEB29_0200">DTO</param>
        public void InsertData(MEB29_0200 MEB29_0200)
        {
            string sSql = "INSERT INTO " +
                          " MEB29_0200 (  station_code,  mac_code ) " +
                          "     VALUES ( @station_code, @mac_code ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB29_0200);
            }
        }

        /// <summary>
        /// 傳入一個MEB29_0200的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEB29_0200">DTO</param>
        public void UpdateData(MEB29_0200 MEB29_0200)
        {
            string sSql = " UPDATE MEB29_0200                    " +
                          "    SET station_code = @station_code, " +
                          "        mac_code     = @mac_code      " +
                          "  WHERE meb29_0200   = @meb29_0200    " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB29_0200);
                
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MEB29_0200 WHERE meb29_0200 = @meb29_0200;";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { meb29_0200 = pTkCode });
                
            }
        }

        
    }
}