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
    public class WMB06_0000
    {
        [Key]
        [DisplayName("產品編號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string pro_code { get; set; }

        [DisplayName("產品名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string pro_name { get; set; }

        [DisplayName("產品規格")]
        [StringLength(200, ErrorMessage = "長度最多{1}個字!")]
        public string pro_spc { get; set; }

        [DisplayName("產品類別")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string pro_type { get; set; }

        [DisplayName("產品類別名稱")]
        public string pro_type_name { get; set; }

        [DisplayName("基本用量單位")]
        public string unit_code { get; set; }

        [DisplayName("單位名稱")]
        public string unit_name { get; set; }

        [DisplayName("最小單位用量")]
        public decimal unit_qty_min { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}