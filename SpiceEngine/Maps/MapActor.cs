using OpenTK;
using System.Collections.Generic;
using SpiceEngine.Entities;
using SpiceEngine.Entities.Models;
using SpiceEngine.Game;
using SpiceEngine.Physics.Collision;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Scripting.Behaviors;
using SpiceEngine.Scripting.StimResponse;
using SpiceEngine.Rendering.Vertices;
using SpiceEngine.Rendering.Meshes;
using SpiceEngine.Rendering.Materials;
using SpiceEngine.Utilities;
using System.Linq;
using SpiceEngine.Rendering.Animations;
using SpiceEngine.Physics.Shapes;

namespace SpiceEngine.Maps
{
    public class MapActor : MapEntity3D<Actor>
    {
        public string Name { get; set; }
        public Vector3 Orientation { get; set; }

        public string ModelFilePath { get; set; }
        public List<TexturePaths> TexturesPaths { get; set; } = new List<TexturePaths>();

        public string BehaviorFilePath { get; set; }
        public List<Stimulus> Stimuli { get; private set; } = new List<Stimulus>();
        public List<GameProperty> Properties { get; set; }

        public bool HasCollision { get; set; }
        //public ICollider Collider { get; set; }

        public IEnumerable<IMesh3D> ToMeshes()
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

                        yield return new Mesh3D<JointVertex3D>(vertices, mesh.GetIndices().ToList());
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

                        yield return new Mesh3D<Vertex3D>(vertices, mesh.GetIndices().ToList());
                    }
                }
            }
        }

        public override Actor ToEntity(/*TextureManager textureManager = null*/)
        {
            var actor = new Actor(Name);

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
                        var animatedActor = new AnimatedActor(Name);

                        animatedActor.Animator.AnimationEnd += (s, args) => animatedActor.Animator.CurrentAnimation = animatedActor.Animator.Animations.First();
                        animatedActor.Animator.Animate += (s, args) =>
                        {
                            for (var i = 0; i < 6; i++)
                            {
                                animatedActor.RootJoint.ApplyKeyFrameTransforms(i, args.KeyFrame, Matrix4.Identity, animatedActor._globalInverseTransform);
                            }

                            var meshTransforms = animatedActor.RootJoint.GetMeshTransforms(args.KeyFrame);
                            foreach (var transforms in meshTransforms)
                            {
                                var jointTransforms = ArrayExtensions.Initialize(AnimatedActor.MAX_JOINTS, Matrix4.Identity);

                                for (var i = 0; i < AnimatedActor.MAX_JOINTS; i++)
                                {
                                    jointTransforms[i] = Matrix4.Zero;
                                }

                                foreach (var kvp in transforms.TransformsByBoneIndex)
                                {
                                    jointTransforms[kvp.Key] = kvp.Value;
                                }

                                animatedActor.SetJointTransforms(transforms.MeshIndex, jointTransforms);
                            }
                        };

                        foreach (var texture in scene.Textures)
                        {
                            //texture.
                        }

                        /*if (textureManager != null)
                        {
                            jointMesh.TextureMapping = new TexturePaths(scene.Materials[mesh.MaterialIndex], Path.GetDirectoryName(filePath)).ToTextureMapping(textureManager);
                        }*/

                        // Every bone has an offset matrix, which converts it from model-space to bone-space (animation transforms are done in bone-space)
                        animatedActor.RootJoint = Joint.CreateJoint(scene.RootNode, scene.Meshes);

                        for (var i = 0; i < scene.Meshes.Count; i++)
                        {
                            animatedActor.RootJoint.CalculateInverseBindTransforms(i, Matrix4.Identity);
                        }
                        //_globalInverseTransform = scene.RootNode.Transform.ToMatrix4().Inverted();
                        animatedActor._globalInverseTransform = scene.RootNode.Children[1].Transform.ToMatrix4().Inverted();

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

                            animatedActor.Animator.Animations.Add(animation);
                        }

                        actor = animatedActor;
                    }

                    for (var i = 0; i < scene.Meshes.Count; i++)
                    {
                        var material = new Material(scene.Materials[scene.Meshes[i].MaterialIndex]);
                        actor.AddMaterial(i, material);
                    }
                }
            }

            actor.Position = Position;
            actor.OriginalRotation = Rotation;
            actor.Scale = Scale;
            actor.Orientation = Quaternion.FromEulerAngles(Orientation);

            if (!string.IsNullOrEmpty(BehaviorFilePath))
            {
                actor.Behaviors = Behavior.Load(BehaviorFilePath);
            }

            actor.Stimuli.AddRange(Stimuli);

            if (Properties != null)
            {
                foreach (var property in Properties)
                {
                    actor.Properties.Add(property.Name, property);
                }
            }

            return actor;
        }

        public override Shape3D ToShape()
        {
            using (var importer = new Assimp.AssimpContext())
            {
                var scene = importer.ImportFile(ModelFilePath, Assimp.PostProcessSteps.JoinIdenticalVertices
                    | Assimp.PostProcessSteps.CalculateTangentSpace
                    | Assimp.PostProcessSteps.LimitBoneWeights
                    | Assimp.PostProcessSteps.Triangulate
                    | Assimp.PostProcessSteps.GenerateSmoothNormals
                    | Assimp.PostProcessSteps.FlipUVs);

                var vertices = scene.Meshes.SelectMany(m => m.Vertices.Select(v => v.ToVector3()));

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
    }
}
