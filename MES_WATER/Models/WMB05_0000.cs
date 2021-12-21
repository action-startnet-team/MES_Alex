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
    public class WMB05_0000
    {
        [Key]
        [DisplayName("單別號碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string rel_type { get; set; }

        [DisplayName("單別名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(30, ErrorMessage = "長度最多{1}個字!")]
        public string rel_name { get; set; }

        [DisplayName("入/出庫別")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string io_type { get; set; }

        [DisplayName("入/出庫別名稱")]
        public string io_type_name { get; set; }

        [DisplayName("移轉入庫單別")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string i_rel_type { get; set; }

        [DisplayName("移轉出庫單別")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string o_rel_type { get; set; }

        [DisplayName("ERP單別")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string erp_code { get; set; }

        [DisplayName("單別種類")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string rel_kind { get; set; }

        [DisplayName("單別種類名稱")]
        public string rel_kind_name { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}