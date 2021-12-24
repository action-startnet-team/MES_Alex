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
    public class MEB50_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MEB50_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEB50_0000</returns>
        public MEB50_0000 GetDTO(string pTkCode)
        {
            MEB50_0000 datas = new MEB50_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB50_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEB50_0000 where ITEM_CODE=@ITEM_CODE";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@ITEM_CODE", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEB50_0000
                        {

                            ITEM_CODE = comm.sGetString(reader["ITEM_CODE"].ToString()),
                            ITEM_SPECIFICATION = comm.sGetString(reader["ITEM_SPECIFICATION"].ToString()),
                            _pro_type = comm.sGetString(reader["_pro_type"].ToString()),
                            pro_uph = comm.sGetDecimal(reader["pro_uph"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MEB50_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MEB50_0000</returns>
        public List<MEB50_0000> Get_DataList(string pTkCode)
        {
            List<MEB50_0000> list = new List<MEB50_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB50_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEB50_0000 where ITEM_CODE=@ITEM_CODE";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@ITEM_CODE", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB50_0000 data = new MEB50_0000();

                    data.ITEM_CODE = comm.sGetString(reader["ITEM_CODE"].ToString());
                    data.ITEM_SPECIFICATION = comm.sGetString(reader["ITEM_SPECIFICATION"].ToString());
                    data._pro_type = comm.sGetString(reader["_pro_type"].ToString());
                    data.pro_uph = comm.sGetDecimal(reader["pro_uph"].ToString());
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
        public List<MEB50_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_mac_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MEB50_0000> list = new List<MEB50_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = "SELECT * FROM MEB50_0000";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB50_0000 data = new MEB50_0000();

                    data.ITEM_CODE = comm.sGetString(reader["ITEM_CODE"].ToString());
                    data.ITEM_SPECIFICATION = comm.sGetString(reader["ITEM_SPECIFICATION"].ToString());
                    data._pro_type = comm.sGetString(reader["_pro_type"].ToString());
                    data.pro_uph = comm.sGetDecimal(reader["pro_uph"].ToString());

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
        public List<MEB50_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MEB50_0000> list = new List<MEB50_0000>();

            //string sSql = " SELECT distinct MEB50_0000.mac_code, MEB50_0000.*, MEB14_0000.mac_type_name, BDP21_0100.field_name as ip_type_name, MEB12_0000.line_name " +
            //              " FROM MEB50_0000 " +
            //              @"left join (
            //                    select MEB45_0100.*, MEB45_0000.stop_name
            //                    from MEB45_0100
            //                    left join MEB45_0000 on MEB45_0000.stop_code = MEB45_0100.stop_code
            //                ) as s on s.mac_code = MEB50_0000.mac_code " +
            //              " left join MEB14_0000 on MEB14_0000.mac_type_code = MEB50_0000.mac_type_code " +
            //              " left join BDP21_0100 on BDP21_0100.field_code = MEB50_0000.ip_type and BDP21_0100.code_code = 'ip_type' " +
            //              " left join MEB12_0000 on MEB12_0000.line_code = MEB50_0000.line_code ";

            string sSql = " SELECT * FROM MEB50_0000 ";

            // 取得資料
            list = comm.Get_ListByQuery<MEB50_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個MEB50_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEB50_0000">DTO</param>
        public void InsertData(MEB50_0000 MEB50_0000)
        {
            string sSql = "INSERT INTO " +
                          " MEB50_0000 (  ITEM_CODE,  ITEM_SPECIFICATION, _pro_type,  pro_uph ) " +
                          "     VALUES ( @ITEM_CODE, @ITEM_SPECIFICATION, @_pro_type, @pro_uph ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB50_0000);
            }
        }

        /// <summary>
        /// 傳入一個MEB50_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEB50_0000">DTO</param>
        public void UpdateData(MEB50_0000 MEB50_0000)
        {
            string sSql = " UPDATE MEB50_0000                        " +
                          "    SET ITEM_SPECIFICATION       =  @ITEM_SPECIFICATION,      " +
                          "        _pro_type      =  @_pro_type,     " +
                          "        pro_uph       =  @pro_uph       " +
                          "  WHERE ITEM_CODE       =  @ITEM_CODE       ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB50_0000);
                
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MEB50_0000 WHERE ITEM_CODE = @ITEM_CODE;";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { mac_code = pTkCode });
            }
        }
        

    }
}