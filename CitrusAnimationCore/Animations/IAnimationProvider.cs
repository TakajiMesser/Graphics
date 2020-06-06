namespace CitrusAnimationCore.Animations
{
    public interface IAnimationProvider
    {
        // TODO - IAnimate, IAnimator, and IAnimation are confusing names
        void AddAnimated(int entityID, IAnimate animated);
    }
}
