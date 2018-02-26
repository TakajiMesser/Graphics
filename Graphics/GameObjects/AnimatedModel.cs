using Graphics.Inputs;
using Graphics.Meshes;
using Graphics.Physics.Collision;
using Graphics.Rendering.Matrices;
using Graphics.Rendering.Shaders;
using Graphics.Scripting.BehaviorTrees;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Graphics.Lighting;
using Graphics.Rendering.Textures;
using OpenTK.Graphics.OpenGL;
using Graphics.Rendering.Animations;
using Graphics.Rendering.Vertices;
using OpenTK.Graphics;
using Graphics.Materials;
using Graphics.Utilities;
using System.IO;

namespace Graphics.GameObjects
{
    public class AnimatedModel : Model
    {
        public List<JointMesh> Meshes { get; private set; } = new List<JointMesh>();
        public override List<Vector3> Vertices => Meshes.SelectMany(m => m.Vertices.Select(v => v.Position)).Distinct().ToList();
        public Animator Animator { get; set; } = new Animator();
        public Joint RootJoint { get; set; }

        private Matrix4 _globalInverseTransform;

        public AnimatedModel(string filePath, Assimp.Scene scene, TextureManager textureManager)
        {
            Animator.AnimationEnd += (s, args) => Animator.CurrentAnimation = Animator.Animations.First();
            Animator.Animate += (s, args) =>
            {
                for (var i = 0; i < 6; i++)
                {
                    RootJoint.ApplyKeyFrameTransforms(i, args.KeyFrame, Matrix4.Identity, _globalInverseTransform);
                }
                
                var meshTransforms = RootJoint.GetMeshTransforms(args.KeyFrame);
                foreach (var transforms in meshTransforms)
                {
                    Meshes[transforms.MeshIndex].SetJointTransforms(transforms);
                }
            };

            foreach (var texture in scene.Textures)
            {
                //texture.
            }

            foreach (var mesh in scene.Meshes)
            {
                var material = new Material(scene.Materials[mesh.MaterialIndex]);
                var vertices = new List<JointVertex>();

                for (var i = 0; i < mesh.VertexCount; i++)
                {
                    var vertex = new JointVertex()
                    {
                        Position = mesh.Vertices[i].ToVector3(),
                        Color = new Color4()
                    };

                    if (mesh.HasNormals)
                    {
                        vertex.Normal = mesh.Normals[i].ToVector3();
                    }

                    if (mesh.HasTextureCoords(0))
                    {
                        vertex.TextureCoords = mesh.TextureCoordinateChannels[0][i].ToVector2();
                    }

                    if (mesh.HasTangentBasis)
                    {
                        vertex.Tangent = mesh.Tangents[i].ToVector3();
                    }

                    if (mesh.HasBones)
                    {
                        var matches = mesh.Bones.Select((bone, boneIndex) => new { bone, boneIndex })
                            .Where(b => b.bone.VertexWeights.Any(v => v.VertexID == i))
                            .ToList();

                        for (var j = 0; j < 4 && j < matches.Count; j++)
                        {
                            vertex.BoneIDs[j] = matches[j].boneIndex;
                            vertex.BoneWeights[j] = matches[j].bone.VertexWeights.First(v => v.VertexID == i).Weight;
                        }
                    }

                    vertices.Add(vertex);
                }

                Meshes.Add(new JointMesh(vertices, material, mesh.GetIndices().ToList())
                {
                    TextureMapping = new TexturePaths(scene.Materials[mesh.MaterialIndex], Path.GetDirectoryName(filePath)).ToTextureMapping(textureManager)
                });
            }

            // Every bone has an offset matrix, which converts it from model-space to bone-space (animation transforms are done in bone-space)
            RootJoint = Joint.CreateJoint(scene.RootNode, scene.Meshes);

            for (var i = 0; i < scene.Meshes.Count; i++)
            {
                RootJoint.CalculateInverseBindTransforms(i, Matrix4.Identity);
            }
            //_globalInverseTransform = scene.RootNode.Transform.ToMatrix4().Inverted();
            _globalInverseTransform = scene.RootNode.Children[1].Transform.ToMatrix4().Inverted();

            foreach (var sceneAnimation in scene.Animations)
            {
                var animation = new Animation(sceneAnimation.Name)
                {
                    Duration = (float)sceneAnimation.DurationInTicks
                };

                sceneAnimation.TicksPerSecond = 60.0;

                var keyFrames = new Dictionary<double, KeyFrame>();

                if (sceneAnimation.HasNodeAnimations)
                {
                    foreach (var nodeChannel in sceneAnimation.NodeAnimationChannels)
                    {
                        var transforms = new Dictionary<double, JointTransform>();

                        foreach (var positionKey in nodeChannel.PositionKeys)
                        {
                            if (!transforms.ContainsKey(positionKey.Time))
                            {
                                transforms.Add(positionKey.Time, new JointTransform());
                            }

                            transforms[positionKey.Time].Position = positionKey.Value.ToVector3();
                        }

                        foreach (var rotationKey in nodeChannel.RotationKeys)
                        {
                            if (!transforms.ContainsKey(rotationKey.Time))
                            {
                                transforms.Add(rotationKey.Time, new JointTransform());
                            }

                            transforms[rotationKey.Time].Rotation = rotationKey.Value.ToQuaternion();
                        }

                        foreach (var scaleKey in nodeChannel.ScalingKeys)
                        {
                            if (!transforms.ContainsKey(scaleKey.Time))
                            {
                                transforms.Add(scaleKey.Time, new JointTransform());
                            }

                            transforms[scaleKey.Time].Scale = scaleKey.Value.ToVector3();
                        }

                        var matches = scene.Meshes
                            .Select((mesh, meshIndex) => new { mesh, meshIndex, boneIndex = mesh.Bones.FindIndex(b => b.Name == nodeChannel.NodeName) })
                            .Where(m => m.boneIndex >= 0);
                        
                        foreach (var kvp in transforms)
                        {
                            if (!keyFrames.ContainsKey(kvp.Key))
                            {
                                keyFrames.Add(kvp.Key, new KeyFrame()
                                {
                                    Time = (float)kvp.Key
                                });
                            }

                            foreach (var match in matches)
                            {
                                kvp.Value.JointIndices.Add(new JointIndex(match.meshIndex, match.boneIndex));
                            }

                            kvp.Value.Name = nodeChannel.NodeName;
                            
                            keyFrames[kvp.Key].Transforms.Add(kvp.Value);
                        }
                    }
                }
                
                if (sceneAnimation.HasMeshAnimations)
                {
                    foreach (var channel in sceneAnimation.MeshAnimationChannels)
                    {
                        foreach (var meshKey in channel.MeshKeys)
                        {
                            if (!keyFrames.ContainsKey(meshKey.Time))
                            {
                                var keyFrame = new KeyFrame
                                {
                                    Time = (float)meshKey.Time
                                };
                                keyFrame.Transforms.Add(new JointTransform());
                                keyFrames.Add(meshKey.Time, keyFrame);
                            }

                            //keyFrames[meshKey.Time].Transforms.First().JointIndices.First().MeshIndex = meshKey.Value;
                        }
                    }
                }

                /*foreach (var kvp in keyFrames)
                {
                    kvp.Value.Transforms.OrderBy(t => t.MeshIndex).ToList();
                }*/

                animation.KeyFrames.AddRange(keyFrames.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value));

                Animator.Animations.Add(animation);
            }
        }

