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
    public class MEB48_0000
    {
        [Key]
        [DisplayName("識別碼")]
        public int meb48_0000 { get; set; }

        [DisplayName("顯示順序")]
        [Required(ErrorMessage = "請輸入{0}")]
        public string scr_no { get; set; }

        [DisplayName("TABLE代碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        public string table_code { get; set; }

        [DisplayName("報工名稱")]
        public string table_name { get; set; }

        [DisplayName("欄位代碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        public string field_code { get; set; }

        [DisplayName("欄位名稱")]
        public string field_name { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}