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
    public class RPT23_0000
    {
        [DisplayName("訂單單號")]
        public string DOC_NO { get; set; }

        [DisplayName("序號")]
        public string SequenceNumber { get; set; }


        [DisplayName("預計交貨日")]
        public string PLAN_DELIVERY_DATE { get; set; }

        [DisplayName("業務回覆")]
        public string bussiness_reply { get; set; }

        [DisplayName("生管回覆")]
        public string production_reply { get; set; }

        [DisplayName("出貨回覆")]
        public string shipment_reply { get; set; }

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