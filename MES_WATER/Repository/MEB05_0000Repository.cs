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
    public class MEB05_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MEB05_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEB05_0000</returns>
        public MEB05_0000 GetDTO(string pTkCode)
        {
            MEB05_0000 datas = new MEB05_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT * FROM MEB05_0000";
            }
            else
            {
                sSql = " SELECT * FROM MEB05_0000 where samp_code=@samp_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@samp_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEB05_0000
                        {
                            samp_code = comm.sGetString(reader["samp_code"].ToString()),
                            samp_name = comm.sGetString(reader["samp_name"].ToString()),
                            cmemo = comm.sGetString(reader["cmemo"].ToString()),
                            ins_date = comm.sGetDateTime(reader["ins_date"].ToString()),
                            usr_code = comm.sGetString(reader["usr_code"].ToString()),
                            gooflow_json = comm.sGetString(reader["gooflow_json"].ToString())

                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MEB05_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MEB05_0000</returns>
        //public List<MEB05_0000> Get_DataList(string pTkCode)
        //{
        //    List<MEB05_0000> list = new List<MEB05_0000>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM MEB05_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM MEB05_0000 where samp_code=@samp_code";
        //    }


        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@samp_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            MEB05_0000 data = new MEB05_0000();

        //            data.samp_code = comm.sGetString(reader["samp_code"].ToString());
        //            data.samp_name = comm.sGetString(reader["samp_name"].ToString());
        //            data.cmemo = comm.sGetString(reader["cmemo"].ToString());
        //            data.ins_date = comm.sGetDateTime(reader["ins_date"].ToString());
        //            data.usr_code = comm.sGetString(reader["usr_code"].ToString());
        //            data.gooflow_json = comm.sGetString(reader["gooflow_json"].ToString());


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
        public List<MEB05_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_samp_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MEB05_0000> list = new List<MEB05_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM MEB05_0000";
            sSql = "SELECT * FROM MEB05_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@samp_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB05_0000 data = new MEB05_0000();

                    data.samp_code = comm.sGetString(reader["samp_code"].ToString());
                    data.samp_name = comm.sGetString(reader["samp_name"].ToString());
                    data.cmemo = comm.sGetString(reader["cmemo"].ToString());
                    data.ins_date = comm.sGetDateTime(reader["ins_date"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                    data.gooflow_json = comm.sGetString(reader["gooflow_json"].ToString());


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
        public List<MEB05_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MEB05_0000> list = new List<MEB05_0000>();

            string sSql = " SELECT MEB05_0000.*" +
                          " FROM MEB05_0000 " ;

            // 取得資料
            list = comm.Get_ListByQuery<MEB05_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個MEB05_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEB05_0000">DTO</param>
        public void InsertData(MEB05_0000 MEB05_0000)
        {

            string sSql = "INSERT INTO " +
                          " MEB05_0000 (  samp_code,  samp_name,  cmemo,  ins_date,  usr_code,  gooflow_json ) " +
                          "     VALUES ( @samp_code, @samp_name, @cmemo, @ins_date, @usr_code, @gooflow_json ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB05_0000);

            }
        }

        /// <summary>
        /// 傳入一個MEB05_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEB05_0000">DTO</param>
        public void UpdateData(MEB05_0000 MEB05_0000)
        {
            string sSql = " UPDATE MEB05_0000              " +
                          "    SET samp_name = @samp_name, " +
                          "        cmemo     = @cmemo,     " +
                          "        usr_code  = @usr_code   " +
                          "  WHERE samp_code = @samp_code  ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB05_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MEB05_0000 WHERE samp_code = @samp_code;";
            //sSql += " Delete from BDP09_0100 where samp_code = @samp_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { samp_code = pTkCode });
            }
        }
    }
}