using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class MEB12_0100
    {
        [Key]
        [DisplayName("資料識別碼")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public Int32 meb12_0100 { get; set; }

        [DisplayName("線別代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string line_code { get; set; }

        [DisplayName("線別名稱")]
        public string line_name{ get; set; }

        [DisplayName("產品代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string pro_code { get; set; }

        [DisplayName("產品名稱")]
        [ReadOnly(true)]
        public string pro_name { get; set; }

        [DisplayName("每小時產量")]
        [Required(ErrorMessage = "請輸入{0}")]
        public Int32 std_qty { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}