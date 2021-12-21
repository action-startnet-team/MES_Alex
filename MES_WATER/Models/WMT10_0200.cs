using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MES_WATER.Models
{
    public class WMT10_0200
    {
        [Key]
        [DisplayName("識別碼")]
        public string wmt10_0200 { get; set; }

        [DisplayName("配料單號")]
        public string ing_code { get; set; }

        [DisplayName("順序")]
        public string scr_no { get; set; }

        [DisplayName("容器編號")]
        public string loc_code { get; set; }


    }
}