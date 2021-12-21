using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System;

namespace MES_WATER.Models
{
    public class ECT02_0200
    {
        [Key]
        [DisplayName("紀錄主檔主鍵")]
        public string SALES_ORDER_NO_ID { get; set; }

        [DisplayName("出貨單轉檔主鍵")]
        public string SALES_CUSTOMER_CODE_EDITION { get; set; }

        [DisplayName("出貨單號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string SALES_ORDER_NO { get; set; }

        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}