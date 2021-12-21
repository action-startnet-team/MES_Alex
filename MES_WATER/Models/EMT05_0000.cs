using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class EMT05_0000
    {
        [Key]
        [DisplayName("叫修單號")]
        public string call_code { get; set; }

        [DisplayName("叫修日期")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string call_date { get; set; }

        [DisplayName("設備編號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string dev_code {  get; set; }

        [DisplayName("設備名稱")]
        public string dev_name { get; set; }

        [DisplayName("設備點檢表")]
        //[Required(ErrorMessage = "請輸入{0}")]
        public string dev_check_code { get; set; }

        [DisplayName("點檢項目代碼")]
        //[Required(ErrorMessage = "請輸入{0}")]
        public string chk_item_code { get; set; }

        [DisplayName("點檢項目名稱")]
        public string chk_item_name { get; set; }

        [DisplayName("保養計畫表")]
        //[Required(ErrorMessage = "請輸入{0}")]
        public string maintain_code { get; set; }

        [DisplayName("保養計畫表名稱")]
        public string maintain_name { get; set; }

        [NotMapped]
        [DisplayName("保養項目識別碼")]
        //[Required(ErrorMessage = "請輸入{0}")]
        public string emt01_0100 { get; set; }

        [DisplayName("保養項目代碼")]
        //[Required(ErrorMessage = "請輸入{0}")]
        public string main_item_code { get; set; }

        [DisplayName("保養項目名稱")]
        public string main_item_name { get; set; }

        [DisplayName("保養日期")]
        public string maintain_date { get; set; }

        [DisplayName("叫修單說明")]
        [StringLength(200, ErrorMessage = "長度最多{1}個字!")]
        public string call_memo { get; set; }

        [DisplayName("建立日期")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string ins_date { get; set; }

        [DisplayName("建立時間")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string ins_time { get; set; }

        [DisplayName("使用者編號")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string usr_code { get; set; }

        [DisplayName("使用者名稱")]
        public string usr_name { get; set; }


        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }
    }
}