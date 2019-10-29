using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngineCore.Scripting.Meters
{
    public class Meter
    {
        private List<Trigger> _triggers = new List<Trigger>();

        public int Value { get; private set; }
        public int MinimumTriggerValue => _triggers.Min(t => t.Value);

        public event EventHandler<TriggeredEventArgs> Triggered;

        public void AddTrigger(Trigger trigger) => _triggers.Add(trigger);

        public void ClearTriggers() => _triggers.Clear();

        public bool Increment(int amount = 1)
        {
            Value += amount;

            bool isTriggered = false;
            bool shouldReset = false;

            foreach (var trigger in _triggers)
            {
                if (trigger.Check(Value))
                {
                    if (trigger.ResetOnTrigger)
                    {
                        shouldReset = true;
                    }

                    isTriggered = true;
                    Triggered?.Invoke(this, new TriggeredEventArgs(trigger));
                }
            }

            if (shouldReset)
            {
                Reset();
            }

            return isTriggered;
        }

        public void Decrement(int amount = 1) => Value -= amount;

        public void Reset()
        {
            Value = 0;

            foreach (var trigger in _triggers)
            {
                trigger.Reset();
            }
        }
    }
}
