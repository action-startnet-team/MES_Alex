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
    public class WMT0300
    {
        


        [DisplayName("單別")]
      
        public string rel_type { get; set; }

        [DisplayName("訂單單號")]
        public string id { get; set; }




        [DisplayName("物料編號")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string pro_code { get; set; }


        [DisplayName("批號")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string lot_no { get; set; }

        [DisplayName("數量")]
        public decimal pro_qty { get; set; }

        //[DisplayName("客戶")]
        //[StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        //public string cus_code { get; set; }

        //[DisplayName("刷讀品項的來源識別碼")]
        //public int wmt0100 { get; set; }

        [DisplayName("出貨日期")]
        
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public string ins_date { get; set; }


        //[DisplayName("來源單號")]
        //[StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        //public string sor_no { get; set; }

        //[DisplayName("供應商編號")]
        //[StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        //public string tra_code { get; set; }

        [DisplayName("客戶代號")]

        public string cus_code { get; set; }



        


        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}