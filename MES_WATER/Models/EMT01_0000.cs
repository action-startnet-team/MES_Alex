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
    public class EMT01_0000
    {
        [Key]
        [DisplayName("保養計畫編號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多為{1}個字!")]
        public string maintain_code { get; set; }

        [DisplayName("保養計畫名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多為{1}個字!")]
        public string maintain_name { get; set; }

        [DisplayName("設備代碼")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        public string dev_code { get; set; }

        [DisplayName("設備名稱")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        public string dev_name { get; set; }

        [DisplayName("備註")]
        [StringLength(100, ErrorMessage = "長度最多為{1}個字!")]
        public string cmemo { get; set; }

        [DisplayName("建立日期")]
        public string ins_date { get; set; }

        [DisplayName("建立時間")]
        public string ins_time { get; set; }

        [DisplayName("使用者編號")]
        public string usr_code { get; set; }

        [DisplayName("使用者名稱")]
        public string usr_name { get; set; }

        [DisplayName("是否使用")]
        public string is_use { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}