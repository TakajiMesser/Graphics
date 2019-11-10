using SpiceEngineCore.Utilities;
using System;

namespace SpiceEngineCore.Scripting.Nodes
{
    public abstract class Node
    {
        public event EventHandler<BehaviorCompletedEventArgs> Completed;

        public abstract BehaviorStatus Tick(BehaviorContext context);

        public abstract void Reset();

        protected virtual void OnCompleted(BehaviorCompletedEventArgs e) => Completed?.Invoke(this, e);
    }
}
