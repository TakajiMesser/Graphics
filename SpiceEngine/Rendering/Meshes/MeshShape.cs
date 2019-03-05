using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Rendering.Buffers;
using SpiceEngine.Rendering.Materials;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Vertices;
using OpenTK.Graphics.OpenGL;

namespace SpiceEngine.Rendering.Meshes
{
    public class MeshShape
    {
        public List<Vector3> Vertices { get; }
        public List<int> TriangleIndices { get; }

        public MeshShape(IEnumerable<Vector3> vertices, IEnumerable<int> triangleIndices)
        {
            Vertices = vertices;
            TriangleIndices = triangleIndices;
        }

        public static MeshShape Box(float width, float height, float depth)
        {
            var vertices = new List<Vector3>
            {
                new Vector3(-width / 2.0f, -height / 2.0f, -depth / 2.0f),
                new Vector3(-width / 2.0f, height / 2.0f, -depth / 2.0f),
                new Vector3(-width / 2.0f, -height / 2.0f, depth / 2.0f),
                new Vector3(-width / 2.0f, height / 2.0f, depth / 2.0f),
                new Vector3(width / 2.0f, -height / 2.0f, -depth / 2.0f),
                new Vector3(width / 2.0f, height / 2.0f, -depth / 2.0f),
                new Vector3(width / 2.0f, -height / 2.0f, depth / 2.0f),
                new Vector3(width / 2.0f, height / 2.0f, depth / 2.0f)
            };

            var triangleIndices = new List<int>
            {
                8, 7, 5, 8, 5, 6,
                2, 4, 8, 2, 8, 6,
                4, 3, 7, 4, 7, 8,
                2, 1, 3, 2, 3, 4,
                3, 1, 5, 3, 5, 7,
                6, 5, 1, 6, 1, 2
            };

            return new MeshShape(vertices, triangleIndices);
        }

        public static MeshShape Rectangle(float width, float height)
        {
            var vertices = new List<Vector3>
            {
                new Vector3(-width / 2.0f, -height / 2.0f, 0.0f),
                new Vector3(-width / 2.0f, height / 2.0f, 0.0f),
                new Vector3(width / 2.0f, -height / 2.0f, 0.0f),
                new Vector3(width / 2.0f, height / 2.0f, 0.0f)
            };

            var triangleIndices = new List<int>
            {
                0, 2, 1, 1, 2, 3
            };

            return new MeshShape(vertices, triangleIndices);
        }

        public static MeshShape Sphere(float radius)
        {
            var vertices = new List<Vector3>();
            var triangleIndices = new List<int>();

            return new MeshShape(vertices, triangleIndices);
        }

        public static MeshShape Circle(float radius)
        {
            var vertices = new List<Vector3>();
            var triangleIndices = new List<int>();

            return new MeshShape(vertices, triangleIndices);
        }

        public static MeshShape Cylinder(float radius, float height, int nSides)
        {
            var vertices = new List<Vector3>();
            var triangleIndices = new List<int>();

            return new MeshShape(vertices, triangleIndices);
        }

        public static MeshShape Cone(float radius, float height, int nSides)
        {
            var vertices = new List<Vector3>();
            var triangleIndices = new List<int>();

            return new MeshShape(vertices, triangleIndices);
        }
    }
}
