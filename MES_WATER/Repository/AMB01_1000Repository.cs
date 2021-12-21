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
    public class AMB01_1000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得AMB01_1000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO AMB01_1000</returns>
        public AMB01_1000 GetDTO(string pTkCode)
        {
            AMB01_1000 datas = new AMB01_1000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM AMB01_1000";
            }
            else
            {
                sSql = "SELECT * FROM AMB01_1000 where amb01_1000=@amb01_1000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@amb01_1000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new AMB01_1000
                        {

                            amb01_1000 = comm.sGetInt32(reader["amb01_1000"].ToString()),
                            alm_table = comm.sGetString(reader["alm_table"].ToString()),
                            alm_field = comm.sGetString(reader["alm_field"].ToString()),
                            table_name = comm.sGetString(reader["table_name"].ToString()),
                            field_name = comm.sGetString(reader["field_name"].ToString()),
                            
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得AMB01_1000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List AMB01_1000</returns>
        public List<AMB01_1000> Get_DataList(string pTkCode)
        {
            List<AMB01_1000> list = new List<AMB01_1000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM AMB01_1000";
            }
            else
            {
                sSql = "SELECT * FROM AMB01_1000 where amb01_1000=@amb01_1000";
            }


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@amb01_1000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    AMB01_1000 data = new AMB01_1000();

                    data.amb01_1000 = comm.sGetInt32(reader["amb01_1000"].ToString());
                    data.alm_table = comm.sGetString(reader["alm_table"].ToString());
                    data.alm_field = comm.sGetString(reader["alm_field"].ToString());
                    data.table_name = comm.sGetString(reader["table_name"].ToString());
                    data.field_name = comm.sGetString(reader["field_name"].ToString());


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
        public List<AMB01_1000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_amb01_1000", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<AMB01_1000> list = new List<AMB01_1000>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM AMB01_1000";
            sSql = "SELECT * FROM AMB01_1000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@amb01_1000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    AMB01_1000 data = new AMB01_1000();

                    data.amb01_1000 = comm.sGetInt32(reader["amb01_1000"].ToString());
                    data.alm_table = comm.sGetString(reader["alm_table"].ToString());
                    data.alm_field = comm.sGetString(reader["alm_field"].ToString());
                    data.table_name = comm.sGetString(reader["table_name"].ToString());
                    data.field_name = comm.sGetString(reader["field_name"].ToString());


                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.amb01_1000)) {
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
        public List<AMB01_1000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<AMB01_1000> list = new List<AMB01_1000>();

            string sSql = " SELECT AMB01_1000.*" +
                          " FROM AMB01_1000 " ;

            // 取得資料
            list = comm.Get_ListByQuery<AMB01_1000>(sSql, pWhere, pUsrCode, pPrgCode);

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
                //        data.alm_table = data.amb01_1000 + " - " + comm.sGetString(reader["alm_table"].ToString());
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
        /// 傳入一個AMB01_1000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="AMB01_1000">DTO</param>
        public void InsertData(AMB01_1000 AMB01_1000)
        {
            string sSql = "INSERT INTO " +
                          " AMB01_1000 (   alm_table,   alm_field,  table_name,   field_name )  " +
                          "     VALUES (  @alm_table,  @alm_field, @table_name,  @field_name )  ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, AMB01_1000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@amb01_1000", AMB01_1000.amb01_1000));
                //sqlCommand.Parameters.Add(new SqlParameter("@amb01_1000", AMB01_1000.amb01_1000));
                //sqlCommand.Parameters.Add(new SqlParameter("@alm_table", AMB01_1000.alm_table));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個AMB01_1000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="AMB01_1000">DTO</param>
        public void UpdateData(AMB01_1000 AMB01_1000)
        {
            string sSql = " UPDATE AMB01_1000 " +
                          "    SET alm_table  = @alm_table,    " +
                          "        alm_field  = @alm_field,    " +
                          "        table_name  = @table_name,      " +
                          "        field_name  = @field_name      " +
                          "  WHERE amb01_1000= @amb01_1000 ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, AMB01_1000);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@amb01_1000", AMB01_1000.amb01_1000));
                //sqlCommand.Parameters.Add(new SqlParameter("@amb01_1000", AMB01_1000.amb01_1000));
                //sqlCommand.Parameters.Add(new SqlParameter("@alm_table", AMB01_1000.alm_table));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM AMB01_1000 WHERE amb01_1000 = @amb01_1000;";
            //sSql += " Delete from BDP09_0100 where amb01_1000 = @amb01_1000; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { amb01_1000 = pTkCode });

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@amb01_1000", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }






        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得AMB01_1000角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetAMB01_1000_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("amb01_1000", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("amb01_1000", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("alm_table", System.Type.GetType("System.String"].ToString());

            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM AMB01_1000";
            }
            else
            {
                sSql = "SELECT * FROM AMB01_1000 where amb01_1000='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["amb01_1000"] = dtTmp.Rows[i]["amb01_1000"];
                drow["amb01_1000"] = dtTmp.Rows[i]["amb01_1000"];
                drow["alm_table"] = dtTmp.Rows[i]["alm_table"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}