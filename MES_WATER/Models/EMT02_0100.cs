using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MES_WATER.Models
{
    public class EMT02_0100
    {
        [Key]
        [DisplayName("識別碼")]
        [HiddenInJqgrid]
        public int emt02_0100 { get; set; }

        [DisplayName("設備點檢表")]
        [HiddenInJqgrid]
        public string dev_check_code { get; set; }

        [DisplayName("點檢項目代碼")]
        public string chk_item_code { get; set; }

        [DisplayName("點檢項目名稱")]
        public string chk_item_name { get; set; }

        [DisplayName("點檢結果")]
        public string is_ok { get; set; }

        [DisplayName("點檢結果名稱")]
        public string is_ok_name { get; set; }

        [DisplayName("點檢說明")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string des_memo { get; set; }

        [DisplayName("叫修單號")]
        public string sor_code { get; set; }

        [DisplayName("順序")]
        public string scr_no { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}