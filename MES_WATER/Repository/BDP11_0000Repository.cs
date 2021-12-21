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
    public class BDP11_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得BDP11_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO BDP11_0000</returns>
        public BDP11_0000 GetDTO(string pTkCode)
        {
            BDP11_0000 datas = new BDP11_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP11_0000";
            }
            else
            {
                sSql = "SELECT * FROM BDP11_0000 where dut_code=@dut_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@dut_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new BDP11_0000
                        {
                            dut_code = comm.sGetString(reader["dut_code"].ToString()),
                            dut_name = comm.sGetString(reader["dut_name"].ToString()),
                            dut_level = comm.sGetString(reader["dut_level"].ToString()),
                            dut_memo = comm.sGetString(reader["dut_memo"].ToString()),
                            is_use = comm.sGetString(reader["is_use"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得BDP11_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List BDP11_0000</returns>
        public List<BDP11_0000> Get_DataList(string pTkCode)
        {
            List<BDP11_0000> list = new List<BDP11_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP11_0000";
            }
            else
            {
                sSql = "SELECT * FROM BDP11_0000 where dut_code=@dut_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@dut_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP11_0000 data = new BDP11_0000();

                    data.dut_code = comm.sGetString(reader["dut_code"].ToString());
                    data.dut_name = comm.sGetString(reader["dut_name"].ToString());
                    data.dut_level = comm.sGetString(reader["dut_level"].ToString());
                    data.dut_memo = comm.sGetString(reader["dut_memo"].ToString());
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
        public List<BDP11_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_dut_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<BDP11_0000> list = new List<BDP11_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM BDP11_0000";
            sSql = "SELECT * FROM BDP11_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@dut_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP11_0000 data = new BDP11_0000();

                    data.dut_code = comm.sGetString(reader["dut_code"].ToString());
                    data.dut_name = comm.sGetString(reader["dut_name"].ToString());
                    data.dut_level = comm.sGetString(reader["dut_level"].ToString());
                    data.dut_memo = comm.sGetString(reader["dut_memo"].ToString());
                    data.is_use = comm.sGetString(reader["is_use"].ToString());


                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.dut_code)) {
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
        public List<BDP11_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<BDP11_0000> list = new List<BDP11_0000>();

            string sSql = "SELECT * FROM BDP11_0000";

            // 取得資料
            list = comm.Get_ListByQuery<BDP11_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個BDP11_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="BDP11_0000">DTO</param>
        public void InsertData(BDP11_0000 BDP11_0000)
        {
            string sSql = "INSERT INTO " +
                          " BDP11_0000 (  dut_code,  dut_name,  dut_level,  dut_memo,  is_use ) " +
                          "     VALUES ( @dut_code, @dut_name, @dut_level, @dut_memo, @is_use ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, BDP11_0000);
            }
        }

        /// <summary>
        /// 傳入一個BDP11_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="BDP11_0000">DTO</param>
        public void UpdateData(BDP11_0000 BDP11_0000)
        {
            string sSql = " UPDATE BDP11_0000 " +
                          "    SET dut_name  =  @dut_name,  " +
                          "        dut_level =  @dut_level, " +
                          "        dut_memo  =  @dut_memo,  " +
                          "        is_use    =  @is_use     " +
                          "  WHERE dut_code  =  @dut_code   " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, BDP11_0000);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@dut_code", BDP11_0000.dut_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@dut_code", BDP11_0000.dut_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@dut_name", BDP11_0000.dut_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM BDP11_0000 WHERE dut_code = @dut_code;";
            //sSql += " Delete from BDP09_0100 where dut_code = @dut_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { dut_code = pTkCode });

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@dut_code", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }






        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得BDP11_0000角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetBDP11_0000_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("dut_code", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("dut_code", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("dut_name", System.Type.GetType("System.String"].ToString());

            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP11_0000";
            }
            else
            {
                sSql = "SELECT * FROM BDP11_0000 where dut_code='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["dut_code"] = dtTmp.Rows[i]["dut_code"];
                drow["dut_code"] = dtTmp.Rows[i]["dut_code"];
                drow["dut_name"] = dtTmp.Rows[i]["dut_name"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}