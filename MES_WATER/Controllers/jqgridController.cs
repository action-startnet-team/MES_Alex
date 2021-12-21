using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;

using MES_WATER.Models;
using System.Web.WebPages;
using System.Data;

namespace MES_WATER.Controllers
{
    public class jqgridController : JsonNetController
    {
        // 共用函式庫
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 這個partial view 會在html的head和footer嵌入plugin和script
        /// 預設抓資料的Ajax url 為 {pPrgCode}/Get_GridDataByQuery
        /// 接收資料的欄位由 type決定，在其Model設定屬性 Key (鍵值) 和 HiddenInput (預設隱藏)
        /// </summary>
        /// <param name="pColumnModel">model</param>
        /// <param name="pPrgCode">功能代號</param>
        /// <returns></returns>
        public PartialViewResult pJqgrid_A(
            Type pModelType,
            string pPrgCode,
            List<ColumnInfo> pColumnInfoList = null,
            string pGetDataUrl = "",
            string pAddPostDataJs = "",
            string pPrimaryKey = "",
            List<JqgridButton> pAddBtnsInActionCol = null,
            string pEnableRowFilter = "",
            string pIsMultiselect = "",
            int pPageCount = 0,
            string pCustomHtml_header = "",
            string pCustomJs_bottom = "",
            string pCustomJs_loadComplete = "",
            string onEachRowJs = ""
            )
        {
            // 基本設定
            Type model_type = pModelType;  // 欄位對應的model的type
            string usr_code = User.Identity.Name;  //使用者
            string prg_code = pPrgCode;  // 程式代號
            // (預設)
            string limit_str = comm.Get_LimitByUsrCode(usr_code, prg_code);   // 權限字串
            string primary_key = string.IsNullOrEmpty(pPrimaryKey) ? gmv.GetKey(model_type) : pPrimaryKey;    // 主檔鍵值
            List<ColumnInfo> ColumnInfoList = pColumnInfoList == null ? gmv.Get_ColumnInfoList(model_type) : pColumnInfoList;  // 欄位設定

            string sAddPostDataJs = pAddPostDataJs;  // js語法, 直接加在pWhere後面

            // (預設) 資料處理 相關Url
            string sGetDataUrl = string.IsNullOrEmpty(pGetDataUrl) ? Url.Action("Get_GridDataByQuery", prg_code) : pGetDataUrl;  // 主檔資料來源的ajax url
            string sInsertUrl = Url.Action("Insert", prg_code);  // 新增按鈕的跳轉頁面 url
            string sUpdateUrl = Url.Action("Update", prg_code);  // 修改按鈕的跳轉頁面 url 
            string sDeleteUrl = Url.Action("Delete", prg_code);  // 刪除資料的ajax url
            string sChkDelUrl = Url.Action("Chk_Del_Main", prg_code);  // 刪除資料前檢查的ajax url

            // (預設) 查詢 modal form的選項
            List<DDLList> query_DDL = comm.Get_BDP30_0200(prg_code, "C");   // 查詢下拉選單
            List<DDLList> field_op_DDL = comm.Get_DDLOption("field_operator");  // 查詢的運算子的下拉選單

            // (預設) jqgrid 頁面部分選項控制
            string sEnableRowFilter = string.IsNullOrEmpty(pEnableRowFilter) ? comm.Get_QueryData("BDP00_0000", "jqgrid_rowfilter", "par_name", "par_value") : pEnableRowFilter;  // 是否有row filter
            string sIsMultiselect = pIsMultiselect;  // 是否多選
            int sPageCount = pPageCount <= 0 ? comm.sGetInt32(comm.Get_QueryData("BDP00_0000", "page_count", "par_name", "par_value")) : pPageCount;  // 每頁顯示筆數
            //
            sEnableRowFilter = string.IsNullOrEmpty(sEnableRowFilter) ? "Y" : sEnableRowFilter.ToUpper();  // 預設 Y
            sIsMultiselect = string.IsNullOrEmpty(sIsMultiselect) ? "N" : sIsMultiselect.ToUpper();  // 預設 N
            sPageCount = sPageCount <= 0 ? 30 : sPageCount;  // 預設 30

            // 額外增加按鈕 (在功能欄位)
            List<JqgridButton> sAddBtnsInActionCol = pAddBtnsInActionCol;
            if (sAddBtnsInActionCol == null)
            {
                sAddBtnsInActionCol = new List<JqgridButton>();
            }

            // 自訂義html或js
            string sCustomHtml_header = pCustomHtml_header;
            string sCustomJs_bottom = pCustomJs_bottom;
            string sCustomJs_loadComplete = pCustomJs_loadComplete;
            string sOnEachRowJs = onEachRowJs;

            // 欄位顯示設定 (BPD36_0000)
            string sSql = "Select * from BDP36_0000 Where model_name = @model_name";
            DataTable dt_hiddenSettings = comm.Get_DataTable(sSql, "model_name", model_type.Name);

            //
            ViewBag.dt_hiddenSettings = dt_hiddenSettings;

            //
            ViewBag.prg_code = prg_code;
            ViewBag.limit_str = limit_str;
            ViewBag.model_type = model_type;


            ViewBag.query_DDL = query_DDL;
            ViewBag.field_op_DDL = field_op_DDL;

            ViewBag.ColumnInfoList = ColumnInfoList;
            ViewBag.primary_key = primary_key;

            ViewBag.sPostDataJs = sAddPostDataJs;

            //
            ViewBag.sGetDataUrl = sGetDataUrl;
            ViewBag.sInsertUrl = sInsertUrl;
            ViewBag.sUpdateUrl = sUpdateUrl;
            ViewBag.sDeleteUrl = sDeleteUrl;
            ViewBag.sChkDelUrl = sChkDelUrl;

            // 
            ViewBag.enableRowFilter = sEnableRowFilter;
            ViewBag.isMultiSelect = sIsMultiselect;
            ViewBag.page_count = sPageCount;

            // 功能欄位按鈕
            ViewBag.addBtnsInActionCol = sAddBtnsInActionCol;

            // 自訂義html, js
            ViewBag.customHtml_header = sCustomHtml_header;
            ViewBag.customJs_bottom = sCustomJs_bottom;
            ViewBag.customJs_loadComplete = sCustomJs_loadComplete;
            ViewBag.onEachRowJs = sOnEachRowJs;

            return PartialView();
        }

