using CitrusAnimationCore;
using CitrusAnimationCore.Animations;
using SpiceEngineCore.Rendering;
using System.Collections.Generic;

namespace SpiceEngineCore.Maps
{
    public interface IMapModel
    {
        IRenderable LoadRenderable();

        IAnimator LoadAnimator(int entityID);
        IEnumerable<IAnimation> LoadAnimations();
    }
}
