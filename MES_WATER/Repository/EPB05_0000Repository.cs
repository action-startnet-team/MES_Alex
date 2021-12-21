using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

using MES_WATER.Models;
using Dapper;


namespace MES_WATER.Repository
{
    public class EPB05_0000Repository
    {
        Comm comm = new Comm();
        Review RV = new Review();
        ReportReview RpRv = new ReportReview();
        CheckData CD = new CheckData();

        /// <summary>
        /// 取得EPB05_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO EPB05_0000</returns>
        public EPB05_0000 GetDTO(string pTkCode)
        {
            EPB05_0000 datas = new EPB05_0000();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EPB05_0000";
            }
            else
            {
                sSql = "SELECT * FROM EPB05_0000 where epb_type_code=@epb_type_code";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@epb_type_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new EPB05_0000
                        {

                        };
                    }
                }
            }
            return datas;
        }

        #region
        /// <summary>
        /// 取得EPB05_0000資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > List EPB05_0000</returns>
        public List<EPB05_0000> Get_DataList(string pTkCode)
        {
            List<EPB05_0000> list = new List<EPB05_0000>();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM EPB05_0000";
            }
            else
            {
                sSql = "SELECT * FROM EPB05_0000 where epb_type_code=@epb_type_code";
            }
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@epb_type_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EPB05_0000 data = new EPB05_0000();

                    



                    data.can_delete = "Y";
                    data.can_update = "Y";
                    list.Add(data);
                }

            }
            return list;
        }

        /// <summary>
        /// 取得使用者可以編輯的資料，結合商務邏輯權限
        /// </summary>
        /// <param name="pUsrCode"></param>
        /// <param name="pPrgCode"></param>
        /// <returns></returns>
        public List<EPB05_0000> Get_DataList(string pUsrCode, string pPrgCode)
        {
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
            string sLockGrpCode = comm.Get_QueryData("BDP00_0000", "lock_epb_type_code", "par_name", "par_value");
            var arr_LockGrpCode = sLockGrpCode.Split(',');

            List<EPB05_0000> list = new List<EPB05_0000>();
            string sSql = "";

            //取得該使用者可以看的資料
            //sSql = "SELECT * FROM EPB05_0000";
            sSql = "SELECT * FROM EPB05_0000";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                //sqlCommand.Parameters.Add(new SqlParameter("@epb_type_code", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    EPB05_0000 data = new EPB05_0000();

                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改
                    //if (arr_LockGrpCode.Contains(data.epb_type_code)) {
                    //    data.can_delete = "N";
                    //    data.can_update = "N";
                    //}

                    list.Add(data);
                }
            }
            return list;
        }
        #endregion

        /// <summary>
        /// 取得查詢資料，結合使用者權限
        /// </summary>
        /// <param name="pUsrCode">使用者代碼</param>
        /// <param name="pPrgCode">功能代碼</param>
        /// <param name="pWhere">JSON查詢字串</param>
        /// <returns></returns>
        public List<EPB05_0000> Get_DataListByQuery(string pUsrCode, string pPrgCode, string pWhere)
        {
            List<EPB05_0000> list = new List<EPB05_0000>();

            string sSql = " SELECT * FROM EPB05_0000";

            // 取得資料
            list = comm.Get_ListByQuery<EPB05_0000>(sSql, pWhere, pUsrCode, pPrgCode);

            // 權限設定
            string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);            
            for (int i = 0; i < list.Count; i++)
            {

                list[i].review_progress = Get_ReviewProgress(list[i].report_group_code);
                list[i].review = Chk_UsrCanReview(list[i].report_group_code, pUsrCode);
                list[i].epb_name = comm.Get_QueryData("EPB02_0000", list[i].epb_code, "epb_code", "epb_name");
                //list[i].usr_code = list[i].usr_code + " - " + comm.Get_QueryData("BDP08_0000", list[i].usr_code, "usr_code", "usr_name");
                //檢查授權刪除、修改
                list[i].can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                list[i].can_update = sLimitStr.Contains("M") ? "Y" : "N";           
            }
            return list;
        }


        public string Chk_UsrCanReview(string sEpbKey,string pUsrCode) {
            string sValue = "";
            if (RpRv.Chk_UsrCanReview(sEpbKey, pUsrCode))
            {
                sValue = "<a class='btn btn-white btn-info btn-round' id='Review' href='/EPB050A/Report?K=" + sEpbKey + "'><i class='fa fa-pencil-square-o bigger-150 blue'></i>審核</a>";
            }
            else
            {
                sValue = "<a class='btn btn-white btn-success btn-round' id='Review_readonly' href='/EPB050A/Report?K=" + sEpbKey + "'><i class='fa fa-eye bigger-150 green'></i>查看</a>";
            }
            return sValue;
        }


        public string Get_ReviewProgress(string sEpbKey) {
            string sReviewCode = RpRv.Get_ReviewCode(sEpbKey);
            string sValue = "";

            //審核人員數 + 打單人員
            int iReviewLength = 1;
            if (RpRv.Get_ReviewUser(sReviewCode) != "") {
                iReviewLength = RpRv.Get_ReviewUser(sReviewCode).Split(',').Length + 1;
            }

            //電子表單資料的最後一個審核人員的層級(待審核人員)
            string sFinalUsr = RpRv.Get_FinalReviewData(sEpbKey, "usr_code"); 
            //審核層級0的是打單人員          
            decimal iFinalUsrLevel = 0;
            if (sFinalUsr != "") {
                iFinalUsrLevel = decimal.Parse(RpRv.Get_UsrReviewLevel(sReviewCode, sFinalUsr));
            }                
            //結案時 最後一筆還是同一個審核人員，但是審核進度要加1                  
            if (RpRv.Get_FinalReviewData(sEpbKey, "is_ok") == "Y") { iFinalUsrLevel ++; }

            decimal Rate = iFinalUsrLevel / iReviewLength * 100;

            string sActive = "active";
            string sClass = "warning";
            if (RpRv.Get_FinalReviewData(sEpbKey,"result_code") == "99") {
                sClass = "success";
                sActive = "";
            }
            if (RpRv.Get_FinalReviewData(sEpbKey, "result_code") == "98") {
                sClass = "pink";
                sActive = "";
            }

            sValue = "<div class='progress progress-striped "+ sActive + "'><div class='progress-bar progress-bar-"+ sClass + "' style='width:"+ Rate.ToString("G29") + "%;'><label style ='color:black' >"+ iFinalUsrLevel.ToString() + " / " + iReviewLength.ToString() + "</label ></div ></div >";
            //sValue = sFinalUsrLevel + " / " + iReviewLength.ToString();

            return sValue;
        }



        /// <summary>
        /// 傳入一個EPB05_0000的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="EPB05_0000">DTO</param>
        public void InsertData(EPB05_0000 EPB05_0000)
        {            
            string sSql = "INSERT INTO " +
                          " EPB05_0000 (   epb_type_code,  epb_type_name,  cmemo )" +
                          "     VALUES (  @epb_type_code, @epb_type_name, @cmemo )";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, EPB05_0000);                
            }
        }



    }
}