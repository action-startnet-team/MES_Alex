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
    public class WMT0200
    {
        [Key]
        [DisplayName("識別碼")]
        [StringLength(32, ErrorMessage = "長度最多{1}個字!")]
        public string wmt0200 { get; set; }

        [DisplayName("單別")]
        [StringLength(4, ErrorMessage = "長度最多{1}個字!")]
        public string rel_type { get; set; }

        [DisplayName("單號")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string rel_code { get; set; }

        [DisplayName("序號")]
        public int scr_no { get; set; }

        [DisplayName("異動類別")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string ins_type { get; set; }

        [DisplayName("異動類別名稱")]
        public string ins_type_name { get; set; }

        [DisplayName("單據日期")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public string sto_date { get; set; }

        [DisplayName("單據時間")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [HiddenInJqgrid]
        public string sto_time { get; set; }

        [DisplayName("倉庫編號")]
        [StringLength(6, ErrorMessage = "長度最多{1}個字!")]
        public string sto_code { get; set; }

        [DisplayName("倉庫名稱")]
        public string sto_name { get; set; }

        [DisplayName("儲位")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string loc_code { get; set; }

        [DisplayName("儲位名稱")]
        public string loc_name { get; set; }

        [DisplayName("容器代號")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string container { get; set; }

        [DisplayName("容器名稱")]
        public string container_name { get; set; }

        [DisplayName("物料編號")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string pro_code { get; set; }

        [DisplayName("物料名稱")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string pro_name { get; set; }

        [DisplayName("批號")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string lot_no { get; set; }

        [DisplayName("數量")]
        public decimal pro_qty { get; set; }

        [DisplayName("SAP回傳訊息")]
        public string sap_code { get; set; }

        //[DisplayName("客戶")]
        //[StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        //public string cus_code { get; set; }

        //[DisplayName("刷讀品項的來源識別碼")]
        //public int wmt0100 { get; set; }

        [DisplayName("異動人員")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string ins_user { get; set; }

        [DisplayName("異動人員名稱")]
        public string usr_name { get; set; }

        [DisplayName("異動日期")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public string ins_date { get; set; }

        [DisplayName("異動時間")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [HiddenInJqgrid]
        public string ins_time { get; set; }

        [DisplayName("QR條碼")]
        [StringLength(200, ErrorMessage = "長度最多{1}個字!")]
        public string barcode { get; set; }

        //[DisplayName("來源單號")]
        //[StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        //public string sor_no { get; set; }

        //[DisplayName("供應商編號")]
        //[StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        //public string tra_code { get; set; }

        [DisplayName("SKU識別碼")]
        [StringLength(32, ErrorMessage = "長度最多{1}個字!")]
        public string identifier { get; set; }

        [DisplayName("廠商編號")]
        [HiddenInJqgrid]
        public string sup_code { get; set; }

        [DisplayName("廠商名稱")]
        [HiddenInJqgrid]
        public string sup_name { get; set; }

        [DisplayName("廠商批號")]
        [HiddenInJqgrid]
        public string sup_lot_no { get; set; }

        [DisplayName("製造日期")]
        [HiddenInJqgrid]
        public string mft_date { get; set; }

        [DisplayName("有效日期")]
        [HiddenInJqgrid]
        public string exp_date { get; set; }
        


        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}