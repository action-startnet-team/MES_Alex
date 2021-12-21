﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

using MES_WATER.Models;
using Dapper;


namespace MES_WATER.Repository
{
    public class MED06_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MED06_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MED06_0000</returns>
        public MED06_0000 GetDTO(string pTkCode)
        {
            MED06_0000 datas = new MED06_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MED06_0000";
            }
            else
            {
                sSql = "SELECT * FROM MED06_0000 where med06_0000=@med06_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@med06_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MED06_0000
                        {
                            med06_0000 = comm.sGetInt32(reader["med06_0000"].ToString()),
                            mo_code = comm.sGetString(reader["mo_code"].ToString()),
                            wrk_code = comm.sGetString(reader["wrk_code"].ToString()),
                            work_code = comm.sGetString(reader["work_code"].ToString()),
                            station_code = comm.sGetString(reader["station_code"].ToString()),
                            mac_code = comm.sGetString(reader["mac_code"].ToString()),
                            pro_code = comm.sGetString(reader["pro_code"].ToString()),
                            lot_no = comm.sGetString(reader["lot_no"].ToString()),
                            pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString()),
                            pro_unit = comm.sGetString(reader["pro_unit"].ToString()),
                            loc_code = comm.sGetString(reader["loc_code"].ToString()),
                            ins_date = comm.sGetString(reader["ins_date"].ToString()),
                            ins_time = comm.sGetString(reader["ins_time"].ToString()),
                            usr_code = comm.sGetString(reader["usr_code"].ToString()),
                            des_memo = comm.sGetString(reader["des_memo"].ToString()),
                            is_ng = comm.sGetString(reader["is_ng"].ToString()),
                            is_end = comm.sGetString(reader["is_end"].ToString()),
                            end_memo = comm.sGetString(reader["end_memo"].ToString()),
                            end_date = comm.sGetString(reader["end_date"].ToString()),
                            end_time = comm.sGetString(reader["end_time"].ToString()),
                            end_usr_code = comm.sGetString(reader["end_usr_code"].ToString()),
                            pallet_code = comm.sGetString(reader["pallet_code"].ToString()),
                            user_field_01 = comm.sGetString(reader["user_field_01"].ToString()),
                            user_field_02 = comm.sGetString(reader["user_field_02"].ToString()),
                            user_field_03 = comm.sGetString(reader["user_field_03"].ToString()),
                            user_field_04 = comm.sGetString(reader["user_field_04"].ToString()),
                            user_field_05 = comm.sGetString(reader["user_field_05"].ToString()),
                            user_field_06 = comm.sGetString(reader["user_field_06"].ToString()),
                            user_field_07 = comm.sGetString(reader["user_field_07"].ToString()),
                            user_field_08 = comm.sGetString(reader["user_field_08"].ToString()),
                            user_field_09 = comm.sGetString(reader["user_field_09"].ToString()),
                            user_field_10 = comm.sGetString(reader["user_field_10"].ToString())
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MED06_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MED06_0000</returns>
        public List<MED06_0000> Get_DataList(string pTkCode)
        {
            List<MED06_0000> list = new List<MED06_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MED06_0000";
            }
            else
            {
                sSql = "SELECT * FROM MED06_0000 where med06_0000=@med06_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@med06_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MED06_0000 data = new MED06_0000();

                    data.med06_0000 = comm.sGetInt32(reader["med06_0000"].ToString());
                    data.mo_code = comm.sGetString(reader["mo_code"].ToString());
                    data.wrk_code = comm.sGetString(reader["wrk_code"].ToString());
                    data.mac_code = comm.sGetString(reader["mac_code"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.lot_no = comm.sGetString(reader["lot_no"].ToString());
                    data.pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString());
                    data.pro_unit = comm.sGetString(reader["pro_unit"].ToString());
                    data.loc_code = comm.sGetString(reader["loc_code"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                    data.des_memo = comm.sGetString(reader["des_memo"].ToString());
                    data.is_ng = comm.sGetString(reader["is_ng"].ToString());
                    data.is_end = comm.sGetString(reader["is_end"].ToString());
                    data.end_memo = comm.sGetString(reader["end_memo"].ToString());
                    data.end_date = comm.sGetString(reader["end_date"].ToString());
                    data.end_time = comm.sGetString(reader["end_time"].ToString());
                    data.end_usr_code = comm.sGetString(reader["end_usr_code"].ToString());

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
        public List<MED06_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_med06_0000", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MED06_0000> list = new List<MED06_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = "SELECT * FROM MED06_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MED06_0000 data = new MED06_0000();


                    data.med06_0000 = comm.sGetInt32(reader["med06_0000"].ToString());
                    data.mo_code = comm.sGetString(reader["mo_code"].ToString());
                    data.wrk_code = comm.sGetString(reader["wrk_code"].ToString());
                    data.mac_code = comm.sGetString(reader["mac_code"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.lot_no = comm.sGetString(reader["lot_no"].ToString());
                    data.pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString());
                    data.pro_unit = comm.sGetString(reader["pro_unit"].ToString());
                    data.loc_code = comm.sGetString(reader["loc_code"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                    data.des_memo = comm.sGetString(reader["des_memo"].ToString());
                    data.is_ng = comm.sGetString(reader["is_ng"].ToString());
                    data.is_end = comm.sGetString(reader["is_end"].ToString());
                    data.end_memo = comm.sGetString(reader["end_memo"].ToString());
                    data.end_date = comm.sGetString(reader["end_date"].ToString());
                    data.end_time = comm.sGetString(reader["end_time"].ToString());
                    data.end_usr_code = comm.sGetString(reader["end_usr_code"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.med06_0000)) {
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
        public List<MED06_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MED06_0000> list = new List<MED06_0000>();

            string sSql = " SELECT MED06_0000.*, MEB15_0000.mac_name, MEB20_0000.pro_name, WMB03_0000.pallet_name, " +
                          " MEB29_0000.station_name as station_name,"+
                          " MEB30_0000.work_name as work_name"+
                          " FROM MED06_0000 " +
                          " left join MEB29_0000 on MEB29_0000.station_code = MED06_0000.station_code"+
                          " left join MEB30_0000 on MEB30_0000.work_code = MED06_0000.work_code"+
                          " left join MEB15_0000 on MEB15_0000.mac_code = MED06_0000.mac_code " +
                          " left join MEB20_0000 on MEB20_0000.pro_code = MED06_0000.pro_code " +
                          " left join WMB03_0000 on WMB03_0000.pallet_code = MED06_0000.pallet_code ";

            // 取得資料
            list = comm.Get_ListByQuery<MED06_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個MED06_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MED06_0000">DTO</param>
        public void InsertData(MED06_0000 MED06_0000)
        {
            string sSql = "INSERT INTO " +
                          " MED06_0000 (  mo_code,   wrk_code,  work_code, station_code,  mac_code,  pro_code,  lot_no,  pro_qty, " +
                          "               pro_unit,  loc_code,  ins_date,  ins_time,  usr_code,  des_memo,  is_ng,  is_end,  end_memo, " +
                          "               end_date,  end_time,  end_usr_code,  pallet_code ) " +

                          "     VALUES ( @mo_code, @wrk_code, @work_code, @station_code ,@mac_code, @pro_code, @lot_no, @pro_qty, " +
                          "              @pro_unit, @loc_code, @ins_date, @ins_time, @usr_code, @des_memo, @is_ng, @is_end, @end_memo, " +
                          "              @end_date, @end_time, @end_usr_code, @pallet_code ) " ;

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MED06_0000);
            }
        }

        /// <summary>
        /// 傳入一個MED06_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MED06_0000">DTO</param>
        public void UpdateData(MED06_0000 MED06_0000)
        {
            string sSql = " UPDATE MED06_0000                     " +
                          "    SET mo_code       =  @mo_code,     " +
                          "        wrk_code      =  @wrk_code,    " +
                          "        work_code     =  @work_code,    " +
                          "        station_code  =  @station_code, " +
                          "        mac_code      =  @mac_code,    " +
                          "        pro_code      =  @pro_code,    " +
                          "        lot_no        =  @lot_no,      " +
                          "        pro_qty       =  @pro_qty,     " +
                          "        pro_unit      =  @pro_unit,    " +
                          "        loc_code      =  @loc_code,    " +
                          "        ins_date      =  @ins_date,    " +
                          "        ins_time      =  @ins_time,    " +
                          "        usr_code      =  @usr_code,    " +
                          "        des_memo      =  @des_memo,    " +
                          "        is_ng         =  @is_ng,       " +
                          "        is_end        =  @is_end,      " +
                          "        end_memo      =  @end_memo,    " +
                          "        end_date      =  @end_date,    " +
                          "        end_time      =  @end_time,    " +
                          "        end_usr_code  =  @end_usr_code," +
                          "        pallet_code   =  @pallet_code,  " +
                          "        user_field_01     =  @user_field_01,      " +
                          "        user_field_02     =  @user_field_02,      " +
                          "        user_field_03     =  @user_field_03,      " +
                          "        user_field_04     =  @user_field_04,      " +
                          "        user_field_05     =  @user_field_05,      " +
                          "        user_field_06     =  @user_field_06,      " +
                          "        user_field_07     =  @user_field_07,      " +
                          "        user_field_08     =  @user_field_08,      " +
                          "        user_field_09     =  @user_field_09,      " +
                          "        user_field_10     =  @user_field_10       " +
                          "  WHERE med06_0000    =  @med06_0000   ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MED06_0000);
                
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MED06_0000 WHERE med06_0000 = @med06_0000;";
            //sSql += " Delete from BDP09_0100 where med06_0000 = @med06_0000; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { med06_0000 = pTkCode });
                
            }
        }
        /// <summary>
        /// 傳入物料號、派工單號、數量，更新MEP02數量
        /// </summary>
        /// <param name="pProCode"></param>
        /// <param name="pWrkCode"></param>
        /// <param name="pProQty"></param>
        public void UpdataMEP02(string pProCode,string pWrkCode ,string pProQty) {
            string sSql = "Update MEP02_0000 set use_qty= use_qty +@use_qty,total_qty=total_qty+@total_qty "+
                          "where pro_code =@pro_code  and wrk_code =@wrk_code;";
            //sSql += " Delete from BDP09_0100 where med06_0000 = @med06_0000; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { use_qty  = pProQty,pro_code = pProCode ,wrk_code= pWrkCode, total_qty= pProQty });

            }

        }
        /// <summary>
        /// 傳入MED06索引值、結案時間，更新MED06結案時間
        /// </summary>
        /// <param name="MED06_0000"></param>
        /// <param name="pEndDate"></param>
        /// <param name="pEndTime"></param>
        public void UpdataMED06EndTime(string MED06_0000,string pEndDate,string pEndTime)
        {
            string sSql = "Update MED06_0000 set end_date= @end_date , end_time =@end_time " +
                          " where med06_0000=@med06_0000;";
            //sSql += " Delete from BDP09_0100 where med06_0000 = @med06_0000; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { end_date = pEndDate, end_time = pEndTime, med06_0000 = MED06_0000 });

            }

        }
    }
        
        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得MED06_0000角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetMED06_0000_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("med06_0000", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("med06_0000", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("sup_type_name", System.Type.GetType("System.String"].ToString());

            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MED06_0000";
            }
            else
            {
                sSql = "SELECT * FROM MED06_0000 where med06_0000='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["med06_0000"] = dtTmp.Rows[i]["med06_0000"];
                drow["med06_0000"] = dtTmp.Rows[i]["med06_0000"];
                drow["sup_type_name"] = dtTmp.Rows[i]["sup_type_name"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    
}