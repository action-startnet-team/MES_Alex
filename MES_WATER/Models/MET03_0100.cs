using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class MET03_0100
    {

        [DisplayName("工單號碼")]
        public string wrk_code { get; set; }

        [DisplayName("途程代碼")]
        public string work_code { get; set; }

        [DisplayName("途程名稱")]
        public string work_name { get; set; }

        [DisplayName("站別名稱")]
        public string station_name { get; set; }

        [DisplayName("工單狀態")]
        public string mo_status { get; set; }

        [DisplayName("建立日期")]
        public string ins_date { get; set; }

        [DisplayName("建立時間")]
        public string ins_time { get; set; }

        [DisplayName("使用者編號")]
        public string usr_code { get; set; }


        //--------------old Model-----------//
        //[Key]
        //[DisplayName("資料識別碼")]
        //[HiddenInJqgrid]
        //public int met03_0100 { get; set; }

        //[DisplayName("工單號碼")]
        //[Required(ErrorMessage = "請輸入{0}")]
        //[StringLength(30, ErrorMessage = "長度最多{1}個字!")]
        //[HiddenInJqgrid]
        //public string wrk_code { get; set; }

        //[DisplayName("產品編號")]
        //[Required(ErrorMessage = "請輸入{0}")]
        //[StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        //[HiddenInJqgrid]
        //public string pro_code { get; set; }

        //[DisplayName("產品名稱")]
        //[ReadOnly(true)]
        //[HiddenInJqgrid]
        //public string pro_name { get; set; }

        //[DisplayName("批號")]
        //[Required(ErrorMessage = "請輸入{0}")]
        //[StringLength(30, ErrorMessage = "長度最多{1}個字!")]
        //[HiddenInJqgrid]
        //public string lot_no { get; set; }

        //[DisplayName("備料明細識別碼")]
        //[HiddenInJqgrid]
        //public int wmt06_0110 { get; set; }
        //--------------option-----------//

        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}