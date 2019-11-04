using OpenTK;
using SpiceEngineCore.Rendering.Animations;

namespace SpiceEngineCore.Rendering.Models
{
    public interface IAnimatedModel : IModel
    {
        Joint RootJoint { get; }
        Matrix4 GlobalInverseTransform { get; }

        Matrix4[] GetJointTransforms(int meshIndex);
        void SetKeyFrame(KeyFrame keyFrame);
    }
}
