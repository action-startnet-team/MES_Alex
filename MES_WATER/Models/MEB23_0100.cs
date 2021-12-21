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
    public class MEB23_0100
    {
        [Key]
        [DisplayName("明細識別碼")]
        [HiddenInJqgrid]
        public string meb23_0100 { get; set; }

        [DisplayName("BOM代號")]
        [HiddenInJqgrid]
        public string bom_code { get; set; }

        [DisplayName("物料代碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多為{1}個字!")]
        public string pro_code { get; set; }

        [DisplayName("物料名稱")]
        public string pro_name { get; set; }

        [DisplayName("原料屬性")]
        public string is_throw { get; set; }

        [DisplayName("原料屬性名稱")]
        public string is_throw_name { get; set; }

        [DisplayName("標準用量")]
        [Required(ErrorMessage = "請輸入{0}")]
        public decimal pro_qty { get; set; }

        [DisplayName("標準耗損")]
        [Required(ErrorMessage = "請輸入{0}")]
        public decimal dis_qty { get; set; }

        [DisplayName("公差(%)")]
        public decimal tol_qty { get; set; }

        [DisplayName("標準單位")]
        public string unit_code { get; set; }

        [DisplayName("包裝用量")]
        [Required(ErrorMessage = "請輸入{0}")]
        public decimal pro_qty_min { get; set; }

        [DisplayName("包裝重")]
        public decimal pack_qty { get; set; }

        [DisplayName("包裝公差(%)")]
        public decimal pack_tol_qty { get; set; }

        [DisplayName("包裝單位")]
        public string unit_code_min { get; set; }

        [DisplayName("是否分秤")]
        public string is_ready { get; set; }

        [DisplayName("投料順序")]
        [StringLength(4, ErrorMessage = "長度最多為{1}個字!")]
        public string in_scr_no { get; set; }

        [DisplayName("製程代碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        public string work_code { get; set; }

        [DisplayName("製程名稱")]
        [ReadOnly(true)]
        public string work_name { get; set; }

        [DisplayName("投料口")]
        public string loc_code { get; set; }

        [DisplayName("投料口名稱")]
        [ReadOnly(true)]
        public string loc_name { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }
        
        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}