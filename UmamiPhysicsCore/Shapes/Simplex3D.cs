using OpenTK;
using System.Collections.Generic;

namespace UmamiPhysicsCore.Shapes
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
