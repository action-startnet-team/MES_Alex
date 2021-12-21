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
    public class MEB03_0000
    {
        [Key]
        [DisplayName("批號類型代碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        public string lot_type_code { get; set; }

        [DisplayName("批號類型名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多為{1}個字!")]
        public string lot_type_name { get; set; }

        [DisplayName("批號類型說明")]
        [StringLength(100, ErrorMessage = "長度最多為{1}個字!")]
        public string lot_type_memo { get; set; }

        [DisplayName("日期前置碼格式")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(2, ErrorMessage = "長度最多為{1}個字!")]
        public string lot_lead_type { get; set; }

        [DisplayName("日期前置碼格式名稱")]
        public string lot_lead_type_name { get; set; }

        [DisplayName("前置碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(6, ErrorMessage = "長度最多為{1}個字!")]
        public string lot_lead_code { get; set; }

        [DisplayName("流水碼長度")]
        [Required(ErrorMessage = "請輸入{0}")]
        public int lot_no_qty { get; set; }

        [DisplayName("編碼格式長度")]
        [Required(ErrorMessage = "請輸入{0}")]
        public int lot_no_length { get; set; }




        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}