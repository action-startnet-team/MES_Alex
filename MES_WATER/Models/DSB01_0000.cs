using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MES_WATER.Models
{
    public class DSB01_0000
    {
        [Key]
        [DisplayName("資料識別碼")]
        public int dsb01_0000 { get; set; }

        [DisplayName("機台代碼")]
        public string mac_code { get; set; }

        [DisplayName("機台名稱")]
        public string mac_name { get; set; }

        [DisplayName("稼動率")]
        //[Range(0, 200, ErrorMessage = "請輸入0~100")]
        public decimal utilization_rate { get; set; }

        [DisplayName("產能效率")]
        //[Range(0, 200, ErrorMessage = "請輸入0~100")]
        public decimal capacity_efficiency { get; set; }

        [DisplayName("良率")]
        //[Range(0, 200, ErrorMessage = "請輸入0~100")]
        public decimal yield { get; set; }

        [DisplayName("產量(良品量)")]
        public decimal pro_qty { get; set; }

        [DisplayName("不良品量")]
        public decimal ng_qty { get; set; }

        [DisplayName("實際工作時間")]
        public decimal act_time { get; set; }

        [DisplayName("計畫工作時間")]
        public decimal wrk_time { get; set; }

        [DisplayName("計畫工作產能")]
        public decimal wrk_qty { get; set; }

        [Required(ErrorMessage = "請輸入{0}")]
        [DisplayName("日期")]
        public string cal_date { get; set; }

        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        [NotMapped]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        [NotMapped]
        public string can_update { get; set; }
    }
}