using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MES_WATER.Models
{
    public class MAC01_0000
    {
        [Key]
        [DisplayName("機台代碼")]
        [Required]
        [StringLength(20)]
        public string mac_code { get; set; }

        [DisplayName("機台名稱")]
        [StringLength(50)]
        public string mac_name { get; set; }

        [DisplayName("計畫工作時間")]
        public decimal wrk_time { get; set; }

        [DisplayName("計畫工作產量")]
        public decimal wrk_qty { get; set; }

        [DisplayName("稼動率期望值")]
        public decimal u_limit { get; set; }

        public string img_url { get; set; }
    }
}