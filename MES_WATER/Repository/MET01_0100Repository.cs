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
    public class MET01_0100Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        // <summary>
        // 取得MET01_0100資料表內容
        // </summary>
        // <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        // <returns> DTO MET01_0100</returns>
        public MET01_0100 GetDTO(string pTkCode)
        {
            MET01_0100 datas = new MET01_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MET01_0100";
            }
            else
            {
                sSql = "SELECT * FROM MET01_0100 where met01_0100=@met01_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@met01_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MET01_0100
                        {
                            met01_0100 = comm.sGetInt32(reader["met01_0100"].ToString()),
                            mo_code = comm.sGetString(reader["mo_code"].ToString()),
                            pro_code = comm.sGetString(reader["pro_code"].ToString()),
                            pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString()),
                            dis_qty = comm.sGetDecimal(reader["dis_qty"].ToString()),
                            //pro_unit = comm.sGetString(reader["pro_unit"].ToString()),
                            work_code = comm.sGetString(reader["work_code"].ToString()),
                            is_ready = comm.sGetString(reader["is_ready"].ToString()),
                            //VORNR = comm.sGetString(reader["VORNR"].ToString()),
                            //LGORT = comm.sGetString(reader["LGORT"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        ///// <summary>
        ///// 取得MET01_0100資料表內容
        ///// </summary>
        ///// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        ///// < returns > List MET01_0100</returns>
        //public List<MET01_0100> Get_DataList(string pTkCode)
        //{
        //    List<MET01_0100> list = new List<MET01_0100>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM MET01_0100";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM MET01_0100 where mo_code=@mo_code";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@mo_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            MET01_0100 data = new MET01_0100();

        //            data.met01_0100 = comm.sGetInt32(reader["met01_0100"].ToString());
        //            data.mo_code = comm.sGetString(reader["mo_code"].ToString());
        //            data.pro_code = comm.sGetString(reader["pro_code"].ToString());
        //            data.pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString());
        //            data.dis_qty = comm.sGetDecimal(reader["dis_qty"].ToString());
        //            data.pro_unit = comm.sGetString(reader["pro_unit"].ToString());
        //            data.work_code = comm.sGetString(reader["work_code"].ToString());
        //            data.is_ready = comm.sGetString(reader["is_ready"].ToString());
        //            data.VORNR = comm.sGetString(reader["VORNR"].ToString());
        //            data.LGORT = comm.sGetString(reader["LGORT"].ToString());

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
        //public List<MET01_0100> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_met01_0100", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<MET01_0100> list = new List<MET01_0100>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    sSql = " SELECT * FROM MET01_0100 ";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        //sqlCommand.Parameters.Add(new SqlParameter("@met01_0100", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            MET01_0100 data = new MET01_0100();

        //            data.met01_0100 = comm.sGetInt32(reader["met01_0100"].ToString());
        //            data.mo_code = comm.sGetString(reader["mo_code"].ToString());
        //            data.pro_code = comm.sGetString(reader["pro_code"].ToString());
        //            data.pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString());
        //            data.dis_qty = comm.sGetDecimal(reader["dis_qty"].ToString());
        //            data.pro_unit = comm.sGetString(reader["pro_unit"].ToString());
        //            data.work_code = comm.sGetString(reader["work_code"].ToString());
        //            data.is_ready = comm.sGetString(reader["is_ready"].ToString());
        //            data.VORNR = comm.sGetString(reader["VORNR"].ToString());
        //            data.LGORT = comm.sGetString(reader["LGORT"].ToString());

        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            //資料邏輯刪除、修改
        //            //if (arr_LockGrpCode.Contains(data.met01_0100)) {
        //            //    data.can_delete = "N";
        //            //    data.can_update = "N";
        //            //}

        //            list.Add(data);
        //        }
        //    }
        //    return list;
        //}
        #endregion
        public List<MET01_0100> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<MET01_0100> list = new List<MET01_0100>();
            string foreignKey = gmv.GetKey<MET01_0000>(new MET01_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT MET01_0100.*, MEB20_0000.pro_name as pro_name, MEB30_0000.work_name as work_name, MEB30_0000.work_code + ' - ' + MEB30_0000.work_name as work_code_hidden, C.field_name as pro_kind_name, D.field_name as is_throw_name, WMB02_0000.loc_name " +
                       " FROM MET01_0100 " +
                       " left join MEB20_0000 on MEB20_0000.pro_code = MET01_0100.pro_code " +
                       " left join MEB30_0000 on MEB30_0000.work_code = MET01_0100.work_code " +
                       " left join MET01_0000 on MET01_0000.mo_code = MET01_0100.mo_code" +
                       " left join BDP21_0100 as C on C.field_code = MET01_0100.pro_kind and C.code_code = 'pro_kind' " +
                       " left join BDP21_0100 as D on D.field_code = MET01_0100.is_throw and D.code_code = 'is_throw' " +
                       " left join WMB02_0000 on WMB02_0000.loc_code = MET01_0100.loc_code " +
                       " where MET01_0100. " + foreignKey + "=@" + foreignKey ;
                

                //sSql = " SELECT MET01_0100.*, MEB20_0000.pro_name as pro_name, MEB30_0000.work_name as work_name, MEB30_0000.work_code + ' - ' + MEB30_0000.work_name as work_code_hidden, " +
                //       " MEB23_0100.pro_qty_min, MEB23_0100.unit_code_min, MEB23_0100.tol_qty, MEB23_0100.in_scr_no, MEB23_0100.pack_qty" +
                //       " FROM MET01_0100 " +
                //       " left join MEB20_0000 on MEB20_0000.pro_code = MET01_0100.pro_code " +
                //       " left join MEB30_0000 on MEB30_0000.work_code = MET01_0100.work_code " +
                //       " left join MET01_0000 on MET01_0000.mo_code = MET01_0100.mo_code" +
                //       " left join MEB23_0000 on MEB23_0000.bom_code = MET01_0000.bom_code " +
                //       " left join MEB23_0100 on MEB23_0100.bom_code = MEB23_0000.bom_code and MEB23_0100.pro_code = MET01_0100.pro_code" +
                //       " left join WMB07_0000 on WMB07_0000.unit_code = MEB23_0100.unit_code" +
                //       " left join WMB07_0000 as C on C.unit_code = MEB23_0100.unit_code_min" +
                //       " where MET01_0100. " + foreignKey + "=@" + foreignKey +
                //       " order by work_code, pro_code ";
            }
            else
            {
                sSql = "SELECT * FROM MET01_0100";
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
                    MET01_0100 data = new MET01_0100();

                    data.met01_0100 = comm.sGetInt32(reader["met01_0100"].ToString());
                    data.mo_code = comm.sGetString(reader["mo_code"].ToString());
                    data.work_code = comm.sGetString(reader["work_code"].ToString());
                    data.work_name = comm.sGetString(reader["work_name"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.pro_name = comm.sGetString(reader["pro_name"].ToString());
                    data.pro_kind = comm.sGetString(reader["pro_kind"].ToString());
                    data.pro_kind_name = comm.sGetString(reader["pro_kind_name"].ToString());
                    data.is_throw = comm.sGetString(reader["is_throw"].ToString());
                    data.is_throw_name = comm.sGetString(reader["is_throw_name"].ToString());
                    data.pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString());
                    data.dis_qty = comm.sGetDecimal(reader["dis_qty"].ToString());
                    data.unit_code = comm.sGetString(reader["unit_code"].ToString());
                    data.tol_qty = comm.sGetDecimal(reader["tol_qty"].ToString());
                    data.pro_qty_min = comm.sGetDecimal(reader["pro_qty_min"].ToString());
                    data.pack_qty = comm.sGetDecimal(reader["pack_qty"].ToString());
                    data.unit_code_min = comm.sGetString(reader["unit_code_min"].ToString());
                    data.pack_tol_qty = comm.sGetDecimal(reader["pack_tol_qty"].ToString());
                    data.is_ready = comm.sGetString(reader["is_ready"].ToString());
                    data.in_scr_no = comm.sGetString(reader["in_scr_no"].ToString());
                    data.loc_code = comm.sGetString(reader["loc_code"].ToString());
                    data.loc_name = comm.sGetString(reader["loc_name"].ToString());

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
        /// 傳入一個MET01_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MET01_0100">DTO</param>
        public void InsertData(MET01_0100 MET01_0100)
        {
            //string sSql = "INSERT INTO " +
            //              " MET01_0100 (  mo_code,  pro_code,  pro_qty,  dis_qty,  pro_unit,  work_code,  is_ready,  VORNR,  LGORT ) " +
            //              "     VALUES ( @mo_code, @pro_code, @pro_qty, @dis_qty, @pro_unit, @work_code, @is_ready, @VORNR, @LGORT ) " ;
            string sSql = "INSERT INTO " +
                          " MET01_0100 (  mo_code,  pro_code,  pro_qty,  dis_qty,  unit_code,  pro_qty_min,  unit_code_min, " +
                          "               tol_qty,  work_code,  is_ready,  is_throw,  in_scr_no,  pack_qty,  pack_tol_qty,  loc_code, pro_kind ) " +
                          "     VALUES ( @mo_code, @pro_code, @pro_qty, @dis_qty, @unit_code, @pro_qty_min, @unit_code_min, " +
                          "              @tol_qty, @work_code, @is_ready, @is_throw, @in_scr_no, @pack_qty, @pack_tol_qty, @loc_code , @pro_kind ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MET01_0100);
            }
        }

        /// <summary>
        /// 傳入一個MET01_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MET01_0100">DTO</param>
        //public void InsertData2(MET01_0100 MET01_0100)
        //{
        //    string sSql = "INSERT INTO " +
        //                  " MET01_0100 (  mo_code,  pro_code,  pro_qty,  dis_qty,  pro_unit,  work_code,  is_ready,  VORNR,  LGORT,  pro_kind,  meb23_0100 ) " +
        //                  "     VALUES ( @mo_code, @pro_code, @pro_qty, @dis_qty, @pro_unit, @work_code, @is_ready, @VORNR, @LGORT, @pro_kind, @meb23_0100 ) ";
        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        con_db.Execute(sSql, MET01_0100);
        //    }
        //}

        /// <summary>
        /// 傳入一個MET01_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MET01_0100">DTO</param>
        public void UpdateData(MET01_0100 MET01_0100)
        {
            //string sSql = " UPDATE MET01_0100                 " +
            //              "    SET mo_code     =  @mo_code,   " +
            //              "        pro_code    =  @pro_code,  " +
            //              "        pro_qty     =  @pro_qty,   " +
            //              "        dis_qty     =  @dis_qty,   " +
            //              "        pro_unit    =  @pro_unit,  " +
            //              "        work_code   =  @work_code, " +
            //              "        is_ready    =  @is_ready,  " +
            //              "        VORNR       =  @VORNR,     " +
            //              "        LGORT       =  @LGORT      " +
            //              "  WHERE met01_0100  =  @met01_0100 " ;
            string sSql = " UPDATE MET01_0100                       " +
                          "    SET pro_code      =  @pro_code,      " +
                          "        pro_kind      =  @pro_kind,      " +
                          "        pro_qty       =  @pro_qty,       " +
                          "        dis_qty       =  @dis_qty,       " +
                          "        unit_code     =  @unit_code,     " +
                          "        pro_qty_min   =  @pro_qty_min,   " +
                          "        unit_code_min =  @unit_code_min, " +
                          "        tol_qty       =  @tol_qty,       " +
                          "        work_code     =  @work_code,     " +
                          "        is_ready      =  @is_ready,      " +
                          "        is_throw      =  @is_throw,      " +
                          "        in_scr_no     =  @in_scr_no,     " +
                          "        pack_qty      =  @pack_qty,      " +
                          "        pack_tol_qty  =  @pack_tol_qty,  " +
                          "        loc_code      =  @loc_code       " +
                          "  WHERE met01_0100    =  @met01_0100     ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MET01_0100);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MET01_0100 WHERE met01_0100 = @met01_0100;";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { met01_0100 = pTkCode });
            }
        }

        /// <summary>
        /// 取得指定Bom明細寫入MET01_0100
        /// </summary>
        /// <param name="bom_code">傳入的bom_code</param>
        public void InsertByMEB23_0100(string bom_code, string mo_code, decimal plan_qty)
        {
            List<MEB23_0100> list = new List<MEB23_0100>();
            string sSql = " select * from MEB23_0100 where bom_code = @bom_code ";
            // 取得資料
            Dictionary<string, object> sSqlParams = new Dictionary<string, object>();
            sSqlParams.Add("@bom_code", bom_code);
            DataTable dt = comm.Get_DataTable(sSql, sSqlParams);
            decimal pro_qty = comm.Get_QueryData<decimal>("MEB23_0000", bom_code, "bom_code", "pro_qty");

            for (int i=0; i < dt.Rows.Count; i++)
            {
                MET01_0100 data = new MET01_0100();
                data.mo_code = mo_code;
                data.work_code = dt.Rows[i]["work_code"].ToString();
                data.pro_code = dt.Rows[i]["pro_code"].ToString();
                data.is_throw = comm.sGetString((dt.Rows[i]["is_throw"].ToString()));
                data.pro_qty = pro_qty != 0 && Convert.ToDecimal(dt.Rows[i]["pro_qty"]) != 0 ? Convert.ToDecimal(dt.Rows[i]["pro_qty"]) * plan_qty / pro_qty : 0;
                data.dis_qty = pro_qty != 0 && Convert.ToDecimal(dt.Rows[i]["dis_qty"]) != 0 ? Convert.ToDecimal(dt.Rows[i]["dis_qty"]) * plan_qty / pro_qty : 0;
                data.unit_code = comm.sGetString((dt.Rows[i]["unit_code"].ToString()));
                data.tol_qty = comm.sGetDecimal((dt.Rows[i]["tol_qty"].ToString()));
                data.pro_qty_min = comm.sGetDecimal((dt.Rows[i]["pro_qty_min"].ToString()));
                data.pack_qty = comm.sGetDecimal((dt.Rows[i]["pack_qty"].ToString()));
                data.pack_tol_qty = comm.sGetDecimal((dt.Rows[i]["pack_tol_qty"].ToString()));
                data.unit_code_min = comm.sGetString((dt.Rows[i]["unit_code_min"].ToString()));

                data.is_ready = comm.sGetString((dt.Rows[i]["is_ready"].ToString()));
                data.in_scr_no = comm.sGetString((dt.Rows[i]["in_scr_no"].ToString()));
                data.loc_code = comm.sGetString((dt.Rows[i]["loc_code"].ToString()));
                data.pro_kind = comm.Get_QueryData("MEB20_0000", data.pro_code, "pro_code", "pro_type");
                InsertData(data);


                //data.pro_unit = dt.Rows[i]["unit_code"].ToString();
                //data.VORNR = "";
                //data.LGORT = "";
                //data.pro_kind = "";
                //data.meb23_0100 = comm.sGetInt32(dt.Rows[i]["meb23_0100"].ToString());
                //InsertData2(data);
            }
        }

        /// <summary>
        /// 取得指定Bom明細修改MET01_0100
        /// </summary>
        /// <param name="bom_code"></param>
        /// <param name="mo_code"></param>
        /// <param name="plan_qty"></param>
        public void UpdateByMEB23_0100(string bom_code, string mo_code, decimal plan_qty)
        {
            string sSql = "";
            decimal dProQty = 0;
            decimal dDisQty = 0;

            sSql = "select * from MET01_0100 where mo_code='" + mo_code + "'";
            DataTable dtTmp = comm.Get_DataTable(sSql);
            
            //取得BOM標準用量，換算比例
            decimal pro_qty = comm.Get_QueryData<decimal>("MEB23_0000", bom_code, "bom_code", "pro_qty");

            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                dProQty = pro_qty != 0 && Convert.ToDecimal(dtTmp.Rows[i]["pro_qty"]) != 0 ? Convert.ToDecimal(dtTmp.Rows[i]["pro_qty"]) * plan_qty / pro_qty : 0;
                dDisQty = pro_qty != 0 && Convert.ToDecimal(dtTmp.Rows[i]["dis_qty"]) != 0 ? Convert.ToDecimal(dtTmp.Rows[i]["dis_qty"]) * plan_qty / pro_qty : 0;

                sSql = "update MET01_0100 " +
                       "   set pro_qty='" + dProQty + "'" +
                       "      ,dis_qty='" + dDisQty + "'" +
                       " where met01_0100=" + dtTmp.Rows[i]["met01_0100"].ToString();
                comm.Connect_DB(sSql);
            }



            //string sSql = "DELETE FROM MET01_0100 WHERE mo_code = @mo_code;";
            //using (SqlConnection con_db = comm.Set_DBConnection())
            //{
            //    con_db.Execute(sSql, new { mo_code = mo_code });
            //}

            //List<MEB23_0100> list = new List<MEB23_0100>();
            //sSql = " select * from MEB23_0100 where bom_code = @bom_code ";
            //// 取得資料
            //Dictionary<string, object> sSqlParams = new Dictionary<string, object>();
            //sSqlParams.Add("@bom_code", bom_code);
            //DataTable dt = comm.Get_DataTable(sSql, sSqlParams);
            //decimal pro_qty = comm.Get_QueryData<decimal>("MEB23_0000", bom_code, "bom_code", "pro_qty");

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    MET01_0100 data = new MET01_0100();
            //    data.mo_code = mo_code;
            //    data.pro_code = dt.Rows[i]["pro_code"].ToString();
            //    data.pro_qty = pro_qty != 0 && Convert.ToDecimal(dt.Rows[i]["pro_qty"]) != 0 ? Convert.ToDecimal(dt.Rows[i]["pro_qty"]) * plan_qty / pro_qty : 0;
            //    data.dis_qty = Convert.ToDecimal(dt.Rows[i]["dis_qty"]);
            //    //data.pro_unit = dt.Rows[i]["unit_code"].ToString();
            //    data.work_code = dt.Rows[i]["work_code"].ToString();
            //    data.is_ready = dt.Rows[i]["is_ready"].ToString();
            //    //data.VORNR = "";
            //    //data.LGORT = "";
            //    //data.pro_kind = "";
            //    //data.meb23_0100 = comm.sGetInt32(dt.Rows[i]["meb23_0100"].ToString());
            //    //InsertData2(data);
            //}
        }




    }
}