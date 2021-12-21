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
    public class MEB45_0000
    {
        [Key]
        [DisplayName("停機原因代碼")]        
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string stop_code { get; set; }

        [DisplayName("停機原因名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(40, ErrorMessage = "長度最多{1}個字!")]
        public string stop_name { get; set; }

        [DisplayName("停機原因說明")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string cmemo {  get; set; }

        [DisplayName("停機類型")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string stop_type { get; set; }

        [DisplayName("停機類型名稱")]
        public string stop_type_name { get; set; }


        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}