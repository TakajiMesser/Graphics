using SpiceEngine.Utilities;
using SpiceEngineCore.Geometry.Vectors;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Utilities;
using SweetGraphicsCore.Rendering.Meshes;
using SweetGraphicsCore.Rendering.Models;
using SweetGraphicsCore.Rendering.Textures;
using SweetGraphicsCore.Vertices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SpiceEngine.Rendering.Models
{
    public class Model : IModel
    {
        public List<IMesh> Meshes { get; } = new List<IMesh>();
        
        public bool IsTransparent => Meshes.Any(m => m.Alpha < 1.0f);
        public bool IsAnimated => Meshes.Any(m => m.IsAnimated);
        public bool IsSelectable { get; set; } = true;

        public event EventHandler<AlphaEventArgs> AlphaChanged;

        public Model() { }
        public Model(string filePath)
        {
            using (var importer = new Assimp.AssimpContext())
            {
                var scene = importer.ImportFile(filePath, Assimp.PostProcessSteps.JoinIdenticalVertices
                    | Assimp.PostProcessSteps.CalculateTangentSpace
                    | Assimp.PostProcessSteps.LimitBoneWeights
                    | Assimp.PostProcessSteps.Triangulate
                    | Assimp.PostProcessSteps.GenerateSmoothNormals
                    | Assimp.PostProcessSteps.FlipUVs);

                if (scene.HasAnimations)
                {
                    foreach (var mesh in scene.Meshes)
                    {
                        var vertices = new List<AnimatedVertex3D>();

                        for (var i = 0; i < mesh.VertexCount; i++)
                        {
                            var position = mesh.Vertices[i].ToVector3();
                            var normals = mesh.HasNormals ? mesh.Normals[i].ToVector3() : new Vector3();
                            var tangents = mesh.HasTangentBasis ? mesh.Tangents[i].ToVector3() : new Vector3();
                            var textureCoords = mesh.HasTextureCoords(0) ? mesh.TextureCoordinateChannels[0][i].ToVector2() : new Vector2();

                            var boneIDX = 0.0f;
                            var boneIDY = 0.0f;
                            var boneIDZ = 0.0f;
                            var boneIDW = 0.0f;

                            var boneWeightX = 0.0f;
                            var boneWeightY = 0.0f;
                            var boneWeightZ = 0.0f;
                            var boneWeightW = 0.0f;

                            if (mesh.HasBones)
                            {
                                var matches = mesh.Bones.Select((bone, boneIndex) => new { bone, boneIndex })
                                    .Where(b => b.bone.VertexWeights.Any(v => v.VertexID == i))
                                    .ToList();

                                if (matches.Count > 0)
                                {
                                    boneIDX = matches[0].boneIndex;
                                    boneWeightX = matches[0].bone.VertexWeights.First(v => v.VertexID == i).Weight;
                                }

                                if (matches.Count > 1)
                                {
                                    boneIDY = matches[1].boneIndex;
                                    boneWeightY = matches[1].bone.VertexWeights.First(v => v.VertexID == i).Weight;
                                }

                                if (matches.Count > 2)
                                {
                                    boneIDZ = matches[2].boneIndex;
                                    boneWeightZ = matches[2].bone.VertexWeights.First(v => v.VertexID == i).Weight;
                                }

                                if (matches.Count > 3)
                                {
                                    boneIDW = matches[3].boneIndex;
                                    boneWeightW = matches[3].bone.VertexWeights.First(v => v.VertexID == i).Weight;
                                }
                            }

                            vertices.Add(new AnimatedVertex3D(position, normals, tangents, textureCoords, new Vector4(boneIDX, boneIDY, boneIDZ, boneIDW), new Vector4(boneWeightX, boneWeightY, boneWeightZ, boneWeightW)));
                        }

                        var triangleIndices = mesh.GetIndices().ToList();

                        var texturedMesh = new TexturedMesh<AnimatedVertex3D>(new Vertex3DSet<AnimatedVertex3D>(vertices, triangleIndices))
                        {
                            Material = scene.Materials[mesh.MaterialIndex].ToMaterial()
                        };

                        Add(texturedMesh);
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

                        var triangleIndices = mesh.GetIndices().ToList();

                        var texturedMesh = new TexturedMesh<Vertex3D>(new Vertex3DSet<Vertex3D>(vertices, triangleIndices))
                        {
                            Material = scene.Materials[mesh.MaterialIndex].ToMaterial()
                        };

                        Add(texturedMesh);
                    }
                }
            }
        }

        public void Add(IMesh mesh)
        {
            mesh.AlphaChanged += (s, args) => AlphaChanged?.Invoke(s, args);
            Meshes.Add(mesh);
        }

        public void Load()
        {
            foreach (var mesh in Meshes)
            {
                mesh.Load();
            }
        }

        public void Draw()
        {
            foreach (var mesh in Meshes)
            {
                mesh.Draw();
            }
        }

        public IModel Duplicate()
        {
            var model = new Model();

            foreach (var mesh in Meshes)
            {
                model.Add(mesh.Duplicate());
            }

            return model;
        }

        public static IEnumerable<TexturePaths> GetTexturePaths(string filePath)
        {
            using (var importer = new Assimp.AssimpContext())
            {
                var scene = importer.ImportFile(filePath);

                for (var i = 0; i < scene.Meshes.Count; i++)
                {
                    yield return scene.Materials[scene.Meshes[i].MaterialIndex].ToTexturePaths(Path.GetDirectoryName(filePath));
                }
            }
        }
    }
}
