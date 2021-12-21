using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.ModelBinding;

namespace MES_WATER.Models
{
    public class QMB14_0000
    {
        [Key]
        [DisplayName("管制點編號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多為{1}個字!")]
        public string spc_code { get; set; }

        [DisplayName("管制點名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多為{1}個字!")]
        public string spc_name { get; set; }

        [DisplayName("上限")]
        public decimal up_limit { get; set; }

        [DisplayName("下限")]
        public decimal down_limit { get; set; }

        [DisplayName("表單代號")]
        [StringLength(20, ErrorMessage = "長度最多為{1}個字!")]
        public string epb_code { get; set; }

        [DisplayName("表單名稱")]
        public string epb_name { get; set; }

        [DisplayName("表單欄位代號")]
        [StringLength(20, ErrorMessage = "長度最多為{1}個字!")]
        public string epb_field_code { get; set; }

        [DisplayName("表單欄位名稱")]
        public string epb_field_name { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}