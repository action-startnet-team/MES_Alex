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
    public class MEB29_0100
    {
        [Key]
        [DisplayName("識別碼")]
        [HiddenInJqgrid]
        public int meb29_0100 { get; set; }

        [DisplayName("站別代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        [HiddenInJqgrid]
        public string station_code { get; set; }

        [DisplayName("人員代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多為{1}個字!")]
        public string usr_code { get; set; }

        [DisplayName("人員名稱")]
        [ReadOnly(true)]
        public string usr_name { get; set; }

        [DisplayName("控制類別")]
        [HiddenInJqgrid]
        public string control_type { get; set; }

        [DisplayName("控制類別名稱")]
        [HiddenInJqgrid]
        public string control_type_name { get; set; }


        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}