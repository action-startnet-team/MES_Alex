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
    public class MED09_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MED09_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MED09_0000</returns>
        public MED09_0000 GetDTO(string pTkCode)
        {
            MED09_0000 datas = new MED09_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MED09_0000";
            }
            else
            {
                sSql = "SELECT * FROM MED09_0000 where med09_0000=@med09_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@med09_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MED09_0000
                        {

                            med09_0000 = comm.sGetInt32(reader["med09_0000"].ToString()),
                            mo_code = comm.sGetString(reader["mo_code"].ToString()),
                            wrk_code = comm.sGetString(reader["wrk_code"].ToString()),
                            work_code = comm.sGetString(reader["work_code"].ToString()),
                            station_code = comm.sGetString(reader["station_code"].ToString()),
                            mac_code = comm.sGetString(reader["mac_code"].ToString()),
                            pro_code = comm.sGetString(reader["pro_code"].ToString()),
                            pro_lot_no = comm.sGetString(reader["pro_lot_no"].ToString()),
                            pro_qty = comm.sGetfloat(reader["pro_qty"].ToString()),
                            ins_date = comm.sGetString(reader["ins_date"].ToString()),
                            ins_time = comm.sGetString(reader["ins_time"].ToString()),
                            usr_code = comm.sGetString(reader["usr_code"].ToString()),
                            des_memo = comm.sGetString(reader["des_memo"].ToString()),
                            is_ng = comm.sGetString(reader["is_ng"].ToString()),
                            is_end = comm.sGetString(reader["is_end"].ToString()),
                            end_memo = comm.sGetString(reader["end_memo"].ToString()),
                            end_date = comm.sGetString(reader["end_date"].ToString()),
                            end_time = comm.sGetString(reader["end_time"].ToString()),
                            end_usr_code = comm.sGetString(reader["end_usr_code"].ToString()),
                            pallet_code = comm.sGetString(reader["pallet_code"].ToString()),
                            user_field_01 = comm.sGetString(reader["user_field_01"].ToString()),
                            user_field_02 = comm.sGetString(reader["user_field_02"].ToString()),
                            user_field_03 = comm.sGetString(reader["user_field_03"].ToString()),
                            user_field_04 = comm.sGetString(reader["user_field_04"].ToString()),
                            user_field_05 = comm.sGetString(reader["user_field_05"].ToString()),
                            user_field_06 = comm.sGetString(reader["user_field_06"].ToString()),
                            user_field_07 = comm.sGetString(reader["user_field_07"].ToString()),
                            user_field_08 = comm.sGetString(reader["user_field_08"].ToString()),
                            user_field_09 = comm.sGetString(reader["user_field_09"].ToString()),
                            user_field_10 = comm.sGetString(reader["user_field_10"].ToString())

                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MED09_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MED09_0000</returns>
        public List<MED09_0000> Get_DataList(string pTkCode)
        {
            List<MED09_0000> list = new List<MED09_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MED09_0000";
            }
            else
            {
                sSql = "SELECT * FROM MED09_0000 where med09_0000=@med09_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@med09_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MED09_0000 data = new MED09_0000();

                    data.med09_0000 = comm.sGetInt32(reader["med09_0000"].ToString());
                    data.mo_code = comm.sGetString(reader["mo_code"].ToString());
                    data.wrk_code = comm.sGetString(reader["wrk_code"].ToString());
                    data.mac_code = comm.sGetString(reader["mac_code"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.pro_qty = comm.sGetfloat(reader["pro_qty"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());

                    data.des_memo = comm.sGetString(reader["des_memo"].ToString());
                    data.is_ng = comm.sGetString(reader["is_ng"].ToString());
                    data.is_end = comm.sGetString(reader["is_end"].ToString());
                    data.end_memo = comm.sGetString(reader["end_memo"].ToString());
                    data.end_date = comm.sGetString(reader["end_date"].ToString());
                    data.end_time = comm.sGetString(reader["end_time"].ToString());
                    data.end_usr_code = comm.sGetString(reader["end_usr_code"].ToString());

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
        public List<MED09_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_med09_0000", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MED09_0000> list = new List<MED09_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = "SELECT * FROM MED09_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MED09_0000 data = new MED09_0000();


                    data.med09_0000 = comm.sGetInt32(reader["med09_0000"].ToString());
                    data.mo_code = comm.sGetString(reader["mo_code"].ToString());
                    data.wrk_code = comm.sGetString(reader["wrk_code"].ToString());
                    data.mac_code = comm.sGetString(reader["mac_code"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.pro_qty = comm.sGetfloat(reader["pro_qty"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());

                    data.des_memo = comm.sGetString(reader["des_memo"].ToString());
                    data.is_ng = comm.sGetString(reader["is_ng"].ToString());
                    data.is_end = comm.sGetString(reader["is_end"].ToString());
                    data.end_memo = comm.sGetString(reader["end_memo"].ToString());
                    data.end_date = comm.sGetString(reader["end_date"].ToString());
                    data.end_time = comm.sGetString(reader["end_time"].ToString());
                    data.end_usr_code = comm.sGetString(reader["end_usr_code"].ToString());


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
        public List<MED09_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MED09_0000> list = new List<MED09_0000>();

            string sSql = " SELECT MED09_0000.*, MEB15_0000.mac_name as mac_name, MEB20_0000.pro_name as pro_name, BDP08_0000.usr_name as usr_name, WMB03_0000.pallet_name, " +
                          " MEB29_0000.station_name as station_name, " +
                          " MEB30_0000.work_name as work_name " +
                          " FROM MED09_0000 " +
                          " left join MEB29_0000 on MEB29_0000.station_code = MED09_0000.station_code " +
                          " left join MEB30_0000 on MEB30_0000.work_code = MED09_0000.work_code " +
                          " left join MEB15_0000 on MEB15_0000.mac_code = MED09_0000.mac_code " +
                          " left join MEB20_0000 on MEB20_0000.pro_code = MED09_0000.pro_code " +
                          " left join BDP08_0000 on BDP08_0000.usr_code = MED09_0000.usr_code " +
                          " left join WMB03_0000 on WMB03_0000.pallet_code = MED09_0000.pallet_code " ;


            // 取得資料
            list = comm.Get_ListByQuery<MED09_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個MED09_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MED09_0000">DTO</param>
        public void InsertData(MED09_0000 MED09_0000)
        {
            string sSql = "INSERT INTO " +
                          " MED09_0000 (  mo_code,  wrk_code,  work_code,  station_code,  mac_code,  pro_code, " +
                          "               pro_lot_no,  pro_qty,  ins_date,  ins_time,  usr_code,  pallet_code ) " +

                          "     VALUES ( @mo_code, @wrk_code, @work_code, @station_code, @mac_code, @pro_code, " +
                          "              @pro_lot_no, @pro_qty, @ins_date, @ins_time, @usr_code, @pallet_code ) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MED09_0000);
            }
        }

        /// <summary>
        /// 傳入一個MED09_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MED09_0000">DTO</param>
        public void UpdateData(MED09_0000 MED09_0000)
        {
            string sSql = " UPDATE MED09_0000                     " +
                          "    SET mo_code      =  @mo_code,      " +
                          "        wrk_code     =  @wrk_code,     " +
                          "        work_code     =  @work_code,     " +
                          "        station_code =  @station_code,     " +
                          "        mac_code     =  @mac_code,     " +
                          "        pro_code     =  @pro_code,     " +
                          "        pro_lot_no   =  @pro_lot_no,   " +
                          "        pro_qty      =  @pro_qty,      " +
                          "        ins_date     =  @ins_date,     " +
                          "        ins_time     =  @ins_time,     " +
                          "        usr_code     =  @usr_code,     " +
                          "        des_memo     =  @des_memo,     " +
                          "        is_ng        =  @is_ng,        " +
                          "        is_end       =  @is_end,       " +
                          "        end_memo     =  @end_memo,     " +
                          "        end_date     =  @end_date,     " +
                          "        end_time     =  @end_time,     " +
                          "        end_usr_code =  @end_usr_code, " +
                          "        pallet_code  =  @pallet_code,   " +
                          "        user_field_01     =  @user_field_01,      " +
                          "        user_field_02     =  @user_field_02,      " +
                          "        user_field_03     =  @user_field_03,      " +
                          "        user_field_04     =  @user_field_04,      " +
                          "        user_field_05     =  @user_field_05,      " +
                          "        user_field_06     =  @user_field_06,      " +
                          "        user_field_07     =  @user_field_07,      " +
                          "        user_field_08     =  @user_field_08,      " +
                          "        user_field_09     =  @user_field_09,      " +
                          "        user_field_10     =  @user_field_10       " +
                          "  WHERE med09_0000   =  @med09_0000    ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MED09_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MED09_0000 WHERE med09_0000 = @med09_0000;";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { med09_0000 = pTkCode });
            }
        }
        
    }
}