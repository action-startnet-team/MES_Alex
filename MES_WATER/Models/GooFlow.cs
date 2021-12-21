using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MES_WATER.Models
{
    public class GooFlow
    {
        public string title { get; set; }

        public Dictionary<string, node> nodes { get; set; }
        public Dictionary<string, line> lines { get; set; }

        public object areas { get; set; }

        public int initNum { get; set; }


        public class node
        {
            public string name { get; set; }
            public int left { get; set; }
            public string type { get; set; }
            public int top { get; set; }
            public int width { get; set; }
            public int height { get; set; }

            // 
            public bool alt { get; set;  }
        }

        public class line
        {
            public string name { get; set; }
            public string type { get; set; }
            public string from { get; set; }
            public string to { get; set; }

            // 有轉折的線會出現的屬性
            public int M { get; set; }

            //
            public bool alt { get; set;  }
        }


    }
}