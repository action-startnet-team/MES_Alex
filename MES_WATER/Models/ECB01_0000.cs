using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MES_WATER.Models
{
    public class ECB01_0000
    {
        [Key]
        [DisplayName("客戶編號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string CUSTOMER_CODE { get; set; }

        [DisplayName("客戶簡稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string CUSTOMER_NAME { get; set; }

        [DisplayName("客戶全稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(72, ErrorMessage = "長度最多{1}個字!")]
        public string CUSTOMER_FULL_NAME { get; set; }

        [DisplayName("客戶屬性資訊")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string CUSTOMER_PROPERTY { get; set; }

        [DisplayName("開業日期")]
        [Required(ErrorMessage = "請輸入{0}")]
        public DateTime SETUP_DATE { get; set; }

        [DisplayName("開業日期")]
        [HiddenInJqgrid]
        public string SETUP_DATE2 { get; set; }

        [DisplayName("備註")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string REMARK { get; set; }

        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}