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
    public class WMB06_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得WMB06_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO WMB06_0000</returns>
        public WMB06_0000 GetDTO(string pTkCode)
        {
            WMB06_0000 datas = new WMB06_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM WMB06_0000";
            }
            else
            {
                sSql = "SELECT * FROM WMB06_0000 where pro_code=@pro_code";
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
                        datas = new WMB06_0000
                        {
                            pro_code = comm.sGetString(reader["pro_code"].ToString()),
                            pro_name = comm.sGetString(reader["pro_name"].ToString()),
                            pro_spc = comm.sGetString(reader["pro_spc"].ToString()),
                            pro_type = comm.sGetString(reader["pro_type"].ToString()),
                            unit_qty_min = comm.sGetDecimal(reader["unit_qty_min"].ToString()),
                            unit_code = comm.sGetString(reader["unit_code"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得WMB06_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List WMB06_0000</returns>
        public List<WMB06_0000> Get_DataList(string pTkCode)
        {
            List<WMB06_0000> list = new List<WMB06_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM WMB06_0000";
            }
            else
            {
                sSql = "SELECT * FROM WMB06_0000 where pro_code=@pro_code";
            }
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@pro_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    WMB06_0000 data = new WMB06_0000();

                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.pro_name = comm.sGetString(reader["pro_name"].ToString());
                    data.pro_spc = comm.sGetString(reader["pro_spc"].ToString());
                    data.pro_type = comm.sGetString(reader["pro_type"].ToString());


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
        public List<WMB06_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_pro_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<WMB06_0000> list = new List<WMB06_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM WMB06_0000";
            sSql = "SELECT * FROM WMB06_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@pro_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    WMB06_0000 data = new WMB06_0000();

                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.pro_name = comm.sGetString(reader["pro_name"].ToString());
                    data.pro_spc = comm.sGetString(reader["pro_spc"].ToString());
                    data.pro_type = comm.sGetString(reader["pro_type"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.pro_code)) {
                    //    data.can_delete = "N";
                    //    data.can_update = "N";
                    //}

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
        public List<WMB06_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<WMB06_0000> list = new List<WMB06_0000>();

            string sSql = " SELECT WMB06_0000.*, BDP21_0100.field_name as pro_type_name, WMB07_0000.unit_name " +
                          " FROM WMB06_0000 " +
                          " left join BDP21_0100 on BDP21_0100.field_code = WMB06_0000.pro_type AND BDP21_0100.code_code = 'pro_type' " +
                          " left join WMB07_0000 on WMB07_0000.unit_code = WMB06_0000.unit_code ";

            // 取得資料
            list = comm.Get_ListByQuery<WMB06_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個WMB06_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="WMB06_0000">DTO</param>
        public void InsertData(WMB06_0000 WMB06_0000)
        {
            string sSql = "INSERT INTO " +
                          " WMB06_0000 (  pro_code,  pro_name,  pro_spc,  pro_type,  unit_code,  unit_qty_min )" +
                          "     VALUES ( @pro_code, @pro_name, @pro_spc, @pro_type, @unit_code, @unit_qty_min )";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMB06_0000);
            }
        }

        /// <summary>
        /// 傳入一個WMB06_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="WMB06_0000">DTO</param>
        public void UpdateData(WMB06_0000 WMB06_0000)
        {
            string sSql = " UPDATE WMB06_0000                     " +
                          "    SET pro_name     =  @pro_name,     " +
                          "        pro_spc      =  @pro_spc,      " +
                          "        pro_type     =  @pro_type,     " +
                          "        unit_code    =  @unit_code,    " +
                          "        unit_qty_min =  @unit_qty_min  " +
                          "  WHERE pro_code     =  @pro_code      " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMB06_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " delete from WMB06_0000 where pro_code = @pro_code ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { pro_code = pTkCode });
            }
        }


    }
}