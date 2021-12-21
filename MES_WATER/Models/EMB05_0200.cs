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
    public class EMB05_0200
    {
        [Key]
        [DisplayName("識別碼")]
        [HiddenInJqgrid]
        public int emb05_0200 { get; set; }

        [DisplayName("保養設備群組代碼")]
        [HiddenInJqgrid]
        public string mai_type_code { get; set; }

        [DisplayName("保養設備群組名稱")]
        [HiddenInJqgrid]
        public string mai_type_name { get; set; }

        [DisplayName("故障原因編號")]
        public string fault_reason_code { get; set; }

        [DisplayName("故障原因編號")]
        public string fault_reason_name { get; set; }


        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }

    }
}