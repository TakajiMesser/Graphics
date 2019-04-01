using SpiceEngine.Scripting.Nodes;
using System;

namespace SpiceEngine.Scripting.Properties
{
    public interface IAlertProperty
    {
        int Alertness { get; set; }
        event EventHandler<BehaviorInterruptedEventArgs> Alerted;
    }
}
