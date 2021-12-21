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
    public class QMB03_0100
    {
        [Key]
        [DisplayName("識別碼")]
        [HiddenInJqgrid]
        public int qmb03_0100 { get; set; }

        [DisplayName("檢驗記錄表代號")]
        [HiddenInJqgrid]
        public string qsheet_code { get; set; }

        [DisplayName("檢驗項目代碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        public string qtest_item_code { get; set; }

        [DisplayName("檢驗項目名稱")]
        [ReadOnly(true)]
        public string qtest_item_name { get; set; }

        [DisplayName("檢驗資料類型")]
        [Required(ErrorMessage = "請輸入{0}")]
        public string qtest_item_type { get; set; }

        [DisplayName("檢驗資料類型名稱")]
        [ReadOnly(true)]
        public string qtest_item_type_name { get; set; }

        [DisplayName("順序")]
        [Required(ErrorMessage = "請輸入{0}")]
        [HiddenInJqgrid]
        public string scr_no { get; set; }

        [DisplayName("資料代碼")]
        [ReadOnly(true)]
        public string datacode { get; set; }

        [DisplayName("規格上限")]
        public decimal qtest_up { get; set; }

        [DisplayName("規格下限")]
        public decimal qtest_down { get; set; }

        [DisplayName("檢測工具")]
        public string tool_code { get; set; }

        [DisplayName("工具名稱")]
        [ReadOnly(true)]
        public string tool_name { get; set; }

        [DisplayName("單位代碼")]
        public string unit_code { get; set; }

        [DisplayName("單位名稱")]
        [ReadOnly(true)]
        public string unit_name { get; set; }

        [DisplayName("檢測頻率")]
        public string qtest_rate { get; set; }

        [DisplayName("檢驗標準說明")]
        public string qtest_memo { get; set; }

        [DisplayName("製程代碼")]
        public string work_code { get; set; }

        [DisplayName("製程名稱")]
        [ReadOnly(true)]
        public string work_name { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}