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
    public class MEB290BController : Controller
    {
        public string sPrgCode = "MEB290B";
        Comm comm = new Comm();
        MEB29_0000Repository repoMEB29_0000 = new MEB29_0000Repository();
        MEB29_0200Repository repoMEB29_0200 = new MEB29_0200Repository();
        // GET: MEB290B
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

            List<MEB29_0000> list = new List<MEB29_0000>();
            list = repoMEB29_0000.Get_DataListByQuery(sUsrCode, sPrgCode, pWhere);

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        //首頁 主檔 JqGird的資料來源
        public ActionResult Get_GridData()
        {
            List<MEB29_0000> list = repoMEB29_0000.Get_DataList("");

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //首頁 明細檔 JqGird的資料來源
        public ActionResult Get_GridData_D1(string pTkCode)
        {
            List<MEB29_0200> list = Get_MacCodeByStationcCode(pTkCode);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 分類畫面取資料
        /// </summary>
        /// <param name="pStationcCode">分類主編號</param>
        /// <param name="bGetNonSelected">true/false 抓未分派的資料</param>
        /// <returns></returns>
        public JsonResult Get_MacCodeList(string pStationcCode, bool bGetNonSelected)
        {
            object selectedList = new object();
            object nonSelectedList = new object();

            //修改處
            if (!string.IsNullOrEmpty(pStationcCode))
            {
                List<MEB29_0200> data = Get_MacCodeByStationcCode(pStationcCode);
                selectedList = data.Select(
                    item => new {
                        optionValue = item.mac_code,
                        optionText = item.mac_code + " - " + item.mac_name
                    }
                ).ToList();
            }

            if (bGetNonSelected)
            {
                List<MEB29_0200> data = Get_MacCode_NonSelected(pStationcCode);
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
        public void Update_MacCode(List<string> pMacCodeList, string pStationcCode)
        {
            comm.Del_QueryData("MEB29_0200", "station_code", pStationcCode);

            // 將選擇的資料新增
            for (int i = 0; i < pMacCodeList.Count; i++)
            {
                MEB29_0200 data = new MEB29_0200();
                data.station_code = pStationcCode;
                data.mac_code = pMacCodeList[i];

                if (!string.IsNullOrEmpty(pMacCodeList[i]))
                {
                    repoMEB29_0200.InsertData(data);
                    // 新增紀錄資料
                    comm.Ins_BDP20_0000ForMdy(User.Identity.Name, sPrgCode, "insert", "", data);

                }
            }
        }



        /// <summary>
        /// 取得尚未分派的機台:一個機台只能分派到一個站別
        /// </summary>
        /// <param name="pStationcCode"></param>pMacCode
        /// <returns></returns>
        private List<MEB29_0200> Get_MacCode_NonSelected(string pStationcCode)
        {
            List<MEB29_0200> list = new List<MEB29_0200>();
            ///string sSql = " Select MEB15_0000.mac_code,mac_name from MEB15_0000 " +
            ///              " Where mac_code not in (Select mac_code from MEB29_0200 where station_code = @station_code) ";
            ///              
            string sSql = " Select MEB15_0000.mac_code,mac_name from MEB15_0000 " +
              " Where mac_code not in (Select mac_code from MEB29_0200) ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                list = con_db.Query<MEB29_0200>(sSql, new { station_code = pStationcCode }).ToList();
            }

            return list;
        }



        /// <summary>
        /// 取得單一站別的機台代號清單
        /// </summary>
        /// <param name="pStationcCode">站別代號的代碼</param>
        /// <returns></returns>
        private List<MEB29_0200> Get_MacCodeByStationcCode(string pStationcCode)
        {
            List<MEB29_0200> list = new List<MEB29_0200>();
            string sSql = " SELECT MEB29_0200.*,MEB15_0000.mac_name as mac_name " +
                          " FROM MEB29_0200 " +
                          " left join MEB15_0000 on MEB15_0000.mac_code = MEB29_0200.mac_code " +
                          " where MEB29_0200.station_code = @station_code ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                list = con_db.Query<MEB29_0200>(sSql, new { station_code = pStationcCode }).ToList();
            }

            return list;
        }


    }
}