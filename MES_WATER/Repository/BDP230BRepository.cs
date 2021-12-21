using Dapper;
using MES_WATER.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MES_WATER.Repository
{
    public class BDP230BRepository
    {

        Comm comm = new Comm();

        public List<BDP23_0000> Get_BoardData(string pUsrCode, string pSorDate, string pShowOKData = "N")
        {
            List<BDP23_0000> list = new List<BDP23_0000>();

            DynamicParameters sqlParams = new DynamicParameters();

            string SubWhere = "";

            // show okData
            if (pShowOKData == "Y")
            {
                SubWhere += "";
            }
            else
            {
                SubWhere += " and  (BDP23_0100.is_ok != 'Y')";
            }

            //

            string sSql = " Select BDP23_0000.*, BDP08_0000.usr_name, BDP23_0100.is_ok, BDP23_0100.ok_date, BDP23_0100.usr_memo, BDP23_0100.bdp23_0100 " +
                          " From BDP23_0000 " +
                          " left join BDP23_0100 on BDP23_0100.bdp23_0000 = BDP23_0000.bdp23_0000 " +
                          " left join BDP08_0000 on BDP08_0000.usr_code = BDP23_0000.usr_code " +
                          //" left join BDB08_0000 on BDB08_0000.usr_code = BDP23_0000.usr_code " +
                          " Where 1 = 1 " +
                          // 使用者可看的 + 所有人都可看的
                          " and ( (BDP23_0000.bull_type = '2' and BDP23_0100.usr_code = @usr_code " + SubWhere + ") or BDP23_0000.bull_type = '1' ) " +
                          // 日期篩選
                          " and (BDP23_0000.bull_date <= @SorDate and BDP23_0000.eff_date >= @SorDate ) " +
                          // 排序
                          " order by BDP23_0000.bdp23_0000";
            // sql參數給值
            sqlParams.Add("@SorDate", pSorDate);
            sqlParams.Add("@usr_code", pUsrCode);

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                list = con_db.Query<BDP23_0000>(sSql, sqlParams).ToList();
            }

            return list;
        }

        public void UpdateData(string pTkCode, string pIsOk, string pOkDate, string pUsrMemo)
        {
            string sSql = " UPDATE BDP23_0100 " +
                          "    SET is_ok  = @is_ok " +
                          "       ,ok_date = @ok_date   " +
                          "       ,usr_memo = @usr_memo   " +
                          "  WHERE bdp23_0100 = @bdp23_0100 ";
            DynamicParameters sqlParams = new DynamicParameters();
            sqlParams.Add("@is_ok", pIsOk);
            sqlParams.Add("@ok_date", pOkDate);
            sqlParams.Add("@usr_memo", pUsrMemo);
            sqlParams.Add("@bdp23_0100", pTkCode);
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, sqlParams);
            }
        }

    }
}