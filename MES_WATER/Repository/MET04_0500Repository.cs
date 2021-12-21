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
    public class MET04_0500Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        //// <summary>
        //// 取得MET04_0500資料表內容
        //// </summary>
        //// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        //// < returns > DTO MET04_0500</returns>
        public MET04_0500 GetDTO(string pTkCode)
        {
            MET04_0500 datas = new MET04_0500();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MET04_0500";
            }
            else
            {
                sSql = "SELECT * FROM MET04_0500 where ureport_code=@ureport_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@ureport_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MET04_0500
                        {
                            ureport_code = comm.sGetString(reader["ureport_code"].ToString()),
                            ureport_date = comm.sGetString(reader["ureport_date"].ToString()),
                            mo_code = comm.sGetString(reader["mo_code"].ToString()),
                            mac_code = comm.sGetString(reader["mac_code"].ToString()),
                            stop_code = comm.sGetString(reader["stop_code"].ToString()),
                            wrk_date_s = comm.sGetString(reader["wrk_date_s"].ToString()),
                            wrk_time_s = comm.sGetString(reader["wrk_time_s"].ToString()),
                            wrk_date_e = comm.sGetString(reader["wrk_date_e"].ToString()),
                            wrk_time_e = comm.sGetString(reader["wrk_time_e"].ToString()),
                            sub_minute = comm.sGetInt32(reader["sub_minute"].ToString()),
                            sub_hour = comm.sGetDecimal(reader["sub_hour"].ToString()),
                         

                        };
                    }
                }
            }
            return datas;
        }

        #region
        //// <summary>
        //// 取得MET04_0500資料表內容
        //// </summary>
        //// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        //// < returns > List MET04_0500</returns>
        public List<MET04_0500> Get_DataList(string pTkCode)
        {
            List<MET04_0500> list = new List<MET04_0500>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MET04_0500";
            }
            else
            {
                sSql = "SELECT * FROM MET04_0500 where ureport_code=@ureport_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@ureport_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MET04_0500 data = new MET04_0500();

                    data.ureport_code = comm.sGetString(reader["ureport_code"].ToString());
                    data.ureport_date = comm.sGetString(reader["ureport_date"].ToString());
                    data.mo_code = comm.sGetString(reader["mo_code"].ToString());
                    data.mac_code = comm.sGetString(reader["mac_code"].ToString());
                    data.stop_code = comm.sGetString(reader["stop_code"].ToString());
                    data.wrk_date_s = comm.sGetString(reader["wrk_date_s"].ToString());
                    data.wrk_time_s = comm.sGetString(reader["wrk_time_s"].ToString());
                    data.wrk_date_e = comm.sGetString(reader["wrk_date_e"].ToString());
                    data.wrk_time_e = comm.sGetString(reader["wrk_time_e"].ToString());
                    data.sub_minute = comm.sGetInt32(reader["sub_minute"].ToString());
                    data.sub_hour = comm.sGetDecimal(reader["sub_hour"].ToString());
                    data.can_delete = "Y";
                    data.can_update = "Y";
                    list.Add(data);
                }

            }
            return list;
        }

        //// <summary>
        //// 取得使用者可以編輯的資料，結合商務邏輯權限
        //// </summary>
        //// <param name = "pUsrCode" ></ param >
        //// < param name="pPrgCode"></param>
        //// <returns></returns>
        public List<MET04_0500> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_ureport_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MET04_0500> list = new List<MET04_0500>();
            string sSql = "";

            ////取得該使用者可以看的資料
            sSql = " SELECT * FROM MET04_0500 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MET04_0500 data = new MET04_0500();

                    data.ureport_code = comm.sGetString(reader["ureport_code"].ToString());
                    data.ureport_date = comm.sGetString(reader["ureport_date"].ToString());
                    data.mo_code = comm.sGetString(reader["mo_code"].ToString());
                    data.mac_code = comm.sGetString(reader["mac_code"].ToString());
                    data.stop_code = comm.sGetString(reader["stop_code"].ToString());
                    data.wrk_date_s = comm.sGetString(reader["wrk_date_s"].ToString());
                    data.wrk_time_s = comm.sGetString(reader["wrk_time_s"].ToString());
                    data.wrk_date_e = comm.sGetString(reader["wrk_date_e"].ToString());
                    data.wrk_time_e = comm.sGetString(reader["wrk_time_e"].ToString());
                    data.sub_minute = comm.sGetInt32(reader["sub_minute"].ToString());
                    data.sub_hour = comm.sGetDecimal(reader["sub_hour"].ToString());
                  

                    ////檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    ////資料邏輯刪除、修改
                    if (arr_LockGrpCode.Contains(data.ureport_code))
                    {
                        data.can_delete = "N";
                        data.can_update = "N";
                    }

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
        public List<MET04_0500> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MET04_0500> list = new List<MET04_0500>();

            string sSql = " SELECT MET04_0500.*, MEB15_0000.mac_name as mac_name , MEB45_0000.stop_name as stop_name" +
                                " FROM MET04_0500" +
                                " left join MEB15_0000 on MEB15_0000.mac_code = MET04_0500.mac_code " +
                                " left join MEB45_0000 on MEB45_0000.stop_code = MET04_0500.stop_code ";

            // 取得資料
            list = comm.Get_ListByQuery<MET04_0500>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個MET04_0500的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name = "MET04_0500" > DTO </ param >
        public void InsertData(MET04_0500 MET04_0500)
        {
            string sSql = "INSERT INTO " +
                          " MET04_0500 (     ureport_code,   ureport_date,    mo_code,    mac_code,    stop_code,    wrk_date_s , " +
                          "                           wrk_time_s,   wrk_date_e,    wrk_time_e,    sub_minute,    sub_hour     ) " +
                          "     VALUES     (  @ureport_code, @ureport_date, @mo_code, @mac_code, @stop_code, @wrk_date_s, "+
                          "                       @wrk_time_s,     @wrk_date_e,    @wrk_time_e,    @sub_minute,    @sub_hour ) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MET04_0500);
            }
        }

        /// <summary>
        /// 傳入一個MET04_0500的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name = "MET04_0500" > DTO </ param >
        public void UpdateData(MET04_0500 MET04_0500)
        {

            string sSql = " UPDATE MET04_0500                     " +
                          "    SET ureport_date =  @ureport_date, " +
                          "        mo_code     =  @mo_code,     " +
                          "        mac_code      =  @mac_code,      " +
                          "        stop_code     =  @stop_code,     " +
                          "        wrk_date_s       =  @wrk_date_s ,       " +
                          "        wrk_time_s       =  @wrk_time_s ,       " +
                          "        wrk_date_e       =  @wrk_date_e ,       " +
                          "        wrk_time_e       =  @wrk_time_e ,       " +
                          "        sub_minute       =  @sub_minute ,       " +
                          "        sub_hour       =  @sub_hour       " +

                          "  WHERE ureport_code   =  @ureport_code    ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MET04_0500);

            }
        }

        //// <summary>
        //// 傳入一個鍵值，刪除、一次刪除一筆
        //// </summary>
        //// <param name = "pTkCode" > 資料鍵值 </ param >
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MET04_0500 WHERE ureport_code = @ureport_code;";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { ureport_code = pTkCode });
            }
        }
        ////暫存DataTable參考
        ////<summary>
        ////取得MET04_0500角色的DataTable
        ////</summary>
        ////<param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        ////<returns></returns>
        //public DataTable GetMET04_0500_dt(string pTkCode)
        //{
        //    DataTable dtTmp = new DataTable();
        //    DataTable dtDat = new DataTable();
        //    dtDat.Columns.Add("ureport_code", System.Type.GetType("System.String").ToString());
        //    dtDat.Columns.Add("ureport_code", System.Type.GetType("System.String").ToString());
        //    dtDat.Columns.Add("ureport_date", System.Type.GetType("System.String").ToString());
        //    string sSql = "";
        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM MET04_0500";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM MET04_0500 where ureport_code='" + pTkCode + "'";
        //    }
        //    dtTmp = comm.Get_DataTable(sSql);

        //    int i;
        //    for (i = 1; i < dtTmp.Rows.Count - 1; i++)
        //    {
        //        DataRow drow = dtDat.NewRow();
        //        drow["ureport_code"] = dtTmp.Rows[i]["ureport_code"];
        //        drow["ureport_code"] = dtTmp.Rows[i]["ureport_code"];
        //        drow["ureport_date"] = dtTmp.Rows[i]["ureport_date"];
        //        dtDat.Rows.Add(drow);
        //    }
        //    return dtDat;
        //}
    }
}