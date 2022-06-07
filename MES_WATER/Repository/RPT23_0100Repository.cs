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
    public class RPT23_0100Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得WMB01_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO WMB01_0000</returns>


        /// <summary>
        /// 傳入一個WMB01_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="RPT23_0100">DTO</param>
        public void InsertData(RPT23_0100 RPT23_0100)
        {
            string sSql = @" INSERT INTO 
                           MBA_E30 (  DOC_NO, SequenceNumber, buy_reply,  store_reply,  update_at, usr_code ) 
                               VALUES ( @DOC_NO, @SequenceNumber, @buy_reply, @store_reply,  @update_at, @usr_code  ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, RPT23_0100);
            }
        }

        /// <summary>
        /// 傳入一個WMB01_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="RPT23_0100">DTO</param>
        public void UpdateData(RPT23_0100 RPT23_0100)
        {
            string sSql = " UPDATE MBA_E30               " +
                          "    SET buy_reply =  @buy_reply,     " +
                          "        store_reply  =  @store_reply,      " +
                        "        update_at  =  @update_at,      " +
                        "        usr_code  =  @usr_code      " +
                          "  WHERE DOC_NO =  @DOC_NO      " +
                            "  and SequenceNumber =  @SequenceNumber      ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, RPT23_0100);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@sto_code", WMB01_0000.sto_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@sto_code", WMB01_0000.sto_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@sto_name", WMB01_0000.sto_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>


        ////暫存DataTable參考
        //// <summary>
        //// 取得WMB01_0000角色的DataTable
        //// </summary>
        //// <param name = "pTkCode" > 有傳鍵值取一筆，鍵值空白取全部</param>
        //// <returns></returns>
        //public DataTable GetWMB01_0000_dt(string pTkCode)
        //{
        //    DataTable dtTmp = new DataTable();
        //    DataTable dtDat = new DataTable();
        //    dtDat.Columns.Add("sto_code", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("sto_code", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("sto_name", System.Type.GetType("System.String"].ToString());

        //    string sSql = "";
        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM WMB01_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM WMB01_0000 where sto_code='" + pTkCode + "'";
        //    }
        //    dtTmp = comm.Get_DataTable(sSql);

        //    int i;
        //    for (i = 1; i < dtTmp.Rows.Count - 1; i++)
        //    {
        //        DataRow drow = dtDat.NewRow();
        //        drow["sto_code"] = dtTmp.Rows[i]["sto_code"];
        //        drow["sto_code"] = dtTmp.Rows[i]["sto_code"];
        //        drow["sto_name"] = dtTmp.Rows[i]["sto_name"];
        //        dtDat.Rows.Add(drow);
        //    }
        //    return dtDat;
        //}
    }
}