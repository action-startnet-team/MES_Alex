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
    public class WMB05_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得WMB05_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO WMB05_0000</returns>
        public WMB05_0000 GetDTO(string pTkCode)
        {
            WMB05_0000 datas = new WMB05_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM WMB05_0000";
            }
            else
            {
                sSql = "SELECT * FROM WMB05_0000 where rel_type=@rel_type";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@rel_type", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new WMB05_0000
                        {

                            rel_type = comm.sGetString(reader["rel_type"].ToString()),
                            rel_name = comm.sGetString(reader["rel_name"].ToString()),
                            io_type = comm.sGetString(reader["io_type"].ToString()),
                            i_rel_type = comm.sGetString(reader["i_rel_type"].ToString()),
                            o_rel_type = comm.sGetString(reader["o_rel_type"].ToString()),
                            erp_code = comm.sGetString(reader["erp_code"].ToString()),
                            rel_kind = comm.sGetString(reader["rel_kind"].ToString()),


                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得WMB05_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List WMB05_0000</returns>
        public List<WMB05_0000> Get_DataList(string pTkCode)
        {
            List<WMB05_0000> list = new List<WMB05_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM WMB05_0000";
            }
            else
            {
                sSql = "SELECT * FROM WMB05_0000 where rel_type=@rel_type";
            }
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@rel_type", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    WMB05_0000 data = new WMB05_0000();

                    data.rel_type = comm.sGetString(reader["rel_type"].ToString());
                    data.rel_name = comm.sGetString(reader["rel_name"].ToString());
                    data.io_type = comm.sGetString(reader["io_type"].ToString());
                    data.i_rel_type = comm.sGetString(reader["i_rel_type"].ToString());
                    data.i_rel_type = comm.sGetString(reader["o_rel_type"].ToString());
                    data.i_rel_type = comm.sGetString(reader["erp_code"].ToString());
                    data.i_rel_type = comm.sGetString(reader["rel_kind"].ToString());


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
        public List<WMB05_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_rel_type", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<WMB05_0000> list = new List<WMB05_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM WMB05_0000";
            sSql = "SELECT * FROM WMB05_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@rel_type", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    WMB05_0000 data = new WMB05_0000();

                    data.rel_type = comm.sGetString(reader["rel_type"].ToString());
                    data.rel_name = comm.sGetString(reader["rel_name"].ToString());
                    data.io_type = comm.sGetString(reader["io_type"].ToString());
                    data.i_rel_type = comm.sGetString(reader["i_rel_type"].ToString());
                    data.i_rel_type = comm.sGetString(reader["o_rel_type"].ToString());
                    data.i_rel_type = comm.sGetString(reader["erp_code"].ToString());
                    data.i_rel_type = comm.sGetString(reader["rel_kind"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.rel_type)) {
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
        public List<WMB05_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<WMB05_0000> list = new List<WMB05_0000>();

            string sSql = " SELECT WMB05_0000.*, BDP21_0100.field_name as io_type_name, A.field_name as rel_kind_name " +
                          " FROM WMB05_0000 " +
                          " LEFT JOIN BDP21_0100 on BDP21_0100.field_code = WMB05_0000.io_type AND BDP21_0100.code_code = 'io_type' " +
                          " LEFT JOIN BDP21_0100 as A on A.field_code = WMB05_0000.rel_kind AND A.code_code = 'rel_kind' ";

            // 取得資料
            list = comm.Get_ListByQuery<WMB05_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個WMB05_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="WMB05_0000">DTO</param>
        public void InsertData(WMB05_0000 WMB05_0000)
        {
            string sSql = "INSERT INTO " +
                          " WMB05_0000 (   rel_type,  rel_name,  io_type,  i_rel_type,  o_rel_type,  erp_code,  rel_kind  ) " +
                          "     VALUES (  @rel_type, @rel_name, @io_type, @i_rel_type, @o_rel_type, @erp_code, @rel_kind  ) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMB05_0000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@rel_type", WMB05_0000.rel_type));
                //sqlCommand.Parameters.Add(new SqlParameter("@rel_type", WMB05_0000.rel_type));
                //sqlCommand.Parameters.Add(new SqlParameter("@rel_name", WMB05_0000.rel_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個WMB05_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="WMB05_0000">DTO</param>
        public void UpdateData(WMB05_0000 WMB05_0000)
        {
            string sSql = " UPDATE WMB05_0000                   " +
                          "    SET rel_name     =  @rel_name,   " +
                          "        io_type      =  @io_type,    " +
                          "        i_rel_type   =  @i_rel_type, " +
                          "        o_rel_type   =  @o_rel_type, " +
                          "        erp_code     =  @erp_code,   " +
                          "        rel_kind     =  @rel_kind    " +
                          "  WHERE rel_type     =  @rel_type    " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMB05_0000);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@rel_type", WMB05_0000.rel_type));
                //sqlCommand.Parameters.Add(new SqlParameter("@rel_type", WMB05_0000.rel_type));
                //sqlCommand.Parameters.Add(new SqlParameter("@rel_name", WMB05_0000.rel_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM WMB05_0000 WHERE rel_type = @rel_type;";
            //sSql += " Delete from BDP09_0100 where rel_type = @rel_type; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { rel_type = pTkCode });

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@rel_type", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }






        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得WMB05_0000角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetWMB05_0000_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("rel_type", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("rel_type", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("rel_name", System.Type.GetType("System.String"].ToString());

            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM WMB05_0000";
            }
            else
            {
                sSql = "SELECT * FROM WMB05_0000 where rel_type='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["rel_type"] = dtTmp.Rows[i]["rel_type"];
                drow["rel_type"] = dtTmp.Rows[i]["rel_type"];
                drow["rel_name"] = dtTmp.Rows[i]["rel_name"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}