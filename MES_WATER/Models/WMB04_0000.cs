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
    public class WMB04_0000
    {
        [Key]
        [DisplayName("識別碼")]
        public int wmb04_0000 { get; set;  }

        [DisplayName("產品編號")]

        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string pro_code { get; set; }

        [DisplayName("產品名稱")]
        public string pro_name { get; set; }

        [DisplayName("基本單位")]
 
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string unit_code_base { get; set; }

        [DisplayName("基本量")]
        public decimal base_rate { get; set; }

        [DisplayName("換算單位")]

        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string unit_code_chg { get; set; }

        [DisplayName("換算量")]
        public decimal chg_rate { get; set; }


        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}