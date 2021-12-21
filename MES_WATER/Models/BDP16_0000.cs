using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MES_WATER.Models
{
    public class BDP16_0000
    {
        [DisplayName("待辦代號")]
        public string todo_code { get; set; }

        [DisplayName("待辦名稱")]
        public string todo_name { get; set; }

        [DisplayName("待辦類別")]
        public string todo_type { get; set; }

        [DisplayName("外部連結")]
        public string todo_url { get; set; }

        [DisplayName("外部連結鍵值")]
        public string todo_key { get; set; }

        [DisplayName("是否使用")]
        public string is_use { get; set; }

        [DisplayName("是否已完成")]
        public string is_ok { get; set; }

        [DisplayName("使用者帳號")]
        public string usr_code { get; set; }

        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}