using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class MEB30_0000
    {
        [Key]
        //[DisplayName("製程代碼")]  
        [DisplayName("製程代碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string work_code { get; set; }

        //[DisplayName("製程名稱")]
        [DisplayName("製程名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string work_name { get; set; }

        //[DisplayName("製程說明")]
        [DisplayName("製程說明")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string cmemo {  get; set; }


        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }

    }
}