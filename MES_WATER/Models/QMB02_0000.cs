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
    public class QMB02_0000
    {
        [Key]
        [DisplayName("檢驗項目代碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        public string qtest_item_code { get; set; }

        [DisplayName("檢驗項目名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多為{1}個字!")]
        public string qtest_item_name { get; set; }

        [DisplayName("物料代碼")]
        [HiddenInJqgrid]
        public string pro_code { get; set; }

        [DisplayName("物料名稱")]
        [HiddenInJqgrid]
        public string pro_name { get; set; }

        [DisplayName("檢驗項目說明")]
        [StringLength(100, ErrorMessage = "長度最多為{1}個字!")]
        public string qtest_item_memo { get; set; }

        [DisplayName("檢驗類別代碼")]
        public string qtest_type_code { get; set; }

        [DisplayName("檢驗類別名稱")]
        public string qtest_type_name { get; set; }

        [DisplayName("規格上限")]
        [HiddenInJqgrid]
        public string qtest_up { get; set; }

        [DisplayName("允差下限")]
        [HiddenInJqgrid]
        public string qtest_down { get; set; }

        [DisplayName("檢驗資料類型")]
        public string qtest_item_type { get; set; }

        [DisplayName("檢驗資料類型名稱")]
        public string qtest_item_type_name { get; set; }

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