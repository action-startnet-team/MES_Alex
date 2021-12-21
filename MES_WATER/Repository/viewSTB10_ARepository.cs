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
    public class viewSTB10_ARepository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得STB10_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO STB10_0000</returns>
        public viewSTB10_A GetDTO(string pTkCode)
        {
            viewSTB10_A datas = new viewSTB10_A();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM STB10_0000";
            }
            else
            { 
                sSql = "SELECT STB10_0000.*, STB10_1000.* FROM STB10_0000 " +
                       "    left join STB10_1000 on STB10_1000.sup_code = STB10_0000.sup_code " +
                       "    where STB10_0000.sup_code=@sup_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@sup_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new viewSTB10_A
                        {
                            // STB10_0000
                            sup_code = reader.GetString(reader.GetOrdinal("sup_code")),
                            sup_name = reader.GetString(reader.GetOrdinal("sup_name")),
                            sup_hypo = reader.GetString(reader.GetOrdinal("sup_hypo")),
                            sup_kind = reader.GetString(reader.GetOrdinal("sup_kind")),
                            zip_code = reader.GetString(reader.GetOrdinal("zip_code")),
                            att_add = reader.GetString(reader.GetOrdinal("att_add")),
                            inv_idno = reader.GetString(reader.GetOrdinal("inv_idno")),
                            inv_title = reader.GetString(reader.GetOrdinal("inv_title")),
                            sup_tel1 = reader.GetString(reader.GetOrdinal("sup_tel1")),
                            sup_tel2 = reader.GetString(reader.GetOrdinal("sup_tel2")),
                            //------
                            sup_fax = reader.GetString(reader.GetOrdinal("sup_fax")),
                            sup_email = reader.GetString(reader.GetOrdinal("sup_email")),
                            sup_url = reader.GetString(reader.GetOrdinal("sup_url")),
                            blk_code = reader.GetString(reader.GetOrdinal("blk_code")),
                            led_name = reader.GetString(reader.GetOrdinal("led_name")),
                            rea_code = reader.GetString(reader.GetOrdinal("rea_code")),
                            bus_code = reader.GetString(reader.GetOrdinal("bus_code")),
                            stv_code = reader.GetString(reader.GetOrdinal("stv_code")),
                            is_use = reader.GetString(reader.GetOrdinal("is_use")),
                            sto_code = reader.GetString(reader.GetOrdinal("sto_code")),

                            //------
                            com_mode = reader.GetString(reader.GetOrdinal("com_mode")),
                            cmemo = reader.GetString(reader.GetOrdinal("cmemo")),
                            tra_kind = reader.GetString(reader.GetOrdinal("tra_kind")),
                            inv_add = reader.GetString(reader.GetOrdinal("inv_add")),

                            usr_code = reader.IsDBNull(reader.GetOrdinal("usr_code")) ? "" : reader.GetString(reader.GetOrdinal("usr_code")),
                            ins_date = reader.IsDBNull(reader.GetOrdinal("ins_date")) ? "" : reader.GetString(reader.GetOrdinal("ins_date")),
                            ins_time = reader.IsDBNull(reader.GetOrdinal("ins_time")) ? "" : reader.GetString(reader.GetOrdinal("ins_time")),
                        

                            //------

                            // STB10_1000
                            pur_memo = reader.GetString(reader.GetOrdinal("pur_memo")),
                            pay_term = reader.GetString(reader.GetOrdinal("pay_term")),
                            crd_amt = reader.GetDouble(reader.GetOrdinal("crd_amt")),
                            pay_type = reader.GetString(reader.GetOrdinal("pay_type")),
                            is_week = reader.GetString(reader.GetOrdinal("is_week")),
                            is_month = reader.GetString(reader.GetOrdinal("is_month")),
                            cmonth0 = reader.GetInt32(reader.GetOrdinal("cmonth0")),
                            cmonth1 = reader.GetInt32(reader.GetOrdinal("cmonth1")),
                            cmonth2 = reader.GetInt32(reader.GetOrdinal("cmonth2")),
                            cmonth3 = reader.GetInt32(reader.GetOrdinal("cmonth3")),
                            cday0 = reader.GetInt32(reader.GetOrdinal("cday0")),
                            cday1 = reader.GetInt32(reader.GetOrdinal("cday1")), 
                            cday2 = reader.GetInt32(reader.GetOrdinal("cday2")),
                            cday3 = reader.GetInt32(reader.GetOrdinal("cday3")),
                            dis_days = reader.GetInt32(reader.GetOrdinal("dis_days")),
                            dis_memo = reader.GetString(reader.GetOrdinal("dis_memo")),
                            dis_percent = reader.GetDouble(reader.GetOrdinal("dis_percent")),
                            act_code01 = reader.GetString(reader.GetOrdinal("act_code01")),
                            act_code02 = reader.GetString(reader.GetOrdinal("act_code02")),
                            act_code03 = reader.GetString(reader.GetOrdinal("act_code03")),
                            act_code04 = reader.GetString(reader.GetOrdinal("act_code04")),
                            act_code05 = reader.GetString(reader.GetOrdinal("act_code05")),
                            act_code06 = reader.GetString(reader.GetOrdinal("act_code06")),
                            act_code07 = reader.GetString(reader.GetOrdinal("act_code07")),


                        };
                    }
                }
            }
            return datas;
        }

            

    }
}