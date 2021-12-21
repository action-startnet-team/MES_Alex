using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MES_WATER.Models
{
    public class WMT10_0100
    {
        [Key]
        [DisplayName("識別碼")]
        public string wmt10_0100 { get; set; }

        [DisplayName("配料單號")]
        public string inq_code { get; set; }

        [DisplayName("順序")]
        public string scr_no { get; set; }

        [DisplayName("料號")]
        public string pro_code { get; set; }

        [DisplayName("批號")]
        public string lot_no { get; set; }

        [DisplayName("容器編號")]
        public string loc_code { get; set; }

        [DisplayName("實際用量")]
        public decimal pro_qty { get; set; }

        [DisplayName("單位編號")]
        public string unit_code { get; set; }

        [DisplayName("計畫用量")]
        public decimal plan_qty { get; set; }

        [DisplayName("計畫單位")]
        public string plan_unit_code { get; set; }

        [DisplayName("計畫單位")]
        public string plan_work_code { get; set; }

        [DisplayName("分裝用量")]
        public decimal pro_qty_min { get; set; }

        [DisplayName("分裝單位")]
        public string unit_code_min { get; set; }

        [DisplayName("標準差")]
        public decimal tol_qty { get; set; }

        [DisplayName("配科完成標記")]
        public string is_ok { get; set; }

        [DisplayName("備註")]
        public string des_memo { get; set; }

        [DisplayName("備料量")]
        public decimal par_qty { get; set; }

        [DisplayName("備料單位")]
        public string par_qty_unit { get; set; }

        [DisplayName("備料完成標記")]
        public string is_par { get; set; }

        [DisplayName("投料量")]
        public decimal in_qty { get; set; }

        [DisplayName("投料單位")]
        public string in_qty_unit { get; set; }

        [DisplayName("投料完成標記")]
        public string is_in { get; set; }

        [DisplayName("製程代碼")]
        public string work_code { get; set; }

        [DisplayName("製令代碼")]
        public string mo_code { get; set; }

    }
}