using OpenTK;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Meshes;
using SpiceEngineCore.Rendering.Models;
using SpiceEngineCore.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngineCore.Rendering.Models
{
    public class ModelMesh : IModelShape, ITexturedShape
    {
        public List<ModelFace> Faces { get; set; } = new List<ModelFace>();
        public UVMap UVMap
        {
            set
            {
                foreach (var face in Faces)
                {
                    face.UVMap = value;
                }
            }
        }

        public ModelMesh Duplicated() => new ModelMesh()
        {
            Faces = Faces.Select(f => f.Duplicated()).ToList()
        };

        public void Transform(Transform transform)
        {
            foreach (var face in Faces)
            {
                face.Transform(transform);
            }
        }

        public void Translate(Vector3 translation)
        {
            foreach (var face in Faces)
            {
                face.Translate(translation);
            }
        }

        public void Rotate(Quaternion rotation)
        {
            foreach (var face in Faces)
            {
                face.Rotate(rotation);
            }
        }

        public void Scale(Vector3 scale)
        {
            foreach (var face in Faces)
            {
                face.Scale(scale);
            }
        }

        public void TranslateTexture(float x, float y)
        {
            foreach (var face in Faces)
            {
                face.TranslateTexture(x, y);
            }
        }

        public void RotateTexture(float angle)
        {
            foreach (var face in Faces)
            {
                face.RotateTexture(angle);
            }
        }
        public void ScaleTexture(float x, float y)
        {
            foreach (var face in Faces)
            {
                face.ScaleTexture(x, y);
            }
        }

        public Vector3 GetAveragePosition()
        {
            var xSum = 0.0f;
            var ySum = 0.0f;
            var zSum = 0.0f;
            var nVertices = 0;

            foreach (var face in Faces)
            {
                foreach (var vertex in face.Vertices)
                {
                    xSum += vertex.Position.X + vertex.Origin.X;
                    ySum += vertex.Position.Y + vertex.Origin.Y;
                    zSum += vertex.Position.Z + vertex.Origin.Z;
                    nVertices++;
                }
            }

            return new Vector3()
            {
                X = xSum / nVertices,
                Y = ySum / nVertices,
                Z = zSum / nVertices
            };
        }

        public void CenterAround(Vector3 position)
        {
            foreach (var face in Faces)
            {
                face.CenterAround(position);
            }
        }

        public static ModelMesh Box(float width, float height, float depth)
        {
            var shape = new ModelMesh();

            // +X Face
            shape.Faces.Add(ModelFace.Rectangle(height, depth)
                .Rotated(Vector3.UnitZ, MathExtensions.HALF_PI)
                .Translated(0.0f, 0.0f, width / 2.0f)
                .Rotated(Vector3.UnitY, MathExtensions.HALF_PI));

            // -X Face
            shape.Faces.Add(ModelFace.Rectangle(height, depth)
                .Rotated(Vector3.UnitZ, -MathExtensions.HALF_PI)
                .Translated(0.0f, 0.0f, width / 2.0f)
                .Rotated(Vector3.UnitY, -MathExtensions.HALF_PI));

            // +Y Face
            shape.Faces.Add(ModelFace.Rectangle(width, depth)
                .Rotated(Vector3.UnitZ, MathExtensions.PI)
                .Translated(0.0f, 0.0f, height / 2.0f)
                .Rotated(Vector3.UnitX, -MathExtensions.HALF_PI));

            // -Y Face
            shape.Faces.Add(ModelFace.Rectangle(width, depth)
                .Translated(0.0f, 0.0f, height / 2.0f)
                .Rotated(Vector3.UnitX, MathExtensions.HALF_PI));

            // +Z Face
            shape.Faces.Add(ModelFace.Rectangle(width, height)
                .Translated(0.0f, 0.0f, depth / 2.0f));

            // -Z Face
            shape.Faces.Add(ModelFace.Rectangle(width, height)
                .Translated(0.0f, 0.0f, depth / 2.0f)
                .Rotated(Vector3.UnitY, MathExtensions.PI));

            return shape;
        }

        public static ModelMesh Cone(float radius, float height, int nSides)
        {
            var shape = new ModelMesh();

            return shape;
        }

        public static ModelMesh Cylinder(float radius, float height, int nSides)
        {
            var shape = new ModelMesh();

            return shape;
        }

        public static ModelMesh Sphere(float radius, int nSides)
        {
            var shape = new ModelMesh();

            return shape;
        }

        /// <summary>
        /// Generate a polygon with equal angles and sidelengths
        /// </summary>
        /// <param name="nSides">The number of sides for the polygon</param>
        /// <param name="apothem">The distance from the center to the midpoint of a side</param>
        /// <returns></returns>
        public static ModelMesh RegularPolyhedron(int nSides, float apothem)
        {
            var meshShape = new ModelMesh();

            var meshFace = new ModelFace();

            var sideLength = apothem / (float)Math.Cos(MathExtensions.PI / nSides);
            var exteriorAngle = MathExtensions.TWO_PI / nSides;
            var rotation = Quaternion.FromAxisAngle(Vector3.UnitZ, exteriorAngle);

            var direction = Vector3.UnitX;
            meshFace.AddVertex(new Vector3(-sideLength, -apothem, 0.0f));

            for (var i = 0; i < nSides; i++)
            {
                meshFace.AddVertex(meshFace.Vertices[i].Position + direction);
                direction = rotation * direction;
            }

            return meshShape;
        }

        /*public static MeshShape Box(float width, float height, float depth) => new MeshShape()
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
        };*/
    }

    /*public class MeshShape
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

        public static MeshShape Box(float width, float height, float depth) => new MeshShape()
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

        public static MeshShape Circle(float radius) => new MeshShape();

        public static MeshShape Sphere(float radius) => new MeshShape()
        {
            Vertices = new List<Vector3>
            {
                new Vector3(radius * 0.000000f, radius * -1.000000f, radius * 0.000000f),
                new Vector3(radius * 0.723607f, radius * -0.447220f, radius * 0.525725f),
                new Vector3(radius * -0.276388f, radius * -0.447220f, radius * 0.850649f),
                new Vector3(radius * -0.894426f, radius * -0.447216f, radius * 0.000000f),
                new Vector3(radius * -0.276388f, radius * -0.447220f, radius * -0.850649f),
                new Vector3(radius * 0.723607f, radius * -0.447220f, radius * -0.525725f),
                new Vector3(radius * 0.276388f, radius * 0.447220f, radius * 0.850649f),
                new Vector3(radius * -0.723607f, radius * 0.447220f, radius * 0.525725f),
                new Vector3(radius * -0.723607f, radius * 0.447220f, radius * -0.525725f),
                new Vector3(radius * 0.276388f, radius * 0.447220f, radius * -0.850649f),
                new Vector3(radius * 0.894426f, radius * 0.447216f, radius * 0.000000f),
                new Vector3(radius * 0.000000f, radius * 1.000000f, radius * 0.000000f),
                new Vector3(radius * -0.232822f, radius * -0.657519f, radius * 0.716563f),
                new Vector3(radius * -0.162456f, radius * -0.850654f, radius * 0.499995f),
                new Vector3(radius * -0.077607f, radius * -0.967950f, radius * 0.238853f),
                new Vector3(radius * 0.203181f, radius * -0.967950f, radius * 0.147618f),
                new Vector3(radius * 0.425323f, radius * -0.850654f, radius * 0.309011f),
                new Vector3(radius * 0.609547f, radius * -0.657519f, radius * 0.442856f),
                new Vector3(radius * 0.531941f, radius * -0.502302f, radius * 0.681712f),
                new Vector3(radius * 0.262869f, radius * -0.525738f, radius * 0.809012f),
                new Vector3(radius * -0.029639f, radius * -0.502302f, radius * 0.864184f),
                new Vector3(radius * 0.812729f, radius * -0.502301f, radius * -0.295238f),
                new Vector3(radius * 0.850648f, radius * -0.525736f, radius * 0.000000f),
                new Vector3(radius * 0.812729f, radius * -0.502301f, radius * 0.295238f),
                new Vector3(radius * 0.203181f, radius * -0.967950f, radius * -0.147618f),
                new Vector3(radius * 0.425323f, radius * -0.850654f, radius * -0.309011f),
                new Vector3(radius * 0.609547f, radius * -0.657519f, radius * -0.442856f),
                new Vector3(radius * -0.753442f, radius * -0.657515f, radius * 0.000000f),
                new Vector3(radius * -0.525730f, radius * -0.850652f, radius * 0.000000f),
                new Vector3(radius * -0.251147f, radius * -0.967949f, radius * 0.000000f),
                new Vector3(radius * -0.483971f, radius * -0.502302f, radius * 0.716565f),
                new Vector3(radius * -0.688189f, radius * -0.525736f, radius * 0.499997f),
                new Vector3(radius * -0.831051f, radius * -0.502299f, radius * 0.238853f),
                new Vector3(radius * -0.232822f, radius * -0.657519f, radius * -0.716563f),
                new Vector3(radius * -0.162456f, radius * -0.850654f, radius * -0.499995f),
                new Vector3(radius * -0.077607f, radius * -0.967950f, radius * -0.238853f),
                new Vector3(radius * -0.831051f, radius * -0.502299f, radius * -0.238853f),
                new Vector3(radius * -0.688189f, radius * -0.525736f, radius * -0.499997f),
                new Vector3(radius * -0.483971f, radius * -0.502302f, radius * -0.716565f),
                new Vector3(radius * -0.029639f, radius * -0.502302f, radius * -0.864184f),
                new Vector3(radius * 0.262869f, radius * -0.525738f, radius * -0.809012f),
                new Vector3(radius * 0.531941f, radius * -0.502302f, radius * -0.681712f),
                new Vector3(radius * 0.956626f, radius * 0.251149f, radius * 0.147618f),
                new Vector3(radius * 0.951058f, radius * -0.000000f, radius * 0.309013f),
                new Vector3(radius * 0.860698f, radius * -0.251151f, radius * 0.442858f),
                new Vector3(radius * 0.860698f, radius * -0.251151f, radius * -0.442858f),
                new Vector3(radius * 0.951058f, radius * 0.000000f, radius * -0.309013f),
                new Vector3(radius * 0.956626f, radius * 0.251149f, radius * -0.147618f),
                new Vector3(radius * 0.155215f, radius * 0.251152f, radius * 0.955422f),
                new Vector3(radius * 0.000000f, radius * -0.000000f, radius * 1.000000f),
                new Vector3(radius * -0.155215f, radius * -0.251152f, radius * 0.955422f),
                new Vector3(radius * 0.687159f, radius * -0.251152f, radius * 0.681715f),
                new Vector3(radius * 0.587786f, radius * 0.000000f, radius * 0.809017f),
                new Vector3(radius * 0.436007f, radius * 0.251152f, radius * 0.864188f),
                new Vector3(radius * -0.860698f, radius * 0.251151f, radius * 0.442858f),
                new Vector3(radius * -0.951058f, radius * -0.000000f, radius * 0.309013f),
                new Vector3(radius * -0.956626f, radius * -0.251149f, radius * 0.147618f),
                new Vector3(radius * -0.436007f, radius * -0.251152f, radius * 0.864188f),
                new Vector3(radius * -0.587786f, radius * 0.000000f, radius * 0.809017f),
                new Vector3(radius * -0.687159f, radius * 0.251152f, radius * 0.681715f),
                new Vector3(radius * -0.687159f, radius * 0.251152f, radius * -0.681715f),
                new Vector3(radius * -0.587786f, radius * -0.000000f, radius * -0.809017f),
                new Vector3(radius * -0.436007f, radius * -0.251152f, radius * -0.864188f),
                new Vector3(radius * -0.956626f, radius * -0.251149f, radius * -0.147618f),
                new Vector3(radius * -0.951058f, radius * 0.000000f, radius * -0.309013f),
                new Vector3(radius * -0.860698f, radius * 0.251151f, radius * -0.442858f),
                new Vector3(radius * 0.436007f, radius * 0.251152f, radius * -0.864188f),
                new Vector3(radius * 0.587786f, radius * -0.000000f, radius * -0.809017f),
                new Vector3(radius * 0.687159f, radius * -0.251152f, radius * -0.681715f),
                new Vector3(radius * -0.155215f, radius * -0.251152f, radius * -0.955422f),
                new Vector3(radius * 0.000000f, radius * 0.000000f, radius * -1.000000f),
                new Vector3(radius * 0.155215f, radius * 0.251152f, radius * -0.955422f),
                new Vector3(radius * 0.831051f, radius * 0.502299f, radius * 0.238853f),
                new Vector3(radius * 0.688189f, radius * 0.525736f, radius * 0.499997f),
                new Vector3(radius * 0.483971f, radius * 0.502302f, radius * 0.716565f),
                new Vector3(radius * 0.029639f, radius * 0.502302f, radius * 0.864184f),
                new Vector3(radius * -0.262869f, radius * 0.525738f, radius * 0.809012f),
                new Vector3(radius * -0.531941f, radius * 0.502302f, radius * 0.681712f),
                new Vector3(radius * -0.812729f, radius * 0.502301f, radius * 0.295238f),
                new Vector3(radius * -0.850648f, radius * 0.525736f, radius * 0.000000f),
                new Vector3(radius * -0.812729f, radius * 0.502301f, radius * -0.295238f),
                new Vector3(radius * -0.531941f, radius * 0.502302f, radius * -0.681712f),
                new Vector3(radius * -0.262869f, radius * 0.525738f, radius * -0.809012f),
                new Vector3(radius * 0.029639f, radius * 0.502302f, radius * -0.864184f),
                new Vector3(radius * 0.483971f, radius * 0.502302f, radius * -0.716565f),
                new Vector3(radius * 0.688189f, radius * 0.525736f, radius * -0.499997f),
                new Vector3(radius * 0.831051f, radius * 0.502299f, radius * -0.238853f),
                new Vector3(radius * 0.077607f, radius * 0.967950f, radius * 0.238853f),
                new Vector3(radius * 0.162456f, radius * 0.850654f, radius * 0.499995f),
                new Vector3(radius * 0.232822f, radius * 0.657519f, radius * 0.716563f),
                new Vector3(radius * 0.753442f, radius * 0.657515f, radius * 0.000000f),
                new Vector3(radius * 0.525730f, radius * 0.850652f, radius * 0.000000f),
                new Vector3(radius * 0.251147f, radius * 0.967949f, radius * 0.000000f),
                new Vector3(radius * -0.203181f, radius * 0.967950f, radius * 0.147618f),
                new Vector3(radius * -0.425323f, radius * 0.850654f, radius * 0.309011f),
                new Vector3(radius * -0.609547f, radius * 0.657519f, radius * 0.442856f),
                new Vector3(radius * -0.203181f, radius * 0.967950f, radius * -0.147618f),
                new Vector3(radius * -0.425323f, radius * 0.850654f, radius * -0.309011f),
                new Vector3(radius * -0.609547f, radius * 0.657519f, radius * -0.442856f),
                new Vector3(radius * 0.077607f, radius * 0.967950f, radius * -0.238853f),
                new Vector3(radius * 0.162456f, radius * 0.850654f, radius * -0.499995f),
                new Vector3(radius * 0.232822f, radius * 0.657519f, radius * -0.716563f),
                new Vector3(radius * 0.361800f, radius * 0.894429f, radius * -0.262863f),
                new Vector3(radius * 0.638194f, radius * 0.723610f, radius * -0.262864f),
                new Vector3(radius * 0.447209f, radius * 0.723612f, radius * -0.525728f),
                new Vector3(radius * -0.138197f, radius * 0.894430f, radius * -0.425319f),
                new Vector3(radius * -0.052790f, radius * 0.723612f, radius * -0.688185f),
                new Vector3(radius * -0.361804f, radius * 0.723612f, radius * -0.587778f),
                new Vector3(radius * -0.447210f, radius * 0.894429f, radius * 0.000000f),
                new Vector3(radius * -0.670817f, radius * 0.723611f, radius * -0.162457f),
                new Vector3(radius * -0.670817f, radius * 0.723611f, radius * 0.162457f),
                new Vector3(radius * -0.138197f, radius * 0.894430f, radius * 0.425319f),
                new Vector3(radius * -0.361804f, radius * 0.723612f, radius * 0.587778f),
                new Vector3(radius * -0.052790f, radius * 0.723612f, radius * 0.688185f),
                new Vector3(radius * 0.361800f, radius * 0.894429f, radius * 0.262863f),
                new Vector3(radius * 0.447209f, radius * 0.723612f, radius * 0.525728f),
                new Vector3(radius * 0.638194f, radius * 0.723610f, radius * 0.262864f),
                new Vector3(radius * 0.861804f, radius * 0.276396f, radius * -0.425322f),
                new Vector3(radius * 0.809019f, radius * 0.000000f, radius * -0.587782f),
                new Vector3(radius * 0.670821f, radius * 0.276397f, radius * -0.688189f),
                new Vector3(radius * -0.138199f, radius * 0.276397f, radius * -0.951055f),
                new Vector3(radius * -0.309016f, radius * -0.000000f, radius * -0.951057f),
                new Vector3(radius * -0.447215f, radius * 0.276397f, radius * -0.850649f),
                new Vector3(radius * -0.947213f, radius * 0.276396f, radius * -0.162458f),
                new Vector3(radius * -1.000000f, radius * 0.000001f, radius * 0.000000f),
                new Vector3(radius * -0.947213f, radius * 0.276397f, radius * 0.162458f),
                new Vector3(radius * -0.447216f, radius * 0.276397f, radius * 0.850648f),
                new Vector3(radius * -0.309017f, radius * -0.000001f, radius * 0.951056f),
                new Vector3(radius * -0.138199f, radius * 0.276397f, radius * 0.951055f),
                new Vector3(radius * 0.670820f, radius * 0.276396f, radius * 0.688190f),
                new Vector3(radius * 0.809019f, radius * -0.000002f, radius * 0.587783f),
                new Vector3(radius * 0.861804f, radius * 0.276394f, radius * 0.425323f),
                new Vector3(radius * 0.309017f, radius * -0.000000f, radius * -0.951056f),
                new Vector3(radius * 0.447216f, radius * -0.276398f, radius * -0.850648f),
                new Vector3(radius * 0.138199f, radius * -0.276398f, radius * -0.951055f),
                new Vector3(radius * -0.809018f, radius * -0.000000f, radius * -0.587783f),
                new Vector3(radius * -0.670819f, radius * -0.276397f, radius * -0.688191f),
                new Vector3(radius * -0.861803f, radius * -0.276396f, radius * -0.425324f),
                new Vector3(radius * -0.809018f, radius * 0.000000f, radius * 0.587783f),
                new Vector3(radius * -0.861803f, radius * -0.276396f, radius * 0.425324f),
                new Vector3(radius * -0.670819f, radius * -0.276397f, radius * 0.688191f),
                new Vector3(radius * 0.309017f, radius * 0.000000f, radius * 0.951056f),
                new Vector3(radius * 0.138199f, radius * -0.276398f, radius * 0.951055f),
                new Vector3(radius * 0.447216f, radius * -0.276398f, radius * 0.850648f),
                new Vector3(radius * 1.000000f, radius * 0.000000f, radius * 0.000000f),
                new Vector3(radius * 0.947213f, radius * -0.276396f, radius * 0.162458f),
                new Vector3(radius * 0.947213f, radius * -0.276396f, radius * -0.162458f),
                new Vector3(radius * 0.361803f, radius * -0.723612f, radius * -0.587779f),
                new Vector3(radius * 0.138197f, radius * -0.894429f, radius * -0.425321f),
                new Vector3(radius * 0.052789f, radius * -0.723611f, radius * -0.688186f),
                new Vector3(radius * -0.447211f, radius * -0.723612f, radius * -0.525727f),
                new Vector3(radius * -0.361801f, radius * -0.894429f, radius * -0.262863f),
                new Vector3(radius * -0.638195f, radius * -0.723609f, radius * -0.262863f),
                new Vector3(radius * -0.638195f, radius * -0.723609f, radius * 0.262864f),
                new Vector3(radius * -0.361801f, radius * -0.894428f, radius * 0.262864f),
                new Vector3(radius * -0.447211f, radius * -0.723610f, radius * 0.525729f),
                new Vector3(radius * 0.670817f, radius * -0.723611f, radius * -0.162457f),
                new Vector3(radius * 0.670818f, radius * -0.723610f, radius * 0.162458f),
                new Vector3(radius * 0.447211f, radius * -0.894428f, radius * 0.000001f),
                new Vector3(radius * 0.052790f, radius * -0.723612f, radius * 0.688185f),
                new Vector3(radius * 0.138199f, radius * -0.894429f, radius * 0.425321f),
                new Vector3(radius * 0.361805f, radius * -0.723611f, radius * 0.587779f)
            },
            TriangleIndices = new List<int>
            {
                1, 16, 15,
                2, 18, 24,
                1, 15, 30,
                1, 30, 36,
                1, 36, 25,
                2, 24, 45,
                3, 21, 51,
                4, 33, 57,
                5, 39, 63,
                6, 42, 69,
                2, 45, 52,
                3, 51, 58,
                4, 57, 64,
                5, 63, 70,
                6, 69, 46,
                7, 75, 90,
                8, 78, 96,
                9, 81, 99,
                10, 84, 102,
                11, 87, 91,
                93, 100, 12,
                92, 103, 93,
                91, 104, 92,
                93, 103, 100,
                103, 101, 100,
                92, 104, 103,
                104, 105, 103,
                103, 105, 101,
                105, 102, 101,
                91, 87, 104,
                87, 86, 104,
                104, 86, 105,
                86, 85, 105,
                105, 85, 102,
                85, 10, 102,
                100, 97, 12,
                101, 106, 100,
                102, 107, 101,
                100, 106, 97,
                106, 98, 97,
                101, 107, 106,
                107, 108, 106,
                106, 108, 98,
                108, 99, 98,
                102, 84, 107,
                84, 83, 107,
                107, 83, 108,
                83, 82, 108,
                108, 82, 99,
                82, 9, 99,
                97, 94, 12,
                98, 109, 97,
                99, 110, 98,
                97, 109, 94,
                109, 95, 94,
                98, 110, 109,
                110, 111, 109,
                109, 111, 95,
                111, 96, 95,
                99, 81, 110,
                81, 80, 110,
                110, 80, 111,
                80, 79, 111,
                111, 79, 96,
                79, 8, 96,
                94, 88, 12,
                95, 112, 94,
                96, 113, 95,
                94, 112, 88,
                112, 89, 88,
                95, 113, 112,
                113, 114, 112,
                112, 114, 89,
                114, 90, 89,
                96, 78, 113,
                78, 77, 113,
                113, 77, 114,
                77, 76, 114,
                114, 76, 90,
                76, 7, 90,
                88, 93, 12,
                89, 115, 88,
                90, 116, 89,
                88, 115, 93,
                115, 92, 93,
                89, 116, 115,
                116, 117, 115,
                115, 117, 92,
                117, 91, 92,
                90, 75, 116,
                75, 74, 116,
                116, 74, 117,
                74, 73, 117,
                117, 73, 91,
                73, 11, 91,
                48, 87, 11,
                47, 118, 48,
                46, 119, 47,
                48, 118, 87,
                118, 86, 87,
                47, 119, 118,
                119, 120, 118,
                118, 120, 86,
                120, 85, 86,
                46, 69, 119,
                69, 68, 119,
                119, 68, 120,
                68, 67, 120,
                120, 67, 85,
                67, 10, 85,
                72, 84, 10,
                71, 121, 72,
                70, 122, 71,
                72, 121, 84,
                121, 83, 84,
                71, 122, 121,
                122, 123, 121,
                121, 123, 83,
                123, 82, 83,
                70, 63, 122,
                63, 62, 122,
                122, 62, 123,
                62, 61, 123,
                123, 61, 82,
                61, 9, 82,
                66, 81, 9,
                65, 124, 66,
                64, 125, 65,
                66, 124, 81,
                124, 80, 81,
                65, 125, 124,
                125, 126, 124,
                124, 126, 80,
                126, 79, 80,
                64, 57, 125,
                57, 56, 125,
                125, 56, 126,
                56, 55, 126,
                126, 55, 79,
                55, 8, 79,
                60, 78, 8,
                59, 127, 60,
                58, 128, 59,
                60, 127, 78,
                127, 77, 78,
                59, 128, 127,
                128, 129, 127,
                127, 129, 77,
                129, 76, 77,
                58, 51, 128,
                51, 50, 128,
                128, 50, 129,
                50, 49, 129,
                129, 49, 76,
                49, 7, 76,
                54, 75, 7,
                53, 130, 54,
                52, 131, 53,
                54, 130, 75,
                130, 74, 75,
                53, 131, 130,
                131, 132, 130,
                130, 132, 74,
                132, 73, 74,
                52, 45, 131,
                45, 44, 131,
                131, 44, 132,
                44, 43, 132,
                132, 43, 73,
                43, 11, 73,
                67, 72, 10,
                68, 133, 67,
                69, 134, 68,
                67, 133, 72,
                133, 71, 72,
                68, 134, 133,
                134, 135, 133,
                133, 135, 71,
                135, 70, 71,
                69, 42, 134,
                42, 41, 134,
                134, 41, 135,
                41, 40, 135,
                135, 40, 70,
                40, 5, 70,
                61, 66, 9,
                62, 136, 61,
                63, 137, 62,
                61, 136, 66,
                136, 65, 66,
                62, 137, 136,
                137, 138, 136,
                136, 138, 65,
                138, 64, 65,
                63, 39, 137,
                39, 38, 137,
                137, 38, 138,
                38, 37, 138,
                138, 37, 64,
                37, 4, 64,
                55, 60, 8,
                56, 139, 55,
                57, 140, 56,
                55, 139, 60,
                139, 59, 60,
                56, 140, 139,
                140, 141, 139,
                139, 141, 59,
                141, 58, 59,
                57, 33, 140,
                33, 32, 140,
                140, 32, 141,
                32, 31, 141,
                141, 31, 58,
                31, 3, 58,
                49, 54, 7,
                50, 142, 49,
                51, 143, 50,
                49, 142, 54,
                142, 53, 54,
                50, 143, 142,
                143, 144, 142,
                142, 144, 53,
                144, 52, 53,
                51, 21, 143,
                21, 20, 143,
                143, 20, 144,
                20, 19, 144,
                144, 19, 52,
                19, 2, 52,
                43, 48, 11,
                44, 145, 43,
                45, 146, 44,
                43, 145, 48,
                145, 47, 48,
                44, 146, 145,
                146, 147, 145,
                145, 147, 47,
                147, 46, 47,
                45, 24, 146,
                24, 23, 146,
                146, 23, 147,
                23, 22, 147,
                147, 22, 46,
                22, 6, 46,
                27, 42, 6,
                26, 148, 27,
                25, 149, 26,
                27, 148, 42,
                148, 41, 42,
                26, 149, 148,
                149, 150, 148,
                148, 150, 41,
                150, 40, 41,
                25, 36, 149,
                36, 35, 149,
                149, 35, 150,
                35, 34, 150,
                150, 34, 40,
                34, 5, 40,
                34, 39, 5,
                35, 151, 34,
                36, 152, 35,
                34, 151, 39,
                151, 38, 39,
                35, 152, 151,
                152, 153, 151,
                151, 153, 38,
                153, 37, 38,
                36, 30, 152,
                30, 29, 152,
                152, 29, 153,
                29, 28, 153,
                153, 28, 37,
                28, 4, 37,
                28, 33, 4,
                29, 154, 28,
                30, 155, 29,
                28, 154, 33,
                154, 32, 33,
                29, 155, 154,
                155, 156, 154,
                154, 156, 32,
                156, 31, 32,
                30, 15, 155,
                15, 14, 155,
                155, 14, 156,
                14, 13, 156,
                156, 13, 31,
                13, 3, 31,
                22, 27, 6,
                23, 157, 22,
                24, 158, 23,
                22, 157, 27,
                157, 26, 27,
                23, 158, 157,
                158, 159, 157,
                157, 159, 26,
                159, 25, 26,
                24, 18, 158,
                18, 17, 158,
                158, 17, 159,
                17, 16, 159,
                159, 16, 25,
                16, 1, 25,
                13, 21, 3,
                14, 160, 13,
                15, 161, 14,
                13, 160, 21,
                160, 20, 21,
                14, 161, 160,
                161, 162, 160,
                160, 162, 20,
                162, 19, 20,
                15, 16, 161,
                16, 17, 161,
                161, 17, 162,
                17, 18, 162,
                162, 18, 19,
                18, 2, 19
            }
        };

        public static MeshShape Cylinder(float radius, float height) => new MeshShape();

        public static MeshShape Cone(float radius, float height) => new MeshShape();
    }*/
}
