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
    public class EPB04_0000
    {
        [Key]
        [DisplayName("審核設定代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string review_code { get; set; }

        [DisplayName("電子表單代號")]
        //[Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多為{1}個字!")]
        [NotMapped]
        public string epb_code { get; set; }

        [DisplayName("電子表單名稱")]
        public string epb_name { get; set; }

        [DisplayName("報表代號")]
        //[Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多為{1}個字!")]
        [NotMapped]
        public string report_code { get; set; }

        [DisplayName("報表名稱")]
        public string report_name { get; set; }

        [DisplayName("審核設定說明")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string review_memo { get; set; }

        [DisplayName("是否使用")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多為{1}個字!")]
        public string is_use { get; set; }

        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}