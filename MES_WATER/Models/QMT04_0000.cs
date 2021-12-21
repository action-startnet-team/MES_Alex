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
    public class QMT04_0000
    {
        [Key]
        [DisplayName("檢驗記錄表編號")]
        public string qmt_code { get; set; }

        [DisplayName("入/出庫識別碼")]
        public string wmt0200 { get; set; }

        [DisplayName("關聯單別")]
        [Required(ErrorMessage = "請輸入{0}")]
        public string rel_type { get; set; }

        [DisplayName("關聯單號")]
        [Required(ErrorMessage = "請輸入{0}")]
        public string rel_code { get; set; }

        [DisplayName("物料號碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        public string pro_code { get; set; }

        [DisplayName("物料名稱")]
        public string pro_name { get; set; }

        [DisplayName("批號")]
        public string lot_no { get; set; }

        [DisplayName("順序")]
        public int scr_no { get; set; }

        [DisplayName("版次")]
        public string version { get; set; }

        [DisplayName("是否允收")]
        public string is_rec { get; set; }

        [DisplayName("是否允收名稱")]
        public string is_rec_name { get; set; }

        [DisplayName("特採原因代號")]
        public string spur_code { get; set; }

        [DisplayName("特採原因名稱")]
        public string spur_name { get; set; }

        [DisplayName("檢驗水準代號")]
        public string ins_level_code { get; set; }

        [DisplayName("檢驗水準名稱")]
        public string ins_level_name { get; set; }

        [DisplayName("開立日期")]
        public string ins_date { get; set; }

        [DisplayName("開立時間")]
        public string ins_time { get; set; }

        [DisplayName("開立人員")]
        public string usr_code { get; set; }

        [DisplayName("開立人員名稱")]
        public string usr_name { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}