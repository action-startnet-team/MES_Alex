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
    public class MEP01_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MEP01_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEP01_0000</returns>
        public MEP01_0000 GetDTO(string pTkCode)
        {
            MEP01_0000 datas = new MEP01_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEP01_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEP01_0000 where mep01_0000=@mep01_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@mep01_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEP01_0000
                        {
                            mep01_0000 = reader.GetInt32(reader.GetOrdinal("mep01_0000")),
                            mo_code = reader.GetString(reader.GetOrdinal("mo_code")),
                            wrk_code = reader.GetString(reader.GetOrdinal("wrk_code")),
                            work_code = reader.GetString(reader.GetOrdinal("work_code")),
                            station_code = reader.GetString(reader.GetOrdinal("station_code")),
                            mac_code = reader.GetString(reader.GetOrdinal("mac_code")),
                            usr_code = reader.GetString(reader.GetOrdinal("usr_code")),
                            pro_code = reader.GetString(reader.GetOrdinal("pro_code")),
                            pro_lot_no = reader.GetString(reader.GetOrdinal("pro_lot_no")),
                            iot_ok_qty = reader.GetDecimal(reader.GetOrdinal("iot_ok_qty")),
                            ok_qty = reader.GetDecimal(reader.GetOrdinal("ok_qty")),
                            ok_unit = reader.GetString(reader.GetOrdinal("ok_unit")),
                            iot_ng_qty = reader.GetDecimal(reader.GetOrdinal("iot_ng_qty")),
                            ng_qty = reader.GetDecimal(reader.GetOrdinal("ng_qty")),
                            ng_unit = reader.GetString(reader.GetOrdinal("ng_unit")),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MEP01_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MEP01_0000</returns>
        public List<MEP01_0000> Get_DataList(string pTkCode)
        {
            List<MEP01_0000> list = new List<MEP01_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEP01_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEP01_0000 where mep01_0000=@mep01_0000";
            }


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@mep01_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEP01_0000 data = new MEP01_0000();

                    data.mep01_0000 = reader.GetInt32(reader.GetOrdinal("mep01_0000"));
                    data.mo_code = reader.GetString(reader.GetOrdinal("mo_code"));
                    data.wrk_code = reader.GetString(reader.GetOrdinal("wrk_code"));
                    data.work_code = reader.GetString(reader.GetOrdinal("work_code"));
                    data.station_code = reader.GetString(reader.GetOrdinal("station_code"));
                    data.mac_code = reader.GetString(reader.GetOrdinal("mac_code"));
                    data.usr_code = reader.GetString(reader.GetOrdinal("usr_code"));
                    data.pro_code = reader.GetString(reader.GetOrdinal("pro_code"));
                    data.pro_lot_no = reader.GetString(reader.GetOrdinal("pro_lot_no"));
                    data.iot_ok_qty = reader.GetDecimal(reader.GetOrdinal("iot_ok_qty"));
                    data.ok_qty = reader.GetDecimal(reader.GetOrdinal("ok_qty"));
                    data.ok_unit = reader.GetString(reader.GetOrdinal("ok_unit"));
                    data.iot_ng_qty = reader.GetDecimal(reader.GetOrdinal("iot_ng_qty"));
                    data.ng_qty = reader.GetDecimal(reader.GetOrdinal("ng_qty"));
                    data.ng_unit = reader.GetString(reader.GetOrdinal("ng_unit"));

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
        public List<MEP01_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_sup_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MEP01_0000> list = new List<MEP01_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = "SELECT * FROM MEP01_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEP01_0000 data = new MEP01_0000();

                    data.mep01_0000 = reader.GetInt32(reader.GetOrdinal("mep01_0000"));
                    data.mo_code = reader.GetString(reader.GetOrdinal("mo_code"));
                    data.wrk_code = reader.GetString(reader.GetOrdinal("wrk_code"));
                    data.work_code = reader.GetString(reader.GetOrdinal("work_code"));
                    data.station_code = reader.GetString(reader.GetOrdinal("station_code"));
                    data.mac_code = reader.GetString(reader.GetOrdinal("mac_code"));
                    data.usr_code = reader.GetString(reader.GetOrdinal("usr_code"));
                    data.pro_code = reader.GetString(reader.GetOrdinal("pro_code"));
                    data.pro_lot_no = reader.GetString(reader.GetOrdinal("pro_lot_no"));
                    data.iot_ok_qty = reader.GetDecimal(reader.GetOrdinal("iot_ok_qty"));
                    data.ok_qty = reader.GetDecimal(reader.GetOrdinal("ok_qty"));
                    data.ok_unit = reader.GetString(reader.GetOrdinal("ok_unit"));
                    data.iot_ng_qty = reader.GetDecimal(reader.GetOrdinal("iot_ng_qty"));
                    data.ng_qty = reader.GetDecimal(reader.GetOrdinal("ng_qty"));
                    data.ng_unit = reader.GetString(reader.GetOrdinal("ng_unit"));

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
        public List<MEP01_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MEP01_0000> list = new List<MEP01_0000>();

            string sSql = " SELECT MEP01_0000.*, MEB30_0000.work_name, MEB29_0000.station_name, MEB15_0000.mac_name, BDP08_0000.usr_name, MEB20_0000.pro_name " +
                          " FROM MEP01_0000 " +
                          " left join MEB30_0000 on MEB30_0000.work_code = MEP01_0000.work_code " +
                          " left join MEB29_0000 on MEB29_0000.station_code = MEP01_0000.station_code " +
                          " left join MEB15_0000 on MEB15_0000.mac_code = MEP01_0000.mac_code " +
                          " left join BDP08_0000 on BDP08_0000.usr_code = MEP01_0000.usr_code " +
                          " left join MEB20_0000 on MEB20_0000.pro_code = MEP01_0000.pro_code " ;

            // 取得資料
            list = comm.Get_ListByQuery<MEP01_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個MEP01_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEP01_0000">DTO</param>
        public void InsertData(MEP01_0000 MEP01_0000)
        {
            string sSql = "INSERT INTO " +
                          " MEP01_0000 (  mo_code,  wrk_code,  work_code,  station_code,  mac_code,  usr_code,  pro_code, " +
                          "               pro_lot_no,  iot_ok_qty,  ok_qty,  ok_unit,  iot_ng_qty,  ng_qty,  ng_unit ) " +
                          "     VALUES ( @mo_code, @wrk_code, @work_code, @station_code, @mac_code, @usr_code, @pro_code, " +
                          "              @pro_lot_no, @iot_ok_qty, @ok_qty, @ok_unit, @iot_ng_qty, @ng_qty, @ng_unit )";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEP01_0000);
            }
        }

        /// <summary>
        /// 傳入一個MEP01_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEP01_0000">DTO</param>
        public void UpdateData(MEP01_0000 MEP01_0000)
        {
            string sSql = " UPDATE MEP01_0000 " +
                          "    SET mo_code       =  @mo_code,      " +
                          "        wrk_code      =  @wrk_code,     " +
                          "        work_code     =  @work_code,    " +
                          "        station_code  =  @station_code, " +
                          "        mac_code      =  @mac_code,     " +
                          "        usr_code      =  @usr_code,     " +
                          "        pro_code      =  @pro_code,     " +
                          "        pro_lot_no    =  @pro_lot_no,   " +
                          "        iot_ok_qty    =  @iot_ok_qty,   " +
                          "        ok_qty        =  @ok_qty,       " +
                          "        ok_unit       =  @ok_unit,      " +
                          "        iot_ng_qty    =  @iot_ng_qty,   " +
                          "        ng_qty        =  @ng_qty,       " +
                          "        ng_unit       =  @ng_unit       " +
                          "  WHERE mep01_0000    =  @mep01_0000    ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEP01_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MEP01_0000 WHERE mep01_0000 = @mep01_0000;";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { mep01_0000 = pTkCode });
            }
        }

    }
}