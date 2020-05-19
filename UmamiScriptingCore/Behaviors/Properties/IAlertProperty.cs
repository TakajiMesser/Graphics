using UmamiScriptingCore.Behaviors.Nodes;
using System;

namespace UmamiScriptingCore.Behaviors.Properties
{
    public interface IAlertProperty
    {
        int Alertness { get; set; }
        event EventHandler<BehaviorInterruptedEventArgs> Alerted;
    }
}
