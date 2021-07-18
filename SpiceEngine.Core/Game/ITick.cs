using System;

namespace SpiceEngineCore.Game
{
    public interface ITick
    {
        void Tick();
        event EventHandler<EventArgs> Ticked;
    }
}
