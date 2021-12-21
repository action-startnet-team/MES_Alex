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
    public class MET04_0000Repository
    {
        Comm comm = new Comm();
        
        /// <summary>
        /// 取得MET01_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MET01_0000</returns>
        public List<MET01_0000> Get_DataList(string pTkCode)
        {
            List<MET01_0000> list = new List<MET01_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM MET01_0000";
            }
            else
            {
                sSql = "SELECT * FROM MET01_0000 where mo_code=@mo_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@mo_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MET01_0000 data = new MET01_0000();

                    data.mo_code = comm.sGetString(reader["mo_code"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.bom_code = comm.sGetString(reader["bom_code"].ToString());
                    data.sor_code = comm.sGetString(reader["sor_code"].ToString());
                    data.ord_code = comm.sGetString(reader["ord_code"].ToString());
                    data.cus_code = comm.sGetString(reader["cus_code"].ToString());
                    data.plan_start_date = comm.sGetString(reader["plan_start_date"].ToString());
                    data.plan_end_date = comm.sGetString(reader["plan_end_date"].ToString());
                    data.plan_out_date = comm.sGetString(reader["plan_out_date"].ToString());
                    data.plan_line_code = comm.sGetString(reader["plan_line_code"].ToString());
                    data.plan_qty = comm.sGetDecimal(reader["plan_qty"].ToString());
                    data.pro_unit = comm.sGetString(reader["pro_unit"].ToString());
                    data.sch_date_s = comm.sGetString(reader["sch_date_s"].ToString());
                    data.sch_date_e = comm.sGetString(reader["sch_date_e"].ToString());
                    data.sch_time_s = comm.sGetString(reader["sch_time_s"].ToString());
                    data.sch_time_e = comm.sGetString(reader["sch_time_e"].ToString());

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
        public List<MET01_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_mo_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<MET01_0000> list = new List<MET01_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = "SELECT * FROM MET01_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    MET01_0000 data = new MET01_0000();

                    data.mo_code = comm.sGetString(reader["mo_code"].ToString());
                    data.pro_code = comm.sGetString(reader["pro_code"].ToString());
                    data.bom_code = comm.sGetString(reader["bom_code"].ToString());
                    data.sor_code = comm.sGetString(reader["sor_code"].ToString());
                    data.ord_code = comm.sGetString(reader["ord_code"].ToString());
                    data.cus_code = comm.sGetString(reader["cus_code"].ToString());
                    data.plan_start_date = comm.sGetString(reader["plan_start_date"].ToString());
                    data.plan_end_date = comm.sGetString(reader["plan_end_date"].ToString());
                    data.plan_out_date = comm.sGetString(reader["plan_out_date"].ToString());
                    data.plan_line_code = comm.sGetString(reader["plan_line_code"].ToString());
                    data.plan_qty = comm.sGetDecimal(reader["plan_qty"].ToString());
                    data.pro_unit = comm.sGetString(reader["pro_unit"].ToString());
                    data.sch_date_s = comm.sGetString(reader["sch_date_s"].ToString());
                    data.sch_date_e = comm.sGetString(reader["sch_date_e"].ToString());
                    data.sch_time_s = comm.sGetString(reader["sch_time_s"].ToString());
                    data.sch_time_e = comm.sGetString(reader["sch_time_e"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

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
        public List<MET01_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MET01_0000> list = new List<MET01_0000>();

            string sSql = " SELECT MET01_0000.*, MEB20_0000.pro_name, MEB23_0000.bom_name, MEB25_0000.cus_name, MEB12_0000.line_name as plan_line_name " +
                          " FROM MET01_0000" +
                          " left join MEB20_0000 on MEB20_0000.pro_code = MET01_0000.pro_code " +
                          " left join MEB23_0000 on MEB23_0000.bom_code = MET01_0000.bom_code " +
                          " left join MEB25_0000 on MEB25_0000.cus_code = MET01_0000.cus_code " +
                          " left join MEB12_0000 on MEB12_0000.line_code = MET01_0000.plan_line_code ";

            // 取得資料
            list = comm.Get_ListByQuery<MET01_0000>(sSql, pWhere, pUsrCode, pPrgCode);

            // 權限設定
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);

            for (int i = 0; i < list.Count; i++)
            {
                //檢查授權刪除、修改
                list[i].can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                list[i].can_update = sLimitStr.Contains("M") ? "Y" : "N";

                string mo_code = list[i].mo_code;
                bool bProOk = comm.Chk_Mo_IsOk("MET04_0100", mo_code);
                bool bMacOk = comm.Chk_Mo_IsOk("MET04_0200", mo_code);
                list[i].mo_end_date = list[i].plan_start_date;
                list[i].mo_qty = Convert.ToDecimal(comm.Get_MoQty(mo_code));
                list[i].up_qty = comm.Get_UpQty(mo_code);
                if (bProOk) { list[i].is_pro_ok = "已確認"; }
                if (bMacOk) { list[i].is_mac_ok = "已確認"; }

                
            }

            return list;

        }
        
    }
}