using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class MET04_0600
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

        [DisplayName("製程代碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string work_code { get; set; }

        [DisplayName("製程名稱")]
        public string work_name { get; set; }


        [DisplayName("不良狀態代碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string ng_code { get; set; }

        [DisplayName("不良狀態名稱")]
        public string ng_name { get; set; }

        [DisplayName("不良數量")]
        [Required(ErrorMessage = "請輸入{0}")]

        public double ng_qty { get; set; }


        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }
    }
}