using System;

namespace SpiceEngine.Game
{
    public interface ITick
    {
        void Tick();
        event EventHandler<EventArgs> Ticked;
    }
}
