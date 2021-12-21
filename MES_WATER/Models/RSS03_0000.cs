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
    public class RSS03_0000
    {
        [Key]
        [DisplayName("報表資料群組碼")]
        public string report_group_code { get; set; }

        [DisplayName("報表代號")]
        public string report_code { get; set; }

        [DisplayName("報表名稱")]
        public string report_name { get; set; }

        [DisplayName("出表日期")]
        public string ins_date { get; set; }

        [DisplayName("出表時間")]
        public string ins_time { get; set; }

        [DisplayName("出表人員")]
        public string usr_code { get; set; }

        [DisplayName("結案日期")]
        public string end_date { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}