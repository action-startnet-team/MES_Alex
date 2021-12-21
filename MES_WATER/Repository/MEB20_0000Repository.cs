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
    public class MEB20_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MEB20_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEB20_0000</returns>
        public MEB20_0000 GetDTO(string pTkCode)
        {
            MEB20_0000 datas = new MEB20_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB20_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEB20_0000 where pro_code=@pro_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@pro_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEB20_0000
                        {
                            pro_code = comm.sGetString(reader["pro_code"].ToString()),
                            pro_name = comm.sGetString(reader["pro_name"].ToString()),
                            pro_spc = comm.sGetString(reader["pro_spc"].ToString()),
                            unit_code = comm.sGetString(reader["unit_code"].ToString()),
                            line_code = comm.sGetString(reader["line_code"].ToString()),
                            pro_type = comm.sGetString(reader["pro_type"].ToString()),
                            pro_type_code = comm.sGetString(reader["pro_type_code"].ToString()),
                            is_use = comm.sGetString(reader["is_use"].ToString()),
                            cmemo = comm.sGetString(reader["cmemo"].ToString()),
                            exp_num = comm.sGetInt32(reader["exp_num"].ToString()),
                            open_exp_num = comm.sGetInt32(reader["open_exp_num"].ToString()),
                            mtp_rec_rate = comm.sGetDecimal(reader["mtp_rec_rate"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        ///// <summary>
        ///// 取得MEB20_0000資料表內容
        ///// </summary>
        ///// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        ///// < returns > List MEB20_0000</returns>
        //public List<MEB20_0000> Get_DataList(string pTkCode)
        //{
        //    List<MEB20_0000> list = new List<MEB20_0000>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM MEB20_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM MEB20_0000 where pro_code=@pro_code";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@pro_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            MEB20_0000 data = new MEB20_0000();
                   
        //            data.pro_code = comm.sGetString(reader["pro_code"].ToString());
        //            data.pro_name = comm.sGetString(reader["pro_name"].ToString());
        //            data.unit_code = comm.sGetString(reader["unit_code"].ToString());
        //            data.pro_spc = comm.sGetString(reader["pro_spc"].ToString());
        //            data.line_code = comm.sGetString(reader["line_code"].ToString());
        //            data.pro_type_code = comm.sGetString(reader["pro_type_code"].ToString());
        //            data.is_use = comm.sGetString(reader["is_use"].ToString());
        //            data.cmemo = comm.sGetString(reader["cmemo"].ToString());
                

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
        //public List<MEB20_0000> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_pro_code", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<MEB20_0000> list = new List<MEB20_0000>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    sSql = "SELECT * FROM MEB20_0000";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            MEB20_0000 data = new MEB20_0000();

        //            data.pro_code = comm.sGetString(reader["pro_code"].ToString());
        //            data.pro_name = comm.sGetString(reader["pro_name"].ToString());
        //            data.unit_code = comm.sGetString(reader["unit_code"].ToString());
        //            data.pro_spc = comm.sGetString(reader["pro_spc"].ToString());
        //            data.line_code = comm.sGetString(reader["line_code"].ToString());
        //            data.pro_type_code = comm.sGetString(reader["pro_type_code"].ToString());
        //            data.is_use = comm.sGetString(reader["is_use"].ToString());
        //            data.cmemo = comm.sGetString(reader["cmemo"].ToString());

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
        public List<MEB20_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MEB20_0000> list = new List<MEB20_0000>();

            string sSql = " SELECT distinct MEB20_0000.pro_code, MEB20_0000.*, MEB21_0000.pro_type_name as pro_type_name, MEB12_0000.line_name, WMB07_0000.unit_name " +
                          " FROM MEB20_0000 " +
                          " left join MEB20_0100 on MEB20_0100.pro_code = MEB20_0100.pro_code " +
                          " left join MEB21_0000 on MEB21_0000.pro_type_code = MEB20_0000.pro_type_code " +
                          " left join MEB12_0000 on MEB12_0000.line_code = MEB20_0000.line_code " +
                          " left join WMB07_0000 on WMB07_0000.unit_code = MEB20_0000.unit_code " +
                          " left join MEB30_0000 on MEB30_0000.work_code = MEB20_0100.work_code ";
            // 取得資料
            list = comm.Get_ListByQuery<MEB20_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個MEB20_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEB20_0000">DTO</param>
        public void InsertData(MEB20_0000 MEB20_0000)
        {
            string sSql = "INSERT INTO " +
                          " MEB20_0000 (  pro_code,  pro_name,  pro_spc,  unit_code,  line_code, pro_type, " +
                          "               pro_type_code,  is_use,  cmemo,  exp_num,  open_exp_num,  mtp_rec_rate ) " +
                          "     VALUES ( @pro_code, @pro_name, @pro_spc, @unit_code, @line_code, @pro_type, " +
                          "              @pro_type_code, @is_use, @cmemo, @exp_num, @open_exp_num, @mtp_rec_rate ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB20_0000);
            }
        }

        /// <summary>
        /// 傳入一個MEB20_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEB20_0000">DTO</param>
        public void UpdateData(MEB20_0000 MEB20_0000)
        {
            string sSql = " UPDATE MEB20_0000                       " +
                          "    SET pro_name      =  @pro_name,      " +
                          "        unit_code     =  @unit_code,     " +
                          "        pro_spc       =  @pro_spc,       " +
                          "        line_code     =  @line_code,     " +
                          "        pro_type      =  @pro_type,      " +
                          "        pro_type_code =  @pro_type_code, " +
                          "        is_use        =  @is_use,        " +
                          "        cmemo         =  @cmemo,         " +
                          "        exp_num       =  @exp_num,       " +
                          "        open_exp_num  =  @open_exp_num,  " +
                          "        mtp_rec_rate  =  @mtp_rec_rate   " +
                          "  WHERE pro_code      =  @pro_code       ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB20_0000);
                
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM MEB20_0000 WHERE pro_code = @pro_code ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { pro_code = pTkCode });
                
            }
        }
        
    }
}