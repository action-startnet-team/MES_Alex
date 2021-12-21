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
    public class WMB02_0000
    {
        [Key]
        [DisplayName("儲位編號")]        
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        
        public string loc_code { get; set; }

        [DisplayName("儲位名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string loc_name { get; set; }

        [DisplayName("倉庫編號")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string sto_code {  get; set; }

        [DisplayName("倉庫名稱")]
        public string sto_name { get; set; }

        [DisplayName("儲位類別")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string loc_type { get; set; }


        [DisplayName("儲位類別名稱")]
        [ReadOnly(true)]
        public string loc_type_name { get; set; }


        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}