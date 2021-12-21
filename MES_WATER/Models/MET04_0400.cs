using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class MET04_0400
    {
        [Key]
        [DisplayName("報工單號")]
        public string ureport_code { get; set; }

        [DisplayName("報工日期")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string ureport_date { get; set; }

        [DisplayName("製令單號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string mo_code { get; set; }

        [DisplayName("機台代碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string mac_code { get; set; }

        [DisplayName("機台名稱")]
        public string mac_name { get; set; }

        [DisplayName("使用者編號")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string usr_code { get; set; }

        [DisplayName("使用者名稱")]
        public string usr_name { get; set; }


        [DisplayName("工單開始日期")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string wrk_date_s { get; set; }

        [DisplayName("工單開始時間")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string wrk_time_s { get; set; }

        [DisplayName("工單完成日期")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string wrk_date_e { get; set; }

        [DisplayName("工單完成時間")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string wrk_time_e { get; set; }

        [DisplayName("共計(分)")]
        [Required(ErrorMessage = "請輸入{0}")]
        public Int32 sub_minute { get; set; }

        [DisplayName("共計(時)")]
        [Required(ErrorMessage = "請輸入{0}")]
        public decimal sub_hour { get; set; }


        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }
    }
}