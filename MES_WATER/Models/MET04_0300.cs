using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class MET04_0300
    {
        [Key]
        [DisplayName("報工單號")]
        public string ureport_code { get; set; }

        [DisplayName("報工日期")]
        public string ureport_date { get; set; }

        [DisplayName("製令單號")]
        public string mo_code { get; set; }

        [DisplayName("物料編號")]
        public string pro_code { get; set; }

        [DisplayName("數量")]
        public decimal pro_qty { get; set; }

        [DisplayName("單位")]
        public string pro_unit { get; set; }

        [DisplayName("批號")]
        public string lot_no { get; set; }

        [DisplayName("建立日期")]
        public string ins_date { get; set; }

        [DisplayName("建立時間")]
        public string ins_time { get; set; }

        [DisplayName("使用者編號")]
        public string usr_code { get; set; }

        [DisplayName("SAP號碼")]
        public string sap_code { get; set; }

        [DisplayName("SAP計數")]
        public string sap_no { get; set; }

        [DisplayName("SAP訊息")]
        public string sap_message { get; set; }

        public string is_del { get; set; }


        [HiddenInJqgrid]
        [DisplayName("是否能刪除(控制用)")]
        public string can_delete { get; set; }
        [HiddenInJqgrid]
        [DisplayName("是否能修改(控制用)")]
        public string can_update { get; set; }
    }
}