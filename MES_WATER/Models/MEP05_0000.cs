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
    public class MEP05_0000
    {
        [Key]
        [DisplayName("識別碼")]
        public string mep05_0000 { get; set; }

        [DisplayName("製令號碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多為{1}個字!")]
        public string mo_code { get; set; }

        [DisplayName("工單號碼")]
        [StringLength(30, ErrorMessage = "長度最多為{1}個字!")]
        public string wrk_code { get; set; }

        [DisplayName("製程代碼")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        public string work_code { get; set; }

        [DisplayName("製程名稱")]
        public string work_name { get; set; }

        [DisplayName("站別代號")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        public string station_code { get; set; }

        [DisplayName("站別名稱")]
        public string station_name { get; set; }

        [DisplayName("機台代號")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        public string mac_code { get; set; }

        [DisplayName("機台名稱")]
        public string mac_name { get; set; }

        [DisplayName("工作時間(秒)")]
        public decimal work_second { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}