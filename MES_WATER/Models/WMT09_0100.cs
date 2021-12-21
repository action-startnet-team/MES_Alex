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
    public class WMT09_0100
    {
        [Key]
        [DisplayName("識別碼")]
        [HiddenInJqgrid]
        public int wmt09_0100 { get; set; }

        [DisplayName("盤點單號")]
        [HiddenInJqgrid]
        public string erp_inventory_code { get; set; }

        [DisplayName("順序")]
        [StringLength(4, ErrorMessage = "長度最多為{1}個字!")]
        public string scr_no { get; set; }

        [DisplayName("料號")]
        public string pro_code { get; set; }

        [DisplayName("料名")]
        public string pro_name { get; set; }

        [DisplayName("批號")]
        [StringLength(20, ErrorMessage = "長度最多為{1}個字!")]
        public string lot_no { get; set; }

        [DisplayName("儲位編號")]
        public string loc_code { get; set; }

        [DisplayName("儲位名稱")]
        public string loc_name { get; set; }

        [DisplayName("實盤量")]
        public decimal pro_qty { get; set; }

        [DisplayName("erp帳冊量")]
        public decimal sto_qty { get; set; }

        [DisplayName("wms差異量")]
        public decimal diff_qty { get; set; }

        [DisplayName("單位編號")]
        public string unit_code { get; set; }

        [DisplayName("單位名稱")]
        public string unit_name { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}