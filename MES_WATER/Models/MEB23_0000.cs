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
    public class MEB23_0000
    {
        [Key]
        [DisplayName("BOM代號")]
        public string bom_code { get; set; }

        [DisplayName("BOM名稱")]
        //[Required(ErrorMessage = "請輸入{0}")]
        [StringLength(40, ErrorMessage = "長度最多為{1}個字!")]
        public string bom_name { get; set; }

        [DisplayName("BOM版本")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        [RegularExpression(@"[0-9]*$", ErrorMessage = "必須輸入數字")]
        public string version { get; set; }

        [DisplayName("是否為預設BOM")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多為{1}個字!")]
        public string now_version { get; set; }

        [DisplayName("產品編號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多為{1}個字!")]
        public string pro_code { get; set; }

        [DisplayName("產品名稱")]
        public string pro_name { get; set; }

        [DisplayName("產品群組")]
        public string pro_type { get; set; }

        [DisplayName("最小生產數量")]
        public decimal pro_qty { get; set; }

        [DisplayName("預計產出量")]
        public decimal plan_qty { get; set; }

        [DisplayName("生產單位")]
        public string unit_code { get; set; }

        [DisplayName("生產模式")]
        public string in_type { get; set; }

        [DisplayName("備註")]
        [StringLength(100, ErrorMessage = "長度最多為{1}個字!")]
        public string cmemo { get; set; }

        [DisplayName("建立日期")]
        public string ins_date { get; set; }

        [DisplayName("建立時間")]
        public string ins_time { get; set; }

        [DisplayName("使用者編號")]
        public string usr_code { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}