using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

using MES_WATER.Models;
using Dapper;
using System.Globalization;

namespace MES_WATER.Repository
{
    public class WMT0400Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得WMT0400資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO WMT0400</returns>
        public WMT0400 GetDTO(string pTkCode)
        {
            WMT0400 datas = new WMT0400();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT  * FROM WMT0400";
            }
            else
            {
                sSql = "SELECT * FROM WMT0400 where WMT0400=@WMT0400";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@WMT0400", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new WMT0400
                        {
                            prepare_code = comm.sGetString(reader["prepare_code"].ToString()),
                            pro_code = comm.sGetString(reader["pro_code"].ToString()),
                            pro_name = comm.sGetString(reader["pro_name"].ToString()),
                            pro_qty = comm.sGetInt32(reader["pro_qty"].ToString()),
                            pro_unit = comm.sGetString(reader["pro_code"].ToString()),
                            is_share = comm.sGetString(reader["is_share"].ToString()),
                            lot_no = comm.sGetString(reader["lot_no"].ToString()),

                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得WMT0400資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List WMT0400</returns>
        //public List<WMT0400> Get_DataList(string pTkCode)
        //{
        //    List<WMT0400> list = new List<WMT0400>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM WMT0400";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM WMT0400 where WMT0400=@WMT0400";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@WMT0400", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            WMT0400 data = new WMT0400();
        //            data.WMT0400 = comm.sGetInt32(reader["WMT0400"].ToString());
        //            data.rel_type = comm.sGetString(reader["rel_type"].ToString());
        //            data.rel_code = comm.sGetString(reader["rel_code"].ToString());
        //            data.ins_type = comm.sGetString(reader["ins_type"].ToString());
        //            data.scr_no = comm.sGetInt32(reader["scr_no"].ToString());
        //            data.sto_date = !string.IsNullOrEmpty(reader["sto_date"].ToString()) ? DateTime.Parse(reader["sto_date"].ToString()).ToString("yyyy/MM/dd") : "";
        //            data.pro_code = comm.sGetString(reader["pro_code"].ToString());
        //            data.pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString());
        //            data.sto_code = comm.sGetString(reader["sto_code"].ToString());
        //            data.loc_code = comm.sGetString(reader["loc_code"].ToString());
        //            data.cus_code = comm.sGetString(reader["cus_code"].ToString());
        //            data.res_qty = comm.sGetDecimal(reader["res_qty"].ToString());
        //            data.is_print = comm.sGetString(reader["is_print"].ToString());
        //            data.is_use = comm.sGetString(reader["is_use"].ToString());
        //            data.is_error = comm.sGetString(reader["is_error"].ToString());
        //            data.lot_no = comm.sGetString(reader["lot_no"].ToString());
        //            data.ins_user = comm.sGetString(reader["ins_user"].ToString());
        //            data.ins_date = !string.IsNullOrEmpty(reader["ins_date"].ToString()) ? DateTime.Parse(reader["ins_date"].ToString()).ToString("yyyy/MM/dd") : "";
        //            data.ins_time = !string.IsNullOrEmpty(reader["ins_date"].ToString()) ? DateTime.Parse(reader["ins_date"].ToString()).ToString("HH:mm:ss") : "";
        //            data.upd_date = !string.IsNullOrEmpty(reader["upd_date"].ToString()) ? DateTime.Parse(reader["upd_date"].ToString()).ToString("yyyy/MM/dd") : "";
        //            data.upd_time = !string.IsNullOrEmpty(reader["upd_date"].ToString()) ? DateTime.Parse(reader["upd_date"].ToString()).ToString("HH:mm:ss") : "";


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
        //public List<WMT0400> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_WMT0400", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<WMT0400> list = new List<WMT0400>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    sSql = "SELECT * FROM WMT0400";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            WMT0400 data = new WMT0400();
        //            data.WMT0400 = comm.sGetInt32(reader["WMT0400"].ToString());
        //            data.rel_type = comm.sGetString(reader["rel_type"].ToString());
        //            data.rel_code = comm.sGetString(reader["rel_code"].ToString());
        //            data.ins_type = comm.sGetString(reader["ins_type"].ToString());
        //            data.scr_no = comm.sGetInt32(reader["scr_no"].ToString());
        //            data.sto_date = !string.IsNullOrEmpty(reader["sto_date"].ToString()) ? DateTime.Parse(reader["sto_date"].ToString()).ToString("yyyy/MM/dd") : "";
        //            data.pro_code = comm.sGetString(reader["pro_code"].ToString());
        //            data.pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString());
        //            data.sto_code = comm.sGetString(reader["sto_code"].ToString());
        //            data.loc_code = comm.sGetString(reader["loc_code"].ToString());
        //            data.cus_code = comm.sGetString(reader["cus_code"].ToString());
        //            data.res_qty = comm.sGetDecimal(reader["res_qty"].ToString());
        //            data.is_print = comm.sGetString(reader["is_print"].ToString());
        //            data.is_use = comm.sGetString(reader["is_use"].ToString());
        //            data.is_error = comm.sGetString(reader["is_error"].ToString());
        //            data.lot_no = comm.sGetString(reader["lot_no"].ToString());
        //            data.ins_user = comm.sGetString(reader["ins_user"].ToString());
        //            data.ins_date = !string.IsNullOrEmpty(reader["ins_date"].ToString()) ? DateTime.Parse(reader["ins_date"].ToString()).ToString("yyyy/MM/dd") : "";
        //            data.ins_time = !string.IsNullOrEmpty(reader["ins_date"].ToString()) ? DateTime.Parse(reader["ins_date"].ToString()).ToString("HH:mm:ss") : "";
        //            data.upd_date = !string.IsNullOrEmpty(reader["upd_date"].ToString()) ? DateTime.Parse(reader["upd_date"].ToString()).ToString("yyyy/MM/dd") : "";
        //            data.upd_time = !string.IsNullOrEmpty(reader["upd_date"].ToString()) ? DateTime.Parse(reader["upd_date"].ToString()).ToString("HH:mm:ss") : "";

        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

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
        public List<WMT0400> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<WMT0400> list = new List<WMT0400>();

            string sSql = " SELECT * , WMT0200.lot_no, MEB20_0000.pro_name  FROM WMT06_0100 " +
                          " left join MEB20_0000 on MEB20_0000.pro_code = WMT06_0100.pro_code " +
                          " left join WMT0200 ON WMT06_0100.pro_code = WMT0200.pro_code  ";
            // 取得資料
            list = comm.Get_ListByQuery<WMT0400>(sSql, pWhere, pUsrCode, pPrgCode);

            // 權限設定
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);

            for (int i = 0; i < list.Count; i++)
            {
                //string InsDate = list[i].ins_date;
                //string UpdDate = list[i].upd_date;

                //list[i].sto_date = !string.IsNullOrEmpty(list[i].sto_date) ? DateTime.Parse(list[i].sto_date).ToString("yyyy/MM/dd") : "";
                //list[i].ins_date = !string.IsNullOrEmpty(InsDate) ? DateTime.Parse(InsDate).ToString("yyyy/MM/dd") : "";
                //list[i].ins_time = !string.IsNullOrEmpty(InsDate) ? DateTime.Parse(InsDate).ToString("HH:mm:ss") : "";
                //list[i].upd_date = !string.IsNullOrEmpty(UpdDate) ? DateTime.Parse(UpdDate).ToString("yyyy/MM/dd") : "";
                //list[i].upd_time = !string.IsNullOrEmpty(UpdDate) ? DateTime.Parse(UpdDate).ToString("HH:mm:ss") : "";

                //檢查授權刪除、修改
                list[i].can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                list[i].can_update = sLimitStr.Contains("M") ? "Y" : "N";
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
        public List<WMT0400> Get_DataListByQuery_OUT(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<WMT0400> list = new List<WMT0400>();

            string sSql = " SELECT * , MEB20_0000.pro_name  FROM WMT06_0100 " +
                          " left join MEB20_0000 on MEB20_0000.pro_code = WMT06_0100.pro_code " ;
            // 取得資料
            list = comm.Get_ListByQuery<WMT0400>(sSql, pWhere, pUsrCode, pPrgCode);

            // 權限設定
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);

            for (int i = 0; i < list.Count; i++)
            {
                //string InsDate = list[i].ins_date;
                //string UpdDate = list[i].upd_date;

                //list[i].sto_date = !string.IsNullOrEmpty(list[i].sto_date) ? DateTime.Parse(list[i].sto_date).ToString("yyyy/MM/dd") : "";
                //list[i].ins_date = !string.IsNullOrEmpty(InsDate) ? DateTime.Parse(InsDate).ToString("yyyy/MM/dd") : "";
                //list[i].ins_time = !string.IsNullOrEmpty(InsDate) ? DateTime.Parse(InsDate).ToString("HH:mm:ss") : "";
                //list[i].upd_date = !string.IsNullOrEmpty(UpdDate) ? DateTime.Parse(UpdDate).ToString("yyyy/MM/dd") : "";
                //list[i].upd_time = !string.IsNullOrEmpty(UpdDate) ? DateTime.Parse(UpdDate).ToString("HH:mm:ss") : "";

                //檢查授權刪除、修改
                list[i].can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                list[i].can_update = sLimitStr.Contains("M") ? "Y" : "N";
            }

            return list;

        }
        //public int InsertData(WMT0400 WMT0400)
        //{

        //    int identity = 0;

        //    string sSql = "INSERT INTO " +
        //                  " WMT0400 (    rel_type,  rel_code,  ins_type,  scr_no,    sto_date,  " +
        //                  "              pro_code,  pro_qty,   sto_code,  loc_code,  cus_code,  " +
        //                  "              res_qty,   is_print,  is_use,    is_error,  lot_no,    " +
        //                  "              ins_user,  ins_date,  upd_date,  sup_code,  sup_lot_no, " +
        //                  "              mft_date,  exp_date ) " +
        //                  "  VALUES (   @rel_type, @rel_code, @ins_type, @scr_no,   @sto_date, " +
        //                  "             @pro_code, @pro_qty,  @sto_code, @loc_code, @cus_code, " +
        //                  "             @res_qty,  @is_print, @is_use,   @is_error, @lot_no,   " +
        //                  "             @ins_user, @ins_date, @upd_date, @sup_code, @sup_lot_no, " +
        //                  "             @mft_date, @exp_date ) ";
        //    sSql += " select scope_identity() as 'scope_identity' ";

        //    try
        //    {
        //        using (SqlConnection con_db = comm.Set_DBConnection())
        //        {
        //            var tmp_list = con_db.Query(sSql, WMT0400).ToList();
        //            identity = (int)tmp_list.First().scope_identity;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string message = ex.Message;
        //    }
        //    return identity;
        //}

        ///// <summary>
        ///// 傳入一個WMT0400的DTO，修改，一次修改一筆
        ///// </summary>
        ///// <param name="WMT0400">DTO</param>
        public void UpdateData(WMT0400 WMT0400,string other_data="0")
        {
            string sSql = " UPDATE WMT0200                    " +
                          "    SET lot_no  =  '1' ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMT0400);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM WMT0400 WHERE WMT0400 = @WMT0400; " +
                          " DELETE FROM WMT0200 WHERE WMT0400 = @WMT0400; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { WMT0400 = pTkCode });
            }
        }


    }
}