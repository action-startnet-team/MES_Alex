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
    public class MBA_E10
    {
        [Key]
        [DisplayName("MES生產資料")]
        public Guid MBA_E10_ID { get; set; }

        [DisplayName("工廠ID")]
        public Guid PLANT_ID { get; set; }

        [DisplayName("工廠編號")]
        public string PLANT_CODE { get; set; }

        [DisplayName("工廠名稱")]
        public string PLANT_NAME { get; set; }

        [DisplayName("工作中心")]
        public Guid WORK_CENTER_ID { get; set; }

        [DisplayName("工作中心編號")]
        public string WORK_CENTER_CODE { get; set; }

        [DisplayName("工作中心名稱")]
        public string WORK_CENTER_NAME { get; set; }

        [DisplayName("製程ID")]
        public Guid OPERATION_ID { get; set; }

        [DisplayName("製程編號")]
        public string OPERATION_CODE { get; set; }

        [DisplayName("製程名稱")]
        public string OPERATION_NAME { get; set; }

        [DisplayName("工單ID")]
        public Guid MO_ID { get; set; }

        [DisplayName("工單單號")]
        public string MO_DOC_NO { get; set; }

        [DisplayName("入庫日期")]
        public DateTime TRANSACTION_DATE { get; set; }

        [DisplayName("入庫數量")]
        public decimal QTY { get; set; }

        [DisplayName("不良品")]
        public decimal SCRAP_QTY { get; set; }

        [DisplayName("作業員ID")]
        public Guid XOPERATOR_ID { get; set; }

        [DisplayName("作業員編號")]
        public string XOPERATOR_CODE { get; set; }

        [DisplayName("作業員名稱")]
        public string XOPERATOR_NAME { get; set; }

        [DisplayName("班別ID")]
        public Guid XWORK_SHIFT_ID { get; set; }

        [DisplayName("班別編號")]
        public string XWORK_SHIFT_CODE { get; set; }

        [DisplayName("班別名稱")]
        public string XWORK_SHIFT_NAME { get; set; }

        [DisplayName("機台ID")]
        public Guid XMACHINE_ID { get; set; }

        [DisplayName("機台編號")]
        public string XMACHINE_CODE { get; set; }

        [DisplayName("機台名稱")]
        public string XMACHINE_NAME { get; set; }

        [DisplayName("狀態")]
        public string STATUS { get; set; }

        [DisplayName("錯誤訊息")]
        public string ERRMSG { get; set; }

        [DisplayName("工單序號")]
        public int OP_SEQ { get; set; }
        

        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}
