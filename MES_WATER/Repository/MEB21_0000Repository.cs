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
    public class MEB21_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MEB21_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MEB21_0000</returns>
        public MEB21_0000 GetDTO(string pTkCode)
        {
            MEB21_0000 datas = new MEB21_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB21_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEB21_0000 where pro_type_code=@pro_type_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@pro_type_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MEB21_0000
                        {

                            pro_type_code = comm.sGetString(reader["pro_type_code"].ToString()),
                            pro_type_name = comm.sGetString(reader["pro_type_name"].ToString()),
                            wip_type = comm.sGetString(reader["wip_type"].ToString()),
                            is_consum = comm.sGetString(reader["is_consum"].ToString()),
                            dis_type = comm.sGetString(reader["dis_type"].ToString()),
                            chk_type = comm.sGetString(reader["chk_type"].ToString()),
                            cmemo = comm.sGetString(reader["cmemo"].ToString()),

                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MEB21_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MEB21_0000</returns>
        public List<MEB21_0000> Get_DataList(string pTkCode)
        {
            List<MEB21_0000> list = new List<MEB21_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB21_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEB21_0000 where pro_type_code=@pro_type_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@pro_type_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB21_0000 data = new MEB21_0000();

                    data.pro_type_code = comm.sGetString(reader["pro_type_code"].ToString());
                    data.pro_type_name = comm.sGetString(reader["pro_type_name"].ToString());
                    data.wip_type = comm.sGetString(reader["wip_type"].ToString());
                    data.is_consum = comm.sGetString(reader["is_consum"].ToString());
                    data.dis_type = comm.sGetString(reader["dis_type"].ToString());
                    data.chk_type = comm.sGetString(reader["chk_type"].ToString());
                    data.cmemo = comm.sGetString(reader["cmemo"].ToString());


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
        public List<MEB21_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_pro_type_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MEB21_0000> list = new List<MEB21_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = "SELECT * FROM MEB21_0000";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@pro_type_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MEB21_0000 data = new MEB21_0000();

                    data.pro_type_code = comm.sGetString(reader["pro_type_code"].ToString());
                    data.pro_type_name = comm.sGetString(reader["pro_type_name"].ToString());
                    data.wip_type = comm.sGetString(reader["wip_type"].ToString());
                    data.is_consum = comm.sGetString(reader["is_consum"].ToString());
                    data.dis_type = comm.sGetString(reader["dis_type"].ToString());
                    data.chk_type = comm.sGetString(reader["chk_type"].ToString());
                    data.cmemo = comm.sGetString(reader["cmemo"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.pro_type_code)) {
                    //    data.can_delete = "N";
                    //    data.can_update = "N";
                    //}

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
        public List<MEB21_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MEB21_0000> list = new List<MEB21_0000>();

            string sSql = " SELECT distinct MEB21_0000.pro_type_code, MEB21_0000.* , A.field_name as wip_type_name, B.field_name as dis_type_name         " +
                          " FROM MEB21_0000                                                                         " +
                          " left join MEB30_0200 on MEB30_0200.pro_type_code = MEB21_0000.pro_type_code                  " +
                          " left join BDP21_0100 as A on A.field_code = MEB21_0000.wip_type AND A.code_code = 'wip_type' " +
                          " left join BDP21_0100 as B on B.field_code = MEB21_0000.dis_type AND B.code_code = 'dis_type' ";

            // 取得資料
            list = comm.Get_ListByQuery<MEB21_0000>(sSql, pWhere, pUsrCode, pPrgCode);

            // 權限設定
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            //string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_mtp_code", "par_name", "par_value");
            //var arr_LockGrpCode = sLockGrpCode.Split(',');

            for (int i = 0; i < list.Count; i++)
            {
                //檢查授權刪除、修改
                list[i].can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                list[i].can_update = sLimitStr.Contains("M") ? "Y" : "N";
                list[i].pro_type_code = list[i].pro_type_code.Trim();

                //        // 特例 轉換
                //        data.sup_name = data.sup_code + " - " + comm.sGetString(reader["sup_name"].ToString());
                //        data.sto_name = comm.sGetString(reader["sto_code"].ToString()) + " - " + comm.sGetString(reader["sto_name"].ToString());

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
        /// 傳入一個MEB21_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MEB21_0000">DTO</param>
        public void InsertData(MEB21_0000 MEB21_0000)
        {
            string sSql = "INSERT INTO " +
                          " MEB21_0000 (  pro_type_code,  pro_type_name,  wip_type,  is_consum,  dis_type,  chk_type,  cmemo ) " +
                          "     VALUES ( @pro_type_code, @pro_type_name, @wip_type, @is_consum, @dis_type, @chk_type, @cmemo ) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB21_0000);
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@pro_type_code", MEB21_0000.pro_type_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@pro_type_code", MEB21_0000.pro_type_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@area_name", MEB21_0000.area_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個MEB21_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MEB21_0000">DTO</param>
        public void UpdateData(MEB21_0000 MEB21_0000)
        {
            string sSql = " UPDATE MEB21_0000                        " +
                          "    SET pro_type_name =  @pro_type_name,  " +
                          "        wip_type      =  @wip_type,       " +
                          "        is_consum     =  @is_consum,      " +
                          "        dis_type      =  @dis_type,       " +
                          "        chk_type      =  @chk_type,       " +
                          "        cmemo         =  @cmemo           " +
                          "  WHERE pro_type_code =  @pro_type_code   ";


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MEB21_0000);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@pro_type_code", MEB21_0000.pro_type_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@pro_type_code", MEB21_0000.pro_type_code));
                //sqlCommand.Parameters.Add(new SqlParameter("@area_name", MEB21_0000.area_name));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM MEB21_0000 WHERE pro_type_code = @pro_type_code;";
            //sSql += " Delete from BDP09_0100 where pro_type_code = @pro_type_code; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { pro_type_code = pTkCode });

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@pro_type_code", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }






        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得MEB21_0000角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetMEB21_0000_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();

            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("pro_type_code", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("pro_type_code", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("area_name", System.Type.GetType("System.String"].ToString());

            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MEB21_0000";
            }
            else
            {
                sSql = "SELECT * FROM MEB21_0000 where pro_type_code='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["pro_type_code"] = dtTmp.Rows[i]["pro_type_code"];
                drow["pro_type_code"] = dtTmp.Rows[i]["pro_type_code"];
                drow["area_name"] = dtTmp.Rows[i]["area_name"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}