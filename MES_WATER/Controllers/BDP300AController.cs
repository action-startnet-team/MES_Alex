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
using System.Collections;
using System.Data.SqlClient;
using Dapper;

namespace MES_WATER.Controllers
{
    [Authorize] //登入驗證
    [HandleError(View = "Error")]  //錯誤導向

    public class BDP300AController : JsonNetController
    {
        //程式代號
        public static string pubPrgCode = "BDP300A";
        BDP30_0000Repository repoBDP30_0000 = new BDP30_0000Repository();
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();
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
            List<BDP30_0000> list = new List<BDP30_0000>();
            list = repoBDP30_0000.Get_DataList(sUsrCode, sPrgCode);

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
            List<BDP30_0000> _myRecords = new List<BDP30_0000>();
            _myRecords = repoBDP30_0000.Get_DataList(sUsrCode, sPrgCode);
            //總資料
            var query = _myRecords.AsEnumerable();
            //查詢   
            if (!string.IsNullOrEmpty(searchText))
            {
                // 對 class 中的 member 動態產生 query 清單
                BDP30_0000 obj = new BDP30_0000();
                List<IEnumerable<BDP30_0000>> arrQuery = new List<IEnumerable<BDP30_0000>>();
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

        [HttpPost]
        //  資料處理 向下  //
        //public ActionResult Update(string prg_code, string view_code)
        public ActionResult Update(FormCollection form)
        {

            BDP30_0000 data = new BDP30_0000();

            data.usr_code = User.Identity.Name;
            //data.prg_code = comm.sGetString(prg_code);
            //data.view_code = comm.sGetString(view_code);
            data.prg_code = form["prg_code"];
            data.view_code = form["view_code"];
            var colIndex = form.AllKeys.Where(x => x != "prg_code" && x != "view_code").ToArray();

            DataTable dtTmp = new DataTable();

            // 建立欄位
            dtTmp.Columns.Add("colIndex", typeof(int));
            dtTmp.Columns.Add("colWidth", typeof(string));
            for (int i = 0; i < colIndex.Length; i++)
            {
                // datatable
                DataRow row = dtTmp.NewRow();
                row["colIndex"] = comm.sGetInt32(colIndex[i]);
                row["colWidth"] = form[colIndex[i]];
                dtTmp.Rows.Add(row);
            }

            ViewBag.dtTmp = dtTmp;
            ViewBag.prg_code = pubPrgCode;
            return View(data);
        }

        //[HttpPost]
        //public ActionResult Update(FormCollection form, BDP30_0000 model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        BDP30_0000 data = new BDP30_0000();

        //        comm.Set_ModelValue(data, form);

        //        //取值並且做html值與DB所需值的轉換

        //        repoBDP30_0000.UpdateData(data);

        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.prg_code = pubPrgCode;
        //    return View(model);
        //}

        public ActionResult Insert()
        {
            //新增模式傳DTO是為了呈現欄位名稱
            BDP30_0000 data = new BDP30_0000();
            ViewBag.prg_code = pubPrgCode;
            return View(data);
        }
        [HttpPost]
        public ActionResult Insert(FormCollection form, BDP30_0000 model)
        {
            if (ModelState.IsValid)
            {
                BDP30_0000 data = new BDP30_0000();

                comm.Set_ModelValue(data, form);

                //取值並且做html值與DB所需值的轉換

                repoBDP30_0000.InsertData(data);

                return RedirectToAction("Index");
            }
            ViewBag.prg_code = pubPrgCode;
            return View(model);
        }

        public ActionResult Delete(string pTkCode)
        {
            repoBDP30_0000.DeleteData(pTkCode);

            return RedirectToAction("Index");
        }

        //資料處理 向上//
        [HttpPost]
        public ActionResult Check_Data(FormCollection form, BDP30_0000 model)
        {
            bool isSuccess = false;
            //檢查資料代碼是否重覆
            string key = gmv.GetKey<BDP30_0000>(new BDP30_0000());
            string sWhere = " where prg_code = '" + form["prg_code"] + "'" +
                            "   and usr_code = '" + form["usr_code"] + "'" +
                            "   and view_code = '" + form["view_code"] + "'";
            bool hasRow = !comm.Chk_RelData("BDP30_0000", sWhere);
            if (hasRow)
            {
                ModelState.AddModelError(form["view_code"], "view代碼已存在!");
                isSuccess = true;
            }
            var returnData = new
            {
                // 成功與否
                IsSuccess = isSuccess,
                // ModelState錯誤訊息 
                ModelStateErrors = ModelState.Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(k => k.Key, k => k.Value.Errors.Select(e => e.ErrorMessage).ToArray())
            };
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(returnData), "application/json");
        }


