using System.Collections.Generic;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

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
