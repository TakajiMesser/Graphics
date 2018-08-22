using OpenTK;
using OpenTK.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Rendering.Animations;
using SpiceEngine.Rendering.Materials;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Rendering.Vertices;
using SpiceEngine.Utilities;

namespace SpiceEngine.Entities.Models
{
    public class AnimatedModel : Model3D
    {
        public List<JointMesh> Meshes { get; private set; } = new List<JointMesh>();
        public override List<Vector3> Vertices => Meshes.SelectMany(m => m.Vertices.Select(v => v.Position)).Distinct().ToList();
        public Animator Animator { get; set; } = new Animator();
        public Joint RootJoint { get; set; }

        private Matrix4 _globalInverseTransform;

        public AnimatedModel(string filePath, Assimp.Scene scene, TextureManager textureManager = null)
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

                var jointMesh = new JointMesh(vertices, material, mesh.GetIndices().ToList());
                if (textureManager != null)
                {
                    jointMesh.TextureMapping = new TexturePaths(scene.Materials[mesh.MaterialIndex], Path.GetDirectoryName(filePath)).ToTextureMapping(textureManager);
                }

                Meshes.Add(jointMesh);
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

        public override void Load()
        {
            foreach (var mesh in Meshes)
            {
                mesh.Load();
            }
        }

        public override void Draw()
        {
            foreach (var mesh in Meshes)
            {
                mesh.Draw();
            }
        }

        public override void SetUniforms(ShaderProgram program, TextureManager textureManager)
        {
            _modelMatrix.Set(program);

            foreach (var mesh in Meshes)
            {
                mesh.SetUniforms(program, textureManager);
            }
        }

        public override void SetUniformsAndDraw(ShaderProgram program, TextureManager textureManager)
        {
            _modelMatrix.Set(program);

            foreach (var mesh in Meshes)
            {
                mesh.SetUniforms(program, textureManager);
                mesh.Draw();
            }
        }
    }
}
