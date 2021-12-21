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
    public class WMB17_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得WMB17_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO WMB17_0000</returns>
        public WMB17_0000 GetDTO(string pTkCode)
        {
            WMB17_0000 datas = new WMB17_0000();
            string sSql = @"declare @today datetime
                    set @today = cast(getdate() as varchar(8))
                    SELECT DISTINCT a.pro_code,c.pro_name,lot_no,b.sto_code,a.loc_code,sto_date 
                    ,isnull((
	                    SELECT sum(qty)
	                    FROM V_STO_QTY 
	                    WHERE pro_code= a.pro_code
		                    and loc_code = a.loc_code
	                    GROUP BY pro_code
                    ), 0) AS qty
                    ,DATEDIFF(DAY, sto_date, @today) AS days
                        FROM WMT0200 a 
                        LEFT JOIN WMB02_0000 b ON a.loc_code=b.loc_code
                        LEFT JOIN MEB20_0000 c ON a.pro_code=c.pro_code 
                    ";

            //if (string.IsNullOrEmpty(pTkCode))
            //{
            //    sSql = "SELECT * FROM WMB17_0000";
            //}
            //else
            //{
            //    sSql = "SELECT * FROM WMB17_0000 where sup_code=@sup_code";
            //}

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
                        datas = new WMB17_0000
                        {
                            pro_code = reader["pro_code"].ToString(),
                            pro_name = reader["pro_name"].ToString(),
                            lot_no = reader["lot_no"].ToString(),
                            sto_code = reader["sto_code"].ToString(),
                            loc_code = reader["loc_code"].ToString(),
                            sto_date = DateTime.Parse(reader["sto_date"].ToString()).ToString("yyyy/MM/dd"),
                            qty = comm.sGetInt32(reader["qty"].ToString()),
                            days = comm.sGetInt32(reader["days"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        //#region
        ///// <summary>
        ///// 取得WMB17_0000資料表內容
        ///// </summary>
        ///// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        ///// < returns > List WMB17_0000</returns>
        //public List<WMB17_0000> Get_DataList(string pTkCode)
        //{
        //    List<WMB17_0000> list = new List<WMB17_0000>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM WMB17_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM WMB17_0000 where sto_code=@sto_code";
        //    }


        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@sto_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            WMB17_0000 data = new WMB17_0000();


        //            data.sto_name = comm.sGetString(reader["sto_name"].ToString());
        //            data.loc_code = comm.sGetString(reader["loc_code"].ToString());
        //            data.pro_code = comm.sGetString(reader["pro_code"].ToString());
        //            data.pro_name = comm.sGetString(reader["pro_name"].ToString());
        //            data.lot_no = comm.sGetString(reader["lot_no"].ToString());
        //            data.sto_qty = comm.sGetDouble(reader["sto_qty"].ToString());

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
        //public List<WMB17_0000> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_sup_code", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<WMB17_0000> list = new List<WMB17_0000>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    //sSql = "SELECT * FROM WMB17_0000";
        //    sSql = "SELECT * FROM WMB17_0000";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        //sqlCommand.Parameters.Add(new SqlParameter("@sup_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            WMB17_0000 data = new WMB17_0000();

        //            data.sto_name = comm.sGetString(reader["sto_name"].ToString());
        //            data.loc_code = comm.sGetString(reader["loc_code"].ToString());
        //            data.pro_code = comm.sGetString(reader["pro_code"].ToString());
        //            data.pro_name = comm.sGetString(reader["pro_name"].ToString());
        //            data.lot_no = comm.sGetString(reader["lot_no"].ToString());
        //            data.sto_qty = comm.sGetDouble(reader["sto_qty"].ToString());

        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            //資料邏輯刪除、修改
        //            //if (arr_LockGrpCode.Contains(data.sup_code)) {
        //            //    data.can_delete = "N";
        //            //    data.can_update = "N";
        //            //}

        //            list.Add(data);
        //        }
        //    }
        //    return list;
        //}
        //#endregion

        /// <summary>
        /// 取得查詢資料，結合使用者權限
        /// </summary>
        /// <param name="pUsrCode">使用者代碼</param>
        /// <param name="pPrgCode">功能代碼</param>
        /// <param name="pWhere">JSON查詢字串</param>
        /// <returns></returns>
        public List<WMB17_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<WMB17_0000> list = new List<WMB17_0000>();

            string sSql = @"
                declare @today datetime
                    set @today = cast(getdate() as varchar(8))
                    SELECT DISTINCT a.pro_code,c.pro_name,lot_no,b.sto_code,a.loc_code,sto_date 
                    ,isnull((
	                    SELECT sum(qty)
	                    FROM V_STO_QTY 
	                    WHERE pro_code= a.pro_code
		                    and loc_code = a.loc_code
	                    GROUP BY pro_code
                    ), 0) AS qty
                    ,DATEDIFF(DAY, sto_date, @today) AS days
                        FROM WMT0200 a 
                        LEFT JOIN WMB02_0000 b ON a.loc_code=b.loc_code
                        LEFT JOIN MEB20_0000 c ON a.pro_code=c.pro_code 
            ";
            // 取得資料
            list = comm.Get_ListByQuery<WMB17_0000>(sSql, pWhere, pUsrCode, pPrgCode);

            // 權限設定
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            //string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_mtp_code", "par_name", "par_value");
            //var arr_LockGrpCode = sLockGrpCode.Split(',');

            //for (int i = 0; i < list.Count; i++)
            //{
            //    //檢查授權刪除、修改
            //    list[i].can_delete = sLimitStr.Contains("D") ? "Y" : "N";
            //    list[i].can_update = sLimitStr.Contains("M") ? "Y" : "N";
            //}

            return list;

        }
        /// <summary>
        /// 取得查詢資料，結合使用者權限
        /// </summary>
        /// <param name="pUsrCode">使用者代碼</param>
        /// <param name="pPrgCode">功能代碼</param>
        /// <param name="pWhere">JSON查詢字串</param>
        /// <returns></returns>
        public List<WMB17_0000> Get_DataListByQuery_3(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<WMB17_0000> list = new List<WMB17_0000>();
            string sSql = @"
                declare @today datetime
                    set @today = cast(getdate() as varchar(8))
                    SELECT DISTINCT a.pro_code,c.pro_name,lot_no,b.sto_code,a.loc_code,sto_date 
                    ,isnull((
	                    SELECT sum(qty)
	                    FROM V_STO_QTY 
	                    WHERE pro_code= a.pro_code
		                    and loc_code = a.loc_code
	                    GROUP BY pro_code
                    ), 0) AS qty
                    ,DATEDIFF(DAY, sto_date, @today) AS days
                        FROM WMT0200 a 
                        LEFT JOIN WMB02_0000 b ON a.loc_code=b.loc_code
                        LEFT JOIN MEB20_0000 c ON a.pro_code=c.pro_code 
            ";
            // 取得資料
            list = comm.Get_ListByQuery<WMB17_0000>(sSql, pWhere, pUsrCode, pPrgCode);

            // 權限設定
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            //string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_mtp_code", "par_name", "par_value");
            //var arr_LockGrpCode = sLockGrpCode.Split(',');

            //for (int i = 0; i < list.Count; i++)
            //{
            //    //檢查授權刪除、修改
            //    list[i].can_delete = sLimitStr.Contains("D") ? "Y" : "N";
            //    list[i].can_update = sLimitStr.Contains("M") ? "Y" : "N";
            //}

            return list;

        }

        /// <summary>
        /// 傳入一個WMB17_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="WMB17_0000">DTO</param>
        public void InsertData(WMB17_0000 WMB17_0000)
        {

            string sSql = "INSERT INTO " +
                          " WMB17_0000 (  sto_name,  loc_code,   pro_code,  pro_name,   lot_no, sto_qty )  " +
                          "     VALUES ( @sto_name, @loc_code,  @pro_code, @pro_name,  @lot_no, @sto_qty)  ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMB17_0000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@sup_code", WMB17_0000.sup_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@sup_code", WMB17_0000.sup_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@sup_name", WMB17_0000.sup_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個WMB17_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="WMB17_0000">DTO</param>
        public void UpdateData(WMB17_0000 WMB17_0000)
        {
            string sSql = " UPDATE WMB17_0000 " +
                          "    SET sto_name  = @sto_name,    " +
                          "        loc_code  = @loc_code,    " +
                          "        pro_code  = @pro_code,      " +
                          "        pro_name  = @pro_name      " +
                          "        lot_no  = @lot_no      " +
                          "        sto_qty  = @sto_qty      " +
                          "  WHERE sto_name  = @sto_name, ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMB17_0000);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@sup_code", WMB17_0000.sup_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@sup_code", WMB17_0000.sup_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@sup_name", WMB17_0000.sup_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM WMB17_0000 WHERE sup_code = @sup_code;";
            //sSql += " Delete from BDP09_0100 where sup_code = @sup_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { sup_code = pTkCode });

            }
        }

    }
}