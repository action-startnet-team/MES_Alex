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
    public class MEP02_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MEP02_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEP02_0000</returns>
        public MEP02_0000 GetDTO(string pTkCode)
        {
            MEP02_0000 datas = new MEP02_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEP02_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEP02_0000 where mep02_0000=@mep02_0000";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@mep02_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEP02_0000
                        {
                            mep02_0000 = reader.GetInt32(reader.GetOrdinal("mep02_0000")),
                            mo_code = reader.GetString(reader.GetOrdinal("mo_code")),
                            wrk_code = reader.GetString(reader.GetOrdinal("wrk_code")),
                            work_code = reader.GetString(reader.GetOrdinal("work_code")),
                            station_code = reader.GetString(reader.GetOrdinal("station_code")),
                            mac_code = reader.GetString(reader.GetOrdinal("mac_code")),
                            usr_code = reader.GetString(reader.GetOrdinal("usr_code")),
                            pro_code = reader.GetString(reader.GetOrdinal("pro_code")),
                            pro_lot_no = reader.GetString(reader.GetOrdinal("pro_lot_no")),
                            iot_use_qty = reader.GetDecimal(reader.GetOrdinal("iot_use_qty")),
                            use_qty = reader.GetDecimal(reader.GetOrdinal("use_qty")),
                            use_unit = reader.GetString(reader.GetOrdinal("use_unit")),
                            iot_rtn_qty = reader.GetDecimal(reader.GetOrdinal("iot_rtn_qty")),
                            rtn_qty = reader.GetDecimal(reader.GetOrdinal("rtn_qty")),
                            rtn_unit = reader.GetString(reader.GetOrdinal("rtn_unit")),
                            iot_total_qty = reader.GetDecimal(reader.GetOrdinal("iot_total_qty")),
                            total_qty = reader.GetDecimal(reader.GetOrdinal("total_qty")),
                            total_unit = reader.GetString(reader.GetOrdinal("total_unit")),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MEP02_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MEP02_0000</returns>
        public List<MEP02_0000> Get_DataList(string pTkCode)
        {
            List<MEP02_0000> list = new List<MEP02_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEP02_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEP02_0000 where mep02_0000=@mep02_0000";
            }


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@mep02_0000", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEP02_0000 data = new MEP02_0000();

                    data.mep02_0000 = reader.GetInt32(reader.GetOrdinal("mep02_0000"));
                    data.mo_code = reader.GetString(reader.GetOrdinal("mo_code"));
                    data.wrk_code = reader.GetString(reader.GetOrdinal("wrk_code"));
                    data.work_code = reader.GetString(reader.GetOrdinal("work_code"));
                    data.station_code = reader.GetString(reader.GetOrdinal("station_code"));
                    data.mac_code = reader.GetString(reader.GetOrdinal("mac_code"));
                    data.usr_code = reader.GetString(reader.GetOrdinal("usr_code"));
                    data.pro_code = reader.GetString(reader.GetOrdinal("pro_code"));
                    data.pro_lot_no = reader.GetString(reader.GetOrdinal("pro_lot_no"));
                    data.iot_use_qty = reader.GetDecimal(reader.GetOrdinal("iot_use_qty"));
                    data.use_qty = reader.GetDecimal(reader.GetOrdinal("use_qty"));
                    data.use_unit = reader.GetString(reader.GetOrdinal("use_unit"));
                    data.iot_rtn_qty = reader.GetDecimal(reader.GetOrdinal("iot_rtn_qty"));
                    data.rtn_qty = reader.GetDecimal(reader.GetOrdinal("rtn_qty"));
                    data.rtn_unit = reader.GetString(reader.GetOrdinal("rtn_unit"));
                    data.iot_total_qty = reader.GetDecimal(reader.GetOrdinal("iot_total_qty"));
                    data.total_qty = reader.GetDecimal(reader.GetOrdinal("total_qty"));
                    data.total_unit = reader.GetString(reader.GetOrdinal("total_unit"));

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
        public List<MEP02_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_sup_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MEP02_0000> list = new List<MEP02_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = "SELECT * FROM MEP02_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEP02_0000 data = new MEP02_0000();

                    data.mep02_0000 = reader.GetInt32(reader.GetOrdinal("mep02_0000"));
                    data.mo_code = reader.GetString(reader.GetOrdinal("mo_code"));
                    data.wrk_code = reader.GetString(reader.GetOrdinal("wrk_code"));
                    data.work_code = reader.GetString(reader.GetOrdinal("work_code"));
                    data.station_code = reader.GetString(reader.GetOrdinal("station_code"));
                    data.mac_code = reader.GetString(reader.GetOrdinal("mac_code"));
                    data.usr_code = reader.GetString(reader.GetOrdinal("usr_code"));
                    data.pro_code = reader.GetString(reader.GetOrdinal("pro_code"));
                    data.pro_lot_no = reader.GetString(reader.GetOrdinal("pro_lot_no"));
                    data.iot_use_qty = reader.GetDecimal(reader.GetOrdinal("iot_use_qty"));
                    data.use_qty = reader.GetDecimal(reader.GetOrdinal("use_qty"));
                    data.use_unit = reader.GetString(reader.GetOrdinal("use_unit"));
                    data.iot_rtn_qty = reader.GetDecimal(reader.GetOrdinal("iot_rtn_qty"));
                    data.rtn_qty = reader.GetDecimal(reader.GetOrdinal("rtn_qty"));
                    data.rtn_unit = reader.GetString(reader.GetOrdinal("rtn_unit"));
                    data.iot_total_qty = reader.GetDecimal(reader.GetOrdinal("iot_total_qty"));
                    data.total_qty = reader.GetDecimal(reader.GetOrdinal("total_qty"));
                    data.total_unit = reader.GetString(reader.GetOrdinal("total_unit"));

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
        public List<MEP02_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MEP02_0000> list = new List<MEP02_0000>();

            string sSql = " SELECT MEP02_0000.*, MEB30_0000.work_name, MEB29_0000.station_name, MEB15_0000.mac_name, BDP08_0000.usr_name, MEB20_0000.pro_name " +
                          " FROM MEP02_0000 " +
                          " left join MEB30_0000 on MEB30_0000.work_code = MEP02_0000.work_code " +
                          " left join MEB29_0000 on MEB29_0000.station_code = MEP02_0000.station_code " +
                          " left join MEB15_0000 on MEB15_0000.mac_code = MEP02_0000.mac_code " +
                          " left join BDP08_0000 on BDP08_0000.usr_code = MEP02_0000.usr_code " +
                          " left join MEB20_0000 on MEB20_0000.pro_code = MEP02_0000.pro_code ";

            // 取得資料
            list = comm.Get_ListByQuery<MEP02_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個MEP02_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEP02_0000">DTO</param>
        public void InsertData(MEP02_0000 MEP02_0000)
        {
            string sSql = "INSERT INTO " +
                          " MEP02_0000 (  mo_code,  wrk_code,  work_code,  station_code,  mac_code,  usr_code,  pro_code, " +
                          "               pro_lot_no,  iot_use_qty,  use_qty,  use_unit,  iot_rtn_qty,  rtn_qty,  rtn_unit, " +
                          "               iot_total_qty,  total_qty,  total_unit ) " +
                          "     VALUES ( @mo_code, @wrk_code, @work_code, @station_code, @mac_code, @usr_code, @pro_code, " +
                          "              @pro_lot_no, @iot_use_qty, @use_qty, @use_unit, @iot_rtn_qty, @rtn_qty, @rtn_unit, " +
                          "              @iot_total_qty, @total_qty, @total_unit ) " ;

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEP02_0000);
            }
        }

        /// <summary>
        /// 傳入一個MEP02_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEP02_0000">DTO</param>
        public void UpdateData(MEP02_0000 MEP02_0000)
        {
            string sSql = " UPDATE MEP02_0000 " +
                          "    SET mo_code       =  @mo_code,      " +
                          "        wrk_code      =  @wrk_code,     " +
                          "        work_code     =  @work_code,    " +
                          "        station_code  =  @station_code, " +
                          "        mac_code      =  @mac_code,     " +
                          "        usr_code      =  @usr_code,     " +
                          "        pro_code      =  @pro_code,     " +
                          "        pro_lot_no    =  @pro_lot_no,   " +
                          "        iot_use_qty   =  @iot_use_qty,  " +
                          "        use_qty       =  @use_qty,      " +
                          "        use_unit      =  @use_unit,     " +
                          "        iot_rtn_qty   =  @iot_rtn_qty,  " +
                          "        rtn_qty       =  @rtn_qty,      " +
                          "        rtn_unit      =  @rtn_unit,     " +
                          "        iot_total_qty =  @iot_total_qty," +
                          "        total_qty     =  @total_qty,    " +
                          "        total_unit    =  @total_unit    " +
                          "  WHERE mep02_0000    =  @mep02_0000    ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEP02_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MEP02_0000 WHERE mep02_0000 = @mep02_0000;";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { mep02_0000 = pTkCode });
            }
        }

    }
}