using CitrusAnimationCore.Animations;
using CitrusAnimationCore.Bones;
using CitrusAnimationCore.Rendering;
using SpiceEngine.Utilities;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.Rendering.Shaders;
using SpiceEngineCore.Utilities;
using System.Collections.Generic;

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
                    _jointTransformsByMeshIndex[transforms.MeshIndex] = transforms.Transforms;
                }
            }
        }

        public override void SetUniforms(IShader shader, int meshIndex) => shader.SetUniform("jointTransforms", GetJointTransforms(meshIndex));
    }
}
