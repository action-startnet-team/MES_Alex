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
    public class MEP03_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MEP03_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEP03_0000</returns>
        public MEP03_0000 GetDTO(string pTkCode)
        {
            MEP03_0000 datas = new MEP03_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEP03_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEP03_0000 where mep03_0000=@mep03_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@mep03_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEP03_0000
                        {
                            mep03_0000 = reader.GetInt32(reader.GetOrdinal("mep03_0000")),
                            mo_code = reader.GetString(reader.GetOrdinal("mo_code")),
                            wrk_code = reader.GetString(reader.GetOrdinal("wrk_code")),
                            usr_code = reader.GetString(reader.GetOrdinal("usr_code")),
                            work_second = reader.GetDecimal(reader.GetOrdinal("work_second")),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MEP03_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MEP03_0000</returns>
        public List<MEP03_0000> Get_DataList(string pTkCode)
        {
            List<MEP03_0000> list = new List<MEP03_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEP03_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEP03_0000 where mep03_0000=@mep03_0000";
            }


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@mep03_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEP03_0000 data = new MEP03_0000();

                    data.mep03_0000 = reader.GetInt32(reader.GetOrdinal("mep03_0000"));
                    data.mo_code = reader.GetString(reader.GetOrdinal("mo_code"));
                    data.wrk_code = reader.GetString(reader.GetOrdinal("wrk_code"));
                    data.usr_code = reader.GetString(reader.GetOrdinal("usr_code"));
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
        public List<MEP03_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_sup_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MEP03_0000> list = new List<MEP03_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = "SELECT * FROM MEP03_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEP03_0000 data = new MEP03_0000();

                    data.mep03_0000 = reader.GetInt32(reader.GetOrdinal("mep03_0000"));
                    data.mo_code = reader.GetString(reader.GetOrdinal("mo_code"));
                    data.wrk_code = reader.GetString(reader.GetOrdinal("wrk_code"));
                    data.usr_code = reader.GetString(reader.GetOrdinal("usr_code"));
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
        public List<MEP03_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MEP03_0000> list = new List<MEP03_0000>();

            string sSql = " SELECT MEP03_0000.*, BDP08_0000.usr_name " +
                          " FROM MEP03_0000 " +
                          " left join BDP08_0000 on BDP08_0000.usr_code = MEP03_0000.usr_code " ;

            // 取得資料
            list = comm.Get_ListByQuery<MEP03_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個MEP03_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEP03_0000">DTO</param>
        public void InsertData(MEP03_0000 MEP03_0000)
        {
            string sSql = "INSERT INTO " +
                          " MEP03_0000 (  mo_code,  wrk_code,  usr_code,  work_second ) " +
                          "     VALUES ( @mo_code, @wrk_code, @usr_code, @work_second ) ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEP03_0000);
            }
        }

        /// <summary>
        /// 傳入一個MEP03_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEP03_0000">DTO</param>
        public void UpdateData(MEP03_0000 MEP03_0000)
        {
            string sSql = " UPDATE MEP03_0000                     " +
                          "    SET mo_code      =  @mo_code,      " +
                          "        wrk_code     =  @wrk_code,     " +
                          "        usr_code     =  @usr_code,     " +
                          "        work_second  =  @work_second   " +
                          "  WHERE mep03_0000   =  @mep03_0000    " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEP03_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MEP03_0000 WHERE mep03_0000 = @mep03_0000 ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { mep03_0000 = pTkCode });
            }
        }

    }
}