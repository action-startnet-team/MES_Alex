using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class MEB43_0100
    {
        [Key]
        [DisplayName("識別碼")]
        public int meb43_0100 { get; set; }

        [DisplayName("不良現象代號")]
        public string ng_code { get; set; }

        [DisplayName("不良現象名稱")]
        public string ng_name { get; set; }

        [DisplayName("不良原因代碼")]
        public string ng_memo_code { get; set; }

        [DisplayName("不良原因名稱")]
        public string ng_memo_name { get; set; }


        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }

    }
}