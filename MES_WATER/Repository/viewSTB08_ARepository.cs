using MES_WATER.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MES_WATER.Repository
{
    public class viewSTB08_ARepository
    {

        Comm comm = new Comm();

        /// <summary>
        /// 取得STB08_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO STB08_0000</returns>
        public viewSTB08_A GetDTO(string pTkCode)
        {
            viewSTB08_A datas = new viewSTB08_A();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM STB08_0000";
            }
            else
            {
                //sSql = "SELECT * FROM STB08_0000 where cus_code=@cus_code";

                sSql = "SELECT STB08_0000.*, STB08_1000.*, STB08_2000.ent_date, STB08_2000.exp_date, STB08_2000.ent_lel, STB08_2000.bir_date, STB08_2000.cus_sex" +
                       "    FROM STB08_0000 " + 
                       "    left join STB08_1000 on STB08_1000.cus_code = STB08_0000.cus_code " +
                       "    left join STB08_2000 on STB08_2000.cus_code = STB08_0000.cus_code " +
                       "    where STB08_0000.cus_code = @cus_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@cus_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new viewSTB08_A
                        {
                            // STB08_0000
                            cus_code = reader.GetString(reader.GetOrdinal("cus_code")),
                            cus_name = reader.GetString(reader.GetOrdinal("cus_name")),
                            cus_hypo = reader.GetString(reader.GetOrdinal("cus_hypo")),
                            inv_title = reader.GetString(reader.GetOrdinal("inv_title")),
                            zip_code = reader.GetString(reader.GetOrdinal("zip_code")),
                            att_add = reader.GetString(reader.GetOrdinal("att_add")),
                            inv_idno = reader.GetString(reader.GetOrdinal("inv_idno")),
                            cus_tel1 = reader.GetString(reader.GetOrdinal("cus_tel1")),
                            cus_tel2 = reader.GetString(reader.GetOrdinal("cus_tel2")),
                            cus_fax = reader.GetString(reader.GetOrdinal("cus_fax")),
                            //------
                            cus_email = reader.GetString(reader.GetOrdinal("cus_email")),
                            cus_url = reader.GetString(reader.GetOrdinal("cus_url")),
                            blk_code = reader.GetString(reader.GetOrdinal("blk_code")),
                            led_name = reader.GetString(reader.GetOrdinal("led_name")),
                            rea_code = reader.GetString(reader.GetOrdinal("rea_code")),
                            bus_code = reader.GetString(reader.GetOrdinal("bus_code")),
                            is_use = reader.GetString(reader.GetOrdinal("is_use")),
                            sto_code = reader.GetString(reader.GetOrdinal("sto_code")),
                            pri_lel = reader.GetString(reader.GetOrdinal("pri_lel")),
                            per_code = reader.GetString(reader.GetOrdinal("per_code")),
                            ////------
                            com_mode = reader.GetString(reader.GetOrdinal("com_mode")),
                            cmemo = reader.GetString(reader.GetOrdinal("cmemo")),
                            tra_kind = reader.GetString(reader.GetOrdinal("tra_kind")),
                            inv_add = reader.GetString(reader.GetOrdinal("inv_add")),
                            acc_add = reader.GetString(reader.GetOrdinal("acc_add")),
                            acc_zip = reader.GetString(reader.GetOrdinal("acc_zip")),


                            //------
                            unit_group = reader.IsDBNull(reader.GetOrdinal("unit_group")) ? "" : reader.GetString(reader.GetOrdinal("unit_group")),
                            usr_code = reader.IsDBNull(reader.GetOrdinal("usr_code")) ? "" : reader.GetString(reader.GetOrdinal("usr_code")),
                            ins_date = reader.IsDBNull(reader.GetOrdinal("ins_date")) ? "" : reader.GetString(reader.GetOrdinal("ins_date")),
                            ins_time = reader.IsDBNull(reader.GetOrdinal("ins_time")) ? "" : reader.GetString(reader.GetOrdinal("ins_time")),

                            cap_amt = reader.GetDouble(reader.GetOrdinal("cap_amt")),
                            tra_amt = reader.GetDouble(reader.GetOrdinal("tra_amt")),
                            cre_date = reader.GetString(reader.GetOrdinal("cre_date")),
                            per_num = reader.GetDouble(reader.GetOrdinal("per_num")),
                            not_trans = reader.GetString(reader.GetOrdinal("not_trans")),
                            //pro_list = reader.GetString(reader.GetOrdinal("pro_list")),
                            //hol_code = reader.GetString(reader.GetOrdinal("hol_code")),
                            //is_stamp = reader.GetString(reader.GetOrdinal("is_stamp")),
                            //is_accounts = reader.GetString(reader.GetOrdinal("is_accounts")),
                            //------
                            //spl_accounts = reader.GetString(reader.GetOrdinal("spl_accounts")),
                            //route_code = reader.GetString(reader.GetOrdinal("route_code")),
                            //branch_no = reader.GetString(reader.GetOrdinal("branch_no")),
                            //head_no = reader.GetString(reader.GetOrdinal("head_no")),
                            //web_01 = reader.GetString(reader.GetOrdinal("web_01")),
                            //web_02 = reader.GetString(reader.GetOrdinal("web_02")),
                            //web_03 = reader.GetString(reader.GetOrdinal("web_03")),
                            //web_04 = reader.GetString(reader.GetOrdinal("web_04")),
                            //web_05 = reader.GetString(reader.GetOrdinal("web_05")),
                            //web_06 = reader.GetString(reader.GetOrdinal("web_06")),
                            ////------
                            //web_07 = reader.GetString(reader.GetOrdinal("web_07")),
                            //web_08 = reader.GetString(reader.GetOrdinal("web_08")),
                            //web_09 = reader.GetString(reader.GetOrdinal("web_09")),
                            //web_10 = reader.GetString(reader.GetOrdinal("web_10")),
                            //exp_date = reader.GetString(reader.GetOrdinal("exp_date")),
                            //bir_date = reader.GetString(reader.GetOrdinal("bir_date")),
                            //cus_sex = reader.GetString(reader.GetOrdinal("cus_sex")),
                            //per_idno = reader.GetString(reader.GetOrdinal("per_idno")),
                            //------

                            // STB08_10000
                            stv_code = reader.GetString(reader.GetOrdinal("stv_code")),
                            inv_type = reader.GetString(reader.GetOrdinal("inv_type")),
                            sal_memo = reader.GetString(reader.GetOrdinal("sal_memo")),
                            pay_term = reader.GetString(reader.GetOrdinal("pay_term")),
                            crd_amt = reader.GetDouble(reader.GetOrdinal("crd_amt")),
                            rec_type = reader.GetString(reader.GetOrdinal("rec_type")),
                            is_month = reader.GetString(reader.GetOrdinal("is_month")),
                            cmonth0 = reader.GetInt32(reader.GetOrdinal("cmonth0")),
                            cmonth1 = reader.GetInt32(reader.GetOrdinal("cmonth1")),
                            cmonth2 = reader.GetInt32(reader.GetOrdinal("cmonth2")),
                            cmonth3 = reader.GetInt32(reader.GetOrdinal("cmonth3")),
                            cday0 = reader.GetInt32(reader.GetOrdinal("cday0")),
                            cday1 = reader.GetInt32(reader.GetOrdinal("cday1")),
                            cday2 = reader.GetInt32(reader.GetOrdinal("cday2")),
                            cday3 = reader.GetInt32(reader.GetOrdinal("cday3")),
                            act_code01 = reader.GetString(reader.GetOrdinal("act_code01")),
                            act_code02 = reader.GetString(reader.GetOrdinal("act_code02")),
                            act_code03 = reader.GetString(reader.GetOrdinal("act_code03")),
                            act_code04 = reader.GetString(reader.GetOrdinal("act_code04")),
                            act_code05 = reader.GetString(reader.GetOrdinal("act_code05")),
                            act_code06 = reader.GetString(reader.GetOrdinal("act_code06")),
                            act_code07 = reader.GetString(reader.GetOrdinal("act_code07")),
                            max_dis = reader.GetInt32(reader.GetOrdinal("max_dis")),
                            acc_rel = reader.GetString(reader.GetOrdinal("acc_rel")),

                            // 原來的
                            //acc_rel = Get_CusDetail("STB08_1000",reader.GetString(reader.GetOrdinal("cus_code")), "acc_rel"),
                            //act_code01 = Get_CusDetail("STB08_1000", reader.GetString(reader.GetOrdinal("cus_code")), "act_code01"),
                            //act_code02 = Get_CusDetail("STB08_1000", reader.GetString(reader.GetOrdinal("cus_code")), "act_code02"),
                            //act_code03 = Get_CusDetail("STB08_1000", reader.GetString(reader.GetOrdinal("cus_code")), "act_code03"),
                            //act_code04 = Get_CusDetail("STB08_1000", reader.GetString(reader.GetOrdinal("cus_code")), "act_code04"),
                            //act_code05 = Get_CusDetail("STB08_1000", reader.GetString(reader.GetOrdinal("cus_code")), "act_code05"),
                            //act_code06 = Get_CusDetail("STB08_1000", reader.GetString(reader.GetOrdinal("cus_code")), "act_code06"),
                            //cday0 = Int32.Parse(Get_CusDetail("STB08_1000", reader.GetString(reader.GetOrdinal("cus_code")), "cday0")),
                            //cday1 = Int32.Parse(Get_CusDetail("STB08_1000", reader.GetString(reader.GetOrdinal("cus_code")), "cday1")),
                            //cday2 = Int32.Parse(Get_CusDetail("STB08_1000", reader.GetString(reader.GetOrdinal("cus_code")), "cday2")),
                            //cday3 = Int32.Parse(Get_CusDetail("STB08_1000", reader.GetString(reader.GetOrdinal("cus_code")), "cday3")),
                            //cmonth0 = Int32.Parse(Get_CusDetail("STB08_1000", reader.GetString(reader.GetOrdinal("cus_code")), "cmonth0")),
                            //cmonth1 = Int32.Parse(Get_CusDetail("STB08_1000", reader.GetString(reader.GetOrdinal("cus_code")), "cmonth1")),
                            //cmonth2 = Int32.Parse(Get_CusDetail("STB08_1000", reader.GetString(reader.GetOrdinal("cus_code")), "cmonth2")),
                            //cmonth3 = Int32.Parse(Get_CusDetail("STB08_1000", reader.GetString(reader.GetOrdinal("cus_code")), "cmonth3")),
                            //crd_amt = Decimal.Parse(Get_CusDetail("STB08_1000", reader.GetString(reader.GetOrdinal("cus_code")), "crd_amt")),
                            //inv_type = Get_CusDetail("STB08_1000", reader.GetString(reader.GetOrdinal("cus_code")), "inv_type"),
                            //is_month = Get_CusDetail("STB08_1000", reader.GetString(reader.GetOrdinal("cus_code")), "is_month"),
                            //max_dis = Int32.Parse(Get_CusDetail("STB08_1000", reader.GetString(reader.GetOrdinal("cus_code")), "max_dis")),
                            //pay_term = Get_CusDetail("STB08_1000", reader.GetString(reader.GetOrdinal("cus_code")), "pay_term"),
                            //rec_type = Get_CusDetail("STB08_1000", reader.GetString(reader.GetOrdinal("cus_code")), "rec_type"),
                            //stv_code = Get_CusDetail("STB08_1000", reader.GetString(reader.GetOrdinal("cus_code")), "stv_code"),
                            //ent_lel = Get_CusDetail("STB08_2000", reader.GetString(reader.GetOrdinal("cus_code")), "ent_lel"),
                            //bonus_amt = Decimal.Parse(Get_CusDetail("STB08_2000", reader.GetString(reader.GetOrdinal("cus_code")), "bonus_amt")),
                            ////------
                        };
                    }
                }
            }
            return datas;
        }

        private string Get_CusDetail(string pTable,string pCusCode,string pFieldCode) {
            string sSql = "select * from "+ pTable + " " +
                          " where cus_code = '" + pCusCode + "'";
            var dtTmp = comm.Get_DataTable(sSql);

            if (dtTmp.Rows.Count > 0)
            {
                return dtTmp.Rows[0][pFieldCode].ToString();
            }
            else { 
                sSql = "SELECT * FROM INFORMATION_SCHEMA.Columns " + 
                       " Where Table_Name = '" + pTable + "'" +
                       "   and COLUMN_NAME = '" + pFieldCode + "'";
                dtTmp = comm.Get_DataTable(sSql);
                if (dtTmp.Rows.Count > 0) {
                    switch (dtTmp.Rows[0]["DATA_TYPE"].ToString()) {
                        case "nvarchar":
                            return "";
                        case "decimal":
                            return "0";
                        case "int":
                            return "0";
                        default:
                            return "";
                    }
                }
                return "";
            }
        }

        

    }
}