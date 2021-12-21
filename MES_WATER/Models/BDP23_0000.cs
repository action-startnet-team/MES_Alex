using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class BDP23_0000
    {
        [Key]
        [DisplayName("識別碼")]
        public int bdp23_0000 { get; set; }

        [DisplayName("公告人員")]
        public string usr_code { get; set; }

        [DisplayName("主旨")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string theme { get; set; }

        [DisplayName("內容")]
        [StringLength(1000, ErrorMessage = "長度最多{1}個字!")]
        public string bull_con { get; set; }

        [DisplayName("公告日期")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string bull_date { get; set; }

        [DisplayName("公告結束日期")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string eff_date { get; set; }

        [DisplayName("公告類別")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string bull_type { get; set; }

        [DisplayName("公告類別名稱")]
        public string bull_type_name { get; set; }

        [DisplayName("公告種類")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string bull_kind { get; set; }

        [DisplayName("公告種類名稱")]
        public string bull_kind_name { get; set; }

        //public string rel_code { get; set; }
        
        //public string rel_mode { get; set; }
        
        //public string rel_key { get; set; }
        
        //public string rel_prg { get; set; }

        [DisplayName("公告建立時間")]
        public string bull_time { get; set; }

        // 以下是join的欄位 
        //public string usr_name { get; set; }
        
        //public string is_ok { get; set; }
        
        //public string ok_date { get; set; }
        
        //public string usr_memo { get; set; }
        
        //public int? bdb23_0100 { get; set; }


        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }

    }
}