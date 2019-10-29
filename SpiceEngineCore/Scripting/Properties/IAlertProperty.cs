using SpiceEngineCore.Scripting.Nodes;
using System;

namespace SpiceEngineCore.Scripting.Properties
{
    public interface IAlertProperty
    {
        int Alertness { get; set; }
        event EventHandler<BehaviorInterruptedEventArgs> Alerted;
    }
}
