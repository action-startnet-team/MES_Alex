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
    public class EMB11_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得EMB11_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO EMB11_0000</returns>
        public EMB11_0000 GetDTO(string pTkCode)
        {
            EMB11_0000 datas = new EMB11_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMB11_0000";
            }
            else
            {
                sSql = "SELECT * FROM EMB11_0000 where cycle_code=@cycle_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@cycle_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new EMB11_0000
                        {

                            cycle_code = comm.sGetString(reader["cycle_code"].ToString()),
                            cycle_name = comm.sGetString(reader["cycle_name"].ToString()),
                            month = comm.sGetInt32(reader["month"].ToString()),
                            days = comm.sGetInt32(reader["days"].ToString()),
                            cmemo = comm.sGetString(reader["cmemo"].ToString()),



                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得EMB11_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List EMB11_0000</returns>
        public List<EMB11_0000> Get_DataList(string pTkCode)
        {
            List<EMB11_0000> list = new List<EMB11_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMB11_0000";
            }
            else
            {
                sSql = "SELECT * FROM EMB11_0000 where cycle_code=@cycle_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@cycle_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EMB11_0000 data = new EMB11_0000();

                    data.cycle_code = comm.sGetString(reader["cycle_code"].ToString());
                    data.cycle_name = comm.sGetString(reader["cycle_name"].ToString());
                    data.month = comm.sGetInt32(reader["month"].ToString());
                    data.days = comm.sGetInt32(reader["days"].ToString());
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
        public List<EMB11_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_cycle_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<EMB11_0000> list = new List<EMB11_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM EMB11_0000";
            sSql = "SELECT * FROM EMB11_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@cycle_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EMB11_0000 data = new EMB11_0000();

                    data.cycle_code = comm.sGetString(reader["cycle_code"].ToString());
                    data.cycle_name = comm.sGetString(reader["cycle_name"].ToString());
                    data.month = comm.sGetInt32(reader["month"].ToString());
                    data.days = comm.sGetInt32(reader["days"].ToString());
                    data.cmemo = comm.sGetString(reader["cmemo"].ToString());


                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.cycle_code)) {
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
        public List<EMB11_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<EMB11_0000> list = new List<EMB11_0000>();

            string sSql = "SELECT * FROM EMB11_0000";

            // 取得資料
            list = comm.Get_ListByQuery<EMB11_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個EMB11_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="EMB11_0000">DTO</param>
        public void InsertData(EMB11_0000 EMB11_0000)
        {
            string sSql = "INSERT INTO " +
                          " EMB11_0000 (  cycle_code,  cycle_name,  month,  days,  cmemo ) " +
                          "     VALUES ( @cycle_code, @cycle_name, @month, @days, @cmemo ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMB11_0000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@cycle_code", EMB11_0000.cycle_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@cycle_code", EMB11_0000.cycle_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@cycle_name", EMB11_0000.cycle_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個EMB11_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="EMB11_0000">DTO</param>
        public void UpdateData(EMB11_0000 EMB11_0000)
        {
            string sSql = " UPDATE EMB11_0000                " +
                          "    SET cycle_name = @cycle_name, " +
                          "        month      = @month,      " +
                          "        days       = @days,       " +
                          "        cmemo      = @cmemo       " +
                          "  WHERE cycle_code = @cycle_code  ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMB11_0000);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@cycle_code", EMB11_0000.cycle_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@cycle_code", EMB11_0000.cycle_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@cycle_name", EMB11_0000.cycle_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM EMB11_0000 WHERE cycle_code = @cycle_code;";
            //sSql += " Delete from BDP09_0100 where cycle_code = @cycle_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { cycle_code = pTkCode });

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@cycle_code", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }






        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得EMB11_0000角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetEMB11_0000_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("cycle_code", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("cycle_code", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("cycle_name", System.Type.GetType("System.String"].ToString());

            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMB11_0000";
            }
            else
            {
                sSql = "SELECT * FROM EMB11_0000 where cycle_code='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["cycle_code"] = dtTmp.Rows[i]["cycle_code"];
                drow["cycle_code"] = dtTmp.Rows[i]["cycle_code"];
                drow["cycle_name"] = dtTmp.Rows[i]["cycle_name"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}