        public PartialViewResult pJqgrid_C(
            Type pMasterModelType,
            Type pDetailModelType,
            string pPrgCode,
            List<ColumnInfo> pColumnInfoList = null,
            List<ColumnInfo> pColumnInfoList_D1 = null,
            string pPrimaryKey = "",
            string pForeignKey = "",
            string pGetDataUrl = "",
            string pGetDataUrl_D1 = "",
            string pPostDataJs = "",
            string pLimitStr = "",
            List<JqgridButton> pAddBtnsInActionCol = null,
            string pEnableSubgrid = "",
            string pEnableRowFilter = "",
            string pIsMultiselect = "",
            int pPageCount = 0,
            string pCustomHtml_header = "",
            string pCustomJs_bottom = "",
            string pCustomJs_loadComplete = ""
            )
        {

            // 基本設定
            Type master_type = pMasterModelType;  // 主檔欄位對應的model的type
            Type detail_type = pDetailModelType;  // 明細欄位對應的model的type

            string usr_code = User.Identity.Name;  //使用者
            string prg_code = pPrgCode;  // 程式代號
            // (預設)
            string limit_str = string.IsNullOrEmpty(pLimitStr) ? comm.Get_LimitByUsrCode(usr_code, prg_code) : pLimitStr;   // 權限字串
            string primary_key = string.IsNullOrEmpty(pPrimaryKey) ? gmv.GetKey(master_type) : pPrimaryKey;    // 主檔鍵值
            string foreign_key = string.IsNullOrEmpty(pForeignKey) ? primary_key : pForeignKey;    // 關聯鍵，預設等同主檔鍵值

            List<ColumnInfo> ColumnInfoList = pColumnInfoList == null ? gmv.Get_ColumnInfoList(master_type) : pColumnInfoList;  // 主檔欄位設定
            List<ColumnInfo> ColumnInfoList_D1 = pColumnInfoList_D1 == null ? gmv.Get_ColumnInfoList(detail_type) : pColumnInfoList_D1;  // 主檔欄位設定
            string sPostDataJs = pPostDataJs;  // js語法, 直接加在pWhere後面

            // (預設) 資料處理 相關Url
            string sGetDataUrl = string.IsNullOrEmpty(pGetDataUrl) ? Url.Action("Get_GridDataByQuery", prg_code) : pGetDataUrl;  // 主檔資料來源的ajax url
            string sInsertUrl = Url.Action("Insert", prg_code);  // 新增按鈕的跳轉頁面 url
            string sUpdateUrl = Url.Action("Update", prg_code);  // 修改按鈕的跳轉頁面 url 
            string sDeleteUrl = Url.Action("Delete", prg_code);  // 刪除資料的ajax url
            string sChkDelUrl = Url.Action("Chk_Del_Main", prg_code);  // 刪除資料前檢查的ajax url
            string sGetDataUrl_D1 = string.IsNullOrEmpty(pGetDataUrl_D1) ? Url.Action("Get_GridData_D1", prg_code) : pGetDataUrl_D1;  // 主檔資料來源的ajax url

            // (預設) 查詢 modal form的選項
            List<DDLList> query_DDL = comm.Get_BDP30_0200(prg_code, "C");   // 查詢下拉選單
            List<DDLList> field_op_DDL = comm.Get_DDLOption("field_operator");  // 查詢的運算子的下拉選單

            // (預設) jqgrid 頁面部分選項控制
            string sEnableSubgrid = pEnableSubgrid;  // 是否開啟subgrid
            string sEnableRowFilter = string.IsNullOrEmpty(pEnableRowFilter) ? comm.Get_QueryData("BDP00_0000", "jqgrid_rowfilter", "par_name", "par_value") : pEnableRowFilter;  // 是否有row filter
            string sIsMultiselect = pIsMultiselect;  // 是否多選
            int sPageCount = pPageCount <= 0 ? comm.sGetInt32(comm.Get_QueryData("BDP00_0000", "page_count", "par_name", "par_value")) : pPageCount;  // 每頁顯示筆數
            // 預設值
            sEnableSubgrid = string.IsNullOrEmpty(sEnableSubgrid) ? "Y" : sEnableSubgrid.ToUpper();  // 預設 Y
            sEnableRowFilter = string.IsNullOrEmpty(sEnableRowFilter) ? "Y" : sEnableRowFilter.ToUpper();  // 預設 Y
            sIsMultiselect = string.IsNullOrEmpty(sIsMultiselect) ? "N" : sIsMultiselect.ToUpper();  // 預設 N
            sPageCount = sPageCount <= 0 ? 30 : sPageCount;  // 預設 30

            // 額外增加按鈕 (在功能欄位)
            List<JqgridButton> sAddBtnsInActionCol = pAddBtnsInActionCol;
            if (sAddBtnsInActionCol == null)
            {
                sAddBtnsInActionCol = new List<JqgridButton>();
            }


            // 自訂義html或js
            string sCustomHtml_header = pCustomHtml_header;
            string sCustomJs_bottom = pCustomJs_bottom;
            string sCustomJs_loadComplete = pCustomJs_loadComplete;

            // 欄位顯示設定 (BPD36_0000)
            string sSql = "Select * from BDP36_0000 Where model_name = @model_name";
            DataTable dt_hiddenSettings = comm.Get_DataTable(sSql, "model_name", master_type.Name);
            DataTable dt_hiddenSettings_details = comm.Get_DataTable(sSql, "model_name", detail_type.Name);

            //
            ViewBag.dt_hiddenSettings = dt_hiddenSettings;
            ViewBag.dt_hiddenSettings_details = dt_hiddenSettings_details;

            //
            ViewBag.prg_code = prg_code;
            ViewBag.limit_str = limit_str;

            //
            ViewBag.model_type = master_type;
            ViewBag.detail_type = detail_type;

            //
            ViewBag.primary_key = primary_key;
            ViewBag.ColumnInfoList = ColumnInfoList;
            ViewBag.ColumnInfoList_D1 = ColumnInfoList_D1;

            //
            ViewBag.query_DDL = query_DDL;
            ViewBag.field_op_DDL = field_op_DDL;

            //
            ViewBag.sPostDataJs = sPostDataJs;

            //
            ViewBag.sGetDataUrl = sGetDataUrl;
            ViewBag.sInsertUrl = sInsertUrl;
            ViewBag.sUpdateUrl = sUpdateUrl;
            ViewBag.sDeleteUrl = sDeleteUrl;
            ViewBag.sChkDelUrl = sChkDelUrl;
            ViewBag.sGetDataUrl_D1 = sGetDataUrl_D1;

            // 
            ViewBag.sEnableSubgrid = sEnableSubgrid;
            ViewBag.enableRowFilter = sEnableRowFilter;
            ViewBag.isMultiSelect = sIsMultiselect;
            ViewBag.page_count = sPageCount;

            // 功能欄位按鈕
            ViewBag.addBtnsInActionCol = sAddBtnsInActionCol;

            // 自訂義html, js
            ViewBag.customHtml_header = sCustomHtml_header;
            ViewBag.customJs_bottom = sCustomJs_bottom;
            ViewBag.customJs_loadComplete = sCustomJs_loadComplete;


            return PartialView();
        }

