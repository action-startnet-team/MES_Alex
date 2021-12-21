using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MES_WATER.Models;
using MES_WATER.Repository;
using System.Data;
using System.Linq.Dynamic;
using System.Web.Security;
using System.Reflection;
using Newtonsoft.Json;

namespace MES_WATER.Controllers
{
    [Authorize] //登入驗證
    [HandleError(View = "Error")]  //錯誤導向

    public class MET050AController : JsonNetController
    {
        //程式代號
        string sPrgCode = "MET050A";
        //需要用到的Repo
        MET04_0000Repository repoMET01_0000 = new MET04_0000Repository();
        MET04_0600Repository repoMET04_0600 = new MET04_0600Repository();
        //共用函式庫
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /* 資料處理 向下 */
        /// <summary>
        /// (固定區) 主檔 首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //要結合權限控制
            //ViewBag.limit_str = comm.Get_LimitByUsrCode(User.Identity.Name, sPrgCode);
            ViewBag.prg_code = sPrgCode;

            // 使用者, controllerName, actionName
            string usr_code = User.Identity.Name;
            string prg_code = sPrgCode;
            string view_code = ControllerContext.RouteData.Values["action"].ToString();

            //取得欄位寬度
            List<BDP30_0000> colWidth_list = comm.Get_BDP30_0000(usr_code, prg_code, view_code);
            List<BDP30_0000> colWidth_list_D1 = comm.Get_BDP30_0000(usr_code, prg_code, view_code + "_D1");
            ViewBag.colWidth_list = colWidth_list;
            ViewBag.colWidth_list_D1 = colWidth_list_D1;

            //取得欄位顯示
            List<BDP30_0100> is_show_list = comm.Get_BDP30_0100(usr_code, prg_code, view_code);
            List<BDP30_0100> is_show_D1_list = comm.Get_BDP30_0100(usr_code, prg_code, view_code + "_D1");
            ViewBag.is_show_list = is_show_list;
            ViewBag.is_show_D1_list = is_show_D1_list;

            return View();
        }

        public ActionResult Class(string pTkCode)
        {
            //取得製程列表
            ViewBag.mo_code = pTkCode;
            ViewBag.work_data = Get_WorkData(pTkCode);

            //建立異常列表
            DataTable dtNgList = new DataTable();
            dtNgList.Columns.Add("ureport_code");
            dtNgList.Columns.Add("mo_code");
            dtNgList.Columns.Add("work_code");
            dtNgList.Columns.Add("ng_code");
            dtNgList.Columns.Add("ng_name");
            dtNgList.Columns.Add("ng_qty");
            //取得MED03資料
            DataTable MED03 = Get_MED03(pTkCode);
            foreach (DataRow row in MED03.Rows)
            {
                DataRow drow = dtNgList.NewRow();
                drow["ureport_code"] = "";
                drow["mo_code"] = pTkCode;
                drow["work_code"] = row["work_code"].ToString();
                drow["ng_code"] = row["ng_code"].ToString();
                drow["ng_name"] = row["ng_name"].ToString();
                drow["ng_qty"] = row["ng_qty"].ToString();

                dtNgList.Rows.Add(drow);
            }
            //取得MET04_0600資料
            List<MET04_0600> MET04List = repoMET04_0600.Get_DataList(pTkCode);
            foreach (MET04_0600 MET04_0600 in MET04List)
            {
                DataRow drow = dtNgList.NewRow();
                drow["ureport_code"] = MET04_0600.ureport_code;
                drow["mo_code"] = pTkCode;
                drow["work_code"] = MET04_0600.work_code;
                drow["ng_code"] = MET04_0600.ng_code;
                drow["ng_name"] = MET04_0600.ng_name;
                drow["ng_qty"] = MET04_0600.ng_qty;

                dtNgList.Rows.Add(drow);
            }

            ViewBag.ng_data = dtNgList;

            return View();
        }

        
        [HttpPost]
        public ActionResult Class(FormCollection form)
        {
            string pTkCode = form["mo_code"];
            //取得製程列表
            ViewBag.mo_code = pTkCode;
            DataTable dtWork = Get_WorkData(pTkCode);

            //建立異常列表
            DataTable dtNgList = new DataTable();
            dtNgList.Columns.Add("ureport_code");
            dtNgList.Columns.Add("mo_code");
            dtNgList.Columns.Add("work_code");
            dtNgList.Columns.Add("ng_code");
            dtNgList.Columns.Add("ng_name");
            dtNgList.Columns.Add("ng_qty");
            //取得MED03資料
            DataTable MED03 = Get_MED03(pTkCode);
            foreach (DataRow row in MED03.Rows)
            {
                DataRow drow = dtNgList.NewRow();
                drow["ureport_code"] = "";
                drow["mo_code"] = pTkCode;
                drow["work_code"] = row["work_code"].ToString();
                drow["ng_code"] = row["ng_code"].ToString();
                drow["ng_name"] = row["ng_name"].ToString();
                drow["ng_qty"] = row["ng_qty"].ToString();

                dtNgList.Rows.Add(drow);
            }

            foreach (DataRow row in dtWork.Rows)
            {
                string work_code = row["work_code"].ToString();
                string ng_code = form["ng_code_" + work_code];
                string ng_qty = form["qty_" + work_code];
                double qty = 0;

                if (ng_code != "" && double.TryParse(ng_qty,out qty))
                {
                    MET04_0600 MET04_0600 = new MET04_0600();
                    MET04_0600.ureport_code = comm.Get_TkCode("MET050A");
                    MET04_0600.ureport_date = DateTime.Now.ToString("yyyy/MM/dd");
                    MET04_0600.mo_code = pTkCode;
                    MET04_0600.work_code = work_code;
                    MET04_0600.ng_code = ng_code;
                    MET04_0600.ng_qty = qty;
                    repoMET04_0600.InsertData(MET04_0600);
                }
            }

            //取得MET04_0600資料
            List<MET04_0600> MET04List = repoMET04_0600.Get_DataList(pTkCode);
            foreach (MET04_0600 MET04_0600 in MET04List)
            {
                DataRow drow = dtNgList.NewRow();
                drow["ureport_code"] = MET04_0600.ureport_code;
                drow["mo_code"] = pTkCode;
                drow["work_code"] = MET04_0600.work_code;
                drow["ng_code"] = MET04_0600.ng_code;
                drow["ng_name"] = MET04_0600.ng_name;
                drow["ng_qty"] = MET04_0600.ng_qty;

                dtNgList.Rows.Add(drow);
            }


            ViewBag.ng_data = dtNgList;
            dtWork = Get_WorkData(pTkCode);
            ViewBag.work_data = dtWork;

            return View();
        }

