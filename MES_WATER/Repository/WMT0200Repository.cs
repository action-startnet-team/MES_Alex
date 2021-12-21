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
    public class WMT0200Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得WMT0200資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO WMT0200</returns>
        public WMT0200 GetDTO(string pTkCode)
        {
            WMT0200 datas = new WMT0200();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM WMT0200";
            }
            else
            {
                sSql = "SELECT * FROM WMT0200 where wmt0200=@wmt0200";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@wmt0200", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        WMT0200 data = new WMT0200
                        {
                            wmt0200 = comm.sGetString(reader["wmt0200"].ToString()),
                            lot_no = comm.sGetString(reader["lot_no"].ToString()),
                            rel_type = comm.sGetString(reader["rel_type"].ToString()),
                            rel_code = comm.sGetString(reader["rel_code"].ToString()),
                            ins_type = comm.sGetString(reader["ins_type"].ToString()),
                            scr_no = comm.sGetInt32(reader["scr_no"].ToString()),
                            sto_date = comm.sGetString(DateTime.Parse(reader["sto_date"].ToString()).ToString("yyyy/MM/dd")),
                            sto_time = comm.sGetString(DateTime.Parse(reader["sto_date"].ToString()).ToString("HH:mm:ss")),
                            pro_code = comm.sGetString(reader["pro_code"].ToString()),
                            pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString()),
                            sto_code = comm.sGetString(reader["sto_code"].ToString()),
                            loc_code = comm.sGetString(reader["loc_code"].ToString()),
                            //cus_code = comm.sGetString(reader["cus_code"].ToString()),
                            //wmt0100 = comm.sGetInt32(reader["wmt0100"].ToString()),
                            ins_user = comm.sGetString(reader["ins_user"].ToString()),
                            ins_date = DateTime.Parse(reader["ins_date"].ToString()).ToString("yyyy/MM/dd"),
                            ins_time = DateTime.Parse(reader["ins_date"].ToString()).ToString("HH:mm:ss"),
                            container = comm.sGetString(reader["container"].ToString()),
                            barcode = comm.sGetString(reader["barcode"].ToString()),
                            //sor_no = comm.sGetString(reader["sor_no"].ToString()),
                            //tra_code = comm.sGetString(reader["tra_code"].ToString()),
                            identifier = comm.sGetString(reader["identifier"].ToString()),
                            sup_code = comm.sGetString(reader["sup_code"].ToString()),
                            sup_lot_no = comm.sGetString(reader["sup_lot_no"].ToString()),
                            mft_date = comm.sGetString(reader["mft_date"].ToString()),
                            exp_date = comm.sGetString(reader["exp_date"].ToString()),
                        };
                        return data;
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得WMT0200資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List WMT0200</returns>
        //public List<WMT0200> Get_DataList(string pTkCode)
        //{
        //    List<WMT0200> list = new List<WMT0200>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM WMT0200";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM WMT0200 where wmt0200=@wmt0200";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@wmt0200", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            WMT0200 data = new WMT0200();

        //            data.wmt0200 = comm.sGetString(reader["wmt0200"].ToString());
        //            data.lot_no = comm.sGetString(reader["lot_no"].ToString());
        //            data.rel_type = comm.sGetString(reader["rel_type"].ToString());
        //            data.rel_code = comm.sGetString(reader["rel_code"].ToString());
        //            data.ins_type = comm.sGetString(reader["ins_type"].ToString());
        //            data.scr_no = comm.sGetInt32(reader["scr_no"].ToString());
        //            data.sto_date = comm.sGetDateTime(reader["sto_date"].ToString());
        //            data.pro_code = comm.sGetString(reader["pro_code"].ToString());
        //            data.pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString());
        //            data.sto_code = comm.sGetString(reader["sto_code"].ToString());
        //            data.loc_code = comm.sGetString(reader["loc_code"].ToString());
        //            //data.cus_code = comm.sGetString(reader["cus_code"].ToString());
        //            //data.wmt0100 = comm.sGetInt32(reader["wmt0100"].ToString());
        //            data.ins_user = comm.sGetString(reader["ins_user"].ToString());
        //            data.ins_date = comm.sGetString(reader["ins_date"].ToString());
        //            data.container = comm.sGetString(reader["container"].ToString());
        //            data.barcode = comm.sGetString(reader["barcode"].ToString());
        //            //data.sor_no = comm.sGetString(reader["sor_no"].ToString());
        //            //data.tra_code = comm.sGetString(reader["tra_code"].ToString());
        //            data.identifier = comm.sGetString(reader["identifier"].ToString());


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
        //public List<WMT0200> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_wmt0200", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<WMT0200> list = new List<WMT0200>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    sSql = "SELECT * FROM WMT0200";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            WMT0200 data = new WMT0200();

        //            data.wmt0200 = comm.sGetString(reader["wmt0200"].ToString());
        //            data.lot_no = comm.sGetString(reader["lot_no"].ToString());
        //            data.rel_type = comm.sGetString(reader["rel_type"].ToString());
        //            data.rel_code = comm.sGetString(reader["rel_code"].ToString());
        //            data.ins_type = comm.sGetString(reader["ins_type"].ToString());
        //            data.scr_no = comm.sGetInt32(reader["scr_no"].ToString());
        //            data.sto_date = comm.sGetDateTime(reader["sto_date"].ToString());
        //            data.pro_code = comm.sGetString(reader["pro_code"].ToString());
        //            data.pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString());
        //            data.sto_code = comm.sGetString(reader["sto_code"].ToString());
        //            data.loc_code = comm.sGetString(reader["loc_code"].ToString());
        //            //data.cus_code = comm.sGetString(reader["cus_code"].ToString());
        //            //data.wmt0100 = comm.sGetInt32(reader["wmt0100"].ToString());
        //            data.ins_user = comm.sGetString(reader["ins_user"].ToString());
        //            data.ins_date = comm.sGetString(reader["ins_date"].ToString());
        //            data.container = comm.sGetString(reader["container"].ToString());
        //            data.barcode = comm.sGetString(reader["barcode"].ToString());
        //            //data.sor_no = comm.sGetString(reader["sor_no"].ToString());
        //            //data.tra_code =comm.sGetString(reader["tra_code"].ToString());
        //            data.identifier = comm.sGetString(reader["identifier"].ToString());

        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            list.Add(data);
        //        }
        //    }
        //    return list;
        //}
        #endregion

        public List<WMT0200> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<WMT0200> list = new List<WMT0200>();
            string foreignKey = gmv.GetKey<WMT0100>(new WMT0100());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT WMT0200.*, A.field_name as ins_type_name, WMB06_0000.pro_name, WMB02_0000.loc_name, BDP08_0000.usr_name, WMB10_0000.sup_name " +
                       " FROM WMT0200 " +
                       " left join BDP21_0100 as A on A.field_code = WMT0200.ins_type and code_code = 'ins_type' " +
                       " left join WMB06_0000 on WMB06_0000.pro_code = WMT0200.pro_code  " +
                       " left join WMB02_0000 on WMB02_0000.loc_code = WMT0200.loc_code  " +
                       " left join BDP08_0000 on BDP08_0000.usr_code = WMT0200.ins_user  " +
                       " left join WMB10_0000 on WMB10_0000.sup_code = WMT0200.sup_code " +
                       " where WMT0200." + foreignKey + "=@" + foreignKey;
            }
            else
            {
                sSql = "SELECT * FROM WMT0200";
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
                    WMT0200 data = new WMT0200();

                    data.wmt0200 = comm.sGetString(reader["wmt0200"].ToString());
                    data.lot_no = comm.sGetString(reader["lot_no"].ToString());
                    data.rel_type = comm.sGetString(reader["rel_type"].ToString());
                    data.rel_code = comm.sGetString(reader["rel_code"].ToString());
                    data.ins_type = comm.sGetString(reader["ins_type"].ToString());
                    data.ins_type_name = comm.sGetString(reader["ins_type_name"].ToString());
                    data.scr_no = comm.sGetInt32(reader["scr_no"].ToString());
                    data.sto_date = !string.IsNullOrEmpty(reader["sto_date"].ToString()) ? DateTime.Parse(reader["sto_date"].ToString()).ToString("yyyy/MM/dd HH:mm:ss") : "";
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.pro_name = comm.sGetString(reader["pro_name"].ToString());
                    data.pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString());
                    data.sto_code = comm.sGetString(reader["sto_code"].ToString());
                    data.loc_code = comm.sGetString(reader["loc_code"].ToString());
                    data.loc_name = comm.sGetString(reader["loc_name"].ToString());
                    //data.cus_code = comm.sGetString(reader["cus_code"].ToString());
                    //data.wmt0100 = comm.sGetInt32(reader["wmt0100"].ToString());
                    data.ins_user = comm.sGetString(reader["ins_user"].ToString());
                    data.usr_name = comm.sGetString(reader["usr_name"].ToString());
                    data.ins_date = !string.IsNullOrEmpty(reader["ins_date"].ToString()) ? DateTime.Parse(reader["ins_date"].ToString()).ToString("yyyy/MM/dd HH:mm:ss") : "";
                    //data.ins_time = !string.IsNullOrEmpty(reader["ins_date"].ToString()) ? DateTime.Parse(reader["ins_date"].ToString()).ToString("HH:mm:ss") : "";
                    data.container = comm.sGetString(reader["container"].ToString());
                    data.barcode = comm.sGetString(reader["barcode"].ToString());
                    //data.sor_no = comm.sGetString(reader["sor_no"].ToString());
                    //data.tra_code = comm.sGetString(reader["tra_code"].ToString());
                    data.identifier = comm.sGetString(reader["identifier"].ToString());
                    data.sup_code = comm.sGetString(reader["sup_code"].ToString());
                    data.sup_lot_no = comm.sGetString(reader["sup_lot_no"].ToString());
                    data.mft_date = comm.sGetString(reader["mft_date"].ToString());
                    data.exp_date = comm.sGetString(reader["exp_date"].ToString());

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
        /// 取得查詢資料，結合使用者權限
        /// </summary>
        /// <param name="pUsrCode">使用者代碼</param>
        /// <param name="pPrgCode">功能代碼</param>
        /// <param name="pWhere">JSON查詢字串</param>
        /// <returns></returns>
        public List<WMT0200> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<WMT0200> list = new List<WMT0200>();

            string sSql = " SELECT *, BDP21_0100.field_name as ins_type_name, WMB01_0000.sto_name, WMB02_0000.loc_name, WMB03_0000.pallet_name as container_name, WMB10_0000.sup_name,WMT0200.scr_no  " +
                          " FROM WMT0200 " +
                          " left join BDP21_0100 on BDP21_0100.field_code = WMT0200.ins_type and BDP21_0100.code_code = 'ins_type' " +
                          " left join WMB01_0000 on WMB01_0000.sto_code = WMT0200.sto_code " +
                          " left join WMB02_0000 on WMB02_0000.loc_code = WMT0200.loc_code " +
                          " left join WMB03_0000 on WMB03_0000.pallet_code = WMT0200.container " +
                          " left join WMB10_0000 on WMB10_0000.sup_code = WMT0200.sup_code " ;

            // 取得資料
            list = comm.Get_ListByQuery<WMT0200>(sSql, pWhere, pUsrCode, pPrgCode);

            // 權限設定
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);

            for (int i = 0; i < list.Count; i++)
            {
                //檢查授權刪除、修改
                //list[i].sto_date = DateTime.Parse(list[i].sto_date).ToString("yyyy/MM/dd");
                //list[i].ins_date = DateTime.Parse(list[i].ins_date).ToString("yyyy/MM/dd");
                //list[i].ins_time = DateTime.Parse(list[i].ins_date).ToString("HH:mm:ss");
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
        public List<WMT0200> Get_DataListByQuery2(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<WMT0200> list = new List<WMT0200>();

            string sSql = " SELECT *, BDP21_0100.field_name as ins_type_name, WMB01_0000.sto_name, WMB02_0000.loc_name, WMB03_0000.pallet_name as container_name, WMB10_0000.sup_name " +
                          " FROM WMT0200 " +
                          " left join BDP21_0100 on BDP21_0100.field_code = WMT0200.ins_type and BDP21_0100.code_code = 'ins_type' " +
                          " left join WMB01_0000 on WMB01_0000.sto_code = WMT0200.sto_code " +
                          " left join WMB02_0000 on WMB02_0000.loc_code = WMT0200.loc_code " +
                          " left join WMB03_0000 on WMB03_0000.pallet_code = WMT0200.container " +
                          " left join WMB10_0000 on WMB10_0000.sup_code = WMT0200.sup_code " +
                          " where ins_type = 'I' " +
                          " and WMT0200.pro_code in (select pro_code from QMB03_0200) " +
                          " and WMT0200.wmt0200 not in (select wmt0200 from QMT04_0000 where is_rec <> 'P')";

            // 取得資料
            list = comm.Get_ListByQuery<WMT0200>(sSql, pWhere, pUsrCode, pPrgCode);

            // 權限設定
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);

            for (int i = 0; i < list.Count; i++)
            {
                //檢查授權刪除、修改
                //list[i].sto_date = DateTime.Parse(list[i].sto_date).ToString("yyyy/MM/dd");
                //list[i].ins_date = DateTime.Parse(list[i].ins_date).ToString("yyyy/MM/dd");
                //list[i].ins_time = DateTime.Parse(list[i].ins_date).ToString("HH:mm:ss");
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
        public List<WMT0200> Get_DataListByQuery3(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<WMT0200> list = new List<WMT0200>();

            string sSql = " SELECT *, BDP21_0100.field_name as ins_type_name, WMB01_0000.sto_name, WMB02_0000.loc_name, WMB03_0000.pallet_name as container_name, WMB10_0000.sup_name,WMT0200.scr_no  " +
                          " FROM WMT0200 " +
                          " left join BDP21_0100 on BDP21_0100.field_code = WMT0200.ins_type and BDP21_0100.code_code = 'ins_type' " +
                          " left join WMB01_0000 on WMB01_0000.sto_code = WMT0200.sto_code " +
                          " left join WMB02_0000 on WMB02_0000.loc_code = WMT0200.loc_code " +
                          " left join WMB03_0000 on WMB03_0000.pallet_code = WMT0200.container " +
                          " left join WMB10_0000 on WMB10_0000.sup_code = WMT0200.sup_code ";

            // 取得資料
            list = comm.Get_ListByQuery<WMT0200>(sSql, pWhere, pUsrCode, pPrgCode);

            // 權限設定
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);

            for (int i = 0; i < list.Count; i++)
            {
                //檢查授權刪除、修改
                //list[i].sto_date = DateTime.Parse(list[i].sto_date).ToString("yyyy/MM/dd");
                //list[i].ins_date = DateTime.Parse(list[i].ins_date).ToString("yyyy/MM/dd");
                //list[i].ins_time = DateTime.Parse(list[i].ins_date).ToString("HH:mm:ss");
                list[i].can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                list[i].can_update = sLimitStr.Contains("M") ? "Y" : "N";
            }

            return list;

        }
        public List<WMT0300> Get_DataListByQuery4(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<WMT0300> list = new List<WMT0300>();

            string sSql = " SELECT *, BDP21_0100.field_name as ins_type_name, WMB01_0000.sto_name, WMB02_0000.loc_name, WMB03_0000.pallet_name as container_name, WMB10_0000.sup_name"+
                          "         ,WMT0200.scr_no,WMT0200.rel_type+'-'+WMT0200.rel_code+'-'+ CONVERT ( varchar,isNull(WMT0200.scr_no,''))  as id  " +
                          " FROM WMT0200 " +
                          " left join BDP21_0100 on BDP21_0100.field_code = WMT0200.ins_type and BDP21_0100.code_code = 'ins_type' " +
                          " left join WMB01_0000 on WMB01_0000.sto_code = WMT0200.sto_code " +
                          " left join WMB02_0000 on WMB02_0000.loc_code = WMT0200.loc_code " +
                          " left join WMB03_0000 on WMB03_0000.pallet_code = WMT0200.container " +
                          " left join WMB10_0000 on WMB10_0000.sup_code = WMT0200.sup_code ";

            // 取得資料
            list = comm.Get_ListByQuery<WMT0300>(sSql, pWhere, pUsrCode, pPrgCode);

            // 權限設定
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);

            for (int i = 0; i < list.Count; i++)
            {
                //檢查授權刪除、修改
                //list[i].sto_date = DateTime.Parse(list[i].sto_date).ToString("yyyy/MM/dd");
                //list[i].ins_date = DateTime.Parse(list[i].ins_date).ToString("yyyy/MM/dd");
                //list[i].ins_time = DateTime.Parse(list[i].ins_date).ToString("HH:mm:ss");
                list[i].can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                list[i].can_update = sLimitStr.Contains("M") ? "Y" : "N";
            }

            return list;

        }
        /// <summary>
        /// 傳入一個WMT0200的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="WMT0200">DTO</param>
        public void InsertData(WMT0200 WMT0200)
        {
            //string sSql = "INSERT INTO " +
            //              " WMT0200 (  wmt0200,  lot_no,  rel_type,  rel_code,  ins_type,  scr_no,  sto_date,  pro_code, " +
            //              "            pro_qty,  sto_code,  loc_code,  ins_user,  ins_date,  container,  barcode,  identifier ) " +

            //              "  VALUES ( @wmt0200, @lot_no, @rel_type, @rel_code, @ins_type, @scr_no, @sto_date, @pro_code, " +
            //              "           @pro_qty, @sto_code, @loc_code, @ins_user, @ins_date, @container, @barcode, @identifier ) ";
            //using (SqlConnection con_db = comm.Set_DBConnection())
            //{
            //    con_db.Execute(sSql, WMT0200);
            //}
        }

        /// <summary>
        /// 傳入一個WMT0200的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="WMT0200">DTO</param>
        public void UpdateData(WMT0200 WMT0200)
        {
            string sSql = " UPDATE WMT0200                    " +
                          "    SET pro_qty   =  @pro_qty,     " +
                          "        sup_code  =  @sup_code,    " +
                          "        sup_lot_no=  @sup_lot_no,  " +
                          "        mft_date  =  @mft_date,    " +
                          "        exp_date  =  @exp_date     " +
                          "  WHERE wmt0200   =  @wmt0200      " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMT0200);
            }
        }

        public void UpdateQty(int wmt0100)
        {
            string sSql = " select * from WMT0200 where wmt0100 = @wmt0100 ";
            Dictionary<string, object> sSqlParams = new Dictionary<string, object>();
            sSqlParams.Add("@wmt0100", wmt0100);
            DataTable dt = comm.Get_DataTable(sSql, sSqlParams);

            decimal tt_qty = 0;
            decimal res_qty = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                tt_qty += Convert.ToDecimal(dt.Rows[i]["pro_qty"]);
            }

            //pro_qty = Convert.ToDecimal(comm.Get_QueryData<decimal>("WMT0100", Convert.ToString(wmt0100), "wmt0100", "pro_qty"));
            res_qty = /*pro_qty -*/ tt_qty;

            sSql = " UPDATE WMT0100 SET res_qty = @res_qty WHERE wmt0100 =  @wmt0100 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { res_qty = res_qty, wmt0100 = wmt0100 });
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM WMT0200 WHERE wmt0200 = @wmt0200;";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { wmt0200 = pTkCode });
            }
        }
        

    }
}