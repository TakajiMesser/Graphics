using SpiceEngineCore.Rendering.Matrices;
using System.Runtime.InteropServices;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SweetGraphicsCore.Vertices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Simple3DVertex : IVertex3D
    {
        public Vector3 Position { get; private set; }

        public Simple3DVertex(Vector3 position) => Position = position;

        public Simple3DVertex(float x, float y, float z) => Position = new Vector3(x, y, z);

        public IVertex3D Transformed(Transform transform) => new Simple3DVertex((transform.ToMatrix() * new Vector4(Position, 1.0f)).Xyz);
    }
}
