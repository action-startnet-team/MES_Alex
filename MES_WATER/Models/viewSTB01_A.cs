using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MES_WATER.Models
{
    public class viewSTB01_A
    {
        [Key]
        [DisplayName("產品編號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string pro_code { get; set; }

        [DisplayName("名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(30, ErrorMessage = "長度最多{1}個字!")]
        public string pro_name { get; set; }

        [DisplayName("發票品名")]
        [StringLength(30, ErrorMessage = "長度最多{1}個字!")]
        public string inv_name { get; set; }

        [DisplayName("英文名稱")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string pro_ename { get; set; }

        [DisplayName("規格小類編號")]
        [StringLength(6, ErrorMessage = "長度最多{1}個字!")]
        public string spc_code1 { get; set; }

        [DisplayName("規格編號尾碼")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string spc_code2 { get; set; }

        [DisplayName("規格描述")]
        [StringLength(250, ErrorMessage = "長度最多{1}個字!")]
        public string pro_spc { get; set; }

        [DisplayName("產品描述一")]
        [StringLength(250, ErrorMessage = "長度最多{1}個字!")]
        public string pro_scpt1 { get; set; }

        [DisplayName("產品描述二")]
        [StringLength(250, ErrorMessage = "長度最多{1}個字!")]
        public string pro_scpt2 { get; set; }

        [DisplayName("單　　位")]
        [StringLength(6, ErrorMessage = "長度最多{1}個字!")]
        public string pro_unit { get; set; }

        [DisplayName("品牌編號")]
        [StringLength(4, ErrorMessage = "長度最多{1}個字!")]
        public string fct_code { get; set; }

        [DisplayName("性質")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(2, ErrorMessage = "長度最多{1}個字!")]
        public string kind_code { get; set; }

        [DisplayName("ABC分類")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string abc_level { get; set; }


        [DisplayName("條 碼 一")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string barcode1 { get; set; }

        [DisplayName("條 碼 二")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string barcode2 { get; set; }

        [DisplayName("C.C.C NO")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string ccc_code { get; set; }

        [DisplayName("使用")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string is_use { get; set; }

        [DisplayName("進貨模式")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string mtp_mode { get; set; }

        [DisplayName("銷貨模式")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string sal_mode { get; set; }

        [DisplayName("使用公司")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string com_mode { get; set; }

        [DisplayName("標準成本")]
        public decimal pro_cost { get; set; }

        [DisplayName("備　　註")]
        [StringLength(250, ErrorMessage = "長度最多{1}個字!")]
        public string cmemo { get; set; }

        [DisplayName("售價A(最高)")]
        public decimal sal_price_a { get; set; }

        [DisplayName("售價B")]
        public decimal sal_price_b { get; set; }

        [DisplayName("售價C")]
        public decimal sal_price_c { get; set; }

        [DisplayName("售價D")]
        public decimal sal_price_d { get; set; }

        [DisplayName("售價E(最低)")]
        public decimal sal_price_e { get; set; }

        [DisplayName("大類編號")]
        [StringLength(4, ErrorMessage = "長度最多{1}個字!")]
        public string typ1_code { get; set; }

        [DisplayName("小類編號")]
        [StringLength(6, ErrorMessage = "長度最多{1}個字!")]
        public string typ2_code { get; set; }




        [DisplayName("規格項目一")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string det_code00 { get; set; }

        [DisplayName("規格項目二")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string det_code01 { get; set; }

        [DisplayName("規格項目三")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string det_code02 { get; set; }

        [DisplayName("規格項目四")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string det_code03 { get; set; }

        [DisplayName("規格項目五")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string det_code04 { get; set; }

        [DisplayName("規格項目六")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string det_code05 { get; set; }

        [DisplayName("規格項目七")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string det_code06 { get; set; }

        [DisplayName("規格項目八")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string det_code07 { get; set; }

        [DisplayName("規格項目九")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string det_code08 { get; set; }

        [DisplayName("規格項目十")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string det_code09 { get; set; }

        [DisplayName("規格選項一")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string opt_code00 { get; set; }

        [DisplayName("規格選項二")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string opt_code01 { get; set; }

        [DisplayName("規格選項三")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string opt_code02 { get; set; }

        [DisplayName("規格選項四")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string opt_code03 { get; set; }

        [DisplayName("規格選項五")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string opt_code04 { get; set; }

        [DisplayName("規格選項六")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string opt_code05 { get; set; }

        [DisplayName("規格選項七")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string opt_code06 { get; set; }

        [DisplayName("規格選項八")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string opt_code07 { get; set; }

        [DisplayName("規格選項九")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string opt_code08 { get; set; }

        [DisplayName("規格選項十")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string opt_code09 { get; set; }

        [DisplayName("效期警示天數")]
        public Int32 exp_num { get; set; }

        [DisplayName("條碼乘率")]
        public decimal bcode_rate { get; set; }

        [DisplayName("加印張數")]
        public decimal bcode_num { get; set; }

        [DisplayName("相關網址")]
        [StringLength(250, ErrorMessage = "長度最多{1}個字!")]
        public string rel_url { get; set; }

        [DisplayName("建立者")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string usr_code { get; set; }

        [DisplayName("建立日期")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string ins_date { get; set; }

        [DisplayName("建立時間")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string ins_time { get; set; }

        [DisplayName("其他成本")]
        public decimal pro_cost2 { get; set; }

        [DisplayName("其他成本2")]
        public decimal pro_cost3 { get; set; }

        [DisplayName("採購備註")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string pur_memo { get; set; }

        [DisplayName("售價稅別")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string sal_tax_type { get; set; }


        [DisplayName("DM標示參考")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string dm_memo { get; set; }


        [DisplayName("內部報價參考")]
        [StringLength(1000, ErrorMessage = "長度最多{1}個字!")]
        public string in_memo { get; set; }


        [DisplayName("空項")]
        public decimal labor_cost { get; set; }

        [DisplayName("空項")]
        public decimal mft_expense { get; set; }


        [DisplayName("空項")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string last_mdy_date { get; set; }

        [DisplayName("空項")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string has_upd_date { get; set; }


      


        [DisplayName("單價鎖定")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string lock_price { get; set; }

        [DisplayName("空項")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string pro_version { get; set; }

        [DisplayName("空項")]
        [StringLength(250, ErrorMessage = "長度最多{1}個字!")]
        public string cer_memo { get; set; }

        [DisplayName("空項")]
        [StringLength(250, ErrorMessage = "長度最多{1}個字!")]
        public string cer_date { get; set; }


        [DisplayName("空項")]
        [StringLength(12, ErrorMessage = "長度最多{1}個字!")]
        public string spl_code { get; set; }

        [DisplayName("空項")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string scer_date { get; set; }

        [DisplayName("空項")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string bcer_date { get; set; }

        [DisplayName("空項")]
        [StringLength(6, ErrorMessage = "長度最多{1}個字!")]
        public string tmp_code { get; set; }

        [DisplayName("空項")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string sale_flag { get; set; }

        [DisplayName("商品標示")]
        [StringLength(350, ErrorMessage = "長度最多{1}個字!")]
        public string label { get; set; }

        [DisplayName("NCC")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string ncc { get; set; }

        [DisplayName("BSMI")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string bsmi { get; set; }

        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }
    }
}