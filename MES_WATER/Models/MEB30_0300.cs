using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class MEB30_0300
    {
        [Key]
        [DisplayName("識別碼")]
        public int meb30_0300 { get; set; }

        [DisplayName("製程代碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string work_code { get; set; }

        [DisplayName("製程名稱")]
        [NotMapped]
        [HiddenInJqgrid]
        public string work_name { get; set; }

        [DisplayName("不良現象代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string ng_code { get; set; }

        [DisplayName("不良現象名稱")]
        [ReadOnly(true)]
        public string ng_name { get; set; }


        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }
    }
}