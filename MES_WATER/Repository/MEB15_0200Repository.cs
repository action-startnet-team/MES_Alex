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
    public class MEB15_0200Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得MEB15_0200資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEB15_0200</returns>
        public MEB15_0200 GetDTO(string pTkCode)
        {
            MEB15_0200 datas = new MEB15_0200();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB15_0200";
            }
            else
            {
                sSql = "SELECT * FROM MEB15_0200 where meb15_0200=@meb15_0200";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@meb15_0200", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEB15_0200
                        {
                            meb15_0200 = comm.sGetInt32(reader["meb15_0200"].ToString()),
                            mac_code = comm.sGetString(reader["mac_code"].ToString()),
                            dev_code = comm.sGetString(reader["dev_code"].ToString()),
                            des_memo = comm.sGetString(reader["des_memo"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MEB15_0200資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MEB15_0200</returns>
        //public List<MEB15_0200> Get_DataList(string pTkCode)
        //{
        //    List<MEB15_0200> list = new List<MEB15_0200>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM MEB15_0200";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM MEB15_0200 where meb15_0200=@meb15_0200";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@meb15_0200", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            MEB15_0200 data = new MEB15_0200();

        //            data.meb15_0200 = comm.sGetInt32(reader["meb15_0200"].ToString());
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
        //public List<MEB15_0200> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_meb15_0200", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<MEB15_0200> list = new List<MEB15_0200>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    sSql = " SELECT * FROM MEB15_0200 ";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        //sqlCommand.Parameters.Add(new SqlParameter("@meb15_0200", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            MEB15_0200 data = new MEB15_0200();

        //            data.meb15_0200 = comm.sGetInt32(reader["meb15_0200"].ToString());
        //            data.mac_code = comm.sGetString(reader["mac_code"].ToString());
        //            data.usr_code = comm.sGetString(reader["usr_code"].ToString());
        //            data.control_type = comm.sGetString(reader["control_type"].ToString());

        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            //資料邏輯刪除、修改
        //            //if (arr_LockGrpCode.Contains(data.meb15_0200)) {
        //            //    data.can_delete = "N";
        //            //    data.can_update = "N";
        //            //}

        //            list.Add(data);
        //        }
        //    }
        //    return list;
        //}
        #endregion

        public List<MEB15_0200> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<MEB15_0200> list = new List<MEB15_0200>();
            string foreignKey = gmv.GetKey<MEB15_0000>(new MEB15_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT MEB15_0200.*, MEB49_0000.dev_name " +
                       " FROM MEB15_0200 " +
                       " left join MEB49_0000 on MEB49_0000.dev_code = MEB15_0200.dev_code " +
                       " where MEB15_0200. " + foreignKey + "=@" + foreignKey;
            }
            else
            {
                sSql = "SELECT * FROM MEB15_0200";
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

                    MEB15_0200 data = new MEB15_0200();

                    data.meb15_0200 = comm.sGetInt32(reader["meb15_0200"].ToString());
                    data.mac_code = comm.sGetString(reader["mac_code"].ToString());
                    data.dev_code = comm.sGetString(reader["dev_code"].ToString());
                    data.dev_name = comm.sGetString(reader["dev_name"].ToString());
                    data.des_memo = comm.sGetString(reader["des_memo"].ToString());

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
        /// 傳入一個MEB15_0200的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEB15_0200">DTO</param>
        public void InsertData(MEB15_0200 MEB15_0200)
        {
            string sSql = "INSERT INTO " +
                          " MEB15_0200 (  mac_code,  dev_code,  des_memo ) " +
                          "     VALUES ( @mac_code, @dev_code, @des_memo ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB15_0200);
            }
        }

        /// <summary>
        /// 傳入一個MEB15_0200的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEB15_0200">DTO</param>
        public void UpdateData(MEB15_0200 MEB15_0200)
        {
            string sSql = " UPDATE MEB15_0200                " +
                          "    SET mac_code   =  @mac_code,  " +
                          "        dev_code   =  @dev_code,  " +
                          "        des_memo   =  @des_memo   " +
                          "  WHERE meb15_0200 =  @meb15_0200 ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB15_0200);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM MEB15_0200 WHERE meb15_0200 = @meb15_0200 ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { meb15_0200 = pTkCode });
            }
        }


    }
}