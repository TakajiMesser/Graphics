using CitrusAnimationCore.Animations;
using CitrusAnimationCore.Bones;
using OpenTK;

namespace CitrusAnimationCore.Rendering
{
    public interface IAnimatedModel : IModel, IAnimate
    {
        Joint RootJoint { get; }
        Matrix4 GlobalInverseTransform { get; }

        Matrix4[] GetJointTransforms(int meshIndex);
    }
}
