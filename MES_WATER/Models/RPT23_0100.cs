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
    public class RPT23_0100
    {
        [DisplayName("單號")]
        public string DOC_NO { get; set; }

        [DisplayName("序號")]
        public string SequenceNumber { get; set; }

        [DisplayName("採購回覆")]
        public string buy_reply { get; set; }

        [DisplayName("倉管回覆")]
        public string store_reply { get; set; }

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