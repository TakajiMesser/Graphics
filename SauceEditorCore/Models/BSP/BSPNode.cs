using System;
using System.Collections.Generic;
using System.Text;

namespace SauceEditorCore.Models.BSP
{
    public enum ConstructiveTypes
    {
        Union,
        Intersection,
        Negation
    }

    /*
    A CSG Tree is a binary tree, meaning each node can have either NodeA or NodeB
    If this is a leaf, it should have a Body3D associated with it
     */

    public class BSPNode : INode
    {
        public ConstructiveTypes ConstructiveType { get; set; }

        public INode NodeA { get; set; }
        public INode NodeB { get; set; }

        public IEnumerable<int> GetIDs()
        {
            foreach (var id in NodeA.GetIDs())
            {
                yield return id;
            }

            foreach (var id in NodeB.GetIDs())
            {
                yield return id;
            }
        }

        public IEnumerable<Face> GetFaces()
        {
            foreach (var face in NodeA.GetFaces())
            {
                yield return face;
            }

            foreach (var face in NodeB.GetFaces())
            {
                yield return face;
            }
        }
    }
}
