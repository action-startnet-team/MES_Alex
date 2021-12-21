using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class WMB01_0000
    {
        [Key]
        [DisplayName("倉庫編號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        public string sto_code { get; set; }

        [DisplayName("倉庫名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多為{1}個字!")]
        public string sto_name { get; set; }

        [DisplayName("倉庫地址")]
        [StringLength(100, ErrorMessage = "長度最多為{1}個字!")]
        public string sto_add { get; set; }

        [DisplayName("倉庫類別")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多為{1}個字!")]
        public string sto_type { get; set; }

        [DisplayName("倉庫類別名稱")]
        public string sto_type_name { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }
    }
}