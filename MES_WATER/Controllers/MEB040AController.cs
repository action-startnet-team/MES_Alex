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
    public class MEB040AController : Controller
    {
        public string sPrgCode = "MEB040A";
        Comm comm = new Comm();
        MEB03_0000Repository repoMEB03_0000 = new MEB03_0000Repository();
        MEB02_0000Repository repoMEB02_0000 = new MEB02_0000Repository();
        // GET: MEB040A
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

        public class test : ILearnInterface
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

            List<MEB03_0000> list = new List<MEB03_0000>();
            list = repoMEB03_0000.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        //首頁 主檔 JqGird的資料來源
        public ActionResult Get_GridData()
        {
            List<MEB03_0000> list = repoMEB03_0000.Get_DataList("");

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //首頁 明細檔 JqGird的資料來源
        public ActionResult Get_GridData_D1(string pTkCode)
        {
            List<MEB02_0000> list = Get_MoTypeCodeByLotTypeCode(pTkCode);
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 分類畫面取資料
        /// </summary>
        /// <param name="pLotTypeCode">分類主編號</param>
        /// <param name="bGetNonSelected">true/false 抓未分派的資料</param>
        /// <returns></returns>
        public JsonResult Get_DualListBoxData(string pLotTypeCode, bool bGetNonSelected)
        {
            object selectedList = new object();
            object nonSelectedList = new object();

            //修改處
            if (!string.IsNullOrEmpty(pLotTypeCode))
            {
                List<MEB02_0000> data = Get_MoTypeCodeByLotTypeCode(pLotTypeCode);
                selectedList = data.Select(
                    item => new {
                        optionValue = item.mo_type_code,
                        optionText = item.mo_type_code + " - " + item.mo_type_name
                    }
                ).ToList();
            }

            if (bGetNonSelected)
            {
                List<MEB02_0000> data = Get_MoTypeCodeByLotTypeCode("");
                nonSelectedList = data.Select(
                    item => new {
                        optionValue = item.mo_type_code,
                        optionText = item.mo_type_code + " - " + item.mo_type_name
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
        public void Update_LotTypeCode(List<string> pMoTypeCodeList, string pLotTypeCode)
        {
            List<string> MEB02_0000List = Get_MoTypeCodeByLotTypeCode(pLotTypeCode).Select(x => x.mo_type_code).ToList();

            // view 某批號類型為空
            // view 傳 null給 pMoTypeCodeList, pMoTypeCodeList轉為 [""] 
            if (pMoTypeCodeList.Count == 1 && string.IsNullOrEmpty(pMoTypeCodeList[0]))
            {
                // 某批號類型的所有工單類型的批號類型值設為空
                for (int i = 0; i < MEB02_0000List.Count; i++)
                {
                    comm.Upd_QueryData("MEB02_0000", "mo_type_code", MEB02_0000List[i], "lot_type_code", "");
                }
            }
            else
            {
                // 原本批號類型的工單類型不在傳來的批號類型工單類型清單內，則批號類型設為空
                List<string> exclude_list = MEB02_0000List.Where(x => !pMoTypeCodeList.Contains(x)).ToList();
                for (int i = 0; i < exclude_list.Count; i++)
                {
                    comm.Upd_QueryData("MEB02_0000", "mo_type_code", exclude_list[i], "lot_type_code", "");
                }

                // 變更傳來的清單的批號類型
                for (int i = 0; i < pMoTypeCodeList.Count; i++)
                {
                    comm.Upd_QueryData("MEB02_0000", "mo_type_code", pMoTypeCodeList[i], "lot_type_code", pLotTypeCode);
                }
            }
        }

        /// <summary>
        /// 取得批號類型的工單類型清單
        /// </summary>
        /// <param name="pLotTypeCode">工單類型批號類型的代碼</param>
        /// <returns></returns>
        private List<MEB02_0000> Get_MoTypeCodeByLotTypeCode(string pLotTypeCode)
        {
            List<MEB02_0000> list = new List<MEB02_0000>();
            //if (pLotTypeCode == null)
            //{
            //    // 全部資料
            //    list = repoMEB02_0000.Get_DataList("");

            //    return list;
            //}

            // 批號類型的工單類型
            string sSql = "SELECT * FROM MEB02_0000 where lot_type_code=@lot_type_code";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                list = con_db.Query<MEB02_0000>(sSql, new { lot_type_code = pLotTypeCode }).ToList();
            }

            return list;
        }

    }
}