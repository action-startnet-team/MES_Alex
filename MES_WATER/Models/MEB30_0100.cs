using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class MEB30_0100
    {
        [Key]
        [DisplayName("識別碼")]        
        public Int32 meb30_0100 { get; set; }

        //[DisplayName("製程代碼")]
        [DisplayName("製程代碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string work_code { get; set; }

        [DisplayName("站別代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string station_code {  get; set; }

        [DisplayName("站別名稱")]
        [ReadOnly(true)]
        public string station_name { get; set; }

        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }

    }
}