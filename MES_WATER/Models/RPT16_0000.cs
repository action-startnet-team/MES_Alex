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
    public class RPT16_0000
    {

        [DisplayName("工單代號")]
        public string wrk_code { get; set; }

        [DisplayName("產品編號")]
        public string pro_code { get; set; }

        [DisplayName("產品名稱")]
        public string pro_name { get; set; }

        [DisplayName("製程代號")]
        public string work_code { get; set; }

        [DisplayName("製程名稱")]
        public string work_name { get; set; }

        [DisplayName("計畫數量")]
        public string plan_qty { get; set; }

        [DisplayName("良品量")]
        public string ok_qty { get; set; }

        [DisplayName("不良品量")]
        public string ng_qty { get; set; }

        [DisplayName("排程生產日期")]
        public string sch_date_s { get; set; }

        [DisplayName("排程生產時間")]
        public string sch_time_s { get; set; }

        [DisplayName("是否能刪除(控制用)")]
        [NotMapped]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [NotMapped]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}