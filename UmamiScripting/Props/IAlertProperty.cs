using System;
using UmamiScriptingCore.Behaviors;

namespace UmamiScriptingCore.Props
{
    public interface IAlertProperty
    {
        int Alertness { get; set; }
        event EventHandler<BehaviorInterruptedEventArgs> Alerted;
    }
}
