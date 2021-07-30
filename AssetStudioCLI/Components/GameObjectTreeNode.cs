using AssetStudio;
using System.Collections.Generic;

namespace AssetStudioCLI
{
    internal class TreeNode
    {
        public string Text;
        public List<TreeNode> Nodes;
        public bool Checked;

        public TreeNode()
        {
            Nodes = new List<TreeNode>();
        }
    }
    internal class GameObjectTreeNode : TreeNode
    {
        public GameObject gameObject;

        public GameObjectTreeNode(string name)
        {
            Text = name;
        }

        public GameObjectTreeNode(GameObject gameObject)
        {
            this.gameObject = gameObject;
            Text = gameObject.m_Name;
        }
    }
}
