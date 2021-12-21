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
    public class BDP32_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得BDP32_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO BDP32_0000</returns>
        public BDP32_0000 GetDTO(string pTkCode)
        {
            BDP32_0000 datas = new BDP32_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP32_0000";
            }
            else
            {
                sSql = "SELECT * FROM BDP32_0000 where bdp32_0000=@bdp32_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@bdp32_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new BDP32_0000
                        {

                            bdp32_0000 = comm.sGetInt32(reader["bdp32_0000"].ToString()),
                            prg_code = comm.sGetString(reader["prg_code"].ToString()),
                            scr_no = comm.sGetInt32(reader["scr_no"].ToString()),
                            field_code = comm.sGetString(reader["field_code"].ToString()),
                            field_name = comm.sGetString(reader["field_name"].ToString()),
                            field_type = comm.sGetString(reader["field_type"].ToString()),
                            ctr_type = comm.sGetString(reader["ctr_type"].ToString()),
                            data_source = comm.sGetString(reader["data_source"].ToString()),
                            default_value = comm.sGetString(reader["default_value"].ToString()),

                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得BDP32_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List BDP32_0000</returns>
        public List<BDP32_0000> Get_DataList(string pTkCode)
        {
            List<BDP32_0000> list = new List<BDP32_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP32_0000";
            }
            else
            {
                sSql = "SELECT * FROM BDP32_0000 where bdp32_0000=@bdp32_0000";
            }
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@bdp32_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP32_0000 data = new BDP32_0000();

                    data.bdp32_0000 = comm.sGetInt32(reader["bdp32_0000"].ToString());
                    data.prg_code = comm.sGetString(reader["prg_code"].ToString());
                    data.scr_no = comm.sGetInt32(reader["scr_no"].ToString());
                    data.field_code = comm.sGetString(reader["field_code"].ToString());
                    data.field_name = comm.sGetString(reader["field_name"].ToString());
                    data.field_type = comm.sGetString(reader["field_type"].ToString());
                    data.ctr_type = comm.sGetString(reader["ctr_type"].ToString());
                    data.data_source = comm.sGetString(reader["data_source"].ToString());
                    data.default_value = comm.sGetString(reader["default_value"].ToString());


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
        public List<BDP32_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_bdp32_0000", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<BDP32_0000> list = new List<BDP32_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM BDP32_0000";
            sSql = "SELECT * FROM BDP32_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@bdp32_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP32_0000 data = new BDP32_0000();

                    data.bdp32_0000 = comm.sGetInt32(reader["bdp32_0000"].ToString());
                    data.prg_code = comm.sGetString(reader["prg_code"].ToString());
                    data.scr_no = comm.sGetInt32(reader["scr_no"].ToString());
                    data.field_code = comm.sGetString(reader["field_code"].ToString());
                    data.field_name = comm.sGetString(reader["field_name"].ToString());
                    data.field_type = comm.sGetString(reader["field_type"].ToString());
                    data.ctr_type = comm.sGetString(reader["ctr_type"].ToString());
                    data.data_source = comm.sGetString(reader["data_source"].ToString());
                    data.default_value = comm.sGetString(reader["default_value"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.bdp32_0000)) {
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
        public List<BDP32_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<BDP32_0000> list = new List<BDP32_0000>();

            string sSql = "    SELECT BDP32_0000.*, BDP04_0000.prg_name as prg_name, A.field_name as field_type_name, B.field_name as ctr_type_name " +
                          "      FROM BDP32_0000 " +
                          " left join BDP04_0000 on BDP04_0000.prg_code = BDP32_0000.prg_code " +
                          " left join BDP21_0100 as A on A.field_code = BDP32_0000.field_type and A.code_code = 'field_type' " +
                          " left join BDP21_0100 as B on B.field_code = BDP32_0000.ctr_type and B.code_code = 'ctr_type' ";

            // 取得資料
            list = comm.Get_ListByQuery<BDP32_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個BDP32_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="BDP32_0000">DTO</param>
        public void InsertData(BDP32_0000 BDP32_0000)
        {
            string sSql = "INSERT INTO " +
                          " BDP32_0000 (  prg_code,  scr_no,  field_code,  field_name,  field_type,  ctr_type,  data_source,  default_value  )" +
                          "     VALUES ( @prg_code, @scr_no, @field_code, @field_name, @field_type, @ctr_type, @data_source, @default_value  )" ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, BDP32_0000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@bdp32_0000", BDP32_0000.bdp32_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@bdp32_0000", BDP32_0000.bdp32_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@prg_code", BDP32_0000.prg_code));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個BDP32_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="BDP32_0000">DTO</param>
        public void UpdateData(BDP32_0000 BDP32_0000)
        {
            string sSql = " UPDATE BDP32_0000 " +
                          "    SET prg_code      =   @prg_code,    " +
                          "        scr_no        =   @scr_no,      " +
                          "        field_code    =   @field_code,  " +
                          "        field_name    =   @field_name,  " +
                          "        field_type    =   @field_type,  " +
                          "        ctr_type      =   @ctr_type,    " +
                          "        data_source   =   @data_source, " +
                          "        default_value =   @default_value     " +
                          "  WHERE bdp32_0000    =   @bdp32_0000   " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, BDP32_0000);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@bdp32_0000", BDP32_0000.bdp32_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@bdp32_0000", BDP32_0000.bdp32_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@prg_code", BDP32_0000.prg_code));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM BDP32_0000 WHERE bdp32_0000 = @bdp32_0000;";
            //sSql += " Delete from BDP09_0100 where bdp32_0000 = @bdp32_0000; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { bdp32_0000 = pTkCode });

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@bdp32_0000", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }






        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得BDP32_0000角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetBDP32_0000_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("bdp32_0000", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("bdp32_0000", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("prg_code", System.Type.GetType("System.String"].ToString());

            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP32_0000";
            }
            else
            {
                sSql = "SELECT * FROM BDP32_0000 where bdp32_0000='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["bdp32_0000"] = dtTmp.Rows[i]["bdp32_0000"];
                drow["bdp32_0000"] = dtTmp.Rows[i]["bdp32_0000"];
                drow["prg_code"] = dtTmp.Rows[i]["prg_code"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}