        public ActionResult Delete(string pTkCode, string pKey)
        {
            //刪除前的檢查要在JqGrid送出前檢查，所以對應Chk_Del_Main這個函數
            repoMET04_0600.DeleteData(pKey);
            return RedirectToAction("Class", sPrgCode, new { pTkCode = pTkCode });
        }


        /// <summary>
        /// (固定區)主檔 首頁 按下查詢按鈕 JqGrid資料來源
        /// </summary>
        /// <param name="pWhere">使用者下的查詢條件 Json</param>
        /// <returns></returns>
        public ActionResult Get_GridDataByQuery(string pWhere)
        {
            string sUsrCode = User.Identity.Name;
            //string sPrgCode = sPrgCode;

            List<MET01_0000> list = new List<MET01_0000>();
            list = repoMET01_0000.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

            list = list.Where(x => x.mo_status == "30").ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public DataTable Get_WorkData(string pMoCode)
        {
            string sSql = "SELECT a.work_code,b.work_name,a.ng_qty,0 AS class_qty,a.ng_unit" +
                " FROM MEM01_0000 a" +
                " LEFT JOIN MEB30_0000 b ON a.work_code = b.work_code" +
                " WHERE mo_code = @mo_code";
            DataTable dtTmp = comm.Get_DataTable(sSql, "mo_code", pMoCode);
            dtTmp.Columns["ng_qty"].ReadOnly = false;
            dtTmp.Columns["class_qty"].ReadOnly = false;
            foreach (DataRow row in dtTmp.Rows)
            {
                string work_code = row["work_code"].ToString();
                row["ng_qty"] = double.Parse(row["ng_qty"].ToString());
                row["class_qty"] = Get_ClassQty(pMoCode, work_code);
            }

            return dtTmp;
        }

        public DataTable Get_MED03(string pMoCode)
        {
            string sSql = "SELECT a.mo_code,a.ng_code,b.ng_name,c.work_code,SUM(a.ng_qty) AS ng_qty" +
                " FROM MED03_0000 a" +
                " LEFT JOIN MEB37_0000 b ON a.ng_code = b.ng_code" +
                " LEFT JOIN MEB30_0100 c ON a.mac_code = c.station_code" +
                " WHERE a.mo_code = @mo_code" +
                " AND is_end<> 'Y'" +
                " GROUP BY a.mo_code,a.ng_code,b.ng_name,c.work_code";
            DataTable dtTmp = comm.Get_DataTable(sSql, "mo_code", pMoCode);

            return dtTmp;
        }

        //統計已分類數量
        public double Get_ClassQty(string pMoCode, string pWorkCode)
        {
            double class_qty = 0;
            //取得MED0300數量
            string sSql = "SELECT ISNULL(SUM(a.ng_qty),0) AS ng_qty" +
                " FROM MED03_0000 a" +
                " LEFT JOIN MEB37_0000 b ON a.ng_code = b.ng_code" +
                " LEFT JOIN MEB30_0100 c ON a.mac_code = c.station_code" +
                " WHERE a.mo_code = @mo_code" +
                " AND c.work_code = @work_code" +
                " AND is_end<> 'Y'";
            DataTable dtTmp = comm.Get_DataTable(sSql, "mo_code,work_code", pMoCode + "," + pWorkCode);
            if (dtTmp.Rows.Count>0)
            {
                class_qty += double.Parse(dtTmp.Rows[0]["ng_qty"].ToString());
            }

            //取得MET04_0600數量
            sSql = "SELECT ISNULL(SUM(a.ng_qty),0) AS ng_qty" +
                " FROM MET04_0600 a" +
                " WHERE a.mo_code = @mo_code" +
                " AND a.work_code = @work_code";
            dtTmp = comm.Get_DataTable(sSql, "mo_code,work_code", pMoCode + "," + pWorkCode);
            if (dtTmp.Rows.Count > 0)
            {
                class_qty += double.Parse(dtTmp.Rows[0]["ng_qty"].ToString());
            }
            return class_qty;
        }
        /* 資料處理 向上 */


    }
}