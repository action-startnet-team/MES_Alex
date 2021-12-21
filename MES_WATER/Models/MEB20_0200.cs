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
    public class MEB20_0200
    {
        [Key]
        [DisplayName("識別碼")]
        [HiddenInJqgrid]
        public int meb20_0200 { get; set; }

        [DisplayName("產品代號")]
        [HiddenInJqgrid]
        public string pro_code { get; set; }

        [DisplayName("基本單位")]
        [Required(ErrorMessage = "請輸入{0}")]
        public string unit_code_base { get; set; }

        [DisplayName("基本單位名稱")]
        public string unit_name_base { get; set; }

        [DisplayName("基本量")]
        public decimal base_qty { get; set; }

        [DisplayName("換算單位")]
        public string unit_code_chg { get; set; }

        [DisplayName("換算單位名稱")]
        public string unit_name_chg { get; set; }

        [DisplayName("換算量")]
        public decimal chg_qty { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}