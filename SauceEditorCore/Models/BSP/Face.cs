using OpenTK;
using System.Collections.Generic;

namespace SauceEditorCore.Models.BSP
{
    public class Face
    {
        public List<Vector3> Vertices { get; } = new List<Vector3>();
        public Vector3 Normal { get; set; }
        public Vector3 Tangent { get; set; }
    }
}
