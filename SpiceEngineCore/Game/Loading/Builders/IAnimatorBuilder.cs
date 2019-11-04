using OpenTK;
using SpiceEngineCore.Components.Animations;
using System.Collections.Generic;

namespace SpiceEngineCore.Game.Loading.Builders
{
    public interface IAnimatorBuilder : IComponentBuilder<IAnimator>
    {
        Vector3 Position { get; set; }
        IEnumerable<Animation> Animations { get; }

        IAnimator ToAnimator();
    }
}
