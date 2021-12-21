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
    public class viewSTB01_ARepository
    {
        Comm comm = new Comm();

        /// <summary>
        /// 取得STB01_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO STB01_0000</returns>
        public viewSTB01_A GetDTO(string pTkCode)
        {
            viewSTB01_A data = new viewSTB01_A();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM STB01_0000";
            }
            else
            {
                sSql = "SELECT * FROM STB01_0000 where pro_code=@pro_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                // dapper
                data = con_db.QuerySingleOrDefault<viewSTB01_A>(sSql, new { pro_code = pTkCode});


                //SqlCommand sqlCommand = new SqlCommand(sSql);
                //sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@pro_code", pTkCode));
                //SqlDataReader reader = sqlCommand.ExecuteReader();

                //if (reader.HasRows)
                //{
                //    while (reader.Read())
                //    {
                //        datas = new viewSTB01_A
                //        {

                //            pro_code = reader.GetString(reader.GetOrdinal("pro_code")),
                //            pro_name = reader.GetString(reader.GetOrdinal("pro_name")),
                //            inv_name = reader.GetString(reader.GetOrdinal("inv_name")),
                //            pro_ename = reader.GetString(reader.GetOrdinal("pro_ename")),
                //            spc_code1 = reader.GetString(reader.GetOrdinal("spc_code1")),
                //            spc_code2 = reader.GetString(reader.GetOrdinal("spc_code2")),
                //            pro_spc = reader.GetString(reader.GetOrdinal("pro_spc")),
                //            pro_scpt1 = reader.GetString(reader.GetOrdinal("pro_scpt1")),
                //            pro_scpt2 = reader.GetString(reader.GetOrdinal("pro_scpt2")),
                //            pro_unit = reader.GetString(reader.GetOrdinal("pro_unit")),
                //            //------
                //            fct_code = reader.GetString(reader.GetOrdinal("fct_code")),
                //            kind_code = reader.GetString(reader.GetOrdinal("kind_code")),
                //            abc_level = reader.GetString(reader.GetOrdinal("abc_level")),
                //            barcode1 = reader.GetString(reader.GetOrdinal("barcode1")),
                //            barcode2 = reader.GetString(reader.GetOrdinal("barcode2")),
                //            ccc_code = reader.GetString(reader.GetOrdinal("ccc_code")),
                //            is_use = reader.GetString(reader.GetOrdinal("is_use")),
                //            mtp_mode = reader.GetString(reader.GetOrdinal("mtp_mode")),
                //            sal_mode = reader.GetString(reader.GetOrdinal("sal_mode")),
                //            com_mode = reader.GetString(reader.GetOrdinal("com_mode")),
                //            //------
                //            pro_cost = reader.GetDecimal(reader.GetOrdinal("pro_cost")),
                //            cmemo = reader.GetString(reader.GetOrdinal("cmemo")),
                //            sal_price_a = reader.GetDecimal(reader.GetOrdinal("sal_price_a")),
                //            sal_price_b = reader.GetDecimal(reader.GetOrdinal("sal_price_b")),
                //            sal_price_c = reader.GetDecimal(reader.GetOrdinal("sal_price_c")),
                //            sal_price_d = reader.GetDecimal(reader.GetOrdinal("sal_price_d")),
                //            sal_price_e = reader.GetDecimal(reader.GetOrdinal("sal_price_e")),
                //            typ1_code = reader.GetString(reader.GetOrdinal("typ1_code")),
                //            typ2_code = reader.GetString(reader.GetOrdinal("typ2_code")),

                //            //------
                //            det_code00 = reader.GetString(reader.GetOrdinal("det_code00")),
                //            det_code01 = reader.GetString(reader.GetOrdinal("det_code01")),
                //            det_code02 = reader.GetString(reader.GetOrdinal("det_code02")),
                //            det_code03 = reader.GetString(reader.GetOrdinal("det_code03")),
                //            det_code04 = reader.GetString(reader.GetOrdinal("det_code04")),
                //            det_code05 = reader.GetString(reader.GetOrdinal("det_code05")),
                //            det_code06 = reader.GetString(reader.GetOrdinal("det_code06")),
                //            det_code07 = reader.GetString(reader.GetOrdinal("det_code07")),
                //            det_code08 = reader.GetString(reader.GetOrdinal("det_code08")),
                //            det_code09 = reader.GetString(reader.GetOrdinal("det_code09")),
                //            //------
                //            opt_code00 = reader.GetString(reader.GetOrdinal("opt_code00")),
                //            opt_code01 = reader.GetString(reader.GetOrdinal("opt_code01")),
                //            opt_code02 = reader.GetString(reader.GetOrdinal("opt_code02")),
                //            opt_code03 = reader.GetString(reader.GetOrdinal("opt_code03")),
                //            opt_code04 = reader.GetString(reader.GetOrdinal("opt_code04")),
                //            opt_code05 = reader.GetString(reader.GetOrdinal("opt_code05")),
                //            opt_code06 = reader.GetString(reader.GetOrdinal("opt_code06")),
                //            opt_code07 = reader.GetString(reader.GetOrdinal("opt_code07")),
                //            opt_code08 = reader.GetString(reader.GetOrdinal("opt_code08")),
                //            opt_code09 = reader.GetString(reader.GetOrdinal("opt_code09")),
                //            //------
                //            exp_num = reader.GetInt32(reader.GetOrdinal("exp_num")),
                //            bcode_rate = reader.GetDecimal(reader.GetOrdinal("bcode_rate")),
                //            bcode_num = reader.GetDecimal(reader.GetOrdinal("bcode_num")),
                //            rel_url = reader.GetString(reader.GetOrdinal("rel_url")),
                //            usr_code = reader.GetString(reader.GetOrdinal("usr_code")),
                //            ins_date = reader.GetString(reader.GetOrdinal("ins_date")),
                //            ins_time = reader.GetString(reader.GetOrdinal("ins_time")),
                //            pro_cost2 = reader.GetDecimal(reader.GetOrdinal("pro_cost2")),
                //            pur_memo = reader.GetString(reader.GetOrdinal("pur_memo")),
                //            sal_tax_type = reader.GetString(reader.GetOrdinal("sal_tax_type")),
                //            //------
                //            dm_memo = reader.GetString(reader.GetOrdinal("dm_memo")),
                //            in_memo = reader.GetString(reader.GetOrdinal("in_memo")),
                //            labor_cost = reader.GetDecimal(reader.GetOrdinal("labor_cost")),
                //            mft_expense = reader.GetDecimal(reader.GetOrdinal("mft_expense")),
                //            last_mdy_date = reader.GetString(reader.GetOrdinal("last_mdy_date")),
                //            has_upd_date = reader.GetString(reader.GetOrdinal("has_upd_date")),
                //            pro_cost3 = reader.GetDecimal(reader.GetOrdinal("pro_cost3")),
                //            lock_price = reader.GetString(reader.GetOrdinal("lock_price")),
                //            pro_version = reader.GetString(reader.GetOrdinal("pro_version")),
                //            cer_memo = reader.GetString(reader.GetOrdinal("cer_memo")),
                //            //------
                //            cer_date = reader.GetString(reader.GetOrdinal("cer_date")),
                //            spl_code = reader.GetString(reader.GetOrdinal("spl_code")),
                //            scer_date = reader.GetString(reader.GetOrdinal("scer_date")),
                //            bcer_date = reader.GetString(reader.GetOrdinal("bcer_date")),
                //            tmp_code = reader.GetString(reader.GetOrdinal("tmp_code")),
                //            sale_flag = reader.GetString(reader.GetOrdinal("sale_flag")),
                //            //------

                //        };
                //    }
                //}
            }
            return data;
        }

    }
}