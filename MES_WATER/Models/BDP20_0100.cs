using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MES_WATER.Models
{
    public class BDP20_0100
    {
        [DisplayName("資料代號")]
        public Int32 bdp20_0100 { get; set; }

        [DisplayName("使用者代號")]
        public string usr_code { get; set; }


        [DisplayName("程式代號")]
        public string prg_code { get; set; }

        [DisplayName("使用日期")]
        public string usr_date { get; set; }

        [DisplayName("使用時間")]
        public string usr_time { get; set; }

        [DisplayName("使用備註")]
        public string cmemo { get; set; }

        [DisplayName("錯誤訊息")]
        public string exception { get; set; }

        [DisplayName("函數名稱")]
        public string fun_code { get; set; }

        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}