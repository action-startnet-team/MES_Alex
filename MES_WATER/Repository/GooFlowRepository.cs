using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MES_WATER.Models;
using Newtonsoft.Json;
using System.Data.SqlClient;
using Dapper;
using System.Data;

namespace MES_WATER.Repository
{
    public class GooFlowRepository
    {
        /* 共用函數庫 */
        Comm comm = new Comm();

        /// <summary>
        /// 儲存流程圖主檔的json資料
        /// </summary>
        /// <param name="pTkCode"></param>
        /// <param name="pJson">流程圖的json資料</param>
        public void SaveJson(string pTkCode, string pJson, string pUsrCode)
        {

            // 資料已存在 (key: samp_code)
            if (!comm.Chk_RelData("MEB05_0000", "samp_code", pTkCode))
            {
                // 修改
                UpdateJson(pTkCode, pUsrCode, pJson);
                return;
            }

            // 新增
            InsertJson(pTkCode, pUsrCode, pJson);
        }

        /// <summary>
        /// 儲存流程圖的nodes
        /// </summary>
        /// <param name="pTkCode">鍵值</param>
        /// <param name="pNodes">流程圖的nodes</param>
        public void SaveNodes(string pTkCode, Dictionary<string, GooFlow.node> pNodes)
        {
            // 刪除原本資料
            DeleteNodes(pTkCode);

            // 新增資料
            foreach (KeyValuePair<string, GooFlow.node> item in pNodes)
            {
                InsertNode(pTkCode, item);
            }
        }

        /// <summary>
        /// 儲存流程圖的lines
        /// </summary>
        /// <param name="pTkCode">鍵值</param>
        /// <param name="pLines">流程圖的lines</param>
        public void SaveLines(string pTkCode, Dictionary<string, GooFlow.line> pLines)
        {
            // 刪除原本資料
            DeleteLines(pTkCode);

            // 新增資料
            foreach (KeyValuePair<string, GooFlow.line> item in pLines)
            {
                InsertLine(pTkCode, item);
            }
        }


