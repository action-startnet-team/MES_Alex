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
    public class BDP30_0200
    {
        [Key]
        [DisplayName("識別碼")]
        public int bdp30_0200 { get; set; }

        [DisplayName("程式代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多為{1}個字!")]
        public string prg_code { get; set; }

        [DisplayName("程式名稱")]
        public string prg_name { get; set; }

        [DisplayName("顥示序號")]
        [Required(ErrorMessage = "請輸入{0}")]
        public int scr_no  { get; set; }

        [DisplayName("資料表代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(30, ErrorMessage = "長度最多為{1}個字!")]
        public string table_code { get; set; }

        [DisplayName("欄位代號")]
        [StringLength(30, ErrorMessage = "長度最多為{1}個字!")]
        [Required(ErrorMessage = "請輸入{0}")]
        public string field_code { get; set; }

        [DisplayName("欄位名稱")]
        [StringLength(30, ErrorMessage = "長度最多為{1}個字!")]
        [Required(ErrorMessage = "請輸入{0}")]
        public string field_name { get; set; }


        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}