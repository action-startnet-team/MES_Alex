using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MES_WATER.Models;
using System.Web.Mvc;
using System.Data;
using Newtonsoft.Json;
using System.Reflection;

namespace MES_WATER.Models
{
    public class GetData
    {
        Comm comm = new Comm();
        WebReference.WmsApi WA = new WebReference.WmsApi();

        public int Get_SplitLength(string pArray,char pSplit = ',') {
            int val = 0;
            if (!string.IsNullOrEmpty(pArray)) {
                val = pArray.Split(pSplit).Length;
            }
            return val;
        }

        public string Get_Tip(string pPrgCode,string pFieldCode) {
            string sReturn = "";
            string sSql = "";
            DataTable dtTmp = new DataTable();

            sSql = "select * from BDP34_0000" +
                   " where prg_code = @prg_code" +
                   "   and field_code = @field_code";
            dtTmp = comm.Get_DataTable(sSql, "prg_code,field_code", pPrgCode + "," + pFieldCode);
            if (dtTmp.Rows.Count > 0) {
                DataRow r = dtTmp.Rows[0];
                sReturn = r["cmemo"].ToString();
            }
            return sReturn;
        }





        /// <summary>
        /// 傳入欄位及欄位值轉成json格式
        /// </summary>
        /// <param name="sApiFieldArray"></param>
        /// <param name="sApiValueArray"></param>
        /// <returns></returns>
        #region
        public string DataToJson(string sApiFieldArray,string sApiValueArray) {
            DataTable dtApi = new DataTable();

            for (int i = 0; i < sApiFieldArray.Split(',').Length; i++)
            {
                string sApiField = sApiFieldArray.Split(',')[i];
                dtApi.Columns.Add(sApiField);
            }
            DataRow Row = dtApi.NewRow();
            for (int i = 0; i < sApiFieldArray.Split(',').Length; i++)
            {
                string sApiField = sApiFieldArray.Split(',')[i];
                string sApiValue = sApiValueArray.Split(',')[i];
                Row[sApiField] = sApiValue;
            }
            dtApi.Rows.Add(Row);
            string JsonApi = JsonConvert.SerializeObject(dtApi);
            JsonApi = JsonApi.Replace("[", "").Replace("]", "");
            return JsonApi;
        }

        public string DataToJson(string sApiFieldArray, FormCollection form)
        {
            DataTable dtApi = new DataTable();

            for (int i = 0; i < sApiFieldArray.Split(',').Length; i++)
            {
                string sApiField = sApiFieldArray.Split(',')[i];
                dtApi.Columns.Add(sApiField);
            }
            DataRow Row = dtApi.NewRow();
            for (int i = 0; i < sApiFieldArray.Split(',').Length; i++)
            {
                string sApiField = sApiFieldArray.Split(',')[i];
                if (!string.IsNullOrEmpty(form[sApiField]))
                {
                    Row[sApiField] = form[sApiField];
                }
                else
                {
                    Row[sApiField] = "";
                }                
            }
            dtApi.Rows.Add(Row);
            string JsonApi = JsonConvert.SerializeObject(dtApi);
            JsonApi = JsonApi.Replace("[", "").Replace("]", "");
            return JsonApi;
        }
        #endregion



        public string DateStrParse(string pDateStr) {
            string val = "";
            
            //代號轉中文
            switch (pDateStr) {
                case "year":
                    val = "年";
                    break;
                case "year_half":
                    val = "半年";
                    break;
                case "period":
                    val = "期";
                    break;
                case "season":
                    val = "季";                    
                    break;
                case "month":
                    val = "月";
                    break;
                case "month_half":
                    val = "半月";
                    break;
                case "week":
                    val = "週";
                    break;
                case "special":
                    val = "特定";
                    break;
            }
            //中文轉代號
            //switch (pDateStr)
            //{
            //}
            return val;
        }



        public int Get_MaxScrNo(string pTable,string pWhere, string pField) {
            string sSql = "";
            sSql = "select isnull(MAX(" + pField + "),0) as " + pField +
                   "  from " + pTable +
                   " " +pWhere;
            return comm.sGetInt32(DataFieldToStr(sSql,"scr_no"));
        }


        public string Get_ObjectValue(object pObject, string pField)
        {
            string val = "";
            PropertyInfo Info = pObject.GetType().GetProperty(pField);
            if (Info != null)
                if(Info.GetValue(pObject) != null)
                {
                    val = Info.GetValue(pObject).ToString();
                }
            return val;
        }



        /// <summary>
        /// 取得使用者可使用的電子表單
        /// </summary>
        /// <param name="pUsrCode"></param>
        /// <returns></returns>
        public string Get_UsrEpbArray(string pUsrCode) {
            //取得電子表單權限
            string sSql = "select * from BDP09_0200 " +
                          " where usr_code = '" + pUsrCode + "'" +
                          "   and is_use = 'Y'";
            string sEpbCodeArray = DataFieldToStr(sSql, "epb_code");
            return sEpbCodeArray;
        }


