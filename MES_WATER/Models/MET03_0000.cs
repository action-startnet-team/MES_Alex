using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class MET03_0000
    {
        //[Key]
        [DisplayName("工單號碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(30, ErrorMessage = "長度最多{1}個字!")]
        public string wrk_code { get; set; }

        [Key]
        [DisplayName("製令號碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string mo_code { get; set; }

        [DisplayName("工單日期")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string wrk_date { get; set; }

        [DisplayName("產品編號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string pro_code { get; set; }

        [DisplayName("產品名稱")]
        public string pro_name { get; set; }

        [DisplayName("生產數量")]
        [Required(ErrorMessage = "請輸入{0}")]
        public decimal pro_qty { get; set; }

        [DisplayName("單位")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(3, ErrorMessage = "長度最多{1}個字!")]
        public string pro_unit { get; set; }

        [DisplayName("批次數量")]
        public string lot_conut { get; set; }

        [DisplayName("途程代碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string work_code { get; set; }

        [DisplayName("途程名稱")]
        public string work_name { get; set; }

        [DisplayName("站別代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string station_code { get; set; }

        [DisplayName("站別名稱")]
        public string station_name { get; set; }

        [DisplayName("工單狀態")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(4, ErrorMessage = "長度最多{1}個字!")]
        public string mo_status { get; set; }

        [DisplayName("工單狀態名稱")]
        public string mo_status_name { get; set; }

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