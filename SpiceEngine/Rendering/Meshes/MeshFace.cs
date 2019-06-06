using OpenTK;
using System;
using System.Collections.Generic;
using SpiceEngine.Utilities;
using System.Linq;

namespace SpiceEngine.Rendering.Meshes
{
    public class MeshFace
    {
        /// <summary>
        /// The vertices should be in clockwise order.
        /// </summary>
        public List<MeshVertex> Vertices { get; set; } = new List<MeshVertex>();
        //public List<MeshTriangle> Triangles { get; set; } = new List<MeshTriangle>();

        private Vector3 _normal = Vector3.UnitZ;
        public Vector3 Normal
        {
            get => _normal;
            set
            {
                _normal = value;

                foreach (var vertex in Vertices)
                {
                    vertex.Normal = value;
                }
            }
        }

        private Vector3 _tangent = Vector3.UnitY;
        public Vector3 Tangent
        {
            get => _tangent;
            set
            {
                _tangent = value;

                foreach (var vertex in Vertices)
                {
                    vertex.Tangent = value;
                }
            }
        }
        public UVMap UVMap { get; set; } = UVMap.Standard;

        public Vector3 Bitangent => -Vector3.Cross(Normal, Tangent);
        public float UVXOrigin => Vertices.Min(v => Vector3.Dot(Quaternion.FromAxisAngle(Normal, UVMap.Rotation) * Bitangent, v));
        public float UVYOrigin => Vertices.Min(v => Vector3.Dot(Quaternion.FromAxisAngle(Normal, UVMap.Rotation) * Tangent, v));

        public MeshFace() { }
        public MeshFace(params MeshVertex[] vertices) : this(LINQExtensions.Generate(vertices)) { }
        public MeshFace(IEnumerable<MeshVertex> vertices) => Vertices.AddRange(vertices);
        public MeshFace(IEnumerable<Vector3> vertices) : this(vertices.Select(v => new MeshVertex() { Position = v })) { }

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

        public void Translate(float x, float y, float z)
        {
            var translation = new Vector3(x, y, z);

            for (var i = 0; i < Vertices.Count; i++)
            {
                Vertices[i].Position += translation;
            }
        }

        public MeshFace Translated(float x, float y, float z)
        {
            var translated = new MeshFace(Vertices)
            {
                Normal = Normal,
                Tangent = Tangent
            };

            var translation = new Vector3(x, y, z);

            for (var i = 0; i < Vertices.Count; i++)
            {
                translated.Vertices[i].Position = Vertices[i].Position + translation;
            }

            return translated;
        }

        public void Rotate(Vector3 axis, float angle)
        {
            var rotation = Quaternion.FromAxisAngle(axis, angle);

            for (var i = 0; i < Vertices.Count; i++)
            {
                Vertices[i].Position = rotation * Vertices[i].Position;
            }
        }

        public MeshFace Rotated(Vector3 axis, float angle)
        {
            var rotation = Quaternion.FromAxisAngle(axis, angle);

            var rotated = new MeshFace(Vertices)
            {
                Normal = rotation * Normal,
                Tangent = rotation * Tangent
            };

            for (var i = 0; i < Vertices.Count; i++)
            {
                rotated.Vertices[i].Position = rotation * Vertices[i].Position;
            }

            return rotated;
        }

        public void Scale(float amount)
        {
            if (amount <= 0.0f) throw new ArgumentOutOfRangeException("Scale amount cannot be less than or equal to zero");

            for (var i = 0; i < Vertices.Count; i++)
            {
                Vertices[i].Position *= amount;
            }
        }

        public MeshFace Scaled(float amount)
        {
            var scaled = new MeshFace(Vertices)
            {
                Normal = Normal,
                Tangent = Tangent
            };

            for (var i = 0; i < Vertices.Count; i++)
            {
                scaled.Vertices[i].Position = Vertices[i].Position * amount;
            }

            return scaled;
        }

        public static MeshFace Rectangle(float width, float height)
        {
            return new MeshFace()
            {
                Vertices = new List<Vector3>()
                {
                    new Vector3(-width / 2.0f, -height / 2.0f, 0.0f),
                    new Vector3(-width / 2.0f, height / 2.0f, 0.0f),
                    new Vector3(width / 2.0f, height / 2.0f, 0.0f),
                    new Vector3(width / 2.0f, -height / 2.0f, 0.0f)
                },
            };
        }

        /// <summary>
        /// Generate a polygon with equal angles and sidelengths
        /// </summary>
        /// <param name="nSides">The number of sides for the polygon</param>
        /// <param name="apothem">The distance from the center to the midpoint of a side</param>
        /// <returns></returns>
        public static MeshFace RegularPolygon(int nSides, float apothem)
        {
            var meshFace = new MeshFace();

            var sideLength = apothem / (float)Math.Cos(MathExtensions.PI / nSides);
            var exteriorAngle = MathExtensions.TWO_PI / nSides;
            var rotation = Quaternion.FromAxisAngle(Vector3.UnitZ, exteriorAngle);

            var direction = Vector3.UnitX;
            meshFace.Vertices.Add(new Vector3(-sideLength, -apothem, 0.0f));

            for (var i = 0; i < nSides; i++)
            {
                meshFace.Vertices.Add(meshFace.Vertices[i] + direction);
                direction = rotation * direction;
            }

            return meshFace;
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
