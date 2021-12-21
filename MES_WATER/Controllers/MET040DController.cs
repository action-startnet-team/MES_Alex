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

    public class MET040DController : JsonNetController
    {
        //程式代號
        string sPrgCode = "MET040D";
        //需要用到的Repo
        MET04_0300Repository repoMET04_0300 = new MET04_0300Repository();
        //共用函式庫
        Comm comm = new Comm();
        GetModelValidation gmv = new GetModelValidation();
        
        /* 資料處理 向下 */
        /// <summary>
        /// (固定區) 主檔 首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //要結合權限控制
            ViewBag.limit_str = comm.Get_LimitByUsrCode(User.Identity.Name, sPrgCode);
            ViewBag.prg_code = sPrgCode;
            ViewBag.date = "";
            //取得歷程
            ViewBag.rec_data = Get_RecData(ViewBag.date);
            ViewBag.chk_data = Chk_RecData(ViewBag.date);

            ////取得用料列表
            ////DataTable dtProList = Get_ProList();
            //DataTable dtProList = new DataTable();
            ////建立對應工單列表
            //DataTable dtMoList = new DataTable();
            //DataTable dtTmpMoList = new DataTable();
            ////dtMoList.Columns.Add("wmt06_0100");
            //dtMoList.Columns.Add("mo_code");
            ////dtMoList.Columns.Add("wrk_code");
            ////dtMoList.Columns.Add("work_code");
            //dtMoList.Columns.Add("pro_name");
            //dtMoList.Columns.Add("plan_qty");
            //dtMoList.Columns.Add("up_qty");
            //dtMoList.Columns.Add("pro_unit");
            ////dtMoList.Columns.Add("lot_no");
            //dtMoList.Columns.Add("rate");
            //dtMoList.Columns.Add("use_qty");
            ////取得當日已結案工單
            //dtTmpMoList = Get_EndMoList();
            ////取得當日工單應備料量與實際用料量
            //for(int i = 0; i < dtMoList.Rows.Count; i++)
            //{
            //    DataRow TmpDataRow = dtMoList.Rows[i];
            //    DataRow nrow = dtMoList.NewRow();
            //    //
            //    string mo_code = TmpDataRow["mo_code"].ToString();
            //    string pro_code = TmpDataRow["pro_code"].ToString();
            //    string pro_name = comm.Get_QueryData("MEB20_0000", pro_code, "pro_code", "pro_name");
            //    string plan_qty = TmpDataRow["plan_qty"].ToString();
            //    string pro_unit = TmpDataRow["pro_unit"].ToString();
            //    string sum_qty = "0";//總生產量
            //    //double rate = 0;
            //    //if (double.Parse( sum_qty ) > 0)
            //    //{
            //    //    rate = up_qty / sum_qty;
            //    //}
            //    nrow["mo_code"] = mo_code;
            //    nrow["pro_name"] = pro_name;
            //    nrow["plan_qty"] = plan_qty;
            //    nrow["up_qty"] = sum_qty;
            //    nrow["pro_unit"] = pro_unit;
            //    nrow["rate"] = "";
            //    //取得該工單用料
            //    DataTable dtProCodelist = Get_ProCodeByMoCode(mo_code);
            //    for(int j = 0; j < dtProCodelist.Rows.Count; j++)
            //    {
            //        DataRow drProData = dtProCodelist.Rows[j];
            //        string sProCode = drProData["pro_code"].ToString();
            //        string sPlanQty = drProData["pro_qty"].ToString();
            //        string sWorkCode = drProData["work_code"].ToString();
            //        //取得上料量、退料量
            //        double dUseQty = Get_ProQtyUse(mo_code, sProCode);
            //        double dNoUseQty = Get_ProQtyNotUse(mo_code, sProCode);
            //        //實際用量=上料量-退料量
            //        string sRelUseQty = (dUseQty - dNoUseQty).ToString("#0");

            //     }

            //}


            //for (int i = 0; i < dtProList.Rows.Count; i++)
            //{
            //    DataRow prorow = dtProList.Rows[i];
            //    string wmt06_0100 = prorow["wmt06_0100"].ToString();
            //    string prepare_code = prorow["prepare_code"].ToString();
            //    string pro_code = prorow["pro_code"].ToString();
            //    double pro_qty = Get_UseQty(pro_code); //取得用料數量
            //    prorow["pro_qty"] = pro_qty.ToString("#0");

            //    //取得工單資料
            //    DataTable dtMoData = Get_MoList(prepare_code, pro_code);
            //    //取得總生產量
            //    double sum_qty = Convert.ToDouble(dtMoData.Compute("SUM(up_qty)", ""));

            //    for (int j = 0; j < dtMoData.Rows.Count; j++)
            //    {
            //        DataRow morow = dtMoData.Rows[j];
            //        DataRow nrow = dtMoList.NewRow();
            //        double plan_qty = double.Parse(morow["plan_qty"].ToString());
            //        double up_qty = double.Parse(morow["up_qty"].ToString());
            //        double rate = 0;
            //        if (sum_qty > 0)
            //        {
            //            rate = up_qty / sum_qty;
            //        }
            //        nrow["wmt06_0100"] = wmt06_0100;
            //        nrow["mo_code"] = morow["mo_code"].ToString();
            //        nrow["wrk_code"] = morow["wrk_code"].ToString();
            //        nrow["work_code"] = morow["work_code"].ToString();
            //        nrow["pro_name"] = morow["pro_name"].ToString();
            //        nrow["plan_qty"] = plan_qty.ToString("#0");
            //        nrow["up_qty"] = up_qty;
            //        nrow["pro_unit"] = morow["pro_unit"].ToString();
            //        nrow["lot_no"] = Get_LotNo(wmt06_0100, pro_code);
            //        nrow["rate"] = (rate * 100).ToString("#0.##");
            //        nrow["use_qty"] = (rate * pro_qty).ToString("#0");
            //        dtMoList.Rows.Add(nrow);
            //    }
            //}
            //ViewBag.pro_data = dtProList;
            ViewBag.mo_data = Get_MoTable(ViewBag.date);

            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            //要結合權限控制
            ViewBag.limit_str = comm.Get_LimitByUsrCode(User.Identity.Name, sPrgCode);
            ViewBag.prg_code = sPrgCode;
            
            // 自訂義 資料驗證
            bool bIsOK = Chk_Ins_Main(form);
            string ureport_date = DateTime.Now.ToString("yyyy/MM/dd");

           

            if (form["btn_share"] == "查詢") {
                ViewBag.mo_data = Get_MoTable(form["cal_date"].ToString());
                ViewBag.date = form["cal_date"].ToString();
                ViewBag.Share = "Y";
                ViewBag.rec_data = Get_RecData(ViewBag.date);
                ViewBag.chk_data = Chk_RecData(ViewBag.date);
                return View();
            }



            //取得用料列表
           
            DataTable dtMoData = Get_MoTable(form["cal_date"].ToString());
            //建立對應工單列表
            DataTable dtMoList = new DataTable();
            
            dtMoList.Columns.Add("mo_code");
            dtMoList.Columns.Add("wrk_code");
            dtMoList.Columns.Add("work_code");
            dtMoList.Columns.Add("pro_name");
            dtMoList.Columns.Add("plan_qty");
            dtMoList.Columns.Add("up_qty");
            dtMoList.Columns.Add("pro_unit");
            dtMoList.Columns.Add("lot_no");
            dtMoList.Columns.Add("rate");
            dtMoList.Columns.Add("use_qty");
            foreach(DataRow drMoData in dtMoData.Rows)
            {
                DataTable dtProList = Get_ModTable(drMoData["mo_code"].ToString());
                foreach(DataRow drProList in dtProList.Rows)
                {
                    //抓取數量的id=mo_code+pro_code+lot_no
                    string sKey = drMoData["mo_code"].ToString();
                    sKey += drProList["pro_code"].ToString();
                    sKey += drProList["lot_no"].ToString();
                    string sUseQty = form[sKey];
                    decimal use_qty = decimal.Parse(sUseQty);
                    //寫入MET04_0300
                    MET04_0300 data = new MET04_0300();
                    data.ureport_code = comm.Get_TkCode("MET040D");
                    data.ureport_date = ureport_date;
                    data.mo_code = drMoData["mo_code"].ToString();
                    data.pro_code = drProList["pro_code"].ToString();
                    data.pro_qty = use_qty;
                    data.pro_unit = "";
                    data.lot_no = drProList["lot_no"].ToString();
                    data.usr_code = User.Identity.Name;
                    data.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
                    data.ins_time = DateTime.Now.ToString("HH:mm:ss");
                    data.sap_code = "";
                    data.sap_message = "";
                    data.sap_no = "";
                    data.is_del = "";

                    repoMET04_0300.InsertData(data);
                }
            }
            //for (int i = 0; i < dtProList.Rows.Count; i++)
            //{
            //    DataRow prorow = dtProList.Rows[i];
            //    string wmt06_0100 = prorow["wmt06_0100"].ToString();
            //    string prepare_code = prorow["prepare_code"].ToString();
            //    string pro_code = prorow["pro_code"].ToString();
            //    string pro_unit = prorow["pro_unit"].ToString();
            //    double pro_qty = double.Parse(prorow["pro_qty"].ToString()); //取得用料數量
            //    prorow["pro_qty"] = pro_qty.ToString("#0");

            //    //取得工單資料
            //    DataTable dtMoData = Get_MoList(prepare_code, pro_code);
            //    //取得總生產量
            //    double sum_qty = Convert.ToDouble(dtMoData.Compute("SUM(up_qty)", ""));

            //    for (int j = 0; j < dtMoData.Rows.Count; j++)
            //    {
            //        DataRow morow = dtMoData.Rows[j];
            //        DataRow nrow = dtMoList.NewRow();
            //        double plan_qty = double.Parse(morow["plan_qty"].ToString());
            //        double up_qty = double.Parse(morow["up_qty"].ToString());
            //        double rate = 0;
            //        string use_qty = form[wmt06_0100 + "_" + morow["mo_code"].ToString()];
            //        if (sum_qty > 0)
            //        {
            //            rate = up_qty / sum_qty;
            //        }
            //        string lot_no = Get_LotNo(wmt06_0100, pro_code);

            //        nrow["wmt06_0100"] = wmt06_0100;
            //        nrow["mo_code"] = morow["mo_code"].ToString();
            //        nrow["wrk_code"] = morow["wrk_code"].ToString();
            //        nrow["work_code"] = morow["work_code"].ToString();
            //        nrow["pro_name"] = morow["pro_name"].ToString();
            //        nrow["plan_qty"] = plan_qty.ToString("#0");
            //        nrow["up_qty"] = up_qty;
            //        nrow["pro_unit"] = morow["pro_unit"].ToString();
            //        nrow["lot_no"] = lot_no;
            //        nrow["rate"] = (rate * 100).ToString("#0.##");
            //        nrow["use_qty"] = use_qty;
            //        dtMoList.Rows.Add(nrow);

            //        if (bIsOK)
            //        {
            //            //寫入MET04_0300
            //            MET04_0300 data = new MET04_0300();
            //            data.ureport_code = comm.Get_TkCode("MET040D");
            //            data.ureport_date = ureport_date;
            //            data.mo_code = morow["mo_code"].ToString();
            //            data.pro_code = pro_code;
            //            data.pro_qty = decimal.Parse(use_qty);
            //            data.pro_unit = pro_unit;
            //            data.lot_no = lot_no;
            //            data.usr_code = User.Identity.Name;
            //            data.ins_date = DateTime.Now.ToString("yyyy/MM/dd");
            //            data.ins_time = DateTime.Now.ToString("HH:mm:ss");
            //            data.sap_code = "";
            //            data.sap_message = "";
            //            data.sap_no = "";
            //            data.is_del = "";

            //            repoMET04_0300.InsertData(data);
            //        }
            //    }
            //}
            //取得列表
            List<MET04_0300> list = repoMET04_0300.Get_DataList(ureport_date);
            // 新增DTS01
            comm.Ins_DTS01_0000("ZMES9997", User.Identity.Name, list);

            //ViewBag.pro_data = dtProList;
            //ViewBag.mo_data = dtMoList;
            ViewBag.rec_data = Get_RecData(ViewBag.date);
            ViewBag.chk_data = Chk_RecData(ViewBag.date);

            return View();
        }

        public ActionResult Delete(string pTkCode)
        {
            //刪除前的檢查要在JqGrid送出前檢查，所以對應Chk_Del_Main這個函數
            //取得工單分類資料
            DataTable dtList = repoMET04_0300.Get_MoList(pTkCode);
            // 新增DTS01
            comm.Ins_DTS01_0000("ZMES9993-1", User.Identity.Name, dtList);
            //更新狀態
            foreach(DataRow row in dtList.Rows)
            {
                string mo_code = row["mo_code"].ToString();
                repoMET04_0300.UpdateStatus(mo_code, pTkCode);
            }

            //List<MET04_0300> sBefore = repoMET04_0300.Get_DataList(pTkCode);

            //for (int i = 0; i < sBefore.Count; i++)
            //{
            //    // 新增DTS01
            //    comm.Ins_DTS01_0000("ZMES9993-1", User.Identity.Name, sBefore[i]);

            //    sBefore[i].is_del = "P";
            //    repoMET04_0300.UpdateData(sBefore[i]);
            //}
            return RedirectToAction("Index", sPrgCode);
        }

        private bool Chk_Ins_Main(FormCollection form)
        {
            bool bIsOK = true;

            //** 依作業不同有不同的檢查點


            //檢查結果回傳
            return bIsOK;
        }

        public DataTable Get_RecData(string pDate)
        {
            string sMoList = Get_MoListByDate(pDate);
            string sDate = DateTime.Now.ToString("yyyy/MM/dd");
            string sSql = "SELECT a.*,b.pro_name FROM MET04_0300 a" +
                " LEFT JOIN MEB20_0000 b ON a.pro_code = b.pro_code" +
                " WHERE a.mo_code in(" +sMoList+")"+
                " ORDER BY ins_time DESC";
            return comm.Get_DataTable(sSql);
        }

        public string Chk_RecData(string pDate)
        {
            string sMoList = Get_MoListByDate(pDate);
            string sDate = DateTime.Now.ToString("yyyy/MM/dd");
            string sSql = "SELECT a.*,b.pro_name FROM MET04_0300 a" +
                " LEFT JOIN MEB20_0000 b ON a.pro_code = b.pro_code" +
                " WHERE a.mo_code in(" + sMoList + ")" +" AND is_del<>'Y'";
            DataTable dtTmp = comm.Get_DataTable(sSql, "date", sDate);
            if (dtTmp.Rows.Count > 0)
            {
                return "Y";
            } else
            {
                return "N";
            }
        }
        
        //取得當日備料單列表
        public DataTable Get_ProList()
        {
            string sDate = DateTime.Now.ToString("yyyy/MM/dd");
            string sSql = "SELECT a.*,c.pro_name,b.line_code FROM WMT06_0100 a" +
                " LEFT JOIN WMT06_0000 b ON a.prepare_code = b.prepare_code" +
                " LEFT JOIN MEB20_0000 c ON a.pro_code = c.pro_code" +
                " WHERE b.prepare_date = @date" +
                " AND wmt06_0100 IN (SELECT wmt06_0100 FROM WMT06_0110)" +
                " ORDER BY a.pro_code";
            return comm.Get_DataTable(sSql,"date", sDate);
        }
        

        //依據備料單號及料號取得工單列表
        public DataTable Get_MoList(string pPrepareCode,string pProCode)
        {
            string sSql = "SELECT a.mo_code,a.wrk_code,a.work_code,c.pro_name,b.plan_qty,b.pro_unit" +
                ",(SELECT ISNULL(SUM(pro_qty), 0) FROM MET04_0200 WHERE mo_code = a.mo_code AND is_del <> 'Y') AS up_qty" +
                " FROM WMT07_0000 a" +
                " LEFT JOIN MET01_0000 b ON a.mo_code = b.mo_code" +
                " LEFT JOIN MEB20_0000 c ON b.pro_code = c.pro_code" +
                " WHERE a.prepare_code = @prepare_code AND a.pro_code = @pro_code" +
                " ORDER BY a.scr_no";
            string sValue = pPrepareCode + "," + pProCode;
            return comm.Get_DataTable(sSql, "prepare_code,pro_code", sValue);
        }

        public string Get_LotNo(string pKey,string pProCode)
        {
            string sSql = "SELECT lot_no FROM WMT06_0110 WHERE wmt06_0100=@key AND pro_code=@pro_code";
            string sValue = pKey + "," + pProCode;
            DataTable dtTmp = comm.Get_DataTable(sSql, "key,pro_code", sValue);
            if (dtTmp.Rows.Count > 0)
            {
                return dtTmp.Rows[0]["lot_no"].ToString();
            }
            return "";
        }

        public double Get_UseQty(string pProCode)
        {
            string sDate = DateTime.Now.ToString("yyyy/MM/dd");
            double dUseQty = 0;
            //取得投料數量
            string sSql = "SELECT ISNULL(SUM(a.res_qty),0) AS res_qty" +
                " FROM WMT07_0000 a" +
                " LEFT JOIN WMT06_0000 b ON a.prepare_code = b.prepare_code" +
                " WHERE a.pro_code = @pro_code" +
                " AND b.prepare_date = @date";
            DataTable dtTmp = comm.Get_DataTable(sSql, "pro_code,date", pProCode + "," + sDate);
            if (dtTmp.Rows.Count > 0)
            {
                dUseQty += double.Parse(dtTmp.Rows[0]["res_qty"].ToString());
            }
            //取得退料數量
            sSql = "SELECT ISNULL(SUM(a.pro_qty),0) AS pro_qty" +
                " FROM MED07_0000 a" +
                " WHERE a.pro_code = @pro_code" +
                " AND a.ins_date = @date" +
                " AND a.is_end <> 'Y'";
            dtTmp = comm.Get_DataTable(sSql, "pro_code,date", pProCode + "," + sDate);
            if (dtTmp.Rows.Count > 0)
            {
                dUseQty -= double.Parse(dtTmp.Rows[0]["pro_qty"].ToString());
            }

            if (dUseQty < 0) dUseQty = 0;
            return dUseQty;
        }
        /* 資料處理 向上 */

        /*修改地方向下*/
        /// <summary>
        /// 取得今日已完工工單
        /// </summary>
        /// <returns></returns>
        public DataTable Get_EndMoList()
        {
            string sDate = DateTime.Now.ToString("yyyy/MM/dd");
            string sSql = "SELECT * FROM  MET01_0000" +
                        " where mo_end_date= @date and mo_status='30'";
            return comm.Get_DataTable(sSql, "date", sDate);
        }
        /// <summary>
        /// 取得該工單之用料
        /// </summary>
        /// <param name="pMoCode"></param>
        /// <returns></returns>
        public DataTable Get_ProCodeByMoCode(string pMoCode)
        {
            string sSql = "SELECT * FROM MET01_0100 " +
                          " WHERE mo_code =@mo_code";
            return comm.Get_DataTable(sSql, "@mo_code", pMoCode);
        }
        /// <summary>
        /// 取得總上料數量
        /// </summary>
        /// <param name="pProCode"></param>
        /// <param name="pLotNo"></param>
        /// <param name="pEndDate"></param>
        /// <returns></returns>
        public double Get_ProQtyUse(string pProCode, string pLotNo, string pEndDate)
        {
            string sSql = "select sum(pro_qty) as use_qty from MED06_0000 " +
                        " where pro_code='" + pProCode + "'"+
                        "   and lot_no='"+ pLotNo + "'"+
                        "   and ins_date='"+ pEndDate + "'";
            DataTable dtTmp=comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(dtTmp.Rows[0]["use_qty"].ToString()))
                {
                    return 0;
                }
                double dProQty = double.Parse(dtTmp.Rows[0]["use_qty"].ToString());
                return dProQty;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 取得該結案日的所有退料量
        /// 
        ///  </summary>
        /// <param name="pProCode"></param>
        /// <param name="pLotNo"></param>
        /// <param name="pEndDate"></param>
        /// <returns></returns>
        public double Get_ProQtyNotUse(string pProCode ,string pLotNo,string pEndDate)
        {
            string sSql = "select sum(pro_qty) as use_qty from MED07_0000 " +
                          " where  pro_code='" + pProCode + "'"+
                          "   and  lot_no='"+ pLotNo  + "'"+
                          "   and  ins_date='" + pEndDate + "'";
            DataTable dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(dtTmp.Rows[0]["use_qty"].ToString()))
                {
                    return 0;
                }
                double dProQty = double.Parse(dtTmp.Rows[0]["use_qty"].ToString());
                return dProQty;
            }
            else
            {
                return 0;
            }
        }




        /// <summary>
        /// 取得當前工單，只抓生產中或強制結案的工單
        /// </summary>
        /// <returns></returns>
        public DataTable Get_MoTable(string pDate) {
            string sSql = "";
            string sDate = "";
            if (!string.IsNullOrEmpty(pDate) ) {
                sDate = DateTime.Parse(pDate).ToString("yyyy/MM/dd");
            }
            DataTable dtTmp = new DataTable();
            //檢查工單狀態為以生產或強制結案，且以製費報工的工單
            sSql = "select MET01_0000.*,pro_name,0 as rate " +
                   "       ,(select isnull(sum(pro_qty),'0') as pro_qty from MET04_0200 "+
                   "         where mo_code = MET01_0000.mo_code  and MET04_0200.is_del='') as up_qty " +
                   "  from MET01_0000" +
                   "  left join MEB20_0000 on MET01_0000.pro_code = MEB20_0000.pro_code" +
                   " where (mo_status='30' or mo_status='90')"+
                   "  and  mo_end_date = '" + sDate + "'"+
                   "  and mo_code in (select mo_code from MET04_0200 group by mo_code )";
            dtTmp = comm.Get_DataTable(sSql);
            return dtTmp;
        }
        /// <summary>
        /// 取得當前工單用料量
        /// </summary>
        /// <param name="pMoCode"></param>
        /// <returns></returns>
        public DataTable Get_ModTable(string pMoCode,string pShare="")
        {
            string sSql = "";
            DataTable dtData = new DataTable();
            dtData.Columns.Add("pro_name");
            dtData.Columns.Add("pro_unit");
            dtData.Columns.Add("lot_no");
            dtData.Columns.Add("mo_code");
            dtData.Columns.Add("pro_code");
            dtData.Columns.Add("pro_qty");

            DataTable dtTmp = new DataTable();
            sSql = "select MED06_0000.*,pro_name " +
                   "  from MED06_0000" +
                   "  left join MEB20_0000 on MED06_0000.pro_code = MEB20_0000.pro_code" +
                   " where mo_code = '"+ pMoCode + "'" ;
            dtTmp = comm.Get_DataTable(sSql);
            foreach(DataRow drTmp in dtTmp.Rows)
            {
                DataRow drData = dtData.NewRow();
                string sMoCode= drTmp["mo_code"].ToString();
                string sProCode= drTmp["pro_code"].ToString();
                string sLotNo = drTmp["lot_no"].ToString();
                decimal dProQty = comm.sGetDecimal(drTmp["pro_qty"].ToString()) - Get_OutProQty(sMoCode, sProCode, sLotNo);
                drData["pro_name"] = drTmp["pro_name"].ToString();
                drData["pro_unit"] = drTmp["pro_unit"].ToString();
                drData["lot_no"] = drTmp["lot_no"].ToString();
                drData["mo_code"] = drTmp["mo_code"].ToString();
                drData["pro_code"] = drTmp["pro_code"].ToString();
                drData["pro_qty"] = Get_ShareQty(sMoCode,sProCode,dProQty.ToString("G29"),sLotNo);                
                dtData.Rows.Add(drData);
            }


            return dtData;
        }
        /// <summary>
        ///抓取MED07_0000退料清單
        /// </summary>
        /// <param name="pMoCode"></param>
        /// <param name="pProCode"></param>
        /// <param name="pLotNo"></param>
        /// <returns></returns>
        public decimal Get_OutProQty( string pMoCode,string pProCode ,string pLotNo)
        {
            decimal OutQty = 0;
            string sSql = "";
            sSql = "select sum(pro_qty) as pro_qty from MED07_0000 " +
                  " where mo_code ='" + pMoCode + "'" +
                  "   and pro_code='" + pProCode + "'" +
                  "   and lot_no='" + pLotNo + "'";
            DataTable dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0)
            {
                OutQty = comm.sGetDecimal(dtTmp.Rows[0]["pro_qty"].ToString());
            }
            return OutQty;
        }
    
        /// <summary>
        /// 確認共用料
        /// </summary>
        /// <param name="pProCode"></param>
        /// <returns></returns>
        public string Chk_ProCodeIsShare(string pProCode) {
            string val = "";
            string sSql = "select count(*) as cnt" +
                          "  from MED06_0000" +
                          "  left join MET01_0000 on MET01_0000.mo_code = MED06_0000.mo_code" +
                          " where mo_out_date = '" + DateTime.Now.ToString("yyyy/MM/dd") + "'" +
                          "   and MED06_0000.pro_code = '"+ pProCode + "'";
            var dtTmp = comm.Get_DataTable(sSql);
            if (dtTmp.Rows.Count > 0) {
                int iCnt = comm.sGetInt32(dtTmp.Rows[0]["cnt"].ToString());
                if (iCnt > 1) { val = "共用料"; }               
            }
            return val;
        }
        /// <summary>
        /// 抓取共用料分攤數量
        /// </summary>
        /// <param name="pMoCode"></param>
        /// <param name="pProCode"></param>
        /// <param name="pProQty"></param>
        /// <returns></returns>
        public decimal Get_ShareQty(string pMoCode,string pProCode,string pProQty,string pLotNo)
        {
            decimal dShareQty = 0;
            //抓取生產結束日期
            string sMoEndDate= comm.Get_QueryData("MET01_0000", pMoCode, "mo_code", "mo_end_date");
            if (sMoEndDate == "")
            {
                return comm.sGetDecimal(pProQty);
            }
            //抓取該工單用料比率、物料總使用量、總退料量
            decimal dUseQty =comm.sGetDecimal( Get_ProQtyUse(pProCode, pLotNo, sMoEndDate).ToString("G29"));
            decimal dNoUseQty = comm.sGetDecimal(Get_ProQtyNotUse(pProCode, pLotNo, sMoEndDate).ToString("G29"));
            decimal dRate = Get_MoRate(pMoCode,sMoEndDate,pProCode);
            dShareQty = (dUseQty - dNoUseQty) * dRate;
            return dShareQty;
        }
        /// <summary>
        /// 抓取用料比率
        /// </summary>
        /// <param name="pMoCode"></param>
        /// <param name="pMoEndDate"></param>
        /// <param name="pProCode"></param>
        /// <returns></returns>
        public decimal Get_MoRate(string pMoCode,string pMoEndDate,string pProCode)
        {
            decimal dRate = 1;
            DateTime tMoEndDate = comm.sGetDateTime(pMoEndDate);
            //抓取工單結案當日所有相同用料量與當前工單用量
            string sSql="select sum(pro_qty) as total_qty,"+
                        "   (select MET01_0100.pro_qty from MET01_0000 "+
                        "      left join MET01_0100 on MET01_0000.mo_code = MET01_0100.mo_code "+
                        "    where MET01_0000.mo_code='"+ pMoCode + "' and MET01_0100.pro_code='"+ pProCode + "')   as pro_qty "+
                        " from MET01_0000 " +
                        "  left join MET01_0100 on MET01_0000.mo_code =MET01_0100.mo_code "+
                        " where MET01_0100.pro_code='"+ pProCode + "'"+
                        "   and MET01_0000.mo_end_date='"+ tMoEndDate.ToString("yyyy/MM/dd") + "'";
            DataTable dtTotal = comm.Get_DataTable(sSql);
            if (dtTotal.Rows.Count > 0)
            {
                decimal dTotalQty = comm.sGetDecimal(dtTotal.Rows[0]["total_qty"].ToString());
                decimal dProQty = comm.sGetDecimal(dtTotal.Rows[0]["pro_qty"].ToString());
                if (dTotalQty != 0)
                {
                    dRate = dProQty / dTotalQty;
                }
            }
            return dRate;
        }
        public string Get_MoListByDate(string pDate) {
            string sMoList = "''";
            string sSql = "";
            DateTime dDate=DateTime.Now;
            if (DateTime.TryParse(pDate, out dDate)) {
                DataTable dtTmp = Get_MoTable(dDate.ToString("yyyy/MM/dd"));
                if (dtTmp.Rows.Count > 0)
                {
                    for(int i = 0; i < dtTmp.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            sMoList="'"+dtTmp.Rows[i]["mo_code"]+"'";
                        }
                        else
                        {
                            sMoList += ",'" + dtTmp.Rows[i]["mo_code"] + "'";
                        }
                    }
                }
            }

            return sMoList;
        }



        /*修改地方向上*/

    }
}