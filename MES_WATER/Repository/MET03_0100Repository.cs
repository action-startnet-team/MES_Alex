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
    public class MET03_0100Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得MET03_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MET03_0100</returns>
        public MET03_0100 GetDTO(string pTkCode)
        {
            MET03_0100 datas = new MET03_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MET03_0100";
            }
            else
            {
                sSql = "SELECT * FROM MET03_0100 where met03_0100=@met03_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@met03_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MET03_0100
                        {
                            //met03_0100 = comm.sGetInt32(reader["met03_0100"].ToString()),
                            wrk_code = comm.sGetString(reader["wrk_code"].ToString()),
                            //pro_code = comm.sGetString(reader["pro_code"].ToString()),
                            //lot_no = comm.sGetString(reader["lot_no"].ToString()),
                            //wmt06_0110 = comm.sGetInt32(reader["wmt06_0110"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MET03_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MET03_0100</returns>
        public List<MET03_0100> Get_DataList(string pTkCode)
        {
            List<MET03_0100> list = new List<MET03_0100>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MET03_0100";
            }
            else
            {
                sSql = "SELECT * FROM MET03_0100 where met03_0100=@met03_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@met03_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MET03_0100 data = new MET03_0100();

                    //data.met03_0100 = comm.sGetInt32(reader["met03_0100"].ToString());
                    data.wrk_code = comm.sGetString(reader["wrk_code"].ToString());
                    //data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    //data.lot_no = comm.sGetString(reader["lot_no"].ToString());
                    //data.wmt06_0110 = comm.sGetInt32(reader["wmt06_0110"].ToString());

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
        public List<MET03_0100> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_met03_0100", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MET03_0100> list = new List<MET03_0100>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = " SELECT * FROM MET03_0100 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@met03_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MET03_0100 data = new MET03_0100();

                    //data.met03_0100 = comm.sGetInt32(reader["met03_0100"].ToString());
                    data.wrk_code = comm.sGetString(reader["wrk_code"].ToString());
                    //data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    //data.lot_no = comm.sGetString(reader["lot_no"].ToString());
                    //data.wmt06_0110 = comm.sGetInt32(reader["wmt06_0110"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.met03_0100)) {
                    //    data.can_delete = "N";
                    //    data.can_update = "N";
                    //}

                    list.Add(data);
                }
            }
            return list;
        }
        #endregion
        public List<MET03_0100> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<MET03_0100> list = new List<MET03_0100>();
            string foreignKey = gmv.GetKey<MET03_0000>(new MET03_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                //sSql = " SELECT MET03_0100.*, MEB20_0000.pro_name " +
                //       " FROM MET03_0100 " +
                //       " left join MEB20_0000 on MEB20_0000.pro_code = MET03_0100.pro_code " +
                //       " where MET03_0100. " + foreignKey + "=@" + foreignKey;

                sSql = "  SELECT MET03_0000.wrk_code,MET03_0000.work_code,MEB30_0000.work_name, " +
              " MET03_0000.wrk_code,MET03_0000.*,MEB30_0000.work_name, "+
                "MEB29_0000.station_name,BDP21_0100.field_name as mo_status_name"+
              " FROM MET03_0000 " +
              " left join MEB30_0000 on MEB30_0000.work_code = MET03_0000.work_code  " +
              " left join MEB29_0000 on MEB29_0000.station_code = MET03_0000.station_code  " +
              " left join BDP21_0100 on BDP21_0100.field_code = MET03_0000.mo_status and BDP21_0100.code_code = 'mo_status'  " +
              " where MET03_0000. " + foreignKey + "= '" + pTkCode +"' ";
            }
            else
            {
                sSql = "SELECT * FROM MET03_0100";
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
                    MET03_0100 data = new MET03_0100();

                    //data.met03_0100 = comm.sGetInt32(reader["met03_0100"].ToString());
                    //data.wrk_code = comm.sGetString(reader["wrk_code"].ToString());
                    //data.lot_no = comm.sGetString(reader["lot_no"].ToString());
                    //data.wmt06_0110 = comm.sGetInt32(reader["wmt06_0110"].ToString());
                    data.wrk_code = comm.sGetString(reader["wrk_code"].ToString());
                    data.work_code = comm.sGetString(reader["work_code"].ToString());
                    data.work_name = comm.sGetString(reader["work_name"].ToString());
                    data.station_name = comm.sGetString(reader["station_name"].ToString());
                    data.ins_date = comm.sGetString(reader["ins_date"].ToString());
                    data.ins_time = comm.sGetString(reader["ins_time"].ToString());
                    data.usr_code = comm.sGetString(reader["usr_code"].ToString());
                    data.station_name = comm.sGetString(reader["station_name"].ToString());
                    data.mo_status = comm.sGetString(reader["mo_status"].ToString());


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
        /// 傳入一個MET03_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MET03_0100">DTO</param>
        public void InsertData(MET03_0100 MET03_0100)
        {
            string sSql = "INSERT INTO " +
                          " MET03_0100 (  wrk_code,  pro_code,  lot_no,  wmt06_0110 ) " +
                          "     VALUES ( @wrk_code, @pro_code, @lot_no, @wmt06_0110 ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MET03_0100);
            }
        }

        /// <summary>
        /// 傳入一個MET03_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MET03_0100">DTO</param>
        public void UpdateData(MET03_0100 MET03_0100)
        {
            //string pTkCode = MET03_0100.met03_0100.ToString();
            //Int32 iProQty = comm.sGetInt32(comm.Get_Data("MET03_0100", pTkCode, "met03_0100", "pro_qty"));
            //Int32 iSorSerial = comm.sGetInt32(comm.Get_Data("MET03_0100", pTkCode, "met03_0100", "sor_serial"));

            //ws.Cal_TraQty("DEL", "STT01_0100", "res_qty", iProQty, "where stt01_0100=" + iSorSerial);
            //ws.Cal_TraQty("ADD", "STT01_0100", "res_qty", comm.sGetInt32(MET03_0100.pro_qty.ToString()), "where stt01_0100=" + comm.sGetString(MET03_0100.sor_serial.ToString()));


            string sSql = " UPDATE MET03_0100                " +
                          "    SET wrk_code   =  @wrk_code,  " +
                          "        pro_code   =  @pro_code,  " +
                          "        lot_no     =  @lot_no,    " +
                          "        wmt06_0110 =  @wmt06_0110 " +
                          "  WHERE met03_0100 =  @met03_0100 " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MET03_0100);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@met03_0100", MET03_0100.met03_0100));
                //sqlCommand.Parameters.Add(new SqlParameter("@met03_0100", MET03_0100.met03_0100));
                //sqlCommand.Parameters.Add(new SqlParameter("@wrk_code", MET03_0100.wrk_code));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MET03_0100 WHERE met03_0100 = @met03_0100;";
            //sSql += " Delete from BDP09_0100 where met03_0100 = @met03_0100; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { met03_0100 = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@met03_0100", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }
        ////暫存DataTable參考
        ////<summary>
        ////取得MET03_0100角色的DataTable
        ////</summary>
        ////<param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        ////<returns></returns>
        //public DataTable GetMET03_0100_dt(string pTkCode)
        //{
        //    DataTable dtTmp = new DataTable();
        //    DataTable dtDat = new DataTable();
        //    dtDat.Columns.Add("met03_0100", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("met03_0100", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("wrk_code", System.Type.GetType("System.String"].ToString());
        //    string sSql = "";
        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM MET03_0100";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM MET03_0100 where met03_0100='" + pTkCode + "'";
        //    }
        //    dtTmp = comm.Get_DataTable(sSql);

        //    int i;
        //    for (i = 1; i < dtTmp.Rows.Count - 1; i++)
        //    {
        //        DataRow drow = dtDat.NewRow();
        //        drow["met03_0100"] = dtTmp.Rows[i]["met03_0100"];
        //        drow["met03_0100"] = dtTmp.Rows[i]["met03_0100"];
        //        drow["wrk_code"] = dtTmp.Rows[i]["wrk_code"];
        //        dtDat.Rows.Add(drow);
        //    }
        //    return dtDat;
        //}
    }
}