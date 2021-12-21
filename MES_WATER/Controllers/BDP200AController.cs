using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MES_WATER.Models;
using MES_WATER.Repository;
using System.Data.SqlClient;
using Dapper;

using Newtonsoft.Json;

namespace MES_WATER.Controllers
{
    public class BDP200AController : JsonNetController
    {
        Comm comm = new Comm();
        BDP20_0000Repository repoBDP20_0000 = new BDP20_0000Repository();
        // GET: TimeLine
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index2()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Get_TimeLineData(FormCollection form)
        {
            List<BDP20_0000> returnList = new List<BDP20_0000>();

            DynamicParameters sqlParams = new DynamicParameters();
            string SubWhere = "";

            string sUsrCode = form["usr_code"];
            List<string> sUsrCodeList = JsonConvert.DeserializeObject<List<string>>(sUsrCode);

            string sUsrDateStart = form["usr_date_start"];
            string sUsrDateEnd = form["usr_date_end"];
            string sPrgCode = form["prg_code"];
            string sUsrTimeStart = form["usr_time_start"];
            string sUsrTimeEnd = form["usr_time_end"];
            string sUsrType = form["usr_type"];

            List<string> sUsrTypeList = JsonConvert.DeserializeObject<List<string>>(sUsrType);

            string sCMEMO = form["cmemo"];

            if (string.IsNullOrEmpty(sPrgCode) && string.IsNullOrEmpty(sUsrType))
            {
                return Json(returnList, JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrEmpty(sUsrCode))
            {
                SubWhere += " and usr_code in @usr_code_list";
                sqlParams.Add("@usr_code_list", sUsrCodeList);
            }

            if (!string.IsNullOrEmpty(sUsrDateStart))
            {
                SubWhere += " and usr_date >= @usr_date_start";
                sqlParams.Add("@usr_date_start", sUsrDateStart);
            }

            if (!string.IsNullOrEmpty(sUsrDateEnd))
            {
                SubWhere += " and usr_date <= @usr_date_end";
                sqlParams.Add("@usr_date_end", sUsrDateEnd);
            }

            if (!string.IsNullOrEmpty(sPrgCode))
            {
                SubWhere += " and prg_code = @prg_code";
                sqlParams.Add("@prg_code", sPrgCode);
            }
            if (!string.IsNullOrEmpty(sUsrTimeStart))
            {
                SubWhere += " and usr_time >= @usr_time_start";
                sqlParams.Add("@usr_time_start", sUsrTimeStart);
            }
            if (!string.IsNullOrEmpty(sUsrTimeEnd))
            {
                SubWhere += " and usr_time <= @usr_time_end";
                sqlParams.Add("@usr_time_end", sUsrTimeEnd);
            }
            if (!string.IsNullOrEmpty(sUsrType))
            {
                SubWhere += " and usr_type in @usr_type_list";
                sqlParams.Add("@usr_type_list", sUsrTypeList);
            }
            if (!string.IsNullOrEmpty(sCMEMO))
            {
                SubWhere += " and cmemo like @cmemo";
                sqlParams.Add("@cmemo", "%" + sCMEMO + "%");
            }

            string sSql = " Select * " +
                          " from BDP20_0000 as S " +
                          " where usr_time in ( " +
                          "         select top 1000 usr_time from BDP20_0000 " +
                          "         where S.usr_date = usr_date " +
                          SubWhere +
                          "         group by usr_date,usr_time " +
                          "         order by usr_date desc, usr_time desc " +
                          "     ) " +
                          SubWhere +
                          "     order by usr_date desc, usr_time desc";

            comm.Ins_BDP20_0000(User.Identity.Name, "BDP200A", "Select", sSql);

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                returnList = con_db.Query<BDP20_0000>(sSql, sqlParams).ToList();
            }

            return Json(returnList, JsonRequestBehavior.AllowGet);

        }


    }
}