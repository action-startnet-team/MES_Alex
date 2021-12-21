using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MES_WATER.Models
{
    public class BDP21_0100
    {

        [Key]
        [DisplayName("資料代號")]
        public int bdp21_0100 { get; set; }

        [DisplayName("選項代碼")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string code_code { get; set; }

        [DisplayName("欄位代碼")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string field_code { get; set; }

        [DisplayName("欄位名稱")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string field_name { get; set; }

        [DisplayName("顯示序號")]
        public int scr_no { get; set; }

        [DisplayName("使用標記")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string is_use { get; set; }

        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}