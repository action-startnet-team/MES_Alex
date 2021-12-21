using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MES_WATER.Models
{
    
    public class JqGridQueryData
    {
        
        public string query_type { get; set; }

        public List<FieldData> query_conditions { get; set; }

        public class FieldData
        {
            public string field_code { get; set; }
            public string field_operator { get; set; }
            public  string field_value { get; set; }
        }

        // 初始 query_conditions
        public JqGridQueryData()
        {
            query_conditions = new List<FieldData>();
        }

        /// <summary>
        /// 選取指定欄位的查詢資料( Contains 篩選 )
        /// </summary>
        /// <param name="pFieldCode">欄位代碼</param>
        /// <returns></returns>
        public List<FieldData> GetParaConds(string pFieldCode)
        {
            return this.query_conditions.Where(x => x.field_code.Contains(pFieldCode)).ToList();
        }

        /// <summary>
        /// 取得欄位的起始值或結束值
        /// </summary>
        /// <param name="pFieldCode">欄位代碼</param>
        /// <param name="pStartOrEnd">輸入 S 或 E  ( Start / End ) </param>
        /// <returns></returns>
        public string GetParaConds(string pFieldCode, string pStartOrEnd)
        {
            string result = "";
            if (GetParaConds(pFieldCode).Count <= 0)
            {
                return result;
            }
            switch (pStartOrEnd.ToUpper())
            {
                case "S":
                    result = GetParaConds(pFieldCode)[0].field_value;
                    break;
                case "E":
                    if (GetParaConds(pFieldCode).Count > 1)
                    {
                        result = GetParaConds(pFieldCode)[1].field_value;
                    }
                    else
                    {
                        //result = GetParaConds(pFieldCode)[0].field_value;
                        result = "";
                    }
                    break;
                default:
                    result = GetParaConds(pFieldCode)[0].field_value;
                    break;
            }
            return result;
        }

        /// <summary>
        /// 取得某個欄位值，第二個參數以"S"和"E"找出起始或結束值，若沒有結束值則回傳空字串
        /// </summary>
        /// <param name="pFieldCode"></param>
        /// <param name="pStartOrEnd"></param>
        /// <returns></returns>
        public string find(string pColCode, string pStartOrEnd = "S")
        {
            return GetParaConds(pColCode, pStartOrEnd);
        }


    } // end class
}