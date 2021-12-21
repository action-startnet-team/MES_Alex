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
    public class EPB04_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得EPB04_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO EPB04_0000</returns>
        public EPB04_0000 GetDTO(string pTkCode)
        {
            EPB04_0000 datas = new EPB04_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EPB04_0000";
            }
            else
            {
                sSql = "SELECT * FROM EPB04_0000 where review_code=@review_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@review_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new EPB04_0000
                        {
                            review_code = comm.sGetString(reader["review_code"].ToString()),
                            epb_code = comm.sGetString(reader["epb_code"].ToString()),
                            report_code = comm.sGetString(reader["report_code"].ToString()),
                            review_memo = comm.sGetString(reader["review_memo"].ToString()),
                            is_use = comm.sGetString(reader["is_use"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        /// <summary>
        /// 取得EPB04_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List EPB04_0000</returns>
        public List<EPB04_0000> Get_DataList(string pTkCode)
        {
            List<EPB04_0000> list = new List<EPB04_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EPB04_0000";
            }
            else
            {
                sSql = "SELECT * FROM EPB04_0000 where review_code=@review_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@review_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EPB04_0000 data = new EPB04_0000();

                    data.review_code = comm.sGetString(reader["review_code"].ToString());
                    data.epb_code = comm.sGetString(reader["epb_code"].ToString());
                    data.review_memo = comm.sGetString(reader["review_memo"].ToString());
                    data.is_use = comm.sGetString(reader["is_use"].ToString());

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
        public List<EPB04_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_review_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<EPB04_0000> list = new List<EPB04_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = " SELECT * FROM EPB04_0000 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@review_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EPB04_0000 data = new EPB04_0000();

                    data.review_code = comm.sGetString(reader["review_code"].ToString());
                    data.epb_code = comm.sGetString(reader["epb_code"].ToString());
                    data.review_memo = comm.sGetString(reader["review_memo"].ToString());
                    data.is_use = comm.sGetString(reader["is_use"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.review_code)) {
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
        public List<EPB04_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<EPB04_0000> list = new List<EPB04_0000>();

            string sSql = " SELECT distinct EPB04_0000.review_code, EPB04_0000.*, EPB02_0000.epb_name,report_name " +
                          " FROM EPB04_0000 " +
                          " left join EPB04_0100 on EPB04_0100.review_code = EPB04_0000.review_code " +
                          " left join EPB02_0000 on EPB02_0000.epb_code = EPB04_0000.epb_code " +
                          " left join BDP08_0000 on BDP08_0000.usr_code = EPB04_0100.usr_code" +
                          " left join RSS02_0000 on EPB04_0000.report_code = RSS02_0000.report_code";

            // 取得資料
            list = comm.Get_ListByQuery<EPB04_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
                //        data.sup_name = data.cmemo + " - " + comm.sGetString(reader["sup_name"].ToString());
                //        data.epb_code = comm.sGetString(reader["review_code"].ToString()) + " - " + comm.sGetString(reader["epb_code"].ToString());

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
        /// 傳入一個EPB04_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="EPB04_0000">DTO</param>
        public void InsertData(EPB04_0000 EPB04_0000)
        {
            string sSql = " INSERT INTO " +
                          " EPB04_0000 (  review_code,  epb_code,  review_memo,  is_use ) " +
                          "     VALUES ( @review_code, @epb_code, @review_memo, @is_use ) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EPB04_0000);
            }
        }

        /// <summary>
        /// 傳入一個EPB04_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="EPB04_0000">DTO</param>
        public void UpdateData(EPB04_0000 EPB04_0000)
        {
            string sSql = " UPDATE EPB04_0000                   " +
                          "    SET epb_code    =  @epb_code,    " +
                          "        review_memo =  @review_memo, " +
                          "        is_use      =  @is_use       " +
                          "  WHERE review_code =  @review_code  " ;

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EPB04_0000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@review_code", EPB04_0000.review_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@review_code", EPB04_0000.review_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@epb_code", EPB04_0000.epb_code));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM EPB04_0000 WHERE review_code = @review_code; " +
                          " DELETE FROM EPB04_0100 WHERE review_code = @review_code; " ;
            //sSql += " Delete from BDP09_0100 where review_code = @review_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { review_code = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@review_code", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        ////暫存DataTable參考
        //// <summary>
        //// 取得EPB04_0000角色的DataTable
        //// </summary>
        //// <param name = "pTkCode" > 有傳鍵值取一筆，鍵值空白取全部</param>
        //// <returns></returns>
        //public DataTable GetEPB04_0000_dt(string pTkCode)
        //{
        //    DataTable dtTmp = new DataTable();
        //    DataTable dtDat = new DataTable();
        //    dtDat.Columns.Add("review_code", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("review_code", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("epb_code", System.Type.GetType("System.String"].ToString());

        //    string sSql = "";
        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM EPB04_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM EPB04_0000 where review_code='" + pTkCode + "'";
        //    }
        //    dtTmp = comm.Get_DataTable(sSql);

        //    int i;
        //    for (i = 1; i < dtTmp.Rows.Count - 1; i++)
        //    {
        //        DataRow drow = dtDat.NewRow();
        //        drow["review_code"] = dtTmp.Rows[i]["review_code"];
        //        drow["review_code"] = dtTmp.Rows[i]["review_code"];
        //        drow["epb_code"] = dtTmp.Rows[i]["epb_code"];
        //        dtDat.Rows.Add(drow);
        //    }
        //    return dtDat;
        //}
    }
}