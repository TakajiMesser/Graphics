using System;

namespace SpiceEngine.Scripting.Meters
{
    public class TriggeredEventArgs : EventArgs
    {
        public string Name { get; }

        public TriggeredEventArgs(Trigger trigger) => Name = trigger.Name;
    }
}
