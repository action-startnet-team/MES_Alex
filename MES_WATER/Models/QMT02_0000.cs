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
    public class QMT02_0000
    {
        [DisplayName("特採")]
        public string is_ok_P { get; set; }

        [Key]
        [DisplayName("識別碼")]
        public string qmt02_0000 { get; set; }

        [DisplayName("檢驗記錄編號")]
        public string qmt_code { get; set; }

        [DisplayName("關聯單別")]
        public string rel_type { get; set; }

        [DisplayName("關聯單號")]
        public string rel_code { get; set; }

        [DisplayName("物料代號")]
        public string pro_code { get; set; }

        [DisplayName("物料名稱")]
        public string pro_name { get; set; }

        [DisplayName("批號")]
        public string lot_no { get; set; }

        [DisplayName("順序")]
        public int scr_no { get; set; }

        [DisplayName("資料代碼")]
        public string datacode { get; set; }

        [DisplayName("檢驗項目代碼")]
        public string qtest_item_code { get; set; }

        [DisplayName("檢驗項目名稱")]
        public string qtest_item_name { get; set; }

        [DisplayName("檢驗結果值")]
        public string qmt_value { get; set; }

        [DisplayName("判定結果")]
        public string is_ok { get; set; }

        [DisplayName("判定結果名稱")]
        public string is_ok_name { get; set; }

        [DisplayName("檢驗日期")]
        public string ins_date { get; set; }

        [DisplayName("檢驗時間")]
        public string ins_time { get; set; }

        [DisplayName("檢驗人員代號")]
        public string usr_code { get; set; }

        [DisplayName("檢驗人員名稱")]
        public string usr_name { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}