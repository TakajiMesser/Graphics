using SweetGraphicsCore.Rendering.Models;

namespace SpiceEngineCore.Components.Animations
{
    public interface IAnimationProvider
    {
        // TODO - IAnimate, IAnimator, and IAnimation are confusing names
        void AddAnimated(int entityID, IAnimate animated);
    }
}
