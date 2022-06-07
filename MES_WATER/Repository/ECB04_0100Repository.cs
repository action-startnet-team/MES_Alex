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
    public class ECB04_0100Repository
    {
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();

        /// <summary>
        /// 取得ECB04_0100資料表內容
        /// </summary>
        /// <param name = "pTkCode" > 鍵值 / 傳空值取全部資料 </ param >
        /// < returns > DTO ECB04_0100</returns>
        public ECB04_0100 GetDTO(string pTkCode)
        {
            ECB04_0100 datas = new ECB04_0100();
            string sSql = "";

            if (string.IsNullOrEmpty(pTkCode))
            {
                sSql = "SELECT * FROM ECB04_0100 ";
            }
            else
            {
                sSql = "SELECT * FROM ECB04_0100 where ecb04_0100 = @ecb04_0100 ";
            }

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter("@ecb04_0100", pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        datas = new ECB04_0100
                        {
                            ecb04_0100 = comm.sGetString(reader["ecb04_0100"].ToString()),
                            SALES_CUSTOMER_CODE_EDITION = comm.sGetString(reader["SALES_CUSTOMER_CODE_EDITION"].ToString()),
                            SERIAL_NUM = comm.sGetString(reader["SERIAL_NUM"].ToString()),
                            ERP_FIELD_CODE = comm.sGetString(reader["ERP_FIELD_CODE"].ToString()),
                            ERP_FIELD_VALUE = comm.sGetString(reader["ERP_FIELD_VALUE"].ToString()),
                            EXCEL_CODE = comm.sGetString(reader["EXCEL_CODE"].ToString()),
                        };
                    }
                }
            }
            return datas;
        }

        /// <summary>
        /// 取得查詢資料，結合使用者權限
        /// </summary>
        /// <param name="pUsrCode">使用者代碼</param>
        /// <param name="pPrgCode">功能代碼</param>
        /// <param name="pWhere">JSON查詢字串</param>
        /// <returns></returns>
        public List<ECB04_0100> Get_DataList(string pUsrCode, string pPrgCode, string pTkCode = "")
        {
            List<ECB04_0100> list = new List<ECB04_0100>();
            string foreignKey = gmv.GetKey<ECB04_0000>(new ECB04_0000());
            string sSql = "";
            if (!string.IsNullOrEmpty(pTkCode))
            {
                sSql = " SELECT ECB04_0100.*,ECB05_0000.ERP_FIELD_NAME, ECB05_0000.ERP_FORM_NAME, ECB05_0000.ERP_FIELD_VALUE from ECB04_0100 " +
                    " left join ECB05_0000 on ECB04_0100.ERP_FIELD_CODE = ECB05_0000.ERP_FIELD_CODE " +
                " where ECB04_0100." + foreignKey + "=@" + foreignKey + " order by SERIAL_NUM";
                //" order by inspect_item_code";
            }
            else
            {
                sSql = "SELECT * FROM ECB04_0100";
            }
            //取得該使用者可以看的資料
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(sSql);
                sqlCommand.Connection = con_db;
                sqlCommand.Parameters.Add(new SqlParameter(foreignKey, pTkCode));
                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    ECB04_0100 data = new ECB04_0100();

                    data.ecb04_0100 = comm.sGetString(reader["ecb04_0100"].ToString());
                    data.SALES_CUSTOMER_CODE_EDITION = comm.sGetString(reader["SALES_CUSTOMER_CODE_EDITION"].ToString());
                    data.SERIAL_NUM = comm.sGetString(reader["SERIAL_NUM"].ToString());
                    data.ERP_FIELD_CODE = comm.sGetString(reader["ERP_FIELD_CODE"].ToString());
                    data.ERP_FIELD_NAME = comm.sGetString(reader["ERP_FIELD_NAME"].ToString());
                    data.ERP_FIELD_VALUE = comm.sGetString(reader["ERP_FIELD_VALUE"].ToString());
                    data.EXCEL_CODE = comm.sGetString(reader["EXCEL_CODE"].ToString());
                    data.ERP_FORM_NAME = comm.sGetString(reader["ERP_FORM_NAME"].ToString());

                    string sLimitStr = comm.Get_LimitByUsrCode(pUsrCode, pPrgCode);
                    //檢查授權刪除、修改
                    data.can_delete = sLimitStr.Contains("D") ? "Y" : "N";
                    data.can_update = sLimitStr.Contains("M") ? "Y" : "N";

                    //資料邏輯刪除、修改

                    list.Add(data);
                }

            }
            return list;
        }


        /// <summary>
        /// 傳入一個ECB04_0100的DTO，存檔，一次存檔一筆
        /// </summary>
        /// <param name="ECB04_0100">DTO</param>
        public void InsertData(ECB04_0100 ECB04_0100)
        {
            string sSql = " INSERT INTO " +
                          " ECB04_0100 (  ecb04_0100,  SALES_CUSTOMER_CODE_EDITION,  SERIAL_NUM,  ERP_FIELD_CODE,  EXCEL_CODE ) " +
                          "     VALUES ( @ecb04_0100, @SALES_CUSTOMER_CODE_EDITION, @SERIAL_NUM, @ERP_FIELD_CODE, @EXCEL_CODE ) ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, ECB04_0100);
            }
        }

        /// <summary>
        /// 傳入一個ECB04_0100的DTO，修改，一次修改一筆
        /// </summary>
        /// <param name="ECB04_0100">DTO</param>
        public void UpdateData(ECB04_0100 ECB04_0100)
        {
            string sSql = " UPDATE ECB04_0100                               " +
                          "    SET SERIAL_NUM =  @SERIAL_NUM, " +
                          "        ERP_FIELD_CODE      =  @ERP_FIELD_CODE,      " +
                          "        EXCEL_CODE          =  @EXCEL_CODE          " +
                          "  WHERE ecb04_0100        =  @ecb04_0100         ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, ECB04_0100);

            }
        }

        /// <summary>
        /// 傳入一個鍵值，刪除、一次刪除一筆
        /// </summary>
        /// <param name="pTkCode">資料鍵值</param>
        public void DeleteData(string pTkCode)
        {
            string sSql = "DELETE FROM ECB04_0100 WHERE ecb04_0100 = @ecb04_0100 ";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { ecb04_0100 = pTkCode });

            }
        }

    }
}