using System;

namespace SpiceEngineCore.Game.Loading
{
    public class RendererLoadEventArgs : EventArgs
    {
        public string Name { get; private set; }

        public RendererLoadEventArgs(string name) => Name = name;
    }
}
