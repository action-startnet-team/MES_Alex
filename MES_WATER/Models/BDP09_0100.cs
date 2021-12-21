using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MES_WATER.Models
{
    public class BDP09_0100
    {
        [DisplayName("資料代碼")]
        public Int32 bdp09_0100 { get; set; }

        [DisplayName("角色代號")]
        public string grp_code { get; set; }


        [DisplayName("程式代碼")]
        public string prg_code { get; set; }

        [DisplayName("權限字串")]
        public string limit_str { get; set; }

        [DisplayName("是否使用")]
        public string is_use { get; set; }

        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}