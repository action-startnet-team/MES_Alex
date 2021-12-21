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
    public class EPB02_0100Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得EPB02_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO EPB02_0100</returns>
        public EPB02_0100 GetDTO(string pTkCode)
        {
            EPB02_0100 datas = new EPB02_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EPB02_0100";
            }
            else
            {
                sSql = "SELECT * FROM EPB02_0100 where epb02_0100=@epb02_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@epb02_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new EPB02_0100
                        {                   
                            epb02_0100 = comm.sGetInt32(reader["epb02_0100"].ToString()),
                            epb_code = comm.sGetString(reader["epb_code"].ToString()),
                            field_code = comm.sGetString(reader["field_code"].ToString()),
                            field_name = comm.sGetString(reader["field_name"].ToString()),
                            field_memo = comm.sGetString(reader["field_memo"].ToString()),
                            scr_no = comm.sGetInt32(reader["scr_no"].ToString()),
                            ctr_type = comm.sGetString(reader["ctr_type"].ToString()),
                            ctr_default_value = comm.sGetString(reader["ctr_default_value"].ToString()),
                            data_type = comm.sGetString(reader["data_type"].ToString()),
                            field_length = comm.sGetInt32(reader["field_length"].ToString()),
                            select_code = comm.sGetString(reader["select_code"].ToString()),
                            is_key = comm.sGetString(reader["is_key"].ToString()),
                            need_value = comm.sGetString(reader["need_value"].ToString()),
                            save_field = comm.sGetString(reader["save_field"].ToString()),
                            is_multi = comm.sGetString(reader["is_multi"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得EPB02_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List EPB02_0100</returns>
        public List<EPB02_0100> Get_DataList(string pTkCode)
        {
            List<EPB02_0100> list = new List<EPB02_0100>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EPB02_0100";
            }
            else
            {
                sSql = "SELECT * FROM EPB02_0100 where epb02_0100=@epb02_0100";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@epb02_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EPB02_0100 data = new EPB02_0100();

                    data.epb02_0100 = comm.sGetInt32(reader["epb02_0100"].ToString());
                    data.epb_code = comm.sGetString(reader["epb_code"].ToString());
                    data.field_code = comm.sGetString(reader["field_code"].ToString());
                    data.field_name = comm.sGetString(reader["field_name"].ToString());
                    data.field_memo = comm.sGetString(reader["field_memo"].ToString());
                    data.scr_no = comm.sGetInt32(reader["scr_no"].ToString());
                    data.ctr_type = comm.sGetString(reader["ctr_type"].ToString());
                    data.ctr_default_value = comm.sGetString(reader["ctr_default_value"].ToString());
                    data.data_type = comm.sGetString(reader["data_type"].ToString());
                    data.field_length = comm.sGetInt32(reader["field_length"].ToString());
                    data.select_code = comm.sGetString(reader["select_code"].ToString());
                    data.is_key = comm.sGetString(reader["is_key"].ToString());
                    data.need_value = comm.sGetString(reader["need_value"].ToString());

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
        public List<EPB02_0100> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_epb02_0100", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<EPB02_0100> list = new List<EPB02_0100>();
            string sSql = "";

            //取得該使用者可以看的資料
            sSql = " SELECT * FROM EPB02_0100 " ;

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@epb02_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EPB02_0100 data = new EPB02_0100();

                    data.epb02_0100 = comm.sGetInt32(reader["epb02_0100"].ToString());
                    data.epb_code = comm.sGetString(reader["epb_code"].ToString());
                    data.field_code = comm.sGetString(reader["field_code"].ToString());
                    data.field_name = comm.sGetString(reader["field_name"].ToString());
                    data.field_memo = comm.sGetString(reader["field_memo"].ToString());
                    data.scr_no = comm.sGetInt32(reader["scr_no"].ToString());
                    data.ctr_type = comm.sGetString(reader["ctr_type"].ToString());
                    data.ctr_default_value = comm.sGetString(reader["ctr_default_value"].ToString());
                    data.data_type = comm.sGetString(reader["data_type"].ToString());
                    data.field_length = comm.sGetInt32(reader["field_length"].ToString());
                    data.select_code = comm.sGetString(reader["select_code"].ToString());
                    data.is_key = comm.sGetString(reader["is_key"].ToString());
                    data.need_value = comm.sGetString(reader["need_value"].ToString());

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.epb02_0100)) {
                    //    data.can_delete = "N";
                    //    data.can_update = "N";
                    //}

                    list.Add(data);
                }
            }
            return list;
        }
        #endregion
        public List<EPB02_0100> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<EPB02_0100> list = new List<EPB02_0100>();
            string foreignKey = gmv.GetKey<EPB02_0000>(new EPB02_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT EPB02_0100.*, BDPctr.field_name as ctr_name , BDPdata.field_name as data_name     " +
                       " FROM EPB02_0100                                                    " +
                       " left join BDP21_0100 AS BDPctr on BDPctr.field_code = EPB02_0100.ctr_type and BDPctr.code_code = 'ctr_type'  " +
                       " left join BDP21_0100 AS BDPdata on BDPdata.field_code = EPB02_0100.data_type and BDPdata.code_code = 'data_type' " +
                       " where EPB02_0100. " + foreignKey + "=@" + foreignKey ;
            }
            else
            { 
                sSql = "SELECT * FROM EPB02_0100";
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
                    
                    EPB02_0100 data = new EPB02_0100();

                    data.epb02_0100 = comm.sGetInt32(reader["epb02_0100"].ToString());
                    data.epb_code = comm.sGetString(reader["epb_code"].ToString());
                    data.common_str = "<a href='/EPB020A/Common_String?Key="+ comm.sGetInt32(reader["epb02_0100"].ToString()) + "' target='_blank'><i class='ace-icon fa fa-book bigger-150 blue'></i></a>";
                    data.export_commonstr = Chk_IsHaveCommonStr(comm.sGetString(reader["epb02_0100"].ToString()));
                    data.field_code = comm.sGetString(reader["field_code"].ToString());
                    data.field_name = comm.sGetString(reader["field_name"].ToString());                  
                    data.field_memo = comm.sGetString(reader["field_memo"].ToString());
                    data.scr_no = comm.sGetInt32(reader["scr_no"].ToString());
                    data.ctr_type = comm.sGetString(reader["ctr_type"].ToString());
                    data.ctr_name = comm.sGetString(reader["ctr_name"].ToString());
                    data.ctr_default_value = comm.sGetString(reader["ctr_default_value"].ToString());
                    data.data_type = comm.sGetString(reader["data_type"].ToString());
                    data.data_name = comm.sGetString(reader["data_name"].ToString());
                    data.field_length = comm.sGetInt32(reader["field_length"].ToString());
                    data.select_code = comm.sGetString(reader["select_code"].ToString());
                    data.is_key = comm.sGetString(reader["is_key"].ToString());
                    data.need_value = comm.sGetString(reader["need_value"].ToString());
                    data.save_field = comm.sGetString(reader["save_field"].ToString());
                    data.is_multi = comm.sGetString(reader["is_multi"].ToString());

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


        public string Chk_IsHaveCommonStr(string epb02_0100) {
            string sValue = "未設定";
            string sSql = "select * from EPB02_0200 " +
                          " where epb02_0100 = '" + epb02_0100 + "'";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0) {
                sValue = "<a href='/Export/Export_CommonStr?Key=" + epb02_0100 + "' title='常用字串列印' ><i class='ace-icon fa fa-file-excel-o bigger-150 green'></i></a>";
            }
            return sValue;
        }

        
        /// <summary>
        /// 傳入一個EPB02_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="EPB02_0100">DTO</param>
        public void InsertData(EPB02_0100 EPB02_0100)
        {
            string sSql = "INSERT INTO " +
                          " EPB02_0100 (  epb_code,           field_code,   field_name,    field_memo,    scr_no,   ctr_type, is_multi,    " +
                          "               ctr_default_value,  data_type,    field_length,  select_code,   is_key,   need_value , save_field ) " +

                          "     VALUES ( @epb_code,          @field_code,  @field_name,   @field_memo,   @scr_no,  @ctr_type, @is_multi,   " +
                          "              @ctr_default_value, @data_type,   @field_length, @select_code,  @is_key,  @need_value ,@save_field) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EPB02_0100);
            }
        }

        /// <summary>
        /// 傳入一個EPB02_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="EPB02_0100">DTO</param>
        public void UpdateData(EPB02_0100 EPB02_0100)
        {
            //string pTkCode = EPB02_0100.epb02_0100.ToString();
            //Int32 iProQty = comm.sGetInt32(comm.Get_Data("EPB02_0100", pTkCode, "epb02_0100", "pro_qty"));
            //Int32 iSorSerial = comm.sGetInt32(comm.Get_Data("EPB02_0100", pTkCode, "epb02_0100", "sor_serial"));

            //ws.Cal_TraQty("DEL", "STT01_0100", "res_qty", iProQty, "where stt01_0100=" + iSorSerial);
            //ws.Cal_TraQty("ADD", "STT01_0100", "res_qty", comm.sGetInt32(EPB02_0100.pro_qty.ToString()), "where stt01_0100=" + comm.sGetString(EPB02_0100.sor_serial.ToString()));

             
            string sSql = " UPDATE EPB02_0100                              " +
                          "    SET epb_code          = @epb_code,          " +
                          "        field_code        = @field_code,        " +
                          "        field_name        = @field_name,        " +
                          "        field_memo        = @field_memo,        " +
                          "        scr_no            = @scr_no,            " +
                          "        ctr_type          = @ctr_type,          " +
                          "        ctr_default_value = @ctr_default_value, " +
                          "        data_type         = @data_type,         " +
                          "        field_length      = @field_length,      " +
                          "        select_code       = @select_code,       " +
                          "        is_key            = @is_key,            " +
                          "        is_multi          = @is_multi,          " +
                          "        need_value        = @need_value,       " +
                          "        save_field        = @save_field" +

                          "  WHERE epb02_0100 = @epb02_0100  " ;


            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EPB02_0100);

                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@epb02_0100", EPB02_0100.epb02_0100));
                //sqlCommand.Parameters.Add(new SqlParameter("@epb02_0100", EPB02_0100.epb02_0100));
                //sqlCommand.Parameters.Add(new SqlParameter("@epb_code", EPB02_0100.epb_code));
                //sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM EPB02_0100 WHERE epb02_0100 = @epb02_0100;";
            //sSql += " Delete from BDP09_0100 where epb02_0100 = @epb02_0100; ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { epb02_0100 = pTkCode });
                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@epb02_0100", pTkCode));
                //sqlCommand.ExecuteNonQuery();
            }
        }
        
        /*
         * 暫存DataTable參考
        /// <summary>
        /// 取得EPB02_0100角色的DataTable
        /// </summary>
        /// <param name="pTkCode">有傳鍵值取一筆，鍵值空白取全部</param>
        /// <returns></returns>
        public DataTable GetEPB02_0100_dt(string pTkCode)
        {
            DataTable dtTmp = new DataTable();
            DataTable dtDat = new DataTable();
            dtDat.Columns.Add("epb02_0100", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("epb02_0100", System.Type.GetType("System.String"].ToString());
            dtDat.Columns.Add("epb_code", System.Type.GetType("System.String"].ToString());
            string sSql = "";
            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EPB02_0100";
            }
            else
            {
                sSql = "SELECT * FROM EPB02_0100 where epb02_0100='" + pTkCode + "'";
            }
            dtTmp = comm.Get_DataTable(sSql);

            int i;
            for (i = 1; i < dtTmp.Rows.Count - 1; i++)
            {
                DataRow drow = dtDat.NewRow();
                drow["epb02_0100"] = dtTmp.Rows[i]["epb02_0100"];
                drow["epb02_0100"] = dtTmp.Rows[i]["epb02_0100"];
                drow["epb_code"] = dtTmp.Rows[i]["epb_code"];
                dtDat.Rows.Add(drow);
            }
            return dtDat;
        }
        */

    }
}