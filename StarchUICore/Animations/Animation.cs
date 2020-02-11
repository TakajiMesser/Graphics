namespace StarchUICore.Animations
{
    public abstract class Animation : IAnimation
    {
        protected IElement _element;
        private int _remainingDelay;
        private int _currentTick;

        public int Duration { get; set; }
        public int Delay { get; set; }

        public bool IsEnded { get; protected set; }

        public void Apply(IElement element, int delay = 0)
        {
            _element = element;

            Delay = delay;
            _remainingDelay = delay;
        }

        public void Update(int nTicks)
        {
            if (_remainingDelay > 0)
            {
                if (_remainingDelay > nTicks)
                {
                    _remainingDelay -= nTicks;
                }
                else
                {
                    _remainingDelay -= nTicks;

                }
            }
            else
            {
                _currentTick += nTicks;
                Update((float)nTicks / Duration);
            }
        }

        protected abstract void Update(float percent);

        public void Reset()
        {
            _element = null;
            _remainingDelay = Delay;
            _currentTick = 0;
            IsEnded = false;
        }
    }
}
