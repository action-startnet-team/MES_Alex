using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class RSS01_0100
    {
        [Key]
        [DisplayName("識別碼")]
        public int rss01_0100 { get; set; }

        [DisplayName("ETL代號")]
        public string etl_code { get; set; }

        [DisplayName("欄位編號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string field_code { get; set; }

        [DisplayName("欄位名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string field_name { get; set; }

        [DisplayName("資料型態")]
        [Required(ErrorMessage = "請輸入{0}")]
        public string data_type { get; set; }

        [DisplayName("資料型態名稱")]
        [ReadOnly(true)]
        public string data_type_name { get; set; }


        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }
    }
}