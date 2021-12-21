using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MES_WATER.Controllers
{
    public class CustomJqgridController : Controller
    {
        public class ColModel
        {
            string name { get; set; }
            int width { get; set; }
            bool hidden { get; set; }
        }
        public List<int> Get_ColWidth(string pPrgCode, string pViewCode, string pUsrCode)
        {
            List<int> list = new List<int>();

            return list;
        }

        public List<bool> Get_ColVis(string pPrgCode, string pViewCode, string pUsrCode)
        {
            List<bool> list = new List<bool>();
            return list;
        }

    }
}