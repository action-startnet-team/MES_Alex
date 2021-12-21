using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class RSS02_0100
    {
        [Key]
        [DisplayName("識別碼")]
        public int rss02_0100 { get; set; }

        [DisplayName("報表代號")]
        public string report_code { get; set; }

        [DisplayName("欄位編號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string field_code { get; set; }

        [DisplayName("欄位名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string field_name { get; set; }

        [DisplayName("顯示序號")]
        [Required(ErrorMessage = "請輸入{0}")]
        public int scr_no { get; set; }

        [DisplayName("控制項類別")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string ctr_type { get; set; }

        [DisplayName("控制項類別名稱")]
        [ReadOnly(true)]
        public string ctr_type_name { get; set; }

        [DisplayName("控制項預設值")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string ctr_default_value { get; set; }

        [DisplayName("資料型態")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string data_type { get; set; }

        [DisplayName("資料型態名稱")]
        [ReadOnly(true)]
        public string data_type_name { get; set; }

        [DisplayName("資料來源代號")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string select_code { get; set; }

        [DisplayName("資料來源名稱")]
        [ReadOnly(true)]
        public string select_name { get; set; }


        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }
    }
}