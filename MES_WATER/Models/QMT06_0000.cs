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
    public class QMT06_0000
    {
        [Key]
        [DisplayName("製程檢驗記錄表編號")]
        public string oqm_code { get; set; }

        [DisplayName("報工入庫識別碼")]
        public int med09_0000 { get; set; }

        [DisplayName("製令號碼")]
        [Required(ErrorMessage = "請輸入{0}")]

        public string mo_code { get; set; }


        [DisplayName("物料號碼")]
        [Required(ErrorMessage = "請輸入{0}")]

        public string pro_code { get; set; }

        [DisplayName("物料名稱")]
        public string pro_name { get; set; }

        [DisplayName("批號")]
        public string lot_no { get; set; }

        [DisplayName("版次")]
        public string version { get; set; }

        [DisplayName("開立日期")]
        public string ins_date { get; set; }

        [DisplayName("開立時間")]
        public string ins_time { get; set; }

        [DisplayName("開立人員")]
        public string usr_code { get; set; }

        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}