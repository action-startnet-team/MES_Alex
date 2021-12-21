using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MES_WATER.Models;
using MES_WATER.Repository;
using Dapper;
using System.Data.SqlClient;

namespace MES_WATER.Controllers
{
    public class QMB040AController : Controller
    {
        public string sPrgCode = "QMB040A";
        Comm comm = new Comm();
        QMB03_0000Repository repoQMB03_0000 = new QMB03_0000Repository();
        QMB03_0200Repository repoQMB03_0200 = new QMB03_0200Repository();
        // GET: QMB040A
        public ActionResult Index_old()
        {
            return View();
        }
        public ActionResult Index()
        {
            //要結合權限控制
            //comm.ConvertNull("WMB01_0000B");

            // 使用者, controllerName, actionName
            string usr_code = User.Identity.Name;
            string prg_code = ControllerContext.RouteData.Values["controller"].ToString();
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

        public ActionResult Insert()
        {
            return View();
        }

        /// <summary>
        /// (固定區)主檔 首頁 按下查詢按鈕 JqGrid資料來源
        /// </summary>
        /// <param name="pWhere">使用者下的查詢條件 Json</param>
        /// <returns></returns>
        public ActionResult Get_GridDataByQuery(string pWhere)
        {
            string sUsrCode = User.Identity.Name;
            //string sPrgCode = pubPrgCode;

            List<QMB03_0200> list = new List<QMB03_0200>();
            list = repoQMB03_0200.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        //首頁 主檔 JqGird的資料來源
        //public ActionResult Get_GridData()
        //{
        //    List<QMB03_0000> list = repoQMB03_0000.Get_DataList("");

        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}

        //首頁 明細檔 JqGird的資料來源
        public ActionResult Get_GridData_D1(string pTkCode)
        {
            List<QMB03_0200> list = Get_ProCodeByQSheetCode(pTkCode);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 分類畫面取資料
        /// </summary>
        /// <param name="pQSheetCode">分類主編號</param>
        /// <param name="bGetNonSelected">true/false 抓未分派的資料</param>
        /// <returns></returns>
        public JsonResult Get_ProCodeList(string pQSheetCode, bool bGetNonSelected)
        {
            object selectedList = new object();
            object nonSelectedList = new object();

            //修改處
            if (!string.IsNullOrEmpty(pQSheetCode))
            {
                List<QMB03_0200> data = Get_ProCodeByQSheetCode(pQSheetCode);
                selectedList = data.Select(
                    item => new {
                        optionValue = item.pro_code,
                        optionText = item.pro_code + " - " + item.pro_name
                    }
                ).ToList();
            }

            if (bGetNonSelected)
            {
                List<QMB03_0200> data = Get_ProCode_NonSelected(pQSheetCode);
                nonSelectedList = data.Select(
                    item => new {
                        optionValue = item.pro_code,
                        optionText = item.pro_code + " - " + item.pro_name
                    }
               ).ToList();
            }
            //修改處

            var returnObj = new
            {
                selectedList = selectedList,
                nonSelectedList = nonSelectedList
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public void Update_ProCode(List<string> pProCodeList, string pQSheetCode)
        {
            comm.Del_QueryData("QMB03_0200", "qsheet_code", pQSheetCode);

            // 將選擇的資料新增
            for (int i = 0; i < pProCodeList.Count; i++)
            {
                QMB03_0200 data = new QMB03_0200();
                data.qsheet_code = pQSheetCode;
                data.pro_code = pProCodeList[i];

                if (!string.IsNullOrEmpty(pProCodeList[i]))
                {
                    repoQMB03_0200.InsertData(data);
                    // 新增紀錄資料
                    comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);
                }
            }
        }



        /// <summary>
        /// 取得尚未分派的產品
        /// </summary>
        /// <param name="pQSheetCode"></param>pProCode
        /// <returns></returns>
        private List<QMB03_0200> Get_ProCode_NonSelected(string pQSheetCode)
        {
            List<QMB03_0200> list = new List<QMB03_0200>();
            string sSql = " Select MEB20_0000.pro_code,pro_name from MEB20_0000 " +
                          " Where pro_code not in (Select pro_code from QMB03_0200 ) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                list = con_db.Query<QMB03_0200>(sSql, new { qsheet_code = pQSheetCode }).ToList();
            }

            return list;
        }



        /// <summary>
        /// 取得單一檢驗記錄表的產品清單
        /// </summary>
        /// <param name="pQSheetCode">檢驗記錄表的代碼</param>
        /// <returns></returns>
        private List<QMB03_0200> Get_ProCodeByQSheetCode(string pQSheetCode)
        {
            List<QMB03_0200> list = new List<QMB03_0200>();
            string sSql = " SELECT QMB03_0200.*,MEB20_0000.pro_name as pro_name " +
                          " FROM QMB03_0200 " +
                          " left join MEB20_0000 on MEB20_0000.pro_code = QMB03_0200.pro_code " +
                          " where QMB03_0200.qsheet_code = @qsheet_code ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                list = con_db.Query<QMB03_0200>(sSql, new { qsheet_code = pQSheetCode }).ToList();
            }

            return list;
        }
    }
}