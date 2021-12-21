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
    public class WMT12_0100Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得WMT12_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO WMT12_0100</returns>
        public WMT12_0100 GetDTO(string pTkCode)
        {
            WMT12_0100 datas = new WMT12_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM WMT12_0100";
            }
            else
            {
                sSql = "SELECT * FROM WMT12_0100 where wmt12_0100=@wmt12_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@wmt12_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new WMT12_0100
                        {
                            wmt12_0100 = comm.sGetInt32(reader["wmt12_0100"].ToString()),
                            par_code = comm.sGetString(reader["par_code"].ToString()),
                            pro_code = comm.sGetString(reader["pro_code"].ToString()),
                            lot_no = comm.sGetString(reader["lot_no"].ToString()),
                            loc_code = comm.sGetString(reader["loc_code"].ToString()),
                            pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString()),
                            unit_code = comm.sGetString(reader["unit_code"].ToString()),
                            is_ok = comm.sGetString(reader["is_ok"].ToString()),
                            ins_date = comm.sGetString(reader["ins_date"].ToString()),
                            ins_time = comm.sGetString(reader["ins_time"].ToString()),
                            usr_code = comm.sGetString(reader["usr_code"].ToString()),
                            mac_code = comm.sGetString(reader["mac_code"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得WMT12_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List WMT12_0100</returns>
        //public List<WMT12_0100> Get_DataList(string pTkCode)
        //{
        //    List<WMT12_0100> list = new List<WMT12_0100>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM WMT12_0100";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM WMT12_0100 where wmt12_0100=@wmt12_0100";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@wmt12_0100", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            WMT12_0100 data = new WMT12_0100();

        //            data.wmt12_0100 = comm.sGetString(reader["wmt12_0100"].ToString());
        //            data.par_code = comm.sGetString(reader["par_code"].ToString());
        //            data.is_ok = comm.sGetString(reader["is_ok"].ToString());

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
        //public List<WMT12_0100> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_wmt12_0100", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<WMT12_0100> list = new List<WMT12_0100>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    //sSql = "SELECT * FROM WMT12_0100";
        //    sSql = "SELECT * FROM WMT12_0100";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        //sqlCommand.Parameters.Add(new SqlParameter("@wmt12_0100", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            WMT12_0100 data = new WMT12_0100();

        //            data.wmt12_0100 = comm.sGetString(reader["wmt12_0100"].ToString());
        //            data.par_code = comm.sGetString(reader["par_code"].ToString());
        //            data.is_ok = comm.sGetString(reader["is_ok"].ToString());


        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            //資料邏輯刪除、修改
        //            //if (arr_LockGrpCode.Contains(data.wmt12_0100)) {
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
        public List<WMT12_0100> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<WMT12_0100> list = new List<WMT12_0100>();

            string sSql = "SELECT * FROM WMT12_0100";

            // 取得資料
            list = comm.Get_ListByQuery<WMT12_0100>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個WMT12_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="WMT12_0100">DTO</param>
        public void InsertData(WMT12_0100 WMT12_0100)
        {
            string sSql = "INSERT INTO " +
                          " WMT12_0100 (  wmt12_0100,  par_code,  pro_code,  lot_no,  loc_code,  pro_qty, " +
                          "               unit_code,  is_ok,  ins_date,  ins_time,  usr_code,  mac_code ) " +
                          "     VALUES ( @wmt12_0100, @par_code, @pro_code, @lot_no, @loc_code, @pro_qty, " +
                          "              @unit_code, @is_ok, @ins_date, @ins_time, @usr_code, @mac_code ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMT12_0100);
            }
        }

        /// <summary>
        /// 傳入一個WMT12_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="WMT12_0100">DTO</param>
        public void UpdateData(WMT12_0100 WMT12_0100)
        {
            string sSql = " UPDATE WMT12_0100               " +
                          "    SET par_code   = @par_code,  " +
                          "        pro_code   = @pro_code,  " +
                          "        lot_no     = @lot_no,    " +
                          "        loc_code   = @loc_code,  " +
                          "        pro_qty    = @pro_qty,   " +
                          "        unit_code  = @unit_code, " +
                          "        is_ok      = @is_ok,     " +
                          "        ins_date   = @ins_date,  " +
                          "        ins_time   = @ins_time,  " +
                          "        usr_code   = @usr_code,  " +
                          "        mac_code   = @mac_code   " +
                          "  WHERE wmt12_0100 = @wmt12_0100 ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMT12_0100);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM WMT12_0100 WHERE wmt12_0100 = @wmt12_0100 ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { wmt12_0100 = pTkCode });
            }
        }


    }
}