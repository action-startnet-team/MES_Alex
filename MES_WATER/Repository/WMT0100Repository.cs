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
    public class WMT0100Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得WMT0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO WMT0100</returns>
        public WMT0100 GetDTO(string pTkCode)
        {
            WMT0100 datas = new WMT0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT  * FROM WMT0100";
            }
            else
            {
                sSql = "SELECT * FROM WMT0100 where wmt0100=@wmt0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@wmt0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new WMT0100
                        {
                            wmt0100 = comm.sGetInt32(reader["wmt0100"].ToString()),
                            rel_type = comm.sGetString(reader["rel_type"].ToString()),
                            rel_code = comm.sGetString(reader["rel_code"].ToString()),
                            ins_type = comm.sGetString(reader["ins_type"].ToString()),
                            scr_no = comm.sGetInt32(reader["scr_no"].ToString()),
                            //sto_date = comm.sGetString(reader["sto_date"].ToString()),
                            sto_date = !string.IsNullOrEmpty(reader["sto_date"].ToString()) ? DateTime.Parse(reader["sto_date"].ToString()).ToString("yyyy/MM/dd") : "",
                            //sto_time = !string.IsNullOrEmpty(reader["sto_date"].ToString()) ? DateTime.Parse(reader["sto_date"].ToString()).ToString("HH:mm:ss") : "",
                            pro_code = comm.sGetString(reader["pro_code"].ToString()),
                            pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString()),
                            sto_code = comm.sGetString(reader["sto_code"].ToString()),
                            loc_code = comm.sGetString(reader["loc_code"].ToString()),
                            cus_code = comm.sGetString(reader["cus_code"].ToString()),
                            res_qty = comm.sGetDecimal(reader["res_qty"].ToString()),
                            is_print = comm.sGetString(reader["is_print"].ToString()),
                            is_use = comm.sGetString(reader["is_use"].ToString()),
                            is_error = comm.sGetString(reader["is_error"].ToString()),
                            lot_no = comm.sGetString(reader["lot_no"].ToString()),
                            ins_user = comm.sGetString(reader["ins_user"].ToString()),
                            ins_date = !string.IsNullOrEmpty(reader["ins_date"].ToString()) ? DateTime.Parse(reader["ins_date"].ToString()).ToString("yyyy/MM/dd") : "",
                            ins_time = !string.IsNullOrEmpty(reader["ins_date"].ToString()) ? DateTime.Parse(reader["ins_date"].ToString()).ToString("HH:mm:ss") : "",
                            upd_date = !string.IsNullOrEmpty(reader["upd_date"].ToString()) ? DateTime.Parse(reader["upd_date"].ToString()).ToString("yyyy/MM/dd") : "",
                            upd_time = !string.IsNullOrEmpty(reader["upd_date"].ToString()) ? DateTime.Parse(reader["upd_date"].ToString()).ToString("HH:mm:ss") : "",
                            sup_code = comm.sGetString(reader["sup_code"].ToString()),
                            sup_lot_no = comm.sGetString(reader["sup_lot_no"].ToString()),
                            mft_date = comm.sGetString(reader["mft_date"].ToString()),
                            exp_date = comm.sGetString(reader["exp_date"].ToString()),
                            unit_code = comm.sGetString(reader["unit_code"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得WMT0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List WMT0100</returns>
        //public List<WMT0100> Get_DataList(string pTkCode)
        //{
        //    List<WMT0100> list = new List<WMT0100>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM WMT0100";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM WMT0100 where wmt0100=@wmt0100";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@wmt0100", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            WMT0100 data = new WMT0100();
        //            data.wmt0100 = comm.sGetInt32(reader["wmt0100"].ToString());
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
        //public List<WMT0100> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_wmt0100", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<WMT0100> list = new List<WMT0100>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    sSql = "SELECT * FROM WMT0100";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            WMT0100 data = new WMT0100();
        //            data.wmt0100 = comm.sGetInt32(reader["wmt0100"].ToString());
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
        public List<WMT0100> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<WMT0100> list = new List<WMT0100>();

            string sSql = " SELECT distinct WMT0100.wmt0100, WMT0100.wmt0100, WMT0100.rel_type, WMT0100.rel_code, WMT0100.ins_type, WMT0100.scr_no, WMT0100.pro_code, WMT0100.pro_qty, WMT0100.sto_code, " +
                          " WMT0100.loc_code, WMT0100.cus_code, WMT0100.res_qty, WMT0100.is_print, WMT0100.is_use, WMT0100.is_error, WMT0100.lot_no, WMT0100.ins_user, WMT0100.ins_date, " +
                          " WMT0100.upd_date, WMT0100.sup_code, WMT0100.sup_lot_no, WMT0100.mft_date, WMT0100.exp_date, CONVERT(varchar(10), WMT0100.sto_date, 111) as sto_date, " +
                          " WMB01_0000.sto_name, WMB02_0000.loc_name, BDP21_0100.field_name as ins_type_name, MEB20_0000.pro_name, WMB10_0000.sup_name " +
                          " FROM WMT0100 " +
                          " left join WMB01_0000 on WMB01_0000.sto_code = WMT0100.sto_code " +
                          " left join WMB02_0000 on WMB02_0000.loc_code = WMT0100.loc_code  " +
                          " left join BDP21_0100 on BDP21_0100.field_code = WMT0100.ins_type AND bdp21_0100.code_code = 'ins_type'" +
                          " left join WMT0200 on WMT0200.wmt0100 = WMT0100.wmt0100 " +
                          " left join WMB10_0000 on WMB10_0000.sup_code = WMT0100.sup_code " +
                          //" left join WMB07_0000 on WMB07_0000.unit_code = WMT0100.unit_code " +
                          " left join MEB20_0000 on MEB20_0000.pro_code = WMT0100.pro_code ";
            // 取得資料
            list = comm.Get_ListByQuery<WMT0100>(sSql, pWhere, pUsrCode, pPrgCode);

            // 權限設定
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);

            for (int i = 0; i < list.Count; i++)
            {
                string InsDate = list[i].ins_date;
                string UpdDate = list[i].upd_date;

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


        public int InsertData(WMT0100 WMT0100)
        {

            int identity = 0;

            string sSql = "INSERT INTO " +
                          " WMT0100 (    rel_type,  rel_code,  ins_type,  scr_no,    sto_date,  " +
                          "              pro_code,  pro_qty,   sto_code,  loc_code,  cus_code,  " +
                          "              res_qty,   is_print,  is_use,    is_error,  lot_no,    " +
                          "              ins_user,  ins_date,  upd_date,  sup_code,  sup_lot_no, " +
                          "              mft_date,  exp_date,  unit_code, mo_code ) " +
                          "  VALUES (   @rel_type, @rel_code, @ins_type, @scr_no,   @sto_date, " +
                          "             @pro_code, @pro_qty,  @sto_code, @loc_code, @cus_code, " +
                          "             @res_qty,  @is_print, @is_use,   @is_error, @lot_no,   " +
                          "             @ins_user, @ins_date, @upd_date, @sup_code, @sup_lot_no, " +
                          "             @mft_date, @exp_date, @unit_code, @mo_code ) ";
            sSql += " select scope_identity() as 'scope_identity' ";

            try
            {
                using (SqlConnection con_db = comm.Set_DBConnection())
                {
                    var tmp_list = con_db.Query(sSql, WMT0100).ToList();
                    identity = (int)tmp_list.First().scope_identity;
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return identity;
        }

        /// <summary>
        /// 傳入一個WMT0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="WMT0100">DTO</param>
        public void UpdateData(WMT0100 WMT0100)
        {
            string sSql = " UPDATE WMT0100                    " +
                          "    SET rel_type  =  @rel_type,    " +
                          "        rel_code  =  @rel_code,    " +
                          "        ins_type  =  @ins_type,    " +
                          "        scr_no    =  @scr_no,      " +
                          "        sto_date  =  @sto_date,    " +
                          "        pro_code  =  @pro_code,    " +
                          "        pro_qty   =  @pro_qty,     " +
                          "        sto_code  =  @sto_code,    " +
                          "        loc_code  =  @loc_code,    " +
                          "        cus_code  =  @cus_code,    " +
                          "        res_qty   =  @res_qty,     " +
                          "        is_print  =  @is_print,    " +
                          "        is_use    =  @is_use,      " +
                          "        is_error  =  @is_error,    " +
                          "        lot_no    =  @lot_no,      " +
                          "        ins_user  =  @ins_user,    " +
                          "        upd_date  =  @upd_date,    " +
                          "        sup_code  =  @sup_code,    " +
                          "        sup_lot_no=  @sup_lot_no,  " +
                          "        mft_date  =  @mft_date,    " +
                          "        exp_date  =  @exp_date,    " +
                          "        unit_code =  @unit_code    " +
                          "  WHERE wmt0100   =  @wmt0100      " ;

            WMT0100.upd_date = WMT0100.upd_date + " " + DateTime.Now.ToString("HH:mm:ss");
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMT0100);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM WMT0100 WHERE wmt0100 = @wmt0100 " +
                          " DELETE FROM WMT0200 WHERE wmt0100 = @wmt0100 ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { wmt0100 = pTkCode });
            }
        }


    }
}