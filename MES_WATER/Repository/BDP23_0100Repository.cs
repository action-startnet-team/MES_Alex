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
    public class BDP23_0100Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得BDP23_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO BDP23_0100</returns>
        public BDP23_0100 GetDTO(string pTkCode)
        {
            BDP23_0100 datas = new BDP23_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP23_0100";
            }
            else
            {
                sSql = "SELECT * FROM BDP23_0100 where bdp23_0100=@bdp23_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@bdp23_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new BDP23_0100
                        {
                            bdp23_0100 = comm.sGetInt32(reader["bdp23_0100"].ToString()),
                            bdp23_0000 = comm.sGetInt32(reader["bdp23_0000"].ToString()),
                            usr_code = comm.sGetString(reader["usr_code"].ToString()),
                            is_ok = comm.sGetString(reader["is_ok"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得BDP23_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List BDP23_0100</returns>
        public List<BDP23_0100> Get_DataList(string pTkCode)
        {
            List<BDP23_0100> list = new List<BDP23_0100>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM BDP23_0100";
            }
            else
            {
                sSql = "SELECT * FROM BDP23_0100 where bdp23_0100=@bdp23_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@bdp23_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP23_0100 data = new BDP23_0100();

                    data.bdp23_0100 = comm.sGetInt32(reader["bdp23_0100"].ToString());
                    data.bdp23_0000 = comm.sGetInt32(reader["bdp23_0000"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                    data.is_ok = comm.sGetString(reader["is_ok"].ToString());

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
        public List<BDP23_0100> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_bdp23_0100", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<BDP23_0100> list = new List<BDP23_0100>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = " SELECT * FROM BDP23_0100 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@bdp23_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    BDP23_0100 data = new BDP23_0100();

                    data.bdp23_0100 = comm.sGetInt32(reader["bdp23_0100"].ToString());
                    data.bdp23_0000 = comm.sGetInt32(reader["bdp23_0000"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                    data.is_ok = comm.sGetString(reader["is_ok"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.bdp23_0100)) {
                    //    data.can_delete = "N";
                    //    data.can_update = "N";
                    //}

                    list.Add(data);
                }
            }
            return list;
        }
        #endregion
        public List<BDP23_0100> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<BDP23_0100> list = new List<BDP23_0100>();
            string foreignKey = gmv.GetKey<BDP23_0000>(new BDP23_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT BDP23_0100.* " +
                       " FROM BDP23_0100 " +
                       " where BDP23_0100. " + foreignKey + "=@" + foreignKey;
            }
            else
            {
                sSql = "SELECT * FROM BDP23_0100";
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

                    BDP23_0100 data = new BDP23_0100();

                    data.bdp23_0100 = comm.sGetInt32(reader["bdp23_0100"].ToString());
                    data.bdp23_0000 = comm.sGetInt32(reader["bdp23_0000"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                    data.is_ok = comm.sGetString(reader["is_ok"].ToString());

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
        /// 傳入一個BDP23_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="BDP23_0100">DTO</param>
        public void InsertData(BDP23_0100 BDP23_0100)
        {
            string sSql = "INSERT INTO " +
                          " BDP23_0100 (  bdp23_0000,  usr_code,  is_ok ) " +
                          "     VALUES ( @bdp23_0000, @usr_code, @is_ok ) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, BDP23_0100);
            }
        }

        /// <summary>
        /// 傳入一個BDP23_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="BDP23_0100">DTO</param>
        public void UpdateData(BDP23_0100 BDP23_0100)
        {
            //string pTkCode = BDP23_0100.bdp23_0100.ToString();
            //Int32 iProQty = comm.sGetInt32(comm.Get_Data("BDP23_0100", pTkCode, "bdp23_0100", "pro_qty"));
            //Int32 iSorSerial = comm.sGetInt32(comm.Get_Data("BDP23_0100", pTkCode, "bdp23_0100", "sor_serial"));

            //ws.Cal_TraQty("DEL", "STT01_0100", "res_qty", iProQty, "where stt01_0100=" + iSorSerial);
            //ws.Cal_TraQty("ADD", "STT01_0100", "res_qty", comm.sGetInt32(BDP23_0100.pro_qty.ToString()), "where stt01_0100=" + comm.sGetString(BDP23_0100.sor_serial.ToString()));


            string sSql = " UPDATE BDP23_0100                  " +
                          "    SET bdp23_0000  =  @bdp23_0000, " +
                          "        usr_code    =  @usr_code,   " +
                          "        is_ok       =  @is_ok       " +
                          "  WHERE bdp23_0100  =  @bdp23_0100  " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, BDP23_0100);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@bdp23_0100", BDP23_0100.bdp23_0100));
                //sqlCommand.Parameters.Add(new SqlParameter("@bdp23_0100", BDP23_0100.bdp23_0100));
                //sqlCommand.Parameters.Add(new SqlParameter("@bdp23_0000", BDP23_0100.bdp23_0000));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM BDP23_0100 WHERE bdp23_0100 = @bdp23_0100;";
            //sSql += " Delete from BDP09_0100 where bdp23_0100 = @bdp23_0100; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { bdp23_0100 = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@bdp23_0100", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }
        ////暫存DataTable參考
        ////<summary>
        ////取得BDP23_0100角色的DataTable
        ////</summary>
        ////<param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        ////<returns></returns>
        //public DataTable GetBDP23_0100_dt(string pTkCode)
        //{
        //    DataTable dtTmp = new DataTable();
        //    DataTable dtDat = new DataTable();
        //    dtDat.Columns.Add("bdp23_0100", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("bdp23_0100", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("bdp23_0000", System.Type.GetType("System.String"].ToString());
        //    string sSql = "";
        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM BDP23_0100";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM BDP23_0100 where bdp23_0100='" + pTkCode + "'";
        //    }
        //    dtTmp = comm.Get_DataTable(sSql);

        //    int i;
        //    for (i = 1; i < dtTmp.Rows.Count - 1; i++)
        //    {
        //        DataRow drow = dtDat.NewRow();
        //        drow["bdp23_0100"] = dtTmp.Rows[i]["bdp23_0100"];
        //        drow["bdp23_0100"] = dtTmp.Rows[i]["bdp23_0100"];
        //        drow["bdp23_0000"] = dtTmp.Rows[i]["bdp23_0000"];
        //        dtDat.Rows.Add(drow);
        //    }
        //    return dtDat;
        //}
    }
}