using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class MET01_0000
    {
        [Key]
        [DisplayName("製令號碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string mo_code { get; set; }

        [DisplayName("產品編號")]
        [Required(ErrorMessage = "請輸入{0}")]
        public string pro_code { get; set; }

        [DisplayName("產品名稱")]
        public string pro_name { get; set; }

        [DisplayName("bom代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        public string bom_code { get; set; }

        [DisplayName("bom名稱")]
        public string bom_name { get; set; }


        [DisplayName("來源單號")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string sor_code { get; set; }

        [DisplayName("訂單單號")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string ord_code { get; set; }

        [DisplayName("客戶編號")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string cus_code { get; set; }

        [DisplayName("客戶名稱")]
        public string cus_name { get; set; }

        [DisplayName("計劃開工日")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string plan_start_date { get; set; }

        [DisplayName("計劃完工日")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string plan_end_date { get; set; }

        [DisplayName("計劃出貨日")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string plan_out_date { get; set; }

        [DisplayName("計劃投入線別")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string plan_line_code { get; set; }
        //[DisplayName("最小生產數量")]
        //public decimal pro_qty { get; set; }
        [DisplayName("計劃投入線別名稱")]
        public string plan_line_name { get; set; }

        [DisplayName("計劃產量")]
        [Required(ErrorMessage = "請輸入{0}")]
        public decimal plan_qty { get; set; }

        [DisplayName("批次數量")]
        public string lot_conut { get; set; }

        [DisplayName("單位")]
        [StringLength(3, ErrorMessage = "長度最多{1}個字!")]
        public string pro_unit { get; set; }


        [DisplayName("排程開工日")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string sch_date_s { get; set; }

        [DisplayName("排程結束日")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string sch_date_e { get; set; }

        [DisplayName("排程開工時間")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string sch_time_s { get; set; }

        [DisplayName("排程結束時間")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string sch_time_e { get; set; }

        [DisplayName("工單狀態")]
        public string mo_status { get; set; }

        [DisplayName("工單狀態名稱")]
        public string mo_status_name { get; set; }

        [DisplayName("異常原因")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string err_memo { get; set; }

        [DisplayName("實際開工日")]
        public string mo_start_date { get; set; }

        [DisplayName("實際完工日")]
        public string mo_end_date { get; set; }

        [DisplayName("實際出貨日")]
        public string mo_out_date { get; set; }

        [DisplayName("實際產量")]
        public decimal mo_qty { get; set; }

        [DisplayName("建立日期")]
        public string ins_date { get; set; }

        [DisplayName("建立時間")]
        public string ins_time { get; set; }

        [DisplayName("使用者編號")]
        public string usr_code { get; set; }

        [DisplayName("最後修改日期")]
        public string last_date { get; set; }

        [DisplayName("最後修改時間")]
        public string last_time { get; set; }
        
        [DisplayName("已報工產量")]
        public double up_qty { get; set; }

        [DisplayName("產量是否確認")]
        public string is_pro_ok { get; set; }

        [DisplayName("機時是否確認")]
        public string is_mac_ok { get; set; }

        [DisplayName("備註")]
        [StringLength(200, ErrorMessage = "長度最多{1}個字!")]
        public string mo_memo { get; set; }
        [Required(ErrorMessage = "請輸入{0}")]
        [DisplayName("工單類別")]

        public string mo_type { get; set; }

        [DisplayName("工單類別名稱")]
        public string mo_type_name { get; set; }

        [DisplayName("製令層級")]
        public string mo_level { get; set; }

        [DisplayName("母製令號碼")]
        public string up_mo_code { get; set; }


        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}