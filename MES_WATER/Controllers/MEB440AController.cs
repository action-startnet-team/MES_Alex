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
    public class MEB440AController : Controller
    {
        public string sPrgCode = "MEB440A";
        Comm comm = new Comm();
        MEB43_0000Repository repoMEB43_0000 = new MEB43_0000Repository();
        MEB43_0100Repository repoMEB43_0100 = new MEB43_0100Repository();
        // GET: MEB440A
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

            List<MEB43_0000> list = new List<MEB43_0000>();
            list = repoMEB43_0000.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        //首頁 主檔 JqGird的資料來源
        public ActionResult Get_GridData()
        {
            List<MEB43_0000> list = repoMEB43_0000.Get_DataList("");

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //首頁 明細檔 JqGird的資料來源
        public ActionResult Get_GridData_D1(string pTkCode)
        {
            List<MEB43_0100> list = Get_NgCodeByNgMemoCode(pTkCode);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 分類畫面取資料
        /// </summary>
        /// <param name="pNgMemoCode">分類主編號</param>
        /// <param name="bGetNonSelected">true/false 抓未分派的資料</param>
        /// <returns></returns>
        public JsonResult Get_NgCodeList(string pNgMemoCode, bool bGetNonSelected)
        {
            object selectedList = new object();
            object nonSelectedList = new object();

            //修改處
            if (!string.IsNullOrEmpty(pNgMemoCode))
            {
                List<MEB43_0100> data = Get_NgCodeByNgMemoCode(pNgMemoCode);
                selectedList = data.Select(
                    item => new {
                        optionValue = item.ng_code,
                        optionText = item.ng_code + " - " + item.ng_name
                    }
                ).ToList();
            }

            if (bGetNonSelected)
            {
                List<MEB43_0100> data = Get_NgCode_NonSelected(pNgMemoCode);
                nonSelectedList = data.Select(
                    item => new {
                        optionValue = item.ng_code,
                        optionText = item.ng_code + " - " + item.ng_name
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
        public void Update_NgCode(List<string> pNgCodeList, string pNgMemoCode)
        {
            comm.Del_QueryData("MEB43_0100", "ng_memo_code", pNgMemoCode);

            // 將選擇的資料新增
            for (int i = 0; i < pNgCodeList.Count; i++)
            {
                MEB43_0100 data = new MEB43_0100();
                data.ng_memo_code = pNgMemoCode;
                data.ng_code = pNgCodeList[i];

                if (!string.IsNullOrEmpty(pNgCodeList[i]))
                {
                    repoMEB43_0100.InsertData(data);
                    // 新增紀錄資料
                    comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);

                }
            }
        }



        /// <summary>
        /// 取得尚未分派的不良現象代號
        /// </summary>
        /// <param name="pNgMemoCode"></param>pNgCode
        /// <returns></returns>
        private List<MEB43_0100> Get_NgCode_NonSelected(string pNgMemoCode)
        {
            List<MEB43_0100> list = new List<MEB43_0100>();
            string sSql = " Select MEB37_0000.ng_code,ng_name from MEB37_0000 " +
                          " Where ng_code not in (Select ng_code from MEB43_0100 where ng_memo_code = @ng_memo_code) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                list = con_db.Query<MEB43_0100>(sSql, new { ng_memo_code = pNgMemoCode }).ToList();
            }

            return list;
        }



        /// <summary>
        /// 取得單一不良原因代號的不良現象代號清單
        /// </summary>
        /// <param name="pNgMemoCode">不良原因代號的代碼</param>
        /// <returns></returns>
        private List<MEB43_0100> Get_NgCodeByNgMemoCode(string pNgMemoCode)
        {
            List<MEB43_0100> list = new List<MEB43_0100>();
            string sSql = " SELECT MEB43_0100.*,MEB37_0000.ng_name as ng_name " +
                          " FROM MEB43_0100 " +
                          " left join MEB37_0000 on MEB37_0000.ng_code = MEB43_0100.ng_code " +
                          " where MEB43_0100.ng_memo_code = @ng_memo_code " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                list = con_db.Query<MEB43_0100>(sSql, new { ng_memo_code = pNgMemoCode }).ToList();
            }

            return list;
        }


    }
}