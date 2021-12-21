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
    public class WMT06_0100Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得WMT06_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO WMT06_0100</returns>
        public WMT06_0100 GetDTO(string pTkCode)
        {
            WMT06_0100 datas = new WMT06_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM WMT06_0100";
            }
            else
            {
                sSql = "SELECT * FROM WMT06_0100 where wmt06_0100=@wmt06_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@wmt06_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new WMT06_0100
                        {
                            wmt06_0100 = comm.sGetInt32(reader["wmt06_0100"].ToString()),
                            prepare_code = comm.sGetString(reader["prepare_code"].ToString()),
                            pro_code = comm.sGetString(reader["pro_code"].ToString()),
                            pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString()),
                            pro_unit = comm.sGetString(reader["pro_unit"].ToString()),
                            is_share = comm.sGetString(reader["is_share"].ToString()),
                            lot_no = comm.sGetString(reader["is_share"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得WMT06_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List WMT06_0100</returns>
        public List<WMT06_0100> Get_DataList(string pTkCode)
        {
            List<WMT06_0100> list = new List<WMT06_0100>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM WMT06_0100";
            }
            else
            {
                sSql = "SELECT * FROM WMT06_0100 where wmt06_0100=@wmt06_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@wmt06_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    WMT06_0100 data = new WMT06_0100();

                    data.wmt06_0100 = comm.sGetInt32(reader["wmt06_0100"].ToString());
                    data.prepare_code = comm.sGetString(reader["prepare_code"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString());
                    data.pro_unit = comm.sGetString(reader["pro_unit"].ToString());
                    data.is_share = comm.sGetString(reader["is_share"].ToString());

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
        public List<WMT06_0100> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_wmt06_0100", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<WMT06_0100> list = new List<WMT06_0100>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = " SELECT * FROM WMT06_0100 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@wmt06_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    WMT06_0100 data = new WMT06_0100();

                    data.wmt06_0100 = comm.sGetInt32(reader["wmt06_0100"].ToString());
                    data.prepare_code = comm.sGetString(reader["prepare_code"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString());
                    data.pro_unit = comm.sGetString(reader["pro_unit"].ToString());
                    data.is_share = comm.sGetString(reader["is_share"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.wmt06_0100)) {
                    //    data.can_delete = "N";
                    //    data.can_update = "N";
                    //}

                    list.Add(data);
                }
            }
            return list;
        }
        #endregion
        public List<WMT06_0100> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<WMT06_0100> list = new List<WMT06_0100>();
            string foreignKey = gmv.GetKey<WMT06_0000>(new WMT06_0000());
            string sSql = "";
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT WMT06_0100.*, MEB20_0000.pro_name " +
                       " FROM WMT06_0100 " +
                       " left join MEB20_0000 on MEB20_0000.pro_code = WMT06_0100.pro_code " +
                       " where WMT06_0100. " + foreignKey + "=@" + foreignKey;
            }
            else
            {
                sSql = "SELECT * FROM WMT06_0100";
            }
            //取得該使用者可以看的資料
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter(foreignKey, pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {

                    WMT06_0100 data = new WMT06_0100();

                    data.wmt06_0100 = comm.sGetInt32(reader["wmt06_0100"].ToString());
                    data.prepare_code = comm.sGetString(reader["prepare_code"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.pro_name = comm.sGetString(reader["pro_name"].ToString());
                    data.pro_qty = comm.sGetDecimal(reader["pro_qty"].ToString());
                    data.pro_unit = comm.sGetString(reader["pro_unit"].ToString());
                    data.is_share = comm.sGetString(reader["is_share"].ToString());
                    data.lot_no = comm.sGetString(reader["is_share"].ToString());
                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改

                    list.Add(data);
                }

            }
            return list;
        }

        /// <summary>
        /// 傳入一個WMT06_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="WMT06_0100">DTO</param>
        public void InsertData(WMT06_0100 WMT06_0100)
        {
            string sSql = "INSERT INTO " +
                          " WMT06_0100 (  prepare_code,  pro_code,  pro_qty,  pro_unit,  is_share, lot_no ) " +
                          "     VALUES ( @prepare_code, @pro_code, @pro_qty, @pro_unit, @is_share, @lot_no ) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMT06_0100);
            }
        }

        /// <summary>
        /// 傳入一個WMT06_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="WMT06_0100">DTO</param>
        public void UpdateData(WMT06_0100 WMT06_0100)
        {
            //string pTkCode = WMT06_0100.wmt06_0100.ToString();
            //Int32 iProQty = comm.sGetInt32(comm.Get_Data("WMT06_0100", pTkCode, "wmt06_0100", "pro_qty"));
            //Int32 iSorSerial = comm.sGetInt32(comm.Get_Data("WMT06_0100", pTkCode, "wmt06_0100", "sor_serial"));

            //ws.Cal_TraQty("DEL", "STT01_0100", "res_qty", iProQty, "where stt01_0100=" + iSorSerial);
            //ws.Cal_TraQty("ADD", "STT01_0100", "res_qty", comm.sGetInt32(WMT06_0100.pro_qty.ToString()), "where stt01_0100=" + comm.sGetString(WMT06_0100.sor_serial.ToString()));


            string sSql = " UPDATE WMT06_0100                     " +
                          "    SET prepare_code =  @prepare_code, " +
                          "        pro_code     =  @pro_code,     " +
                          "        pro_qty      =  @pro_qty,      " +
                          "        pro_unit     =  @pro_unit,     " +
                          "        is_share     =  @is_share      " +
                          "        lot_no       =  @lot_no        " +
                          "  WHERE wmt06_0100   =  @wmt06_0100    " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMT06_0100);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@wmt06_0100", WMT06_0100.wmt06_0100));
                //sqlCommand.Parameters.Add(new SqlParameter("@wmt06_0100", WMT06_0100.wmt06_0100));
                //sqlCommand.Parameters.Add(new SqlParameter("@prepare_code", WMT06_0100.prepare_code));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM WMT06_0100 WHERE wmt06_0100 = @wmt06_0100;";
            //sSql += " Delete from BDP09_0100 where wmt06_0100 = @wmt06_0100; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { wmt06_0100 = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@wmt06_0100", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }
        ////暫存DataTable參考
        ////<summary>
        ////取得WMT06_0100角色的DataTable
        ////</summary>
        ////<param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        ////<returns></returns>
        //public DataTable GetWMT06_0100_dt(string pTkCode)
        //{
        //    DataTable dtTmp = new DataTable();
        //    DataTable dtDat = new DataTable();
        //    dtDat.Columns.Add("wmt06_0100", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("wmt06_0100", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("prepare_code", System.Type.GetType("System.String"].ToString());
        //    string sSql = "";
        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM WMT06_0100";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM WMT06_0100 where wmt06_0100='" + pTkCode + "'";
        //    }
        //    dtTmp = comm.Get_DataTable(sSql);

        //    int i;
        //    for (i = 1; i < dtTmp.Rows.Count - 1; i++)
        //    {
        //        DataRow drow = dtDat.NewRow();
        //        drow["wmt06_0100"] = dtTmp.Rows[i]["wmt06_0100"];
        //        drow["wmt06_0100"] = dtTmp.Rows[i]["wmt06_0100"];
        //        drow["prepare_code"] = dtTmp.Rows[i]["prepare_code"];
        //        dtDat.Rows.Add(drow);
        //    }
        //    return dtDat;
        //}
    }
}