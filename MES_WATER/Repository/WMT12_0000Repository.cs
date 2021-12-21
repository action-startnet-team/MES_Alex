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
    public class WMT12_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得WMT12_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO WMT12_0000</returns>
        public WMT12_0000 GetDTO(string pTkCode)
        {
            WMT12_0000 datas = new WMT12_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM WMT12_0000";
            }
            else
            {
                sSql = "SELECT * FROM WMT12_0000 where par_code=@par_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@par_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new WMT12_0000
                        {
                            par_code = comm.sGetString(reader["par_code"].ToString()),
                            par_date = comm.sGetString(reader["par_date"].ToString()),
                            par_type = comm.sGetString(reader["par_type"].ToString()),
                            mo_code = comm.sGetString(reader["mo_code"].ToString()),
                            pro_lot = comm.sGetString(reader["pro_lot"].ToString()),
                            pro_code = comm.sGetString(reader["pro_code"].ToString()),
                            scr_no = comm.sGetString(reader["scr_no"].ToString()),
                            pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString()),
                            par_qty = comm.sGetDecimal(reader["par_qty"].ToString()),
                            res_qty = comm.sGetDecimal(reader["res_qty"].ToString()),
                            unit_code = comm.sGetString(reader["unit_code"].ToString()),
                            cmemo = comm.sGetString(reader["cmemo"].ToString()),
                            is_par = comm.sGetString(reader["is_par"].ToString()),
                            is_sto_in = comm.sGetString(reader["is_sto_in"].ToString()),
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
        /// <summary>
        /// 取得WMT12_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List WMT12_0000</returns>
        //public List<WMT12_0000> Get_DataList(string pTkCode)
        //{
        //    List<WMT12_0000> list = new List<WMT12_0000>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM WMT12_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM WMT12_0000 where par_code=@par_code";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@par_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            WMT12_0000 data = new WMT12_0000();

        //            data.par_code = comm.sGetString(reader["par_code"].ToString());
        //            data.par_date = comm.sGetString(reader["par_date"].ToString());
        //            data.par_type = comm.sGetString(reader["par_type"].ToString());

        //            data.can_delete = "Y";
        //            data.can_update = "Y";
        //            list.Add(data);
        //        }

        //    }
        //    return list;
        //}

        /// <summary>
        /// 取得使用者可以編輯的資料，結合商務邏輯權限
        /// </summary>
        /// <param name="pUsrCode"></param>
        /// <param name="pPrgCode"></param>
        /// <returns></returns>
        //public List<WMT12_0000> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_par_code", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<WMT12_0000> list = new List<WMT12_0000>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    //sSql = "SELECT * FROM WMT12_0000";
        //    sSql = "SELECT * FROM WMT12_0000";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        //sqlCommand.Parameters.Add(new SqlParameter("@par_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            WMT12_0000 data = new WMT12_0000();

        //            data.par_code = comm.sGetString(reader["par_code"].ToString());
        //            data.par_date = comm.sGetString(reader["par_date"].ToString());
        //            data.par_type = comm.sGetString(reader["par_type"].ToString());


        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            //資料邏輯刪除、修改
        //            //if (arr_LockGrpCode.Contains(data.par_code)) {
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
        public List<WMT12_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<WMT12_0000> list = new List<WMT12_0000>();

            string sSql = "SELECT * FROM WMT12_0000";

            // 取得資料
            list = comm.Get_ListByQuery<WMT12_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個WMT12_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="WMT12_0000">DTO</param>
        public void InsertData(WMT12_0000 WMT12_0000)
        {
            string sSql = "INSERT INTO " +
                          " WMT12_0000 (  par_code,  par_date,  par_type,  mo_code,  pro_lot,  pro_code, " +
                          "               scr_no,  pro_qty,  par_qty,  res_qty,  unit_code,  cmemo,  is_par, " +
                          "               is_sto_in,  ins_date,  ins_time,  usr_code ) " +
                          "     VALUES ( @par_code, @par_date, @par_type, @mo_code, @pro_lot, @pro_code, " +
                          "              @scr_no, @pro_qty, @par_qty, @res_qty, @unit_code, @cmemo, @is_par, " +
                          "              @is_sto_in, @ins_date, @ins_time, @usr_code ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMT12_0000);
            }
        }

        /// <summary>
        /// 傳入一個WMT12_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="WMT12_0000">DTO</param>
        public void UpdateData(WMT12_0000 WMT12_0000)
        {
            string sSql = " UPDATE WMT12_0000                " +
                          "    SET par_date   = @par_date,   " +
                          "        par_type   = @par_type,   " +
                          "        mo_code    = @mo_code,    " +
                          "        pro_lot    = @pro_lot,    " +
                          "        pro_code   = @pro_code,   " +
                          "        scr_no     = @scr_no,     " +
                          "        pro_qty    = @pro_qty,    " +
                          "        par_qty    = @par_qty,    " +
                          "        res_qty    = @res_qty,    " +
                          "        unit_code  = @unit_code,  " +
                          "        cmemo      = @cmemo,      " +
                          "        is_par     = @is_par,     " +
                          "        is_sto_in  = @is_sto_in,  " +
                          "        ins_date   = @ins_date,   " +
                          "        ins_time   = @ins_time,   " +
                          "        usr_code   = @usr_code    " +
                          "  WHERE par_code   = @par_code    ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMT12_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM WMT12_0000 WHERE par_code = @par_code ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { par_code = pTkCode });
            }
        }


    }
}