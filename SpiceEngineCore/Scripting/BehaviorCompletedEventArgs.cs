using SpiceEngineCore.Utilities;
using System;

namespace SpiceEngineCore.Scripting
{
    public class BehaviorCompletedEventArgs : EventArgs
    {
        public BehaviorStatus Status { get; private set; }

        public BehaviorCompletedEventArgs(BehaviorStatus status) => Status = status;
    }
}
