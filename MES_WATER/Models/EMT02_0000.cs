using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MES_WATER.Models
{
    public class EMT02_0000
    {
        [Key]
        [DisplayName("設備點檢表")]
        public string dev_check_code { get; set; }

        [DisplayName("設備點檢日期")]
        [Required(ErrorMessage = "請輸入{0}")]
        public string dev_check_date { get; set; }

        [DisplayName("設備代碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        public string dev_code { get; set; }

        [DisplayName("設備名稱")]
        public string dev_name { get; set; }

        [DisplayName("備註")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string cmemo { get; set; }

        [DisplayName("點檢日期")]
        public string ins_date { get; set; }

        [DisplayName("點檢時間")]
        public string ins_time { get; set; }

        [DisplayName("點檢人員")]
        public string usr_code { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}