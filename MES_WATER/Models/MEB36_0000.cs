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
    public class MEB36_0000
    {
        [Key]
        [DisplayName("不良現象類型代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string ng_type_code { get; set; }

        [DisplayName("不良現象類型名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(40, ErrorMessage = "長度最多{1}個字!")]
        public string ng_type_name { get; set; }
        
        [DisplayName("備註")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string cmemo { get; set; }


        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}