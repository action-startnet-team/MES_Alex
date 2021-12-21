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
    public class MEP01_0000
    {
        [Key]
        [DisplayName("識別碼")]
        public int mep01_0000 { get; set; }

        [DisplayName("製令號碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多為{1}個字!")]
        public string mo_code { get; set; }

        [DisplayName("工單號碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(30, ErrorMessage = "長度最多為{1}個字!")]
        public string wrk_code { get; set; }

        [DisplayName("製程代碼")]
        public string work_code { get; set; }

        [DisplayName("製程名稱")]
        public string work_name { get; set; }

        [DisplayName("站別代號")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        public string station_code { get; set; }

        [DisplayName("站別名稱")]
        public string station_name { get; set; }

        [DisplayName("機台代號")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        public string mac_code { get; set; }

        [DisplayName("機台名稱")]
        public string mac_name { get; set; }

        [DisplayName("人員代號")]
        [StringLength(20, ErrorMessage = "長度最多為{1}個字!")]
        public string usr_code { get; set; }

        [DisplayName("人員名稱")]
        public string usr_name { get; set; }

        [DisplayName("產品代碼")]
        [StringLength(50, ErrorMessage = "長度最多為{1}個字!")]
        public string pro_code { get; set; }

        [DisplayName("產品名稱")]
        public string pro_name { get; set; }

        [DisplayName("批號")]
        [StringLength(30, ErrorMessage = "長度最多為{1}個字!")]
        public string pro_lot_no { get; set; }

        [DisplayName("IOT良品量")]
        public decimal iot_ok_qty { get; set; }

        [DisplayName("良品量")]
        public decimal ok_qty { get; set; }

        [DisplayName("良品單位")]
        [StringLength(3, ErrorMessage = "長度最多為{1}個字!")]
        public string ok_unit { get; set; }

        [DisplayName("IOT不良品量")]
        public decimal iot_ng_qty { get; set; }

        [DisplayName("不良品量")]
        public decimal ng_qty { get; set; }

        [DisplayName("不良品單位")]
        [StringLength(3, ErrorMessage = "長度最多為{1}個字!")]
        public string ng_unit { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}