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
    public class MEB160AController : Controller
    {
        public string sPrgCode = "MEB160A";
        Comm comm = new Comm();
        MEB14_0000Repository repoMEB14_0000 = new MEB14_0000Repository();
        MEB15_0000Repository repoMEB15_0000 = new MEB15_0000Repository();
        // GET: MEB160A
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

        public interface ILearnInterface
        {
            List<string> GetList();
        }

        public class test: ILearnInterface
        {
            public List<string> GetList()
            {
                List<string> list = new List<string>();
                return list;
            }
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

            List<MEB14_0000> list = new List<MEB14_0000>();
            list = repoMEB14_0000.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        //首頁 主檔 JqGird的資料來源
        public ActionResult Get_GridData()
        {
            List<MEB14_0000> list = repoMEB14_0000.Get_DataList("");

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //首頁 明細檔 JqGird的資料來源
        public ActionResult Get_GridData_D1(string pTkCode)
        {
            List<MEB15_0000> list = Get_MacCodeByType(pTkCode);
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 分類畫面取資料
        /// </summary>
        /// <param name="pTypeCode">分類主編號</param>
        /// <param name="bGetNonSelected">true/false 抓未分派的資料</param>
        /// <returns></returns>
        public JsonResult Get_DualListBoxData(string pTypeCode, bool bGetNonSelected)
        {
            object selectedList = new object();
            object nonSelectedList = new object();

            //修改處
            if (!string.IsNullOrEmpty(pTypeCode))
            {
                List<MEB15_0000> data = Get_MacCodeByType(pTypeCode);
                selectedList = data.Select(
                    item => new {
                        optionValue = item.mac_code,
                        optionText = item.mac_code + " - " + item.mac_name
                    }
                ).ToList();
            }

            if (bGetNonSelected)
            {
                List<MEB15_0000> data = Get_MacCodeByType("");
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
        public void Update_MacType(List<string> pMacCodeList, string pTypeCode)
        {
            List<string> MEB15_0000List = Get_MacCodeByType(pTypeCode).Select(x => x.mac_code).ToList();

            // view 某群組為空
            // view 傳 null給 pMacCodeList, pMacCodeList轉為 [""] 
            if (pMacCodeList.Count == 1 && string.IsNullOrEmpty(pMacCodeList[0]))
            {
                // 某群組的所有機台的群組值設為空
                for (int i = 0; i < MEB15_0000List.Count; i++)
                {
                    comm.Upd_QueryData("MEB15_0000", "mac_code", MEB15_0000List[i], "mac_type_code", "");
                }
            }
            else
            {
                // 原本群組的機台不在傳來的群組機台清單內，則群組設為空
                List<string> exclude_list = MEB15_0000List.Where(x => !pMacCodeList.Contains(x)).ToList();
                for (int i = 0; i < exclude_list.Count; i++)
                {
                    comm.Upd_QueryData("MEB15_0000", "mac_code", exclude_list[i], "mac_type_code", "");
                }

                // 變更傳來的清單的群組
                for (int i = 0; i < pMacCodeList.Count; i++)
                {
                    comm.Upd_QueryData("MEB15_0000", "mac_code", pMacCodeList[i], "mac_type_code", pTypeCode);
                }
            }
        }

        /// <summary>
        /// 取得群組的機台清單
        /// </summary>
        /// <param name="pTypeCode">機台群組的代碼</param>
        /// <returns></returns>
        private List<MEB15_0000> Get_MacCodeByType(string pTypeCode)
        {
            List<MEB15_0000> list = new List<MEB15_0000>();
            //if (pTypeCode == null)
            //{
            //    // 全部資料
            //    list = repoMEB15_0000.Get_DataList("");

            //    return list;
            //}

            // 群組的機台
            string sSql = " SELECT MEB15_0000.*, MEB12_0000.line_name " +
                          " FROM MEB15_0000 " +
                          " left join MEB12_0000 on MEB12_0000.line_code = MEB15_0000.line_code " +
                          " where MEB15_0000.mac_type_code=@mac_type_code " ;
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                list = con_db.Query<MEB15_0000>(sSql, new { mac_type_code = pTypeCode }).ToList();
            }

            return list;
        } 

    }
}