using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceEngine.Scripting.Behaviors
{
    public class BehaviorCompletedEventArgs : EventArgs
    {
        public BehaviorStatus Status { get; private set; }

        public BehaviorCompletedEventArgs(BehaviorStatus status)
        {
            Status = status;
        }
    }
}
