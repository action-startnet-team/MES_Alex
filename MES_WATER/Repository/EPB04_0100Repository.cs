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
    public class EPB04_0100Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得EPB04_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO EPB04_0100</returns>
        public EPB04_0100 GetDTO(string pTkCode)
        {
            EPB04_0100 datas = new EPB04_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EPB04_0100";
            }
            else
            {
                sSql = "SELECT * FROM EPB04_0100 where epb04_0100=@epb04_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@epb04_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new EPB04_0100
                        {
                            epb04_0100 = comm.sGetInt32(reader["epb04_0100"].ToString()),
                            review_code = comm.sGetString(reader["review_code"].ToString()),
                            usr_code = comm.sGetString(reader["usr_code"].ToString()),
                            review_level = comm.sGetInt32(reader["review_level"].ToString()),
                            is_manager = comm.sGetString(reader["is_manager"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得EPB04_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List EPB04_0100</returns>
        public List<EPB04_0100> Get_DataList(string pTkCode)
        {
            List<EPB04_0100> list = new List<EPB04_0100>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EPB04_0100";
            }
            else
            {
                sSql = "SELECT * FROM EPB04_0100 where epb04_0100=@epb04_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@epb04_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EPB04_0100 data = new EPB04_0100();

                    data.epb04_0100 = comm.sGetInt32(reader["epb04_0100"].ToString());
                    data.review_code = comm.sGetString(reader["review_code"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                    data.review_level = comm.sGetInt32(reader["review_level"].ToString());
                    data.is_manager = comm.sGetString(reader["is_manager"].ToString());

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
        public List<EPB04_0100> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_epb04_0100", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<EPB04_0100> list = new List<EPB04_0100>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = " SELECT * FROM EPB04_0100 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@epb04_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EPB04_0100 data = new EPB04_0100();

                    data.epb04_0100 = comm.sGetInt32(reader["epb04_0100"].ToString());
                    data.review_code = comm.sGetString(reader["review_code"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                    data.review_level = comm.sGetInt32(reader["review_level"].ToString());
                    data.is_manager = comm.sGetString(reader["is_manager"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.epb04_0100)) {
                    //    data.can_delete = "N";
                    //    data.can_update = "N";
                    //}

                    list.Add(data);
                }
            }
            return list;
        }
        #endregion
        public List<EPB04_0100> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<EPB04_0100> list = new List<EPB04_0100>();
            string foreignKey = gmv.GetKey<EPB04_0000>(new EPB04_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT EPB04_0100.*, BDP08_0000.usr_name " +
                       " FROM EPB04_0100 " +
                       " left join BDP08_0000 on BDP08_0000.usr_code = EPB04_0100.usr_code" +
                       " where EPB04_0100. " + foreignKey + "=@" + foreignKey;
            }
            else
            {
                sSql = "SELECT * FROM EPB04_0100";
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

                    EPB04_0100 data = new EPB04_0100();

                    data.epb04_0100 = comm.sGetInt32(reader["epb04_0100"].ToString());
                    data.review_code = comm.sGetString(reader["review_code"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                    data.usr_name = comm.sGetString(reader["usr_name"].ToString());
                    data.review_level = comm.sGetInt32(reader["review_level"].ToString());
                    data.is_manager = comm.sGetString(reader["is_manager"].ToString());

                    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
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
        /// 傳入一個EPB04_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="EPB04_0100">DTO</param>
        public void InsertData(EPB04_0100 EPB04_0100)
        {
            string sSql = "INSERT INTO " +
                          " EPB04_0100 (  review_code,  usr_code,  review_level,  is_manager ) " +
                          "     VALUES ( @review_code, @usr_code, @review_level, @is_manager ) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EPB04_0100);
            }
        }

        /// <summary>
        /// 傳入一個EPB04_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="EPB04_0100">DTO</param>
        public void UpdateData(EPB04_0100 EPB04_0100)
        {
            //string pTkCode = EPB04_0100.epb04_0100.ToString();
            //Int32 iProQty = comm.sGetInt32(comm.Get_Data("EPB04_0100", pTkCode, "epb04_0100", "pro_qty"));
            //Int32 iSorSerial = comm.sGetInt32(comm.Get_Data("EPB04_0100", pTkCode, "epb04_0100", "sor_serial"));

            //ws.Cal_TraQty("DEL", "STT01_0100", "res_qty", iProQty, "where stt01_0100=" + iSorSerial);
            //ws.Cal_TraQty("ADD", "STT01_0100", "res_qty", comm.sGetInt32(EPB04_0100.pro_qty.ToString()), "where stt01_0100=" + comm.sGetString(EPB04_0100.sor_serial.ToString()));


            string sSql = " UPDATE EPB04_0100                     " +
                          "    SET review_code  =  @review_code,  " +
                          "        usr_code     =  @usr_code,     " +
                          "        is_manager   =  @is_manager    " +
                          "  WHERE epb04_0100   =  @epb04_0100    " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EPB04_0100);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@epb04_0100", EPB04_0100.epb04_0100));
                //sqlCommand.Parameters.Add(new SqlParameter("@epb04_0100", EPB04_0100.epb04_0100));
                //sqlCommand.Parameters.Add(new SqlParameter("@review_code", EPB04_0100.review_code));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM EPB04_0100 WHERE epb04_0100 = @epb04_0100;";
            //sSql += " Delete from BDP09_0100 where epb04_0100 = @epb04_0100; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { epb04_0100 = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@epb04_0100", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }
        ////暫存DataTable參考
        ////<summary>
        ////取得EPB04_0100角色的DataTable
        ////</summary>
        ////<param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        ////<returns></returns>
        //public DataTable GetEPB04_0100_dt(string pTkCode)
        //{
        //    DataTable dtTmp = new DataTable();
        //    DataTable dtDat = new DataTable();
        //    dtDat.Columns.Add("epb04_0100", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("epb04_0100", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("review_code", System.Type.GetType("System.String"].ToString());
        //    string sSql = "";
        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM EPB04_0100";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM EPB04_0100 where epb04_0100='" + pTkCode + "'";
        //    }
        //    dtTmp = comm.Get_DataTable(sSql);

        //    int i;
        //    for (i = 1; i < dtTmp.Rows.Count - 1; i++)
        //    {
        //        DataRow drow = dtDat.NewRow();
        //        drow["epb04_0100"] = dtTmp.Rows[i]["epb04_0100"];
        //        drow["epb04_0100"] = dtTmp.Rows[i]["epb04_0100"];
        //        drow["review_code"] = dtTmp.Rows[i]["review_code"];
        //        dtDat.Rows.Add(drow);
        //    }
        //    return dtDat;
        //}
    }
}