        public PartialViewResult pJqgrid_A_UpdateAll(
            Type pModelType,
            string pPrgCode,
            List<ColumnInfo> pColumnInfoList = null,
            string pGetDataUrl = "",
            string pPostDataJs = "",
            string pPrimaryKey = "",
            string pLimitStr = "",
            List<JqgridButton> pAddBtnsInActionCol = null,
            string pEnableRowFilter = "",
            string pIsMultiselect = "",
            int pPageCount = 0,
            string pCustomHtml_header = "",
            string pCustomJs_bottom = "",
            string pCustomJs_loadComplete = "",
            List<string> pEditableCols = null
            )
        {
            // 基本設定
            Type model_type = pModelType;  // 欄位對應的model的type
            string usr_code = User.Identity.Name;  //使用者
            string prg_code = pPrgCode;  // 程式代號
            // (預設)
            string limit_str = string.IsNullOrEmpty(pLimitStr) ? comm.Get_LimitByUsrCode(usr_code, prg_code) : pLimitStr;   // 權限字串
            string primary_key = string.IsNullOrEmpty(pPrimaryKey) ? gmv.GetKey(model_type) : pPrimaryKey;    // 主檔鍵值
            List<ColumnInfo> ColumnInfoList = pColumnInfoList == null ? gmv.Get_ColumnInfoList(model_type) : pColumnInfoList;  // 欄位設定

            string sPostDataJs = pPostDataJs;  // js語法, 直接加在pWhere後面

            // (預設) 資料處理 相關Url
            string sGetDataUrl = string.IsNullOrEmpty(pGetDataUrl) ? Url.Action("Get_GridDataByQuery", prg_code) : pGetDataUrl;  // 主檔資料來源的ajax url
            string sInsertUrl = Url.Action("Insert", prg_code);  // 新增按鈕的跳轉頁面 url
            string sUpdateUrl = Url.Action("Update", prg_code);  // 修改按鈕的跳轉頁面 url 
            string sDeleteUrl = Url.Action("Delete", prg_code);  // 刪除資料的ajax url
            string sChkDelUrl = Url.Action("Chk_Del_Main", prg_code);  // 刪除資料前檢查的ajax url

            string sUpdateAllUrl = Url.Action("UpdateAll", prg_code);  // 修改全部 for ajax

            List<string> EditableCols = pEditableCols == null ? new List<string>() : pEditableCols;

            // (預設) 查詢 modal form的選項
            List<DDLList> query_DDL = comm.Get_BDP30_0200(prg_code, "C");   // 查詢下拉選單
            List<DDLList> field_op_DDL = comm.Get_DDLOption("field_operator");  // 查詢的運算子的下拉選單

            // (預設) jqgrid 頁面部分選項控制
            string sEnableRowFilter = string.IsNullOrEmpty(pEnableRowFilter) ? comm.Get_QueryData("BDP00_0000", "jqgrid_rowfilter", "par_name", "par_value") : pEnableRowFilter;  // 是否有row filter
            string sIsMultiselect = pIsMultiselect;  // 是否多選
            int sPageCount = pPageCount <= 0 ? comm.sGetInt32(comm.Get_QueryData("BDP00_0000", "page_count", "par_name", "par_value")) : pPageCount;  // 每頁顯示筆數
            //
            sEnableRowFilter = string.IsNullOrEmpty(sEnableRowFilter) ? "Y" : sEnableRowFilter.ToUpper();  // 預設 Y
            sIsMultiselect = string.IsNullOrEmpty(sIsMultiselect) ? "N" : sIsMultiselect.ToUpper();  // 預設 N
            sPageCount = sPageCount <= 0 ? 30 : sPageCount;  // 預設 30

            // 額外增加按鈕 (在功能欄位)
            List<JqgridButton> sAddBtnsInActionCol = pAddBtnsInActionCol;
            if (sAddBtnsInActionCol == null)
            {
                sAddBtnsInActionCol = new List<JqgridButton>();
            }

            // 自訂義html或js
            string sCustomHtml_header = pCustomHtml_header;
            string sCustomJs_bottom = pCustomJs_bottom;
            string sCustomJs_loadComplete = pCustomJs_loadComplete;

            //
            ViewBag.prg_code = prg_code;
            ViewBag.limit_str = limit_str;

            ViewBag.query_DDL = query_DDL;
            ViewBag.field_op_DDL = field_op_DDL;

            ViewBag.ColumnInfoList = ColumnInfoList;
            ViewBag.primary_key = primary_key;

            ViewBag.sPostDataJs = sPostDataJs;

            //
            ViewBag.sGetDataUrl = sGetDataUrl;
            ViewBag.sInsertUrl = sInsertUrl;
            ViewBag.sUpdateUrl = sUpdateUrl;
            ViewBag.sDeleteUrl = sDeleteUrl;
            ViewBag.sChkDelUrl = sChkDelUrl;

            ViewBag.sUpdateAllUrl = sUpdateAllUrl;
            ViewBag.EditableCols = EditableCols;



            // 
            ViewBag.enableRowFilter = sEnableRowFilter;
            ViewBag.isMultiSelect = sIsMultiselect;
            ViewBag.page_count = sPageCount;

            // 功能欄位按鈕
            ViewBag.addBtnsInActionCol = sAddBtnsInActionCol;

            // 自訂義html, js
            ViewBag.customHtml_header = sCustomHtml_header;
            ViewBag.customJs_bottom = sCustomJs_bottom;
            ViewBag.customJs_loadComplete = sCustomJs_loadComplete;

            return PartialView();
        }


