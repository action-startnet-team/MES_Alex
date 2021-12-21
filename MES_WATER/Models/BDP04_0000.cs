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
    public class BDP04_0000
    {
        [Key]        
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        [DisplayName("程式代號")]
        public string prg_code { get; set; }

        [DisplayName("程式名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string prg_name { get; set; }

        [DisplayName("系統代號")]
        [StringLength(4, ErrorMessage = "長度最多{1}個字!")]
        [NotMapped]
        public string sys_code {  get; set; }

        [DisplayName("使用標記")]
        //[StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        //[Required(ErrorMessage = "請輸入{0}")]
        public string is_use { get; set; }

        [DisplayName("權限字串")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string limit_str { get; set; }


        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}