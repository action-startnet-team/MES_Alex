using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class MEB05_0000
    {
        [Key]
        [DisplayName("範本代碼")]
        public string samp_code { get; set; }

        [DisplayName("範本名稱")]
        public string samp_name { get; set; }

        [DisplayName("備註")]
        public string cmemo { get; set; }

        [DisplayName("建立日期")]
        [NotMapped]
        public DateTime ins_date { get; set; }

        [DisplayName("建立者")]
        [NotMapped]
        public string usr_code { get; set; }


        [DisplayName("流程圖json")]
        [NotMapped]
        public string gooflow_json { get; set; }


        [DisplayName("是否能刪除(控制用)")]
        [HiddenInJqgrid]
        public string can_delete { get; set; }

        [DisplayName("是否能修改(控制用)")]
        [HiddenInJqgrid]
        public string can_update { get; set; }

    }




}