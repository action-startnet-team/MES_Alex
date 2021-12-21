using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System;

namespace MES_WATER.Models
{
    public class ECB04_0000
    {
        [Key]
        [DisplayName("出貨單轉檔主鍵")]
        public string SALES_CUSTOMER_CODE_EDITION { get; set; }

        [DisplayName("客戶編號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string CUSTOMER_CODE { get; set; }

        [DisplayName("版次")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string EDITION { get; set; }

        

        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}