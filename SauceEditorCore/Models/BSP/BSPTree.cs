using System;
using System.Collections.Generic;
using System.Text;

namespace SauceEditorCore.Models.BSP
{
    public class BSPTree
    {
        public List<BSPNode> Nodes { get; } = new List<BSPNode>();

        public ConstructiveTypes ConstructiveType { get; set; }

        public INode NodeA { get; set; }
        public INode NodeB { get; set; }

        public void AddNode(BSPNode node)
        {
            
        }

        public void AddSolid(ISolid solid)
        {

        }
    }
}
