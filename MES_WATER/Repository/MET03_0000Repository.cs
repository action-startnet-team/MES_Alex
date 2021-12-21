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
    public class MET03_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MET03_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MET03_0000</returns>
        public MET03_0000 GetDTO(string pTkCode)
        {
            MET03_0000 datas = new MET03_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MET03_0000";
            }
            else
            {
                sSql = "SELECT * FROM MET03_0000 where wrk_code=@wrk_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@wrk_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MET03_0000
                        {

                            wrk_code = comm.sGetString(reader["wrk_code"].ToString()),
                            mo_code = comm.sGetString(reader["mo_code"].ToString()),
                            wrk_date = comm.sGetString(reader["wrk_date"].ToString()),
                            pro_code = comm.sGetString(reader["pro_code"].ToString()),
                            pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString()),
                            //pro_unit = comm.sGetString(reader["pro_unit"].ToString()),
                            work_code = comm.sGetString(reader["work_code"].ToString()),
                            station_code = comm.sGetString(reader["station_code"].ToString()),
                            mo_status = comm.sGetString(reader["mo_status"].ToString()),
                            ins_date = comm.sGetString(reader["ins_date"].ToString()),
                            ins_time = comm.sGetString(reader["ins_time"].ToString()),
                            usr_code = comm.sGetString(reader["usr_code"].ToString()),

                        };
                    }
                }
            }
            return datas;
        }

        /// <summary>
        /// 取得MET03_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MET03_0000</returns>
        public List<MET03_0000> Get_DataList(string pTkCode)
        {
            List<MET03_0000> list = new List<MET03_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MET03_0000";
            }
            else
            {
                sSql = "SELECT * FROM MET03_0000 where wrk_code=@wrk_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@wrk_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MET03_0000 data = new MET03_0000();

                    data.wrk_code = comm.sGetString(reader["wrk_code"].ToString());
                    data.mo_code = comm.sGetString(reader["mo_code"].ToString());
                    data.wrk_date = comm.sGetString(reader["wrk_date"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString());
                    //data.pro_unit = comm.sGetString(reader["pro_unit"].ToString());
                    data.work_code = comm.sGetString(reader["work_code"].ToString());
                    data.station_code = comm.sGetString(reader["station_code"].ToString());
                    data.mo_status = comm.sGetString(reader["mo_status"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());

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
        public List<MET03_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_wrk_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MET03_0000> list = new List<MET03_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = " SELECT * FROM MET03_0000 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MET03_0000 data = new MET03_0000();

                    data.wrk_code = comm.sGetString(reader["wrk_code"].ToString());
                    data.mo_code = comm.sGetString(reader["mo_code"].ToString());
                    data.wrk_date = comm.sGetString(reader["wrk_date"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString());
                    //data.pro_unit = comm.sGetString(reader["pro_unit"].ToString());
                    data.work_code = comm.sGetString(reader["work_code"].ToString());
                    data.station_code = comm.sGetString(reader["station_code"].ToString());
                    data.mo_status = comm.sGetString(reader["mo_status"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.wrk_code)) {
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
        public List<MET03_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MET03_0000> list = new List<MET03_0000>();

            //string sSql = " SELECT distinct MET03_0000.wrk_code, MET03_0000.*, MEB20_0000.pro_name, MEB30_0000.work_name, MEB29_0000.station_name, BDP21_0100.field_name as mo_status_name " +
            //              " FROM MET03_0000 " +
            //              " left join MET03_0100 on MET03_0100.wrk_code = MET03_0000.wrk_code " +
            //              " left join MEB20_0000 on MEB20_0000.pro_code = MET03_0000.pro_code " +
            //              " left join MEB30_0000 on MEB30_0000.work_code = MET03_0000.work_code " +
            //              " left join MEB29_0000 on MEB29_0000.station_code = MET03_0000.station_code " +
            //              " left join BDP21_0100 on BDP21_0100.field_code = MET03_0000.mo_status and BDP21_0100.code_code = 'mo_status' ";

            string sSql = " SELECT distinct MET03_0000.mo_code,MET03_0000.wrk_date,MET03_0000.pro_code , MEB20_0000.pro_unit,  MEB20_0000.pro_name, BDP21_0100.field_name as mo_status_name   " +
                          " FROM MET03_0000  " +
                          " left join MET03_0100 on MET03_0100.wrk_code = MET03_0000.wrk_code  " +
                          " left join MEB20_0000 on MEB20_0000.pro_code = MET03_0000.pro_code  " +
                          " left join MEB30_0000 on MEB30_0000.work_code = MET03_0000.work_code  " +
                          " left join MEB29_0000 on MEB29_0000.station_code = MET03_0000.station_code   " +
                          " left join BDP21_0100 on BDP21_0100.field_code = MET03_0000.mo_status and BDP21_0100.code_code = 'mo_status'   ";
            // " group by MET03_0000.mo_code, MEB20_0000.pro_name, MEB29_0000.station_name,BDP21_0100.field_name   ";

            //sSql += " Where MET01_0000.mo_status='10'";

            //comm.Ins_BDP20_0000("ivan", "MET030A", "FUN", sSql);

            // 取得資料
            list = comm.Get_ListByQuery<MET03_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
                //        data.sup_name = data.pro_qty + " - " + comm.sGetString(reader["sup_name"].ToString());
                //        data.mo_code = comm.sGetString(reader["wrk_code"].ToString()) + " - " + comm.sGetString(reader["mo_code"].ToString());

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
        /// 傳入一個MET03_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MET03_0000">DTO</param>
        public void InsertData(MET03_0000 MET03_0000)
        {
            string sSql = " INSERT INTO " +
                          " MET03_0000 (  wrk_code,  mo_code,  wrk_date,  pro_code,  pro_qty,  pro_unit,  work_code,  station_code,  mo_status,  ins_date,  ins_time,  usr_code ) " +
                          "     VALUES ( @wrk_code, @mo_code, @wrk_date, @pro_code, @pro_qty, @pro_unit, @work_code, @station_code, @mo_status, @ins_date, @ins_time, @usr_code ) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MET03_0000);
            }
        }

        /// <summary>
        /// 傳入一個MET03_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MET03_0000">DTO</param>
        public void UpdateData(MET03_0000 MET03_0000)
        {
            string sSql = " UPDATE MET03_0000                     " +
                          "    SET mo_code      =  @mo_code,      " +
                          "        wrk_date     =  @wrk_date,     " +
                          "        pro_code     =  @pro_code,     " +
                          "        pro_qty      =  @pro_qty,      " +
                          "        pro_unit     =  @pro_unit,     " +
                          "        work_code    =  @work_code,    " +
                          "        station_code =  @station_code, " +
                          "        mo_status    =  @mo_status,    " +
                          "        ins_date     =  @ins_date,     " +
                          "        ins_time     =  @ins_time,     " +
                          "        usr_code     =  @usr_code      " +
                          "  WHERE wrk_code     =  @wrk_code      " ;

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MET03_0000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@wrk_code", MET03_0000.wrk_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@wrk_code", MET03_0000.wrk_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@mo_code", MET03_0000.mo_code));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MET03_0000 WHERE wrk_code = @wrk_code;" +
                          "DELETE FROM MET03_0100 WHERE wrk_code = @wrk_code;";
            //sSql += " Delete from BDP09_0100 where wrk_code = @wrk_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { wrk_code = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@wrk_code", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        ////暫存DataTable參考
        //// <summary>
        //// 取得MET03_0000角色的DataTable
        //// </summary>
        //// <param name = "pTkCode" > 有傳鍵值取一筆，鍵值空白取全部</param>
        //// <returns></returns>
        //public DataTable GetMET03_0000_dt(string pTkCode)
        //{
        //    DataTable dtTmp = new DataTable();
        //    DataTable dtDat = new DataTable();
        //    dtDat.Columns.Add("wrk_code", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("wrk_code", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("mo_code", System.Type.GetType("System.String"].ToString());

        //    string sSql = "";
        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM MET03_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM MET03_0000 where wrk_code='" + pTkCode + "'";
        //    }
        //    dtTmp = comm.Get_DataTable(sSql);

        //    int i;
        //    for (i = 1; i < dtTmp.Rows.Count - 1; i++)
        //    {
        //        DataRow drow = dtDat.NewRow();
        //        drow["wrk_code"] = dtTmp.Rows[i]["wrk_code"];
        //        drow["wrk_code"] = dtTmp.Rows[i]["wrk_code"];
        //        drow["mo_code"] = dtTmp.Rows[i]["mo_code"];
        //        dtDat.Rows.Add(drow);
        //    }
        //    return dtDat;
        //}
    }
}