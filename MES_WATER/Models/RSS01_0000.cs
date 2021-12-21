using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class RSS01_0000
    {
        [Key]
        [DisplayName("ETL代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string etl_code { get; set; }

        [DisplayName("ETL名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string etl_name { get; set; }

        [DisplayName("ETL說明")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string cmemo { get; set; }

        [DisplayName("Select 語法")]
        [StringLength(5000, ErrorMessage = "長度最多{1}個字!")]
        public string select_string { get; set; }

        [DisplayName("Where 語法")]
        [StringLength(500, ErrorMessage = "長度最多{1}個字!")]
        public string where_string { get; set; }

        [DisplayName("建立日期")]
        public string ins_date { get; set; }

        [DisplayName("建立時間")]
        public string ins_time { get; set; }

        [DisplayName("使用者編號")]
        public string usr_code { get; set; }


        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }
    }
}