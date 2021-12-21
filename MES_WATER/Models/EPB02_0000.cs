using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class EPB02_0000
    {
        [Key]
        [DisplayName("表單代號")]        
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        
        public string epb_code { get; set; }

        [DisplayName("表單名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string epb_name { get; set; }

        [DisplayName("電子表單類別代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string epb_type_code { get; set; }

        [DisplayName("電子表單類別名稱")]
        public string epb_type_name { get; set; }

        [DisplayName("備註")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string cmemo {  get; set; }

        [DisplayName("是否使用")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string is_use { get; set; }

        [DisplayName("存檔類別")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string save_type { get; set; }

        [DisplayName("存檔目標")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string save_method { get; set; }


        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}