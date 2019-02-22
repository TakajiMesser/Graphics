using OpenTK;
using SpiceEngine.Physics.Collisions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Physics.Shapes
{
    public struct PolygonVertex3D
    {
        public Vector3 Vertex { get; }
        public Vector3 Normal { get; }

        public PolygonVertex3D(Vector3 vertex, Vector3 normal)
        {
            Vertex = vertex;
            Normal = normal;
        }
    }
}
