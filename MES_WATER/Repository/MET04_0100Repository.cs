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
    public class MET04_0100Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        //// <summary>
        //// 取得MET04_0100資料表內容
        //// </summary>
        //// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        //// < returns > DTO MET04_0100</returns>
        public MET04_0100 GetDTO(string pTkCode)
        {
            MET04_0100 datas = new MET04_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MET04_0100";
            }
            else
            {
                sSql = "SELECT * FROM MET04_0100 where ureport_code=@ureport_code";
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
                        datas = new MET04_0100
                        {
                            ureport_code = comm.sGetString(reader["ureport_code"].ToString()),
                            ureport_date = comm.sGetString(reader["ureport_date"].ToString()),
                            mo_code = comm.sGetString(reader["mo_code"].ToString()),
                            pro_code = comm.sGetString(reader["pro_code"].ToString()),
                            pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString()),
                            pro_unit = comm.sGetString(reader["pro_unit"].ToString()),
                            loc_code = comm.sGetString(reader["loc_code"].ToString()),
                            lot_no = comm.sGetString(reader["lot_no"].ToString()),
                            pro_date_s = comm.sGetString(reader["pro_date_s"].ToString()),
                            pro_time_s = comm.sGetString(reader["pro_time_s"].ToString()),
                            pro_date_e = comm.sGetString(reader["pro_date_e"].ToString()),
                            pro_time_e = comm.sGetString(reader["pro_time_e"].ToString()),
                            ins_date = comm.sGetString(reader["ins_date"].ToString()),
                            ins_time = comm.sGetString(reader["ins_time"].ToString()),
                            usr_code = comm.sGetString(reader["usr_code"].ToString()),
                            sap_code = comm.sGetString(reader["sap_code"].ToString()),
                            sap_no = comm.sGetString(reader["sap_no"].ToString()),
                            sap_message = comm.sGetString(reader["sap_message"].ToString()),
                            is_ok = comm.sGetString(reader["is_ok"].ToString()),
                            is_del = comm.sGetString(reader["is_del"].ToString()),

                        };
                    }
                }
            }
            return datas;
        }

        #region
        //// <summary>
        //// 取得MET04_0100資料表內容
        //// </summary>
        //// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        //// < returns > List MET04_0100</returns>
        public List<MET04_0100> Get_DataList(string pTkCode)
        {
            List<MET04_0100> list = new List<MET04_0100>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MET04_0100";
            }
            else
            {
                sSql = "SELECT * FROM MET04_0100 where ureport_code=@ureport_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@ureport_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MET04_0100 data = new MET04_0100();

                    data.ureport_code = comm.sGetString(reader["ureport_code"].ToString());
                    data.ureport_date = comm.sGetString(reader["ureport_date"].ToString());
                    data.mo_code = comm.sGetString(reader["mo_code"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString());
                    data.pro_unit = comm.sGetString(reader["pro_unit"].ToString());
                    data.loc_code = comm.sGetString(reader["loc_code"].ToString());
                    data.lot_no = comm.sGetString(reader["lot_no"].ToString());
                    data.pro_date_s = comm.sGetString(reader["pro_date_s"].ToString());
                    data.pro_time_s = comm.sGetString(reader["pro_time_s"].ToString());
                    data.pro_date_e = comm.sGetString(reader["pro_date_e"].ToString());
                    data.pro_time_e = comm.sGetString(reader["pro_time_e"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                    data.sap_code = comm.sGetString(reader["sap_code"].ToString());
                    data.sap_no = comm.sGetString(reader["sap_no"].ToString());
                    data.sap_message = comm.sGetString(reader["sap_message"].ToString());
                    data.is_ok = comm.sGetString(reader["is_ok"].ToString());
                    data.is_del = comm.sGetString(reader["is_del"].ToString());
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
        public List<MET04_0100> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_ureport_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MET04_0100> list = new List<MET04_0100>();
            string sSql = "";

            ////取得該使用者可以看的資料
            sSql = " SELECT * FROM MET04_0100 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MET04_0100 data = new MET04_0100();

                    data.ureport_code = comm.sGetString(reader["ureport_code"].ToString());
                    data.ureport_date = comm.sGetString(reader["ureport_date"].ToString());
                    data.mo_code = comm.sGetString(reader["mo_code"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString());
                    data.pro_unit = comm.sGetString(reader["pro_unit"].ToString());
                    data.loc_code = comm.sGetString(reader["loc_code"].ToString());
                    data.lot_no = comm.sGetString(reader["lot_no"].ToString());
                    data.pro_date_s = comm.sGetString(reader["pro_date_s"].ToString());
                    data.pro_time_s = comm.sGetString(reader["pro_time_s"].ToString());
                    data.pro_date_e = comm.sGetString(reader["pro_date_e"].ToString());
                    data.pro_time_e = comm.sGetString(reader["pro_time_e"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                    data.sap_code = comm.sGetString(reader["sap_code"].ToString());
                    data.sap_no = comm.sGetString(reader["sap_no"].ToString());
                    data.sap_message = comm.sGetString(reader["sap_message"].ToString());
                    data.is_ok = comm.sGetString(reader["is_ok"].ToString());
                    data.is_del = comm.sGetString(reader["is_del"].ToString());

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
        public List<MET04_0100> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MET04_0100> list = new List<MET04_0100>();

            string sSql = " SELECT MET04_0100.*, MEB20_0000.pro_name, BDP21_0100.field_name as is_ok_name " +
                                " FROM MET04_0100" +
                                " left join MEB20_0000 on MEB20_0000.pro_code = MET04_0100.pro_code " +
                                " left join BDP21_0100 on BDP21_0100.field_code = MET04_0100.is_ok and BDP21_0100.code_code = 'is_ok' ";

            // 取得資料
            list = comm.Get_ListByQuery<MET04_0100>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個MET04_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name = "MET04_0100" > DTO </ param >
        public void InsertData(MET04_0100 MET04_0100)
        {
            string sSql = "INSERT INTO " +
                          " MET04_0100 (     ureport_code,   ureport_date,    mo_code,    pro_code,    pro_qty,    pro_unit , " +
                          "                   loc_code,  lot_no,   pro_date_s,    pro_time_s,    pro_date_e,    pro_time_e,    ins_date,  " +
                          "                          ins_time,   usr_code,    sap_code,    sap_no,    sap_message,    is_ok,    is_del ) " +
                          "     VALUES     (  @ureport_code, @ureport_date, @mo_code, @pro_code, @pro_qty, @pro_unit, "+
                          "                 @loc_code,  @lot_no,     @pro_date_s,    @pro_time_s,    @pro_date_e,    @pro_time_e,    @ins_date ," +
                          "                        @ins_time,   @usr_code,    @sap_code,    @sap_no,    @sap_message,    @is_ok,   @is_del ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MET04_0100);
            }
        }

        /// <summary>
        /// 傳入一個MET04_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name = "MET04_0100" > DTO </ param >
        public void UpdateData(MET04_0100 MET04_0100)
        {

            string sSql = " UPDATE MET04_0100                     " +
                          "    SET ureport_date =  @ureport_date, " +
                          "        mo_code     =  @mo_code,     " +
                          "        pro_code      =  @pro_code,      " +
                          "        pro_qty     =  @pro_qty,     " +
                          "        pro_unit       =  @pro_unit ,       " +
                          "        loc_code       =  @loc_code ,       " +
                          "        lot_no       =  @lot_no ,       " +
                          "        pro_date_s       =  @pro_date_s ,       " +
                          "        pro_time_s       =  @pro_time_s ,       " +
                          "        pro_date_e       =  @pro_date_e ,       " +
                          "        pro_time_e       =  @pro_time_e ,       " +
                          "        ins_date       =  @ins_date ,       " +
                          "        ins_time       =  @ins_time ,       " +
                          "        usr_code       =  @usr_code ,       " +
                          "        sap_code       =  @sap_code ,       " +
                          "        sap_no       =  @sap_no ,       " +
                          "        sap_message       =  @sap_message ,       " +
                          "        is_ok       =  @is_ok ,     " +
                          "        is_del       =  @is_del       " +

                          "  WHERE ureport_code   =  @ureport_code    ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MET04_0100);

            }
        }

        //// <summary>
        //// 傳入一個鍵值，刪除、一次刪除一筆
        //// </summary>
        //// <param name = "pTkCode" > 資料鍵值 </ param >
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MET04_0100 WHERE ureport_code = @ureport_code;";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { ureport_code = pTkCode });
            }
        }
        ////暫存DataTable參考
        ////<summary>
        ////取得MET04_0100角色的DataTable
        ////</summary>
        ////<param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        ////<returns></returns>
        //public DataTable GetMET04_0100_dt(string pTkCode)
        //{
        //    DataTable dtTmp = new DataTable();
        //    DataTable dtDat = new DataTable();
        //    dtDat.Columns.Add("ureport_code", System.Type.GetType("System.String").ToString());
        //    dtDat.Columns.Add("ureport_code", System.Type.GetType("System.String").ToString());
        //    dtDat.Columns.Add("ureport_date", System.Type.GetType("System.String").ToString());
        //    string sSql = "";
        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM MET04_0100";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM MET04_0100 where ureport_code='" + pTkCode + "'";
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