        public PartialViewResult Update(
            Type pModelType,
            string pPrgCode,
            string pForeignKey,
            string pTkCode,
            string pDetailNumber = "1",
            Dictionary<string, List<DDLList>> pDropDowns = null,
            List<ColumnInfo> pColumnInfoList = null,
            string pPostDataJs = "",
            string pPrimaryKey = "",
            string pLimitStr = "",
            List<JqgridButton> pAddBtnsInActionCol = null,
            bool pDisableEditButton = false,
            bool pDisableDelButton = false,
            string pEnableRowFilter = "N",
            string pIsMultiselect = "",
            int pPageCount = 0,
            string pCustomHtml_header = "",
            string pCustomJs_bottom = "",
            string pCustomJs_loadComplete = "",
            bool isGroup = false,
            string sGroupField = "",
            string sGroupFieldName = "",
            string sGroupFieldRelTextCode = "",
            string grid_id = "grid-table",
            string grid_pager = "grid-pager"
            )
        {

            // 基本設定
            Type model_type = pModelType;  // 欄位對應的model的type
            string usr_code = User.Identity.Name;  //使用者
            string prg_code = pPrgCode;  // 程式代號
            // (預設)
            string limit_str = string.IsNullOrEmpty(pLimitStr) ? comm.Get_LimitByUsrCode(usr_code, prg_code) : pLimitStr;   // 權限字串
            string primary_key = string.IsNullOrEmpty(pPrimaryKey) ? gmv.GetKey(model_type) : pPrimaryKey;    // 主檔鍵值
            List<ColumnInfo> ColumnInfoList = pColumnInfoList == null ? gmv.Get_ColumnInfoList(model_type) : pColumnInfoList;  // 欄位設定

            string sPostDataJs = pPostDataJs;  // js語法, 直接加在pWhere後面

            // (預設) 資料處理 相關Url
            string sGetDataUrl = Url.Action("Get_GridData_D" + pDetailNumber, prg_code);  // 主檔資料來源的ajax url
            string sInsertUrl = Url.Action("Insert_D" + pDetailNumber, prg_code);  // 新增按鈕的跳轉頁面 url
            string sUpdateUrl = Url.Action("Update_D" + pDetailNumber, prg_code);  // 修改按鈕的跳轉頁面 url 
            string sDeleteUrl = Url.Action("Delete_D" + pDetailNumber, prg_code);  // 刪除資料的ajax url

            string sChkInsUrl = Url.Action("Chk_Ins_D" + pDetailNumber, prg_code);  // 刪除資料前檢查的ajax url
            string sChkDelUrl = Url.Action("Chk_Del_D" + pDetailNumber, prg_code);  // 刪除資料前檢查的ajax url
            string sChkUpdUrl = Url.Action("Chk_Upd_D" + pDetailNumber, prg_code);  // 刪除資料前檢查的ajax url


            // (預設) 查詢 modal form的選項
            List<DDLList> query_DDL = comm.Get_BDP30_0200(prg_code, "C");   // 查詢下拉選單
            List<DDLList> field_op_DDL = comm.Get_DDLOption("field_operator");  // 查詢的運算子的下拉選單

            // (預設) jqgrid 頁面部分選項控制
            string sEnableRowFilter = string.IsNullOrEmpty(pEnableRowFilter) ? comm.Get_QueryData("BDP00_0000", "jqgrid_rowfilter", "par_name", "par_value") : pEnableRowFilter;  // 是否有row filter
            string sIsMultiselect = pIsMultiselect;  // 是否多選
            int sPageCount = pPageCount <= 0 ? comm.sGetInt32(comm.Get_QueryData("BDP00_0000", "page_count", "par_name", "par_value")) : pPageCount;  // 每頁顯示筆數
            //
            sEnableRowFilter = string.IsNullOrEmpty(sEnableRowFilter) ? "Y" : sEnableRowFilter.ToUpper();  // 預設 Y
            sIsMultiselect = string.IsNullOrEmpty(sIsMultiselect) ? "N" : sIsMultiselect.ToUpper();  // 預設 N
            sPageCount = sPageCount <= 0 ? 30 : sPageCount;  // 預設 30

            // 額外增加按鈕 (在功能欄位)
            List<JqgridButton> sAddBtnsInActionCol = pAddBtnsInActionCol;
            if (sAddBtnsInActionCol == null)
            {
                sAddBtnsInActionCol = new List<JqgridButton>();
            }

            // 自訂義html或js
            string sCustomHtml_header = pCustomHtml_header;
            string sCustomJs_bottom = pCustomJs_bottom;
            string sCustomJs_loadComplete = pCustomJs_loadComplete;

            // 欄位顯示設定 (BPD36_0000)
            string sSql = "Select * from BDP36_0000 Where model_name = @model_name";
            DataTable dt_hiddenSettings = comm.Get_DataTable(sSql, "model_name", model_type.Name);

            //
            ViewBag.dt_hiddenSettings = dt_hiddenSettings;
            ViewBag.pDetailNumber = pDetailNumber;

            //
            ViewBag.prg_code = prg_code;
            ViewBag.limit_str = limit_str;

            ViewBag.query_DDL = query_DDL;
            ViewBag.field_op_DDL = field_op_DDL;

            ViewBag.ColumnInfoList = ColumnInfoList;
            ViewBag.primary_key = primary_key;

            ViewBag.sPostDataJs = sPostDataJs;

            ViewBag.pForeignKey = pForeignKey;
            ViewBag.pTkCode = pTkCode;

            //
            ViewBag.sGetDataUrl = sGetDataUrl;
            ViewBag.sInsertUrl = sInsertUrl;
            ViewBag.sUpdateUrl = sUpdateUrl;
            ViewBag.sDeleteUrl = sDeleteUrl;

            ViewBag.sChkInsUrl = sChkInsUrl;
            ViewBag.sChkUpdUrl = sChkUpdUrl;
            ViewBag.sChkDelUrl = sChkDelUrl;


            // 
            ViewBag.enableRowFilter = sEnableRowFilter;
            ViewBag.isMultiSelect = sIsMultiselect;
            ViewBag.page_count = sPageCount;
            ViewBag.addBtnsInActionCol = sAddBtnsInActionCol;
            ViewBag.pDisableEditButton = pDisableEditButton;
            ViewBag.pDisableDelButton = pDisableDelButton;

            // 自訂義html, js
            ViewBag.customHtml_header = sCustomHtml_header;
            ViewBag.customJs_bottom = sCustomJs_bottom;
            ViewBag.customJs_loadComplete = sCustomJs_loadComplete;

            //下拉選項
            // 如果沒傳入該參數時，會預設為從partial view傳來的參數
            ViewBag.dropdowns = pDropDowns;

            //jqgrid的group功能
            ViewBag.isGroup = isGroup;
            ViewBag.sGroupField = sGroupField;
            ViewBag.sGroupFieldName = string.IsNullOrEmpty(sGroupFieldName) ? gmv.GetDisplayName(model_type.GetProperty(sGroupField), model_type) : sGroupFieldName;
            ViewBag.sGroupFieldRelTextCode = sGroupFieldRelTextCode;

            // grid_id
            ViewBag.grid_id = grid_id;
            ViewBag.grid_pager = grid_pager;

            return PartialView();
        }

