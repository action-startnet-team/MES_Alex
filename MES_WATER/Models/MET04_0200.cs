using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class MET04_0200
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

        [DisplayName("作業號碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string up_type { get; set; }

        [DisplayName("良品量")]
        [Required(ErrorMessage = "請輸入{0}")]
        public decimal pro_qty { get; set; }

        [DisplayName("單位")]
        public string pro_unit { get; set; }

        [DisplayName("差異原因")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string stop_code { get; set; }

        [DisplayName("工單開始日期")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string pro_date_s { get; set; }

        [DisplayName("工單開始時間")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string pro_time_s { get; set; }

        [DisplayName("工單完成日期")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string pro_date_e { get; set; }

        [DisplayName("工單完成時間")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string pro_time_e { get; set; }

        [DisplayName("人時")]
        [Required(ErrorMessage = "請輸入{0}")]
        public decimal ISM01 { get; set; }

        [Required(ErrorMessage = "請輸入{0}")]
        public decimal ISM0101 { get; set; }

        [DisplayName("機時")]
        [Required(ErrorMessage = "請輸入{0}")]
        public decimal ISM02 { get; set; }

        [DisplayName("電費(度)")]
        [Required(ErrorMessage = "請輸入{0}")]
        public decimal ISM03 { get; set; }

        [DisplayName("電費固定數")]
        [Required(ErrorMessage = "請輸入{0}")]
        public decimal ISM0301 { get; set; }

        [DisplayName("燃料(蒸氣)")]
        [Required(ErrorMessage = "請輸入{0}")]
        public decimal ISM04 { get; set; }

        [DisplayName("燃料固定數")]
        [Required(ErrorMessage = "請輸入{0}")]
        public decimal ISM0401 { get; set; }

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

        [DisplayName("SAP號碼")]
        public string sap_code { get; set; }

        [DisplayName("SAP計數")]
        public string sap_no { get; set; }

        [DisplayName("SAP訊息")]
        public string sap_message { get; set; }

        [DisplayName("上報SAP順序")]
        public int sap_scr_no { get; set; }

        [DisplayName("確認狀態")]
        [Required(ErrorMessage = "請輸入{0}")]
        public string is_ok { get; set; }

        [DisplayName("確認狀態名稱")]
        public string is_ok_name { get; set; }

        public string is_del { get; set; }


        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }
    }
}