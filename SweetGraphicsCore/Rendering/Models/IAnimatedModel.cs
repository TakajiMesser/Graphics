using OpenTK;
using SweetGraphicsCore.Rendering.Animations;

namespace SweetGraphicsCore.Rendering.Models
{
    public interface IAnimatedModel : IModel, IAnimate
    {
        Joint RootJoint { get; }
        Matrix4 GlobalInverseTransform { get; }

        Matrix4[] GetJointTransforms(int meshIndex);
    }
}