        [HttpPost]
        public void SaveState(string pUrlPath, string pGridId, string pJsonState, string pIsSubgrid = "")
        {
            string pUsrCode = User.Identity.Name;

            if (string.IsNullOrEmpty(pUsrCode) || string.IsNullOrEmpty(pUrlPath) || string.IsNullOrEmpty(pGridId))
            {
                return;
            }

            DynamicParameters sqlparams = new DynamicParameters();
            sqlparams.Add("@usr_code", pUsrCode);
            sqlparams.Add("@url_path", pUrlPath);
            sqlparams.Add("@grid_id", pGridId);
            sqlparams.Add("@is_subgrid", pIsSubgrid);
            sqlparams.Add("@json_state", pJsonState);

            string sSql = "";

            string sChkWhere = "";
            if (pIsSubgrid == "Y")
            {
                sChkWhere = "Where usr_code = @usr_code and url_path = @url_path and grid_id = @grid_id and is_subgrid = @is_subgrid";
            }
            else
            {
                sChkWhere = "Where usr_code = @usr_code and url_path = @url_path and grid_id = @grid_id";
            }

            bool notExist = true;
            if (pIsSubgrid == "Y")
            {
                notExist = comm.Chk_RelData("BDP33_0000", sChkWhere, "usr_code,url_path,grid_id,is_subgrid", pUsrCode + "," + pUrlPath + "," + pGridId + "," + pIsSubgrid);
            }
            else
            {
                notExist = comm.Chk_RelData("BDP33_0000", sChkWhere, "usr_code,url_path,grid_id", pUsrCode + "," + pUrlPath + "," + pGridId);
            }


            if (notExist)
            {
                using (SqlConnection con_db = comm.Set_DBConnection())
                {
                    sSql = "Insert into BDP33_0000 (usr_code, url_path, grid_id, is_subgrid, json_state) values (@usr_code, @url_path, @grid_id, @is_subgrid, @json_state)";
                    con_db.Execute(sSql, sqlparams);
                }
            }
            else
            {
                using (SqlConnection con_db = comm.Set_DBConnection())
                {
                    if (pIsSubgrid == "Y")
                    {
                        sSql = "Update BDP33_0000 Set json_state = @json_state where usr_code = @usr_code and url_path = @url_path and grid_id = @grid_id and is_subgrid = @is_subgrid";
                    }
                    else
                    {
                        sSql = "Update BDP33_0000 Set json_state = @json_state where usr_code = @usr_code and url_path = @url_path and grid_id = @grid_id";
                    }
                    con_db.Execute(sSql, sqlparams);
                }
            }

        }