        /* 資料庫操作 */
        /// <summary>
        /// 新增json字串
        /// </summary>
        /// <param name="pTkCode">鍵值</param>
        /// <param name="sUsrCode">成員代碼</param>
        /// <param name="pJson">json資料</param>
        public void InsertJson(string pTkCode, string sUsrCode, string pJson)
        {
            string sSql = " INSERT INTO " +
                   " MEB05_0000 ( samp_code, usr_code, gooflow_json )" +
                   " VALUES ( @samp_code, @usr_code, @gooflow_json )";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { samp_code = pTkCode, usr_code = sUsrCode, gooflow_json = pJson });
            }
        }

        /// <summary>
        /// 修改json字串
        /// </summary>
        /// <param name="pTkCode">鍵值</param>
        /// <param name="sUsrCode">成員代碼</param>
        /// <param name="pJson">json資料</param>
        public void UpdateJson(string pTkCode, string sUsrCode, string pJson)
        {
            string sSql = " Update MEB05_0000 " +
                          " Set gooflow_json = @gooflow_json " +
                          " Where samp_code = @samp_code " +
                          "   and usr_code = @usr_code";
            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { samp_code = pTkCode, usr_code = sUsrCode, gooflow_json = pJson });
            }

        }

        /// <summary>
        /// 刪除所有符合鍵值的node
        /// </summary>
        /// <param name="pTkCode">鍵值</param>
        public void DeleteNodes(string pTkCode)
        {
            string sSql = " Delete MEB05_0100 where samp_code = @samp_code ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { samp_code = pTkCode });
            }
        }

        /// <summary>
        /// 新增一個node
        /// </summary>
        /// <param name="pTkCode">鍵值</param>
        /// <param name="pNode">一個node</param>
        public void InsertNode(string pTkCode, KeyValuePair<string, GooFlow.node> pNode)
        {
            string sSql = " INSERT INTO " +
                          " MEB05_0100 ( samp_code, node_id, node_name, node_type, node_left, node_top, node_width, node_height ) " +
                          " VALUES ( @samp_code, @id, @name, @type, @left, @top, @width, @height ) ";

            DynamicParameters sqlParams = new DynamicParameters();

            sqlParams.Add("@samp_code", pTkCode);
            sqlParams.Add("@id", pNode.Key);
            sqlParams.AddDynamicParams(pNode.Value);

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, sqlParams);
            }
        }


        /// <summary>
        /// 刪除所有符合鍵值的line
        /// </summary>
        /// <param name="pTkCode">鍵值</param>
        public void DeleteLines(string pTkCode)
        {
            string sSql = " Delete MEB05_0200 where samp_code = @samp_code ";

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, new { samp_code = pTkCode });
            }
        }

        /// <summary>
        /// 新增一個line
        /// </summary>
        /// <param name="pTkCode">鍵值</param>
        /// <param name="pLine">一個line</param>
        public void InsertLine(string pTkCode, KeyValuePair<string, GooFlow.line> pLine)
        {
            string sSql = " INSERT INTO " +
                          " MEB05_0200 ( samp_code, line_id, line_name, line_type, line_from, line_to, line_M ) " +
                          " VALUES ( @samp_code, @id, @name, @type, @from, @to, @M ) ";

            DynamicParameters sqlParams = new DynamicParameters();

            sqlParams.Add("@samp_code", pTkCode);
            sqlParams.Add("@id", pLine.Key);
            sqlParams.AddDynamicParams(pLine.Value);

            using (SqlConnection con_db = comm.Set_DBConnection())
            {
                con_db.Execute(sSql, sqlParams);
            }
        }

        /// <summary>
        /// 取得排序的node list，取得的datatable
        /// </summary>
        /// <param name="pSampCode"></param>
        /// <returns></returns>
        public DataTable Get_OrderedDt(string pSampCode)
        {

            DataTable MEB05_0100 = Get_MEB05_0100(pSampCode);  // nodes

            DataTable result = MEB05_0100.Clone();

            List<string> orderedNodes = Get_OrderedNodeList(pSampCode);

            string startNode = FindStartNode(MEB05_0100);
            string endNode = FindEndNode(MEB05_0100);

            orderedNodes.Remove(startNode);
            orderedNodes.Remove(endNode);
            //orderedNodes.RemoveAt(0);
            //orderedNodes.RemoveAt(orderedNodes.Count -1);

            for (int i = 0; i < orderedNodes.Count; i++)
            {
                foreach(DataRow dr in MEB05_0100.Rows)
                {
                    if (dr["node_id"].ToString() == orderedNodes[i])
                    {
                        result.Rows.Add(dr.ItemArray);
                        break;
                    }
                }


            }

            return result;

        }

        /// <summary>
        /// 取得排序的node list
        /// </summary>
        /// <param name="pSampCode"></param>
        /// <returns></returns>
        public List<string> Get_OrderedNodeList(string pSampCode)
        {

            List<string> result = new List<string>();

            DataTable MEB05_0100 = Get_MEB05_0100(pSampCode);  // nodes
            DataTable MEB05_0200 = Get_MEB05_0200(pSampCode);  // lines

            string current = "";

            string start = FindStartNode(MEB05_0100);

            current = start;

            result.Add(start);

            while (!string.IsNullOrEmpty(current))
            {
                current = FindNextNode(MEB05_0200, current);

                result.Add(current);
            }

            // 去除最後一項空字串
            result.RemoveAt(result.Count() - 1);

            return result;

        }

        public string FindStartNode(DataTable pMEM05_0100)
        {
            foreach (DataRow dr in pMEM05_0100.Rows)
            {
                if (dr["node_type"].ToString().Contains("start"))
                {
                    return dr["node_id"].ToString();
                }
            }

            if (pMEM05_0100.Rows.Count > 0)
            {
                return pMEM05_0100.Rows[0]["node_id"].ToString();
            }

            return "";

        }

        public string FindEndNode(DataTable pMEM05_0100)
        {
            foreach (DataRow dr in pMEM05_0100.Rows)
            {
                if (dr["node_type"].ToString().Contains("end"))
                {
                    return dr["node_id"].ToString();
                }
            }

            if (pMEM05_0100.Rows.Count > 0)
            {
                return pMEM05_0100.Rows[0]["node_id"].ToString();
            }

            return "";

        }

        public string FindNextNode(DataTable pMEM05_0200, string pCurrentNode)
        {
            foreach (DataRow dr in pMEM05_0200.Rows)
            {
                if (dr["line_from"].ToString() == pCurrentNode)
                {
                    return dr["line_to"].ToString();
                }
            }

            return "";
        }

        public DataTable Get_MEB05_0100(string pSampCode)
        {
            Dictionary<string, object> sSqlParams = new Dictionary<string, object>();
            sSqlParams.Add("@samp_code", pSampCode);
            string sSql = @"select * from MEB05_0100 where samp_code = @samp_code";
            DataTable dt = comm.Get_DataTable(sSql, sSqlParams);

            return dt;
        }

        public DataTable Get_MEB05_0200(string pSampCode)
        {
            Dictionary<string, object> sSqlParams = new Dictionary<string, object>();
            sSqlParams.Add("@samp_code", pSampCode);
            string sSql = @"select * from MEB05_0200 where samp_code = @samp_code";
            DataTable dt = comm.Get_DataTable(sSql, sSqlParams);

            return dt;
        }


    }
}