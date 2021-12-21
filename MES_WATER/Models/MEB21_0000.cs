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
    public class MEB21_0000
    {
        [Key]
        [DisplayName("產品類別代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string pro_type_code { get; set; }

        [DisplayName("產品類別名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多為{1}個字!")]
        public string pro_type_name { get; set; }

        [DisplayName("WIP管理方式")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多為{1}個字!")]
        public string wip_type { get; set; }

        [DisplayName("WIP管理方式名稱")]
        public string wip_type_name { get; set; }

        [DisplayName("消耗品")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多為{1}個字!")]
        public string is_consum { get; set; }

        [DisplayName("扣料方式")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多為{1}個字!")]
        public string dis_type { get; set; }

        [DisplayName("扣料方式名稱")]
        public string dis_type_name { get; set; }

        [DisplayName("工單控管庫存")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多為{1}個字!")]
        public string chk_type { get; set; }

        [DisplayName("備註")]
        [StringLength(100, ErrorMessage = "長度最多為{1}個字!")]
        public string cmemo { get; set; }

        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }

    }
}