using OpenTK;
using SpiceEngine.Utilities;
using SpiceEngineCore.Rendering.Animations;
using SpiceEngineCore.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngine.Rendering.Meshes
{
    public class EditorModel
    {
        public EditorModel() { }
        public EditorModel(ModelMesh mesh) => Meshes.Add(mesh);

        public List<ModelMesh> Meshes { get; set; } = new List<ModelMesh>();
        public Skeleton Skeleton { get; set; }
        public UVMap UVMap
        {
            set
            {
                foreach (var mesh in Meshes)
                {
                    mesh.UVMap = value;
                }
            }
        }

        public bool HasAnimations => Skeleton != null;

        public IEnumerable<ModelBuilder> GetMeshBuilds()
        {
            foreach (var mesh in Meshes)
            {
                yield return new ModelBuilder(mesh);
            }
        }

        public static EditorModel LoadFromFile(string filePath)
        {
            using (var importer = new Assimp.AssimpContext())
            {
                var scene = importer.ImportFile(filePath, Assimp.PostProcessSteps.JoinIdenticalVertices
                    | Assimp.PostProcessSteps.CalculateTangentSpace
                    | Assimp.PostProcessSteps.LimitBoneWeights
                    | Assimp.PostProcessSteps.Triangulate
                    | Assimp.PostProcessSteps.GenerateSmoothNormals
                    | Assimp.PostProcessSteps.FlipUVs);

                var modelShape = new EditorModel();

                if (scene.HasAnimations)
                {
                    modelShape.Skeleton = new Skeleton()
                    {
                        Name = "",
                    };
                    modelShape.Skeleton.Root = scene.RootNode.ToBone();
                }

                foreach (var mesh in scene.Meshes)
                {
                    var modelMesh = new ModelMesh();

                    foreach (var face in mesh.Faces)
                    {
                        var modelFace = new ModelFace(face.Indices.Select(i => new ModelVertex()
                        {
                            Position = mesh.Vertices[i].ToVector3(),
                            Normal = mesh.HasNormals ? mesh.Normals[i].ToVector3() : new Vector3(),
                            Tangent = mesh.HasTangentBasis ? mesh.Tangents[i].ToVector3() : new Vector3(),
                            UV = mesh.HasTextureCoords(0) ? mesh.TextureCoordinateChannels[0][i].ToVector2() : new Vector2()
                        }));

                        if (mesh.HasNormals)
                        {
                            modelFace.Normal = face.Indices.Select(i => mesh.Normals[i].ToVector3()).Average();
                        }

                        if (mesh.HasTangentBasis)
                        {
                            modelFace.Tangent = face.Indices.Select(i => mesh.Tangents[i].ToVector3()).Average();
                        }

                        modelMesh.Faces.Add(modelFace);
                    }

                    modelShape.Meshes.Add(modelMesh);
                }

                /*if (scene.HasAnimations)
                {
                    foreach (var mesh in scene.Meshes)
                    {
                        for (var i = 0; i < mesh.VertexCount; i++)
                        {
                            var position = mesh.Vertices[i].ToVector3();
                            var normals = mesh.HasNormals ? mesh.Normals[i].ToVector3() : new Vector3();
                            var tangents = mesh.HasTangentBasis ? mesh.Tangents[i].ToVector3() : new Vector3();
                            var textureCoords = mesh.HasTextureCoords(0) ? mesh.TextureCoordinateChannels[0][i].ToVector2() : new Vector2();
                            var boneIDs = new Vector4();
                            var boneWeights = new Vector4();

                            if (mesh.HasBones)
                            {
                                var matches = mesh.Bones.Select((bone, boneIndex) => new { bone, boneIndex })
                                    .Where(b => b.bone.VertexWeights.Any(v => v.VertexID == i))
                                    .ToList();

                                for (var j = 0; j < 4 && j < matches.Count; j++)
                                {
                                    boneIDs[j] = matches[j].boneIndex;
                                    boneWeights[j] = matches[j].bone.VertexWeights.First(v => v.VertexID == i).Weight;
                                }
                            }

                            vertices.Add(new JointVertex3D(position, normals, tangents, textureCoords, boneIDs, boneWeights));
                        }

                        var meshShape = new MeshShape();

                        foreach (var face in mesh.Faces)
                        {
                            // TODO - Determine face Normal and Tangent
                            var meshFace = new MeshFace(face.Indices.Select(i => vertices[i]));
                            meshShape.Faces.Add(meshShape);
                        }

                        modelShape.Meshes.Add(meshShape);
                    }
                }*/

                return modelShape;
            }
        }
    }
}