        public override void Load(ShaderProgram program) => Meshes.ForEach(m => m.Load(program));
        public override void ClearLights() => Meshes.ForEach(m => m.ClearLights());
        public override void AddPointLights(IEnumerable<PointLight> lights) => Meshes.ForEach(m => m.AddPointLights(lights));

        public override void AddTestColors()
        {
            /*var vertices = new List<JointVertex>();

            for (var i = 0; i < Mesh.Vertices.Count; i++)
            {
                if (i % 3 == 0)
                {
                    vertices.Add(Mesh.Vertices[i].Colored(Color4.Lime));
                }
                else if (i % 3 == 1)
                {
                    vertices.Add(Mesh.Vertices[i].Colored(Color4.Red));
                }
                else if (i % 3 == 2)
                {
                    vertices.Add(Mesh.Vertices[i].Colored(Color4.Blue));
                }
            }

            Mesh.ClearVertices();
            Mesh.AddVertices(vertices);
            Mesh.RefreshVertices();*/
        }

        public override void Draw(ShaderProgram program, TextureManager textureManager)
        {
            _modelMatrix.Set(program);

            /*for (var i = 0; i < Meshes.Count; i++)
            {
                if (i == 5)
                {
                    Meshes[i].Draw(program, textureManager);
                }
            }*/

            foreach (var mesh in Meshes)
            {
                mesh.Draw(program, textureManager);
            }
        }
    }
}
