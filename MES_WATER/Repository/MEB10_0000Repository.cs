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
    public class MEB10_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MEB10_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEB10_0000</returns>
        public MEB10_0000 GetDTO(string pTkCode)
        {
            MEB10_0000 datas = new MEB10_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB10_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEB10_0000 where factory_code=@factory_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@factory_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEB10_0000
                        {
                            factory_code = comm.sGetString(reader["factory_code"].ToString()),
                            factory_name = comm.sGetString(reader["factory_name"].ToString()),
                            factory_add = comm.sGetString(reader["factory_add"].ToString()),
                            country_code = comm.sGetString(reader["country_code"].ToString()),
                            cmemo = comm.sGetString(reader["cmemo"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MEB10_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MEB10_0000</returns>
        public List<MEB10_0000> Get_DataList(string pTkCode)
        {
            List<MEB10_0000> list = new List<MEB10_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB10_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEB10_0000 where factory_code=@factory_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@factory_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB10_0000 data = new MEB10_0000();

                    data.factory_code = comm.sGetString(reader["factory_code"].ToString());
                    data.factory_name = comm.sGetString(reader["factory_name"].ToString());
                    data.factory_add  = comm.sGetString(reader["factory_add"].ToString());
                    data.country_code = comm.sGetString(reader["country_code"].ToString());
                    data.cmemo        = comm.sGetString(reader["cmemo"].ToString());


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
        public List<MEB10_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_factory_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MEB10_0000> list = new List<MEB10_0000>();
            string sSql = "";

            //取得該使用者可以看的資料

            sSql = "SELECT * FROM MEB10_0000";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@factory_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB10_0000 data = new MEB10_0000();

                    data.factory_code = comm.sGetString(reader["factory_code"].ToString());
                    data.factory_name = comm.sGetString(reader["factory_name"].ToString());
                    data.factory_add  = comm.sGetString(reader["factory_add"].ToString());
                    data.country_code = comm.sGetString(reader["country_code"].ToString());
                    data.cmemo        = comm.sGetString(reader["cmemo"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.factory_code)) {
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
        public List<MEB10_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MEB10_0000> list = new List<MEB10_0000>();

            string sSql = "SELECT* FROM MEB10_0000 " ;
            // 取得資料
            list = comm.Get_ListByQuery<MEB10_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個MEB10_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEB10_0000">DTO</param>
        public void InsertData(MEB10_0000 MEB10_0000)
        {
            string sSql = "INSERT INTO " +
                          " MEB10_0000 (  factory_code,  factory_name,  factory_add,  country_code,  cmemo )" +
                          "     VALUES ( @factory_code, @factory_name, @factory_add, @country_code, @cmemo )";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB10_0000);
            }
        }

        /// <summary>
        /// 傳入一個MEB10_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEB10_0000">DTO</param>
        public void UpdateData(MEB10_0000 MEB10_0000)
        {
            string sSql = " UPDATE MEB10_0000 " +
                          "    SET factory_name  =  @factory_name, " +
                          "        factory_add   =  @factory_add,  " +
                          "        country_code  =  @country_code, " +
                          "        cmemo         =  @cmemo         " +
                          "  WHERE factory_code  =  @factory_code  " ;

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB10_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MEB10_0000 WHERE factory_code = @factory_code ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { factory_code = pTkCode });
            }
        }
        

    }
}