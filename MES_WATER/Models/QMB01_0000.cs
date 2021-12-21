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
    public class QMB01_0000
    {
        [Key]
        [DisplayName("檢驗類別代碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        public string qtest_type_code { get; set; }

        [DisplayName("檢驗類別名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多為{1}個字!")]
        public string qtest_type_name { get; set; }

        [DisplayName("檢驗類型說明")]
        [StringLength(100, ErrorMessage = "長度最多為{1}個字!")]
        public string qtest_type_memo { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}