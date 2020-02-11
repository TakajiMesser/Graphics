using System.Collections.Generic;

namespace StarchUICore.Animations
{
    public class Animator : IAnimator
    {
        private List<IAnimation> _animations = new List<IAnimation>();

        public void Apply(IAnimation animation, IElement element, int delay = 0)
        {
            animation.Delay = delay;
            animation.Apply(element, delay);

            _animations.Add(animation);
        }

        public void Update(int nTicks)
        {
            for (var i = _animations.Count - 1; i >= 0; i--)
            {
                var animation = _animations[i];

                if (animation.IsEnded)
                {
                    _animations.RemoveAt(i);
                }
                else
                {
                    animation.Update(nTicks);
                }
            }
        }
    }
}
