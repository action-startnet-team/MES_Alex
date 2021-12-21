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
    public class MET04_0300Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MET04_0300資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MET04_0300</returns>
        public MET04_0300 GetDTO(string pTkCode)
        {
            MET04_0300 datas = new MET04_0300();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MET04_0300";
            }
            else
            {
                sSql = "SELECT * FROM MET04_0300 where ureport_code=@ureport_code";
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
                        datas = new MET04_0300
                        {

                            ureport_code = comm.sGetString(reader["ureport_code"].ToString()),
                            ureport_date = comm.sGetString(reader["ureport_date"].ToString()),
                            mo_code = comm.sGetString(reader["mo_code"].ToString()),
                            pro_code = comm.sGetString(reader["pro_code"].ToString()),
                            pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString()),
                            pro_unit = comm.sGetString(reader["pro_unit"].ToString()),
                            lot_no = comm.sGetString(reader["lot_no"].ToString()),
                            ins_date = comm.sGetString(reader["ins_date"].ToString()),
                            ins_time = comm.sGetString(reader["ins_time"].ToString()),
                            usr_code = comm.sGetString(reader["usr_code"].ToString()),
                            sap_code = comm.sGetString(reader["sap_code"].ToString()),
                            sap_no = comm.sGetString(reader["sap_no"].ToString()),
                            sap_message = comm.sGetString(reader["sap_message"].ToString()),
                            is_del = comm.sGetString(reader["is_del"].ToString()),

                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MET04_0300資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MET04_0300</returns>
        public List<MET04_0300> Get_DataList(string pTkCode)
        {
            List<MET04_0300> list = new List<MET04_0300>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MET04_0300";
            }
            else
            {
                sSql = "SELECT * FROM MET04_0300 where ureport_date=@ureport_date AND is_del<>'Y' ORDER BY mo_code,pro_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@ureport_date", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MET04_0300 data = new MET04_0300();

                    data.ureport_code = comm.sGetString(reader["ureport_code"].ToString());
                    data.ureport_date = comm.sGetString(reader["ureport_date"].ToString());
                    data.mo_code = comm.sGetString(reader["mo_code"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString());
                    data.pro_unit = comm.sGetString(reader["pro_unit"].ToString());
                    data.lot_no = comm.sGetString(reader["lot_no"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                    data.sap_code = comm.sGetString(reader["sap_code"].ToString());
                    data.sap_no = comm.sGetString(reader["sap_no"].ToString());
                    data.sap_message = comm.sGetString(reader["sap_message"].ToString());
                    data.is_del = comm.sGetString(reader["is_del"].ToString());

                    data.can_delete = "Y";
                    data.can_update = "Y";
                    list.Add(data);
                }

            }
            return list;
        }

        public DataTable Get_MoList(string pTkCode)
        {
            string sSql = "SELECT DISTINCT mo_code,sap_code,sap_no FROM MET04_0300" +
                          " WHERE ureport_date=@ureport_date AND is_del<>'Y'";

            return comm.Get_DataTable(sSql, "ureport_date", pTkCode);
        }

        /// <summary>
        /// 取得使用者可以編輯的資料，結合商務邏輯權限
        /// </summary>
        /// <param name="pUsrCode"></param>
        /// <param name="pPrgCode"></param>
        /// <returns></returns>
        public List<MET04_0300> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_ureport_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MET04_0300> list = new List<MET04_0300>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = "SELECT * FROM MET04_0300";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MET04_0300 data = new MET04_0300();

                    data.ureport_code = comm.sGetString(reader["ureport_code"].ToString());
                    data.ureport_date = comm.sGetString(reader["ureport_date"].ToString());
                    data.mo_code = comm.sGetString(reader["mo_code"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString());
                    data.pro_unit = comm.sGetString(reader["pro_unit"].ToString());
                    data.lot_no = comm.sGetString(reader["lot_no"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                    data.sap_code = comm.sGetString(reader["sap_code"].ToString());
                    data.sap_no = comm.sGetString(reader["sap_no"].ToString());
                    data.sap_message = comm.sGetString(reader["sap_message"].ToString());
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
        public List<MET04_0300> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MET04_0300> list = new List<MET04_0300>();

            string sSql = " SELECT * FROM MET04_0300 ";

            // 取得資料
            list = comm.Get_ListByQuery<MET04_0300>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個MET04_0300的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MET04_0300">DTO</param>
        public void InsertData(MET04_0300 MET04_0300)
        {
            string sSql = "INSERT INTO " +
                          " MET04_0300 (  ureport_code,  ureport_date,  mo_code,  pro_code,  pro_qty,  pro_unit, " +
                          "               lot_no,  ins_date,  ins_time,  usr_code,  sap_code,  sap_no,  sap_message,  is_del ) " +

                          "     VALUES ( @ureport_code, @ureport_date, @mo_code, @pro_code, @pro_qty, @pro_unit, " +
                          "              @lot_no, @ins_date, @ins_time, @usr_code, @sap_code, @sap_no, @sap_message, @is_del )   ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MET04_0300);
            }
        }

        /// <summary>
        /// 傳入一個MET04_0300的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MET04_0300">DTO</param>
        public void UpdateData(MET04_0300 MET04_0300)
        {
            string sSql = " UPDATE MET04_0300                     " +
                          "    SET ureport_date =  @ureport_date, " +
                          "        mo_code      =  @mo_code,      " +
                          "        pro_code     =  @pro_code,     " +
                          "        pro_qty      =  @pro_qty,      " +
                          "        pro_unit     =  @pro_unit,     " +
                          "        lot_no       =  @lot_no,       " +
                          "        ins_date     =  @ins_date,     " +
                          "        ins_time     =  @ins_time,     " +
                          "        usr_code     =  @usr_code,     " +
                          "        sap_code     =  @sap_code,     " +
                          "        sap_no       =  @sap_no,       " +
                          "        sap_message  =  @sap_message,  " +
                          "        is_del       =  @is_del        " +
                          "  WHERE ureport_code =  @ureport_code  ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MET04_0300);
            }
        }

        public void UpdateStatus(string mo_code, string ureport_date)
        {
            string sSql = "UPDATE MET04_0300" +
                          "   SET is_del = 'P'" +
                          " WHERE mo_code =  @mo_code" +
                          "   AND ureport_date = @ureport_date" +
                          "   AND is_del<>'Y'";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { mo_code = mo_code, ureport_date = ureport_date });
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MET04_0300 WHERE ureport_code = @ureport_code";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { ureport_code = pTkCode });
            }
        }

    }
}