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
    public class WMT06_0200Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得WMT06_0200資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO WMT06_0200</returns>
        public WMT06_0200 GetDTO(string pTkCode)
        {
            WMT06_0200 data = new WMT06_0200();

            if (string.IsNullOrEmpty(pTkCode))
            {
                return data;
            }

            string sSql = "SELECT * FROM WMT06_0200 where wmt06_0200=@wmt06_0200";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                data = con_db.QueryFirstOrDefault<WMT06_0200>(sSql, new { wmt06_0200 = pTkCode });
            }

            return data;
        }

        public List<WMT06_0200> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<WMT06_0200> list = new List<WMT06_0200>();

            string sSql = "";
            string foreignKey = gmv.GetKey<WMT06_0000>();

            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT WMT06_0200.* " +
                       " FROM WMT06_0200 " +
                       " where WMT06_0200. " + foreignKey + "=@" + foreignKey;
            }
            else
            {
                sSql = "SELECT * FROM WMT06_0200";
            }

            DynamicParameters sqlParams = new DynamicParameters();
            sqlParams.Add("@" + foreignKey, pTkCode);

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                list = con_db.Query<WMT06_0200>(sSql, sqlParams).ToList();
                //data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                //data.can_update = sLimitStr.Contains("M") ? "Y" : "N";
            }
            
            //取得該使用者可以看的資料
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            list.ForEach(x => {
                x.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                x.can_update = sLimitStr.Contains("M") ? "Y" : "N";
            });

            return list;
        }

        /// <summary>
        /// 傳入一個WMT06_0200的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="WMT06_0200">DTO</param>
        public void InsertData(WMT06_0200 WMT06_0200)
        {
            string sSql = "INSERT INTO " +
                          " WMT06_0200 ( prepare_code, wrk_code ) " +
                          "     VALUES ( @prepare_code, @wrk_code ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMT06_0200);
            }
        }

        /// <summary>
        /// 傳入一個WMT06_0200的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="WMT06_0200">DTO</param>
        public void UpdateData(WMT06_0200 WMT06_0200)
        {
            //string pTkCode = WMT06_0200.wmt06_0200.ToString();
            //Int32 iProQty = comm.sGetInt32(comm.Get_Data("WMT06_0200", pTkCode, "wmt06_0200", "pro_qty"));
            //Int32 iSorSerial = comm.sGetInt32(comm.Get_Data("WMT06_0200", pTkCode, "wmt06_0200", "sor_serial"));

            //ws.Cal_TraQty("DEL", "STT01_0100", "res_qty", iProQty, "where stt01_0100=" + iSorSerial);
            //ws.Cal_TraQty("ADD", "STT01_0100", "res_qty", comm.sGetInt32(WMT06_0200.pro_qty.ToString()), "where stt01_0100=" + comm.sGetString(WMT06_0200.sor_serial.ToString()));


            string sSql = " UPDATE WMT06_0200                     " +
                          "    SET prepare_code =  @prepare_code " +
                          "       ,wrk_code     =  @wrk_code     " +
                          "  WHERE wmt06_0200   =  @wmt06_0200    ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, WMT06_0200);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@wmt06_0200", WMT06_0200.wmt06_0200));
                //sqlCommand.Parameters.Add(new SqlParameter("@wmt06_0200", WMT06_0200.wmt06_0200));
                //sqlCommand.Parameters.Add(new SqlParameter("@prepare_code", WMT06_0200.prepare_code));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM WMT06_0200 WHERE wmt06_0200 = @wmt06_0200;";
            //sSql += " Delete from BDP09_0100 where wmt06_0200 = @wmt06_0200; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { wmt06_0200 = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@wmt06_0200", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }
        ////暫存DataTable參考
        ////<summary>
        ////取得WMT06_0200角色的DataTable
        ////</summary>
        ////<param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        ////<returns></returns>
        //public DataTable GetWMT06_0200_dt(string pTkCode)
        //{
        //    DataTable dtTmp = new DataTable();
        //    DataTable dtDat = new DataTable();
        //    dtDat.Columns.Add("wmt06_0200", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("wmt06_0200", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("prepare_code", System.Type.GetType("System.String"].ToString());
        //    string sSql = "";
        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM WMT06_0200";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM WMT06_0200 where wmt06_0200='" + pTkCode + "'";
        //    }
        //    dtTmp = comm.Get_DataTable(sSql);

        //    int i;
        //    for (i = 1; i < dtTmp.Rows.Count - 1; i++)
        //    {
        //        DataRow drow = dtDat.NewRow();
        //        drow["wmt06_0200"] = dtTmp.Rows[i]["wmt06_0200"];
        //        drow["wmt06_0200"] = dtTmp.Rows[i]["wmt06_0200"];
        //        drow["prepare_code"] = dtTmp.Rows[i]["prepare_code"];
        //        dtDat.Rows.Add(drow);
        //    }
        //    return dtDat;
        //}
    }
}