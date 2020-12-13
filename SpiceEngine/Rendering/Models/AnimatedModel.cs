using CitrusAnimationCore.Animations;
using CitrusAnimationCore.Bones;
using CitrusAnimationCore.Rendering;
using SpiceEngine.Utilities;
using SpiceEngineCore.Utilities;
using System.Collections.Generic;

using Color4 = OpenTK.Graphics.Color4;
using Matrix2 = OpenTK.Matrix2;
using Matrix3 = OpenTK.Matrix3;
using Matrix4 = OpenTK.Matrix4;
using Quaternion = OpenTK.Quaternion;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace SpiceEngine.Rendering.Models
{
    public class AnimatedModel : Model, IAnimatedModel
    {
        public const int MAX_JOINTS = 100;

        private Dictionary<int, Matrix4[]> _jointTransformsByMeshIndex = new Dictionary<int, Matrix4[]>();

        public AnimatedModel() : base() { }
        public AnimatedModel(string filePath) : base(filePath)
        {
            using (var importer = new Assimp.AssimpContext())
            {
                var scene = importer.ImportFile(filePath, Assimp.PostProcessSteps.JoinIdenticalVertices
                    | Assimp.PostProcessSteps.CalculateTangentSpace
                    | Assimp.PostProcessSteps.LimitBoneWeights
                    | Assimp.PostProcessSteps.Triangulate
                    | Assimp.PostProcessSteps.GenerateSmoothNormals
                    | Assimp.PostProcessSteps.FlipUVs);

                // Every bone has an offset matrix, which converts it from model-space to bone-space (animation transforms are done in bone-space)
                RootJoint = scene.RootNode.ToJoint(scene.Meshes);

                for (var i = 0; i < scene.Meshes.Count; i++)
                {
                    RootJoint.CalculateInverseBindTransforms(i, Matrix4.Identity);
                }
                
                GlobalInverseTransform = scene.RootNode.Children[1].Transform.ToMatrix4().Inverted();
            }
        }

        public Joint RootJoint { get; private set; }
        public Matrix4 GlobalInverseTransform { get; private set; }

        public Matrix4[] GetJointTransforms(int meshIndex) => _jointTransformsByMeshIndex.ContainsKey(meshIndex)
            ? _jointTransformsByMeshIndex[meshIndex]
            : ArrayExtensions.Initialize(MAX_JOINTS, Matrix4.Identity);

        public void SetKeyFrame(IKeyFrame keyFrame)
        {
            if (keyFrame is KeyFrame castKeyFrame)
            {
                for (var i = 0; i < 6; i++)
                {
                    RootJoint.ApplyKeyFrameTransforms(i, castKeyFrame, Matrix4.Identity, GlobalInverseTransform);
                }

                var meshTransforms = RootJoint.GetMeshTransforms(castKeyFrame);

                foreach (var transforms in meshTransforms)
                {
                    var jointTransforms = ArrayExtensions.Initialize(MAX_JOINTS, Matrix4.Identity);

                    for (var i = 0; i < MAX_JOINTS; i++)
                    {
                        jointTransforms[i] = Matrix4.Zero;
                    }

                    foreach (var kvp in transforms.TransformsByBoneIndex)
                    {
                        jointTransforms[kvp.Key] = kvp.Value;
                    }

                    _jointTransformsByMeshIndex[transforms.MeshIndex] = jointTransforms;
                }
            }
        }

        //public override void SetUniforms(ShaderProgram shaderProgram, int meshIndex) => shaderProgram.SetUniform("jointTransforms", GetJointTransforms(meshIndex));
    }
}
