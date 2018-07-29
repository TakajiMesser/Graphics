using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiceEngine.Scripting.Behaviors;

namespace SpiceEngine.Scripting.Properties
{
    public interface IAlertProperty
    {
        int Alertness { get; set; }
        event EventHandler<BehaviorInterruptedEventArgs> Alerted;
    }
}
