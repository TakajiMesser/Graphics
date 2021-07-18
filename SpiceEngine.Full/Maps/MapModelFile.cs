using CitrusAnimationCore;
using CitrusAnimationCore.Animations;
using CitrusAnimationCore.Bones;
using SpiceEngine.Rendering.Models;
using SpiceEngine.Utilities;
using SpiceEngineCore.Rendering;
using SweetGraphicsCore.Rendering.Models;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngineCore.Maps
{
    public class MapModelFile : IMapModel, IModelPather
    {
        public string ModelFilePath { get; set; }

        public IRenderable LoadRenderable()
        {
            using var importer = new Assimp.AssimpContext();

            var scene = importer.ImportFile(ModelFilePath, Assimp.PostProcessSteps.JoinIdenticalVertices
                | Assimp.PostProcessSteps.CalculateTangentSpace
                | Assimp.PostProcessSteps.LimitBoneWeights
                | Assimp.PostProcessSteps.Triangulate
                | Assimp.PostProcessSteps.GenerateSmoothNormals
                | Assimp.PostProcessSteps.FlipUVs);

            var model = scene.HasAnimations
                ? new AnimatedModel(ModelFilePath)
                : new Model(ModelFilePath);

            return model;
        }

        public IAnimator LoadAnimator(int entityID)
        {
            using var importer = new Assimp.AssimpContext();

            var scene = importer.ImportFile(ModelFilePath, Assimp.PostProcessSteps.JoinIdenticalVertices
                | Assimp.PostProcessSteps.CalculateTangentSpace
                | Assimp.PostProcessSteps.LimitBoneWeights
                | Assimp.PostProcessSteps.Triangulate
                | Assimp.PostProcessSteps.GenerateSmoothNormals
                | Assimp.PostProcessSteps.FlipUVs);

            return scene.HasAnimations ? new Animator(entityID) : null;
        }

        public IEnumerable<IAnimation> LoadAnimations()
        {
            using var importer = new Assimp.AssimpContext();

            var scene = importer.ImportFile(ModelFilePath, Assimp.PostProcessSteps.JoinIdenticalVertices
                | Assimp.PostProcessSteps.CalculateTangentSpace
                | Assimp.PostProcessSteps.LimitBoneWeights
                | Assimp.PostProcessSteps.Triangulate
                | Assimp.PostProcessSteps.GenerateSmoothNormals
                | Assimp.PostProcessSteps.FlipUVs);

            if (scene.HasAnimations)
            {
                foreach (var sceneAnimation in scene.Animations)
                {
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
                                    kvp.Value.AddJointIndex(match.meshIndex, match.boneIndex);
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

                    yield return new Animation(sceneAnimation.Name, (float)sceneAnimation.DurationInTicks)
                    {
                        KeyFrames = keyFrames.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).Cast<IKeyFrame>().ToList()
                    };
                }
            }
        }
    }
}
