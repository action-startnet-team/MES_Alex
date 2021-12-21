using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System;

namespace MES_WATER.Models
{
    public class ECT02_0100
    {
        [Key]
        [DisplayName("紀錄明細檔主鍵")]
        [HiddenInJqgrid]
        public string ect02_0100 { get; set; }

        [DisplayName("紀錄主檔主鍵")]
        [HiddenInJqgrid]
        public string SALES_ORDER_NO_ID { get; set; }

        [DisplayName("序號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string SERIAL_NUM { get; set; }

        [DisplayName("ERP欄位代碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string ERP_FIELD_CODE { get; set; }

        [DisplayName("中文名稱")]
        [ReadOnly(true)]
        public string ERP_FIELD_NAME { get; set; }

        [DisplayName("值")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string VALUE { get; set; }

        

        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}