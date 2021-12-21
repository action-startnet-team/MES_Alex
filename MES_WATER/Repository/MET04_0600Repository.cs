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
    public class MET04_0600Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        //// <summary>
        //// 取得MET04_0600資料表內容
        //// </summary>
        //// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        //// < returns > DTO MET04_0600</returns>
        public MET04_0600 GetDTO(string pTkCode)
        {
            MET04_0600 datas = new MET04_0600();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MET04_0600";
            }
            else
            {
                sSql = "SELECT * FROM MET04_0600 where ureport_code=@ureport_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@ureport_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MET04_0600
                        {
                            ureport_code = comm.sGetString(reader["ureport_code"].ToString()),
                            ureport_date = comm.sGetString(reader["ureport_date"].ToString()),
                            mo_code = comm.sGetString(reader["mo_code"].ToString()),
                            work_code = comm.sGetString(reader["work_code"].ToString()),
                            ng_code = comm.sGetString(reader["ng_code"].ToString()),
                            ng_qty = comm.sGetDouble(reader["ng_qty"].ToString()),
                         

                        };
                    }
                }
            }
            return datas;
        }

        #region
        //// <summary>
        //// 取得MET04_0600資料表內容
        //// </summary>
        //// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        //// < returns > List MET04_0600</returns>
        public List<MET04_0600> Get_DataList(string pTkCode)
        {
            List<MET04_0600> list = new List<MET04_0600>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MET04_0600";
            }
            else
            {
                sSql = "SELECT * FROM MET04_0600 where mo_code=@mo_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@mo_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MET04_0600 data = new MET04_0600();

                    data.ureport_code = comm.sGetString(reader["ureport_code"].ToString());
                    data.ureport_date = comm.sGetString(reader["ureport_date"].ToString());
                    data.mo_code = comm.sGetString(reader["mo_code"].ToString());
                    data.work_code = comm.sGetString(reader["work_code"].ToString());
                    data.ng_code = comm.sGetString(reader["ng_code"].ToString());
                    data.ng_qty = comm.sGetDouble(reader["ng_qty"].ToString());
                    data.ng_name = comm.Get_QueryData("MEB37_0000", data.ng_code, "ng_code", "ng_name");
                    data.can_delete = "Y";
                    data.can_update = "Y";
                    list.Add(data);
                }

            }
            return list;
        }

        //// <summary>
        //// 取得使用者可以編輯的資料，結合商務邏輯權限
        //// </summary>
        //// <param name = "pUsrCode" ></ param >
        //// < param name="pPrgCode"></param>
        //// <returns></returns>
        public List<MET04_0600> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_ureport_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MET04_0600> list = new List<MET04_0600>();
            string sSql = "";

            ////取得該使用者可以看的資料
            sSql = " SELECT * FROM MET04_0600 ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MET04_0600 data = new MET04_0600();

                    data.ureport_code = comm.sGetString(reader["ureport_code"].ToString());
                    data.ureport_date = comm.sGetString(reader["ureport_date"].ToString());
                    data.mo_code = comm.sGetString(reader["mo_code"].ToString());
                    data.work_code = comm.sGetString(reader["work_code"].ToString());
                    data.ng_code = comm.sGetString(reader["ng_code"].ToString());
                    data.ng_qty = comm.sGetDouble(reader["ng_qty"].ToString());
                  

                    ////檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    ////資料邏輯刪除、修改
                    if (arr_LockGrpCode.Contains(data.ureport_code))
                    {
                        data.can_delete = "N";
                        data.can_update = "N";
                    }

                    list.Add(data);
                }
            }
            return list;
        }
        #endregion
        /// <summary>
        /// 取得查詢資料，結合使用者權限
        /// </summary>
        /// <param name="pUsrCode">使用者代碼</param>
        /// <param name="pPrgCode">功能代碼</param>
        /// <param name="pWhere">JSON查詢字串</param>
        /// <returns></returns>
        public List<MET04_0600> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MET04_0600> list = new List<MET04_0600>();

            string sSql = " SELECT MET04_0600.*, MEB30_0000.work_name as work_name , MEB37_0000.ng_name as ng_name" +
                                " FROM MET04_0600" +
                                " left join MEB30_0000 on MEB30_0000.work_code = MET04_0600.work_code " +
                                " left join MEB37_0000 on MEB37_0000.ng_code = MET04_0600.ng_code ";

            // 取得資料
            list = comm.Get_ListByQuery<MET04_0600>(sSql, pWhere, pUsrCode, pPrgCode);

            // 權限設定
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);

            for (int i = 0; i < list.Count; i++)
            {
                //檢查授權刪除、修改
                list[i].can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                list[i].can_update = sLimitStr.Contains("M") ? "Y" : "N";

            }

            return list;

        }
        /// <summary>
        /// 傳入一個MET04_0600的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name = "MET04_0600" > DTO </ param >
        public void InsertData(MET04_0600 MET04_0600)
        {
            string sSql = "INSERT INTO " +
                          " MET04_0600 (   ureport_code,  ureport_date,  mo_code,  work_code,  ng_code,  ng_qty ) " +
                          "     VALUES (  @ureport_code, @ureport_date, @mo_code, @work_code, @ng_code, @ng_qty ) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MET04_0600);
            }
        }

        /// <summary>
        /// 傳入一個MET04_0600的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name = "MET04_0600" > DTO </ param >
        public void UpdateData(MET04_0600 MET04_0600)
        {

            string sSql = " UPDATE MET04_0600                     " +
                          "    SET ureport_date =  @ureport_date, " +
                          "        mo_code     =  @mo_code,     " +
                          "        work_code      =  @work_code,      " +
                          "        ng_code     =  @ng_code,     " +
                          "        ng_qty       =  @ng_qty       " +

                          "  WHERE ureport_code   =  @ureport_code    ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MET04_0600);

            }
        }

        //// <summary>
        //// 傳入一個鍵值，刪除、一次刪除一筆
        //// </summary>
        //// <param name = "pTkCode" > 資料鍵值 </ param >
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MET04_0600 WHERE ureport_code = @ureport_code;";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { ureport_code = pTkCode });
            }
        }
    }
}