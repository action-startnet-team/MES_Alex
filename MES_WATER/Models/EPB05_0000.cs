using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class EPB05_0000
    {
        [Key]
        [NotMapped]
        [DisplayName("資料識別碼")]
        public string epb05_0000 { get; set; }

        [DisplayName("審核設定代號")]
        [NotMapped]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string review_code { get; set; }

        [DisplayName("審核")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string review { get; set; }

        [DisplayName("審核進度")]
        public string review_progress { get; set; }

        [DisplayName("電子表單代號")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string epb_code { get; set; }

        [DisplayName("電子表單名稱")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string epb_name { get; set; }

        [DisplayName("電子表單資料鍵值")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        [HiddenInJqgrid]
        public string epb_key { get; set; }

        [DisplayName("報表資料群組碼")]
        public string report_group_code { get; set; }


        [DisplayName("處理標記")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string is_ok { get; set; }


        [DisplayName("資料順序")]
        public string scr_no { get; set; }

        [DisplayName("建立日期")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string ins_date { get; set; }


        [DisplayName("建立時間")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string ins_time { get; set; }


        [DisplayName("使用者編號")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string usr_code { get; set; }

        [DisplayName("審核結果")]
        [StringLength(6, ErrorMessage = "長度最多{1}個字!")]
        public string result_code { get; set; }

        [DisplayName("審核日期")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string out_date { get; set; }

        [DisplayName("審核時間")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string out_time { get; set; }


        [DisplayName("下一級應審人員")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string next_usr_code { get; set; }

        [DisplayName("審核意見")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string review_memo { get; set; }


        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}