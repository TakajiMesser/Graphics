namespace StarchUICore.Animations
{
    public class FadeAnimation : Animation
    {
        public FadeAnimation(int alpha) => Alpha = alpha;

        public int Alpha { get; }

        protected override void Update(float percent)
        {
            _element.Alpha += Alpha * percent;
        }
    }
}
