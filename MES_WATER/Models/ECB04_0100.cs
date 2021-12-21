﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System;

namespace MES_WATER.Models
{
    public class ECB04_0100
    {
        [Key]
        [DisplayName("出貨單明細檔主鍵")]
        [HiddenInJqgrid]
        public string ecb04_0100 { get; set; }

        [DisplayName("出貨單轉檔主鍵")]
        [HiddenInJqgrid]
        public string SALES_CUSTOMER_CODE_EDITION { get; set; }

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

        [DisplayName("EXCEL欄位代碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string EXCEL_CODE { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}