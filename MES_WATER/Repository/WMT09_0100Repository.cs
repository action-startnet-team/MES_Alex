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
    public class WMT09_0100Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();
        /// <summary>
        /// 取得WMT09_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO WMT09_0100</returns>
        public WMT09_0100 GetDTO(string pTkCode)
        {
            WMT09_0100 datas = new WMT09_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM WMT09_0100";
            }
            else
            {
                sSql = "SELECT * FROM WMT09_0100 WHERE wmt09_0100 = @wmt09_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@wmt09_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new WMT09_0100
                        {
                            wmt09_0100 = comm.sGetInt32(reader.GetOrdinal("wmt09_0100").ToString()),
                            erp_inventory_code = comm.sGetString(reader.GetOrdinal("erp_inventory_code").ToString()),
                            scr_no = comm.sGetString(reader.GetOrdinal("scr_no").ToString()),
                            pro_code = comm.sGetString(reader.GetOrdinal("pro_code").ToString()),
                            lot_no = comm.sGetString(reader.GetOrdinal("lot_no").ToString()),
                            loc_code = comm.sGetString(reader.GetOrdinal("loc_code").ToString()),
                            pro_qty = comm.sGetDecimal(reader.GetOrdinal("pro_qty").ToString()),
                            sto_qty = comm.sGetDecimal(reader.GetOrdinal("sto_qty").ToString()),
                            diff_qty = comm.sGetDecimal(reader.GetOrdinal("diff_qty").ToString()),
                            unit_code = comm.sGetString(reader.GetOrdinal("unit_code").ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 根據erp_inventory_code取得WMT09_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List WMT09_0100</returns>
        //public List<WMT09_0100> Get_DataList(string pTkCode)
        //{
        //    List<WMT09_0100> list = new List<WMT09_0100>();
        //    string sSql = "";
        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM WMT09_0100 order by erp_inventory_code, scr_no ";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM WMT09_0100 WHERE erp_inventory_code=@erp_inventory_code order by scr_no";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@erp_inventory_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            WMT09_0100 data = new WMT09_0100();

        //            data.wmt09_0100 = reader.GetInt32(reader.GetOrdinal("wmt09_0100"));
        //            data.erp_inventory_code = reader.GetString(reader.GetOrdinal("erp_inventory_code"));
        //            data.qtest_item_code = reader.GetString(reader.GetOrdinal("qtest_item_code"));
        //            data.scr_no = reader.GetString(reader.GetOrdinal("scr_no"));
        //            data.pro_code = reader.GetDecimal(reader.GetOrdinal("pro_code"));
        //            data.lot_no = reader.GetDecimal(reader.GetOrdinal("lot_no"));
        //            data.barcode = reader.GetDecimal(reader.GetOrdinal("barcode"));
        //            data.pro_qty = reader.GetString(reader.GetOrdinal("pro_qty"));
        //            data.unit_code = reader.GetString(reader.GetOrdinal("unit_code"));
        //            data.ins_time = reader.GetString(reader.GetOrdinal("ins_time"));
        //            data.usr_code = reader.GetString(reader.GetOrdinal("usr_code"));
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
        //public List<WMT09_0100> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);

        //    List<WMT09_0100> list = new List<WMT09_0100>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    sSql = "SELECT * FROM WMT09_0100 order by ";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            WMT09_0100 data = new WMT09_0100();

        //            data.wmt09_0100 = reader.GetInt32(reader.GetOrdinal("wmt09_0100"));
        //            data.erp_inventory_code = reader.GetString(reader.GetOrdinal("erp_inventory_code"));
        //            data.qtest_item_code = reader.GetString(reader.GetOrdinal("qtest_item_code"));
        //            data.scr_no = reader.GetString(reader.GetOrdinal("scr_no"));
        //            data.pro_code = reader.GetDecimal(reader.GetOrdinal("pro_code"));
        //            data.lot_no = reader.GetDecimal(reader.GetOrdinal("lot_no"));
        //            data.barcode = reader.GetDecimal(reader.GetOrdinal("barcode"));
        //            data.pro_qty = reader.GetString(reader.GetOrdinal("pro_qty"));
        //            data.unit_code = reader.GetString(reader.GetOrdinal("unit_code"));
        //            data.ins_time = reader.GetString(reader.GetOrdinal("ins_time"));
        //            data.usr_code = reader.GetString(reader.GetOrdinal("usr_code"));

        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            //資料邏輯刪除、修改

        //            list.Add(data);
        //        }

        //    }
        //    return list;
        //}

        /// <summary>
        /// 取得查詢資料，結合使用者權限
        /// </summary>
        /// <param name="pUsrCode">使用者代碼</param>
        /// <param name="pPrgCode">功能代碼</param>
        /// <param name="pWhere">JSON查詢字串</param>
        /// <returns></returns>
        public List<Controllers.WMT090AController.WMT09_0100> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<Controllers.WMT090AController.WMT09_0100> list = new List<Controllers.WMT090AController.WMT09_0100>();

            string sSql = " SELECT WMT09_0100.*, WMB06_0000.pro_name, WMB02_0000.loc_name, WMB07_0000.unit_name " +
                          " FROM WMT09_0100 " +
                          " left join WMB06_0000 on WMB06_0000.pro_code = WMT09_0100.pro_code " +
                          " left join WMB02_0000 on WMB02_0000.loc_code = WMT09_0100.loc_code " +
                          " left join WMB07_0000 on WMB07_0000.unit_code = WMT09_0100.unit_code ";
            // 取得資料
            list = comm.Get_ListByQuery<Controllers.WMT090AController.WMT09_0100>(sSql, pWhere, pUsrCode, pPrgCode);

            // 權限設定
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);

            //for (int i = 0; i < list.Count; i++)
            //{
            //    string InsDate = list[i].ins_date;
            //    string UpdDate = list[i].upd_date;

            //    檢查授權刪除、修改
            //    list[i].can_delete = sLimitStr.Contains("D") ? "Y" : "N";
            //    list[i].can_update = sLimitStr.Contains("M") ? "Y" : "N";
            //}

            return list;

        }
        #endregion

        /// <summary>
        /// 根據pTkCode取得WMT09_0100資料表內容，並結合使用者權限
        /// </summary>
        /// <param name="pUsrCode">使用者代號</param>
        /// <param name="pPrgCode">功能代號</param>
        /// <param name="pTkCode">要抓取的條件，field value</param>
        /// <returns></returns>
        public List<WMT09_0100> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<WMT09_0100> list = new List<WMT09_0100>();
            string foreignKey = gmv.GetKey<WMT09_0000>(new WMT09_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT WMT09_0100.*, WMB06_0000.pro_name, WMB02_0000.loc_name, WMB07_0000.unit_name " +
                       " FROM WMT09_0100 " +
                       " left join WMB06_0000 on WMB06_0000.pro_code = WMT09_0100.pro_code " +
                       " left join WMB02_0000 on WMB02_0000.loc_code = WMT09_0100.loc_code " +
                       " left join WMB07_0000 on WMB07_0000.unit_code = WMT09_0100.unit_code " +
                       " where " + foreignKey + "=@" + foreignKey +
                       " order by WMT09_0100.scr_no";
            }
            else
            {
                sSql = " SELECT * FROM WMT09_0100 ";
            }
            //取得該使用者可以看的資料
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter(foreignKey, pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    WMT09_0100 data = new WMT09_0100();
                    data.wmt09_0100 = comm.sGetInt32(reader["wmt09_0100"].ToString());
                    data.erp_inventory_code = comm.sGetString(reader["erp_inventory_code"].ToString());
                    data.scr_no = comm.sGetString(reader["scr_no"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.pro_name = comm.sGetString(reader["pro_name"].ToString());
                    data.lot_no = comm.sGetString(reader["lot_no"].ToString());
                    data.loc_code = comm.sGetString(reader["loc_code"].ToString());
                    data.loc_name = comm.sGetString(reader["loc_name"].ToString());
                    data.pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString());
                    data.sto_qty = comm.sGetDecimal(reader["sto_qty"].ToString());
                    data.diff_qty = comm.sGetDecimal(reader["diff_qty"].ToString());
                    data.unit_code = comm.sGetString(reader["unit_code"].ToString());
                    data.unit_name = comm.sGetString(reader["unit_name"].ToString());

                    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改

                    list.Add(data);
                }

            }
            return list;
        }

        /// <summary>
        /// 傳入一個WMT09_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="WMT09_0100">DTO</param>
        public void InsertData(WMT09_0100 WMT09_0100)
        {
            string sSql = " INSERT INTO " +
                          " WMT09_0100 (  erp_inventory_code,  scr_no,  pro_code,  lot_no,  loc_code, " +
                          "               pro_qty,  sto_qty,  diff_qty,  unit_code ) " +

                          "     VALUES ( @erp_inventory_code, @scr_no, @pro_code, @lot_no, @loc_code, " +
                          "              @pro_qty, @sto_qty, @diff_qty, @unit_code ) ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMT09_0100);
            }
        }

        /// <summary>
        /// 傳入一個WMT09_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="WMT09_0100">DTO</param>
        public void UpdateData(WMT09_0100 WMT09_0100)
        {
            string sSql = " UPDATE WMT09_0100                                " +
                          "    SET erp_inventory_code = @erp_inventory_code, " +
                          "        scr_no             = @scr_no,             " +
                          "        pro_code           = @pro_code,           " +
                          "        lot_no             = @lot_no,             " +
                          "        loc_code           = @loc_code,           " +
                          "        pro_qty            = @pro_qty,            " +
                          "        sto_qty            = @sto_qty,            " +
                          "        diff_qty           = @diff_qty,           " +
                          "        unit_code          = @unit_code           " +
                          "  WHERE wmt09_0100         = @wmt09_0100          ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMT09_0100);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM WMT09_0100 WHERE wmt09_0100 = @wmt09_0100 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { wmt09_0100 = pTkCode });
            }
        }
        /// <summary>
        /// 輸入盤點單號、項次、數量，更新數量
        /// </summary>
        /// <param name="pInvetoryCode"></param>
        /// <param name="pScr_no"></param>
        /// <param name="pProQty"></param>
        public void UpdProQty(string pInvetoryCode, string pScr_no, string pProQty)
        {
            string sSql = "UPDATE WMT09_0100 SET pro_qty='" + pProQty + "'" +
                          " WHERE erp_inventory_code='" + pInvetoryCode + "'" +
                          "   and scr_no='" + pScr_no + "'";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql);
            }
        }
        public string Get_WMS_ProQty(string pInvetoryCode,string pProCode)
        {
            string sSql = "Select sum(pro_qty) from WMT08_0100" +
                         " where  inventory_code ='" + pInvetoryCode + "'"+
                         "   and  pro_code ='"+ pProCode +"'";
            DataTable dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                return dtTmp.Rows[0]["pro_qty"].ToString();
            }
            return "0";
        }
        /// <summary>
        /// 取得ERP盤點單物料
        /// </summary>
        /// <param name="pInvetoryCode"></param>
        /// <returns></returns>
        public DataTable Get_ERP_ProCode(string pInvetoryCode)
        {
            DataTable dtTmp = new DataTable();
            string sSql = "";
            sSql="select pro_code from WMT09_0100 where erp_inventory_code='"+pInvetoryCode+"'";
            dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                return dtTmp;
            }
            else
            {
                DataTable newData = new DataTable();
                return newData;
            }
        }


    }
}