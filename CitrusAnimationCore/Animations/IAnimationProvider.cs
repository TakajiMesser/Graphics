using SpiceEngineCore.Game;

namespace CitrusAnimationCore.Animations
{
    public interface IAnimationProvider : IGameSystem
    {
        // TODO - IAnimate, IAnimator, and IAnimation are confusing names
        void AddAnimated(int entityID, IAnimate animated);
    }
}
