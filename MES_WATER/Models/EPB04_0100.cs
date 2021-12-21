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
    public class EPB04_0100
    {
        [Key]
        [DisplayName("資料識別碼")]
        public int epb04_0100 { get; set; }

        [DisplayName("審核設定代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多為{1}個字!")]
        public string review_code { get; set; }

        [DisplayName("使用者代碼")]        
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string usr_code { get; set; }

        [DisplayName("使用者名稱")]
        public string usr_name { get; set; }

        [DisplayName("層級")]
        [ReadOnly(true)]
        public int review_level { get; set; }

        [DisplayName("可否決行")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string is_manager { get; set; }
      
        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}