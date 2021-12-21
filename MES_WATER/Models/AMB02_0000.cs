using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MES_WATER.Models
{
    public class AMB02_0000
    {
        [Key]
        [DisplayName("識別碼")]
        public int amb02_0000 { get; set; }

        [DisplayName("警報項目編號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string alm_code { get; set; }

        [DisplayName("使用者帳號")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string usr_code { get; set; }

        [DisplayName("使用者名稱")]
        public string usr_name { get; set; }

        [DisplayName("機台代號")]
        public string mac_code { get; set; }

        [DisplayName("機台名稱")]
        public string mac_name { get; set; }

        [DisplayName("使用標記")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string is_use { get; set; }


        [DisplayName("警報開始時間")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string time_start { get; set; }

        [DisplayName("警報結束時間")]
        [StringLength(8, ErrorMessage = "長度最多{1}個字!")]
        public string time_end { get; set; }

        [DisplayName("警報訊息")]
        [StringLength(100, ErrorMessage = "長度最多{1}個字!")]
        public string alm_message { get; set; }


        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}