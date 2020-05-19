using OpenTK;
using SpiceEngineCore.Components.Animations;
using System.Collections.Generic;

namespace SpiceEngineCore.Components.Builders
{
    public interface IAnimatorBuilder : IComponentBuilder<IAnimator>
    {
        Vector3 Position { get; set; }
        IEnumerable<IAnimation> Animations { get; }
    }
}
