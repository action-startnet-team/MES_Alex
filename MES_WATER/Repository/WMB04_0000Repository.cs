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
    public class WMB04_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得WMB04_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO WMB04_0000</returns>
        public WMB04_0000 GetDTO(string pTkCode)
        {
            WMB04_0000 datas = new WMB04_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM WMB04_0000";
            }
            else
            {
                sSql = "SELECT * FROM WMB04_0000 where wmb04_0000=@wmb04_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@wmb04_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new WMB04_0000
                        {
                            
                            wmb04_0000 = comm.sGetInt32(reader["wmb04_0000"].ToString()),
                            pro_code = comm.sGetString(reader["pro_code"].ToString()),
                            unit_code_base = comm.sGetString(reader["unit_code_base"].ToString()),
                            unit_code_chg = comm.sGetString(reader["unit_code_chg"].ToString()),
                            chg_rate = comm.sGetDecimal(reader["chg_rate"].ToString()),
                         

                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得WMB04_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List WMB04_0000</returns>
        public List<WMB04_0000> Get_DataList(string pTkCode)
        {
            List<WMB04_0000> list = new List<WMB04_0000>();
            string sSql = "";


            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM WMB04_0000";
            }
            else
            {
                sSql = "SELECT * FROM WMB04_0000 where wmb04_0000=@wmb04_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@wmb04_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    WMB04_0000 data = new WMB04_0000();
                   
                    data.wmb04_0000 = comm.sGetInt32(reader["wmb04_0000"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.unit_code_base = comm.sGetString(reader["unit_code_base"].ToString());
                    data.unit_code_chg = comm.sGetString(reader["unit_code_chg"].ToString());
                    data.chg_rate = comm.sGetDecimal(reader["chg_rate"].ToString());
          
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
        public List<WMB04_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_wmb04_0000", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<WMB04_0000> list = new List<WMB04_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM WMB04_0000";
            sSql = "SELECT * FROM WMB04_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@wmb04_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    WMB04_0000 data = new WMB04_0000();

                    data.wmb04_0000 = comm.sGetInt32(reader["wmb04_0000"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.unit_code_base = comm.sGetString(reader["unit_code_base"].ToString());
                    data.unit_code_chg = comm.sGetString(reader["unit_code_chg"].ToString());
                    data.chg_rate = comm.sGetDecimal(reader["chg_rate"].ToString());
                    

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.wmb04_0000)) {
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
        public List<WMB04_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<WMB04_0000> list = new List<WMB04_0000>();

            string sSql = "SELECT WMB04_0000.*, WMB06_0000.pro_name as pro_name,  WMB06_0000.unit_qty_min as base_rate" +
                          "  FROM WMB04_0000    " +
                          " left join WMB06_0000 on WMB06_0000.pro_code = WMB04_0000.pro_code ";
            // 取得資料
            list = comm.Get_ListByQuery<WMB04_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
                //        data.sup_name = data.chg_rate + " - " + comm.sGetString(reader["sup_name"].ToString());
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
        /// 傳入一個WMB04_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="WMB04_0000">DTO</param>
        public void InsertData(WMB04_0000 WMB04_0000)
        {


            string sSql = "INSERT INTO " +
                          " WMB04_0000  (  pro_code,  unit_code_base,  unit_code_chg,  chg_rate )   " +
                          "     VALUES  ( @pro_code, @unit_code_base, @unit_code_chg, @chg_rate )   ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMB04_0000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@wmb04_0000", WMB04_0000.wmb04_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@wmb04_0000", WMB04_0000.wmb04_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@inq_date", WMB04_0000.inq_date));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個WMB04_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="WMB04_0000">DTO</param>
        public void UpdateData(WMB04_0000 WMB04_0000)
        {
            string sSql = " UPDATE WMB04_0000 " +
                          "    SET       pro_code  = @pro_code,        " +
                          "        unit_code_base  = @unit_code_base,  " +
                          "         unit_code_chg  = @unit_code_chg,   " +
                          "              chg_rate  = @chg_rate         " +
                        
                          "  WHERE wmb04_0000= @wmb04_0000 ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMB04_0000);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@wmb04_0000", WMB04_0000.wmb04_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@wmb04_0000", WMB04_0000.wmb04_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@inq_date", WMB04_0000.inq_date));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM WMB04_0000 WHERE wmb04_0000 = @wmb04_0000;";
            //sSql += " Delete from BDP09_0100 where wmb04_0000 = @wmb04_0000; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { wmb04_0000 = pTkCode });

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@wmb04_0000", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        




        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得WMB04_0000角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetWMB04_0000_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("wmb04_0000", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("wmb04_0000", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("inq_date", System.Type.GetType("System.String"].ToString());

            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM WMB04_0000";
            }
            else
            {
                sSql = "SELECT * FROM WMB04_0000 where wmb04_0000='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["wmb04_0000"] = dtTmp.Rows[i]["wmb04_0000"];
                drow["wmb04_0000"] = dtTmp.Rows[i]["wmb04_0000"];
                drow["inq_date"] = dtTmp.Rows[i]["inq_date"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}