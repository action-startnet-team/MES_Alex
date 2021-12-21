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
    public class BDP08_0000
    {
        [Key]
        [DisplayName("使用者帳號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string usr_code { get; set; }

        [DisplayName("使用者名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(32, ErrorMessage = "長度最多為{1}個字!")]
        public string usr_name { get; set; }

        [DisplayName("密碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(100, ErrorMessage = "長度最多為{1}個字!")]
        [HiddenInJqgrid]
        public string usr_pass { get; set; }

        [DisplayName("確認密碼")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(100, ErrorMessage = "長度最多為{1}個字!")]
        [HiddenInJqgrid]
        public string usr_pass_chk { get; set; }

        [DisplayName("連絡電話一")]
        [StringLength(16, ErrorMessage = "長度最多為{1}個字!")]
        public string usr_tel1 { get; set; }

        [DisplayName("連絡電話二")]
        [StringLength(16, ErrorMessage = "長度最多為{1}個字!")]
        public string usr_tel2 { get; set; }

        [DisplayName("連絡mail")]
        [StringLength(100, ErrorMessage = "長度最多為{1}個字!")]
        public string usr_mail { get; set; }

        [DisplayName("權限類別")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多為{1}個字!")]
        public string limit_type { get; set; }

        [DisplayName("權限類別名稱")]
        public string limit_type_name { get; set; }

        [DisplayName("角色代號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(4, ErrorMessage = "長度最多為{1}個字!")]
        public string grp_code { get; set; }

        [DisplayName("角色名稱")]
        public string grp_name { get; set; }

        [DisplayName("是否使用")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多為{1}個字!")]
        public string is_use { get; set; }

        [DisplayName("部門代號")]
        [StringLength(20, ErrorMessage = "長度最多為{1}個字!")]
        public string dep_code { get; set; }

        [DisplayName("部門名稱")]
        public string dep_name { get; set; }

        [DisplayName("職稱代號")]
        [StringLength(20, ErrorMessage = "長度最多為{1}個字!")]
        public string dut_code { get; set; }

        [DisplayName("職稱名稱")]
        public string dut_name { get; set; }

        [DisplayName("供應商代號")]
        [StringLength(50, ErrorMessage = "長度最多為{1}個字!")]
        public string sup_code { get; set; }

        [DisplayName("token")]
        [StringLength(20, ErrorMessage = "長度最多為{1}個字!")]
        public string token { get; set; }


        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}