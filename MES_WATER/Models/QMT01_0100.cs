using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class QMT01_0100
    {
        [Key]
        [DisplayName("識別碼")]
        public Int32 qmt01_0100 { get; set; }

        [DisplayName("採購檢驗編號")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string qmt_code { get; set; }

        [DisplayName("檢驗項目名稱")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string qtest_item_name {  get; set; }

        [DisplayName("檢驗項目說明")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string qtest_item_memo { get; set; }

        [DisplayName("檢驗類別代碼")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string qtest_type_code { get; set; }

        [DisplayName("檢驗單位")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string qtest_unit { get; set; }

        [DisplayName("規格上限")]
        public decimal qtest_up { get; set; }

        [DisplayName("允差下限")]
        public decimal qtest_down { get; set; }

        [DisplayName("檢驗資料類型")]
        [StringLength(2, ErrorMessage = "長度最多{1}個字!")]
        public string qtest_item_type { get; set; }

        [DisplayName("檢驗資料類型名稱")]
        public string qtest_item_type_name { get; set; }

        [DisplayName("作業號碼")]
        [StringLength(4, ErrorMessage = "長度最多{1}個字!")]
        public string VORNR { get; set; }

        [DisplayName("檢驗特性號碼")]
        [StringLength(4, ErrorMessage = "長度最多{1}個字!")]
        public string MERKNR { get; set; }

        [DisplayName("代碼群組")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string AUSWMENGE1 { get; set; }

        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }
    }
}