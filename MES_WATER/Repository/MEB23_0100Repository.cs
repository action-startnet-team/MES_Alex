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
    public class MEB23_0100Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得MEB23_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEB23_0100</returns>
        public MEB23_0100 GetDTO(string pTkCode)
        {
            MEB23_0100 datas = new MEB23_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT * FROM MEB23_0100 ";
            }
            else
            {
                sSql = " SELECT * FROM MEB23_0100 where meb23_0100=@meb23_0100 ";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@meb23_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEB23_0100
                        {
                            meb23_0100 = comm.sGetString(reader["meb23_0100"].ToString()),
                            bom_code = comm.sGetString(reader["bom_code"].ToString()),
                            pro_code = comm.sGetString(reader["pro_code"].ToString()),
                            pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString()),
                            dis_qty = comm.sGetDecimal(reader["dis_qty"].ToString()),
                            unit_code = comm.sGetString(reader["unit_code"].ToString()),
                            pro_qty_min = comm.sGetDecimal(reader["pro_qty_min"].ToString()),
                            unit_code_min = comm.sGetString(reader["unit_code_min"].ToString()),
                            tol_qty = comm.sGetDecimal(reader["tol_qty"].ToString()),
                            work_code = comm.sGetString(reader["work_code"].ToString()),
                            is_ready = comm.sGetString(reader["is_ready"].ToString()),
                            is_throw = comm.sGetString(reader["is_throw"].ToString()),
                            in_scr_no = comm.sGetString(reader["in_scr_no"].ToString()),
                            pack_qty = comm.sGetDecimal(reader["pack_qty"].ToString()),
                            pack_tol_qty = comm.sGetDecimal(reader["pack_tol_qty"].ToString()),
                            loc_code = comm.sGetString(reader["loc_code"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MEB23_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MEB23_0100</returns>
        //public List<MEB23_0100> Get_DataList(string pTkCode)
        //{
        //    List<MEB23_0100> list = new List<MEB23_0100>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM MEB23_0100";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM MEB23_0100 where meb23_0100=@meb23_0100";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@meb23_0100", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            MEB23_0100 data = new MEB23_0100();

        //            data.meb23_0100 = comm.sGetString(reader["meb23_0100"].ToString());
        //            data.bom_code = comm.sGetString(reader["bom_code"].ToString());
        //            data.pro_code = comm.sGetString(reader["pro_code"].ToString());
        //            data.pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString());
        //            data.dis_qty = comm.sGetDecimal(reader["dis_qty"].ToString());
        //            data.tol_qty = comm.sGetDecimal(reader["tol_qty"].ToString());
        //            data.unit_code = comm.sGetString(reader["unit_code"].ToString());
        //            data.work_code = comm.sGetString(reader["work_code"].ToString());
        //            data.is_ready = comm.sGetString(reader["is_ready"].ToString());

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
        //public List<MEB23_0100> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_meb23_0100", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<MEB23_0100> list = new List<MEB23_0100>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    sSql = " SELECT * FROM MEB23_0100 ";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        //sqlCommand.Parameters.Add(new SqlParameter("@meb23_0100", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            MEB23_0100 data = new MEB23_0100();

        //            data.meb23_0100 = comm.sGetString(reader["meb23_0100"].ToString());
        //            data.bom_code = comm.sGetString(reader["bom_code"].ToString());
        //            data.pro_code = comm.sGetString(reader["pro_code"].ToString());
        //            data.pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString());
        //            data.dis_qty = comm.sGetDecimal(reader["dis_qty"].ToString());
        //            data.unit_code = comm.sGetString(reader["unit_code"].ToString());
        //            data.work_code = comm.sGetString(reader["work_code"].ToString());
        //            data.is_ready = comm.sGetString(reader["is_ready"].ToString());

        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            //資料邏輯刪除、修改
        //            //if (arr_LockGrpCode.Contains(data.meb23_0100)) {
        //            //    data.can_delete = "N";
        //            //    data.can_update = "N";
        //            //}

        //            list.Add(data);
        //        }
        //    }
        //    return list;
        //}
        #endregion

        public List<MEB23_0100> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<MEB23_0100> list = new List<MEB23_0100>();
            string foreignKey = gmv.GetKey<MEB23_0000>(new MEB23_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT MEB23_0100.*, MEB20_0000_2.pro_name as pro_name, MEB30_0000.work_name as work_name, BDP21_0100.field_name as is_throw_name, WMB02_0000.loc_name " +
                       " FROM MEB23_0100 " +
                       " left join MEB20_0000 as MEB20_0000_2 on MEB20_0000_2.pro_code = MEB23_0100.pro_code " +
                       " left join MEB30_0000 on MEB30_0000.work_code = MEB23_0100.work_code " +
                       " left join BDP21_0100 on BDP21_0100.field_code = MEB23_0100.is_throw and BDP21_0100.code_code = 'is_throw' " +
                       " left join WMB02_0000 on WMB02_0000.loc_code = MEB23_0100.loc_code " +
                       " where MEB23_0100. " + foreignKey + "=@" + foreignKey +
                       " order by work_code, pro_code ";
            }
            else
            {
                sSql = "SELECT * FROM MEB23_0100";
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

                    MEB23_0100 data = new MEB23_0100();

                    data.meb23_0100 = comm.sGetString(reader["meb23_0100"].ToString());
                    data.bom_code = comm.sGetString(reader["bom_code"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.pro_name = comm.sGetString(reader["pro_name"].ToString());
                    data.pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString());
                    data.dis_qty = comm.sGetDecimal(reader["dis_qty"].ToString());
                    data.unit_code = comm.sGetString(reader["unit_code"].ToString());
                    data.pro_qty_min = comm.sGetDecimal(reader["pro_qty_min"].ToString());
                    data.unit_code_min = comm.sGetString(reader["unit_code_min"].ToString());
                    data.tol_qty = comm.sGetDecimal(reader["tol_qty"].ToString());
                    data.work_code = comm.sGetString(reader["work_code"].ToString());
                    data.work_name = comm.sGetString(reader["work_name"].ToString());
                    data.is_ready = comm.sGetString(reader["is_ready"].ToString());
                    data.is_throw = comm.sGetString(reader["is_throw"].ToString());
                    data.is_throw_name = comm.sGetString(reader["is_throw_name"].ToString());
                    data.in_scr_no = comm.sGetString(reader["in_scr_no"].ToString());
                    data.pack_qty = comm.sGetDecimal(reader["pack_qty"].ToString());
                    data.pack_tol_qty = comm.sGetDecimal(reader["pack_tol_qty"].ToString());
                    data.loc_code = comm.sGetString(reader["loc_code"].ToString());
                    data.loc_name = comm.sGetString(reader["loc_name"].ToString());
                    //--

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
        /// 傳入一個MEB23_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEB23_0100">DTO</param>
        public void InsertData(MEB23_0100 MEB23_0100)
        {
            string sSql = "INSERT INTO " +
                          " MEB23_0100 (  bom_code,  pro_code,  pro_qty,  dis_qty,  unit_code,  pro_qty_min,  unit_code_min, " +
                          "               tol_qty,  work_code,  is_ready,  is_throw,  in_scr_no,  pack_qty,  pack_tol_qty,  loc_code ) " +
                          "     VALUES ( @bom_code, @pro_code, @pro_qty, @dis_qty, @unit_code, @pro_qty_min, @unit_code_min, " +
                          "              @tol_qty, @work_code, @is_ready, @is_throw, @in_scr_no, @pack_qty, @pack_tol_qty, @loc_code ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB23_0100);
            }
        }

        /// <summary>
        /// 傳入一個MEB23_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEB23_0100">DTO</param>
        public void UpdateData(MEB23_0100 MEB23_0100)
        {
            string sSql = " UPDATE MEB23_0100                       " +
                          "    SET bom_code      =  @bom_code,      " +
                          "        pro_code      =  @pro_code,      " +
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
                          "  WHERE meb23_0100    =  @meb23_0100     " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB23_0100);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM MEB23_0100 WHERE meb23_0100 = @meb23_0100 ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { meb23_0100 = pTkCode });
            }
        }


    }
}