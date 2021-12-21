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
    public class MEP05_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MEP05_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEP05_0000</returns>
        public MEP05_0000 GetDTO(string pTkCode)
        {
            MEP05_0000 datas = new MEP05_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEP05_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEP05_0000 where mep05_0000=@mep05_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@mep05_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEP05_0000
                        {
                            mep05_0000 = comm.sGetString(reader["mep05_0000"].ToString()),
                            mo_code = comm.sGetString(reader["mo_code"].ToString()),
                            wrk_code = comm.sGetString(reader["wrk_code"].ToString()),
                            work_code = comm.sGetString(reader["work_code"].ToString()),
                            station_code = comm.sGetString(reader["station_code"].ToString()),
                            mac_code = comm.sGetString(reader["mac_code"].ToString()),
                            work_second = comm.sGetDecimal(reader["work_second"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }


        #region
        ///// <summary>
        ///// 取得MEP05_0000資料表內容
        ///// </summary>
        ///// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        ///// < returns > List MEP05_0000</returns>
        //public List<MEP05_0000> Get_DataList(string pTkCode)
        //{
        //    List<MEP05_0000> list = new List<MEP05_0000>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM MEP05_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM MEP05_0000 where mep05_0000=@mep05_0000";
        //    }


        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@mep05_0000", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            MEP05_0000 data = new MEP05_0000();

        //            data.mo_code = comm.sGetString(reader["mo_code"].ToString());
        //            data.wrk_code = comm.sGetString(reader["wrk_code"].ToString());
        //            data.work_code = comm.sGetString(reader["work_code"].ToString());
        //            data.station_code = comm.sGetString(reader["station_code"].ToString());
        //            data.mac_code = comm.sGetString(reader["mac_code"].ToString());
        //            data.work_second = comm.sGetDecimal(reader["work_second"].ToString());
        //            data.date = comm.sGetString(reader["date"].ToString());

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
        //public List<MEP05_0000> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_sup_code", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<MEP05_0000> list = new List<MEP05_0000>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    sSql = "SELECT * FROM MEP05_0000";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            MEP05_0000 data = new MEP05_0000();


        //            data.mo_code = comm.sGetString(reader["mo_code"].ToString());
        //            data.wrk_code = comm.sGetString(reader["wrk_code"].ToString());
        //            data.work_code = comm.sGetString(reader["work_code"].ToString());
        //            data.station_code = comm.sGetString(reader["station_code"].ToString());
        //            data.mac_code = comm.sGetString(reader["mac_code"].ToString());
        //            data.work_second = comm.sGetDecimal(reader["work_second"].ToString());
        //            data.date = comm.sGetString(reader["date"].ToString());
        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

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
        /// 
        public List<MEP05_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MEP05_0000> list = new List<MEP05_0000>();

            string sSql = " SELECT MEP05_0000.*, MEB30_0000.work_name, MEB29_0000.station_name, MEB15_0000.mac_name " +
                          " FROM MEP05_0000 " +
                          " LEFT JOIN MEB30_0000 on MEB30_0000.work_code = MEP05_0000.work_code " +
                          " LEFT JOIN MEB29_0000 on MEB29_0000.station_code = MEP05_0000.station_code " +
                          " LEFT JOIN MEB15_0000 on MEB15_0000.mac_code = MEP05_0000.mac_code " ;

            // 取得資料
            list = comm.Get_ListByQuery<MEP05_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個MEP05_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEP05_0000">DTO</param>



        public void InsertData(MEP05_0000 MEP05_0000)
        {
           
            string sSql = "INSERT INTO " +
                          " MEP05_0000 (  mo_code,  wrk_code,  work_code,  station_code,  mac_code,  work_second ) " +
                          "     VALUES ( @mo_code, @wrk_code, @work_code, @station_code, @mac_code, @work_second ) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEP05_0000);
            }
        }

        /// <summary>
        /// 傳入一個MEP05_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEP05_0000">DTO</param>
        public void UpdateData(MEP05_0000 MEP05_0000)
        {
            string sSql = " UPDATE MEP05_0000                    " +
                          "    SET mo_code      = @mo_code,      " +
                          "        wrk_code     = @wrk_code,     " +
                          "        work_code    = @work_code,    " +
                          "        station_code = @station_code, " +
                          "        mac_code     = @mac_code,     " +
                          "        work_second  = @work_second   " +
                          "  WHERE mep05_0000   = @mep05_0000    " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {

                con_db.Execute(sSql, MEP05_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM MEP05_0000 WHERE mep05_0000 = @mep05_0000 " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { mep05_0000 = pTkCode });
            }
        }

    }
}