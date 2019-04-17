namespace SpiceEngine.Scripting.Meters
{
    public class Trigger
    {
        public enum TriggerTypes
        {
            FireOnce,
            FireMultiple
        }

        public string Name { get; private set; }
        public int Value { get; private set; }
        public bool IsTriggered { get; private set; }

        public TriggerTypes TriggerType { get; set; } = TriggerTypes.FireMultiple;
        public bool ResetOnTrigger { get; set; }

        public Trigger(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public bool Check(int value)
        {
            if (TriggerType == TriggerTypes.FireMultiple && value >= Value)
            {
                IsTriggered = true;
                return true;
            }
            else if (TriggerType == TriggerTypes.FireOnce && !IsTriggered && value >= Value)
            {
                IsTriggered = true;
                return true;
            }

            return false;
        }

        public void Reset()
        {
            IsTriggered = false;
        }
    }
}
