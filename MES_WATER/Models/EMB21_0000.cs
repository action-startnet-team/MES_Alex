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
    public class EMB21_0000
    {
        [Key]
        [DisplayName("點檢項目代碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string chk_item_code { get; set; }

        [DisplayName("點檢項目名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多為{1}個字!")]
        public string chk_item_name { get; set; }

        [DisplayName("點檢說明")]
        [StringLength(100, ErrorMessage = "長度最多為{1}個字!")]
        public string chk_item_memo { get; set; }

        [DisplayName("檢驗資料類型")]
        public string chk_item_type { get; set; }

        [DisplayName("檢驗資料類型名稱")]
        public string chk_item_type_name { get; set; }

        [DisplayName("適用設備")]
        public string dev_code { get; set; }

        [DisplayName("設備名稱")]
        public string dev_name { get; set; }

        [DisplayName("順序")]
        [StringLength(4, ErrorMessage = "長度最多為{1}個字!")]
        public string scr_no { get; set; }

        [DisplayName("是否使用")]
        public string is_use { get; set; }

        [DisplayName("建立日期")]
        public string ins_date { get; set; }

        [DisplayName("建立時間")]
        public string ins_time { get; set; }

        [DisplayName("使用者編號")]
        public string usr_code { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}