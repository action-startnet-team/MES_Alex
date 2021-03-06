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
    public class MED09_0000
    {
        [Key]
        [DisplayName("識別碼")]
        public int med09_0000 { get; set; }

        [DisplayName("製令號碼")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string mo_code { get; set; }

        [DisplayName("派工單")]
        [StringLength(30, ErrorMessage = "長度最多{1}個字!")]
        public string wrk_code { get; set; }
        
        [DisplayName("製程代碼")]
        [StringLength(30, ErrorMessage = "長度最多{1}個字!")]
        public string work_code { get; set; }

        [DisplayName("製程名稱")]
        public string work_name { get; set; }

        [DisplayName("站別號碼")]
        [StringLength(30, ErrorMessage = "長度最多{1}個字!")]
        public string station_code { get; set; }

        [DisplayName("站別名稱")]
        public string station_name { get; set; }
        
        [DisplayName("機台代碼")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string mac_code { get; set; }

        [DisplayName("機台名稱")]
        public string mac_name { get; set; }

        [DisplayName("產品代碼")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string pro_code { get; set; }

        [DisplayName("產品名稱")]
        public string pro_name { get; set; }

        [DisplayName("批號")]
        [StringLength(30, ErrorMessage = "長度最多{1}個字!")]
        public string pro_lot_no { get; set; }

        [DisplayName("生產數量")]
        public float pro_qty { get; set; }

        [DisplayName("容器編號")]
        public string pallet_code { get; set; }

        [DisplayName("容器名稱")]
        public string pallet_name { get; set; }

        [DisplayName("建立日期")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string ins_date { get; set; }

        [DisplayName("建立時間")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string ins_time { get; set; }

        [DisplayName("使用者編號")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string usr_code { get; set; }

        [DisplayName("使用者名稱")]
        public string usr_name { get; set; }

        [DisplayName("資料說明")]
        [StringLength(200, ErrorMessage = "長度最多{1}個字!")]
        public string des_memo { get; set; }

        [DisplayName("是否異常")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string is_ng { get; set; }

        [DisplayName("強制結案")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string is_end { get; set; }

        [DisplayName("結案說明")]
        public string end_memo { get; set; }

        [DisplayName("結案日期")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string end_date { get; set; }

        [DisplayName("結案時間")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string end_time { get; set; }

        [DisplayName("結案人員")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string end_usr_code { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

        [DisplayName("自訂義01")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string user_field_01 { get; set; }

        [DisplayName("自訂義02")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string user_field_02 { get; set; }

        [DisplayName("自訂義03")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string user_field_03 { get; set; }

        [DisplayName("自訂義04")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string user_field_04 { get; set; }

        [DisplayName("自訂義05")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string user_field_05 { get; set; }

        [DisplayName("自訂義06")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string user_field_06 { get; set; }

        [DisplayName("自訂義07")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string user_field_07 { get; set; }

        [DisplayName("自訂義08")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string user_field_08 { get; set; }

        [DisplayName("自訂義09")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string user_field_09 { get; set; }

        [DisplayName("自訂義10")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string user_field_10 { get; set; }
    }
}