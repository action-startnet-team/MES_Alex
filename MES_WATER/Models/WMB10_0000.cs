using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MES_WATER.Models
{
    public class WMB10_0000
    {
        [Key]
        [DisplayName("廠商編號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string sup_code { get; set; }

        [DisplayName("廠商名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(50, ErrorMessage = "長度最多{1}個字!")]
        public string sup_name { get; set; }

        [DisplayName("廠商簡碼")]
        [StringLength(2, ErrorMessage = "長度最多{1}個字!")]
        public string sup_scode { get; set; }

        [DisplayName("是否使用")]
        public string is_use { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }
    }
}