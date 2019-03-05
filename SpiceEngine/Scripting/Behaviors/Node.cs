using System;

namespace SpiceEngine.Scripting.Behaviors
{
    public abstract class Node
    {
        public event EventHandler<BehaviorCompletedEventArgs> Completed;

        protected virtual void OnCompleted(BehaviorCompletedEventArgs e) => Completed?.Invoke(this, e);

        public abstract BehaviorStatus Tick(BehaviorContext context);

        public abstract void Reset();
    }
}
