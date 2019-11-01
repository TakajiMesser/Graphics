using System;

namespace SpiceEngineCore.Game.Loading
{
    public class RendererEventArgs : EventArgs
    {
        public string Name { get; private set; }

        public RendererEventArgs(string name) => Name = name;
    }
}
