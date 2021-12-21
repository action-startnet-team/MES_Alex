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
    public class MEB15_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MEB15_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEB15_0000</returns>
        public MEB15_0000 GetDTO(string pTkCode)
        {
            MEB15_0000 datas = new MEB15_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB15_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEB15_0000 where mac_code=@mac_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@mac_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEB15_0000
                        {

                            mac_code = comm.sGetString(reader["mac_code"].ToString()),
                            mac_name = comm.sGetString(reader["mac_name"].ToString()),
                            cmemo = comm.sGetString(reader["cmemo"].ToString()),
                            line_code = comm.sGetString(reader["line_code"].ToString()),
                            mac_type = comm.sGetString(reader["mac_type"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MEB15_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MEB15_0000</returns>
        public List<MEB15_0000> Get_DataList(string pTkCode)
        {
            List<MEB15_0000> list = new List<MEB15_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB15_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEB15_0000 where mac_code=@mac_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@mac_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB15_0000 data = new MEB15_0000();

                    data.mac_code = comm.sGetString(reader["mac_code"].ToString());
                    data.mac_name = comm.sGetString(reader["mac_name"].ToString());
                    data.cmemo = comm.sGetString(reader["cmemo"].ToString());
                    data.line_code = comm.sGetString(reader["line_code"].ToString());
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
        public List<MEB15_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_mac_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MEB15_0000> list = new List<MEB15_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = "SELECT * FROM MEB15_0000";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB15_0000 data = new MEB15_0000();

                    data.mac_code = comm.sGetString(reader["mac_code"].ToString());
                    data.mac_name = comm.sGetString(reader["mac_name"].ToString());
                    data.cmemo = comm.sGetString(reader["cmemo"].ToString());
                    data.line_code = comm.sGetString(reader["line_code"].ToString());

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
        public List<MEB15_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MEB15_0000> list = new List<MEB15_0000>();

            //string sSql = " SELECT distinct MEB15_0000.mac_code, MEB15_0000.*, MEB14_0000.mac_type_name, BDP21_0100.field_name as ip_type_name, MEB12_0000.line_name " +
            //              " FROM MEB15_0000 " +
            //              @"left join (
            //                    select MEB45_0100.*, MEB45_0000.stop_name
            //                    from MEB45_0100
            //                    left join MEB45_0000 on MEB45_0000.stop_code = MEB45_0100.stop_code
            //                ) as s on s.mac_code = MEB15_0000.mac_code " +
            //              " left join MEB14_0000 on MEB14_0000.mac_type_code = MEB15_0000.mac_type_code " +
            //              " left join BDP21_0100 on BDP21_0100.field_code = MEB15_0000.ip_type and BDP21_0100.code_code = 'ip_type' " +
            //              " left join MEB12_0000 on MEB12_0000.line_code = MEB15_0000.line_code ";

            string sSql = " SELECT distinct MEB15_0000.mac_code, MEB15_0000.*, MEB12_0000.line_name " +
                        " FROM MEB15_0000 " +
                        " left join MEB12_0000 on MEB12_0000.line_code = MEB15_0000.line_code ";

            // 取得資料
            list = comm.Get_ListByQuery<MEB15_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個MEB15_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEB15_0000">DTO</param>
        public void InsertData(MEB15_0000 MEB15_0000)
        {
            string sSql = "INSERT INTO " +
                          " MEB15_0000 (  mac_code,  mac_name,  cmemo,  line_code,  mac_type ) " +
                          "     VALUES ( @mac_code, @mac_name, @cmemo, @line_code, @mac_type ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB15_0000);
            }
        }

        /// <summary>
        /// 傳入一個MEB15_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEB15_0000">DTO</param>
        public void UpdateData(MEB15_0000 MEB15_0000)
        {
            string sSql = " UPDATE MEB15_0000                        " +
                          "    SET mac_name       =  @mac_name,      " +
                          "        cmemo          =  @cmemo,         " +
                          "        line_code      =  @line_code,     " +
                          "        mac_type       =  @mac_type       " +
                          "  WHERE mac_code       =  @mac_code       " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB15_0000);
                
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MEB15_0000 WHERE mac_code = @mac_code;";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { mac_code = pTkCode });
            }
        }
        

    }
}