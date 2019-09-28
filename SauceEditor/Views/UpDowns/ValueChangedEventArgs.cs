using System;

namespace SauceEditor.Views.UpDowns
{
    public class ValueChangedEventArgs : EventArgs
    {
        public float OldValue { get; private set; }
        public float NewValue { get; private set; }

        public ValueChangedEventArgs(float oldValue, float newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
