using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MES_WATER.Models
{
    public class Review
    {
        Comm comm = new Comm();
        DynamicTable DT = new DynamicTable();
        GetData GD = new GetData();



        /// <summary>
        /// 取得電子表單資料的表單代號
        /// </summary>
        /// <param name="K"></param>
        /// <returns></returns>
        public string Get_EpbCode(string K) {
            return comm.Get_QueryData("EPB03_0000", K, "field_value", "epb_code");
        }

        /// <summary>
        /// 取的電子表單資料的審核代號
        /// </summary>
        /// <param name="K"></param>
        /// <returns></returns>
        public string Get_ReviewCode(string K)
        {
            string sEpbCode = comm.Get_QueryData("EPB03_0000", K, "field_value", "epb_code");
            string sReviewCode = comm.Get_QueryData("EPB04_0000", sEpbCode, "epb_code", "review_code");
            return sReviewCode;
        }


        /// <summary>
        /// 取得決行結案的電子表單資料
        /// </summary>
        /// <param name="pEpbCode"></param>
        /// <returns></returns>
        public string Get_ReviewPassData(string pEpbCode) {
            string sSql = "select epb_key from EPB05_0000 " +
                          " where result_code = '99' " +
                          "   and epb_code = @epb_code";
            var dtTmp = comm.Get_DataTable(sSql, "epb_code", pEpbCode);
            return GD.DataFieldToStr(dtTmp,"epb_key");
        }



        /// <summary>
        /// 審核當前資料
        /// </summary>
        /// <param name="K">電子表單資料</param>
        /// <param name="Result">審核結果</param>
        /// <param name="Review_memo">審核意見</param>
        /// <param name="pUsrCode">人員</param>
        public void Get_Review(string K, string pResultCode, string Review_memo,string pUsrCode)
        {
            //下一位待審人員(若目前人員可決行 下一位為空值)
            string sNextUsr = Get_NextUser(K, pUsrCode, pResultCode);
            string sResult = pResultCode;
                              
            string sSql = "update EPB05_0000 " +
                          "  set result_code = @result_code" +
                          "     ,is_ok       = @is_ok" +
                          "     ,out_date    = @out_date " +
                          "     ,out_time    = @out_time " +
                          "     ,next_usr_code = @next_usr_code " +
                          "     ,review_memo = @review_memo" +
                          " where epb_key = @epb_key" +
                          "   and is_ok = 'P'";
            object data = new object();
            data = new
            {
                result_code = sResult,
                is_ok = "Y",
                out_date = DateTime.Now.ToString("yyyy/MM/dd"),
                out_time = DateTime.Now.ToString("HH:mm:ss"),
                next_usr_code = sNextUsr,
                review_memo = comm.sGetString(Review_memo),
                epb_key = K,
            };
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, data);
            }

            //做出決定後 把待辦事項改成Y
            TodoList_Ok(K, pUsrCode);                   
        }

        /// <summary>
        /// 清除待辦事項
        /// </summary>
        public void TodoList_Ok(string K,string pUsrCode) {
            string  sSql = "update BDP16_0000 " +
                           "  set is_ok = 'Y'" +
                           " where todo_key = @todo_key" +
                           "   and usr_code = @usr_code";
            object data = new object();
            data = new
            {
                todo_key = K,
                usr_code = pUsrCode,
            };
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, data);
            }
        }


        /// <summary>
        /// 新增下一位待審人員
        /// </summary>
        /// <param name="Key">電子資料生成碼</param>
        public void Ins_Review(string Key,string pUsrCode)
        {
            object data = new object();
            string sEpbCode = comm.Get_QueryData("EPB03_0000", Key, "field_value", "epb_code");
            string sReviewCode = comm.Get_QueryData("EPB04_0000", sEpbCode, "epb_code", "review_code");

            //檢查此表單是否有審核設定作業
            if (Chk_CanReview(sEpbCode))
            {
                data = new
                {
                    review_code = sReviewCode,
                    epb_code = sEpbCode,
                    epb_key = Key,
                    result_code = "",
                    is_ok = "P",
                    scr_no = Get_ReviewScrNo(Key),
                    ins_date = DateTime.Now.ToString("yyyy/MM/dd"),
                    ins_time = DateTime.Now.ToString("HH:mm:ss"),
                    usr_code = pUsrCode,
                    out_date = "",
                    out_time = "",
                    next_usr_code = "",
                    review_memo = "",
                };
                DT.InsertData("EPB05_0000", data);

                data = new
                {
                    todo_code = comm.Get_TkCode("ToDoList"),
                    todo_name = "您有新的待審核電子資料，審核代號:" + sReviewCode,
                    todo_url = "/EPB050A/Report?K=",
                    todo_key = Key,
                    is_use = "Y",
                    is_ok = "N",
                    usr_code = pUsrCode,
                };
                DT.InsertData("BDP16_0000", data);
            }
        }


        /// <summary>
        /// 取得該電子表單資料的最後一筆審核資料的指定欄位
        /// </summary>
        /// <param name="K">電子表單生成碼</param>
        /// <param name="pFieldCode">指定欄位</param>
        /// <returns></returns>
        public string Get_FinalReviewData(string K,string pFieldCode) {
            string sValue = "";
            string sSql = "select top 1 * from EPB05_0000 " +
                          " where epb_key = '" + K + "'" +
                          "   order by scr_no desc";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                sValue = dtTmp.Rows[0][pFieldCode].ToString();
            }
            return sValue;
        }



        /// <summary>
        /// 檢查此電子表單資料的指定人員是否有審核通過
        /// </summary>
        /// <param name="K">電子資料生成碼</param>
        /// <param name="pUsrCode">人員</param>
        /// <returns></returns>
        public bool Chk_IsReview(string K,string pUsrCode) {
            bool sValue = false;
                 
            string sFinalUsr = Get_FinalReviewData(K, "usr_code");
            int iFinalUsrLevel = int.Parse(Get_UsrReviewLevel(Get_ReviewCode(K), sFinalUsr));
            int iUsrLevel = int.Parse(Get_UsrReviewLevel(Get_ReviewCode(K), pUsrCode));

            //如果目前人員的層級小於最後一筆審核資料的人員層級，即為有審核過
            if (iUsrLevel < iFinalUsrLevel) {
                sValue = true;
            }

            //若沒有尚未處理的審核資料 即為全數通過
            //if (sFinalUsr == "") { sValue = true; }
            //if(Chk_ReviewIsEnd(K)) { sValue = true; }
            if (Get_UsrFinalReviewData(K, pUsrCode, "result_code") == "99" || Get_UsrFinalReviewData(K, pUsrCode, "result_code") == "98") { sValue = true; }
            return sValue;
        }

        /// <summary>
        /// 取得審核進度圖的顏色
        /// </summary>
        /// <param name="K"></param>
        /// <param name="pUsrCode"></param>
        /// <returns></returns>
        public string Get_ReviewClass(string K, string pUsrCode) {
            string sLabelClass = "";
            if (Chk_IsReview(K, pUsrCode))
            {
                sLabelClass = "info";
            }
            else
            {
                sLabelClass = "light";
            }
            if (Get_UsrFinalReviewData(K, pUsrCode, "result_code") == "99") { sLabelClass = "success"; }
            if (Get_UsrFinalReviewData(K, pUsrCode, "result_code") == "98") { sLabelClass = "pink"; }
            return sLabelClass;
        }


        /// <summary>
        /// 取得通過審核的人員的時間
        /// </summary>
        /// <param name="K"></param>
        /// <param name="pUsrCode"></param>
        /// <returns></returns>
        public string Get_IsReviewDatetime(string K,string pUsrCode) {
            string sValue = "";

            if (Chk_IsReview(K, pUsrCode)) {
                sValue = Get_UsrFinalReviewData(K, pUsrCode, "out_date") + " " + Get_UsrFinalReviewData(K, pUsrCode, "out_time");
                //sValue = "<a class='white'><i class='ace-icon glyphicon glyphicon-time'></i>"+ sValue + "</a>";
            }
            return sValue;
        }


        /// <summary>
        /// 取得指定人員在此電子資料中 最後一次處理
        /// </summary>
        /// <param name="K"></param>
        /// <param name="pUsrCode"></param>
        /// <returns></returns>
        public string Get_UsrFinalReviewData(string K, string pUsrCode,string sFieldCode)
        {
            string sValue = "";
            string sSql = "select top 1 * from EPB05_0000 " +
                              " where epb_key = '" + K + "'" +
                              "   and usr_code = '" + pUsrCode + "'" +
                              "  order by scr_no desc";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0) {
                sValue = dtTmp.Rows[0][sFieldCode].ToString();
            }
            return sValue;
        }



        /// <summary>
        /// 檢查電子資料是否已經結案
        /// </summary>
        /// <param name="K"></param>
        /// <returns></returns>
        public bool Chk_ReviewIsEnd(string K) {
            bool sValue = false;
            string sSql = "select * from EPB05_0000 " +
                          " where epb_key = '" + K + "'" +
                          "   and result_code in ('99','98')";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0) {
                sValue = true;
            }
            return sValue;
        }



        /// <summary>
        /// 取得審核作業裡面 該資料的下一個順序
        /// </summary>
        /// <param name="Key">電子資料生成碼</param>
        /// <returns></returns>
        public int Get_ReviewScrNo(string Key) {
            int sValue = 1;
            string sSql = "select * from EPB05_0000" +
                          " where epb_key = '" + Key + "'" +
                          "  order by scr_no desc ";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0) {
                sValue = int.Parse(dtTmp.Rows[0]["scr_no"].ToString()) + 1;
            }
            return sValue;
        }

        /// <summary>
        /// 檢查是否可以退回審核(打單人員無法退回)
        /// </summary>
        /// <returns></returns>
        public bool Chk_CanReturn(string pReviewCode, string pUsrCode)
        {
            bool sValue = false;
            string sSql = "select * from EPB04_0100" +
                       " where review_code = '" + pReviewCode + "'" +
                       "   and usr_code = '" + pUsrCode + "'";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                sValue = true;
            }
            return sValue;
        }



        /// <summary>
        /// 檢查是否已經設定審核作業
        /// </summary>
        /// <param name="pEpbCode"></param>
        /// <returns></returns>
        public bool Chk_CanReview(string pEpbCode)
        {
            bool sValue = false;
            string sSql = "select * from EPB04_0000" +
                          " where epb_code = '" + pEpbCode + "'" +
                          "   and is_use = 'Y'";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                sValue = true;
            }
            return sValue;
        }


        /// <summary>
        /// 檢查該使用者是否可以審核此電子資料
        /// </summary>
        /// <param name="K"></param>
        /// <param name="pUsrCode"></param>
        /// <returns></returns>
        public bool Chk_UsrCanReview(string K, string pUsrCode)
        {
            bool sValue = false;
            if (Get_UsrFinalReviewData(K, pUsrCode, "is_ok") == "P") { sValue = true; }
            return sValue;
        }

        /// <summary>
        /// 檢查該使用者是否為該電子表單的審核人員
        /// </summary>
        /// <param name="K"></param>
        /// <param name="pUsrCode"></param>
        /// <returns></returns>
        public bool Chk_IsReviewUsrOfEpb(string pEpbCode, string pUsrCode)
        {
            bool sValue = false;
            string sSql = "select * from EPB04_0000 " +
                          "  left join EPB04_0100 on EPB04_0000.review_code = EPB04_0100.review_code" +
                          " where epb_code = '"+ pEpbCode + "'" +
                          "   and usr_code = '"+ pUsrCode + "'";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0) {
                sValue = true;
            }
            return sValue;
        }



        /// <summary>
        /// 取得下一位審查人員(退回時，若沒有則是打單人員)
        /// </summary>
        /// <param name="K">電子資料生成碼</param>
        /// <param name="pUsrCode">目前人員</param>
        /// <param name="pResult">審核結果(01:升階，02:降階)</param>
        /// <returns></returns>
        public string Get_NextUser(string K, string pUsrCode, string pResult)
        {
            string sEpbCode = comm.Get_QueryData("EPB03_0000", K, "field_value", "epb_code");
            string sReviewCode = comm.Get_QueryData("EPB04_0000", sEpbCode, "epb_code", "review_code");

            string sValue = "";
            int NextLevel = 1;
            //先依照審核代號跟使用者找出審核層級
            //沒有找到的話即為打單人員
            string sSql = "select * from EPB04_0100" +
                          " where review_code = '" + sReviewCode + "'" +
                          "   and usr_code = '" + pUsrCode + "'";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                //審核結果決定+1或-1
                switch (pResult)
                {
                    case "01":
                        NextLevel = int.Parse(GD.DataFieldToStr(sSql, "review_level")) + 1;
                        break;
                    case "02":
                        NextLevel = int.Parse(GD.DataFieldToStr(sSql, "review_level")) - 1;
                        break;
                    default:
                        NextLevel = 6;
                        break;
                }
                if (NextLevel != 0)
                {
                    sSql = "select * from EPB04_0100" +
                           " where review_code = '" + sReviewCode + "'" +
                           "   and review_level = '" + NextLevel.ToString() + "'";
                }
                else
                {
                    sSql = "select * from EPB05_0000" +
                           " where epb_key = '" + K + "'" +
                           "   and scr_no = '1'";
                }
            }
            else
            {
                //打單人員的審核通過即為給審核層級1的人
                sSql = "select * from EPB04_0100" +
                       " where review_code = '" + sReviewCode + "'" +
                       "   and review_level = '1'";
            }
            sValue = GD.DataFieldToStr(sSql, "usr_code");          
            return sValue;
        }

        /// <summary>
        /// 檢查該人員是否可以決行
        /// </summary>
        /// <param name="pReviewCode"></param>
        /// <param name="pUsrCode"></param>
        /// <returns></returns>
        public bool Chk_IsManager(string pReviewCode,string pUsrCode) {
            bool sValue = false;
            string sSql = "select * from EPB04_0100 " +
                          " where review_code = '" + pReviewCode + "'" +
                          "   and usr_code = '" + pUsrCode + "'";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0) 
                if (dtTmp.Rows[0]["is_manager"].ToString() == "Y") {
                    sValue = true;
                }           
            return sValue;
        }


        /// <summary>
        /// 檢查指定人員是否是審核設定作業的最後一個層級
        /// </summary>
        /// <param name="pReviewCode"></param>
        /// <param name="pUsrCode"></param>
        /// <returns></returns>
        public bool Get_UsrIsFinalLevel(string pReviewCode, string pUsrCode) {
            bool sValue = false;
            string sSql = "select top 1 * from EPB04_0100" +
                          " where review_code = '" + pReviewCode + "'" +
                          "   and usr_code <> ''" +
                          "  order by review_level desc";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
                if (pUsrCode == dtTmp.Rows[0]["usr_code"].ToString()) { sValue = true; }
            return sValue;
        }


        /// <summary>
        /// 取得該人員在審核設定作業的層級
        /// </summary>
        /// <param name="pReviewCode">審核設定作業</param>
        /// <param name="pUsrCode">人員</param>
        /// <returns></returns>
        public string Get_UsrReviewLevel(string pReviewCode,string pUsrCode) {
            string sValue = "0";
            string sSql = "select * from EPB04_0100" +
                          " where review_code = '" + pReviewCode + "'" +
                          "   and usr_code = '" + pUsrCode + "'";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0) {
                sValue = dtTmp.Rows[0]["review_level"].ToString();
            }
            return sValue;
        }



        /// <summary>
        /// 檢查該資料是否可以刪除，只有審核作業是打單人員未處理的情況下才能刪除(若已經過審核則必須退回打單人員才能刪除)
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public bool Chk_CanDelete(string Key)
        {
            bool sValue = false;
            string sUsrCode = "";
            //取得該資料的打單人員(scr_no = 1)
            string sSql = "select * from EPB05_0000" +
                          " where epb_key = '" + Key + "'" +
                          "   and scr_no = '1'";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                sUsrCode = dtTmp.Rows[0]["usr_code"].ToString();

                //最後順序要是打單人的的is_ok = P才能刪除
                sSql = "select * from EPB05_0000" +
                       " where epb_key = '" + Key + "'" +
                       "   and usr_code = '" + sUsrCode + "'" +
                       "   and is_ok = 'P'";
                dtTmp = comm.Get_DataTable(sSql);
                if (dtTmp.Rows.Count > 0)
                {
                    sValue = true;
                }
            }
            else
            {
                sValue = true;
            }
            //如果該資料決行結案  則可以刪除
            if (Get_FinalReviewData(Key,"result_code") == "98") { sValue = true; }
             
            return sValue;
        }



        /// <summary>
        /// 取得該審核代號設定的審核人員
        /// </summary>
        /// <param name="pReviewCode"></param>
        /// <returns></returns>
        public string Get_ReviewUser(string pReviewCode) {
            string sValue = "";
            string sSql = "select * from EPB04_0100" +
                          " where review_code = '" + pReviewCode + "'" +
                          "   and usr_code <> ''" +
                          "  order by review_level";
            sValue = GD.DataFieldToStr(sSql, "usr_code");
            return sValue;
        }


        /// <summary>
        /// 檢查使用者在該表單裡面是否有指定權限代號
        /// </summary>
        /// <param name="pUsrCode">使用者</param>
        /// <param name="pEpbCode">表單代號</param>
        /// <param name="pLimitCode">權限代號</param>
        /// <returns></returns>
        public bool Chk_UsrEpbLimit(string pUsrCode,string pEpbCode,string pLimitCode) {
            bool sValue = false;
            string sSql = "select * from BDP09_0200 " +
                          " where usr_code = '" + pUsrCode + "'" +
                          "   and epb_code = '" + pEpbCode + "'";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0) {
                string sLimitStr = dtTmp.Rows[0]["limit_str"].ToString();
                if (sLimitStr.Contains(pLimitCode)) { sValue = true; }
            }
            return sValue;
        }

        /// <summary>
        /// 檢查使用者是否還有待處理的審核作業
        /// </summary>
        /// <returns></returns>
        public bool Chk_UsrHaveReview(string pUsrCode) {
            bool sValue = false;
            string sSql = "select * from EPB05_0000 " +
                          " where is_ok = 'P'" +
                          "   and usr_code = '"+ pUsrCode + "'";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0) {
                sValue = true;
            }
            return sValue;
        }




    }
}