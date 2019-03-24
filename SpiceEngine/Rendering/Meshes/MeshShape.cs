using OpenTK;
using SpiceEngine.Rendering.Vertices;
using System;
using System.Collections.Generic;

namespace SpiceEngine.Rendering.Meshes
{
    public class MeshShape
    {
        public List<Vector3> Vertices { get; set; } = new List<Vector3>();
        public List<int> TriangleIndices { get; set; } = new List<int>();

        public static MeshShape Rectangle(float width, float height) => new MeshShape()
        {
            Vertices = new List<Vector3>
            {
                new Vector3(-width / 2.0f, -height / 2.0f, 0.0f),
                new Vector3(-width / 2.0f, height / 2.0f, 0.0f),
                new Vector3(width / 2.0f, -height / 2.0f, 0.0f),
                new Vector3(width / 2.0f, height / 2.0f, 0.0f)
            },
            TriangleIndices = new List<int>()
            {
                0, 2, 1, 1, 2, 3
            }
        };

        public static MeshShape RectangularPrism(float width, float height, float depth) => new MeshShape()
        {
            Vertices = new List<Vector3>
            {
                new Vector3(-width / 2.0f, -height / 2.0f, -depth / 2.0f),
                new Vector3(-width / 2.0f, height / 2.0f, -depth / 2.0f),
                new Vector3(-width / 2.0f, -height / 2.0f, depth / 2.0f),
                new Vector3(-width / 2.0f, height / 2.0f, depth / 2.0f),
                new Vector3(width / 2.0f, -height / 2.0f, -depth / 2.0f),
                new Vector3(width / 2.0f, height / 2.0f, -depth / 2.0f),
                new Vector3(width / 2.0f, -height / 2.0f, depth / 2.0f),
                new Vector3(width / 2.0f, height / 2.0f, depth / 2.0f)
            },
            TriangleIndices = new List<int>()
            {
                8, 7, 5, 8, 5, 6,
                2, 4, 8, 2, 8, 6,
                4, 3, 7, 4, 7, 8,
                2, 1, 3, 2, 3, 4,
                3, 1, 5, 3, 5, 7,
                6, 5, 1, 6, 1, 2
            }
        };
    }
}
