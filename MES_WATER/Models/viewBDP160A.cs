using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace MVC.Models
{
    public class viewBDP160A
    {
        [DisplayName("待辦名稱")]
        public string todo_name { get; set; }

        [DisplayName("待辦名稱")]
        public string todo_type { get; set; }

        [DisplayName("外部連結")]
        public string todo_url { get; set; }

        [DisplayName("外部連結鍵值")]
        public string todo_key { get; set; }

        [DisplayName("待辦筆數")]
        public string todo_count { get; set; }
    }
}