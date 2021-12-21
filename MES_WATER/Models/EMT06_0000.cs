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
    public class EMT06_0000
    {
        [Key]
        [DisplayName("維修單號")]       
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string rep_code { get; set; }

        [DisplayName("維修日期")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        public string rep_date { get; set; }

        [DisplayName("叫修單號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多為{1}個字!")]
        public string call_code { get; set; }

        [DisplayName("設備編號")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        public string dev_code { get; set; }

        [DisplayName("設備名稱")]
        public string dev_name { get; set; }

        [DisplayName("故障現象代碼")]
        [StringLength(20, ErrorMessage = "長度最多為{1}個字!")]
        public string fault_code { get; set; }

        [DisplayName("故障現象名稱")]
        public string fault_name { get; set; }

        [DisplayName("維修人員")]
        [StringLength(20, ErrorMessage = "長度最多為{1}個字!")]
        public string per_code { get; set; }

        [NotMapped]
        [DisplayName("人員名稱")]
        public string per_name { get; set; }

        [DisplayName("連絡電話")]
        [StringLength(20, ErrorMessage = "長度最多為{1}個字!")]
        public string per_tel { get; set; }

        [DisplayName("連絡Mail")]
        [StringLength(50, ErrorMessage = "長度最多為{1}個字!")]
        public string per_mail { get; set; }

        [DisplayName("故障處置編號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多為{1}個字!")]
        public string fault_handle_code { get; set; }

        [DisplayName("故障處置名稱")]
        public string fault_handle_name { get; set; }

        [DisplayName("維修備註")]
        [StringLength(200, ErrorMessage = "長度最多為{1}個字!")]
        public string rep_memo { get; set; }

        [DisplayName("建立日期")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        public string ins_date { get; set; }

        [DisplayName("建立時間")]
        [StringLength(8, ErrorMessage = "長度最多為{1}個字!")]
        public string ins_time { get; set; }

        [DisplayName("使用者編號")]
        [StringLength(20, ErrorMessage = "長度最多為{1}個字!")]
        public string usr_code { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}