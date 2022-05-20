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
    public class MET02_0000Repository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得MET02_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO MET02_0000</returns>
        //public MET02_0000 GetDTO(string pTkCode)
        //{
        //    MET02_0000 datas = new MET02_0000();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM MET02_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM MET02_0000 where mo_code=@mo_code";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@mo_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                datas = new MET02_0000
        //                {
        //                    seq_no = comm.sGetInt32(reader["seq_no"].ToString()),
        //                    plan_out_date = comm.sGetString(reader["plan_out_date"].ToString()),
        //                    mo_code = comm.sGetString(reader["mo_code"].ToString()),
        //                    sor_code = comm.sGetString(reader["sor_code"].ToString()),
        //                    cus_name = comm.sGetString(reader["cus_name"].ToString()),
        //                    pro_code = comm.sGetString(reader["pro_code"].ToString()),
        //                    pro_name = comm.sGetString(reader["pro_name"].ToString()),
        //                    pro_spec = comm.sGetString(reader["pro_spec"].ToString()),
        //                    spec_a = comm.sGetString(reader["spec_a"].ToString()),
        //                    spec_b = comm.sGetString(reader["spec_b"].ToString()),
        //                    spec_c = comm.sGetString(reader["spec_c"].ToString()),
        //                    plan_qty = comm.sGetDecimal(reader["plan_qty"].ToString()),
        //                    mo_qty = comm.sGetDecimal(reader["mo_qty"].ToString()),
        //                    mo_status = comm.sGetString(reader["mo_status"].ToString()),
        //                };
        //            }
        //        }
        //    }
        //    return datas;
        //}

        #region
        /// <summary>
        /// 取得MET02_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List MET02_0000</returns>
        //public List<MET02_0000> Get_DataList(string pTkCode)
        //{
        //    List<MET02_0000> list = new List<MET02_0000>();
        //    string sSql = "";

        //    if (string.IsNullOrEmpty(pTkCode))
        //    {
        //        sSql = "SELECT * FROM MET02_0000";
        //    }
        //    else
        //    {
        //        sSql = "SELECT * FROM MET02_0000 where mo_code=@mo_code";
        //    }

        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        sqlCommand.Parameters.Add(new SqlParameter("@mo_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            MET02_0000 data = new MET02_0000();

        //            data.seq_no = comm.sGetInt32(reader["seq_no"].ToString());
        //            data.plan_out_date = comm.sGetString(reader["plan_out_date"].ToString());
        //            data.mo_code = comm.sGetString(reader["mo_code"].ToString());
        //            data.sor_code = comm.sGetString(reader["sor_code"].ToString());
        //            data.cus_name = comm.sGetString(reader["cus_name"].ToString());
        //            data.pro_code = comm.sGetString(reader["pro_code"].ToString());
        //            data.pro_name = comm.sGetString(reader["pro_name"].ToString());
        //            data.pro_spec = comm.sGetString(reader["pro_spec"].ToString());
        //            data.spec_a = comm.sGetString(reader["spec_a"].ToString());
        //            data.spec_b = comm.sGetString(reader["spec_b"].ToString());
        //            data.spec_c = comm.sGetString(reader["spec_c"].ToString());
        //            data.plan_qty = comm.sGetDecimal(reader["plan_qty"].ToString());
        //            data.mo_qty = comm.sGetDecimal(reader["mo_qty"].ToString());
        //            data.mo_status = comm.sGetString(reader["mo_status"].ToString());


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
        //public List<MET02_0000> Get_DataList(string pUsrCode, string pPrgCode)
        //{
        //    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
        //    string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_mo_code", "par_name", "par_value");
        //    var arr_LockGrpCode = sLockGrpCode.Split(',');

        //    List<MET02_0000> list = new List<MET02_0000>();
        //    string sSql = "";

        //    //取得該使用者可以看的資料

        //    sSql = "SELECT * FROM MET02_0000";


        //    using (SqlConnection con_db = comm.Set_DBConnection())
        //    {
        //        SqlCommand sqlCommand = new SqlCommand(sSql);
        //        sqlCommand.Connection = con_db;
        //        //sqlCommand.Parameters.Add(new SqlParameter("@mo_code", pTkCode));
        //        SqlDataReader reader = sqlCommand.ExecuteReader();

        //        while (reader.Read()) 
        //        {
        //            MET02_0000 data = new MET02_0000();

        //            data.seq_no = comm.sGetInt32(reader["seq_no"].ToString());
        //            data.plan_out_date = comm.sGetString(reader["plan_out_date"].ToString());
        //            data.mo_code = comm.sGetString(reader["mo_code"].ToString());
        //            data.sor_code = comm.sGetString(reader["sor_code"].ToString());
        //            data.cus_name = comm.sGetString(reader["cus_name"].ToString());
        //            data.pro_code = comm.sGetString(reader["pro_code"].ToString());
        //            data.pro_name = comm.sGetString(reader["pro_name"].ToString());
        //            data.pro_spec = comm.sGetString(reader["pro_spec"].ToString());
        //            data.spec_a = comm.sGetString(reader["spec_a"].ToString());
        //            data.spec_b = comm.sGetString(reader["spec_b"].ToString());
        //            data.spec_c = comm.sGetString(reader["spec_c"].ToString());
        //            data.plan_qty = comm.sGetDecimal(reader["plan_qty"].ToString());
        //            data.mo_qty = comm.sGetDecimal(reader["mo_qty"].ToString());
        //            data.mo_status = comm.sGetString(reader["mo_status"].ToString());

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
        public List<MET02_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<MET02_0000> list = new List<MET02_0000>();

            //string sSql = "SELECT seq_no,plan_out_date,mo_code,sor_code,cus_name, "+
            //              "pro_code,pro_name,pro_spec,spec_a,spec_b,spec_c,plan_qty,mo_qty,mo_status,seq_type FROM MET02_0000";
            string sSql = "SELECT * from  MET02_0000 WITH ( NOLOCK )";

            // 取得資料
            list = comm.Get_ListByQuery<MET02_0000>(sSql, pWhere, pUsrCode, pPrgCode);

            // 權限設定
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);

            for (int i = 0; i < list.Count; i++)
            {
                //檢查授權刪除、修改
                list[i].can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                list[i].can_update = sLimitStr.Contains("M") ? "Y" : "N";
                //生產狀態變中文
                switch (list[i].MO_STATUS)
                {
                    case "W":
                        list[i].MO_STATUS = "待排程";
                        break;
                    case "0":
                        list[i].MO_STATUS = "待生產";
                        break;
                    case "1":
                        list[i].MO_STATUS = "生產中";
                        break;
                    case "2":
                        list[i].MO_STATUS = "結案";
                        break;
                    case "3":
                        list[i].MO_STATUS = "強制結案";
                        break;
                }
            }

            return list;

        }

        /// <summary>
        /// 傳入一個MET02_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="MET02_0000">DTO</param>
        public void InsertData(MET02_0000 MET02_0000)
        {
            string sSql = "INSERT INTO " +
                          " MET02_0000 (  mo_code,  seq_no,  plan_out_date,  sor_code,  cus_name,  pro_code ,  pro_name,  pro_spec,  spec_a,  spec_b ,  spec_c,  plan_qty,  mo_qty,  mo_status )" +
                          "     VALUES (  @mo_code,  @seq_no,  @plan_out_date,  @sor_code,  @cus_name,  @pro_code ,  @pro_name,  @pro_spec,  @spec_a,  @spec_b ,  @spec_c,  @plan_qty,  @mo_qty,  @mo_status )";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MET02_0000);
            }
        }

        public void InsertHistory()
        {
            string nowtime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            DataTable dt = comm.Get_DataTable("select * from MET02_0000 WITH ( NOLOCK )");
            if (dt.Rows.Count>0) {
                foreach (DataRow dr in dt.Rows)
                {
                    string plan_start_date = comm.sGetDateTime(dr["PLAN_START_DATE"].ToString()).ToString("yyyy/MM/dd");
                    string ins_date = comm.sGetDateTime(dr["ins_date"].ToString()).ToString("yyyy/MM/dd");

                    string sql = "INSERT INTO MET02_0000_H (mo_code, seq_no, plan_out_date, sor_code, cus_name, " +
                         "pro_code, pro_name, pro_spec, plan_qty, spec_1, spec_2, spec_3, spec_4, spec_5, spec_6, spec_7, spec_8, " +
                         "PLANT_ID, PLANT_CODE, PLANT_NAME, WORK_CENTER_ID, WORK_CENTER_CODE, WORK_CENTER_NAME, " +
                         "OPERATION_ID, OPERATION_CODE, OPERATION_NAME, MO_ID, MACHINE_ID, MACHINE_CODE, MACHINE_DESCRIPTION,ins_date, PLAN_START_DATE, MO_STATUS, QTY, SCRAP_QTY, syn_time) " +
                         "values (@DOC_NO, @SEQUENCE, @PLAN_DELIVERY_DATE, @DEMAND_DOC, @CUSTOMER_NAME, " +
                         " @ITEM_CODE, @ITEM_NAME, @ITEM_SPECIFICATION, @PLAN_PRODUCTION_QTY, @spec_1, @spec_2, @spec_3,@spec_4, @spec_5, @spec_6,@spec_7, @spec_8,  " +
                         " @PLANT_ID, @PLANT_CODE, @PLANT_NAME, @WORK_CENTER_ID, @WORK_CENTER_CODE, @WORK_CENTER_NAME, " +
                         " @OPERATION_ID, @OPERATION_CODE, @WORK_CENTER_NAME, @MO_ID, @MACHINE_ID, @MACHINE_CODE, @MACHINE_DESCRIPTION, @ins_date, @PLAN_START_DATE, @MO_STATUS, @QTY, @SCRAP_QTY, @syn_time) ";

                    using (SqlConnection con_db = comm.Set_DBConnection())
                    {
                        SqlCommand sqlCommand = new SqlCommand(sql);
                        sqlCommand.Connection = con_db;
                        sqlCommand.Parameters.Add(new SqlParameter("@DOC_NO", dr["mo_code"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@SEQUENCE", dr["seq_no"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@PLAN_DELIVERY_DATE", dr["plan_out_date"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@DEMAND_DOC", dr["sor_code"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@CUSTOMER_NAME", dr["cus_name"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@ITEM_CODE", dr["pro_code"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@ITEM_NAME", dr["pro_name"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@ITEM_SPECIFICATION", dr["pro_spec"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@PLAN_PRODUCTION_QTY", dr["plan_qty"].ToString()));
                        //sqlCommand.Parameters.Add(new SqlParameter("@spec_a", sdt["spec_a"].ToString()));
                        //sqlCommand.Parameters.Add(new SqlParameter("@spec_d", sdt["spec_d"].ToString()));
                        //sqlCommand.Parameters.Add(new SqlParameter("@spec_f", sdt["spec_f"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@spec_1", dr["spec_1"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@spec_2", dr["spec_2"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@spec_3", dr["spec_3"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@spec_4", dr["spec_4"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@spec_5", dr["spec_5"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@spec_6", dr["spec_6"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@spec_7", dr["spec_7"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@spec_8", dr["spec_8"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@PLANT_ID", dr["PLANT_ID"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@PLANT_CODE", dr["PLANT_CODE"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@PLANT_NAME", dr["PLANT_NAME"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@WORK_CENTER_ID", dr["WORK_CENTER_ID"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@WORK_CENTER_CODE", dr["WORK_CENTER_CODE"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@WORK_CENTER_NAME", dr["WORK_CENTER_NAME"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@OPERATION_ID", dr["OPERATION_ID"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@OPERATION_CODE", dr["OPERATION_CODE"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@OPERATION_NAME", dr["OPERATION_NAME"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@MO_ID", dr["MO_ID"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@MACHINE_ID", dr["MACHINE_ID"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@MACHINE_CODE", dr["MACHINE_CODE"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@MACHINE_DESCRIPTION", dr["MACHINE_DESCRIPTION"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@ins_date", ins_date));
                        sqlCommand.Parameters.Add(new SqlParameter("@PLAN_START_DATE", plan_start_date));
                        sqlCommand.Parameters.Add(new SqlParameter("@MO_STATUS", dr["MO_STATUS"].ToString()));
                        //sqlCommand.Parameters.Add(new SqlParameter("@MO_STATUS2", MO_STATUS2));
                        sqlCommand.Parameters.Add(new SqlParameter("@QTY", dr["QTY"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@SCRAP_QTY", dr["SCRAP_QTY"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@syn_time", nowtime));
                        sqlCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        //ERP同步
        public void Synchronize2()
        {
            //刪除狀態為W、0的工單
            DeleteData5();

            //狀態2、3的工單順序先變為0
            string sql3 = "UPDATE MET02_0000 WITH ( ROWLOCK ) set seq_no = '0' where MO_STATUS in ('2','3')";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sql3);
            }

            string sql = "SELECT a.DOC_NO,convert(int, a.SEQUENCE)  as SEQUENCE ,convert(nvarchar(10), a.PLAN_DELIVERY_DATE, 126) as PLAN_DELIVERY_DATE, " +
                          "a.PLAN_START_DATE,a.DEMAND_DOC,a.CUSTOMER_NAME,a.ITEM_CODE,a.ITEM_NAME,a.ITEM_SPECIFICATION,a.PLAN_PRODUCTION_QTY, " +
                          "ISNULL(b.spec_1, '') as spec_1 ,ISNULL(b.spec_2, '') as spec_2 ,ISNULL(b.spec_3, '') as spec_3 , " +
                          "ISNULL(b.spec_4, '') as spec_4 ,ISNULL(b.spec_5, '') as spec_5 ,ISNULL(b.spec_6, '') as spec_6 , " +
                          "ISNULL(b.spec_7, '') as spec_7 ,ISNULL(b.spec_8, '') as spec_8 , " +
                          "a.PLANT_ID, a.PLANT_CODE, a.PLANT_NAME, a.WORK_CENTER_ID, a.WORK_CENTER_CODE, a.WORK_CENTER_NAME, " +
                          "a.OPERATION_ID, a.OPERATION_CODE, a.OPERATION_NAME, a.MO_ID, a.MACHINE_ID, a.MACHINE_CODE, a.MACHINE_DESCRIPTION " +
                          "FROM MET05_0000 a left join MEB50_0000 b on a.ITEM_CODE = b.ITEM_CODE order by SEQUENCE";
            DataTable dt = comm.Get_DataTable(sql);
            foreach (DataRow sdt in dt.Rows)
            {
                string mocode = sdt["DOC_NO"].ToString();
                int sequence = Convert.ToInt32(sdt["SEQUENCE"].ToString());
                string nowdate = DateTime.Now.ToString("yyyy/MM/dd");

                //若工單號存在，則不新增
                DataTable dt2 = comm.Get_DataTable("select * from MET02_0000 WITH ( NOLOCK ) where mo_code = '" + mocode + "'");
                if (dt2.Rows.Count == 0) {
                    InsertData2(sdt,sequence);
                }
                else
                {
                    if (dt2.Rows[0]["MO_STATUS"].ToString()=="1")
                    {
                        //狀態1的工單順序變為MET05中的順序
                        string sql2 = "UPDATE MET02_0000 WITH ( ROWLOCK ) set seq_no = " + sequence + ",ins_date='"+ nowdate + "' where MO_STATUS = '1' and mo_code = '" + mocode + "'";
                        using (SqlConnection con_db = comm.Set_DBConnection())
                        {
                            con_db.Execute(sql2);
                        }
                    }
                    else
                    {
                        //狀態2、3的工單若存在於MET05，順序變為MET05中的順序
                        string sql4 = "UPDATE MET02_0000 WITH ( ROWLOCK ) set seq_no = " + sequence + ",ins_date='" + nowdate + "' where seq_no='0' and mo_code='" + mocode + "'";
                        using (SqlConnection con_db = comm.Set_DBConnection())
                        {
                            con_db.Execute(sql4);
                        }
                    }
                    //更新狀態1、2、3的工單的產品規格
                    string sql5 = "UPDATE MET02_0000 WITH ( ROWLOCK ) set spec_1=@spec_1,spec_2=@spec_2,spec_3=@spec_3,spec_4=@spec_4, " +
                                  "spec_5 = @spec_5,spec_6 = @spec_6,spec_7 = @spec_7,spec_8 = @spec_8 where mo_code = '" + mocode + "'";
                    using (SqlConnection con_db = comm.Set_DBConnection())
                    {
                        SqlCommand sqlCommand = new SqlCommand(sql5);
                        sqlCommand.Connection = con_db;
                        sqlCommand.Parameters.Add(new SqlParameter("@spec_1", sdt["spec_1"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@spec_2", sdt["spec_2"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@spec_3", sdt["spec_3"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@spec_4", sdt["spec_4"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@spec_5", sdt["spec_5"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@spec_6", sdt["spec_6"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@spec_7", sdt["spec_7"].ToString()));
                        sqlCommand.Parameters.Add(new SqlParameter("@spec_8", sdt["spec_8"].ToString()));
                        sqlCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        public void InsertData2(DataRow sdt,int sequence)
        {
            string sSql = "INSERT INTO MET02_0000 (mo_code, seq_no, plan_out_date, sor_code, cus_name, " +
                         "pro_code, pro_name, pro_spec, plan_qty, spec_1, spec_2, spec_3, spec_4, spec_5, spec_6, spec_7, spec_8, " +
                         "PLANT_ID, PLANT_CODE, PLANT_NAME, WORK_CENTER_ID, WORK_CENTER_CODE, WORK_CENTER_NAME, " +
                         "OPERATION_ID, OPERATION_CODE, OPERATION_NAME, MO_ID, MACHINE_ID, MACHINE_CODE, MACHINE_DESCRIPTION,ins_date, PLAN_START_DATE, MO_STATUS, QTY, SCRAP_QTY) " +
                         "values (@DOC_NO, @SEQUENCE, @PLAN_DELIVERY_DATE, @DEMAND_DOC, @CUSTOMER_NAME, " +
                         " @ITEM_CODE, @ITEM_NAME, @ITEM_SPECIFICATION, @PLAN_PRODUCTION_QTY, @spec_1, @spec_2, @spec_3,@spec_4, @spec_5, @spec_6,@spec_7, @spec_8,  " +
                         " @PLANT_ID, @PLANT_CODE, @PLANT_NAME, @WORK_CENTER_ID, @WORK_CENTER_CODE, @WORK_CENTER_NAME, " +
                         " @OPERATION_ID, @OPERATION_CODE, @WORK_CENTER_NAME, @MO_ID, @MACHINE_ID, @MACHINE_CODE, @MACHINE_DESCRIPTION, @ins_date, @PLAN_START_DATE, @MO_STATUS, @QTY, @SCRAP_QTY) ";
            
            string plan_start_date = comm.sGetDateTime(sdt["PLAN_START_DATE"].ToString()).ToString("yyyy/MM/dd");
            string mDOC_NO = sdt["DOC_NO"].ToString();
            string MO_STATUS = "W";
            decimal qty = 0;
            decimal scrapqty = 0;
            
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@DOC_NO", mDOC_NO));
                sqlCommand.Parameters.Add(new SqlParameter("@SEQUENCE", sequence));
                sqlCommand.Parameters.Add(new SqlParameter("@PLAN_DELIVERY_DATE", sdt["PLAN_DELIVERY_DATE"].ToString()));
                sqlCommand.Parameters.Add(new SqlParameter("@DEMAND_DOC", sdt["DEMAND_DOC"].ToString()));
                sqlCommand.Parameters.Add(new SqlParameter("@CUSTOMER_NAME", sdt["CUSTOMER_NAME"].ToString()));
                sqlCommand.Parameters.Add(new SqlParameter("@ITEM_CODE", sdt["ITEM_CODE"].ToString()));
                sqlCommand.Parameters.Add(new SqlParameter("@ITEM_NAME", sdt["ITEM_NAME"].ToString()));
                sqlCommand.Parameters.Add(new SqlParameter("@ITEM_SPECIFICATION", sdt["ITEM_SPECIFICATION"].ToString()));
                sqlCommand.Parameters.Add(new SqlParameter("@PLAN_PRODUCTION_QTY", sdt["PLAN_PRODUCTION_QTY"].ToString()));
                //sqlCommand.Parameters.Add(new SqlParameter("@spec_a", sdt["spec_a"].ToString()));
                //sqlCommand.Parameters.Add(new SqlParameter("@spec_d", sdt["spec_d"].ToString()));
                //sqlCommand.Parameters.Add(new SqlParameter("@spec_f", sdt["spec_f"].ToString()));
                sqlCommand.Parameters.Add(new SqlParameter("@spec_1", sdt["spec_1"].ToString()));
                sqlCommand.Parameters.Add(new SqlParameter("@spec_2", sdt["spec_2"].ToString()));
                sqlCommand.Parameters.Add(new SqlParameter("@spec_3", sdt["spec_3"].ToString()));
                sqlCommand.Parameters.Add(new SqlParameter("@spec_4", sdt["spec_4"].ToString()));
                sqlCommand.Parameters.Add(new SqlParameter("@spec_5", sdt["spec_5"].ToString()));
                sqlCommand.Parameters.Add(new SqlParameter("@spec_6", sdt["spec_6"].ToString()));
                sqlCommand.Parameters.Add(new SqlParameter("@spec_7", sdt["spec_7"].ToString()));
                sqlCommand.Parameters.Add(new SqlParameter("@spec_8", sdt["spec_8"].ToString()));
                sqlCommand.Parameters.Add(new SqlParameter("@PLANT_ID", sdt["PLANT_ID"].ToString()));
                sqlCommand.Parameters.Add(new SqlParameter("@PLANT_CODE", sdt["PLANT_CODE"].ToString()));
                sqlCommand.Parameters.Add(new SqlParameter("@PLANT_NAME", sdt["PLANT_NAME"].ToString()));
                sqlCommand.Parameters.Add(new SqlParameter("@WORK_CENTER_ID", sdt["WORK_CENTER_ID"].ToString()));
                sqlCommand.Parameters.Add(new SqlParameter("@WORK_CENTER_CODE", sdt["WORK_CENTER_CODE"].ToString()));
                sqlCommand.Parameters.Add(new SqlParameter("@WORK_CENTER_NAME", sdt["WORK_CENTER_NAME"].ToString()));
                sqlCommand.Parameters.Add(new SqlParameter("@OPERATION_ID", sdt["OPERATION_ID"].ToString()));
                sqlCommand.Parameters.Add(new SqlParameter("@OPERATION_CODE", sdt["OPERATION_CODE"].ToString()));
                sqlCommand.Parameters.Add(new SqlParameter("@OPERATION_NAME", sdt["OPERATION_NAME"].ToString()));
                sqlCommand.Parameters.Add(new SqlParameter("@MO_ID", sdt["MO_ID"].ToString()));
                sqlCommand.Parameters.Add(new SqlParameter("@MACHINE_ID", sdt["MACHINE_ID"].ToString()));
                sqlCommand.Parameters.Add(new SqlParameter("@MACHINE_CODE", sdt["MACHINE_CODE"].ToString()));
                sqlCommand.Parameters.Add(new SqlParameter("@MACHINE_DESCRIPTION", sdt["MACHINE_DESCRIPTION"].ToString()));
                sqlCommand.Parameters.Add(new SqlParameter("@ins_date", DateTime.Now.ToString("yyyy/MM/dd")));
                sqlCommand.Parameters.Add(new SqlParameter("@PLAN_START_DATE", plan_start_date));
                sqlCommand.Parameters.Add(new SqlParameter("@MO_STATUS", MO_STATUS));
                //sqlCommand.Parameters.Add(new SqlParameter("@MO_STATUS2", MO_STATUS2));
                sqlCommand.Parameters.Add(new SqlParameter("@QTY", qty));
                sqlCommand.Parameters.Add(new SqlParameter("@SCRAP_QTY", scrapqty));
                sqlCommand.ExecuteNonQuery();
            }
        }


        public void InsertData3(DataRow sdt)
        {
         
            decimal qty = comm.sGetDecimal(sdt["QTY"].ToString());
            decimal qty2 = comm.sGetDecimal(sdt["QTY2"].ToString());
                //生產差量(MBA_E00的生產量 - MET02_0000的生產量)
                decimal mqty = qty2 - qty;

            //decimal scrapqty = comm.sGetDecimal(dt.Rows[0]["SCRAP_QTY"].ToString());
            decimal scrapqty = comm.sGetDecimal(sdt["SCRAP_QTY"].ToString());
            decimal scrapqty2 = comm.sGetDecimal(sdt["SCRAP_QTY2"].ToString());
                //不良差量(MBA_E00的不良量 - MET02_0000的不良量)
                decimal mscrapqty = scrapqty2 - scrapqty;

            //DateTime TRANSACTION_DATE = comm.sGetDateTime(dt.Rows[0]["TRANSACTION_DATE"].ToString());

            //DateTime TRANSACTION_DATE = sdt["TRANSACTION_DATE2"].ToString() != "" ? comm.sGetDateTime(sdt["TRANSACTION_DATE2"].ToString()) : comm.sGetDateTime("1900-01-01");
            //TRANSACTION_DATE = comm.sGetDateTime(TRANSACTION_DATE.ToString("yyyy/MM/dd"));
            DateTime TRANSACTION_DATE = comm.sGetDateTime(sdt["TRANSACTION_DATE2"].ToString());
            string TRANSACTION_DATE2 = comm.sGetDateTime(sdt["TRANSACTION_DATE2"].ToString()).ToString("yyyy/MM/dd");
            string TRANSACTION_DATE3 = Convert.ToDateTime(sdt["TRANSACTION_DATE2"]).ToString("yyyy-MM-dd HH:mm:ss.fff");
            //DateTime TRANSACTION_DATE4 = comm.sGetDateTime(comm.sGetDateTime(sdt["TRANSACTION_DATE2"].ToString()).ToString("yyyy-MM-dd HH:mm:ss.fff"));
            DateTime TRANSACTION_DATE4 = Convert.ToDateTime(sdt["TRANSACTION_DATE2"]);

            string MO_STATUS = sdt["MO_STATUS3"].ToString();
            //string MO_STATUS2= "生產中";
            //if (MO_STATUS=="2")
            //{
            //    MO_STATUS2 = "結案";
            //}
            //if (MO_STATUS == "3")
            //{
            //    MO_STATUS2 = "強制結案";
            //}

            string guid = Guid.NewGuid().ToString();
                string sSql = "INSERT INTO " +
                             " MBA_E10 (  MBA_E10_ID,  PLANT_ID,  PLANT_CODE,  PLANT_NAME,  WORK_CENTER_ID,  WORK_CENTER_CODE ,  WORK_CENTER_NAME,  OPERATION_ID,  OPERATION_CODE,  OPERATION_NAME ,  MO_ID,  MO_DOC_NO,  TRANSACTION_DATE,  QTY,  SCRAP_QTY,  XOPERATOR_ID,  XOPERATOR_CODE,  XOPERATOR_NAME,  XMACHINE_ID,  XMACHINE_CODE,  XMACHINE_NAME,  STATUS,  OP_SEQ )" +
                             "  VALUES ( " + "'" + guid + "'" + ", @PLANT_ID, @PLANT_CODE, @PLANT_NAME, @WORK_CENTER_ID, @WORK_CENTER_CODE , @WORK_CENTER_NAME, @OPERATION_ID, @OPERATION_CODE, @OPERATION_NAME , @MO_ID, @mo_code, @TRANSACTION_DATE2,  @QTY,  @SCRAP_QTY,  @XOPERATOR_ID,  @XOPERATOR_CODE,  @XOPERATOR_NAME, @MACHINE_ID, @MACHINE_CODE, @MACHINE_DESCRIPTION , @STATUS, @seq_no )";

                using (SqlConnection con_db = comm.Set_DBConnection())
                {
                    SqlCommand sqlCommand = new SqlCommand(sSql);
                    sqlCommand.Connection = con_db;
                    sqlCommand.Parameters.Add(new SqlParameter("@PLANT_ID", sdt["PLANT_ID"].ToString()));
                    sqlCommand.Parameters.Add(new SqlParameter("@PLANT_CODE", sdt["PLANT_CODE"].ToString()));
                    sqlCommand.Parameters.Add(new SqlParameter("@PLANT_NAME", sdt["PLANT_NAME"].ToString()));
                    sqlCommand.Parameters.Add(new SqlParameter("@WORK_CENTER_ID", sdt["WORK_CENTER_ID"].ToString()));
                    sqlCommand.Parameters.Add(new SqlParameter("@WORK_CENTER_CODE", sdt["WORK_CENTER_CODE"].ToString()));
                    sqlCommand.Parameters.Add(new SqlParameter("@WORK_CENTER_NAME", sdt["WORK_CENTER_NAME"].ToString()));
                    sqlCommand.Parameters.Add(new SqlParameter("@OPERATION_ID", sdt["OPERATION_ID"].ToString()));
                    sqlCommand.Parameters.Add(new SqlParameter("@OPERATION_CODE", sdt["OPERATION_CODE"].ToString()));
                    sqlCommand.Parameters.Add(new SqlParameter("@OPERATION_NAME", sdt["OPERATION_NAME"].ToString()));
                    sqlCommand.Parameters.Add(new SqlParameter("@MO_ID", sdt["MO_ID"].ToString()));
                    sqlCommand.Parameters.Add(new SqlParameter("@mo_code", sdt["mo_code"].ToString()));
                    sqlCommand.Parameters.Add(new SqlParameter("@TRANSACTION_DATE2", TRANSACTION_DATE4));
                    sqlCommand.Parameters.Add(new SqlParameter("@QTY", mqty));
                    sqlCommand.Parameters.Add(new SqlParameter("@SCRAP_QTY", mscrapqty));
                    sqlCommand.Parameters.Add(new SqlParameter("@XOPERATOR_ID", sdt["XOPERATOR_ID"].ToString()));
                    sqlCommand.Parameters.Add(new SqlParameter("@XOPERATOR_CODE", sdt["XOPERATOR_CODE"].ToString()));
                    sqlCommand.Parameters.Add(new SqlParameter("@XOPERATOR_NAME", sdt["XOPERATOR_NAME"].ToString()));
                    sqlCommand.Parameters.Add(new SqlParameter("@MACHINE_ID", sdt["MACHINE_ID"].ToString()));
                    sqlCommand.Parameters.Add(new SqlParameter("@MACHINE_CODE", sdt["MACHINE_CODE"].ToString()));
                    sqlCommand.Parameters.Add(new SqlParameter("@MACHINE_DESCRIPTION", sdt["MACHINE_DESCRIPTION"].ToString()));
                    sqlCommand.Parameters.Add(new SqlParameter("@STATUS", "N"));
                    sqlCommand.Parameters.Add(new SqlParameter("@seq_no", sdt["OP_SEQ"].ToString()));
                    sqlCommand.ExecuteNonQuery();
                }
            //更新MET02_0000 的入庫量及不良量為最後累計量
            string sql2 = "UPDATE MET02_0000  WITH ( ROWLOCK ) SET QTY = " + qty2 + ",SCRAP_QTY = " + scrapqty2 + " ,mo_status = " + "'" + MO_STATUS + "'"  + " ,TRANSACTION_DATE = " + "'" + TRANSACTION_DATE3 + "'" + " where mo_code = " + "'" + sdt["mo_code"] + "'";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sql2);
            }
            //string sql3 = "UPDATE MET02_0000 SET QTY = " + qty2 + ",SCRAP_QTY = " + scrapqty2 + " ,mo_status = " + "'" + MO_STATUS + "'" + " ,TRANSACTION_DATE = " + "'" + TRANSACTION_DATE3 + "'" + " where mo_code = " + "'*" + sdt["mo_code"] + "'";
            //using (SqlConnection con_db = comm.Set_DBConnection())
            //{
            //    con_db.Execute(sql3);
            //}
            //}
        }
        public void Synchronize()
        {

            //判斷製令號碼不重複
            string sql = @"select a.*,b.TRANSACTION_DATE as TRANSACTION_DATE2,b.QTY as QTY2,
                        b.SCRAP_QTY as SCRAP_QTY2,b.MO_STATUS as MO_STATUS3 from MET02_0000 a 
                        join (select * from MBA_E00 where TRANSACTION_DATE in (select max(TRANSACTION_DATE) from MBA_E00 group by MO_DOC_NO)) b on a.mo_code = b.MO_DOC_NO  ";
            DataTable dt = comm.Get_DataTable(sql);
            foreach (DataRow sdt in dt.Rows)
            {
                DataTable dt2 = comm.Get_DataTable("select * from MET02_0000 where mo_code = " + "'" + sdt["mo_code"] + "'");

                DateTime t1 = comm.sGetDateTime(sdt["TRANSACTION_DATE2"].ToString());
                DateTime t2 = comm.sGetDateTime(dt2.Rows[0]["TRANSACTION_DATE"].ToString());
                int result = DateTime.Compare(t1, t2);
                if (result>0) { 
                InsertData3(sdt);
                }
            }

        }
        public void InsertOriData()
        {
            string sql = " Select MO.DOC_NO, XOPERATION_SCHEDULING_D.SEQUENCE, ITEM.ITEM_CODE, ITEM.ITEM_NAME, "

                       + " ITEM.ITEM_SPECIFICATION, PLANT.PLANT_ID, PLANT.PLANT_CODE, PLANT.PLANT_NAME, WORK_CENTER.WORK_CENTER_ID, WORK_CENTER.WORK_CENTER_CODE, "
                       + " XOPERATION_SCHEDULING_D.MACHINE_ID, MACHINE.MACHINE_CODE, MACHINE.MACHINE_DESCRIPTION,XOPERATION_SCHEDULING_D.PLAN_START_DATE, "
                       + " WORK_CENTER.WORK_CENTER_NAME, XOPERATION_SCHEDULING.OPERATION_ID, OPERATION.OPERATION_CODE, "
                       + " OPERATION.OPERATION_NAME, XOPERATION_SCHEDULING_D.MO_ID, CUSTOMER.CUSTOMER_NAME, XOPERATION_SCHEDULING_D.PLAN_PRODUCTION_QTY,"
                       + " SALES_ORDER_DOC.DOC_NO + '-' + CONVERT(nvarchar, SALES_ORDER_DOC_D.SequenceNumber) + '-' + CONVERT(nvarchar, SALES_ORDER_DOC_SD.SequenceNumber) AS DEMAND_DOC, "
                       + " XOPERATION_SCHEDULING_D.PLAN_DELIVERY_DATE "

                       + " FROM CUSTOMER RIGHT OUTER JOIN"
                       + " XOPERATION_SCHEDULING INNER JOIN"
                       + " XOPERATION_SCHEDULING_D ON"
                       + " XOPERATION_SCHEDULING.XOPERATION_SCHEDULING_ID = XOPERATION_SCHEDULING_D.XOPERATION_SCHEDULING_ID"
                       + " INNER JOIN"
                       + " OPERATION ON XOPERATION_SCHEDULING.OPERATION_ID = OPERATION.OPERATION_ID INNER JOIN"
                       + " ITEM ON XOPERATION_SCHEDULING_D.ITEM_ID = ITEM.ITEM_BUSINESS_ID INNER JOIN"
                       + " ITEM AS FITEM ON XOPERATION_SCHEDULING_D.FITEM_ID = FITEM.ITEM_BUSINESS_ID LEFT OUTER JOIN"
                       + " ITEM AS USED_ITEM ON XOPERATION_SCHEDULING_D.USED_ITEM = USED_ITEM.ITEM_BUSINESS_ID INNER JOIN"
                       + " MO ON XOPERATION_SCHEDULING_D.MO_ID = MO.MO_ID INNER JOIN"
                       + " MACHINE ON XOPERATION_SCHEDULING_D.MACHINE_ID = MACHINE.MACHINE_ID INNER JOIN"
                       + " WORK_CENTER_D ON WORK_CENTER_D.SOURCE_ID_ROid = MACHINE.MACHINE_ID INNER JOIN"
                       + " WORK_CENTER ON WORK_CENTER.WORK_CENTER_ID = WORK_CENTER_D.WORK_CENTER_ID INNER JOIN"
                       + " PLANT ON XOPERATION_SCHEDULING.Owner_Org_ROid = PLANT.PLANT_ID LEFT OUTER JOIN"
                       + " SALES_ORDER_DOC_D INNER JOIN"
                       + " SALES_ORDER_DOC ON"
                       + " SALES_ORDER_DOC_D.SALES_ORDER_DOC_ID = SALES_ORDER_DOC.SALES_ORDER_DOC_ID INNER JOIN"
                       + " SALES_ORDER_DOC_SD ON"
                       + " SALES_ORDER_DOC_D.SALES_ORDER_DOC_D_ID = SALES_ORDER_DOC_SD.SALES_ORDER_DOC_D_ID ON"
                       + " XOPERATION_SCHEDULING_D.DEMAND_DOC_ROid = SALES_ORDER_DOC_SD.SALES_ORDER_DOC_SD_ID ON"
                       + " CUSTOMER.CUSTOMER_BUSINESS_ID = XOPERATION_SCHEDULING_D.CUSTOMER_ID LEFT OUTER JOIN"
                       + " OPERATION AS PRE_OPERATION ON XOPERATION_SCHEDULING_D.PRE_OP_ID = PRE_OPERATION.OPERATION_ID LEFT OUTER JOIN"
                       + " OPERATION AS NEXT_OPERATION ON XOPERATION_SCHEDULING_D.NEXT_OP_ID = NEXT_OPERATION.OPERATION_ID LEFT OUTER JOIN"
                       + " MACHINE AS NEXT_MACHINE ON XOPERATION_SCHEDULING_D.NEXT_OP_MACHINE = NEXT_MACHINE.MACHINE_ID"
                       + " WHERE MACHINE.MACHINE_CODE in ('1001-M1','1001-M2','1001-M3','1001-M4','1001-M5') and XOPERATION_SCHEDULING_D.SEQUENCE <> '9999'"
                       + " ORDER BY XOPERATION_SCHEDULING_D.SEQUENCE";

            DataTable dt = comm.Get_AlexDataTable(sql);

            foreach (DataRow dr in dt.Rows)
            {
                string DOC_NO = dr["DOC_NO"].ToString();
                decimal SEQUENCE = comm.sGetDecimal(dr["SEQUENCE"].ToString());
                string ITEM_CODE = dr["ITEM_CODE"].ToString();
                string ITEM_NAME = dr["ITEM_NAME"].ToString();
                string ITEM_SPECIFICATION = dr["ITEM_SPECIFICATION"].ToString();
                string CUSTOMER_NAME = dr["CUSTOMER_NAME"].ToString();
                decimal PLAN_PRODUCTION_QTY = comm.sGetDecimal(dr["PLAN_PRODUCTION_QTY"].ToString());
                string DEMAND_DOC = dr["DEMAND_DOC"].ToString();
                DateTime PLAN_DELIVERY_DATE = comm.sGetDateTime(dr["PLAN_DELIVERY_DATE"].ToString());
                string PLANT_ID = dr["PLANT_ID"].ToString();
                string PLANT_CODE = dr["PLANT_CODE"].ToString();
                string PLANT_NAME = dr["PLANT_NAME"].ToString();
                string OPERATION_ID = dr["OPERATION_ID"].ToString();
                string OPERATION_CODE = dr["OPERATION_CODE"].ToString();
                string OPERATION_NAME = dr["OPERATION_NAME"].ToString();
                string WORK_CENTER_ID = dr["WORK_CENTER_ID"].ToString();
                string WORK_CENTER_CODE = dr["WORK_CENTER_CODE"].ToString();
                string WORK_CENTER_NAME = dr["WORK_CENTER_NAME"].ToString();
                string MACHINE_ID = dr["MACHINE_ID"].ToString();
                string MACHINE_CODE = dr["MACHINE_CODE"].ToString();
                string MACHINE_DESCRIPTION = dr["MACHINE_DESCRIPTION"].ToString();
                string MO_ID = dr["MO_ID"].ToString();
                DateTime PLAN_START_DATE = comm.sGetDateTime(dr["PLAN_START_DATE"].ToString());
                string INS_DATETIME = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                string sSql = " INSERT INTO " +
                              " MET05_0000 (  DOC_NO,  SEQUENCE,  ITEM_CODE,  ITEM_NAME,  ITEM_SPECIFICATION,  CUSTOMER_NAME,  PLAN_PRODUCTION_QTY,  DEMAND_DOC,  PLAN_DELIVERY_DATE,  PLANT_ID,  PLANT_CODE,  PLANT_NAME,  WORK_CENTER_ID,  WORK_CENTER_CODE,  WORK_CENTER_NAME,  OPERATION_ID,  OPERATION_CODE,  OPERATION_NAME,  MO_ID, MACHINE_ID, MACHINE_CODE, MACHINE_DESCRIPTION, PLAN_START_DATE) " +
                              "     VALUES ( @DOC_NO, @SEQUENCE, @ITEM_CODE, @ITEM_NAME, @ITEM_SPECIFICATION, @CUSTOMER_NAME, @PLAN_PRODUCTION_QTY, @DEMAND_DOC, @PLAN_DELIVERY_DATE, @PLANT_ID, @PLANT_CODE, @PLANT_NAME, @WORK_CENTER_ID, @WORK_CENTER_CODE,  @WORK_CENTER_NAME, @OPERATION_ID, @OPERATION_CODE, @OPERATION_NAME, @MO_ID, @MACHINE_ID, @MACHINE_CODE, @MACHINE_DESCRIPTION, @PLAN_START_DATE ) ";

                string sSql2 = " INSERT INTO " +
                              " MET05_0000_H (  DOC_NO,  SEQUENCE,  ITEM_CODE,  ITEM_NAME,  ITEM_SPECIFICATION,  CUSTOMER_NAME,  PLAN_PRODUCTION_QTY,  DEMAND_DOC,  PLAN_DELIVERY_DATE,  PLANT_ID,  PLANT_CODE,  PLANT_NAME,  WORK_CENTER_ID,  WORK_CENTER_CODE,  WORK_CENTER_NAME,  OPERATION_ID,  OPERATION_CODE,  OPERATION_NAME,  MO_ID, MACHINE_ID, MACHINE_CODE, MACHINE_DESCRIPTION, PLAN_START_DATE, INS_DATETIME) " +
                              "     VALUES   ( @DOC_NO, @SEQUENCE, @ITEM_CODE, @ITEM_NAME, @ITEM_SPECIFICATION, @CUSTOMER_NAME, @PLAN_PRODUCTION_QTY, @DEMAND_DOC, @PLAN_DELIVERY_DATE, @PLANT_ID, @PLANT_CODE, @PLANT_NAME, @WORK_CENTER_ID, @WORK_CENTER_CODE,  @WORK_CENTER_NAME, @OPERATION_ID, @OPERATION_CODE, @OPERATION_NAME, @MO_ID, @MACHINE_ID, @MACHINE_CODE, @MACHINE_DESCRIPTION, @PLAN_START_DATE ,@INS_DATETIME) ";

                sSql = sSql + sSql2;

                using (SqlConnection con_db = comm.Set_DBConnection())
                {
                    SqlCommand sqlCommand = new SqlCommand(sSql);
                    sqlCommand.Connection = con_db;
                    sqlCommand.Parameters.Add(new SqlParameter("@DOC_NO", DOC_NO));
                    sqlCommand.Parameters.Add(new SqlParameter("@SEQUENCE", SEQUENCE));
                    sqlCommand.Parameters.Add(new SqlParameter("@ITEM_CODE", ITEM_CODE));
                    sqlCommand.Parameters.Add(new SqlParameter("@ITEM_NAME", ITEM_NAME));
                    sqlCommand.Parameters.Add(new SqlParameter("@ITEM_SPECIFICATION", ITEM_SPECIFICATION));
                    sqlCommand.Parameters.Add(new SqlParameter("@CUSTOMER_NAME", CUSTOMER_NAME));
                    sqlCommand.Parameters.Add(new SqlParameter("@PLAN_PRODUCTION_QTY", PLAN_PRODUCTION_QTY));
                    sqlCommand.Parameters.Add(new SqlParameter("@DEMAND_DOC", DEMAND_DOC));
                    sqlCommand.Parameters.Add(new SqlParameter("@PLAN_DELIVERY_DATE", PLAN_DELIVERY_DATE));
                    sqlCommand.Parameters.Add(new SqlParameter("@PLANT_ID", PLANT_ID));
                    sqlCommand.Parameters.Add(new SqlParameter("@PLANT_CODE", PLANT_CODE));
                    sqlCommand.Parameters.Add(new SqlParameter("@PLANT_NAME", PLANT_NAME));
                    sqlCommand.Parameters.Add(new SqlParameter("@WORK_CENTER_ID", WORK_CENTER_ID));
                    sqlCommand.Parameters.Add(new SqlParameter("@WORK_CENTER_CODE", WORK_CENTER_CODE));
                    sqlCommand.Parameters.Add(new SqlParameter("@WORK_CENTER_NAME", WORK_CENTER_NAME));
                    sqlCommand.Parameters.Add(new SqlParameter("@OPERATION_ID", OPERATION_ID));
                    sqlCommand.Parameters.Add(new SqlParameter("@OPERATION_CODE", OPERATION_CODE));
                    sqlCommand.Parameters.Add(new SqlParameter("@OPERATION_NAME", OPERATION_NAME));
                    sqlCommand.Parameters.Add(new SqlParameter("@MACHINE_ID", MACHINE_ID));
                    sqlCommand.Parameters.Add(new SqlParameter("@MACHINE_CODE", MACHINE_CODE));
                    sqlCommand.Parameters.Add(new SqlParameter("@MACHINE_DESCRIPTION", MACHINE_DESCRIPTION));
                    sqlCommand.Parameters.Add(new SqlParameter("@MO_ID", MO_ID));
                    sqlCommand.Parameters.Add(new SqlParameter("@PLAN_START_DATE", PLAN_START_DATE));
                    sqlCommand.Parameters.Add(new SqlParameter("@INS_DATETIME", INS_DATETIME));
                    sqlCommand.ExecuteNonQuery();
                }
            }
            //string sql2 = " SELECT * FROM( SELECT ITEM.ITEM_CODE, ITEM.ITEM_NAME, ITEM.ITEM_SPECIFICATION,"
            //            + " FEATURE.FEATURE_NAME, ITEM_FEATURE_VALUE.FEATURE_VALUE_DESC"
            //            + " FROM ITEM INNER JOIN ITEM_FEATURE_VALUE ON ITEM.ITEM_BUSINESS_ID = ITEM_FEATURE_VALUE.ITEM_BUSINESS_ID"
            //            + " INNER JOIN FEATURE ON ITEM_FEATURE_VALUE.FEATURE_ID = FEATURE.FEATURE_ID"
            //            + " WHERE ITEM.ITEM_CODE LIKE 'BA01%' AND ITEM.ApproveStatus = 'Y') t"
            //            + " PIVOT("
            //            + " MAX(FEATURE_VALUE_DESC)"
            //            + " FOR FEATURE_NAME IN([產品大類],[3-車圈型號],[F-特殊孔加工],[車圈EYE],[6-車圈尺寸],[8-顏色],[D-車圈/花鼓孔數],[C-車圈孔徑],[4-安全線],[G-車圈車邊])"
            //            + " ) p ORDER BY ITEM_CODE";


            //DataTable dt2 = comm.Get_AlexDataTable(sql2);

            //for (int i = 0; i < dt2.Rows.Count; i++)
            //{
            //    string ITEM_CODE = dt2.Rows[i]["ITEM_CODE"].ToString();
            //    string ITEM_NAME = dt2.Rows[i]["ITEM_NAME"].ToString();
            //    string ITEM_SPECIFICATION = dt2.Rows[i]["ITEM_SPECIFICATION"].ToString();
            //    string _pro_type = dt2.Rows[i]["產品大類"].ToString();
            //    string spec_a = dt2.Rows[i]["3-車圈型號"].ToString();
            //    string spec_b = dt2.Rows[i]["F-特殊孔加工"].ToString();
            //    string spec_c = dt2.Rows[i]["車圈EYE"].ToString();
            //    string spec_d = dt2.Rows[i]["6-車圈尺寸"].ToString();
            //    string spec_e = dt2.Rows[i]["8-顏色"].ToString();
            //    string spec_f = dt2.Rows[i]["D-車圈/花鼓孔數"].ToString();
            //    string spec_g = dt2.Rows[i]["C-車圈孔徑"].ToString();
            //    string spec_h = dt2.Rows[i]["4-安全線"].ToString();
            //    string spec_i = dt2.Rows[i]["G-車圈車邊"].ToString();

            //    string sSql = " INSERT INTO " +
            //                  " MEB50_0000 (  ITEM_CODE,  ITEM_NAME,  ITEM_SPECIFICATION,  _pro_type ,  spec_a,  spec_b,  spec_c,  spec_d,  spec_e,  spec_f,  spec_g,  spec_h,  spec_i ) " +
            //                  "     VALUES ( @ITEM_CODE, @ITEM_NAME, @ITEM_SPECIFICATION, @_pro_type , @spec_a, @spec_b, @spec_c, @spec_d, @spec_e, @spec_f, @spec_g, @spec_h, @spec_i ) ";

            //    using (SqlConnection con_db = comm.Set_DBConnection())
            //    {
            //        SqlCommand sqlCommand = new SqlCommand(sSql);
            //        sqlCommand.Connection = con_db;
            //        sqlCommand.Parameters.Add(new SqlParameter("@ITEM_CODE", ITEM_CODE));
            //        sqlCommand.Parameters.Add(new SqlParameter("@ITEM_NAME", ITEM_NAME));
            //        sqlCommand.Parameters.Add(new SqlParameter("@ITEM_SPECIFICATION", ITEM_SPECIFICATION));
            //        sqlCommand.Parameters.Add(new SqlParameter("@_pro_type", _pro_type));
            //        sqlCommand.Parameters.Add(new SqlParameter("@spec_a", spec_a));
            //        sqlCommand.Parameters.Add(new SqlParameter("@spec_b", spec_b));
            //        sqlCommand.Parameters.Add(new SqlParameter("@spec_c", spec_c));
            //        sqlCommand.Parameters.Add(new SqlParameter("@spec_d", spec_d));
            //        sqlCommand.Parameters.Add(new SqlParameter("@spec_e", spec_e));
            //        sqlCommand.Parameters.Add(new SqlParameter("@spec_f", spec_f));
            //        sqlCommand.Parameters.Add(new SqlParameter("@spec_g", spec_g));
            //        sqlCommand.Parameters.Add(new SqlParameter("@spec_h", spec_h));
            //        sqlCommand.Parameters.Add(new SqlParameter("@spec_i", spec_i));
            //        sqlCommand.ExecuteNonQuery();
            //    }
            //}
        }


        public void InsertOriData2()
        {
            //string sql2 = " SELECT * FROM( SELECT ITEM.ITEM_CODE, ITEM.ITEM_NAME, ITEM.ITEM_SPECIFICATION,"
            //            + " FEATURE.FEATURE_NAME, ITEM_FEATURE_VALUE.FEATURE_VALUE_DESC"
            //            + " FROM ITEM INNER JOIN ITEM_FEATURE_VALUE ON ITEM.ITEM_BUSINESS_ID = ITEM_FEATURE_VALUE.ITEM_BUSINESS_ID"
            //            + " INNER JOIN FEATURE ON ITEM_FEATURE_VALUE.FEATURE_ID = FEATURE.FEATURE_ID"
            //            + " WHERE ITEM.ITEM_CODE LIKE 'BA01%' AND ITEM.ApproveStatus = 'Y') t"
            //            + " PIVOT("
            //            + " MAX(FEATURE_VALUE_DESC)"
            //            + " FOR FEATURE_NAME IN([產品大類],[3-車圈型號],[F-特殊孔加工],[車圈EYE],[6-車圈尺寸],[8-顏色],[D-車圈/花鼓孔數],[C-車圈孔徑],[4-安全線],[G-車圈車邊])"
            //            + " ) p ORDER BY ITEM_CODE";


            //DataTable dt2 = comm.Get_AlexDataTable(sql2);

            //for (int i = 0; i < dt2.Rows.Count; i++)
            //{
            //    string ITEM_CODE = dt2.Rows[i]["ITEM_CODE"].ToString();
            //    string ITEM_NAME = dt2.Rows[i]["ITEM_NAME"].ToString();
            //    string ITEM_SPECIFICATION = dt2.Rows[i]["ITEM_SPECIFICATION"].ToString();
            //    string _pro_type = dt2.Rows[i]["產品大類"].ToString();
            //    string spec_a = dt2.Rows[i]["3-車圈型號"].ToString();
            //    string spec_b = dt2.Rows[i]["F-特殊孔加工"].ToString();
            //    string spec_c = dt2.Rows[i]["車圈EYE"].ToString();
            //    string spec_d = dt2.Rows[i]["6-車圈尺寸"].ToString();
            //    string spec_e = dt2.Rows[i]["8-顏色"].ToString();
            //    string spec_f = dt2.Rows[i]["D-車圈/花鼓孔數"].ToString();
            //    string spec_g = dt2.Rows[i]["C-車圈孔徑"].ToString();
            //    string spec_h = dt2.Rows[i]["4-安全線"].ToString();
            //    string spec_i = dt2.Rows[i]["G-車圈車邊"].ToString();

            //    string sSql = " INSERT INTO " +
            //                  " MEB50_0000 (  ITEM_CODE,  ITEM_NAME,  ITEM_SPECIFICATION,  _pro_type ,  spec_a,  spec_b,  spec_c,  spec_d,  spec_e,  spec_f,  spec_g,  spec_h,  spec_i ) " +
            //                  "     VALUES ( @ITEM_CODE, @ITEM_NAME, @ITEM_SPECIFICATION, @_pro_type , @spec_a, @spec_b, @spec_c, @spec_d, @spec_e, @spec_f, @spec_g, @spec_h, @spec_i ) ";

            //    using (SqlConnection con_db = comm.Set_DBConnection())
            //    {
            //        SqlCommand sqlCommand = new SqlCommand(sSql);
            //        sqlCommand.Connection = con_db;
            //        sqlCommand.Parameters.Add(new SqlParameter("@ITEM_CODE", ITEM_CODE));
            //        sqlCommand.Parameters.Add(new SqlParameter("@ITEM_NAME", ITEM_NAME));
            //        sqlCommand.Parameters.Add(new SqlParameter("@ITEM_SPECIFICATION", ITEM_SPECIFICATION));
            //        sqlCommand.Parameters.Add(new SqlParameter("@_pro_type", _pro_type));
            //        sqlCommand.Parameters.Add(new SqlParameter("@spec_a", spec_a));
            //        sqlCommand.Parameters.Add(new SqlParameter("@spec_b", spec_b));
            //        sqlCommand.Parameters.Add(new SqlParameter("@spec_c", spec_c));
            //        sqlCommand.Parameters.Add(new SqlParameter("@spec_d", spec_d));
            //        sqlCommand.Parameters.Add(new SqlParameter("@spec_e", spec_e));
            //        sqlCommand.Parameters.Add(new SqlParameter("@spec_f", spec_f));
            //        sqlCommand.Parameters.Add(new SqlParameter("@spec_g", spec_g));
            //        sqlCommand.Parameters.Add(new SqlParameter("@spec_h", spec_h));
            //        sqlCommand.Parameters.Add(new SqlParameter("@spec_i", spec_i));
            //        sqlCommand.ExecuteNonQuery();
            //    }
            //}

            string sql = @"select t1.ITEM_CODE,t1.ITEM_NAME, t1.ITEM_SPECIFICATION,t1.EXPR6,t1.EXPR7,t1.[A01-01],t1.[A01-03],t1.[A01-091],t2.[3-車圈型號],t2.[6-車圈尺寸],t2.[D-車圈/花鼓孔數] 
                         from(
                         select *
                          from(
                          SELECT     ITEM.ITEM_CODE, ITEM.ITEM_NAME, ITEM.ITEM_SPECIFICATION, FEATURE.FEATURE_CODE, ITEM_FEATURE_VALUE.FEATURE_VALUE,
                                               ITEM_FEATURE_VALUE_1.FEATURE_VALUE AS EXPR6,
                                                ITEM_FEATURE_VALUE_1.FEATURE_VALUE_DESC AS EXPR7
                          FROM         FEATURE INNER JOIN
                                                ITEM_FEATURE_VALUE ON FEATURE.FEATURE_ID = ITEM_FEATURE_VALUE.FEATURE_ID INNER JOIN
                                                ITEM_FEATURE_VALUE ITEM_FEATURE_VALUE_1 INNER JOIN
                                                FEATURE FEATURE_1 ON ITEM_FEATURE_VALUE_1.FEATURE_ID = FEATURE_1.FEATURE_ID INNER JOIN
                                                ITEM INNER JOIN
                                                BOM_D ON ITEM.ITEM_BUSINESS_ID = BOM_D.PARENT_ITEM_ID INNER JOIN
                                                ITEM ITEM_1 ON BOM_D.SOURCE_ID_ROid = ITEM_1.ITEM_BUSINESS_ID ON ITEM_FEATURE_VALUE_1.ITEM_BUSINESS_ID = ITEM_1.ITEM_BUSINESS_ID ON
                                                ITEM_FEATURE_VALUE.ITEM_BUSINESS_ID = ITEM.ITEM_BUSINESS_ID
                          WHERE ITEM.ApproveStatus = 'Y' AND ITEM_1.ApproveStatus = 'Y' AND FEATURE_1.FEATURE_CODE = 'B01-01' AND FEATURE.FEATURE_CODE in ('A01-01', 'A01-03', 'A01-091')
                          ) t
                          pivot(
                              MAX(FEATURE_VALUE)
                              FOR FEATURE_CODE IN([A01-01],[A01-03],[A01-091])
                          ) p
                          ) t1
                          left join(
                          select *
                          from(
                          SELECT     ITEM.ITEM_CODE, FEATURE.FEATURE_NAME,
                                                ITEM_FEATURE_VALUE.FEATURE_VALUE_DESC, ITEM_FEATURE_VALUE_1.FEATURE_VALUE AS EXPR6,
                                                ITEM_FEATURE_VALUE_1.FEATURE_VALUE_DESC AS EXPR7
                          FROM         FEATURE INNER JOIN
                                                ITEM_FEATURE_VALUE ON FEATURE.FEATURE_ID = ITEM_FEATURE_VALUE.FEATURE_ID INNER JOIN
                                                ITEM_FEATURE_VALUE ITEM_FEATURE_VALUE_1 INNER JOIN
                                                FEATURE FEATURE_1 ON ITEM_FEATURE_VALUE_1.FEATURE_ID = FEATURE_1.FEATURE_ID INNER JOIN
                                                ITEM INNER JOIN
                                                BOM_D ON ITEM.ITEM_BUSINESS_ID = BOM_D.PARENT_ITEM_ID INNER JOIN
                                                ITEM ITEM_1 ON BOM_D.SOURCE_ID_ROid = ITEM_1.ITEM_BUSINESS_ID ON ITEM_FEATURE_VALUE_1.ITEM_BUSINESS_ID = ITEM_1.ITEM_BUSINESS_ID ON
                                                ITEM_FEATURE_VALUE.ITEM_BUSINESS_ID = ITEM.ITEM_BUSINESS_ID
                          WHERE ITEM.ApproveStatus = 'Y' AND ITEM_1.ApproveStatus = 'Y' AND FEATURE_1.FEATURE_CODE = 'B01-01' AND FEATURE.FEATURE_CODE in ('A01-01', 'A01-03', 'A01-091')
                          ) t
                          pivot(
                              MAX(FEATURE_VALUE_DESC)
                              FOR FEATURE_NAME IN([3-車圈型號],[6-車圈尺寸],[D-車圈/花鼓孔數])
                          ) p
                          ) t2 on t1.ITEM_CODE = t2.ITEM_CODE ";

            DataTable dt2 = comm.Get_AlexDataTable(sql);

            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                string ITEM_CODE = dt2.Rows[i]["ITEM_CODE"].ToString();
                string ITEM_NAME = dt2.Rows[i]["ITEM_NAME"].ToString();
                string ITEM_SPECIFICATION = dt2.Rows[i]["ITEM_SPECIFICATION"].ToString();
                string spec_1 = dt2.Rows[i]["A01-01"].ToString();
                string spec_2 = dt2.Rows[i]["3-車圈型號"].ToString();
                string spec_3 = dt2.Rows[i]["EXPR6"].ToString();
                string spec_4 = dt2.Rows[i]["EXPR7"].ToString();
                string spec_5 = dt2.Rows[i]["A01-03"].ToString();
                string spec_6 = dt2.Rows[i]["6-車圈尺寸"].ToString();
                string spec_7 = dt2.Rows[i]["A01-091"].ToString();
                string spec_8 = dt2.Rows[i]["D-車圈/花鼓孔數"].ToString();

                string sSql = " INSERT INTO " +
                              " MEB50_0000 (  ITEM_CODE,  ITEM_NAME,  ITEM_SPECIFICATION,   spec_1 ,   spec_2,   spec_3,  spec_4,  spec_5,  spec_6,  spec_7,  spec_8, pro_uph ) " +
                              "     VALUES ( @ITEM_CODE, @ITEM_NAME, @ITEM_SPECIFICATION,  @spec_1 ,  @spec_2,  @spec_3, @spec_4, @spec_5, @spec_6, @spec_7, @spec_8 , 0) ";

                using (SqlConnection con_db = comm.Set_DBConnection())
                {
                    SqlCommand sqlCommand = new SqlCommand(sSql);
                    sqlCommand.Connection = con_db;
                    sqlCommand.Parameters.Add(new SqlParameter("@ITEM_CODE", ITEM_CODE));
                    sqlCommand.Parameters.Add(new SqlParameter("@ITEM_NAME", ITEM_NAME));
                    sqlCommand.Parameters.Add(new SqlParameter("@ITEM_SPECIFICATION", ITEM_SPECIFICATION));
                    sqlCommand.Parameters.Add(new SqlParameter("@spec_1", spec_1));
                    sqlCommand.Parameters.Add(new SqlParameter("@spec_2", spec_2));
                    sqlCommand.Parameters.Add(new SqlParameter("@spec_3", spec_3));
                    sqlCommand.Parameters.Add(new SqlParameter("@spec_4", spec_4));
                    sqlCommand.Parameters.Add(new SqlParameter("@spec_5", spec_5));
                    sqlCommand.Parameters.Add(new SqlParameter("@spec_6", spec_6));
                    sqlCommand.Parameters.Add(new SqlParameter("@spec_7", spec_7));
                    sqlCommand.Parameters.Add(new SqlParameter("@spec_8", spec_8));
                    sqlCommand.ExecuteNonQuery();
                }
            }

        }
        /// <summary>
        /// 傳入一個MET02_0000的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="MET02_0000">DTO</param>
        public void UpdateData(MET02_0000 MET02_0000)
        {
            string sSql = " UPDATE MET02_0000 " +
                          "    SET plan_out_date  =  @plan_out_date, " +
                          "        seq_no   =  @seq_no,  " +
                          "        sor_code   =  @sor_code,  " +
                          "        cus_name  =  @cus_name, " +
                          "        pro_code  =  @pro_code, " +
                          "        pro_name   =  @pro_name,  " +
                          "        pro_spec  =  @pro_spec, " +
                          "        spec_a  =  @spec_a, " +
                          "        spec_b   =  @spec_b,  " +
                          "        spec_c  =  @spec_c, " +
                          "        plan_qty  =  @plan_qty, " +
                          "        mo_qty   =  @mo_qty,  " +
                          "        mo_status =  @mo_status         " +
                          "  WHERE mo_code  =  @mo_code  ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, MET02_0000);
            }
        }

        public void UpdateData2(string pTkCode)
        {
            string sSql = "update MET02_0000 set mo_code = '#'+@mo_code ,MO_STATUS = '3' where mo_code = @mo_code";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { mo_code = pTkCode });
            }
        }
        public void UpdateData3()
        {
            string sql = "update MET02_0000 set MO_STATUS2 = '待生產' where mo_status = '0'";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sql);
                sqlCommand.Connection = con_db;
                sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "update MET02_0000 set mo_code = '#'+@mo_code ,MO_STATUS = '3' where mo_code = @mo_code";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { mo_code = pTkCode });
            }
        }
        public void DeleteData2(string pTable)
        {
            string sSql = "DELETE FROM " + pTable;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql);
            }
        }

        public void DeleteData4(string pTkCode)
        {
            string sSql = "DELETE FROM MET02_0000 WHERE mo_code = @mo_code ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { mo_code = pTkCode });
            }
        }
        public void DeleteData3(string pTkCode)
        {
            string sSql = "DELETE FROM MBA_E10 WHERE MO_DOC_NO = @mo_code ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { mo_code = pTkCode });
            }
        }

        public void DeleteData5()
        {
            string sSql = "DELETE FROM MET02_0000 WHERE MO_STATUS in ('W','0') ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql);
            }
        }

        public void DeleteData6()
        {
            string date = DateTime.Now.ToString("yyyy/MM/dd");
            string sSql = "DELETE FROM MET02_0000 WHERE MO_STATUS = 'W' and ins_date < @ins_date";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { ins_date = date });
            }
        }
        public void set_seq_type(string id)
        {
            string linetype = "";
            if (id=="A")
            {
                linetype = "單線";
            }
            if (id == "B")
            {
                linetype = "混線";
            }
            string sSql = "UPDATE MET02_0000 SET seq_type ='" + linetype + "' where 1=1";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql);
            }
        }
    }
}