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
    public class MBA_E00
    {
        [Key]
        [DisplayName("MES生產資料")]
        public Guid MBA_E00_ID { get; set; }

        [DisplayName("工單單號")]
        public string MO_DOC_NO { get; set; }

        [DisplayName("入庫日期")]
        public DateTime TRANSACTION_DATE { get; set; }

        [DisplayName("入庫數量")]
        public decimal QTY { get; set; }

        [DisplayName("不良品")]
        public decimal SCRAP_QTY { get; set; }

        [DisplayName("生產狀態")]
        public Guid MO_STATUS { get; set; }

        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}
