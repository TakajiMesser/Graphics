using SpiceEngineCore.Game;
using System;

namespace SpiceEngine.Game
{
    public class DelayedUpdate
    {
        private IUpdate _updater;
        private ITick _ticker;
        private int _nTicks;
        private Action _action;

        public DelayedUpdate(IUpdate updater, int nTicks, Action action)
        {
            _updater = updater;
            _nTicks = nTicks;
            _action = action;

            _updater.Updated += PerformUpdateAction;
        }

        public DelayedUpdate(ITick ticker, int nTicks, Action action)
        {
            _ticker = ticker;
            _nTicks = nTicks;
            _action = action;

            _ticker.Ticked += PerformTickAction;
        }

        private void PerformUpdateAction(object sender, EventArgs args)
        {
            _nTicks--;

            if (_nTicks <= 0)
            {
                _action();
                _updater.Updated -= PerformUpdateAction;
            }
        }

        private void PerformTickAction(object sender, EventArgs args)
        {
            _nTicks--;
            
            if (_nTicks <= 0)
            {
                _action();
                _ticker.Ticked -= PerformTickAction;
            }
        }
    }
}
