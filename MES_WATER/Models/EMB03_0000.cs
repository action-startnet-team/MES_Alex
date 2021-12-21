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
    public class EMB03_0000
    {
        [Key]
        [DisplayName("廠商代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string sup_code { get; set; }

        [DisplayName("廠商名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(100, ErrorMessage = "長度最多為{1}個字!")]
        public string sup_name { get; set; }

        [DisplayName("是否使用")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多為{1}個字!")]
        public string is_use { get; set; }

        [DisplayName("備註")]
        [StringLength(100, ErrorMessage = "長度最多為{1}個字!")]
        public string cmemo { get; set; }

        [DisplayName("維修人員")]
        [StringLength(30, ErrorMessage = "長度最多為{1}個字!")]
        public string main_per_name { get; set; }

        [DisplayName("維修電話")]
        [StringLength(30, ErrorMessage = "長度最多為{1}個字!")]
        public string main_tel { get; set; }

        [DisplayName("廠商地址")]
        [StringLength(100, ErrorMessage = "長度最多為{1}個字!")]
        public string sup_add { get; set; }


        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }

    }
}