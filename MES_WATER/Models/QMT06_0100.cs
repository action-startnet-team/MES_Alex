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
    public class QMT06_0100
    {
        
        [Key]
        [DisplayName("識別碼")]
        [HiddenInJqgrid]
        public int qmt06_0100 { get; set; }

        [DisplayName("製程檢驗記錄表編號")]
        [HiddenInJqgrid]
        public string oqm_code { get; set; }
        
        [DisplayName("檢驗項目代碼")]
        public string qtest_item_code { get; set; }

        [DisplayName("檢驗項目名稱")]
        [ReadOnly(true)]
        public string qtest_item_name { get; set; }

        [DisplayName("順序")]
        public string scr_no { get; set; }

        [DisplayName("規格上限")]
        public decimal qtest_up { get; set; }

        [DisplayName("規格下限")]
        public decimal qtest_down { get; set; }

        [DisplayName("檢驗資料類型")]
        [ReadOnly(true)]
        public string qtest_item_type { get; set; }

        [DisplayName("檢驗資料類型名稱")]
        [ReadOnly(true)]
        public string qtest_item_type_name { get; set; }


        [DisplayName("判定結果")]
        public string is_ok { get; set; }

        [DisplayName("檢驗日期")]
        [ReadOnly(true)]
        public string ins_date { get; set; }

        [DisplayName("檢驗時間")]
        [ReadOnly(true)]
        public string ins_time { get; set; }

        [DisplayName("檢驗人員")]
        [ReadOnly(true)]
        public string usr_code { get; set; }

        [DisplayName("檢驗人員名稱")]
        [ReadOnly(true)]
        public string usr_name { get; set; }


        [DisplayName("抽樣數量(總數)")]
        [ReadOnly(true)]
        public decimal sample_sum_qty { get; set; }


        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}