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
    public class DTS01_0000
    {
        [Key]
        [DisplayName("資料識別碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(32, ErrorMessage = "長度最多{1}個字!")]
        public string dts01_0000 { get; set; }

        [DisplayName("來源系統別")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string con_code { get; set; }

        [DisplayName("執行來源")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string con_type { get; set; }

        [DisplayName("RFC函數名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string con_function { get; set; }

        [DisplayName("傳送查詢參數")]
        [Required(ErrorMessage = "請輸入{0}")]

        public string con_request { get; set; }


        [DisplayName("回傳結果")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string result { get; set; }

        [DisplayName("回傳訊息")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(500, ErrorMessage = "長度最多{1}個字!")]
        public string message { get; set; }

        [DisplayName("建立日期")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string ins_date { get; set; }

        [DisplayName("建立時間")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string ins_time { get; set; }

        [DisplayName("執行人員")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string usr_code { get; set; }

        [DisplayName("排程日期")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string sch_date { get; set; }


        [DisplayName("排程時間")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string sch_time { get; set; }

        [DisplayName("排程人員")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string sch_usr_code { get; set; }

        [DisplayName("處理標記")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string data_flag { get; set; }

        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }
    }
}