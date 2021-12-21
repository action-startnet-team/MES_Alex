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
    public class WMT0100
    {
        [Key]
        [DisplayName("識別碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        public int wmt0100 { get; set; }

        [DisplayName("單別")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(4, ErrorMessage = "長度最多{1}個字!")]
        public string rel_type { get; set; }

        [DisplayName("單號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string rel_code { get; set; }

        [DisplayName("序號")]
        [Required(ErrorMessage = "請輸入{0}")]
        public int scr_no { get; set; }

        [DisplayName("入/出庫類別")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string ins_type { get; set; }

        //[DisplayName("單據日期")]
        //[Required(ErrorMessage = "請輸入{0}")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        //public DateTime sto_date { get; set; }

        [DisplayName("單據日期")]
        [Required(ErrorMessage = "請輸入{0}")]
        public string sto_date { get; set; }

        //[DisplayName("單據時間")]
        //[Required(ErrorMessage = "請輸入{0}")]
        //[HiddenInJqgrid]
        //public string sto_time { get; set; }

        [DisplayName("客戶")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string cus_code { get; set; }

        [DisplayName("料號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string pro_code { get; set; }

        [DisplayName("品名")]
        public string pro_name { get; set; }

        [DisplayName("批號")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string lot_no { get; set; }

        [DisplayName("倉庫編號")]
        [StringLength(6, ErrorMessage = "長度最多{1}個字!")]
        public string sto_code { get; set; }

        [DisplayName("倉庫名稱")]
        public string sto_name { get; set; }

        [DisplayName("儲位代號")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string loc_code { get; set; }

        [DisplayName("儲位名稱")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string loc_name { get; set; }

        [DisplayName("單據數量")]
        [Required(ErrorMessage = "請輸入{0}")]
        public decimal pro_qty { get; set; }


        [DisplayName("單位數量")]
        [HiddenInJqgrid]
        public decimal print_qty { get; set; }

        [DisplayName("已處理量")]
        public decimal res_qty { get; set; }

        [DisplayName("列印標記")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string is_print { get; set; }

        [DisplayName("是否使用")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string is_use { get; set; }

        [DisplayName("是否異常")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string is_error { get; set; }


        [DisplayName("建立人員")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string ins_user { get; set; }

        [DisplayName("建立日期")]
        public string ins_date { get; set; }

        //[DisplayName("建立時間")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        //public DateTime ins_date { get; set; }

        [DisplayName("建立時間")]
        [HiddenInJqgrid]
        public string ins_time { get; set; }

        [DisplayName("更新日期")]
        public string upd_date { get; set; }

        //[DisplayName("更新時間")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        //public DateTime upd_date { get; set; }

        [DisplayName("更新時間")]
        [HiddenInJqgrid]
        public string upd_time { get; set; }

        [DisplayName("廠商編號")]
        public string sup_code { get; set; }

        [DisplayName("廠商名稱")]
        public string sup_name { get; set; }

        [DisplayName("廠商批號")]
        public string sup_lot_no { get; set; }

        [DisplayName("製造日期")]
        public string mft_date { get; set; }

        [DisplayName("有效日期")]
        public string exp_date { get; set; }

        [DisplayName("單位")]
        public string unit_code { get; set; }

        [DisplayName("單位名稱")]
        public string unit_name { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}