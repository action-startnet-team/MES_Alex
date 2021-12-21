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
    public class RPT20_0000
    {
        [DisplayName("製程代碼")]
        public string work_code { get; set; }

        [DisplayName("製程名稱")]
        public string work_name { get; set; }

        [DisplayName("站別代碼")]
        public string station_code { get; set; }

        [DisplayName("站別名稱")]
        public string station_name { get; set; }

        [DisplayName("機器代碼")]
        public string mac_code { get; set; }

        [DisplayName("機器名稱")]
        public string mac_name { get; set; }


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