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
    public class MEB29_0100Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得MEB29_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEB29_0100</returns>
        public MEB29_0100 GetDTO(string pTkCode)
        {
            MEB29_0100 datas = new MEB29_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB29_0100";
            }
            else
            {
                sSql = "SELECT * FROM MEB29_0100 where meb29_0100=@meb29_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@meb29_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEB29_0100
                        {
                            meb29_0100 = comm.sGetInt32(reader["meb29_0100"].ToString()),
                            station_code = comm.sGetString(reader["station_code"].ToString()),
                            usr_code = comm.sGetString(reader["usr_code"].ToString()),
                            control_type = comm.sGetString(reader["control_type"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MEB29_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MEB29_0100</returns>
        public List<MEB29_0100> Get_DataList(string pTkCode)
        {
            List<MEB29_0100> list = new List<MEB29_0100>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB29_0100";
            }
            else
            {
                sSql = "SELECT * FROM MEB29_0100 where meb29_0100=@meb29_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@meb29_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB29_0100 data = new MEB29_0100();

                    data.meb29_0100 = comm.sGetInt32(reader["meb29_0100"].ToString());
                    data.station_code = comm.sGetString(reader["station_code"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                    data.control_type = comm.sGetString(reader["control_type"].ToString());

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
        public List<MEB29_0100> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_meb29_0100", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MEB29_0100> list = new List<MEB29_0100>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = " SELECT * FROM MEB29_0100 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@meb29_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB29_0100 data = new MEB29_0100();

                    data.meb29_0100 = comm.sGetInt32(reader["meb29_0100"].ToString());
                    data.station_code = comm.sGetString(reader["station_code"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                    data.control_type = comm.sGetString(reader["control_type"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.meb29_0100)) {
                    //    data.can_delete = "N";
                    //    data.can_update = "N";
                    //}

                    list.Add(data);
                }
            }
            return list;
        }
        #endregion
        public List<MEB29_0100> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<MEB29_0100> list = new List<MEB29_0100>();
            string foreignKey = gmv.GetKey<MEB29_0000>(new MEB29_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT MEB29_0100.*, BDP21_0100.field_name as control_type_name, BDP08_0000.usr_name " +
                       " FROM MEB29_0100 " +
                       " left join BDP21_0100 on BDP21_0100.field_code = MEB29_0100.control_type and BDP21_0100.code_code = 'control_type' " +
                       " left join BDP08_0000 on BDP08_0000.usr_code = MEB29_0100.usr_code " +
                       " where MEB29_0100. " + foreignKey + "=@" + foreignKey;
            }
            else
            {
                sSql = "SELECT * FROM MEB29_0100";
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

                    MEB29_0100 data = new MEB29_0100();

                    data.meb29_0100 = comm.sGetInt32(reader["meb29_0100"].ToString());
                    data.station_code = comm.sGetString(reader["station_code"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                    data.usr_name = comm.sGetString(reader["usr_name"].ToString());
                    data.control_type = comm.sGetString(reader["control_type"].ToString());
                    data.control_type_name = comm.sGetString(reader["control_type_name"].ToString());

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
        /// 傳入一個MEB29_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEB29_0100">DTO</param>
        public void InsertData(MEB29_0100 MEB29_0100)
        {
            string sSql = "INSERT INTO " +
                          " MEB29_0100 (  station_code,  usr_code ) " +
                          "     VALUES ( @station_code, @usr_code ) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB29_0100);
            }
        }

        /// <summary>
        /// 傳入一個MEB29_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEB29_0100">DTO</param>
        public void UpdateData(MEB29_0100 MEB29_0100)
        {
            //string pTkCode = MEB29_0100.meb29_0100.ToString();
            //Int32 iProQty = comm.sGetInt32(comm.Get_Data("MEB29_0100", pTkCode, "meb29_0100", "pro_qty"));
            //Int32 iSorSerial = comm.sGetInt32(comm.Get_Data("MEB29_0100", pTkCode, "meb29_0100", "sor_serial"));

            //ws.Cal_TraQty("DEL", "STT01_0100", "res_qty", iProQty, "where stt01_0100=" + iSorSerial);
            //ws.Cal_TraQty("ADD", "STT01_0100", "res_qty", comm.sGetInt32(MEB29_0100.pro_qty.ToString()), "where stt01_0100=" + comm.sGetString(MEB29_0100.sor_serial.ToString()));


            string sSql = " UPDATE MEB29_0100                     " +
                          "    SET station_code =  @station_code, " +
                          "        usr_code     =  @usr_code      " +
                          "  WHERE meb29_0100   =  @meb29_0100    " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB29_0100);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@meb29_0100", MEB29_0100.meb29_0100));
                //sqlCommand.Parameters.Add(new SqlParameter("@meb29_0100", MEB29_0100.meb29_0100));
                //sqlCommand.Parameters.Add(new SqlParameter("@station_code", MEB29_0100.station_code));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MEB29_0100 WHERE meb29_0100 = @meb29_0100;";
            //sSql += " Delete from BDP09_0100 where meb29_0100 = @meb29_0100; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { meb29_0100 = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@meb29_0100", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }
        ////暫存DataTable參考
        ////<summary>
        ////取得MEB29_0100角色的DataTable
        ////</summary>
        ////<param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        ////<returns></returns>
        //public DataTable GetMEB29_0100_dt(string pTkCode)
        //{
        //    DataTable dtTmp = new DataTable();
        //    DataTable dtDat = new DataTable();
        //    dtDat.Columns.Add("meb29_0100", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("meb29_0100", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("station_code", System.Type.GetType("System.String"].ToString());
        //    string sSql = "";
        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM MEB29_0100";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM MEB29_0100 where meb29_0100='" + pTkCode + "'";
        //    }
        //    dtTmp = comm.Get_DataTable(sSql);

        //    int i;
        //    for (i = 1; i < dtTmp.Rows.Count - 1; i++)
        //    {
        //        DataRow drow = dtDat.NewRow();
        //        drow["meb29_0100"] = dtTmp.Rows[i]["meb29_0100"];
        //        drow["meb29_0100"] = dtTmp.Rows[i]["meb29_0100"];
        //        drow["station_code"] = dtTmp.Rows[i]["station_code"];
        //        dtDat.Rows.Add(drow);
        //    }
        //    return dtDat;
        //}
    }
}