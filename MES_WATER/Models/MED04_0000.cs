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
    public class MED04_0000
    {
        //[Key]
        //[DisplayName("識別碼")]
        //public int med04_0000 { get; set; }

        [DisplayName("機台名稱")]
        public string mac_name { get; set; }


        [DisplayName("停機開始時間")]
        [StringLength(30, ErrorMessage = "長度最多{1}個字!")]
        public string time_s { get; set; }


        [DisplayName("停機結束時間")]
        [StringLength(30, ErrorMessage = "長度最多{1}個字!")]
        public string time_e { get; set; }


        [DisplayName("異常原因輸入")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string ins_date { get; set; }

        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}