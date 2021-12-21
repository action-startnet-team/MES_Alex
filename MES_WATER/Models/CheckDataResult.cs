using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MES_WATER.Models
{
    public class CheckDataResult
    {
        public bool bIsOK { get; set; }
        public string message { get; set; }

        public CheckDataResult()
        {
            bIsOK = true;
            message = "";
        }
    }
}