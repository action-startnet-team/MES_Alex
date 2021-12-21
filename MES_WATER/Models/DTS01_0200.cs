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
    public class DTS01_0200
    {
        [Key]
        [DisplayName("資料識別碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(32, ErrorMessage = "長度最多{1}個字!")]
        public string dts01_0200 { get; set; }

        [DisplayName("資料識別碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(32, ErrorMessage = "長度最多{1}個字!")]
        public string dts01_0100 { get; set; }

        [DisplayName("鍵值欄位名")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string Key_field_code { get; set; }

        [DisplayName("鍵值")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string key_field_value { get; set; }

        [DisplayName("執行類型")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string edit_type { get; set; }

        [DisplayName("回傳結果")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string result { get; set; }

        [DisplayName("回傳訊息")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(500, ErrorMessage = "長度最多{1}個字!")]
        public string message { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }
    }
}