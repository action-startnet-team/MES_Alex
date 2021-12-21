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
    public class MEB20_0000
    {
        [Key]
        [DisplayName("產品代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string pro_code { get; set; }

        [DisplayName("產品名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(100, ErrorMessage = "長度最多為{1}個字!")]
        public string pro_name { get; set; }

        [DisplayName("產品規格")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(150, ErrorMessage = "長度最多為{1}個字!")]
        public string pro_spc { get; set; }

        [DisplayName("單位")]
        [Required(ErrorMessage = "請輸入{0}")]
        public string unit_code { get; set; }

        [DisplayName("單位名稱")]
        public string unit_name { get; set; }

        [DisplayName("預設線別")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        public string line_code { get; set; }

        [DisplayName("預設線別名稱")]
        public string line_name { get; set; }

        [DisplayName("產品性質")]
        [Required(ErrorMessage = "請輸入{0}")]
        public string pro_type { get; set; }

        [DisplayName("產品類別代號")]
        [StringLength(20, ErrorMessage = "長度最多為{1}個字!")]
        public string pro_type_code { get; set; }

        [DisplayName("產品類別名稱")]
        public string pro_type_name { get; set; }

        [DisplayName("是否使用")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多為{1}個字!")]
        public string is_use { get; set; }

        [DisplayName("備註")]
        [StringLength(100, ErrorMessage = "長度最多為{1}個字!")]
        public string cmemo { get; set; }

        [DisplayName("管控有效天數")]
        public int exp_num { get; set; }

        [DisplayName("開封有效天數")]
        public int open_exp_num { get; set; }

        [DisplayName("超收率")]
        public decimal mtp_rec_rate { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}