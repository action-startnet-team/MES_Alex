using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MES_WATER.Controllers
{
    public class SampleController : JsonNetController
    {
        // GET: Sample
        public ActionResult PieNest()
        {
            return View();
        }

        public ActionResult TreeBasic()
        {
            return View();
        }

        public ActionResult TreeBasic_test() {
            return View();
        }

        public ActionResult TreeBasic_sample()
        {
            return View();
        }

        public JsonResult Get_PieNest_Data()
        {
            List<string> legend_data = new List<string>() { "直达", "营销广告", "搜索引擎", "邮件营销", "联盟广告", "视频广告", "百度", "谷歌", "必应", "其他" };

            List<object> series = new List<object>();

            string seriesItem1_name = "访问来源";
            List<object> seriesItem1_data = new List<object>() {
                new { value = 335, name = "直达", selected = true },
                new { value = 679, name = "营销广告"  },
                new { value = 1548, name = "搜索引擎"  },
            };

            string seriesItem2_name = "访问来源";
            List<object> seriesItem2_data = new List<object>() {
                new { value = 335, name = "直达", selected = true },
                new { value = 679, name = "营销广告"  },
                new { value = 1548, name = "搜索引擎"  },
                new {value = 335, name= "直达"},
                new {value = 310, name= "邮件营销"},
                new {value= 234, name="联盟广告"},
                new {value= 135, name= "视频广告"},
                new {value= 1048, name= "百度"},
                new {value= 251, name= "谷歌"},
                new {value= 147, name= "必应"},
                new {value= 102, name= "其他"}
            };

            series.Add(new { name = seriesItem1_name, data = seriesItem1_data });
            series.Add(new { name = seriesItem2_name, data = seriesItem2_data });

            var returnObj = new {
                legend = new {
                    data = legend_data
                },
                series = series
            };

            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get_TreeBasic_Data()
        {
            TreeNode root = new TreeNode("flare");

            string path1 = "analytics > cluster";
            List<TreeNode> path1_leafNodes = new List<TreeNode>() {
                new TreeNode("AgglomerativeCluster", 3938),
                new TreeNode("CommunityStructure", 3812),
                new TreeNode("HierarchicalCluster", 6714),
                new TreeNode("MergeEdge", 743),
            };
            root.appendBranches(path1, path1_leafNodes);

            string path2 = "analytics > graph";
            List<TreeNode> path2_leafNodes = new List<TreeNode>() {
                new TreeNode("BetweennessCentrality", 3534),
                new TreeNode ("LinkDistance", 5731),
                new TreeNode("MaxFlowMinCut", 7840),
                new TreeNode("ShortestPaths", 5914),
                new TreeNode("SpanningTree", 3416),
            };
            root.appendBranches(path2, path2_leafNodes);

            string path3 = "analytics > optimization";
            List<TreeNode> path3_leafNodes = new List<TreeNode>() {
                new TreeNode("AspectRatioBanker", 7074),
            };
            root.appendBranches(path3, path3_leafNodes);

            string path4 = "animate";
            List<TreeNode> path4_leafNodes = new List<TreeNode>() {
                new TreeNode("Easing", 17010),
            };
            root.appendBranches(path4, path4_leafNodes);

            string path5 = "animate";
            List<TreeNode> path5_leafNodes = new List<TreeNode>() {
                new TreeNode("FunctionSequence", 5842),
            };
            root.appendBranches(path5, path5_leafNodes);

            string path6 = "animate > interpolate";
            List<TreeNode> path6_leafNodes = new List<TreeNode>() {
               new TreeNode(){ name =  "ArrayInterpolator", value =  1983 },
               new TreeNode() { name =  "ColorInterpolator", value =  2047 },
               new TreeNode() { name =  "DateInterpolator", value =  1375 },
               new TreeNode() { name =  "Interpolator", value =  8746 },
               new TreeNode() { name =  "MatrixInterpolator", value =  2202 },
               new TreeNode() { name =  "NumberInterpolator", value =  1382 },
               new TreeNode() { name =  "ObjectInterpolator", value =  1629 },
               new TreeNode() { name =  "PointInterpolator", value =  1675 },
               new TreeNode() { name =  "RectangleInterpolator", value =  204 }
            };
            root.appendBranches(path6, path6_leafNodes);

            string path7 = "animate";
            List<TreeNode> path7_leafNodes = new List<TreeNode>() {
               new TreeNode(){ name =  "ISchedulable", value =  1041 },
            };
            root.appendBranches(path7, path7_leafNodes);

            string path8 = "data > converters";
            List<TreeNode> path8_leafNodes = new List<TreeNode>() {
               new TreeNode(){ name =  "Converters", value =  721 },
               new TreeNode(){ name =  "DelimitedTextConverter", value =  4294 },
               new TreeNode(){ name =  "GraphMLConverter", value =  9800 },
               new TreeNode(){ name =  "IDataConverter", value =  1314 },
               new TreeNode(){ name =  "JSONConverter", value =  2220 },
            };
            root.appendBranches(path8, path8_leafNodes);


            return Json(root, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get_TreeBasic_Data_2()
        {
            TreeNode root = new TreeNode("flare");

            root.appendChildNodes(new List<TreeNode>() {
                new TreeNode("analytics"),
                new TreeNode("animate"),
                new TreeNode("data"),
                new TreeNode("display"),
                new TreeNode("flex"),
                new TreeNode("physics"),
                new TreeNode("query"),
                new TreeNode("scale"),
            });

            TreeNode node_analytics = root.findChildNode("analytics")
                .appendChildNodes(new List<TreeNode>() {
                    new TreeNode("cluster"),
                    new TreeNode("graph"),
                    new TreeNode("optimization"),
                });

            node_analytics.findChildNode("graph")
                          .appendChildNodes(new List<TreeNode>() {
                              new TreeNode("BetweennessCentrality", 3534),
                              new TreeNode ("LinkDistance", 5731),
                              new TreeNode("MaxFlowMinCut", 7840),
                              new TreeNode("ShortestPaths", 5914),
                              new TreeNode("SpanningTree", 3416),
                          });
            node_analytics.findChildNode("cluster")
                          .appendChildNodes(new List<TreeNode>() {
                              new TreeNode("AgglomerativeCluster", 3938),
                              new TreeNode("CommunityStructure", 3812),
                              new TreeNode("HierarchicalCluster", 6714),
                              new TreeNode("MergeEdge", 743),
                          });

            return Json(root, JsonRequestBehavior.AllowGet);
        }



        public class TreeNode
        {
            public string name { get; set; }

            public double value { get; set; }

            public List<TreeNode> children { get; set; }

            public TreeNode(string name = "", double value = 0) {
                this.name = name;
                this.value = value;
            }

            public TreeNode appendToNode(TreeNode parentNode)
            {
                parentNode.appendChildNode(this);

                return this;
            }

            public TreeNode appendChildNode(TreeNode childNode)
            {
                if (this.children == null)
                {
                    this.children = new List<TreeNode>();
                }

                this.children.Add(childNode);

                return this;
            }

            public TreeNode appendChildNodes(List<TreeNode> childNodes)
            {
                if (childNodes.Count == 0)
                {
                    return this;
                }

                if (this.children == null)
                {
                    this.children = new List<TreeNode>();
                }

                for(int i = 0; i < childNodes.Count; i++)
                {
                    this.children.Add(childNodes[i]);
                }

                return this;
            }

            public TreeNode appendBranches(List<TreeNode> pathNodes, List<TreeNode> leafNodes = null)
            {
                TreeNode currentNode = this;

                for (int i = 0; i < pathNodes.Count; i++)
                {
                    TreeNode thisFindNode = currentNode.findChildNode(pathNodes[i].name);
                    if (thisFindNode == null)
                    {
                        currentNode = pathNodes[i].appendToNode(currentNode);
                    }
                    else
                    {
                        currentNode = thisFindNode;
                    }
                }

                if (leafNodes != null)
                {
                    currentNode.appendChildNodes(leafNodes);
                }

                return this;
            }

            public TreeNode appendBranches(string path, List<TreeNode> leafNodes = null)
            {
                //
                if (string.IsNullOrEmpty(path))
                {
                    return this;
                }

                // parse path string
                string[] parsePath = path.Split('>').Select(item => item.Trim()).ToArray();

                //
                if (parsePath.Length == 0)
                {
                    return this;
                }

                // 邏輯處理
                TreeNode currentNode = this;
                for (int i = 0; i < parsePath.Length; i++)
                {
                    TreeNode thisFindNode = currentNode.findChildNode(parsePath[i]);
                    if (thisFindNode == null)
                    {
                        TreeNode node = new TreeNode(parsePath[i]);
                        currentNode = node.appendToNode(currentNode);
                    }
                    else
                    {
                        currentNode = thisFindNode;
                    }
                   
                }

                // 末端
                if (leafNodes != null)
                {
                    currentNode.appendChildNodes(leafNodes);
                }

                return this;
            }

            public bool hasChildNode(TreeNode childNode)
            {
                if (this.children == null)
                {
                    return false;
                }

                for (int i = 0; i < this.children.Count; i++)
                {
                    if (this.children[i].name == childNode.name)
                    {
                        return true;
                    }
                }
                return false;
            }


            public TreeNode findChildNode(string nodeName)
            {
                if (this.children == null)
                {
                    return null;
                }

                for (int i = 0; i < this.children.Count; i++)
                {
                    if (this.children[i].name == nodeName)
                    {
                        return this.children[i];
                    }
                }

                return null;
            }

        }

    }
}