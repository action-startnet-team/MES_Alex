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
    public class MEM01_0000
    {
        [Key]
        [DisplayName("識別碼")]
        public int mem01_0000 { get; set; }

        [DisplayName("製令號碼")]
        public string mo_code { get; set; }

        [DisplayName("製程代碼")]
        public string work_code { get; set; }
        
        [DisplayName("站別代號")]
        public string station_code { get; set; }

        [DisplayName("機台代號")]
        public string mac_code { get; set; }
        
        [DisplayName("工作開始時間")]
        public string work_time_s { get; set; }

        [DisplayName("工作結束時間")]
        public string work_time_e { get; set; }

        [DisplayName("警報型態名稱")]
        public string alm_type_name { get; set; }

        [DisplayName("良品量")]
        public decimal ok_qty { get; set; }

        [DisplayName("良品單位")]
        public string ok_unit { get; set; }

        [DisplayName("不良品量")]
        public decimal ng_qty { get; set; }

        [DisplayName("不良品單位")]
        public string ng_unit { get; set; }

        [DisplayName("工作時間(秒)")]
        public int work_sec { get; set; }


        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }

    }
}