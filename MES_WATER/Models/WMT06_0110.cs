using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class WMT06_0110
    {
        [Key]
        [DisplayName("明細識別碼")]
        public int wmt06_0110 { get; set; }

        [DisplayName("備料明細識別碼")]
        public string wmt06_0100 { get; set; }

        [DisplayName("物料代碼")]
        public string pro_code { get; set; }

        [DisplayName("批號")]
        public string lot_no { get; set; }

        [DisplayName("備料量")]
        public decimal pro_qty { get; set; }

        [DisplayName("單位")]
        public string pro_unit { get; set; }

        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }

    }
}