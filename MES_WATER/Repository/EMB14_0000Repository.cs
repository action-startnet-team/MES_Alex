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
    public class EMB14_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得EMB14_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO EMB14_0000</returns>
        public EMB14_0000 GetDTO(string pTkCode)
        {
            EMB14_0000 datas = new EMB14_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMB14_0000";
            }
            else
            {
                sSql = "SELECT * FROM EMB14_0000 where per_code=@per_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@per_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new EMB14_0000
                        {

                            per_code = comm.sGetString(reader["per_code"].ToString()),
                            per_name = comm.sGetString(reader["per_name"].ToString()),
                            per_kind = comm.sGetString(reader["per_kind"].ToString()),
                            sup_code = comm.sGetString(reader["sup_code"].ToString()),
                            dep_code = comm.sGetString(reader["dep_code"].ToString()),
                            per_tel = comm.sGetString(reader["per_tel"].ToString()),
                            per_mail = comm.sGetString(reader["per_mail"].ToString()),
                            cmemo = comm.sGetString(reader["cmemo"].ToString()),
                            is_use = comm.sGetString(reader["is_use"].ToString()),



                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得EMB14_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List EMB14_0000</returns>
        public List<EMB14_0000> Get_DataList(string pTkCode)
        {
            List<EMB14_0000> list = new List<EMB14_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMB14_0000";
            }
            else
            {
                sSql = "SELECT * FROM EMB14_0000 where per_code=@per_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@per_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EMB14_0000 data = new EMB14_0000();

                    data.per_code = comm.sGetString(reader["per_code"].ToString());
                    data.per_name = comm.sGetString(reader["per_name"].ToString());
                    data.per_kind = comm.sGetString(reader["per_kind"].ToString());
                    data.sup_code = comm.sGetString(reader["sup_code"].ToString());
                    data.dep_code = comm.sGetString(reader["dep_code"].ToString());
                    data.per_tel = comm.sGetString(reader["per_tel"].ToString());
                    data.per_mail = comm.sGetString(reader["per_mail"].ToString());
                    data.cmemo = comm.sGetString(reader["cmemo"].ToString());
                    data.is_use = comm.sGetString(reader["is_use"].ToString());

                    data.can_delete = "Y";
                    data.can_update = "Y";
                    list.Add(data);
                }

            }
            return list;
        }

        /// <summary>
        /// 取得使用者可以編輯的資料，結合商務邏輯權限
        /// </summary>
        /// <param name="pUsrCode"></param>
        /// <param name="pPrgCode"></param>
        /// <returns></returns>
        public List<EMB14_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_per_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<EMB14_0000> list = new List<EMB14_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM EMB14_0000";
            sSql = "SELECT * FROM EMB14_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EMB14_0000 data = new EMB14_0000();

                    data.per_code = comm.sGetString(reader["per_code"].ToString());
                    data.per_name = comm.sGetString(reader["per_name"].ToString());
                    data.per_kind = comm.sGetString(reader["per_kind"].ToString());
                    data.sup_code = comm.sGetString(reader["sup_code"].ToString());
                    data.dep_code = comm.sGetString(reader["dep_code"].ToString());
                    data.per_tel = comm.sGetString(reader["per_tel"].ToString());
                    data.per_mail = comm.sGetString(reader["per_mail"].ToString());
                    data.cmemo = comm.sGetString(reader["cmemo"].ToString());
                    data.is_use = comm.sGetString(reader["is_use"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";
                    

                    list.Add(data);
                }
            }
            return list;
        }
        #endregion

        /// <summary>
        /// 取得查詢資料，結合使用者權限
        /// </summary>
        /// <param name="pUsrCode">使用者代碼</param>
        /// <param name="pPrgCode">功能代碼</param>
        /// <param name="pWhere">JSON查詢字串</param>
        /// <returns></returns>
        public List<EMB14_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<EMB14_0000> list = new List<EMB14_0000>();

            string sSql = " SELECT *, BDP21_0100.field_name as per_kind_name, EMB03_0000.sup_name, EMB02_0000.dep_name " +
                          " FROM EMB14_0000 " +
                          " left join BDP21_0100 on BDP21_0100.field_code = EMB14_0000.per_kind and BDP21_0100.code_code = 'per_kind' " +
                          " left join EMB03_0000 on EMB03_0000.sup_code = EMB14_0000.sup_code " +
                          " left join EMB02_0000 on EMB02_0000.dep_code = EMB14_0000.dep_code ";

            // 取得資料
            list = comm.Get_ListByQuery<EMB14_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個EMB14_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="EMB14_0000">DTO</param>
        public void InsertData(EMB14_0000 EMB14_0000)
        {

            string sSql = "INSERT INTO " +
                          " EMB14_0000 (  per_code,  per_name,  per_kind,  sup_code,  dep_code,  per_tel,  per_mail,  cmemo,  is_use ) " +
                          "     VALUES ( @per_code, @per_name, @per_kind, @sup_code, @dep_code, @per_tel, @per_mail, @cmemo, @is_use ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMB14_0000);
            }
        }

        /// <summary>
        /// 傳入一個EMB14_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="EMB14_0000">DTO</param>
        public void UpdateData(EMB14_0000 EMB14_0000)
        {
            string sSql = " UPDATE EMB14_0000              " +
                          "    SET per_name  =  @per_name, " +
                          "        per_kind  =  @per_kind, " +
                          "        sup_code  =  @sup_code, " +
                          "        dep_code  =  @dep_code, " +
                          "        per_tel   =  @per_tel,  " +
                          "        per_mail  =  @per_mail, " +
                          "        cmemo     =  @cmemo,    " +
                          "        is_use    =  @is_use    " +
                          "  WHERE per_code  =  @per_code  ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMB14_0000);
                
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM EMB14_0000 WHERE per_code = @per_code;";
            //sSql += " Delete from BDP09_0100 where per_code = @per_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { per_code = pTkCode });
                
            }
        }

        
    }
}