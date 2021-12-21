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
    public class QMT04_0110
    {
        [Key]
        [DisplayName("識別碼")]
        public int qmt04_0110 { get; set; }

        [DisplayName("識別碼")]
        public int qmt04_0100 { get; set; }

        [DisplayName("檢驗項目代碼")]
        public string qtest_item_code { get; set; }

        [DisplayName("檢驗結果值")]
        public string qmt_value { get; set; }

        [DisplayName("判定結果")]
        public string is_ok { get; set; }

        [DisplayName("檢驗日期")]
        public string ins_date { get; set; }

        [DisplayName("檢驗時間")]
        public string ins_time { get; set; }

        [DisplayName("檢驗人員")]
        public string usr_code { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}