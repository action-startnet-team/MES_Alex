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
    public class MEB13_0000
    {
        [Key]
        [DisplayName("班別代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string class_code { get; set; }

        [DisplayName("班別名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string class_name { get; set; }

        [DisplayName("備註")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string cmemo { get; set; }

        [DisplayName("開始時間")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(5, ErrorMessage = "長度最多{1}個字!")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: HH:mm}")]
        public string time_s { get; set; }

        [DisplayName("結束時間")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(5, ErrorMessage = "長度最多{1}個字!")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: HH:mm}")]
        public string time_e { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}