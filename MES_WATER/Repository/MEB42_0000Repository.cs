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
    public class MEB42_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MEB42_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEB42_0000</returns>
        public MEB42_0000 GetDTO(string pTkCode)
        {
            MEB42_0000 datas = new MEB42_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB42_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEB42_0000 where dis_scrapped_code=@dis_scrapped_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@dis_scrapped_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEB42_0000
                        {
                            
                            dis_scrapped_code = comm.sGetString(reader["dis_scrapped_code"].ToString()),
                            dis_scrapped_name = comm.sGetString(reader["dis_scrapped_name"].ToString()),
                            cmemo = comm.sGetString(reader["cmemo"].ToString()),
                          

                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MEB42_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MEB42_0000</returns>
        public List<MEB42_0000> Get_DataList(string pTkCode)
        {
            List<MEB42_0000> list = new List<MEB42_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB42_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEB42_0000 where dis_scrapped_code=@dis_scrapped_code";
            }


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@dis_scrapped_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB42_0000 data = new MEB42_0000();
                   
                    data.dis_scrapped_code = comm.sGetString(reader["dis_scrapped_code"].ToString());
                    data.dis_scrapped_name = comm.sGetString(reader["dis_scrapped_name"].ToString());
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
        public List<MEB42_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_dis_scrapped_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MEB42_0000> list = new List<MEB42_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM MEB42_0000";
            sSql = "SELECT * FROM MEB42_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@dis_scrapped_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB42_0000 data = new MEB42_0000();

                    data.dis_scrapped_code = comm.sGetString(reader["dis_scrapped_code"].ToString());
                    data.dis_scrapped_name = comm.sGetString(reader["dis_scrapped_name"].ToString());
                    data.cmemo = comm.sGetString(reader["cmemo"].ToString());
 

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.dis_scrapped_code)) {
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
        public List<MEB42_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MEB42_0000> list = new List<MEB42_0000>();

            string sSql = "SELECT * FROM MEB42_0000";

            // 取得資料
            list = comm.Get_ListByQuery<MEB42_0000>(sSql, pWhere, pUsrCode, pPrgCode);

            // 權限設定
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            //string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_mtp_code", "par_name", "par_value");
            //var arr_LockGrpCode = sLockGrpCode.Split(',');

            for (int i = 0; i < list.Count; i++)
            {
                //檢查授權刪除、修改
                list[i].can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                list[i].can_update = sLimitStr.Contains("M") ? "Y" : "N";

                //        // 特例 轉換
                //        data.dis_scrapped_name = data.dis_scrapped_code + " - " + comm.sGetString(reader["dis_scrapped_name"].ToString());
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
        /// 傳入一個MEB42_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEB42_0000">DTO</param>
        public void InsertData(MEB42_0000 MEB42_0000)
        {
            string sSql = "INSERT INTO " +
                          " MEB42_0000 (  dis_scrapped_code,  dis_scrapped_name, cmemo )  " +
                          "     VALUES ( @dis_scrapped_code, @dis_scrapped_name, @cmemo )  " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB42_0000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@dis_scrapped_code", MEB42_0000.dis_scrapped_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@dis_scrapped_code", MEB42_0000.dis_scrapped_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@dis_scrapped_name", MEB42_0000.dis_scrapped_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個MEB42_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEB42_0000">DTO</param>
        public void UpdateData(MEB42_0000 MEB42_0000)
        {
            string sSql = " UPDATE MEB42_0000 " +
                          "    SET dis_scrapped_name = @dis_scrapped_name,  " +
                          "        cmemo    = @cmemo      " +              
                          "  WHERE dis_scrapped_code = @dis_scrapped_code ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB42_0000);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@dis_scrapped_code", MEB42_0000.dis_scrapped_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@dis_scrapped_code", MEB42_0000.dis_scrapped_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@dis_scrapped_name", MEB42_0000.dis_scrapped_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MEB42_0000 WHERE dis_scrapped_code = @dis_scrapped_code;";
            //sSql += " Delete from BDP09_0100 where dis_scrapped_code = @dis_scrapped_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { dis_scrapped_code = pTkCode });

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@dis_scrapped_code", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        




        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得MEB42_0000角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetMEB42_0000_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("dis_scrapped_code", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("dis_scrapped_code", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("dis_scrapped_name", System.Type.GetType("System.String"].ToString());

            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB42_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEB42_0000 where dis_scrapped_code='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["dis_scrapped_code"] = dtTmp.Rows[i]["dis_scrapped_code"];
                drow["dis_scrapped_code"] = dtTmp.Rows[i]["dis_scrapped_code"];
                drow["dis_scrapped_name"] = dtTmp.Rows[i]["dis_scrapped_name"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}