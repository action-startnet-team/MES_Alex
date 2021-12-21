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
    public class WMB01_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得WMB01_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO WMB01_0000</returns>
        public WMB01_0000 GetDTO(string pTkCode)
        {
            WMB01_0000 datas = new WMB01_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT  * FROM WMB01_0000";
            }
            else
            {
                sSql = "SELECT * FROM WMB01_0000 where sto_code=@sto_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@sto_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new WMB01_0000
                        {

                            sto_code = comm.sGetString(reader["sto_code"].ToString()),
                            sto_name = comm.sGetString(reader["sto_name"].ToString()),
                            sto_add = comm.sGetString(reader["sto_add"].ToString()),
                            sto_type = comm.sGetString(reader["sto_type"].ToString()),

                        };
                    }
                }
            }
            return datas;
        }

        /// <summary>
        /// 取得WMB01_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List WMB01_0000</returns>
        public List<WMB01_0000> Get_DataList(string pTkCode)
        {
            List<WMB01_0000> list = new List<WMB01_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM WMB01_0000";
            }
            else
            {
                sSql = "SELECT * FROM WMB01_0000 where sto_code=@sto_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@sto_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    WMB01_0000 data = new WMB01_0000();

                    data.sto_code = comm.sGetString(reader["sto_code"].ToString());
                    data.sto_name = comm.sGetString(reader["sto_name"].ToString());
                    data.sto_add = comm.sGetString(reader["sto_add"].ToString());
                    data.sto_type = comm.sGetString(reader["sto_type"].ToString());
                    data.sto_type_name = comm.sGetString(reader["sto_type_name"].ToString());

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
        public List<WMB01_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_sto_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<WMB01_0000> list = new List<WMB01_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = " SELECT * FROM WMB01_0000 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@sto_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    WMB01_0000 data = new WMB01_0000();

                    data.sto_code = comm.sGetString(reader["sto_code"].ToString());
                    data.sto_name = comm.sGetString(reader["sto_name"].ToString());
                    data.sto_add = comm.sGetString(reader["sto_add"].ToString());
                    data.sto_type = comm.sGetString(reader["sto_type"].ToString());
                    data.sto_type_name = comm.sGetString(reader["sto_type_name"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.sto_code)) {
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
        public List<WMB01_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<WMB01_0000> list = new List<WMB01_0000>();/*, B.field_name as loc_type_name*/

            string sSql = " SELECT distinct WMB01_0000.sto_code, WMB01_0000.*, A.field_name as sto_type_name                               " +
                          " FROM WMB01_0000                                                                                                " +
                          " left join WMB02_0000 on WMB02_0000.sto_code = WMB01_0000.sto_code                                              " +
                          " left join BDP21_0100 AS A on A.field_code = WMB01_0000.sto_type AND A.code_code = 'sto_type'                   " +
                          " left join BDP21_0100 as B on B.field_code = WMB02_0000.loc_type and B.code_code = 'loc_type'                   ";

            // 取得資料
            list = comm.Get_ListByQuery<WMB01_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
                //        data.sup_name = data.sto_type + " - " + comm.sGetString(reader["sup_name"].ToString());
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
        /// 傳入一個WMB01_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="WMB01_0000">DTO</param>
        public void InsertData(WMB01_0000 WMB01_0000)
        {
            string sSql = " INSERT INTO " +
                          " WMB01_0000 (  sto_code,  sto_name,  sto_add,  sto_type ) " +
                          "     VALUES ( @sto_code, @sto_name, @sto_add, @sto_type ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMB01_0000);
            }
        }

        /// <summary>
        /// 傳入一個WMB01_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="WMB01_0000">DTO</param>
        public void UpdateData(WMB01_0000 WMB01_0000)
        {
            string sSql = " UPDATE WMB01_0000                 " +
                          "    SET sto_name =  @sto_name,     " +
                          "        sto_add  =  @sto_add,      " +
                          "        sto_type =  @sto_type      " +
                          "  WHERE sto_code =  @sto_code      ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMB01_0000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@sto_code", WMB01_0000.sto_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@sto_code", WMB01_0000.sto_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@sto_name", WMB01_0000.sto_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM WMB01_0000 WHERE sto_code = @sto_code; " +
                          " DELETE FROM WMB02_0000 WHERE sto_code = @sto_code; " ;
            //sSql += " Delete from BDP09_0100 where sto_code = @sto_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { sto_code = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@sto_code", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        ////暫存DataTable參考
        //// <summary>
        //// 取得WMB01_0000角色的DataTable
        //// </summary>
        //// <param name = "pTkCode" > 有傳鍵值取一筆，鍵值空白取全部</param>
        //// <returns></returns>
        //public DataTable GetWMB01_0000_dt(string pTkCode)
        //{
        //    DataTable dtTmp = new DataTable();
        //    DataTable dtDat = new DataTable();
        //    dtDat.Columns.Add("sto_code", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("sto_code", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("sto_name", System.Type.GetType("System.String"].ToString());

        //    string sSql = "";
        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM WMB01_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM WMB01_0000 where sto_code='" + pTkCode + "'";
        //    }
        //    dtTmp = comm.Get_DataTable(sSql);

        //    int i;
        //    for (i = 1; i < dtTmp.Rows.Count - 1; i++)
        //    {
        //        DataRow drow = dtDat.NewRow();
        //        drow["sto_code"] = dtTmp.Rows[i]["sto_code"];
        //        drow["sto_code"] = dtTmp.Rows[i]["sto_code"];
        //        drow["sto_name"] = dtTmp.Rows[i]["sto_name"];
        //        dtDat.Rows.Add(drow);
        //    }
        //    return dtDat;
        //}
    }
}