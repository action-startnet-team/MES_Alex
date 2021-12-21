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
    public class MEB450BController : Controller
    {
        public string sPrgCode = "MEB450B";
        Comm comm = new Comm();
        MEB15_0000Repository repoMEB15_0000 = new MEB15_0000Repository();
        MEB45_0100Repository repoMEB45_0100 = new MEB45_0100Repository();
        // GET: MEB450B
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

            List<MEB15_0000> list = new List<MEB15_0000>();
            list = repoMEB15_0000.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        //首頁 主檔 JqGird的資料來源
        public ActionResult Get_GridData()
        {
            List<MEB15_0000> list = repoMEB15_0000.Get_DataList("");

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //首頁 明細檔 JqGird的資料來源
        public ActionResult Get_GridData_D1(string pTkCode)
        {
            List<MEB45_0100> list = Get_StopCodeByMacCode(pTkCode);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 分類畫面取資料
        /// </summary>
        /// <param name="pMacCode">分類主編號</param>
        /// <param name="bGetNonSelected">true/false 抓未分派的資料</param>
        /// <returns></returns>
        public JsonResult Get_StopCodeList(string pMacCode, bool bGetNonSelected)
        {
            object selectedList = new object();
            object nonSelectedList = new object();

            //修改處
            if (!string.IsNullOrEmpty(pMacCode))
            {
                List<MEB45_0100> data = Get_StopCodeByMacCode(pMacCode);
                selectedList = data.Select(
                    item => new {
                        optionValue = item.stop_code,
                        optionText = item.stop_code + " - " + item.stop_name
                    }
                ).ToList();
            }

            if (bGetNonSelected)
            {
                List<MEB45_0100> data = Get_StopCode_NonSelected(pMacCode);
                nonSelectedList = data.Select(
                    item => new {
                        optionValue = item.stop_code,
                        optionText = item.stop_code + " - " + item.stop_name
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
        public void Update_StopCode(List<string> pStopCodeList, string pMacCode)
        {
            comm.Del_QueryData("MEB45_0100", "mac_code", pMacCode);

            // 將選擇的資料新增
            for (int i = 0; i < pStopCodeList.Count; i++)
            {
                MEB45_0100 data = new MEB45_0100();
                data.mac_code = pMacCode;
                data.stop_code = pStopCodeList[i];

                if (!string.IsNullOrEmpty(pStopCodeList[i]))
                {
                    repoMEB45_0100.InsertData(data);
                    // 新增紀錄資料
                    comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);
                }
            }
        }



        /// <summary>
        /// 取得尚未分派的停機原因
        /// </summary>
        /// <param name="pMacCode"></param>pStopCode
        /// <returns></returns>
        private List<MEB45_0100> Get_StopCode_NonSelected(string pMacCode)
        {
            List<MEB45_0100> list = new List<MEB45_0100>();
            string sSql = " Select MEB45_0000.stop_code,stop_name from MEB45_0000 " +
                          " Where stop_code not in (Select stop_code from MEB45_0100 where mac_code = @mac_code) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                list = con_db.Query<MEB45_0100>(sSql, new { mac_code = pMacCode }).ToList();
            }

            return list;
        }



        /// <summary>
        /// 取得單一機台的停機原因清單
        /// </summary>
        /// <param name="pMacCode">機台的代碼</param>
        /// <returns></returns>
        private List<MEB45_0100> Get_StopCodeByMacCode(string pMacCode)
        {
            List<MEB45_0100> list = new List<MEB45_0100>();
            string sSql = " SELECT MEB45_0100.*,MEB45_0000.stop_name as stop_name " +
                          " FROM MEB45_0100 " +
                          " left join MEB45_0000 on MEB45_0000.stop_code = MEB45_0100.stop_code " +
                          " where MEB45_0100.mac_code = @mac_code ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                list = con_db.Query<MEB45_0100>(sSql, new { mac_code = pMacCode }).ToList();
            }

            return list;
        }


        //Insert2


        /// <summary>
        /// 分類畫面取資料
        /// </summary>
        /// <param name="pStopCode">分類主編號</param>
        /// <param name="bGetNonSelected">true/false 抓未分派的資料</param>
        /// <returns></returns>
        public JsonResult Get_MacCodeList(string pStopCode, bool bGetNonSelected)
        {
            object selectedList = new object();
            object nonSelectedList = new object();

            //修改處
            if (!string.IsNullOrEmpty(pStopCode))
            {
                List<MEB45_0100> data = Get_MacCodeByStopCode(pStopCode);
                selectedList = data.Select(
                    item => new {
                        optionValue = item.mac_code,
                        optionText = item.mac_code + " - " + item.mac_name
                    }
                ).ToList();
            }

            if (bGetNonSelected)
            {
                List<MEB45_0100> data = Get_MacCode_NonSelected(pStopCode);
                nonSelectedList = data.Select(
                    item => new {
                        optionValue = item.mac_code,
                        optionText = item.mac_code + " - " + item.mac_name
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
        public void Update_MacCode(List<string> pMacCodeList, string pStopCode)
        {
            comm.Del_QueryData("MEB45_0100", "stop_code", pStopCode);

            // 將選擇的資料新增
            for (int j = 0; j < pMacCodeList.Count; j++)
            {
                MEB45_0100 data = new MEB45_0100();
                data.mac_code = pMacCodeList[j];
                data.stop_code = pStopCode;

                if (!string.IsNullOrEmpty(pMacCodeList[j]))
                {
                    repoMEB45_0100.InsertData(data);
                    // 新增紀錄資料
                    comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);
                }
            }
        }



        /// <summary>
        /// 取得尚未分派的機台
        /// </summary>
        /// <param name="pStopCode"></param>
        /// <returns></returns>
        private List<MEB45_0100> Get_MacCode_NonSelected(string pStopCode)
        {
            List<MEB45_0100> list2 = new List<MEB45_0100>();
            string sSql = " Select MEB15_0000.mac_code,mac_name from MEB15_0000 " +
                          " Where mac_code not in (Select mac_code from MEB45_0100 where stop_code = @stop_code) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                list2 = con_db.Query<MEB45_0100>(sSql, new { stop_code = pStopCode }).ToList();
            }

            return list2;
        }



        /// <summary>
        /// 取得單一停機原因的機台清單
        /// </summary>
        /// <param name="pStopCode">機台的代碼</param>
        /// <returns></returns>
        private List<MEB45_0100> Get_MacCodeByStopCode(string pStopCode)
        {
            List<MEB45_0100> list2 = new List<MEB45_0100>();
            string sSql = " SELECT MEB45_0100.*,MEB15_0000.mac_name as mac_name" +
                          " FROM MEB45_0100 " +
                          " left join MEB15_0000 on MEB15_0000.mac_code = MEB45_0100.mac_code " +
                          " where MEB45_0100.stop_code = @stop_code";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                list2 = con_db.Query<MEB45_0100>(sSql, new { stop_code = pStopCode }).ToList();
            }

            return list2;
        }

    }
}