        //資料檢查 向上//

        /// <summary>
        /// 欄位寬度 新增修改
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddEdit(List<BDP30_0000> list)
        {
            string sSorViewCode = "";
            for (int i = 0; i < list.Count; i++)
            {

                string usr_code = User.Identity.Name;
                list[i].usr_code = usr_code;

                string prg_code = list[i].prg_code;
                string view_code = list[i].view_code;
                sSorViewCode = view_code;
                int col_index = list[i].col_index;

                string sSql = "select top 1 * from BDP30_0000 " +
                             "  where usr_code = @usr_code " +
                             "      and prg_code = @prg_code " +
                             "      and view_code = @view_code " +
                             "      and col_index = @col_index ";

                string Para = "usr_code,prg_code,view_code,col_index";
                string ParaValue = usr_code + "," + prg_code + "," + view_code + "," + col_index.ToString();
                DataTable dtTmp = comm.Get_DataTable(sSql, Para, ParaValue);
                if (dtTmp.Rows.Count <= 0)
                {
                    repoBDP30_0000.InsertData(list[i]);
                }
                else
                {
                    repoBDP30_0000.UpdateData(list[i]);
                }

            }

            return sSorViewCode + "儲存成功!";
        }

        public string InsertUpdateIsShow(List<BDP30_0100> list)
        {
            BDP30_0100 data = new BDP30_0100();
            string sSorViewCode = "";

            // 一般 usr_code, prg_code, view_code是一樣的
            data.usr_code = User.Identity.Name;
            data.prg_code = list[0].prg_code;
            data.view_code = list[0].view_code;

            sSorViewCode = list[0].view_code;

            for (int i = 0; i < list.Count; i++)
            {
                // 取值 
                data.col_index = list[i].col_index;
                data.is_show = list[i].is_show;

                // 檢查資料是否已存在
                string sSql = "select top 1 * from BDP30_0100 " +
                             "  where usr_code = @usr_code " +
                             "      and prg_code = @prg_code " +
                             "      and view_code = @view_code " +
                             "      and col_index = @col_index ";

                string Para = "usr_code,prg_code,view_code,col_index";
                string ParaValue = data.usr_code + "," + data.prg_code + "," + data.view_code + "," + data.col_index;
                DataTable dtTmp = comm.Get_DataTable(sSql, Para, ParaValue);
                if (dtTmp.Rows.Count <= 0)
                {
                    // 新增
                    string sSql_Insert = "INSERT INTO " +
                          " BDP30_0100 (prg_code, usr_code, view_code, col_index, is_show) " +
                          "     VALUES (@prg_code, @usr_code, @view_code, @col_index, @is_show) ";
                    using (SqlConnection con_db = comm.Set_DBConnection())
                    {
                        con_db.Execute(sSql_Insert, data);
                    }
                }
                else
                {
                    // 修改
                    string sSql_Update = " UPDATE BDP30_0100 " +
                          "     SET is_show = @is_show" +
                          "     WHERE prg_code= @prg_code " +
                          "         and usr_code = @usr_code " +
                          "         and view_code = @view_code " +
                          "         and col_index = @col_index ";
                    using (SqlConnection con_db = comm.Set_DBConnection())
                    {
                        con_db.Execute(sSql_Update, data);
                    }
                }
                // end of dt
            }
            // end of for loop

            return sSorViewCode + " 欄位調整 儲存成功!";
        }


    }
}
