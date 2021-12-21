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
    public class MEB12_0100Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得MEB12_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEB12_0100</returns>
        public MEB12_0100 GetDTO(string pTkCode)
        {
            MEB12_0100 datas = new MEB12_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB12_0100";
            }
            else
            {
                sSql = "SELECT * FROM MEB12_0100 where meb12_0100=@meb12_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@meb12_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEB12_0100
                        {
                            meb12_0100 = comm.sGetInt32(reader["meb12_0100"].ToString()),
                            line_code = comm.sGetString(reader["line_code"].ToString()),
                            pro_code = comm.sGetString(reader["pro_code"].ToString()),
                            std_qty = comm.sGetInt32(reader["std_qty"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MEB12_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MEB12_0100</returns>
        public List<MEB12_0100> Get_DataList(string pTkCode)
        {
            List<MEB12_0100> list = new List<MEB12_0100>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB12_0100";
            }
            else
            {
                sSql = "SELECT * FROM MEB12_0100 where meb12_0100=@meb12_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@meb12_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB12_0100 data = new MEB12_0100();

                    data.meb12_0100 = comm.sGetInt32(reader["meb12_0100"].ToString());
                    data.line_code = comm.sGetString(reader["line_code"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.std_qty = comm.sGetInt32(reader["std_qty"].ToString());

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
        public List<MEB12_0100> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_meb12_0100", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MEB12_0100> list = new List<MEB12_0100>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = " SELECT * FROM MEB12_0100 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@meb12_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB12_0100 data = new MEB12_0100();

                    data.meb12_0100 = comm.sGetInt32(reader["meb12_0100"].ToString());
                    data.line_code = comm.sGetString(reader["line_code"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.std_qty = comm.sGetInt32(reader["std_qty"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.meb12_0100)) {
                    //    data.can_delete = "N";
                    //    data.can_update = "N";
                    //}

                    list.Add(data);
                }
            }
            return list;
        }
        #endregion
        public List<MEB12_0100> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<MEB12_0100> list = new List<MEB12_0100>();
            string foreignKey = gmv.GetKey<MEB12_0000>(new MEB12_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT MEB12_0100.*, MEB12_0000.line_name as line_name, MEB20_0000.pro_name  as pro_name              " +
                       " FROM MEB12_0100                                                                                       " +
                       " left join MEB12_0000 on MEB12_0000.line_code = MEB12_0100.line_code        " +
                       " left join MEB20_0000 on MEB20_0000.pro_code = MEB12_0100.pro_code          " +
                       " where MEB12_0100. " + foreignKey + "=@" + foreignKey;
            }
            else
            {
                sSql = "SELECT * FROM MEB12_0100";
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

                    MEB12_0100 data = new MEB12_0100();

                    data.meb12_0100 = comm.sGetInt32(reader["meb12_0100"].ToString());
                    data.line_code = comm.sGetString(reader["line_code"].ToString());
                    data.line_name = comm.sGetString(reader["line_name"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.pro_name = comm.sGetString(reader["pro_name"].ToString());
                    data.std_qty = comm.sGetInt32(reader["std_qty"].ToString());


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
        /// 傳入一個MEB12_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEB12_0100">DTO</param>
        public void InsertData(MEB12_0100 MEB12_0100)
        {
            string sSql = "INSERT INTO " +
                          " MEB12_0100  (    line_code,       pro_code,      std_qty ) " +
                          "     VALUES      (  @line_code,   @pro_code,   @std_qty ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB12_0100);
            }
        }

        /// <summary>
        /// 傳入一個MEB12_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEB12_0100">DTO</param>
        public void UpdateData(MEB12_0100 MEB12_0100)
        {
            //string pTkCode = MEB12_0100.meb12_0100.ToString();
            //Int32 iProQty = comm.sGetInt32(comm.Get_Data("MEB12_0100", pTkCode, "meb12_0100", "pro_qty"));
            //Int32 iSorSerial = comm.sGetInt32(comm.Get_Data("MEB12_0100", pTkCode, "meb12_0100", "sor_serial"));

            //ws.Cal_TraQty("DEL", "STT01_0100", "res_qty", iProQty, "where stt01_0100=" + iSorSerial);
            //ws.Cal_TraQty("ADD", "STT01_0100", "res_qty", comm.sGetInt32(MEB12_0100.pro_qty.ToString()), "where stt01_0100=" + comm.sGetString(MEB12_0100.sor_serial.ToString()));


            string sSql = " UPDATE MEB12_0100            " +
                          "    SET line_code = @line_code, " +
                          "        pro_code = @pro_code, " +
                          "        std_qty = @std_qty  " +
                          "  WHERE meb12_0100 = @meb12_0100  ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB12_0100);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@meb12_0100", MEB12_0100.meb12_0100));
                //sqlCommand.Parameters.Add(new SqlParameter("@meb12_0100", MEB12_0100.meb12_0100));
                //sqlCommand.Parameters.Add(new SqlParameter("@line_code", MEB12_0100.line_code));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MEB12_0100 WHERE meb12_0100 = @meb12_0100;";
            //sSql += " Delete from BDP09_0100 where meb12_0100 = @meb12_0100; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { meb12_0100 = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@meb12_0100", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得MEB12_0100角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetMEB12_0100_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();
            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("meb12_0100", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("meb12_0100", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("line_code", System.Type.GetType("System.String"].ToString());
            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB12_0100";
            }
            else
            {
                sSql = "SELECT * FROM MEB12_0100 where meb12_0100='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["meb12_0100"] = dtTmp.Rows[i]["meb12_0100"];
                drow["meb12_0100"] = dtTmp.Rows[i]["meb12_0100"];
                drow["line_code"] = dtTmp.Rows[i]["line_code"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}