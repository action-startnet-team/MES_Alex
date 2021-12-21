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
    public class MEB20_0100
    {
        [Key]
        [DisplayName("識別碼")]
        public Int32 meb20_0100 { get; set; }

        [DisplayName("產品代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string pro_code { get; set; }

        [DisplayName("產品名稱")]
        public string pro_name { get; set; }

        [DisplayName("標準產量")]
        [Required(ErrorMessage = "請輸入{0}")]
        public Int32 std_qty { get; set; }

        [DisplayName("標準工時(秒)")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(150, ErrorMessage = "長度最多為{1}個字!")]
        public string std_time { get; set; }

        [DisplayName("單位")]
        //[Required(ErrorMessage = "請輸入{0}")]
        [StringLength(3, ErrorMessage = "長度最多為{1}個字!")]
        public string pro_unit { get; set; }

        [DisplayName("站別代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        public string station_code { get; set; }

        [DisplayName("站別名稱")]
        public string station_name { get; set; }

        [DisplayName("製程代碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        public string work_code { get; set; }

        [DisplayName("製程名稱")]
        public string work_name { get; set; }

        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}