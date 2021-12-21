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
    public class MEB15_0100
    {
        [Key]
        [DisplayName("識別碼")]
        [HiddenInJqgrid]
        public int meb15_0100 { get; set; }

        [DisplayName("機器代號")]
        [HiddenInJqgrid]
        public string mac_code { get; set; }

        [DisplayName("儲位代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        public string loc_code { get; set; }

        [DisplayName("儲位名稱")]
        [ReadOnly(true)]
        public string loc_name { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}