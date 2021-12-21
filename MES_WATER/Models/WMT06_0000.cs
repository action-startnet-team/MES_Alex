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
    public class WMT06_0000
    {

        [DisplayName("刪除備料單")]
        public string cancel { get; set; }

        [Key]
        [DisplayName("備料單號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(30, ErrorMessage = "長度最多{1}個字!")]
        public string prepare_code { get; set; }

        [DisplayName("備料日期")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        public string prepare_date { get; set; }

        [DisplayName("線別代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        public string line_code { get; set; }

        [DisplayName("線別名稱")]
        public string line_name { get; set; }

        [DisplayName("狀態")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(4, ErrorMessage = "長度最多為{1}個字!")]
        public string prepare_status { get; set; }

        [DisplayName("狀態名稱")]
        public string prepare_status_name { get; set; }

        [DisplayName("結案日期")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        public string end_date { get; set; }

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