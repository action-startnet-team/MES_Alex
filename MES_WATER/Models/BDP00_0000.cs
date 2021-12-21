using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MES_WATER.Models
{
    public class BDP00_0000
    {
        [Key]
        [DisplayName("參數名稱")]
        public string par_name { get; set; }

        [DisplayName("參數值")]
        public string par_value { get; set; }

        [DisplayName("參數說明")]
        public string par_memo { get; set; }

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
