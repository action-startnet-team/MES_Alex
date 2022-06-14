using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MES_WATER.Models
{
    public class ECB05_0000
    {
        [Key]
        [DisplayName("ERP欄位代碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string ERP_FIELD_CODE { get; set; }

        [DisplayName("匯入中文名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string ERP_FORM_NAME { get; set; }

        [DisplayName("匯出中文名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string ERP_FIELD_NAME { get; set; }

        [DisplayName("匯出固定內容")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string ERP_FIELD_VALUE { get; set; }

        [DisplayName("匯出後置字元")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string BACK_VALUE { get; set; }

        [DisplayName("匯出是否空值")]
        [Required(ErrorMessage = "請輸入{0}")]
        public string is_null { get; set; }

        [DisplayName("自行編輯資料內容")]
        [Required(ErrorMessage = "請輸入{0}")]
        public string is_edit { get; set; }


        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}