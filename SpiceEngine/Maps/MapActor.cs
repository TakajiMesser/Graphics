using CitrusAnimationCore;
using CitrusAnimationCore.Animations;
using CitrusAnimationCore.Bones;
using SavoryPhysicsCore;
using SavoryPhysicsCore.Bodies;
using SavoryPhysicsCore.Shapes.ThreeDimensional;
using SpiceEngine.Maps;
using SpiceEngine.Rendering.Models;
using SpiceEngine.Utilities;
using SpiceEngineCore.Components;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Entities.Actors;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering;
using SpiceEngineCore.Rendering.Materials;
using SpiceEngineCore.Utilities;
using SweetGraphicsCore.Rendering.Meshes;
using SweetGraphicsCore.Rendering.Models;
using SweetGraphicsCore.Rendering.Textures;
using SweetGraphicsCore.Vertices;
using System.Collections.Generic;
using System.Linq;
using UmamiScriptingCore;
using UmamiScriptingCore.Props;
using UmamiScriptingCore.Scripts;
using UmamiScriptingCore.StimResponse;

namespace SpiceEngineCore.Maps
{
    public class MapActor : MapEntity<IActor>, IMapActor, IModelPather, ITexturePather, IBodyBuilder, IAnimatorBuilder, IBehaviorBuilder
    {
        public string Name { get; set; }

        public Vector3 Orientation { get; set; }
        public Vector3 Offset { get; set; }

        public string ModelFilePath { get; set; }
        public List<Vertex3D> Vertices { get; set; } = new List<Vertex3D>();
        public Material Material { get; set; }
        public List<int> TriangleIndices { get; set; } = new List<int>();
        public Color4 Color { get; set; }
        public List<TexturePaths> TexturesPaths { get; set; } = new List<TexturePaths>();

        public MapBehavior Behavior { get; set; }

        public List<Stimulus> Stimuli { get; set; } = new List<Stimulus>();
        public List<Property> Properties { get; set; } = new List<Property>();

        public bool IsPhysical { get; set; }
        //public ICollider Collider { get; set; }

        public IEnumerable<IScript> GetScripts() => Behavior != null ? Behavior.GetScripts() : Enumerable.Empty<IScript>();

        public IEnumerable<IStimulus> GetStimuli() => Stimuli;

        public IEnumerable<IProperty> GetProperties() => Properties;

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

        public IRenderable ToRenderable()
        {
            //if (string.IsNullOrEmpty(ModelFilePath))
            if (!string.IsNullOrEmpty(ModelFilePath))
            {
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
            else
            {
                // TODO - Determine if Actor's should always be Models, or if they can also just be singular Meshes...
                var model = new Model();

                if (TexturesPaths.Any())
                {
                    model.Add(new TexturedMesh<Vertex3D>(new Vertex3DSet<Vertex3D>(Vertices, TriangleIndices))
                    {
                        Material = Material
                    });
                }
                else
                {
                    model.Add(new ColoredMesh<Vertex3D>(new Vertex3DSet<Vertex3D>(Vertices.Select(v => (Vertex3D)v.Colored(Color)).ToList(), TriangleIndices)));
                }

                return model;
            }
        }

        IBody IComponentBuilder<IBody>.ToComponent(int entityID)
        {
            if (!string.IsNullOrEmpty(ModelFilePath))
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

                    if (Name == "Player"/* || Name == "BasicEnemy"*/)
                    {
                        return new RigidBody(entityID, new Sphere(vertices));
                    }
                    else
                    {
                        return new RigidBody(entityID, new Box(vertices));
                    }
                }

                /*actor.HasCollision = mapActor.HasCollision;
                actor.Bounds = actor.Name == "Player"
                    ? (Bounds)new BoundingCircle(actor, meshes.SelectMany(m => m.Vertices.Select(v => v.Position)))
                    : new BoundingBox(actor, meshes.SelectMany(m => m.Vertices.Select(v => v.Position)));*/
            }
            else
            {
                return new RigidBody(entityID, new Box(Vertices.Select(v => v.Position)));
            }
        }

        IBehavior IComponentBuilder<IBehavior>.ToComponent(int entityID) => Behavior?.ToBehavior(entityID);

        IAnimator IComponentBuilder<IAnimator>.ToComponent(int entityID)
        {
            if (!string.IsNullOrEmpty(ModelFilePath))
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
                        return new Animator(entityID);
                    }
                }
            }

            return null;
        }

        public IEnumerable<IAnimation> Animations
        {
            get
            {
                if (!string.IsNullOrEmpty(ModelFilePath))
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
