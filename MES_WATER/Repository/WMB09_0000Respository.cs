
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
    public class WMB09_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得WMB09_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO WMB09_0000</returns>
        public WMB09_0000 GetDTO(string pTkCode)
        {
            WMB09_0000 datas = new WMB09_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM WMB09_0000";
            }
            else
            {
                sSql = "SELECT * FROM WMB09_0000 where label_code=@label_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@label_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new WMB09_0000
                        {

                            label_code = comm.sGetString(reader["label_code"].ToString()),
                            label_name = comm.sGetString(reader["label_name"].ToString()),
                            label_type = comm.sGetString(reader["label_type"].ToString()),
                            is_use = comm.sGetString(reader["is_use"].ToString()),


                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得BDP07_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List BDP07_0000</returns>
        public List<BDP07_0000> Get_DataList(string pTkCode)
        {
            List<BDP07_0000> list = new List<BDP07_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP07_0000";
            }
            else
            {
                sSql = "SELECT * FROM BDP07_0000 where grp_code=@grp_code";
            }


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@grp_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP07_0000 data = new BDP07_0000();

                    data.grp_code = comm.sGetString(reader["grp_code"].ToString());
                    data.grp_name = comm.sGetString(reader["grp_name"].ToString());
                    data.is_use = comm.sGetString(reader["is_use"].ToString());


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
        public List<BDP07_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_sup_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<BDP07_0000> list = new List<BDP07_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM BDP07_0000";
            sSql = "SELECT * FROM BDP07_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@grp_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP07_0000 data = new BDP07_0000();

                    data.grp_code = comm.sGetString(reader["grp_code"].ToString());
                    data.grp_name = comm.sGetString(reader["grp_name"].ToString());
                    data.is_use = comm.sGetString(reader["is_use"].ToString());



                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.grp_code)) {
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
        public List<WMB09_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<WMB09_0000> list = new List<WMB09_0000>();

            string sSql = " SELECT WMB09_0000.* " +
                          " FROM WMB09_0000 ";

            // 取得資料
            list = comm.Get_ListByQuery<WMB09_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
                //        data.label_name = data.label_code + " - " + comm.sGetString(reader["label_name"].ToString());
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
        /// 傳入一個WMB09_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="WMB09_0000">DTO</param>
        public void InsertData(WMB09_0000 WMB09_0000)
        {
            string sSql = "INSERT INTO " +
                          " WMB09_0000 (  label_code,  label_name, label_type,  is_use )  " +
                          "     VALUES ( @label_code, @label_name,@label_type, @is_use )  ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMB09_0000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@label_code", MEB18_0000.label_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@label_code", MEB18_0000.label_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@label_name", MEB18_0000.label_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個WMB09_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="WMB09_0000">DTO</param>
        public void UpdateData(WMB09_0000 WMB09_0000)
        {
            string sSql = " UPDATE WMB09_0000 " +
                          "    SET label_name  = @label_name,    " +
                          "    label_type  =@label_type,       " +
                          "        is_use  = @is_use      " +
                          "  WHERE label_code= @label_code ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMB09_0000);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@label_code", MEB18_0000.label_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@label_code", MEB18_0000.label_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@label_name", MEB18_0000.label_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM WMB09_0000 WHERE label_code = @label_code;";
            //sSql += " Delete from BDP09_0100 where label_code = @label_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { label_code = pTkCode });

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@label_code", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }






        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得WMB09_0000角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetWMB09_0000_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("sup_code", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("sup_code", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("label_name", System.Type.GetType("System.String"].ToString());

            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM WMB09_0000";
            }
            else
            {
                sSql = "SELECT * FROM WMB09_0000 where sup_code='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["sup_code"] = dtTmp.Rows[i]["sup_code"];
                drow["sup_code"] = dtTmp.Rows[i]["sup_code"];
                drow["label_name"] = dtTmp.Rows[i]["label_name"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}