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
    public class DTS01_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得DTS01_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO DTS01_0000</returns>
        public DTS01_0000 GetDTO(string pTkCode)
        {
            DTS01_0000 datas = new DTS01_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT  * FROM DTS01_0000";
            }
            else
            {
                sSql = "SELECT * FROM DTS01_0000 where dts01_0000=@dts01_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@dts01_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new DTS01_0000
                        {

                            dts01_0000 = comm.sGetString(reader["dts01_0000"].ToString()),
                            con_code = comm.sGetString(reader["con_code"].ToString()),
                            con_type = comm.sGetString(reader["con_type"].ToString()),
                            con_function = comm.sGetString(reader["con_function"].ToString()),
                            con_request = comm.sGetString(reader["con_request"].ToString()),
                            result = comm.sGetString(reader["result"].ToString()),
                            message = comm.sGetString(reader["message"].ToString()),
                            ins_date = comm.sGetString(reader["ins_date"].ToString()),
                            ins_time = comm.sGetString(reader["ins_time"].ToString()),
                            usr_code = comm.sGetString(reader["usr_code"].ToString()),
                            sch_date = comm.sGetString(reader["sch_date"].ToString()),
                            sch_time = comm.sGetString(reader["sch_time"].ToString()),
                            sch_usr_code = comm.sGetString(reader["sch_usr_code"].ToString()),
                            data_flag = comm.sGetString(reader["data_flag"].ToString()),

                        };
                    }
                }
            }
            return datas;
        }
        
        /// <summary>
        /// 取得DTS01_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List DTS01_0000</returns>
        public List<DTS01_0000> Get_DataList(string pTkCode)
        {
            List<DTS01_0000> list = new List<DTS01_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM DTS01_0000";
            }
            else
            {
                sSql = "SELECT * FROM DTS01_0000 where dts01_0000=@dts01_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@dts01_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    DTS01_0000 data = new DTS01_0000();

                    data.dts01_0000 = comm.sGetString(reader["dts01_0000"].ToString());
                    data.con_code = comm.sGetString(reader["con_code"].ToString());
                    data.con_type = comm.sGetString(reader["con_type"].ToString());
                    data.con_function = comm.sGetString(reader["con_function"].ToString());
                    data.con_request = comm.sGetString(reader["con_request"].ToString());
                    data.result = comm.sGetString(reader["result"].ToString());
                    data.message = comm.sGetString(reader["message"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                    data.sch_date = comm.sGetString(reader["sch_date"].ToString());
                    data.sch_time = comm.sGetString(reader["sch_time"].ToString());
                    data.sch_usr_code = comm.sGetString(reader["sch_usr_code"].ToString());
                    data.data_flag = comm.sGetString(reader["data_flag"].ToString());

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
        public List<DTS01_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_dts01_0000", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<DTS01_0000> list = new List<DTS01_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = " SELECT * FROM DTS01_0000 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);   
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@dts01_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    DTS01_0000 data = new DTS01_0000();

                    data.dts01_0000 = comm.sGetString(reader["dts01_0000"].ToString());
                    data.con_code = comm.sGetString(reader["con_code"].ToString());
                    data.con_type = comm.sGetString(reader["con_type"].ToString());
                    data.con_function = comm.sGetString(reader["con_function"].ToString());
                    data.con_request = comm.sGetString(reader["con_request"].ToString());
                    data.result = comm.sGetString(reader["result"].ToString());
                    data.message = comm.sGetString(reader["message"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                    data.sch_date = comm.sGetString(reader["sch_date"].ToString());
                    data.sch_time = comm.sGetString(reader["sch_time"].ToString());
                    data.sch_usr_code = comm.sGetString(reader["sch_usr_code"].ToString());
                    data.data_flag = comm.sGetString(reader["data_flag"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.dts01_0000)) {
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
        public List<DTS01_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<DTS01_0000> list = new List<DTS01_0000>();/*, B.field_name as loc_type_name*/

            string sSql = " SELECT DTS01_0000.dts01_0000, DTS01_0000.*, BDPcon.field_name as con_type , BDPdata.field_name as data_flag " +
                          " FROM DTS01_0000 " +
                          " left join DTS01_0100 on DTS01_0100.dts01_0000 = DTS01_0000.dts01_0000 " +
                          " left join BDP21_0100 as BDPcon  on BDPcon.field_code = DTS01_0000.con_type and BDPcon.code_code = 'con_type' " + 
                          " left join BDP21_0100 as BDPdata on BDPdata.field_code = DTS01_0000.data_flag and BDPdata.code_code = 'data_flag' "; 

            // 取得資料
            list = comm.Get_ListByQuery<DTS01_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
                //        data.sup_name = data.con_function + " - " + comm.sGetString(reader["sup_name"].ToString());
                //        data.con_code = comm.sGetString(reader["dts01_0000"].ToString()) + " - " + comm.sGetString(reader["con_code"].ToString());

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
        /// 傳入一個DTS01_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="DTS01_0000">DTO</param>
        public void InsertData(DTS01_0000 DTS01_0000)
        {
            string sSql = " INSERT INTO " +
                          " DTS01_0000 (  dts01_0000,   con_code,  con_type,  con_function,  " +
                          "               con_request,  result,    message,   ins_date,      " +
                          "               ins_time,     usr_code,  sch_date,  sch_time,      " +
                          "               sch_usr_code, data_flag  ) " +

                          "     VALUES (  @dts01_0000,   @con_code,  @con_type,  @con_function,  " +
                          "               @con_request,  @result,    @message,   @ins_date,      " +
                          "               @ins_time,     @usr_code,  @sch_date,  @sch_time,      " +
                          "               @sch_usr_code, @data_flag  ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, DTS01_0000);
            }
        }

        /// <summary>
        /// 傳入一個DTS01_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="DTS01_0000">DTO</param>
        public void UpdateData(DTS01_0000 DTS01_0000)
        {
            string sSql = " UPDATE DTS01_0000                       " +
                          "    SET con_code     =  @con_code,       " +
                          "        con_type     =  @con_type,       " +
                          "        con_function =  @con_function,   " +
                          "        con_request  =  @con_request,    " +
                          "        result       =  @result,         " +
                          "        message      =  @message,     " +
                          "        ins_date     =  @ins_date,    " +
                          "        ins_time     =  @ins_time,    " +
                          "        usr_code     =  @usr_code,    " +
                          "        sch_date     =  @sch_date,    " +
                          "        sch_time     =  @sch_time,    " +
                          "        sch_usr_code =  @sch_usr_code " +

                          "  WHERE dts01_0000 =  @dts01_0000      " ;
            
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, DTS01_0000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@dts01_0000", DTS01_0000.dts01_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@dts01_0000", DTS01_0000.dts01_0000));
                //sqlCommand.Parameters.Add(new SqlParameter("@con_code", DTS01_0000.con_code));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM DTS01_0000 WHERE dts01_0000 = @dts01_0000;";
            //sSql += " Delete from BDP09_0100 where dts01_0000 = @dts01_0000; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { dts01_0000 = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@dts01_0000", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        ////暫存DataTable參考
        //// <summary>
        //// 取得DTS01_0000角色的DataTable
        //// </summary>
        //// <param name = "pTkCode" > 有傳鍵值取一筆，鍵值空白取全部</param>
        //// <returns></returns>
        //public DataTable GetDTS01_0000_dt(string pTkCode)
        //{
        //    DataTable dtTmp = new DataTable();
        //    DataTable dtDat = new DataTable();
        //    dtDat.Columns.Add("dts01_0000", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("dts01_0000", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("con_code", System.Type.GetType("System.String"].ToString());

        //    string sSql = "";
        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM DTS01_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM DTS01_0000 where dts01_0000='" + pTkCode + "'";
        //    }
        //    dtTmp = comm.Get_DataTable(sSql);

        //    int i;
        //    for (i = 1; i < dtTmp.Rows.Count - 1; i++)
        //    {
        //        DataRow drow = dtDat.NewRow();
        //        drow["dts01_0000"] = dtTmp.Rows[i]["dts01_0000"];
        //        drow["dts01_0000"] = dtTmp.Rows[i]["dts01_0000"];
        //        drow["con_code"] = dtTmp.Rows[i]["con_code"];
        //        dtDat.Rows.Add(drow);
        //    }
        //    return dtDat;
        //}

        public DTS01_0000 newData(string function, string usr)
        {
            DTS01_0000 DTS01_0000 = new DTS01_0000();
            DTS01_0000.dts01_0000 = comm.Get_Guid();
            DTS01_0000.con_code = "RFC";
            DTS01_0000.con_type = "B";
            DTS01_0000.con_function = function;
            DTS01_0000.con_request = "";
            DTS01_0000.result = "";
            DTS01_0000.message = "";
            DTS01_0000.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
            DTS01_0000.ins_time = DateTime.Now.ToString("HH:mm:ss");
            DTS01_0000.usr_code = usr;
            DTS01_0000.sch_date = "";
            DTS01_0000.sch_time = "";
            DTS01_0000.sch_usr_code = usr;
            DTS01_0000.data_flag = "N";
            return DTS01_0000;
        }
    }
}