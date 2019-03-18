namespace SpiceEngine.Game
{
    public abstract class UpdateManager
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
            }
        }

        protected abstract void Update();
    }
}
