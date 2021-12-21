using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MES_WATER.Models;
using System.Linq.Expressions;
using System.Reflection;

namespace MES_WATER.Helpers
{

    public static class CustomHelper
    {

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="name">選單select的屬性</param>
        ///// <param name="options">選單option的陣列</param>
        ///// <returns></returns>
        //public static MvcHtmlString DropdownList(string pCtlName , List<DDLList> options, string pDefaultValue)
        //{
        //    Comm comm = new Comm();
        //    var dropdown = new TagBuilder("select");            
        //    dropdown.Attributes.Add("name", pCtlName);

        //    StringBuilder option = new StringBuilder();
        //    option.Append("<option value='' >請選擇</option>");
        //    if (option != null)
        //    {
        //        string selected = "";
        //        foreach (DDLList item in options)
        //        {
        //            if (item.field_code == pDefaultValue)
        //            {
        //                selected = " selected ";
        //            }
        //            else
        //            {
        //                selected = "";
        //            }

        //            //判斷show_type給選項
        //                switch (item.show_type)
        //            {
        //                case "A": // 預設
        //                    option = option.Append("<option " + selected + " value ='" + item.field_code + "'>" + item.field_code + "-" + item.field_name + "</option>");
        //                    break;
        //                case "B": // 只秀代碼
        //                    option = option.Append("<option " + selected + " value='" + item.field_code + "'>" + item.field_code + "</option>");
        //                    break;
        //                case "C": // 只秀名稱
        //                    option = option.Append("<option " + selected + " value ='" + item.field_code + "'>" + item.field_name + "</option>");
        //                    break;
        //                default:
        //                    break;
        //            }

        //        }
        //    }
        //    dropdown.InnerHtml = option.ToString();
        //    return MvcHtmlString.Create(dropdown.ToString(TagRenderMode.Normal));
        //}

        /// <summary>
        /// 產生Html Option選項
        /// </summary>
        /// <param name="options">Option資料來源</param>
        /// <param name="pDefaultValue">預設值</param>
        /// <param name="pFirstOption">是否需要第一個選項(請選擇)</param>
        /// <param name="pFirstOptionText">自訂義[請選擇]的文字，僅在pFirstOption為true時有效</param>
        /// <returns></returns>
        public static MvcHtmlString Get_Option(List<DDLList> options, string pDefaultValue, Boolean pFirstOption, string pFirstOptionText = "", string pSeparator = " - ")
        {
            StringBuilder option = new StringBuilder();
            if (pFirstOption) {
                if (string.IsNullOrEmpty(pFirstOptionText))
                {
                    option.Append("<option value='' >--請選擇--</option>");
                } else
                {
                    option.Append("<option value='' >" + pFirstOptionText + "</option>");
                }
            }
            string selected = "";
            foreach (DDLList item in options)
            {
                selected = (item.field_code == pDefaultValue ? " selected " : "");
                //判斷show_type給選項
                switch (item.show_type)
                {
                    case "A": // 預設全秀
                        option = option.Append("<option " + selected + " value ='" + item.field_code + "'>" + item.field_code + pSeparator + item.field_name + "</option>");
                        break;
                    case "B": // 只秀代碼
                        option = option.Append("<option " + selected + " value='" + item.field_code + "'>" + item.field_code + "</option>");
                        break;
                    case "C": // 只秀名稱
                        option = option.Append("<option " + selected + " value ='" + item.field_code + "'>" + item.field_name + "</option>");
                        break;
                    default:  // if show_type is empty or null
                        option = option.Append("<option " + selected + " value ='" + item.field_code + "'>" + item.field_code + pSeparator + item.field_name + "</option>");
                        break;
                }
            }
            return MvcHtmlString.Create(option.ToString());
        }