        public JsonResult GetState(string pUrlPath, string pGridId, string pIsSubgrid = "")
        {
            string pUsrCode = User.Identity.Name;

            string result = "";

            DynamicParameters sqlparams = new DynamicParameters();
            sqlparams.Add("@usr_code", pUsrCode);
            sqlparams.Add("@url_path", pUrlPath);
            sqlparams.Add("@grid_id", pGridId);
            sqlparams.Add("@is_subgrid", pIsSubgrid);

            string sSql = "";
            if (pIsSubgrid == "Y")
            {
                sSql = " Select top 1 json_state from BDP33_0000 where usr_code=@usr_code and url_path=@url_path and grid_id=@grid_id and is_subgrid = 'Y' ";
            }
            else
            {
                sSql = " Select top 1 json_state from BDP33_0000 where usr_code=@usr_code and url_path=@url_path and grid_id=@grid_id and is_subgrid <> 'Y' ";
            }
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                if (!string.IsNullOrEmpty(sSql)) { result = con_db.QueryFirstOrDefault<string>(sSql, sqlparams); }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void DeleteState(string pUrlPath, string pGridId)
        {
            string pUsrCode = User.Identity.Name;

            DynamicParameters sqlparams = new DynamicParameters();
            sqlparams.Add("@usr_code", pUsrCode);
            sqlparams.Add("@url_path", pUrlPath);
            sqlparams.Add("@grid_id", pGridId);

            string sSql = "Delete from BDP33_0000 where usr_code=@usr_code and url_path=@url_path and grid_id=@grid_id ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, sqlparams);
            }

        }






