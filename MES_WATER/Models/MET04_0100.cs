using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class MET04_0100
    {
        [Key]
        [DisplayName("報工單號")]

        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
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
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string pro_code { get; set; }


        [DisplayName("產品名稱")]
        public string pro_name { get; set; }

        [DisplayName("上報數量")]
        [Required(ErrorMessage = "請輸入{0}")]
        public decimal pro_qty { get; set; }

        [DisplayName("單位")]
        [StringLength(3, ErrorMessage = "長度最多{1}個字!")]
        public string pro_unit { get; set; }

        [DisplayName("儲存地點")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string loc_code { get; set; }

        [DisplayName("產品批號")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string lot_no { get; set; }

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

        [DisplayName("建立日期")]

        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string ins_date { get; set; }

        [DisplayName("建立時間")]

        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string ins_time { get; set; }

        [DisplayName("使用者編號")]

        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string usr_code { get; set; }

        [DisplayName("SAP號碼")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string sap_code { get; set; }

        [DisplayName("SAP計數")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string sap_no { get; set; }

        [DisplayName("SAP訊息")]
        [StringLength(220, ErrorMessage = "長度最多{1}個字!")]
        public string sap_message { get; set; }

        [DisplayName("確認狀態")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
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