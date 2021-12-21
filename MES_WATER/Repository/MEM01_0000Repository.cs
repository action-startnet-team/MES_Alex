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
    public class MEM01_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MEM01_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEM01_0000</returns>
        public MEM01_0000 GetDTO(string pTkCode)
        {
            MEM01_0000 datas = new MEM01_0000();

            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEM01_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEM01_0000 where mem01_0000=@mem01_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@mem01_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEM01_0000
                        {
                            mem01_0000 = comm.sGetInt32(reader["mem01_0000"].ToString()),
                            mo_code = comm.sGetString(reader["mo_code"].ToString()),
                            work_code = comm.sGetString(reader["work_code"].ToString()),
                            station_code = comm.sGetString(reader["station_code"].ToString()),
                            mac_code = comm.sGetString(reader["mac_code"].ToString()),
                            work_time_s = comm.sGetString(reader["work_time_s"].ToString()),
                            work_time_e = comm.sGetString(reader["work_time_e"].ToString()),
                            ok_qty = comm.sGetDecimal(reader["ok_qty"].ToString()),
                            ok_unit = comm.sGetString(reader["ok_unit"].ToString()),
                            ng_qty = comm.sGetDecimal(reader["ng_qty"].ToString()),
                            ng_unit = comm.sGetString(reader["ng_unit"].ToString()),
                            work_sec = comm.sGetInt32(reader["work_sec"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MEM01_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MEM01_0000</returns>
        //  public List<MEM01_0000> Get_DataList(string pTkCode)
        //  {
        //      List<MEM01_0000> list = new List<MEM01_0000>();
        //      string sSql = "";
        //
        //      if (string.IsNullOrEmpty(pTkCode))
        //      {
        //          sSql = "SELECT * FROM MEM01_0000";
        //      }
        //      else
        //      {
        //          sSql = "SELECT * FROM MEM01_0000 where mem01_0000=@mem01_0000";
        //      }
        //
        //
        //      using (SqlConnection con_db = comm.Set_DBConnection())
        //      {
        //          SqlCommand sqlCommand = new SqlCommand(sSql);
        //          sqlCommand.Connection = con_db;
        //          sqlCommand.Parameters.Add(new SqlParameter("@mem01_0000", pTkCode));
        //          SqlDataReader reader = sqlCommand.ExecuteReader();
        //
        //          while (reader.Read())
        //          {
        //              MEM01_0000 data = new MEM01_0000();
        //
        //              data.mem01_0000 = comm.sGetInt32(reader["mem01_0000"].ToString());
        //              data.mo_code = comm.sGetString(reader["mo_code"].ToString());
        //              data.work_code = comm.sGetString(reader["work_code"].ToString());
        //              data.station_code = comm.sGetString(reader["station_code"].ToString());
        //              data.mac_code = comm.sGetString(reader["mac_code"].ToString());
        //              data.work_time_s = comm.sGetString(reader["work_time_s"].ToString());
        //              data.work_time_e = comm.sGetString(reader["work_time_e"].ToString());
        //              data.ok_qty = comm.sGetfloat(reader["ok_qty"].ToString());
        //              data.ok_unit = comm.sGetfloat(reader["ok_unit"].ToString());
        //              data.ng_qty = comm.sGetString(reader["ng_qty"].ToString());
        //
        //              data.can_delete = "Y";
        //              data.can_update = "Y";
        //              list.Add(data);
        //          }
        //
        //      }
        //      return list;
        //  }

        /// <summary>
        /// 取得使用者可以編輯的資料，結合商務邏輯權限
        /// </summary>
        /// <param name="pUsrCode"></param>
        /// <param name="pPrgCode"></param>
        /// <returns></returns>
        //public List<MEM01_0000> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_mem01_0000", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<MEM01_0000> list = new List<MEM01_0000>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    //sSql = "SELECT * FROM MEM01_0000";
        //    sSql = "SELECT * FROM MEM01_0000";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        //sqlCommand.Parameters.Add(new SqlParameter("@mem01_0000", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            MEM01_0000 data = new MEM01_0000();

        //            data.mem01_0000 = comm.sGetInt32(reader["mem01_0000"].ToString());
        //            data.mo_code = comm.sGetString(reader["mo_code"].ToString());
        //            data.work_code = comm.sGetString(reader["work_code"].ToString());
        //            data.station_code = comm.sGetString(reader["station_code"].ToString());
        //            data.mac_code = comm.sGetString(reader["mac_code"].ToString());


        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            //資料邏輯刪除、修改
        //            //if (arr_LockGrpCode.Contains(data.mem01_0000)) {
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
        public List<MEM01_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MEM01_0000> list = new List<MEM01_0000>();
            
            string sSql = " SELECT * FROM MEM01_0000 ";

            // 取得資料
            list = comm.Get_ListByQuery<MEM01_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個MEM01_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEM01_0000">DTO</param>
        public void InsertData(MEM01_0000 MEM01_0000)
        {
            string sSql = "INSERT INTO " +
                          " MEM01_0000 (   mo_code,  work_code,  station_code,  mac_code,  work_time_s, "  +
                          "                work_time_e,  ok_qty,  ok_unit,  ng_qty,  ng_unit,  work_sec ) " +

                          "     VALUES (  @mo_code, @work_code, @station_code, @mac_code, @work_time_s, " +
                          "               @work_time_e, @ok_qty, @ok_unit, @ng_qty, @ng_unit, @work_sec ) " ;

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEM01_0000);
            }
        }

        /// <summary>
        /// 傳入一個MEM01_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEM01_0000">DTO</param>
        public void UpdateData(MEM01_0000 MEM01_0000)
        {
            string sSql = " UPDATE MEM01_0000                     " +
                          "    SET mo_code      = @mo_code,       " +
                          "        work_code    = @work_code,     " +
                          "        station_code = @station_code,  " +
                          "        mac_code     = @mac_code,      " +
                          "        work_time_s  = @work_time_s,   " +
                          "        work_time_e  = @work_time_e,   " +
                          "        ok_qty       = @ok_qty,        " +
                          "        ok_unit      = @ok_unit,       " +
                          "        ng_qty       = @ng_qty,        " +
                          "        ng_unit      = @ng_unit,       " +
                          "        work_sec     = @work_sec       " +
                          "  WHERE mem01_0000   = @mem01_0000     " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEM01_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MEM01_0000 WHERE mem01_0000 = @mem01_0000 ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { mem01_0000 = pTkCode });
            }
        }

    }
}