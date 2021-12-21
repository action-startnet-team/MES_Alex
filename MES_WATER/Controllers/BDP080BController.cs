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
using Newtonsoft.Json;
using System.Reflection;

namespace MES_WATER.Controllers
{
    [Authorize] //登入驗證
    [HandleError(View = "Error")]  //錯誤導向

    public class BDP080BController : Controller
    {
        //程式代號
        public static string pubPrgCode = "BDP080B";

        BDP08_0000Repository repoBDP08_0000 = new BDP08_0000Repository();
        BDP07_0000Repository repoBDP07_0000 = new BDP07_0000Repository();
        Comm comm = new Comm();

        public ActionResult Index()
        {
            ViewBag.limit_str = comm.Get_LimitByUsrCode(User.Identity.Name, pubPrgCode);
            ViewBag.prg_code = pubPrgCode;
            return View();
        }
        private List<string> Get_ColumnsNameList(string prg_code)
        {
            Comm comm = new Comm();
            DataTable table = comm.Get_DataTable(prg_code);
            List<string> list = new List<string>();
            for (int i = 1; i <= table.Rows.Count(); i++)
            {
                list.Add(table.Rows[i]["ColumnsName"].ToString());
            }
            return list;
        }

        [HttpPost]
        public ActionResult GetData_Full(int draw, int start, int length, string searchText)
        {
            string sPrgCode = pubPrgCode;
            string sUsrCode = User.Identity.Name;

            //程式修改處 向下//
            //要回傳的分頁資料
            List<BDP08_0000> _myRecords = new List<BDP08_0000>();
            _myRecords = repoBDP08_0000.Get_DataList(sUsrCode, sPrgCode);

            //總資料
            var query = _myRecords.AsEnumerable();

            //查詢   
            if (!string.IsNullOrEmpty(searchText))
            {
                // 對 class 中的 member 動態產生 query 清單
                BDP08_0000 obj = new BDP08_0000();
                List<IEnumerable<BDP08_0000>> arrQuery = new List<IEnumerable<BDP08_0000>>();
                foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
                {
                    arrQuery.Add(_myRecords.Where(m => propertyInfo.GetValue(m).ToString().Contains(searchText)));
                }

                // 欄位從第一欄依循查詢
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
            FormCollection form = new FormCollection();
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
            BDP08_0000 data = repoBDP08_0000.GetDTO(pTkCode);
            string limit_type = comm.Get_QueryData("BDP08_0000", User.Identity.Name, "usr_code", "limit_type");
            string grp_code = comm.Get_QueryData("BDP08_0000", User.Identity.Name, "usr_code", "grp_code");
            string sup_code = comm.Get_QueryData("BDP08_0000", User.Identity.Name, "usr_code", "sup_code");
            string sup_name = comm.Get_QueryData("MPB01_0000", sup_code, "sup_code", "sup_name");

            ViewBag.limit_type = limit_type;
            ViewBag.grp_code = grp_code;
            ViewBag.sup_code = sup_code;
            ViewBag.sup_name = sup_name;
            ViewBag.prg_code = pubPrgCode;

            ViewBag.prg_code = pubPrgCode;
            return View(data);
        }

        [HttpPost]
        public ActionResult Update(FormCollection form)
        {
            //取值並且做html值與DB所需值的轉換
            BDP08_0000 newData = new BDP08_0000();

            newData.usr_code = comm.Chg_HtmlToDB(form["usr_code"], "textbox");
            newData.usr_name = comm.Chg_HtmlToDB(form["usr_name"], "textbox");
            newData.usr_pass = comm.Chg_HtmlToDB(form["usr_pass"], "textbox");
            newData.usr_tel1 = comm.Chg_HtmlToDB(form["usr_tel1"], "textbox");
            newData.usr_tel2 = comm.Chg_HtmlToDB(form["usr_tel2"], "textbox");
            newData.usr_mail = comm.Chg_HtmlToDB(form["usr_mail"], "textbox");
            newData.limit_type = comm.Chg_HtmlToDB(form["limit_type"], "textbox");
            newData.grp_code = comm.Chg_HtmlToDB(form["grp_code"], "textbox");
            newData.is_use = comm.Chg_HtmlToDB(form["is_use"], "checkbox");


            BDP08_0000 sBefore = comm.GetData<BDP08_0000>(newData);
            repoBDP08_0000.UpdateData(newData);
            // 紀錄資料
            comm.Ins_BDP20_0000ForMdy(User.Identity.Name, pubPrgCode, "update", sBefore, newData);
            return RedirectToAction("Index");
        }

        public ActionResult Insert()
        {
            //新增模式傳DTO是為了呈現欄位名稱
            BDP08_0000 model = new BDP08_0000();

            string limit_type = comm.Get_QueryData("BDP08_0000", User.Identity.Name, "usr_code", "limit_type");
            string grp_code = comm.Get_QueryData("BDP08_0000", User.Identity.Name, "usr_code", "grp_code");
            string sup_code = comm.Get_QueryData("BDP08_0000", User.Identity.Name, "usr_code", "sup_code");
            string sup_name = comm.Get_QueryData("MPB01_0000", sup_code, "sup_code", "sup_name");

            ViewBag.limit_type = limit_type;
            ViewBag.grp_code = grp_code;
            ViewBag.sup_code = sup_code;
            ViewBag.sup_name = sup_name;
            ViewBag.prg_code = pubPrgCode;
            return View(model);
        }

        [HttpPost]
        public ActionResult Insert(FormCollection form)
        {
            BDP08_0000 newData = new BDP08_0000();
            newData.usr_code = comm.Chg_HtmlToDB(form["usr_code"], "textbox");
            newData.usr_name = comm.Chg_HtmlToDB(form["usr_name"], "textbox");
            newData.usr_pass = comm.Chg_HtmlToDB(form["usr_pass"], "textbox");
            newData.usr_tel1 = comm.Chg_HtmlToDB(form["usr_tel1"], "textbox");
            newData.usr_tel2 = comm.Chg_HtmlToDB(form["usr_tel2"], "textbox");
            newData.usr_mail = comm.Chg_HtmlToDB(form["usr_mail"], "textbox");
            newData.limit_type = comm.Chg_HtmlToDB(form["limit_type"], "textbox");
            newData.grp_code = comm.Chg_HtmlToDB(form["grp_code"], "textbox");
            newData.is_use = comm.Chg_HtmlToDB(form["is_use"], "checkbox");

            repoBDP08_0000.InsertData(newData);

            // 紀錄資料
            comm.Ins_BDP20_0000ForMdy(User.Identity.Name, pubPrgCode, "insert", "", newData);
            return RedirectToAction("Index");
            //BDP08_0000 data = new BDP08_0000();
            //ViewBag.prg_code = pubPrgCode;
            //ViewBag.ErrorMessage = "代碼重覆";
            //return View();
        }

        public ActionResult Delete(string pTkCode)
        {
            BDP08_0000 sBefore = comm.GetData<BDP08_0000>(pTkCode);
            repoBDP08_0000.DeleteData(pTkCode);
            comm.Ins_BDP20_0000ForMdy(User.Identity.Name, pTkCode, "delete", sBefore, "");
            return RedirectToAction("Index");
        }
        //資料處理 向上//


        //資料檢查 向下//
        [HttpPost]
        public ActionResult Check_Data(string pTkCode)
        {
            //檢查資料代碼是否重覆
            string usr_name = comm.Get_QueryData("BDP08_0000", pTkCode, "usr_code", "usr_name");
            if (string.IsNullOrEmpty(usr_name))
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