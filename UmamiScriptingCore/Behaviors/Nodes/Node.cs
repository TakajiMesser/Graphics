using System;

namespace UmamiScriptingCore.Behaviors.Nodes
{
    public abstract class Node
    {
        public event EventHandler<BehaviorCompletedEventArgs> Completed;

        public abstract BehaviorStatus Tick(BehaviorContext context);

        public abstract void Reset();

        protected virtual void OnCompleted(BehaviorCompletedEventArgs e) => Completed?.Invoke(this, e);
    }
}
