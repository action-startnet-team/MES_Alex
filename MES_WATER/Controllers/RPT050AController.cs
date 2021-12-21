using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using MES_WATER.Models;
using System.Data;
using MES_WATER.Repository;

namespace MES_WATER.Controllers
{
    public class RPT050AController : JsonNetController
    {
        //
        Comm comm = new Comm();
        GetData GD = new GetData();

        //
        MET01_0000Repository repoMET01_0000 = new MET01_0000Repository();

        public ActionResult Index()
        {
            // 根

            return View();
        }

        public JsonResult Get_TreeData(string pMoCode)
        {
            DataTable MET01_0000 = Get_MET01_0000(pMoCode);

            TreeNode root = new TreeNode();

            root.name = "製令: " + pMoCode;
            if (MET01_0000.Rows.Count > 0)
            {
                root.name += "\n\n" + "產品: " + MET01_0000.Rows[0]["pro_name"];
                root.name += "\n\n" + "數量: " + comm.sGetDecimal(MET01_0000.Rows[0]["plan_qty"].ToString()).ToString("G29");
            }

            DataTable dtTmp = Get_MED06_0000(pMoCode);
            foreach (DataRow dr in dtTmp.Rows)
            {
                //               
                TreeNode child = new TreeNode();
                
                string sQmtCode = GD.Get_DataByMultiField("QMT04_0000", dr["pro_code"].ToString() + "," + dr["lot_no"].ToString(), "pro_code,lot_no", "qmt_code");
                if (string.IsNullOrEmpty(sQmtCode)) { sQmtCode = "未檢驗"; }

                child.name = "製程: " + dr["work_name"].ToString();
                child.name += "\n" + "物料: " + dr["pro_name"].ToString();
                child.name += "\n" + "批號: " + dr["lot_no"].ToString();
                child.name += "\n" + "數量: " + comm.sGetDecimal(dr["pro_qty"].ToString()).ToString("G29");
                child.name += "\n" + "進貨檢驗: " + sQmtCode;
                //
                child.appendToNode(root);
            }


            //WMT07_0000是應備料檔，改抓實際上料檔MED06_0000
            //DataTable WMT07_0000 = Get_WMT07_0000(pMoCode);
            //foreach(DataRow dr in WMT07_0000.Rows)
            //{
            //    //
            //    TreeNode child = new TreeNode();
            //    child.name = "製程: " + dr["work_name"].ToString();
            //    child.name += "\n" + "物料: " + dr["pro_name"].ToString();
            //    child.name += "\n" + "數量: " + dr["pro_qty"].ToString();
            //    child.name += "\n" + "檢驗日期: " ;

            //    //
            //    child.appendToNode(root);
            //}

            return Json(root, JsonRequestBehavior.AllowGet);
        }

        private DataTable Get_MED06_0000(string pMoCode)
        {
            Dictionary<string, object> sSqlParams = new Dictionary<string, object>();
            sSqlParams.Add("@mo_code", pMoCode);
            string sSql = @"Select MED06_0000.*, MEB20_0000.pro_name, MEB30_0000.work_name
                            from MED06_0000
                            left join MEB20_0000 on MEB20_0000.pro_code = MED06_0000.pro_code
                            left join MEB30_0000 on MEB30_0000.work_code = MED06_0000.work_code
                            where mo_code = @mo_code
                            order by work_code,ins_date desc ,ins_time desc ";
            DataTable dt = comm.Get_DataTable(sSql, sSqlParams);
            return dt;
        }


        // 之前的Get_TreeData，先註解起來
        //public JsonResult Get_TreeData(string pMoCode)
        //{
        //    DataTable dtTmp = Get_MET03_0000(pMoCode);

        //    if (dtTmp.Rows.Count <= 0)
        //    {
        //        return Json("", JsonRequestBehavior.AllowGet);
        //    }

        //    string pro_code = dtTmp.Rows[0]["pro_code"].ToString();
        //    string pro_name = comm.Get_QueryData("MEB20_0000", pro_code, "pro_code", "pro_name");

        //    // 初始
        //    TreeNode root = new TreeNode();
        //    root.name = pMoCode + "\n" + pro_name;
        //    //root.data = JsonConvert.SerializeObject(new { pro_code = pro_code, pro_name = pro_name });


