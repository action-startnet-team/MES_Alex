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
    public class MEP06_0000
    {
        [Key]
        [DisplayName("識別碼")]
        public int mep06_0000 { get; set; }

        [DisplayName("機台代號")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        public string mac_code { get; set; }

        [DisplayName("機台名稱")]
        public string mac_name { get; set; }

        [DisplayName("上工日期")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        public string date_s { get; set; }

        [DisplayName("上工時間")]
        [StringLength(8, ErrorMessage = "長度最多為{1}個字!")]
        public string time_s { get; set; }

        [DisplayName("下工日期")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        public string date_e { get; set; }

        [DisplayName("下工時間")]
        [StringLength(8, ErrorMessage = "長度最多為{1}個字!")]
        public string time_e { get; set; }

        [DisplayName("工作時間(秒)")]
        public decimal work_second { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}