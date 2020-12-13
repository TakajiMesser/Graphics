using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Utilities;
using SweetGraphicsCore.Rendering.Meshes;
using System;
using System.Collections.Generic;
using System.Linq;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SweetGraphicsCore.Rendering.Models
{
    public class ModelFace : IModelShape, ITexturedShape
    {
        /// <summary>
        /// The vertices should be in clockwise order.
        /// </summary>
        public List<ModelVertex> Vertices { get; set; } = new List<ModelVertex>();
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
        public float UVXOrigin => Vertices.Min(v => Vector3.Dot(new Quaternion(Normal, UVMap.Rotation) * Bitangent, v.Position));
        public float UVYOrigin => Vertices.Min(v => Vector3.Dot(new Quaternion(Normal, UVMap.Rotation) * Tangent, v.Position));

        public ModelFace() { }
        public ModelFace(params ModelVertex[] vertices) : this(LINQExtensions.Generate(vertices)) { }
        public ModelFace(IEnumerable<Vector3> vertices) : this(vertices.Select(v => new ModelVertex() { Position = v })) { }
        public ModelFace(IEnumerable<ModelVertex> vertices) => Vertices.AddRange(vertices);

        public ModelFace Duplicated() => new ModelFace()
        {
            Vertices = Vertices.Select(v => v.Duplicated()).ToList(),
            UVMap = UVMap.Duplicated(),
            _normal = _normal,
            _tangent = _tangent
        };

        public void AddVertex(ModelVertex vertex) => Vertices.Add(vertex);
        public void AddVertex(Vector3 vertex) => Vertices.Add(new ModelVertex() { Position = vertex });

        public Vector3 GetAveragePosition() => Vertices.Select(v => v.GetAveragePosition()).Average();

        public void CenterAround(Vector3 position)
        {
            foreach (var vertex in Vertices)
            {
                vertex.CenterAround(position);
            }
        }

        public IEnumerable<ModelTriangle> GetTriangles()
        {
            if (Vertices.Count < 3) throw new InvalidOperationException("MeshFace must consist of at least 3 vertices");

            for (var i = 1; i < Vertices.Count - 1; i++)
            {
                var vertexA = Vertices[0];
                var vertexB = Vertices[i];
                var vertexC = Vertices[i + 1];

                yield return new ModelTriangle(vertexA, vertexB, vertexC)
                {
                    Normal = Normal,
                    Tangent = Tangent
                };
            }
        }

        public void Transform(Transform transform)
        {
            foreach (var vertex in Vertices)
            {
                vertex.Transform(transform);
            }
        }

        public void Translate(Vector3 translation)
        {
            foreach (var vertex in Vertices)
            {
                vertex.Translate(translation);
            }
        }

        public ModelFace Translated(float x, float y, float z)
        {
            var translated = new ModelFace(Vertices)
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

        public void Rotate(Quaternion rotation)
        {
            foreach (var vertex in Vertices)
            {
                vertex.Rotate(rotation);
            }

            // Center is the absolute position of the center of this entity
            // Vertex.Origin is the absolute position that this vertex's position is based off of thinking is the origin (0, 0, 0)

            //var center = GetAveragePosition();

            for (var i = 0; i < Vertices.Count; i++)
            {
                /*var initialPosition = Vertices[i].Position;
                var centeredPosition = initialPosition - center;
                var rotatedPosition = rotation * centeredPosition;
                var movedPosition = rotatedPosition + center;
                Vertices[i].Position = movedPosition;*/
                //Vertices[i].Position = (rotation * (Vertices[i].Position - center)) + center;
                //Vertices[i].Position = rotation * Vertices[i].Position;
            }
        }

        public ModelFace Rotated(Vector3 axis, float angle)
        {
            var rotation = new Quaternion(axis, angle);

            var rotated = new ModelFace(Vertices)
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

        public void Scale(Vector3 scale)
        {
            var center = GetAveragePosition();

            for (var i = 0; i < Vertices.Count; i++)
            {
                Vertices[i].Position = (Vertices[i].Position - center) * scale;
            }
        }

        public ModelFace Scaled(float amount)
        {
            var scaled = new ModelFace(Vertices)
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

        public void TranslateTexture(float x, float y)
        {
            for (var i = 0; i < Vertices.Count; i++)
            {
                Vertices[i].TranslateTexture(new Vector2(x, y));
            }
        }

        public void RotateTexture(float angle)
        {
            var center = GetAveragePosition();

            for (var i = 0; i < Vertices.Count; i++)
            {
                Vertices[i].RotateTexture(center, angle);
            }
        }

        public void ScaleTexture(float x, float y)
        {
            for (var i = 0; i < Vertices.Count; i++)
            {
                Vertices[i].ScaleTexture(x, y);
            }
        }

        public static ModelFace Rectangle(float width, float height)
        {
            return new ModelFace(new List<Vector3>()
            {
                new Vector3(-width / 2.0f, -height / 2.0f, 0.0f),
                new Vector3(-width / 2.0f, height / 2.0f, 0.0f),
                new Vector3(width / 2.0f, height / 2.0f, 0.0f),
                new Vector3(width / 2.0f, -height / 2.0f, 0.0f)
            });
        }

        /// <summary>
        /// Generate a polygon with equal angles and sidelengths
        /// </summary>
        /// <param name="nSides">The number of sides for the polygon</param>
        /// <param name="apothem">The distance from the center to the midpoint of a side</param>
        /// <returns></returns>
        public static ModelFace RegularPolygon(int nSides, float apothem)
        {
            var vertices = new List<Vector3>();

            var sideLength = apothem / (float)Math.Cos(MathExtensions.PI / nSides);
            var exteriorAngle = MathExtensions.TWO_PI / nSides;
            var rotation = new Quaternion(Vector3.UnitZ, -exteriorAngle);

            var direction = -Vector3.UnitX;
            vertices.Add(new Vector3(sideLength / 2.0f, -apothem, 0.0f));

            for (var i = 0; i < nSides - 1; i++)
            {
                vertices.Add(vertices[i] + sideLength * direction);
                direction = rotation * direction;
            }

            return new ModelFace(vertices);
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
