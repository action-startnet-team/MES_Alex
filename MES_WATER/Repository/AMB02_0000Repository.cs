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
    public class AMB02_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得AMB02_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO AMB02_0000</returns>
        public AMB02_0000 GetDTO(string pTkCode)
        {
            AMB02_0000 datas = new AMB02_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM AMB02_0000";
            }
            else
            {
                sSql = "SELECT * FROM AMB02_0000 where amb02_0000=@amb02_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@amb02_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new AMB02_0000
                        {
                            amb02_0000 = comm.sGetInt32(reader["amb02_0000"].ToString()),
                            alm_code = comm.sGetString(reader["alm_code"].ToString()),
                            usr_code = comm.sGetString(reader["usr_code"].ToString()),
                            mac_code = comm.sGetString(reader["mac_code"].ToString()),
                            is_use = comm.sGetString(reader["is_use"].ToString()),
                            time_start = comm.sGetString(reader["time_start"].ToString()),
                            time_end = comm.sGetString(reader["time_end"].ToString()),
                            alm_message = comm.sGetString(reader["alm_message"].ToString())
                        };
                    }
                }
            }
            return datas;
        }

        //#region
        ///// <summary>
        ///// 取得AMB02_0000資料表內容
        ///// </summary>
        ///// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        ///// < returns > List AMB02_0000</returns>
        //public List<AMB02_0000> Get_DataList(string pTkCode)
        //{
        //    List<AMB02_0000> list = new List<AMB02_0000>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM AMB02_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM AMB02_0000 where amb02_0000=@amb02_0000";
        //    }


        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@amb02_0000", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            AMB02_0000 data = new AMB02_0000();

        //            data.amb02_0000 = comm.sGetString(reader["amb02_0000"].ToString());
        //            data.alm_code = comm.sGetString(reader["alm_code"].ToString());
        //            data.usr_code = comm.sGetString(reader["usr_code"].ToString());
        //            data.is_use = comm.sGetString(reader["is_use"].ToString());
        //            data.time_start = comm.sGetString(reader["time_start"].ToString());
        //            data.time_end = comm.sGetString(reader["time_end"].ToString());
        //            data.alm_message = comm.sGetString(reader["alm_message"].ToString());

        //            data.can_delete = "Y";
        //            data.can_update = "Y";
        //            list.Add(data);
        //        }

        //    }
        //    return list;
        //}

        ///// <summary>
        ///// 取得使用者可以編輯的資料，結合商務邏輯權限
        ///// </summary>
        ///// <param name="pUsrCode"></param>
        ///// <param name="pPrgCode"></param>
        ///// <returns></returns>
        //public List<AMB02_0000> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_amb02_0000", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<AMB02_0000> list = new List<AMB02_0000>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    //sSql = "SELECT * FROM AMB02_0000";
        //    sSql = "SELECT * FROM AMB02_0000";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        //sqlCommand.Parameters.Add(new SqlParameter("@amb02_0000", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            AMB02_0000 data = new AMB02_0000();

        //            data.amb02_0000 = comm.sGetString(reader["amb02_0000"].ToString());
        //            data.alm_code = comm.sGetString(reader["alm_code"].ToString());
        //            data.usr_code = comm.sGetString(reader["usr_code"].ToString());
        //            data.is_use = comm.sGetString(reader["is_use"].ToString());
        //            data.time_start = comm.sGetString(reader["time_start"].ToString());


        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            //資料邏輯刪除、修改
        //            //if (arr_LockGrpCode.Contains(data.amb02_0000)) {
        //            //    data.can_delete = "N";
        //            //    data.can_update = "N";
        //            //}

        //            list.Add(data);
        //        }
        //    }
        //    return list;
        //}
        //#endregion

        /// <summary>
        /// 取得查詢資料，結合使用者權限
        /// </summary>
        /// <param name="pUsrCode">使用者代碼</param>
        /// <param name="pPrgCode">功能代碼</param>
        /// <param name="pWhere">JSON查詢字串</param>
        /// <returns></returns>
        public List<AMB02_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<AMB02_0000> list = new List<AMB02_0000>();
            
            string sSql = " SELECT distinct AMB02_0000.* , BDP08_0000.usr_name, MEB15_0000.mac_name " +
                          " FROM AMB02_0000 " +
                          " left join BDP08_0000 on BDP08_0000.usr_code = AMB02_0000.usr_code " +
                          " left join MEB15_0000 on MEB15_0000.mac_code = AMB02_0000.mac_code " ;

            // 取得資料
            list = comm.Get_ListByQuery<AMB02_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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

        /// <summary>
        /// 傳入一個AMB02_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="AMB02_0000">DTO</param>
        public void InsertData(AMB02_0000 AMB02_0000)
        {
            string sSql = "INSERT INTO " +
                          " AMB02_0000 (  alm_code,  usr_code,  mac_code,  is_use,  time_start,  time_end,  alm_message ) " +
                          "     VALUES ( @alm_code, @usr_code, @mac_code, @is_use, @time_start, @time_end, @alm_message ) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, AMB02_0000);
            }
        }

        /// <summary>
        /// 傳入一個AMB02_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="AMB02_0000">DTO</param>
        public void UpdateData(AMB02_0000 AMB02_0000)
        {
            string sSql = " UPDATE AMB02_0000                 " +
                          "    SET alm_code    = @alm_code,   " +
                          "        usr_code    = @usr_code,   " +
                          "        mac_code    = @mac_code,   " +
                          "        is_use      = @is_use,     " +
                          "        time_start  = @time_start, " +
                          "        time_end    = @time_end,   " +
                          "        alm_message = @alm_message " +
                          "  WHERE amb02_0000  = @amb02_0000  " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, AMB02_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM AMB02_0000 WHERE amb02_0000 = @amb02_0000;";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { amb02_0000 = pTkCode });
            }
        }

    }
}