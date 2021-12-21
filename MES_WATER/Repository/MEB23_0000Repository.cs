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
    public class MEB23_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MEB23_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEB23_0000</returns>
        public MEB23_0000 GetDTO(string pTkCode)
        {
            MEB23_0000 datas = new MEB23_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB23_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEB23_0000 where bom_code=@bom_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@bom_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEB23_0000
                        {
                            bom_code = comm.sGetString(reader["bom_code"].ToString()),
                            bom_name = comm.sGetString(reader["bom_name"].ToString()),
                            pro_code = comm.sGetString(reader["pro_code"].ToString()),
                            pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString()),
                            unit_code = comm.sGetString(reader["unit_code"].ToString()),
                            version = comm.sGetString(reader["version"].ToString()),
                            now_version = comm.sGetString(reader["now_version"].ToString()),
                            cmemo = comm.sGetString(reader["cmemo"].ToString()),
                            ins_date = comm.sGetString(reader["ins_date"].ToString()),
                            ins_time = comm.sGetString(reader["ins_time"].ToString()),
                            usr_code = comm.sGetString(reader["usr_code"].ToString()),
                            plan_qty = comm.sGetDecimal(reader["plan_qty"].ToString()),
                            in_type = comm.sGetString(reader["in_type"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MEB23_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MEB23_0000</returns>
        //public List<MEB23_0000> Get_DataList(string pTkCode)
        //{
        //    List<MEB23_0000> list = new List<MEB23_0000>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM MEB23_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM MEB23_0000 where bom_code=@bom_code";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@bom_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            MEB23_0000 data = new MEB23_0000();

        //            data.bom_code = comm.sGetString(reader["bom_code"].ToString());
        //            data.bom_name = comm.sGetString(reader["bom_name"].ToString());
        //            data.pro_code = comm.sGetString(reader["pro_code"].ToString());
        //            data.pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString());
        //            data.unit_code = comm.sGetString(reader["unit_code"].ToString());
        //            data.version = comm.sGetString(reader["version"].ToString());
        //            data.now_version = comm.sGetString(reader["now_version"].ToString());
        //            data.cmemo = comm.sGetString(reader["cmemo"].ToString());
        //            data.ins_date = comm.sGetString(reader["ins_date"].ToString());
        //            data.ins_time = comm.sGetString(reader["ins_time"].ToString());
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
        //public List<MEB23_0000> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_bom_code", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<MEB23_0000> list = new List<MEB23_0000>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    sSql = " SELECT * FROM MEB23_0000 " ;

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        //sqlCommand.Parameters.Add(new SqlParameter("@bom_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            MEB23_0000 data = new MEB23_0000();

        //            data.bom_code = comm.sGetString(reader["bom_code"].ToString());
        //            data.bom_name = comm.sGetString(reader["bom_name"].ToString());
        //            data.pro_code = comm.sGetString(reader["pro_code"].ToString());
        //            data.pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString());
        //            data.unit_code = comm.sGetString(reader["unit_code"].ToString());
        //            data.version = comm.sGetString(reader["version"].ToString());
        //            data.now_version = comm.sGetString(reader["now_version"].ToString());
        //            data.cmemo = comm.sGetString(reader["cmemo"].ToString());
        //            data.ins_date = comm.sGetString(reader["ins_date"].ToString());
        //            data.ins_time = comm.sGetString(reader["ins_time"].ToString());
        //            data.usr_code = comm.sGetString(reader["usr_code"].ToString());

        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            //資料邏輯刪除、修改
        //            //if (arr_LockGrpCode.Contains(data.bom_code)) {
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
        public List<MEB23_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MEB23_0000> list = new List<MEB23_0000>();

            string sSql = " SELECT distinct MEB23_0000.bom_code, MEB23_0000.*, MEB20_0000.pro_name, MEB21_0000.pro_type_name as pro_type " +
                          " FROM MEB23_0000 " +
                          " left join MEB23_0100 on MEB23_0100.bom_code = MEB23_0000.bom_code " +
                          " left join MEB20_0000 on MEB20_0000.pro_code = MEB23_0000.pro_code " +
                          " left join MEB20_0000 as MEB20_0000_2 on MEB20_0000_2.pro_code = MEB23_0100.pro_code " +
                          " left join MEB30_0000 on MEB30_0000.work_code = MEB23_0100.work_code " +
                          " left join MEB21_0000 on MEB21_0000.pro_type_code = MEB20_0000.pro_type_code ";

            // 取得資料
            list = comm.Get_ListByQuery<MEB23_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個MEB23_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEB23_0000">DTO</param>
        public void InsertData(MEB23_0000 MEB23_0000)
        {
            string sSql = " INSERT INTO " +
                          " MEB23_0000 (  bom_code,  bom_name,  pro_code,  pro_qty,  unit_code,  version,  now_version, " +
                          "               cmemo,  ins_date,  ins_time,  usr_code,  plan_qty,  in_type ) " +
                          "     VALUES ( @bom_code, @bom_name, @pro_code, @pro_qty, @unit_code, @version, @now_version, " +
                          "              @cmemo, @ins_date, @ins_time, @usr_code, @plan_qty, @in_type ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB23_0000);
            }
        }

        /// <summary>
        /// 傳入一個MEB23_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEB23_0000">DTO</param>
        public void UpdateData(MEB23_0000 MEB23_0000)
        {
            string sSql = " UPDATE MEB23_0000                   " +
                          "    SET bom_name    =  @bom_name,    " +
                          "        pro_code    =  @pro_code,    " +
                          "        pro_qty     =  @pro_qty,     " +
                          "        unit_code   =  @unit_code,   " +
                          "        version     =  @version,     " +
                          "        now_version =  @now_version, " +
                          "        cmemo       =  @cmemo,       " +
                          "        ins_date    =  @ins_date,    " +
                          "        ins_time    =  @ins_time,    " +
                          "        usr_code    =  @usr_code,    " +
                          "        plan_qty    =  @plan_qty,    " +
                          "        in_type     =  @in_type      " +
                          "  WHERE bom_code    =  @bom_code     " ;

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB23_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM MEB23_0000 WHERE bom_code = @bom_code " +
                          " DELETE FROM MEB23_0100 WHERE bom_code = @bom_code ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { bom_code = pTkCode });
            }
        }


    }
}