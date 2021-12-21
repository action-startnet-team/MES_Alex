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
    public class viewSTB08_A
    {
        [Key]
        [DisplayName("客戶編號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(6, ErrorMessage = "長度最多{1}個字!")]
        public string cus_code { get; set; }

        [DisplayName("名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [NotMapped]
        [StringLength(60, ErrorMessage = "長度最多{1}個字!")]
        public string cus_name { get; set; }

        [DisplayName("簡稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string cus_hypo { get; set; }

        [DisplayName("統一編號")]        
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string inv_idno { get; set; }

        [DisplayName("發票名稱")]
        [NotMapped]
        [StringLength(60, ErrorMessage = "長度最多{1}個字!")]
        public string inv_title { get; set; }

        [DisplayName("郵遞區號")]
        [NotMapped]
        [StringLength(6, ErrorMessage = "長度最多{1}個字!")]
        public string zip_code { get; set; }

        [DisplayName("通訊地址")]
        [NotMapped]
        [StringLength(150, ErrorMessage = "長度最多{1}個字!")]
        public string att_add { get; set; }


        [DisplayName("電 話 一")]
        [StringLength(16, ErrorMessage = "長度最多{1}個字!")]
        public string cus_tel1 { get; set; }

        [DisplayName("電 話 二")]
        [StringLength(16, ErrorMessage = "長度最多{1}個字!")]
        public string cus_tel2 { get; set; }

        [DisplayName("傳真")]
        [StringLength(16, ErrorMessage = "長度最多{1}個字!")]
        public string cus_fax { get; set; }

        [DisplayName("電子郵件")]
        [NotMapped]
        [StringLength(40, ErrorMessage = "長度最多{1}個字!")]
        public string cus_email { get; set; }

        [DisplayName("網址")]
        [NotMapped]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string cus_url { get; set; }

        [DisplayName("區域別")]
        [NotMapped]
        [StringLength(4, ErrorMessage = "長度最多{1}個字!")]
        public string blk_code { get; set; }

        [DisplayName("負 責 人")]
        [StringLength(12, ErrorMessage = "長度最多{1}個字!")]
        public string led_name { get; set; }

        [DisplayName("群組碼")]
        [StringLength(4, ErrorMessage = "長度最多{1}個字!")]
        public string rea_code { get; set; }

        [DisplayName("行業別")]
        [StringLength(24, ErrorMessage = "長度最多{1}個字!")]
        public string bus_code { get; set; }

        [DisplayName("是否使用")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string is_use { get; set; }

        [DisplayName("關連倉庫")]
        [NotMapped]
        [StringLength(6, ErrorMessage = "長度最多{1}個字!")]
        public string sto_code { get; set; }

        [DisplayName("售價等級(A-Z)")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string pri_lel { get; set; }

        [DisplayName("業務員")]
        [StringLength(6, ErrorMessage = "長度最多{1}個字!")]
        public string per_code { get; set; }

        [DisplayName("有效期限")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string exp_date { get; set; }

        [DisplayName("生    日")]
        [NotMapped]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string bir_date { get; set; }

        [DisplayName("性    別")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string cus_sex { get; set; }

        [DisplayName("會員等級")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string ent_lel { get; set; }

        [DisplayName("累積交易額")]
        public decimal bonus_amt { get; set; }

        [DisplayName("使用公司")]
        [NotMapped]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string com_mode { get; set; }

        [DisplayName("備註")]
        [NotMapped]
        [StringLength(250, ErrorMessage = "長度最多{1}個字!")]
        public string cmemo { get; set; }

        [DisplayName("交易性質")]
        [NotMapped]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string tra_kind { get; set; }

        [DisplayName("發票地址")]
        [NotMapped]
        [StringLength(150, ErrorMessage = "長度最多{1}個字!")]
        public string inv_add { get; set; }

        [DisplayName("帳款歸屬")]
        [StringLength(6, ErrorMessage = "長度最多{1}個字!")]
        public string acc_rel { get; set; }

        [DisplayName("帳款地址")]
        [NotMapped]
        [StringLength(150, ErrorMessage = "長度最多{1}個字!")]
        public string acc_add { get; set; }

        [DisplayName("郵遞區號")]
        [StringLength(6, ErrorMessage = "長度最多{1}個字!")]
        public string acc_zip { get; set; }

        [DisplayName("空項")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        [NotMapped]
        public string unit_group { get; set; }

        [DisplayName("建立者")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string usr_code { get; set; }

        [DisplayName("建立日期")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string ins_date { get; set; }

        [DisplayName("建立時間")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string ins_time { get; set; }

        [DisplayName("空項")]
        [NotMapped]
        public double cap_amt { get; set; }

        [DisplayName("空項")]
        [NotMapped]
        public double tra_amt { get; set; }

        [DisplayName("空項")]
        [NotMapped]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string cre_date { get; set; }

        [DisplayName("空項")]
        [NotMapped]
        public double per_num { get; set; }

        [DisplayName("空項")]
        [NotMapped]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string not_trans { get; set; }

        [DisplayName("空項")]
        [NotMapped]
        [StringLength(250, ErrorMessage = "長度最多{1}個字!")]
        public string pro_list { get; set; }

        [DisplayName("空項")]
        [NotMapped]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string hol_code { get; set; }

        [DisplayName("空項")]
        [NotMapped]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string is_stamp { get; set; }

        [DisplayName("空項")]
        [NotMapped]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string is_accounts { get; set; }

        [DisplayName("空項")]
        [NotMapped]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string spl_accounts { get; set; }

        [DisplayName("空項")]
        [NotMapped]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string route_code { get; set; }

        [DisplayName("空項")]
        [NotMapped]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string branch_no { get; set; }

        [DisplayName("空項")]
        [NotMapped]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string head_no { get; set; }

        [DisplayName("空項")]
        [NotMapped]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string web_01 { get; set; }

        [DisplayName("空項")]
        [NotMapped]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string web_02 { get; set; }

        [DisplayName("空項")]
        [NotMapped]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string web_03 { get; set; }

        [DisplayName("空項")]
        [NotMapped]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string web_04 { get; set; }

        [DisplayName("空項")]
        [NotMapped]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string web_05 { get; set; }

        [DisplayName("空項")]
        [NotMapped]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string web_06 { get; set; }

        [DisplayName("空項")]
        [NotMapped]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string web_07 { get; set; }

        [DisplayName("空項")]
        [NotMapped]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string web_08 { get; set; }

        [DisplayName("空項")]
        [NotMapped]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string web_09 { get; set; }

        [DisplayName("空項")]
        [NotMapped]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string web_10 { get; set; }

        [DisplayName("空項")]
        [NotMapped]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string per_idno { get; set; }

        [DisplayName("交易幣別")]
        [NotMapped]
        [StringLength(4, ErrorMessage = "長度最多{1}個字!")]
        public string stv_code { get; set; }

        [DisplayName("發票聯式")]
        [NotMapped]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string inv_type { get; set; }

        [DisplayName("付款條件")]
        [StringLength(60, ErrorMessage = "長度最多{1}個字!")]
        public string pay_term { get; set; }

        [DisplayName("信用額度")]
        [NotMapped]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public double crd_amt { get; set; }

        [DisplayName("收款模式")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string rec_type { get; set; }

        [DisplayName("月結標記")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string is_month { get; set; }


        [DisplayName("月結日    銷貨")]
        public int cmonth0 { get; set; }

        [DisplayName("月 結 日")]
        public int cday0 { get; set; }

        [DisplayName("對帳日    月結")]
        public int cmonth1 { get; set; }

        [DisplayName("對 帳 日")]
        public int cday1 { get; set; }

        [DisplayName("收票日    月結")]
        public int cmonth2 { get; set; }

        [DisplayName("收 票 日")]
        public int cday2 { get; set; }

        [DisplayName("兌現日    月結")]
        public int cmonth3 { get; set; }

        [DisplayName("兌 現 日")]
        public int cday3 { get; set; }

        [DisplayName("應收帳款")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string act_code01 { get; set; }

        [DisplayName("應收票據")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string act_code02 { get; set; }

        [DisplayName("預收貨款")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string act_code03 { get; set; }

        [DisplayName("銀行存款")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string act_code04 { get; set; }

        [DisplayName("應付佣金")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string act_code05 { get; set; }

        [DisplayName("佣金支出")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string act_code06 { get; set; }


        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string act_code07 { get; set; }


        [DisplayName("現金折讓最大天數")]
        public int max_dis { get; set; }

        [DisplayName("銷貨條文")]
        [StringLength(250, ErrorMessage = "長度最多{1}個字!")]
        public string sal_memo { get; set; }

        [DisplayName("入會日期")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string ent_date { get; set; }





        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }
    }
}