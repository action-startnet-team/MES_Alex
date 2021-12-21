using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class EPB02_0100
    {
        [Key]
        [DisplayName("識別碼")]
        [ReadOnly(true)]
        [HiddenInJqgrid]
        public Int32 epb02_0100 { get; set; }

        [DisplayName("表單代號")]        
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        [ReadOnly(true)]
        [HiddenInJqgrid]
        public string epb_code { get; set; }
      
        [DisplayName("常用字串設定")]
        [ReadOnly(true)]
        public string common_str { get; set; }

        [DisplayName("常用字串列印")]
        [ReadOnly(true)]
        public string export_commonstr { get; set; }

        [DisplayName("欄位代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        [ReadOnly(true)]

        public string field_code { get; set; }

        [DisplayName("欄位名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string field_name { get; set; }

        [DisplayName("存檔欄位")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string save_field { get; set; }



        [DisplayName("欄位說明")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string field_memo { get; set; }

        [DisplayName("顯示序號")]
        [Required(ErrorMessage = "請輸入{0}")]
        public Int32 scr_no {  get; set; }

        [DisplayName("控制項類別")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string ctr_type { get; set; }

        [DisplayName("控制項名稱")]
        [ReadOnly(true)]
        public string ctr_name { get; set; }


        [DisplayName("控制項預設值")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string ctr_default_value { get; set; }

        [DisplayName("資料型態")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string data_type { get; set; }

        [DisplayName("資料型態")]
        [ReadOnly(true)]
        public string data_name { get; set; }


        [DisplayName("資料長度")]
        [Required(ErrorMessage = "請輸入{0}")]
        public Int32 field_length { get; set; }


        [DisplayName("資料來源代號")]  
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string select_code { get; set; }


        [DisplayName("是否為鍵值")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string is_key { get; set; }

        [DisplayName("是否必填")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string need_value { get; set; }

        [DisplayName("是否能重複")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string is_multi { get; set; }





        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}