using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MES_WATER.Models
{
    public class TreeNode
    {
        public string name { get; set; }

        public string code { get; set; }

        //public bool collapsed { get; set; }
        public string data { get; set; }

        public int level { get; set; }

        public List<TreeNode> children { get; set; }

        public TreeNode(string name = "", string code = "")
        {
            this.name = name;
            this.code = code;
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

            for (int i = 0; i < childNodes.Count; i++)
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