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
    public class WMT06_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得WMT06_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO WMT06_0000</returns>
        public WMT06_0000 GetDTO(string pTkCode)
        {
            WMT06_0000 datas = new WMT06_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM WMT06_0000";
            }
            else
            {
                sSql = "SELECT * FROM WMT06_0000 where prepare_code=@prepare_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@prepare_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new WMT06_0000
                        {
                            prepare_code = comm.sGetString(reader["prepare_code"].ToString()),
                            prepare_date = comm.sGetString(reader["prepare_date"].ToString()),
                            line_code = comm.sGetString(reader["line_code"].ToString()),
                            prepare_status = comm.sGetString(reader["prepare_status"].ToString()),
                            end_date = comm.sGetString(reader["end_date"].ToString()),
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
        /// 取得WMT06_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List WMT06_0000</returns>
        public List<WMT06_0000> Get_DataList(string pTkCode)
        {
            List<WMT06_0000> list = new List<WMT06_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM WMT06_0000";
            }
            else
            {
                sSql = "SELECT * FROM WMT06_0000 where prepare_code=@prepare_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@prepare_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    WMT06_0000 data = new WMT06_0000();

                    data.prepare_code = comm.sGetString(reader["prepare_code"].ToString());
                    data.prepare_date = comm.sGetString(reader["prepare_date"].ToString());
                    data.line_code = comm.sGetString(reader["line_code"].ToString());
                    data.prepare_status = comm.sGetString(reader["prepare_status"].ToString());
                    data.end_date = comm.sGetString(reader["end_date"].ToString());
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
        public List<WMT06_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_prepare_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<WMT06_0000> list = new List<WMT06_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = " SELECT * FROM WMT06_0000 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@prepare_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    WMT06_0000 data = new WMT06_0000();

                    data.prepare_code = comm.sGetString(reader["prepare_code"].ToString());
                    data.prepare_date = comm.sGetString(reader["prepare_date"].ToString());
                    data.line_code = comm.sGetString(reader["line_code"].ToString());
                    data.prepare_status = comm.sGetString(reader["prepare_status"].ToString());
                    data.end_date = comm.sGetString(reader["end_date"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.prepare_code)) {
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
        public List<WMT06_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<WMT06_0000> list = new List<WMT06_0000>();

            string sSql = " SELECT distinct WMT06_0000.*, MEB12_0000.line_name, BDP21_0100.field_name as prepare_status_name " +
                          " FROM WMT06_0000 " +
                          " left join WMT06_0100 on WMT06_0100.prepare_code = WMT06_0000.prepare_code " +
                          " left join MEB12_0000 on MEB12_0000.line_code = WMT06_0000.line_code " +
                          " left join BDP21_0100 on BDP21_0100.field_code = WMT06_0000.prepare_status and BDP21_0100.code_code = 'prepare_status' " ;

            // 取得資料
            list = comm.Get_ListByQuery<WMT06_0000>(sSql, pWhere, pUsrCode, pPrgCode);

            // 權限設定
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            //string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_mtp_code", "par_name", "par_value");
            //var arr_LockGrpCode = sLockGrpCode.Split(',');

            for (int i = 0; i < list.Count; i++)
            {
                list[i].cancel = Chk_CanDeletePrepare(list[i].prepare_code);

                //檢查授權刪除、修改
                list[i].can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                list[i].can_update = sLimitStr.Contains("M") ? "Y" : "N";

                //        // 特例 轉換
                //        data.sup_name = data.ins_date + " - " + comm.sGetString(reader["sup_name"].ToString());
                //        data.prepare_date = comm.sGetString(reader["prepare_code"].ToString()) + " - " + comm.sGetString(reader["prepare_date"].ToString());

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
        /// 檢查是否可以取消備料單
        /// </summary>
        /// <param name="pPrepareCode"></param>
        /// <returns></returns>
        public string Chk_CanDeletePrepare(string pPrepareCode) {
            string sValue = "";
            string sSql = "select * from WMT06_0110" +
                          "  left join WMT06_0100 on WMT06_0110.wmt06_0100 = WMT06_0100.wmt06_0100 " +
                          " where prepare_code = '" + pPrepareCode + "'";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count <= 0) {
                sValue = "<a id='DeletePrepare' href='/WMT060A/DeletePrepare?K=" + pPrepareCode + "'><i class='ace-icon fa fa-times bigger-150 red'></i></a>";
            }            
            return sValue;
        }

        /// <summary>
        /// 傳入一個WMT06_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="WMT06_0000">DTO</param>
        public void InsertData(WMT06_0000 WMT06_0000)
        {
            string sSql = " INSERT INTO " +
                          " WMT06_0000 (  prepare_code,  prepare_date,  prepare_status,  end_date,  ins_date,  ins_time,  usr_code ) " +
                          "     VALUES ( @prepare_code, @prepare_date, @prepare_status, @end_date, @ins_date, @ins_time, @usr_code ) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMT06_0000);
            }
        }

        /// <summary>
        /// 傳入一個WMT06_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="WMT06_0000">DTO</param>
        public void UpdateData(WMT06_0000 WMT06_0000)
        {
            string sSql = " UPDATE WMT06_0000                         " +
                          "    SET prepare_date   =  @prepare_date,   " +
                          "        prepare_status =  @prepare_status, " +
                          "        end_date       =  @end_date,       " +
                          "        ins_date       =  @ins_date,       " +
                          "        ins_time       =  @ins_time,       " +
                          "        usr_code       =  @usr_code        " +
                          "  WHERE prepare_code   =  @prepare_code    " ;

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMT06_0000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@prepare_code", WMT06_0000.prepare_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@prepare_code", WMT06_0000.prepare_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@prepare_date", WMT06_0000.prepare_date));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM WMT06_0000 WHERE prepare_code = @prepare_code; " +
                          "DELETE FROM WMT06_0100 WHERE prepare_code = @prepare_code; " ;
            //sSql += " Delete from BDP09_0100 where prepare_code = @prepare_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { prepare_code = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@prepare_code", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        ////暫存DataTable參考
        //// <summary>
        //// 取得WMT06_0000角色的DataTable
        //// </summary>
        //// <param name = "pTkCode" > 有傳鍵值取一筆，鍵值空白取全部</param>
        //// <returns></returns>
        //public DataTable GetWMT06_0000_dt(string pTkCode)
        //{
        //    DataTable dtTmp = new DataTable();
        //    DataTable dtDat = new DataTable();
        //    dtDat.Columns.Add("prepare_code", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("prepare_code", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("prepare_date", System.Type.GetType("System.String"].ToString());

        //    string sSql = "";
        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM WMT06_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM WMT06_0000 where prepare_code='" + pTkCode + "'";
        //    }
        //    dtTmp = comm.Get_DataTable(sSql);

        //    int i;
        //    for (i = 1; i < dtTmp.Rows.Count - 1; i++)
        //    {
        //        DataRow drow = dtDat.NewRow();
        //        drow["prepare_code"] = dtTmp.Rows[i]["prepare_code"];
        //        drow["prepare_code"] = dtTmp.Rows[i]["prepare_code"];
        //        drow["prepare_date"] = dtTmp.Rows[i]["prepare_date"];
        //        dtDat.Rows.Add(drow);
        //    }
        //    return dtDat;
        //}
    }
}