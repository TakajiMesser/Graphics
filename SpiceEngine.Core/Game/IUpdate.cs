using System;

namespace SpiceEngineCore.Game
{
    public interface IUpdate
    {
        //void Update();
        event EventHandler<EventArgs> Updated;
    }
}
