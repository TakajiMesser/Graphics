using System;
using System.Collections.Generic;
using System.Text;

namespace SauceEditorCore.Models.BSP
{
    public enum ConstructiveTypes
    {
        None,
        Intersection,
        Union,
        Negation
    }


    public class BSPNode
    {
        public ConstructiveTypes ConstructiveType { get; set; }

        public BSPNode NodeA { get; set; }
        public BSPNode NodeB { get; set; }

        public ISolid SolidA { get; set; }
        public ISolid SolidB { get; set; }

        public void AddNode(BSPNode node)
        {
            
        }

        public void AddSolid(ISolid solid)
        {

        }
    }
}
