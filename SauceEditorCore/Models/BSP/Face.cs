using System.Collections.Generic;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SauceEditorCore.Models.BSP
{
    public class Face
    {
        public List<Vector3> Vertices { get; } = new List<Vector3>();
        public Vector3 Normal { get; set; }
        public Vector3 Tangent { get; set; }
    }
}
