using System;

namespace SpiceEngineCore.Entities.Selection
{
    public class DuplicationEventArgs : EventArgs
    {
        public Duplication Duplication { get; private set; }

        public DuplicationEventArgs(Duplication duplication) => Duplication = duplication;
    }
}
