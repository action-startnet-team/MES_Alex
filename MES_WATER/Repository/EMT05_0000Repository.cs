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
    public class EMT05_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得EMT05_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO EMT05_0000</returns>
        public EMT05_0000 GetDTO(string pTkCode)
        {
            EMT05_0000 datas = new EMT05_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMT05_0000";
            }
            else
            {
                sSql = "SELECT * FROM EMT05_0000 where call_code=@call_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@call_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new EMT05_0000
                        {

                            call_code = comm.sGetString(reader["call_code"].ToString()),
                            call_date = comm.sGetString(reader["call_date"].ToString()),
                            dev_code = comm.sGetString(reader["dev_code"].ToString()),
                            dev_check_code = comm.sGetString(reader["dev_check_code"].ToString()),
                            chk_item_code = comm.sGetString(reader["chk_item_code"].ToString()),
                            call_memo = comm.sGetString(reader["call_memo"].ToString()),         
                            ins_date = comm.sGetString(reader["ins_date"].ToString()),
                            ins_time = comm.sGetString(reader["ins_time"].ToString()),
                            usr_code = comm.sGetString(reader["usr_code"].ToString()),
                            emt01_0100 = comm.sGetString(reader["emt01_0100"].ToString()),


                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得EMT05_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List EMT05_0000</returns>
        public List<EMT05_0000> Get_DataList(string pTkCode)
        {
            List<EMT05_0000> list = new List<EMT05_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMT05_0000";
            }
            else
            {
                sSql = "SELECT * FROM EMT05_0000 where call_code=@call_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@call_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EMT05_0000 data = new EMT05_0000();

                    data.call_code = comm.sGetString(reader["call_code"].ToString());
                    data.call_date = comm.sGetString(reader["call_date"].ToString());
                    data.dev_code = comm.sGetString(reader["dev_code"].ToString());
                    data.chk_item_code = comm.sGetString(reader["chk_item_code"].ToString());
                    data.call_memo = comm.sGetString(reader["call_memo"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());


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
        public List<EMT05_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_call_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<EMT05_0000> list = new List<EMT05_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM EMT05_0000";
            sSql = "SELECT * FROM EMT05_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@call_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EMT05_0000 data = new EMT05_0000();


                    data.call_code = comm.sGetString(reader["call_code"].ToString());
                    data.call_date = comm.sGetString(reader["call_date"].ToString());
                    data.dev_code = comm.sGetString(reader["dev_code"].ToString());
                    data.chk_item_code = comm.sGetString(reader["chk_item_code"].ToString());
                    data.call_memo = comm.sGetString(reader["call_memo"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.call_code)) {
                    //    data.can_delete = "N";
                    //    data.can_update = "N";
                    //}

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
        public List<EMT05_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<EMT05_0000> list = new List<EMT05_0000>();

            string sSql = " SELECT EMT05_0000.*, dev_name, chk_item_name, usr_name,EMT01_0000.maintain_code,maintain_name,EMT01_0100.main_item_code,main_item_name,EMT01_0100.ins_date as maintain_date" +
                          " FROM EMT05_0000 " +
                          " left join EMB07_0000 on EMB07_0000.dev_code = EMT05_0000.dev_code " +
                          " left join BDP08_0000 on BDP08_0000.usr_code = EMT05_0000.usr_code " +
                          " left join EMB21_0000 on EMB21_0000.chk_item_code = EMT05_0000.chk_item_code " +
                          " left join EMT01_0100 on EMT05_0000.emt01_0100 = EMT01_0100.emt01_0100" +
                          " left join EMT01_0000 on EMT01_0100.maintain_code = EMT01_0000.maintain_code" +
                          " left join EMB08_0000 on EMT01_0100.main_item_code = EMB08_0000.main_item_code";

            // 取得資料
            list = comm.Get_ListByQuery<EMT05_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個EMT05_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="EMT05_0000">DTO</param>
        public void InsertData(EMT05_0000 EMT05_0000)
        {
            string sSql = "INSERT INTO " +
                          " EMT05_0000 (  call_code,  call_date,  dev_code,  dev_check_code,  chk_item_code, " +
                          "               call_memo,  ins_date,  ins_time,  usr_code ) " +

                          "     VALUES ( @call_code, @call_date, @dev_code, @dev_check_code, @chk_item_code, " +
                          "              @call_memo, @ins_date, @ins_time, @usr_code ) ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMT05_0000);
            }
        }

        /// <summary>
        /// 傳入一個EMT05_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="EMT05_0000">DTO</param>
        public void UpdateData(EMT05_0000 EMT05_0000)
        {
            string sSql = " UPDATE EMT05_0000                         " +
                          "    SET call_date      =  @call_date,      " +
                          "        dev_code       =  @dev_code,       " +
                          "        dev_check_code =  @dev_check_code, " +
                          "        chk_item_code  =  @chk_item_code,  " +
                          "        call_memo      =  @call_memo,      " +
                          "        ins_date       =  @ins_date,       " +
                          "        ins_time       =  @ins_time,       " +
                          "        usr_code       =  @usr_code        " +  
                          "  WHERE call_code      =  @call_code       " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMT05_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM EMT05_0000 WHERE call_code = @call_code ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { call_code = pTkCode });
            }
        }


    }
}