        //    TreeNode current = root;
        //    string sNowWrkCode = "";
        //    foreach(DataRow dr in dtTmp.Rows)
        //    {
        //        if (sNowWrkCode == "")
        //        {
        //            sNowWrkCode = dr["wrk_code"].ToString();
        //        }

        //        TreeNode node = new TreeNode();
        //        node.name = dr["wrk_code"].ToString();
        //        node.name += "\n";
        //        node.name += "加工人員:" + comm.Get_QueryData("BDP08_0000", dr["usr_code"].ToString(), "usr_code", "usr_name");
        //        node.name += "\n";
        //        node.name += "加工日期:" + dr["ins_date"].ToString() + "-" + dr["ins_time"].ToString();
        //        //node.data = SerializeDataRow(dr);
        //        current.appendChildNode(node);

        //        //current = node;

        //        //if (sNowWrkCode== dr["wrk_code"].ToString())
        //        //    {
        //        //        //同樣的單號不同的報工資料
        //        //        current.appendChildNode(node);
        //        //    }
        //        //    else
        //        //    {
        //        //        current = node.appendToNode(current);
        //        //        sNowWrkCode = dr["wrk_code"].ToString();
        //        //    }

        //    }

        //    //// 第一層
        //    //TreeNode root = new TreeNode();
        //    ////root.code = "mo_code";
        //    //root.name = "1";

        //    //// 第二層
        //    //TreeNode node_1_1 = new TreeNode();
        //    //node_1_1.name = "1-1";
        //    //root.appendChildNode(node_1_1);

        //    //TreeNode node_1_2 = new TreeNode();
        //    //node_1_2.name = "1-2";
        //    //root.appendChildNode(node_1_2);

        //    //// 第三層
        //    //TreeNode node_1_1_1 = new TreeNode();
        //    //node_1_1_1.name = "1-1-1";
        //    //node_1_1.appendChildNode(node_1_1_1);

        //    //TreeNode node_1_1_2 = new TreeNode();
        //    //node_1_1_2.name = "1-1-2";
        //    //node_1_1.appendChildNode(node_1_1_2);

        //    //// 取得製令主檔資料
        //    //MET01_0000 root_data = repoMET01_0000.GetDTO(pMoCode);
        //    //root.data = JsonConvert.SerializeObject(root_data);

        //    //// 製令下的製程
        //    //DataTable processDt = Get_ProcessDt(pMoCode);

        //    //AddNodesFromDt(root, "process_code", processDt);

        //    //// find 下面有子製令的製程 (Q004寫死)
        //    //TreeNode thisProcessNode = null;
        //    //if (root.children != null)
        //    //{
        //    //    foreach (TreeNode node in root.children)
        //    //    {
        //    //        if (node.code == "process_code" && node.name == "Q004")
        //    //        {
        //    //            thisProcessNode = node;
        //    //            break;
        //    //        }
        //    //    }
        //    //}

        //    //if (thisProcessNode == null)
        //    //{
        //    //    return Json(root, JsonRequestBehavior.AllowGet);
        //    //}

        //    //// 增加子製令在Q004下
        //    //DataTable subMoDt = Get_SubMoDt(pMoCode);

        //    //AddNodesFromDt(thisProcessNode, "mo_code", subMoDt);

        //    //// 子製令的製程
        //    //thisProcessNode.children.ForEach(node => {
        //    //    AddNodesFromDt(node, "process_code", Get_ProcessDt(node.name));
        //    //});

        //    // 參考
        //    //string path1 = "analytics > cluster";
        //    //List<TreeNode> path1_leafNodes = new List<TreeNode>() {
        //    //    new TreeNode("AgglomerativeCluster", 3938),
        //    //    new TreeNode("CommunityStructure", 3812),
        //    //    new TreeNode("HierarchicalCluster", 6714),
        //    //    new TreeNode("MergeEdge", 743),
        //    //};
        //    //root.appendBranches(path1, path1_leafNodes);


        //    return Json(root, JsonRequestBehavior.AllowGet);
        //}