        /// <summary>
        /// 取得使用者可使用的電子表單的類別
        /// </summary>
        /// <param name="pUsrCode"></param>
        /// <returns></returns>
        public string Get_EpbCanUseType(string pUsrCode)
        {
            string sValue = "";
            string sSql = "select epb_type_code from EPB02_0000" +
                          " where epb_code in (" + StrArrayToSql(Get_UsrEpbArray(pUsrCode))  + ")" +
                          "  group by epb_type_code";
            sValue = DataFieldToStr(sSql, "epb_type_code");
            return sValue;
        }



        /// <summary>
        /// 檢查使用者在該表單裡面是否有指定權限代號
        /// </summary>
        /// <param name="pUsrCode">使用者</param>
        /// <param name="pEpbCode">表單代號</param>
        /// <param name="pLimitCode">權限代號</param>
        /// <returns></returns>
        public bool Chk_UsrEpbLimit(string pUsrCode, string pEpbCode, string pLimitCode)
        {
            bool sValue = false;
            string sSql = "select * from BDP09_0200 " +
                          " where usr_code = '" + pUsrCode + "'" +
                          "   and epb_code = '" + pEpbCode + "'";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                string sLimitStr = dtTmp.Rows[0]["limit_str"].ToString();
                if (sLimitStr.Contains(pLimitCode)) { sValue = true; }
            }
            return sValue;
        }





        /// <summary>
        /// 依照輸入型態，分類where語法
        /// </summary>
        /// <param name="form"></param>
        /// <param name="CtrType"></param>
        /// <param name="sField"></param>
        /// <returns></returns>
        public string Sort_WhereType(FormCollection form, string CtrType, string sField)
        {
            string sValue = "";
            string sWhereStr = "";
            if (!string.IsNullOrEmpty(form[sField]))
                switch (CtrType)
                {
                    case "A":
                        //區間
                        if (form[sField].Split(',')[0] != "" && form[sField].Split(',')[1] != "")
                        {
                            sValue += sWhereStr + " and " + sField + " between '" + form[sField].Split(',')[0] + "' and '" + form[sField].Split(',')[1] + "'";
                        }
                        break;
                    case "T":
                        //Textbox                            
                        sValue += sWhereStr + " and " + sField + " like '" + form[sField] + "'";
                        break;
                    case "S":
                        //下拉                           
                        sValue += sWhereStr + " and " + sField + " = '" + form[sField] + "'";
                        break;
                    case "D":
                        //日期                          
                        if (form[sField].Split(',')[0] != "" && form[sField].Split(',')[1] != "")
                        {
                            sValue += sWhereStr + " and " + sField + " between '" + form[sField].Split(',')[0] + "' and '" + form[sField].Split(',')[1] + "'";
                        }
                        break;
                    case "M":
                        //複選下拉                            
                        sValue += sWhereStr + " and " + sField + " in (" + StrArrayToSql(form[sField]) + ")";
                        break;
                }
            return sValue;
        }



