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
    public class MEB49_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MEB49_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEB49_0000</returns>
        public MEB49_0000 GetDTO(string pTkCode)
        {
            MEB49_0000 datas = new MEB49_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB49_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEB49_0000 where dev_code=@dev_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@dev_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEB49_0000
                        {
                            dev_code = comm.sGetString(reader["dev_code"].ToString()),
                            dev_name = comm.sGetString(reader["dev_name"].ToString()),
                            dev_ip = comm.sGetString(reader["dev_ip"].ToString()),
                            connect_code = comm.sGetString(reader["connect_code"].ToString()),
                            cmemo = comm.sGetString(reader["cmemo"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MEB49_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MEB49_0000</returns>
        //public List<MEB49_0000> Get_DataList(string pTkCode)
        //{
        //    List<MEB49_0000> list = new List<MEB49_0000>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM MEB49_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM MEB49_0000 where dev_code=@dev_code";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@dev_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            MEB49_0000 data = new MEB49_0000();

        //            data.dev_code = comm.sGetString(reader["dev_code"].ToString());
        //            data.dev_name = comm.sGetString(reader["dev_name"].ToString());
        //            data.dev_ip = comm.sGetString(reader["dev_ip"].ToString());
        //            data.connect_code = comm.sGetString(reader["connect_code"].ToString());
        //            data.cmemo = comm.sGetString(reader["cmemo"].ToString());

        //            data.can_delete = "Y";
        //            data.can_update = "Y";
        //            list.Add(data);
        //        }

        //    }
        //    return list;
        //}

        /// <summary>
        /// 取得使用者可以編輯的資料，結合商務邏輯權限
        /// </summary>
        /// <param name="pUsrCode"></param>
        /// <param name="pPrgCode"></param>
        /// <returns></returns>
        //public List<MEB49_0000> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_dev_code", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<MEB49_0000> list = new List<MEB49_0000>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    sSql = "SELECT * FROM MEB49_0000";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            MEB49_0000 data = new MEB49_0000();

        //            data.dev_code = comm.sGetString(reader["dev_code"].ToString());
        //            data.dev_name = comm.sGetString(reader["dev_name"].ToString());
        //            data.dev_ip = comm.sGetString(reader["dev_ip"].ToString());


        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            //資料邏輯刪除、修改
        //            //if (arr_LockGrpCode.Contains(data.dev_code)) {
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
        public List<MEB49_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MEB49_0000> list = new List<MEB49_0000>();

            string sSql = "SELECT * FROM MEB49_0000";

            // 取得資料
            list = comm.Get_ListByQuery<MEB49_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個MEB49_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEB49_0000">DTO</param>
        public void InsertData(MEB49_0000 MEB49_0000)
        {
            string sSql = "INSERT INTO " +
                          " MEB49_0000 (  dev_code,  dev_name,  dev_ip,  connect_code,  cmemo ) " +
                          "     VALUES ( @dev_code, @dev_name, @dev_ip, @connect_code, @cmemo ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB49_0000);
            }
        }

        /// <summary>
        /// 傳入一個MEB49_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEB49_0000">DTO</param>
        public void UpdateData(MEB49_0000 MEB49_0000)
        {
            string sSql = " UPDATE MEB49_0000 " +
                          "    SET dev_name     = @dev_name,     " +
                          "        dev_ip       = @dev_ip,       " +
                          "        connect_code = @connect_code, " +
                          "        cmemo        = @cmemo         " +
                          "  WHERE dev_code     = @dev_code      ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB49_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MEB49_0000 WHERE dev_code = @dev_code ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { dev_code = pTkCode });
            }
        }




    }
}