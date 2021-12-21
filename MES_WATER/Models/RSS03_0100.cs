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
    public class RSS03_0100
    {
        [Key]
        [DisplayName("識別碼")]
        [HiddenInJqgrid]
        public int rss03_0100 { get; set; }

        [DisplayName("報表資料群組碼")]
        [HiddenInJqgrid]
        public string report_group_code { get; set; }

        [DisplayName("識別碼")]
        public string epb_key_value { get; set; }

        [DisplayName("表單代號")]
        public string epb_code { get; set; }

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