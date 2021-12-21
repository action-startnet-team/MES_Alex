﻿using System;
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
    public class EMB18_0000
    {
        [Key]
        [DisplayName("故障現象編號")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(20, ErrorMessage = "長度最多{1}個字!")]
        public string fault_code { get; set; }

        [DisplayName("故障現象名稱")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(30, ErrorMessage = "長度最多為{1}個字!")]
        public string fault_name { get; set; }

        [DisplayName("備註")]
        [StringLength(100, ErrorMessage = "長度最多為{1}個字!")]
        public string cmemo { get; set; }

        [DisplayName("是否使用")]
        [Required(ErrorMessage = "請輸入{0}")]
        [StringLength(1, ErrorMessage = "長度最多為{1}個字!")]
        public string is_use { get; set; }



        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }
}