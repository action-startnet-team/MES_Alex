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
    public class QMB13_0000
    {
        [Key]
        [DisplayName("抽樣表代碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string sampling_code { get; set; }

        [DisplayName("抽樣表名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string sampling_name { get; set; }

        [DisplayName("抽樣表說明")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string sampling_memo { get; set; }

        [DisplayName("抽樣數量")]
        [Required(ErrorMessage = "請輸入{0}")]
        public int pro_qty { get; set; }

        [DisplayName("抽樣量-加量")]
        [Required(ErrorMessage = "請輸入{0}")]
        public int pro_qty_add { get; set; }

        [DisplayName("抽樣量-減量")]
        [Required(ErrorMessage = "請輸入{0}")]
        public int pro_qty_dis { get; set; }

        [DisplayName("允收數")]
        [Required(ErrorMessage = "請輸入{0}")]
        public int ok_qty { get; set; }

        [DisplayName("拒收數")]
        [Required(ErrorMessage = "請輸入{0}")]
        public int ng_qty { get; set; }


        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}