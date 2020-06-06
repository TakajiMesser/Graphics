using CitrusAnimationCore.Animations;
using SpiceEngineCore.Components;
using System.Collections.Generic;

namespace CitrusAnimationCore
{
    public interface IAnimatorBuilder : IComponentBuilder<IAnimator>
    {
        IEnumerable<IAnimation> Animations { get; }
    }
}