        /// <summary>
        /// 取得下拉選單
        /// </summary>
        /// <param name="pSelectCode">下拉選單代號</param>
        /// <returns></returns>
        public string Get_DDLData(string pSelectCode,string pSplit = "-")
        {
            string sValue = "";
            string sSql_str = "";
            string sSql = "select * from BDP31_0000 " +
                          " where select_code = '"+ pSelectCode + "'";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0) {
                sSql_str = dtTmp.Rows[0]["tsql_select"].ToString() + " " + dtTmp.Rows[0]["tsql_where"].ToString() + " " + dtTmp.Rows[0]["tsql_order"].ToString();
                dtTmp = comm.Get_DataTable(sSql_str);
                for (int i = 0; i < dtTmp.Rows.Count;i++ ) {
                    if (i > 0) { sValue += ","; }

                    for (int u = 0; u < dtTmp.Columns.Count; u++)
                    {
                        if (u > 0) { sValue += pSplit; }
                        string ColName = dtTmp.Columns[u].ColumnName;
                        sValue += dtTmp.Rows[i][ColName].ToString();
                    }
                }
            }            
            return sValue;
        }

        /// <summary>
        /// 使","逗號分隔的字串轉換成Sql的單引號語法
        /// </summary>
        /// <param name="pString"></param>
        /// <returns></returns>
        public string StrArrayToSql(string pString) {
            string sValue = "";
            if (!string.IsNullOrEmpty(pString))
            {
                for (int i = 0; i < pString.Split(',').Length; i++)
                {
                    if (i > 0) { sValue += ","; }
                    sValue += "'" + pString.Split(',')[i] + "'";
                }
            }
            else
            {
                sValue = "''";
            }           
            return sValue;
        }


        /// <summary>
        /// 表單預設值
        /// </summary>
        /// <param name="pCtrType">欄位型態</param>
        /// <param name="pValue">預設值</param>
        /// <returns></returns>
        public string Default_Value(string pCtrType, string pValue)
        {
            string sValue = "";
            switch (pCtrType)
            {
                case "T":
                    switch (pValue)
                    {
                        case "TIMENOW":
                            sValue = DateTime.Now.ToString("HH:mm");
                            break;
                        default:
                            sValue = pValue;
                            break;
                    }
                    break;
                case "D":
                    switch (pValue)
                    {
                        case "NOW":
                            sValue = DateTime.Now.ToString("yyyy/MM/dd");
                            break;
                        default:
                            sValue = pValue;
                            break;
                    }
                    break;
            }
            return sValue;
        }


        /// <summary>
        /// 表單預設值
        /// </summary>
        /// <param name="pCtrType">欄位型態</param>
        /// <param name="pValue">預設值</param>
        /// <returns></returns>
        public string Default_Value(string pCtrType, string pValue, string pUsrCode)
        {
            string sValue = "";
            switch (pCtrType)
            {
                case "T":
                    switch (pValue)
                    {
                        case "TIMENOW":
                            sValue = DateTime.Now.ToString("HH:mm");
                            break;
                        case "USER":
                            sValue = pUsrCode;
                            break;
                        default:
                            sValue = pValue;
                            break;
                    }
                    break;
                case "D":
                    switch (pValue)
                    {
                        case "NOW":
                            sValue = DateTime.Now.ToString("yyyy/MM/dd");
                            break;
                        default:
                            sValue = pValue;
                            break;
                    }
                    break;
            }
            return sValue;
        }


        /// <summary>
        /// 利用SQL語法取得指定欄位(","逗號分隔)的字串形式
        /// </summary>
        /// <param name="pSqlStr">Sql語法</param>
        /// <param name="pFieldCode">欄位的","逗號分隔字串</param>
        /// <returns></returns>
        public string DataFieldToSTA(string pSqlStr, string pFieldCode,string Split = ",")
        {
            var dtTmp = comm.Get_DataTable(pSqlStr);
            string sValue = "";
            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                if (i > 0) { sValue += Split; };
                for (int u = 0; u < pFieldCode.Split(',').Length; u++)
                {
                    string sField = pFieldCode.Split(',')[u];
                    if (u > 0) { sValue += "|"; }
                    sValue += dtTmp.Rows[i][sField].ToString();
                }
            }
            return sValue;
        }

        public string DataFieldToSTA(DataTable dtTmp, string pFieldCode, string Split = ",")
        {
            string sValue = "";
            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                if (i > 0) { sValue += Split; };
                for (int u = 0; u < pFieldCode.Split(',').Length; u++)
                {
                    string sField = pFieldCode.Split(',')[u];
                    if (u > 0) { sValue += "|"; }
                    sValue += dtTmp.Rows[i][sField].ToString();
                }
            }
            return sValue;
        }



        /// <summary>
        /// 利用SQL語法取得指定欄位的字串形式
        /// </summary>
        /// <param name="pFieldCode">指定欄位</param>
        /// <returns></returns>
        public string DataFieldToStr(string pSqlStr, string pFieldCode)
        {
            var dtTmp = comm.Get_DataTable(pSqlStr);
            string sValue = "";
            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                if (i > 0) { sValue += ","; };
                sValue += dtTmp.Rows[i][pFieldCode].ToString();
            }
            return sValue;
        }

        public string DataFieldToStr(DataTable dtTmp, string pFieldCode)
        {
            string sValue = "";
            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                if (i > 0) { sValue += ","; };
                sValue += dtTmp.Rows[i][pFieldCode].ToString();
            }
            return sValue;
        }


        public string Get_DutName(string pUsrCode) {
            string sDutCode = comm.Get_QueryData("BDP08_0000", pUsrCode, "usr_code", "dut_code");
            return comm.Get_QueryData("BDP11_0000", sDutCode, "dut_code", "dut_name");
        }

        /// <summary>
        /// 取得資料
        /// </summary>
        /// <param name="pTableCode">資料表</param>
        /// <param name="pKeyValue">索引值</param>
        /// <param name="pKeyCode">索引鍵</param>
        /// <param name="pFieldValue">欄位資料</param>
        /// <returns></returns>
        public string Get_Data(string pTableCode, string pKeyValue, string pKeyCode, string pFieldValue)
        {
            //串SQL字串
            string sSql = "select " + pFieldValue + " from " + pTableCode + " where " + pKeyCode + " = @" + pKeyCode + "";
            DataTable dtTmp = comm.Get_DataTable(sSql, pKeyCode, pKeyValue);
            return DataFieldToStr(dtTmp, pFieldValue);
        }


        public string Get_DataByMultiField(string pTableCode, string pKeyValueArray, string pKeyCodeArray, string pFieldValue)
        {
            //串SQL字串
            string sSql = "select " + pFieldValue + " from " + pTableCode;
            string sSubWhere = " where ";
            for (int i = 0; i < pKeyCodeArray.Split(',').Length; i++)
            {
                string sKeyCode = pKeyCodeArray.Split(',')[i];
                if (i > 0) { sSubWhere += " and "; }
                sSubWhere += sKeyCode + "=@" + sKeyCode;
            }
            sSql += sSubWhere;
            DataTable dtTmp = comm.Get_DataTable(sSql, pKeyCodeArray, pKeyValueArray);
            return DataFieldToStr(dtTmp, pFieldValue);
        }



    }
}