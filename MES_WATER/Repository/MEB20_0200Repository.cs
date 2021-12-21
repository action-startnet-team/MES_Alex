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
    public class MEB20_0200Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得MEB20_0200資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEB20_0200</returns>
        public MEB20_0200 GetDTO(string pTkCode)
        {
            MEB20_0200 datas = new MEB20_0200();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB20_0200";
            }
            else
            {
                sSql = "SELECT * FROM MEB20_0200 where meb20_0200=@meb20_0200";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@meb20_0200", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEB20_0200
                        {

                            meb20_0200 = comm.sGetInt32(reader["meb20_0200"].ToString()),
                            pro_code = comm.sGetString(reader["pro_code"].ToString()),
                            unit_code_base = comm.sGetString(reader["unit_code_base"].ToString()),
                            base_qty = comm.sGetDecimal(reader["base_qty"].ToString()),
                            unit_code_chg = comm.sGetString(reader["unit_code_chg"].ToString()),
                            chg_qty = comm.sGetDecimal(reader["chg_qty"].ToString()),

                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MEB20_0200資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MEB20_0200</returns>
        //public List<MEB20_0200> Get_DataList(string pTkCode)
        //{
        //    List<MEB20_0200> list = new List<MEB20_0200>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM MEB20_0200";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM MEB20_0200 where meb20_0200=@meb20_0200";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@meb20_0200", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            MEB20_0200 data = new MEB20_0200();

        //            data.meb20_0200 = comm.sGetInt32(reader["meb20_0200"].ToString());
        //            data.pro_code = comm.sGetString(reader["pro_code"].ToString());
        //            data.unit_code_base = comm.sGetString(reader["unit_code_base"].ToString());
        //            data.base_qty = comm.sGetDecimal(reader["base_qty"].ToString());
        //            data.unit_code_chg = comm.sGetString(reader["unit_code_chg"].ToString());
        //            data.chg_qty = comm.sGetDecimal(reader["chg_qty"].ToString());

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
        //public List<MEB20_0200> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_meb20_0200", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<MEB20_0200> list = new List<MEB20_0200>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料

        //    sSql = "SELECT * FROM MEB20_0200";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            MEB20_0200 data = new MEB20_0200();

        //            data.meb20_0200 = comm.sGetInt32(reader["meb20_0200"].ToString());
        //            data.pro_code = comm.sGetString(reader["pro_code"].ToString());
        //            data.unit_code_base = comm.sGetInt32(reader["unit_code_base"].ToString());
        //            data.base_qty = comm.sGetString(reader["base_qty"].ToString());
        //            data.unit_code_chg = comm.sGetString(reader["unit_code_chg"].ToString());
        //            data.chg_qty = comm.sGetString(reader["chg_qty"].ToString());
        //            data.work_code = comm.sGetString(reader["work_code"].ToString());

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
        public List<MEB20_0200> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<MEB20_0200> list = new List<MEB20_0200>();
            string foreignKey = gmv.GetKey<MEB20_0000>(new MEB20_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT MEB20_0200.*, WMB07_0000.unit_name as unit_name_base, A.unit_name as unit_name_chg" +
                       " FROM MEB20_0200 " +
                       " left join WMB07_0000 on WMB07_0000.unit_code = MEB20_0200.unit_code_base " +
                       " left join WMB07_0000 as A on A.unit_code = MEB20_0200.unit_code_chg " +
                       " where MEB20_0200. " + foreignKey + "=@" + foreignKey +
                       " order by meb20_0200 ";
            }
            else
            {
                sSql = "SELECT * FROM MEB20_0200";
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

                    MEB20_0200 data = new MEB20_0200();

                    data.meb20_0200 = comm.sGetInt32(reader["meb20_0200"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.unit_code_base = comm.sGetString(reader["unit_code_base"].ToString());
                    data.unit_name_base = comm.sGetString(reader["unit_name_base"].ToString());
                    data.base_qty = comm.sGetDecimal(reader["base_qty"].ToString());
                    data.unit_code_chg = comm.sGetString(reader["unit_code_chg"].ToString());
                    data.unit_name_chg = comm.sGetString(reader["unit_name_chg"].ToString());
                    data.chg_qty = comm.sGetDecimal(reader["chg_qty"].ToString());


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
        /// 傳入一個MEB20_0200的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEB20_0200">DTO</param>
        public void InsertData(MEB20_0200 MEB20_0200)
        {
            string sSql = "INSERT INTO " +
                          " MEB20_0200 (  pro_code,  unit_code_base,  base_qty,  unit_code_chg,  chg_qty ) " +
                          "     VALUES ( @pro_code, @unit_code_base,  '1',        @unit_code_chg, @chg_qty ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB20_0200);
            }
        }

        /// <summary>
        /// 傳入一個MEB20_0200的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEB20_0200">DTO</param>
        public void UpdateData(MEB20_0200 MEB20_0200)
        {
            string sSql = " UPDATE MEB20_0200                         " +
                          "    SET pro_code       =  @pro_code,       " +
                          "        unit_code_base =  @unit_code_base, " +
                          "        base_qty       =  @base_qty,       " +
                          "        unit_code_chg  =  @unit_code_chg,  " +
                          "        chg_qty        =  @chg_qty         " +
                          "  WHERE meb20_0200     =  @meb20_0200      ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB20_0200);

            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM MEB20_0200 WHERE meb20_0200 = @meb20_0200 ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { meb20_0200 = pTkCode });

            }
        }

        public void DeleteByProCode(string pProCode)
        {
            string sSql = " DELETE FROM MEB20_0200 WHERE pro_code = @pro_code ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { pro_code = pProCode });

            }
        }

    }
}