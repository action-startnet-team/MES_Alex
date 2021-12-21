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
    public class WMT12_0100
    {
        [Key]
        [DisplayName("識別碼")]
        public int wmt12_0100 { get; set; }

        [DisplayName("備料單號")]
        public string par_code { get; set; }

        [DisplayName("料號")]
        public string pro_code { get; set; }

        [DisplayName("批號")]
        public string lot_no { get; set; }

        [DisplayName("容器編號")]
        public string loc_code { get; set; }

        [DisplayName("實際用料")]
        public decimal pro_qty { get; set; }

        [DisplayName("單位編號")]
        public string unit_code { get; set; }

        [DisplayName("投料狀態")]
        public string is_ok { get; set; }

        [DisplayName("建立日期")]
        public string ins_date { get; set; }

        [DisplayName("建立時間")]
        public string ins_time { get; set; }

        [DisplayName("使用者編號")]
        public string usr_code { get; set; }

        [DisplayName("機台編號")]
        public string mac_code { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}