using OpenTK;
using System;
using System.Collections.Generic;
using SpiceEngine.Utilities;

namespace SpiceEngine.Rendering.Meshes
{
    public class MeshFace
    {
        public List<Vector3> Vertices { get; set; } = new List<Vector3>();
        public Vector3 Normal { get; set; }
        public Vector3 Tangent { get; set; }

        public MeshFace() { }
        public MeshFace(params Vector3[] vertices) : this(LINQExtensions.Generate(vertices)) { }
        public MeshFace(IEnumerable<Vector3> vertices)
        {
            Vertices.AddRange(vertices);
        }

        public IEnumerable<MeshTriangle> GetMeshTriangles()
        {
            if (Vertices.Count < 3) throw new InvalidOperationException("MeshFace must consist of at least 3 vertices");

            for (var i = 1; i < Vertices.Count - 1; i++)
            {
                var vertexA = Vertices[0];
                var vertexB = Vertices[i];
                var vertexC = Vertices[i + 1];

                yield return new MeshTriangle(vertexA, vertexB, vertexC)
                {
                    Normal = Normal,
                    Tangent = Tangent
                };
            }
        }

        public static MeshFace Rectangle(float width, float height, Vector3 center, Vector3 normal, Vector3 tangent)
        {
            return new MeshFace()
            {
                Vertices = new List<Vector3>()
                {
                    new Vector3()
                },
                Normal = normal,
                Tangent = tangent
            };
        }

        /*public static MeshShape Rectangle(float width, float height) => new MeshShape()
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
        };*/
    }
}
