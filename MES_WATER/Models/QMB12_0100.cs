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
    public class QMB12_0100
    {
        [Key]
        [DisplayName("識別碼")]
        [HiddenInJqgrid]
        public string qmb12_0100 { get; set; }

        [DisplayName("抽樣表代碼")]
        [HiddenInJqgrid]
        public string aql_code { get; set; }

        [DisplayName("批量下限")]
        public decimal aql_down { get; set; }

        [DisplayName("批量上限")]
        public decimal aql_up { get; set; }

        [DisplayName("建議抽樣數")]
        public decimal sample_qty { get; set; }

        [DisplayName("嚴重缺陷")]
        public decimal ng_a { get; set; }

        [DisplayName("主要缺陷")]
        public decimal ng_b { get; set; }

        [DisplayName("次要缺陷")]
        public decimal ng_c { get; set; }


        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}