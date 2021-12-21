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
    public class BDP30_0200Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得BDP30_0200資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO BDP30_0200</returns>
        public BDP30_0200 GetDTO(string pTkCode)
        {
            BDP30_0200 datas = new BDP30_0200();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP30_0200";
            }
            else
            {
                sSql = "SELECT * FROM BDP30_0200 where bdp30_0200=@bdp30_0200";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@bdp30_0200", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new BDP30_0200
                        {
                            bdp30_0200 = comm.sGetInt32(reader["bdp30_0200"].ToString()),
                            prg_code = comm.sGetString(reader["prg_code"].ToString()),
                            scr_no = comm.sGetInt32(reader["scr_no"].ToString()),
                            table_code = comm.sGetString(reader["table_code"].ToString()),
                            field_code = comm.sGetString(reader["field_code"].ToString()),
                            field_name = comm.sGetString(reader["field_name"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得BDP30_0200資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List BDP30_0200</returns>
        public List<BDP30_0200> Get_DataList(string pTkCode)
        {
            List<BDP30_0200> list = new List<BDP30_0200>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                //sSql = "SELECT * FROM BDP30_0200";
                sSql = "SELECT * FROM BDP30_0200";
            }
            else
            {
                //sSql = "SELECT * FROM BDP30_0200 where bdp30_0200=@bdp30_0200";
                sSql = "SELECT * FROM BDP30_0200 where bdp30_0200=@bdp30_0200";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@bdp30_0200", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP30_0200 data = new BDP30_0200();

                    data.bdp30_0200 = comm.sGetInt32(reader["bdp30_0200"].ToString());
                    data.prg_code = comm.sGetString(reader["prg_code"].ToString());
                    data.scr_no = comm.sGetInt32(reader["scr_no"].ToString());
                    data.table_code = comm.sGetString(reader["table_code"].ToString());
                    data.field_code = comm.sGetString(reader["field_code"].ToString());
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
        public List<BDP30_0200> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("prg_code", "scr_no", "field_code", "field_name");
            var arr_LockGrpCode = sLockGrpCode.Split(',');
            List<BDP30_0200> list = new List<BDP30_0200>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM BDP30_0200";
            sSql = " SELECT * FROM BDP30_0200";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@30_0200", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP30_0200 data = new BDP30_0200();

                    data.bdp30_0200 = comm.sGetInt32(reader["bdp30_0200"].ToString());
                    data.prg_code = comm.sGetString(reader["prg_code"].ToString());
                    data.scr_no = comm.sGetInt32(reader["scr_no"].ToString());
                    data.table_code = comm.sGetString(reader["table_code"].ToString());
                    data.field_code = comm.sGetString(reader["field_code"].ToString());
                    data.field_name = comm.sGetString(reader["field_name"].ToString());
                    
                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.BDP30_0200)) {
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
        public List<BDP30_0200> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<BDP30_0200> list = new List<BDP30_0200>();

            string sSql = " SELECT BDP30_0200.*, BDP04_0000.prg_name as prg_name " +
                          " FROM BDP30_0200 " +
                          " left join BDP04_0000 on BDP04_0000.prg_code = BDP30_0200.prg_code ";
            // 取得資料
            list = comm.Get_ListByQuery<BDP30_0200>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個BDP30_0200的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="BDP30_0200">DTO</param>
        public void InsertData(BDP30_0200 bdp30_0200)
        {
            string sSql = "INSERT INTO " +
                          " BDP30_0200 ( prg_code,  scr_no,  table_code,  field_code,  field_name )" +
                          "     VALUES ( @prg_code, @scr_no, @table_code, @field_code, @field_name )";
                          
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, bdp30_0200);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@BDP30_0200", BDP30_0200.BDP30_0200));
                //sqlCommand.Parameters.Add(new SqlParameter("@BDP30_0200", BDP30_0200.BDP30_0200));
                //sqlCommand.Parameters.Add(new SqlParameter("@prg_code", BDP30_0200.prg_code));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個BDP30_0200的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="BDP30_0200">DTO</param>
        public void UpdateData(BDP30_0200 bdp30_0200)
        {
            string sSql = " UPDATE BDP30_0200 " +
                          "    SET prg_code    =   @prg_code,      " +
                          "        scr_no      =   @scr_no,        " +
                          "        table_code  =   @table_code,    " +
                          "        field_code  =   @field_code,    " +
                          "        field_name  =   @field_name     " +
                          "  WHERE bdp30_0200  =   @bdp30_0200     " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, bdp30_0200);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@BDP30_0200", BDP30_0200.BDP30_0200));
                //sqlCommand.Parameters.Add(new SqlParameter("@BDP30_0200", BDP30_0200.BDP30_0200));
                //sqlCommand.Parameters.Add(new SqlParameter("@prg_code", BDP30_0200.prg_code));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM BDP30_0200 WHERE bdp30_0200 = @bdp30_0200;";
            //sSql += " Delete from BDP09_0100 where bdp30_0200 = @bdp30_0200; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { bdp30_0200 = pTkCode });

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@BDP30_0200", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }






        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得BDP30_0200角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetBDP30_0200_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("BDP30_0200", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("BDP30_0200", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("prg_code", System.Type.GetType("System.String"].ToString());

            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP30_0200";
            }
            else
            {
                sSql = "SELECT * FROM BDP30_0200 where bdp30_0200='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["BDP30_0200"] = dtTmp.Rows[i]["BDP30_0200"];
                drow["BDP30_0200"] = dtTmp.Rows[i]["BDP30_0200"];
                drow["prg_code"] = dtTmp.Rows[i]["prg_code"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}