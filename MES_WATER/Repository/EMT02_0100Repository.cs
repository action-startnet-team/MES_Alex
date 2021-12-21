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
    public class EMT02_0100Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得EMT02_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO EMT02_0100</returns>
        public EMT02_0100 GetDTO(string pTkCode)
        {
            EMT02_0100 datas = new EMT02_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMT02_0100 ";
            }
            else
            {
                sSql = "SELECT * FROM EMT02_0100 where emt02_0100 = @emt02_0100 ";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@emt02_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new EMT02_0100
                        {
                            emt02_0100 = comm.sGetInt32(reader["emt02_0100"].ToString()),
                            dev_check_code = comm.sGetString(reader["dev_check_code"].ToString()),
                            chk_item_code = comm.sGetString(reader["chk_item_code"].ToString()),
                            is_ok = comm.sGetString(reader["is_ok"].ToString()),
                            des_memo = comm.sGetString(reader["des_memo"].ToString()),
                            sor_code = comm.sGetString(reader["sor_code"].ToString()),
                            scr_no = comm.sGetString(reader["scr_no"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }


        #region
        /// <summary>
        /// 取得EMT02_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List EMT02_0100</returns>
        //public List<EMT02_0100> Get_DataList(string pTkCode)
        //{
        //    List<EMT02_0100> list = new List<EMT02_0100>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM EMT02_0100";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM EMT02_0100 where emt02_0100=@emt02_0100";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@emt02_0100", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            EMT02_0100 data = new EMT02_0100();

        //            data.emt02_0100 = comm.sGetString(reader["emt02_0100"].ToString());
        //            data.dev_check_code = comm.sGetString(reader["dev_check_code"].ToString());
        //            data.chk_item_code = comm.sGetString(reader["chk_item_code"].ToString());
        //            data.is_ok = comm.sGetString(reader["is_ok"].ToString());
        //            data.sor_code = comm.sGetString(reader["sor_code"].ToString());

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
        //public List<EMT02_0100> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_emt02_0100", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<EMT02_0100> list = new List<EMT02_0100>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料

        //    sSql = "SELECT * FROM EMT02_0100";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            EMT02_0100 data = new EMT02_0100();

        //            data.emt02_0100 = comm.sGetString(reader["emt02_0100"].ToString());
        //            data.dev_check_code = comm.sGetString(reader["dev_check_code"].ToString());
        //            data.chk_item_code = comm.sGetString(reader["chk_item_code"].ToString());
        //            data.is_ok = comm.sGetString(reader["is_ok"].ToString());
        //            data.sor_code = comm.sGetString(reader["sor_code"].ToString());

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
        public List<EMT02_0100> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<EMT02_0100> list = new List<EMT02_0100>();
            string foreignKey = gmv.GetKey<EMT02_0000>(new EMT02_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT EMT02_0100.*, EMB21_0000.chk_item_name, BDP21_0100.field_name as is_ok_name " +
                       " FROM EMT02_0100 " +
                       " left join EMB21_0000 on EMB21_0000.chk_item_code = EMT02_0100.chk_item_code " +
                       " left join BDP21_0100 on BDP21_0100.field_code = EMT02_0100.is_ok and BDP21_0100.code_code = 'is_ok_EMT02_0100' " +
                       " where EMT02_0100. " + foreignKey + "=@" + foreignKey +
                       " order by emt02_0100";
            }
            else
            {
                sSql = "SELECT * FROM EMT02_0100";
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
                    EMT02_0100 data = new EMT02_0100();

                    data.emt02_0100 = comm.sGetInt32(reader["emt02_0100"].ToString());
                    data.dev_check_code = comm.sGetString(reader["dev_check_code"].ToString());
                    data.chk_item_code = comm.sGetString(reader["chk_item_code"].ToString());
                    data.chk_item_name = comm.sGetString(reader["chk_item_name"].ToString());
                    data.is_ok = comm.sGetString(reader["is_ok"].ToString());
                    data.is_ok_name = comm.sGetString(reader["is_ok_name"].ToString());
                    data.des_memo = comm.sGetString(reader["des_memo"].ToString());
                    data.sor_code = comm.sGetString(reader["sor_code"].ToString());
                    data.scr_no = comm.sGetString(reader["scr_no"].ToString());

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
        /// 傳入一個EMT02_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="EMT02_0100">DTO</param>
        public void InsertData(EMT02_0100 EMT02_0100)
        {
            string sSql = " INSERT INTO " +
                          " EMT02_0100 (  dev_check_code,  chk_item_code,  is_ok,  des_memo,  sor_code,  scr_no ) " +
                          "     VALUES ( @dev_check_code, @chk_item_code, @is_ok, @des_memo, @sor_code, @scr_no ) ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMT02_0100);
            }
        }

        /// <summary>
        /// 傳入一個EMT02_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="EMT02_0100">DTO</param>
        public void UpdateData(EMT02_0100 EMT02_0100)
        {
            string sSql = " UPDATE EMT02_0100                       " +
                          "    SET chk_item_code =  @chk_item_code, " +
                          "        is_ok         =  @is_ok,         " +
                          "        des_memo      =  @des_memo       " +
                          "  WHERE emt02_0100    =  @emt02_0100     ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMT02_0100);

            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM EMT02_0100 WHERE emt02_0100 = @emt02_0100 ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { emt02_0100 = pTkCode });

            }
        }

    }
}