using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MES_WATER.Models;
using MES_WATER.Repository;
using Newtonsoft.Json;
using System.Collections;
using System.Data.SqlClient;
using Dapper;
using System.ComponentModel;


namespace MES_WATER.Controllers
{
    public class WMB140BController : JsonNetController
    {
        Comm comm = new Comm();
        MET01_0000Repository repoMET01_0000 = new MET01_0000Repository();
        WMB140AController WMB140AController = new WMB140AController();

        // GET: WMB140B
        public ActionResult Index(string pro_code = "")
        {
            string pro_name = comm.Get_QueryData("MEB20_0000", pro_code, "pro_code", "pro_name");



            ViewBag.pro_code = pro_code;
            ViewBag.pro_name = pro_name;

            return View();
        }

        [HttpPost]
        public JsonResult Get_TimeLineData(FormCollection form)
        {

            string pro_code = form["pro_code"];
            string cal_date = form["cal_date"];
            string lot_no = form["lot_no"];

            JqGridQueryData query_data = new JqGridQueryData();

            query_data.query_conditions.Add(new JqGridQueryData.FieldData() { field_code = "pro_code",  field_value = pro_code });
            query_data.query_conditions.Add(new JqGridQueryData.FieldData() { field_code = "cal_date",  field_value = cal_date });
            query_data.query_conditions.Add(new JqGridQueryData.FieldData() { field_code = "lot_no", field_value = lot_no });

            List<WMB140AController.WMB140A> data = WMB140AController.Get_StatData(query_data);

            return Json(data, JsonRequestBehavior.AllowGet);

        }


    }
}