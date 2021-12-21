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
    public class EMB07_0000
    {
        [Key]
        [DisplayName("設備代碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        public string dev_code { get; set; }

        [DisplayName("設備名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多為{1}個字!")]
        public string dev_name { get; set; }

        [DisplayName("保養設備群組代碼")]
        public string mai_type_code { get; set; }

        [DisplayName("設備說明")]
        [StringLength(100, ErrorMessage = "長度最多為{1}個字!")]
        public string dev_memo { get; set; }

        [DisplayName("廠牌名稱")]
        [StringLength(50, ErrorMessage = "長度最多為{1}個字!")]
        public string fac_name { get; set; }

        [DisplayName("廠商編號")]
        public string sup_code { get; set; }

        [DisplayName("廠商名稱")]
        public string sup_name { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}