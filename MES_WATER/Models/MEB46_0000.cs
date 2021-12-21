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
    public class MEB46_0000
    {
        [Key]
        [DisplayName("除外工時代碼")]        
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string except_code { get; set; }

        [DisplayName("除外工時名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(40, ErrorMessage = "長度最多{1}個字!")]
        public string except_name { get; set; }

        [DisplayName("除外工時說明")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string cmemo {  get; set; }

        [DisplayName("除外耗時(分)")]
        public decimal except_cost { get; set; }

        [DisplayName("除外工時類型")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string except_type { get; set; }

        [DisplayName("除外工時類型名稱")]
        public string except_type_name { get; set; }


        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}