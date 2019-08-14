using SpiceEngine.Physics.Bodies;
using System;
using System.Collections.Generic;
using System.Text;

namespace SauceEditorCore.Models.BSP
{
    public interface INode
    {
        IEnumerable<int> GetIDs();
        IEnumerable<Face> GetFaces();
    }
}
