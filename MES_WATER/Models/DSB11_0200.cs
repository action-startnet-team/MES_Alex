using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MES_WATER.Models
{
    public class DSB11_0200
    {
        [Key]
        [DisplayName("製令單號")]
        public string mo_code { get; set; }

        [DisplayName("產品品號")]
        public string pro_code { get; set; }

        [DisplayName("OK數量")]
        public string ok_qty { get; set; }

        [DisplayName("NG數量")]
        public string ng_qty { get; set; }

        [DisplayName("生產時間")]
        public string pro_time { get; set; }

        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        [NotMapped]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        [NotMapped]
        public string can_update { get; set; }
    }
}