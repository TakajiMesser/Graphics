using SpiceEngineCore.Utilities;
using System;

namespace SpiceEngine.Scripting
{
    public class BehaviorCompletedEventArgs : EventArgs
    {
        public BehaviorStatus Status { get; private set; }

        public BehaviorCompletedEventArgs(BehaviorStatus status) => Status = status;
    }
}
