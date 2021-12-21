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
    public class MEB15_0200
    {
        [Key]
        [DisplayName("識別碼")]
        [HiddenInJqgrid]
        public int meb15_0200 { get; set; }

        [DisplayName("機台代號")]
        [HiddenInJqgrid]
        public string mac_code { get; set; }

        [DisplayName("設備代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        public string dev_code { get; set; }

        [DisplayName("設備名稱")]
        public string dev_name { get; set; }

        [DisplayName("備註")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string des_memo { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}