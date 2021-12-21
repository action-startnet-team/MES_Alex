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
    public class MET02_0000
    {
        [Key]
        [DisplayName("met02_0000")]
        [HiddenInJqgrid]
        public int met02_0000 { get; set; }

        [DisplayName("工單順序")]
        public Int32 seq_no { get; set; }
        
        [DisplayName("工單號碼")]
        public string mo_code { get; set; }

        [DisplayName("預計交貨日")]
        public string plan_out_date { get; set; }

        [DisplayName("需求單號")]
        public string sor_code { get; set; }

        [DisplayName("客戶名稱")]
        public string cus_name { get; set; }

        [DisplayName("品號")]
        public string pro_code { get; set; }

        [DisplayName("品名")]
        public string pro_name { get; set; }

        [DisplayName("規格")]
        public string pro_spec { get; set; }

        //[DisplayName("鋁條型號")]
        //public string spec_a { get; set; }

        //[DisplayName("車圈尺寸")]
        //public string spec_b { get; set; }

        //[DisplayName("車圈孔數")]
        //public string spec_c { get; set; }

        [DisplayName("車圈型號屬性值")]
        public string spec_1 { get; set; }

        [DisplayName("車圈型號描述值")]
        public string spec_2 { get; set; }

        [DisplayName("鋁條型號屬性值")]
        public string spec_3 { get; set; }

        [DisplayName("鋁條型號描述值")]
        public string spec_4 { get; set; }

        [DisplayName("車圈尺寸屬性值")]
        public string spec_5 { get; set; }

        [DisplayName("車圈尺吋描述值")]
        public string spec_6 { get; set; }

        [DisplayName("車圈孔數屬性值")]
        public string spec_7 { get; set; }

        [DisplayName("車圈孔數描述值")]
        public string spec_8 { get; set; }

        

        [DisplayName("計畫產量")]
        public decimal plan_qty { get; set; }

        [DisplayName("完成數量")]
        public decimal QTY { get; set; }

        [DisplayName("進度")]
        //[HiddenInJqgrid]
        public string MO_STATUS { get; set; }

        //[DisplayName("進度")]
        //public string MO_STATUS2 { get; set; }

        [DisplayName("排程別")]
        public string seq_type { get; set; }

        [DisplayName("工廠ID")]
        public string PLANT_ID { get; set; }

        [DisplayName("工廠編號")]
        public string PLANT_CODE { get; set; }

        [DisplayName("工廠名稱")]
        public string PLANT_NAME { get; set; }

        [DisplayName("工作中心")]
        public string WORK_CENTER_ID { get; set; }

        [DisplayName("工作中心編號")]
        public string WORK_CENTER_CODE { get; set; }

        [DisplayName("工作中心名稱")]
        public string WORK_CENTER_NAME { get; set; }

        [DisplayName("製程ID")]
        public string OPERATION_ID { get; set; }

        [DisplayName("製程編號")]
        public string OPERATION_CODE { get; set; }

        [DisplayName("製程名稱")]
        public string OPERATION_NAME { get; set; }

        [DisplayName("工單ID")]
        public string MO_ID { get; set; }

        [DisplayName("機台ID")]
        public string MACHINE_ID { get; set; }

        [DisplayName("機台編號")]
        public string MACHINE_CODE { get; set; }

        [DisplayName("機台名稱")]
        public string MACHINE_DESCRIPTION { get; set; }

        [DisplayName("入庫日期")]
        public DateTime TRANSACTION_DATE { get; set; }

        [DisplayName("不良品")]
        public decimal SCRAP_QTY { get; set; }

        [DisplayName("作業員")]
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

        [DisplayName("工單序號")]
        public int OP_SEQ { get; set; }

        [DisplayName("建立日期")]
        public DateTime ins_date { get; set; }

        [DisplayName("預計開工日期")]
        public DateTime PLAN_START_DATE { get; set; }


        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}
