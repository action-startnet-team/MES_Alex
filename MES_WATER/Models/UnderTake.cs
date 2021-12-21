using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MES_WATER.Models;
using MES_WATER.Repository;
using System.Data;
using System.Linq.Dynamic;
using System.Web.Security;
using System.Reflection;
using Newtonsoft.Json;

namespace MES_WATER.Models
{
    public class UnderTake
    {
        //共用函式庫
        Comm comm = new Comm();
        //取得單號
        WebReference.WmsApi ws = new WebReference.WmsApi();
        GetModelValidation gmv = new GetModelValidation();

        //取得承接的資料庫語法
        public string Get_TableDataStr(string pTable) {
            string sTableDataStr = "";

            switch (pTable)
            {
                case "STT01_0100":
                    sTableDataStr = "select  STB01_0000.pro_name as cpro_name,STB01_0000.pro_unit as cpro_unit,STB01_0000.pro_spc as cpro_spc,STB01_0000.pro_scpt1 as cpro_scpt1 " +
                                    "       ,STB01_0000.sal_price_a,STT01_0000.*,STT01_0100.*,STT01_0100.POR_code as csor_code,STT01_0000.POR_date as csor_date,STT01_0100.STT01_0100 as csor_serial " +
                                    "       ,(pro_qty-res_qty) as sor_qty,(pro_qty-res_qty) as sor_qty2,STT01_0000.cus_code,STT01_0000.cus_name,STT01_0000.cus_tel1,STT01_0000.cus_tel2,STT01_0000.out_add  " +
                                    "  from ((STT01_0100 " +
                                    "   left join STT01_0000 on STT01_0100.POR_code=STT01_0000.POR_code) " +
                                    "   left join STB01_0000 on STT01_0100.pro_code=STB01_0000.pro_code) " +
                                    "   left join BDM06_0000 on STT01_0000.POR_code=BDM06_0000.tk_code  ";
                    break;
                case "STT02_0100":
                    //採購單
                    //多加了一個小計欄位(pro_qty-res_qty) * pro_price as sum_price
                    sTableDataStr = "select  STB01_0000.pro_name as cpro_name,STB01_0000.pro_unit as cpro_unit,STB01_0000.pro_spc as cpro_spc,STB01_0000.pro_scpt1 as cpro_scpt1 " +
                                    "      , STB01_0000.sal_price_a,STT02_0000.*,STT02_0100.*,STT02_0100.PUR_code as csor_code,STT02_0000.PUR_date as csor_date,STT02_0100.STT02_0100 as csor_serial " +
                                    "      ,(pro_qty - res_qty) as sor_qty,(pro_qty - res_qty) as sor_qty2,rea_price,sup_code,pay_term,com_code,STT02_0000.cmemo,(pro_qty-res_qty) * pro_price as sum_price   " +
                                    "  from((STT02_0100 " +
                                    "   left join STT02_0000 on STT02_0100.PUR_code = STT02_0000.PUR_code) " +
                                    "   left join STB01_0000 on STT02_0100.pro_code = STB01_0000.pro_code)    " +
                                    "   left join BDM06_0000 on STT02_0000.PUR_code = BDM06_0000.tk_code  ";
                    break;
                case "STT03_0100":
                    //進貨單
                    //多加了一個小計欄位(pro_qty-res_qty) * pro_price as sum_price
                    sTableDataStr = "select STB01_0000.pro_name as cpro_name,STB01_0000.pro_unit as cpro_unit,STB01_0000.pro_spc as cpro_spc, " +
                                    "       STB01_0000.pro_scpt1 as cpro_scpt1,STB01_0000.sal_price_a,STT03_0000.*,STT03_0100.*, " +
                                    "       STT03_0100.MTP_code as csor_code,STT03_0000.MTP_date as csor_date,STT03_0100.STT03_0100 as csor_serial " +
                                    "       ,(pro_qty - res_qty) as sor_qty,(pro_qty - res_qty) as sor_qty2,sup_code,com_code,k_inv_no,inv_code,STT03_0000.tax_type,(pro_qty-res_qty) * pro_price as sum_price " +
                                    "  from((STT03_0100 " +
                                    "   left join STT03_0000 on STT03_0100.MTP_code = STT03_0000.MTP_code) " +
                                    "   left join STB01_0000 on STT03_0100.pro_code = STB01_0000.pro_code) " +
                                    "   left join BDM06_0000 on STT03_0000.MTP_code = BDM06_0000.tk_code";
                    break;
                case "STT18_0100":
                    //借出單
                    //多加了一個小計欄位(pro_qty-res_qty) * pro_price as sum_price
                    sTableDataStr = "select  STB01_0000.pro_name as cpro_name,STB01_0000.pro_unit as cpro_unit,STB01_0000.pro_spc as cpro_spc " +
                                    "       ,STB01_0000.pro_scpt1 as cpro_scpt1,STB01_0000.sal_price_a,STT18_0000.*,STT18_0100.* " +
                                    "       ,STT18_0100.IOA_code as csor_code,STT18_0000.IOA_date as csor_date,STT18_0100.STT18_0100 as csor_serial, " +
                                    "       (pro_qty - res_qty) as sor_qty,(pro_qty - res_qty) as sor_qty2,cst_code,cst_type,(pro_qty-res_qty) * pro_price as sum_price " +
                                    "  from((STT18_0100 " +
                                    "   left join STT18_0000 on STT18_0100.IOA_code = STT18_0000.IOA_code) " +
                                    "   left join STB01_0000 on STT18_0100.pro_code = STB01_0000.pro_code) " +
                                    "   left join BDM06_0000 on STT18_0000.IOA_code = BDM06_0000.tk_code  ";
                    break;
                case "STT22_0000":
                    sTableDataStr = "select STB01_0000.pro_name as cpro_name,STB01_0000.pro_unit as cpro_unit,STB01_0000.pro_spc as cpro_spc,STB01_0000.pro_scpt1 as cpro_scpt1,STT22_0000.*,STT22_0000.INQ_code as csor_code,STT22_0000.INQ_date as csor_date,pro_qty as sor_qty,pro_qty as sor_qty2 " +
                                    "  from (STT22_0000 " +
                                    "   left join STB01_0000 on STT22_0000.pro_code=STB01_0000.pro_code) " +
                                    "   left join BDM06_0000 on STT22_0000.INQ_code=BDM06_0000.tk_code ";
                    break;
                
            }
            //考量網頁速度，先顯示500筆
            sTableDataStr = sTableDataStr.Replace("select", "select top 500 ");
            return sTableDataStr;
        }


