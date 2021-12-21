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
    public class MET04_0200Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MET04_0200資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MET04_0200</returns>
        public MET04_0200 GetDTO(string pTkCode)
        {
            MET04_0200 datas = new MET04_0200();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MET04_0200";
            }
            else
            {
                sSql = "SELECT * FROM MET04_0200 where ureport_code=@ureport_code";
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
                        datas = new MET04_0200
                        {

                            ureport_code = comm.sGetString(reader["ureport_code"].ToString()),
                            ureport_date = comm.sGetString(reader["ureport_date"].ToString()),
                            mo_code = comm.sGetString(reader["mo_code"].ToString()),
                            up_type = comm.sGetString(reader["up_type"].ToString()),
                            pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString()),
                            pro_unit = comm.sGetString(reader["pro_unit"].ToString()),
                            stop_code = comm.sGetString(reader["stop_code"].ToString()),

                            pro_date_s = comm.sGetString(reader["pro_date_s"].ToString()),
                            pro_time_s = comm.sGetString(reader["pro_time_s"].ToString()),
                            pro_date_e = comm.sGetString(reader["pro_date_e"].ToString()),
                            pro_time_e = comm.sGetString(reader["pro_time_e"].ToString()),
                            ISM01 = comm.sGetDecimal(reader["ISM01"].ToString()),
                            ISM02 = comm.sGetDecimal(reader["ISM02"].ToString()),
                            ISM03 = comm.sGetDecimal(reader["ISM03"].ToString()),
                            ISM04 = comm.sGetDecimal(reader["ISM04"].ToString()),
                            ISM05 = comm.sGetDecimal(reader["ISM05"].ToString()),
                            ISM06 = comm.sGetDecimal(reader["ISM06"].ToString()),
                            ins_date = comm.sGetString(reader["ins_date"].ToString()),
                            ins_time = comm.sGetString(reader["ins_time"].ToString()),
                            usr_code = comm.sGetString(reader["usr_code"].ToString()),
                            sap_code = comm.sGetString(reader["sap_code"].ToString()),
                            sap_no = comm.sGetString(reader["sap_no"].ToString()),
                            sap_message = comm.sGetString(reader["sap_message"].ToString()),
                            sap_scr_no = comm.sGetInt32(reader["sap_scr_no"].ToString()),
                            is_ok = comm.sGetString(reader["is_ok"].ToString()),
                            is_del = comm.sGetString(reader["is_del"].ToString()),

                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MET04_0200資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MET04_0200</returns>
        public List<MET04_0200> Get_DataList(string pTkCode)
        {
            List<MET04_0200> list = new List<MET04_0200>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MET04_0200";
            }
            else
            {
                sSql = "SELECT * FROM MET04_0200 where ureport_code=@ureport_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@ureport_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MET04_0200 data = new MET04_0200();

                    data.ureport_code = comm.sGetString(reader["ureport_code"].ToString());
                    data.ureport_date = comm.sGetString(reader["ureport_date"].ToString());
                    data.mo_code = comm.sGetString(reader["mo_code"].ToString());
                    data.up_type = comm.sGetString(reader["up_type"].ToString());
                    data.pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString());
                    data.pro_unit = comm.sGetString(reader["pro_unit"].ToString());
                    data.stop_code = comm.sGetString(reader["stop_code"].ToString());
                    data.pro_date_s = comm.sGetString(reader["pro_date_s"].ToString());
                    data.pro_time_s = comm.sGetString(reader["pro_time_s"].ToString());
                    data.pro_date_e = comm.sGetString(reader["pro_date_e"].ToString());
                    data.pro_time_e = comm.sGetString(reader["pro_time_e"].ToString());
                    data.ISM01 = comm.sGetDecimal(reader["ISM01"].ToString());
                    data.ISM02 = comm.sGetDecimal(reader["ISM02"].ToString());
                    data.ISM03 = comm.sGetDecimal(reader["ISM03"].ToString());
                    data.ISM04 = comm.sGetDecimal(reader["ISM04"].ToString());
                    data.ISM05 = comm.sGetDecimal(reader["ISM05"].ToString());
                    data.ISM06 = comm.sGetDecimal(reader["ISM06"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                    data.sap_code = comm.sGetString(reader["sap_code"].ToString());
                    data.sap_no = comm.sGetString(reader["sap_no"].ToString());
                    data.sap_message = comm.sGetString(reader["sap_message"].ToString());
                    data.sap_scr_no = comm.sGetInt32(reader["sap_scr_no"].ToString());
                    data.is_ok = comm.sGetString(reader["is_ok"].ToString());
                    data.is_del = comm.sGetString(reader["is_del"].ToString());

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
        public List<MET04_0200> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_ureport_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MET04_0200> list = new List<MET04_0200>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = "SELECT * FROM MET04_0200";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MET04_0200 data = new MET04_0200();

                    data.ureport_code = comm.sGetString(reader["ureport_code"].ToString());
                    data.ureport_date = comm.sGetString(reader["ureport_date"].ToString());
                    data.mo_code = comm.sGetString(reader["mo_code"].ToString());
                    data.up_type = comm.sGetString(reader["up_type"].ToString());
                    data.pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString());
                    data.pro_unit = comm.sGetString(reader["pro_unit"].ToString());
                    data.stop_code = comm.sGetString(reader["stop_code"].ToString());
                    data.pro_date_s = comm.sGetString(reader["pro_date_s"].ToString());
                    data.pro_time_s = comm.sGetString(reader["pro_time_s"].ToString());
                    data.pro_date_e = comm.sGetString(reader["pro_date_e"].ToString());
                    data.pro_time_e = comm.sGetString(reader["pro_time_e"].ToString());
                    data.ISM01 = comm.sGetDecimal(reader["ISM01"].ToString());
                    data.ISM02 = comm.sGetDecimal(reader["ISM02"].ToString());
                    data.ISM03 = comm.sGetDecimal(reader["ISM03"].ToString());
                    data.ISM04 = comm.sGetDecimal(reader["ISM04"].ToString());
                    data.ISM05 = comm.sGetDecimal(reader["ISM05"].ToString());
                    data.ISM06 = comm.sGetDecimal(reader["ISM06"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                    data.sap_code = comm.sGetString(reader["sap_code"].ToString());
                    data.sap_no = comm.sGetString(reader["sap_no"].ToString());
                    data.sap_message = comm.sGetString(reader["sap_message"].ToString());
                    data.sap_scr_no = comm.sGetInt32(reader["sap_scr_no"].ToString());
                    data.is_ok = comm.sGetString(reader["is_ok"].ToString());
                    data.is_del = comm.sGetString(reader["is_del"].ToString());

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
        public List<MET04_0200> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MET04_0200> list = new List<MET04_0200>();

            string sSql = " SELECT * FROM MET04_0200 ";

            // 取得資料
            list = comm.Get_ListByQuery<MET04_0200>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個MET04_0200的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MET04_0200">DTO</param>
        public void InsertData(MET04_0200 MET04_0200)
        {
            string sSql = "INSERT INTO " +
                          " MET04_0200 (  ureport_code,  ureport_date,  mo_code, up_type, pro_qty, pro_unit, stop_code, pro_date_s,  pro_time_s,  pro_date_e,  pro_time_e, " +
                          "               ISM01,  ISM02,  ISM03,  ISM04,  ISM05,  ISM06,  ins_date,  ins_time,  usr_code,  sap_code, " +
                          "               sap_no,  sap_message,  sap_scr_no,  is_ok,  is_del )   " +
                          "     VALUES ( @ureport_code, @ureport_date, @mo_code, @up_type, @pro_qty, @pro_unit, @stop_code, @pro_date_s, @pro_time_s, @pro_date_e, @pro_time_e, " +
                          "              @ISM01, @ISM02, @ISM03, @ISM04, @ISM05, @ISM06, @ins_date, @ins_time, @usr_code, @sap_code, " +
                          "              @sap_no, @sap_message, @sap_scr_no, @is_ok, @is_del )   ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MET04_0200);
            }
        }

        /// <summary>
        /// 傳入一個MET04_0200的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MET04_0200">DTO</param>
        public void UpdateData(MET04_0200 MET04_0200)
        {
            string sSql = " UPDATE MET04_0200                     " +
                          "    SET ureport_date =  @ureport_date, " +
                          "        mo_code      =  @mo_code,      " +
                          "        up_type   =  @up_type,   " +
                          "        pro_qty   =  @pro_qty,   " +
                          "        pro_unit   =  @pro_unit,   " +
                          "        stop_code   =  @stop_code,   " +
                          "        pro_date_s   =  @pro_date_s,   " +
                          "        pro_time_s   =  @pro_time_s,   " +
                          "        pro_date_e   =  @pro_date_e,   " +
                          "        pro_time_e   =  @pro_time_e,   " +
                          "        ISM01        =  @ISM01,        " +
                          "        ISM02        =  @ISM02,        " +
                          "        ISM03        =  @ISM03,        " +
                          "        ISM04        =  @ISM04,        " +
                          "        ISM05        =  @ISM05,        " +
                          "        ISM06        =  @ISM06,        " +
                          "        ins_date     =  @ins_date,     " +
                          "        ins_time     =  @ins_time,     " +
                          "        usr_code     =  @usr_code,     " +
                          "        sap_code     =  @sap_code,     " +
                          "        sap_no       =  @sap_no,       " +
                          "        sap_message  =  @sap_message,  " +
                          "        sap_scr_no   =  @sap_scr_no,   " +
                          "        is_ok        =  @is_ok,        " +
                          "        is_del       =  @is_del        " +
                          "  WHERE ureport_code =  @ureport_code  ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MET04_0200);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MET04_0200 WHERE ureport_code = @ureport_code";
            //sSql += " Delete from BDP09_0100 where ureport_code = @ureport_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { ureport_code = pTkCode });
            }
        }

    }
}