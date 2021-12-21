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
	public class WMB13_0000
	{
        [DisplayName("倉庫")]
        public string sto_name { get; set; }

        [DisplayName("儲位編號")]
        public string loc_code { get; set; }

        [DisplayName("儲位名稱")]
        public string loc_name { get; set; }

        [DisplayName("品號")]
        public string pro_code { get; set; }

        [DisplayName("品名")]
        public string pro_name { get; set; }

        [DisplayName("批號")]
        public string lot_no { get; set; }

        [DisplayName("數量")]
        public double sto_qty { get; set; }


    }
}