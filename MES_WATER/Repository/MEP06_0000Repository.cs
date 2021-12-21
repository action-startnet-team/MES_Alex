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
    public class MEP06_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MEP06_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEP06_0000</returns>
        public MEP06_0000 GetDTO(string pTkCode)
        {
            MEP06_0000 datas = new MEP06_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEP06_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEP06_0000 where mep06_0000=@mep06_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@mep06_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEP06_0000
                        {
                            mep06_0000 = reader.GetInt32(reader.GetOrdinal("mep06_0000")),
                            mac_code = reader.GetString(reader.GetOrdinal("mac_code")),
                            date_s = reader.GetString(reader.GetOrdinal("date_s")),
                            time_s = reader.GetString(reader.GetOrdinal("time_s")),
                            date_e = reader.GetString(reader.GetOrdinal("date_e")),
                            time_e = reader.GetString(reader.GetOrdinal("time_e")),
                            work_second = reader.GetDecimal(reader.GetOrdinal("work_second")),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MEP06_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MEP06_0000</returns>
        public List<MEP06_0000> Get_DataList(string pTkCode)
        {
            List<MEP06_0000> list = new List<MEP06_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEP06_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEP06_0000 where mep06_0000=@mep06_0000";
            }


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@mep06_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEP06_0000 data = new MEP06_0000();

                    data.mep06_0000 = reader.GetInt32(reader.GetOrdinal("mep06_0000"));
                    data.mac_code = reader.GetString(reader.GetOrdinal("mac_code"));
                    data.date_s = reader.GetString(reader.GetOrdinal("date_s"));
                    data.time_s = reader.GetString(reader.GetOrdinal("time_s"));
                    data.date_e = reader.GetString(reader.GetOrdinal("date_e"));
                    data.time_e = reader.GetString(reader.GetOrdinal("time_e"));
                    data.work_second = reader.GetDecimal(reader.GetOrdinal("work_second"));

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
        public List<MEP06_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_sup_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MEP06_0000> list = new List<MEP06_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = "SELECT * FROM MEP06_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEP06_0000 data = new MEP06_0000();

                    data.mep06_0000 = reader.GetInt32(reader.GetOrdinal("mep06_0000"));
                    data.mac_code = reader.GetString(reader.GetOrdinal("mac_code"));
                    data.date_s = reader.GetString(reader.GetOrdinal("date_s"));
                    data.time_s = reader.GetString(reader.GetOrdinal("time_s"));
                    data.date_e = reader.GetString(reader.GetOrdinal("date_e"));
                    data.time_e = reader.GetString(reader.GetOrdinal("time_e"));
                    data.work_second = reader.GetDecimal(reader.GetOrdinal("work_second"));

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
        public List<MEP06_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MEP06_0000> list = new List<MEP06_0000>();

            string sSql = " SELECT MEP06_0000.*, MEB15_0000.mac_name " +
                          " FROM MEP06_0000 " +
                          " left join MEB15_0000 on MEB15_0000.mac_code = MEP06_0000.mac_code ";

            // 取得資料
            list = comm.Get_ListByQuery<MEP06_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個MEP06_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEP06_0000">DTO</param>
        public void InsertData(MEP06_0000 MEP06_0000)
        {
            string sSql = "INSERT INTO " +
                          " MEP06_0000 (  mac_code,  date_s,  time_s,  date_e,  time_e,  work_second ) " +
                          "     VALUES ( @mac_code, @date_s, @time_s, @date_e, @time_e, @work_second ) ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEP06_0000);
            }
        }

        /// <summary>
        /// 傳入一個MEP06_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEP06_0000">DTO</param>
        public void UpdateData(MEP06_0000 MEP06_0000)
        {
            string sSql = " UPDATE MEP06_0000 " +
                          "    SET mac_code      =  @mac_code,     " +
                          "        date_s        =  @date_s,       " +
                          "        time_s        =  @time_s,       " +
                          "        date_e        =  @date_e,       " +
                          "        time_e        =  @time_e,       " +
                          "        work_second   =  @work_second   " +
                          "  WHERE mep06_0000    =  @mep06_0000    ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEP06_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MEP06_0000 WHERE mep06_0000 = @mep06_0000;";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { mep06_0000 = pTkCode });
            }
        }

    }
}