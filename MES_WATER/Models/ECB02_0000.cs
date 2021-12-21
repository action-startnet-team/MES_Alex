using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System;

namespace MES_WATER.Models
{
    public class ECB02_0000
    {
        [Key]
        [DisplayName("訂貨單轉檔主鍵")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(32, ErrorMessage = "長度最多{1}個字!")]
        public string CUSTOMER_CODE_EDITION { get; set; }

        [DisplayName("客戶編號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string CUSTOMER_CODE { get; set; }

        [DisplayName("客戶簡稱")]
        public string CUSTOMER_NAME { get; set; }

        [DisplayName("版次")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string EDITION { get; set; }

        [DisplayName("訂單編號對應資訊")]
        public string ORDER_NO_MAPPING { get; set; }

        [DisplayName("訂單序號對應資訊")]
        public string ORDER_DETAIL_NO { get; set; }

        [DisplayName("產品品號對應資訊")]
        public string ITEM_ID_MAPPING { get; set; }

        [DisplayName("訂購數量對應資訊")]
        public string PACKING_QTY_MAPPING { get; set; }

        [DisplayName("需求時間對應資訊")]
        public string NEED_DELIVERY_DATE_MAPPING { get; set; }

        [DisplayName("客戶備註對應資訊")]
        public string CUSTOMER_REMARK { get; set; }

        [DisplayName("生產備註對應資訊1")]
        public string PRODUCTION_REMARK1 { get; set; }

        [DisplayName("生產備註對應資訊2")]
        public string PRODUCTION_REMARK2 { get; set; }

        [DisplayName("建立者")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string SETUP_ROLE { get; set; }

        [DisplayName("建立日期")]
        public DateTime SETUP_DATE { get; set; }

        [DisplayName("建立日期")]
        [HiddenInJqgrid]
        public string SETUP_DATE2 { get; set; }

        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}