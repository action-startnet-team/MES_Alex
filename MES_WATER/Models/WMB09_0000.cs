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
    public class WMB09_0000
    {
        [Key]
        [DisplayName("標籤代碼")]
        [Required(ErrorMessage = "請輸入{0}")]

        public string label_code { get; set; }

        [DisplayName("標籤名稱")]
        public string label_name { get; set; }


        [DisplayName("標籤種類")]
        public string label_type { get; set; }

        [DisplayName("是否使用")]
        public string is_use { get; set; }

        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}