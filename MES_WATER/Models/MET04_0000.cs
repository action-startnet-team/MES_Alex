using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class MET04_0000
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

        [DisplayName("產品編號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string pro_code { get; set; }

        [DisplayName("產品名稱")]
        public string pro_name { get; set; }

        [DisplayName("良品量")]
        [Required(ErrorMessage = "請輸入{0}")]
        public decimal pro_qty { get; set; }

        [DisplayName("單位")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(3, ErrorMessage = "長度最多{1}個字!")]
        public string pro_unit { get; set; }

        [DisplayName("生產開始日期")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string pro_date_s { get; set; }

        [DisplayName("生產開始時間")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string pro_time_s { get; set; }

        [DisplayName("生產完成日期")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string pro_date_e { get; set; }

        [DisplayName("生產完成時間")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string pro_time_e { get; set; }

        [DisplayName("人時")]
        [Required(ErrorMessage = "請輸入{0}")]
        public decimal ISM01 { get; set; }

        [DisplayName("機時")]
        [Required(ErrorMessage = "請輸入{0}")]
        public decimal ISM02 { get; set; }

        [DisplayName("電費(度)")]
        [Required(ErrorMessage = "請輸入{0}")]
        public decimal ISM03 { get; set; }

        [DisplayName("燃料(蒸氣)")]
        [Required(ErrorMessage = "請輸入{0}")]
        public decimal ISM04 { get; set; }

        [DisplayName("變動製費(時)")]
        [Required(ErrorMessage = "請輸入{0}")]
        public decimal ISM05 { get; set; }

        [DisplayName("固定製費(時)")]
        [Required(ErrorMessage = "請輸入{0}")]
        public decimal ISM06 { get; set; }

        [DisplayName("建立日期")]
        public string ins_date { get; set; }

        [DisplayName("建立時間")]
        public string ins_time { get; set; }

        [DisplayName("使用者編號")]
        public string usr_code { get; set; }


        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }
    }
}