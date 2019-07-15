using OpenTK;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Rendering.Meshes
{
    // TODO - Pick a name that isn't shit
    public class ModelBuilder
    {
        public List<Vector3> Positions { get; set; } = new List<Vector3>();
        public List<Vector3> Normals { get; set; } = new List<Vector3>();
        public List<Vector3> Tangents { get; set; } = new List<Vector3>();
        public List<Vector2> UVs { get; set; } = new List<Vector2>();
        public List<Vector4> BoneIDs { get; set; } = new List<Vector4>();
        public List<Vector4> BoneWeights { get; set; } = new List<Vector4>();
        public List<int> TriangleIndices { get; set; } = new List<int>();

        public ModelBuilder(ModelMesh modelMesh)
        {
            foreach (var face in modelMesh.Faces)
            {
                AddFace(face);
            }

            Normalize();
        }

        public ModelBuilder(ModelFace modelFace)
        {
            AddFace(modelFace);
            Normalize();
        }

        public ModelBuilder(ModelTriangle modelTriangle)
        {
            var uvRotation = Quaternion.Identity;//Quaternion.FromAxisAngle(meshTriangle.Normal, meshTriangle.UVMap.Rotation);
            var uvXOrigin = 0.0f;//face.Vertices.Min(v => Vector3.Dot(uvRotation * face.Bitangent, v.Position));
            var uvYOrigin = 0.0f;//face.Vertices.Min(v => Vector3.Dot(uvRotation * face.Tangent, v.Position));
            var uvMap = UVMap.Standard;

            AddTriangle(modelTriangle, uvXOrigin, uvYOrigin, uvRotation, uvMap);
        }

        public IEnumerable<ModelVertex> GetVertices()
        {
            for (var i = 0; i < Positions.Count; i++)
            {
                yield return GetVertexAt(i);
            }
        }

        public ModelVertex GetVertexAt(int index)
        {
            var modelVertex = new ModelVertex()
            {
                Position = Positions[index],
                Normal = Normals[index],
                Tangent = Tangents[index],
                UV = UVs[index]
            };

            if (index < BoneIDs.Count && index < BoneWeights.Count)
            {
                modelVertex.BoneIDs = BoneIDs[index];
                modelVertex.BoneWeights = BoneWeights[index];
            }

            return modelVertex;
        }

        public void Normalize()
        {
            foreach (var normal in Normals)
            {
                normal.Normalize();
            }

            foreach (var tangent in Tangents)
            {
                tangent.Normalize();
            }
        }

        private void AddFace(ModelFace face)
        {
            var uvRotation = Quaternion.FromAxisAngle(face.Normal, face.UVMap.Rotation);
            var uvXOrigin = face.Vertices.Min(v => Vector3.Dot(uvRotation * face.Bitangent, v.Position));
            var uvYOrigin = face.Vertices.Min(v => Vector3.Dot(uvRotation * face.Tangent, v.Position));

            foreach (var triangle in face.GetMeshTriangles())
            {
                AddTriangle(triangle, uvXOrigin, uvYOrigin, uvRotation, face.UVMap);
            }
        }

        private void AddTriangle(ModelTriangle triangle, float uvXOrigin, float uvYOrigin, Quaternion uvRotation, UVMap uvMap)
        {
            var uvA = triangle.VertexA.UV != Vector2.Zero ? triangle.VertexA.UV : new Vector2()
            {
                X = (Vector3.Dot(uvRotation * triangle.Bitangent, triangle.VertexA.Position) - uvXOrigin + uvMap.Translation.X) / uvMap.Scale.X,
                Y = (Vector3.Dot(uvRotation * triangle.Tangent, triangle.VertexA.Position) - uvYOrigin + uvMap.Translation.Y) / uvMap.Scale.Y
            };

            var uvB = triangle.VertexB.UV != Vector2.Zero ? triangle.VertexB.UV : new Vector2()
            {
                X = (Vector3.Dot(uvRotation * triangle.Bitangent, triangle.VertexB.Position) - uvXOrigin + uvMap.Translation.X) / uvMap.Scale.X,
                Y = (Vector3.Dot(uvRotation * triangle.Tangent, triangle.VertexB.Position) - uvYOrigin + uvMap.Translation.Y) / uvMap.Scale.Y
            };

            var uvC = triangle.VertexC.UV != Vector2.Zero ? triangle.VertexC.UV : new Vector2()
            {
                X = (Vector3.Dot(uvRotation * triangle.Bitangent, triangle.VertexC.Position) - uvXOrigin + uvMap.Translation.X) / uvMap.Scale.X,
                Y = (Vector3.Dot(uvRotation * triangle.Tangent, triangle.VertexC.Position) - uvYOrigin + uvMap.Translation.Y) / uvMap.Scale.Y
            };

            var indexA = AddVertex(triangle.VertexA.Position, triangle.Normal, triangle.Tangent, uvA);
            var indexB = AddVertex(triangle.VertexB.Position, triangle.Normal, triangle.Tangent, uvB);
            var indexC = AddVertex(triangle.VertexC.Position, triangle.Normal, triangle.Tangent, uvC);

            // Be mindful of wind-order here
            TriangleIndices.Add(indexC);//indexA);
            TriangleIndices.Add(indexB);
            TriangleIndices.Add(indexA);//indexC);
        }

        private int AddVertex(Vector3 position, Vector3 normal, Vector3 tangent, Vector2 uv)
        {
            var index = -1;
            for (var i = 0; i < Positions.Count; i++)
            {
                if (Positions[i] == position && UVs[i] == uv)
                {
                    index = i;
                }
            }

            if (index >= 0)
            {
                Normals[index] += normal;
                Tangents[index] += tangent;
            }
            else
            {
                index = Positions.Count;

                Positions.Add(position);
                Normals.Add(normal);
                Tangents.Add(tangent);
                UVs.Add(uv);
            }

            return index;
        }

        /*for (var i = 0; i < vertexIndices.Count; i++)
        {
            // Grab vertexIndices, three at a time, to form each triangle
            if (i % 3 == 0)
            {
                // For a given triangle with vertex positions P0, P1, P2 and corresponding UV texture coordinates T0, T1, and T2:
                // deltaPos1 = P1 - P0;
                // delgaPos2 = P2 - P0;
                // deltaUv1 = T1 - T0;
                // deltaUv2 = T2 - T0;
                // r = 1 / (deltaUv1.x * deltaUv2.y - deltaUv1.y - deltaUv2.x);
                // tangent = (deltaPos1 * deltaUv2.y - deltaPos2 * deltaUv1.y) * r;
                var deltaPos1 = vertices[vertexIndices[i + 1]] - vertices[vertexIndices[i]];
                var deltaPos2 = vertices[vertexIndices[i + 2]] - vertices[vertexIndices[i]];
                var deltaUV1 = uvs[uvIndices[i + 1]] - uvs[uvIndices[i]];
                var deltaUV2 = uvs[uvIndices[i + 1]] - uvs[uvIndices[i]];

                var r = 1.0f / (deltaUV1.X * deltaUV2.Y - deltaUV1.Y - deltaUV2.X);
                tangent = (deltaPos1 * deltaUV2.Y - deltaPos2 * deltaUV1.Y) * r;
            }

            var uv = uvIndices[i] > 0 ? uvs[uvIndices[i]] : Vector2.Zero;

            var vertex = new Vertex(vertices[vertexIndices[i]], normals[normalIndices[i]], tangent.Normalized(), uv, materialIndices[i]);
            var existingIndex = verticies.FindIndex(v => v.Position == vertex.Position
                && v.Normal == vertex.Normal
                && v.TextureCoords == vertex.TextureCoords
                && v.MaterialIndex == vertex.MaterialIndex);

            if (existingIndex >= 0)
            {
                triangleIndices.Add(existingIndex);
            }
            else
            {
                triangleIndices.Add(verticies.Count);
                verticies.Add(vertex);
            }
        }*/
    }
}
