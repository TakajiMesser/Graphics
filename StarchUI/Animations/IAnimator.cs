namespace StarchUICore.Animations
{
    public interface IAnimator
    {
        void Apply(IAnimation animation, IElement element, int delay = 0);
        void Update(int nTicks);
    }
}
