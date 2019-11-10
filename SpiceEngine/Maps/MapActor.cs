using OpenTK;
using SpiceEngine.Rendering.Models;
using SpiceEngine.Utilities;
using SpiceEngineCore.Components.Animations;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Actors;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Physics.Shapes;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Animations;
using SpiceEngineCore.Rendering.Textures;
using SpiceEngineCore.Scripting;
using SpiceEngineCore.Scripting.Properties;
using SpiceEngineCore.Scripting.Scripts;
using SpiceEngineCore.Scripting.StimResponse;
using SpiceEngineCore.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngineCore.Maps
{
    public class MapActor : MapEntity3D<IActor>, IMapActor
    {
        public string Name { get; set; }

        public Vector3 Orientation { get; set; }
        public Vector3 Offset { get; set; }

        public string ModelFilePath { get; set; }
        public List<TexturePaths> TexturesPaths { get; set; } = new List<TexturePaths>();

        public MapBehavior Behavior { get; set; }

        public IEnumerable<Script> Scripts => Behavior != null ? Behavior.GetScripts() : Enumerable.Empty<Script>();
        public List<Stimulus> Stimuli { get; private set; } = new List<Stimulus>();
        public List<Property> Properties { get; set; } = new List<Property>();

        public bool IsPhysical { get; set; }
        //public ICollider Collider { get; set; }

        public override IEntity ToEntity(/*TextureManager textureManager = null*/)
        {
            var actor = new Actor()
            {
                Name = Name
            };

            actor.Position = Position;
            actor.Orientation = Quaternion.FromEulerAngles(Orientation.ToRadians());
            actor.Rotation = Quaternion.FromEulerAngles(Rotation.ToRadians());
            actor.Scale = Scale;

            return actor;
        }

        IRenderable IComponentBuilder<IRenderable>.ToComponent()
        {
            //if (string.IsNullOrEmpty(ModelFilePath))

            using (var importer = new Assimp.AssimpContext())
            {
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
        }

        IShape IComponentBuilder<IShape>.ToComponent()
        {
            using (var importer = new Assimp.AssimpContext())
            {
                var scene = importer.ImportFile(ModelFilePath, Assimp.PostProcessSteps.JoinIdenticalVertices
                    | Assimp.PostProcessSteps.CalculateTangentSpace
                    | Assimp.PostProcessSteps.LimitBoneWeights
                    | Assimp.PostProcessSteps.Triangulate
                    | Assimp.PostProcessSteps.GenerateSmoothNormals
                    | Assimp.PostProcessSteps.FlipUVs);

                /*public Quaternion Rotation
                {
                    get => Orientation * _modelMatrix.Rotation;
                    set => _modelMatrix.Rotation = Orientation * value;
                }

                actor.Orientation = Quaternion.FromEulerAngles(Orientation);*/

                var vertices = scene.Meshes
                    .SelectMany(m => m.Vertices
                        .Select(v => v.ToVector3()))
                    .Select(v => (Matrix4.CreateScale(Scale)
                        * Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(Orientation.ToRadians())
                        * Quaternion.FromEulerAngles(Rotation.ToRadians()))
                        * new Vector4(v, 1.0f)).Xyz);

                if (Name == "Player")
                {
                    return new Sphere(vertices);
                }
                else
                {
                    return new Box(vertices);
                }
            }

            /*actor.HasCollision = mapActor.HasCollision;
            actor.Bounds = actor.Name == "Player"
                ? (Bounds)new BoundingCircle(actor, meshes.SelectMany(m => m.Vertices.Select(v => v.Position)))
                : new BoundingBox(actor, meshes.SelectMany(m => m.Vertices.Select(v => v.Position)));*/
        }

        IBehavior IComponentBuilder<IBehavior>.ToComponent() => Behavior?.ToBehavior();

        IAnimator IComponentBuilder<IAnimator>.ToComponent()
        {
            using (var importer = new Assimp.AssimpContext())
            {
                var scene = importer.ImportFile(ModelFilePath, Assimp.PostProcessSteps.JoinIdenticalVertices
                    | Assimp.PostProcessSteps.CalculateTangentSpace
                    | Assimp.PostProcessSteps.LimitBoneWeights
                    | Assimp.PostProcessSteps.Triangulate
                    | Assimp.PostProcessSteps.GenerateSmoothNormals
                    | Assimp.PostProcessSteps.FlipUVs);

                if (scene.HasAnimations)
                {
                    return new Animator();
                }
            }

            return null;
        }

        public IEnumerable<Animation> Animations
        {
            get
            {
                using (var importer = new Assimp.AssimpContext())
                {
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

                            yield return new Animation(sceneAnimation.Name, (float)sceneAnimation.DurationInTicks)
                            {
                                KeyFrames = keyFrames.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToList()
                            };
                        }
                    }
                }
            }
        }

        /*public Script ToScript()
        {
            actor.Stimuli.AddRange(Stimuli);

            if (Properties != null)
            {
                foreach (var property in Properties)
                {
                    actor.Properties.Add(property.Name, property);
                }
            }
        }*/
    }
}
