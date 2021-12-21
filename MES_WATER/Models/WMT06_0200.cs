using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class WMT06_0200
    {
        [Key]
        [DisplayName("識別碼")]
        public int wmt06_0200 { get; set; }

        [DisplayName("備料單號")]
        public string prepare_code { get; set; }

        [DisplayName("派工單號")]
        public string wrk_code { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }

    }



}