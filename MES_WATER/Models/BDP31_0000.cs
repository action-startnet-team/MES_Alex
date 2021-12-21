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
    public class BDP31_0000
    {
        [Key]
        [DisplayName("資料來源設定")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(30, ErrorMessage = "長度最多為{1}個字!")]
        public string select_code { get; set; }

        [DisplayName("資料來源名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多為{1}個字!")]
        public string select_name { get; set; }

        [DisplayName("資料來源類別")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多為{1}個字!")]
        public string select_type { get; set; }

        [DisplayName("Sql select")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(200, ErrorMessage = "長度最多為{1}個字!")]
        public string tsql_select { get; set; }

        [DisplayName("Sql where")]
        [StringLength(100, ErrorMessage = "長度最多為{1}個字!")]
        public string tsql_where { get; set; }

        [DisplayName("Sql Order by")]
        [StringLength(100, ErrorMessage = "長度最多為{1}個字!")]
        public string tsql_order { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}