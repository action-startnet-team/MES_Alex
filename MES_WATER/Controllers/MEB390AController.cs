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
    public class MEB390AController : Controller
    {
        public string sPrgCode = "MEB390A";
        Comm comm = new Comm();
        MEB38_0000Repository repoMEB38_0000 = new MEB38_0000Repository();
        MEB37_0000Repository repoMEB37_0000 = new MEB37_0000Repository();
        // GET: MEB390A
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

            List<MEB38_0000> list = new List<MEB38_0000>();
            list = repoMEB38_0000.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        //首頁 主檔 JqGird的資料來源
        public ActionResult Get_GridData()
        {
            List<MEB38_0000> list = repoMEB38_0000.Get_DataList("");

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //首頁 明細檔 JqGird的資料來源
        public ActionResult Get_GridData_D1(string pTkCode)
        {
            List<MEB37_0000> list = Get_NgCodeByNgKindCode(pTkCode);
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 分類畫面取資料
        /// </summary>
        /// <param name="pNgKindCode">分類主編號</param>
        /// <param name="bGetNonSelected">true/false 抓未分派的資料</param>
        /// <returns></returns>
        public JsonResult Get_DualListBoxData(string pNgKindCode, bool bGetNonSelected)
        {
            object selectedList = new object();
            object nonSelectedList = new object();

            //修改處
            if (!string.IsNullOrEmpty(pNgKindCode))
            {
                List<MEB37_0000> data = Get_NgCodeByNgKindCode(pNgKindCode);
                selectedList = data.Select(
                    item => new {
                        optionValue = item.ng_code,
                        optionText = item.ng_code + " - " + item.ng_name
                    }
                ).ToList();
            }

            if (bGetNonSelected)
            {
                List<MEB37_0000> data = Get_NgCodeByNgKindCode("");
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
        public void Update_NgKindCode(List<string> pNgCodeList, string pNgKindCode)
        {
            List<string> MEB37_0000List = Get_NgCodeByNgKindCode(pNgKindCode).Select(x => x.ng_code).ToList();

            // view 某群組為空
            // view 傳 null給 pNgCodeList, pNgCodeList轉為 [""] 
            if (pNgCodeList.Count == 1 && string.IsNullOrEmpty(pNgCodeList[0]))
            {
                // 某群組的所有不良現象代號的群組值設為空
                for (int i = 0; i < MEB37_0000List.Count; i++)
                {
                    comm.Upd_QueryData("MEB37_0000", "ng_code", MEB37_0000List[i], "ng_kind_code", "");
                }
            }
            else
            {
                // 原本群組的不良現象代號不在傳來的群組不良現象代號清單內，則群組設為空
                List<string> exclude_list = MEB37_0000List.Where(x => !pNgCodeList.Contains(x)).ToList();
                for (int i = 0; i < exclude_list.Count; i++)
                {
                    comm.Upd_QueryData("MEB37_0000", "ng_code", exclude_list[i], "ng_kind_code", "");
                }

                // 變更傳來的清單的群組
                for (int i = 0; i < pNgCodeList.Count; i++)
                {
                    comm.Upd_QueryData("MEB37_0000", "ng_code", pNgCodeList[i], "ng_kind_code", pNgKindCode);
                }
            }
        }

        /// <summary>
        /// 取得群組的不良現象代號清單
        /// </summary>
        /// <param name="pNgKindCode">不良現象代號群組的代碼</param>
        /// <returns></returns>
        private List<MEB37_0000> Get_NgCodeByNgKindCode(string pNgKindCode)
        {
            List<MEB37_0000> list = new List<MEB37_0000>();
            //if (pNgKindCode == null)
            //{
            //    // 全部資料
            //    list = repoMEB37_0000.Get_DataList("");

            //    return list;
            //}

            // 群組的不良現象代號
            string sSql = "SELECT * FROM MEB37_0000 where ng_kind_code=@ng_kind_code";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                list = con_db.Query<MEB37_0000>(sSql, new { ng_kind_code = pNgKindCode }).ToList();
            }

            return list;
        }

    }
}