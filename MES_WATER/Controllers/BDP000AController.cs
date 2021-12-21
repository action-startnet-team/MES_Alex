using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MES_WATER.Models;
using MES_WATER.Repository;
using System.Data;
using System.Linq.Dynamic;
using System.Web.Security;
using System.Reflection;
using Newtonsoft.Json;

namespace MES_WATER.Controllers
{
    [Authorize] //登入驗證
    [HandleError(View = "Error")]  //錯誤導向

    public class BDP000AController : Controller
    {
        //程式代號
        public static string pubPrgCode = "BDP000A";
        BDP00_0000Repository repoBDP00_0000 = new BDP00_0000Repository();
        Comm comm = new Comm();

        public ActionResult Index()
        {
            ViewBag.limit_str = comm.Get_LimitByUsrCode(User.Identity.Name, pubPrgCode);
            ViewBag.prg_code = pubPrgCode;
            return View();
        }

        public JsonResult GetData_DataTable()
        {
            string sPrgCode = pubPrgCode;
            string sUsrCode = User.Identity.Name;
            List<BDP00_0000> list = new List<BDP00_0000>();
            list = repoBDP00_0000.Get_DataList(sUsrCode, sPrgCode);

            var returnObj = new
            {
                data = list
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetData_Full(int draw, int start, int length, string searchText)
        {
            string sPrgCode = pubPrgCode;
            string sUsrCode = User.Identity.Name;

            //程式修改處 向下//
            //要回傳的分頁資料
            List<BDP00_0000> _myRecords = new List<BDP00_0000>();
            _myRecords = repoBDP00_0000.Get_DataList(sUsrCode, sPrgCode);

            //總資料
            var query = _myRecords.AsEnumerable();

            //查詢   
            if (!string.IsNullOrEmpty(searchText))
            {
                // 對 class 中的 member 動態產生 query 清單
                BDP00_0000 obj = new BDP00_0000();
                List<IEnumerable<BDP00_0000>> arrQuery = new List<IEnumerable<BDP00_0000>>();
                foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
                {
                    arrQuery.Add(_myRecords.Where(m => propertyInfo.GetValue(m).ToString().Contains(searchText)));
                }
                for (int i = 0; i < arrQuery.Count(); i++)
                {
                    if (Utils.IsAny(arrQuery[i]))
                    {
                        query = arrQuery[i];
                        break;
                    }
                }
            }
            //程式修改處 向上//


            int skip = start;//起始資料列索引值(略過幾筆)

            #region jQuery DataTables的排序資料行
            //jQuery DataTable的Column index
            string col_index = Request.Form["order[0][column]"];
            //col_index 換算成 資料行名稱
            //排序資料行名稱
            string sortColName = string.IsNullOrEmpty(col_index) ? "sysid" : Request.Form[$@"columns[{col_index}][data]"];
            //升冪或降冪
            string asc_desc = string.IsNullOrEmpty(Request.Form["order[0][dir]"]) ? "desc" : Request.Form["order[0][dir]"];//防呆
            #endregion

            //查詢&排序後的總筆數
            int recordsTotal = 0;

            //排序
            query = query.OrderBy($@"{sortColName} {asc_desc}"); //排序使用到System.Linq.Dynamic

            recordsTotal = query.Count();//查詢後的總筆數

            if (length == -1)
            {//抓全部資料
                _myRecords = query.ToList();
            }
            else
            {//分頁 
                _myRecords = query.Skip(skip).Take(length)
                            .ToList();
            }

            //回傳Json資料
            var returnObj =
              new
              {
                  draw = draw,
                  recordsTotal = recordsTotal,
                  recordsFiltered = recordsTotal,
                  data = _myRecords//分頁後的資料 
              };
            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }

        //  資料處理 向下  //
        public ActionResult Update(string pTkCode)
        {
            BDP00_0000 data = repoBDP00_0000.GetDTO(pTkCode);
            ViewBag.prg_code = pubPrgCode;
            return View(data);
        }

        [HttpPost]
        public ActionResult Update(FormCollection form)
        {
            //取值並且做html值與DB所需值的轉換
            BDP00_0000 newData = new BDP00_0000();
            newData.par_name = comm.Chg_HtmlToDB(form["par_name"], "textbox");
            newData.par_value = comm.Chg_HtmlToDB(form["par_value"], "textbox");
            newData.par_memo = comm.Chg_HtmlToDB(form["par_memo"], "textbox");

            repoBDP00_0000.UpdateData(newData);
            return RedirectToAction("Index");
        }

        public ActionResult Insert()
        {
            //新增模式傳DTO是為了呈現欄位名稱
            BDP00_0000 data = new BDP00_0000();
            ViewBag.prg_code = pubPrgCode;
            return View(data);
        }

        [HttpPost]
        public ActionResult Insert(FormCollection form)
        {
            BDP00_0000 newData = new BDP00_0000();
            newData.par_name = comm.Chg_HtmlToDB(form["par_name"], "textbox");
            newData.par_value = comm.Chg_HtmlToDB(form["par_value"], "textbox");
            newData.par_memo = comm.Chg_HtmlToDB(form["par_memo"], "textbox");
            repoBDP00_0000.InsertData(newData);
            return RedirectToAction("Index");
            //BDP00_0000 data = new BDP00_0000();
            //ViewBag.prg_code = pubPrgCode;
            //ViewBag.ErrorMessage = "代碼重覆";
            //return View();
        }

        public ActionResult Delete(string pTkCode)
        {

            repoBDP00_0000.DeleteData(pTkCode);
            return RedirectToAction("Index");
        }
        //資料處理 向上//


        //資料檢查 向下//
        [HttpPost]
        public ActionResult Check_Data(string pTkCode)
        {
            //檢查資料代碼是否重覆
            string grp_name = comm.Get_QueryData("BDP00_0000", pTkCode, "par_name", "par_value");
            if (string.IsNullOrEmpty(grp_name))
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }
        //資料檢查 向上//
    }
}