using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class QMB03_0200
    {
        [Key]
        [DisplayName("識別碼")]
        [HiddenInJqgrid]
        public int qmb03_0200 { get; set; }

        [DisplayName("檢驗記錄表代號")]
        public string qsheet_code { get; set; }

        [DisplayName("檢驗記錄表名稱")]
        public string qsheet_name { get; set; }

        [DisplayName("物料代號")]
        public string pro_code { get; set; }

        [DisplayName("物料名稱")]
        public string pro_name { get; set; }


        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}