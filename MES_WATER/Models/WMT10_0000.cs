using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MES_WATER.Models
{
    public class WMT10_0000
    {
        [Key]
        [DisplayName("配料單號")]
        public string inq_code { get; set; }

        [DisplayName("配料日期")]
        public string inq_date { get; set; }

        [DisplayName("配料種類")]
        public string inq_type { get; set; }

        [DisplayName("工單編號")]
        public string mo_code { get; set; }

        [DisplayName("料號")]
        public string pro_code { get; set; }

        [DisplayName("生產批次")]
        public string pro_lot { get; set; }

        [DisplayName("數量")]
        public decimal pro_qty { get; set; }

        [DisplayName("單位編號")]
        public string unit_code { get; set; }

        [DisplayName("備註")]
        public string cmemo { get; set; }

        [DisplayName("配料狀態")]
        public string is_ok { get; set; }

        [DisplayName("建立日期")]
        public string ins_date { get; set; }

        [DisplayName("建立時間")]
        public string ins_time { get; set; }

        [DisplayName("使用者編號")]
        public string usr_code { get; set; }

        [DisplayName("備料狀態")]
        public string is_par { get; set; }

        [DisplayName("投料狀態")]
        public string is_in { get; set; }

    }

}