        /// <summary>
        /// 產生Html Option選項
        /// </summary>
        /// <param name="options">Option資料來源</param>
        /// <param name="pDefaultValue">預設值</param>
        /// <param name="pFirstOption">是否需要第一個選項(請選擇)</param>
        /// <returns></returns>
        public static MvcHtmlString Get_MutiOption(List<DDLList> options, string pDefaultValue, Boolean pFirstOption)
        {
            StringBuilder option = new StringBuilder();
            if (pFirstOption) { option.Append("<option value='' >--請選擇--</option>"); }
            string selected = "";
            foreach (DDLList item in options)
            {
                selected = (pDefaultValue.Contains(item.field_code) ? " selected " : "");
                //判斷show_type給選項
                switch (item.show_type)
                {
                    case "A": // 預設全秀
                        option = option.Append("<option " + selected + " value ='" + item.field_code + "'>" + item.field_code + "-" + item.field_name + "</option>");
                        break;
                    case "B": // 只秀代碼
                        option = option.Append("<option " + selected + " value='" + item.field_code + "'>" + item.field_code + "</option>");
                        break;
                    case "C": // 只秀名稱
                        option = option.Append("<option " + selected + " value ='" + item.field_code + "'>" + item.field_name + "</option>");
                        break;
                    default: // if show_type is empty or null
                        option = option.Append("<option " + selected + " value ='" + item.field_code + "'>" + item.field_code + "-" + item.field_name + "</option>");
                        break;
                }
            }
            return MvcHtmlString.Create(option.ToString());
        }

        public static MvcHtmlString Set_CheckBox(string pValue, string pPrgCode, string pLimitStr)
        {
            StringBuilder option = new StringBuilder();
            switch (pValue)
            {
                case "Y":
                    option.Append("<input id=\"checkbox-" + pPrgCode + "-" + pLimitStr + "\" name=\"checkbox-" + pPrgCode + "-" + pLimitStr + "\" class=\"ace input-lg\" checked=\"checked\"  type=\"checkbox\" />  <span class=\"lbl bigger-120\"/>");
                    break;
                case "N":
                    option.Append("<input id=\"checkbox-" + pPrgCode + "-" + pLimitStr + "\" name=\"checkbox-" + pPrgCode + "-" + pLimitStr + "\" class=\"ace input-lg\" type=\"checkbox\" />  <span class=\"lbl bigger-120\"/>");
                    break;
                case "L":
                    option.Append("<input id=\"checkbox-" + pPrgCode + "-" + pLimitStr + "\" name=\"checkbox-" + pPrgCode + "-" + pLimitStr + "\" class=\"ace input-lg\" type=\"checkbox\" disabled />");
                    break;
                default:
                    option.Append("<input id=\"checkbox-" + pPrgCode + "-" + pLimitStr + "\" name=\"checkbox-" + pPrgCode + "-" + pLimitStr + "\" class=\"ace input-lg\" type=\"checkbox\" />  <span class=\"lbl bigger-120\"/>");
                    break;
            }
            return MvcHtmlString.Create(option.ToString());
        }


        public static MvcHtmlString Get_Label<T>(T pModel,string pPrgCode, string pField, object htmlAttributes = null) {
            GetData GD = new GetData();
            GetModelValidation gmv = new GetModelValidation();
            StringBuilder htmlStr = new StringBuilder();
            string sHtmlAttribute = "";
            string sDefaultStr = "<label class = 'col-sm-4 col-xs-5 control-label no-padding-right'></label>";

            PropertyInfo Info = pModel.GetType().GetProperty(pField);
            //預設樣式，保持原本的格式
            if (Info == null) { return MvcHtmlString.Create(sDefaultStr); }

            string sFieldName = gmv.GetDisplayName(Info); //欄位名稱
            bool bRequired = gmv.GetRequired(Info).AllowEmptyStrings; //是否必填
                   
            PropertyInfo[] AttributesArray = htmlAttributes.GetType().GetProperties();
            //巡覽htmlAttributes的欄位
            for (int i = 0; i < AttributesArray.Length; i++) {
                PropertyInfo Attribute = AttributesArray[i];
                string sAttribute = Attribute.Name; //屬性名稱
                string sAttributeStr = GD.Get_ObjectValue(htmlAttributes, sAttribute); //屬性內容

                //特殊邏輯------------------------------------------------------------
                switch (sAttribute)
                {
                    case "class":
                        //判斷欄位若為必填，則class加上required屬性(後方會有星號)
                        if (!bRequired) { sAttributeStr += " required "; }
                        break;
                    default:
                        break;
                }
                //-------------------------------------------------------------------

                sHtmlAttribute += " " + sAttribute + " = '"+ sAttributeStr + "' ";               
            }

            //檢查提示框table是否有值
            string sTip = GD.Get_Tip(pPrgCode, pField);
            if (!string.IsNullOrEmpty(sTip)) {
                sTip = "<span title='"+ sTip + "'> 💬 </span>";
                sFieldName = sTip + sFieldName;
            }          

            htmlStr.Append("<label " + sHtmlAttribute + ">"+ sFieldName + "</label>");

            return MvcHtmlString.Create(htmlStr.ToString());
        }

    }
}