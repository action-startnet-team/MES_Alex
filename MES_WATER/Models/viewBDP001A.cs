using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace MES_WATER.Models
{
    public class viewBDP001A
    {
        [DisplayName("功能代號")]
        public string prg_code { get; set; }

        [DisplayName("功能名稱")]
        public string prg_name { get; set; }

        [DisplayName("是否使用")]
        public string is_use { get; set; }

        [DisplayName("A.新增")]
        public string a { get; set; }

        [DisplayName("M.修改")]
        public string m { get; set; }

        [DisplayName("D.刪除")]
        public string d { get; set; }

        [DisplayName("E.匯出")]
        public string e { get; set; }

        [DisplayName("P.印表")]
        public string p { get; set; }

    }
}