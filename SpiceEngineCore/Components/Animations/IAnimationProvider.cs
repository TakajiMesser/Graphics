using SpiceEngineCore.Rendering.Animations;
using SpiceEngineCore.Rendering.Models;
using System;

namespace SpiceEngineCore.Components.Animations
{
    public interface IAnimationProvider
    {
        void AddModel(int entityID, IAnimatedModel model);
    }
}