        //取得來源作業的資料庫名稱(明細)
        public string Chk_LinkTable(string pLink) {
            string sTableName = "";

            switch (pLink) {
                case "STT02_A":
                    sTableName = "STT02_0100";
                    break;
                case "STT03_A":
                    sTableName = "STT03_0100";                    
                    break;
                case "STT04_A":
                    sTableName = "STT04_0100";
                    break;
                case "STT09_A":
                    sTableName = "STT09_0100";
                    break;
                case "STT20_A":
                    sTableName = "STT20_0100";
                    break;
            }
            return sTableName;
        }

        //取得來源作業的資料庫鍵值欄位(明細)
        public string Chk_LinkKey(string pLink)
        {
            string sTableKey = "";

            switch (pLink)
            {
                case "STT02_A":
                    sTableKey = "stt02_0100";
                    break;
                case "STT03_A":
                    sTableKey = "stt03_0100";
                    break;
                case "STT04_A":
                    sTableKey = "stt04_0100";
                    break;
                case "STT09_A":
                    sTableKey = "stt09_0100";
                    break;
                case "STT20_A":
                    sTableKey = "stt20_0100";
                    break;
            }
            return sTableKey;
        }

        /// <summary>
        /// 取得承接語法的欄位資訊
        /// </summary>
        /// <param name="pTableDataStr">承接語法</param>
        /// <param name="pKeyField">鍵值欄位</param>
        /// <param name="pKeyValue">鍵值</param>
        /// <param name="pField">取回的欄位</param>
        /// <returns></returns>
        public string Get_SorData(string pTableDataStr,string pKeyField, string pKeyValue, string pField)
        {
            string sSql = pTableDataStr +
                          " where "+ pKeyField + " = '" + pKeyValue + "'";
            return comm.DataFieldToStr(sSql, pField);
        }



        public int Get_NextScrNo(string pTable,string pKeyField, string pKeyCode)
        {
            int iNo;
            iNo = comm.Get_AutoIntMax(pTable, "scr_no", "And "+ pKeyField + " like '" + pKeyCode + "%'") + 1;

            return iNo;
        }

        //取得所有人員
        public string Get_Usr()
        {
            string sSql = "select * from STB18_0000" +
                          " order by per_code";
            return comm.DataFieldToStr(sSql, "per_code");
        }

        //取得供應商編號
        public string Get_SupCode() {
            string sSql = "select * from STB10_0000 " +
                          " order by sup_code ";
            return comm.DataFieldToStr(sSql, "sup_code");
        }

        //取得客戶編號
        public string Get_CusCode()
        {
            string sSql = "select * from STB08_0000 " +
                          " order by cus_code ";
            return comm.DataFieldToStr(sSql, "cus_code");
        }

        //取得資料表的所有資料的key值
        public string Get_ALLDataKey(string pTable, string pKeyField) {
            string sSql = "select * from "+ pTable + " " +
                          " order by "+ pKeyField + " ";
            return comm.DataFieldToStr(sSql, pKeyField);
        }

        /// <summary>
        /// 檢查取回量
        /// </summary>
        /// <param name="pCanQty">可取量</param>
        /// <param name="pGetQty">取回量</param>
        /// <returns></returns>
        public decimal Chk_SorQty(decimal pCanQty, decimal pGetQty)
        {
            //超過可取量則回傳可取量
            decimal sReturn = 0;
            if (pGetQty <= pCanQty)
            {
                sReturn = pGetQty;
            }
            else
            {
                sReturn = pCanQty;
            }
            return sReturn;
        }       



    }
}