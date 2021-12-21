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
    public class MEB29_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MEB29_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEB29_0000</returns>
        public MEB29_0000 GetDTO(string pTkCode)
        {
            MEB29_0000 datas = new MEB29_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB29_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEB29_0000 where station_code=@station_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@station_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEB29_0000
                        {
                            station_code = comm.sGetString(reader["station_code"].ToString()),
                            station_name = comm.sGetString(reader["station_name"].ToString()),
                            station_type_code = comm.sGetString(reader["station_type_code"].ToString()),
                            cmemo = comm.sGetString(reader["cmemo"].ToString()),
                            is_sto_in = comm.sGetString(reader["is_sto_in"].ToString()),
                            is_sto_out = comm.sGetString(reader["is_sto_out"].ToString()),
                            is_check_per = comm.sGetString(reader["is_check_per"].ToString()),
                            loc_code = comm.sGetString(reader["loc_code"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        /// <summary>
        /// 取得MEB29_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MEB29_0000</returns>
        public List<MEB29_0000> Get_DataList(string pTkCode)
        {
            List<MEB29_0000> list = new List<MEB29_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB29_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEB29_0000 where station_code=@station_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@station_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB29_0000 data = new MEB29_0000();

                    data.station_code = comm.sGetString(reader["station_code"].ToString());
                    data.station_name = comm.sGetString(reader["station_name"].ToString());
                    data.station_type_code = comm.sGetString(reader["station_type_code"].ToString());
                    data.cmemo = comm.sGetString(reader["cmemo"].ToString());
                    data.is_sto_in = comm.sGetString(reader["is_sto_in"].ToString());
                    data.is_sto_out = comm.sGetString(reader["is_sto_out"].ToString());
                    data.is_check_per = comm.sGetString(reader["is_check_per"].ToString());
                    data.loc_code = comm.sGetString(reader["loc_code"].ToString());

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
        public List<MEB29_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_station_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MEB29_0000> list = new List<MEB29_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = " SELECT * FROM MEB29_0000 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB29_0000 data = new MEB29_0000();
                    
                    data.station_code = comm.sGetString(reader["station_code"].ToString());
                    data.station_name = comm.sGetString(reader["station_name"].ToString());
                    data.station_type_code = comm.sGetString(reader["station_type_code"].ToString());
                    data.cmemo = comm.sGetString(reader["cmemo"].ToString());
                    data.is_sto_in = comm.sGetString(reader["is_sto_in"].ToString());
                    data.is_sto_out = comm.sGetString(reader["is_sto_out"].ToString());
                    data.is_check_per = comm.sGetString(reader["is_check_per"].ToString());
                    data.loc_code = comm.sGetString(reader["loc_code"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.station_code)) {
                    //    data.can_delete = "N";
                    //    data.can_update = "N";
                    //}

                    list.Add(data);
                }
            }
            return list;
        }

        /// <summary>
        /// 取得查詢資料，結合使用者權限
        /// </summary>
        /// <param name="pUsrCode">使用者代碼</param>
        /// <param name="pPrgCode">功能代碼</param>
        /// <param name="pWhere">JSON查詢字串</param>
        /// <returns></returns>
        public List<MEB29_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MEB29_0000> list = new List<MEB29_0000>();

            string sSql = " SELECT distinct MEB29_0000.station_code, MEB29_0000.* " +
                          " FROM MEB29_0000 " +
                          @"left join(
                                select MEB29_0200.*, MEB15_0000.mac_name
                                from MEB29_0200
                                left join MEB15_0000 on MEB29_0200.mac_code = MEB15_0000.mac_code
                            ) as s on s.station_code = MEB29_0000.station_code " +
                          " left join MEB29_0100 on MEB29_0100.station_code = MEB29_0000.station_code " +
                          " left join BDP21_0100 on BDP21_0100.field_code = MEB29_0100.control_type and BDP21_0100.code_code = 'control_type' " +
                          " left join BDP08_0000 on BDP08_0000.usr_code = MEB29_0100.usr_code ";

            // 取得資料
            list = comm.Get_ListByQuery<MEB29_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個MEB29_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEB29_0000">DTO</param>
        public void InsertData(MEB29_0000 MEB29_0000)
        {
            string sSql = " INSERT INTO " +
                          " MEB29_0000 (  station_code,  station_name,  station_type_code,  cmemo ) " +
                          "     VALUES ( @station_code, @station_name, @station_type_code, @cmemo ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB29_0000);
            }
        }

        /// <summary>
        /// 傳入一個MEB29_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEB29_0000">DTO</param>
        public void UpdateData(MEB29_0000 MEB29_0000)
        {
            string sSql = " UPDATE MEB29_0000                               " +
                          "    SET station_name      =  @station_name,      " +
                          "        station_type_code =  @station_type_code, " +
                          "        cmemo             =  @cmemo              " +
                          "  WHERE station_code      =  @station_code       " ;

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB29_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM MEB29_0000 WHERE station_code = @station_code " +
                          " DELETE FROM MEB29_0100 WHERE station_code = @station_code " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { station_code = pTkCode });
            }
        }


    }
}