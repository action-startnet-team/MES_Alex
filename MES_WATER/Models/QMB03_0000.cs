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
    public class QMB03_0000
    {
        
        [Key]
        [DisplayName("檢驗記錄表代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(30, ErrorMessage = "長度最多{1}個字!")]
        public string qsheet_code { get; set; }

        [DisplayName("檢驗記錄表名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string qsheet_name { get; set; }

        [DisplayName("產品編號")]
        public string pro_code { get; set; }

        [DisplayName("產品名稱")]
        public string pro_name { get; set; }

        [DisplayName("檢驗記錄表說明")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string qsheet_memo { get; set; }

        [DisplayName("檢驗類型")]
        public string qsheet_type { get; set; }

        [DisplayName("檢驗等級")]
        public string qtest_level_code { get; set; }

        [DisplayName("檢驗等級名稱")]
        public string qtest_level_name { get; set; }

        [DisplayName("檢驗水準")]
        public string ins_level_code { get; set; }

        [DisplayName("檢驗水準名稱")]
        public string ins_level_name { get; set; }

        [DisplayName("製程代碼")]
        public string work_code { get; set; }

        [DisplayName("製程名稱")]
        public string work_name { get; set; }


        [DisplayName("表單代號")]
        public string epb_code { get; set; }

        [DisplayName("表單名稱")]
        public string epb_name { get; set; }

        [DisplayName("建立日期")]
        public string ins_date { get; set; }

        [DisplayName("建立時間")]
        public string ins_time { get; set; }

        [DisplayName("使用者編號")]
        public string usr_code { get; set; }

        [DisplayName("使用者名稱")]
        public string usr_name { get; set; }

        [DisplayName("版次")]
        public string version { get; set; }


        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}