using System;

namespace SpiceEngine.Game
{
    public abstract class UpdateManager : ITick, IUpdate
    {
        private int _currentTick = 0;

        public int TickRate { get; set; } = 1;

        public void Tick()
        {
            _currentTick++;

            if (_currentTick >= TickRate)
            {
                _currentTick = 0;
                Update();
                Updated?.Invoke(this, new EventArgs());
            }

            Ticked?.Invoke(this, new EventArgs());
        }

        protected abstract void Update();

        public event EventHandler<EventArgs> Ticked;
        public event EventHandler<EventArgs> Updated;
    }
}
