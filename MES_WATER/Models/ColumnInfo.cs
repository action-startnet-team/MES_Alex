using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MES_WATER.Models
{
    public class ColumnInfo
    {
        public string name { get; set; }
        public string displayName { get; set; }
        public string propertyType { get; set; }
        public bool required { get; set; }
        public bool editable { get; set; }
        public bool readonlyattr { get; set; }
        public string requiredMsg { get; set; }
        public int maxlength { get; set; }
        public string maxlengthMsg { get; set; }
        public string dataType { get; set; }

        public bool hidden { get; set; }

    }
}