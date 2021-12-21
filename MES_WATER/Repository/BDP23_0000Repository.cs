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
    public class BDP23_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得BDP23_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO BDP23_0000</returns>
        public BDP23_0000 GetDTO(string pTkCode)
        {
            BDP23_0000 datas = new BDP23_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP23_0000";
            }
            else
            {
                sSql = "SELECT * FROM BDP23_0000 where bdp23_0000=@bdp23_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@bdp23_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new BDP23_0000
                        {
                            bdp23_0000 = comm.sGetInt32(reader["bdp23_0000"].ToString()),
                            usr_code = comm.sGetString(reader["usr_code"].ToString()),
                            theme = comm.sGetString(reader["theme"].ToString()),
                            bull_con = comm.sGetString(reader["bull_con"].ToString()),
                            bull_date = comm.sGetString(reader["bull_date"].ToString()),
                            eff_date = comm.sGetString(reader["eff_date"].ToString()),
                            bull_type = comm.sGetString(reader["bull_type"].ToString()),
                            bull_kind = comm.sGetString(reader["bull_kind"].ToString()),
                            bull_time = comm.sGetString(reader["bull_time"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        /// <summary>
        /// 取得BDP23_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List BDP23_0000</returns>
        public List<BDP23_0000> Get_DataList(string pTkCode)
        {
            List<BDP23_0000> list = new List<BDP23_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP23_0000";
            }
            else
            {
                sSql = "SELECT * FROM BDP23_0000 where bdp23_0000=@bdp23_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@bdp23_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP23_0000 data = new BDP23_0000();

                    data.bdp23_0000 = comm.sGetInt32(reader["bdp23_0000"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                    data.theme = comm.sGetString(reader["theme"].ToString());
                    data.bull_con = comm.sGetString(reader["bull_con"].ToString());
                    data.bull_date = comm.sGetString(reader["bull_date"].ToString());
                    data.eff_date = comm.sGetString(reader["eff_date"].ToString());
                    data.bull_type = comm.sGetString(reader["bull_type"].ToString());
                    data.bull_kind = comm.sGetString(reader["bull_kind"].ToString());
                    data.bull_time = comm.sGetString(reader["bull_time"].ToString());

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
        public List<BDP23_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_bdp23_0000", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<BDP23_0000> list = new List<BDP23_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = " SELECT * FROM BDP23_0000 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@bdp23_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP23_0000 data = new BDP23_0000();

                    data.bdp23_0000 = comm.sGetInt32(reader["bdp23_0000"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                    data.theme = comm.sGetString(reader["theme"].ToString());
                    data.bull_con = comm.sGetString(reader["bull_con"].ToString());
                    data.bull_date = comm.sGetString(reader["bull_date"].ToString());
                    data.eff_date = comm.sGetString(reader["eff_date"].ToString());
                    data.bull_type = comm.sGetString(reader["bull_type"].ToString());
                    data.bull_kind = comm.sGetString(reader["bull_kind"].ToString());
                    data.bull_time = comm.sGetString(reader["bull_time"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.bdp23_0000)) {
                    //    data.can_delete = "N";
                    //    data.can_update = "N";
                    //}

                    list.Add(data);
                }
            }
            return list;
        }

        /// <summary>
        /// 取得查詢資料，結合使用者權限
        /// </summary>
        /// <param name="pUsrCode">使用者代碼</param>
        /// <param name="pPrgCode">功能代碼</param>
        /// <param name="pWhere">JSON查詢字串</param>
        /// <returns></returns>
        public List<BDP23_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<BDP23_0000> list = new List<BDP23_0000>();

            string sSql = " SELECT distinct BDP23_0000.bdp23_0000, BDP23_0000.*, BDP21_0100.field_name as bull_type_name, A.field_name as bull_kind_name " +
                          " FROM BDP23_0000 " +
                          " left join BDP23_0100 on BDP23_0100.bdp23_0000 = BDP23_0000.bdp23_0000 " +
                          " left join BDP21_0100 on BDP21_0100.field_code = BDP23_0000.bull_type and BDP21_0100.code_code = 'bull_type' " +
                          " left join BDP21_0100 as A on A.field_code = BDP23_0000.bull_kind and A.code_code = 'bull_kind' " ;

            // 取得資料
            list = comm.Get_ListByQuery<BDP23_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
                //        data.sup_name = data.bull_date + " - " + comm.sGetString(reader["sup_name"].ToString());
                //        data.usr_code = comm.sGetString(reader["bdp23_0000"].ToString()) + " - " + comm.sGetString(reader["usr_code"].ToString());

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
        /// 傳入一個BDP23_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="BDP23_0000">DTO</param>
        public void InsertData(BDP23_0000 BDP23_0000)
        {
            string sSql = " INSERT INTO " +
                          " BDP23_0000 (  usr_code,  theme,  bull_con,  bull_date,  eff_date,  bull_type,  bull_kind,  bull_time ) " +
                          "     VALUES ( @usr_code, @theme, @bull_con, @bull_date, @eff_date, @bull_type, @bull_kind, @bull_time ) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, BDP23_0000);
            }
        }

        /// <summary>
        /// 傳入一個BDP23_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="BDP23_0000">DTO</param>
        public void UpdateData(BDP23_0000 BDP23_0000)
        {
            string sSql = " UPDATE BDP23_0000                 " +
                          "    SET usr_code    =  @usr_code,  " +
                          "        theme       =  @theme,     " +
                          "        bull_con    =  @bull_con,  " +
                          "        bull_date   =  @bull_date, " +
                          "        eff_date    =  @eff_date,  " +
                          "        bull_type   =  @bull_type, " +
                          "        bull_kind   =  @bull_kind, " +
                          "        bull_time   =  @bull_time  " +
                          "  WHERE bdp23_0000  =  @bdp23_0000 " ;

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, BDP23_0000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@bdp23_0000", BDP23_0000.bdp23_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@bdp23_0000", BDP23_0000.bdp23_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@usr_code", BDP23_0000.usr_code));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM BDP23_0000 WHERE bdp23_0000 = @bdp23_0000; " +
                          "DELETE FROM BDP23_0100 WHERE bdp23_0000 = @bdp23_0000; " ;
            //sSql += " Delete from BDP09_0100 where bdp23_0000 = @bdp23_0000; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { bdp23_0000 = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@bdp23_0000", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        ////暫存DataTable參考
        //// <summary>
        //// 取得BDP23_0000角色的DataTable
        //// </summary>
        //// <param name = "pTkCode" > 有傳鍵值取一筆，鍵值空白取全部</param>
        //// <returns></returns>
        //public DataTable GetBDP23_0000_dt(string pTkCode)
        //{
        //    DataTable dtTmp = new DataTable();
        //    DataTable dtDat = new DataTable();
        //    dtDat.Columns.Add("bdp23_0000", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("bdp23_0000", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("usr_code", System.Type.GetType("System.String"].ToString());

        //    string sSql = "";
        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM BDP23_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM BDP23_0000 where bdp23_0000='" + pTkCode + "'";
        //    }
        //    dtTmp = comm.Get_DataTable(sSql);

        //    int i;
        //    for (i = 1; i < dtTmp.Rows.Count - 1; i++)
        //    {
        //        DataRow drow = dtDat.NewRow();
        //        drow["bdp23_0000"] = dtTmp.Rows[i]["bdp23_0000"];
        //        drow["bdp23_0000"] = dtTmp.Rows[i]["bdp23_0000"];
        //        drow["usr_code"] = dtTmp.Rows[i]["usr_code"];
        //        dtDat.Rows.Add(drow);
        //    }
        //    return dtDat;
        //}
    }
}