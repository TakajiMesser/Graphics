using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceEditor.Controls.UpDowns
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
