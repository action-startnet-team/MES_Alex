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
    public class RPT19_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得RPT19_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO RPT19_0000</returns>
        public RPT19_0000 GetDTO(string pTkCode)
        {
            RPT19_0000 datas = new RPT19_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM RPT19_0000";
            }
            else
            {
                sSql = "SELECT * FROM RPT19_0000 where etl_code=@etl_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@etl_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new RPT19_0000
                        {
                            factory_code = comm.sGetString(reader["factory_code"].ToString()),
                            factory_name = comm.sGetString(reader["factory_name"].ToString()),
                            area_code = comm.sGetString(reader["area_code"].ToString()),
                            area_name = comm.sGetString(reader["area_name"].ToString()),
                            line_code = comm.sGetString(reader["line_code"].ToString()),
                            line_name = comm.sGetString(reader["line_name"].ToString()),

                    };
                    }
                }
            }
            return datas;
        }

        /// <summary>
        /// 取得RPT19_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List RPT19_0000</returns>
        public List<RPT19_0000> Get_DataList(string pTkCode)
        {
            List<RPT19_0000> list = new List<RPT19_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM RPT19_0000";
            }
            else
            {
                sSql = "SELECT * FROM RPT19_0000 where etl_code=@etl_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@etl_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    RPT19_0000 data = new RPT19_0000();
                    data.factory_code = comm.sGetString(reader["factory_code"].ToString());
                    data.factory_name = comm.sGetString(reader["factory_name"].ToString());
                    data.area_code = comm.sGetString(reader["area_code"].ToString());
                    data.area_name = comm.sGetString(reader["area_name"].ToString());
                    data.line_code = comm.sGetString(reader["line_code"].ToString());
                    data.line_name = comm.sGetString(reader["line_name"].ToString());
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
        public List<RPT19_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_etl_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<RPT19_0000> list = new List<RPT19_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = " SELECT * FROM RPT19_0000 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@etl_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    RPT19_0000 data = new RPT19_0000();
                    data.factory_code= comm.sGetString(reader["factory_code"].ToString());
                    data.factory_name =comm.sGetString(reader["factory_name"].ToString());
                    data.area_code = comm.sGetString(reader["area_code"].ToString());
                    data.area_name =  comm.sGetString(reader["area_name"].ToString());
                    data.line_code = comm.sGetString(reader["line_code"].ToString());
                    data.line_name = comm.sGetString(reader["line_name"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.etl_code)) {
                    //    data.can_delete = "N";
                    //    data.can_update = "N";
                    //}

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
        public List<RPT19_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<RPT19_0000> list = new List<RPT19_0000>();

            string sSql = "" ;
            sSql = " SELECT MEB12_0000.*, MEB11_0000.area_name, MEB11_0000.factory_code, MEB10_0000.factory_name " +
                              " FROM MEB12_0000 " +
                              " LEFT JOIN MEB11_0000 on MEB11_0000.area_code = MEB12_0000.area_code " +
                              " LEFT JOIN MEB10_0000 on MEB10_0000.factory_code = MEB11_0000.factory_code ";

                       // 取得資料
                       list = comm.Get_ListByQuery<RPT19_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM RPT19_0000 WHERE etl_code = @etl_code; " +
                          " DELETE FROM RSS01_0100 WHERE etl_code = @etl_code; " ;
            //sSql += " Delete from BDP09_0100 where etl_code = @etl_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { etl_code = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@etl_code", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        ////暫存DataTable參考
        //// <summary>
        //// 取得RPT19_0000角色的DataTable
        //// </summary>
        //// <param name = "pTkCode" > 有傳鍵值取一筆，鍵值空白取全部</param>
        //// <returns></returns>
        //public DataTable GetRPT19_0000_dt(string pTkCode)
        //{
        //    DataTable dtTmp = new DataTable();
        //    DataTable dtDat = new DataTable();
        //    dtDat.Columns.Add("etl_code", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("etl_code", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("etl_name", System.Type.GetType("System.String"].ToString());

        //    string sSql = "";
        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM RPT19_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM RPT19_0000 where etl_code='" + pTkCode + "'";
        //    }
        //    dtTmp = comm.Get_DataTable(sSql);

        //    int i;
        //    for (i = 1; i < dtTmp.Rows.Count - 1; i++)
        //    {
        //        DataRow drow = dtDat.NewRow();
        //        drow["etl_code"] = dtTmp.Rows[i]["etl_code"];
        //        drow["etl_code"] = dtTmp.Rows[i]["etl_code"];
        //        drow["etl_name"] = dtTmp.Rows[i]["etl_name"];
        //        dtDat.Rows.Add(drow);
        //    }
        //    return dtDat;
        //}
    }
}