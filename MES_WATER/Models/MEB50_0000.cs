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
    public class MEB50_0000
    {
        [Key]
        [DisplayName("產品代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string ITEM_CODE { get; set; }

        [DisplayName("產品描述")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string ITEM_SPECIFICATION { get; set; }

        //[DisplayName("機台群組代號")]
        //[StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        //public string mac_type_code { get; set; }

        //[DisplayName("機台群組名稱")]
        //public string mac_type_name { get; set; }

        //[DisplayName("IP種類")]
        //[StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        //public string ip_type { get; set; }

        //[DisplayName("IP種類名稱")]
        //public string ip_name { get; set; }

        //[DisplayName("機台IP")]
        //[StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        //public string mac_ip { get; set; }

       

        [DisplayName("線別")]
        [Required(ErrorMessage = "請輸入{0}")]
        public string _pro_type { get; set; }

        //[DisplayName("線別名稱")]
        //public string line_name { get; set; }

        //[DisplayName("每小時標準產量")]
        //[HiddenInJqgrid]
        //public Int32 std_qty { get; set; }

        //[DisplayName("每日標準運作時間(秒)")]
        //[Required(ErrorMessage = "請輸入{0}")]
        //public Int32 std_time { get; set; }

        [DisplayName("標準工時")]
        public decimal pro_uph { get; set; }

        //[DisplayName("備註")]
        //[StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        //public string cmemo { get; set; }

        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}