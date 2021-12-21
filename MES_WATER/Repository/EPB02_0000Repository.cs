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
    public class EPB02_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得EPB02_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO EPB02_0000</returns>
        public EPB02_0000 GetDTO(string pTkCode)
        {
            EPB02_0000 datas = new EPB02_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT  * FROM EPB02_0000";
            }
            else
            {
                sSql = "SELECT * FROM EPB02_0000 where epb_code=@epb_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@epb_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new EPB02_0000
                        {
                            epb_code = comm.sGetString(reader["epb_code"].ToString()),
                            epb_name = comm.sGetString(reader["epb_name"].ToString()),
                            epb_type_code = comm.sGetString(reader["epb_type_code"].ToString()),
                            cmemo = comm.sGetString(reader["cmemo"].ToString()),
                            is_use = comm.sGetString(reader["is_use"].ToString()),
                            save_type = comm.sGetString(reader["save_type"].ToString()),
                            save_method = comm.sGetString(reader["save_method"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }
        
        /// <summary>
        /// 取得EPB02_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List EPB02_0000</returns>
        public List<EPB02_0000> Get_DataList(string pTkCode)
        {
            List<EPB02_0000> list = new List<EPB02_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EPB02_0000";
            }
            else
            {
                sSql = "SELECT * FROM EPB02_0000 where epb_code=@epb_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@epb_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EPB02_0000 data = new EPB02_0000();

                    data.epb_code = comm.sGetString(reader["epb_code"].ToString());
                    data.epb_name = comm.sGetString(reader["epb_name"].ToString());
                    data.epb_type_code = comm.sGetString(reader["epb_type_code"].ToString());
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
        public List<EPB02_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_epb_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<EPB02_0000> list = new List<EPB02_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = " SELECT * FROM EPB02_0000 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);   
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@epb_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EPB02_0000 data = new EPB02_0000();

                    data.epb_code = comm.sGetString(reader["epb_code"].ToString());
                    data.epb_name = comm.sGetString(reader["epb_name"].ToString());
                    data.epb_type_code = comm.sGetString(reader["epb_type_code"].ToString());
                    data.cmemo = comm.sGetString(reader["cmemo"].ToString());
                    data.is_use = comm.sGetString(reader["is_use"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.epb_code)) {
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
        public List<EPB02_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<EPB02_0000> list = new List<EPB02_0000>();/*, B.field_name as loc_type_name*/

            string sSql = " SELECT distinct  EPB02_0000.epb_code, EPB02_0000.*, EPB01_0000.epb_type_name as epb_type_name   " +
                          " FROM EPB02_0000 " +
                          " left join EPB02_0100 on EPB02_0100.epb_code = EPB02_0000.epb_code " +
                          " left join EPB01_0000 on EPB01_0000.epb_type_code = EPB02_0000.epb_type_code " +
                          " left join BDP21_0100 AS BDPctr on BDPctr.field_code = EPB02_0100.ctr_type and BDPctr.code_code = 'ctr_type' " +
                          " left join BDP21_0100 AS BDPdata on BDPdata.field_code = EPB02_0100.data_type and BDPdata.code_code = 'data_type' " ;


            // 取得資料
            list = comm.Get_ListByQuery<EPB02_0000>(sSql, pWhere, pUsrCode, pPrgCode);

            // 權限設定
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            //string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_mtp_code", "par_name", "par_value");
            //var arr_LockGrpCode = sLockGrpCode.Split(',');

            for (int i = 0; i < list.Count; i++)
            {
                //檢查授權刪除、修改
                list[i].can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                list[i].can_update = sLimitStr.Contains("M") ? "Y" : "N";

                //        // 特例 轉換
                //        data.sup_name = data.cmemo + " - " + comm.sGetString(reader["sup_name"].ToString());
                //        data.epb_name = comm.sGetString(reader["epb_code"].ToString()) + " - " + comm.sGetString(reader["epb_name"].ToString());

                //        data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                //        data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                //        //資料邏輯刪除、修改
                //        //if (arr_LockGrpCode.Contains(data.mtp_code)) {
                //        //    data.can_delete = "N";
                //        //    data.can_update = "N";
                //        //}
            }
            return list;
        }

        /// <summary>
        /// 傳入一個EPB02_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="EPB02_0000">DTO</param>
        public void InsertData(EPB02_0000 EPB02_0000)
        {
            string sSql = " INSERT INTO " +
                          " EPB02_0000 (  epb_code,  epb_name,  epb_type_code,  cmemo,  is_use,  save_type,  save_method) " +
                          "     VALUES ( @epb_code, @epb_name, @epb_type_code, @cmemo, @is_use, @save_type, @save_method) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EPB02_0000);
            }
        }

        /// <summary>
        /// 傳入一個EPB02_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="EPB02_0000">DTO</param>
        public void UpdateData(EPB02_0000 EPB02_0000)
        {
            string sSql = " UPDATE EPB02_0000                        " +
                          "    SET epb_name       =  @epb_name,      " +
                          "        epb_type_code  =  @epb_type_code, " +
                          "        cmemo          =  @cmemo,         " +
                          "        is_use         =  @is_use,        " +
                          "        save_type      =  @save_type,     " +
                          "        save_method    =  @save_method    " +
                          "  WHERE epb_code       =  @epb_code       " ;
            
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EPB02_0000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@epb_code", EPB02_0000.epb_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@epb_code", EPB02_0000.epb_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@epb_name", EPB02_0000.epb_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM EPB02_0000 WHERE epb_code = @epb_code; " +
                          " DELETE FROM EPB02_0100 WHERE epb_code = @epb_code; " ;
            //sSql += " Delete from BDP09_0100 where epb_code = @epb_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { epb_code = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@epb_code", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        ////暫存DataTable參考
        //// <summary>
        //// 取得EPB02_0000角色的DataTable
        //// </summary>
        //// <param name = "pTkCode" > 有傳鍵值取一筆，鍵值空白取全部</param>
        //// <returns></returns>
        //public DataTable GetEPB02_0000_dt(string pTkCode)
        //{
        //    DataTable dtTmp = new DataTable();
        //    DataTable dtDat = new DataTable();
        //    dtDat.Columns.Add("epb_code", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("epb_code", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("epb_name", System.Type.GetType("System.String"].ToString());

        //    string sSql = "";
        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM EPB02_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM EPB02_0000 where epb_code='" + pTkCode + "'";
        //    }
        //    dtTmp = comm.Get_DataTable(sSql);

        //    int i;
        //    for (i = 1; i < dtTmp.Rows.Count - 1; i++)
        //    {
        //        DataRow drow = dtDat.NewRow();
        //        drow["epb_code"] = dtTmp.Rows[i]["epb_code"];
        //        drow["epb_code"] = dtTmp.Rows[i]["epb_code"];
        //        drow["epb_name"] = dtTmp.Rows[i]["epb_name"];
        //        dtDat.Rows.Add(drow);
        //    }
        //    return dtDat;
        //}
    }
}