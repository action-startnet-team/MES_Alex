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
    public class EMB11_0000
    {
        [Key]
        [DisplayName("保養周期編號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string cycle_code { get; set; }

        [DisplayName("保養周期名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多為{1}個字!")]
        public string cycle_name { get; set; }

        [DisplayName("月數")]
        [Required(ErrorMessage = "請輸入{0}")]
        public int month { get; set; }

        [DisplayName("日數")]
        [Required(ErrorMessage = "請輸入{0}")]
        public int days { get; set; }

        [DisplayName("備註")]
        [StringLength(100, ErrorMessage = "長度最多為{1}個字!")]
        public string cmemo { get; set; }


        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }

    }
}