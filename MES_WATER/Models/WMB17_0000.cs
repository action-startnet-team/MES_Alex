using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.ModelBinding;

namespace MES_WATER.Models
{
	public class WMB17_0000
	{
        [DisplayName("料號")]
        public string pro_code { get; set; }

        [DisplayName("品名")]
        public string pro_name { get; set; }

        [DisplayName("批號")]
        public string lot_no { get; set; }

        [DisplayName("倉庫")]
        public string sto_code { get; set; }

        [DisplayName("儲位")]
        public string loc_code { get; set; }

        [DisplayName("庫存日期")]
        public string sto_date { get; set; }
        //public DateTime sto_date { get; set; }

        [DisplayName("數量")]
        public int qty { get; set; }

        [DisplayName("天數")]
        public int days { get; set; }
    }
}