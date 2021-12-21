using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class BDP30_0000
    {
        [Key]
        [DisplayName("識別碼")]
        public int bdp30_0000 { get; set; }

        [DisplayName("程式代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多為{1}個字!")]
        public string prg_code { get; set; }

        [DisplayName("使用者帳號")]
        [StringLength(20, ErrorMessage = "長度最多為{1}個字!")]
        public string usr_code { get; set; }

        [DisplayName("View代號")]
        [StringLength(30, ErrorMessage = "長度最多為{1}個字!")]
        [Required(ErrorMessage = "請輸入{0}")]
        public string view_code { get; set; }

        [DisplayName("欄位索引")]
        [Required(ErrorMessage = "請輸入{0}")]
        public int col_index { get; set; }

        [DisplayName("欄位寬度")]
        [Required(ErrorMessage = "請輸入{0}")]
        public int col_width { get; set; }

        [DisplayName("是否能刪除(控制用)")]
        [NotMapped]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [NotMapped]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}