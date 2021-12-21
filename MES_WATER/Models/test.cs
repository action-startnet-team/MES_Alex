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
    public class test
    {
        [DisplayName("工廠代號")]
        [StringLength(10, ErrorMessage = "長度最多為{1}個字!")]
        [Required(ErrorMessage = "請輸入{0}")]
        public string factory_code { get; set; }
    }
}