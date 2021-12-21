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
    public class MED08_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MED08_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MED08_0000</returns>
        public MED08_0000 GetDTO(string pTkCode)
        {
            MED08_0000 data = new MED08_0000();

            string sSql = "";

            DynamicParameters sSqlParams = new DynamicParameters();

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MED08_0000";
            }
            else
            {
                sSqlParams.Add("@med08_0000", pTkCode);
                sSql = "SELECT * FROM MED08_0000 where med08_0000 = @med08_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                data = con_db.QueryFirstOrDefault<MED08_0000>(sSql, sSqlParams);
            }
            return data;
        }

        /// <summary>
        /// 取得查詢資料，結合使用者權限
        /// </summary>
        /// <param name="pUsrCode">使用者代碼</param>
        /// <param name="pPrgCode">功能代碼</param>
        /// <param name="pWhere">JSON查詢字串</param>
        /// <returns></returns>
        public List<MED08_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MED08_0000> list = new List<MED08_0000>();

            string sSql = " SELECT MED08_0000.*, MEB15_0000.mac_name   " +
                          " FROM MED08_0000 " +
                          " left join MEB15_0000 on MEB15_0000.mac_code = MED08_0000.mac_code ";
            //" left join BDP08_0000 on BDP08_0000.usr_code   = MED08_0000.end_usr_code "

            // 取得資料
            list = comm.Get_ListByQuery<MED08_0000>(sSql, pWhere, pUsrCode, pPrgCode);

            // 權限設定
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            //string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_mtp_code", "par_name", "par_value");
            //var arr_LockGrpCode = sLockGrpCode.Split(',');

            //
            for (int i = 0; i < list.Count; i++)
            {
                //檢查授權刪除、修改
                list[i].can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                list[i].can_update = sLimitStr.Contains("M") ? "Y" : "N";

                //        // 特例 轉換
                //        data.sup_name = data.sup_code + " - " + comm.sGetString(reader["sup_name"].ToString());
                //        data.sto_name = comm.sGetString(reader["sto_code"].ToString()) + " - " + comm.sGetString(reader["sto_name"].ToString());

                //        data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                //        data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                //        //資料邏輯刪除、修改
                //        //if (arr_LockGrpCode.Contains(data.mtp_code)) {
                //        //    data.can_delete = "N";
                //        //    data.can_update = "N";
                //        //}
            }

            return list;

        }

        /// <summary>
        /// 傳入一個MED08_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MED08_0000">DTO</param>
        public void InsertData(MED08_0000 MED08_0000)
        {
            string sSql = "INSERT INTO " 
                        +  " MED08_0000 (  mo_code,  wrk_code,           " 
                        + "                         date_s,  time_s,  date_e,  time_e, des_memo,       "
                        +  "                         is_ng,  is_end,  end_memo,  end_date,  end_time,  end_usr_code )   " 
                       +   "     VALUES ( @mo_code, @wrk_code,     " 
                       + "                    @date_s,  @time_s, @date_e,  @time_e,   @des_memo,       "
                       +   "                    @is_ng, @is_end, @end_memo, @end_date, @end_time, @end_usr_code )   ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MED08_0000);
               
            }
        }

        /// <summary>
        /// 傳入一個MED08_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MED08_0000">DTO</param>
        public void UpdateData(MED08_0000 MED08_0000)
        {
            string sSql = " UPDATE MED08_0000                     " 
                        + "    SET mo_code       =  @mo_code,     " 
                        + "           wrk_code      =  @wrk_code,    " 
                        + "           mac_code      =  @mac_code ,   " 
                        + "           date_s      =  @date_s ,   "
                        + "           time_s      =  @time_s ,   "
                        + "           date_e      =  @date_e ,   "
                        + "           time_e      =  @time_e ,   "
                        + "           des_memo      =  @des_memo,    " 
                        + "           is_ng         =  @is_ng,       " 
                        + "           is_end        =  @is_end,      " 
                        + "           end_memo      =  @end_memo,    " 
                        + "           end_date      =  @end_date,    " 
                        + "           end_time      =  @end_time,    " 
                        + "           end_usr_code  =  @end_usr_code, "

                          + "        user_field_01     =  @user_field_01,      " 
                          +"        user_field_02     =  @user_field_02,      " 
                          +"        user_field_03     =  @user_field_03,      " 
                          +"        user_field_04     =  @user_field_04,      " 
                          +"        user_field_05     =  @user_field_05,      " 
                          +"        user_field_06     =  @user_field_06,      " 
                          +"        user_field_07     =  @user_field_07,      " 
                          +"        user_field_08     =  @user_field_08,      " 
                          +"        user_field_09     =  @user_field_09,      " 
                          +"        user_field_10     =  @user_field_10       " 
                        + "  WHERE med08_0000    =  @med08_0000   ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MED08_0000);

            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MED08_0000 WHERE med08_0000 = @med08_0000;";
            //sSql += " Delete from BDP09_0100 where med08_0000 = @med08_0000; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { med08_0000 = pTkCode });

            }
        }

       

    }
}