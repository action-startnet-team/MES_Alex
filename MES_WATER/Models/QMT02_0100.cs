using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class QMT02_0100
    {
        [Key]
        [DisplayName("識別碼")]
        public int qmt02_0100 { get; set; }

        [DisplayName("檢驗記錄編號")]
        public string qmt_code { get; set; }

        [DisplayName("資料代碼")]
        public string datacode { get; set; }

        [DisplayName("檢驗結果值")]
        public string qmt_value { get; set; }

        [DisplayName("判定結果")]
        public string is_ok { get; set; }

        [DisplayName("檢驗日期")]
        public string ins_date { get; set; }

        [DisplayName("檢驗時間")]
        public string ins_time { get; set; }

        [DisplayName("檢驗人員")]
        public string usr_code { get; set; }


        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }
    }
}