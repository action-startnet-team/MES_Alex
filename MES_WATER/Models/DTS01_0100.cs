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
    public class DTS01_0100
    {
        [Key]
        [DisplayName("資料識別碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(32, ErrorMessage = "長度最多{1}個字!")]
        public string dts01_0100 { get; set; }

        [DisplayName("資料識別碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(32, ErrorMessage = "長度最多{1}個字!")]
        public string dts01_0000 { get; set; }

        [DisplayName("回傳的明細table名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string table_name { get; set; }

        [DisplayName("單據內容")]
        [Required(ErrorMessage = "請輸入{0}")]
        public string con_response { get; set; }

        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }
    }
}