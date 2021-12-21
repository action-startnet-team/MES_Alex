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
    public class WMT09_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得WMT09_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO WMT09_0000</returns>
        public WMT09_0000 GetDTO(string pTkCode)
        {
            WMT09_0000 datas = new WMT09_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM WMT09_0000 ";
            }
            else
            {
                sSql = "SELECT * FROM WMT09_0000 where erp_inventory_code=@erp_inventory_code ";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@erp_inventory_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new WMT09_0000
                        {
                            erp_inventory_code = comm.sGetString(reader["erp_inventory_code"].ToString()),
                            erp_inventory_date = comm.sGetString(reader["erp_inventory_date"].ToString()),
                            erp_inventory_type = comm.sGetString(reader["erp_inventory_type"].ToString()),
                            cmemo = comm.sGetString(reader["cmemo"].ToString()),
                            is_ok = comm.sGetString(reader["is_ok"].ToString()),
                            ins_date = comm.sGetString(reader["ins_date"].ToString()),
                            ins_time = comm.sGetString(reader["ins_time"].ToString()),
                            usr_code = comm.sGetString(reader["usr_code"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }
        #region
        ///// <summary>
        ///// 取得WMT09_0000資料表內容
        ///// </summary>
        ///// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        ///// < returns > List WMT09_0000</returns>
        //public List<WMT09_0000> Get_DataList(string pTkCode)
        //{
        //    List<WMT09_0000> list = new List<WMT09_0000>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM WMT09_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM WMT09_0000 where erp_inventory_code=@erp_inventory_code";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@erp_inventory_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            WMT09_0000 data = new WMT09_0000();

        //            data.erp_inventory_code = comm.sGetString(reader["erp_inventory_code"].ToString());
        //            data.erp_inventory_type = comm.sGetString(reader["erp_inventory_type"].ToString());
        //            data.cmemo = comm.sGetString(reader["cmemo"].ToString());
        //            data.pro_code = comm.sGetString(reader["pro_code"].ToString());
        //            data.lot_no = comm.sGetString(reader["lot_no"].ToString());
        //            data.scr_no = comm.sGetInt32(reader["scr_no"].ToString());
        //            data.qsheet_code = comm.sGetString(reader["qsheet_code"].ToString());
        //            data.is_ok = comm.sGetString(reader["is_ok"].ToString());

        //            data.can_delete = "Y";
        //            data.can_update = "Y";
        //            list.Add(data);
        //        }

        //    }
        //    return list;
        //}

        ///// <summary>
        ///// 取得使用者可以編輯的資料，結合商務邏輯權限
        ///// </summary>
        ///// <param name="pUsrCode"></param>
        ///// <param name="pPrgCode"></param>
        ///// <returns></returns>
        //public List<WMT09_0000> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_erp_inventory_code", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<WMT09_0000> list = new List<WMT09_0000>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    sSql = " SELECT * FROM WMT09_0000 ";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            WMT09_0000 data = new WMT09_0000();

        //            data.erp_inventory_code = comm.sGetString(reader["erp_inventory_code"].ToString());
        //            data.erp_inventory_type = comm.sGetString(reader["erp_inventory_type"].ToString());
        //            data.cmemo = comm.sGetString(reader["cmemo"].ToString());
        //            data.pro_code = comm.sGetString(reader["pro_code"].ToString());
        //            data.lot_no = comm.sGetString(reader["lot_no"].ToString());
        //            data.scr_no = comm.sGetInt32(reader["scr_no"].ToString());
        //            data.qsheet_code = comm.sGetString(reader["qsheet_code"].ToString());
        //            data.is_ok = comm.sGetString(reader["is_ok"].ToString());

        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            //資料邏輯刪除、修改
        //            //if (arr_LockGrpCode.Contains(data.erp_inventory_code)) {
        //            //    data.can_delete = "N";
        //            //    data.can_update = "N";
        //            //}

        //            list.Add(data);
        //        }
        //    }
        //    return list;
        //}
        #endregion

        /// <summary>
        /// 取得查詢資料，結合使用者權限
        /// </summary>
        /// <param name="pUsrCode">使用者代碼</param>
        /// <param name="pPrgCode">功能代碼</param>
        /// <param name="pWhere">JSON查詢字串</param>
        /// <returns></returns>
        public List<WMT09_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<WMT09_0000> list = new List<WMT09_0000>();

            string sSql = " SELECT distinct WMT09_0000.erp_inventory_code, WMT09_0000.*, BDP21_0100.field_name as inventory_type_name, BDP08_0000.usr_name " +
                          " FROM WMT09_0000 " +
                          " left join WMT09_0100 on WMT09_0100.erp_inventory_code = WMT09_0000.erp_inventory_code " +
                          " left join BDP21_0100 on BDP21_0100.field_code = WMT09_0000.erp_inventory_type and BDP21_0100.code_code = 'inventory_type' " +
                          " left join BDP08_0000 on BDP08_0000.usr_code = WMT09_0000.usr_code " +
                          " left join WMB06_0000 on WMB06_0000.pro_code = WMT09_0100.pro_code " +
                          " left join WMB02_0000 on WMB02_0000.loc_code = WMT09_0100.loc_code " +
                          " left join WMB07_0000 on WMB07_0000.unit_code = WMT09_0100.unit_code ";

            // 取得資料
            list = comm.Get_ListByQuery<WMT09_0000>(sSql, pWhere, pUsrCode, pPrgCode);

            // 權限設定
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);

            for (int i = 0; i < list.Count; i++)
            {
                //檢查授權刪除、修改
                list[i].can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                list[i].can_update = sLimitStr.Contains("M") ? "Y" : "N";
            }
            return list;
        }

        /// <summary>
        /// 傳入一個WMT09_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="WMT09_0000">DTO</param>
        public void InsertData(WMT09_0000 WMT09_0000)
        {
            string sSql = " INSERT INTO " +
                          " WMT09_0000 (  erp_inventory_code,  erp_inventory_date,  erp_inventory_type, " +
                          "               cmemo,  is_ok,  ins_date,  ins_time,  usr_code ) " +

                          "     VALUES ( @erp_inventory_code, @erp_inventory_date, @erp_inventory_type, " +
                          "              @cmemo, @is_ok, @ins_date, @ins_time, @usr_code ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMT09_0000);
            }
        }

        /// <summary>
        /// 傳入一個WMT09_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="WMT09_0000">DTO</param>
        public void UpdateData(WMT09_0000 WMT09_0000)
        {
            string sSql = " UPDATE WMT09_0000                                  " +
                          "    SET erp_inventory_date  =  @erp_inventory_date, " +
                          "        erp_inventory_type  =  @erp_inventory_type, " +
                          "        cmemo               =  @cmemo,              " +
                          "        is_ok               =  @is_ok,              " +
                          "        ins_date            =  @ins_date,           " +
                          "        ins_time            =  @ins_time,           " +
                          "        usr_code            =  @usr_code            " +
                          "  WHERE erp_inventory_code  =  @erp_inventory_code  ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMT09_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM WMT09_0000 WHERE erp_inventory_code = @erp_inventory_code " +
                          " DELETE FROM WMT09_0100 WHERE erp_inventory_code = @erp_inventory_code ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { erp_inventory_code = pTkCode });
            }
        }

    }
}