using System.Collections.Generic;

using Color4 = SpiceEngineCore.Geometry.Color4;
using Matrix2 = SpiceEngineCore.Geometry.Matrix2;
using Matrix3 = SpiceEngineCore.Geometry.Matrix3;
using Matrix4 = SpiceEngineCore.Geometry.Matrix4;
using Quaternion = SpiceEngineCore.Geometry.Quaternion;
using Vector2 = SpiceEngineCore.Geometry.Vector2;
using Vector3 = SpiceEngineCore.Geometry.Vector3;
using Vector4 = SpiceEngineCore.Geometry.Vector4;

namespace SavoryPhysicsCore.Shapes
{
    public class Simplex3D : ISimplex
    {
        private List<Vector3> _vertices = new List<Vector3>();

        public void Add(Vector3 vertex)
        {
            _vertices.Add(vertex);
        }

        /*public bool ContainsOrigin(Vector3 direction)
        {
            var a = _vertices.Last();

            var ao = -a;

            if (_vertices.Count == 3)
            {
                // Get B and C of triangle
                var b = getB();
                var c = getC();


            }

            return false;
        }*/
    }
}
