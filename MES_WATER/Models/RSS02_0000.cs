using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class RSS02_0000
    {
        [Key]
        [DisplayName("報表代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string report_code { get; set; }

        [DisplayName("報表名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string report_name { get; set; }

        [DisplayName("報表型態")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string report_type { get; set; }

        [DisplayName("報表型態名稱")]
        public string report_type_name { get; set; }

        [DisplayName("報表說明")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string cmemo { get; set; }

        [DisplayName("資料來源類別")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string data_source_type { get; set; }

        [DisplayName("資料來源類別名稱")]
        public string data_source_type_name { get; set; }

        [DisplayName("資料來源代號")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string etl_code { get; set; }

        [DisplayName("資料來源名稱")]
        public string etl_name { get; set; }

        [DisplayName("表單代號")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string epb_code { get; set; }

        [DisplayName("表單名稱")]
        public string epb_name { get; set; }

        [DisplayName("建立日期")]
        public string ins_date { get; set; }

        [DisplayName("建立時間")]
        public string ins_time { get; set; }

        [DisplayName("使用者編號")]
        public string usr_code { get; set; }

        [DisplayName("上傳範本")]
        public string upload { get; set; }


        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }
    }
}