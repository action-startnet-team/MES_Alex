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
    public class EMT01_0100B
    {
        [Key]
        [DisplayName("識別碼")]
        public string emt01_0100 { get; set; }

        [DisplayName("保養計畫編號")]
        public string maintain_code { get; set; }

        [DisplayName("保養計畫名稱")]
        public string maintain_name { get; set; }

        [DisplayName("設備代號")]
        public string dev_code { get; set; }

        [DisplayName("設備名稱")]
        public string dev_name { get; set; }

        [DisplayName("保養項目代碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        public string main_item_code { get; set; }

        [DisplayName("保養項目名稱")]
        public string main_item_name { get; set; }

        [DisplayName("預計保養日期")]
        public string ins_date { get; set; }

        [DisplayName("預計開始時間")]
        public string ins_time { get; set; }

        [DisplayName("預計保養人員代號")]
        public string usr_code { get; set; }

        [DisplayName("預計保養人員名稱")]
        public string usr_name { get; set; }

        [DisplayName("實際保養日期")]
        public string act_date { get; set; }

        [DisplayName("實際保養時間")]
        public string act_time { get; set; }

        [DisplayName("實際保養人員")]
        public string act_usr_code { get; set; }

        [DisplayName("實際保養人員名稱")]
        public string act_usr_name { get; set; }

        [DisplayName("保養週期")]
        public string maintain_cycle { get; set; }

        [DisplayName("保養說明")]
        [StringLength(100, ErrorMessage = "長度最多為{1}個字!")]
        public string maintain_memo { get; set; }

        [DisplayName("保養完成")]
        public string is_ok { get; set; }

        [DisplayName("叫修單號")]
        public string sor_code { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}