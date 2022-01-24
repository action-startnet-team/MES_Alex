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
    public class RPT23_0000
    {
        [DisplayName("訂單單號")]
        public string DOC_NO { get; set; }

        [DisplayName("業務回覆")]
        public string UDF021 { get; set; }

        [DisplayName("生管回覆")]
        public string UDF022 { get; set; }

        [DisplayName("出貨回覆")]
        public string UDF023 { get; set; }


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