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
    public class MEB43_0100Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得MEB43_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEB43_0100</returns>
        public MEB43_0100 GetDTO(string pTkCode)
        {
            MEB43_0100 datas = new MEB43_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB43_0100";
            }
            else
            {
                sSql = "SELECT * FROM MEB43_0100 where ng_memo_code=@ng_memo_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@ng_memo_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEB43_0100
                        {
                            meb43_0100 = comm.sGetInt32(reader["meb43_0100"].ToString()),
                            ng_memo_code = comm.sGetString(reader["ng_memo_code"].ToString()),
                            ng_code = comm.sGetString(reader["ng_code"].ToString()),

                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MEB43_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MEB43_0100</returns>
        public List<MEB43_0100> Get_DataList(string pTkCode)
        {
            List<MEB43_0100> list = new List<MEB43_0100>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB43_0100";
            }
            else
            {
                sSql = "SELECT * FROM MEB43_0100 where ng_memo_code=@ng_memo_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@ng_memo_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB43_0100 data = new MEB43_0100();

                    data.meb43_0100 = comm.sGetInt32(reader["meb43_0100"].ToString());
                    data.ng_memo_code = comm.sGetString(reader["ng_memo_code"].ToString());
                    data.ng_code = comm.sGetString(reader["ng_code"].ToString());

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
        public List<MEB43_0100> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_ng_memo_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MEB43_0100> list = new List<MEB43_0100>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = " SELECT * FROM MEB43_0100 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@ng_memo_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB43_0100 data = new MEB43_0100();

                    data.meb43_0100 = comm.sGetInt32(reader["meb43_0100"].ToString());
                    data.ng_memo_code = comm.sGetString(reader["ng_memo_code"].ToString());
                    data.ng_code = comm.sGetString(reader["ng_code"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.ng_memo_code)) {
                    //    data.can_delete = "N";
                    //    data.can_update = "N";
                    //}

                    list.Add(data);
                }
            }
            return list;
        }
        #endregion
        public List<MEB43_0100> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<MEB43_0100> list = new List<MEB43_0100>();
            //string foreignKey = gmv.GetKey<MEB43_0000>();
            string foreignKey = gmv.GetKey<MEB37_0000>(new MEB37_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT MEB43_0100.*, MEB37_0000.ng_name , MEB43_0000.ng_memo_name " +
                       " FROM MEB43_0100 " +
                       " left join MEB37_0000 on MEB37_0000.ng_code = MEB43_0100.ng_code " +
                       " left join MEB43_0000 on MEB43_0000.ng_memo_code = MEB43_0100.ng_memo_code " +
                       " where MEB43_0100. " + foreignKey + "=@" + foreignKey;
            }
            else
            {
                sSql = "SELECT * FROM MEB43_0100";
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

                    MEB43_0100 data = new MEB43_0100();

                    data.meb43_0100 = comm.sGetInt32(reader["meb43_0100"].ToString());
                    data.ng_memo_code = comm.sGetString(reader["ng_memo_code"].ToString());
                    data.ng_name = comm.sGetString(reader["ng_name"].ToString());
                    data.ng_code = comm.sGetString(reader["ng_code"].ToString());
                    data.ng_memo_name = comm.sGetString(reader["ng_memo_name"].ToString());

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
        /// 取得MEB43_0100的資料
        /// </summary>
        /// <param name="pUsrCode"></param>
        /// <param name="pPrgCode"></param>
        /// <param name="pTkCode"></param>
        /// <param name="pTkValue"></param>
        /// <returns></returns>
        public List<MEB43_0100> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode, string pTkValue)
        {
            List<MEB43_0100> list = new List<MEB43_0100>();
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT MEB43_0100.*, MEB37_0000.ng_memo_name " +
                       " FROM MEB43_0100 " +
                       " left join MEB37_0000 on MEB37_0000.ng_code = MEB43_0100.ng_code " +
                       " where MEB43_0100. " + pTkCode + "=@" + pTkCode;
            }
            else
            {
                sSql = "SELECT * FROM MEB43_0100";
            }

            //取得該使用者可以看的資料
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter(pTkCode, pTkValue));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {

                    MEB43_0100 data = new MEB43_0100();

                    data.meb43_0100 = comm.sGetInt32(reader["meb43_0100"].ToString());
                    data.ng_memo_code = comm.sGetString(reader["ng_memo_code"].ToString());
                    data.ng_code = comm.sGetString(reader["ng_code"].ToString());
                    data.ng_memo_name = comm.sGetString(reader["ng_memo_name"].ToString());

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
        /// 傳入一個MEB43_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEB43_0100">DTO</param>
        /// 

        //取得識別碼

        public void InsertData(MEB43_0100 MEB43_0100)
        {
            //MEB43_0100.meb43_0100 = comm.sGetInt32(ws.AutoInt2("MEB43_0100").ToString());

            string sSql = " INSERT INTO " +
                          " MEB43_0100 (  ng_memo_code ,  ng_code   ) " +
                          "     VALUES ( @ng_memo_code , @ng_code   ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB43_0100);
            }
        }


        /// <summary>
        /// 傳入一個MEB43_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEB43_0100">DTO</param>
        public void UpdateData(MEB43_0100 MEB43_0100)
        {
            //string pTkCode = MEB43_0100.ng_memo_code.ToString();
            //Int32 iProQty = comm.sGetInt32(comm.Get_Data("MEB43_0100", pTkCode, "ng_memo_code", "pro_qty"));
            //Int32 iSorSerial = comm.sGetInt32(comm.Get_Data("MEB43_0100", pTkCode, "ng_memo_code", "sor_serial"));
            //ws.Cal_TraQty("DEL", "STT01_0100", "res_qty", iProQty, "where stt01_0100=" + iSorSerial);
            //ws.Cal_TraQty("ADD", "STT01_0100", "res_qty", comm.sGetInt32(MEB43_0100.pro_qty.ToString()), "where stt01_0100=" + comm.sGetString(MEB43_0100.sor_serial.ToString()));


            string sSql = " UPDATE MEB43_0100                     " +
                          "    SET ng_memo_code     = @ng_memo_code,    " +
                          "        ng_code = @ng_code " +
                          "  WHERE meb43_0100    = @meb43_0100    ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB43_0100);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@ng_memo_code", MEB43_0100.ng_memo_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@ng_memo_code", MEB43_0100.ng_memo_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@meb43_0100", MEB43_0100.meb43_0100));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MEB43_0100 WHERE meb43_0100 = @meb43_0100;";
            //sSql += " Delete from BDP09_0100 where ng_memo_code = @ng_memo_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { meb43_0100 = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@ng_memo_code", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }


        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得MEB43_0100角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetMEB43_0100_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();
            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("ng_memo_code", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("ng_memo_code", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("meb43_0100", System.Type.GetType("System.String"].ToString());
            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB43_0100";
            }
            else
            {
                sSql = "SELECT * FROM MEB43_0100 where ng_memo_code='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["ng_memo_code"] = dtTmp.Rows[i]["ng_memo_code"];
                drow["ng_memo_code"] = dtTmp.Rows[i]["ng_memo_code"];
                drow["meb43_0100"] = dtTmp.Rows[i]["meb43_0100"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}