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
    public class WMT08_0000
    {
        [Key]
        [DisplayName("盤點單號")]
        public string inventory_code { get; set; }

        [DisplayName("盤點日期")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        public string inventory_date { get; set; }

        [DisplayName("盤點種類")]
        public string inventory_type { get; set; }

        [DisplayName("盤點種類名稱")]
        public string inventory_type_name { get; set; }

        [DisplayName("備註")]
        [StringLength(50, ErrorMessage = "長度最多為{1}個字!")]
        public string cmemo { get; set; }

        [DisplayName("盤點狀態")]
        public string is_ok { get; set; }

        [DisplayName("建立日期")]
        public string ins_date { get; set; }

        [DisplayName("建立時間")]
        public string ins_time { get; set; }

        [DisplayName("使用者編號")]
        public string usr_code { get; set; }

        [DisplayName("使用者名稱")]
        public string usr_name { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}