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
    public class MET01_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MET01_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MET01_0000</returns>
        public MET01_0000 GetDTO(string pTkCode)
        {
            MET01_0000 datas = new MET01_0000();
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

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new MET01_0000
                        {
                            mo_code = comm.sGetString(reader["mo_code"].ToString()),
                            pro_code = comm.sGetString(reader["pro_code"].ToString()),
                            bom_code = comm.sGetString(reader["bom_code"].ToString()),
                            sor_code = comm.sGetString(reader["sor_code"].ToString()),
                            ord_code = comm.sGetString(reader["ord_code"].ToString()),
                            cus_code = comm.sGetString(reader["cus_code"].ToString()),
                            plan_start_date = comm.sGetString(reader["plan_start_date"].ToString()),
                            plan_end_date = comm.sGetString(reader["plan_end_date"].ToString()),
                            plan_out_date = comm.sGetString(reader["plan_out_date"].ToString()),
                            plan_line_code = comm.sGetString(reader["plan_line_code"].ToString()),
                            plan_qty = comm.sGetDecimal(reader["plan_qty"].ToString()),
                            pro_unit = comm.sGetString(reader["pro_unit"].ToString()),
                            mo_status = comm.sGetString(reader["mo_status"].ToString()),
                            err_memo = comm.sGetString(reader["err_memo"].ToString()),
                            mo_start_date = comm.sGetString(reader["mo_start_date"].ToString()),
                            mo_end_date = comm.sGetString(reader["mo_end_date"].ToString()),
                            mo_out_date = comm.sGetString(reader["mo_out_date"].ToString()),
                            mo_qty = comm.sGetDecimal(reader["mo_qty"].ToString()),
                            sch_date_s = comm.sGetString(reader["sch_date_s"].ToString()),
                            sch_date_e = comm.sGetString(reader["sch_date_e"].ToString()),
                            sch_time_s = comm.sGetString(reader["sch_time_s"].ToString()),
                            sch_time_e = comm.sGetString(reader["sch_time_e"].ToString()),
                            ins_date = comm.sGetString(reader["ins_date"].ToString()),
                            ins_time = comm.sGetString(reader["ins_time"].ToString()),
                            usr_code = comm.sGetString(reader["usr_code"].ToString()),
                            last_date = comm.sGetString(reader["last_date"].ToString()),
                            last_time = comm.sGetString(reader["last_time"].ToString()),
                            mo_memo = comm.sGetString(reader["mo_memo"].ToString()),
                            mo_type = comm.sGetString(reader["mo_type"].ToString()),
                            mo_level = comm.sGetString(reader["mo_level"].ToString()),
                            up_mo_code = comm.sGetString(reader["up_mo_code"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得MET01_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MET01_0000</returns>
        //public List<MET01_0000> Get_DataList(string pTkCode)
        //{
        //    List<MET01_0000> list = new List<MET01_0000>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM MET01_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM MET01_0000 where mo_code=@mo_code";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@mo_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            MET01_0000 data = new MET01_0000();

        //            data.mo_code = comm.sGetString(reader["mo_code"].ToString());
        //            data.pro_code = comm.sGetString(reader["pro_code"].ToString());
        //            data.bom_code = comm.sGetString(reader["bom_code"].ToString());
        //            data.sor_code = comm.sGetString(reader["sor_code"].ToString());
        //            data.ord_code = comm.sGetString(reader["ord_code"].ToString());
        //            data.cus_code = comm.sGetString(reader["cus_code"].ToString());
        //            data.plan_start_date = comm.sGetString(reader["plan_start_date"].ToString());
        //            data.plan_end_date = comm.sGetString(reader["plan_end_date"].ToString());
        //            data.plan_out_date = comm.sGetString(reader["plan_out_date"].ToString());
        //            data.plan_line_code = comm.sGetString(reader["plan_line_code"].ToString());
        //            data.plan_qty = comm.sGetDecimal(reader["plan_qty"].ToString());
        //            data.plan_qty = comm.sGetDecimal(reader["pro_unit"].ToString());
        //            data.mo_status = comm.sGetString(reader["mo_status"].ToString());
        //            data.err_memo = comm.sGetString(reader["err_memo"].ToString());
        //            data.mo_start_date = comm.sGetString(reader["mo_start_date"].ToString());
        //            data.mo_end_date = comm.sGetString(reader["mo_end_date"].ToString());
        //            data.mo_out_date = comm.sGetString(reader["mo_out_date"].ToString());
        //            data.mo_qty = comm.sGetDecimal(reader["mo_qty"].ToString());
        //            data.sch_date_s = comm.sGetString(reader["sch_date_s"].ToString());
        //            data.sch_date_e = comm.sGetString(reader["sch_date_e"].ToString());
        //            data.sch_time_s = comm.sGetString(reader["sch_time_s"].ToString());
        //            data.sch_time_e = comm.sGetString(reader["sch_time_e"].ToString());
        //            data.ins_date = comm.sGetString(reader["ins_date"].ToString());
        //            data.ins_time = comm.sGetString(reader["ins_time"].ToString());
        //            data.usr_code = comm.sGetString(reader["usr_code"].ToString());
        //            data.usr_code = comm.sGetString(reader["last_date"].ToString());
        //            data.usr_code = comm.sGetString(reader["last_time"].ToString());

        //            data.can_delete = "Y";
        //            data.can_update = "Y";
        //            list.Add(data);
        //        }

        //    }
        //    return list;
        //}

        /// <summary>
        /// 取得使用者可以編輯的資料，結合商務邏輯權限
        /// </summary>
        /// <param name="pUsrCode"></param>
        /// <param name="pPrgCode"></param>
        /// <returns></returns>
        //public List<MET01_0000> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_mo_code", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<MET01_0000> list = new List<MET01_0000>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料
        //    sSql = " SELECT * FROM MET01_0000 ";

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        //sqlCommand.Parameters.Add(new SqlParameter("@mo_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            MET01_0000 data = new MET01_0000();

        //            data.mo_code = comm.sGetString(reader["mo_code"].ToString());
        //            data.pro_code = comm.sGetString(reader["pro_code"].ToString());
        //            data.bom_code = comm.sGetString(reader["bom_code"].ToString());
        //            data.sor_code = comm.sGetString(reader["sor_code"].ToString());
        //            data.ord_code = comm.sGetString(reader["ord_code"].ToString());
        //            data.cus_code = comm.sGetString(reader["cus_code"].ToString());
        //            data.plan_start_date = comm.sGetString(reader["plan_start_date"].ToString());
        //            data.plan_end_date = comm.sGetString(reader["plan_end_date"].ToString());
        //            data.plan_out_date = comm.sGetString(reader["plan_out_date"].ToString());
        //            data.plan_line_code = comm.sGetString(reader["plan_line_code"].ToString());
        //            data.plan_qty = comm.sGetDecimal(reader["plan_qty"].ToString());
        //            data.plan_qty = comm.sGetDecimal(reader["pro_unit"].ToString());
        //            data.mo_status = comm.sGetString(reader["mo_status"].ToString());
        //            data.err_memo = comm.sGetString(reader["err_memo"].ToString());
        //            data.mo_start_date = comm.sGetString(reader["mo_start_date"].ToString());
        //            data.mo_end_date = comm.sGetString(reader["mo_end_date"].ToString());
        //            data.mo_out_date = comm.sGetString(reader["mo_out_date"].ToString());
        //            data.mo_qty = comm.sGetDecimal(reader["mo_qty"].ToString());
        //            data.sch_date_s = comm.sGetString(reader["sch_date_s"].ToString());
        //            data.sch_date_e = comm.sGetString(reader["sch_date_e"].ToString());
        //            data.sch_time_s = comm.sGetString(reader["sch_time_s"].ToString());
        //            data.sch_time_e = comm.sGetString(reader["sch_time_e"].ToString());
        //            data.ins_date = comm.sGetString(reader["ins_date"].ToString());
        //            data.ins_time = comm.sGetString(reader["ins_time"].ToString());
        //            data.usr_code = comm.sGetString(reader["usr_code"].ToString());
        //            data.usr_code = comm.sGetString(reader["last_date"].ToString());
        //            data.usr_code = comm.sGetString(reader["last_time"].ToString());


        //            //檢查授權刪除、修改
        //            data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
        //            data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

        //            //資料邏輯刪除、修改
        //            //if (arr_LockGrpCode.Contains(data.mo_code)) {
        //            //    data.can_delete = "N";
        //            //    data.can_update = "N";
        //            //}

        //            list.Add(data);
        //        }
        //    }
        //    return list;
        //}
        #endregion

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

            string sSql = " SELECT distinct MET01_0000.mo_code, MET01_0000.*, A.pro_name, MEB23_0000.bom_name, MEB12_0000.line_name as plan_line_name,"+
                          " MEB25_0000.cus_name, BDP21_0100.field_name as mo_status_name, MEB23_0000.pro_qty, MEB23_0000.unit_code as pro_unit, B.field_name as mo_type_name " +
                          " FROM MET01_0000 " +
                          " left join MET01_0100 on MET01_0100.mo_code = MET01_0000.mo_code " +
                          " left join MEB20_0000 on MEB20_0000.pro_code = MET01_0100.pro_code " +
                          " left join MEB30_0000 on MEB30_0000.work_code = MET01_0100.work_code " +
                          " left join MEB20_0000 as A on A.pro_code = MET01_0000.pro_code " +
                          " left join MEB23_0000 on MEB23_0000.bom_code = MET01_0000.bom_code " +
                          " left join MEB12_0000 on MEB12_0000.line_code = MET01_0000.plan_line_code " +
                          " left join MEB25_0000 on MEB25_0000.cus_code = MET01_0000.cus_code " +
                          " left join BDP21_0100 on BDP21_0100.field_code = MET01_0000.mo_status and BDP21_0100.code_code = 'mo_status' " +
                          " left join BDP21_0100 as B on B.field_code = MET01_0000.mo_type and B.code_code = 'mo_type' ";

            // 取得資料
            list = comm.Get_ListByQuery<MET01_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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

        public List<MET01_0000> Get_DataListByQuery_ForMET03(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MET01_0000> list = new List<MET01_0000>();

            string sSql = " SELECT distinct MET01_0000.mo_code, MET01_0000.*, A.pro_name, MEB23_0000.bom_name, MEB12_0000.line_name as plan_line_name," +
                          " MEB25_0000.cus_name, BDP21_0100.field_name as mo_status_name, MEB23_0000.pro_qty, MEB23_0000.unit_code as pro_unit, B.field_name as mo_type_name " +
                          " FROM MET01_0000 " +
                          " left join MET01_0100 on MET01_0100.mo_code = MET01_0000.mo_code " +
                          " left join MEB20_0000 on MEB20_0000.pro_code = MET01_0100.pro_code " +
                          " left join MEB30_0000 on MEB30_0000.work_code = MET01_0100.work_code " +
                          " left join MEB20_0000 as A on A.pro_code = MET01_0000.pro_code " +
                          " left join MEB23_0000 on MEB23_0000.bom_code = MET01_0000.bom_code " +
                          " left join MEB12_0000 on MEB12_0000.line_code = MET01_0000.plan_line_code " +
                          " left join MEB25_0000 on MEB25_0000.cus_code = MET01_0000.cus_code " +
                          " left join BDP21_0100 on BDP21_0100.field_code = MET01_0000.mo_status and BDP21_0100.code_code = 'mo_status' " +
                       " left join BDP21_0100 as C on C.field_code = MET01_0100.pro_kind and C.code_code = 'pro_kind' " +
                       " left join BDP21_0100 as D on D.field_code = MET01_0100.is_throw and D.code_code = 'is_throw' " +
                          " left join BDP21_0100 as B on B.field_code = MET01_0000.mo_type and B.code_code = 'mo_type' ";
            sSql += " Where MET01_0000.mo_status='10'";

            // 取得資料
            list = comm.Get_ListByQuery<MET01_0000>(sSql, pWhere, pUsrCode, pPrgCode);

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
        /// 傳入一個MET01_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MET01_0000">DTO</param>
        public void InsertData(MET01_0000 MET01_0000)
        {
            string sSql = " INSERT INTO " +
                          " MET01_0000 (  mo_code,      pro_code,         bom_code,       sor_code,       ord_code,       " +  
                          "               cus_code,     plan_start_date,  plan_end_date,  plan_out_date,  plan_line_code, " +
                          "               plan_qty,     pro_unit,         mo_status,      err_memo,       mo_start_date,  " +
                          "               mo_end_date,  mo_out_date,      mo_qty,         sch_date_s,     sch_date_e,     " +
                          "               sch_time_s,   sch_time_e,       ins_date,       ins_time,       usr_code,       " +
                          "               last_date,    last_time,        mo_memo,        mo_type,        mo_level,       " +
                          "               up_mo_code)" +
                          "     VALUES ( @mo_code,     @pro_code,        @bom_code,      @sor_code,      @ord_code,       " +
                          "              @cus_code,    @plan_start_date, @plan_end_date, @plan_out_date, @plan_line_code, " +
                          "              @plan_qty,    @pro_unit,        @mo_status,     @err_memo,      @mo_start_date,  " +
                          "              @mo_end_date, @mo_out_date,     @mo_qty,        @sch_date_s,    @sch_date_e,     " +
                          "              @sch_time_s,  @sch_time_e,      @ins_date,      @ins_time,      @usr_code,       " +
                          "              @last_date,   @last_time,       @mo_memo,       @mo_type,       @mo_level,       " +
                          "              @up_mo_code )";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MET01_0000);
            }
        }

        /// <summary>
        /// 傳入一個MET01_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MET01_0000">DTO</param>
        public void UpdateData(MET01_0000 MET01_0000)
        {
            string sSql = " UPDATE MET01_0000                           " +
                          "    SET pro_code        =  @pro_code,        " +
                          "        bom_code        =  @bom_code,        " +
                          "        sor_code        =  @sor_code,        " +
                          "        ord_code        =  @ord_code,        " +
                          "        cus_code        =  @cus_code,        " +
                          "        plan_start_date =  @plan_start_date, " +
                          "        plan_end_date   =  @plan_end_date,   " +
                          "        plan_out_date   =  @plan_out_date,   " +
                          "        plan_line_code  =  @plan_line_code,  " +
                          "        plan_qty        =  @plan_qty,        " +
                          "        pro_unit        =  @pro_unit,        " +
                          "        mo_status       =  @mo_status,       " +
                          "        err_memo        =  @err_memo,        " +
                          "        mo_start_date   =  @mo_start_date,   " +
                          "        mo_end_date     =  @mo_end_date,     " +
                          "        mo_out_date     =  @mo_out_date,     " +
                          "        mo_qty          =  @mo_qty,          " +
                          "        sch_date_s      =  @sch_date_s,      " +
                          "        sch_date_e      =  @sch_date_e,      " +
                          "        sch_time_s      =  @sch_time_s,      " +
                          "        sch_time_e      =  @sch_time_e,      " +
                          "        ins_date        =  @ins_date,        " +
                          "        ins_time        =  @ins_time,        " +
                          "        usr_code        =  @usr_code,        " +
                          "        last_date       =  @last_date,       " +
                          "        last_time       =  @last_time,       " +
                          "        mo_memo         =  @mo_memo,         " +
                          "        mo_type         =  @mo_type          " +
                          "  WHERE mo_code         =  @mo_code          " ;

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MET01_0000);
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = " DELETE FROM MET01_0000 WHERE mo_code = @mo_code " +
                          " DELETE FROM MET01_0100 WHERE mo_code = @mo_code ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { mo_code = pTkCode });
            }
        }
        /// <summary>
        /// 更新工單狀態，傳入工單號與愈更改的工單狀態
        /// </summary>
        /// <param name="pMoCode"></param>
        /// <param name="pMoStatus"></param>
        public void Upd_MoStatus(string pMoCode,string pMoStatus)
        {
            string sSql = "Update MET01_0000 set mo_status=@mo_status where mo_code=@mo_code;";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { mo_code = pMoCode,mo_status=pMoStatus });
            }
        }
        /// <summary>
        /// 刪除WMT07_0000相關資料
        /// </summary>
        /// <param name="pMoCode"></param>
        public  void Del_WMT07_0000_Data(string pMoCode)
        {
            string sSql = "DELETE WMT07_0000 where mo_code=@mo_code";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { mo_code = pMoCode });
            }
        }
        /// <summary>
        /// 更新MEM01_0000結案時間
        /// </summary>
        /// <param name="pMoCode"></param>
        public void Upd_MEM01_0000_Data(string pMoCode)
        {
            string sWorkTimeE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sSql = "UPDATE MEM01_0000 set work_time_e=@work_time_e where mo_code=@mo_code";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { mo_code = pMoCode, work_time_e = sWorkTimeE });
            }
        }
        /// <summary>
        /// 刪除MEM01_0000相關資料
        /// </summary>
        /// <param name="pMoCode"></param>
        public void Del_MEM01_0000_Data(string pMoCode)
        {
            string sSql = "DELETE MEM01_0000 where mo_code=@mo_code";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { mo_code = pMoCode });
            }
        }
        /// <summary>
        /// 新增MED02_0000每個工站的強制結案資料
        /// </summary>
        /// <param name="pMoCode"></param>
        public void Ins_MED02_0000(string pMoCode)
        {
            string sSql = "Select * from MED02_0000 where mo_code='" + pMoCode + "'";
            DataTable dtTmp = comm.Get_DataTable(sSql);
            foreach (DataRow row in dtTmp.Rows)
            {
                MED02_0000 MED02_0000 = new MED02_0000();
                MED02_0000.mo_code = pMoCode;
                MED02_0000.wrk_code = row["wrk_code"].ToString();
                MED02_0000.mac_code = row["mac_code"].ToString();
                MED02_0000.mo_status_wrk = "END";
                MED02_0000.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
                MED02_0000.ins_time = DateTime.Now.ToString("HH:mm:ss");
                MED02_0000.usr_code = "";
                MED02_0000.des_memo = "";
                MED02_0000.is_ng = "N";
                MED02_0000.is_end = "Y";
                MED02_0000.end_memo = "";
                MED02_0000.des_memo = "";
                MED02_0000.end_date = "";
                MED02_0000.end_time = "";
                MED02_0000.end_usr_code = "";

                MED02_0000Repository repoMED02_0000 = new MED02_0000Repository();
                repoMED02_0000.InsertEndData(MED02_0000);
            }
        }
        public void Upd_MET03_MoStatus(string pMoCode,string pMoStatus)
        {
            string sWorkTimeE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sSql = "UPDATE MET03_0000 set mo_status=@mo_status where mo_code=@mo_code";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { mo_code = pMoCode, mo_status = pMoStatus });
            }
        }
        /// <summary>
        /// 更新取消工單的排成時間
        /// </summary>
        /// <param name="pMoCode"></param>
        public void Upd_MET01_SchData(string pMoCode)
        {
            string sSql= "UPDATE MET01_0000 set sch_date_s=@sch_date_s, "+
                                          "  sch_time_s=@sch_time_s, " +
                                          "  sch_date_e=@sch_date_e, "+
                                          "  sch_time_e=@sch_time_e " +
                                          " where mo_code=@mo_code";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { mo_code = pMoCode, sch_date_s = "", sch_time_s ="", sch_date_e ="", sch_time_e =""});
            }

        }
        ////暫存DataTable參考
        //// <summary>
        //// 取得MET01_0000角色的DataTable
        //// </summary>
        //// <param name = "pTkCode" > 有傳鍵值取一筆，鍵值空白取全部</param>
        //// <returns></returns>
        //public DataTable GetMET01_0000_dt(string pTkCode)
        //{
        //    DataTable dtTmp = new DataTable();
        //    DataTable dtDat = new DataTable();
        //    dtDat.Columns.Add("mo_code", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("mo_code", System.Type.GetType("System.String"].ToString());
        //    dtDat.Columns.Add("pro_code", System.Type.GetType("System.String"].ToString());

        //    string sSql = "";
        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM MET01_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM MET01_0000 where mo_code='" + pTkCode + "'";
        //    }
        //    dtTmp = comm.Get_DataTable(sSql);

        //    int i;
        //    for (i = 1; i < dtTmp.Rows.Count - 1; i++)
        //    {
        //        DataRow drow = dtDat.NewRow();
        //        drow["mo_code"] = dtTmp.Rows[i]["mo_code"];
        //        drow["mo_code"] = dtTmp.Rows[i]["mo_code"];
        //        drow["pro_code"] = dtTmp.Rows[i]["pro_code"];
        //        dtDat.Rows.Add(drow);
        //    }
        //    return dtDat;
        //}
    }
}