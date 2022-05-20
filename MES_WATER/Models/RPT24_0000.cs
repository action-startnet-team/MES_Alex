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
    public class RPT24_0000
    {
        [DisplayName("品號")]
        public string ITEM_CODE { get; set; }

        [DisplayName("品名")]
        public string ITEM_NAME { get; set; }

        [DisplayName("規格")]
        public string ITEM_SPECIFICATION { get; set; }

        [DisplayName("最後入庫日")]
        public string LAST_RECEIPT_DATE { get; set; }

        [DisplayName("最後出庫日")]
        public string LAST_ISSUE_DATE { get; set; }

        [DisplayName("庫存量")]
        public decimal INVENTORY_QTY { get; set; }


        [DisplayName("呆料天數")]
        public string DAYDIFF { get; set; }

        [DisplayName("上傳日期")]
        public string update_at { get; set; }

        [DisplayName("上傳人員")]
        public string usr_code { get; set; }


        [DisplayName("是否能刪除(控制用)")]
        [NotMapped]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [NotMapped]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}