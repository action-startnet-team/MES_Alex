using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MES_WATER.Models
{
    public class AMB01_0000
    {
        [Key]
        [DisplayName("識別碼")]
        public int amb01_0000 { get; set; }

        [DisplayName("警報項目編號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(10, ErrorMessage = "長度最多{1}個字!")]
        public string alm_code { get; set; }

        [DisplayName("警報項目名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(30, ErrorMessage = "長度最多{1}個字!")]
        public string alm_name { get; set; }


        [DisplayName("使用標記")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string is_use { get; set; }

        [DisplayName("關聯TABLE")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string alm_table { get; set; }


        [DisplayName("關聯FIELD")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string alm_field { get; set; }

        [DisplayName("警報型態")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多{1}個字!")]
        public string alm_type { get; set; }

        [DisplayName("警報型態名稱")]
        public string alm_type_name { get; set; }

        [DisplayName("上限值")]
        public double upper_limit { get; set; }

        [DisplayName("下限值")]
        public double lower_limit { get; set; }
        
        [DisplayName("公式")]
        public string alm_formula { get; set; }
         
        
        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}