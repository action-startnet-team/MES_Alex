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
    public class BDP08_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得BDP08_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO BDP08_0000</returns>
        public BDP08_0000 GetDTO(string pTkCode)
        {
            BDP08_0000 datas = new BDP08_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP08_0000";
            }
            else
            {
                sSql = "SELECT * FROM BDP08_0000 where usr_code=@usr_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@usr_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new BDP08_0000
                        {

                            usr_code = comm.sGetString(reader["usr_code"].ToString()),
                            usr_name = comm.sGetString(reader["usr_name"].ToString()),
                            usr_pass = comm.sGetString(reader["usr_pass"].ToString()),
                            usr_tel1 = comm.sGetString(reader["usr_tel1"].ToString()),
                            usr_tel2 = comm.sGetString(reader["usr_tel2"].ToString()),
                            usr_mail = comm.sGetString(reader["usr_mail"].ToString()),
                            limit_type = comm.sGetString(reader["limit_type"].ToString()),
                            grp_code = comm.sGetString(reader["grp_code"].ToString()),
                            is_use = comm.sGetString(reader["is_use"].ToString()),
                            dep_code = comm.sGetString(reader["dep_code"].ToString()),
                            dut_code = comm.sGetString(reader["dut_code"].ToString()),
                            token = comm.sGetString(reader["token"].ToString()),

                        };
                    }
                }
            }
            return datas;
        }

        /// <summary>
        /// 取得BDP08_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List BDP08_0000</returns>
        public List<BDP08_0000> Get_DataList(string pTkCode)
        {
            List<BDP08_0000> list = new List<BDP08_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP08_0000";
            }
            else
            {
                sSql = "SELECT * FROM BDP08_0000 where usr_code=@usr_code";
            }


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@usr_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP08_0000 data = new BDP08_0000();

                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                    data.usr_name = comm.sGetString(reader["usr_name"].ToString());
                    data.usr_pass = comm.sGetString(reader["usr_pass"].ToString());
                    data.usr_tel1 = comm.sGetString(reader["usr_tel1"].ToString());
                    data.usr_tel2 = comm.sGetString(reader["usr_tel2"].ToString());
                    data.usr_mail = comm.sGetString(reader["usr_mail"].ToString());
                    data.limit_type = comm.sGetString(reader["limit_type"].ToString());
                    data.grp_code = comm.sGetString(reader["grp_code"].ToString());
                    data.is_use = comm.sGetString(reader["is_use"].ToString());
                    data.dep_code = comm.sGetString(reader["dep_code"].ToString());
                    data.dut_code = comm.sGetString(reader["dut_code"].ToString());
                    data.token = comm.sGetString(reader["token"].ToString());


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
        public List<BDP08_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_usr_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<BDP08_0000> list = new List<BDP08_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM BDP08_0000";
            sSql = "SELECT * FROM BDP08_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@usr_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP08_0000 data = new BDP08_0000();

                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                    data.usr_name = comm.sGetString(reader["usr_name"].ToString());
                    data.usr_pass = comm.sGetString(reader["usr_pass"].ToString());
                    data.usr_tel1 = comm.sGetString(reader["usr_tel1"].ToString());
                    data.usr_tel2 = comm.sGetString(reader["usr_tel2"].ToString());
                    data.usr_mail = comm.sGetString(reader["usr_mail"].ToString());
                    data.limit_type = comm.sGetString(reader["limit_type"].ToString());
                    data.grp_code = comm.sGetString(reader["grp_code"].ToString());
                    data.is_use = comm.sGetString(reader["is_use"].ToString());
                    data.dep_code = comm.sGetString(reader["dep_code"].ToString());
                    data.dut_code = comm.sGetString(reader["dut_code"].ToString());
                    data.token = comm.sGetString(reader["token"].ToString());


                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.usr_code)) {
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
        public List<BDP08_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<BDP08_0000> list = new List<BDP08_0000>();

            string sSql = " SELECT BDP08_0000.*, BDP07_0000.grp_name, BDP10_0000.dep_name, BDP11_0000.dut_name, BDP21_0100.field_name as limit_type_name " +
                          " FROM BDP08_0000 " +
                          " left join BDP07_0000 on BDP07_0000.grp_code = BDP08_0000.grp_code " +
                          " left join BDP10_0000 on BDP10_0000.dep_code = BDP08_0000.dep_code " +
                          " left join BDP11_0000 on BDP11_0000.dut_code = BDP08_0000.dut_code " +
                          " left join BDP21_0100 on BDP21_0100.field_code = BDP08_0000.limit_type and BDP21_0100.code_code = 'limit_type' " ;

            // 取得資料
            list = comm.Get_ListByQuery<BDP08_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
                //        data.usr_name = data.usr_code + " - " + comm.sGetString(reader["usr_name"].ToString());
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
        /// 傳入一個BDP08_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="BDP08_0000">DTO</param>
        public void InsertData(BDP08_0000 BDP08_0000)
        {
            string sSql = "INSERT INTO " +
                          " BDP08_0000 (  usr_code,  usr_name,  usr_pass,  usr_tel1,  usr_tel2, " +
                          "               usr_mail,  limit_type,  grp_code,  is_use,  dep_code, " +
                          "               dut_code, token ) " +
                          "     VALUES ( @usr_code, @usr_name, @usr_pass, @usr_tel1, @usr_tel2, " +
                          "              @usr_mail, @limit_type, @grp_code, @is_use, @dep_code, " +
                          "              @dut_code, @token ) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, BDP08_0000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@usr_code", BDP08_0000.usr_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@usr_code", BDP08_0000.usr_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@usr_name", BDP08_0000.usr_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個BDP08_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="BDP08_0000">DTO</param>
        public void UpdateData(BDP08_0000 BDP08_0000)
        {
            string sSql = " UPDATE BDP08_0000                  " +
                          "    SET usr_name    =  @usr_name,   " +
                          "        usr_pass    =  @usr_pass,   " +
                          "        usr_tel1    =  @usr_tel1,   " +
                          "        usr_tel2    =  @usr_tel2,   " +
                          "        usr_mail    =  @usr_mail,   " +
                          "        limit_type  =  @limit_type, " +
                          "        grp_code    =  @grp_code,   " +
                          "        is_use      =  @is_use,     " +
                          "        dep_code    =  @dep_code,   " +
                          "        dut_code    =  @dut_code    " +
                          "  WHERE usr_code    =  @usr_code    " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, BDP08_0000);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@usr_code", BDP08_0000.usr_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@usr_code", BDP08_0000.usr_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@usr_name", BDP08_0000.usr_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM BDP08_0000 WHERE usr_code = @usr_code;";
            //sSql += " Delete from BDP09_0100 where usr_code = @usr_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { usr_code = pTkCode });

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@usr_code", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }


        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得BDP08_0000角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetBDP08_0000_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("usr_code", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("usr_code", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("usr_name", System.Type.GetType("System.String"].ToString());

            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP08_0000";
            }
            else
            {
                sSql = "SELECT * FROM BDP08_0000 where usr_code='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["usr_code"] = dtTmp.Rows[i]["usr_code"];
                drow["usr_code"] = dtTmp.Rows[i]["usr_code"];
                drow["usr_name"] = dtTmp.Rows[i]["usr_name"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}