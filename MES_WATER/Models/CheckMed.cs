using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MES_WATER.Models
{
    public class CheckMed
    {
        Comm comm = new Comm();
        /// <summary>
        /// 檢查登入紀錄
        /// </summary>
        /// <param name="pDate"></param>
        public void Chk_Med01(string pDate)
        {
            //取得當日登入紀錄
            string sSql = "SELECT * FROM MED01_0000 WHERE ins_date=@ins_date AND is_end='N'";
            DataTable dtTmp = comm.Get_DataTable(sSql, "ins_date", pDate);
            for (int i = 0; i < dtTmp.Rows.Count; i++)
            {
                string med01_0000 = dtTmp.Rows[i]["med01_0000"].ToString();
                string mac_code = dtTmp.Rows[i]["mac_code"].ToString();
                string usr_code = dtTmp.Rows[i]["usr_code"].ToString();
                string login_status = dtTmp.Rows[i]["login_status"].ToString();
                string ins_time = dtTmp.Rows[i]["ins_time"].ToString();
                string des_memo = dtTmp.Rows[i]["des_memo"].ToString();

                DataTable dtRec;
                string sQuery = usr_code + "," + pDate + "," + ins_time;
                //取得上一筆資料
                sSql = "SELECT TOP 1 * FROM MED01_0000" +
                       " WHERE usr_code=@usr_code" +
                       " AND ins_date=@ins_date" +
                       " AND ins_time<@ins_time" +
                       " AND is_end='N'" +
                       " ORDER BY ins_time DESC";
                dtRec = comm.Get_DataTable(sSql, "usr_code,ins_date,ins_time", sQuery);
                if (login_status == "I")
                {
                    bool bReLogin = false;
                    //有資料則檢查是否重複上工
                    if (dtRec.Rows.Count > 0)
                    {
                        string rec_status = dtRec.Rows[0]["login_status"].ToString();
                        if (rec_status == "I")
                        {
                            bReLogin = true;
                        }
                    }
                    //寫入重複上工錯誤
                    des_memo = Set_DesMemo(des_memo, "重複上工。", bReLogin);
                }
                else if (login_status == "O")
                {
                    bool bNoLogin = false;
                    bool bReLogout = false;
                    //有資料則檢查是否重複下工
                    if (dtRec.Rows.Count > 0)
                    {
                        string rec_status = dtRec.Rows[0]["login_status"].ToString();
                        if (rec_status == "O")
                        {
                            bReLogout = true;
                        }
                    } else
                    {
                        bNoLogin = true;
                    }
                    //寫入重複上工錯誤
                    des_memo = Set_DesMemo(des_memo, "重複下工。", bReLogout);
                    //寫入未上工錯誤
                    des_memo = Set_DesMemo(des_memo, "未上工。", bNoLogin);
                }
                //取得下一筆資料
                sSql = "SELECT TOP 1 * FROM MED01_0000" +
                       " WHERE usr_code=@usr_code" +
                       " AND ins_date=@ins_date" +
                       " AND ins_time>@ins_time" + 
                       " AND is_end='N'" +
                       " ORDER BY ins_time"; 
                dtRec = comm.Get_DataTable(sSql, "usr_code,ins_date,ins_time", sQuery);
                if (login_status == "I")
                {
                    bool bNoLogout = false;
                    //有資料則檢查是否未下工
                    if (dtRec.Rows.Count > 0)
                    {
                        string rec_status = dtRec.Rows[0]["login_status"].ToString();
                        if (rec_status == "I")
                        {
                            bNoLogout = true;
                        }
                    }
                    else
                    {
                        bNoLogout = true;
                    }
                    //寫入未下工錯誤
                    des_memo = Set_DesMemo(des_memo, "未下工。", bNoLogout);
                }

                string is_ng = "Y";
                if (des_memo == "") { is_ng = "N"; }
                sSql = "UPDATE MED01_0000 SET des_memo=@des_memo,is_ng=@is_ng WHERE med01_0000=@med01_0000";
                object data = new object();
                data = new
                {
                    des_memo = des_memo,
                    is_ng = is_ng,
                    med01_0000 = med01_0000
                };
                using (SqlConnection con_db = comm.Set_DBConnection())
                {
                    con_db.Execute(sSql, data);
                }
            }
        }

        private string Set_DesMemo(string sDes, string sMsg, bool bChk)
        {
            if (bChk)
            {
                if (!sDes.Contains(sMsg))
                {
                    sDes += sMsg;
                }
            }
            else
            {
                sDes = sDes.Replace(sMsg, "");
            }
            return sDes;
        }
    }
}