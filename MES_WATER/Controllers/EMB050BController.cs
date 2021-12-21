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
    public class EMB050BController : Controller
    {
        public string sPrgCode = "EMB050B";
        Comm comm = new Comm();
        EMB05_0000Repository repoEMB05_0000 = new EMB05_0000Repository();
        EMB05_0100Repository repoEMB05_0100 = new EMB05_0100Repository();
        // GET: EMB210A
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
            List<EMB05_0100> list = Get_FaultCodeByMaiTypeCode(pTkCode);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 分類畫面取資料
        /// </summary>
        /// <param name="pMaiTypeCode">分類主編號</param>
        /// <param name="bGetNonSelected">true/false 抓未分派的資料</param>
        /// <returns></returns>
        public JsonResult Get_FaultCodeList(string pMaiTypeCode, bool bGetNonSelected)
        {
            object selectedList = new object();
            object nonSelectedList = new object();

            //修改處
            if (!string.IsNullOrEmpty(pMaiTypeCode))
            {
                List<EMB05_0100> data = Get_FaultCodeByMaiTypeCode(pMaiTypeCode);
                selectedList = data.Select(
                    item => new {
                        optionValue = item.fault_code,
                        optionText = item.fault_code + " - " + item.fault_name
                    }
                ).ToList();
            }

            if (bGetNonSelected)
            {
                List<EMB05_0100> data = Get_FaultCode_NonSelected(pMaiTypeCode);
                nonSelectedList = data.Select(
                    item => new {
                        optionValue = item.fault_code,
                        optionText = item.fault_code + " - " + item.fault_name
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
        public void Update_FaultCode(List<string> pFaultCodeList, string pMaiTypeCode)
        {
            comm.Del_QueryData("EMB05_0100", "mai_type_code", pMaiTypeCode);

            // 將選擇的資料新增
            for (int i = 0; i < pFaultCodeList.Count; i++)
            {
                EMB05_0100 data = new EMB05_0100();
                data.mai_type_code = pMaiTypeCode;
                data.fault_code = pFaultCodeList[i];

                if (!string.IsNullOrEmpty(pFaultCodeList[i]))
                {
                    repoEMB05_0100.InsertData(data);
                    // 新增紀錄資料
                    comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);
                }
            }
        }



        /// <summary>
        /// 取得尚未分派的故障現象
        /// </summary>
        /// <param name="pMaiTypeCode"></param>pFaultCode
        /// <returns></returns>
        private List<EMB05_0100> Get_FaultCode_NonSelected(string pMaiTypeCode)
        {
            List<EMB05_0100> list = new List<EMB05_0100>();
            string sSql = " Select EMB18_0000.fault_code,fault_name from EMB18_0000 " +
                          " Where fault_code not in (Select fault_code from EMB05_0100 where mai_type_code = @mai_type_code) ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                list = con_db.Query<EMB05_0100>(sSql, new { mai_type_code = pMaiTypeCode }).ToList();
            }

            return list;
        }



        /// <summary>
        /// 取得單一保養設備群組的故障現象清單
        /// </summary>
        /// <param name="pMaiTypeCode">保養設備群組的代碼</param>
        /// <returns></returns>
        private List<EMB05_0100> Get_FaultCodeByMaiTypeCode(string pMaiTypeCode)
        {
            List<EMB05_0100> list = new List<EMB05_0100>();
            string sSql = " SELECT EMB05_0100.*,EMB18_0000.fault_name as fault_name " +
                          " FROM EMB05_0100 " +
                          " left join EMB18_0000 on EMB18_0000.fault_code = EMB05_0100.fault_code " +
                          " where EMB05_0100.mai_type_code = @mai_type_code "+
                          " order by fault_code ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                list = con_db.Query<EMB05_0100>(sSql, new { mai_type_code = pMaiTypeCode }).ToList();
            }

            return list;
        }


        //Insert2


        /// <summary>
        /// 分類畫面取資料
        /// </summary>
        /// <param name="pFaultCode">分類主編號</param>
        /// <param name="bGetNonSelected">true/false 抓未分派的資料</param>
        /// <returns></returns>
        public JsonResult Get_MaiTypeCodeList(string pFaultCode, bool bGetNonSelected)
        {
            object selectedList = new object();
            object nonSelectedList = new object();

            //修改處
            if (!string.IsNullOrEmpty(pFaultCode))
            {
                List<EMB05_0100> data = Get_MaiTypeCodeByFaultCode(pFaultCode);
                selectedList = data.Select(
                    item => new {
                        optionValue = item.mai_type_code,
                        optionText = item.mai_type_code + " - " + item.mai_type_name
                    }
                ).ToList();
            }

            if (bGetNonSelected)
            {
                List<EMB05_0100> data = Get_MaiTypeCode_NonSelected(pFaultCode);
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
        public void Update_MaiTypeCode(List<string> pMaiTypeCodeList, string pFaultCode)
        {
            comm.Del_QueryData("EMB05_0100", "fault_code", pFaultCode);

            // 將選擇的資料新增
            for (int j = 0; j < pMaiTypeCodeList.Count; j++)
            {
                EMB05_0100 data = new EMB05_0100();
                data.mai_type_code = pMaiTypeCodeList[j];
                data.fault_code = pFaultCode;

                if (!string.IsNullOrEmpty(pMaiTypeCodeList[j]))
                {
                    repoEMB05_0100.InsertData(data);
                    // 新增紀錄資料
                    comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);
                }
            }
        }



        /// <summary>
        /// 取得尚未分派的保養設備群組
        /// </summary>
        /// <param name="pFaultCode"></param>
        /// <returns></returns>
        private List<EMB05_0100> Get_MaiTypeCode_NonSelected(string pFaultCode)
        {
            List<EMB05_0100> list2 = new List<EMB05_0100>();
            string sSql = " Select EMB05_0000.mai_type_code,mai_type_name from EMB05_0000 " +
                          " Where mai_type_code not in (Select mai_type_code from EMB05_0100 where fault_code = @fault_code) " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                list2 = con_db.Query<EMB05_0100>(sSql, new { fault_code = pFaultCode }).ToList();
            }

            return list2;
        }



        /// <summary>
        /// 取得單一故障現象的保養設備群組清單
        /// </summary>
        /// <param name="pFaultCode">保養設備群組的代碼</param>
        /// <returns></returns>
        private List<EMB05_0100> Get_MaiTypeCodeByFaultCode(string pFaultCode)
        {
            List<EMB05_0100> list2 = new List<EMB05_0100>();
            string sSql = " SELECT EMB05_0100.*,EMB05_0000.mai_type_name as mai_type_name" +
                          " FROM EMB05_0100 " +
                          " left join EMB05_0000 on EMB05_0000.mai_type_code = EMB05_0100.mai_type_code " +
                          " where EMB05_0100.fault_code = @fault_code" +
                          " order by mai_type_code ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                list2 = con_db.Query<EMB05_0100>(sSql, new { fault_code = pFaultCode }).ToList();
            }

            return list2;
        }

    }
}