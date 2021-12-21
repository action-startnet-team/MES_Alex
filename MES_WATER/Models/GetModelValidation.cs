using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;
using System.Linq.Dynamic;
using MES_WATER.Repository;
using System.Reflection;
using System.ComponentModel;
using System.Web.Mvc;

using System.Web.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES_WATER.Models
{
    public class GetModelValidation
    {
        //公用函數庫
        private Comm comm = new Comm();

        /// <summary>
        /// 取得類別中DisplayName值的清單，第一格功能欄位
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="hasNotMapped">是否抓取設有NotMapped的欄位，預設false不抓</param>
        /// <returns></returns>
        public List<string> Get_DisplayNames<T>(T obj, bool hasNotMapped = false)
        {
            List<string> list = new List<string>();
            list.Add("(功能)");
            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();
            foreach (var info in properties)
            {
                if (!hasNotMapped)
                {
                    object[] attributes = info.GetCustomAttributes(typeof(NotMappedAttribute), false);
                    if (attributes == null || attributes.Length <= 0)
                    {
                        list.Add(GetDisplayName(info));
                    }
                }
                else
                {
                    list.Add(GetDisplayName(info));
                }
            }
            return list;
        }
        public string GetDisplayName(PropertyInfo info)
        {
            if (info == null) return "";

            object[] attributes = info.GetCustomAttributes(typeof(DisplayNameAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                var displayName = (DisplayNameAttribute)attributes[0];
                return displayName.DisplayName;
            }
            return info.Name;
        }

        /// <summary>
        /// 從BDP36_0000取得欄位名稱
        /// </summary>
        /// <param name="pFieldCode">欄位名稱 (class的成員名稱)</param>
        /// <param name="pModelName">class本身的名稱</param>
        /// <returns></returns>
        public string GetDisplayNameFromDB(string pFieldCode, string pModelName)
        {
            try
            {
                string model_name = pModelName;
                string field_code = pFieldCode;

                string sSql = " Select * "
                            + " from BDP36_0000 "
                            + " where model_name = @model_name "
                            + "   and field_code = @field_code ";
                DataTable dtTmp = comm.Get_DataTable(sSql, "model_name,field_code", model_name + "," + field_code);
                if (dtTmp.Rows.Count > 0)
                {
                    return dtTmp.Rows[0]["display_name"].ToString();
                }

                return "";
            }
            catch (Exception)
            {
                return "";
            }

        }

        /// <summary>
        /// 綜合取得欄位名稱
        /// </summary>
        /// <param name="info">對應class中的成員</param>
        /// <param name="type">對應class</param>
        /// <returns></returns>
        public string GetDisplayName(PropertyInfo info, Type type)
        {
            string field_code = info == null ? "" : info.Name;
            string model_name = type == null ? "" : type.Name;

            //先判斷有沒有在資料庫，如果沒有在取得model的設置
            string displayName = GetDisplayNameFromDB(field_code, model_name);
            if (!string.IsNullOrEmpty(displayName))
            {
                return displayName;
            }
            else
            {
                displayName = GetDisplayName(info);
                return displayName;
            }
        }



        //test
        public void SetDisplayName(PropertyInfo info)
        {
            object[] attributes = info.GetCustomAttributes(typeof(DisplayNameAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                var displayName = (DisplayNameAttribute)attributes[0];
            }
            var a = new DisplayNameAttribute();
        }

        public StringLengthAttribute GetStringLength(PropertyInfo info)
        {
            object[] attributes = info.GetCustomAttributes(typeof(StringLengthAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                var stringlength = (StringLengthAttribute)attributes[0];
                return stringlength;
            }
            return new StringLengthAttribute(200);
        }
        public RequiredAttribute GetRequired(PropertyInfo info)
        {
            object[] attributes = info.GetCustomAttributes(typeof(RequiredAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                var required = (RequiredAttribute)attributes[0];
                return required;
            }
            return new RequiredAttribute() { AllowEmptyStrings = true };
        }

        public EditableAttribute GetEditTable(PropertyInfo info)
        {
            object[] attributes = info.GetCustomAttributes(typeof(EditableAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                var editable = (EditableAttribute)attributes[0];
                return editable;
            }
            return new EditableAttribute(true);
        }

        public ReadOnlyAttribute GetReadOnly(PropertyInfo info)
        {
            object[] attributes = info.GetCustomAttributes(typeof(ReadOnlyAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                var readonlyattr = (ReadOnlyAttribute)attributes[0];
                return readonlyattr;
            }
            return new ReadOnlyAttribute(false);
        }

        public string GetDataType(PropertyInfo info)
        {
            object[] attributes = info.GetCustomAttributes(typeof(DataTypeAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                var datatype = (DataTypeAttribute)attributes[0];
                return datatype.DataType.ToString();
            }
            return "No GetDataType";
        }
        public string GetNotMapped(PropertyInfo info)
        {
            object[] attributes = info.GetCustomAttributes(typeof(NotMappedAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                var notmapped = (NotMappedAttribute)attributes[0];
                return "";
            }
            return "No NotMapped";
        }

        public bool GetHidden(PropertyInfo info)
        {
            object[] attributes = info.GetCustomAttributes(typeof(HiddenInJqgrid), false);
            if (attributes != null && attributes.Length > 0)
            {
                //var notmapped = (HiddenInputAttribute)attributes[0];
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="b">決定要不要抓虛欄位，預設false不抓</param>
        /// <returns></returns>
        public List<string> GetColNames<T>(T obj, bool b = false)
        {

            List<string> list = new List<string>();
            Type type = obj.GetType();
            var properties = type.GetProperties();
            foreach (var info in properties)
            {
                if (b)
                {
                    list.Add(info.Name);
                }
                else
                {
                    object[] attributes = info.GetCustomAttributes(typeof(NotMappedAttribute), false);
                    if (attributes == null || attributes.Length <= 0)
                    {
                        list.Add(info.Name);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 取得欄位資訊的清單
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<ColumnInfo> Get_ColumnInfoList<T>(T obj)
        {
            List<ColumnInfo> list = new List<ColumnInfo>();
            Type type = obj.GetType();
            var properties = type.GetProperties();
            foreach (var info in properties)
            {
                ColumnInfo data = new ColumnInfo();
                object[] NotMapped = info.GetCustomAttributes(typeof(NotMappedAttribute), false);
                if (NotMapped == null || NotMapped.Length <= 0)
                {
                    data.name = info.Name;
                    string temp = info.PropertyType.Name.ToLower();
                    if (temp.Contains("int"))
                    {
                        temp = "int";
                    }
                    data.propertyType = temp;
                    data.required = !GetRequired(info).AllowEmptyStrings;
                    data.maxlength = GetStringLength(info).MaximumLength;
                    data.requiredMsg = GetRequired(info).ErrorMessage;
                    data.maxlengthMsg = GetStringLength(info).ErrorMessage;
                    data.editable = GetEditTable(info).AllowEdit;
                    data.readonlyattr = GetReadOnly(info).IsReadOnly;
                    data.dataType = GetDataType(info);
                    data.displayName = GetDisplayName(info);

                    data.hidden = GetHidden(info);

                    list.Add(data);
                };
            }
            return list;
        }

        public List<ColumnInfo> Get_ColumnInfoList<T>()
        {
            List<ColumnInfo> list = new List<ColumnInfo>();
            Type type = typeof(T);
            var properties = type.GetProperties();
            foreach (var info in properties)
            {
                ColumnInfo data = new ColumnInfo();
                object[] NotMapped = info.GetCustomAttributes(typeof(NotMappedAttribute), false);
                if (NotMapped == null || NotMapped.Length <= 0)
                {
                    data.name = info.Name;
                    string temp = info.PropertyType.Name.ToLower();
                    if (temp.Contains("int"))
                    {
                        temp = "int";
                    }
                    data.propertyType = temp;
                    data.required = !GetRequired(info).AllowEmptyStrings;
                    data.maxlength = GetStringLength(info).MaximumLength;
                    data.requiredMsg = GetRequired(info).ErrorMessage;
                    data.maxlengthMsg = GetStringLength(info).ErrorMessage;
                    data.editable = GetEditTable(info).AllowEdit;
                    data.readonlyattr = GetReadOnly(info).IsReadOnly;
                    data.dataType = GetDataType(info);
                    data.displayName = GetDisplayName(info);
                    data.hidden = GetHidden(info);

                    list.Add(data);
                };
            }
            return list;
        }

        /// <summary>
        /// 取得model的資料清單
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<ColumnInfo> Get_ColumnInfoList(Type type)
        {
            List<ColumnInfo> list = new List<ColumnInfo>();
            var properties = type.GetProperties();
            foreach (var info in properties)
            {
                ColumnInfo data = new ColumnInfo();
                object[] NotMapped = info.GetCustomAttributes(typeof(NotMappedAttribute), false);
                if (NotMapped == null || NotMapped.Length <= 0)
                {
                    data.name = info.Name;
                    string temp = info.PropertyType.Name.ToLower();
                    if (temp.Contains("int"))
                    {
                        temp = "int";
                    }
                    data.propertyType = temp;
                    data.required = !GetRequired(info).AllowEmptyStrings;
                    data.maxlength = GetStringLength(info).MaximumLength;
                    data.requiredMsg = GetRequired(info).ErrorMessage;
                    data.maxlengthMsg = GetStringLength(info).ErrorMessage;
                    data.editable = GetEditTable(info).AllowEdit;
                    data.readonlyattr = GetReadOnly(info).IsReadOnly;
                    data.dataType = GetDataType(info);
                    data.displayName = GetDisplayName(info, type);
                    data.hidden = GetHidden(info);

                    list.Add(data);
                };
            }
            return list;
        }

        /// <summary>
        /// 預設是空字串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetKey<T>(T obj)
        {

            PropertyInfo[] props = obj.GetType().GetProperties();
            object[] attributes = null;
            foreach (PropertyInfo prop in props)
            {
                attributes = prop.GetCustomAttributes(typeof(KeyAttribute), false);
                if (attributes != null && attributes.Length > 0)
                {
                    return prop.Name;
                }
            }

            return "";

            // 舊版code，Model沒設KeyAttribute會出錯
            //var key = obj.GetType().GetProperties().FirstOrDefault(prop => prop.IsDefined(typeof(KeyAttribute), false));
            //return key.Name;
        }

        /// <summary>
        /// 預設是空字串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string GetKey<T>()
        {
            PropertyInfo[] props = typeof(T).GetProperties();
            object[] attributes = null;
            foreach (PropertyInfo prop in props)
            {
                attributes = prop.GetCustomAttributes(typeof(KeyAttribute), false);
                if (attributes != null && attributes.Length > 0)
                {
                    return prop.Name;
                }
            }

            return "";
        }

        /// <summary>
        /// 預設是空字串
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetKey(Type type)
        {
            PropertyInfo[] props = type.GetProperties();
            object[] attributes = null;
            foreach (PropertyInfo prop in props)
            {
                attributes = prop.GetCustomAttributes(typeof(KeyAttribute), false);
                if (attributes != null && attributes.Length > 0)
                {
                    return prop.Name;
                }
            }

            return "";
        }


        public string GetColNameToStr<T>(T obj, bool b = false)
        {
            string ColName = "";
            int Cnt = 0;
            Type type = obj.GetType();
            var properties = type.GetProperties();
            foreach (var info in properties)
            {
                if (b)
                {
                    if (Cnt > 0) { ColName += ","; }
                    ColName += info.Name;
                }
                else
                {
                    object[] attributes = info.GetCustomAttributes(typeof(NotMappedAttribute), false);
                    if (attributes == null || attributes.Length <= 0)
                    {
                        if (Cnt > 0) { ColName += ","; }
                        ColName += info.Name;
                    }
                }
                Cnt += 1;
            }
            return ColName;
        }

        /// <summary>
        /// 取得物件在Model有設Key屬性的欄位值，若沒設Key則回傳null
        /// 倚賴this.GetKey<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public object GetKeyValue<T>(object obj)
        {
            if (obj == null) return null;

            string key = this.GetKey<T>();

            if (string.IsNullOrEmpty(key)) return null;

            var prop = obj.GetType().GetProperty(key);
            if (prop == null) return null;

            var val = prop.GetValue(obj);

            return val;
        }

    }
}