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
    public class EMB230AController : Controller
    {
        public string sPrgCode = "EMB230A";
        Comm comm = new Comm();
        EMB05_0000Repository repoEMB05_0000 = new EMB05_0000Repository();
        EMB05_0300Repository repoEMB05_0300 = new EMB05_0300Repository();
        // GET: EMB230A
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

            List<EMB05_0000> list = new List<EMB05_0000>();
            list = repoEMB05_0000.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        //首頁 主檔 JqGird的資料來源
        public ActionResult Get_GridData()
        {
            List<EMB05_0000> list = repoEMB05_0000.Get_DataList("");

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //首頁 明細檔 JqGird的資料來源
        public ActionResult Get_GridData_D1(string pTkCode)
        {
            List<EMB05_0300> list = Get_FalutHandleCodeByMaiTypeCode(pTkCode);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 分類畫面取資料
        /// </summary>
        /// <param name="pMaiTypeCode">分類主編號</param>
        /// <param name="bGetNonSelected">true/false 抓未分派的資料</param>
        /// <returns></returns>
        public JsonResult Get_FalutHandleCodeList(string pMaiTypeCode, bool bGetNonSelected)
        {
            object selectedList = new object();
            object nonSelectedList = new object();

            //修改處
            if (!string.IsNullOrEmpty(pMaiTypeCode))
            {
                List<EMB05_0300> data = Get_FalutHandleCodeByMaiTypeCode(pMaiTypeCode);
                selectedList = data.Select(
                    item => new {
                        optionValue = item.fault_handle_code,
                        optionText = item.fault_handle_code + " - " + item.fault_handle_name
                    }
                ).ToList();
            }

            if (bGetNonSelected)
            {
                List<EMB05_0300> data = Get_FalutHandleCode_NonSelected(pMaiTypeCode);
                nonSelectedList = data.Select(
                    item => new {
                        optionValue = item.fault_handle_code,
                        optionText = item.fault_handle_code + " - " + item.fault_handle_name
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
        public void Update_FalutHandleCode(List<string> pFalutHandleCodeList, string pMaiTypeCode)
        {
            comm.Del_QueryData("EMB05_0300", "mai_type_code", pMaiTypeCode);

            // 將選擇的資料新增
            for (int i = 0; i < pFalutHandleCodeList.Count; i++)
            {
                EMB05_0300 data = new EMB05_0300();
                data.mai_type_code = pMaiTypeCode;
                data.fault_handle_code = pFalutHandleCodeList[i];

                if (!string.IsNullOrEmpty(pFalutHandleCodeList[i]))
                {
                    repoEMB05_0300.InsertData(data);
                    // 新增紀錄資料
                    comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);
                }
            }
        }



        /// <summary>
        /// 取得尚未分派的故障原因
        /// </summary>
        /// <param name="pMaiTypeCode"></param>pFalutHandleCode
        /// <returns></returns>
        private List<EMB05_0300> Get_FalutHandleCode_NonSelected(string pMaiTypeCode)
        {
            List<EMB05_0300> list = new List<EMB05_0300>();
            string sSql = " Select EMB20_0000.fault_handle_code,fault_handle_name from EMB20_0000 " +
                          " Where fault_handle_code not in (Select fault_handle_code from EMB05_0300 where mai_type_code = @mai_type_code) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                list = con_db.Query<EMB05_0300>(sSql, new { mai_type_code = pMaiTypeCode }).ToList();
            }

            return list;
        }



        /// <summary>
        /// 取得單一保養設備群組的故障原因清單
        /// </summary>
        /// <param name="pMaiTypeCode">保養設備群組的代碼</param>
        /// <returns></returns>
        private List<EMB05_0300> Get_FalutHandleCodeByMaiTypeCode(string pMaiTypeCode)
        {
            List<EMB05_0300> list = new List<EMB05_0300>();
            string sSql = " SELECT EMB05_0300.*,EMB20_0000.fault_handle_name as fault_handle_name " +
                          " FROM EMB05_0300 " +
                          " left join EMB20_0000 on EMB20_0000.fault_handle_code = EMB05_0300.fault_handle_code " +
                          " where EMB05_0300.mai_type_code = @mai_type_code " +
                          " order by fault_handle_code ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                list = con_db.Query<EMB05_0300>(sSql, new { mai_type_code = pMaiTypeCode }).ToList();
            }

            return list;
        }


        //Insert2


        /// <summary>
        /// 分類畫面取資料
        /// </summary>
        /// <param name="pFalutHandleCode">分類主編號</param>
        /// <param name="bGetNonSelected">true/false 抓未分派的資料</param>
        /// <returns></returns>
        public JsonResult Get_MaiTypeCodeList(string pFalutHandleCode, bool bGetNonSelected)
        {
            object selectedList = new object();
            object nonSelectedList = new object();

            //修改處
            if (!string.IsNullOrEmpty(pFalutHandleCode))
            {
                List<EMB05_0300> data = Get_MaiTypeCodeByFalutHandleCode(pFalutHandleCode);
                selectedList = data.Select(
                    item => new {
                        optionValue = item.mai_type_code,
                        optionText = item.mai_type_code + " - " + item.mai_type_name
                    }
                ).ToList();
            }

            if (bGetNonSelected)
            {
                List<EMB05_0300> data = Get_MaiTypeCode_NonSelected(pFalutHandleCode);
                nonSelectedList = data.Select(
                    item => new {
                        optionValue = item.mai_type_code,
                        optionText = item.mai_type_code + " - " + item.mai_type_name
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
        public void Update_MaiTypeCode(List<string> pMaiTypeCodeList, string pFalutHandleCode)
        {
            comm.Del_QueryData("EMB05_0300", "fault_handle_code", pFalutHandleCode);

            // 將選擇的資料新增
            for (int j = 0; j < pMaiTypeCodeList.Count; j++)
            {
                EMB05_0300 data = new EMB05_0300();
                data.mai_type_code = pMaiTypeCodeList[j];
                data.fault_handle_code = pFalutHandleCode;

                if (!string.IsNullOrEmpty(pMaiTypeCodeList[j]))
                {
                    repoEMB05_0300.InsertData(data);
                    // 新增紀錄資料
                    comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);
                }
            }
        }



        /// <summary>
        /// 取得尚未分派的保養設備群組
        /// </summary>
        /// <param name="pFalutHandleCode"></param>
        /// <returns></returns>
        private List<EMB05_0300> Get_MaiTypeCode_NonSelected(string pFalutHandleCode)
        {
            List<EMB05_0300> list2 = new List<EMB05_0300>();
            string sSql = " Select EMB05_0000.mai_type_code,mai_type_name from EMB05_0000 " +
                          " Where mai_type_code not in (Select mai_type_code from EMB05_0300 where fault_handle_code = @fault_handle_code) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                list2 = con_db.Query<EMB05_0300>(sSql, new { fault_handle_code = pFalutHandleCode }).ToList();
            }

            return list2;
        }



        /// <summary>
        /// 取得單一故障原因的保養設備群組清單
        /// </summary>
        /// <param name="pFalutHandleCode">保養設備群組的代碼</param>
        /// <returns></returns>
        private List<EMB05_0300> Get_MaiTypeCodeByFalutHandleCode(string pFalutHandleCode)
        {
            List<EMB05_0300> list2 = new List<EMB05_0300>();
            string sSql = " SELECT EMB05_0300.*,EMB05_0000.mai_type_name as mai_type_name" +
                          " FROM EMB05_0300 " +
                          " left join EMB05_0000 on EMB05_0000.mai_type_code = EMB05_0300.mai_type_code " +
                          " where EMB05_0300.fault_handle_code = @fault_handle_code" +
                          " order by mai_type_code ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                list2 = con_db.Query<EMB05_0300>(sSql, new { fault_handle_code = pFalutHandleCode }).ToList();
            }

            return list2;
        }

    }
}