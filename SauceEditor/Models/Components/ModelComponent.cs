using OpenTK;
using SauceEditor.Models.Entities;
using SpiceEngine.Maps;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Vertices;
using SpiceEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SauceEditor.Models.Components
{
    public class ModelComponent : Component//, IGameViewable
    {
        public MapActor Model { get; set; }

        public IEnumerable<FaceEntity> GetShapeEntities() => throw new NotImplementedException();
        public IEnumerable<FaceEntity> GetFaceEntities() => throw new NotImplementedException();

        public override void Save()
        {
            /*using (var reader = XmlReader.Create(path))
            {
                var serializer = new NetDataContractSerializer();

                var project = serializer.ReadObject(reader, true) as Project;
                project.Path = path;

                return project;
            }*/
        }

        public override void Load()
        {
            Model = new MapActor()
            {
                Name = "Player",
                Position = Vector3.Zero,
                Rotation = Vector3.Zero,
                Orientation = Vector3.Zero,
                Scale = Vector3.One,
                ModelFilePath = Path
            };
        }

        private IEnumerable<IMesh> GetMeshes()
        {
            using (var importer = new Assimp.AssimpContext())
            {
                var scene = importer.ImportFile(Path, Assimp.PostProcessSteps.JoinIdenticalVertices
                    | Assimp.PostProcessSteps.CalculateTangentSpace
                    | Assimp.PostProcessSteps.LimitBoneWeights
                    | Assimp.PostProcessSteps.Triangulate
                    | Assimp.PostProcessSteps.GenerateSmoothNormals
                    | Assimp.PostProcessSteps.FlipUVs);

                if (scene.HasAnimations)
                {
                    foreach (var mesh in scene.Meshes)
                    {
                        var vertices = new List<JointVertex3D>();

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

                        yield return new Mesh<JointVertex3D>(vertices, mesh.GetIndices().ToList());
                    }
                }
                else
                {
                    foreach (var mesh in scene.Meshes)
                    {
                        var vertices = new List<Vertex3D>();

                        for (var i = 0; i < mesh.VertexCount; i++)
                        {
                            var position = mesh.Vertices[i].ToVector3();
                            var normals = mesh.HasNormals ? mesh.Normals[i].ToVector3() : new Vector3();
                            var tangents = mesh.HasTangentBasis ? mesh.Tangents[i].ToVector3() : new Vector3();
                            var textureCoords = mesh.HasTextureCoords(0) ? mesh.TextureCoordinateChannels[0][i].ToVector2() : new Vector2();

                            vertices.Add(new Vertex3D(position, normals, tangents, textureCoords));
                        }

                        yield return new Mesh<Vertex3D>(vertices, mesh.GetIndices().ToList());
                    }
                }
            }
        }
    }
}
