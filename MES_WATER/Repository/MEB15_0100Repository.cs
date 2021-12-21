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
    public class MEB15_0100Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得MEB15_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEB15_0100</returns>
        public MEB15_0100 GetDTO(string pTkCode)
        {
            MEB15_0100 datas = new MEB15_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB15_0100";
            }
            else
            {
                sSql = "SELECT * FROM MEB15_0100 where meb15_0100=@meb15_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@meb15_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEB15_0100
                        {
                            meb15_0100 = comm.sGetInt32(reader["meb15_0100"].ToString()),
                            mac_code = comm.sGetString(reader["mac_code"].ToString()),
                            loc_code = comm.sGetString(reader["loc_code"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MEB15_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MEB15_0100</returns>
        //public List<MEB15_0100> Get_DataList(string pTkCode)
        //{
        //    List<MEB15_0100> list = new List<MEB15_0100>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM MEB15_0100";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM MEB15_0100 where meb15_0100=@meb15_0100";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@meb15_0100", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            MEB15_0100 data = new MEB15_0100();

        //            data.meb15_0100 = comm.sGetInt32(reader["meb15_0100"].ToString());
        //            data.mac_code = comm.sGetString(reader["mac_code"].ToString());
        //            data.usr_code = comm.sGetString(reader["usr_code"].ToString());
        //            data.control_type = comm.sGetString(reader["control_type"].ToString());

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
        //public List<MEB15_0100> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_meb15_0100", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<MEB15_0100> list = new List<MEB15_0100>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    sSql = " SELECT * FROM MEB15_0100 ";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        //sqlCommand.Parameters.Add(new SqlParameter("@meb15_0100", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            MEB15_0100 data = new MEB15_0100();

        //            data.meb15_0100 = comm.sGetInt32(reader["meb15_0100"].ToString());
        //            data.mac_code = comm.sGetString(reader["mac_code"].ToString());
        //            data.usr_code = comm.sGetString(reader["usr_code"].ToString());
        //            data.control_type = comm.sGetString(reader["control_type"].ToString());

        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            //資料邏輯刪除、修改
        //            //if (arr_LockGrpCode.Contains(data.meb15_0100)) {
        //            //    data.can_delete = "N";
        //            //    data.can_update = "N";
        //            //}

        //            list.Add(data);
        //        }
        //    }
        //    return list;
        //}
        #endregion

        public List<MEB15_0100> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<MEB15_0100> list = new List<MEB15_0100>();
            string foreignKey = gmv.GetKey<MEB15_0000>(new MEB15_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT MEB15_0100.*, WMB02_0000.loc_name " +
                       " FROM MEB15_0100 " +
                       " left join WMB02_0000 on MEB15_0100.loc_code = WMB02_0000.loc_code " +
                       " where MEB15_0100. " + foreignKey + "=@" + foreignKey;
            }
            else
            {
                sSql = "SELECT * FROM MEB15_0100";
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

                    MEB15_0100 data = new MEB15_0100();

                    data.meb15_0100 = comm.sGetInt32(reader["meb15_0100"].ToString());
                    data.mac_code = comm.sGetString(reader["mac_code"].ToString());
                    data.loc_code = comm.sGetString(reader["loc_code"].ToString());
                    data.loc_name = comm.sGetString(reader["loc_name"].ToString());

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
        /// 傳入一個MEB15_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEB15_0100">DTO</param>
        public void InsertData(MEB15_0100 MEB15_0100)
        {
            string sSql = "INSERT INTO " +
                          " MEB15_0100 (  mac_code,  loc_code ) " +
                          "     VALUES ( @mac_code, @loc_code ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB15_0100);
            }
        }

        /// <summary>
        /// 傳入一個MEB15_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEB15_0100">DTO</param>
        public void UpdateData(MEB15_0100 MEB15_0100)
        {
            string sSql = " UPDATE MEB15_0100                " +
                          "    SET mac_code   =  @mac_code,  " +
                          "        loc_code   =  @loc_code   " +
                          "  WHERE meb15_0100 =  @meb15_0100 ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB15_0100);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM MEB15_0100 WHERE meb15_0100 = @meb15_0100 ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { meb15_0100 = pTkCode });
            }
        }


    }
}