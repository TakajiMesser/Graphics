using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoEngine.Scripting.Behaviors;

namespace TakoEngine.Scripting.Properties
{
    public interface IAlertProperty
    {
        int Alertness { get; set; }
        event EventHandler<BehaviorInterruptedEventArgs> Alerted;
    }
}
