using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System;

namespace MES_WATER.Models
{
    public class ECT01_0100
    {
        [Key]
        [DisplayName("訂貨單轉檔紀錄主鍵")]
        [HiddenInJqgrid]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(32, ErrorMessage = "長度最多{1}個字!")]
        public string ORDER_NO_ID { get; set; }

        [DisplayName("客戶編號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string CUSTOMER_CODE { get; set; }

        [DisplayName("客戶單號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string ORDER_NO { get; set; }

        [DisplayName("客戶簡稱")]
        [HiddenInJqgrid]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string CUSTOMER_NAME { get; set; }

        [DisplayName("版次")]
        [HiddenInJqgrid]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string EDITION { get; set; }

        [DisplayName("產品編號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string ITEM_ID { get; set; }

        [DisplayName("序號")]
        public Int32 ORDER_NUM { get; set; }

        [DisplayName("訂購數量")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public int PACKING_QTY { get; set; }

        //[DisplayName("欲交日")]
        //[HiddenInJqgrid]
        //public DateTime NEED_DELIVERY_DATE { get; set; }

        [DisplayName("欲交日")]
        public string NEED_DELIVERY_DATE { get; set; }

        //[DisplayName("需求時間")]
        //[HiddenInJqgrid]
        //public string NEED_DELIVERY_DATE2 { get; set; }

        [DisplayName("客戶備註")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string CUSTOMER_REMARK { get; set; }

        [DisplayName("生產備註(一)")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string PRODUCTION_REMARK1 { get; set; }

        [DisplayName("生產備註(二)")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string PRODUCTION_REMARK2 { get; set; }

        [DisplayName("轉單人員")]
        [HiddenInJqgrid]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string CHANGE_ROLE { get; set; }

        [DisplayName("轉單日期")]
        [HiddenInJqgrid]
        public DateTime CHANGE_DATE { get; set; }

        [DisplayName("轉單日期")]
        [HiddenInJqgrid]
        public string CHANGE_DATE2 { get; set; }

        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}