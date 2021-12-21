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
    public class MEB29_0000
    {
        [Key]
        [DisplayName("站別代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string station_code { get; set; }

        [DisplayName("站別名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多為{1}個字!")]
        public string station_name { get; set; }

        [DisplayName("站別類型代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        public string station_type_code { get; set; }

        [DisplayName("站別說明")]
        [StringLength(100, ErrorMessage = "長度最多為{1}個字!")]
        public string cmemo { get; set; }

        [DisplayName("庫存是否需要轉入")]
        [HiddenInJqgrid]
        public string is_sto_in { get; set; }

        [DisplayName("庫存是否需要轉出")]
        [HiddenInJqgrid]
        public string is_sto_out { get; set; }

        [DisplayName("是否執行人員管制")]
        [HiddenInJqgrid]
        public string is_check_per { get; set; }

        [DisplayName("線上倉儲位")]
        [HiddenInJqgrid]
        public string loc_code { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}