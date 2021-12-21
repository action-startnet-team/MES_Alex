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
    public class WMB08_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得WMB08_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO WMB08_0000</returns>
        public WMB08_0000 GetDTO(string pTkCode)
        {
            WMB08_0000 datas = new WMB08_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM WMB08_0000";
            }
            else
            {
                sSql = "SELECT * FROM WMB08_0000 where wmb08_0000=@wmb08_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@wmb08_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new WMB08_0000
                        {
                            wmb08_0000 = comm.sGetInt32(reader["wmb08_0000"].ToString()),
                            sor_sys_code = comm.sGetString(reader["sor_sys_code"].ToString()),
                            sor_sto_code = comm.sGetString(reader["sor_sto_code"].ToString()),
                            sto_code = comm.sGetString(reader["sto_code"].ToString()),
                            cmemo = comm.sGetString(reader["cmemo"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得WMB08_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List WMB08_0000</returns>
        public List<WMB08_0000> Get_DataList(string pTkCode)
        {
            List<WMB08_0000> list = new List<WMB08_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM WMB08_0000";
            }
            else
            {
                sSql = "SELECT * FROM WMB08_0000 where wmb08_0000=@wmb08_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@wmb08_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    WMB08_0000 data = new WMB08_0000();

                    data.wmb08_0000 = comm.sGetInt32(reader["wmb08_0000"].ToString());
                    data.sor_sys_code = comm.sGetString(reader["sor_sys_code"].ToString());
                    data.sor_sto_code = comm.sGetString(reader["sor_sto_code"].ToString());
                    data.sto_code = comm.sGetString(reader["sto_code"].ToString());
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
        public List<WMB08_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_wmb08_0000", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<WMB08_0000> list = new List<WMB08_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM WMB08_0000";
            sSql = "SELECT * FROM WMB08_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@wmb08_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    WMB08_0000 data = new WMB08_0000();

                    data.wmb08_0000 = comm.sGetInt32(reader["wmb08_0000"].ToString());
                    data.sor_sys_code = comm.sGetString(reader["sor_sys_code"].ToString());
                    data.sor_sto_code = comm.sGetString(reader["sor_sto_code"].ToString());
                    data.sto_code = comm.sGetString(reader["sto_code"].ToString());
                    data.cmemo = comm.sGetString(reader["cmemo"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.wmb08_0000)) {
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
        public List<WMB08_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<WMB08_0000> list = new List<WMB08_0000>();

            string sSql = " SELECT *, WMB01_0000.sto_name as sto_name " +
                          " FROM WMB08_0000 " +
                          " left join WMB01_0000 on WMB01_0000.sto_code = WMB08_0000.sto_code ";
            // 取得資料
            list = comm.Get_ListByQuery<WMB08_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個WMB08_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="WMB08_0000">DTO</param>
        public void InsertData(WMB08_0000 WMB08_0000)
        {
            string sSql = "INSERT INTO " +
                          " WMB08_0000 (  sor_sys_code,  sor_sto_code,  sto_code,  cmemo ) " +
                          "     VALUES ( @sor_sys_code, @sor_sto_code, @sto_code, @cmemo ) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMB08_0000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@wmb08_0000", WMB08_0000.wmb08_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@wmb08_0000", WMB08_0000.wmb08_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@sor_sys_code", WMB08_0000.sor_sys_code));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個WMB08_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="WMB08_0000">DTO</param>
        public void UpdateData(WMB08_0000 WMB08_0000)
        {
            string sSql = " UPDATE WMB08_0000                     " +
                          "    SET sor_sys_code =  @sor_sys_code, " +
                          "        sor_sto_code =  @sor_sto_code, " +
                          "        sto_code     =  @sto_code,     " +
                          "        cmemo        =  @cmemo         " +
                          "  WHERE wmb08_0000   =  @wmb08_0000    " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMB08_0000);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@wmb08_0000", WMB08_0000.wmb08_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@wmb08_0000", WMB08_0000.wmb08_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@sor_sys_code", WMB08_0000.sor_sys_code));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM WMB08_0000 WHERE wmb08_0000 = @wmb08_0000;";
            //sSql += " Delete from BDP09_0100 where wmb08_0000 = @wmb08_0000; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { wmb08_0000 = pTkCode });

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@wmb08_0000", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }






        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得WMB08_0000角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetWMB08_0000_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("wmb08_0000", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("wmb08_0000", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("sor_sys_code", System.Type.GetType("System.String"].ToString());

            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM WMB08_0000";
            }
            else
            {
                sSql = "SELECT * FROM WMB08_0000 where wmb08_0000='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["wmb08_0000"] = dtTmp.Rows[i]["wmb08_0000"];
                drow["wmb08_0000"] = dtTmp.Rows[i]["wmb08_0000"];
                drow["sor_sys_code"] = dtTmp.Rows[i]["sor_sys_code"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}