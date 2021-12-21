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
    public class EMT01_0100BRepository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得EMT01_0100B資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO EMT01_0100B</returns>
        public EMT01_0100B GetDTO(string pTkCode)
        {
            EMT01_0100B datas = new EMT01_0100B();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMT01_0100";
            }
            else
            {
                sSql = "SELECT * FROM EMT01_0100 where emt01_0100=@emt01_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@emt01_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new EMT01_0100B
                        {
                            emt01_0100 = comm.sGetString(reader["emt01_0100"].ToString()),
                            maintain_code = comm.sGetString(reader["maintain_code"].ToString()),
                            main_item_code = comm.sGetString(reader["main_item_code"].ToString()),
                            ins_date = comm.sGetString(reader["ins_date"].ToString()),
                            ins_time = comm.sGetString(reader["ins_time"].ToString()),
                            usr_code = comm.sGetString(reader["usr_code"].ToString()),
                            act_date = comm.sGetString(reader["act_date"].ToString()),
                            act_time = comm.sGetString(reader["act_time"].ToString()),
                            act_usr_code = comm.sGetString(reader["act_usr_code"].ToString()),
                            maintain_cycle = comm.sGetString(reader["maintain_cycle"].ToString()),
                            maintain_memo = comm.sGetString(reader["maintain_memo"].ToString()),
                            is_ok = comm.sGetString(reader["is_ok"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }


        #region
        /// <summary>
        /// 取得EMT01_0100B資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List EMT01_0100B</returns>
        //public List<EMT01_0100B> Get_DataList(string pTkCode)
        //{
        //    List<EMT01_0100B> list = new List<EMT01_0100B>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM EMT01_0100B";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM EMT01_0100B where emt01_0100=@emt01_0100";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@emt01_0100", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            EMT01_0100B data = new EMT01_0100B();

        //            data.emt01_0100 = comm.sGetString(reader["emt01_0100"].ToString());
        //            data.maintain_code = comm.sGetString(reader["maintain_code"].ToString());
        //            data.main_item_code = comm.sGetString(reader["main_item_code"].ToString());
        //            data.ins_date = comm.sGetString(reader["ins_date"].ToString());
        //            data.usr_code = comm.sGetString(reader["usr_code"].ToString());

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
        //public List<EMT01_0100B> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_emt01_0100", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<EMT01_0100B> list = new List<EMT01_0100B>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料

        //    sSql = "SELECT * FROM EMT01_0100B";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            EMT01_0100B data = new EMT01_0100B();

        //            data.emt01_0100 = comm.sGetString(reader["emt01_0100"].ToString());
        //            data.maintain_code = comm.sGetString(reader["maintain_code"].ToString());
        //            data.main_item_code = comm.sGetString(reader["main_item_code"].ToString());
        //            data.ins_date = comm.sGetString(reader["ins_date"].ToString());
        //            data.usr_code = comm.sGetString(reader["usr_code"].ToString());

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
        public List<EMT01_0100B> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<EMT01_0100B> list = new List<EMT01_0100B>();
            string foreignKey = gmv.GetKey<EMT01_0000>(new EMT01_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT EMT01_0100.*, EMB08_0000.main_item_name, A.usr_name " +
                       " FROM EMT01_0100 " +
                       " left join EMB08_0000 on EMB08_0000.main_item_code = EMT01_0100.main_item_code " +
                       " left join BDP08_0000 as A on A.usr_code = EMT01_0100.usr_code " +
                       " where EMT01_0100. " + foreignKey + "=@" + foreignKey +
                       " order by emt01_0100";
            }
            else
            {
                sSql = "SELECT * FROM EMT01_0100";
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
                    EMT01_0100B data = new EMT01_0100B();

                    data.emt01_0100 = comm.sGetString(reader["emt01_0100"].ToString());
                    data.maintain_code = comm.sGetString(reader["maintain_code"].ToString());
                    data.main_item_code = comm.sGetString(reader["main_item_code"].ToString());
                    data.main_item_name = comm.sGetString(reader["main_item_name"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                    data.usr_name = comm.sGetString(reader["usr_name"].ToString());
                    data.maintain_cycle = comm.sGetString(reader["maintain_cycle"].ToString());
                    data.maintain_memo = comm.sGetString(reader["maintain_memo"].ToString());
                    data.is_ok = comm.sGetString(reader["is_ok"].ToString());

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
        public List<EMT01_0100B> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<EMT01_0100B> list = new List<EMT01_0100B>();

            string sSql = " SELECT EMT01_0100.*, EMT01_0000.maintain_name, EMT01_0000.dev_code, EMB07_0000.dev_name, EMB08_0000.main_item_name, BDP08_0000.usr_name, A.usr_name as act_usr_name " +
                          " FROM EMT01_0100 " +
                          " left join EMT01_0000 on EMT01_0000.maintain_code = EMT01_0100.maintain_code " +
                          " left join EMB08_0000 on EMB08_0000.main_item_code = EMT01_0100.main_item_code " +
                          " left join BDP08_0000 on BDP08_0000.usr_code = EMT01_0100.usr_code " +
                          " left join BDP08_0000 as A on A.usr_code = EMT01_0100.act_usr_code " +
                          " left join EMB07_0000 on EMB07_0000.dev_code = EMT01_0000.dev_code ";

            // 取得資料
            list = comm.Get_ListByQuery<EMT01_0100B>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個EMT01_0100B的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="EMT01_0100B">DTO</param>
        public void InsertData(EMT01_0100B EMT01_0100B)
        {
            string sSql = " INSERT INTO " +
                          " EMT01_0100 (  maintain_code,  main_item_code,  ins_date,  ins_time,  usr_code, " +
                          "               act_date,  act_time,  act_usr_code,  maintain_cycle,  maintain_memo,  is_ok ) " +
                          "     VALUES ( @maintain_code, @main_item_code, @ins_date, @ins_time, @usr_code, " +
                          "              @act_date, @act_time, @act_usr_code, @maintain_cycle, @maintain_memo, @is_ok ) ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMT01_0100B);
            }
        }

        /// <summary>
        /// 傳入一個EMT01_0100B的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="EMT01_0100B">DTO</param>
        public void UpdateData(EMT01_0100B EMT01_0100B)
        {
            string sSql = " UPDATE EMT01_0100                         " +
                          "    SET main_item_code =  @main_item_code, " +
                          "        ins_date       =  @ins_date,       " +
                          "        ins_time       =  @ins_time,       " +
                          "        usr_code       =  @usr_code,       " +
                          "        act_date       =  @act_date,       " +
                          "        act_time       =  @act_time,       " +
                          "        act_usr_code   =  @act_usr_code,   " +
                          "        maintain_cycle =  @maintain_cycle, " +
                          "        maintain_memo  =  @maintain_memo,  " +
                          "        is_ok          =  @is_ok           " +
                          "  WHERE emt01_0100     =  @emt01_0100      ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMT01_0100B);

            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM EMT01_0100 WHERE emt01_0100 = @emt01_0100 ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { emt01_0100 = pTkCode });

            }
        }

    }
}