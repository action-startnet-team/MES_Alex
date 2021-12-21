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
    public class EMB01_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得EMB01_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO EMB01_0000</returns>
        public EMB01_0000 GetDTO(string pTkCode)
        {
            EMB01_0000 datas = new EMB01_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMB01_0000";
            }
            else
            {
                sSql = "SELECT * FROM EMB01_0000 where factory_code=@factory_code";
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
                        datas = new EMB01_0000
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
        /// 取得EMB01_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List EMB01_0000</returns>
        public List<EMB01_0000> Get_DataList(string pTkCode)
        {
            List<EMB01_0000> list = new List<EMB01_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMB01_0000";
            }
            else
            {
                sSql = "SELECT * FROM EMB01_0000 where factory_code=@factory_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@factory_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EMB01_0000 data = new EMB01_0000();

                    data.factory_code = comm.sGetString(reader["factory_code"].ToString());
                    data.factory_name = comm.sGetString(reader["factory_name"].ToString());
                    data.factory_add = comm.sGetString(reader["factory_add"].ToString());
                    data.country_code = comm.sGetString(reader["country_code"].ToString());
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
        public List<EMB01_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_factory_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<EMB01_0000> list = new List<EMB01_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM EMB01_0000";
            sSql = "SELECT * FROM EMB01_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@factory_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EMB01_0000 data = new EMB01_0000();

                    data.factory_code = comm.sGetString(reader["factory_code"].ToString());
                    data.factory_name = comm.sGetString(reader["factory_name"].ToString());
                    data.factory_add = comm.sGetString(reader["factory_add"].ToString());
                    data.country_code = comm.sGetString(reader["country_code"].ToString());
                    data.cmemo = comm.sGetString(reader["cmemo"].ToString());

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
        public List<EMB01_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<EMB01_0000> list = new List<EMB01_0000>();

            string sSql = " SELECT EMB01_0000.*, BDP21_0100.field_name as country_name " +
                          " FROM EMB01_0000 " +
                          " left join BDP21_0100 on BDP21_0100.field_code = EMB01_0000.country_code and BDP21_0100.code_code = 'country_code' " ;

            // 取得資料
            list = comm.Get_ListByQuery<EMB01_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
                //        data.sup_name = data.sup_code + " - " + comm.sGetString(reader["sup_name"].ToString());
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
        /// 傳入一個EMB01_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="EMB01_0000">DTO</param>
        public void InsertData(EMB01_0000 EMB01_0000)
        {
            string sSql = "INSERT INTO " +
                          " EMB01_0000 (  factory_code,  factory_name,  factory_add,  country_code,  cmemo )   " +
                          "     VALUES ( @factory_code, @factory_name, @factory_add, @country_code, @cmemo )   " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMB01_0000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@factory_code", EMB01_0000.factory_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@factory_code", EMB01_0000.factory_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@factory_name", EMB01_0000.factory_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個EMB01_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="EMB01_0000">DTO</param>
        public void UpdateData(EMB01_0000 EMB01_0000)
        {
            string sSql = " UPDATE EMB01_0000                    " +
                          "    SET factory_name = @factory_name, " +
                          "        factory_add  = @factory_add,  " +
                          "        country_code = @country_code, " +
                          "        cmemo        = @cmemo         " +
                          "  WHERE factory_code = @factory_code  ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMB01_0000);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@factory_code", EMB01_0000.factory_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@factory_code", EMB01_0000.factory_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@factory_name", EMB01_0000.factory_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM EMB01_0000 WHERE factory_code = @factory_code;";
            //sSql += " Delete from BDP09_0100 where factory_code = @factory_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { factory_code = pTkCode });

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@factory_code", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }






        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得EMB01_0000角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetEMB01_0000_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("factory_code", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("factory_code", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("factory_name", System.Type.GetType("System.String"].ToString());

            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMB01_0000";
            }
            else
            {
                sSql = "SELECT * FROM EMB01_0000 where factory_code='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["factory_code"] = dtTmp.Rows[i]["factory_code"];
                drow["factory_code"] = dtTmp.Rows[i]["factory_code"];
                drow["factory_name"] = dtTmp.Rows[i]["factory_name"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}