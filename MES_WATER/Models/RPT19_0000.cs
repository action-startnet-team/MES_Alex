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
    public class RPT19_0000
    {
        [DisplayName("工廠代號")]
        public string factory_code { get; set; }

        [DisplayName("工廠名稱")]
        public string factory_name { get; set; }

        [DisplayName("區域代號")]
        public string area_code { get; set; }

        [DisplayName("區域名稱")]
        public string area_name { get; set; }

        [DisplayName("線別代號")]
        public string line_code { get; set; }

        [DisplayName("線別名稱")]
        public string line_name { get; set; }

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