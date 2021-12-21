﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

using MES_WATER.Models;
using Dapper;


namespace MES_WATER.Repository
{
    public class EMB05_0200Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得EMB05_0200資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO EMB05_0200</returns>
        public EMB05_0200 GetDTO(string pTkCode)
        {
            EMB05_0200 datas = new EMB05_0200();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMB05_0200";
            }
            else
            {
                sSql = "SELECT * FROM EMB05_0200 where mai_type_code=@mai_type_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@mai_type_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new EMB05_0200
                        {
                            emb05_0200 = comm.sGetInt32(reader["emb05_0200"].ToString()),
                            mai_type_code = comm.sGetString(reader["mai_type_code"].ToString()),
                            fault_reason_code = comm.sGetString(reader["fault_reason_code"].ToString()),

                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得EMB05_0200資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List EMB05_0200</returns>
        public List<EMB05_0200> Get_DataList(string pTkCode)
        {
            List<EMB05_0200> list = new List<EMB05_0200>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMB05_0200";
            }
            else
            {
                sSql = "SELECT * FROM EMB05_0200 where mai_type_code=@mai_type_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@mai_type_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EMB05_0200 data = new EMB05_0200();

                    data.emb05_0200 = comm.sGetInt32(reader["emb05_0200"].ToString());
                    data.mai_type_code = comm.sGetString(reader["mai_type_code"].ToString());
                    data.fault_reason_code = comm.sGetString(reader["fault_reason_code"].ToString());

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
        public List<EMB05_0200> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_mai_type_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<EMB05_0200> list = new List<EMB05_0200>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = " SELECT * FROM EMB05_0200 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@mai_type_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EMB05_0200 data = new EMB05_0200();

                    data.emb05_0200 = comm.sGetInt32(reader["emb05_0200"].ToString());
                    data.mai_type_code = comm.sGetString(reader["mai_type_code"].ToString());
                    data.fault_reason_code = comm.sGetString(reader["fault_reason_code"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    list.Add(data);
                }
            }
            return list;
        }
        #endregion
        public List<EMB05_0200> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<EMB05_0200> list = new List<EMB05_0200>();
            //string foreignKey = gmv.GetKey<EMB05_0000>();
            string foreignKey = gmv.GetKey<EMB19_0000>(new EMB19_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT EMB05_0200.*, EMB19_0000.fault_reason_name " +
                       " FROM EMB05_0200 " +
                       " left join EMB19_0000 on EMB19_0000.fault_reason_code = EMB05_0200.fault_reason_code " +
                       " where EMB05_0200. " + foreignKey + "=@" + foreignKey;
            }
            else
            {
                sSql = "SELECT * FROM EMB05_0200 " ;
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

                    EMB05_0200 data = new EMB05_0200();

                    data.emb05_0200 = comm.sGetInt32(reader["emb05_0200"].ToString());
                    data.mai_type_code = comm.sGetString(reader["mai_type_code"].ToString());
                    data.fault_reason_code = comm.sGetString(reader["fault_reason_code"].ToString());
                    data.fault_reason_name = comm.sGetString(reader["fault_reason_name"].ToString());

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
        /// 取得EMB05_0200的資料
        /// </summary>
        /// <param name="pUsrCode"></param>
        /// <param name="pPrgCode"></param>
        /// <param name="pTkCode"></param>
        /// <param name="pTkValue"></param>
        /// <returns></returns>
        public List<EMB05_0200> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode, string pTkValue)
        {
            List<EMB05_0200> list = new List<EMB05_0200>();
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT EMB05_0200.*, EMB19_0000.fault_reason_name " +
                       " FROM EMB05_0200 " +
                       " left join EMB19_0000 on EMB19_0000.fault_reason_code = EMB05_0200.fault_reason_code " +
                       " where EMB05_0200. " + pTkCode + "=@" + pTkCode;
            }
            else
            {
                sSql = "SELECT * FROM EMB05_0200";
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

                    EMB05_0200 data = new EMB05_0200();

                    data.emb05_0200 = comm.sGetInt32(reader["emb05_0200"].ToString());
                    data.mai_type_code = comm.sGetString(reader["mai_type_code"].ToString());
                    data.fault_reason_code = comm.sGetString(reader["fault_reason_code"].ToString());
                    data.fault_reason_name = comm.sGetString(reader["fault_reason_name"].ToString());

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
        /// 傳入一個EMB05_0200的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="EMB05_0200">DTO</param>
        /// 

        //取得識別碼

        public void InsertData(EMB05_0200 EMB05_0200)
        {
            //EMB05_0200.emb05_0200 = comm.sGetInt32(ws.AutoInt2("EMB05_0200").ToString());

            string sSql = " INSERT INTO " +
                          " EMB05_0200 (  mai_type_code ,  fault_reason_code   ) " +
                          "     VALUES ( @mai_type_code , @fault_reason_code   ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMB05_0200);
            }
        }


        /// <summary>
        /// 傳入一個EMB05_0200的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="EMB05_0200">DTO</param>
        public void UpdateData(EMB05_0200 EMB05_0200)
        {
            //string pTkCode = EMB05_0200.mai_type_code.ToString();
            //Int32 iProQty = comm.sGetInt32(comm.Get_Data("EMB05_0200", pTkCode, "mai_type_code", "pro_qty"));
            //Int32 iSorSerial = comm.sGetInt32(comm.Get_Data("EMB05_0200", pTkCode, "mai_type_code", "sor_serial"));
            //ws.Cal_TraQty("DEL", "STT01_0100", "res_qty", iProQty, "where stt01_0100=" + iSorSerial);
            //ws.Cal_TraQty("ADD", "STT01_0100", "res_qty", comm.sGetInt32(EMB05_0200.pro_qty.ToString()), "where stt01_0100=" + comm.sGetString(EMB05_0200.sor_serial.ToString()));


            string sSql = " UPDATE EMB05_0200                             " +
                          "    SET mai_type_code     = @mai_type_code,    " +
                          "        fault_reason_code = @fault_reason_code " +
                          "  WHERE emb05_0200        = @emb05_0200        ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EMB05_0200);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@mai_type_code", EMB05_0200.mai_type_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@mai_type_code", EMB05_0200.mai_type_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@emb05_0200", EMB05_0200.emb05_0200));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM EMB05_0200 WHERE emb05_0200 = @emb05_0200;";
            //sSql += " Delete from BDP09_0100 where mai_type_code = @mai_type_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { emb05_0200 = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@mai_type_code", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }


        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得EMB05_0200角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetEMB05_0200_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();
            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("mai_type_code", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("mai_type_code", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("emb05_0200", System.Type.GetType("System.String"].ToString());
            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EMB05_0200";
            }
            else
            {
                sSql = "SELECT * FROM EMB05_0200 where mai_type_code='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["mai_type_code"] = dtTmp.Rows[i]["mai_type_code"];
                drow["mai_type_code"] = dtTmp.Rows[i]["mai_type_code"];
                drow["emb05_0200"] = dtTmp.Rows[i]["emb05_0200"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}