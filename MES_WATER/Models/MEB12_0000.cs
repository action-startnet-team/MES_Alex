using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class MEB12_0000
    {
        [Key]
        [DisplayName("線別代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string line_code { get; set; }

        [DisplayName("線別名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string line_name { get; set; }

        //[DisplayName("區域代號")]
        //[Required(ErrorMessage = "請輸入{0}")]
        //[StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        //public string area_code { get; set; }

        //[DisplayName("區域名稱")]
        //public string area_name { get; set; }

        //[DisplayName("工廠代號")]
        //public string factory_code { get; set; }

        //[DisplayName("工廠名稱")]
        //public string factory_name { get; set; }

        //[DisplayName("備註")]
        //[StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        //public string cmemo { get; set; }


        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}