using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MES_WATER.Models;
using MES_WATER.Repository;

namespace MES_WATER.Controllers
{
    public class TitleController : Controller
    {
        // GET: Title
        Comm comm = new Comm();

        public ActionResult pTitle()
        {
            //連線字串
            //if (!string.IsNullOrEmpty(Session["iDbName"].ToString()))
            //{
            //    pDbConn.pDbName = Session["iDbName"].ToString();
            //}

            //公司名稱
            ViewBag.prj_name = comm.Get_QueryData("BDP00_0000", "prj_name", "par_name", "par_value");
            ViewBag.com_name = comm.Get_QueryData("BDP00_0000", "com_name", "par_name", "par_value");
            ViewBag.usr_name = HttpContext.User.Identity.Name;

            string usr_code = HttpContext.User.Identity.Name;

            //連絡事項資訊
            List<BDP16_0000> data = comm.Get_BDP16_0000(usr_code);

            // 公告資訊
            BDP230BRepository repo = new BDP230BRepository();

            //string sor_date = "2020/02/17";
            string sor_date = comm.Get_Date();
            string bull_url = "/Bulletin/board/";

            List<BDP23_0000> bulletinData = repo.Get_BoardData(User.Identity.Name, sor_date);

            List<BDP16_0000> todoList_bull = bulletinData.Select(
                x => new BDP16_0000()
                {
                    todo_name = "(公告) " + x.theme,
                    todo_url = bull_url + x.bdp23_0000.ToString(),
                    todo_key = x.bdp23_0000.ToString()
                }).ToList();

            //data = data.Concat(todoList_bull).ToList();
            ViewBag.todoList_bull = todoList_bull;

            ViewBag.todolist = data;

            return PartialView();
        }
    }
}