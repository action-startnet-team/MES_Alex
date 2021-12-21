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
    public class BDP03_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得BDP03_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO BDP03_0000</returns>
        public BDP03_0000 GetDTO(string pTkCode)
        {
            BDP03_0000 datas = new BDP03_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP03_0000";
            }
            else
            {
                sSql = "SELECT * FROM BDP03_0000 where menu_code=@menu_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@menu_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new BDP03_0000
                        {
                            
                            menu_code = reader.GetString(reader.GetOrdinal("menu_code")),
                            menu_name = reader.GetString(reader.GetOrdinal("menu_name")),
                            sys_code = reader.GetString(reader.GetOrdinal("sys_code")),
                            prg_code = reader.GetString(reader.GetOrdinal("prg_code")),
                            menu_type = reader.GetString(reader.GetOrdinal("menu_type")),
                            menu_level = reader.GetString(reader.GetOrdinal("menu_level")),
                            is_use = reader.GetString(reader.GetOrdinal("is_use")),
                            menu_src = reader.GetString(reader.GetOrdinal("menu_src")),


                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得BDP03_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List BDP03_0000</returns>
        public List<BDP03_0000> Get_DataList(string pTkCode)
        {
            List<BDP03_0000> list = new List<BDP03_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP03_0000";
            }
            else
            {
                sSql = "SELECT * FROM BDP03_0000 where menu_code=@menu_code";
            }


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@menu_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP03_0000 data = new BDP03_0000();
                    
                    data.menu_code = reader["menu_code"].ToString();
                    data.menu_name = reader["menu_name"].ToString();
                    data.sys_code = reader["sys_code"].ToString();
                    data.prg_code = reader["prg_code"].ToString();
                    data.menu_type = reader["menu_type"].ToString();
                    data.menu_level = reader["menu_level"].ToString();
                    data.is_use = reader["is_use"].ToString();
                    data.menu_src = reader["menu_src"].ToString();


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
        public List<BDP03_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_menu_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<BDP03_0000> list = new List<BDP03_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM BDP03_0000";
            sSql = "SELECT * FROM BDP03_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@menu_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP03_0000 data = new BDP03_0000();
                    
                    data.menu_code = reader["menu_code"].ToString();
                    data.menu_name = reader["menu_name"].ToString();
                    data.sys_code = reader["sys_code"].ToString();
                    data.prg_code = reader["prg_code"].ToString();
                    data.menu_type = reader["menu_type"].ToString();
                    data.menu_level = reader["menu_level"].ToString();
                    data.is_use = reader["is_use"].ToString();
                    data.menu_src = reader["menu_src"].ToString();


                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.menu_code)) {
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
        public List<BDP03_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<BDP03_0000> list = new List<BDP03_0000>();

            string sSql = " SELECT * FROM BDP03_0000 " ;

            // 取得資料
            list = comm.Get_ListByQuery<BDP03_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
                //        data.sup_name = data.menu_code + " - " + comm.sGetString(reader["sup_name"].ToString());
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
        /// 傳入一個BDP03_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="BDP03_0000">DTO</param>
        public void InsertData(BDP03_0000 BDP03_0000)
        {
            string sSql = "INSERT INTO " +
                          " BDP03_0000 (menu_code, menu_name,  sys_code, prg_code," +
                          "             menu_type, menu_level, is_use,   menu_src)" +
                          "     VALUES (@menu_code, @menu_name,  @sys_code, @prg_code," +
                          "             @menu_type, @menu_level, @is_use,   @menu_src)";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, BDP03_0000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@menu_code", BDP03_0000.menu_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@menu_code", BDP03_0000.menu_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@sup_name", BDP03_0000.sup_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個BDP03_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="BDP03_0000">DTO</param>
        public void UpdateData(BDP03_0000 BDP03_0000)
        {
            string sSql = " UPDATE BDP03_0000                 " +
                          "    SET menu_code  = @menu_code,   " +
                          "        menu_name  = @menu_name,   " +
                          "        sys_code   = @sys_code,    " +
                          "        prg_code   = @prg_code,    " +
                          "        menu_type  = @menu_type,   " +
                          "        menu_level = @menu_level,  " +
                          "        is_use     = @is_use,      " +
                          "        menu_src   = @menu_src     " +
                          "  WHERE menu_code  = @menu_code    " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, BDP03_0000);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@menu_code", BDP03_0000.menu_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@menu_code", BDP03_0000.menu_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@sup_name", BDP03_0000.sup_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM BDP03_0000 WHERE menu_code = @menu_code;";
            //sSql += " Delete from BDP09_0100 where menu_code = @menu_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { menu_code = pTkCode });

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@menu_code", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }






        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得BDP03_0000角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetBDP03_0000_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("menu_code", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("menu_code", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("sup_name", System.Type.GetType("System.String"].ToString());

            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP03_0000";
            }
            else
            {
                sSql = "SELECT * FROM BDP03_0000 where menu_code='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["menu_code"] = dtTmp.Rows[i]["menu_code"];
                drow["menu_code"] = dtTmp.Rows[i]["menu_code"];
                drow["sup_name"] = dtTmp.Rows[i]["sup_name"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}