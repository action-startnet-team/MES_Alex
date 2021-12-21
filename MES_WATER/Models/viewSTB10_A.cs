using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.ModelBinding;


namespace MES_WATER.Models
{
    public class viewSTB10_A
    {
        [Key]
        [DisplayName("廠商編號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(6, ErrorMessage = "長度最多{1}個字!")]
        public string sup_code { get; set; }

        [DisplayName("名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(60, ErrorMessage = "長度最多{1}個字!")]
        public string sup_name { get; set; }

        [DisplayName("簡稱")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string sup_hypo { get; set; }

        [DisplayName("供應性質")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string sup_kind { get; set; }

        [DisplayName("郵遞區號")]
        [StringLength(6, ErrorMessage = "長度最多{1}個字!")]
        public string zip_code { get; set; }

        [DisplayName("通訊地址")]
        [NotMapped]
        [StringLength(150, ErrorMessage = "長度最多{1}個字!")]
        public string att_add { get; set; }

        [DisplayName("統一編號")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string inv_idno { get; set; }

        [DisplayName("發票名稱")]
        [StringLength(60, ErrorMessage = "長度最多{1}個字!")]
        public string inv_title { get; set; }

        [DisplayName("電 話 一")]
        [StringLength(16, ErrorMessage = "長度最多{1}個字!")]
        public string sup_tel1 { get; set; }

        [DisplayName("電 話 二")]
        [StringLength(16, ErrorMessage = "長度最多{1}個字!")]
        public string sup_tel2 { get; set; }

        [DisplayName("傳真")]
        [StringLength(16, ErrorMessage = "長度最多{1}個字!")]
        public string sup_fax { get; set; }

        [DisplayName("電子郵件")]
        [StringLength(40, ErrorMessage = "長度最多{1}個字!")]
        public string sup_email { get; set; }

        [DisplayName("網址")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string sup_url { get; set; }


        [DisplayName("區域別")]
        [StringLength(4, ErrorMessage = "長度最多{1}個字!")]
        public string blk_code { get; set; }

        [DisplayName("負責人")]
        [StringLength(12, ErrorMessage = "長度最多{1}個字!")]
        public string led_name { get; set; }

        [DisplayName("群組碼")]
        [StringLength(4, ErrorMessage = "長度最多{1}個字!")]
        public string rea_code { get; set; }

        [DisplayName("行業別")]
        [StringLength(4, ErrorMessage = "長度最多{1}個字!")]
        public string bus_code { get; set; }

        [DisplayName("交易幣別")]
        [StringLength(4, ErrorMessage = "長度最多{1}個字!")]
        public string stv_code { get; set; }

        [DisplayName("是否使用")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string is_use { get; set; }

        [DisplayName("關連倉庫")]
        [StringLength(6, ErrorMessage = "長度最多{1}個字!")]
        public string sto_code { get; set; }

        [DisplayName("使用公司")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string com_mode { get; set; }

        [DisplayName("備　　註")]
        [StringLength(250, ErrorMessage = "長度最多{1}個字!")]
        public string cmemo { get; set; }

        [DisplayName("交易性質")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string tra_kind { get; set; }

        [DisplayName("發票地址")]
        [NotMapped]
        [StringLength(150, ErrorMessage = "長度最多{1}個字!")]
        public string inv_add { get; set; }

        [DisplayName("建立者")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string usr_code { get; set; }

        [DisplayName("建立日期")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string ins_date { get; set; }

        [DisplayName("建立時間")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string ins_time { get; set; }

        //[DisplayName("空項")]
        //[NotMapped]
        //[StringLength(250, ErrorMessage = "長度最多{1}個字!")]
        //public string pro_list { get; set; }
       
        [DisplayName("採購條文")]
        public string pur_memo { get; set; }

        [DisplayName("付款條件")]
        [StringLength(60, ErrorMessage = "長度最多{1}個字!")]
        public string pay_term { get; set; }

        [DisplayName("信用額度")]
        [StringLength(150, ErrorMessage = "長度最多{1}個字!")]
        public double crd_amt { get; set; }

        [DisplayName("付款模式")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string pay_type { get; set; }

        [DisplayName("週結標記")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string is_week { get; set; }

        [DisplayName("月結標記")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string is_month { get; set; }

        [DisplayName("進貨起算")]
        public Int32 cmonth0 { get; set; }

        [DisplayName("月 結 日")]
        public Int32 cday0 { get; set; }

        [DisplayName("月結起算")]
        [StringLength(40, ErrorMessage = "長度最多{1}個字!")]
        public Int32 cmonth1 { get; set; }

        [DisplayName("對 帳 日")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public Int32 cday1 { get; set; }

        [DisplayName("對帳起算")]
        public Int32 cmonth2 { get; set; }

        [DisplayName("付 票 日")]
        public Int32 cday2 { get; set; }

        [DisplayName("付票起算")]
        public Int32 cmonth3 { get; set; }

        [DisplayName("兌 現 日")]
        public Int32 cday3 { get; set; }

        [DisplayName("交易幣別")]
        public Int32 dis_days { get; set; }

        [DisplayName("是否使用")]
        public double dis_percent { get; set; }

        [DisplayName("關連倉庫")]
        [StringLength(60, ErrorMessage = "長度最多{1}個字!")]
        public string dis_memo { get; set; }

        [DisplayName("應付帳款")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string act_code01 { get; set; }

        [DisplayName("應付票據")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string act_code02 { get; set; }

        [DisplayName("預付貨款")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string act_code03 { get; set; }

        [DisplayName("空項")]
        [NotMapped]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string act_code04 { get; set; }

        [DisplayName("空項")]
        [NotMapped]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string act_code05 { get; set; }

        [DisplayName("空項")]
        [NotMapped]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string act_code06 { get; set; }

        [DisplayName("空項")]
        [NotMapped]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string act_code07 { get; set; }

        //[DisplayName("空項")]
        //[NotMapped]
        //[StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        //public string acb_name { get; set; }


        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }
    }
}