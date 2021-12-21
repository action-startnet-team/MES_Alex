using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class QMT01_0000
    {
        [Key]
        [DisplayName("採購檢驗編號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string qmt_code { get; set; }

        [DisplayName("採購單號")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string pur_code { get; set; }

        [DisplayName("項目號碼")]
        public Int32 scr_no {  get; set; }

        [DisplayName("物料號碼")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string pro_code { get; set; }

        [DisplayName("物料名稱")]
        public string pro_name { get; set; }

        [DisplayName("類型")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string PLNTY { get; set; }

        [DisplayName("群組代碼")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string PLNNR { get; set; }

        [DisplayName("群組計數")]
        [StringLength(2, ErrorMessage = "長度最多{1}個字!")]
        public string PLNAL { get; set; }

        [DisplayName("節點號碼")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string PLNKN { get; set; }

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



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}