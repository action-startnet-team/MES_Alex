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
    public class WMT12_0000
    {
        [Key]
        [DisplayName("備料單號")]
        public string par_code { get; set; }

        [DisplayName("備料日期")]
        public string par_date { get; set; }

        [DisplayName("備料種類")]
        public string par_type { get; set; }

        [DisplayName("工單編號")]
        public string mo_code { get; set; }

        [DisplayName("生產批次")]
        public string pro_lot { get; set; }

        [DisplayName("料號")]
        public string pro_code { get; set; }

        [DisplayName("順序")]
        public string scr_no { get; set; }

        [DisplayName("數量")]
        public decimal pro_qty { get; set; }

        [DisplayName("己備數量")]
        public decimal par_qty { get; set; }

        [DisplayName("己投數量")]
        public decimal res_qty { get; set; }

        [DisplayName("單位編號")]
        public string unit_code { get; set; }

        [DisplayName("備註")]
        public string cmemo { get; set; }

        [DisplayName("備料狀態")]
        public string is_par { get; set; }

        [DisplayName("投料狀態")]
        public string is_sto_in { get; set; }

        [DisplayName("建立日期")]
        public string ins_date { get; set; }

        [DisplayName("建立時間")]
        public string ins_time { get; set; }

        [DisplayName("使用者編號")]
        public string usr_code { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}