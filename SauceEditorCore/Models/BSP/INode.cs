using System.Collections.Generic;

namespace SauceEditorCore.Models.BSP
{
    public interface INode
    {
        IEnumerable<int> GetIDs();
        IEnumerable<Face> GetFaces();
    }
}
