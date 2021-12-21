using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MES_WATER.Models
{
    public class JqgridButton
    {
        public string type { get; set; }
        public string icon_class { get; set; }
        public string title { get; set; }
        public string url { get; set; }

        public string clickJs { get; set; }
        public string showCondJs { get; set; }

    }
}