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
    public class EMT06_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得EMT06_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO EMT06_0000</returns>
        public EMT06_0000 GetDTO(string pTkCode)
        {
            EMT06_0000 datas = new EMT06_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT * FROM EMT06_0000 ";
            }
            else
            {
                sSql = " SELECT * FROM EMT06_0000 where rep_code=@rep_code ";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@rep_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new EMT06_0000
                        {

                            rep_code = comm.sGetString(reader["rep_code"].ToString()),
                            rep_date = comm.sGetString(reader["rep_date"].ToString()),
                            call_code = comm.sGetString(reader["call_code"].ToString()),
                            dev_code = comm.sGetString(reader["dev_code"].ToString()),
                            fault_code = comm.sGetString(reader["fault_code"].ToString()),
                            per_code = comm.sGetString(reader["per_code"].ToString()),
                            per_tel = comm.sGetString(reader["per_tel"].ToString()),
                            per_mail = comm.sGetString(reader["per_mail"].ToString()),
                            fault_handle_code = comm.sGetString(reader["fault_handle_code"].ToString()),
                            rep_memo = comm.sGetString(reader["rep_memo"].ToString()),
                            ins_date = comm.sGetString(reader["ins_date"].ToString()),
                            ins_time = comm.sGetString(reader["ins_time"].ToString()),
                            usr_code = comm.sGetString(reader["usr_code"].ToString()),
                            
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得EMT06_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List EMT06_0000</returns>
        public List<EMT06_0000> Get_DataList(string pTkCode)
        {
            List<EMT06_0000> list = new List<EMT06_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMT06_0000";
            }
            else
            {
                sSql = "SELECT * FROM EMT06_0000 where rep_code=@rep_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@rep_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EMT06_0000 data = new EMT06_0000();

                    data.rep_code = comm.sGetString(reader["rep_code"].ToString());
                    data.rep_date = comm.sGetString(reader["rep_date"].ToString());
                    data.call_code = comm.sGetString(reader["call_code"].ToString());
                    data.dev_code = comm.sGetString(reader["dev_code"].ToString());
                    data.fault_code = comm.sGetString(reader["fault_code"].ToString());
                    data.per_code = comm.sGetString(reader["per_code"].ToString());
                    data.per_tel = comm.sGetString(reader["per_tel"].ToString());
                    data.per_mail = comm.sGetString(reader["per_mail"].ToString());
                    data.fault_handle_code = comm.sGetString(reader["fault_handle_code"].ToString());
                    data.rep_memo = comm.sGetString(reader["rep_memo"].ToString());
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
        public List<EMT06_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_rep_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<EMT06_0000> list = new List<EMT06_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM EMT06_0000";
            sSql = "SELECT * FROM EMT06_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@rep_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EMT06_0000 data = new EMT06_0000();

                    data.rep_code = comm.sGetString(reader["rep_code"].ToString());
                    data.rep_date = comm.sGetString(reader["rep_date"].ToString());
                    data.call_code = comm.sGetString(reader["call_code"].ToString());
                    data.dev_code = comm.sGetString(reader["dev_code"].ToString());
                    data.fault_code = comm.sGetString(reader["fault_code"].ToString());
                    data.per_code = comm.sGetString(reader["per_code"].ToString());
                    data.per_tel = comm.sGetString(reader["per_tel"].ToString());
                    data.per_mail = comm.sGetString(reader["per_mail"].ToString());
                    data.fault_handle_code = comm.sGetString(reader["fault_handle_code"].ToString());
                    data.rep_memo = comm.sGetString(reader["rep_memo"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());


                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.rep_code)) {
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
        public List<EMT06_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<EMT06_0000> list = new List<EMT06_0000>();

            string sSql = " SELECT EMT06_0000.*, EMB07_0000.dev_name, EMB18_0000.fault_name, EMB20_0000.fault_handle_name " +
                          " FROM EMT06_0000 " +
                          " left join EMB07_0000 on EMB07_0000.dev_code = EMT06_0000.dev_code " +
                          " left join EMB18_0000 on EMB18_0000.fault_code = EMT06_0000.fault_code " +
                          " left join EMB14_0000 on EMB14_0000.per_code = EMT06_0000.per_code " +
                          " left join EMB20_0000 on EMB20_0000.fault_handle_code = EMT06_0000.fault_handle_code ";

            // 取得資料
            list = comm.Get_ListByQuery<EMT06_0000>(sSql, pWhere, pUsrCode, pPrgCode);

            // 權限設定
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            //string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_mtp_code", "par_name", "par_value");
            //var arr_LockGrpCode = sLockGrpCode.Split(',');

            for (int i = 0; i < list.Count; i++)
            {
                //檢查授權刪除、修改
                list[i].can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                list[i].can_update = sLimitStr.Contains("M") ? "Y" : "N";
                
            }

            return list;

        }

        /// <summary>
        /// 傳入一個EMT06_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="EMT06_0000">DTO</param>
        public void InsertData(EMT06_0000 EMT06_0000)
        {
            string sSql = " INSERT INTO " +
                          " EMT06_0000 (  rep_code,  rep_date,  call_code,  dev_code,  fault_code,  per_code,  per_tel,  per_mail, " +
                          "               fault_handle_code,  rep_memo,  ins_date,  ins_time,  usr_code     ) " +

                          "     VALUES ( @rep_code, @rep_date, @call_code, @dev_code, @fault_code, @per_code, @per_tel, @per_mail, " +
                          "              @fault_handle_code, @rep_memo, @ins_date, @ins_time, @usr_code     ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMT06_0000);
            }
        }

        /// <summary>
        /// 傳入一個EMT06_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="EMT06_0000">DTO</param>
        public void UpdateData(EMT06_0000 EMT06_0000)
        {
            string sSql = " UPDATE EMT06_0000                               " +
                          "    SET rep_date          =  @rep_date,          " +
                          "        call_code         =  @call_code,         " +
                          "        dev_code          =  @dev_code,          " +
                          "        fault_code        =  @fault_code,        " +
                          "        per_code          =  @per_code,          " +
                          "        per_tel           =  @per_tel,           " +
                          "        per_mail          =  @per_mail,          " +
                          "        fault_handle_code =  @fault_handle_code, " +
                          "        rep_memo          =  @rep_memo,          " +
                          "        ins_date          =  @ins_date,          " +
                          "        ins_time          =  @ins_time,          " +
                          "        usr_code          =  @usr_code           " +
                          "  WHERE rep_code          =  @rep_code           ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMT06_0000);
                
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM EMT06_0000 WHERE rep_code = @rep_code;";
            //sSql += " Delete from BDP09_0100 where rep_code = @rep_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { rep_code = pTkCode });
                
            }
        }
        
    }
}