        /// <summary>
        /// 
        /// </summary>
        /// <param name="pParentNode"></param>
        /// <param name="pCode"></param>
        /// <param name="pDt"></param>
        private void AddNodesFromDt(TreeNode pParentNode, string pColCode, DataTable pDt)
        {
            foreach (DataRow row in pDt.Rows)
            {
                TreeNode node = new TreeNode()
                {
                    code = pColCode,
                    name = row[pColCode].ToString(),
                    data = SerializeDataRow(row)

                };
                pParentNode.appendChildNode(node);

            }
        }

        public string SerializeDataRow(DataRow pDataRow)
        {
            string result = "";

            string s = JsonConvert.SerializeObject(pDataRow.Table);

            List<object> list = JsonConvert.DeserializeObject<List<object>>(s);

            result = JsonConvert.SerializeObject(list[0]);

            return result;
        }

        /// <summary>
        /// 抓製令下的製程
        /// </summary>
        /// <param name="pMoCode"></param>
        /// <returns></returns>
        private DataTable Get_ProcessDt(string pMoCode)
        {
            string sql = " select MET01_0100.*, MEB01_0000.process_name "
                       + " from MET01_0100 "
                       + " left join MEB01_0000 on MEB01_0000.process_code = MET01_0100.process_code"
                       + " where mo_code = @mo_code";

            DataTable dt = comm.Get_DataTable(sql, "mo_code", pMoCode);

            return dt;

        }

        /// <summary>
        /// 抓母製令的子製令 (不包含母製令)
        /// </summary>
        /// <param name="pMoCode"></param>
        /// <returns></returns>
        private DataTable Get_SubMoDt(string pMoCode)
        {
            string sql = " select * "
                       + " from MET01_0000 "
                       + " where up_mo_type + '-' + up_mo_code = @mo_code"
                       + "  and mo_code != @mo_code";  // 母製令的母製令就是自己

            DataTable dt = comm.Get_DataTable(sql, "mo_code", pMoCode);

            return dt;
        }

        private DataTable Get_MET03_0000(string pMoCode)
        {
            Dictionary<string, object> sqlParams = new Dictionary<string, object>();
            sqlParams.Add("@mo_code", pMoCode);

            //string sSql = "Select * from MET03_0000 where mo_code=@mo_code";
            string sSql = "SELECT MET03_0000.wrk_code, MET03_0000.mo_code, MET03_0000.pro_code,MED09_0000.usr_code,MED09_0000.ins_date,MED09_0000.ins_time " +
                          "  FROM MET03_0000 " +
                          "  LEFT JOIN MED09_0000 ON MET03_0000.wrk_code = MED09_0000.wrk_code" +
                          " where MET03_0000.mo_code = @mo_code" +
                          "   and isnull(MED09_0000.usr_code,'')<>''" +
                          " order by wrk_code ";
            DataTable dt = comm.Get_DataTable(sSql, sqlParams);

            return dt;
        }


        /// <summary>
        /// 製令主檔
        /// </summary>
        /// <param name="pMoCode"></param>
        /// <returns></returns>
        private DataTable Get_MET01_0000(string pMoCode)
        {
            Dictionary<string, object> sSqlParams = new Dictionary<string, object>();
            sSqlParams.Add("@mo_code", pMoCode);
            string sSql = @"select MET01_0000.*, MEB20_0000.pro_name
from MET01_0000
left join MEB20_0000 on MEB20_0000.pro_code = MET01_0000.pro_code
where mo_code = @mo_code
";

            DataTable dt = comm.Get_DataTable(sSql, sSqlParams);

            return dt;
        }

        /// <summary>
        /// 用料
        /// </summary>
        /// <param name="pMoCode"></param>
        /// <returns></returns>
        private DataTable Get_WMT07_0000(string pMoCode)
        {
            Dictionary<string, object> sSqlParams = new Dictionary<string, object>();
            sSqlParams.Add("@mo_code", pMoCode);
            string sSql = @"Select WMT07_0000.*, MEB20_0000.pro_name, MEB30_0000.work_name
from WMT07_0000
left join MEB20_0000 on MEB20_0000.pro_code = WMT07_0000.pro_code
left join MEB30_0000 on MEB30_0000.work_code = WMT07_0000.work_code
where mo_code = @mo_code
";

            DataTable dt = comm.Get_DataTable(sSql, sSqlParams);

            return dt;
        }



    }
}