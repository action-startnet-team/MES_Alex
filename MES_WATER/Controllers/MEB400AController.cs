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
    public class MEB400AController : Controller
    {
        public string sPrgCode = "MEB400A";
        Comm comm = new Comm();
        MEB30_0000Repository repoMEB30_0000 = new MEB30_0000Repository();
        MEB30_0300Repository repoMEB30_0300 = new MEB30_0300Repository();
        // GET: MEB400A
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

        public ActionResult Insert2()
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

            List<MEB30_0000> list = new List<MEB30_0000>();
            list = repoMEB30_0000.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        //首頁 主檔 JqGird的資料來源
        public ActionResult Get_GridData()
        {
            List<MEB30_0000> list = repoMEB30_0000.Get_DataList("");

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //首頁 明細檔 JqGird的資料來源
        public ActionResult Get_GridData_D1(string pTkCode)
        {
            List<MEB30_0300> list = Get_NgCodeByWorkCode(pTkCode);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 分類畫面取資料
        /// </summary>
        /// <param name="pWorkCode">分類主編號</param>
        /// <param name="bGetNonSelected">true/false 抓未分派的資料</param>
        /// <returns></returns>
        public JsonResult Get_NgCodeList(string pWorkCode, bool bGetNonSelected)
        {
            object selectedList = new object();
            object nonSelectedList = new object();

            //修改處
            if (!string.IsNullOrEmpty(pWorkCode))
            {
                List<MEB30_0300> data = Get_NgCodeByWorkCode(pWorkCode);
                selectedList = data.Select(
                    item => new {
                        optionValue = item.ng_code,
                        optionText = item.ng_code + " - " + item.ng_name
                    }
                ).ToList();
            }

            if (bGetNonSelected)
            {
                List<MEB30_0300> data = Get_NgCode_NonSelected(pWorkCode);
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
        public void Update_NgCode(List<string> pNgCodeList, string pWorkCode)
        {
            comm.Del_QueryData("MEB30_0300", "work_code", pWorkCode);

            // 將選擇的資料新增
            for (int i = 0; i < pNgCodeList.Count; i++)
            {
                MEB30_0300 data = new MEB30_0300();
                data.work_code = pWorkCode;
                data.ng_code = pNgCodeList[i];

                if (!string.IsNullOrEmpty(pNgCodeList[i]))
                {
                    repoMEB30_0300.InsertData(data);
                    // 新增紀錄資料
                    comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);
                }
            }
        }



        /// <summary>
        /// 取得尚未分派的不良現象代號
        /// </summary>
        /// <param name="pWorkCode"></param>pNgCode
        /// <returns></returns>
        private List<MEB30_0300> Get_NgCode_NonSelected(string pWorkCode)
        {
            List<MEB30_0300> list = new List<MEB30_0300>();
            string sSql = " Select MEB37_0000.ng_code,ng_name from MEB37_0000 " +
                          " Where ng_code not in (Select ng_code from MEB30_0300 where work_code = @work_code) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                list = con_db.Query<MEB30_0300>(sSql, new { work_code = pWorkCode }).ToList();
            }

            return list;
        }



        /// <summary>
        /// 取得單一製程的不良現象代號清單
        /// </summary>
        /// <param name="pWorkCode">製程的代碼</param>
        /// <returns></returns>
        private List<MEB30_0300> Get_NgCodeByWorkCode(string pWorkCode)
        {
            List<MEB30_0300> list = new List<MEB30_0300>();
            string sSql = " SELECT MEB30_0300.*,MEB37_0000.ng_name as ng_name " +
                          " FROM MEB30_0300 " +
                          " left join MEB37_0000 on MEB37_0000.ng_code = MEB30_0300.ng_code " +
                          " where MEB30_0300.work_code = @work_code ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                list = con_db.Query<MEB30_0300>(sSql, new { work_code = pWorkCode }).ToList();
            }

            return list;
        }


        //Insert2


        /// <summary>
        /// 分類畫面取資料
        /// </summary>
        /// <param name="pNgCode">分類主編號</param>
        /// <param name="bGetNonSelected">true/false 抓未分派的資料</param>
        /// <returns></returns>
        public JsonResult Get_WorkCodeList(string pNgCode, bool bGetNonSelected)
        {
            object selectedList = new object();
            object nonSelectedList = new object();

            //修改處
            if (!string.IsNullOrEmpty(pNgCode))
            {
                List<MEB30_0300> data = Get_WorkCodeByNgCode(pNgCode);
                selectedList = data.Select(
                    item => new {
                        optionValue = item.work_code,
                        optionText = item.work_code + " - " + item.work_name
                    }
                ).ToList();
            }

            if (bGetNonSelected)
            {
                List<MEB30_0300> data = Get_WorkCode_NonSelected(pNgCode);
                nonSelectedList = data.Select(
                    item => new {
                        optionValue = item.work_code,
                        optionText = item.work_code + " - " + item.work_name
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
        public void Update_WorkCode(List<string> pWorkCodeList, string pNgCode)
        {
            comm.Del_QueryData("MEB30_0300", "ng_code", pNgCode);

            // 將選擇的資料新增
            for (int j = 0; j < pWorkCodeList.Count; j++)
            {
                MEB30_0300 data = new MEB30_0300();
                data.work_code = pWorkCodeList[j];
                data.ng_code = pNgCode;

                if (!string.IsNullOrEmpty(pWorkCodeList[j]))
                {
                    repoMEB30_0300.InsertData(data);
                    // 新增紀錄資料
                    comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);
                }
            }
        }



        /// <summary>
        /// 取得尚未分派的製程
        /// </summary>
        /// <param name="pNgCode"></param>
        /// <returns></returns>
        private List<MEB30_0300> Get_WorkCode_NonSelected(string pNgCode)
        {
            List<MEB30_0300> list2 = new List<MEB30_0300>();
            string sSql = " Select MEB30_0000.work_code,work_name from MEB30_0000 " +
                          " Where work_code not in (Select work_code from MEB30_0300 where ng_code = @ng_code) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                list2 = con_db.Query<MEB30_0300>(sSql, new { ng_code = pNgCode }).ToList();
            }

            return list2;
        }



        /// <summary>
        /// 取得單一不良現象代號的製程清單
        /// </summary>
        /// <param name="pNgCode">製程的代碼</param>
        /// <returns></returns>
        private List<MEB30_0300> Get_WorkCodeByNgCode(string pNgCode)
        {
            List<MEB30_0300> list2 = new List<MEB30_0300>();
            string sSql = " SELECT MEB30_0300.*,MEB30_0000.work_name as work_name" +
                          " FROM MEB30_0300 " +
                          " left join MEB30_0000 on MEB30_0000.work_code = MEB30_0300.work_code " +
                          " where MEB30_0300.ng_code = @ng_code";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                list2 = con_db.Query<MEB30_0300>(sSql, new { ng_code = pNgCode }).ToList();
            }

            return list2;
        }

    }
}