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
    public class EMB04_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得EMB04_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO EMB04_0000</returns>
        public EMB04_0000 GetDTO(string pTkCode)
        {
            EMB04_0000 datas = new EMB04_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMB04_0000";
            }
            else
            {
                sSql = "SELECT * FROM EMB04_0000 where cal_date=@cal_date";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@cal_date", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new EMB04_0000
                        {

                            cal_date = comm.sGetString(reader["cal_date"].ToString()),
                            is_holiday = comm.sGetString(reader["is_holiday"].ToString()),
                            cmemo = comm.sGetString(reader["cmemo"].ToString()),

                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得EMB04_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List EMB04_0000</returns>
        public List<EMB04_0000> Get_DataList(string pTkCode)
        {
            List<EMB04_0000> list = new List<EMB04_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMB04_0000";
            }
            else
            {
                sSql = "SELECT * FROM EMB04_0000 where cal_date=@cal_date";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@cal_date", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EMB04_0000 data = new EMB04_0000();

                    data.cal_date = comm.sGetString(reader["cal_date"].ToString());
                    data.is_holiday = comm.sGetString(reader["is_holiday"].ToString());
                    data.cmemo = comm.sGetString(reader["cmemo"].ToString());

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
        public List<EMB04_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_cal_date", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<EMB04_0000> list = new List<EMB04_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM EMB04_0000";
            sSql = "SELECT * FROM EMB04_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EMB04_0000 data = new EMB04_0000();

                    data.cal_date = comm.sGetString(reader["cal_date"].ToString());
                    data.is_holiday = comm.sGetString(reader["is_holiday"].ToString());
                    data.cmemo = comm.sGetString(reader["cmemo"].ToString());

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
        public List<EMB04_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<EMB04_0000> list = new List<EMB04_0000>();

            string sSql = " SELECT * FROM EMB04_0000 ";

            // 取得資料
            list = comm.Get_ListByQuery<EMB04_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個EMB04_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="EMB04_0000">DTO</param>
        public void InsertData(EMB04_0000 EMB04_0000)
        {
            string sSql = "INSERT INTO " +
                          " EMB04_0000 (  cal_date,  is_holiday,  cmemo )   " +
                          "     VALUES ( @cal_date, @is_holiday, @cmemo )   " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMB04_0000);
            }
        }

        /// <summary>
        /// 傳入一個EMB04_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="EMB04_0000">DTO</param>
        public void UpdateData(EMB04_0000 EMB04_0000)
        {
            string sSql = " UPDATE EMB04_0000                    " +
                          "    SET is_holiday =  @is_holiday,    " +
                          "        cmemo      =  @cmemo         " +
                          "  WHERE cal_date   =  @cal_date       ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMB04_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM EMB04_0000 WHERE cal_date = @cal_date;";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { cal_date = pTkCode });
            }
        }

    }
}