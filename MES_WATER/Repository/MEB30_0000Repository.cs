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
    public class MEB30_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MEB30_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEB30_0000</returns>
        public MEB30_0000 GetDTO(string pTkCode)
        {
            MEB30_0000 datas = new MEB30_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT  * FROM MEB30_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEB30_0000 where work_code=@work_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@work_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEB30_0000
                        {

                            work_code = comm.sGetString(reader["work_code"].ToString()),
                            work_name = comm.sGetString(reader["work_name"].ToString()),
                            cmemo = comm.sGetString(reader["cmemo"].ToString()),

                        };
                    }
                }
            }
            return datas;
        }
        
        /// <summary>
        /// 取得MEB30_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MEB30_0000</returns>
        public List<MEB30_0000> Get_DataList(string pTkCode)
        {
            List<MEB30_0000> list = new List<MEB30_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB30_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEB30_0000 where work_code=@work_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@work_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB30_0000 data = new MEB30_0000();

                    data.work_code = comm.sGetString(reader["work_code"].ToString());
                    data.work_name = comm.sGetString(reader["work_name"].ToString());
                    data.cmemo = comm.sGetString(reader["cmemo"].ToString());


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
        public List<MEB30_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_work_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MEB30_0000> list = new List<MEB30_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = " SELECT * FROM MEB30_0000 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);   
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@work_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB30_0000 data = new MEB30_0000();

                    data.work_code = comm.sGetString(reader["work_code"].ToString());
                    data.work_name = comm.sGetString(reader["work_name"].ToString());
                    data.cmemo = comm.sGetString(reader["cmemo"].ToString());


                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.work_code)) {
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
        public List<MEB30_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MEB30_0000> list = new List<MEB30_0000>();/*, B.field_name as loc_type_name*/

            string sSql = " SELECT distinct MEB30_0000.work_code, MEB30_0000.*  " +
                          " FROM MEB30_0000                                                  " +
                          @"left join (
                                select MEB30_0300.*, MEB37_0000.ng_name
                                FROM MEB30_0300
                                left join MEB37_0000 on MEB37_0000.ng_code = MEB30_0300.ng_code
                            ) as s on s.work_code = MEB30_0000.work_code " +
                          " left join MEB30_0100 on MEB30_0100.work_code = MEB30_0000.work_code  " +
                          " left join MEB29_0000 on MEB29_0000.station_code = MEB30_0100.station_code " ;
                          

            // 取得資料
            list = comm.Get_ListByQuery<MEB30_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
                //        data.sup_name = data.sto_type + " - " + comm.sGetString(reader["sup_name"].ToString());
                //        data.work_name = comm.sGetString(reader["work_code"].ToString()) + " - " + comm.sGetString(reader["work_name"].ToString());

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
        /// 傳入一個MEB30_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEB30_0000">DTO</param>
        public void InsertData(MEB30_0000 MEB30_0000)
        {
            string sSql = " INSERT INTO " +
                          " MEB30_0000 (  work_code,  work_name,  cmemo ) " +
                          "     VALUES ( @work_code, @work_name, @cmemo ) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB30_0000);
            }
        }

        /// <summary>
        /// 傳入一個MEB30_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEB30_0000">DTO</param>
        public void UpdateData(MEB30_0000 MEB30_0000)
        {
            string sSql = " UPDATE MEB30_0000                 " +
                          "    SET work_name =  @work_name,     " +
                          "        cmemo  =  @cmemo      " +
                          "  WHERE work_code =  @work_code      " ;
            
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB30_0000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@work_code", MEB30_0000.work_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@work_code", MEB30_0000.work_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@work_name", MEB30_0000.work_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MEB30_0000 WHERE work_code = @work_code ;" +
                "DELETE FROM MEB30_0100 WHERE work_code = @work_code";

            //sSql += " Delete from BDP09_0100 where work_code = @work_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { work_code = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@work_code", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        ////暫存DataTable參考
        //// <summary>
        //// 取得MEB30_0000角色的DataTable
        //// </summary>
        //// <param name = "pTkCode" > 有傳鍵值取一筆，鍵值空白取全部</param>
        //// <returns></returns>
        //public DataTable GetMEB30_0000_dt(string pTkCode)
        //{
        //    DataTable dtTmp = new DataTable();
        //    DataTable dtDat = new DataTable();
        //    dtDat.Columns.Add("work_code", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("work_code", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("work_name", System.Type.GetType("System.String"].ToString());

        //    string sSql = "";
        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM MEB30_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM MEB30_0000 where work_code='" + pTkCode + "'";
        //    }
        //    dtTmp = comm.Get_DataTable(sSql);

        //    int i;
        //    for (i = 1; i < dtTmp.Rows.Count - 1; i++)
        //    {
        //        DataRow drow = dtDat.NewRow();
        //        drow["work_code"] = dtTmp.Rows[i]["work_code"];
        //        drow["work_code"] = dtTmp.Rows[i]["work_code"];
        //        drow["work_name"] = dtTmp.Rows[i]["work_name"];
        //        dtDat.Rows.Add(drow);
        //    }
        //    return dtDat;
        //}
    }
}