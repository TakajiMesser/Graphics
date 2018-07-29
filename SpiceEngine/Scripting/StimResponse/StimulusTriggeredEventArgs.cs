using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceEngine.Scripting.StimResponse
{
    public class StimulusTriggeredEventArgs : EventArgs
    {
        public Stimulus Stimulus { get; private set; }

        public StimulusTriggeredEventArgs(Stimulus stimulus)
        {
            Stimulus = stimulus;
        }
    }
}