        //    List<JqgridButton> addBtnsInActionCol = new List<JqgridButton>();  // 放在功能欄位裡 (其實也是在loadComplete裡面)


        //    // 功能欄位的按鈕
        //    var btn1 = new JqgridButton()
        //    {
        //        type = "jumpTo",
        //        icon_class = "ui-icon ui-icon-pencil red",
        //        title = "測試1",
        //        url = "/BDP070A"
        //    };

        //    var btn2 = new JqgridButton()
        //    {
        //        type = "jumpTo",
        //        icon_class = "ui-icon ui-icon-pencil orange",
        //        title = "測試2",
        //        url = "/BDP230B"
        //    };

        //    var btn3 = new JqgridButton()
        //    {
        //        type = "jumpTo",
        //        icon_class = "ui-icon ui-icon-pencil green",
        //        title = "測試3",
        //        url = ""
        //    };

        //    addBtnsInActionCol.Add(btn1);
        //addBtnsInActionCol.Add(btn2);
        //addBtnsInActionCol.Add(btn3);


        ///* 自訂義html或js */

        //// 放在上面按鈕那列
        //    @*Func<object, HelperResult> customBtnTemplate =
        //            @<text>
        //            <div class="pull-left" style="margin-left: 10px">
        //                <a href = "/WMB010B/Upload" class="btn btn-white btn-warning btn-bold" role="button">
        //                    <span>匯入</span>
        //                </a>
        //            </div>
        //            </text>;
        //        string customHtml_header = customBtnTemplate.Invoke(null).ToString();

        //        // 放在在整個js最下面
        //        Func<object, HelperResult> customJsTemplate =
        //            @<text>
        //                console.log('test insert js from partial view. ')
        //            </text>;
        //         string customJs_bottom = customJsTemplate.Invoke(null).ToString();

        //        // 放在loadComplete裡面 (jqgrid event)
        //        Func<object, HelperResult> loadCompleteJsTemplate =
        //            @<text>
        //                console.log('test loadComplete js from partial view. ')
        //            </text>;
        //         string customJs_loadComplete = loadCompleteJsTemplate.Invoke(null).ToString();*@
        //}


        // end of jqgridController
    }
}