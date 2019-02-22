using OpenTK;
using SpiceEngine.Physics.Collisions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Physics.Shapes
{
    public struct PolygonVertex2D
    {
        public Vector2 Vertex { get; }
        public Vector2 Normal { get; }

        public PolygonVertex2D(Vector2 vertex, Vector2 normal)
        {
            Vertex = vertex;
            Normal = normal;
        }
    }
}
